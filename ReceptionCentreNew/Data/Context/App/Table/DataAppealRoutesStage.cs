using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Этапы обращения
/// </summary>
public partial class DataAppealRoutesStage
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Обращение
    /// </summary>
    public Guid DataAppealId { get; set; }

    /// <summary>
    /// Этап
    /// </summary>
    public int SprRoutesStageId { get; set; }

    /// <summary>
    /// Дата начала
    /// </summary>
    [Column(TypeName = "date")]
    public DateOnly DateStart { get; set; }

    /// <summary>
    /// Дата окончания
    /// </summary>
    [Column(TypeName = "date")]
    public DateOnly? DateStop { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Commentt { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    public string EmployeesNameAdd { get; set; } = null!;

    /// <summary>
    /// Регламентная дата
    /// </summary>
    public DateOnly? DateRegulation { get; set; }

    /// <summary>
    /// Регламентное количество дней
    /// </summary>
    public int? CountDayRegulation { get; set; }

    /// <summary>
    /// фактическое количество дней
    /// </summary>
    public string? CountDayFact { get; set; }

    /// <summary>
    /// Время начала
    /// </summary>
    public TimeOnly TimeStart { get; set; }

    /// <summary>
    /// Время окончания
    /// </summary>
    public TimeOnly? TimeStop { get; set; }

    /// <summary>
    /// фио сотрудника
    /// </summary>
    public string? EmployeesName { get; set; }

    public virtual DataAppeal DataAppeal { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprRoutesStage SprRoutesStage { get; set; } = null!;
}
