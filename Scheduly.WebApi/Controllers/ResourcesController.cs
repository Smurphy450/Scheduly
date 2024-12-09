using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Resource;

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
            var category = await _context.Resources.FindAsync(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return await _context.Resources
                .Where(r => r.CategoryId == categoryId)
                .ToListAsync();
        }

        // POST: api/Resources/CreateResource
        [HttpPost("CreateResource")]
        public async Task<ActionResult<Resource>> CreateResource([FromBody] CreateResourceDTO resourceDTO)
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

                return CreatedAtAction("GetResource", new { id = resource.CategoryId }, resource);
            }
            else
            {
                return BadRequest("Name is required");
            }
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
