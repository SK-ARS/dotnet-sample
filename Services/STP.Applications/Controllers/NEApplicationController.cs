using STP.Applications.Providers;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.Custom;
using STP.Domain.ExternalAPI;
using STP.Domain.NonESDAL;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.SecurityAndUsers;
using STP.ServiceAccess.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using static STP.Common.Enums.ExternalApiEnums;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;
using static STP.Domain.NonESDAL.NERouteValidation;

namespace STP.Applications.Controllers
{
    public class NEApplicationController : ApiController
    {
        private readonly IVehicleConfigService vehicleConfigService;
        private readonly IRoutesService routesService;
        private readonly IAuthenticationService authenticationService;
        public NEApplicationController()
        {
        }
        public NEApplicationController(IVehicleConfigService vehicleService, IRoutesService routeService, IAuthenticationService authentication)
        {
            vehicleConfigService = vehicleService;
            routesService = routeService;
            authenticationService = authentication;
        }

        #region Insert Non ESDAL Application
        [HttpPost]
        [Route("NEApplication/InsertNEApplication")]
        public IHttpActionResult InsertNEApplication([FromBody] NEApplication neApplication)
        {
            ExternalApiResponse externalApiResponse = new ExternalApiResponse();
            dynamic validationObj = new ExpandoObject();
            bool validationError = false;
            try
            {
                #region Input Data Check
                if (neApplication == null || neApplication.GeneralDetails == null || neApplication.Vehicles == null || neApplication.Routes == null)
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.BadRequest
                    };
                    return Content(HttpStatusCode.BadRequest, externalApiResponse);
                }
                #endregion

