namespace Scheduly.WebApi.Models
{
    public class UserAuthRequest
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}