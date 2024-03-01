using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Связь ролей и сотрудников
/// </summary>
public partial class SprEmployeesRoleJoin
{
    /// <summary>
    /// первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// связь с пользователями, spr_employees id
    /// </summary>
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// связь с ролью, spr_employees_role id
    /// </summary>
    public int SprEmployeesRoleId { get; set; }

    /// <summary>
    /// кто добавил запись
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата добавления записи
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// комментарий
    /// </summary>
    public string? Commentt { get; set; }

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprEmployeesRole SprEmployeesRole { get; set; } = null!;
}
