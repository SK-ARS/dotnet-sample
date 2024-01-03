using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow.Models
{
   public class WorkflowActivityRoute
    {
//        [
//  {
//    "id": 2,
//    "processDefinition": "Process_SORTSOApplicationApprovalV08",
//    "activity": "Activity_AllocateApplication2RoutingOfficers",
//    "routeUrl": "../SORTApplication/AllocateSORTUser.",
//    "payload": {}
//}
//]

        public int id { get; set; }
        public string processDefinition { get; set; }
        public string activity { get; set; }
        public string routeUrl { get; set; }
        public object payload { get; set; }

    }
}
