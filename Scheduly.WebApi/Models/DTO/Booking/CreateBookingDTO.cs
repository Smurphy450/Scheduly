namespace Scheduly.WebApi.Models.DTO.Booking
{
    public class CreateBookingDTO
    {
        public int UserId { get; set; }
        public int? PremiseId { get; set; }
        public int? ResourceId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public bool? Approved { get; set; }
    }
}
