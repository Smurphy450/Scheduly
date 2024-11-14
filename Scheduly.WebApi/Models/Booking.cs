using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class Booking
{
    public int BookingsId { get; set; }

    public int UserId { get; set; }

    public int? PremisId { get; set; }

    public int? ResourceId { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset? End { get; set; }

    public bool? Approved { get; set; }

    public virtual Premise? Premis { get; set; }

    public virtual Resource? Resource { get; set; }

    public virtual User User { get; set; } = null!;
}
