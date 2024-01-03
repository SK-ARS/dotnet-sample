using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class UpdateVehicleComponentParams
    {
        public ComponentModel ComponentModel { get; set; }
        public string UserSchema { get; set; }
    }
}