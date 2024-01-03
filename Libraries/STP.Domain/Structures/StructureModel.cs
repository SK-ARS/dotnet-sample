using STP.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Domain.Structures
{
    public class StructureModel
    {

        public string OrganizationName { get; set; }
        public string FirstName { get; set; }
        public long StructureId { get; set; }
        public long SectionId { get; set; }
        public string StructureCode { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        public string StructureName { get; set; }
        public string HdnStructureName { get; set; }


        public decimal MaxGrossWeightKgs { get; set; }
        public long HDNMaxGrossWeightKgs { get; set; }

        public decimal MaxAxleWeightKgs { get; set; }
        public long HDNMaxAxleWeightKgs { get; set; }

        public float MaxHeightMetres { get; set; }
        public float HDNMaxHeightMetres { get; set; }

        public float MaxHeightFeet { get; set; }
        public float MaxHeightInch { get; set; }

        public float MaxWidthMetres { get; set; }
        public float HDNMaxWidthMetres { get; set; }

        public float MaxWidthFeet { get; set; }
        public float MaxWidthInch { get; set; }


        public float MaxLengthMetres { get; set; }
        public float HDNMaxLengthMetres { get; set; }

        public float MaxLengthFeet { get; set; }
        public float MaxLengthInch { get; set; }

        public float MinSpeedKmph { get; set; }
        public float MinSpeedUnit { get; set; }

        public int StructureType { get; set; }
        public int TraversalType { get; set; }
        public int TopologyType { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public DateTime HDNStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public DateTime HDNEndDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public int IsDeleted { get; set; }
        public long OrganisationId { get; set; }

        public int OwnerIsContact { get; set; }
        public int IsNodeConstraint { get; set; }

        public string Mode { get; set; }

        public string OwnerName { get; set; }

        [AllowHtml]
        public string DirectionName { get; set; }
        public int DirectionId { get; set; }
        public int HDNDirectionId { get; set; }

        public string StructureTypeName { get; set; }
        public int StructureTypeId { get; set; }
        public int HDNStructureTypeId { get; set; }

        public bool OwnerIsContactFlag { get; set; }
        public bool ISNodeStructureFlag { get; set; }
        public bool HDNOwnerIsContactFlag { get; set; }
        public bool HDNISNodeConstraintFlag { get; set; }

        public long CautionId { get; set; }
        public string CautionName { get; set; }
        public bool SpecificActionB { get; set; }
        public string SpecificActionXML { get; set; }
        public decimal TotalRecordCount { get; set; }

        public long RevisionId { get; set; }
        public Int16 RevisionNo { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime MovementStartDate { get; set; }
        public DateTime MovementEndDate { get; set; }

        public string HaulierName { get; set; }
        public string ESDALReference { get; set; }
        public decimal CountRec { get; set; }
        public int ApplicationStatus { get; set; }
        public long VersionId { get; set; }

        public bool StandardCaution { get; set; }
        public string SpecificAction { get; set; }
        public bool CreatorIsContact { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }

        public double GrossWeight { get; set; }
        public double MaxGrossWeight { get; set; }
        public long MaxGrossWeightUnit { get; set; }

        public double AxleWeight { get; set; }
        public double MaxAxleWeight { get; set; }
        public long MaxAxleWeightUnit { get; set; }

        public float MaxHeight { get; set; }
        public long MaxHeightUnit { get; set; }

        public long MaxLengthUnit { get; set; }
        public float MaxLengthInMetres { get; set; }
        public float MaxLength { get; set; }

        public long MaxWidthUnit { get; set; }

        public long MaxSpeedUnit { get; set; }
        public long MaxSpeedKmph { get; set; }

        public float MaxWidth { get; set; }

        public float MinSpeed { get; set; }

        public ActionType SelectedType { get; set; }
        public string SelectedTypeName { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string HDNStartDateString { get; set; }
        public string HDNEndDateString { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string Speed { get; set; }

        public string HDNHeight { get; set; }
        public string HDNWidth { get; set; }
        public string HDNLength { get; set; }

        public DateTime OccuredTime { get; set; }
        public string HistoryDetails { get; set; }
    }

    public class StructureContactModel
    {

        [Display(Name = "Content Name")]
        public string ContactName { get; set; }

        public string Description { get; set; }

        public long ContactId { get; set; }

        public short ContactNo { get; set; }

        public string FullName { get; set; }

        public string OrganisationName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string PostCode { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string Telephone { get; set; }

        public string Extension { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string EmailPreference { get; set; }

        public long CautionId { get; set; }

        public decimal TotalRecordCount { get; set; }

        public object OrganisationId { get; set; }

        public object RoleType { get; set; }

        public object IsAdHoc { get; set; }

        public object UserSchema { get; set; }

    }

    public class StructureSearchModel
    {
        public string SearchColumn { get; set; } //For search panel id , value pair
        public string SearchValue { get; set; } //For search panel id , value pair
    }

    public class StructureReport
    {
        public string DisplayText { get; set; }
    }
    public class StructureLogModel
    {
        public double AuditId { get; set; }
        public double StructureId { get; set; }
        public string StructureCode { get; set; }
        public TimeSpan OccuredTime { get; set; }
        public string Author { get; set; }
        public string Amendment1 { get; set; }
        public string Amendment2 { get; set; }
        public string Amendment3 { get; set; }
    }
}