using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.RouteAssessment
{
    public class RouteAssessmentParams
    {
        public int routePartId { get; set; }
        public RoutePart routePart { get; set; }
        public string userSchema { get; set; }
    }
}
