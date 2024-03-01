using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Роли
/// </summary>
public partial class SprEmployeesRole
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// наименование роли
    /// </summary>
    public string RoleName { get; set; } = null!;

    public string? Commentt { get; set; }

    public virtual ICollection<SprEmployeesRoleJoin> SprEmployeesRoleJoin { get; set; } = new List<SprEmployeesRoleJoin>();
}
