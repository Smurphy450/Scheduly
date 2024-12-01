using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public bool? Sms { get; set; }

    public bool? Email { get; set; }

    public string? Message { get; set; }

    public virtual User User { get; set; } = null!;
}