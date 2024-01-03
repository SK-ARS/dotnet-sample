using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.RoutePlannerInterface;
using STP.RoutePlannerInterface.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.Routes.Controllers
{
    public class RoutePlannerInterfaceController : ApiController
    {
        [HttpPost]
        [Route("RoutePlanner/PostEx")]
        public IHttpActionResult PostEx(RouteViaWaypointEx routeViaPointEx)
        {
            try
            {
                RoutePlannerConnect rpConnect = new RoutePlannerConnect();
                RouteData data = rpConnect.GetRoute(routeViaPointEx);
                if (data.ListSegments.Count > 0)
                    return Content(HttpStatusCode.OK, data);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoutePlanner/PostEx, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
    }
}
