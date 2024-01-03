using STP.Routes.Provider;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GpxLibrary;
using static GpxLibrary.ConvertGpx;
using System.Text;
using System.Configuration;
using STP.Common.Logger;
using System;
using STP.Common.Constants;
using STP.ServiceAccess.SecurityAndUsers;
using STP.Domain.Routes;
using STP.Domain.ExternalAPI;
using System.Xml;

namespace STP.Routes.Controllers
{
    public class RouteExportController : ApiController
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        private readonly IAuthenticationService authenticationService;
        public RouteExportController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        public RouteExportController()
        {
            
        }

        #region GetRouteList
        [HttpGet]
        [Route("RouteExport/ListRoutes")]
        public IHttpActionResult GetRouteList(string AuthenticationKey = null, int PageNumber = 0, int PageSize = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);
                
                if (authentication.OrganisationId != 0)
                {
                    PageNumber = PageNumber > 0 ? PageNumber : 1;
                    PageSize = PageSize > 0 ? PageSize : 20;
                    RouteExportList routeList = RouteExport.Instance.GetRouteList(authentication.OrganisationId, PageNumber, PageSize);
                    if (routeList.Routes.Count == 0)
                        return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                    else
                        return Content(HttpStatusCode.OK, routeList);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteExport/GetRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Route Details
        [HttpGet]
        [Route("RouteExport/GetRouteDetails")]
        public HttpResponseMessage GetRouteDetails(long routeId, string AuthenticationKey = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(AuthenticationKey);

                if (authentication.OrganisationId != 0)
                {
                    string gpxOutput = GetRouteGPX(routeId, authentication.OrganisationId, false, UserSchema.Portal);

                    if (!string.IsNullOrWhiteSpace(gpxOutput))
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(gpxOutput, Encoding.UTF8, "application/xml")
                        };
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteExport/GetRouteDetails, Exception: " + ex​​​​);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region ExportRouteList
        [HttpPost]
        [Route("RouteExport/ExportRouteList")]
        public IHttpActionResult ExportRouteList(GetRouteExportList routeExportList)
        {
            try
            {
                List<Domain.ExternalAPI.Route> nERouteList = new List<Domain.ExternalAPI.Route>();
                List<ExportRouteList> routeList = RouteExport.Instance.ExportRouteList(routeExportList);
                int pos = 0;
                foreach (var item in routeList)
                {
                    Domain.ExternalAPI.Route route = new Domain.ExternalAPI.Route
                    {
                        RouteName = string.IsNullOrWhiteSpace(item.RouteName) ? null : item.RouteName,
                        RouteDescription = GetRouteDescription(routeExportList.RouteDescription, item.RouteDescription, pos),
                        GPX = Domain.Custom.StringExtraction.Base64Encode(GetRouteGPX(item.RouteId, 0, true, routeExportList.UserSchema))
                    };
                    pos++;
                    nERouteList.Add(route);
                }
                return Content(HttpStatusCode.OK, nERouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - RouteExport/ExportRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }

        private string GetRouteDescription(byte[] routeDescr, string routeDescription, int position)
        {
            string routeDesc = string.Empty;
            if(routeDescr != null)
            {
                byte[] outBoundDecryptString = Common.General.XsltTransformer.Trafo(routeDescr);
                string xmlString = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNodeList node = doc.GetElementsByTagName("RoutePartDescription");
                XmlNode data = node[position];
                using (var stringWriter = new System.IO.StringWriter())
                using (var xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    data.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    routeDesc = stringWriter.GetStringBuilder().ToString();
                }
            }
            else
            {
                routeDesc = !string.IsNullOrWhiteSpace(routeDescription) ? routeDescription : null;
            }
            return routeDesc;
        }
        #endregion

        #region GetRouteGPX
        private string GetRouteGPX(long routeId, int organisationId, bool isApp, string userSchema)
        {
            string xmlDocument = string.Empty;
            CheckRouteExportable checkRouteExportable = RouteExport.Instance.CheckIsRouteExportable(routeId, isApp, userSchema);

            if (checkRouteExportable.RouteCount == 0 || checkRouteExportable.IsBroken > 0 || checkRouteExportable.IsMultiPath > 1 || 
                checkRouteExportable.IsMultiSegment > 1)
                xmlDocument = RouteExportError.ExportError;
            else
            {
                List<TrackPoints> trackPoints = RouteExport.Instance.GetTrackPoints(routeId, organisationId, isApp, userSchema);
                if (trackPoints.Count > 0)
                {
                    TrackPoints[] cTrackpoints = trackPoints.ToArray();
                    List<Tracks> tracks = new List<Tracks>();
                    Tracks track = new Tracks
                    {
                        Name = cTrackpoints[0].TrackName,
                        TrackPoints = cTrackpoints
                    };
                    tracks.Add(track);
                    Tracks[] cTracks = tracks.ToArray();
                    List<WayPoints> wayPoints = RouteExport.Instance.GetWayPoints(routeId, organisationId, isApp, userSchema);
                    WayPoints[] cWaypoints = wayPoints.ToArray();
                    ConvertGpx convertGPX = new ConvertGpx();
                    xmlDocument = convertGPX.GPSConvertToGPX(cWaypoints, cTracks);
                    xmlDocument = xmlDocument.Replace(Environment.NewLine, string.Empty);
                }
            }
            return xmlDocument;
        }
        #endregion
    }
}