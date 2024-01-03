using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using STP.Domain.RouteAssessment.AssessmentInput;
using STP.Domain.RouteAssessment.AssessmentOutput;
using Definitions = STP.Domain.RouteAssessment.AssessmentInput.Definitions;
using EsdalStructure = STP.Domain.RouteAssessment.AssessmentInput.EsdalStructure;
using Properties = STP.Domain.RouteAssessment.AssessmentInput.Properties;
using System.Web.Http;
using STP.Alsat.Providers;
using STP.Domain.Structures;
using STP.ServiceAccess.RouteAssessment;
using System.Net;
using System.Net.Http;
using System.Configuration;

namespace STP.Alsat.Controllers
{
    public class AlsatAssessmentController : ApiController
    {

        private readonly IRouteAssessmentService routeAssessmentService;
        public AlsatAssessmentController()
        {
        }

        public AlsatAssessmentController(IRouteAssessmentService routeAssessmentService)
        {
            this.routeAssessmentService = routeAssessmentService;
        }

        [HttpGet]
        [Route("Alsat/Assessment")]
        [Route("Alsat/Assessment/{sequence_num}")]
        [Route("api/assessment/v1")]
        [Route("api/assessment/v1/{sequence_num}")]
        public IHttpActionResult Assessment(int sequence_num)
        {
            AssessmentResponse assessmentResponse = AlsatAssessmentProvider.Instance.GetAssessment(sequence_num);
            if (assessmentResponse.AssessmentInput != null && assessmentResponse.ExceptionCode == "200")
            {
                return Ok(assessmentResponse.AssessmentInput);
            }
            else if (assessmentResponse.AssessmentInput == null && assessmentResponse.ExceptionCode == "200")
            {
                return Content(HttpStatusCode.OK, StatusMessage.NotFound);
            }
            else if (assessmentResponse.ExceptionCode == "404")
            {
                return NotFound();
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("Alsat/AssessmentResult")]
        [Route("api/assessment_result/v1")]
        public IHttpActionResult PutAssessment(AssessmentOutput assessmentOutput)
        {
            try
            {
                if (assessmentOutput == null)
                {
                    return BadRequest();
                }
                int status;
                StructuresAssessment structuresAssessment = AlsatAssessmentProvider.Instance.PutAssessmentResult(assessmentOutput);
                if (structuresAssessment != null)
                {
                    if (structuresAssessment.SequenceNumber == 0)
                    {
                        return Content(HttpStatusCode.OK, StatusMessage.NotFound);
                    }
                    else if (!structuresAssessment.MovementReference.Equals(assessmentOutput.Properties.MovementId))
                    {
                        status = -1;
                    }
                    else
                    {
                        foreach (var structure in assessmentOutput.Properties.EsdalStructure)
                        {
                            structure.RouteId = (long)structuresAssessment.RoutePartId;
                        }
                        status = routeAssessmentService.UpdateRouteAssessment("", 0, (int)structuresAssessment.AnalysisId, 3, structuresAssessment.PortalSchema == 1 ? UserSchema.Portal : UserSchema.Sort, (int)structuresAssessment.RoutePartId, assessmentOutput);
                    }

                    if (status == 1)
                    {
                        return Content(HttpStatusCode.Created, "Assessment result saved successfully.");
                    }
                    else if (status == -1)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
                    }

                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
                }


            }
            catch (Exception ex)
            {

                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"PutAssessment Exception: {0}", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }



        }
    }

}

