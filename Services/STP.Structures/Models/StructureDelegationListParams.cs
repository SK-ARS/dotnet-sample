using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StructureDelegationListParams
    {
        public long organisationId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string searchType { get; set; }
        public string searchValue { get; set; }
    }
}