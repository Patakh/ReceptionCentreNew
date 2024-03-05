using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Звонки
/// </summary>
public partial class DataCallbackCalls
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Связь с заказом звонка
    /// </summary>
    public Guid DataCallbackId { get; set; }

    /// <summary>
    /// Дата звонка
    /// </summary>
    public DateTime DateCall { get; set; }

    /// <summary>
    /// Продолжительность звонка
    /// </summary>
    public string CallDuration { get; set; } = null!;

    /// <summary>
    /// Сотрудник принявший звонок
    /// </summary>
    public string? EmployeeFio { get; set; }

    /// <summary>
    /// Связь с сотрудником принявшем звонок
    /// </summary>
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// МФЦ в котором принят звонок
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// Связь с МФЦ в котором принят звонок
    /// </summary>
    public Guid SprEmployeesDepartmentId { get; set; }

    /// <summary>
    /// Признак сохранения звонка на FTP
    /// </summary>
    public bool? SaveFtp { get; set; }

    public virtual DataCallback DataCallback { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprEmployeesDepartment SprEmployeesDepartment { get; set; } = null!;
}
