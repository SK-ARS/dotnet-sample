using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using NetSdoGeometry;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using static STP.Routes.Models.RouteModel;

namespace STP.Routes.Models
{
    public class SavePlannedRoutePathParams
    {
        public RoutePart routePart { get; set; }
        public string userSchema { get; set; }
    }
    public class SaveRouteAnnotationParams
    {
        public RoutePart routePart { get; set; }
        public int type { get; set; }
        public string userSchema { get; set; }
    }
    public class AddToLibraryParams
    {
        public int organisationID { get; set; }
        public long routeId { get; set; }
        public string rtType { get; set; }
        public string userSchema { get; set; }
    }
}