using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Вопросы
/// </summary>
public partial class SprSurveyQuestion
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст вопроса
    /// </summary>
    public string? Question { get; set; }

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

    public virtual ICollection<SprSurveyAnswer> SprSurveyAnswer { get; set; } = new List<SprSurveyAnswer>();
}
