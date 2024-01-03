using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class CreateNotifVehicleConfigParams
    {
        public NewConfigurationModel newConfigurationModel { get; set; }
        public string contentRefNo { get; set; }
        public int isNotif { get; set; }
    }
}