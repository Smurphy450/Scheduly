namespace Scheduly.WebApi.Models;

public class AdminSettingDTO
{
    public int SettingsId { get; set; }
    public string Name { get; set; } = null!;
    public bool? Enabled { get; set; }
    public int UserId { get; set; }
}
