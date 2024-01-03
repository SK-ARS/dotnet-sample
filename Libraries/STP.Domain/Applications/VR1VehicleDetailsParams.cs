using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class VR1VehicleDetailsParams
    {
        public ApplyForVR1 VR1Application { get; set; }
        public string ContentNo { get; set; }
        public string UserSchema { get; set; }
        public int Historic { get; set; }
    }
}