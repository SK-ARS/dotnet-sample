using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class UpdateNotifRouteVehicleParams
    {
        public NotificationGeneralDetails NotificationGeneralDetails { get; set; }
        public int RoutePartId { get; set; }
        public int VehicleUnits { get; set; }
    }
}