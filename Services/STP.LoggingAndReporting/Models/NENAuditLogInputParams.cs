using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class NENAuditLogInputParams
    {
       public AuditLogIdentifiers auditLogIdentifiers { get; set; } 
       public string logMsg { get; set; }
        public long Org_ID { get; set; }
        public int User_ID { get; set; }

    }
}