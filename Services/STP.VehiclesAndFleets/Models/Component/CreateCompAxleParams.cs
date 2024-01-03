using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Component
{
    public class CreateCompAxleParams
    {
        public Axle axle { get; set; }
        public int componentId { get; set; }
        public string userSchema { get; set; }
    }
}