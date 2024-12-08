using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
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
        //[HttpPost("CreatePremiseCategory")]
        //public async Task<ActionResult<PremiseCategory>> CreatePremiseCategory([FromForm] CreatePremiseCategoryDTO premiseCategoryDTO)
        //{
        //    if (!string.IsNullOrEmpty(premiseCategoryDTO.Name))
        //    {
        //        var premiseCategory = new PremiseCategory
        //        {
        //            Name = premiseCategoryDTO.Name
        //        };

        //        _context.PremiseCategories.Add(premiseCategory);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetPremiseCategory", new { id = premiseCategory.PremiseCategoryId }, premiseCategory);
        //    }
        //    else
        //    {
        //        return BadRequest("Name is required");
        //    }
        //}
        [HttpPost("CreatePremiseCategory")]
        public async Task<ActionResult<PremiseCategory>> CreatePremiseCategory([FromForm] CreatePremiseCategoryDTO premiseCategoryDTO)
        {
            if (!string.IsNullOrEmpty(premiseCategoryDTO.Name))
            {
                var premiseCategory = new PremiseCategory
                {
                    Name = premiseCategoryDTO.Name
                };

                _context.PremiseCategories.Add(premiseCategory);
                try
                {
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = premiseCategoryDTO.UserId,
                        Action = "CreatePremiseCate", //change size of db col
                        AffectedData = $"Created premise category with name {premiseCategoryDTO.Name}"
                    });

                    return CreatedAtAction("GetPremiseCategory", new { id = premiseCategory.PremiseCategoryId }, premiseCategory);
                }
                catch (Exception ex)
                {
                    await ErrorLoggingHelper.LogErrorAsync(_context, premiseCategoryDTO.UserId, "CreatePremiseCategory", ex);
                    return StatusCode(500, "Internal server error");
                }
            }
            else
            {
                return BadRequest("Name is required");
            }
        }

        // DELETE: api/PremiseCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremiseCategory(int id)
        {
            var premiseCategory = await _context.PremiseCategories.FindAsync(id);
            if (premiseCategory == null)
            {
                return NotFound();
            }

            _context.PremiseCategories.Remove(premiseCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[HttpDelete("{id}")] //with logging
        //public async Task<IActionResult> DeletePremiseCategory(int id, [FromQuery] int userId)
        //{
        //    var premiseCategory = await _context.PremiseCategories.FindAsync(id);
        //    if (premiseCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.PremiseCategories.Remove(premiseCategory);
        //    try
        //    {
        //        await _context.SaveChangesAsync();

        //        await LoggingHelper.LogActionAsync(_context, new LoggingDTO
        //        {
        //            UserId = userId,
        //            Action = "DeletePremiseCategory",
        //            AffectedData = $"Deleted premise category with ID {id}"
        //        });

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeletePremiseCategory", ex);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}
