using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class UpdateSVParams
    {
        public double? WithLoad { get; set; }
        public int VehicleType { get; set; }
        public double? WithoutLoad { get; set; }
        public long SectionId { get; set; }
        public long StructId { get; set; }
        public int SVDerivation { get; set; }
        public string UserName { get; set; }
        public int ManualFlag { get; set; }
        public double? HbWithLoad { get; set; }
        public double? HbWithoutLoad { get; set; }
    }
}