using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace STP.VehiclesAndFleets.Interface
{
  public   interface ICheckFormalNameExists
    {
        int CheckFormalName(int componentId, int organisationId);
        int CheckVR1FormalName(int componentId, int organisationId, string userSchema);
        int CheckAppFormalName(int componentId, int organisationId, string userSchema);
        int CheckFormalNameExistsTemp(int componentId, int organisationId);
        int MovementCheckFormalNameExists(int componentId, int organisationId, string userSchema);
    }
}
