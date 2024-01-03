using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface ICreateComponentRegistration
    {
        int CreateVehicleRegFromCompReg(int componentId, int vehicleId);
        int CreateRegistration(int componentId, string registrationValue, string fleetId);
        int CreateVR1CompRegistration(int componentId, string registrationValue, string fleetId, string userSchema);
        int CreateRegistrationTemp(int componentId, string registrationValue, string fleetId, bool movement, string userSchema);
    }
}
