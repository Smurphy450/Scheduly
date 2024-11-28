namespace Scheduly.WebApi.Models;

public class AdminSettingDto
{
    public int SettingsId { get; set; }
    public string Name { get; set; } = null!;
    public bool? Enabled { get; set; }
}
