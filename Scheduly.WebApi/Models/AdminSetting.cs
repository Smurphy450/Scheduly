using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class AdminSetting
{
    public int SettingsId { get; set; }

    public string Name { get; set; } = null!;

    public bool? Enabled { get; set; }
}
