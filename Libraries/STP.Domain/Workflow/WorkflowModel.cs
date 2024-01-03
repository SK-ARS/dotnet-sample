using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Workflow
{
    public class WorkflowModel
    {
        public long WORKFLOW_ID { get; set; }
        public string WORKFLOW_NAME { get; set; }
        public string WORKFLOW_ENGINE_START_PROCESS_NAME { get; set; }
        public string  WF_MODULE { get; set; }
        public bool WF_STATUS { get; set; }
    }
}