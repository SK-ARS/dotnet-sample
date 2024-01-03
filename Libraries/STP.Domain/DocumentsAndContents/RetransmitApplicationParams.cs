using STP.Domain.HelpdeskTools;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class RetransmitApplicationParams
    {
        public int TransmissionId { get; set; }
        public RetransmitDetails RetransmitDetails { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}