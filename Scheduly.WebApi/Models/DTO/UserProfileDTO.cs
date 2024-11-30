namespace Scheduly.WebApi.Models.DTO
{
    public class UserProfileDTO
    {
        // User properties
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; } // Optional for sending data back to the API

        // Profile properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public int? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Admin { get; set; }
        public string City { get; set; }
    }
}
