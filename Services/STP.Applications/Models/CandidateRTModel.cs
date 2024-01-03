using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class CandidateRTModel
    {
        public long RouteID { get; set; }
        public string Name { get; set; }
        public long AnalysisID { get; set; }
        public long RevisionID { get; set; }
        public int RevisionNo { get; set; }
        public string CandidateDate { get; set; }       
    }
}