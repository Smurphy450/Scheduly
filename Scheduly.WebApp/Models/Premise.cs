using System;
using System.Collections.Generic;

namespace Scheduly.WebApp.Models;

public partial class Premise
{
    public int PremisId { get; set; }

    public string Name { get; set; } = null!;

    public string? Size { get; set; }

    public string? Description { get; set; }

    public bool? MustBeApproved { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
