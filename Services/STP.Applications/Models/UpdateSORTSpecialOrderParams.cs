using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateSORTSpecialOrderParams
    {
        public SORTSpecialOrder SortSpecialOrders { get; set; }
        public List<string> RemovedCoverages { get; set; }
    }
}