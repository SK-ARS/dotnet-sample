using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class AutoAssessingParams
    {
        public int ConfigurationTypeId { get; set; }
        public string ConfigurationType { get; set; }
        public int MovementTypeId { get; set; }
        public string MovementType { get; set; }

    }
}
