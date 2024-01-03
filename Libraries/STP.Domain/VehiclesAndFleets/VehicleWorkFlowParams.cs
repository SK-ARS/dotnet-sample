using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System.Collections.Generic;

namespace STP.Domain.VehiclesAndFleets
{
    public class VehicleComponentsModel
    {
        public ComponentModel ComponentModel { get; set; }
        public List<RegistrationParams> RegistrationDetails { get; set; }
        public List<Axle> AxleDetails { get; set; }
    }
    public class VehicleConfigurationModel
    {
        public NewConfigurationModel ConfigurationModel { get; set; }
        public List<RegistrationParams> RegistrationDetails { get; set; }

    }
    public class VehicleWorkFlowParams
    {
        public VehicleWorkFlowParams()
        {
            VehicleComponentsModels = new List<VehicleComponentsModel>();
            VehicleConfigurationModels = new VehicleConfigurationModel();            
        }
        public List<VehicleComponentsModel> VehicleComponentsModels { get; set; }
        public VehicleConfigurationModel VehicleConfigurationModels { get; set; }
        public int TotalComponents { get; set; }

    }
}
