namespace Scheduly.WebApi.Models.DTO
{
    public class AbsenceTypeDTO
    {
        public int AbsenceTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? WageFactor { get; set; }
        public bool? MustBeApproved { get; set; }
        public int UserId { get; set; }
    }
}
