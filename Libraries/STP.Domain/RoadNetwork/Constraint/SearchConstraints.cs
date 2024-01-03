using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.RoadNetwork.Constraint
{
    public class SearchConstraints
    {
        public string SearchSummaryId { get; set; }
        public string SearchSummaryName { get; set; }
        public string AlternateName { get; set; }
        public string Description { get; set; }
        public string Carries { get; set; }
        public string Crosses { get; set; }
        public string StructureType { get; set; }
        public string ICAMethod { get; set; }
        public string DelegateName{ get; set; } 
    }
}