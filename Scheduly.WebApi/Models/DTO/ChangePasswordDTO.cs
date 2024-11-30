namespace Scheduly.WebApi.Models.DTO
{
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        public string OldPasswordHash { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
