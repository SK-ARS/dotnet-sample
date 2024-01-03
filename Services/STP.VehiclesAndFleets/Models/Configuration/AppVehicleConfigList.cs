using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class AppVehicleConfigList
    {
        public long VehicleID { get; set; }
        public int RoutePartID { get; set; }
        public string VehicleName { get; set; }
        public string RoutePart { get; set; }
        public int AppPartId { get; set; }
        public string routeType { get; set; }
        public string VehicleDescr { get; set; }
    }
}