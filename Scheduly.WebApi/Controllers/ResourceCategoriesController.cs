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
            var resourceCategory = await _context.ResourceCategories.FindAsync(id);

            if (resourceCategory == null)
            {
                return NotFound();
            }

            return resourceCategory;
        }

        // POST: api/ResourceCategories/CreateResourceCategory
        [HttpPost("CreateResourceCategory")]
        public async Task<ActionResult<ResourceCategory>> CreateResourceCategory([FromForm] CreateResourceCategoryDTO resourceCategoryDTO)
        {
            if (!string.IsNullOrEmpty(resourceCategoryDTO.Name))
            {
                var resourceCategory = new ResourceCategory
                {
                    Name = resourceCategoryDTO.Name
                };

                _context.ResourceCategories.Add(resourceCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetResourceCategory", new { id = resourceCategory.CategoryId }, resourceCategory);
            }
            else
            {
                return BadRequest("Name is required");
            }
        }

        // DELETE: api/ResourceCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResourceCategory(int id)
        {
            var resourceCategory = await _context.ResourceCategories.FindAsync(id);
            if (resourceCategory == null)
            {
                return NotFound();
            }

            _context.ResourceCategories.Remove(resourceCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