                #region Authentication Validation
                if (string.IsNullOrWhiteSpace(neApplication.AuthenticationKey))
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.Unauthorized
                    };
                    return Content(HttpStatusCode.Unauthorized, externalApiResponse);
                }

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(neApplication.AuthenticationKey);

                if (orgDetails.OrganisationId == 0)
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.Unauthorized
                    };
                    return Content(HttpStatusCode.Unauthorized, externalApiResponse);
                }
                #endregion

                #region General Details Validations
                NEAppGeneralDetails appGeneralDetails;
                GeneralDetailsValidation validation = new GeneralDetailsValidation();
                appGeneralDetails = validation.ValidateAppGeneralDetail(neApplication.GeneralDetails);

                if (appGeneralDetails.GeneralDetailError != null)
                {
                    validationObj.GeneralDetailsError = appGeneralDetails.GeneralDetailError.ErrorList;
                    validationError = true;
                }
                #endregion

                #region Vehicle Validation

                #region Route namd and vehicle name mapping validationS
                List<string> vehicleRouteName = neApplication.Vehicles.Select(s => s.RouteName).ToList();
                List<string> routesRouteName = neApplication.Routes.Select(s => s.RouteName).ToList();

                int vehicleCount = neApplication.Vehicles.Count;
                int routesCount = neApplication.Routes.Count;
                bool duplicateRouteName = routesRouteName.GroupBy(x => x).Any(g => g.Count() > 1);
                #endregion

                #region Model Declaration for Vehicle Validation
                VehicleValidateInputModel vehicleValidateInputModel;
                VehicleValidationExternal vehicleValidation;
                VehicleImportModel vehicleImportModel;
                VehicleMovementType movementTypeClass = null;
                List<VehicleMovementType> movementTypeClassList = new List<VehicleMovementType>();
                VehicleImportOutput importOutput = new VehicleImportOutput();
                ConfigurationModel configurationModel;
                AssessMoveTypeParams moveTypeParams;
                int previousVehicleMC = 0;
                #endregion

                #region Loop for Multiple Vehicles
                int i = 1;
                StringBuilder vehicleValidationError = new StringBuilder();
                foreach (var vehicle in neApplication.Vehicles)
                {
                    vehicleValidation = new VehicleValidationExternal();

                    #region Perform Vehicle Assessment
                    configurationModel = vehicleValidation.CreateConfigurationModel(vehicle);
                    moveTypeParams = new AssessMoveTypeParams
                    {
                        ForceApplication = true,
                        configuration = configurationModel
                    };
                    movementTypeClass = vehicleConfigService.AssessMovementType(moveTypeParams);
                    movementTypeClassList.Add(movementTypeClass);

                    int vehicleCategory = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(movementTypeClass, false, true);
                    int movementClassification = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryReverseMapping(vehicleCategory);
                    #endregion

                    #region Perform Validation After Assessment
                    vehicleValidateInputModel = new VehicleValidateInputModel
                    {
                        Vehicle = vehicle,
                        IsFleet = false,
                        RouteNames = routesRouteName,
                        VehiclesCount = vehicleCount,
                        RoutesCount = routesCount,
                        PrevVehicleMovementClass = previousVehicleMC,
                        AssessedVehicleMovementClass = vehicleCategory,
                        AssessedMovementClass = movementClassification,
                        IsNen = false,
                        IsNea = true,
                        VehiclePos = i,
                        PreviousMovementType = i > 1 ? movementTypeClassList[0] : null,
                        CurrentMovementType = movementTypeClass
                    };
                    vehicleImportModel = vehicleValidation.ValidateVehicleData(vehicleValidateInputModel, out bool isVr1);
                    previousVehicleMC = vehicleImportModel.VehicleConfigDetails != null ? vehicleImportModel.VehicleConfigDetails.MovementClassification : 0; 
                    if (vehicleImportModel.VehicleError != null)
                        vehicleValidationError.Append(vehicleImportModel.VehicleError.ErrorMessage + "~");
                    else
                    {
                        vehicleImportModel.VehicleConfigDetails.MovementClassification = vehicleCategory;
                        importOutput = vehicleConfigService.ImportVehicleExternal(vehicleImportModel, importOutput.MovementId);
                    }
                    #endregion

                    i++;
                }
                string vehicleErrorMessage = vehicleValidationError.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(vehicleErrorMessage))
                {
                    List<string> vehicleDetailsErrorList = vehicleErrorMessage.Split('~').ToList();
                    vehicleDetailsErrorList.RemoveAt(vehicleDetailsErrorList.Count - 1);
                    validationObj.VehicleDetailsError = vehicleDetailsErrorList;
                    validationError = true;
                }
                #endregion

                #endregion

                #region General Class Validation
                movementTypeClass = movementTypeClassList[0];
                int movGeneralclass = validation.GeneralClassReverseMapping(movementTypeClass.VehicleClass);
                if (movGeneralclass != appGeneralDetails.Classification)
                {
                    ExternalApiGeneralClassificationType inputGCType = (ExternalApiGeneralClassificationType)appGeneralDetails.Classification;
                    ExternalApiGeneralClassificationType outputGCType = (ExternalApiGeneralClassificationType)movementTypeClass.VehicleClass;

                    string inputGeneralClass = inputGCType != 0 ? inputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"]; 
                    string outputGeneralClass = outputGCType != 0 ? outputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"]; 
                    string[] validationErrorList = validation.ValidationFailure["GeneralClassError"].Split(';');
                    validationObj.ClassificationError = validationErrorList[0] + outputGeneralClass + validationErrorList[1] + inputGeneralClass + validationErrorList[2];
                    validationError = true;
                }
                #endregion

                #region Route Validation
                NERouteImport routeImport;
                NERouteImportModel routeImportModel;
                List<NERouteImport> routeImportList = new List<NERouteImport>();
                NERouteValidation nERouteValidation = new NERouteValidation
                {
                    ValidationFailure = validation.ValidationFailure
                };
                i = 1;
                StringBuilder routeValidationError = new StringBuilder();
                foreach (var route in neApplication.Routes)
                {
                    routeImportModel = new NERouteImportModel
                    {
                        Route = new Route { GPX = StringExtraction.Base64Decode(route.GPX), RouteDescription = route.RouteDescription, RouteName = route.RouteName },
                        RouteNames = vehicleRouteName,
                        IsDuplicate = duplicateRouteName
                    };
                    routeImport = nERouteValidation.ValidateNeRoute(routeImportModel, i);
                    if (routeImport.RouteError != null)
                        routeValidationError.Append(routeImport.RouteError.ErrorMessage + "~");
                    else
                        routeImportList.Add(routeImport);
                    i++;
                }
                string routeErrorMessage = routeValidationError.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(routeErrorMessage))
                {
                    List<string> routeDetailsErrorList = routeErrorMessage.Split('~').ToList();
                    routeDetailsErrorList.RemoveAt(routeDetailsErrorList.Count - 1);
                    validationObj.RouteDetailsError = routeDetailsErrorList;
                    validationError = true;
                }
                #endregion

                #region Validation Errors
                if (validationError)
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.ValidationFailure,
                        Data = validationObj
                    };
                    return Content(HttpStatusCode.OK, externalApiResponse);
                }
                #endregion

                #region Application Creation
                appGeneralDetails.NonEsdalKeyId = orgDetails.OrganisationId;
                long appRevId = NEApplicationProvider.Instance.SaveNEApplication(appGeneralDetails);
                #endregion

                #region Application Process
                if (appRevId > 0)
                {
                    #region Route Import API
                    GPXInputModel GPXInputModel;
                    SaveNERouteParams saveNERoute;
                    List<long> routeIds = new List<long>();
                    foreach (var route in routeImportList.Select(item => item.Route))
                    {
                        GPXInputModel = new GPXInputModel
                        {
                            RouteGPX = route.GPX,
                        };
                        saveNERoute = new SaveNERouteParams()
                        {
                            GPXInput = GPXInputModel,
                            RevisionId = appRevId,
                            IsVr1 = appGeneralDetails.IsVR1,
                            RouteName = route.RouteName,
                            RouteDescription = route.RouteDescription
                        };
                        long routeId = routesService.SaveNERoute(saveNERoute);
                        if (routeId > 0)
                            routeIds.Add(routeId);
                        else
                        {
                            externalApiResponse = new ExternalApiResponse
                            {
                                ErrorMessage = validation.ValidationFailure["RouteError"]
                            };
                            return Content(HttpStatusCode.InternalServerError, externalApiResponse);
                        }
                    }

                    #endregion

                    #region Vehicle Import API Call
                    NEVehicleImport neaVehicle = new NEVehicleImport
                    {
                        MovementId = importOutput.MovementId,
                        IsVr1 = appGeneralDetails.IsVR1,
                        RevisionId = appRevId,
                        NotificationId = 0,
                        IsNotif = false
                    };
                    List<long> vehicleIds = vehicleConfigService.SaveNonEsdalVehicle(neaVehicle);
                    if (vehicleIds.Count == 0)
                    {
                        externalApiResponse = new ExternalApiResponse
                        {
                            ErrorMessage = validation.ValidationFailure["VehicleError"]
                        };
                        return Content(HttpStatusCode.InternalServerError, externalApiResponse);
                    }
                    else
                        vehicleConfigService.DeleteTempMovementVehicle(importOutput.MovementId, UserSchema.Portal);
                    #endregion

                    NEApplicationOuput nEApplicationOuput = new NEApplicationOuput
                    {
                        ESDALReferenceNumber = NEApplicationProvider.Instance.SubmitNEApplication(appRevId, appGeneralDetails.IsVR1)
                    };

                    #region Store Input Json File
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(neApplication);
                    JsonStore.StoreInputJsonFile(json, appRevId, nEApplicationOuput.ESDALReferenceNumber);
                    #endregion

                    externalApiResponse = new ExternalApiResponse
                    {
                        Success = true,
                        Data = nEApplicationOuput
                    };

                    return Content(HttpStatusCode.OK, nEApplicationOuput);
                }
                else
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = validation.ValidationFailure["ApplicationError"]
                    };
                    return Content(HttpStatusCode.InternalServerError, externalApiResponse);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Application/InsertNEApplication, Exception: ", ex);
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.InternalServerError
                };
                return Content(HttpStatusCode.InternalServerError, externalApiResponse);
            }
        }
        #endregion

        #region CheckApplicationStatus
        /// <summary>
        /// An external haulier system can check the submitted application status by using ESDAL Reference Number
        /// </summary>
        /// <param name="ESDALReferenceNumber"></param>
        /// <param name="AuthenticationKey"></param>
        /// <param name="HaulierOrgName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("NEApplication/CheckApplicationStatus")]
        public IHttpActionResult CheckApplicationStatus(string ESDALReferenceNumber = null, string AuthenticationKey = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ESDALReferenceNumber))
                    return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(AuthenticationKey);
                if (orgDetails.OrganisationId == 0)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthorizedOrganisation authorizedUsers = authenticationService.GetAuthorizedUsers(ESDALReferenceNumber, true);
                if (authorizedUsers.ValidCount == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotValidEsdalReference);

                var isReceiver = authorizedUsers.Receivers.Count > 0 && authorizedUsers.Receivers.Contains(orgDetails.OrganisationId);
                var isSender = authorizedUsers.SenderId == orgDetails.OrganisationId;
                var isSort = authorizedUsers.SortOrgIds.Contains(orgDetails.OrganisationId);

                if (isReceiver || isSender || isSort)
                {
                    NEApplicationStatus outputModel = new NEApplicationStatus
                    {
                        ApplicationStatus = NEApplicationProvider.Instance.GetNEApplicationStatus(ESDALReferenceNumber)
                    };
                    return Content(HttpStatusCode.OK, outputModel);
                }
                else
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NEApplication/CheckApplicationStatus, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region TestNEApplication
        [HttpPost]
        [Route("NEApplication/TestNEApplication")]
        public IHttpActionResult TestNEApplication([FromBody] NEApplication neApplication)
        {
            ExternalApiResponse externalApiResponse = new ExternalApiResponse();
            dynamic validationObj = new ExpandoObject();
            bool validationError = false;

            #region Input Data Check
            if (neApplication == null || neApplication.GeneralDetails == null || neApplication.Vehicles == null || neApplication.Routes == null)
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.BadRequest
                };
                return Content(HttpStatusCode.BadRequest, externalApiResponse);
            }
            #endregion

            #region Authentication Validation
            if (string.IsNullOrWhiteSpace(neApplication.AuthenticationKey))
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.Unauthorized
                };
                return Content(HttpStatusCode.Unauthorized, externalApiResponse);
            }
            #endregion

            #region General Details Validations
            NEAppGeneralDetails appGeneralDetails;
            GeneralDetailsValidation validation = new GeneralDetailsValidation();
            appGeneralDetails = validation.ValidateAppGeneralDetail(neApplication.GeneralDetails);

            if (appGeneralDetails.GeneralDetailError != null)
            {
                validationObj.GeneralDetailsError = appGeneralDetails.GeneralDetailError.ErrorList;
                validationError = true;
            }
            #endregion

            #region Vehicle Validation

            #region Route namd and vehicle name mapping validationS
            List<string> vehicleRouteName = neApplication.Vehicles.Select(s => s.RouteName).ToList();
            List<string> routesRouteName = neApplication.Routes.Select(s => s.RouteName).ToList();

            int vehicleCount = neApplication.Vehicles.Count;
            int routesCount = neApplication.Routes.Count;
            bool duplicateRouteName = routesRouteName.GroupBy(x => x).Any(g => g.Count() > 1);
            #endregion

            #region Model Declaration for Vehicle Validation
            VehicleValidateInputModel vehicleValidateInputModel;
            VehicleValidationExternal vehicleValidation;
            VehicleImportModel vehicleImportModel;
            VehicleMovementType movementTypeClass = null;
            List<VehicleMovementType> movementTypeClassList = new List<VehicleMovementType>();
            List<VehicleImportModel> vehicleImportModelList = new List<VehicleImportModel>();
            ConfigurationModel configurationModel;
            AssessMoveTypeParams moveTypeParams;
            int previousVehicleMC = 0;
            #endregion

            #region Loop for Multiple Vehicles
            int i = 1;
            StringBuilder vehicleValidationError = new StringBuilder();
            foreach (var vehicle in neApplication.Vehicles)
            {
                vehicleValidation = new VehicleValidationExternal();

                #region Perform Vehicle Assessment
                configurationModel = vehicleValidation.CreateConfigurationModel(vehicle);
                moveTypeParams = new AssessMoveTypeParams
                {
                    ForceApplication = true,
                    configuration = configurationModel
                };
                movementTypeClass = vehicleConfigService.AssessMovementType(moveTypeParams);
                movementTypeClassList.Add(movementTypeClass);

                int vehicleCategory = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(movementTypeClass, false, true);
                int movementClassification = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryReverseMapping(vehicleCategory);
                #endregion

                #region Perform Validation After Assessment
                vehicleValidateInputModel = new VehicleValidateInputModel
                {
                    Vehicle = vehicle,
                    IsFleet = false,
                    RouteNames = routesRouteName,
                    VehiclesCount = vehicleCount,
                    RoutesCount = routesCount,
                    PrevVehicleMovementClass = previousVehicleMC,
                    AssessedVehicleMovementClass = vehicleCategory,
                    AssessedMovementClass = movementClassification,
                    IsNen = false,
                    IsNea = true,
                    VehiclePos = i,
                    PreviousMovementType = i > 1 ? movementTypeClassList[0] : null,
                    CurrentMovementType = movementTypeClass
                };
                vehicleImportModel = vehicleValidation.ValidateVehicleData(vehicleValidateInputModel, out bool isVr1);
                previousVehicleMC = vehicleImportModel.VehicleConfigDetails.MovementClassification;
                if (vehicleImportModel.VehicleError != null)
                    vehicleValidationError.Append(vehicleImportModel.VehicleError.ErrorMessage + "~");
                else
                {
                    vehicleImportModel.VehicleConfigDetails.MovementClassification = vehicleCategory;
                    vehicleImportModelList.Add(vehicleImportModel);
                }
                #endregion

                i++;
            }
            string vehicleErrorMessage = vehicleValidationError.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(vehicleErrorMessage))
            {
                List<string> vehicleDetailsErrorList = vehicleErrorMessage.Split('~').ToList();
                vehicleDetailsErrorList.RemoveAt(vehicleDetailsErrorList.Count - 1);
                validationObj.VehicleDetailsError = vehicleDetailsErrorList;
                validationError = true;
            }
            #endregion

            #endregion

            #region General Class Validation
            movementTypeClass = movementTypeClassList[0];
            int movGeneralclass = validation.GeneralClassReverseMapping(movementTypeClass.VehicleClass);
            if (movGeneralclass != appGeneralDetails.Classification)
            {
                ExternalApiGeneralClassificationType inputGCType = (ExternalApiGeneralClassificationType)appGeneralDetails.Classification;
                ExternalApiGeneralClassificationType outputGCType = (ExternalApiGeneralClassificationType)movementTypeClass.VehicleClass;

                string inputGeneralClass = inputGCType != 0 ? inputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"];
                string outputGeneralClass = outputGCType != 0 ? outputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"];
                string[] validationErrorList = validation.ValidationFailure["GeneralClassError"].Split(';');
                validationObj.ClassificationError = validationErrorList[0] + outputGeneralClass + validationErrorList[1] + inputGeneralClass + validationErrorList[2];
                validationError = true;
            }
            #endregion

            #region Route Validation
            NERouteImport routeImport;
            NERouteImportModel routeImportModel;
            List<NERouteImport> routeImportList = new List<NERouteImport>();
            NERouteValidation nERouteValidation = new NERouteValidation
            {
                ValidationFailure = validation.ValidationFailure
            };
            i = 1;
            StringBuilder routeValidationError = new StringBuilder();
            foreach (var route in neApplication.Routes)
            {
                routeImportModel = new NERouteImportModel
                {
                    Route = new Route { GPX = StringExtraction.Base64Decode(route.GPX), RouteDescription = route.RouteDescription, RouteName = route.RouteName },
                    RouteNames = vehicleRouteName,
                    IsDuplicate = duplicateRouteName
                };
                routeImport = nERouteValidation.ValidateNeRoute(routeImportModel, i);
                if (routeImport.RouteError != null)
                    routeValidationError.Append(routeImport.RouteError.ErrorMessage + "~");
                else
                    routeImportList.Add(routeImport);
                i++;
            }
            string routeErrorMessage = routeValidationError.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(routeErrorMessage))
            {
                List<string> routeDetailsErrorList = routeErrorMessage.Split('~').ToList();
                routeDetailsErrorList.RemoveAt(routeDetailsErrorList.Count - 1);
                validationObj.RouteDetailsError = routeDetailsErrorList;
                validationError = true;
            }
            #endregion

            #region Validation Errors
            if (validationError)
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.ValidationFailure,
                    Data = validationObj
                };
            }
            else
            {
                validationObj.GeneralDetails = appGeneralDetails;
                validationObj.VehicleList = vehicleImportModelList;
                validationObj.RouteList = routeImportList;
                externalApiResponse = new ExternalApiResponse
                {
                    Success = true,
                    Data = validationObj
                };
            }
            return Content(HttpStatusCode.OK, externalApiResponse);
            #endregion
        }
        #endregion
    }
}
