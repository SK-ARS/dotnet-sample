using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RouteAssessment
{
    public class UpdateAssessmentModelParam
    {
        public RouteAssessmentModel RouteAssessmentModel { get; set; }
        public int AnalysisId { get; set; }
        public string UserSchema { get; set; }
    }
}