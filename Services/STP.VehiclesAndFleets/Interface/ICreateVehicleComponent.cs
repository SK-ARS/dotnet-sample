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
    public interface ICreateVehicleComponent
    {
        int CheckConfigNameExists(string vehicleName, int organisationId);
        double CreateComponent(ComponentModel componentModel);
        double InsertAppVehicleComponent(ComponentModel componentModel, string userSchema);
        double InsertVR1VehicleComponent(ComponentModel componentModel, string userSchema);
        VehicleConfigList CreateConfPosnComponent(VehicleConfigList configList);
        double InsertComponentToTemp(ComponentModel componentModel);
        int CheckComponentInternalnameExists(string componentName, int organisationId);
    }
}
