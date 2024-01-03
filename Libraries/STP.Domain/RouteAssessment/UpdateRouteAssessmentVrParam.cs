using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RouteAssessment
{
    public class UpdateRouteAssessmentVrParam
    {
        public int VersionId { get; set; }
        public int OrganisationId { get; set; }
        public int AnalysisId { get; set; }
        public int AnalysisType { get; set; }
        public string UserSchema { get; set; }
    }
}