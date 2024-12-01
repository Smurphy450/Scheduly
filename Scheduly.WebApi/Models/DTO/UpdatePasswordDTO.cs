namespace Scheduly.WebApi.Models.DTO
{
    public class UpdatePasswordDTO
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
    }
}
