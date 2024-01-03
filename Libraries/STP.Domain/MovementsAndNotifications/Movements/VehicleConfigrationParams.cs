using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class VehicleConfigrationParams
    {
        public string Mnemonic { get; set; }

        public string ESDALRefNum { get; set; }

        public string Version { get; set; }

        public long NotificationId { get; set; }

        public int IsSimplified { get; set; }
    }
}