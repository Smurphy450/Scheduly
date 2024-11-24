using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;

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

        // PUT: api/ResourceCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResourceCategory(int id, ResourceCategory resourceCategory)
        {
            if (id != resourceCategory.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(resourceCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceCategoryExists(id))
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

        // POST: api/ResourceCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResourceCategory>> PostResourceCategory(ResourceCategory resourceCategory)
        {
            _context.ResourceCategories.Add(resourceCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResourceCategory), new { name = resourceCategory.Name }, resourceCategory);
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

        private bool ResourceCategoryExists(int id)
        {
            return _context.ResourceCategories.Any(e => e.CategoryId == id);
        }
    }
}
