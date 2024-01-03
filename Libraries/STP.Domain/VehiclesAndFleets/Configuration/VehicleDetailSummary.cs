using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class VehicleDetailSummary
    {
        public string VehicleName { get; set; }
        public long RoutePartId { get; set; }
        public long VehicleId { get; set; }
        public string FormalName { get; set; }
        public string VehicleType { get; set; }
    }
}