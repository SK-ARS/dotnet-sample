using STP.Applications.Providers;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.SecurityAndUsers;
using STP.ServiceAccess.VehiclesAndFleets;
using System;
using System.Globalization;
using System.Net;
using System.Web.Http;

namespace STP.Applications.Controllers
{
    public class ExportApplicationController : ApiController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IVehicleConfigService vehicleConfigService;
        private readonly IRoutesService routesService;
        private readonly IRouteAssessmentService routeAssessmentService;
        public ExportApplicationController(IVehicleConfigService vehicleService, IRoutesService routeService, IAuthenticationService authService, IRouteAssessmentService routeAssessService)
        {
            vehicleConfigService = vehicleService;
            routesService = routeService;
            authenticationService = authService;
            routeAssessmentService = routeAssessService;
        }

        #region Get Application Data
        [HttpGet]
        [Route("ExportApplication/GetApplicationData")]
        public IHttpActionResult GetApplicationData(string ESDALReferenceNumber = null, string AuthenticationKey = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ESDALReferenceNumber))
                    return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                if (string.IsNullOrWhiteSpace(AuthenticationKey))
                    return Content(HttpStatusCode.Unauthorized, StatusMessage.Unauthorized);

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(AuthenticationKey);
                if (orgDetails.OrganisationId == 0)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthorizedOrganisation authorizedUsers = authenticationService.GetAuthorizedUsers(ESDALReferenceNumber, true);
                if (authorizedUsers.ValidCount == 0 && authorizedUsers.SortValid == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotValidEsdalReference);

                var isReceiver = authorizedUsers.Receivers.Count > 0 && authorizedUsers.Receivers.Contains(orgDetails.OrganisationId);
                var isSender = authorizedUsers.SenderId == orgDetails.OrganisationId;
                var isSort = authorizedUsers.SortOrgIds.Contains(orgDetails.OrganisationId);

                if(!isReceiver && !isSender&& !isSort)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                if ((isReceiver || isSender) && authorizedUsers.ValidCount == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotValidEsdalReference);

                string userSchema = UserSchema.Portal;
                if (isSort && authorizedUsers.SortValid > 0)
                    userSchema = UserSchema.Sort;

                ExportApplication applicationData = new ExportApplication();
                var appGeneralDetails = ApplicationProvider.Instance.ExportApplicationData(ESDALReferenceNumber, userSchema);

                if (appGeneralDetails.ApplicationRevisionId == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotFound);

                applicationData.GeneralDetails = GenerateGeneralDetail(appGeneralDetails);
                GetVehicleExportList vehicleExportList = new GetVehicleExportList
                {
                    RevisionId = appGeneralDetails.ApplicationRevisionId,
                    VersionId = appGeneralDetails.VersionId,
                    UserSchema = userSchema
                };
                applicationData.Vehicles = vehicleConfigService.ExportVehicleDetails(vehicleExportList);
                Domain.RouteAssessment.RouteAssessmentModel routeAssessmentModel = null;
                if (appGeneralDetails.AnalysisId > 0)
                    routeAssessmentModel = routeAssessmentService.GetDriveInstructionsinfo(appGeneralDetails.AnalysisId, 2, UserSchema.Portal);

                GetRouteExportList routeExportList = new GetRouteExportList
                {
                    AnalysisId = appGeneralDetails.AnalysisId,
                    RevisionId = appGeneralDetails.ApplicationRevisionId,
                    VersionId = appGeneralDetails.VersionId,
                    OrganisationId = isReceiver ? orgDetails.OrganisationId : 0,
                    RouteDescription = routeAssessmentModel?.RouteDescription,
                    UserSchema = userSchema
                };
                applicationData.Routes = routesService.ExportRouteDetails(routeExportList);
                return Content(HttpStatusCode.OK, applicationData);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - ExportApplication/GetApplicationData,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Convert App General Details
        /// <summary>
        /// Convert NEAppGeneralDetails model to GeneralDetail
        /// </summary>
        /// <param name="nEAppGeneralDetails"></param>
        /// <returns></returns>
        private AppGeneralDetails GenerateGeneralDetail(ExportAppGeneralDetails nEAppGeneralDetails)
        {
            var generalDetails = new AppGeneralDetails
            {
                Classification = nEAppGeneralDetails.Classification,
                HaulierDetails = new HaulierDetail
                {
                    HaulierContact = nEAppGeneralDetails.HaulierContact,
                    HaulierOrgName = nEAppGeneralDetails.HaulierOrgName,
                    TelephoneNumber = nEAppGeneralDetails.HaulierTelephoneNumber,
                    FaxNumber = nEAppGeneralDetails.HaulierFaxNumber,
                    Email = nEAppGeneralDetails.HaulierEmail,
                    Licence = nEAppGeneralDetails.HaulierLicence,
                    HaulierAddress = new HaulierAddress()
                    {
                        AddressLine1 = nEAppGeneralDetails.HaulierAddressLine1,
                        AddressLine2 = nEAppGeneralDetails.HaulierAddressLine2,
                        AddressLine3 = nEAppGeneralDetails.HaulierAddressLine3,
                        AddressLine4 = nEAppGeneralDetails.HaulierAddressLine4,
                        AddressLine5 = nEAppGeneralDetails.HaulierAddressLine5,
                        PostCode = nEAppGeneralDetails.HaulierPostCode,
                        Country = nEAppGeneralDetails.HaulierCountry
                    }
                },
                LoadDetails = new LoadDetail()
                {
                    TotalMoves = nEAppGeneralDetails.TotalMoves,
                    MaxPiecesPerMove = nEAppGeneralDetails.MaxPiecesPerMove,
                    Description = nEAppGeneralDetails.LoadDescription
                },
                MovementDetails = new MovementDetail()
                {
                    StartDate = nEAppGeneralDetails.MovementStart.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = nEAppGeneralDetails.MovementEnd.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StartTime = nEAppGeneralDetails.MovementStart.ToString("HH:mm", CultureInfo.InvariantCulture),
                    EndTime = nEAppGeneralDetails.MovementEnd.ToString("HH:mm", CultureInfo.InvariantCulture),
                    FromSummary = nEAppGeneralDetails.FromSummary,
                    ToSummary = nEAppGeneralDetails.ToSummary,
                },
                SupplimentaryDetails = new SupplimentaryDetails
                {
                    AdditionalConsideration = nEAppGeneralDetails.SupplimentaryInfo.AdditionalConsideration,
                    AdditionalCost = nEAppGeneralDetails.SupplimentaryInfo.AdditionalCost,
                    DateOfAuthority = nEAppGeneralDetails.SupplimentaryInfo.DateOfAuthority,
                    DistanceOfRoad = Convert.ToDecimal(nEAppGeneralDetails.SupplimentaryInfo.TotalDistanceOfRoad),
                    ValueOfLoad = nEAppGeneralDetails.SupplimentaryInfo.ApprValueOfLoad,
                    RiskNature = nEAppGeneralDetails.SupplimentaryInfo.RiskNature,
                    PortNames = nEAppGeneralDetails.SupplimentaryInfo.PortNames,
                    SeaQuotation = nEAppGeneralDetails.SupplimentaryInfo.SeaQuotation,
                    ProposedMovementDetails = nEAppGeneralDetails.SupplimentaryInfo.ProposedMoveDetails,
                    CostOfMovement = nEAppGeneralDetails.SupplimentaryInfo.ApprCostOfMovement
                },
                NotesOnEscort = nEAppGeneralDetails.NotesOnEscort,
                Notes = nEAppGeneralDetails.ApplicationNotes,
                HaulierReference = nEAppGeneralDetails.HauliersReference,
                ApplicationDesc = nEAppGeneralDetails.ApplicationDesc,
                AgentName = nEAppGeneralDetails.AgentName,
                Client = nEAppGeneralDetails.Client
            };
            return generalDetails;
        }
        #endregion
    }
}
