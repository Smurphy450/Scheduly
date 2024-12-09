namespace Scheduly.WebApi.Models.DTO.Absence
{
    public class ApproveAbsenceDTO
    {
        public int AbsenceId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string AbsenceTypeName { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }
    }
}