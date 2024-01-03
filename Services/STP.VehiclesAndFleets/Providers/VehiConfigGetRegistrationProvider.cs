using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Configuration;

namespace STP.VehiclesAndFleets.Providers
{
    public sealed class VehiConfigGetRegistrationProvider : IVehiConfigGetRegistrationProvider
    {
        #region GetRegistrationProvider Singleton
        private VehiConfigGetRegistrationProvider()
        {
        }
        public static VehiConfigGetRegistrationProvider Instance
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
            internal static readonly VehiConfigGetRegistrationProvider instance = new VehiConfigGetRegistrationProvider();
        }
        #region Logger instance
        private const string PolicyName = "VehiConfigGetRegistrationProvider";      
        #endregion
        #endregion
        /// <summary>
        /// To get Vehicle registration  details.
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<VehicleRegistration> GetVehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetRegistration(vehicleId, userSchema);
        }
        /// <summary>
        ///  To get the VR1 vehicle  registration details
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<VehicleRegistration> GetVR1VehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetVR1ApplRegistration(vehicleId, userSchema);
        }
        /// <summary>
        /// To get the App vehicle registration details.
        /// </summary>
        /// <param name="vhclId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public List<VehicleRegistration> GetApplVehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetApplRegistration(vehicleId, userSchema);
        }
        public List<VehicleRegistration> GetMovementVehicleRegDetails(long vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetMovementRegistration(vehicleId, userSchema);
        }
        public List<VehicleRegistration> GetVehicleRegistrationTemp(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetVehicleRegistrationTemp(vehicleId, userSchema);
        }
    }
}