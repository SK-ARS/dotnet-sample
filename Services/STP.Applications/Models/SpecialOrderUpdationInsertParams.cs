using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SpecialOrderUpdationInsertParams
    {       
        public int ProjectID { get; set; }
        public int Status { get; set; }
        public int MovmentVersionNumber { get; set; }
    }
}