using STP.Domain.LoggingAndReporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class MovementActionIdentifiersParams
    {
        public string MovementDescription { get; set; }

        public string UserSchema { get; set; }

        public MovementActionIdentifiers MovementAction { get; set; }
    }
}
