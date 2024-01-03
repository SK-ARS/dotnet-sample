using STP.Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STP.Domain.Structures;
using STP.Domain.Structures.StructureJSON;
using STP.Structures.Providers;
using STP.Common.Constants;

namespace STP.Structures.Controllers
{
    public class StructureAssessmentController : ApiController
    {
        [HttpGet]
        [Route("StructureAssessment/GetStructureAssessmentCount")]
        public IHttpActionResult GetStructureAssessmentCount(string ESRN, long routePartId)
        {
            try
            {
                EsdalStructureJson esdalStructureJson = StructureAssessmentProvider.Instance.GetStructureAssessmentCount(ESRN, routePartId);
                return Content(HttpStatusCode.OK, esdalStructureJson);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureAssessment/GetStructureAssessmentCount Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
       
        [HttpPost]
        [Route("StructureAssessment/PerformAssessment")]
        public IHttpActionResult PerformAssessment(PerformStructureAssessmentParams performStructureAssessmentParams)
        {
            try
            {
                int result = StructureAssessmentProvider.Instance.PerformAssessment(performStructureAssessmentParams.StructureList,performStructureAssessmentParams.NotificationId,performStructureAssessmentParams.MovementReferenceNumber,performStructureAssessmentParams.AnalysisId,performStructureAssessmentParams.RouteId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"StructureAssessment/PerformAssessment Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
    }
}
