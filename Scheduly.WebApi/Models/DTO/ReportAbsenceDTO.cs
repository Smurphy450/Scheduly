namespace Scheduly.WebApp.Models
{
    public class ReportAbsenceDTO
    {
        public DateTimeOffset DatetimeStart { get; set; }
        public DateTimeOffset DatetimeEnd { get; set; }
        public string Description { get; set; }
        public int AbsenceTypeId { get; set; }
        public int UserId { get; set; }
    }
}