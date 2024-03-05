using ReceptionCentreNew.Data.Context.App;

namespace ReceptionCentreNew.Models;
public class ModelsCallBack
{
    public class Response
    {
        public int? AllCount { get; set; }
        public int? UpdateCount { get; set; }
        public int? ErrorCount { get; set; }
        public int? ProcessCount { get; set; }
        public int? NotSyncCount { get; set; }
        public int? SyncCount { get; set; }
    }
    public class CallBackList
    {
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public List<CallBackItem> Value { get; set; }
    }
    public class CallBackQuery
    {
        public Guid? BaseId { get; set; }
        public DateTime DateOrder { get; set; }
        public string PhoneNumber { get; set; }
        public string Ip { get; set; } 
        public int? Type { get; set; }
        public int CallbackId { get; set; }
    }
    public class CallBackViewModel
    {
        public IEnumerable<DataCallback> DataCallback { get; set; }
        public Guid EmployeeId { get; set; }
    }

    public class CallBackApiResponse
    {
        public string Query { get; set; }
    }
    public class ResponseCustomer
    {
        public List<ApiServicesSubCustomers> Customers { get; set; }
    }
    public class ApiServicesSubCustomers
    {
        /// <summary>
        /// ФИО
        /// </summary>
        public string CustomerFio { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? DocumentBirthDate { get; set; }
        /// <summary>
        /// Пол
        /// </summary>
        public string CustomerSex { get; set; }
        /// <summary>
        /// Адрес 
        /// </summary>
        public string CustomerAddress { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string CustomerEmail { get; set; }
        /// <summary>
        /// Снилс
        /// </summary>
        public string CustomerSnils { get; set; }
    }

    public class CallBackItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public string Ip { get; set; }
    }

    public class CallBackConfirmItem
    {
        public int Id { get; set; }
    }
}