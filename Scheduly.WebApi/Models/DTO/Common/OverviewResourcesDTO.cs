namespace Scheduly.WebApi.Models.DTO.Common
{
    public class OverviewResourcesDTO
    {
        public int BookingId { get; set; }
        public string CategoryName { get; set; }
        public string ResourceName { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public bool Approved { get; set; }
    }
}
