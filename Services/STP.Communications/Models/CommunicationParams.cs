using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.CommunicationsInterface.Models
{
    public class CommunicationParams
    {
        public NotificationContacts objContact { get; set; }
        public UserInfo userInfo { get; set; }
        public long TransmId { get; set; }
        public byte[] content { get; set; }
        public byte[] attachment { get; set; }
        public string EsdalReference { get; set; }
        public int xmlAttach { get; set; }
        public bool isImminent { get; set; }
        public string docTypeName { get; set; }
        public string replyToEmail { get; set; }
    }
}