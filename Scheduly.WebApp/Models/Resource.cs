using System;
using System.Collections.Generic;

namespace Scheduly.WebApp.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public bool? MustBeApproved { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ResourceCategory Category { get; set; } = null!;
}
