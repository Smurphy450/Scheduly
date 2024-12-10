using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models.DTO.Premise;
using Scheduly.WebApi.Utilities;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremisesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public PremisesController(SchedulyContext context)
        {
            _context = context;
        }      

        // GET: api/Premises/Category/{premisecategoryId}
        [HttpGet("Category/{premisecategoryId}")]
        public async Task<ActionResult<IEnumerable<PremiseDTO>>> GetPremisesByCategory(int premisecategoryId)
        {
            try
            {
                var category = await _context.PremiseCategories.FindAsync(premisecategoryId);

                if (category == null)
                {
                    return NotFound();
                }

                var premises = await _context.Premises
                    .Where(r => r.PremiseCategoryId == premisecategoryId)
                    .Select(p => new PremiseDTO
                    {
                        PremiseId = p.PremiseId,
                        PremiseCategoryId = p.PremiseCategoryId,
                        Name = p.Name,
                        Size = p.Size,
                        Description = p.Description,
                        MustBeApproved = p.MustBeApproved
                    })
                    .ToListAsync();

                return Ok(premises);
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 1, "GetPremisesByCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreatePremise")]
        public async Task<ActionResult<Premise>> CreatePremise([FromBody] CreatePremiseDTO premiseDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(premiseDTO.Name))
                {
                    var premise = new Premise
                    {
                        PremiseCategoryId = premiseDTO.PremiseCategoryId,
                        Name = premiseDTO.Name,
                        Size = premiseDTO.Size,
                        Description = premiseDTO.Description,
                        MustBeApproved = premiseDTO.MustBeApproved,
                    };

                    _context.Premises.Add(premise);
                
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = premiseDTO.UserId,
                        Action = "CreatePremise",
                        AffectedData = $"Created premise with name {premiseDTO.Name}"
                    });

                    return Ok(new { Success = true });
                }
                else
                {
                    return BadRequest("Name is required");
                }
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, premiseDTO.UserId, "CreatePremise", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremise(int id, [FromQuery] int userId)
        {
            try
            {
                var premise = await _context.Premises.FindAsync(id);
                if (premise == null)
                {
                    return NotFound();
                }

                var premiseName = premise.Name;

                _context.Premises.Remove(premise);
            
                await _context.SaveChangesAsync();

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "DeletePremise",
                    AffectedData = $"Deleted premise with ID {id} and Name {premiseName}"
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeletePremise", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}