using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RoutePlannerInterface
{
    public class RouteData
    {
        public List<UInt32> ListSegments { get; set; }
        public string ResponseMessage { get; set; }
        public RouteData()
        {
            ListSegments = new List<UInt32>();
        }
    }
}