using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Обращения по сотрудникам
/// </summary>
public partial class SprEmployeesTextAppealTemplate
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// связь с сотрудником?SprEmployees  id
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// текст обращения
    /// </summary>
    [Display(Name = "Текст сообщения")]
    public string TextAppeal { get; set; } = null!;

    /// <summary>
    /// Приоритет
    /// </summary>
    [Display(Name = "Приоритет")]
    public int? Sort { get; set; }

    /// <summary>
    /// дата и время добавления записи
    /// </summary>
    [Display(Name = "Дата добавления")]
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// кто добавил запись
    /// </summary>
    [Display(Name = "Добавил")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время изменений
    /// </summary>
    [Display(Name = "Дата изменения")]
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// кто изменил запись
    /// </summary>
    [Display(Name = "Изменил")]
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// комментарий при изменении
    /// </summary>
    [Display(Name = "Причина изменения")]
    public string? CommenttModify { get; set; }

    /// <summary>
    /// ip адрес добавившего запись
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего запись
    /// </summary>
    public string? IpAddressModify { get; set; }

    /// <summary>
    /// признак удаления записи f - не удалена,t - удалена
    /// </summary>
    [Display(Name = "Признак удаления")]
    public bool IsRemove { get; set; }

    public virtual SprEmployees SprEmployees { get; set; } = null!;
}
