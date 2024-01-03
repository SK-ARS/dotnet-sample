using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Providers
{
    public sealed class CheckFormalNameExists: ICheckFormalNameExists
    {
        #region CheckFormalNameExists Singleton
        private CheckFormalNameExists()
        {
        }
        public static CheckFormalNameExists Instance
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
            internal static readonly CheckFormalNameExists instance = new CheckFormalNameExists();
        }
        #region Logger instance
        private const string PolicyName = "CheckFormalNameExists";
        //        private static readonly LogWrapper log = new LogWrapper();
        #endregion
        #endregion
        /// <summary>
        ///  For  Component Formal Name check
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="organisationId"></param>
        /// <returns></returns>
        public int CheckFormalName(int componentId, int organisationId)
        {
            return VehicleConfigDAO.CheckFormalNameExists(componentId, organisationId);
        }
        /// <summary>
        ///   For VR1 Component Formal Name check
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="organisationId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public int CheckVR1FormalName(int componentId, int organisationId, string userSchema)
        {
            return VehicleConfigDAO.CheckVR1FormalNameExists(componentId, organisationId, userSchema);
        }
        /// <summary>
        ///  For Application Component Formal Name check
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="organisationId"></param>
        /// <param name="userSchema"></param>
        /// <returns></returns>
        public int CheckAppFormalName(int componentId, int organisationId, string userSchema)
        {
            return VehicleConfigDAO.CheckAppFormalNameExists(componentId, organisationId, userSchema);
        }

        public int CheckFormalNameExistsTemp(int componentId, int organisationId)
        {
            return VehicleConfigDAO.CheckFormalNameExistsTemp(componentId, organisationId);
        }
        public int MovementCheckFormalNameExists(int componentId, int organisationId, string userSchema)
        {
            return VehicleConfigDAO.MovementCheckFormalNameExists(componentId, organisationId, userSchema);
        }
    }
}