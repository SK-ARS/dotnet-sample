using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class VerificationStatusParams
    {
        public int routeId { get; set; }

        public int isLib { get; set; }

        public int replanStatus { get; set; }

        public string userSchema { get; set; }
    }
}