using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class SaveApplicationRouteParams
    {
        public int routePartId { get; set; }
        public int appRevId { get; set; }
        public int routeType { get; set; }
        public string userSchema { get; set; }
        public string contentRef { get; set; }
        public int versionId { get; set; }
    }
}