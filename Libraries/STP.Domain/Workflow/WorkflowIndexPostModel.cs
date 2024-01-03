using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
    public class WorkflowIndexPostModel
    {
        //string module, int pointOfCommunication, string dataModel
        public string module { get; set; }
        public int pointOfCommunication { get; set; }
        public string dataModel { get; set; }
    }
}
