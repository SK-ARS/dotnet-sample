using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System.Collections.Generic;
using System.Diagnostics;
namespace STP.VehiclesAndFleets.Providers
{
    public sealed class AddToFleet : IAddToFleet
    {
        #region AddToFleet Singleton
        private AddToFleet()
        {
        }
        public static AddToFleet Instance
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
            internal static readonly AddToFleet instance = new AddToFleet();
        }
        #region Logger instance
        private const string PolicyName = "AddToFleet";
        //        private static readonly LogWrapper log = new LogWrapper();
        #endregion
        #endregion
        /// <summary>
        /// AddComponentToFleet
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <returns></returns>
        public int AddComponentToFleet(int componentid, int organisationid)
        {
            return VehicleConfigDAO.AddToFleet(componentid, organisationid);
        }
        /// <summary>
        /// AddApplicationComponentToFleet
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <param name="flag"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public int AddApplicationComponentToFleet(int componentid, int organisationid, int flag, string userSchema)
        {
            return VehicleConfigDAO.AddApplCompToFleet(componentid, organisationid, flag, userSchema);
        }
        /// <summary>
        /// AddVR1ComponentToFleet
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <param name="flag"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public int AddVR1ComponentToFleet(int componentid, int organisationid, int flag, string userSchema)
        {
            return VehicleConfigDAO.AddVR1ApplCompToFleet(componentid, organisationid, flag, userSchema);
        }
        public int AddMovementComponentToFleet(int componentid, int organisationid, string userSchema)
        {
            return VehicleConfigDAO.AddMovementComponentToFleet(componentid, organisationid, userSchema);
        }
    }
}
