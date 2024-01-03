using STP.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StructureModel
    {

        public string ORGANISATION_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public long STRUCTURE_ID { get; set; }
        public long SECTION_ID { get; set; }
        public string STRUCTURE_CODE { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        public string STRUCTURE_NAME { get; set; }
        public string HDNSTRUCTURE_NAME { get; set; }


        public decimal MAX_GROSS_WEIGHT_KGS { get; set; }
        public long HDNMAX_GROSS_WEIGHT_KGS { get; set; }

        public decimal MAX_AXLE_WEIGHT_KGS { get; set; }
        public long HDNMAX_AXLE_WEIGHT_KGS { get; set; }

        public float MAX_HEIGHT_MTRS { get; set; }
        public float HDNMAX_HEIGHT_MTRS { get; set; }

        public float MAX_HEIGHT_FT { get; set; }
        public float MAX_HEIGHT_INCH { get; set; }

        public float MAX_WIDTH_MTRS { get; set; }
        public float HDNMAX_WIDTH_MTRS { get; set; }

        public float MAX_WIDTH_FT { get; set; }
        public float MAX_WIDTH_INCH { get; set; }


        public float MAX_LEN_MTRS { get; set; }
        public float HDNMAX_LEN_MTRS { get; set; }

        public float MAX_LEN_FT { get; set; }
        public float MAX_LEN_INCH { get; set; }

        public float MIN_SPEED_KPH { get; set; }
        public float MIN_SPEED_UNIT { get; set; }

        public int STRUCTURE_TYPE { get; set; }
        public int TRAVERSAL_TYPE { get; set; }
        public int TOPOLOGY_TYPE { get; set; }

        [DataType(DataType.Date)]
        public DateTime START_DATE { get; set; }
        public DateTime HDNSTART_DATE { get; set; }

        [DataType(DataType.Date)]
        public DateTime END_DATE { get; set; }
        public DateTime HDNEND_DATE { get; set; }

        public DateTime LAST_UPDATE { get; set; }

        public int IS_DELETED { get; set; }
        public long ORGANISATION_ID { get; set; }

        public int OWNER_IS_CONTACT { get; set; }
        public int IS_NODE_CONSTRAINT { get; set; }

        public string Mode { get; set; }

        public string OWNER_NAME { get; set; }        
        public string DIRECTION_NAME { get; set; }
        public int Direction_id { get; set; }
        public int HDNDirection_id { get; set; }

        public string STRUCTURE_TYPE_NAME { get; set; }
        public int StructureTypeID { get; set; }
        public int HDNStructureTypeID { get; set; }

        public bool OwnerIsContactFlag { get; set; }
        public bool ISNodeStructureFlag { get; set; }
        public bool HDNOwnerIsContactFlag { get; set; }
        public bool HDNISNodeConstraintFlag { get; set; }

        public long CAUTION_ID { get; set; }
        public string CAUTION_NAME { get; set; }
        public string SPECIFIC_ACTION { get; set; }
        public string SpecificActionXML { get; set; }
        public decimal TotalRecordCount { get; set; }

        public long Revision_ID { get; set; }
        public Int16 Revision_No { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime MovementStartDate { get; set; }
        public DateTime MovementEndDate { get; set; }

        public string HaulierName { get; set; }
        public string ESDALReference { get; set; }
        public decimal countRec { get; set; }
        public int ApplicationStatus { get; set; }
        public long Version_ID { get; set; }

        public bool StandardCaution { get; set; }
        public bool SpecificAction { get; set; }
        public bool CREATOR_IS_CONTACT { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }

        public double MAX_GROSS_WEIGHT { get; set; }
        public long MAX_GROSS_WEIGHT_UNIT { get; set; }

        public double MAX_AXLE_WEIGHT { get; set; }
        public long MAX_AXLE_WEIGHT_UNIT { get; set; }

        public float MAX_HEIGHT { get; set; }
        public long MAX_HEIGHT_UNIT { get; set; }

        public long MAX_LENGTH_UNIT { get; set; }
        public float MAX_LENGTH_MTRS { get; set; }
        public float MAX_LENGTH { get; set; }

        public long MAX_WIDTH_UNIT { get; set; }

        public long MAX_SPEED_UNIT { get; set; }
        public long MAX_SPEED_KPH { get; set; }

        public float MAX_WIDTH { get; set; }

        public float MIN_SPEED { get; set; }

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

        public DateTime OCCURRED_TIME { get; set; }
        public string HISTORYDETAILS { get; set; }
    }

    public class StructureContactModel
    {

        [Display(Name = "Content Name")]
        public string CONTACT_NAME { get; set; }

        public string DESCRIPTION { get; set; }

        public long CONTACT_ID { get; set; }

        public short CONTACT_NO { get; set; }

        public string FULL_NAME { get; set; }

        public string ORGANISATION_NAME { get; set; }

        public string ADDRESS_LINE_1 { get; set; }

        public string ADDRESS_LINE_2 { get; set; }

        public string ADDRESS_LINE_3 { get; set; }

        public string ADDRESS_LINE_4 { get; set; }

        public string ADDRESS_LINE_5 { get; set; }

        public string POST_CODE { get; set; }

        public int CountryID { get; set; }

        public string CountryName { get; set; }

        public string TELEPHONE { get; set; }

        public string EXTENSION { get; set; }

        public string MOBILE { get; set; }

        public string FAX { get; set; }

        public string EMAIL { get; set; }

        public string EMAIL_PREFERENCE { get; set; }

        public long CAUTION_ID { get; set; }

        public decimal TotalRecordCount { get; set; }

        public object ORGANISATION_ID { get; set; }

        public object ROLE_TYPE { get; set; }

        public object IS_AD_HOC { get; set; }

        public object USER_SCHEMA { get; set; }

    }

    public class StructureSearchModel
    {
        public string searchColumn { get; set; } //For search panel id , value pair
        public string searchValue { get; set; } //For search panel id , value pair
    }

    public class StructureReport
    {
        public string DisplayText { get; set; }
    }
    public class StructureLogModel
    {
        public double AUDIT_ID { get; set; }
        public double STRUCTURE_ID { get; set; }
        public string STRUCTURE_CODE { get; set; }
        public TimeSpan OCCURRED_TIME { get; set; }
        public string AUTHOR { get; set; }
        public string AMENDMENT_1 { get; set; }
        public string AMENDMENT_2 { get; set; }
        public string AMENDMENT_3 { get; set; }
    }
}