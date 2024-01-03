using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StructureListParams
    {
        public int orgId { get; set; }
        public int pageNum { get; set; }
        public int pageSize { get; set; }
        public SearchStruct objSearchStruct { get; set; }
    }
}