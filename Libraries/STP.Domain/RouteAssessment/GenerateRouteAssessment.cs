using STP.Domain.RouteAssessment.XmlAnalysedStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.Routes.RouteModel;

namespace STP.Domain.RouteAssessment
{
    public class GenerateRouteAssessment
    {
        public List<RoutePartDetails> RoutePart { get; set; }
        public AnalysedStructures AnalysedStructures { get; set; }
        public int OrganisationId { get; set; }
        public string UserSchema { get; set; }
        public long NotificationId { get; set; }
        public int VSOType { get; set; }
    }

    public class UpdateAnalysedRoutes
    {
        public RouteAssessmentModel RouteAssess { get; set; }
        public long AnalysisId { get; set; }
        public string UserSchema { get; set; }
    }
}
