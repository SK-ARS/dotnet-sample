using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Common.Constants;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class CheckWheelParams
    {
        public int VehicleId { get; set; }
        public string UserSchema { get; set; } = Common.Constants.UserSchema.Portal;
        public int ApplicationRevisionId { get; set; } = 0;
        public bool IsNotif { get; set; } = false;
        public bool IsVR1 { get; set; } = false;
    }
}