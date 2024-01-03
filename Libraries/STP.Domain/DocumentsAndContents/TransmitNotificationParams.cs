using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.DocumentsAndContents
{
    public class TransmitNotificationParams
    {
        public NotificationContacts NotificationContacts { get; set; }
        public UserInfo UserInfo { get; set; }
        public string EsdalReference { get; set; }
        public string NotifHtmlString { get; set; }
        public long TransmissionId { get; set; }
        public bool Indemnity { get; set; }
        public byte[] AttachXml { get; set; }
        public bool IsImminent { get; set; }
        public int DocType { get; set; }
        public int ActionFlag { get; set; }
        public string ErorrMessage { get; set; }
        public string DocTypeName { get; set; }
        public long ProjectId { get; set; }
        public int RevisionNo { get; set; }
        public int VersionNo { get; set; }
    }
}
