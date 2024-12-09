using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.TimeRegistration;

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

        // GET: api/TimeRegistrations/Exists/{userId}
        [HttpGet("Exists/{userId}")]
        public async Task<ActionResult<bool>> TimeRegistrationExistsForToday(int userId)
        {
            var today = DateTimeOffset.UtcNow.Date;
            var exists = await _context.TimeRegistrations
                .AnyAsync(tr => tr.UserId == userId && tr.Start.Date == today &&
                                (tr.End == null || tr.End == DateTimeOffset.MinValue || tr.Start == tr.End));

            return exists;
        }

        // POST: api/TimeRegistrations/TimeRegistrationDTOs
        [HttpPost("TimeRegistrationDTOs")]
        public async Task<ActionResult<IEnumerable<TimeRegistrationDTO>>> GetTimeRegistrationsByDateRange([FromBody] TimeRegistrationDTO query)
        {
            var timeRegistrations = await _context.TimeRegistrations
                .Where(tr => tr.UserId == query.UserId && tr.Start >= query.Start && tr.Start <= query.End)
                .Select(tr => new TimeRegistrationDTO
                {
                    TimeId = tr.TimeId,
                    UserId = tr.UserId,
                    Start = tr.Start,
                    End = tr.End
                })
                .ToListAsync();

            return Ok(timeRegistrations);
        }

        [HttpPost]
        public async Task<ActionResult<TimeRegistration>> PostTimeRegistration(TimeRegistrationDTO timeRegistrationDto)
        {
            var timeRegistration = new TimeRegistration
            {
                UserId = timeRegistrationDto.UserId,
                Start = timeRegistrationDto.Start.Value,
                End = timeRegistrationDto.End
            };

            _context.TimeRegistrations.Add(timeRegistration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeRegistration", new { id = timeRegistration.TimeId }, timeRegistration);
        }

        [HttpPost("RegisterTime")]
        public async Task<IActionResult> RegisterTime([FromBody] TimeRegistrationDTO timeRegistrationDto)
        {
            var userId = timeRegistrationDto.UserId;
            var today = timeRegistrationDto.Start.Value.Date;

            var existingTimeRegistrations = await _context.TimeRegistrations
                .Where(tr => tr.UserId == userId && tr.Start.Date == today && (tr.End == null || tr.Start == tr.End))
                .OrderByDescending(tr => tr.Start)
                .ToListAsync();

            var existingTimeRegistration = existingTimeRegistrations.FirstOrDefault();

            if (existingTimeRegistration != null)
            {
                existingTimeRegistration.End = timeRegistrationDto.End;
                _context.Entry(existingTimeRegistration).State = EntityState.Modified;
            }
            else
            {
                var newTimeRegistration = new TimeRegistration
                {
                    UserId = userId,
                    Start = timeRegistrationDto.Start.Value,
                    End = timeRegistrationDto.Start.Value //vil gerne indsætte null, men det kan jeg ikke.
                };
                _context.TimeRegistrations.Add(newTimeRegistration);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("AverageWeeklyWorkTime/{userId}")]
        public async Task<ActionResult<double>> GetAverageWeeklyWorkTime(int userId)
        {
            var threeMonthsAgo = DateTimeOffset.UtcNow.AddMonths(-3);
            var timeRegistrations = await _context.TimeRegistrations
                .Where(tr => tr.UserId == userId && tr.Start >= threeMonthsAgo)
                .ToListAsync();

            if (!timeRegistrations.Any())
            {
                return NotFound("No time registrations found for the user in the last 3 months.");
            }

            var totalHours = timeRegistrations
                .Where(tr => tr.End.HasValue)
                .Sum(tr => (tr.End.Value - tr.Start).TotalHours);

            var totalWeeks = (DateTimeOffset.UtcNow - threeMonthsAgo).TotalDays / 7;
            var averageWeeklyWorkTime = totalHours / totalWeeks;

            return Ok(averageWeeklyWorkTime);
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
    }
}
