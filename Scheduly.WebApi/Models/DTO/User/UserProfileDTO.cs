namespace Scheduly.WebApi.Models.DTO.User
{
    public class UserProfileDTO
    {
        // User properties
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Password { get; set; } // New property for unhashed password

        // Profile properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public int? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Admin { get; set; }
        public string? City { get; set; }
    }
}