using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Component;

namespace STP.VehiclesAndFleets.Providers
{
    public sealed class RouteVehicle : IRouteVehicle
    {
        #region RouteVehicle Singleton
        private RouteVehicle()
        {
        }
        public static RouteVehicle Instance
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
            internal static readonly RouteVehicle instance = new RouteVehicle();
        }
        #region Logger instance
        private const string PolicyName = "RouteVehicle";
        //        private static readonly LogWrapper log = new LogWrapper();
        #endregion
        #endregion
        #region VehicleConfiguration
        #region GetRouteConfigInfo
        /// <summary>
        /// GetRouteConfigInfo
        /// </summary>
        /// <param name="componentId">Input componentId</param>
        /// <param name="userSchema">Input userschema</param>
        /// <returns></returns>
        public ConfigurationModel GetRouteConfigInfo(int componentId, string userSchema)
        {
            return RouteVehicleDAO.GetRouteVehicleConfigDetails(componentId, userSchema);
        }
        #endregion
        #region GetRouteConfigInfoForVR1
        /// <summary>
        /// function to obtain the vehicle configuration general details for VR1
        /// </summary>
        /// <param name="componentId">Input componentId</param>
        /// <param name="userSchema">Input userschema</param>
        /// <returns></returns>        
        public ConfigurationModel GetRouteConfigInfoForVR1(int componentId, string userSchema, int isEdit = 0)
        {
            return RouteVehicleDAO.GetRouteVehicleConfigDetailsForVR1(componentId, userSchema,isEdit);
        }
        #endregion
        #region GetRouteVehicleConfigVhclID
        /// <summary>
        ///  function to obtain the vehicle configuration position
        /// </summary>
        /// <param name="vehicleId">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        public List<VehicleConfigList> GetRouteVehicleConfigVhclID(int vehicleId, string userSchema)
        {
            return RouteVehicleDAO.GetRouteVehicleConfigPosn(vehicleId, userSchema);
        }
        #endregion
        #region GetRouteVehicleRegistrationDetails
        /// <summary>
        ///  function to obtain the vehicle configuration registration details
        /// </summary>
        /// <param name="vhclID">Input Vehicle Id</param>
        /// <param name="userSchema">Input UserSchema</param>
        /// <returns></returns>
        public List<VehicleRegistration> GetRouteVehicleRegistrationDetails(int vehicleId, string userSchema)
        {
            return RouteVehicleDAO.GetVehicleConfigRegistration(vehicleId, userSchema);
        }
        #endregion
        #region GetNotifVehicleConfig
        /// <summary>
        ///  To get the vehicle configuration notification
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>
        /// <param name="isSimple">Input isSimple</param>
        /// <returns></returns>
        public ConfigurationModel GetNotifVehicleConfig(int vehicleId, int isSimple)
        {
            return RouteVehicleDAO.GetNotifVehicleConfigByVehID(vehicleId, isSimple);
        }
        #endregion
        #region CheckNotifValidVehicle
        /// <summary>
        ///  To check vehicle validity for import
        /// </summary>
        /// <param name="vhclId">Input Vehicle Id</param>      
        /// <returns></returns>
        public bool CheckNotifValidVehicle(int vhclId)
        {
            return RouteVehicleDAO.CheckValidVehilceForImport(vhclId);
        }
        #endregion
        #region CheckValidVehicleCreated
        public long CheckValidVehicleCreated(int vehicleId)
        {
            return RouteVehicleDAO.CheckValidVehilceCreated(vehicleId);
        }
        #endregion
        #region CheckWheelWithSumOfAxel
        public bool CheckWheelWithSumOfAxel(int vhclId, string userSchema, int applnRev, bool isNotif, bool isVR1)
        {
            return RouteVehicleDAO.CheckWheelWithSumOfAxel(vhclId, userSchema, applnRev, isNotif, isVR1);
        }
        #endregion
        #endregion
        #region Vehicle Components
        #region Function to obtain component general details
        public ComponentModel GetRouteComponent(int componentId, string userSchema)
        {
            return RouteVehicleDAO.GetRouteVehicleComponent(componentId, userSchema);
        }
        #endregion
        #region Function to obtain component registration details
        public List<VehicleRegistration> GetRouteComponentRegistrationDetails(int compId, string userSchema)
        {
            return RouteVehicleDAO.GetRouteComponentRegistration(compId, userSchema);
        }
        #endregion
        #region Function to obtain the component axle details
        public List<Axle> ListRouteComponentAxle(int componentId, string userSchema)
        {
            return RouteVehicleDAO.ListRouteComponentAxle(componentId, userSchema);
        }
        #endregion
        #endregion

        public List<MovementVehicleList> GetRouteVehicleList(long revisionId, long versionId, string cont_Ref_No, string userSchema, int isHistoric)
        {
            return VehicleConfigDAO.GetRouteVehicleList(revisionId, versionId, cont_Ref_No, userSchema,isHistoric);
        }
        public List<VehicleDetails> GetVehicleList(long routePartId, string userSchema, int isHistoric = 0)
        {
            return VehicleConfigDAO.GetVehicleList(routePartId, userSchema, isHistoric);
        }
    }
}