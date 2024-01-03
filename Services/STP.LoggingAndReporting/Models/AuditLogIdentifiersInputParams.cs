using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class AuditLogIdentifiersInputParams
    {
        public UserInfo userInfo { get; set; }
        public AuditLogIdentifiers auditLog { get; set; }

       
    }
}