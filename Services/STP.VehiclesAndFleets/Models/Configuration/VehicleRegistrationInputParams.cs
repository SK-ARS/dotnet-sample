using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class VehicleRegistrationInputParams
    {
        public string FleetId { get; set; }
        public string RegistrationId { get; set; }
        public int vhclId { get; set; }
        public string userSchema { get; set; }

    }
}