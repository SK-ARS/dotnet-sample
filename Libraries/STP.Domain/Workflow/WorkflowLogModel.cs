using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
   public class WorkflowLogModel
    {
        public string ActivityKey { get; set; }
        public DateTime ActivityOn { get; set; }
    }

    public class WorkflowLog
    {
        public List<WorkflowLogModel> WorkflowLogModels { get; set; }
    }
}
