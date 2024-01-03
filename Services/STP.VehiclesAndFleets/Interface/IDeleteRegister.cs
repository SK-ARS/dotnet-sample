using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    interface IDeleteRegister
    {
        int DeleteVR1RegConfig(int vehicleId, int IdNumber, string userSchema);
        int DisableVehicle(int vehicleId);
        int DeleteVehicleConfigPosn(int vehicleId, int latpos, int longpos);
        int DeleteApplicationVehicleConfigPosn(int vehicleId, int latpos, int longpos, string userSchema);
        int DeleteVR1VehicleConfigPosn(int vehicleId, int latpos, int longpos, string userSchema);
        int DeletVehicleRegisterConfiguration(int vehicleId, int IdNumber, bool isMovement = false);
        int DeleteAppRegConfig(int vehicleId, int IdNumber, string userSchema);
    }
}
