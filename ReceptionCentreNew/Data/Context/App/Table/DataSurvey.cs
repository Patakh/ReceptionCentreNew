namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Вопросы клиентов
/// </summary>
public partial class DataSurvey
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
    /// Ответ
    /// </summary>
    public Guid SprSurveyAnswerId { get; set; }

    /// <summary>
    /// Текст вопроса
    /// </summary>
    public string? Question { get; set; }

    /// <summary>
    /// Текст ответа
    /// </summary>
    public string? Answer { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Дата опроса
    /// </summary>
    public DateTime? DateSurvey { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }

    public virtual SprSurveyAnswer SprSurveyAnswer { get; set; } = null!;

    public virtual SprSurveyQuestion SprSurveyQuestion { get; set; } = null!;
}
