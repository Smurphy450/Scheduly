namespace Scheduly.WebApi.Models.DTO
{
    public class LoggingDTO
    {
        public int UserId { get; set; }
        public string Action { get; set; } = null!;
        public string AffectedData { get; set; } = null!;
    }
}