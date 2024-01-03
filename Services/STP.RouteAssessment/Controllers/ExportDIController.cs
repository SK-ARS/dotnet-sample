using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using STP.Domain.RouteAssessment;
using STP.RouteAssessment.Providers;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace STP.RouteAssessment.Controllers
{
    public class ExportDIController : ApiController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IRoutesService routesService;
        public ExportDIController(IAuthenticationService authentication, IRoutesService routeService)
        {
            authenticationService = authentication;
            routesService = routeService;
        }

        [HttpGet]
        [Route("DrivingInstructions/GetRouteDetails")]
        public IHttpActionResult GetRouteDetails(string ESDALReferenceNumber = null, int MovementType = 0, string AuthenticationKey = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ESDALReferenceNumber))
                    return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                if (MovementType == 0)//1 - Notification 2 - Movement Version
                    return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(AuthenticationKey);

                if (orgDetails.OrganisationId == 0)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthorizedOrganisation authorizedUsers = authenticationService.GetAuthorizedUsers(ESDALReferenceNumber, MovementType == 2);
                if (authorizedUsers.ValidCount == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotValidEsdalReference);

                var isReceiver = authorizedUsers.Receivers.Count > 0 && authorizedUsers.Receivers.Contains(orgDetails.OrganisationId);
                var isSender = authorizedUsers.SenderId == orgDetails.OrganisationId;
                if (isSender)
                {
                    ExportDIList exportDIList = new ExportDIList();
                    DIList dIList = RouteAssessmentProvider.Instance.GetRouteDetailsForMovement(ESDALReferenceNumber, MovementType, 0, UserSchema.Portal);
                    
                    if (dIList == null)
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);

                    RouteAssessmentModel routeAssessmentModel = RouteAssessmentProvider.Instance.GetDriveInstructionsInfo(dIList.AnalysisId, 9, UserSchema.Portal);
                    GetRouteExportList routeExportList = new GetRouteExportList
                    {
                        AnalysisId = dIList.AnalysisId,
                        ContentRefNo = dIList.ContentRefNum,
                        VersionId = dIList.VersionId,
                        NotificationType = dIList.NotificationType,
                        UserSchema = UserSchema.Portal,
                        RouteDescription = dIList.NotificationType == 1 ? routeAssessmentModel.RouteDescription : null,
                        OrganisationId = isReceiver ? orgDetails.OrganisationId : 0
                    };
                    List<Route> routes = routesService.ExportRouteDetails(routeExportList);
                    exportDIList.EsdalReferenceNumber = ESDALReferenceNumber;
                    exportDIList.MovementStatus = dIList.MovementStatus;
                    foreach (var item in routes)
                    {
                        DIRoute dIRoute = new DIRoute
                        {
                            RouteDescription = item.RouteDescription,
                            GPX = item.GPX,
                            RouteName = item.RouteName,
                            DrivingInstructions = GetDrvingInstruction(item.RouteName, routeAssessmentModel.DriveInst)
                        };
                        exportDIList.Routes.Add(dIRoute);
                    }
                    if (exportDIList.Routes.Count > 0)
                        return Content(HttpStatusCode.OK, exportDIList);
                    else
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - DrivingInstructions/GetRouteDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }

        private string GetDrvingInstruction(string routeName, byte[] drivingInstructions)
        {
            byte[] outBoundDecryptString = Common.General.XsltTransformer.Trafo(drivingInstructions);
            string xmlString = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
            string newXmlString = "";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNodeList node = doc.GetElementsByTagName("DrivingInstructions");
            XmlNode data = node[0];
            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].ChildNodes[1].InnerText == routeName)
                {
                    data = node[i];
                }
            }
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = new XmlTextWriter(stringWriter))
            {
                data.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                newXmlString = stringWriter.GetStringBuilder().ToString();
            }
            newXmlString = newXmlString.Replace(Environment.NewLine, string.Empty);
            return newXmlString;
        }
    }
}
