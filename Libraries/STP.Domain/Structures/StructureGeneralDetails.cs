using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
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
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public string TypeCode { get; set; }
        public string Type1Code { get; set; }
        public string Type2Code { get; set; }
        public string TypeUserDefined { get; set; }
        public string Type1UserDefined { get; set; }
        public string Type2UserDefined { get; set; }
        public int SubType1 { get; set; }
        public int SubType2 { get; set; }
        public double Length { get; set; }
        public long ContactId { get; set; }
        //required for Edit General Details page
        public long OrganisationId { get; set; }

        public long StructureChainNo { get; set; }
        public int StructurePosition { get; set; }
        public long StructureId { get; set; }


        //For Structure Dimensions(Dim) And Construction(Con)
        public string DimConDescription { get; set; }
        public string DimConObjectCarried { get; set; }
        public string DimConObjectCrossed { get; set; }

        public string LevelCrossChainage { get; set; }
        public string UserName { get; set; }
        public long OwnerId { get; set; }
    }

    public class StructureSectionList
    {
        public long SectionId { get; set; }
        public string StructureSections { get; set; }
        public int AffectFlag { get; set; }
        public string VehicleName { get; set; }

    }

    public class StructType
    {
        public long StructureTypeId { get; set; }
        public string StrustureType { get; set; }
    }

    public class StructCategory
    {
        public long StructureCaegorytId { get; set; }
        public string StructureCategory { get; set; }
    }
    //class to show structure owner details in structure general page for affected structures.
    public class StructureOwnerChain
    {
        public long ChainPosition { get; set; }
        public long ChainNo { get; set; }
        public long ChainContactId { get; set; }
        public string ChainOwnerName { get; set; }
    }
}