using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.RouteAssessment.AssessmentOutput
{
    public class UpdateRouteAssessmentSevenParams
    {
        public string ContentRefNo { get; set; }
        public int OrgId { get; set; }
        public int AnalysisId { get; set; }
        public int AnalType { get; set; }
        public string UserSchema { get; set; }
        public int RouteId { get; set; }
        public AssessmentOutput AssessmentResult { get; set; }

    }
}
