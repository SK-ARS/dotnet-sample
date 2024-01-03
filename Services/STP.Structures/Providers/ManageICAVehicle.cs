using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using STP.Structures.Persistance;
using STP.Domain.Structures;
using STP.Structures.Interface;

namespace STP.Structures.Providers
{
    public class ManageICAVehicle : IManageICAVehicle
    {
        #region ManageICAVehicle Singleton

        private ManageICAVehicle()
        {
        }
        public static ManageICAVehicle Instance
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
            static Nested()
            {
            }
            internal static readonly ManageICAVehicle instance = new ManageICAVehicle();
        }

        #region Logger instance

        private const string PolicyName = "ManageICAVehicle";

        #endregion
        #endregion


        #region Get Manage ICAUsage implementation
        public ManageStructureICA GetManageICAUsage(int organisationId, int structureId, int sectionId)
        {
            return StructureDAO.GetStructureICAUsage(organisationId, structureId, sectionId);
        }
        #endregion

        #region Get ICA Vehicle Result

        public string GetICAVehicleResult(ICAVehicleModel objICAVehicleModel, ManageStructureICA objManageStructureICA, int movementClassConfig, int configType, int compNum, int? tractorType, int? trailerType, int structureId, int sectionId, int orgId)
        {
            return StructureDAO.GetVehicleICAResult(objICAVehicleModel, objManageStructureICA, movementClassConfig, configType, compNum, tractorType, trailerType, structureId, sectionId, orgId);
        }

        #endregion

        #region Update ICAUsage implementation
        public int UpdateStructureICAUsage(ManageStructureICA ICAUsage, int orgId, int structureId, int sectionId)
        {
            return StructureDAO.UpdateManageICAUsage(ICAUsage, orgId, structureId, sectionId);
        }
        #endregion
    }
}
