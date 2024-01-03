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
    public class GetComponentRegistration: IGetComponentRegistration
    {
        #region GetComponentRegistration Singleton
        private GetComponentRegistration()
        {
        }
        public static GetComponentRegistration Instance
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
            internal static readonly GetComponentRegistration instance = new GetComponentRegistration();
        }
        #region Logger instance
        private const string PolicyName = "GetComponentRegistration";
        #endregion
        #endregion
        #region Function for getting registartion details
        public List<VehicleRegistration> GetRegistrationDetails(int componentId)
        {
            return VehicleComponentDAO.GetRegistration(componentId);
        }
        #endregion
        #region Function for getting VR1 registartion details
        public List<VehicleRegistration> GetVR1RegistrationDetails(int componentId, string userSchema)
        {
            return VehicleComponentDAO.GetVR1Registration(componentId, userSchema);
        }
        #endregion
        #region Function for getting application registartion details
        public List<VehicleRegistration> GetApplRegistrationDetails(int componentId, string userSchema)
        {
            return VehicleComponentDAO.GetApplRegistration(componentId, userSchema);
        }
        #endregion
        #region Function for getting registartion details for TEMP table
        public List<VehicleRegistration> GetRegistrationTemp(int componentId, bool movement, string userSchema)
        {
            return VehicleComponentDAO.GetRegistrationTemp(componentId, movement, userSchema);
        }
        #endregion
    }
}