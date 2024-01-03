using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateCandidateRouteNMInsertParams
    {      
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public long CandidateRouteID { get; set; }
    }
}