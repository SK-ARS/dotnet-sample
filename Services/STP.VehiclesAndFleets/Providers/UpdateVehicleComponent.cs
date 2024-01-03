using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Component;

namespace STP.VehiclesAndFleets.Providers
{
    public class UpdateVehicleComponent: IUpdateVehicleComponent
    {
        #region UpdateVehicleComponent Singleton
        private UpdateVehicleComponent()
        {
        }
        public static UpdateVehicleComponent Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        public object VehicleDAO { get; private set; }
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
            internal static readonly UpdateVehicleComponent instance = new UpdateVehicleComponent();
        }
        #region Logger instance
        private const string PolicyName = "UpdateVehicleComponent";
        #endregion
        #endregion
        #region Update component in fleet managemnet
        public bool UpdateComponent(ComponentModel componentModel)
        {
            return VehicleComponentDAO.UpdateComponent(componentModel);
        }
        #endregion
        #region Update VR1 vehicle component in fleet managemnet
        public bool UpdateVR1VehComponent(ComponentModel componentModel, string userSchema)
        {
            return VehicleComponentDAO.UpdateVR1VehComponent(componentModel, userSchema);
        }
        #endregion
        #region Update application component
        public bool UpdateAppVehComponent(ComponentModel componentModel, string userSchema)
        {
            return VehicleComponentDAO.UpdateAppVehComponent(componentModel, userSchema);
        }
        #endregion
        #region Insert axle details
        public bool UpdateAxle(Axle axle, int componentId)
        {
           return VehicleComponentDAO.InsertAxleDetails(axle, componentId);
        }
        #endregion
        #region Insert VR1 axle details
        public bool UpdateVR1Axle(Axle axle, int componentId, string userSchema)
        {
            return VehicleComponentDAO.InsertVR1VehAxleDetails(axle, componentId, userSchema);
        }
        #endregion
        #region Insert appl axle details
        public bool UpdateAppAxle(Axle axle, int componentId, string userSchema)
        {
            return VehicleComponentDAO.InsertAppVehAxleDetails(axle, componentId, userSchema);
        }
        #endregion
        #region List of axles from vehicle
        public List<Axle> ListAxle(int componentId)
        {
            return VehicleComponentDAO.ListAxle(componentId);
        }
        #endregion
        #region List of axles from vr1 app vehicle
        public List<Axle> ListVR1vehAxle(int componentId, string userSchema)
        {
            return VehicleComponentDAO.ListVR1vehAxle(componentId, userSchema);
        }
        #endregion
        #region List of axles from app vehicle
        public List<Axle> ListAppvehAxle(int componentId, string userSchema)
        {
            return VehicleComponentDAO.ListAppvehAxle(componentId, userSchema);
        }
        #endregion

        #region Insert axle details to TEMP table
        public bool InsertAxleDetailsTemp(Axle axle, int componentId, bool movement, string userSchema)
        {
            return VehicleComponentDAO.InsertAxleDetailsTemp(axle, componentId, movement, userSchema);
        }
        #endregion

        #region Update component in TEMP table
        public bool UpdateComponentTemp(ComponentModel componentModel)
        {
            return VehicleComponentDAO.UpdateComponentTemp(componentModel);
        }
        #endregion
        #region List of axles from vehicle from TEMP table
        public List<Axle> ListAxleTemp(int componentId, bool movement, string userSchema)
        {
            return VehicleComponentDAO.ListAxleTemp(componentId, movement, userSchema);
        }
        #endregion

        #region Insert component to vehicle config
        public int InsertComponentConfigPosn(int componentId, int vehicleId)
        {
            return VehicleComponentDAO.InsertComponentConfigPosn(componentId, vehicleId);
        }
        #endregion
        #region add to fleet using temp table
        public int AddToFleetTemp(string GUID, int componentId, int vehicleId)
        {
            return VehicleComponentDAO.AddToFleetTemp(GUID,componentId, vehicleId);
        }
        #endregion
        #region Delete component from temp table
        public int DeleteComponentTemp(int componentId)
        {
            return VehicleComponentDAO.DeleteComponentTemp(componentId);
        }
        #endregion
        #region Delete component in config posn
        public int DeleteComponentConfig(int componentId, int vehicleId, bool movement, string userSchema)
        {
            return VehicleComponentDAO.DeleteComponentConfig( componentId, vehicleId, movement, userSchema);
        }

        #region Update component in movement TEMP table
        public bool UpdateMovementComponentTemp(ComponentModel componentModel, string userSchema)
        {
            return VehicleComponentDAO.UpdateMovementComponentTemp(componentModel, userSchema);
        }
        #endregion
        #endregion
    }
}