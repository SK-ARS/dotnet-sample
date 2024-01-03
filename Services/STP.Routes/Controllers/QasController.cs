using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.Routes.QAS;
using STP.Routes.QasService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace STP.Routes.Controllers
{
    public class QasController : ApiController
    {
        [HttpGet]
        [Route("QAS/Search")]
        public IHttpActionResult Search (string searchKeyword)
        {
            try
            {
                List<AddrDetails> addrList = new List<AddrDetails>();
                Esdal2Qas.Search(searchKeyword, ref addrList);
                if (addrList.Count > 0)
                    return Content(HttpStatusCode.OK, addrList);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - QAS/Search, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("QAS/GetAddress")]
        public IHttpActionResult GetAddress(string moniker)
        {
            try
            {
                AddrDetails addDetails = Esdal2Qas.GetAddress(moniker);
                if (addDetails != null)
                    return Content(HttpStatusCode.OK, addDetails);
                else
                    return Content(HttpStatusCode.NotFound, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - QAS/GetAddress, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
