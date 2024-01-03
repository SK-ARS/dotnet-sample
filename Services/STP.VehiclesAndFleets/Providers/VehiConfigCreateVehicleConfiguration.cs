using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Configuration;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;

namespace STP.VehiclesAndFleets.Providers
{
    public sealed class VehiConfigCreateVehicleConfiguration : IVehiConfigCreateVehicleConfiguration
    {
        #region CreateVehicleConfiguration Singleton
        private VehiConfigCreateVehicleConfiguration()
        {
        }
        public static VehiConfigCreateVehicleConfiguration Instance
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
            internal static readonly VehiConfigCreateVehicleConfiguration instance = new VehiConfigCreateVehicleConfiguration();
        }
        #region Logger instance
        private const string PolicyName = "VehiConfigCreateVehicleConfiguration";
        #endregion
        #endregion
        /// <summary>
        /// Create vehicle Configuration.
        /// </summary>        
        /// <returns></returns>
        public double InsertVehicleConfiguration(NewConfigurationModel configurationModel)
        {
            return VehicleConfigDAO.InsertConfiguration(configurationModel);
        }
        /// <summary>
        /// Update vehicle Configuration.
        /// </summary> 
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>
        /// <returns></returns>
        public bool UpdateVehicleConfiguration(NewConfigurationModel configurationModel)
        {
            return VehicleConfigDAO.UpdateVehicle(configurationModel);
        }
        /// <summary>
        /// Create  vehicle Configuration Position
        /// </summary> 
        /// <param name=configurationModel>VehicleConfigList model class is used to input parameters</param>
        /// <returns></returns>
        public VehicleConfigList InsertVehicleConfigPosition(VehicleConfigList ConfigList)
        {
            return VehicleConfigDAO.CreateVehicleConfigPosn(ConfigList);
        }
        /// <summary>
        /// Update  vehicle Configuration Details.
        /// </summary>         
        /// <returns></returns>
        public bool SaveVehicleConfiguration(int configId, string userSchema, int applnRev, bool isNotif, bool isVR1)
        {
            return VehicleConfigDAO.UpdateVehicleDetailsOnFinish(configId, userSchema, applnRev, isNotif, isVR1);
        }
        /// <summary>
        /// Update  app vehicle configuration details.
        /// </summary>         
        /// <returns></returns>
        public bool Updateappvehicleconfig(NewConfigurationModel configurationModel, string userSchema)
        {
            return VehicleConfigDAO.UpdateApplicationVehicle(configurationModel, userSchema);
        }
        /// <summary>
        /// Update  VR1 vehicle configuration details.
        /// </summary>         
        /// <returns></returns>
        public bool UpdateVR1vehicleconfig(NewConfigurationModel configurationModel, string userschema)
        {
            return VehicleConfigDAO.UpdateVR1ApplicationVehicle(configurationModel, userschema);
        }
        /// <summary>
        /// CreateApplVehConfigPosn
        /// </summary>
        /// <param name="ConfigList">VehicleConfigList modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <returns></returns>
        public VehicleConfigList CreateApplVehConfigPosn(VehicleConfigList configList, string userSchema)
        {
            return VehicleConfigDAO.CreateApplVehConfigPosn(configList, userSchema);
        }
        /// <summary>
        /// CreateAppConfigPosn
        /// </summary>
        /// <param name="ConfigList">VehicleConfigList modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <param name="isImportFromFleet">Input isImportFromFleet </param>
        /// <returns></returns>
        public VehicleConfigList CreateAppConfigPosn(VehicleConfigList configList, int isImportFromFleet, string userSchema)
        {
            return VehicleConfigDAO.CreateAppVehicleConfigPosn(configList, isImportFromFleet, userSchema);
        }
        /// <summary>
        /// CreateVR1ConfigPosn
        /// </summary>
        /// <param name="ConfigList">VehicleConfigList modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <returns></returns>
        public VehicleConfigList CreateVR1ConfigPosn(VehicleConfigList configList, string userSchema)
        {
            return VehicleConfigDAO.CreateVR1VehicleConfigPosn(configList, userSchema);
        }
        /// <summary>
        /// InsertVR1VehicleConfiguration
        /// </summary>
        /// <param name="configurationModel">NewConfigurationModel modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <returns></returns>
        public NewConfigurationModel InsertVR1VehicleConfiguration(NewConfigurationModel configurationModel, string userSchema)
        {
            return VehicleConfigDAO.InsertVR1ApplicationVehicleConfiguration(configurationModel, userSchema);
        }
        /// <summary>
        /// InsertApplicationVehicleConfiguration
        /// </summary>
        /// <param name="configurationModel">NewConfigurationModel modal Class is used to input params</param>
        /// <param name="userSchema">Input userSchema </param>
        /// <returns></returns>
        public double InsertApplicationVehicleConfiguration(NewConfigurationModel configurationModel, string userSchema)
        {
            return VehicleConfigDAO.InsertApplicationVehicleConfiguration(configurationModel, userSchema);
        }
        /// <summary>
        /// ImportVehicleFromList
        /// </summary>
        /// <param name="model">ImportVehicleListModel modal Class is used to input params</param>
        /// <returns></returns>
        public long ImportVehicleFromList(ImportVehicleListModel model)
        {
            return VehicleConfigDAO.ImportVehicle(model.ConfigurationId, model.UserSchema, model.ApplicationRevisionId, model.IsNotif, model.IsVR1, model.ContentRefNo, model.IsCandidate, model.VersionType);
        }
        /// <summary>
        /// CopyVehicleFromList
        /// </summary>
        /// <param name="model">ImportVehicleListModel modal Class is used to input params</param>
        /// <returns></returns>
        public long CopyVehicleFromList(ImportVehicleListModel model)
        {
            return VehicleConfigDAO.CopyVehicle(model.ConfigurationId, model.UserSchema, model.ApplicationRevisionId, model.IsNotif, model.IsVR1, model.ContentRefNo, model.IsCandidate);
        }

        /// <summary>
        /// Create vehicle Configuration Temp.
        /// </summary>        
        /// <returns></returns>
        public List<MovementVehicleConfig> InsertConfigurationTemp(NewConfigurationModel configurationModel, string userSchema)
        {
            return VehicleConfigDAO.InsertConfigurationTemp(configurationModel, userSchema);
        }

        /// <summary>
        /// Update vehicle Configuration.
        /// </summary> 
        /// <param name=configurationModel>NewConfigurationModel model class is used to input parameters</param>
        /// <returns></returns>
        public bool UpdateMovementVehicle(NewConfigurationModel configurationModel, string userSchema)
        {
            return VehicleConfigDAO.UpdateMovementVehicle(configurationModel, userSchema);
        }

        #region Save All Components
        public long SaveVehicleComponents(VehicleConfigDetail vehicleDetail, string userSchema)
        {
            return VehicleComponentDAO.SaveVehicleComponents(vehicleDetail, userSchema);
        }
        #endregion

        #region Update All Components
        public long UpdateVehicleComponents(VehicleConfigDetail vehicleDetail, string userSchema)
        {
            return VehicleComponentDAO.UpdateVehicleComponents(vehicleDetail, userSchema);
        }
        #endregion

        public long AddComponentToFleet(List<VehicleComponentModel> componentList)
        {
            return VehicleComponentDAO.AddComponentToFleet(componentList);
        }
    }
}