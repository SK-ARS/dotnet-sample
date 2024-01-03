using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class NENAuditLogInputParams
    {
       public AuditLogIdentifiers AuditLogIdentifiers { get; set; } 
       public string LogMsg { get; set; }
        public long OrganisationId { get; set; }
        public int UserId { get; set; }

    }
}