using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.MovementsAndNotifications.Providers;
using STP.Domain;
using STP.Common.Logger;
using STP.Common.Constants;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.MovementsAndNotifications.Controllers
{
    public class GenericController : ApiController
    {
        [HttpGet]
        [Route("Generic/GetQuickLinksList")]
        public IHttpActionResult GetQuickLinksList(int userId)
        {
            try
            {
                List<QuickLinks> objQuickLinks = QuickLinksProvider.Instance.GetQuickLinksList(userId);
                return Ok(objQuickLinks);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Generic/GetQuickLinksList, Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
