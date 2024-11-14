using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class AbsenceType
{
    public int AbsenceTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal? WageFactor { get; set; }

    public bool? MustBeApproved { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();
}
