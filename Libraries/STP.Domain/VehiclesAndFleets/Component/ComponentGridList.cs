using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class ComponentGridList
    {
        public double ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string FleetList { get; set; }
        public string ComponentDescription { get; set; }
        public string IntendedUse { get; set; }
        public string ComponentType { get; set; }
        public string VehicleIntent { get; set; }
        public decimal AxleSpacing { get; set; }
        public long TotalRecordCount { get; set; }

        public double? Width { get; set; }

        public int IsFavourites { get; set; }
    }
}