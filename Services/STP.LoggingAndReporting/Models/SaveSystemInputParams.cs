using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    public class SaveSystemInputParams
    {
        public MovementActionIdentifiers movementactionidentifier { get; set; }
        public string SysDescrp { get; set; }
        public int userid { get; set; }
        public string userschema { get; set; }

    }
}