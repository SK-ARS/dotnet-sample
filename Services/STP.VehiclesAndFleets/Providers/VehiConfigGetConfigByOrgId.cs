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
    public class VehiConfigGetConfigByOrgId : IVehiConfigGetConfigByOrgId
    {
        #region GetConfigByOrgId Singleton
        private VehiConfigGetConfigByOrgId()
        {
        }
        public static VehiConfigGetConfigByOrgId Instance
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
            internal static readonly VehiConfigGetConfigByOrgId instance = new VehiConfigGetConfigByOrgId();
        }
        #region Logger instance
        private const string PolicyName = "GetConfigByOrgId";
        //        private static readonly LogWrapper log = new LogWrapper();
        #endregion
        #endregion
        #region GetConfigByOrgId implementation
        public List<VehicleConfigurationGridList> GetConfigByOrganisationId(int organisationId, int movtype, int movetype1, string userSchema, int filterFavouritesVehConfig,int presetFilter=1,int? sortOrder=null)
        {
            return VehicleConfigDAO.GetComponentDetailsByID(organisationId, movtype, movetype1, userSchema, filterFavouritesVehConfig,presetFilter,sortOrder);
        }
        #endregion
    }
}