using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class GenerateEsdalNotificationParams
    {
        public int NotificationId { get; set; }
        public int ContactId { get; set; }
        public Dictionary<int, int> ICAStatusDictionary { get; set; }
        public string ImminentMoveStatus { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}