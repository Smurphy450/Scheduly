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
    public class TimeRegistrationsController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public TimeRegistrationsController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/TimeRegistrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeRegistration>>> GetTimeRegistrations()
        {
            return await _context.TimeRegistrations.ToListAsync();
        }

        // GET: api/TimeRegistrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeRegistration>> GetTimeRegistration(int id)
        {
            var timeRegistration = await _context.TimeRegistrations.FindAsync(id);

            if (timeRegistration == null)
            {
                return NotFound();
            }

            return timeRegistration;
        }

        // PUT: api/TimeRegistrations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeRegistration(int id, TimeRegistration timeRegistration)
        {
            if (id != timeRegistration.TimeId)
            {
                return BadRequest();
            }

            _context.Entry(timeRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeRegistrationExists(id))
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

        // POST: api/TimeRegistrations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TimeRegistration>> PostTimeRegistration(TimeRegistration timeRegistration)
        {
            _context.TimeRegistrations.Add(timeRegistration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeRegistration", new { id = timeRegistration.TimeId }, timeRegistration);
        }

        // DELETE: api/TimeRegistrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeRegistration(int id)
        {
            var timeRegistration = await _context.TimeRegistrations.FindAsync(id);
            if (timeRegistration == null)
            {
                return NotFound();
            }

            _context.TimeRegistrations.Remove(timeRegistration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimeRegistrationExists(int id)
        {
            return _context.TimeRegistrations.Any(e => e.TimeId == id);
        }
    }
}
