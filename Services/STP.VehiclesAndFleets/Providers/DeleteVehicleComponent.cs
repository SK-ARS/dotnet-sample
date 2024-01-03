using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Providers
{
    public class DeleteVehicleComponent : IDeleteVehicleComponent
    {
        #region DeleteVehicleComponent Singleton
        private DeleteVehicleComponent()
        {
        }
        public static DeleteVehicleComponent Instance
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
            internal static readonly DeleteVehicleComponent instance = new DeleteVehicleComponent();
        }
        #region Logger instance
        private const string PolicyName = "DeleteVehicleComponent";
        #endregion
        #endregion
        #region Delete vehicle component in fleet management
        public int DeleteVehComponent(int componentId)
        {
            return VehicleComponentDAO.DeleteVehComponent(componentId);
        }
        #endregion
        #region Delete vehicle registration in component
        public int DeleteComponentRegister(int componentId, int idNumber, int flag)
        {
            return VehicleComponentDAO.DeleteRegistrationComponent(componentId, idNumber, flag);
        }
        #endregion
        #region Delete VR1 application Component Registration 
        public int DeleteVR1VehComponentRegister(int componentId, int idNumber, string userSchema)
        {
            return VehicleComponentDAO.DeleteVR1VehRegistrationComponent(componentId, idNumber, userSchema);
        }
        #endregion
        #region Delete Application Component Registration 
        public int DeleteAppVehComponentRegister(int componentId, int idNumber, string userSchema)
        {
            return VehicleComponentDAO.DeleteAppVehRegistrationComponent(componentId,idNumber, userSchema);
        }
        #endregion
    }
}