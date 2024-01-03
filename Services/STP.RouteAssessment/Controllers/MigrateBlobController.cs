using STP.Common.Constants;
using STP.Common.General;
using STP.Common.Logger;
using STP.Domain.Custom;
using STP.Domain.RouteAssessment;
using STP.Domain.RouteAssessment.XmlAnalysedCautions;
using STP.Domain.Routes.TempModelsMigrations;
using STP.RouteAssessment.Providers;
using STP.RouteAssessment.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using static STP.Domain.Routes.RouteModel;

namespace STP.RouteAssessment.Controllers
{
    /// <summary>
    /// This controller will be used for compare blob data and update database with newly generated blob
    /// </summary>
    public class MigrateBlobController : ApiController
    {
        [HttpPost]
        [Route("MigrateBlob/MigrateAffectedCautions")]
        public IHttpActionResult MigrateAffectedCautions(InputBlobModel model)
        {
            try
            {
                int isCandidate = 0;
                int anal_type = 4;
                //Get Existing RouteAnalysis isprocessed is 0
                List<RouteAnalysisModel> routeAnalysisList = RouteAssessmentProvider.Instance.GetRouteAnalysisTemp(model.PageNo, model.PageSize, model.UserSchema);
                if (routeAnalysisList != null)
                {
                    foreach (var item in routeAnalysisList)
                    {
                        if (item.NotificationId > 0 || item.IsCandidate == 1)
                            item.VersionId = 0;//notification
                        else if (item.IsCandidate == 0)
                            item.RevisionId = 0;//applicable in SORT schema

                        List<RoutePartDetails> routeDetails = RouteAssessmentProvider.Instance.GetRouteDetailForAnalysis(item.VersionId,
                            item.Reference, item.RevisionId, isCandidate, model.UserSchema);

                        ProcessRoutePartDetails(model, anal_type, item, routeDetails);

                        //update processed status
                        RouteAssessmentProvider.Instance.UpdateProcessedRowInTempTable(item.AnalysisId, model.UserSchema);
                    }
                    return Content(HttpStatusCode.OK, "success");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "no data found");
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/MigrateAffectedCautions No data found ");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteAssessment/MigrateAffectedCautions Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        private static void ProcessRoutePartDetails(InputBlobModel model, int anal_type, RouteAnalysisModel item, List<RoutePartDetails> routeDetails)
        {
            if (routeDetails != null && routeDetails.Any())
            {
                RouteAssessmentModel objRouteAssessmentModel = RouteAssessmentProvider.Instance.GetDriveInstructionsinfo(item.AnalysisId,
                    anal_type, model.UserSchema);
                AnalysedCautions existingAnalysedCautions = null;
                if (objRouteAssessmentModel != null && objRouteAssessmentModel.Cautions != null)
                {
                    //existing cautions blob is available
                    if (objRouteAssessmentModel.Cautions != null)
                    {
                        string affectedCautionsxml = Encoding.UTF8.GetString(XsltTransformer.Trafo(objRouteAssessmentModel.Cautions));
                        existingAnalysedCautions = StringExtractor.XmlDeserializeCautions(affectedCautionsxml);
                    }
                }

                // generate cautions
                var generatedAffectedCautions = RouteAssessmentProvider.Instance.GenerateAffectedCautionsTemp(routeDetails, 0,
                     model.UserSchema);

                if ((generatedAffectedCautions != null && existingAnalysedCautions != null &&
                    generatedAffectedCautions.AnalysedCautionsPart.Count != existingAnalysedCautions.AnalysedCautionsPart.Count)
                    || (generatedAffectedCautions != null && existingAnalysedCautions == null))//compare both models and check diffrence
                {
                    UpdateGeneratedCautionsBlobInDatabase(model, anal_type, item, generatedAffectedCautions);
                }
            }
        }

        private static void UpdateGeneratedCautionsBlobInDatabase(InputBlobModel model, int anal_type, RouteAnalysisModel item, AnalysedCautions generatedAffectedCautions)
        {
            string xmlVal = StringExtractor.XmlCautionSerializer(generatedAffectedCautions);//create xml
            byte[] affectedCautions = StringExtraction.ZipAndBlob(xmlVal);//create blob
            //update generatedAffectedCautions blob to db
            var i = RouteAssessmentProvider.Instance.UpdateAnalysedRouteTemp(affectedCautions, item.AnalysisId, anal_type, model.UserSchema);
        }
    }
}
