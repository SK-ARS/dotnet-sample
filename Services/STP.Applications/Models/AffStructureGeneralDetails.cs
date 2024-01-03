using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
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

        public long organisationId { get; set; }

        public long lStructureChainNo { get; set; }
        public int iStructurePosition { get; set; }

        //Structure Imposed Constraints detail -- all signed values
        public double SAXLE_GROUP_LEN { get; set; }
        public double SAXLE_GROUP_WEIGHT { get; set; }
        public double SDOUBLE_AXLE_WEIGHT { get; set; }
        public double SGROSS_WEIGHT { get; set; }
        public double SHEIGHT_FEET { get; set; }
        public double SHEIGHT_METRES { get; set; }
        public double SLEN_FEET { get; set; }
        public double SLEN_METRES { get; set; }
        public double SSINGLE_AXLE_WEIGHT { get; set; }
        public double STRIPLE_AXLE_WEIGHT { get; set; }
        public double SWIDTH_FEET { get; set; }
        public double SWIDTH_METRES { get; set; }

    }

    public class AffStructureSectionList
    {
        public long StructureId { get; set; }
        public long SectionId { get; set; }
        public string StructureSections { get; set; }

    }

}