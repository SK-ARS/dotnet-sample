using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.CommunicationsInterface.Models
{
    public class MailResponse
    {
        public int ResponseId { get; set; }
        public int UserId { get; set; }
        public int OrganisationId { get; set; }
        public bool EnableAutoResponse { get; set; }
        public Byte[] ReplyMailText { get; set; }
        public string ReplyMailPdf { get; set; }
        public string EmailID { get; set; }
    }
}