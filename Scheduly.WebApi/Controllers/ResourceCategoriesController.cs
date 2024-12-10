using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models.DTO.Resource;
using Scheduly.WebApi.Utilities;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceCategoriesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public ResourceCategoriesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/ResourceCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceCategory>>> GetResourceCategories()
        {
            return await _context.ResourceCategories.ToListAsync();
        }

        // GET: api/ResourceCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceCategory>> GetResourceCategory(int id)
        {
            try
            {
                var resourceCategory = await _context.ResourceCategories.FindAsync(id);

                if (resourceCategory == null)
                {
                    return NotFound();
                }

                return resourceCategory;
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 1, "GetResourceCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/ResourceCategories/CreateResourceCategory
        [HttpPost("CreateResourceCategory")]
        public async Task<ActionResult<ResourceCategory>> CreateResourceCategory([FromBody] CreateResourceCategoryDTO resourceCategoryDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(resourceCategoryDTO.Name))
                {
                    var resourceCategory = new ResourceCategory
                    {
                        Name = resourceCategoryDTO.Name
                    };

                    _context.ResourceCategories.Add(resourceCategory);
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = resourceCategoryDTO.UserId,
                        Action = "CreateResourceCategory",
                        AffectedData = $"Created resource category with ID: {resourceCategory.CategoryId}"
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
                await ErrorLoggingHelper.LogErrorAsync(_context, resourceCategoryDTO.UserId, "CreateResourceCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/ResourceCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResourceCategory(int id, [FromQuery] int userId)
        {
            try
            {
                var resourceCategory = await _context.ResourceCategories.FindAsync(id);
                if (resourceCategory == null)
                {
                    return NotFound();
                }

                _context.ResourceCategories.Remove(resourceCategory);
                await _context.SaveChangesAsync();

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "DeleteResourceCategory",
                    AffectedData = $"Deleted resource category with ID: {id}"
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeleteResourceCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
