using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IGetVehicleComponent
    {
        ComponentModel GetVehicleComponent(int componentId);
        VehicleConfigList GetConfigForComponent(int componentId);
        ComponentModel GetVR1VehicleComponent(int componentId, string userSchema);
        ComponentModel GetAppVehicleComponent(int componentId, string userSchema);
        List<VehicleConfigList> GetComponentIdTemp(string GUID, int vehicleId, string userSchema);
        ComponentModel GetComponentTemp(int componentId, string GUID, string userSchema);
        List<ComponentGridList> GetComponentFavourite(int organisationId, int movementId);
    }
}
