using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class ConfigOrgIDParams
    {
        
        public int organisationId { get; set; }
        public int movtype { get; set; }
        public int movetype1 { get; set; }
        public string userSchema { get; set; }
    }
}