using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class AuditLogIdentifiersInputParams
    {
        public UserInfo UserInfo { get; set; }
        public AuditLogIdentifiers AuditLog { get; set; }

       
    }
}