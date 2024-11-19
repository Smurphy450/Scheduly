using System;
using System.Collections.Generic;

namespace Scheduly.WebApp.Models;

public partial class ResourceCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
