using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class AppRouteList
    {
        public long routeID { get; set; }
        public string routeName { get; set; }
        public string routetype { get; set; }
        public string routedescr { get; set; }
        public string transportmode { get; set; }
        public string RoutePart { get; set; }
        public long partno { get; set; }
        public int rid { get; set; }
        public int pno { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public long newPartNo { get; set; }
        public string NEN_route_status { get; set; }
    }
}