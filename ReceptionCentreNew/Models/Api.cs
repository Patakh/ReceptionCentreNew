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
    public string name { get; set; }
    public string needs { get; set; }
    public string type { get; set; }
}
public class ServicesSubFailure
{
    public string failure_text { get; set; }
    public string commentt { get; set; }
    public string legal_act { get; set; }
}

public class ServicesSubResult
{
    public string name { get; set; }
    public string result { get; set; }
    public string method { get; set; }
    public string period_provider { get; set; }
    public string period_mfc { get; set; }
}
public class ServicesSubStop
{
    public string text { get; set; }
    public string count_day { get; set; }
    public string commentt { get; set; }
    public string week_name { get; set; }
}
public class ServicesSubTariff
{
    public string type { get; set; }
    public string tariff { get; set; }
    public string count_day_processing { get; set; }
    public string count_day_execution { get; set; }
    public string count_day_return { get; set; }
    public string week { get; set; }
}
public class ServicesSubWay
{
    public string name_way { get; set; }
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
    public string customer_fio { get; set; }
    public DateTime? document_birth_date { get; set; }
    public string customer_sex { get; set; }
    public string customer_address { get; set; }
    public string customer_email { get; set; }
    public string customer_snils { get; set; }
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