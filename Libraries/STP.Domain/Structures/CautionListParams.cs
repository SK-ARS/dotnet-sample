using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class CautionListParams
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long StructureId { get; set; }
        public long SectionId { get; set; }
    }
}