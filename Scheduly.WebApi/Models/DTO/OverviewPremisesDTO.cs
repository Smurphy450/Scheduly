namespace Scheduly.WebApi.Models.DTO
{
    public class OverviewPremisesDTO
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public bool? Approved { get; set; }
    }
}
