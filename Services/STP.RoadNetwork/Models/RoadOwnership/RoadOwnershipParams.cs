using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSdoGeometry;

namespace STP.RoadNetwork.Models.RoadOwnership
{    
    public class FetchRoadInfoParams
    {
        public int organisationId { get; set; }
        public int fetchFlag { get; set; }
        public string areaGeomStr { get; set; }
        public int zoomLevel { get; set; }
    }    
}