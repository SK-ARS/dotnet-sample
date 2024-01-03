using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
   public class SaveMovementActionParam
    {
        public string esdalRef { get; set; }
        public int movementActionType { get; set; }
        public string movementDescription { get; set; }
        public long projectId { get; set; }
        public int revisionNo { get; set; }
        public int versionNo { get; set; }
        public string userSchema { get; set; }
        
    }
}
