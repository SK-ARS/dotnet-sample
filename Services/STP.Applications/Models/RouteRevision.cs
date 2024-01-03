using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class RouteRevision
    {
        public RouteRevision()
        {
            this.RevisionId = 0;
            this.RevisionNumber = 0;
            this.NewAnalysisId = 0;
        }
        public long RevisionId { get; set; }
        public short RevisionNumber { get; set; }
        public long NewAnalysisId { get; set; }
    }
}