using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

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
    /// связь с пользователями, SprEmployees id
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// связь с ролью, SprEmployeesRole id
    /// </summary>
    [Display(Name = "Роль")]
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
    [Display(Name = "Комментарии")]
    public string? Commentt { get; set; }

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprEmployeesRole SprEmployeesRole { get; set; } = null!;
}
