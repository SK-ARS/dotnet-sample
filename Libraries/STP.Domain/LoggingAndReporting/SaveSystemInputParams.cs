using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class SaveSystemInputParams
    {
        public MovementActionIdentifiers MovementActionIdentifier { get; set; }
        public string SystemDescription { get; set; }
        public int UserId { get; set; }
        public string UserSchema { get; set; }

    }
}