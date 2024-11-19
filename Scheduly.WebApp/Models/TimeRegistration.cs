using System;
using System.Collections.Generic;

namespace Scheduly.WebApp.Models;

public partial class TimeRegistration
{
    public int TimeId { get; set; }

    public int UserId { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset? End { get; set; }

    public virtual User User { get; set; } = null!;
}
