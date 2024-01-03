using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class SaveConfigurationModel
    {
        public int ConfigurationId { get; set; }
        public string UserSchema { get; set; } = Common.Constants.UserSchema.Portal;
        public int ApplicationRevisionId { get; set; } = 0;
        public bool IsNotif { get; set; } = false;
        public bool IsVR1 { get; set; } = false;

    }
}