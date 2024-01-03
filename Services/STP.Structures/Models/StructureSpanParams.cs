using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StructureSpanParams
    {
        public SpanData objSpanData { get; set; }
        public long StructureID { get; set; }
        public long SectionID { get; set; }
        public int editSaveFlag { get; set; }
    }
}