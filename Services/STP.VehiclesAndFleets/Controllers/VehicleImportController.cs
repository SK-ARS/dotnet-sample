using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.Custom;
using STP.Domain.ExternalAPI;
using STP.Domain.NonESDAL;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.ServiceAccess.SecurityAndUsers;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.VehiclesAndFleets.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Net;
using System.Web.Http;
using static STP.Common.Enums.ExternalApiEnums;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;

namespace STP.VehiclesAndFleets.Controllers
{
    public class VehicleImportController : ApiController
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];
        private readonly IAuthenticationService authenticationService;
        private readonly IVehicleConfigService vehicleConfigService;
        public VehicleImportController(IAuthenticationService authenticateService, IVehicleConfigService vehicleService)
        {
            authenticationService = authenticateService;
            vehicleConfigService = vehicleService;
        }

        #region Insert Fleet Vehicle
        [HttpPost]
        [Route("VehicleImport/InsertFleetVehicle")]
        public IHttpActionResult InsertFleetVehicle([FromBody] VehicleImportExternal vehicleImportExternal)
        {
            ExternalApiResponse externalApiResponse;
            dynamic validationObj = new ExpandoObject();
            bool validationError = false;

            #region Input data check
            if (vehicleImportExternal == null || vehicleImportExternal.VehicleConfiguration == null || vehicleImportExternal.VehicleComponents == null)
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.BadRequest
                };
                return Content(HttpStatusCode.BadRequest, externalApiResponse);
            }
            #endregion

            #region Authentication
            if (string.IsNullOrWhiteSpace(vehicleImportExternal.AuthenticationKey))
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.Unauthorized
                };
                return Content(HttpStatusCode.Unauthorized, externalApiResponse);
            }
            #endregion

            try
            {
                ValidateAuthentication authentication = authenticationService.ValidateAuthentication(vehicleImportExternal.AuthenticationKey);
                if (authentication.OrganisationId != 0 && authentication.UserTypeId == UserType.Haulier) // If Authorized
                {
                    string vehicleName = vehicleImportExternal.VehicleConfiguration.Name;
                    int isExist = VehicleImport.Instance.CheckFormalName(vehicleName, authentication.OrganisationId);
                    VehicleValidationExternal vehicleValidation = new VehicleValidationExternal();

                    #region Vehicle Creation
                    if (isExist == 0) // if Vehicle name doesn't exist
                    {
                        #region Vehicle Validation check
                        Vehicle vehicle = new Vehicle // Model for vehicle import
                        {
                            UnitSystem = vehicleImportExternal.UnitSystem,
                            VehicleConfiguration = vehicleImportExternal.VehicleConfiguration,
                            VehicleComponents = vehicleImportExternal.VehicleComponents
                        };

                        #region Vehicle assessment
                        ConfigurationModel configurationModel = vehicleValidation.CreateConfigurationModel(vehicle); // Model for vehicle assessment
                        AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                        {
                            configuration = configurationModel
                        };

                        VehicleMovementType movementTypeClass = vehicleConfigService.AssessMovementType(moveTypeParams); // Assessment

                        int vehicleCategory = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(movementTypeClass);
                        int movementClassification = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryReverseMapping(vehicleCategory);
                        #endregion

                        #region Input Valdiation 
                        VehicleValidateInputModel vehicleValidateInputModel = new VehicleValidateInputModel // Model for vehicle validation
                        {
                            Vehicle = vehicle,
                            IsFleet = true,
                            AssessedVehicleMovementClass = vehicleCategory,
                            AssessedMovementClass = movementClassification,
                            IsNen = false,
                            IsNea = false,
                            VehiclePos = 1
                        };
                        VehicleImportModel vehicleImportModel = vehicleValidation.ValidateVehicleData(vehicleValidateInputModel, out bool isVr1);
                        VehicleImportOutput vehicleImport = null;
                        if (vehicleImportModel.VehicleError != null)
                        {
                            validationError = true;
                            validationObj.VehicleDetailsError = vehicleImportModel.VehicleError.ErrorList;
                        }
                        else
                        {
                            vehicleImportModel.VehicleConfigDetails.MovementClassification = vehicleCategory;
                            vehicleImport = VehicleImport.Instance.InsertTempVehicle(vehicleImportModel);
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

                        #endregion

                        #region Vehicle creation process
                        long vehicleId = VehicleImport.Instance.InsertFleetVehicle(vehicleImport, authentication.OrganisationId);

                        if (vehicleId > 0)
                        {
                            #region Store Input Json File
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(vehicleImportExternal);
                            JsonStore.StoreInputJsonFile(json, vehicleId, vehicleImportExternal.VehicleConfiguration.Name);
                            #endregion

                            vehicleConfigService.DeleteTempMovementVehicle(vehicleImport.MovementId, UserSchema.Portal);
                            externalApiResponse = new ExternalApiResponse
                            {
                                Success = true,
                                Data = vehicleValidation.ValidationFailure["VehicleCreated"] + vehicleId
                            };
                            return Content(HttpStatusCode.OK, externalApiResponse);
                        }
                        else
                        {
                            externalApiResponse = new ExternalApiResponse
                            {
                                ErrorMessage = vehicleValidation.ValidationFailure["VehicleError"]
                            };
                            return Content(HttpStatusCode.InternalServerError, externalApiResponse);
                        }
                        #endregion
                    }
                    else
                    {
                        externalApiResponse = new ExternalApiResponse
                        {
                            ErrorMessage = vehicleValidation.ValidationFailure["VehicleExist"]
                        };
                        return Content(HttpStatusCode.OK, externalApiResponse);
                    }
                    #endregion
                }
                else
                {
                    externalApiResponse = new ExternalApiResponse
                    {
                        ErrorMessage = ExternalApiStatusMessage.Unauthorized
                    };
                    return Content(HttpStatusCode.Unauthorized, externalApiResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleImport/InsertFleetVehicle, Exception: " + ex​​​​​);
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.InternalServerError
                };
                return Content(HttpStatusCode.InternalServerError, externalApiResponse);
            }
        }
        #endregion

        #region Temp Vehicle Functions
        [HttpPost]
        [Route("VehicleImport/InsertTempVehicle")]
        public IHttpActionResult InsertIntoTempVehicle(VehicleImportModel vehicleImportModel)
        {
            try
            {
                VehicleImportOutput vehicleImport = VehicleImport.Instance.InsertTempVehicle(vehicleImportModel);
                return Content(HttpStatusCode.OK, vehicleImport);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleImport/InsertTempVehicle, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("VehicleImport/UpdateTempVehicle")]
        public IHttpActionResult UpdateTempVehicle(long movementId, int vehicleCategory, long vehicleId)
        {
            try
            {
                bool success = VehicleImport.Instance.UpdateTempVehicle(movementId, vehicleCategory, vehicleId);
                return Content(HttpStatusCode.OK, success);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleImport/UpdateTempVehicle, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("VehicleImport/DeleteTempMovementVehicle")]
        public IHttpActionResult DeleteTempMovementVehicle(long movementId, string userSchema)
        {
            try
            {
                bool success = VehicleImport.Instance.DeleteTempMovementVehicle(movementId, userSchema);
                return Content(HttpStatusCode.OK, success);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleImport/DeleteTempMovementVehicle, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Insert Non ESDAL App Vehicle
        [HttpPost]
        [Route("VehicleImport/InsertNonEsdalVehicle")]
        public IHttpActionResult InsertNonEsdalVehicle(NEVehicleImport neVehicleImport)
        {
            try
            {
                List<long> vehicleIds = VehicleImport.Instance.InsertNonEsdalVehicle(neVehicleImport);
                return Content(HttpStatusCode.OK, vehicleIds);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleImport/InsertNonEsdalVehicle, Exception: " + ex​​​​​);
                return Content(HttpStatusCode.InternalServerError, ExternalApiStatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Test Fleet Vehicle Import
        [HttpPost]
        [Route("VehicleImport/TestFleetVehicleImport")]
        public IHttpActionResult TestFleetVehicleImport([FromBody] VehicleImportExternal vehicleImportExternal)
        {
            ExternalApiResponse externalApiResponse;
            dynamic validationObj = new ExpandoObject();
            bool validationError = false;

            #region Input data check
            if (vehicleImportExternal == null || vehicleImportExternal.VehicleConfiguration == null || vehicleImportExternal.VehicleComponents == null)
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.BadRequest
                };
                return Content(HttpStatusCode.BadRequest, externalApiResponse);
            }
            #endregion

            #region Authentication
            if (string.IsNullOrWhiteSpace(vehicleImportExternal.AuthenticationKey))
            {
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.Unauthorized
                };
                return Content(HttpStatusCode.Unauthorized, externalApiResponse);
            }
            #endregion

            try
            {
                VehicleValidationExternal vehicleValidation = new VehicleValidationExternal();

                #region Vehicle Validation check
                Vehicle vehicle = new Vehicle // Model for vehicle import
                {
                    UnitSystem = vehicleImportExternal.UnitSystem,
                    VehicleConfiguration = vehicleImportExternal.VehicleConfiguration,
                    VehicleComponents = vehicleImportExternal.VehicleComponents
                };

                #region Vehicle assessment
                ConfigurationModel configurationModel = vehicleValidation.CreateConfigurationModel(vehicle); // Model for vehicle assessment
                AssessMoveTypeParams moveTypeParams = new AssessMoveTypeParams
                {
                    configuration = configurationModel
                };
                VehicleMovementType movementTypeClass = vehicleConfigService.AssessMovementType(moveTypeParams); // Assessment
                int vehicleCategory = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryMapping(movementTypeClass);
                int movementClassification = Domain.VehiclesAndFleets.Vehicles.VehicleCategoryToMovementType.VehicleCategoryReverseMapping(vehicleCategory);
                #endregion

                #region Input Valdiation 
                VehicleValidateInputModel vehicleValidateInputModel = new VehicleValidateInputModel // Model for vehicle validation
                {
                    Vehicle = vehicle,
                    IsFleet = true,
                    AssessedVehicleMovementClass = vehicleCategory,
                    AssessedMovementClass = movementClassification,
                    IsNen = false,
                    IsNea = false,
                    VehiclePos = 1
                };
                VehicleImportModel vehicleImportModel = vehicleValidation.ValidateVehicleData(vehicleValidateInputModel, out bool isVr1);
                if (vehicleImportModel.VehicleError != null)
                {
                    validationError = true;
                    validationObj.VehicleDetailsError = vehicleImportModel.VehicleError.ErrorList;
                }
                vehicleImportModel.VehicleConfigDetails.MovementClassification = vehicleCategory;
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

                externalApiResponse = new ExternalApiResponse
                {
                    Success = true,
                    Data = vehicleImportModel
                };
                return Content(HttpStatusCode.OK, externalApiResponse);
                

                #endregion
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleImport/InsertFleetVehicle, Exception: " + ex​​​​​);
                externalApiResponse = new ExternalApiResponse
                {
                    ErrorMessage = ExternalApiStatusMessage.InternalServerError
                };
                return Content(HttpStatusCode.InternalServerError, externalApiResponse);
            }
        }
        #endregion
    }
}
