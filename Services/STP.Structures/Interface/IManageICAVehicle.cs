using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Structures;

namespace STP.Structures.Interface
{
    public interface IManageICAVehicle
    {
         string GetICAVehicleResult(ICAVehicleModel objICAVehicleModel, ManageStructureICA objManageStructureICA, int movementClassConfig, int configType, int compNum, int? tractorType, int? trailerType, int structureId, int sectionId, int orgId);
         ManageStructureICA GetManageICAUsage(int organisationId, int structureId, int sectionId);
         int UpdateStructureICAUsage(ManageStructureICA ICAUsage, int orgId, int structureId, int sectionId);
    }
}
