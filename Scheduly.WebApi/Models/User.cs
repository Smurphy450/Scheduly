﻿using Scheduly.WebApi.Models.DTO.Common;
using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    public virtual ICollection<SchedulyLogging> SchedulyLoggings { get; set; } = new List<SchedulyLogging>();

    public virtual ICollection<TimeRegistration> TimeRegistrations { get; set; } = new List<TimeRegistration>();
}
