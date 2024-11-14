using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public int? UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Address { get; set; }

    public int? ZipCode { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? Admin { get; set; }

    public virtual User? User { get; set; }

    public virtual ZipCode? ZipCodeNavigation { get; set; }
}
