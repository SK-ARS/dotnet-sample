using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class VehicleConfigurationGridList
    {
       

        public double ConfigurationId { get; set; }
        public string ComponentIdList { get; set; }
        public string ConfigurationName { get; set; }
        public string IndendedUse { get; set; }
        public int VehicleType { get; set; }
        public string FormalName { get; set; }
        public string Response { get; set; }
        public int IsFavourites { get; set; }
        public int VehiclePurpose { get; set; }
        public double width { get; set; }

        public double height { get; set; }
        public double grossWeight { get; set; }
    }
}