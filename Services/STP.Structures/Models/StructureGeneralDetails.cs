using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StructureGeneralDetails
    {
        public string ESRN { get; set; }
        public string StructureClass { get; set; }
        public long OSGRNorth { get; set; }
        public long OSGREast { get; set; }
        public string StructureOwner { get; set; }
        public string StructureName { get; set; }
        public string StructureAlternateNameOne { get; set; }
        public string StructureAlternateNameTwo { get; set; }
        public string StructureAlternateNameThree { get; set; }
        public string StructureKey { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string CategoryCode { get; set; }
        public string Category { get; set; }
        public string CategoryUserDefined { get; set; }
        public string Type { get; set; }
        public string TypeOne { get; set; }
        public string TypeTwo { get; set; }
        public string TypeCode { get; set; }
        public string TypeOneCode { get; set; }
        public string TypeTwoCode { get; set; }
        public string TypeUserDefined { get; set; }
        public string TypeOneUserDefined { get; set; }
        public string TypeTwoUserDefined { get; set; }
        public int SubTypeOne { get; set; }
        public int SubTypeTwo { get; set; }
        public double Length { get; set; }
        public long ContactId { get; set; }
        //required for Edit General Details page
        public long OrganisationId { get; set; }
        public long StructureId { get; set; }


        //For Structure Dimensions And Construction
        public string DimConDescription { get; set; }
        public string DimConObjectCarried { get; set; }
        public string DimConObjectCrossed { get; set; }

        public string LevCroChainage { get; set; }
        public string UserName { get; set; }
        public long OwnerID { get; set; }
    }

    public class StructureSectionList
    {
        public long SectionId { get; set; }
        public string StructureSections { get; set; }
        public int affectFlag { get; set; }

    }

    public class StructType
    {
        public long StructureTypeId { get; set; }
        public string StrustureType { get; set; }
    }

    public class StructCategory
    {
        public long StructureCatId { get; set; }
        public string StructureCat { get; set; }
    }
}