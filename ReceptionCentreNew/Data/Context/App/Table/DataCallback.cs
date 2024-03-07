namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Таблица хранения заказных звонков
/// </summary>
public partial class DataCallback
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата на которую заказан звонок
    /// </summary>
    public DateTime DateOrder { get; set; }

    /// <summary>
    /// Номер телефона заявителя
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Владелец номера телефона
    /// </summary>
    public string? CustomerFio { get; set; }

    /// <summary>
    /// Статус звонка (1 - Новый звонок, 2 - Обработан, 3 - Не отвеченный)
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Количество попыток
    /// </summary>
    public int CountTry { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// ID звонка
    /// </summary>
    public int CallbackId { get; set; }

    /// <summary>
    /// Полное занесение звонка  в базу
    /// </summary>
    public bool IsSync { get; set; }

    /// <summary>
    /// Дата и время занесения записи
    /// </summary>
    public DateTime DateAdd { get; set; }

    /// <summary>
    /// Дата закрытия
    /// </summary>
    public DateOnly? DateClose { get; set; }

    /// <summary>
    /// В ручную?
    /// </summary>
    public bool? IsHand { get; set; }

    /// <summary>
    /// Сотрудник закрывший
    /// </summary>
    public Guid? SprEmployeesIdClose { get; set; }

    public virtual ICollection<DataCallbackCalls> DataCallbackCalls { get; set; } = new List<DataCallbackCalls>();

    public virtual SprEmployees? SprEmployees { get; set; }

    public virtual SprEmployees? SprEmployeesIdCloseNavigation { get; set; }
}
