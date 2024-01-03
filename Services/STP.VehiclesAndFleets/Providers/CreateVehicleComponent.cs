using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;

namespace STP.VehiclesAndFleets.Providers
{
    public class CreateVehicleComponent: ICreateVehicleComponent
    {
        #region CreateVehicleComponent Singleton
        private CreateVehicleComponent()
        {
        }
        public static CreateVehicleComponent Instance
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
            internal static readonly CreateVehicleComponent instance = new CreateVehicleComponent();
        }
        #region Logger instance
        private const string PolicyName = "CreateVehicleComponent";
        #endregion
        #endregion
        #region Function for checking vehicle exists
        public int CheckConfigNameExists(string vehicleName, int organisationId)
        {
            return VehicleComponentDAO.CheckConfigurationExists(vehicleName, organisationId);
        }
        #endregion
        #region Create component in fleet managemnet
        public double CreateComponent(ComponentModel componentModel)
        {
            return VehicleComponentDAO.InsertComponent(componentModel);
        }
        #endregion
        #region InsertAppVehicleComponent implementation
        public double InsertAppVehicleComponent(ComponentModel componentModel, string userSchema)
        {
            return VehicleComponentDAO.InsertAppVehicleComponent(componentModel,userSchema);
        }
        #endregion
        #region InsertVR1VehicleComponent implementation
        public double InsertVR1VehicleComponent(ComponentModel componentModel, string userSchema)
        {
            return VehicleComponentDAO.InsertVR1VehicleComponent(componentModel,userSchema);
        }
        #endregion
        #region For inserting the vehicle and component into vehicle position table with lat and long position as 1
        public VehicleConfigList CreateConfPosnComponent(VehicleConfigList configList)
        {
            return VehicleComponentDAO.CreateVehicleConPosnForComp(configList);
        }
        #endregion

        #region Update component subtype to TEMP table
        public int UpdateComponentSubTypeToTemp(ComponentModel componentModel)
        {
            return VehicleComponentDAO.UpdateComponentSubTypeToTemp(componentModel);
        }
        #endregion

        #region Insert component to TEMP table
        public double InsertComponentToTemp(ComponentModel componentModel)
        {
            return VehicleComponentDAO.InsertComponentToTemp(componentModel);
        }
        #endregion

        public int CheckComponentInternalnameExists(string componentName, int organisationId)
        {
            return VehicleComponentDAO.CheckComponentInternalnameExists(componentName, organisationId);
        }
        public int UpdateConventionalTractorAxleCount(int axleCount, int vehicleId, int fromComponentId, int toComponentId, string userSchema)
        {
            return VehicleComponentDAO.UpdateConventionalTractorAxleCount(axleCount, vehicleId, fromComponentId, toComponentId, userSchema);
        }
    }
}