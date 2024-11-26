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
    public class PremisesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public PremisesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/Premises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Premise>>> GetPremises()
        {
            return await _context.Premises.ToListAsync();
        }

        // GET: api/Premises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Premise>> GetPremise(int id)
        {
            var premise = await _context.Premises.FindAsync(id);

            if (premise == null)
            {
                return NotFound();
            }

            return premise;
        }

        // PUT: api/Premises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPremise(int id, Premise premise)
        {
            if (id != premise.PremiseId)
            {
                return BadRequest();
            }

            _context.Entry(premise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PremiseExists(id))
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

        // POST: api/Premises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Premise>> PostPremise(Premise premise)
        {
            _context.Premises.Add(premise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPremise", new { id = premise.PremiseId }, premise);
        }

        // DELETE: api/Premises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremise(int id)
        {
            var premise = await _context.Premises.FindAsync(id);
            if (premise == null)
            {
                return NotFound();
            }

            _context.Premises.Remove(premise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PremiseExists(int id)
        {
            return _context.Premises.Any(e => e.PremiseId == id);
        }
    }
}
