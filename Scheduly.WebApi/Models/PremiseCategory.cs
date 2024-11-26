using System;
using System.Collections.Generic;

namespace Scheduly.WebApi.Models;

public partial class PremiseCategory
{
    public int PremiseCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Premise> Premises { get; set; } = new List<Premise>();
}
