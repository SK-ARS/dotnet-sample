using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RouteAssessment
{
    public class RouteAssessmentDIParam
    {
        public RouteAssessmentInputs Inputs { get; set; }
        public int AnalType { get; set; }
        public string UserSchema { get; set; }
    }
}