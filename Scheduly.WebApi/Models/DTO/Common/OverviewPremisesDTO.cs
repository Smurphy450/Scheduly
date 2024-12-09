namespace Scheduly.WebApi.Models.DTO.Common
{
    public class OverviewPremisesDTO
    {
        public int BookingId { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Size { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public bool? Approved { get; set; }
    }
}
