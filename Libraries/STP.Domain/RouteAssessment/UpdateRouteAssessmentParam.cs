using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RouteAssessment
{
    public class UpdateRouteAssessmentParam
    {
        public string ContentReferenceNo { get; set; }
        public int RevisionId { get; set; }
        public int OrganisationId { get; set; }
        public int AnalysisId { get; set; }
        public int AnalysisType { get; set; }
        public string UserSchema { get; set; }
    }
}