using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IUpdateVehicleComponent
    {
        bool UpdateComponent(ComponentModel componentModel);
        bool UpdateVR1VehComponent(ComponentModel componentModel, string userSchema);
        bool UpdateAppVehComponent(ComponentModel componentModel, string userSchema);
        bool UpdateAxle(Axle axle, int componentId);
        bool UpdateVR1Axle(Axle axle, int componentId, string userSchema);
        bool UpdateAppAxle(Axle axle, int componentId, string userSchema);
        List<Axle> ListAxle(int componentId);
        List<Axle> ListVR1vehAxle(int componentId, string userSchema);
        List<Axle> ListAppvehAxle(int componentId, string userSchema);
        bool InsertAxleDetailsTemp(Axle axle, int componentId, bool movement, string userSchema);
        List<Axle> ListAxleTemp(int componentId, bool movement, string userSchema);
        int InsertComponentConfigPosn(int componentId, int vehicleId);
        int AddToFleetTemp(string GUID, int componentId, int vehicleId);
        int DeleteComponentTemp(int componentId);
        int DeleteComponentConfig(int componentId, int vehicleId, bool movement, string userSchema);
        bool UpdateComponentTemp(ComponentModel componentModel);
        bool UpdateMovementComponentTemp(ComponentModel componentModel, string userSchema);
    }
}
