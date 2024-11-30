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

        // GET: api/Absence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Absence>>> GetAbsences()
        {
            return await _context.Absences.ToListAsync();
        }

        // GET: api/Absence/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Absence>> GetAbsence(int id)
        {
            var absence = await _context.Absences.FindAsync(id);

            if (absence == null)
            {
                return NotFound();
            }

            return absence;
        }

        // GET: api/Absence/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Absence>>> GetAbsenceByUserId(int userId)
        {
            var absences = await _context.Absences
                .Where(a => a.UserId == userId)
                .ToListAsync();

            if (absences.Count == 0)
            {
                return NotFound();
            }

            return absences;
        }

        [HttpGet("ApproveAbsence/{id}")]
        public async Task<ActionResult<ApproveAbsenceDTO>> GetApproveAbsence(int id)
        {
            var absence = await _context.Absences
                .Include(a => a.User)
                .Include(a => a.AbsenceType)
                .FirstOrDefaultAsync(a => a.AbsenceId == id);

            if (absence == null)
            {
                return NotFound();
            }

            var approveAbsenceDTO = new ApproveAbsenceDTO
            {
                AbsenceId = absence.AbsenceId,
                UserId = absence.UserId,
                Username = absence.User.Username,
                AbsenceTypeName = absence.AbsenceType.Name,
                Start = absence.Start,
                End = absence.End,
                Description = absence.Description,
                Approved = absence.Approved ?? false
            };

            return Ok(approveAbsenceDTO);
        }

        [HttpGet("PendingApproval")]
        public async Task<ActionResult<IEnumerable<ApproveAbsenceDTO>>> GetPendingApprovalAbsences()
        {
            var pendingAbsences = await _context.Absences
                .Where(a => a.Approved == false)
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

        // PUT: api/Absence/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsence(int id, Absence absence)
        {
            if (id != absence.AbsenceId)
            {
                return BadRequest();
            }

            _context.Entry(absence).State = EntityState.Modified;

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

        // POST: api/Absence
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Absence>> PostAbsence(Absence absence)
        {
            _context.Absences.Add(absence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbsence", new { id = absence.AbsenceId }, absence);
        }

        // POST: api/Absence/Report
        //[HttpPost("Report")]
        //public async Task<ActionResult<Absence>> ReportAbsence(ReportAbsenceDTO reportAbsenceDTO)
        //{
        //    var absenceType = await _context.AbsenceTypes.FindAsync(reportAbsenceDTO.AbsenceTypeId);
        //    if (absenceType == null)
        //    {
        //        return BadRequest("Invalid AbsenceTypeId");
        //    }

        //    var absence = new Absence
        //    {
        //        AbsenceTypeId = reportAbsenceDTO.AbsenceTypeId,
        //        UserId = reportAbsenceDTO.UserId,
        //        Start = reportAbsenceDTO.DatetimeStart,
        //        End = reportAbsenceDTO.DatetimeEnd,
        //        Description = reportAbsenceDTO.Description,
        //        Approved = !(absenceType.MustBeApproved ?? false)
        //    };

        //    _context.Absences.Add(absence);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAbsence", new { id = absence.AbsenceId }, absence);
        //}

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
                Approved = !(absenceType.MustBeApproved ?? false)
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

        private bool AbsenceExists(int id)
        {
            return _context.Absences.Any(e => e.AbsenceId == id);
        }
    }
}
