using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Providers
{
    public class VehiConfigCreateRegistrationProvider : IVehiConfigCreateRegistrationProvider
    {
        #region CreateVehicleComponent Singleton
        private VehiConfigCreateRegistrationProvider()
        {
        }
        public static VehiConfigCreateRegistrationProvider Instance
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
            internal static readonly VehiConfigCreateRegistrationProvider instance = new VehiConfigCreateRegistrationProvider();
        }
        #region Logger instance
        private const string PolicyName = "VehiConfigCreateRegistrationProvider";
         #endregion
        #endregion
        /// <summary>
        ///To insert a new Vehicle
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="registrationValue"></param>
        /// <param name="fleetId"></param>
        /// <returns></returns>
        public int InsertVehicleRegistration(int vehicleId, string registrationValue, string fleetId)
        {
            return VehicleConfigDAO.CreateVehicleRegistration(vehicleId, registrationValue, fleetId);
        }
        /// <summary>
        ///To Save  VR1VehicleRegistration
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="registrationValue"></param>
        /// <param name="fleetId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public int SaveVR1VehicleRegistrationId(int vehicleId, string registrationValue, string fleetId, string userSchema)
        {
            return VehicleConfigDAO.SaveVR1VehicleRegistrationId(vehicleId, registrationValue, fleetId, userSchema);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="registrationValue"></param>
        /// <param name="fleetId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns> 
        public int SaveAppVehicleRegistrationId(int vehicleId, string registrationValue, string fleetId, string userSchema)
        {
            return VehicleConfigDAO.SaveAppVehicleRegistrationId(vehicleId, registrationValue, fleetId, userSchema);
        }
        /// <summary>
        ///To insert a new Vehicle
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="registrationValue"></param>
        /// <param name="fleetId"></param>
        /// <returns></returns>
        public int CreateVehicleRegistrationTemp(int vehicleId, string registrationValue, string fleetId, string userSchema)
        {
            return VehicleConfigDAO.CreateVehicleRegistrationTemp(vehicleId, registrationValue, fleetId, userSchema);
        }

    }
}