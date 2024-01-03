using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class CRDetails
    {
        public long RoutePartID { get; set; }
        public string RouteName { get; set; }
        public decimal SegmentNumber { get; set; }
    }
}