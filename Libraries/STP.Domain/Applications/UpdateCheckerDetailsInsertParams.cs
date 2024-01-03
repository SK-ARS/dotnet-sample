using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class UpdateCheckerDetailsInsertParams
    {        
        public int ProjectId { get; set; }
        public int CheckerId { get; set; }
        public int CheckerStatus { get; set; }
    }
}