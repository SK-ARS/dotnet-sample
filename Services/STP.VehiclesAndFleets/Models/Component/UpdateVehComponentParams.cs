using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Component
{
    public class UpdateVehComponentParams
    {
        public ComponentModel componentModel { get; set; }
        public string userSchema { get; set; }
    }
}