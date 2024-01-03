using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class SaveNotificationRouteParam
    {
        public int routePartId { get; set; }
        public int versionId { get; set; }
        public string contentRefNo { get; set; }
        public int routeType { get; set; }
    }
}