namespace Scheduly.WebApi.Models.DTO
{
    public class AbsenceQueryDTO
    {
        public int UserId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
