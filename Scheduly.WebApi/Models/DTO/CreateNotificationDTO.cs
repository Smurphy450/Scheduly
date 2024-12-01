namespace Scheduly.WebApi.Models.DTO
{
    public class CreateNotificationDTO
    {
        public int UserId { get; set; }
        public bool? Sms { get; set; }
        public bool? Email { get; set; }
        public string? Message { get; set; }
    }
}
