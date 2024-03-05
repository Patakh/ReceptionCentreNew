using ReceptionCentreNew.Domain.Models.Entities.Functions;

namespace ReceptionCentreNew.Models.WebApi; 
public class ResponseClaimStatistics : ApiResponse
{
    public DataAppealClaimStatistics ClaimStatistics { get; set; }
}

public class ApiResponse
{
    public string ResponseMessage { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}

public class ResponseClaimWeek : ApiResponse
{
    public IEnumerable<DataAppealClaimWeek> ClaimWeek { get; set; }
}
public class ResponseClaimYear : ApiResponse
{
    public IEnumerable<DataAppealClaimYear> ClaimYear { get; set; }
}

 
public class JitsiCallsRequest
{
    /// <summary>
    /// Id
    /// </summary>        
    public Guid Id { get; set; }

    /// <summary>
    /// Id МФЦ
    /// </summary>        
    public string IdMfc { get; set; }

    /// <summary>
    /// Id Услуги
    /// </summary>        
    public string IdService { get; set; }

    /// <summary>
    /// Id Сотрудника
    /// </summary>        
    public string IdEmployee { get; set; }

    /// <summary>
    /// Id ФТП
    /// </summary>        
    public string IdFtp { get; set; }

    /// <summary>
    /// Номер дела
    /// </summary>        
    public string CaseNumber { get; set; }

    /// <summary>
    /// ФИО заявителя
    /// </summary>        
    public string CustomerName { get; set; }

    /// <summary>
    /// Телефон заявителя
    /// </summary>        
    public string CustomerPhone { get; set; }
    /// <summary>
    /// Формат аудиофайла
    /// </summary>
    public string AudioFormat { get; set; }
    /// <summary>
    /// Тип звонка
    /// </summary>       
    public JitsiCallType CallType { get; set; }
    /// <summary>
    /// IP адрес
    /// </summary>        
    public string IpAddress { get; set; }
    /// <summary>
    /// Дата звонка
    /// </summary>
    public DateTime? DateCall { get; set; }
}
/// <summary>
/// Типы звонков
/// </summary>
public enum JitsiCallType
{
    /// <summary>
    /// Входящий звонок
    /// </summary>
    incoming,
    /// <summary>
    /// Исхорящий звонок
    /// </summary>
    outgoing,
    /// <summary>
    /// Исхорящий звонок
    /// </summary>
    outgoing_callback
}
public class JitsiCallFile
{
    /// <summary>
    /// Имя файла (Guid)
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Время разговора
    /// </summary>
    public string CallDuration { get; set; }
    /// <summary>
    /// Байты
    /// </summary>
    public sbyte[] Filebyte { get; set; }
}

[Serializable]
public class JitsiCallsUnionRequest
{
    /// <summary>
    /// Id
    /// </summary>        
    public Guid Id { get; set; }

    /// <summary>
    /// Id МФЦ
    /// </summary>        
    public string IdMfc { get; set; }

    /// <summary>
    /// Id Услуги
    /// </summary>        
    public string IdService { get; set; }

    /// <summary>
    /// Id Сотрудника
    /// </summary>        
    public string IdEmployee { get; set; }

    /// <summary>
    /// Id ФТП
    /// </summary>        
    public string IdFtp { get; set; }

    /// <summary>
    /// Номер дела
    /// </summary>        
    public string CaseNumber { get; set; }

    /// <summary>
    /// ФИО заявителя
    /// </summary>        
    public string CustomerName { get; set; }

    /// <summary>
    /// Телефон заявителя
    /// </summary>        
    public string CustomerPhone { get; set; }
    /// <summary>
    /// Формат аудиофайла
    /// </summary>
    public string AudioFormat { get; set; }
    /// <summary>
    /// Тип звонка
    /// </summary>       
    public JitsiCallType CallType { get; set; }
    /// <summary>
    /// IP адрес
    /// </summary>        
    public string IpAddress { get; set; }
    /// <summary>
    /// Дата звонка
    /// </summary>
    public DateTime? DateCall { get; set; }
    /// <summary>
    /// Время разговора
    /// </summary>
    public string CallDuration { get; set; }
    /// <summary>
    /// Байты
    /// </summary>
    public sbyte[] Filebyte { get; set; }
    /// <summary>
    /// Фио сотрудника
    /// </summary>
    public string EmployeeFio { get; set; }
}

public class JitsiRequest
{
    /// <summary>
    /// Id
    /// </summary>        
    public Guid Id { get; set; }

    /// <summary>
    /// Id МФЦ
    /// </summary>        
    public string IdMfc { get; set; }

    /// <summary>
    /// Id Услуги
    /// </summary>        
    public string IdService { get; set; }

    /// <summary>
    /// Id Сотрудника
    /// </summary>        
    public string IdEmployee { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string EmployeeFio { get; set; }

    /// <summary>
    /// Id ФТП
    /// </summary>        
    public string IdFtp { get; set; }

    /// <summary>
    /// Номер дела
    /// </summary>        
    public string CaseNumber { get; set; }

    /// <summary>
    /// ФИО заявителя
    /// </summary>        
    public string CustomerName { get; set; }

    /// <summary>
    /// Телефон заявителя
    /// </summary>        
    public string CustomerPhone { get; set; }
    /// <summary>
    /// Формат аудиофайла
    /// </summary>
    public string AudioFormat { get; set; }
    /// <summary>
    /// Тип звонка
    /// </summary>       
    public JitsiCallType CallType { get; set; }
    /// <summary>
    /// IP адрес
    /// </summary>        
    public string IpAddress { get; set; }
    /// <summary>
    /// Дата звонка
    /// </summary>
    public DateTime? DateCall { get; set; }
    /// <summary>
    /// Время разговора
    /// </summary>
    public string CallDuration { get; set; }
    /// <summary>
    /// Байты
    /// </summary>
    public sbyte[] Filebyte { get; set; }

    /// <summary>
    /// Команда 0-входящий, 1-исходящий
    /// </summary>
    public int Command { get; set; }

    /// <summary>
    /// идентефикатор звонка в Jitsi
    /// </summary>
    public string CallId { get; set; }
}