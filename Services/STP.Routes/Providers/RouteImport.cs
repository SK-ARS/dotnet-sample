using NetSdoGeometry;
using STP.Domain.Routes;
using STP.Routes.Interface;
using STP.Routes.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static STP.Domain.Routes.RouteModel;

namespace STP.Routes.Providers
{
    public sealed class RouteImport : IRouteImport
    {
        #region RouteExport Singleton
        private RouteImport()
        {
        }
        public static RouteImport Instance
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
            internal static readonly RouteImport instance = new RouteImport();
        }
        #endregion

        public List<RouteLinkModel> GetLinkFromGPXTrackPoint(sdogeometry pointGeom, string description)
        {
            return RouteImportDao.GetLinkFromGPXTrackPoint(pointGeom, description);
        }
        public List<RouteLinkModel> GetRouteFromGPXTrack(sdogeometry routeGeom, int tolerance)
        {
            return RouteImportDao.GetRouteFromGPXTrack(routeGeom, tolerance);
        }

        public RouteLinkModel GetLinkForAutoReplanPoint(sdogeometry point, string description)
        {
            return RouteImportDao.GetLinkForAutoReplanPoint(point, description);
        }

        public sdogeometry GetTransformedGeometry(sdogeometry routeGeom)
        {
            return RouteImportDao.GetTransformedGeometry(routeGeom);
        }

        public List<RoutePoint> GetRoutePointForReplan(RouteSegment routeSegment, string userSchema)
        {
            return RouteImportDao.GetRoutePointForReplan(routeSegment, userSchema);
        }
    }
}