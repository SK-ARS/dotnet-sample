using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class HaulierNotificationParams
    {
        public int NotificationId { get; set; }
        public Enums.DocumentType DocumentType { get; set; }
        public int ContactId { get; set; }
        public UserInfo SessionInfo { get; set; }
    }
}