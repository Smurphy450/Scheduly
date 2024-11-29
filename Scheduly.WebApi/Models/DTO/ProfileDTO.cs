namespace Scheduly.WebApi.Models;

public class ProfileDTO
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public int ZipCode { get; set; }
    public string City { get; set; }
    public string? PhoneNumber { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
}
