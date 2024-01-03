using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class StructureSpanParams
    {
        public SpanData ObjSpanData { get; set; }
        public long StructureId { get; set; }
        public long SectionId { get; set; }
        public int EditSaveFlag { get; set; }
    }
}