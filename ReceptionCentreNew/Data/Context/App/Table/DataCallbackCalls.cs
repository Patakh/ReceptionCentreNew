using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [ForeignKey("DataCallback")]
    public Guid DataCallbackId { get; set; }

    /// <summary>
    /// Дата звонка
    /// </summary>
    [Display(Name = "Дата звонка")]
    public DateTime DateCall { get; set; }

    /// <summary>
    /// Продолжительность звонка
    /// </summary>
    [Display(Name = "Продолжительность звонка")]
    public string CallDuration { get; set; } = null!;

    /// <summary>
    /// Сотрудник принявший звонок
    /// </summary>
    [Display(Name = "Сотрудник принявший звонок")]
    public string? EmployeeFio { get; set; }

    /// <summary>
    /// Связь с сотрудником принявшем звонок
    /// </summary>
    [Display(Name = "Связь с сотрудником принявшем звонок")]
    [ForeignKey("SprEmployees")]
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// МФЦ в котором принят звонок
    /// </summary>
    [Display(Name = "Отдел")]
    public string? DepartmentName { get; set; }

    /// <summary>
    /// Связь с МФЦ в котором принят звонок
    /// </summary>
    [Display(Name = "Связь с отделом")]
    [ForeignKey("SprEmployeesDepartment")]
    public Guid SprEmployeesDepartmentId { get; set; }

    /// <summary>
    /// Признак сохранения звонка на FTP
    /// </summary>
    [Display(Name = "Признак сохранения звонка на FTP")]
    public bool? SaveFtp { get; set; }

    public virtual DataCallback DataCallback { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprEmployeesDepartment SprEmployeesDepartment { get; set; } = null!;
}
