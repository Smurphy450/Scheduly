using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models.DTO.Absence;
using Scheduly.WebApi.Utilities;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceTypesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public AbsenceTypesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/AbsenceTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbsenceType>>> GetAbsenceTypes()
        {
            return await _context.AbsenceTypes.ToListAsync();
        }

        // POST: api/AbsenceTypes/Upsert
        [HttpPost("Upsert")]
        public async Task<IActionResult> UpsertAbsenceType(AbsenceTypeDTO absenceTypeDTO)
        {
            try
            {
                var absenceType = await _context.AbsenceTypes.FindAsync(absenceTypeDTO.AbsenceTypeId);

                if (absenceType == null)
                {
                    // Create new AbsenceType
                    absenceType = new AbsenceType
                    {
                        Name = absenceTypeDTO.Name,
                        WageFactor = absenceTypeDTO.WageFactor,
                        MustBeApproved = absenceTypeDTO.MustBeApproved
                    };

                    _context.AbsenceTypes.Add(absenceType);
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = absenceTypeDTO.UserId,
                        Action = "CreateAbsenceType",
                        AffectedData = $"Created new absence type with name {absenceTypeDTO.Name}"
                    });

                    //return succes

                    return Ok(absenceType);
                }
                else
                {
                    // Update existing AbsenceType
                    absenceType.Name = absenceTypeDTO.Name;
                    absenceType.WageFactor = absenceTypeDTO.WageFactor;
                    absenceType.MustBeApproved = absenceTypeDTO.MustBeApproved;

                    _context.Entry(absenceType).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();

                        await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                        {
                            UserId = absenceTypeDTO.UserId,
                            Action = "UpdateAbsenceType",
                            AffectedData = $"Updated absence type with ID {absenceTypeDTO.AbsenceTypeId}"
                        });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AbsenceTypeExists(absenceTypeDTO.AbsenceTypeId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, absenceTypeDTO.UserId, "UpsertAbsenceType", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/AbsenceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsenceType(int id)
        {
            var userId = GetUserId();
            var absenceType = await _context.AbsenceTypes.FindAsync(id);
            if (absenceType == null)
            {
                return NotFound();
            }

            _context.AbsenceTypes.Remove(absenceType);
            await _context.SaveChangesAsync();
               
            return NoContent();   
        }

        //[HttpDelete("{id}")] //version with logging
        //public async Task<IActionResult> DeleteAbsenceType(int id, [FromQuery] int userId)
        //{
        //    try
        //    {
        //        var absenceType = await _context.AbsenceTypes.FindAsync(id);
        //        if (absenceType == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.AbsenceTypes.Remove(absenceType);
        //        await _context.SaveChangesAsync();

        //        await LoggingHelper.LogActionAsync(_context, new LoggingDTO
        //        {
        //            UserId = userId,
        //            Action = "DeleteAbsenceType",
        //            AffectedData = $"Deleted absence type with ID {id}"
        //        });

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeleteAbsenceType", ex);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        private bool AbsenceTypeExists(int id)
        {
            return _context.AbsenceTypes.Any(e => e.AbsenceTypeId == id);
        }
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : 0;
        }
    }
}
