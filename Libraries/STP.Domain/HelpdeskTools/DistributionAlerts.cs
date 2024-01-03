using System;
using System.ComponentModel.DataAnnotations;
using STP.Common.Validation;

namespace STP.Domain.HelpdeskTools
{
    public class DistributionAlerts
    {
        public int PresetFilter;

        public int TransmissionId { get; set; }

        public string DateTime { get; set; }
        public DateTime InboxCreationTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string AlertType { get; set; }

        public string ESDALReference { get; set; }

        public string Details { get; set; }

        public int TotalCount { get; set; }

        public int SortOrder { get; set; }

        public int SearchFlag { get; set; }


        //For showing from and TO Organisation in distribution alert        
        public string ToOrganisationName { get; set; }

        public string ToContactName { get; set; }

        public string FromOrganisationName { get; set; }

        public string OrganisationTypeName { get; set; }

        public int OrganisationId { get; set; }

        public decimal ContactId { get; set; }

        public string userID { get; set; }

        public int IsManullyAdded { get; set; }

        //Check as SOA and Police
        public string ItemType { get; set; }
        public int InboxId { get; set; }
        public int NotificationId { get; set; }

        //Check as Haulier
        public int ProjectId { get; set; }
        public int VersionId { get; set; }
        public int VersionNo { get; set; }
        public int RevisionId { get; set; }
        public int RevisionNo { get; set; }
        public int VehiclePurpose { get; set; }
        public int VersionStatus { get; set; }

        //for notification        
        public long VehicleType { get; set; }

        //for distributionAlertfilter
        public string MovementData { get; set; }
        public string ShowAlert { get; set; }
        public byte[] OutboundDocument { get; set; }


    }
    //Class to save document details being transmitted.
    public class TransmittingDocumentDetails
    {
        public byte[] OutboundDocument { get; set; }

        public long InboxItemId { get; set; }

        public long TransmissionId { get; set; }

        public long DocumentId { get; set; }

        public string DocumentType { get; set; }

        public string EsdalReference { get; set; }

        public long ContactId { get; set; }

        public long OrganisationId { get; set; }
        public long? NotificationId { get; set; }
        public int ItemType { get; set; }
    }

    public class RetransmitDetails
    {
        [Display(Name = "Candidate route name")]
        //[Required(ErrorMessage="Please enter candidate name")]
        public string CandidateName { get; set; }

        [Display(Name = "Planner name")]
        //[Required(ErrorMessage = "Please enter planner name")]
        public string PlannerName { get; set; }

        [Display(Name = "Contact name")]
        [Required(ErrorMessage = "Please enter contact name")]
        public string ContactName { get; set; }

        [Display(Name = "Organisation name")]
        [Required(ErrorMessage = "Please enter organisation name")]
        public string OrganisationName { get; set; }

        [Display(Name = "Contact method")]
        [Required(ErrorMessage = "Please enter contact method")]
        public string ContactMethod { get; set; }

        [Display(Name = "Email")]
        [RequiredEmail("efax", "Please enter email address")]
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", ErrorMessage = "Please enter a valid E-mail")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Display(Name = "Fax")]
        [RegularExpression(@"^(\+)?(?<!\d)\d{12}(?!\d)$", ErrorMessage = "Enter 12 digit fax number.For ex. +123456789012")]
        [RequiredFax("efax", "Please enter fax")]
        public string Fax { get; set; }

        public string efax { get; set; }
        public bool distFlag { get; set; }
    }

    //class to save Distribution Comment
    public class DistributionComments
    {
        [Display(Name = "Esdal2 reference")]
        public string esdalReference { get; set; }

        [Display(Name = "Distribution Comments")]
        public string text { get; set; }
    }

    /// <summary>
    /// class to save the document information being taken to view.
    /// </summary>
    public class DocumentInfo
    {
        public string XMLDocument { get; set; }

        public string UserType { get; set; }

        public long DocumentId { get; set; }

        public long OrganisationId { get; set; }
    }
}
