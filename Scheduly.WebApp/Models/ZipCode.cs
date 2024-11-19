using System;
using System.Collections.Generic;

namespace Scheduly.WebApp.Models;

public partial class ZipCode
{
    public int ZipCode1 { get; set; }

    public string City { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
