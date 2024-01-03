using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateCheckerDetailsInsertParams
    {        
        public int ProjectID { get; set; }
        public int CheckerID { get; set; }
        public int CheckerStatus { get; set; }
    }
}