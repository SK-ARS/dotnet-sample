using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class FleetComponenentModel
    {
        public int ComponentId { get; set; }
        public int OrganisationId { get; set; }
        public int Flag { get; set; }
        public string UserSchema { get; set; }

    }
}