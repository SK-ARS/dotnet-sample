using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.Models
{
    public class ApplicationRouteNameParams
    {
        public string RouteName { get; set; }

        public int RevisionId { get; set; }

        public string ContentRefNo { get; set; }

        public int RouteFor { get; set; }

        public string UserSchema { get; set; }
    }
}