using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using STP.MovementsAndNotifications.Providers;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.SecurityAndUsers;
using STP.ServiceAccess.VehiclesAndFleets;
using System;
using System.Globalization;
using System.Net;
using System.Web.Http;
using STP.ServiceAccess.RouteAssessment;

namespace STP.MovementsAndNotifications.Controllers
{
    public class ExportNotificationController : ApiController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IVehicleConfigService vehicleConfigService;
        private readonly IRoutesService routesService;
        private readonly IRouteAssessmentService routeAssessmentService;
        public ExportNotificationController()
        {
        }
        public ExportNotificationController(IAuthenticationService authentication, IVehicleConfigService vehicleService, IRoutesService routeService, IRouteAssessmentService routeAssessService)
        {
            vehicleConfigService = vehicleService;
            routesService = routeService;
            authenticationService = authentication;
            routeAssessmentService = routeAssessService;
        }

        #region Get Notification Data (External API)

        [HttpGet]
        [Route("ExportNotification/GetNotificationData")]
        public IHttpActionResult GetNotificationData(string ESDALReferenceNumber = null, string AuthenticationKey = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ESDALReferenceNumber))
                    return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(AuthenticationKey);

                if (orgDetails.OrganisationId == 0)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthorizedOrganisation authorizedUsers = authenticationService.GetAuthorizedUsers(ESDALReferenceNumber, false);
                if(authorizedUsers.ValidCount == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotValidEsdalReference);

                var isReceiver = authorizedUsers.Receivers.Count > 0 && authorizedUsers.Receivers.Contains(orgDetails.OrganisationId);
                var isSender = authorizedUsers.SenderId == orgDetails.OrganisationId;

                if (!isReceiver && !isSender)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                ExportNotification notificationData = new ExportNotification();
                ExportNotifGeneralDetails notifGeneralDetails = NotificationProvider.Instance.ExportNotifGeneralDetails(ESDALReferenceNumber);

                if (string.IsNullOrWhiteSpace(notifGeneralDetails.ContentReferenceNo))
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);

                notificationData.GeneralDetails = ConvertNotifGeneraDetails(notifGeneralDetails);
                GetVehicleExportList vehicleExportList = new GetVehicleExportList
                {
                    ContentRefNo = notifGeneralDetails.ContentReferenceNo,
                    UserSchema = UserSchema.Portal,
                    OrganisationId = isReceiver ? orgDetails.OrganisationId : 0,
                    NotificationType = notifGeneralDetails.NotificationType
                };
                notificationData.Vehicles = vehicleConfigService.ExportVehicleDetails(vehicleExportList);
                Domain.RouteAssessment.RouteAssessmentModel routeAssessmentModel = null;
                if (notifGeneralDetails.AnalysisId > 0)
                    routeAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(notifGeneralDetails.AnalysisId, 2, UserSchema.Portal);

                GetRouteExportList routeExportList = new GetRouteExportList
                {
                    AnalysisId = notifGeneralDetails.AnalysisId,
                    ContentRefNo = notifGeneralDetails.ContentReferenceNo,
                    OrganisationId = isReceiver ? orgDetails.OrganisationId : 0,
                    NotificationType = notifGeneralDetails.NotificationType,
                    RouteDescription = routeAssessmentModel?.RouteDescription,
                    UserSchema = UserSchema.Portal
                };
                notificationData.Routes = routesService.ExportRouteDetails(routeExportList);
                return Content(HttpStatusCode.OK, notificationData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - ExportNotification/GetNotificationData,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Convert ExportNotifGeneralDetails to NotifGeneralDetail
        private NotifGeneralDetail ConvertNotifGeneraDetails(ExportNotifGeneralDetails notifGeneralDetails)
        {
            NotifGeneralDetail generalDetail = new NotifGeneralDetail
            {
                Classification = notifGeneralDetails.Classification,
                Client = notifGeneralDetails.Client,
                HaulierReference = notifGeneralDetails.HauliersReference,
                Notes = notifGeneralDetails.Notes,
                NotesOnEscort = notifGeneralDetails.NotesOnEscort,
                ApplicationReference = notifGeneralDetails.ApplicationReference,
                IsVR1 = notifGeneralDetails.RequireVR1,
                Indemnity = notifGeneralDetails.Indemnity,
                LoadDetails = new LoadDetail
                {
                    Description = notifGeneralDetails.LoadDescription,
                    MaxPiecesPerMove = notifGeneralDetails.MaxPiecesPerMove,
                    TotalMoves = notifGeneralDetails.TotalMoves
                },
                MovementDetails = new MovementDetail
                {
                    FromSummary = notifGeneralDetails.FromSummary,
                    ToSummary = notifGeneralDetails.ToSummary,
                    StartDate = notifGeneralDetails.MovementStart.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = notifGeneralDetails.MovementEnd.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StartTime = notifGeneralDetails.MovementStart.ToString("HH:mm", CultureInfo.InvariantCulture),
                    EndTime = notifGeneralDetails.MovementEnd.ToString("HH:mm", CultureInfo.InvariantCulture)
                },
                HaulierDetails = new HaulierDetail
                {
                    HaulierContact = notifGeneralDetails.HaulierContact,
                    HaulierOrgName = notifGeneralDetails.HaulierOrgName,
                    Licence = notifGeneralDetails.HaulierLicence,
                    Email = notifGeneralDetails.HaulierEmail,
                    FaxNumber = notifGeneralDetails.HaulierFaxNumber,
                    TelephoneNumber = notifGeneralDetails.HaulierTelephoneNumber,
                    HaulierAddress = new HaulierAddress()
                    {
                        AddressLine1 = notifGeneralDetails.HaulierAddressLine1,
                        AddressLine2 = notifGeneralDetails.HaulierAddressLine2,
                        AddressLine3 = notifGeneralDetails.HaulierAddressLine3,
                        AddressLine4 = notifGeneralDetails.HaulierAddressLine4,
                        AddressLine5 = notifGeneralDetails.HaulierAddressLine5,
                        PostCode = notifGeneralDetails.HaulierPostCode,
                        Country = notifGeneralDetails.HaulierCountry
                    }
                }
            };

            return generalDetail;
        }
        #endregion

    }
}
