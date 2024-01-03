using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.CommunicationsInterface.Models
{
    public class AutoResponseMailParams
    {
        public string EsdalReference { get; set; }
        public string HaulierEmailAddress { get; set; }
        public string organisationName { get; set; }
        public MailResponse responseMailDetails { get; set; }
    }
}