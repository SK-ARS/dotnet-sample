using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class CreateComponentRegistrationParams
    {
        public int ComponentId { get; set; }
        public string FleetId { get; set; }
        public string RegistrationValue { get; set; }
        public string UserSchema { get; set; }
        public bool Movement { get; set; }
    }
}