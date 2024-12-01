using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;

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

        // GET: api/AbsenceTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbsenceType>> GetAbsenceType(int id)
        {
            var absenceType = await _context.AbsenceTypes.FindAsync(id);

            if (absenceType == null)
            {
                return NotFound();
            }

            return absenceType;
        }

        [HttpGet("Names")]
        public async Task<ActionResult<IEnumerable<string>>> GetAbsenceTypeNames()
        {
            var absenceTypeNames = await _context.AbsenceTypes
                .Select(at => at.Name)
                .ToListAsync();

            return Ok(absenceTypeNames);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<AbsenceType>> PostAbsenceType(int id, AbsenceTypeDTO absenceTypeDTO)
        {
            if (id == 0)
            {
                return Ok(new AbsenceTypeDTO());
            }

            var absenceType = await _context.AbsenceTypes.FindAsync(id);
            if (absenceType == null)
            {
                return NotFound();
            }

            return Ok(absenceType);
        }

        // POST: api/AbsenceTypes/Upsert
        [HttpPost("Upsert")]
        public async Task<IActionResult> UpsertAbsenceType(AbsenceTypeDTO absenceTypeDTO)
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

                return CreatedAtAction("GetAbsenceType", new { id = absenceType.AbsenceTypeId }, absenceType);
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

        //[HttpGet("UserAbsences/{userId}")]
        //public async Task<ActionResult<IEnumerable<object>>> GetUserAbsenceTypes(int userId)
        //{
        //    var userAbsenceTypes = await _context.AbsenceTypes
        //        .Select(at => new
        //        {
        //            AbsenceType = at,
        //            AbsenceCount = at.Absences.Count(a => a.UserId == userId)
        //        })
        //        .ToListAsync();

        //    return Ok(userAbsenceTypes);
        //}

        //[HttpGet("UserAbsences/{userId}")]
        //public async Task<ActionResult<IEnumerable<object>>> GetUserAbsenceTypes(int userId)
        //{
        //    var userAbsenceTypes = await _context.AbsenceTypes
        //        .Select(at => new
        //        {
        //            AbsenceType = at,
        //            AbsenceCount = at.Absences.Count(a => a.UserId == userId),
        //            TotalMinutes = at.Absences
        //                .Where(a => a.UserId == userId && a.End.HasValue)
        //                .Sum(a => EF.Functions.DateDiffMinute(a.Start, a.End.Value))
        //        })
        //        .ToListAsync();

        //    return Ok(userAbsenceTypes);
        //}

        [HttpGet("UserAbsences/{userId}")]
        public async Task<ActionResult<IEnumerable<UserAbsenceTypeDto>>> GetUserAbsenceTypes(int userId)
        {
            var userAbsenceTypes = await _context.AbsenceTypes
                .Select(at => new UserAbsenceTypeDto
                {
                    AbsenceTypeId = at.AbsenceTypeId,
                    AbsenceTypeName = at.Name,
                    AbsenceCount = at.Absences.Count(a => a.UserId == userId),
                    TotalMinutes = at.Absences
                        .Where(a => a.UserId == userId && a.End.HasValue)
                        .Sum(a => EF.Functions.DateDiffMinute(a.Start, a.End.Value))
                })
                .ToListAsync();

            return Ok(userAbsenceTypes);
        }


        // PUT: api/AbsenceTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsenceType(int id, AbsenceType absenceType)
        {
            if (id != absenceType.AbsenceTypeId)
            {
                return BadRequest();
            }

            _context.Entry(absenceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbsenceTypeExists(id))
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

        // POST: api/AbsenceTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AbsenceType>> PostAbsenceType(AbsenceType absenceType)
        {
            _context.AbsenceTypes.Add(absenceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbsenceType", new { id = absenceType.AbsenceTypeId }, absenceType);
        }

        // DELETE: api/AbsenceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsenceType(int id)
        {
            var absenceType = await _context.AbsenceTypes.FindAsync(id);
            if (absenceType == null)
            {
                return NotFound();
            }

            _context.AbsenceTypes.Remove(absenceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AbsenceTypeExists(int id)
        {
            return _context.AbsenceTypes.Any(e => e.AbsenceTypeId == id);
        }
    }
}
