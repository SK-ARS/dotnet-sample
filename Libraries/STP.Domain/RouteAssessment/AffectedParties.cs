using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.RouteAssessment
{
    public class AffectedParties
    {
        public int AnalysisId { get; set; }
        public int DistributedMovAnalysisId { get; set; }
        public RouteAssessmentInputs Inputs { get; set; }
        public byte[] AffectedPartie { get; set; }
        public string userSchema { get; set; }         
    }

    public class AffectedPartyBasedOnOrganisation
    {
        public RouteAssessmentModel RouteAssessmentModel { get; set; }
        public int AnalysisId { get; set; }
        public string UserSchema { get; set; }
    }
    public class RoadDistanceInfo
    {
        public string RoadName { get; set; }
        public int OrgId { get; set; }
        public int Distance { get; set; }
    }
}
