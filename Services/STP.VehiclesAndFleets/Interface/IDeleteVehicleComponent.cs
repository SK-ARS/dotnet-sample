using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IDeleteVehicleComponent
    {
        int DeleteVehComponent(int componentId);
        int DeleteComponentRegister(int componentId, int idNumber, int flag);
        int DeleteVR1VehComponentRegister(int componentId, int idNumber, string userSchema);
        int DeleteAppVehComponentRegister(int componentId, int idNumber, string userSchema);
    }
}
