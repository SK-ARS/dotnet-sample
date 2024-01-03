using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class CautionListParams
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public long structureID { get; set; }
        public long SectionID { get; set; }
    }
}