using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SpecialOrderUpdationInsertParams
    {       
        public int ProjectId { get; set; }
        public int Status { get; set; }
        public int MovmentVersionNumber { get; set; }
    }
}