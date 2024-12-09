using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Utilities;
using Scheduly.WebApi.Models.DTO.Absence;

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
            try
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
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 0, "GetPendingApprovalAbsences", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AbsenceDTOs")]
        public async Task<ActionResult<IEnumerable<AbsenceDTO>>> GetAbsenceDTOs([FromBody] AbsenceQueryDTO query)
        {
            try
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

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = query.UserId,
                    Action = "GetAbsenceDTOs",
                    AffectedData = $"Retrieved absences for user {query.UserId} from {query.StartDate} to {query.EndDate}"
                });

                return Ok(absences);
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, query.UserId, "GetAbsenceDTOs", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Report")]
        public async Task<IActionResult> ReportAbsence(ReportAbsenceDTO reportAbsenceDTO)
        {
            try
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

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = reportAbsenceDTO.UserId,
                    Action = "ReportAbsence",
                    AffectedData = $"Reported absence for user {reportAbsenceDTO.UserId}"
                });

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, reportAbsenceDTO.UserId, "ReportAbsence", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            try
            {
                var absence = await _context.Absences.FindAsync(id);
                if (absence == null)
                {
                    return NotFound();
                }

                _context.Absences.Remove(absence);
                await _context.SaveChangesAsync();

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = absence.UserId,
                    Action = "DeleteAbsence",
                    AffectedData = $"Deleted absence with ID {id}"
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 0, "DeleteAbsence", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> ApproveAbsence(int id, [FromBody] bool isApproved)
        {
            try
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

                await _context.SaveChangesAsync();

                await LoggingHelper.LogActionAsync(_context, new LoggingDTO
                {
                    UserId = absence.UserId,
                    Action = "ApproveAbsence",
                    AffectedData = $"Approved absence with ID {id} for user {absence.UserId}"
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLoggingHelper.LogErrorAsync(_context, 0, "ApproveAbsence", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool AbsenceExists(int id)
        {
            return _context.Absences.Any(e => e.AbsenceId == id);
        }
    }
}
