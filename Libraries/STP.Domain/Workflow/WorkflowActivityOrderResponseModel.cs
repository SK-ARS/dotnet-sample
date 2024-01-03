using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
   public class WorkflowActivityOrderResponseModel
    {
        public int id { get; set; }
        public string collaborationId { get; set; }
        public string activityKey { get; set; }
        public string defaultOrder { get; set; }
        public double defaultOrderDouble { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
