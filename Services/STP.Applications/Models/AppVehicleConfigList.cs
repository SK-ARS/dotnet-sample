using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class AppVehicleConfigList
    {
        public long VehicleID { get; set; }
        public int RoutePartID { get; set; }
        public string VehicleName { get; set; }
        public string RoutePart { get; set; }
        public int ApplicationPartID { get; set; }
        public string RouteType { get; set; }
        public string VehicleDescription { get; set; }
    }
}