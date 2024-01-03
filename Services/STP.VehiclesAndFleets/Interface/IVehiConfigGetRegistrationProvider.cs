using STP.Domain;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
  public   interface IVehiConfigGetRegistrationProvider
    {
        List<VehicleRegistration> GetVehicleRegistrationDetails(int vehicleId, string userSchema);
        List<VehicleRegistration> GetVR1VehicleRegistrationDetails(int vehicleId, string userSchema);
        List<VehicleRegistration> GetApplVehicleRegistrationDetails(int vehicleId, string userSchema);
        List<VehicleRegistration> GetMovementVehicleRegDetails(long vehicleId, string userSchema);
        List<VehicleRegistration> GetVehicleRegistrationTemp(int vehicleId, string userSchema);
    }
}
