using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class MovementActionInputParams
    {
        public string MovDescrp { get; set; }

        public string userSchema { get; set; }

        public MovementActionIdentifiers MovactAction { get; set; }
    }
}