using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class AuditLogParams
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public string NENNotificationNo { get; set; }

        public long OrgId { get; set; }
    }
}