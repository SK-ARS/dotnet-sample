using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class CreateComponentAxleParams
    {
        public Axle Axle { get; set; }
        public int ComponentId { get; set; }
        public string UserSchema { get; set; }
        public bool Movement { get; set; }
    }
}