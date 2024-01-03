using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
    public class WorkflowMapper
    {
        public static Dictionary<string, string> WF_Names { get; set; }
        public WorkflowMapper()
        {
            WF_Names = new Dictionary<string, string>
            {
                { "Process_SORTSOApplication", "Process_ManualEntryHaulierDetailsSORT" },
                { "Process_SORTVr1ApplicationApproval", "Process_AllocateApplication2RoutingOfficersSORTVR1" }
            };
        }
        public string UpdateName(string processName)
        {
            return WF_Names.Where(x => x.Key == processName).Select(x => x.Value).SingleOrDefault();
        }
    }
}
