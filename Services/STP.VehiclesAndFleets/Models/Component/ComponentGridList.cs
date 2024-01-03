using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Component
{
    public class ComponentGridList
    {
        public double ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string FleetList { get; set; }
        public string ComponentDescription { get; set; }
        //public int VehicleIntent { get; set; }
        public string IntendedUse { get; set; }
        public string ComponentType { get; set; }
        public string VehicleIntent { get; set; }
        public decimal AxleSpacing { get; set; }
    }
}