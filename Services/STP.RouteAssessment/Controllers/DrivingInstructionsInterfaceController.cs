using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.DrivingInstructionsInterface;
using STP.RouteAssessment.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.RouteAssessment.Controllers
{
    public class DrivingInstructionsInterfaceController : ApiController
    {
        [HttpPost]
        [Route("DrivingInstructor/GenerateDI")]
        public IHttpActionResult GenerateDrivingInstnRouteDesc(DrivInstParams drivingInsParams)
        {
            try
            {
                long errorcode;
                DisConnect drivinInstConnect = new DisConnect();
                errorcode = drivinInstConnect.GenerateDrivingInstnRouteDesc(drivingInsParams.DrivingInstructionReq,
                    drivingInsParams.UserSchema == UserSchema.Portal ? DBSchema.portal : DBSchema.sort);
                return Content(HttpStatusCode.OK, errorcode);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - DrivingInstructor/GenerateDI, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
