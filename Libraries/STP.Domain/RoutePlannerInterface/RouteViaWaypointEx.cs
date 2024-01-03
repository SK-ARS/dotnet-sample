using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RoutePlannerInterface
{
    public class RouteViaWaypointEx
    {
        public UInt32 BeginStartNode { get; set; } 
        public UInt32 BeginPointLinkId { get; set; } 
        public UInt32 BeginPointEndNode { get; set; } 
        public UInt32 EndPointStartNode { get; set; }   
        public UInt32 EndPointLinkId { get; set; } 
        public UInt32 EndPointEndNode { get; set; } 
        public List<WayPoint> WayPoints { get; set; }
        public string MaxHeight { get; set; }
        public string MaxWeight { get; set; }
        public string MaxLength { get; set; }
        public string MaxWidth { get; set; }
        public string MaxNormAxleLoad { get; set; }
        public string MaxShutAxleLoad { get; set; }
        public RouteViaWaypoint GetRouteViaPoint()
        {
            RouteViaWaypoint routeViaPoint = new RouteViaWaypoint();
            routeViaPoint.StartPoint = BeginStartNode + "";
            routeViaPoint.EndPoint = EndPointStartNode + "";

            List<string> strWaypoints = new List<string>();
            if (this.WayPoints != null)
            {
                foreach (WayPoint waypoint in this.WayPoints)
                {
                    strWaypoints.Add(waypoint.WayPointBeginNode + "");
                }
            }
            routeViaPoint.WayPoints = strWaypoints;
            routeViaPoint.MaxHeight = this.MaxHeight;
            routeViaPoint.MaxLength = this.MaxLength;
            routeViaPoint.MaxNormAxleLoad = this.MaxNormAxleLoad;
            routeViaPoint.MaxShutAxleLoad = this.MaxShutAxleLoad;
            routeViaPoint.MaxWeight = this.MaxWeight;
            routeViaPoint.MaxWidth = this.MaxWidth;
            return routeViaPoint;
        }
    }
}