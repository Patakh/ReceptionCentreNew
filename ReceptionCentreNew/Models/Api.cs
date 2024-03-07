namespace ReceptionCentreNew.Models;
public class Token
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public string ExpiresIn { get; set; }
    public string UserName { get; set; }
    public string Issued { get; set; }
    public string Expires { get; set; }
}
public class LsServices
{
    public Guid ServicesSubId { get; set; }
    public string ProviderName { get; set; }
    public string ServiceName { get; set; }
    public string Hashtag { get; set; }
    public string SituationName { get; set; }
}


public class ServicesSubDocuments
{
    public string Name { get; set; }
    public string Needs { get; set; }
    public string Type { get; set; }
}
public class ServicesSubFailure
{
    public string FailureText { get; set; }
    public string Commentt { get; set; }
    public string LegalAct { get; set; }
}

public class ServicesSubResult
{
    public string Name { get; set; }
    public string Result { get; set; }
    public string Method { get; set; }
    public string PeriodProvider { get; set; }
    public string PeriodMfc { get; set; }
}
public class ServicesSubStop
{
    public string Text { get; set; }
    public string CountDay { get; set; }
    public string Commentt { get; set; }
    public string WeekName { get; set; }
}
public class ServicesSubTariff
{
    public string Type { get; set; }
    public string Tariff { get; set; }
    public string CountDayProcessing { get; set; }
    public string CountDayExecution { get; set; }
    public string CountDayReturn { get; set; }
    public string Week { get; set; }
}
public class ServicesSubWay
{
    public string NameWay { get; set; }
}
public class ServicesSubInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string LegalAct { get; set; }
    public List<ServicesSubInfo> Info { get; set; }
    public List<ServicesSubDocuments> Documents { get; set; }
    public List<ServicesSubFailure> Failure { get; set; }
    public List<ServicesSubFailure> FailureDocuments { get; set; }
    public List<ServicesSubResult> Result { get; set; }
    public List<ServicesSubStop> Stop { get; set; }
    public List<ServicesSubTariff> Tariff { get; set; }
    public List<ServicesSubWay> Way { get; set; }
    public List<ServicesSubWay> WayResult { get; set; }
}

public class HashtagServicesList
{
    public Guid Id { get; set; }
    public string Provider { get; set; }
    public string EmployeeFio { get; set; }
    public string Name { get; set; }
    public string[] Hashtags { get; set; }
}

public class ServicesSubCustomers
{
    public string CustomerFio { get; set; }
    public DateTime? DocumentBirthDate { get; set; }
    public string CustomerSex { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerSnils { get; set; }
}

public class ServicesSubCustomerData
{
    public List<ServicesSubCustomers> Customers { get; set; }
}
public class ServicesSubCustomerResponse
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public List<ServicesSubCustomers> Customers { get; set; }
}
public class AccountCase
{
    public string DataServicesInfoId { get; set; }
    public DateTime DateEnter { get; set; }
    public string ServicesProviderName { get; set; }
    public string ServicesSubName { get; set; }
    public DateTime? DateFinishTotal { get; set; }
    public string EmployeesFio { get; set; }
    public string MfcName { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
}