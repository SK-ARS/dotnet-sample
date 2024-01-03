using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Providers
{
    public class CreateComponentRegistration : ICreateComponentRegistration
    {
        #region CreateComponentRegistration Singleton
        private CreateComponentRegistration()
        {
        }
        public static CreateComponentRegistration Instance
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
            internal static readonly CreateComponentRegistration instance = new CreateComponentRegistration();
        }
        #region Logger instance
        private const string PolicyName = "CreateComponentRegistration";
        #endregion
        #endregion
        #region CreateVehicleRegFromComp
        public int CreateVehicleRegFromCompReg(int componentId, int vehicleId)
        {
            return VehicleComponentDAO.CreateVehicleRegFromComp(componentId, vehicleId);
        }
        #endregion
        #region Function for create registration
        public int CreateRegistration(int componentId, string registrationValue, string fleetId)
        {
            return VehicleComponentDAO.CreateRegistration(componentId, registrationValue, fleetId);
        }
        #endregion
        #region Function for create VR1 registration
        public int CreateVR1CompRegistration(int componentId, string registrationValue, string fleetId, string userSchema)
        {
            return VehicleComponentDAO.CreateVR1CompRegistration(componentId, registrationValue, fleetId, userSchema);
        }
        #endregion
        #region Function for create VR1 registration
        public int CreateAppCompRegistration(int componentId, string registrationValue, string fleetId, string userSchema)
        {
            return VehicleComponentDAO.CreateAppCompRegistration(componentId, registrationValue, fleetId, userSchema);
        }
        #endregion

        #region Vehicle workflow TEMP table implementation
        #region Insert registration to TEMP table
        public int CreateRegistrationTemp(int componentId, string registrationValue, string fleetId, bool movement, string userSchema)
        {
            return VehicleComponentDAO.CreateRegistrationTemp(componentId, registrationValue, fleetId, movement, userSchema);
        }
        #endregion
        #endregion
    }
}