using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
  
    public interface IVehiConfigCreateRegistrationProvider
    {
        int InsertVehicleRegistration(int vehicleId, string registrationValue, string fleetId);
        int SaveVR1VehicleRegistrationId(int vehicleId, string registrationValue, string fleetId, string userSchema);
        int SaveAppVehicleRegistrationId(int vehicleId, string registrationValue, string fleetId, string userSchema);
        int CreateVehicleRegistrationTemp(int vehicleId, string registrationValue, string fleetId, string userSchema);
    }
}
