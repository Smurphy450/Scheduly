using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;
using Scheduly.WebApp.Models;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public AbsenceController(SchedulyContext context)
        {
            _context = context;
        }        

        [HttpGet("PendingApproval")]
        public async Task<ActionResult<IEnumerable<ApproveAbsenceDTO>>> GetPendingApprovalAbsences()
        {
            var pendingAbsences = await _context.Absences
                .Where(a => a.Approved == null)
                .Include(a => a.User)
                .Include(a => a.AbsenceType)
                .Select(a => new ApproveAbsenceDTO
                {
                    AbsenceId = a.AbsenceId,
                    UserId = a.UserId,
                    Username = a.User.Username,
                    AbsenceTypeName = a.AbsenceType.Name,
                    Start = a.Start,
                    End = a.End,
                    Description = a.Description,
                    Approved = a.Approved ?? false
                })
                .ToListAsync();

            return Ok(pendingAbsences);
        }

        // POST: api/Absence/AbsenceDTOs
        [HttpPost("AbsenceDTOs")]
        public async Task<ActionResult<IEnumerable<AbsenceDTO>>> GetAbsenceDTOs([FromBody] AbsenceQueryDTO query)
        {
            var absences = await _context.Absences
                .Where(a => a.UserId == query.UserId && a.Start >= query.StartDate && a.Start <= query.EndDate)
                .Include(a => a.User)
                .Include(a => a.AbsenceType)
                .Select(a => new AbsenceDTO
                {
                    AbsenceId = a.AbsenceId,
                    UserId = a.UserId,
                    Username = a.User.Username,
                    AbsenceTypeName = a.AbsenceType.Name,
                    Start = a.Start,
                    End = a.End,
                    Description = a.Description,
                    Approved = a.Approved ?? false
                })
                .ToListAsync();

            return Ok(absences);
        }

        [HttpPost("Report")]
        public async Task<IActionResult> ReportAbsence(ReportAbsenceDTO reportAbsenceDTO)
        {
            var absenceType = await _context.AbsenceTypes.FindAsync(reportAbsenceDTO.AbsenceTypeId);
            if (absenceType == null)
            {
                return BadRequest("Invalid AbsenceTypeId");
            }

            var absence = new Absence
            {
                AbsenceTypeId = reportAbsenceDTO.AbsenceTypeId,
                UserId = reportAbsenceDTO.UserId,
                Start = reportAbsenceDTO.DatetimeStart,
                End = reportAbsenceDTO.DatetimeEnd,
                Description = reportAbsenceDTO.Description,
                Approved = absenceType.MustBeApproved == true ? (bool?)null : true
            };

            _context.Absences.Add(absence);
            await _context.SaveChangesAsync();

            return Ok(new { Success = true });
        }

        // DELETE: api/Absence/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            var absence = await _context.Absences.FindAsync(id);
            if (absence == null)
            {
                return NotFound();
            }

            _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Absence/Approve/{id}
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> ApproveAbsence(int id, [FromBody] bool isApproved)
        {
            var absence = await _context.Absences.FindAsync(id);
            if (absence == null)
            {
                return NotFound();
            }

            absence.Approved = isApproved;
            _context.Entry(absence).State = EntityState.Modified;

            var notification = new Notification
            {
                UserId = absence.UserId,
                Sms = true,
                Email = true,
                Message = $"Your absence request has been {(absence.Approved == true ? "approved" : "denied")}."
            };

            _context.Notifications.Add(notification);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbsenceExists(id))
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

        private bool AbsenceExists(int id)
        {
            return _context.Absences.Any(e => e.AbsenceId == id);
        }
    }
}
