using STP.Domain;
using STP.Domain.RoutePlannerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.RoutePlannerInterface
{
    public interface IRoutePlannerInterfaceService
    {
        RouteData GetRouteData(RouteViaWaypointEx routeViaPointEx);
    }
}
