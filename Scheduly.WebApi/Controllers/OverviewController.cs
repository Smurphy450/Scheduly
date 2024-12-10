using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO.Common;
using Scheduly.WebApi.Models.DTO.TimeRegistration;
using Scheduly.WebApi.Models.DTO.User;
using System.Threading.Tasks;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OverviewController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public OverviewController(SchedulyContext context)
        {
            _context = context;
        }

        [HttpGet("GetOverviewData/{userId}")]
        public async Task<ActionResult<OverviewDataDTO>> GetOverviewData(int userId)
        {
            var today = DateTimeOffset.UtcNow.Date;
            var threeMonthsAgo = DateTimeOffset.UtcNow.AddMonths(-3);

            // Check if the day has started
            var dayStarted = await _context.TimeRegistrations
                .AnyAsync(tr => tr.UserId == userId && tr.Start.Date == today &&
                                (tr.End == null || tr.End == DateTimeOffset.MinValue || tr.Start == tr.End));

            // Calculate average weekly work time
            var timeRegistrations = await _context.TimeRegistrations
                .Where(tr => tr.UserId == userId && tr.Start >= threeMonthsAgo)
                .ToListAsync();

            var totalHours = timeRegistrations
                .Where(tr => tr.End.HasValue)
                .Sum(tr => (tr.End.Value - tr.Start).TotalHours);

            var totalWeeks = (DateTimeOffset.UtcNow - threeMonthsAgo).TotalDays / 7;
            var averageWeeklyWorkTime = totalHours / totalWeeks;

            // Get user overview
            var user = await _context.Users
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Resource)
                .ThenInclude(r => r.Category)
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Premise)
                .ThenInclude(p => p.PremiseCategory)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            var overviewResources = user.Bookings
                .Where(b => b.ResourceId != null && (b.End == null || b.End > DateTimeOffset.Now))
                .Select(b => new OverviewResourcesDTO
                {
                    BookingId = b.BookingsId,
                    CategoryName = b.Resource.Category.Name,
                    ResourceName = b.Resource.Name,
                    Description = b.Resource.Description,
                    Start = b.Start,
                    End = b.End ?? DateTimeOffset.MinValue,
                    Approved = b.Approved ?? false
                }).ToList();

            var overviewPremises = user.Bookings
                .Where(b => b.PremiseId != null && (b.End == null || b.End > DateTimeOffset.Now))
                .Select(b => new OverviewPremisesDTO
                {
                    BookingId = b.BookingsId,
                    Name = b.Premise.Name,
                    CategoryName = b.Premise.PremiseCategory.Name,
                    Size = b.Premise.Size,
                    Start = b.Start,
                    End = b.End,
                    Approved = b.Approved ?? false
                }).ToList();

            var userOverview = new UserOverviewDTO
            {
                OverviewResources = overviewResources,
                OverviewPremises = overviewPremises
            };

            var overviewData = new OverviewDataDTO
            {
                DayStarted = dayStarted,
                AverageWeeklyWorkTime = averageWeeklyWorkTime,
                UserOverview = userOverview
            };

            return Ok(overviewData);
        }
    }
}
