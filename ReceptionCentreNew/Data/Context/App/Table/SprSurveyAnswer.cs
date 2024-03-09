using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

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
    [Display(Name = "Связь с вопросом")]
    public Guid SprSurveyQuestionId { get; set; }

    /// <summary>
    /// Текст ответа
    /// </summary>
    [Display(Name = "Ответ")]
    public string? Answer { get; set; }

    /// <summary>
    /// Номер
    /// </summary>
    [Display(Name = "Номер")]
    public int? RecordNumber { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    [Display(Name = "Признак удаления")]
    public bool IsRemove { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    [Display(Name = "Добавил")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// Дата и время добавления записи
    /// </summary>
    [Display(Name = "Дата добавления")]
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Кто изменил запись
    /// </summary>
    [Display(Name = "Изменил")]
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// Дата и время последних изменений
    /// </summary>
    [Display(Name = "Дата изменения")]
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// Комментарий при изменении
    /// </summary>
    [Display(Name = "Причина изменения")]
    public string? CommenttModify { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    [Display(Name = "Ip add")]
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего
    /// </summary>
    [Display(Name = "Ip edit")]
    public string? IpAddressModify { get; set; }

    public virtual ICollection<DataSurvey> DataSurvey { get; set; } = new List<DataSurvey>();

    public virtual SprSurveyQuestion SprSurveyQuestion { get; set; } = null!;
}
