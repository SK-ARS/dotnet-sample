using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class StructureDelegationListParams
    {
        public long OrganisationId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchType { get; set; }
        public string SearchValue { get; set; }
    }
}