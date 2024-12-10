using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulyLoggingsController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public SchedulyLoggingsController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/SchedulyLoggings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchedulyLogging>>> GetSchedulyLoggings()
        {
            return await _context.SchedulyLoggings.ToListAsync();
        }

        // GET: api/SchedulyLoggings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchedulyLogging>> GetSchedulyLogging(int id)
        {
            var schedulyLogging = await _context.SchedulyLoggings.FindAsync(id);

            if (schedulyLogging == null)
            {
                return NotFound();
            }

            return schedulyLogging;
        }

        // PUT: api/SchedulyLoggings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedulyLogging(int id, SchedulyLogging schedulyLogging)
        {
            if (id != schedulyLogging.LogId)
            {
                return BadRequest();
            }

            _context.Entry(schedulyLogging).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchedulyLoggingExists(id))
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

        // POST: api/SchedulyLoggings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SchedulyLogging>> PostSchedulyLogging(SchedulyLogging schedulyLogging)
        {
            _context.SchedulyLoggings.Add(schedulyLogging);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSchedulyLogging", new { id = schedulyLogging.LogId }, schedulyLogging);
        }

        // DELETE: api/SchedulyLoggings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedulyLogging(int id)
        {
            var schedulyLogging = await _context.SchedulyLoggings.FindAsync(id);
            if (schedulyLogging == null)
            {
                return NotFound();
            }

            _context.SchedulyLoggings.Remove(schedulyLogging);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SchedulyLoggingExists(int id)
        {
            return _context.SchedulyLoggings.Any(e => e.LogId == id);
        }
    }
}
