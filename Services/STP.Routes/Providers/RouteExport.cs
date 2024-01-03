using STP.Domain.ExternalAPI;
using STP.Domain.Routes;
using STP.Routes.Interface;
using STP.Routes.Persistance;
using System.Collections.Generic;
using System.Diagnostics;
using static GpxLibrary.ConvertGpx;

namespace STP.Routes.Provider
{
    public sealed class RouteExport : IRouteExport
    {
        #region RouteExport Singleton
        private RouteExport()
        {
        }
        public static RouteExport Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly RouteExport instance = new RouteExport();
        }
        #endregion

        public RouteExportList GetRouteList(int organisationId, int pageNumber, int pageSize)
        {
            return RouteExportDao.GetRouteList(organisationId, pageNumber, pageSize);
        }
        public List<WayPoints> GetWayPoints(long plannedRouteID, int organisationId, bool isApp, string userSchema)
        {
            return RouteExportDao.GetWayPoints(plannedRouteID, organisationId, isApp, userSchema);
        }
        public List<TrackPoints> GetTrackPoints(long plannedRouteID, int organisationId, bool isApp, string userSchema)
        {
            return RouteExportDao.GetTrackPoints(plannedRouteID, organisationId, isApp, userSchema);
        }
        public List<ExportRouteList> ExportRouteList(GetRouteExportList routeExportList)
        {
            return RouteExportDao.ExportRouteList(routeExportList);
        }
        public CheckRouteExportable CheckIsRouteExportable(long routeId, bool isApp, string userSchema)
        {
            return RouteExportDao.CheckIsRouteExportable(routeId, isApp, userSchema);
        }
    }
}