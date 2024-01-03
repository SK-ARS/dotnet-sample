using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.RouteAssessment
{
    public class NenRouteAssessmentParams
    {
        public int NotificationId { get; set; }
        public int InboxItemId { get; set; }
        public int AnalysisType { get; set; }
        public int OrganisationId { get; set; }
        public string UserSchema { get; set; }
        
    }
}
