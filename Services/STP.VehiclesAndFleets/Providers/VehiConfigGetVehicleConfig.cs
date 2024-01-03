using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.VehiclesAndFleets.Providers
{
    public sealed class VehiConfigGetVehicleConfig : IVehiConfigGetVehicleConfig
    {
        #region GetVehicleConfig Singleton
        private VehiConfigGetVehicleConfig()
        {
        }
        public static VehiConfigGetVehicleConfig Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly VehiConfigGetVehicleConfig instance = new VehiConfigGetVehicleConfig();
        }
        #region Logger instance
        private const string PolicyName = "VehiConfigGetVehicleConfig";
        #endregion
        #endregion
        /// <summary>
        /// Listing the vehicle components in a configuration for VR1 application
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        public List<VehicleConfigList> GetVR1VehicleConfigVhclID(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetVR1VehicleConfigPosn(vehicleId, userSchema);
        }
        /// <summary>
        /// GetAppVehicleConfigVhclID
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema </param>
        /// <returns></returns>     
        public List<VehicleConfigList> GetAppVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetAppVehicleConfigPosn(vehicleId, userSchema);
        }
        /// <summary>
        /// GetVehicleConfigVhclID
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema </param>
        /// <returns></returns>
        public List<VehicleConfigList> GetVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetVehicleConfigPosn(vehicleId, userSchema);
        }
        /// <summary>
        /// function to select all component regardless of show flag
        /// </summary>        
        ///  <param name="componentId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchemal</param>
        /// <returns></returns>


        /// <summary>
        /// GetNotifVehicleConfigByID
        /// </summary>
        /// <param name="VehicleId">Input VehicleId</param>
        ///    
        /// <returns></returns>
        public ConfigurationModel GetNotifVehicleConfigByID(int vehicleId)
        {
            return VehicleConfigDAO.GetNotifVehicleConfigByID(vehicleId);
        }
        /// <summary>
        /// Get vehicle configuration information
        /// </summary>
        /// <param name="componentId">Input VehicleId</param>
        ///  <param name="userSchema">Input userschema as portal</param>  
        /// <returns></returns>
        public ConfigurationModel GetConfigInfo(int componentId, string userSchema)
        {
            return VehicleConfigDAO.GetConfigDetails(componentId, userSchema);
        }
        /// <summary>
        /// To Get the  Application Configuration Information
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public ConfigurationModel GetConfigInfoApplication(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetConfigDetailsApplication(vehicleId, userSchema);
        }
        /// <summary>
        /// To Get the  VR1 Configuration Information
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="schemaType"></param>
        public ConfigurationModel GetConfigInfoVR1(int componentId, string schemaType)
        {
            return VehicleConfigDAO.GetConfigDetailsVR1(componentId, schemaType);
        }

        /// <summary>
        /// Get vehicle components in configuration
        /// </summary>
        /// <param name="componentId">Input VehicleId</param>
        ///  <param name="userSchema">Input userschema as portal</param>  
        /// <returns></returns>
        public List<ComponentModel> GetComponentsInConfiguration(string componentIds, string userSchema, int flag = 0)
        {
            return VehicleConfigDAO.GetComponentsInConfiguration(componentIds, userSchema, flag);
        }

        /// <summary>
        /// Get configuration details of components
        /// </summary>
        /// <param name="componentIds">Input componentIds</param>
        ///  <param name="userSchema">Input userschema as portal</param>  
        /// <returns></returns>
        public ComponentModel GetComponentsConfigurationDetails(string componentIds, bool isMovement, string userSchema)
        {
            return VehicleConfigDAO.GetComponentsConfigurationDetails(componentIds, isMovement, userSchema);
        }
        public List<MovementVehicleConfig> InsertMovementVehicle(InsertMovementVehicle movementVehicle)
        {
            return VehicleConfigDAO.InsertMovementVehicle(movementVehicle);
        }
        public ConfigurationModel GetMovementConfigInfo(long vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetMovementConfigDetails(vehicleId, userSchema);
        }
        public List<VehicleConfigList> GetMovementVehicleConfig(long vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetMovementVehicleConfigPosn(vehicleId, userSchema);
        }
        public bool AssignMovementVehicle(VehicleAssignementParams vehicleAssignement)
        {
            return VehicleConfigDAO.AssignMovementVehicle(vehicleAssignement);
        }
        public bool DeleteMovementVehicle(long movementId, long vehicleId, string userSchema)
        {
            return VehicleConfigDAO.DeleteMovementVehicle(movementId, vehicleId, userSchema);
        }
        public List<VehicleList> GetFavouriteVehicles(int organisationId, int movementId, string userSchema)
        {
            return VehicleConfigDAO.GetFavouriteVehicles(organisationId, movementId, userSchema);
        }

        #region Vehicle workflow TEMP table implementation
        #region Get vehicle dimensions based on component id in TEMP table
        public ConfigurationModel GetConfigDimensions(string GUID, int configTypeId, string userSchema)
        {
            return VehicleConfigDAO.GetConfigDimensions(GUID,configTypeId, userSchema);
        }
        #endregion
        #region Get vehicle dimensions based on vehicle id
        public ConfigurationModel GetVehicleDimensions(long VehicleId, int configTypeId, string userSchema)
        {
            return VehicleConfigDAO.GetVehicleDimensions(VehicleId, configTypeId, userSchema);
        }
        #endregion
        #region Insert components from TEMP table to component table and insert vehicle config posn
        public bool InsertVehicleConfigPosnTemp(string GUID, int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.InsertVehicleConfigPosnTemp(GUID, vehicleId, userSchema);
        }
        #endregion
        public ConfigurationModel GetVehicleDetails(int vehicleId, bool movement, string userSchema)
        {
            return VehicleConfigDAO.GetVehicleDetails(vehicleId, movement, userSchema);
        }
        #region Insert vehicle config posn from application
        public bool InsertMovementConfigPosnTemp(string GUID, int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.InsertMovementConfigPosnTemp(GUID, vehicleId, userSchema);
        }
        #endregion
        #region Get vehicle dimensions based on component id in movement TEMP table
        public ConfigurationModel GetMovementConfigDimensions(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetMovementConfigDimensions(vehicleId, userSchema);
        }
        #endregion
        #endregion

        public ConfigurationModel GetMovementVehicleDetails(int vehicleId, int isRoute, string userSchema)
        {
            return VehicleConfigDAO.GetMovementVehicleDetails(vehicleId, isRoute, userSchema);
        }

        public List<MovementVehicleConfig> SelectMovementVehicle(long movementId, string userSchema)
        {
            return VehicleConfigDAO.SelectMovementVehicle(movementId, userSchema);
        }
        public List<AppVehicleConfigList> GetSortMovementVehicle(long revisionId, int rListType)
        {
            return VehicleConfigDAO.GetSortMovementVehicle(revisionId, rListType);
        }

        public List<VehicleConfigurationGridList> GetFilteredVehicleCombinations(ConfigurationModel configurationModel)
        {
            return VehicleConfigDAO.GetFilteredVehicleCombinations(configurationModel);
        }
        public long ImportFleetVehicleToRoute(long configId, string userSchema, int applnRev = 0)
        {
            return VehicleConfigDAO.ImportFleetVehicleToRoute(configId, userSchema, applnRev);
        }
        public long ChekcVehicleIsValid(long vehicleId, int flag, string userSchema)
        {
            return VehicleConfigDAO.ChekcVehicleIsValid(vehicleId, flag, userSchema);
        }
        public List<AutoFillModel> AutoFillVehicles(string vehicleIds, int vehicleCount, string userSchema)
        {
            return VehicleConfigDAO.AutoFillVehicles(vehicleIds, vehicleCount, userSchema);
        }

        public List<VehicleConfigration> GetNenApiVehiclesList(long notificationId, long organisationId, string userschema)
        {
            return VehicleConfigDAO.GetNenApiVehiclesList(notificationId, organisationId, userschema);
        }
    }
}