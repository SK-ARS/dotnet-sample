using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Workflow
{
    public class WorkflowProcessModel
    {
        public string PROCESS_ID { get; set; }
        public string WORKFLOW_KEY { get; set; }
        public long WORKFLOW_ID { get; set; }
        public decimal WFP_ESDALKEY { get; set; }
        public bool WFP_STATUS { get; set; }
        public long WORKFLOW_TYPE { get; set; }

    }
}