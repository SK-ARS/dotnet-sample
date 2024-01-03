using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SORTMvmtProjectDetailsInsertParams
    {
        public long ProjectId { get; set; }
        public string MovementName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Load { get; set; }
        public string HaulierJobReference { get; set; }
    }
}