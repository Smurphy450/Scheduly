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

        // PUT: api/PremiseCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPremiseCategory(int id, PremiseCategory premiseCategory)
        {
            if (id != premiseCategory.PremiseCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(premiseCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PremiseCategoryExists(id))
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

        // POST: api/PremiseCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PremiseCategory>> PostPremiseCategory(PremiseCategory premiseCategory)
        {
            _context.PremiseCategories.Add(premiseCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPremiseCategory", new { id = premiseCategory.PremiseCategoryId }, premiseCategory);
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

        private bool PremiseCategoryExists(int id)
        {
            return _context.PremiseCategories.Any(e => e.PremiseCategoryId == id);
        }
    }
}
