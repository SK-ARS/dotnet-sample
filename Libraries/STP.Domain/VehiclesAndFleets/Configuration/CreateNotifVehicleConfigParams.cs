using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class CreateNotifVehicleConfigParams
    {
        public NewConfigurationModel NewConfigurationModel { get; set; }
        public string ContentRefNo { get; set; }
        public int IsNotif { get; set; }
    }
}