using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceptionCentreNew.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }
    public class LsServices
    {
        public Guid services_sub_id { get; set; }
        public string provider_name { get; set; }
        public string service_name { get; set; }
        public string hashtag { get; set; }
        public string situation_name { get; set; }
    }
    public class Services_sub_info
    {
        public string name { get; set; }
        public string description { get; set; }
        public string legal_act { get; set; }
    }
    public class Services_sub_documents
    {
        public string name { get; set; }
        public string needs { get; set; }
        public string type { get; set; }
    }
    public class Services_sub_failure
    {
        public string failure_text { get; set; }
        public string commentt { get; set; }
        public string legal_act { get; set; }
    }

    public class Services_sub_result
    {
        public string name { get; set; }
        public string result { get; set; }
        public string method { get; set; }
        public string period_provider { get; set; }
        public string period_mfc { get; set; }
    }
    public class Services_sub_stop
    {
        public string text { get; set; }
        public string CountDay { get; set; }
        public string commentt { get; set; }
        public string week_name { get; set; }
    }
    public class Services_sub_tariff
    {
        public string type { get; set; }
        public string tariff { get; set; }
        public string count_day_processing { get; set; }
        public string count_day_execution { get; set; }
        public string count_day_return { get; set; }
        public string week { get; set; }
    }
    public class Services_sub_way
    {
        public string name_way { get; set; }
    }
    public class ServicesSubInfo
    {
        public List<Services_sub_info> Info { get; set; }
        public List<Services_sub_documents> Documents { get; set; }
        public List<Services_sub_failure> Failure { get; set; }
        public List<Services_sub_failure> FailureDocuments { get; set; }
        public List<Services_sub_result> Result { get; set; }
        public List<Services_sub_stop> Stop { get; set; }
        public List<Services_sub_tariff> Tariff { get; set; }
        public List<Services_sub_way> Way { get; set; }
        public List<Services_sub_way> WayResult { get; set; }
    }

    public class Hashtag_services_list
    {
        public Guid id { get; set; }
        public string provider { get; set; }
        public string EmployeeFio { get; set; }
        public string name { get; set; }
        public string[] hashtags { get; set; }
    }

    public class Services_sub_customers
    {
        public string CustomerFio { get; set; }
        public DateTime? DocumentBirthDate { get; set; }
        public string CustomerSex { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerSnils { get; set; }
    }

    public class Services_sub_customer_data
    {
        public List<Services_sub_customers> Customers { get; set; }
    }
    public class Services_sub_customer_response
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public List<Services_sub_customers> Customers { get; set; }
    }
    public class Account_case
    {
        public string DataServicesInfoId { get; set; }
        public DateTime date_enter { get; set; }
        public string services_provider_name { get; set; }
        public string services_sub_name { get; set; }
        public DateTime? date_finish_total { get; set; }
        public string employees_fio { get; set; }
        public string MfcName { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
    }

}