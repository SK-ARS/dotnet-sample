using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class TransmissionParams
    {
        public NotificationContacts objcontact { get; set; }
        public UserInfo userInfo { get; set; }
        public long transmissionId { get; set; }
        public string esdalRef { get; set; }
        public int actionFlag { get; set; }
        public string errMessage { get; set; }
        public string docType { get; set; }
    }
}