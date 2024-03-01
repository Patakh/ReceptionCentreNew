using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Ответы
/// </summary>
public partial class SprSurveyAnswer
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Вопрос
    /// </summary>
    public Guid SprSurveyQuestionId { get; set; }

    /// <summary>
    /// Текст ответа
    /// </summary>
    public string? Answer { get; set; }

    /// <summary>
    /// Номер
    /// </summary>
    public int? RecordNumber { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsRemove { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// Дата и время добавления записи
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Кто изменил запись
    /// </summary>
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// Дата и время последних изменений
    /// </summary>
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// Комментарий при изменении
    /// </summary>
    public string? CommenttModify { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего
    /// </summary>
    public string? IpAddressModify { get; set; }

    public virtual ICollection<DataSurvey> DataSurvey { get; set; } = new List<DataSurvey>();

    public virtual SprSurveyQuestion SprSurveyQuestion { get; set; } = null!;
}
