using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using STP.Domain.VehiclesAndFleets.Component;

namespace STP.VehiclesAndFleets.Providers
{
    public class VehiConfigGetComponentByOrgId : IVehiConfigGetComponentByOrgId
    {
        #region GetComponentByOrgId Singleton
        private VehiConfigGetComponentByOrgId()
        {
        }
        public static VehiConfigGetComponentByOrgId Instance
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
            internal static readonly VehiConfigGetComponentByOrgId instance = new VehiConfigGetComponentByOrgId();
        }
        #region Logger instance
        private const string PolicyName = "VehiConfigGetComponentByOrgId";
        #endregion
        #endregion
        /// <summary>
        ///GetAllVR1ComponentByOrganisationId.
        /// </summary>  
        /// <param name="componentId">Input componentId</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        public List<ComponentGridList> GetAllVR1ComponentByOrganisationId(long componentId, string userSchema)
        {
            return VehicleComponentDAO.GetAllVR1ComponentDetailsByID(componentId, userSchema);
        }
        /// <summary>
        ///GetAllAppComponentByOrganisationId
        /// </summary>        
        ///  <param name="componentId">InputcomponentId</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<ComponentGridList> GetAllAppComponentByOrganisationId(long componentId, string userSchema)
        {
            return VehicleComponentDAO.GetAllAppComponentDetailsByID(componentId, userSchema);
        }
        /// <summary>
        /// To select all component regardless of show flag  based on Organisation Id.
        /// </summary>        
        ///  <param name="componentId">Input componentId</param>
        /// <param name="organisationId">Input organisationId = portal</param>
        /// <returns></returns>
        public List<ComponentGridList> GetAllComponentByOrganisationId(int organisationId, long componentId)
        {
            return VehicleConfigDAO.GetAllComponentDetailsByID(organisationId, componentId);
        }
        /// <summary>
        ///GetComponentByOrganisationId
        /// </summary>        
        ///  <param name="organisationId">InputcomponentId</param>
        ///  <param name="componentName">Input component name - search</param>
        /// <param name="componentType">Input componentType = search</param>
        ///  <param name="organisationId">Input vehicleIntent - search</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<ComponentGridList> GetComponentByOrganisationId(int organisationId, int pageNumber, int pageSize, string componentName, string componentType, string vehicleIntent, int filterFavourites, string userSchema,int presetFilter,int? sortOrder)
        {
            return VehicleComponentDAO.GetComponentDetailsByID(organisationId, pageNumber, pageSize, componentName, componentType, vehicleIntent,filterFavourites, userSchema,presetFilter,sortOrder);
        }
        /// <summary>
        ///To get the of Components.
        /// </summary>     
        ///  <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<ComponentIdModel> GetListComponentId(int vehicleId)
        {
            return VehicleConfigDAO.GetComponentIDList(vehicleId);
        }
        /// <summary>
        ///To get the list of VR1 Components.
        /// </summary>            
        ///  <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<ComponentIdModel> GetVR1ListComponentId(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetVR1ComponentIDList(vehicleId, userSchema);
        }
        /// <summary>
        ///To get the list of App Components.
        /// </summary>            
        ///  <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema = portal</param>
        /// <returns></returns>
        public List<ComponentIdModel> GetAppListComponentId(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.GetAppComponentIDList(vehicleId, userSchema);
        }
    }
}