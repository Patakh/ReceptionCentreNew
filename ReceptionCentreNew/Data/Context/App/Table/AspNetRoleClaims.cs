﻿using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App;

public partial class AspNetRoleClaims
{
    public int Id { get; set; }

    public Guid RoleId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    public virtual AspNetRoles Role { get; set; } = null!;
}
