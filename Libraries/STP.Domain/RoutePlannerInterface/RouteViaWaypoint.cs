using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RoutePlannerInterface
{
    public class RouteViaWaypoint
    {
        public string StartPoint { get; set; }
        public List<string> WayPoints { get; set; }
        public string EndPoint { get; set; }
        public string MaxHeight { get; set; }
        public string MaxWeight { get; set; }
        public string MaxLength { get; set; }
        public string MaxWidth { get; set; }
        public string MaxNormAxleLoad { get; set; }
        public string MaxShutAxleLoad { get; set; }
    }
}