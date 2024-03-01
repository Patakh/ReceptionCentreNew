using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

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
    public string ErrorMessage { get; set; } = null!;

    /// <summary>
    /// внутреняя ошибка
    /// </summary>
    public string? ErrorInnerException { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string EmployeesName { get; set; } = null!;

    /// <summary>
    /// сотрудник, связь с spr_employees id
    /// </summary>
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// трассировка стека
    /// </summary>
    public string StackTrace { get; set; } = null!;

    /// <summary>
    /// дата ошибки
    /// </summary>
    public DateTime ErrorDate { get; set; }
}
