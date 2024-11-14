using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public string Name { get; set; } = null!;

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? MustBeApproved { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
