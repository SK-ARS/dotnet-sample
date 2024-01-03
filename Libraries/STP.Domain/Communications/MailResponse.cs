using System;

namespace STP.Domain.Communications
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