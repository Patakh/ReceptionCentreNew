﻿using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

public partial class AspNetUserTokens
{
    public Guid UserId { get; set; }

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public virtual AspNetUsers User { get; set; } = null!;
}
