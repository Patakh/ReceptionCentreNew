using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Системные ошибки
/// </summary>
public partial class DataSystemErrors
{
    /// <summary>
    /// первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// текст ошибки
    /// </summary>
    [Display(Name = "Текст ошибки")]
    public string ErrorMessage { get; set; } = null!;

    /// <summary>
    /// внутреняя ошибка
    /// </summary>
    [Display(Name = "Внутреняя ошибка")]
    public string? ErrorInnerException { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    [Display(Name = "ФИО сотрудника")]
    public string EmployeesName { get; set; } = null!;

    /// <summary>
    /// сотрудник, связь с SprEmployees id
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// трассировка стека
    /// </summary>
    [Display(Name = "Трассировка стека")]
    public string StackTrace { get; set; } = null!;

    /// <summary>
    /// дата ошибки
    /// </summary>
    [Display(Name = "Дата ошибки")]
    public DateTime ErrorDate { get; set; }
}
