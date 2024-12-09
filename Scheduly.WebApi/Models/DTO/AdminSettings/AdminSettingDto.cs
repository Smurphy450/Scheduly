namespace Scheduly.WebApi.Models.DTO.AdminSettings
{
    public class AdminSettingDTO
    {
        public int SettingsId { get; set; }
        public string Name { get; set; } = null!;
        public bool? Enabled { get; set; }
        public int UserId { get; set; }
    }
}