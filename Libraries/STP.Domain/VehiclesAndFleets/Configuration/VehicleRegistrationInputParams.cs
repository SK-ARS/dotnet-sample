using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class VehicleRegistrationInputParams
    {
        public string FleetId { get; set; }
        public string RegistrationId { get; set; }
        public int VehicleId { get; set; }
        public string UserSchema { get; set; }

    }
}