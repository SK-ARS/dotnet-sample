using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StuctImposedParams
    {
        public ImposedConstraints StrucImpoConst { get; set; }
        public int StructId { get; set; }
        public int SectionId { get; set; }
        public int StructType { get; set; }
    }
}