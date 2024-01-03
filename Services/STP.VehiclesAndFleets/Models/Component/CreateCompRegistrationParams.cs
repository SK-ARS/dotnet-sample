using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Component
{
    public class CreateCompRegistrationParams
    {
        public int compId { get; set; }
        public string fleetId { get; set; }
        public string registrationValue { get; set; }
        public string userSchema { get; set; }
    }
}