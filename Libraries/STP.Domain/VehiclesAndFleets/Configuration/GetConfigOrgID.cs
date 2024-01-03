using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class ConfigOrgIDParams
    {
        public int presetFilter;
        public int? sortOrder;

        public int OrganisationId { get; set; }
        public int MovementType { get; set; }
        public int MovementType1 { get; set; }
        public string UserSchema { get; set; }
        public int FilterFavouritesVehConfig { get; set; }
    }
}