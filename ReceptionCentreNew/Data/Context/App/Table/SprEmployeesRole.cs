using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

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
    [Display(Name = "Роль")]
    public string RoleName { get; set; } = null!;

    [Display(Name = "Комментарий")]
    public string? Commentt { get; set; }

    public virtual ICollection<SprEmployeesRoleJoin> SprEmployeesRoleJoin { get; set; } = new List<SprEmployeesRoleJoin>();
}
