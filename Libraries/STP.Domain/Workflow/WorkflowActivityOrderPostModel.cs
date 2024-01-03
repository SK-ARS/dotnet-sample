using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
    public class WorkflowActivityOrderPostModel
    {
        public List<WorkflowActivityOrder> activityOrder { get; set; }
        public string collaborationId { get; set; }
    }
}
