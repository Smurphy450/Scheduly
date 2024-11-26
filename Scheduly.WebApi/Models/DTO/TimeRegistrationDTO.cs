namespace Scheduly.WebApp.Models
{
    public class TimeRegistrationDTO
    {
        public int UserId { get; set; }
        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }
    }
}