using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class SearchVehicleCombination
    {
        public string VehicleName { get; set; }
        public string FleetId { get; set; }
        public double? GrossWeight { get; set; }
        public double? RigidLength { get; set; }
        public double? OverAllLength { get; set; }
        public double? OverAllWidth { get; set; }
        public double? LeftOverhang { get; set; }
        public double? FrontOverhang { get; set; }
        public double? RightOverhang { get; set; }
        public double? RearOverhang { get; set; }
        public int? NoOfAxlesTractor { get; set; }
        public int? NoOfAxlesTrailer { get; set; }
        public long OrganisationId { get; set; }
        public int? VehicleType { get; set; }
    }
}
