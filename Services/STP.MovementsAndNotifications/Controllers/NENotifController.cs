using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.General;
using STP.Common.Logger;
using STP.Domain.Custom;
using STP.Domain.DocumentsAndContents;
using STP.Domain.ExternalAPI;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;
using STP.Domain.RouteAssessment;
using STP.Domain.Routes;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.MovementsAndNotifications.Providers;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.RouteAssessment;
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
using static STP.Domain.Routes.RouteModel;

namespace STP.MovementsAndNotifications.Controllers
{
    public class NENotifController : ApiController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IVehicleConfigService vehicleConfigService;
        private readonly IRoutesService routesService;
        private readonly IRouteAssessmentService routeAssessmentService;
        private readonly IDocumentService documentService;
        private readonly IOrganizationService organizationService;
        public NENotifController()
        {
        }
        public NENotifController(IAuthenticationService authentication, IVehicleConfigService vehicleService, IRoutesService routeService, IRouteAssessmentService routeAssessService, IDocumentService docService, IOrganizationService orgService)
        {
            vehicleConfigService = vehicleService;
            routesService = routeService;
            routeAssessmentService = routeAssessService;
            documentService = docService;
            authenticationService = authentication;
            organizationService = orgService;
        }

        #region Insert Non ESDAL Notification
        /// <summary>
        /// An external haulier system to receiver non esdal notification
        /// </summary>
        /// <param name="NENotification"></param>
        /// <returns>NENotifOuput</returns>
        [HttpPost]
        [Route("Notification/InsertNENotification")]
        public IHttpActionResult InsertNENotification([FromBody] NENotification neNotification)
        {
            ExternalApiResponse externalApiResponse = new ExternalApiResponse();
            dynamic validationObj = new ExpandoObject();
            bool validationError = false;
            try
            {
                #region Input Data Check
                if (neNotification == null || neNotification.GeneralDetails == null || neNotification.Vehicles == null || neNotification.Routes == null || neNotification.AffectedParties == null)
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.BadRequest
                    };
                    return Content(HttpStatusCode.BadRequest, externalApiResponse);
                }
                #endregion

