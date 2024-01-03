using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class UpdateSORTSpecialOrderParams
    {
        public SORTSpecialOrder SORTSpecialOrder { get; set; }
        public List<string> RemovedCoverages { get; set; }
    }
}