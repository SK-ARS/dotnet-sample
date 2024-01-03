using STP.Domain;
using STP.VehiclesAndFleets.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using STP.Common.Constants;
using STP.Common.Logger;
using System.Configuration;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Applications;
using STP.Domain.Routes;
using System.Xml.Serialization;
using System.Xml;
using System.Web;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleGlobalConfig;
using STP.Domain.VehiclesAndFleets;
using STP.Common.Enums;
using STP.VehiclesAndFleets.Configurations;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.VehiclesAndFleets.Controllers
{
    public class VehicleConfigController : ApiController
    {
        private static string LogInstance = ConfigurationManager.AppSettings["Instance"];
        #region  VehiConfigCreateVehicleConfiguration--Provider
        #region InsertVehicleConfiguration
        /// <summary>
        /// To Create a new  vehicle Configuration.
        /// </summary>  
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/InsertVehicleConfiguration")]
        public IHttpActionResult InsertVehicleConfiguration(NewConfigurationModel configurationModel)
        {
            try
            {
                int configurationId = Convert.ToInt32(VehiConfigCreateVehicleConfiguration.Instance.InsertVehicleConfiguration(configurationModel));
                return Content(HttpStatusCode.Created, configurationId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region InsertVehicleConfigPosition
        /// <summary>
        /// Create  vehicle Configuration Position
        /// </summary> 
        /// <param name=VehicleConfigList>VehicleConfigList model class is used to input parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/InsertVehicleConfigurationPosition")]
        public IHttpActionResult InsertVehicleConfigurationPosition(VehicleConfigList configList)
        {
            try
            {
                VehicleConfigList objConfig = VehiConfigCreateVehicleConfiguration.Instance.InsertVehicleConfigPosition(configList);
                return Content(HttpStatusCode.Created, objConfig);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertVehicleConfigurationPosition,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion        
        #region CreateApplVehConfigPosition
        /// <summary>
        /// CreateApplVehConfigPosn
        /// </summary> 
        /// <param name=VehicleConfigList>VehicleConfigList model class is used to input parameters</param>
        /// <param name="userSchema"</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/CreateApplVehicleConfigPosition")]
        public IHttpActionResult CreateApplVehicleConfigPosition(VehicleConfigList configList, string userSchema)
        {
            try
            {
                VehicleConfigList objresponse = VehiConfigCreateVehicleConfiguration.Instance.CreateApplVehConfigPosn(configList, userSchema);
                return Content(HttpStatusCode.Created, objresponse);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CreateApplVehicleConfigPosition,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CreateAppConfigPosition
        /// <summary>
        /// CreateAppConfigPosn
        /// </summary> 
        /// <param name=VehicleConfigList>VehicleConfigList model class is used to input parameters</param>
        /// <param name="userSchema"</param>
        /// <param name="isImportFromFleet"</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/CreateApplConfigurationPosition")]
        public IHttpActionResult CreateApplConfigurationPosition(VehicleConfigList configList, int isImportFromFleet, string userSchema)
        {
            try
            {
                VehicleConfigList objResponse = VehiConfigCreateVehicleConfiguration.Instance.CreateAppConfigPosn(configList, isImportFromFleet, userSchema);
                return Content(HttpStatusCode.Created, objResponse);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CreateApplConfigurationPosition,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CreateVR1ConfigPosition
        /// <summary>
        /// CreateVR1ConfigPosn
        /// </summary> 
        /// <param name=VehicleConfigList>VehicleConfigList model class is used to input parameters</param>
        /// <param name="userSchema"</param>      
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/CreateVR1ConfigPosition")]
        public IHttpActionResult CreateVR1ConfigPosition(VehicleConfigList configList, string userSchema)
        {
            try
            {
                VehicleConfigList objResponse = VehiConfigCreateVehicleConfiguration.Instance.CreateVR1ConfigPosn(configList, userSchema);
                return Content(HttpStatusCode.Created, objResponse);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CreateVR1ConfigPosition,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region UpdateVehicleConfiguration
        /// <summary>
        /// Update vehicle Configuration.
        /// </summary> 
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/UpdateVehicleConfiguration")]
        public IHttpActionResult UpdateVehicleConfiguration(NewConfigurationModel configurationModel)
        {
            try
            {
                bool response = VehiConfigCreateVehicleConfiguration.Instance.UpdateVehicleConfiguration(configurationModel);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/UpdateVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  UpdateAppVehicleConfiguration
        /// <summary>
        /// Update  Appvehicle Configuration Details.
        /// </summary>  
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>       
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/UpdateApplVehicleConfiguration")]
        public IHttpActionResult UpdateApplVehicleConfiguration(NewConfigurationModel configurationModel, string userSchema)
        {
            try
            {
                bool response = VehiConfigCreateVehicleConfiguration.Instance.Updateappvehicleconfig(configurationModel, userSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/UpdateApplVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  UpdateVR1vehicleconfiguration
        /// <summary>
        /// Update  VR1 vehicle Configuration Details.
        /// </summary>  
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>       
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/UpdateVR1VehicleConfiguration")]
        public IHttpActionResult UpdateVR1VehicleConfiguration(NewConfigurationModel configurationModel, string userSchema)
        {
            try
            {
                bool response = VehiConfigCreateVehicleConfiguration.Instance.UpdateVR1vehicleconfig(configurationModel, userSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/UpdateVR1VehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SaveVehicleConfiguration
        /// <summary>
        /// Saving vehicle configuration details.
        /// </summary>         
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/SaveVehicleConfiguration")]
        public IHttpActionResult SaveVehicleConfiguration(SaveConfigurationModel saveConfigModel)
        {
            try
            {
                bool response = VehiConfigCreateVehicleConfiguration.Instance.SaveVehicleConfiguration(saveConfigModel.ConfigurationId, saveConfigModel.UserSchema, saveConfigModel.ApplicationRevisionId, saveConfigModel.IsNotif, saveConfigModel.IsVR1);
                return Content(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  InsertVehicleRegistration
        /// <summary>
        /// Creating a new vehicle registration.
        /// </summary>
        /// <param name=vehicleregistrationinputparams>VehicleRegistrationInputParams model class is used to input parameters</param>       
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/InsertVehicleRegistration")]
        public IHttpActionResult InsertVehicleRegistration(VehicleRegistrationInputParams vehicleRegistrationInputParams)
        {
            try
            {
                int registationId = VehiConfigCreateRegistrationProvider.Instance.InsertVehicleRegistration(vehicleRegistrationInputParams.VehicleId, vehicleRegistrationInputParams.RegistrationId, vehicleRegistrationInputParams.FleetId);
                return Content(HttpStatusCode.Created, registationId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertVehicleRegistration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region InsertVR1VehicleConfiguration
        /// <summary>
        /// InsertVR1VehicleConfiguration
        /// </summary>
        /// <param name="configurationModel">NewConfigurationModel modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/InsertVR1VehicleConfiguration")]
        public IHttpActionResult InsertVR1VehicleConfiguration(NewConfigurationModel configurationModel, string userSchema)
        {
            try
            {
                NewConfigurationModel objresponse = VehiConfigCreateVehicleConfiguration.Instance.InsertVR1VehicleConfiguration(configurationModel, userSchema);
                return Content(HttpStatusCode.Created, objresponse);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertVR1VehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region InsertApplVehicleConfiguration
        /// <summary>
        /// InsertApplVehicleConfiguration
        /// </summary>
        /// <param name="configurationModel">NewConfigurationModel modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("VehicleConfig/InsertApplVehicleConfiguration")]
        public IHttpActionResult InsertApplVehicleConfiguration(NewConfigurationModel configurationModel, string userSchema)
        {
            try
            {
                double vehicleconfigid = VehiConfigCreateVehicleConfiguration.Instance.InsertApplicationVehicleConfiguration(configurationModel, userSchema);
                return Content(HttpStatusCode.Created, vehicleconfigid);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertApplVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ImportVehicleFromList
        /// <summary>
        /// ImportVehicleFromList
        /// </summary>
        /// <param name="inputparams">ImportVehicleListModel modal Class is used to input params</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("VehicleConfig/ImportVehicleFromList")]
        public IHttpActionResult ImportVehicleFromList(ImportVehicleListModel inputParams)
        {
            try
            {
                long vehicleId = VehiConfigCreateVehicleConfiguration.Instance.ImportVehicleFromList(inputParams);
                return Content(HttpStatusCode.OK, vehicleId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ImportVehicleFromList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CopyVehicleFromList
        /// <summary>
        /// CopyVehicleFromList
        /// </summary>
        /// <param name="inputparams">ImportVehicleListModel modal Class is used to input params</param>       
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/CopyVehicleFromList")]
        public IHttpActionResult CopyVehicleFromList(ImportVehicleListModel inputParams)
        {
            try
            {
                long vehicleId = VehiConfigCreateVehicleConfiguration.Instance.CopyVehicleFromList(inputParams);
                return Content(HttpStatusCode.OK, vehicleId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ImportVehicleFromList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region VehiConfigGetRegistrationProvider --Provider
        #region SaveVR1VehicleRegistrationId
        /// <summary>
        ///To Save  VR1VehicleRegistration
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="registrationValue"></param>
        /// <param name="fleetId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/SaveVR1VehicleRegistrationId")]
        public IHttpActionResult SaveVR1VehicleRegistrationId(VehicleRegistrationInputParams vehicleregistrationInputParams)
        {
            try
            {
                int vehicleId = VehiConfigCreateRegistrationProvider.Instance.SaveVR1VehicleRegistrationId(vehicleregistrationInputParams.VehicleId, vehicleregistrationInputParams.RegistrationId, vehicleregistrationInputParams.FleetId, vehicleregistrationInputParams.UserSchema);
                return Content(HttpStatusCode.OK, vehicleId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveVR1VehicleRegistrationId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SaveAppVehicleRegistrationId
        /// <summary>
        ///To Save  SaveAppVehicleRegistrationId
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="registrationValue"></param>
        /// <param name="fleetId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/SaveApplVehicleRegistrationId")]
        public IHttpActionResult SaveApplVehicleRegistrationId(VehicleRegistrationInputParams vehicleregistrationInputParams)
        {
            try
            {
                int vehicleId = VehiConfigCreateRegistrationProvider.Instance.SaveAppVehicleRegistrationId(vehicleregistrationInputParams.VehicleId, vehicleregistrationInputParams.RegistrationId, vehicleregistrationInputParams.FleetId, vehicleregistrationInputParams.UserSchema);
                return Content(HttpStatusCode.OK, vehicleId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveApplVehicleRegistrationId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetVehicleRegistrationDetails
        /// <summary>
        ///To get vehicle registration details.
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema </param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetVehicleRegistrationDetails")]
        public IHttpActionResult GetVehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = VehiConfigGetRegistrationProvider.Instance.GetVehicleRegistrationDetails(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVehicleRegistrationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetVR1VehicleRegistrationDetails
        /// <summary>
        ///  function to obtain the VR1vehicle  registration details
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>        
        [HttpGet]
        [Route("VehicleConfig/GetVR1VehicleRegistrationDetails")]
        public IHttpActionResult GetVR1VehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = VehiConfigGetRegistrationProvider.Instance.GetVR1VehicleRegistrationDetails(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVR1VehicleRegistrationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetApplVehicleRegistrationDetails
        /// <summary>
        /// Get AppVehicle Registration Details
        /// </summary>GetApplVehicleRegistrationDetails
        /// <param name="vhclId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>        
        [HttpGet]
        [Route("VehicleConfig/GetApplVehicleRegistrationDetails")]
        public IHttpActionResult GetApplVehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = VehiConfigGetRegistrationProvider.Instance.GetApplVehicleRegistrationDetails(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetApplVehicleRegistrationDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetVR1VehicleConfigVhclID
        /// <summary>
        /// Listing the vehicle components in a configuration for VR1 application
        /// </summary>
        /// <param name="vhclID">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetVR1VehicleConfigVhclID")]
        public IHttpActionResult GetVR1VehicleConfigVhclID(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleConfigList> vehicleConfigLists = VehiConfigGetVehicleConfig.Instance.GetVR1VehicleConfigVhclID(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVR1VehicleConfigVhclID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAllVR1ComponentByOrganisationId
        /// <summary>
        ///To Get All VR1Component byOrganisationId
        /// </summary>  
        /// <param name="componentId">Input componentId</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetAllVR1ComponentByOrganisationId")]
        public IHttpActionResult GetAllVR1ComponentByOrganisationId(long componentId, string userSchema)
        {
            try
            {
                List<ComponentGridList> componentGridLists = VehiConfigGetComponentByOrgId.Instance.GetAllVR1ComponentByOrganisationId(componentId, userSchema);
                return Content(HttpStatusCode.OK, componentGridLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetAllVR1ComponentByOrganisationId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAppVehicleConfigVhclID
        /// <summary>
        /// GetAppVehicleConfigVhclID
        /// </summary>
        /// <param name="vhclID">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema as portal</param>
        /// <returns></returns>        
        [HttpGet]
        [Route("VehicleConfig/GetApplVehicleConfigVhclID")]
        public IHttpActionResult GetApplVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleConfigList> vehicleConfigList = VehiConfigGetVehicleConfig.Instance.GetAppVehicleConfigVhclID(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetApplVehicleConfigVhclID ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetVehicleConfigVhclID
        /// <summary>
        /// GetVehicleConfigVhclID
        /// </summary>
        /// <param name="vhclID">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema as portal</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetVehicleConfigVhclID")]
        public IHttpActionResult GetVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleConfigList> vehicleConfigList = VehiConfigGetVehicleConfig.Instance.GetVehicleConfigVhclID(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVehicleConfigVhclID ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region RouteVehicle--Provider
        #region GetRouteConfigInfo
        /// <summary>
        /// To get the route configuration details
        /// </summary>
        /// <param name="componentId">Input componentId</param>
        /// <param name="userSchema">Input userschema</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetRouteConfigInfo")]
        public IHttpActionResult GetRouteConfigInfo(int componentId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = RouteVehicle.Instance.GetRouteConfigInfo(componentId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetRouteConfigInfo ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetRouteConfigInfoForVR1
        /// <summary>
        /// To get the route configuration details for VR1.
        /// </summary>
        /// <param name="componentId">Input componentId</param>
        /// <param name="userSchema">Input userschema</param>
        /// <returns></returns>        
        [HttpGet]
        [Route("VehicleConfig/GetRouteConfigInfoForVR1")]
        public IHttpActionResult GetRouteConfigInfoForVR1(int componentId, string userSchema, int isEdit = 0)
        {
            try
            {
                ConfigurationModel configuration = RouteVehicle.Instance.GetRouteConfigInfoForVR1(componentId, userSchema, isEdit);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetRouteConfigInfoForVR1 ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetRouteVehicleConfigVhclID
        /// <summary>
        ///To get the route configuration details by VehicleID
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetRouteVehicleConfigVhclID")]
        public IHttpActionResult GetRouteVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleConfigList> vehicleConfigList = RouteVehicle.Instance.GetRouteVehicleConfigVhclID(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetRouteVehicleConfigVhclID ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetRouteVehicleRegistrationDetails
        /// <summary>
        ///  To get route of the vehicle configuration by registration details
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetRouteVehicleRegistrationDetails")]
        public IHttpActionResult GetRouteVehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = RouteVehicle.Instance.GetRouteVehicleRegistrationDetails(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetRouteVehicleRegistrationDetails ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetNotifVehicleConfiguration
        /// <summary>
        ///  To get the vehicle configuration notification
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="isSimple">Input isSimple</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetNotifVehicleConfiguration")]
        public IHttpActionResult GetNotifVehicleConfiguration(int vehicleId, int isSimple)
        {
            try
            {
                ConfigurationModel configuration = RouteVehicle.Instance.GetNotifVehicleConfig(vehicleId, isSimple);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetNotifVehicleConfiguration ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CheckValidVehicleCreated
        /// <summary>
        /// CheckValidVehicleCreated
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>      
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/CheckValidVehicleCreated")]
        public IHttpActionResult CheckValidVehicleCreated(int vehicleId)
        {
            try
            {
                long result = RouteVehicle.Instance.CheckValidVehicleCreated(vehicleId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckValidVehicleCreated ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CheckWheelWithSumOfAxel
        /// <summary>
        ///  To get the vehicle CheckWheelWithSumOfAxel
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/CheckWheelWithSumOfAxel")]
        public IHttpActionResult CheckWheelWithSumOfAxel(CheckWheelParams wheelParams)
        {
            try
            {
                bool response = RouteVehicle.Instance.CheckWheelWithSumOfAxel(wheelParams.VehicleId, wheelParams.UserSchema, wheelParams.ApplicationRevisionId, wheelParams.IsNotif, wheelParams.IsVR1);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckWheelWithSumOfAxel ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region VehiConfigGetComponentByOrgId--Provider
        #region GetAllAppComponentByOrganisationId
        /// <summary>
        /// function to select all app component by component Id
        /// </summary>        
        ///  <param name="componentId">componentId</param>
        /// <param name="userSchema">Input userSchema</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetAllApplComponentByOrganisationId")]
        public IHttpActionResult GetAllApplComponentByOrganisationId(long componentId, string userSchema)
        {
            try
            {
                List<ComponentGridList> componentGridLists = VehiConfigGetComponentByOrgId.Instance.GetAllAppComponentByOrganisationId(componentId, userSchema);
                return Content(HttpStatusCode.OK, componentGridLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetAllApplComponentByOrganisationId ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAllComponentByOrganisationId
        /// <summary>
        /// To select all component by organisation id
        /// </summary>        
        ///  <param name="organisationId">organisationId</param>
        /// <param name="componentId">Input componentId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetAllComponentByOrganisationId")]
        public IHttpActionResult GetAllComponentByOrganisationId(int organisationId, long componentId)
        {
            try
            {
                List<ComponentGridList> componentGridLists = VehiConfigGetComponentByOrgId.Instance.GetAllComponentByOrganisationId(organisationId, componentId);
                return Content(HttpStatusCode.OK, componentGridLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetAllComponentByOrganisationId ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetComponentByOrganisationId
        /// <summary>
        ///GetComponentByOrganisationId
        /// </summary>        
        ///  <param name="organisationId">organisationId</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns> 
        [HttpGet]
        [Route("VehicleConfig/GetComponentByOrganisationId")]
        public IHttpActionResult GetComponentByOrganisationId(int organisationId, int pageNumber, int pageSize, string componentName, string componentType, string vehicleIntent, int filterFavourites, string userSchema,int presetFilter,int? sortOrder=null)
        {
            try
            {
                List<ComponentGridList> componentGridLists = VehiConfigGetComponentByOrgId.Instance.GetComponentByOrganisationId(organisationId, pageNumber, pageSize, componentName, componentType, vehicleIntent, filterFavourites, userSchema,presetFilter,sortOrder);
                return Ok(componentGridLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleComponent/GetComponentByOrganisationId, Exception: {ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion        
        #region GetListComponentId
        /// <summary>
        /// for getting component list 
        /// </summary>        
        ///  <param name="vehicleId">Input Vehicle Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("VehicleConfig/GetListComponentId")]
        public IHttpActionResult GetListComponentId(int vehicleId)
        {
            try
            {
                List<ComponentIdModel> componentList = VehiConfigGetComponentByOrgId.Instance.GetListComponentId(vehicleId);
                return Content(HttpStatusCode.OK, componentList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetListComponentId ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetVR1ListComponentId
        /// <summary>
        /// for getting GetVR1ListComponentId
        /// </summary>        
        ///  <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("VehicleConfig/GetVR1ListComponentId")]
        public IHttpActionResult GetVR1ListComponentId(int vehicleId, string userSchema)
        {
            try
            {
                List<ComponentIdModel> componentList = VehiConfigGetComponentByOrgId.Instance.GetVR1ListComponentId(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, componentList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVR1ListComponentId ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetAppListComponentId
        /// <summary>
        ///To get the list of App Components.
        /// </summary>            
        ///  <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("VehicleConfig/GetApplListComponentId")]
        public IHttpActionResult GetApplListComponentId(int vehicleId, string userSchema)
        {
            try
            {
                List<ComponentIdModel> componentList = VehiConfigGetComponentByOrgId.Instance.GetVR1ListComponentId(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, componentList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetApplListComponentId ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region VehiConfigGetConfigByOrgId--Provider
        #region GetConfigByOrganisationId
        /// <summary>
        /// GetConfigByOrganisationId
        /// </summary>
        /// <param name="organisationId">Input organisationId</param>
        /// <param name="movtype">Input movtype</param>
        /// <param name="movetype1">Input movetype1</param>
        /// <param name="userSchema">Input userSchema</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/GetConfigByOrganisationId")]
        public IHttpActionResult GetConfigByOrganisationId(ConfigOrgIDParams inputParams)
        {
            try
            {
                List<VehicleConfigurationGridList> vehicleConfigurationGridLists = VehiConfigGetConfigByOrgId.Instance.GetConfigByOrganisationId(inputParams.OrganisationId, inputParams.MovementType, inputParams.MovementType1, inputParams.UserSchema, inputParams.FilterFavouritesVehConfig,inputParams.presetFilter,inputParams.sortOrder);
                return Content(HttpStatusCode.OK, vehicleConfigurationGridLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetConfigByOrganisationId ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CheckName--Provider
        #region CheckNotifValidVehicle
        /// <summary>
        ///  To check vehicle validity for import
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>      
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/CheckNotifValidVehicle")]
        public IHttpActionResult CheckNotifValidVehicle(int vehicleId)
        {
            try
            {
                bool response = RouteVehicle.Instance.CheckNotifValidVehicle(vehicleId);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckNotifValidVehicle ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CheckFormalName
        /// <summary>
        ///  For  Component Formal Name check
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/CheckFormalName")]
        public IHttpActionResult CheckFormalName(int componentId, int organisationId)
        {
            try
            {
                int result = CheckFormalNameExists.Instance.CheckFormalName(componentId, organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckFormalName ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region CheckVR1FormalName
        /// <summary>
        ///   For VR1 Component Formal Name check
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="organisationId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/CheckVR1FormalName")]
        public IHttpActionResult CheckVR1FormalName(int componentId, int organisationId, string userSchema)
        {
            try
            {
                int result = CheckFormalNameExists.Instance.CheckVR1FormalName(componentId, organisationId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckVR1FormalName ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region CheckAppFormalName
        /// <summary>
        ///  For Application Component Formal Name check
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="organisationId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/CheckApp1FormalName")]
        public IHttpActionResult CheckApp1FormalName(int componentId, int organisationId, string userSchema)
        {
            try
            {
                int result = CheckFormalNameExists.Instance.CheckAppFormalName(componentId, organisationId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckApp1FormalName ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #endregion
        #region DeleteRegister--Provider
        #region DeleteVR1RegConfiguration
        /// <summary>
        ///  DeleteVR1RegConfiguration
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="IdNumber"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("VehicleConfig/DeleteVR1RegConfiguration")]
        public IHttpActionResult DeleteVR1RegConfiguration(int vehicleId, int IdNumber, string userSchema)
        {
            try
            {
                int response = DeleteRegister.Instance.DeleteVR1RegConfig(vehicleId, IdNumber, userSchema);
                if (response == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteVR1RegConfiguration ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DisableVehicle
        /// <summary>
        ///  Vehicle disable
        /// </summary>
        /// <param name="vehicleId"></param>
        [HttpDelete]
        [Route("VehicleConfig/Disablevehicle")]
        public IHttpActionResult DisableVehicle(int vehicleId)
        {
            try
            {
                int response = DeleteRegister.Instance.DisableVehicle(vehicleId);
                if (response == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DisableVehicle ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteVehicleConfigPosition
        /// <summary>
        ///DeleteVehicleConfigPosition
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="latpos"></param>
        /// <param name="longpos"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("VehicleConfig/DeleteVehicleConfigPosition")]
        public IHttpActionResult DeleteVehicleConfigPosition(int vehicleId, int latpos, int longpos)
        {
            try
            {
                int result = DeleteRegister.Instance.DeleteVehicleConfigPosn(vehicleId, latpos, longpos);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteVehicleConfigPosition ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteApplVehicleConfigPosition
        /// <summary>
        ///  DeleteApplVehicleConfigPosition
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="latpos"></param>
        /// <param name="longpos"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("VehicleConfig/DeleteApplVehicleConfigPosition")]
        public IHttpActionResult DeleteApplVehicleConfigPosition(int vehicleId, int latpos, int longpos, string userSchema)
        {
            try
            {
                int result = DeleteRegister.Instance.DeleteApplicationVehicleConfigPosn(vehicleId, latpos, longpos, userSchema);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteApplVehicleConfigPosition ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteVR1VehicleConfigPosition
        /// <summary>
        ///  DeleteVR1VehicleConfigPosition
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="latpos"></param>
        /// <param name="longpos"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("VehicleConfig/DeleteVR1VehicleConfigPosition")]
        public IHttpActionResult DeleteVR1VehicleConfigPosition(int vehicleId, int latpos, int longpos, string userSchema)
        {
            try
            {
                int result = DeleteRegister.Instance.DeleteVR1VehicleConfigPosn(vehicleId, latpos, longpos, userSchema);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteVR1VehicleConfigPosition ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeletVehicleRegisterConfiguration
        /// <summary>
        ///  DeletVehicleRegisterConfiguration
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="IdNumber"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("VehicleConfig/DeletVehicleRegisterConfiguration")]
        public IHttpActionResult DeletVehicleRegisterConfiguration(int vehicleId, int IdNumber, bool isMovement = false)
        {
            try
            {
                int response = DeleteRegister.Instance.DeletVehicleRegisterConfiguration(vehicleId, IdNumber, isMovement);
                if (response == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeletVehicleRegisterConfiguration ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeleteAppRegConfiguration
        /// <summary>
        ///  DeleteAppRegConfiguration
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="IdNumber"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("VehicleConfig/DeleteApplRegConfiguration")]
        public IHttpActionResult DeleteApplRegConfiguration(int vehicleId, int IdNumber, string userSchema)
        {
            try
            {
                int response = DeleteRegister.Instance.DeleteAppRegConfig(vehicleId, IdNumber, userSchema);
                if (response == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteApplRegConfiguration ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region VehiConfigGetVehicleConfig--Provider
        #region GetNotifVehicleConfigByID
        /// <summary>
        /// GetNotifVehicleConfigByID
        /// </summary>
        /// <param name="VehicleId">Input VehicleId</param>        
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetNotifVehicleConfigByID")]
        public IHttpActionResult GetNotifVehicleConfigByID(int vehicleId)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetNotifVehicleConfigByID(vehicleId);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetNotifVehicleConfigByID ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetConfigurationInfo
        /// <summary>
        /// Get vehicle configuration information
        /// </summary>
        /// <param name="componentId">Input VehicleId</param>
        ///  <param name="userSchema">Input userschema as portal</param>  
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetConfigurationInfo")]
        public IHttpActionResult GetConfigurationInfo(int componentId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetConfigInfo(componentId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetConfigurationInfo ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetConfigInformationApplication
        /// <summary>
        /// To Get the  Application Configuration Information
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleConfig/GetConfigInfoApplication")]
        public IHttpActionResult GetConfigInfoApplication(int vehicleId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetConfigInfoApplication(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetConfigInfoApplication ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetConfigurationInfoVR1
        /// <summary>
        /// To Get the  VR1 Configuration Information
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="schematype"></param>
        [HttpGet]
        [Route("VehicleConfig/GetConfigurationInfoVR1")]
        public IHttpActionResult GetConfigurationInfoVR1(int componentId, string schemaType)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetConfigInfoVR1(componentId, schemaType);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetConfigurationInfoVR1 ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region AddToFleet
        /// <summary>
        /// AddComponentToFleet
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/AddComponentToFleet")]
        public IHttpActionResult AddComponentToFleet(FleetComponenentModel fleetComponenentModel)
        {
            try
            {
                int compId = AddToFleet.Instance.AddComponentToFleet(fleetComponenentModel.ComponentId, fleetComponenentModel.OrganisationId);
                return Content(HttpStatusCode.OK, compId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddComponentToFleet ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// AddApplicationComponentToFleet
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <param name="flag"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/AddApplicationComponentToFleet")]
        public IHttpActionResult AddApplicationComponentToFleet(FleetComponenentModel fleetComponenentModel)
        {
            try
            {
                int compId = AddToFleet.Instance.AddApplicationComponentToFleet(fleetComponenentModel.ComponentId, fleetComponenentModel.OrganisationId, fleetComponenentModel.Flag, fleetComponenentModel.UserSchema);
                return Content(HttpStatusCode.OK, compId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddApplicationComponentToFleet ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        /// <summary>
        /// AddVR1ComponentToFleet
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <param name="flag"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/AddVR1ComponentToFleet")]
        public IHttpActionResult AddVR1ComponentToFleet(FleetComponenentModel fleetComponenentModel)
        {
            try
            {
                int compId = AddToFleet.Instance.AddVR1ComponentToFleet(fleetComponenentModel.ComponentId, fleetComponenentModel.OrganisationId, fleetComponenentModel.Flag, fleetComponenentModel.UserSchema);
                return Content(HttpStatusCode.OK, compId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddVR1ComponentToFleet ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Vehicle Managemnet in Application
        #region  To import vehicle from fleet into a SO application
        [HttpPost]
        [Route("VehicleConfig/SaveSOApplicationvehicleconfig")]
        public IHttpActionResult SaveSOApplicationvehicleconfig(ImportVehicleParams importVehicleParams)
        {
            long result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.SaveSOApplicationvehicleconfig(importVehicleParams.VehicleId, importVehicleParams.ApplicationRevisionId, importVehicleParams.RoutePartId, importVehicleParams.UserSchema);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveSOApplicationvehicleconfig ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  To import vehicle from fleet into a VR1 application
        [HttpPost]
        [Route("VehicleConfig/SaveVR1Applicationvehicleconfig")]
        public IHttpActionResult SaveVR1Applicationvehicleconfig(NewConfigurationModel configurationModel)
        {
            try
            {
                NewConfigurationModel config = ApplicationVehicleProvider.Instance.SaveVR1Applicationvehicleconfig(configurationModel);
                return Content(HttpStatusCode.OK, config);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveVR1Applicationvehicleconfig ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Applicationvehiclelist
        [HttpGet]
        [Route("VehicleConfig/Applicationvehiclelist")]
        public IHttpActionResult Applicationvehiclelist(int partId, int flagSOAppVeh, string routeType, string userSchema)
        {
            List<VehicleDetailSummary> objComponentModelList = new List<VehicleDetailSummary>();
            try
            {
                objComponentModelList = ApplicationVehicleProvider.Instance.Applicationvehiclelist(partId, flagSOAppVeh, routeType, userSchema);
                return Content(HttpStatusCode.OK, objComponentModelList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/Applicationvehiclelist,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Import vehicle from pre movement in application
        [HttpPost]
        [Route("VehicleConfig/ImportApplnVehicleFromPreMove")]
        public IHttpActionResult ImportApplnVehicleFromPreMove(ImportVehicleParams importVehicleParams)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.PrevMove_ImportApplVeh(importVehicleParams.VehicleId, importVehicleParams.ApplicationRevisionId, importVehicleParams.RoutePartId, importVehicleParams.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ImportApplnVehicleFromPreMove,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  ImportRouteVehicleToAppVehicle
        [HttpPost]
        [Route("VehicleConfig/ImportRouteVehicleToAppVehicle")]
        public IHttpActionResult ImportRouteVehicleToAppVehicle(ImportVehicleParams importVehicleParams)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.ImportRouteVehicleToAppVehicle(importVehicleParams.VehicleId, importVehicleParams.ApplicationRevisionId, importVehicleParams.RoutePartId, importVehicleParams.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ImportRouteVehicleToAppVehicle,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  VR1 Application vehicle movemnt list
        [HttpGet]
        [Route("VehicleConfig/VR1AppVehicleMovementList")]
        public IHttpActionResult VR1AppVehicleMovementList(int vehicleId, int apprevisionId, int routepartId, string userSchema)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.VR1AppVehicle_MovementList(vehicleId, apprevisionId, routepartId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/VR1AppVehicleMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  SO Application vehicle movemnt list
        [HttpGet]
        [Route("VehicleConfig/AppVehicleMovementList")]
        public IHttpActionResult AppVehicleMovementList(int vehicleId, int apprevisionId, int routepartId, string userSchema)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.AppVehicle_MovementList(vehicleId, apprevisionId, routepartId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AppVehicleMovementList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete selected vehicleComponent from so application
        [HttpDelete]
        [Route("VehicleConfig/DeleteSelectedVehicleComponent")]
        public IHttpActionResult DeleteSelectedVehicleComponent(int vehicleId, string userSchema)
        {
            try
            {
                int result = ApplicationVehicleProvider.Instance.DeleteSelectedVehicleComponent(vehicleId, userSchema);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteSelectedVehicleComponent,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Delete selected vehicleComponent from vr1 application
        [HttpDelete]
        [Route("VehicleConfig/DeleteSelectedVR1VehicleComponent")]
        public IHttpActionResult DeleteSelectedVR1VehicleComponent(int vehicleId, string userSchema)
        {
            try
            {
                int result = ApplicationVehicleProvider.Instance.DeleteSelectedVR1VehicleComponent(vehicleId, userSchema);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteSelectedVR1VehicleComponent,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ViewVehicleSummaryByID
        [HttpGet]
        [Route("VehicleConfig/ViewVehicleSummaryByID")]
        public IHttpActionResult ViewVehicleSummaryByID(long rPartId, int vr1, string userSchema)
        {
            List<VehicleDetailSummary> vehicleDetailSummaries = new List<VehicleDetailSummary>();
            try
            {
                vehicleDetailSummaries = ApplicationVehicleProvider.Instance.ViewVehicleSummaryByID(rPartId, vr1, userSchema);
                return Content(HttpStatusCode.OK, vehicleDetailSummaries);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ViewVehicleSummaryByID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  CheckFormalNameInApplicationVeh
        [HttpGet]
        [Route("VehicleConfig/CheckFormalNameInApplicationVeh")]
        public IHttpActionResult CheckFormalNameInApplicationVeh(string vehicleName, int organisationId, string userSchema)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.CheckFormalNameInApplicationVeh(vehicleName, organisationId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckFormalNameInApplicationVeh,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Add Vehicle To Fleet
        [HttpGet]
        [Route("VehicleConfig/AddVehicleToFleet")]
        public IHttpActionResult AddVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.AddVehicleToFleet(vehicleId, organisationId, flag, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddVehicleToFleet,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Add VR1 Vehicle To Fleet
        [HttpGet]
        [Route("VehicleConfig/AddVR1VhclToFleet")]
        public IHttpActionResult AddVR1VhclToFleet(int vehicleId, int organisationId, int flag)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.AddVR1VhclToFleet(vehicleId, organisationId, flag);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddVR1VhclToFleet,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Get the list of application vehicle list
        [HttpGet]
        [Route("VehicleConfig/AppVehicleConfigList")]
        public IHttpActionResult AppVehicleConfigList(long apprevisionId, string userSchema)
        {
            try
            {
                List<AppVehicleConfigList> appVehicleConfigLists = ApplicationVehicleProvider.Instance.AppVehicleConfigList(apprevisionId, userSchema);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AppVehicleConfigList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Get the list of VR1 application vehicle list
        [HttpGet]
        [Route("VehicleConfig/AppVehicleConfigListVR1")]
        public IHttpActionResult AppVehicleConfigListVR1(long routePartId, long versionId, string contentRefNo, string userSchema)
        {
            try
            {
                List<AppVehicleConfigList> appVehicleConfigLists = ApplicationVehicleProvider.Instance.AppVehicleConfigListVR1(routePartId, versionId, contentRefNo, userSchema);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AppVehicleConfigListVR1,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Get the list of NEN vehicle list
        [HttpGet]
        [Route("VehicleConfig/GetNenVehicleList")]
        public IHttpActionResult GetNenVehicleList(long routePartId)
        {
            List<AppVehicleConfigList> appVehicleConfigLists = new List<AppVehicleConfigList>();
            try
            {
                appVehicleConfigLists = ApplicationVehicleProvider.Instance.GetNenVehicleList(routePartId);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetNenVehicleList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ListVehCompDetails
        [HttpGet]
        [Route("VehicleConfig/ListVehCompDetails")]
        public IHttpActionResult ListVehCompDetails(int revisionId, string userschema)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                appVehicleConfigLists = ApplicationVehicleProvider.Instance.ListVehCompDetails(revisionId, userschema);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListVehCompDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ListLengthVehCompDetails
        [HttpGet]
        [Route("VehicleConfig/ListLengthVehCompDetails")]
        public IHttpActionResult ListLengthVehCompDetails(int revisionId, int vehicleId, string userschema)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                appVehicleConfigLists = ApplicationVehicleProvider.Instance.ListLengthVehCompDetails(revisionId, vehicleId, userschema);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListLengthVehCompDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Checking vehicle weight against axle weight validation
        [HttpGet]
        [Route("VehicleConfig/ListVehWeightDetails")]
        public IHttpActionResult ListVehWeightDetails(int revisionId, int vehicleId, string userschema, int isVR1)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                appVehicleConfigLists = ApplicationVehicleProvider.Instance.ListVehWeightDetails(revisionId, vehicleId, userschema, isVR1);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListVehWeightDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ListVR1VehCompDetails
        [HttpGet]
        [Route("VehicleConfig/ListVR1VehCompDetails")]
        public IHttpActionResult ListVR1VehCompDetails(int versionId, string contentref)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                appVehicleConfigLists = ApplicationVehicleProvider.Instance.ListVR1VehCompDetails(versionId, contentref);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListVR1VehCompDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ListLengthVR1VehDetails
        [HttpGet]
        [Route("VehicleConfig/ListLengthVR1VehDetails")]
        public IHttpActionResult ListLengthVR1VehDetails(int versionId, string contentref)
        {
            List<ApplVehiclComponents> appVehicleConfigLists = new List<ApplVehiclComponents>();
            try
            {
                appVehicleConfigLists = ApplicationVehicleProvider.Instance.ListLengthVR1VehDetails(versionId, contentref);
                return Content(HttpStatusCode.OK, appVehicleConfigLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListLengthVR1VehDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region Vehicle Managemnet in Notification
        #region ImportFleetRouteVehicle
        [HttpPost]
        [Route("VehicleConfig/ImportFleetRouteVehicle")]
        public IHttpActionResult ImportFleetRouteVehicle(ImportVehicleParams importVehicleParams)
        {
            ListRouteVehicleId listRouteVehicleId = new ListRouteVehicleId();
            try
            {
                listRouteVehicleId = NotificationVehicleProvider.Instance.ImportFleetRouteVehicle(importVehicleParams.VehicleId, importVehicleParams.ContentRefNo, importVehicleParams.Simple, importVehicleParams.RoutePartId);
                return Content(HttpStatusCode.OK, listRouteVehicleId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" -VehicleConfig/ImportFleetRouteVehicle,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ImportFleetRouteVehicle
        [HttpGet]
        [Route("VehicleConfig/ImportReturnRouteVehicle")]
        public IHttpActionResult ImportReturnRouteVehicle(int routePartId, string contentRefNo)
        {
            long routePartID = 0;
            try
            {
                routePartID = NotificationVehicleProvider.Instance.ImportReturnRouteVehicle(routePartId, contentRefNo);

                return Content(HttpStatusCode.OK, routePartID);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"VehicleConfigDao/ImportReturnRouteVehicle, Exception: {ex}​​​​​​​");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ImportFleetRouteVehicle
        [HttpPost]
        [Route("VehicleConfig/UpdateNotifRouteVehicle")]
        public IHttpActionResult UpdateNotifRouteVehicle(UpdateNotifRouteVehicleParams updateNotifRouteVehicleParams)
        {
            try
            {
                int status = NotificationVehicleProvider.Instance.UpdateNotifRouteVehicle(updateNotifRouteVehicleParams.NotificationGeneralDetails, updateNotifRouteVehicleParams.RoutePartId, updateNotifRouteVehicleParams.VehicleUnits);
                return Content(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/UpdateNotifRouteVehicle, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ImportFleetRouteVehicle
        [HttpPost]
        [Route("VehicleConfig/CreateNotifVehicleConfiguration")]
        public IHttpActionResult CreateNotifVehicleConfiguration(CreateNotifVehicleConfigParams createNotifVehicleConfigParams)
        {
            double VehicleId = 0;
            try
            {
                VehicleId = NotificationVehicleProvider.Instance.InsertRouteVehicleConfiguration(createNotifVehicleConfigParams.NewConfigurationModel, createNotifVehicleConfigParams.ContentRefNo, createNotifVehicleConfigParams.IsNotif);
                return Content(HttpStatusCode.OK, VehicleId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CreateNotifVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SaveNotifVehicleRegistrationId
        [HttpPost]
        [Route("VehicleConfig/SaveNotifVehicleRegistrationId")]
        public IHttpActionResult SaveNotifVehicleRegistrationId(VehicleRegistrationInputParams registrationInputParams)
        {
            int result = 0;
            try
            {
                result = NotificationVehicleProvider.Instance.SaveNotifVehicleRegistrationId(registrationInputParams.VehicleId, registrationInputParams.RegistrationId, registrationInputParams.FleetId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveNotifVehicleRegistrationId,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SaveNotifVehicleConfiguration
        [HttpGet]
        [Route("VehicleConfig/SaveNotifVehicleConfiguration")]
        public IHttpActionResult SaveNotifVehicleConfiguration(int vehicleId, int componentId, int componentType)
        {
            int result = 0;
            try
            {
                result = NotificationVehicleProvider.Instance.SaveNotifVehicleConfiguration(vehicleId, componentId, componentType);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveNotifVehicleConfiguration,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region SaveNotifAxel
        [HttpPost]
        [Route("VehicleConfig/SaveNotifAxel")]
        public IHttpActionResult SaveNotifAxel(AxleDetails axle)
        {
            bool result = false;
            try
            {
                result = NotificationVehicleProvider.Instance.SaveNotifAxel(axle);

                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveNotifAxel,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region UpdateMaxAxleWeight
        [HttpPost]
        [Route("VehicleConfig/UpdateMaxAxleWeight")]
        public IHttpActionResult UpdateMaxAxleWeight(long vehicleId)
        {
            try
            {
                bool result = NotificationVehicleProvider.Instance.UpdateMaxAxleWeight(vehicleId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/UpdateMaxAxleWeight, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ListCloneAxelDetails
        [HttpGet]
        [Route("VehicleConfig/ListCloneAxelDetails")]
        public IHttpActionResult ListCloneAxelDetails(int VehicleId)
        {
            List<AxleDetails> axelDetails = new List<AxleDetails>();
            try
            {
                axelDetails = NotificationVehicleProvider.Instance.ListCloneAxelDetails(VehicleId);
                return Content(HttpStatusCode.OK, axelDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListCloneAxelDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region UpdateNewAxleDetails
        [HttpPost]
        [Route("VehicleConfig/UpdateNewAxleDetails")]
        public IHttpActionResult UpdateNewAxleDetails(AxleDetails axle)
        {
            bool result = false;
            try
            {
                result = NotificationVehicleProvider.Instance.UpdateNewAxleDetails(axle);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/UpdateNewAxleDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region GetNotificationVehicle
        [HttpGet]
        [Route("VehicleConfig/GetNotificationVehicle")]
        public IHttpActionResult GetNotificationVehicle(long partId)
        {
            List<VehicleDetailSummary> vehicleDetailSummaries = new List<VehicleDetailSummary>();
            try
            {
                vehicleDetailSummaries = NotificationVehicleProvider.Instance.GetNotificationVehicle(partId);
                return Content(HttpStatusCode.OK, vehicleDetailSummaries);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetNotificationVehicle,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region ImportRouteVehicle
        [HttpGet]
        [Route("VehicleConfig/ImportRouteVehicle")]
        public IHttpActionResult ImportRouteVehicle(int routePartId, int vehicleId, string contentRefNo, int simple)
        {
            long rPartId = 0;
            try
            {
                rPartId = NotificationVehicleProvider.Instance.ImportRouteVehicle(routePartId, vehicleId, contentRefNo, simple);
                return Content(HttpStatusCode.OK, rPartId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ImportRouteVehicle,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking vehicle registraion validation
        [HttpGet]
        [Route("VehicleConfig/ListVehRegDetails")]
        public IHttpActionResult ListVehRegDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleRegistration> objlistveh = NotificationVehicleProvider.Instance.ListVehRegDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListVehRegDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking vehicle import validation
        [HttpGet]
        [Route("VehicleConfig/ListVehicleImportDetails")]
        public IHttpActionResult ListVehicleImportDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleImport> objlistveh = NotificationVehicleProvider.Instance.ListVehicleImportDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListVehicleImportDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking vehicle weight against axle weight validation
        [HttpGet]
        [Route("VehicleConfig/ListNotiVehWeightDetails")]
        public IHttpActionResult ListNotiVehWeightDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleWeight> objlistveh = NotificationVehicleProvider.Instance.ListNotiVehWeightDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListNotiVehWeightDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking vehicle length validation
        [HttpGet]
        [Route("VehicleConfig/ListVehicleLengthDetails")]
        public IHttpActionResult ListVehicleLengthDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleImport> objlistveh = NotificationVehicleProvider.Instance.ListVehicleLengthDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListVehLenDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking vehicle gross weight validation based on vehicle category
        [HttpGet]
        [Route("VehicleConfig/ListVehicleGrossWeightDetails")]
        public IHttpActionResult ListVehicleGrossWeightDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleImport> objlistveh = NotificationVehicleProvider.Instance.ListVehicleGrossWeightDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListVehicleGrossWeightDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking vehicle width validation based on vehicle category
        [HttpGet]
        [Route("VehicleConfig/ListVehicleWidthDetails")]
        public IHttpActionResult ListVehicleWidthDetails(string contentReferenceNo, int reqVR1)
        {
            try
            {
                List<NotifVehicleImport> objlistveh = NotificationVehicleProvider.Instance.ListVehicleWidthDetails(contentReferenceNo, reqVR1);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ListVehicleWidthDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking Axle Weight validation based on vehicle category
        [HttpGet]
        [Route("VehicleConfig/ListVehicleAxleWeightDetails")]
        public IHttpActionResult ListVehicleAxleWeightDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleImport> objlistveh = NotificationVehicleProvider.Instance.ListVehicleAxleWeightDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListVehAxleWeightDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region checking Rigid Length validation based on vehicle category
        [HttpGet]
        [Route("VehicleConfig/ListVehicleRigidLengthDetails")]
        public IHttpActionResult ListVehicleRigidLengthDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifVehicleImport> objlistveh = NotificationVehicleProvider.Instance.ListVehicleRigidLengthDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objlistveh);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/ListVehRigidLengthDetails, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region DeletePrevVehicle
        [HttpDelete]
        [Route("VehicleConfig/DeletePrevVehicle")]
        public IHttpActionResult DeletePrevVehicle(int routePartId)
        {
            try
            {
                int result = NotificationVehicleProvider.Instance.DeletePrevVehicle(routePartId);
                if (result == -1)
                {
                    return Content(HttpStatusCode.OK, StatusMessage.DeletionFailed);
                }
                else
                {
                    return Content(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"VehicleConfig/DeletePrevVehicle, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #endregion
        #region Vehicle New Enhancement
        [HttpGet]
        [Route("VehicleConfig/AutoVehicleConfigType")]
        public IHttpActionResult AutoVehicleConfigType(List<ComponentIdList> componentIds)
        {
            List<AutoAssessingParams> param = new List<AutoAssessingParams>();

            AutoAssessingParams obj = new AutoAssessingParams()
            {
                MovementTypeId = 270002,
                ConfigurationTypeId = 244002
            };
            param.Add(obj);
            return Content(HttpStatusCode.OK, param);
        }
        [HttpGet]
        [Route("VehicleConfig/AutoVehicleMovementType")]
        public IHttpActionResult AutoVehicleMovementType(ConfigurationModel configurationModel)
        {
            bool response = false;
            return Content(HttpStatusCode.OK, response);
        }
        #endregion
        #endregion

        [HttpPost]
        [Route("VehicleConfig/GetSimilarVehicleCombinations")]
        public IHttpActionResult GetSimilarVehicleCombinations(SearchVehicleCombination configDimensions)
        {
            try
            {
                List<VehicleConfigurationGridList> vehicleConfigList = ApplicationVehicleProvider.Instance.GetSimilarVehicleCombinations(configDimensions);
                return Content(HttpStatusCode.OK, vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetSimilarVehicleCombinations ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region AssessConfigurationType 
        [HttpPost]
        [Route("VehicleConfig/AssessConfigurationType")]
        public IHttpActionResult AssessConfigurationType(ComponentIdParams ComponentIdParams)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                string componentIdsString = "";
                foreach (var componentId in ComponentIdParams.componentIds)
                {
                    if (componentIdsString != "")
                    {
                        componentIdsString += "," + componentId;
                    }
                    else
                    {
                        componentIdsString += componentId;
                    }
                }

                //get component list
                List<ComponentModel> componentList = VehiConfigGetVehicleConfig.Instance.GetComponentsInConfiguration(componentIdsString, ComponentIdParams.userSchema, ComponentIdParams.flag);

                //code block for identifying configuration type of the components
                List<uint> vehicleConfig = AssessConfigurationByComponentType(vehicleComponentConfiguration, componentList);

                //code block for filtering configuration type based on movement classification of the components
                //List<uint> vehicleConfigFiltered = AssessConfigurationByMovementClassification(vehicleComponentConfiguration, componentList, vehicleConfig);

                //code block for handling boat mast configuration
                if (!ComponentIdParams.boatMastFlag && vehicleConfig.Count > 0)
                {
                    vehicleConfig.RemoveAll(s => s == (uint)ConfigurationType.BoatMast);
                }

                if (ComponentIdParams.boatMastFlag && vehicleConfig.Count > 0)
                {
                    vehicleConfig.RemoveAll(s => s != (uint)ConfigurationType.BoatMast);
                }

                //code block for handling crane configuration
                if (componentList.Count == 1
                    && !componentList.Any(x => x.ComponentType == (uint)ComponentType.RecoveryVehicle)
                    && componentList.First().ComponentType == (uint)ComponentType.RigidVehicle 
                    && componentList.First().ComponentSubType == (uint)ComponentSubType.MobileCrane)
                {
                    vehicleConfig.RemoveAll(s => s != (uint)ConfigurationType.Crane);
                }

                if (vehicleConfig.Count > 1
                    && !componentList.Any(x => x.ComponentType == (uint)ComponentType.RecoveryVehicle)
                    && componentList.Any(x=>x.ComponentType == (uint)ComponentType.EngineeringPlant)
                    && componentList.Any(x=>x.ComponentSubType == (uint)ComponentSubType.EngPlantTracked))
                {
                    vehicleConfig.RemoveAll(s => s != (uint)ConfigurationType.Tracked);
                }

                if (vehicleConfig.Count > 0
                    && !componentList.Any(x => x.ComponentType == (uint)ComponentType.RecoveryVehicle)){
                    vehicleConfig.RemoveAll(s => s == (uint)ConfigurationType.RecoveryVehicle);
                }

                if (componentList.Count > 2
                    && !componentList.Any(x => x.ComponentType == (uint)ComponentType.RecoveryVehicle)
                    && componentList.Any(x => x.ComponentType == (uint)ComponentType.BallastTractor)
                    && componentList.Any(x => x.ComponentType == (uint)ComponentType.DrawbarTrailer || x.ComponentType == (uint)ComponentType.GirderSet)
                    )
                {
                    vehicleConfig.RemoveAll(s => s != (uint)ConfigurationType.DrawbarTrailer_3_8);
                }
                if (componentList.Count > 2
                    && !componentList.Any(x => x.ComponentType == (uint)ComponentType.RecoveryVehicle)
                    && componentList.Any(x => x.ComponentType == (uint)ComponentType.ConventionalTractor)
                    && componentList.Any(x => x.ComponentType == (uint)ComponentType.SemiTrailer)
                    )
                {
                    vehicleConfig.RemoveAll(s => s != (uint)ConfigurationType.SemiTrailer_3_8);
                }

                return Ok(vehicleConfig);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleConfig/AssessConfigurationType, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        public List<uint> AssessConfigurationByComponentType(VehicleComponentConfiguration vehicleComponentConfiguration, List<ComponentModel> componentList)
        {
            List<uint> vehicleConfig = new List<uint>();

            foreach (var vehicleComponentConfig in vehicleComponentConfiguration.VehicleConfigurationTypes.Configuration)
            {
                List<string> componentTypeList = new List<string>();
                componentTypeList = vehicleComponentConfig.ComponentType.Split(',').ToList();

                List<string> numberofComponents = new List<string>();
                numberofComponents = vehicleComponentConfig.NumberOfComponents.Split('-').ToList();
                if (numberofComponents.Count == 1)
                {
                    numberofComponents.Add(numberofComponents[0]);
                }
                bool configurationTypeFlag = true;

                //validate componentType
                foreach (var component in componentList)
                {
                    if (!componentTypeList.Contains(component.ComponentType.ToString()))
                    {
                        configurationTypeFlag = false;
                        break;
                    }
                }

                //validate number of components
                if (configurationTypeFlag &&
                    (componentList.Count < int.Parse(numberofComponents[0]) || componentList.Count > int.Parse(numberofComponents[1])))
                {
                    configurationTypeFlag = false;
                }

                if (configurationTypeFlag &&
                    vehicleComponentConfig.OnlyTractorInFront)
                {
                    if (componentList.Count==1&&
                        (componentList[0].ComponentType != (int)ComponentType.RecoveryVehicle &&
                        componentList[0].ComponentType != (int)ComponentType.RigidVehicle &&
                        componentList[0].ComponentType != (int)ComponentType.EngineeringPlant &&
                        componentList[0].ComponentType != (int)ComponentType.MobileCrane &&
                        componentList[0].ComponentType != (int)ComponentType.SPMT &&
                        componentList[0].ComponentType != (int)ComponentType.Tracked))
                    {
                        configurationTypeFlag = false;
                    }
                    else if (!(componentList[0].ComponentType == (int)ComponentType.BallastTractor) &&
                    !(componentList[0].ComponentType == (int)ComponentType.ConventionalTractor) &&
                    !(componentList[0].ComponentType == (int)ComponentType.RigidVehicle) &&
                    !(componentList[0].ComponentType == (int)ComponentType.MobileCrane) &&
                    !(componentList[0].ComponentType == (int)ComponentType.EngineeringPlant) &&
                    !(componentList[0].ComponentType == (int)ComponentType.RecoveryVehicle))
                    {
                        configurationTypeFlag = false;
                    }
                }
                

                if (configurationTypeFlag)
                {
                    vehicleConfig.Add(vehicleComponentConfig.ConfigId);
                }
            }

            return vehicleConfig;
        }

        public List<uint> AssessConfigurationByMovementClassification(VehicleComponentConfiguration vehicleComponentConfiguration, List<ComponentModel> componentList, List<uint> configurationTypeList)
        {
            List<uint> vehicleConfig = new List<uint>();
            string movementClassificationId = componentList[0].VehicleIntent.ToString();
            double? configurationGrossWeight = componentList.Sum(x => x.GrossWeight);
            double? configurationLength = componentList.Sum(x => x.RigidLength);
            double? configurationWidth = componentList.Max(x => x.Width);

            foreach (var configurationType in configurationTypeList)
            {
                foreach (var movementClassification in vehicleComponentConfiguration.VehicleConfigurations)
                {
                    List<string> movementClassifcationIds = movementClassification.MovementClassificationID.ToString().Split(',').ToList();
                    if (movementClassification.ConfigurationType == configurationType)
                    {
                        foreach (string id in movementClassifcationIds)
                        {
                            if (id == movementClassificationId)
                            {
                                bool currentComponentFlag = true;
                                foreach (var paramNodeConfig in movementClassification.ParamNode)
                                {
                                    //currentComponentFlag = true;
                                    switch (paramNodeConfig.Items[1])
                                    {
                                        case "Weight":
                                            List<string> weightRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                            if (weightRange[0] != "" &&
                                                (configurationGrossWeight < double.Parse(weightRange[0]) || configurationGrossWeight > double.Parse(weightRange[1])))
                                            {
                                                currentComponentFlag = false;
                                            }
                                            break;
                                        /*case "OverallLength":
                                            List<string> lengthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                            if (lengthRange[0] != "" && 
                                                (configuration.OverallLength < double.Parse(lengthRange[0]) || configuration.OverallLength > double.Parse(lengthRange[1])))
                                            {
                                                currentComponentFlag = false;
                                            }
                                            break;*/
                                        case "Length":
                                            List<string> rigLengthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                            if (rigLengthRange[0] != "" &&
                                                (configurationLength < double.Parse(rigLengthRange[0]) || configurationLength > double.Parse(rigLengthRange[1])))
                                            {
                                                currentComponentFlag = false;
                                            }
                                            break;
                                        case "Width":
                                            List<string> widthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                            if (widthRange[0] != "" &&
                                                (configurationWidth < double.Parse(widthRange[0]) || configurationWidth > double.Parse(widthRange[1])))
                                            {
                                                currentComponentFlag = false;
                                            }
                                            break;
                                            /*case "Speed":
                                                List<string> speedRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                                if (speedRange[0] != "" &&
                                                    (configuration.TravellingSpeed < double.Parse(speedRange[0]) || configuration.TravellingSpeed > double.Parse(speedRange[1])))
                                                {
                                                    currentComponentFlag = false;
                                                }
                                                break;*/
                                            /*case "Tyre Spacing":
                                                List<string> tyreSpacingRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                                if (tyreSpacingRange[0] != "" &&
                                                    (configuration.TyreSpacing < double.Parse(tyreSpacingRange[0]) || configuration.TyreSpacing > double.Parse(tyreSpacingRange[1])))
                                                {
                                                    currentComponentFlag = false;
                                                }
                                                break;*/
                                    }
                                }

                                if (currentComponentFlag)
                                {
                                    vehicleConfig.Add(configurationType);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return vehicleConfig;
        }
        #endregion

        #region AssessConfigurationMovementType 
        [HttpPost]
        [Route("VehicleConfig/AssessMovementClassificationType")]
        public IHttpActionResult AssessMovementClassificationType(AssessMovementTypeParams assessMovementTypeParams)
        {
            try
            {
                //read configuration xml
                XmlSerializer deserializer = new XmlSerializer(typeof(VehicleComponentConfiguration));
                VehicleComponentConfiguration vehicleComponentConfiguration;
                using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/Configurations/VehicleComponents.xml")))
                {
                    vehicleComponentConfiguration = (VehicleComponentConfiguration)deserializer.Deserialize(reader);
                }

                string componentIdsString = "";
                foreach (var componentId in assessMovementTypeParams.componentIds)
                {
                    if (componentIdsString != "")
                    {
                        componentIdsString += "," + componentId;
                    }
                    else
                    {
                        componentIdsString += componentId;
                    }
                }

                //get configuration values from component list
                ComponentModel configuration = VehiConfigGetVehicleConfig.Instance.GetComponentsConfigurationDetails(componentIdsString, assessMovementTypeParams.IsMovement, assessMovementTypeParams.userSchema);

                List<uint> movementClassifications = new List<uint>();
                foreach (var vehicleComponentConfig in vehicleComponentConfiguration.VehicleConfigurations)
                {
                    if (vehicleComponentConfig.ConfigurationType == assessMovementTypeParams.configurationType)
                    {
                        //vehicleComponentConfig
                        bool currentComponentFlag = true;
                        foreach (var paramNodeConfig in vehicleComponentConfig.ParamNode)
                        {
                            //currentComponentFlag = true;
                            switch (paramNodeConfig.Items[1])
                            {
                                case "Weight":
                                    List<string> weightRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                    if (weightRange[0] != "" &&
                                        (configuration.GrossWeight < double.Parse(weightRange[0]) || configuration.GrossWeight > double.Parse(weightRange[1])))
                                    {
                                        currentComponentFlag = false;
                                    }
                                    break;
                                /*case "OverallLength":
                                    List<string> lengthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                    if (lengthRange[0] != "" && 
                                        (configuration.OverallLength < double.Parse(lengthRange[0]) || configuration.OverallLength > double.Parse(lengthRange[1])))
                                    {
                                        currentComponentFlag = false;
                                    }
                                    break;*/
                                case "Length":
                                    List<string> rigLengthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                    if (rigLengthRange[0] != "" &&
                                        (configuration.RigidLength < double.Parse(rigLengthRange[0]) || configuration.RigidLength > double.Parse(rigLengthRange[1])))
                                    {
                                        currentComponentFlag = false;
                                    }
                                    break;
                                case "Width":
                                    List<string> widthRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                    if (widthRange[0] != "" &&
                                        (configuration.Width < double.Parse(widthRange[0]) || configuration.Width > double.Parse(widthRange[1])))
                                    {
                                        currentComponentFlag = false;
                                    }
                                    break;
                                    /*case "Speed":
                                        List<string> speedRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (speedRange[0] != "" &&
                                            (configuration.TravellingSpeed < double.Parse(speedRange[0]) || configuration.TravellingSpeed > double.Parse(speedRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;*/
                                    /*case "Tyre Spacing":
                                        List<string> tyreSpacingRange = paramNodeConfig.Items[4].ToString().Split(',').ToList();
                                        if (tyreSpacingRange[0] != "" &&
                                            (configuration.TyreSpacing < double.Parse(tyreSpacingRange[0]) || configuration.TyreSpacing > double.Parse(tyreSpacingRange[1])))
                                        {
                                            currentComponentFlag = false;
                                        }
                                        break;*/
                            }
                        }

                        if (currentComponentFlag)
                        {
                            List<string> movementClassIds = vehicleComponentConfig.MovementClassificationID.Split(',').ToList();
                            foreach (var item in movementClassIds)
                            {
                                movementClassifications.Add(uint.Parse(item));
                            }
                        }
                    }
                }
                return Ok(movementClassifications);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleConfig/AssessMovementClassificationType, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpPost]
        [Route("VehicleConfig/GetVehicleConfigByPartID")]
        public IHttpActionResult GetVehicleConfigByPartID(VehicleConfigParams vehicleConfigParams)
        {
            try
            {
                List<VehicleDetail> vehicleDetailLists = ApplicationVehicleProvider.Instance.GetVehicleConfigByPartID(vehicleConfigParams.ESDALRef, vehicleConfigParams.VR1Vehicle);
                return Ok(vehicleDetailLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleConfig/GetVehicleConfigByPartID,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/GetVehicleComponentList")]
        public IHttpActionResult ApplicationcomponentList(int routePartId)
        {
            try
            {
                List<ComponentGroupingModel> componentGroupingModel = ApplicationVehicleProvider.Instance.ApplicationcomponentList(routePartId);
                return Ok(componentGroupingModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - VehicleConfig/GetVehicleComponentList,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region Movement Vehicle
        [HttpPost]
        [Route("VehicleConfig/InsertMovementVehicle")]
        public IHttpActionResult InsertMovementVehicle(InsertMovementVehicle movementVehicle)
        {
            try
            {
                List<MovementVehicleConfig> vehicleConfig;
                vehicleConfig = VehiConfigGetVehicleConfig.Instance.InsertMovementVehicle(movementVehicle);
                return Content(HttpStatusCode.OK, vehicleConfig);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertMovementVehicle ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/SelectMovementVehicle")]
        public IHttpActionResult SelectMovementVehicle(long movementId, string userSchema)
        {
            try
            {
                List<MovementVehicleConfig> vehicleConfig = VehiConfigGetVehicleConfig.Instance.SelectMovementVehicle(movementId, userSchema);
                return Content(HttpStatusCode.OK, vehicleConfig);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SelectMovementVehicle ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/GetMovementConfigurationInfo")]
        public IHttpActionResult GetMovementConfigurationInfo(long vehicleId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetMovementConfigInfo(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetMovementConfigurationInfo ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/GetMovementVehicleConfig")]
        public IHttpActionResult GetMovementVehicleConfig(long vehicleId, string userSchema)
        {
            try
            {
                List<VehicleConfigList> vehicleConfigList = VehiConfigGetVehicleConfig.Instance.GetMovementVehicleConfig(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleConfigList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetMovementVehicleConfig ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/GetMovementVehicleRegDetails")]
        public IHttpActionResult GetMovementVehicleRegDetails(long vehicleId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = VehiConfigGetRegistrationProvider.Instance.GetMovementVehicleRegDetails(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetMovementVehicleRegDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("VehicleConfig/GetRouteVehicleList")]
        public IHttpActionResult GetRouteVehicleList(GetRouteVehicleList routeVehicleList)
        {
            try
            {
                List<MovementVehicleList> prevMovementVehicleLists = RouteVehicle.Instance.GetRouteVehicleList(routeVehicleList.RevisionId, routeVehicleList.VersionId, routeVehicleList.ContentRefNum, routeVehicleList.UserSchema,routeVehicleList.IsHistoric);
                return Content(HttpStatusCode.OK, prevMovementVehicleLists);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetRouteVehicleList ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/GetVehicleList")]
        public IHttpActionResult GetVehicleList(long routePartId, string userSchema, int isHistoric = 0)
        {
            try
            {
                List<VehicleDetails> vehicleList = RouteVehicle.Instance.GetVehicleList(routePartId, userSchema, isHistoric);
                return Content(HttpStatusCode.OK, vehicleList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVehicleList ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("VehicleConfig/AssignMovementVehicle")]
        public IHttpActionResult AssignMovementVehicle(VehicleAssignementParams vehicleAssignement)
        {
            try
            {
                bool status = false;
                status = VehiConfigGetVehicleConfig.Instance.AssignMovementVehicle(vehicleAssignement);
                return Content(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AssignMovementVehicle ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("VehicleConfig/DeleteMovementVehicle")]
        public IHttpActionResult DeleteMovementVehicle(DeleteMovementVehicle deleteVehicle)
        {
            try
            {
                bool status = false;
                status = VehiConfigGetVehicleConfig.Instance.DeleteMovementVehicle(deleteVehicle.MovementId, deleteVehicle.VehicleId, deleteVehicle.UserSchema);
                return Content(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/DeleteMovementVehicle ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("VehicleConfig/GetFavouriteVehicles")]
        public IHttpActionResult GetFavouriteVehicles(int organisationId, int movementId, string userSchema)
        {
            try
            {
                List<VehicleList> vehicleList = VehiConfigGetVehicleConfig.Instance.GetFavouriteVehicles(organisationId, movementId, userSchema);
                return Ok(vehicleList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/ListFavouriteVehicles, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #endregion


        #region Vehicle workflow TEMP table implementation
        #region Get vehicle dimensions based on component id in TEMP table
        [HttpGet]
        [Route("VehicleConfig/GetConfigDimensions")]
        public IHttpActionResult GetConfigDimensions(string GUID, int configTypeId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetConfigDimensions(GUID, configTypeId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetConfigDimensions ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        #region Get vehicle dimensions based on vehicle id 
        [HttpGet]
        [Route("VehicleConfig/GetVehicleDimensions")]
        public IHttpActionResult GetVehicleDimensions(long VehicleId, int configTypeId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetVehicleDimensions(VehicleId, configTypeId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVehicleDimensions ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        #region Insert components from TEMP table to component table and insert vehicle config posn
        [HttpGet]
        [Route("VehicleConfig/InsertVehicleConfigPosnTemp")]
        public IHttpActionResult InsertVehicleConfigPosnTemp(string GUID, int vehicleId, string userSchema)
        {
            try
            {
                bool result = VehiConfigGetVehicleConfig.Instance.InsertVehicleConfigPosnTemp(GUID, vehicleId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertVehicleConfigPosnTemp ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        [HttpGet]
        [Route("VehicleConfig/GetVehicleDetails")]
        public IHttpActionResult GetVehicleDetails(int componentId, bool movement, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetVehicleDetails(componentId, movement, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVehicleDetails ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #region check formal name in Temp table
        [HttpGet]
        [Route("VehicleConfig/CheckFormalNameExistsTemp")]
        public IHttpActionResult CheckFormalNameExistsTemp(int componentId, int organisationId)
        {
            try
            {
                int result = CheckFormalNameExists.Instance.CheckFormalNameExistsTemp(componentId, organisationId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckFormalNameExistsTemp ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Insert config to movement vehicle temp table
        /// <summary>
        /// To Create a new  vehicle Configuration.
        /// </summary>  
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/InsertConfigurationTemp")]
        public IHttpActionResult InsertConfigurationTemp(CreateConfigurationParams createConfigParams)
        {
            try
            {
                List<MovementVehicleConfig> movementVehicle = VehiConfigCreateVehicleConfiguration.Instance.InsertConfigurationTemp(createConfigParams.ConfigurationDetails, createConfigParams.UserSchema);
                return Content(HttpStatusCode.Created, movementVehicle);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertConfigurationTemp,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region  Insert vehicle registration to movement vehicle temp table
        /// <summary>
        /// Creating a new vehicle registration.
        /// </summary>
        /// <param name=vehicleregistrationinputparams>VehicleRegistrationInputParams model class is used to input parameters</param>       
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/CreateVehicleRegistrationTemp")]
        public IHttpActionResult CreateVehicleRegistrationTemp(VehicleRegistrationInputParams vehicleRegistrationInputParams)
        {
            try
            {
                int registationId = VehiConfigCreateRegistrationProvider.Instance.CreateVehicleRegistrationTemp(vehicleRegistrationInputParams.VehicleId, vehicleRegistrationInputParams.RegistrationId, vehicleRegistrationInputParams.FleetId, vehicleRegistrationInputParams.UserSchema);
                return Content(HttpStatusCode.Created, registationId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CreateVehicleRegistrationTemp,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Insert vehicle config posn from application
        [HttpGet]
        [Route("VehicleConfig/InsertMovementConfigPosnTemp")]
        public IHttpActionResult InsertMovementConfigPosnTemp(string GUID, int vehicleId, string userSchema)
        {
            try
            {
                bool result = VehiConfigGetVehicleConfig.Instance.InsertMovementConfigPosnTemp(GUID, vehicleId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/InsertMovementConfigPosnTemp ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        #region UpdateMovementVehicle
        /// <summary>
        /// Update vehicle Configuration.
        /// </summary> 
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleConfig/UpdateMovementVehicle")]
        public IHttpActionResult UpdateMovementVehicle(CreateConfigurationParams createConfigParams)
        {
            try
            {
                bool response = VehiConfigCreateVehicleConfiguration.Instance.UpdateMovementVehicle(createConfigParams.ConfigurationDetails, createConfigParams.UserSchema);
                return Content(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/UpdateMovementVehicle,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        [HttpGet]
        [Route("VehicleConfig/GetVehicleRegistrationTemp")]
        public IHttpActionResult GetVehicleRegistrationTemp(int vehicleId, string userSchema)
        {
            try
            {
                List<VehicleRegistration> vehicleRegistrations = VehiConfigGetRegistrationProvider.Instance.GetVehicleRegistrationTemp(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, vehicleRegistrations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetVehicleRegistrationTemp,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region  Add Vehicle To Fleet form movement temp
        [HttpGet]
        [Route("VehicleConfig/AddMovementVehicleToFleet")]
        public IHttpActionResult AddMovementVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema)
        {
            int result = 0;
            try
            {
                result = ApplicationVehicleProvider.Instance.AddMovementVehicleToFleet(vehicleId, organisationId, flag, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddMovementVehicleToFleet,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
        #region Get vehicle dimensions based on component id in movement TEMP table
        [HttpGet]
        [Route("VehicleConfig/GetMovementConfigDimensions")]
        public IHttpActionResult GetMovementConfigDimensions(int vehicleId, string userSchema)
        {
            try
            {
                ConfigurationModel configuration = VehiConfigGetVehicleConfig.Instance.GetMovementConfigDimensions(vehicleId, userSchema);
                return Content(HttpStatusCode.OK, configuration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetMovementConfigDimensions ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion
        [HttpPost]
        [Route("VehicleConfig/AddMovementComponentToFleet")]
        public IHttpActionResult AddMovementComponentToFleet(FleetComponenentModel fleetComponenentModel)
        {
            try
            {
                int compId = AddToFleet.Instance.AddMovementComponentToFleet(fleetComponenentModel.ComponentId, fleetComponenentModel.OrganisationId, fleetComponenentModel.UserSchema);
                return Content(HttpStatusCode.OK, compId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddMovementComponentToFleet ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }


        [HttpGet]
        [Route("VehicleConfig/MovementCheckFormalNameExists")]
        public IHttpActionResult MovementCheckFormalNameExists(int componentId, int organisationId, string userSchema)
        {
            try
            {
                int result = CheckFormalNameExists.Instance.MovementCheckFormalNameExists(componentId, organisationId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/MovementCheckFormalNameExists ,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region AssessMovementType
        [HttpPost]
        [Route("VehicleConfig/AssessMovementType")]
        public IHttpActionResult AssessMovementType(AssessMoveTypeParams moveTypeParams)
        {
            VehicleMovementType movementType = null;

            try
            {
                //get configuration values from temp table if not already available
                if (moveTypeParams.configuration == null 
                    && moveTypeParams != null && moveTypeParams.VehicleId != 0)
                {
                    moveTypeParams.configuration = VehiConfigGetVehicleConfig.Instance.GetMovementVehicleDetails(moveTypeParams.VehicleId, moveTypeParams.IsRoute,
                        moveTypeParams.UserSchema);
                }

                if (moveTypeParams.configuration.VehicleType == (int)ConfigurationType.Crane)
                {
                    movementType = CraneAssessment.AssessMobileCraneLogic(moveTypeParams.configuration, moveTypeParams.ForceApplication);
                    if (movementType.VehicleClass == (int)VehicleClassificationType.NoCrane &&
                        movementType.MovementType == (int)MovementType.no_movement)
                    {
                        movementType = StgoAssessment.AssessMovementTypeLogic(moveTypeParams.configuration, moveTypeParams.ForceApplication);
                    }
                }
                else
                {
                    movementType = StgoAssessment.AssessMovementTypeLogic(moveTypeParams.configuration, moveTypeParams.ForceApplication);
                }
                return Ok(movementType);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleConfig/AssessMovementType, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region AssessMovementTypeStub
        [HttpPost]
        [Route("VehicleConfig/AssessMovementTypeStub")]
        public IHttpActionResult AssessMovementTypeStub(AssessMoveTypeParams moveTypeParams)
        {
            VehicleMovementType movementType = null;

            try
            {
                if (moveTypeParams.configuration.VehicleType == (int)ConfigurationType.Crane)
                {
                    movementType = CraneAssessment.AssessMobileCraneLogic(moveTypeParams.configuration, moveTypeParams.ForceApplication);
                }
                else
                {
                    movementType = StgoAssessment.AssessMovementTypeLogic(moveTypeParams.configuration, moveTypeParams.ForceApplication);
                }
                return Ok(movementType);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + $"- VehicleConfig/AssessMovementTypeStub, Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetApplVehicle
        [HttpGet]
        [Route("VehicleConfig/GetApplVehicle")]
        public IHttpActionResult GetApplVehicle(int PartID, int revisionId, bool IsVRVeh, string userSchema)
        {
            List<VehicleDetails> vehicleDetails = new List<VehicleDetails>();
            try
            {
                vehicleDetails = ApplicationVehicleProvider.Instance.GetApplVehicle(PartID, revisionId, IsVRVeh, userSchema);
                return Content(HttpStatusCode.OK, vehicleDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"/GetApplVehicle, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region SORT Previous/Current Movement VehcileList
        [HttpGet]
        [Route("VehicleConfig/GetSortMovementVehicle")]
        public IHttpActionResult GetSortMovementVehicle(long revisionId, int rListType)
        {
            try
            {
                List<AppVehicleConfigList> appVehicleList = VehiConfigGetVehicleConfig.Instance.GetSortMovementVehicle(revisionId, rListType);
                return Content(HttpStatusCode.OK, appVehicleList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"/GetSortMovementVehicle, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Save All Components
        [HttpPost]
        [Route("VehicleConfig/SaveVehicleComponents")]
        public IHttpActionResult SaveVehicleComponents(VehicleConfigDetail vehicleDetail)
        {
            try
            {
                long result = VehiConfigCreateVehicleConfiguration.Instance.SaveVehicleComponents(vehicleDetail, vehicleDetail.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/SaveVehicleComponents,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Update All Components
        [HttpPost]
        [Route("VehicleConfig/UpdateVehicleComponents")]
        public IHttpActionResult UpdateVehicleComponents(VehicleConfigDetail vehicleDetail)
        {
            try
            {
                long result = VehiConfigCreateVehicleConfiguration.Instance.UpdateVehicleComponents(vehicleDetail, vehicleDetail.UserSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/UpdateVehicleComponents,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Function for add component to fleet
        [HttpPost]
        [Route("VehicleConfig/AddComponentToFleetLibrary")]
        public IHttpActionResult AddComponentToFleetLibrary(List<VehicleComponentModel> componentList)
        {
            try
            {
                long result = VehiConfigCreateVehicleConfiguration.Instance.AddComponentToFleet(componentList);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/AddComponentToFleet,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        [HttpPost]
        [Route("VehicleConfig/GetFilteredVehicleCombinations")]
        public IHttpActionResult GetFilteredVehicleCombinations(ConfigurationModel configurationModel)
        {
            try
            {
                List<VehicleConfigurationGridList> configList = VehiConfigGetVehicleConfig.Instance.GetFilteredVehicleCombinations(configurationModel);
                return Content(HttpStatusCode.OK, configList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/GetFilteredVehicleCombinations,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpPost]
        [Route("VehicleConfig/ImportFleetVehicleToRoute")]
        public IHttpActionResult ImportFleetVehicleToRoute(ImportVehicleListModel inputParams)
        {
            try
            {
                long result = VehiConfigGetVehicleConfig.Instance.ImportFleetVehicleToRoute(inputParams.ConfigurationId, inputParams.UserSchema, inputParams.ApplicationRevisionId);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ImportFleetVehicleToRoute,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpGet]
        [Route("VehicleConfig/ChekcVehicleIsValid")]
        public IHttpActionResult ChekcVehicleIsValid(long vehicleId, int flag, string userSchema)
        {
            try
            {
                long result = VehiConfigGetVehicleConfig.Instance.ChekcVehicleIsValid(vehicleId, flag, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/ChekcVehicleIsValid,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        #region check vehicle validation
        [HttpGet]
        [Route("VehicleConfig/CheckVehicleValidations")]
        public IHttpActionResult CheckVehicleValidations(int vehicleId, string userschema)
        {
            ImportVehicleValidations vehicleValidations = new ImportVehicleValidations();
            try
            {
                vehicleValidations = ApplicationVehicleProvider.Instance.CheckVehicleValidations(vehicleId, userschema);
                return Content(HttpStatusCode.OK, vehicleValidations);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - VehicleConfig/CheckVehicleValidations,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region AutoFill Vehicles
        [HttpGet]
        [Route("VehicleConfig/AutoFillVehicles")]
        public IHttpActionResult AutoFillVehicles(string vehicleIds, int vehicleCount, string userSchema)
        {
            try
            {
                List<AutoFillModel> autoFillModel = VehiConfigGetVehicleConfig.Instance.AutoFillVehicles(vehicleIds, vehicleCount, userSchema);
                return Content(HttpStatusCode.OK, autoFillModel);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $" - VehicleConfig/AutoFillVehicles,Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetNenApiVehiclesList
        [HttpGet]
        [Route("VehicleConfig/GetNenApiVehiclesList")]
        public IHttpActionResult GetNenApiVehiclesList(long notificationId, long organisationId, string userschema)
        {
            try
            {
                List<VehicleConfigration> objVehicleConfigration = VehiConfigGetVehicleConfig.Instance.GetNenApiVehiclesList(notificationId, organisationId, userschema);
                return Content(HttpStatusCode.OK, objVehicleConfigration);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $" - VehicleConfig/GetNenApiVehiclesList,Exception:{ex}");
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

    }
}