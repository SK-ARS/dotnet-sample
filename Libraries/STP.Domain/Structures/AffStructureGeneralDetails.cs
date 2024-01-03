using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class AffStructureGeneralDetails
    {
        public long StructureId { get; set; }
        public string ESRN { get; set; }
        public string StructureClass { get; set; }
        public long OSGR { get; set; }
        public long Easting { get; set; }
        public string OwnerName { get; set; }
        public string Notes { get; set; }
        public string StructureName { get; set; }
        public long ContactId { get; set; }
        public string StructureCategory { get; set; }
        public string StructureType { get; set; }
        public string StructureType1 { get; set; }
        public string StructureType2 { get; set; }
        public double StructureLength { get; set; }
        public string StructureDescription { get; set; }
        public string AlternativeName { get; set; }

        public long OrganisationId { get; set; }

        public long StructureChainNo { get; set; }
        public int StructurePosition { get; set; }

        //Structure Imposed Constraints detail -- all signed values
        public double SignedAxleGroupLength { get; set; }
        public double SignedAxleGroupWeight { get; set; }
        public double SignedDoubleAxleWeight { get; set; }
        public double SignedGrossWeight { get; set; }
        public double SignedHeightInFeet { get; set; }
        public double SignedHeightInMetres { get; set; }
        public double SignedLengthInFeet { get; set; }
        public double SignedLengthInMetres { get; set; }
        public double SignedSingleAxleWeight { get; set; }
        public double SignedTripleAxleWeight { get; set; }
        public double SignedWidthInFeet { get; set; }
        public double SignedWidthInMetres { get; set; }
        public string StructureKey { get; set; }

    }

    public class AffStructureSectionList
    {
        public long StructureId { get; set; }
        public long SectionId { get; set; }
        public string StructureSections { get; set; }

    }

}