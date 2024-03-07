using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceptionCentreNew.Models
{
    public class Sources
    {
    }
    public class SourcesViewModel
    {
        public IEnumerable<SourcesModel> SourceModel { get; set; }

        public PageInfo PageInfo { get; set; }
    }
    public class SourcesModel
    {
        public Guid Id { get; set; }
        public string EmployeesName { get; set; }
        public string ApplicantName { get; set; }
        public string Contact { get; set; }
        public DateTime? DateAdd { get; set; }
        public string Option { get; set; }
        public bool SaveFtp { get; set; }
        public string NumberAppeal { get; set; }
        public SourcesType Type { get; set; }
    }

    public enum SourcesType
    {
        call,
        email,
        sms,
        social_networks
    }
}