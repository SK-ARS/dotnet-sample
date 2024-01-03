using NetSdoGeometry;
using STP.Domain.Routes;
using System.Collections.Generic;
using static STP.Domain.Routes.RouteModel;

namespace STP.Routes.Interface
{
    public interface IRouteImport
    {
        List<RouteLinkModel> GetLinkFromGPXTrackPoint(sdogeometry pointGeom, string description);
        List<RouteLinkModel> GetRouteFromGPXTrack(sdogeometry routeGeom, int tolerance);
        RouteLinkModel GetLinkForAutoReplanPoint(sdogeometry point, string description);
        sdogeometry GetTransformedGeometry(sdogeometry routeGeom);
        List<RoutePoint> GetRoutePointForReplan(RouteSegment routeSegment, string userSchema);
    }
}
