using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class VehicleDetailSummary
    {
        public string Vehicle_Name { get; set; }
        public long Rpart_Id { get; set; }
        public long Veh_Id { get; set; }
        public string FormalName { get; set; }
        public string VehicleType { get; set; }
    }
}