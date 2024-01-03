using STP.Domain;
using STP.Domain.LoggingAndReporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.LoggingAndReporting
{
   public  interface IAuditLogService
    {
       
        long SaveNotifAuditLog(AuditLogIdentifiers auditLogType, string logMsg, int User_ID, long Org_ID = 0);
    }
}
