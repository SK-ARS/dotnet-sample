using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class AuditLogIdentifiersParams
    {
        public AuditLogIdentifiers AuditLogType { get; set; }

        public string LogMsg { get; set; }

        public int UserId { get; set; }

        public long OrganisationId { get; set; }

    }
}