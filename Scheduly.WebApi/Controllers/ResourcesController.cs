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
    public class ResourcesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public ResourcesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
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

        // PUT: api/Resources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource(int id, Resource resource)
        {
            if (id != resource.ResourceId)
            {
                return BadRequest();
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id))
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

        // POST: api/Resources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Resource>> PostResource(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.ResourceId }, resource);
        }

        [HttpPost("CreateResource")]
        public async Task<ActionResult<Resource>> CreateResource([FromForm] CreateResourceDTO resourceDTO)
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

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.ResourceId == id);
        }
    }
}
