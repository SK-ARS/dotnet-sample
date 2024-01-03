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
    public class GetVehicleComponentProvider: IGetVehicleComponent
    {
        #region GetVehicleComponentProvider Singleton
        private GetVehicleComponentProvider()
        {
        }
        public static GetVehicleComponentProvider Instance
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
            internal static readonly GetVehicleComponentProvider instance = new GetVehicleComponentProvider();
        }
        #region Logger instance
        private const string PolicyName = "GetVehicleComponentProvider";
        #endregion
        #endregion
        #region Get component based on component id
        public ComponentModel GetVehicleComponent(int componentId)
        {
            return VehicleComponentDAO.GetComponent(componentId);
        }
        #endregion
        #region Function to check configuration already created
        public VehicleConfigList GetConfigForComponent(int componentId)
        {
            return VehicleComponentDAO.GetConfigByComponent(componentId);
        }
        #endregion
        #region Getting component from route vehicle component table for VR1 appln vehicle
        public ComponentModel GetVR1VehicleComponent(int componentId, string userSchema)
        {
            return VehicleComponentDAO.GetVR1VehComponent(componentId, userSchema);
        }
        #endregion
        #region Getting component from application component
        public ComponentModel GetAppVehicleComponent(int componentId, string userSchema)
        {
            return VehicleComponentDAO.GetApplVehComponent(componentId, userSchema);
        }
        #endregion

        #region Get component based on GUID
        public List<VehicleConfigList> GetComponentIdTemp(string GUID, int vehicleId, string userSchema)
        {
            return VehicleComponentDAO.GetComponentIdTemp(GUID, vehicleId, userSchema);
        }
        #endregion
        #region Get component based on component id and guid from TEMP table
        public ComponentModel GetComponentTemp(int componentId, string GUID, string userSchema)
        {
            return VehicleComponentDAO.GetComponentTemp(componentId, GUID, userSchema);
        }
        #endregion
        #region Get component fav
        public List<ComponentGridList> GetComponentFavourite(int organisationId, int movementId)
        {
            return VehicleComponentDAO.GetComponentFavourite(organisationId, movementId);
        }
        #endregion
    }
}