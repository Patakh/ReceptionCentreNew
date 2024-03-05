namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Шаблоны сообщений по сотрудникам
/// </summary>
public partial class SprEmployeesMessageTemplate
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// связь с сотрудником?SprEmployees  Id
    /// </summary>
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// текст сообщения
    /// </summary>
    public string MessageText { get; set; } = null!;

    /// <summary>
    /// Приоритет
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// дата и время добавления записи
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// кто добавил запись
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время изменений
    /// </summary>
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// кто изменил запись
    /// </summary>
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// комментарий при изменении
    /// </summary>
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
    public bool IsRemove { get; set; }

    public virtual SprEmployees SprEmployees { get; set; } = null!;
}
