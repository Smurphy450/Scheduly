namespace Scheduly.WebApi.Models.DTO.TimeRegistration
{
    public class TimeRegistrationDTO
    {
        public int TimeId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }
    }
}