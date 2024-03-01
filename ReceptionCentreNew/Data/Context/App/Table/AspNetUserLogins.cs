﻿using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

public partial class AspNetUserLogins
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    public Guid UserId { get; set; }

    public virtual AspNetUsers User { get; set; } = null!;
}
