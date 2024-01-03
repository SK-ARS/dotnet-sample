using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class UpdateNotifPlanRouteParam
    {
        public int RoutePartId { get; set; }
        public string ContentReferenceNo { get; set; }
        public int RoutePartNo { get; set; }
        public int ImportVehicle { get; set; }
        public int Flag  { get; set; }
}

}