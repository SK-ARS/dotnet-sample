using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Workflow
{
    public class WorkflowActivityModel
    {
        public long ACTIVITY_ID { get; set; }
        public long WORKFLOW_ID { get; set; }
        public string ACTIVITY_NAME { get; set; }
        public string WFA_ROUTEURL { get; set; }
        public Single WFA_ACTIVITYORDER { get; set; }
        public bool WFA_STATUS { get; set; }
    }
}