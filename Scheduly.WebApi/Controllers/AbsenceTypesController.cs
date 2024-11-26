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
    public class AbsenceTypesController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public AbsenceTypesController(SchedulyContext context)
        {
            _context = context;
        }

        // GET: api/AbsenceTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbsenceType>>> GetAbsenceTypes()
        {
            return await _context.AbsenceTypes.ToListAsync();
        }

        // GET: api/AbsenceTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbsenceType>> GetAbsenceType(int id)
        {
            var absenceType = await _context.AbsenceTypes.FindAsync(id);

            if (absenceType == null)
            {
                return NotFound();
            }

            return absenceType;
        }

        [HttpGet("UserAbsences/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserAbsenceTypes(int userId)
        {
            var userAbsenceTypes = await _context.AbsenceTypes
                .Select(at => new
                {
                    AbsenceType = at,
                    AbsenceCount = at.Absences.Count(a => a.UserId == userId)
                })
                .ToListAsync();

            return Ok(userAbsenceTypes);
        }


        // PUT: api/AbsenceTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsenceType(int id, AbsenceType absenceType)
        {
            if (id != absenceType.AbsenceTypeId)
            {
                return BadRequest();
            }

            _context.Entry(absenceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbsenceTypeExists(id))
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

        // POST: api/AbsenceTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AbsenceType>> PostAbsenceType(AbsenceType absenceType)
        {
            _context.AbsenceTypes.Add(absenceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbsenceType", new { id = absenceType.AbsenceTypeId }, absenceType);
        }

        // DELETE: api/AbsenceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsenceType(int id)
        {
            var absenceType = await _context.AbsenceTypes.FindAsync(id);
            if (absenceType == null)
            {
                return NotFound();
            }

            _context.AbsenceTypes.Remove(absenceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AbsenceTypeExists(int id)
        {
            return _context.AbsenceTypes.Any(e => e.AbsenceTypeId == id);
        }
    }
}
