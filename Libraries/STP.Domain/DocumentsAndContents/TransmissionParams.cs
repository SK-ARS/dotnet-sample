using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class TransmissionParams
    {
        public NotificationContacts ObjectContact { get; set; }
        public UserInfo UserInfo { get; set; }
        public long TransmissionId { get; set; }
        public string ESDALReference { get; set; }
        public int ActionFlag { get; set; }
        public string ErrorMessage { get; set; }
        public string DocumentType { get; set; }
    }
}