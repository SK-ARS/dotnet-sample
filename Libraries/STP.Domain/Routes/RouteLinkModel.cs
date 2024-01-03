using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.Routes
{
    public class RouteLinkModel
    {
        public long LinkId { get; set; }
        public long RefInId { get; set; }
        public long NrefInId { get; set; }
        public string DirTravel { get; set; }
        public short FuncClass { get; set; }
        public sdogeometry RoadGeometry { get; set; }
        public int Lrs { get; set; }
        public string IsRoundabout { get; set; }
        public string Description { get; set; }
    }

    public class RouteLinkResult
    {
        public long LinkId { get; set; }
        public int LinkNo { get; set; }
        public int Direction { get; set; }
        public long LastNode { get; set; }
    }

    public class GPXInputModel
    {
        public string RouteGPX { get; set; }
        public long NotificationId { get; set; }
        public long RevisionId { get; set; }
    }

    public class ProcessedRouteModel
    {
        public List<RouteLinkResult> PathResultLinks { get; set; }
        public List<RouteSegment> RouteSegmentList { get; set; }
        public bool RouteComplete { get; set; }

        public ProcessedRouteModel()
        {
            PathResultLinks = new List<RouteLinkResult>();
            RouteSegmentList = new List<RouteSegment>();
            RouteComplete = false;
        }
    }

    public class ProcessedWaypointModel
    {
        public RouteLinkModel StartLink { get; set; }
        public RouteLinkModel EndLink { get; set; }
        public List<RouteLinkModel> WaypointLinks { get; set; }
        public bool WaypointComplete { get; set; }
        public ProcessedWaypointModel()
        {
            StartLink = null;
            EndLink = null;
            WaypointLinks = new List<RouteLinkModel>();
            WaypointComplete = true;
        }
    }
}
