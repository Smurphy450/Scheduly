using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class SchedulyLogging
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string AffectedData { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
