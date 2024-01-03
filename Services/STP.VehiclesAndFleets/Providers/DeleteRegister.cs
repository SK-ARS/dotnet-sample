using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Providers
{
    public class DeleteRegister : IDeleteRegister
    {
        #region DeleteRegister Singleton
        private DeleteRegister()
        {
        }
        public static DeleteRegister Instance
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
            internal static readonly DeleteRegister instance = new DeleteRegister();
        }
        #region Logger instance
        private const string PolicyName = "DeleteRegister";
        //        private static readonly LogWrapper log = new LogWrapper();
        #endregion
        #endregion
        #region DeleteVR1RegConfig
        public int DeleteVR1RegConfig(int vehicleId, int IdNumber, string userSchema)
        {
            return VehicleConfigDAO.DeleteVR1RegConfig(vehicleId, IdNumber, userSchema);
        }
        #endregion
        #region DisableVehicle
        public int DisableVehicle(int vehicleId)
        {
            return VehicleConfigDAO.Disablevehicle(vehicleId);
        }
        #endregion
        #region DeleteVehicleConfigPosn
        public int DeleteVehicleConfigPosn(int vehicleId, int latpos, int longpos)
        {
            return VehicleConfigDAO.DeleteVehicleconfig(vehicleId, latpos, longpos);
        }
        #endregion
        #region DeleteApplicationVehicleConfigPosn
        public int DeleteApplicationVehicleConfigPosn(int vehicleId, int latpos, int longpos, string userSchema)
        {
            return VehicleConfigDAO.DeleteApplVehicleconfig(vehicleId, latpos, longpos, userSchema);
        }
        #endregion
        #region DeleteVR1VehicleConfigPosn
        public int DeleteVR1VehicleConfigPosn(int vehicleId, int latpos, int longpos, string userSchema)
        {
            return VehicleConfigDAO.DeleteVR1Vehicleconfig(vehicleId, latpos, longpos, userSchema);
        }
        #endregion
        #region DeletVehicleRegisterConfiguration
        public int DeletVehicleRegisterConfiguration(int vehicleId, int IdNumber, bool isMovement = false)
        {
            return VehicleConfigDAO.DeleteVehicleRegistrationConfiguration(vehicleId, IdNumber, isMovement);
        }
        #endregion
        #region DeleteAppRegConfig
        public int DeleteAppRegConfig(int vehicleId, int IdNumber, string userSchema)
        {
            return VehicleConfigDAO.DeleteAppRegConfig(vehicleId, IdNumber, userSchema);
        }
        #endregion
    }
}