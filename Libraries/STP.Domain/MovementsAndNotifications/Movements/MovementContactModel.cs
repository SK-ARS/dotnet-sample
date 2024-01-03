using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class MovementContactModel
    {
        public int ContactId { get; set; } // ContactId is the database primary key of the contact

        public int OnBehalfOfId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric is allowed")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Firstvalid Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } // First name of the contact

        [Required(ErrorMessage = "Last Name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric is allowed")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Please enter a valid Last Name")]
        [Display(Name = "Last Name")]
        public string SurName { get; set; } // First name of the contact

        [Required(ErrorMessage = "Role is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric is allowed")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Please enter a valid Role")]
        [Display(Name = "Role")]
        public string Title { get; set; }// title of Contact. also know as ROLE

        [StringLength(35, MinimumLength = 3, ErrorMessage = "Please enter a valid Address")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; } // Address line 1 of the Contact

        [StringLength(35, MinimumLength = 3, ErrorMessage = "Please enter a valid Address")]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; } // Address line 2 of the Contact

        [StringLength(35, MinimumLength = 3, ErrorMessage = "Please enter a valid Address")]
        [Display(Name = "Address Line 3")]
        public string AddressLine3 { get; set; } // Address line 3 of the Contact

        [StringLength(35, MinimumLength = 3, ErrorMessage = "Please enter a valid Address")]
        [Display(Name = "Address Line 4")]
        public string AddressLine4 { get; set; } // Address line 1 of the Contact

        [StringLength(35, MinimumLength = 3, ErrorMessage = "Please enter a valid Address")]
        [Display(Name = "Address Line 5")]
        public string AddressLine5 { get; set; } // Address line 5 of the Contact

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; } //Postal code of the Contact

        //Contact Country Id
        [DataType(DataType.Text)]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; } //Country in which the Contact is located

        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^(\+)?[0-9\-]+$", ErrorMessage = "Please enter a valid Phone number")]
        [StringLength(100)]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } //Phone number of the Contact

        [Display(Name = "EXTENSION")]
        public string Extension { get; set; }  //EXTENSION number for phonenumber

        [Required(ErrorMessage = "MOBILE Number is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^(\+)?[0-9\-]+$", ErrorMessage = "Please enter a valid Mobile number")]
        [StringLength(100)]
        [Display(Name = "MOBILE")]
        public string Mobile { get; set; } //Phone number of the Contact

        [DataType(DataType.Text)]
        [RegularExpression(@"^(\+)?[0-9\-]+$", ErrorMessage = "Please enter a valid Fax number")]
        [StringLength(100)]
        [Display(Name = "Fax")]
        public string Fax { get; set; } //Fax number of the Haulier

        [Required(ErrorMessage = "The E-mail is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", ErrorMessage = "Please enter a valid E-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } //email address of the contact

        [StringLength(2000)]
        [Display(Name = "Comments")]
        public string Comments { get; set; }  //comments on Contact

        [StringLength(5)]
        [Display(Name = "Initials")]
        public string Initials { get; set; }  //INITIALS for contact

        [Display(Name = "OrganisationId")]
        public int OrganisationId { get; set; } //OrganizationId for related contact

        [Display(Name = "Organisation")]
        public string Organisation { get; set; } //Organization name of the contact

        [Display(Name = "Deleted")]
        public string Deleted { get; set; }  //user for soft delete

        [Display(Name = "Contact Type")]
        public string ContactType { get; set; } //Contact type could be Structure Owner / Interested Parties, Police / ALO

        [Display(Name = "On Behalf Of")]
        public string OnBehalfOf { get; set; } //On behalf of property of the contact

        [Display(Name = "Notification")]
        public string NotifyingMethod { get; set; } //Notify by E-mail or by Fax

        public string SearchType { get; set; } //For search panel id , value pair
        public string SearchName { get; set; } //For search panel id , value pair

        public Byte[] AffectedParties { get; set; }//Affected parties list

        public decimal RecordCount { get; set; } //Used to store search count

        [Display(Name = "Other Organisation")]
        public string OtherOrganisation { get; set; } //Other Organization name where the contact is also plays a role

        public Byte[] RevisedParties { get; set; }//Revised affected parties list
        public string Reason { get; set; }//Reason


        public string IsPolice { get; set; }

        public int DelegatorsOrganisationId { get; set; }
        public string DelegatorsOrganisationName { get; set; }
    }
}