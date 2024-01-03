using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class PlannedRouteListParams
    {
        public int organisationID { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int routeType { get; set; }
        public string serchString { get; set; }
        public string userSchema { get; set; }
    }
}