using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Электронная почта
/// </summary>
public partial class DataAppealEmail
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// id обращения
    /// </summary>
    [Display(Name = "ID обращения")]
    public Guid? DataAppealId { get; set; }

    /// <summary>
    /// электронная почта
    /// </summary>
    [Display(Name = "Электронная почта")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// id сотрудника
    /// </summary>
    [Display(Name = "ID сотрудника")]
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// фио сотрудника
    /// </summary>
    [Display(Name = "Сотрудник")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время письма
    /// </summary>
    [Display(Name = "Дата и время письма")]
    public DateTime DateEmail { get; set; }

    /// <summary>
    /// Текст письма
    /// </summary>
    [Display(Name = "Текст письма")]
    public string? TextEmail { get; set; }

    /// <summary>
    /// 1 исходящий 2 входящий
    /// </summary>
    [Display(Name = "Тип")]
    public short EmailType { get; set; }

    /// <summary>
    /// идентификатор сообщения на почте
    /// </summary>
    [Display(Name = "Идентификатор письма на почте")]
    public string? Uids { get; set; }

    /// <summary>
    /// false - не прочитано, true - прочитано
    /// </summary>
    [Display(Name = "Прочитано/непрочитано")]
    public bool IsRead { get; set; }

    /// <summary>
    /// Заголовок
    /// </summary>
    [Display(Name = "Заголовок письма")]
    public string? Caption { get; set; }

    public virtual DataAppeal? DataAppeal { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }
}
