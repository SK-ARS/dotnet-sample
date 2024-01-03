using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Configuration
{
    public class UpdateNotifRouteVehicleParams
    {
        public NotificationGeneralDetails notificationGeneralDetails { get; set; }
        public int routePartId { get; set; }
        public int vehicleUnits { get; set; }
    }
}