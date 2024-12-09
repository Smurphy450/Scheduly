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
    public class PremiseCategoriesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public PremiseCategoriesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/PremiseCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PremiseCategory>>> GetPremiseCategories()
        {
            return await _context.PremiseCategories.ToListAsync();
        }

        // GET: api/PremiseCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PremiseCategory>> GetPremiseCategory(int id)
        {
            var premiseCategory = await _context.PremiseCategories.FindAsync(id);

            if (premiseCategory == null)
            {
                return NotFound();
            }

            return premiseCategory;
        }

        // POST: api/PremiseCategories/CreatePremiseCategory
        [HttpPost("CreatePremiseCategory")]
        public async Task<ActionResult<PremiseCategory>> CreatePremiseCategory([FromBody] CreatePremiseCategoryDTO premiseCategoryDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(premiseCategoryDTO.Name))
                {
                    var premiseCategory = new PremiseCategory
                    {
                        Name = premiseCategoryDTO.Name
                    };

                    _context.PremiseCategories.Add(premiseCategory);
                
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = premiseCategoryDTO.UserId,
                        Action = "CreatePremiseCategory", //change size of db col
                        AffectedData = $"Created premise category with name {premiseCategoryDTO.Name}"
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
                await ErrorLoggingHelper.LogErrorAsync(_context, premiseCategoryDTO.UserId, "CreatePremiseCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/PremiseCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremiseCategory(int id, [FromQuery] int userId)
        {
            try
            {
                var premiseCategory = await _context.PremiseCategories.FindAsync(id);
                if (premiseCategory == null)
                {
                    return NotFound();
                }

                var premiseCategoryName = premiseCategory.Name;

                _context.PremiseCategories.Remove(premiseCategory);
            
                await _context.SaveChangesAsync();

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "DeletePremiseCategory",
                    AffectedData = $"Deleted premise category with ID {id} and Name {premiseCategoryName}"
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeletePremiseCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