                #region Authentication
                if (string.IsNullOrWhiteSpace(neNotification.AuthenticationKey))
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.Unauthorized
                    };
                    return Content(HttpStatusCode.Unauthorized, externalApiResponse);
                }

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(neNotification.AuthenticationKey);

                if (orgDetails.OrganisationId == 0)
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.Unauthorized
                    };
                    return Content(HttpStatusCode.Unauthorized, externalApiResponse);
                }
                #endregion

                #region General Details Validation
                NENotifGeneralDetails notifGeneralDetails;
                GeneralDetailsValidation validation = new GeneralDetailsValidation();
                ValidNERenotif validNERenotif = null;
                bool isRenotify = false;
                if (!string.IsNullOrWhiteSpace(neNotification.GeneralDetails.ESDALReferenceNumber))
                {
                    isRenotify = true;
                    validNERenotif = NENotifProvider.Instance.IsNenRenotified(neNotification.GeneralDetails.ESDALReferenceNumber);
                    if (validNERenotif.NonEsdalKeyId != orgDetails.OrganisationId)
                    {
                        externalApiResponse = new ExternalApiResponse
                        {
                            ErrorMessage = validation.ValidationFailure["UnauthorizedRenotif"]
                        };
                        return Content(HttpStatusCode.Unauthorized, externalApiResponse);
                    }
                }
                notifGeneralDetails = validation.ValidateNotifGeneralDetail(neNotification.GeneralDetails, validNERenotif);
                if (notifGeneralDetails.GeneralDetailError != null)
                {
                    validationObj.GeneralDetailsError = notifGeneralDetails.GeneralDetailError.ErrorList;
                    validationError = true;
                }
                #endregion

                #region Affected Parties Validation
                List<ContactModel> contactModelFiltered = null;
                AffectedPartiesValidation affectedPartiesValidation = null;
                int affectedPartiesCount = neNotification.AffectedParties.Count;
                if (affectedPartiesCount > 0)
                {
                    string affectedParties = string.Join(",", neNotification.AffectedParties);
                    List<ContactModel> contactModel = organizationService.GetAffectedOrganisationDetails(affectedParties, affectedPartiesCount, UserSchema.Portal);
                    contactModelFiltered = contactModel != null ? contactModel.Where(x => x.IsReceiveNen > 0).ToList() : new List<ContactModel>();

                    List<string> invalidParties;
                    if (contactModel != null && contactModel.Count != affectedPartiesCount)
                    {
                        var validParties = contactModel.Select(x => x.Organisation).ToList();
                        invalidParties = neNotification.AffectedParties.Except(validParties).ToList();
                        affectedPartiesValidation = new AffectedPartiesValidation
                        {
                            ErrorMessage = validation.ValidationFailure["NotValidParties"],
                            InvalidAffectedParties = invalidParties
                        };
                        validationObj.AffectedPartiesError = affectedPartiesValidation;
                        validationError = true;
                    }
                    if (contactModelFiltered.Count == 0)
                    {
                        invalidParties = contactModel.Select(x => x.Organisation).ToList();
                        affectedPartiesValidation = new AffectedPartiesValidation
                        {
                            ErrorMessage = validation.ValidationFailure["PartiesDisabledNen"],
                            InvalidAffectedParties = invalidParties
                        };
                        validationObj.AffectedPartiesDisabled = affectedPartiesValidation;
                        validationError = true;
                    }
                    else if(contactModel != null && contactModel.Count != contactModelFiltered.Count)
                    {
                        var validParties = contactModelFiltered.Select(x => x.Organisation).ToList();
                        invalidParties = contactModel.Select(x => x.Organisation).Except(validParties).ToList();
                        affectedPartiesValidation = new AffectedPartiesValidation
                        {
                            ErrorMessage = validation.ValidationFailure["PartiesDisabledNen"],
                            InvalidAffectedParties = invalidParties
                        };
                    }
                }
                else
                {
                    validationObj.AffectedPartiesError = validation.ValidationFailure["NotAvailableParties"];
                    validationError = true;
                }
                
                #endregion

                #region Vehicle validation

                #region Model for Vehicle validation
                int vehicleCategory = 0;
                int movementClassification = 0;
                int previousVehicleMC = 0;
                VehicleValidationExternal vehicleValidation;
                AssessMoveTypeParams moveTypeParams;
                VehicleImportModel vehicleImportModel;
                VehicleMovementType movementTypeClass = null;
                List<VehicleMovementType> movementTypeClassList = new List<VehicleMovementType>();
                ConfigurationModel configurationModel;
                VehicleValidateInputModel vehicleValidateInputModel;
                VehicleImportOutput importOutput = new VehicleImportOutput();
                bool IsVr1 = false;
                #endregion

                #region Check Route Name and Vehicle Name 
                List<string> vehicleRouteName = neNotification.Vehicles.Select(s => s.RouteName).ToList();
                List<string> routesRouteName = neNotification.Routes.Select(s => s.RouteName).ToList();

                int vehicleCount = neNotification.Vehicles.Count;
                int routesCount = neNotification.Routes.Count;
                bool duplicateVehicleRouteName = routesRouteName.GroupBy(x => x).Any(g => g.Count() > 1);
                #endregion

                #region Loop for validation method
                int i = 1;
                StringBuilder vehicleValidationError = new StringBuilder();
                foreach (var vehicle in neNotification.Vehicles)
                {
                    vehicleValidation = new VehicleValidationExternal();

                    #region Perform Vehicle Assessment
                    configurationModel = vehicleValidation.CreateConfigurationModel(vehicle);
                    moveTypeParams = new AssessMoveTypeParams
                    {
                        configuration = configurationModel
                    };
                    movementTypeClass = vehicleConfigService.AssessMovementType(moveTypeParams);
                    movementTypeClassList.Add(movementTypeClass);
                    vehicleCategory = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(movementTypeClass, true, false);
                    movementClassification = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryReverseMapping(vehicleCategory);
                    #endregion

                    #region Validation After Assessment
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
                        IsNen = true,
                        IsNea = false,
                        VehiclePos = i,
                        PreviousMovementType = i > 1 ? movementTypeClassList[0] : null,
                        CurrentMovementType = movementTypeClass
                    };
                    vehicleImportModel = vehicleValidation.ValidateVehicleData(vehicleValidateInputModel, out IsVr1);
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
                movementTypeClass.PoliceNoticePeriod = movementTypeClassList.Max(x => x.PoliceNoticePeriod);
                movementTypeClass.SOANoticePeriod = movementTypeClassList.Max(x => x.SOANoticePeriod);
                int movGeneralclass = validation.GeneralClassReverseMapping(movementTypeClass.VehicleClass);
                if (movGeneralclass != notifGeneralDetails.Classification)
                {
                    ExternalApiGeneralClassificationType inputGCType = (ExternalApiGeneralClassificationType)notifGeneralDetails.Classification;
                    ExternalApiGeneralClassificationType outputGCType = (ExternalApiGeneralClassificationType)movGeneralclass;

                    string inputGeneralClass = inputGCType != 0 ? inputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"];
                    string outputGeneralClass = outputGCType != 0 ? outputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"];
                    string[]  validationErrorList = validation.ValidationFailure["GeneralClassError"].Split(';');
                    validationObj.ClassificationError = validationErrorList[0] + outputGeneralClass + validationErrorList[1] + inputGeneralClass + validationErrorList[2];
                    validationError = true;
                }
                #endregion

                #region Indemnity Check
                bool isSoaExist = contactModelFiltered.Exists(x => !x.ISPolice);
                if(movementTypeClass.SOANoticePeriod > 0) //Indemnity Required
                {
                    if (notifGeneralDetails.Indemnity == 0)
                    {
                        validationObj.IndemnityError = validation.ValidationFailure["IndemnityRequired"];
                        validationError = true;
                    }
                    else
                        notifGeneralDetails.Indemnity = !isSoaExist ? 0 : notifGeneralDetails.Indemnity;
                }
                else // Indemnity Not Required
                {
                    if(isSoaExist && notifGeneralDetails.Indemnity == 0)
                    {
                        validationObj.IndemnityError = validation.ValidationFailure["IndemnityRequired"];
                        validationError = true;
                    }
                    else
                        notifGeneralDetails.Indemnity = 0;
                }
                
                #endregion

                #region Route Validation
                NERouteValidation nERouteValidation = new NERouteValidation
                {
                    ValidationFailure = validation.ValidationFailure
                };
                NERouteImport routeImport;
                NERouteImportModel routeImportModel;
                List<NERouteImport> routeImportList = new List<NERouteImport>();
                i = 1;
                StringBuilder routeValidationError = new StringBuilder();
                foreach (var route in neNotification.Routes)
                {
                    routeImportModel = new NERouteImportModel
                    {
                        Route = new Route { GPX = StringExtraction.Base64Decode(route.GPX), RouteDescription = route.RouteDescription, RouteName = route.RouteName },
                        RouteNames = vehicleRouteName,
                        IsDuplicate = duplicateVehicleRouteName
                    };
                    routeImport = nERouteValidation.ValidateNeRoute(routeImportModel , i);
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

                #region Notification Creation
                notifGeneralDetails.RequireVR1 = IsVr1 ? 1 : 0;
                notifGeneralDetails.RequireSo = vehicleCategory == (int)ExternalApiMovementClassificationMapping.MC114 ? 1 : 0;
                notifGeneralDetails.Classification = movementTypeClass.VehicleClass;
                notifGeneralDetails.NonEsdalKeyId = orgDetails.OrganisationId;
                NotificationGeneralDetails notifDetails;
                notifDetails = NENotifProvider.Instance.SaveNENotification(notifGeneralDetails);
                #endregion

                #region Notification Process
                if (notifDetails.NotificationId > 0)
                {
                    CommonNotifMethods commonNotif = new CommonNotifMethods();
                    string imminentMsg = string.Empty;

                    #region Route Import API
                    GPXInputModel GPXInputModel;
                    SaveNERouteParams saveNERoute;

                    foreach (var route in routeImportList.Select(item => item.Route))
                    {
                        GPXInputModel = new GPXInputModel
                        {
                            RouteGPX = route.GPX
                        };
                        saveNERoute = new SaveNERouteParams()
                        {
                            GPXInput = GPXInputModel,
                            NotificationId = notifDetails.NotificationId,
                            RouteName = route.RouteName,
                            RouteDescription = route.RouteDescription
                        };
                        long routeId = 0;
                        List<long> routeIds = new List<long>();
                        routeId = routesService.SaveNERoute(saveNERoute);
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
                    NEVehicleImport nenVehicle = new NEVehicleImport
                    {
                        MovementId = importOutput.MovementId,
                        IsVr1 = false,
                        RevisionId = 0,
                        NotificationId = notifDetails.NotificationId,
                        IsNotif = true
                    };
                    List<long> vehicleIds = vehicleConfigService.SaveNonEsdalVehicle(nenVehicle);

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

                    #region RouteAssessment
                    List<RoutePartDetails> routePartDet = routesService.GetRouteDetailForAnalysis(0, notifDetails.ContentReferenceNo, 0, 0, UserSchema.Portal);

                    if (notifGeneralDetails.Classification == (int)ExternalApiGeneralClassificationType.GC001)
                    {
                        int soaNoticePeriod = 0;
                        int policeNoticePeriod = 0;
                        if (notifGeneralDetails.VSOType == (int)ExternalApiVSOType.soa)
                            soaNoticePeriod = 2;
                        else if (notifGeneralDetails.VSOType == (int)ExternalApiVSOType.police)
                            policeNoticePeriod = 2;
                        else if (notifGeneralDetails.VSOType == (int)ExternalApiVSOType.soapolice)
                        {
                            soaNoticePeriod = 2;
                            policeNoticePeriod = 2;
                        }
                        movementTypeClass = new VehicleMovementType
                        {
                            SOANoticePeriod = soaNoticePeriod,
                            PoliceNoticePeriod = policeNoticePeriod
                        };
                    }
                    else
                    {
                        List<int> CountryIdList = new List<int>();
                        int WorkingDays = 0;
                        string strContryID = "";
                        foreach (RoutePartDetails routePart in routePartDet)
                        {
                            CountryIdList = routeAssessmentService.GetCountryId((int)routePart.RouteId);
                        }
                        CountryIdList = CountryIdList.Distinct().ToList();
                        if (CountryIdList.Count == 0)
                        {
                            WorkingDays += NENotifProvider.Instance.GetWorkingDays(notifGeneralDetails.MovementStart, 0);
                        }
                        else
                        {
                            List<int> WorkingDaysList = new List<int>();
                            StringBuilder bld = new StringBuilder();
                            foreach (int CountryID in CountryIdList)
                            {
                                if (CountryID != 0)
                                {
                                    WorkingDaysList.Add(NENotifProvider.Instance.GetWorkingDays(notifGeneralDetails.MovementStart, CountryID));
                                    bld.Append(CountryID + ",");
                                }
                            }
                            strContryID = bld.ToString();
                            WorkingDays = WorkingDaysList.Min();
                        }
                        int imminentStatus;

                        imminentStatus = commonNotif.GetImminent(WorkingDays, movementTypeClass);
                        imminentMsg = commonNotif.GetImminentMessage(imminentStatus, strContryID);
                        if (imminentStatus == 2 && !string.IsNullOrWhiteSpace(imminentMsg))
                        {
                            notifGeneralDetails.VSOType = (int)ExternalApiVSOType.police;
                        }
                    }
                    RouteAssessmentModel routeAssessmentModel = routeAssessmentService.GenerateNenRouteAssessment(routePartDet, notifDetails.AnalysisId, contactModelFiltered, true);
                    #endregion;

                    #region GenerateNotification
                    string EsdalRefNum = NENotifProvider.Instance.GenerateNENotifCode(notifDetails.NotificationId, notifGeneralDetails.Indemnity);
                    string xmlaffectedStructures = Encoding.UTF8.GetString(XsltTransformer.Trafo(routeAssessmentModel.AffectedStructure));
                    Dictionary<int, int> icaStatusDictionary = commonNotif.GetNotifICAstatus(xmlaffectedStructures);
                    AffectedStructConstrParam affectedParam = new AffectedStructConstrParam
                    {
                        AffectedConstraints = commonNotif.GetAffectedConstrList(routeAssessmentModel.Constraints),
                        AffectedSections = commonNotif.GetAffectedStructList(routeAssessmentModel.AffectedStructure),
                        NotificationId = notifDetails.NotificationId
                    };
                    if (affectedParam.AffectedSections.Count > 0 || affectedParam.AffectedConstraints.Count > 0)
                    {
                        SimpleNotificationProvider.Instance.SaveAffectedNotificationDetails(affectedParam);
                    }
                    UserInfo SessionInfo = new UserInfo
                    {
                        UserTypeId = 0,
                        UserSchema = UserSchema.Portal
                    };

                    NotifDistibutionParams distibution = new NotifDistibutionParams
                    {
                        NotificationId = (int)notifDetails.NotificationId,
                        ContactId = 0,
                        ICAStatusDictionary = icaStatusDictionary,
                        ImminentMovestatus = imminentMsg,
                        SessionInfo = SessionInfo,
                        ContactModel = contactModelFiltered,
                        IsNen = true,
                        IsRenotify = isRenotify
                    };
                    //Sending mail, fax or saving inbox only
                    documentService.DistributeNotification(distibution);

                    List<string> afec = new List<string>();
                    foreach (ContactModel contact in contactModelFiltered)
                    {
                        afec.Add(contact.Organisation);
                    }
                    NENotifOuput nENotifOuput = new NENotifOuput
                    {
                        AffectedParties = afec,
                        ESDALReferenceNumber = EsdalRefNum,
                        PartiesNotReceivedNen = affectedPartiesValidation
                    };
                    #endregion

                    #region Create NEN Route relation with each affected parties
                    CreateNenRouteClone(contactModelFiltered, notifDetails);
                    #endregion

                    #region Store Input Json File
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(neNotification);
                    JsonStore.StoreInputJsonFile(json, notifDetails.NotificationId, nENotifOuput.ESDALReferenceNumber);
                    #endregion

                    externalApiResponse = new ExternalApiResponse
                    {
                        Success = true,
                        Data = nENotifOuput
                    };
                    return Content(HttpStatusCode.OK, externalApiResponse);
                }
                #endregion

                #region Validation Error
                else
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = validation.ValidationFailure["NotifError"]
                    };
                    return Content(HttpStatusCode.InternalServerError, externalApiResponse);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"NENotification/InsertNENotification, Exception: ", ex);
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.InternalServerError
                };
                return Content(HttpStatusCode.InternalServerError, externalApiResponse);
            }
        }
        #endregion

        #region CreateNenRouteClone
        /// <summary>
        /// The function used for following
        /// 1 Create a clone of route/vehicle/analysed_routes based on organisation count
        /// 2 Update route assessment for each route based on new route id 
        /// </summary>
        /// <param name="contactModelFiltered"></param>
        /// <param name="notifDetails"></param>
        private void CreateNenRouteClone(List<ContactModel> contactModelFiltered, NotificationGeneralDetails notifDetails)
        {
            CloneNenRoute cloneNenRoute = new CloneNenRoute
            {
                NotificationId = notifDetails.NotificationId,
                ContentRefNo = notifDetails.ContentReferenceNo,
                AnalysisId = notifDetails.AnalysisId,
                Organisations = string.Join(",", contactModelFiltered.Select(x => x.OrganisationId)),
                OrgCount = contactModelFiltered.Count
            };
            List<RoutePartDetails> routePartDet;
            List<NenRouteList> nenRouteLists = routesService.CloneNenRoute(cloneNenRoute);
            if (nenRouteLists != null && nenRouteLists.Any() && contactModelFiltered.Any())
            {
                foreach (var s in contactModelFiltered)
                {
                    routePartDet = nenRouteLists.Where(x => x.OrganisationId == s.OrganisationId).Select(item => new RoutePartDetails
                    {
                        AnalysisId = (int)item.AnalysisId,
                        RouteId = item.RoutePartId,
                        RouteName = item.RouteName,
                        RouteType = item.RouteType
                    }).ToList();
                    routeAssessmentService.GenerateNenRouteAssessment(routePartDet, routePartDet[0].AnalysisId, contactModelFiltered, false);
                }
            }
        }
        #endregion

        #region CheckNotificationStatus
        /// <summary>
        /// An external haulier system can check the submitted application status by using ESDAL Reference Number
        /// </summary>
        /// <param name="ESDALReferenceNumber"></param>
        /// <param name="AuthenticationKey"></param>
        /// <param name="HaulierOrgName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Notification/CheckNotificationStatus")]
        public IHttpActionResult CheckNotificationStatus(string ESDALReferenceNumber = null, string AuthenticationKey = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ESDALReferenceNumber))
                    return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                AuthKeyValid orgDetails = authenticationService.GetOrgDetailsByAuthKey(AuthenticationKey);

                if (orgDetails.OrganisationId == 0)
                    return Content(HttpStatusCode.Unauthorized, ExternalApiStatusMessage.Unauthorized);

                AuthorizedOrganisation authorizedUsers = authenticationService.GetAuthorizedUsers(ESDALReferenceNumber, false);
                if (authorizedUsers.ValidCount == 0)
                    return Content(HttpStatusCode.OK, ExternalApiStatusMessage.NotValidEsdalReference);

                var isReceiver = authorizedUsers.Receivers.Count > 0 && authorizedUsers.Receivers.Contains(orgDetails.OrganisationId);
                var isSender = authorizedUsers.SenderId == orgDetails.OrganisationId;

                if (isSender || isReceiver)
                {
                    var notificationStatusList = NENotifProvider.Instance.GetNENotificationStatus(ESDALReferenceNumber);

                    if (notificationStatusList == null)
                        return Content(HttpStatusCode.BadRequest, ExternalApiStatusMessage.BadRequest);

                    return Content(HttpStatusCode.OK, notificationStatusList);
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

        #region Test NEN Validation
        /// <summary>
        /// An external haulier system to receiver non esdal notification
        /// </summary>
        /// <param name="NENotification"></param>
        /// <returns>NENotifOuput</returns>
        [HttpPost]
        [Route("Notification/TestNEValidation")]
        public IHttpActionResult TestNEValidation([FromBody] NENotification neNotification)
        {
            ExternalApiResponse externalApiResponse = new ExternalApiResponse();
            dynamic validationObj = new ExpandoObject();
            bool validationError = false;

            #region Input Data Check
            if (neNotification == null || neNotification.GeneralDetails == null || neNotification.Vehicles == null || neNotification.Routes == null || neNotification.AffectedParties == null)
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.BadRequest
                };
                return Content(HttpStatusCode.BadRequest, externalApiResponse);
            }
            #endregion

            #region Authentication
            if (string.IsNullOrWhiteSpace(neNotification.AuthenticationKey))
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.Unauthorized
                };
                return Content(HttpStatusCode.Unauthorized, externalApiResponse);
            }
            #endregion

            #region General Details Validation
            NENotifGeneralDetails notifGeneralDetails;
            GeneralDetailsValidation validation = new GeneralDetailsValidation();
            notifGeneralDetails = validation.ValidateNotifGeneralDetail(neNotification.GeneralDetails, null);
            if (notifGeneralDetails.GeneralDetailError != null)
            {
                validationObj.GeneralDetailsError = notifGeneralDetails.GeneralDetailError.ErrorList;
                validationError = true;
            }
            #endregion

            #region Vehicle validation

            #region Model for Vehicle validation
            int vehicleCategory = 0;
            int movementClassification = 0;
            int previousVehicleMC = 0;
            VehicleValidationExternal vehicleValidation;
            AssessMoveTypeParams moveTypeParams;
            VehicleImportModel vehicleImportModel;
            List<VehicleImportModel> vehicleImportModelList = new List<VehicleImportModel>();
            VehicleMovementType movementTypeClass = null;
            List<VehicleMovementType> movementTypeClassList = new List<VehicleMovementType>();
            ConfigurationModel configurationModel;
            VehicleValidateInputModel vehicleValidateInputModel;
            bool IsVr1 = false;
            #endregion

            #region Check Route Name and Vehicle Name 
            List<string> vehicleRouteName = neNotification.Vehicles.Select(s => s.RouteName).ToList();
            List<string> routesRouteName = neNotification.Routes.Select(s => s.RouteName).ToList();

            int vehicleCount = neNotification.Vehicles.Count;
            int routesCount = neNotification.Routes.Count;
            bool duplicateVehicleRouteName = routesRouteName.GroupBy(x => x).Any(g => g.Count() > 1);
            #endregion

            #region Loop for validation method
            int i = 1;
            StringBuilder vehicleValidationError = new StringBuilder();
            foreach (var vehicle in neNotification.Vehicles)
            {
                vehicleValidation = new VehicleValidationExternal();

                #region Perform Vehicle Assessment
                configurationModel = vehicleValidation.CreateConfigurationModel(vehicle);
                moveTypeParams = new AssessMoveTypeParams
                {
                    configuration = configurationModel
                };
                movementTypeClass = vehicleConfigService.AssessMovementType(moveTypeParams);
                movementTypeClassList.Add(movementTypeClass);
                vehicleCategory = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(movementTypeClass, true, false);
                movementClassification = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryReverseMapping(vehicleCategory);

                #endregion

                #region Validation After Assessment
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
                    IsNen = true,
                    IsNea = false,
                    VehiclePos = i,
                    PreviousMovementType = i > 1 ? movementTypeClassList[0] : null,
                    CurrentMovementType = movementTypeClass
                };
                vehicleImportModel = vehicleValidation.ValidateVehicleData(vehicleValidateInputModel, out IsVr1);
                previousVehicleMC = vehicleImportModel.VehicleConfigDetails != null ? vehicleImportModel.VehicleConfigDetails.MovementClassification : 0;
                if (vehicleImportModel.VehicleError != null)
                    vehicleValidationError.Append(vehicleImportModel.VehicleError.ErrorMessage + "~");
                if(vehicleImportModel.VehicleConfigDetails != null)
                    vehicleImportModel.VehicleConfigDetails.MovementClassification = vehicleCategory;
                vehicleImportModelList.Add(vehicleImportModel);
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
            movementTypeClass.PoliceNoticePeriod = movementTypeClassList.Max(x => x.PoliceNoticePeriod);
            movementTypeClass.SOANoticePeriod = movementTypeClassList.Max(x => x.SOANoticePeriod);
            
            string[] validationErrorList;
            int movGeneralclass = 0;
            movGeneralclass = validation.GeneralClassReverseMapping(movementTypeClass.VehicleClass);
            if (movGeneralclass != notifGeneralDetails.Classification)
            {
                ExternalApiGeneralClassificationType inputGCType = (ExternalApiGeneralClassificationType)notifGeneralDetails.Classification;
                ExternalApiGeneralClassificationType outputGCType = (ExternalApiGeneralClassificationType)movGeneralclass;

                string inputGeneralClass = inputGCType != 0 ? inputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"];
                string outputGeneralClass = outputGCType != 0 ? outputGCType.GetEnumDescription() : validation.ValidationFailure["NoGeneralClass"];
                validationErrorList = validation.ValidationFailure["GeneralClassError"].Split(';');
                validationObj.ClassificationError = validationErrorList[0] + outputGeneralClass + validationErrorList[1] + inputGeneralClass + validationErrorList[2];
                validationError = true;
            }
            #endregion

            #region Route Validation
            NERouteValidation nERouteValidation = new NERouteValidation
            {
                ValidationFailure = validation.ValidationFailure
            };
            NERouteImport routeImport;
            NERouteImportModel routeImportModel;
            List<NERouteImport> routeImportList = new List<NERouteImport>();
            i = 1;
            StringBuilder routeValidationError = new StringBuilder();
            foreach (var route in neNotification.Routes)
            {
                routeImportModel = new NERouteImportModel
                {
                    Route = new Route { GPX = StringExtraction.Base64Decode(route.GPX), RouteDescription = route.RouteDescription, RouteName = route.RouteName },
                    RouteNames = vehicleRouteName,
                    IsDuplicate = duplicateVehicleRouteName
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
                validationObj.GeneralDetails = notifGeneralDetails;
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
