using STP.Common.Constants;
using STP.HelpdeskTools.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Common.Logger;
using STP.Domain.HelpdeskTools;

namespace STP.HelpdeskTools.Controllers
{
    public class DistributionStatusController : ApiController
    {
        #region Get Distribution Alerts
        [HttpPost]
        [Route("DistributionStatus/GetDistributionAlerts")]
        public IHttpActionResult GetDistributionAlerts(DistributionAlertsParams objDistributionParams)
        {
            List<DistributionAlerts> list = new List<DistributionAlerts>();
            try
            {
                list = HelpdeskProvider.Instance.GetSORTDistributionAlerts(objDistributionParams.PageNo, objDistributionParams.PageSize, objDistributionParams.ObjDistributionAlert, objDistributionParams.PortalType, objDistributionParams.PresetFilter, objDistributionParams.SortOrder);
                return Content(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - DistributionStatus/GetDistributionAlerts,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
