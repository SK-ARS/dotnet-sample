using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class UpdateNotifPlanRouteParam
    {
        public int RoutePartId { get; set; }
        public string Contentrefno { get; set; }
        public int RoutePartNo { get; set; }
        public int ImportVeh { get; set; }
        public int Flag { get; set; } 
    }
}