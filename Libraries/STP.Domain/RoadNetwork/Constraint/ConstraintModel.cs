using NetSdoGeometry;
using STP.Domain.RouteAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Domain.RoadNetwork.Constraint
{
    public class ConstraintModel
    {
        public int UserId { get; set; }
        public string OrganisationName { get; set; }
        public string FirstName { get; set; }
        public long ConstraintId { get; set; }
        public string ConstraintCode { get; set; }

        public string ConstraintName { get; set; }
        public string HdnConstraintName { get; set; }


        public decimal MaxGrossWeightKgs { get; set; }
        public decimal HdnMaxGrossWeightKgs { get; set; }
        public long HdnGrossWeightKgs { get; set; }

        public decimal MaxAxleWeightKgs { get; set; }
        public decimal HdnMaxAxleWeightKgs { get; set; }
        public long HdnAxleWeightKgs { get; set; }

        public float MaxHeightMtrs { get; set; }
        public float HdnMaxHeightMtrs { get; set; }

        public float MaxHeightFT { get; set; }
        public float MaxHeightInch { get; set; }

        public float MaxWidthMeters { get; set; }
        public float HdnMaxWidthMeters { get; set; }

        public float MaxWidthFeet { get; set; }
        public float MaxWidthInch { get; set; }


        public float MaxLENMeters { get; set; }
        public float HdnMaxLenMeters { get; set; }

        public float MaxLengthFeet { get; set; }
        public float MaxLengthInch { get; set; }

        public float MinSpeedKph { get; set; }
        public float MinSpeedUnit { get; set; }

        public int UserType { get; set; }
        public int ConstraintType { get; set; }
        public int TraversalType { get; set; }
        public int TopologyType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime HdnStartDate { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime HdnEndDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public int IsDeleted { get; set; }
        public long OrganisationId { get; set; }

        public int OwnerIsContact { get; set; }
        public decimal vaildconst { get; set; }
        public int IsNodeConstraint { get; set; }
        public string Mode { get; set; }
        public string OwnerName { get; set; }
        public IEnumerable<SelectListItem> Direction { get; set; }

        public string DirectionName { get; set; }
        public int DirectionId { get; set; }
        public int HdnDirectionId { get; set; }

        public string ConstraintTypeName { get; set; }
        public int ConstraintTypeId { get; set; }
        public int HdnConstraintTypeId { get; set; }
        public IEnumerable<SelectListItem> ConstraintTypeList { get; set; }

        public bool OwnerIsContactFlag { get; set; }
        public bool IsNodeConstraintFlag { get; set; }
        public bool HdnOwnerIsContactFlag { get; set; }
        public bool HdnIsNodeConstraintFlag { get; set; }

        public long CautionId { get; set; }
        public string CautionName { get; set; }
        public string SpecificAction { get; set; }
        public long OwnerOrganisationId { get; set; }

        public long RevisionId { get; set; }
        public Int16 RevisionNo { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime MovementStartDate { get; set; }
        public DateTime MovementEndDate { get; set; }

        public string HaulierName { get; set; }
        public string ESDALReference { get; set; }
        public decimal CountRecord { get; set; }
        public int ApplicationStatus { get; set; }
        public long VersionId { get; set; }

        public bool StandardCaution { get; set; }
        public bool SpecificActionBool { get; set; }
        public string SpecificActionXML { get; set; }
        public decimal TotalRecordCount { get; set; }

        public bool CreatorIsContact { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool UnderLine { get; set; }

        public double MaxGrossWeight { get; set; }
        public long MaxGrossWeightUnit { get; set; }

        public double MaxAxleWeight { get; set; }
        public long MaxAxleWeightUnit { get; set; }

        public float MaxHeight { get; set; }
        public long MaxHeightUnit { get; set; }

        public long MaxLengthUnit { get; set; }
        public float MaxLengthMeters { get; set; }
        public float MaxLength { get; set; }

        public long MaxWidthUnit { get; set; }

        public long MaxSpeedUnit { get; set; }
        public long MaxSpeedKph { get; set; }

        public float MaxWidth { get; set; }

        public float MinSpeed { get; set; }
        public int FEasting { get; set; }
        public int FNorthing { get; set; }

        public ActionType SelectedType { get; set; }
        public string SelectedTypeName { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string HdnStartDateString { get; set; }
        public string HdnEndDateString { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string Speed { get; set; }

        public string HdnHeight { get; set; }
        public string HdnWidth { get; set; }
        public string HdnLength { get; set; }

        public DateTime OccurredTime { get; set; }
        public string HistoryDetails { get; set; }

        public sdogeometry Geometry { get; set; }
        public string AreaGeomStructure { get; set; }
        public List<RouteCautions> CautionList { get; set; }

        public List<ConstraintReferences> ConstraintReferences { get; set; }

        public List<AssessmentContacts> ConstraintContact { get; set; }
        public double GrossWeight { get; set; }
        public double AxleWeight { get; set; }

        public ConstraintModel()
        {
            CautionList = null;
            ConstraintReferences = null;
            ConstraintContact = null;
        }


    }


    public class ConstraintContactModel
    {

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

        public long ConstraintId { get; set; }

        public decimal TotalRecordCount { get; set; }

        public object OrganisationId { get; set; }

        public object RoleType { get; set; }

        public object IsAdHoc { get; set; }

        public object UserSchema { get; set; }

    }

    public class ConstraintSearchModel
    {
        public string SearchColumn { get; set; } //For search panel id , value pair
        public string SearchValue { get; set; } //For search panel id , value pair
    }

    public class ConstraintReport
    {
        public string DisplayText { get; set; }
    }
    public class ConstraintLogModel
    {
        public double AuditId { get; set; }
        public string ConstraintCode { get; set; }
        public TimeSpan OccuredTime { get; set; }
        public string Author { get; set; }
        public string Amendment1 { get; set; }
        public string Amendment2 { get; set; }
        public string Amendment3 { get; set; }
    }
    public class ConstrainteListParams
    {
        public int OrganisationId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SearchConstraintsFilter ObjSearchConstraintsFilter { get; set; }
    }
    public class DropDown
    {
        //Dropdown Id
        public int Id { get; set; }
        //Dropdown Value
        public string Value { get; set; }
    }
    
    public class SearchConstraintsFilter
    {
        public string ConstraintName { get; set; }
        public string ConstraintType { get; set; }
        public string ConstraintCode { get; set; }
        public bool IsValid { get; set; }
        public bool IsOwnerContact { get; set; }
        public int sortOrder { get; set; }
        public int presetFilter { get; set; }
    }
}

public enum ActionType
{
    SpecificAction, 
    StandardCaution
}