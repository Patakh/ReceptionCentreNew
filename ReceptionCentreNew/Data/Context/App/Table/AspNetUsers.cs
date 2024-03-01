﻿using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

public partial class AspNetUsers
{
    public Guid Id { get; set; }

    public Guid SprEmployeesDepartmentId { get; set; }

    public Guid SprEmployeesJobPosId { get; set; }

    public string EmployeesName { get; set; } = null!;

    public string EmployeesLogin { get; set; } = null!;

    public string? EmployeesPass { get; set; }

    public bool? EmployeesActive { get; set; }

    public bool IsRemove { get; set; }

    public string? EmployeesNameAdd { get; set; }

    public DateTime? DateAdd { get; set; }

    public string? EmployeesNameModify { get; set; }

    public DateTime? DateModify { get; set; }

    public string? CommenttModify { get; set; }

    public bool CanTakeAppeal { get; set; }

    public string? IpAddressAdd { get; set; }

    public string? IpAddressModify { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; } = new List<AspNetUserClaims>();

    public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; } = new List<AspNetUserLogins>();

    public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; } = new List<AspNetUserTokens>();

    public virtual ICollection<AspNetRoles> Role { get; set; } = new List<AspNetRoles>();
}
