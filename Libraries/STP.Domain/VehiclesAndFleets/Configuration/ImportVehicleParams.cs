using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class ImportVehicleParams
    {
        public int VehicleId { get; set; }
        public int RoutePartId { get; set; }
        public int ApplicationRevisionId { get; set; }
        public string UserSchema { get; set; }
        public string ContentRefNo { get; set; }
        public int Simple { get; set; }
    }
}
