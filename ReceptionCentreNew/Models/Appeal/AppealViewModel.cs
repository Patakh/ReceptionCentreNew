using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Domain.Concrete;
using ReceptionCentreNew.Domain.Models.Entities.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceptionCentreNew.Models
{
    public partial class AppealViewModel
    {
        public IEnumerable<DataAppealSelect> DataAppealSelectList { get; set; }
        public IEnumerable<DataAppealCommentt> DataAppealCommenttList { get; set; }
        public IEnumerable<DataAppealCommenttRecipients> DataAppealCommenttRecipientList { get; set; }
        public IEnumerable<DataChangeLog> DataChangeLogList { get; set; }
        public IEnumerable<DataAppealRouteStageSelect> DataAppealRoutesStageList { get; set; }
        public IEnumerable<DataAppealEmail> DataAppealEmailList { get; set; }
        public int DataAppealEmailListCount { get; set; }
        public IEnumerable<DataAppealCall> DataAppealCallList { get; set; }
        public int DataAppealCallListCount { get; set; }
        public IEnumerable<DataAppealFile> DataAppealFileList { get; set; }
        public int DataAppealFileListCount { get; set; }
        public IEnumerable<DataAppealMessage> DataAppealSmsList { get; set; }
        public int DataAppealSmsListCount { get; set; }
        public Guid DataAppealId { get; set; }
        public string DataAppealNumber { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid CurrentEmployeeId { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public partial class EmailsViewModel
    {        
        public IEnumerable<DataAppealEmailSelect> EmailList { get; set; }
        public PageInfo PageInfo { get; set; }
    }
    public partial class CallsViewModel
    {
        public IEnumerable<DataAppealCallSelect> CallList { get; set; }
        public PageInfo PageInfo { get; set; }
    }
    public partial class SmsViewModel
    {
        public IEnumerable<DataAppealMessage> SmsList { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public partial class NotificationsViewModel
    {
        public IEnumerable<DataEmployeesNotification> NotificationList { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}