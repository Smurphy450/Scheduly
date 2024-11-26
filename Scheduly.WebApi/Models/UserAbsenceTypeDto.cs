namespace Scheduly.WebApi.Models
{
    public class UserAbsenceTypeDto
    {
        public int AbsenceTypeId { get; set; }
        public string AbsenceTypeName { get; set; } = null!;
        public int AbsenceCount { get; set; }
        public int TotalMinutes { get; set; }
    }
}