using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class SaveConfigurationModel
    {
        public int configId { get; set; }
        public string userSchema { get; set; } = "portal";
        public int applnRev { get; set; } = 0;
        public bool isNotif { get; set; } = false;
        public bool isVR1 { get; set; } = false;

    }
}