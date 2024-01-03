using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class UpdateCandidateRouteNMInsertParams
    {      
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public long CandidateRouteId { get; set; }
    }
}