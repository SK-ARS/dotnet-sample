using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    //Structure(Struct) Imposed(Impos) Parameters (Params)
    public class StuctImposedParams
    {
        public ImposedConstraints StructImposConstraints { get; set; }
        public int StructId { get; set; }
        public int SectionId { get; set; }
        public int StructType { get; set; }
    }

    public class EditDimensionParams
    {
        public DimensionConstruction StructureDimension { get; set; }
        public int StructureId { get; set; }
        public int SectionId { get; set; }
    }
}