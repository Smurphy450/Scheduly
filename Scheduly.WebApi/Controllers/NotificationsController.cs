using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;
using Scheduly.WebApi.Models.DTO;

namespace Scheduly.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly SchedulyContext _context;

        public NotificationsController(SchedulyContext context)
        {
            _context = context;
        }
       
        // POST: api/Notifications
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(CreateNotificationDTO createNotificationDTO)
        {
            var notification = new Notification
            {
                UserId = createNotificationDTO.UserId,
                Sms = createNotificationDTO.Sms,
                Email = createNotificationDTO.Email,
                Message = createNotificationDTO.Message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return Ok(new { Success = true });
        }
    }
}
