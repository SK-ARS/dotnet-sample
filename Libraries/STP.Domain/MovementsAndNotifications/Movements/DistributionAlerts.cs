using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using STP.Common.Validation;

namespace STP.Domain
{
    public class DistributionAlerts
    {
        public int TransmissionId { get; set; }

        public string DateTime { get; set; }

        public string Start_date { get; set; }

        public string End_date { get; set; }

        public string AlertType { get; set; }

        public string ESDALReference { get; set; }

        public string Details { get; set; }

        public int TotalCount { get; set; }


        //For showing from and TO Organisation in distribution alert        
        //distribution alert date:11/5/2015
        public string ToORGNAME { get; set; }

        public string ToContactNAME { get; set; }

        public string FromORGNAME { get; set; }

        public string ORG_TYPE_NAME { get; set; }

        public int OrganisationId { get; set; }

        public decimal Contact_id { get; set; }

        public string userID { get; set; }

        public int IS_MANUALLY_ADDED { get; set; }

        //Check as SOA and Police
        public string Item_Type { get; set; }
        public int Inbox_Id { get; set; }
        public int notif_id { get; set; }

        //Check as Haulier
        public int Project_id { get; set; }
        public int Version_id { get; set; }
        public int Version_no { get; set; }
        public int Revision_id { get; set; }
        public int Revision_no { get; set; }
        public int Veh_purpose { get; set; }
        public int Version_status { get; set; }

        //for notification        
        public long VehicleType { get; set; }

        //for distributionAlertfilter
        public string movementData { get; set; }
        public string showalert { get; set; }
        public byte[] OutboundDocument { get; set; }


    }
	//Class to save document details being transmitted.
    public class TransmittingDocumentDetails
    {
        public byte[] OutboundDocument { get; set; }

        public long inboxItemId { get; set; }

        public long transmissionId { get; set; }

        public long documentId { get; set; }

        public string documentType { get; set; }

        public string esdalReference { get; set; }

        public long contactId { get; set; }

        public long organisationId { get; set; }
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

        [Display(Name = "Distribution comments")]
        public string text { get; set; }
    }

    /// <summary>
    /// class to save the document information being taken to view.
    /// </summary>
    public class DocumentInfo
    {
        public string XMLDocument { get; set; }

        public string userType { get; set; }

        public long documentId { get; set; }

        public long organisationId { get; set; }
    }
}
