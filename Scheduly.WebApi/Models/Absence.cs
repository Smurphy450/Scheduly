using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class Absence
{
    public int AbsenceId { get; set; }

    public int AbsenceTypeId { get; set; }

    public int UserId { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset? End { get; set; }

    public string? Description { get; set; }

    public bool? Approved { get; set; }

    public virtual AbsenceType AbsenceType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
