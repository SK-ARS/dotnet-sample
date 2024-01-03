using STP.Domain.ExternalAPI;
using STP.Domain.Routes;
using System.Collections.Generic;
using static GpxLibrary.ConvertGpx;

namespace STP.Routes.Interface
{
    public interface IRouteExport
    {
        RouteExportList GetRouteList(int organisationId, int pageNumber, int pageSize);
        List<WayPoints> GetWayPoints(long plannedRouteID, int organisationId, bool isApp, string userSchema);
        List<TrackPoints> GetTrackPoints(long plannedRouteID, int organisationId, bool isApp, string userSchema);
        List<ExportRouteList> ExportRouteList(GetRouteExportList routeExportList);
        CheckRouteExportable CheckIsRouteExportable(long routeId, bool isApp, string userSchema);
    }
}
