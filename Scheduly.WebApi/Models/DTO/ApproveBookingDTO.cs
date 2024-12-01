namespace Scheduly.WebApi.Models.DTO
{
    public class ApproveBookingDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public bool Approved { get; set; }
        public string Description { get; set; }
    }
}
