using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    public class UserRegistration
    {
        public int? UserType { get; set; }//changed UserType from int to int? for resolving issue where delegated admin created users are not getting created.
        public string UserTypeName { get; set; }
        public string UserTypeNameDrop { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Please enter a valid name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Organisation Name is required")]
        [DataType(DataType.Text)]
        [AlternateValidation(Suggetion = "Please enter a valid Name")]
        public string OrgUser { get; set; }
        public int OrgList { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[a-z0-9\.!@*_-]+$", ErrorMessage = "Only lowercase, numbers and !.@*_- are allowed")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "The user name must be atleast 4 characters in length")]
        public string UserName { get; set; }
        public string SecurityAnswer { get; set; }
        public string SecurityQuestion { get; set; }
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[a-zA-Z._^%$#!~&*@,-]+$", ErrorMessage = "Please enter only alphabet and characters")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Please enter atleast 3 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[a-zA-Z._^%$#!~&*@,-]+$", ErrorMessage = "Please enter only alphabet and characters")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Please enter atleast 3 characters")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Password is required")]//@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,20}$"
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=[{\]};:<>.?,-~`])[A-Za-z\d!@#$%^&*()_+=[{\]};:<>.?,-~`]{6,12}", ErrorMessage = "Password must contain: Minimum 6 and Maximum 12 characters, atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character (except ', \").")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Please enter a password between 6 and 12 characters")]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        //[Compare("password")]
        [Compare("password", ErrorMessage = "Your passwords do not match. Please try again")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Please enter a password between 6 and 12 characters")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(\+)?([-\._\(\) ]?[\d]{3,20}[-\._\(\) ]?){2,10}", ErrorMessage = "Please enter a valid phone number")]
        [StringLength(32)]
        public string MiscPhone1 { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string MiscPhone1Desc { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(\+)?([-\._\(\) ]?[\d]{3,20}[-\._\(\) ]?){2,10}", ErrorMessage = "Please enter a valid phone number")]
        [StringLength(32)]
        public string MiscPhone2 { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string MiscPhone2Desc { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(\+)?([-\._\(\) ]?[\d]{3,20}[-\._\(\) ]?){2,10}", ErrorMessage = "Please enter a valid phone number")]
        [StringLength(32)]
        public string MiscPhone3 { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string MiscPhone3Desc { get; set; }
        public bool IsResetPW { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+)?[0-9\ -]+$", ErrorMessage = "Please enter a valid mobile number")]
        [StringLength(32)]
        public string Mobile { get; set; }
        [RegularExpression("[0-9]+", ErrorMessage = "Please enter a valid extension number")]
        public int? Extension { get; set; }
        [Required(ErrorMessage = "Post code is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special characters are not allowed")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Please enter a valid postcode")]
        [Display()]
        public string PostCode { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        [Required(ErrorMessage = "E-mail address is required")]
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,39}(?:\.[a-z]{2})?)$", ErrorMessage = "Please enter a valid E-mail")]
        public string Email { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string Notes { get; set; }
        [DataType(DataType.Date)]
        public DateTime VerifiedDate { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string Address { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string AddressLine2 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string AddressLine3 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string AddressLine4 { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string AddressLine5 { get; set; }
        public string Attitude { get; set; }
        [DataType(DataType.Text)]
        [StringLength(15, MinimumLength = 3)]
        public string Association { get; set; }
        public int AssociationId { get; set; }
        public string City { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string Country { get; set; }
        [Required(ErrorMessage = "The Country is required")]
        public string CountryId { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string Region { get; set; }
        public int RegionId { get; set; }
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string Town { get; set; }
        public string Website { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdministrator { get; set; }
        public string RepeatSwitchBoardNumber { get; set; }
        public string SwitchBoardNumber { get; set; }
        [Required(ErrorMessage = "The Telephone is required")]
        [RegularExpression(@"^(\+)?[0-9\ -]+$", ErrorMessage = "Please enter a valid telephone number")]
        public string Telephone { get; set; }
        public long OrganisationId { get; set; }
        public long AdminSelectedOrganisationId { get; set; }
        public string OrganisationType { get; set; }
        public string OrganisationName { get; set; }
        public string MainSwitchBoard { get; set; }
        public string Comments { get; set; }
        public string ContactId { get; set; }
        public string DailyDigest { get; set; }
        public bool EnableStructure { get; set; }
        public string Fax { get; set; }
        public bool OnlyContact { get; set; }
        public bool HasLogo { get; set; }
        public string HaulierMneMonic { get; set; }
        public int IndeminityId { get; set; }
        public string InfluenceLinesLower { get; set; }
        public string InfluenceLinesUpper { get; set; }
        public string LastLicenseNumber { get; set; }
        public string LicenceNumber { get; set; }
        public string NextMoveRef { get; set; }
        public string OrgWebAddress { get; set; }
        public string OwnershipMember { get; set; }
        public int ProjectId { get; set; }
        public string SvScreeningLower { get; set; }
        public string SvScreeningUpper { get; set; }
        public string Roletype { get; set; }
        public string ContactPref { get; set; }
       
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter a title between 2 and 100 characters.")]
        public string Title { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string WeightScreeningLower { get; set; }
        public string WeightScreeningUpper { get; set; }
        //newly added
        public long? Selectorg_Id { get; set; }//changed from int to int? for resolving issue where delegated admin created users are not getting created.
        public string SORTCreateJob { get; set; }
        public string SORTAllocateJob { get; set; }
        public string SORTCanApproveSignVR1 { get; set; }
        public string SORTCanAgreeUpto150 { get; set; }
        public string SORTCanAgreeAllSO { get; set; }
        // public string contactId { get; set; } Due to this property getting an exception
        //for user assigned roles
        public bool NotSpecified { get; set; }
        public bool DataHolder { get; set; }
        public bool NotificationContact { get; set; }
        public bool OfficialContact { get; set; }
        public bool ItContact { get; set; }
        public bool DataOwner { get; set; }
        public string FromDescrp { get; set; }
        public string ToDescrp { get; set; }
        public bool IsSystemPW { get; set; }
        public string CountryName { get; set; }
    }

    public class GetUserList
    {
        public string UserID { get; set; }
        public string ContactId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string SurName { get; set; }
        public string OrgName { get; set; }
        public string Email { get; set; }
        public string SearchType { get; set; }
        public string SearchName { get; set; }
        public bool ContactFlag { get; set; }
        public bool ContactdisabFlag { get; set; }

        public string ddSearchValue { get; set; }
        public string TxtSearchValue { get; set; }
        public bool UserTypeFlag { get; set; }
        public string UserTypeId { get; set; }
        public string OrganisationCode { get; set; }

        public long TotalRecordCount { get; set; }
       
    }
    public class AlternateValidation : ValidationAttribute
    {
        public string Suggetion { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value.ToString() != "0")
                {

                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(Suggetion);

                }
            }
            else
                return new ValidationResult("Value is Null");

        }
    }
}


