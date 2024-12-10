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
    public class ResourcesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public ResourcesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/Resources/Category/{categoryId}
        [HttpGet("Category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResourcesByCategory(int categoryId)
        {
            try
            {
                var category = await _context.Resources.FindAsync(categoryId);

                if (category == null)
                {
                    return NotFound();
                }

                return await _context.Resources
                    .Where(r => r.CategoryId == categoryId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 1, "GetResourcesByCategory", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Resources/CreateResource
        [HttpPost("CreateResource")]
        public async Task<ActionResult<Resource>> CreateResource([FromBody] CreateResourceDTO resourceDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(resourceDTO.Name))
                {
                    var resource = new Resource
                    {
                        CategoryId = resourceDTO.CategoryId,
                        Name = resourceDTO.Name,
                        Description = resourceDTO.Description,
                        Amount = resourceDTO.Amount,
                        MustBeApproved = resourceDTO.MustBeApproved,
                    };

                    _context.Resources.Add(resource);
                    await _context.SaveChangesAsync();

                    await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                    {
                        UserId = resourceDTO.UserId,
                        Action = "CreateResource",
                        AffectedData = $"Created resource with ID: {resource.ResourceId}"
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
                await ErrorLoggingHelper.LogErrorAsync(_context, resourceDTO.UserId, "CreateResource", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id, [FromQuery] int userId)
        {
            try
            {
                var resource = await _context.Resources.FindAsync(id);
                if (resource == null)
                {
                    return NotFound();
                }

                _context.Resources.Remove(resource);

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = userId,
                    Action = "DeleteResource",
                    AffectedData = $"Deleted resource with ID: {id}"
                });

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, userId, "DeleteResource", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
