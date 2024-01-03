using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class RouteRevisionInsertParams
    {        
        public long RouteID { get; set; }
        public string RouteType { get; set; } = string.Empty;
    }
}