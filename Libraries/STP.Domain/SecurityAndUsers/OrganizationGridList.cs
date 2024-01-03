using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STP.Domain.SecurityAndUsers
{
    public class OrganizationGridList
    {
        public double OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrgType { get; set; }
        public string OrgCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Phone { get; set; }
        public string Web { get; set; }
        public string Licence_NR { get; set; }
        public string PostCode { get; set; }
        public string CountryId { get; set; }
        public string AuthenticationKey { get; set; }
        public long TotalRecordCount { get; set; }

    }
    public class Organization
    {
        //[Required(ErrorMessage = "Organisation Name is required")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Please enter a valid Name")]
        public string OrgName { get; set; }
        [Required(ErrorMessage = "Organisation Type is required")]
        [AlternateValidation(Suggetion = "Please Select a Organisation Type:")]
        public string OrgType { get; set; }
        [DataType(DataType.Text)]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Please enter a valid code")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Only uppercase and numbers are allowed")]
        public string OrgCode { get; set; }
        public string AuthenticationKey { get; set; }
        public string KeyValidator { get; set; }
        [DataType(DataType.Text)]
        public string Licence_NR { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(50)]
        public string AddressLine1 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(50)]
        public string AddressLine2 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(50)]
        public string AddressLine3 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(50)]
        public string AddressLine4 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(50)]
        public string AddressLine5 { get; set; }
        //[Required(ErrorMessage = "Post Code is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special characters are not allowed")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Please enter a valid postcode")]
        [Display()]
        public string PostCode { get; set; }
        //[Required(ErrorMessage = "Country is required")]
        [AlternateValidation(Suggetion = "Please Select a Country:")]
        public string CountryID { get; set; }
        public string Country { get; set; }

        //[Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+)?[0-9\ -]+$", ErrorMessage = "Please enter a valid phone number")]
        [StringLength(25)]
        public string Phone { get; set; }

        public string Web { get; set; }

        public string OrgContact { get; set; }
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", ErrorMessage = "Please enter a valid E-mail")]
        public string EmailId { get; set; }
        [RegularExpression(@"^\+?[0-9]{6,}$", ErrorMessage = "Please enter a valid fax number")]
        public string Fax { get; set; }
        public string HaulierContactName { get; set; }
        public decimal OrgID { get; set; }
        public bool IsNENsReceive { get; set; }
        public bool AccessToALSAT { get; set; }
    }

    public class OrganizationTypeList
    {
        public int OrgTypeCode { get; set; }
        public string OrgTypeName { get; set; }
    }

    public class GetHaulContactByOrgID
    {
        public decimal contactId { get; set; }
        public string ContactName { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string AddressLine_1 { get; set; }
        public string AddressLine_2 { get; set; }
        public string AddressLine_3 { get; set; }
        public string AddressLine_4 { get; set; }
        public string AddressLine_5 { get; set; }
        public string PostCode { get; set; }
        public string CountryID { get; set; }
        public string Phone { get; set; }
    }

    public class UserDetailsModel
    {
        public long UserId { get; set; }
        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }

    public class CheckOrganisationExists
    {
        public string OrganisationName { get; set; }
        public int Type { get; set; }
        public string Mode { get; set; }
        public string OrganisationId { get; set; }

    }
}