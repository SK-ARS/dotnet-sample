using STP.Domain;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IGetComponentRegistration
    {
        List<VehicleRegistration> GetRegistrationDetails(int componentId);
        List<VehicleRegistration> GetVR1RegistrationDetails(int componentId, string userSchema);
        List<VehicleRegistration> GetApplRegistrationDetails(int componentId, string userSchema);
        List<VehicleRegistration> GetRegistrationTemp(int componentId, bool movement, string userSchema);
    }
}
