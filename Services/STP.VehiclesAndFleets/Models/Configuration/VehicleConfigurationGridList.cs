using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class VehicleConfigurationGridList
    {
        public double ConfigurationId { get; set; }
        public string ConfigurationName { get; set; }
        public string IndendedUse { get; set; }
        public int VehicleType { get; set; }
        public string FormalName { get; set; }
        public string response { get; set; }
        public double width { get; set; }

        public double height { get; set; }
        public double grossWeight { get; set; }
    }
}