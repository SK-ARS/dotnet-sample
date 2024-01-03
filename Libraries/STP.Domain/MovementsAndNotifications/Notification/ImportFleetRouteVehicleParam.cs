using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class ImportFleetRouteVehicleParam
    {
        public int VehicleId { get; set; }
        public string ContentReferenceNo { get; set; }
        public int Simple { get; set; }
        public int RoutePartId { get; set; }
    }
}