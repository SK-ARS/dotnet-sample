using STP.Domain.LoggingAndReporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class MovementActionInsertParams
    {       
        public MovementActionIdentifiers MovementActionIdentifier { get; set; }
        public string MovementDescription { get; set; }
        public string UserSchema { get; set; }
    }
}