namespace Scheduly.WebApi.Models.DTO.User
{
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        public string OldPasswordHash { get; set; }
        public string NewPasswordHash { get; set; }
        public int ChangedByUserID { get; set; }
    }
}
