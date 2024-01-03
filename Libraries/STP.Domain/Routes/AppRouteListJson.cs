using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Routes
{

    public class AppRouteListJson
    {
        [JsonProperty("RouteID")]
        public long routeID { get; set; }
        [JsonProperty("RouteName")]
        public string routeName { get; set; }
        [JsonProperty("RouteType")]
        public string routetype { get; set; }
        [JsonProperty("RouteDescription")]
        public string routedescr { get; set; }
        [JsonProperty("TransportMode")]
        public string transportmode { get; set; }
        [JsonProperty("RoutePart")]
        public string RoutePart { get; set; }
        [JsonProperty("PartNo")]
        public long partno { get; set; }
        [JsonProperty("RID")]
        public int rid { get; set; }
        [JsonProperty("PNo")]
        public int pno { get; set; }
        [JsonProperty("FromAddress")]
        public string fromAddress { get; set; }
        [JsonProperty("ToAddress")]
        public string toAddress { get; set; }
        [JsonProperty("NewPartNo")]
        public long newPartNo { get; set; }
        [JsonProperty("NENRouteStatus")]
        public string NEN_route_status { get; set; }
    }
}
