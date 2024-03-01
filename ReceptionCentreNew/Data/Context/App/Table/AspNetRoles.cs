using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

public partial class AspNetRoles
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaims>();

    public virtual ICollection<AspNetUsers> User { get; set; } = new List<AspNetUsers>();
}
