using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class SearchStruct
    {
        public string searchSummaryId { get; set; }
        public string searchSummaryName { get; set; }
        public string alternateName { get; set; }
        public string description { get; set; }
        public string carries { get; set; }
        public string crosses { get; set; }
        public string structType { get; set; }
        public string ICAMethod { get; set; }
        public string delegateName { get; set; } 
    }
}