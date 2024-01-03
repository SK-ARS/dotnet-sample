using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModelJson;

namespace STP.Domain.Routes
{
    public class NenRouteModel
    {
        public List<RoutePointJson> RoutePointJsons { get; set; }
        public bool IsStartEndError;
        public string ErrorMoniker;
        public List<ErrorList> ErrorLists;
        public List<NonErrorList> NonErrorLists;
        public int WaypointCount;
        public int ErrorHigh;
    }
}
