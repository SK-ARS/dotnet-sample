using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.LoggingAndReporting;
using STP.LoggingAndReporting.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.LoggingAndReporting.Controllers
{
    public class AuditLogController : ApiController
    {

      
        /// <summary>
        ///To get Audit Log.
        /// </summary>         
        /// <returns></returns>
        [HttpPost]
        [Route("AuditLog/SaveNotificationAuditLog")]
        public IHttpActionResult SaveNotifAuditLog(NENAuditLogInputParams nenAuditLog)
        {
            try
            {
                long result = LoggingProvider.Instance.SaveNotifAuditLog(nenAuditLog.AuditLogIdentifiers, nenAuditLog.LogMsg, nenAuditLog.UserId, nenAuditLog.OrganisationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - AuditLog/SaveNotifAuditLog, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
