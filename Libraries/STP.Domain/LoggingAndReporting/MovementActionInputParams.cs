using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class MovementActionInputParams
    {
        public string MovementDescription { get; set; }

        public string UserSchema { get; set; }

        public MovementActionIdentifiers MovementActAction { get; set; }
    }
}