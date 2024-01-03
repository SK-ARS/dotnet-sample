using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Models
{
    public class UserRegistration
    {
        public int? UserType { get; set; }//changed UserType from int to int? for resolving issue where delegated admin created users are not getting created.
        public string user_type_name { get; set; }
        public string user_type_name_drop { get; set; }
        public string Name { get; set; }
        public string OrgUser { get; set; }
        public int OrgList { get; set; }
        public string Username { get; set; }
        public string SecurityAnswer { get; set; }
        public string SecurityQuestion { get; set; }
        public int QuestionId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string password { get; set; }
        public string ConfirmPassword { get; set; }
        public string MiscPhone1 { get; set; }
        public string MiscPhone1Desc { get; set; }
        public string MiscPhone2 { get; set; }
        public string MiscPhone2Desc { get; set; }
        public string MiscPhone3 { get; set; }
        public string MiscPhone3Desc { get; set; }
        public bool IsResetPW { get; set; }
        public string Mobile { get; set; }
        public int? Extension { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public DateTime VerifiedDate { get; set; }
        public string Address { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Attitude { get; set; }
        public string Association { get; set; }
        public int AssociationID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryID { get; set; }
        public string Region { get; set; }
        public int RegionID { get; set; }
        public string Town { get; set; }
        public string Website { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdministrator { get; set; }
        public string RepeatSwitchBoardNumber { get; set; }
        public string SwitchBoardNumber { get; set; }
        public string Telephone { get; set; }
        public long OrganisationID { get; set; }
        public long AdminSelected_OrganisationID { get; set; }
        public string OrganisationType { get; set; }
        public string OrganisationName { get; set; }
        public string MainSwitchBoard { get; set; }
        public string Comments { get; set; }
        public string ContactID { get; set; }
        public string DailyDigest { get; set; }
        public bool EnableStructure { get; set; }
        public string Fax { get; set; }
        public bool OnlyContact { get; set; }
        public bool HasLogo { get; set; }
        public string HaulierMneMonic { get; set; }
        public int IndeminityID { get; set; }
        public string InfluenceLinesLower { get; set; }
        public string InfluenceLinesUpper { get; set; }
        public string LastLicenseNumber { get; set; }
        public string LicenceNum { get; set; }
        public string NextMoveRef { get; set; }
        public string OrgWebAddress { get; set; }
        public string OwnershipMember { get; set; }
        public int ProjectID { get; set; }
        public string SvScreeningLower { get; set; }
        public string SvScreeningUpper { get; set; }
        public string Roletype { get; set; }
        public string ContactPref { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string WeightScreeningLower { get; set; }
        public string WeightScreeningUpper { get; set; }
        //newly added
        public long? selectorg_id { get; set; }//changed from int to int? for resolving issue where delegated admin created users are not getting created.
        public string SORTCreateJob { get; set; }
        public string SORTAllocateJob { get; set; }
        public string SORTCanApproveSignVR1 { get; set; }
        public string SORTCanAgreeUpto150 { get; set; }
        public string SORTCanAgreeAllSO { get; set; }
        public string contact_Id { get; set; }
        //for user assigned roles
        public bool Notspecified { get; set; }
        public bool Dataholder { get; set; }
        public bool Notificationcontact { get; set; }
        public bool Officialcontact { get; set; }
        public bool Itcontact { get; set; }
        public bool Dataowner { get; set; }
        //added by poonam 06.11.14
        public string FromDescrp { get; set; }
        public string ToDescrp { get; set; }
    }

    public class GetUserList
    {

        public string UserID { get; set; }
        public string contact_Id { get; set; }
        public string UserName { get; set; }
        public string FIRSTNAME { get; set; }
        public string PHONENUMBER { get; set; }
        public string surName { get; set; }
        public string orgName { get; set; }
        public string email { get; set; }
        public string searchType { get; set; }
        public string searchName { get; set; }
        public bool ContactFlag { get; set; }
        public bool ContactdisabFlag { get; set; }

        public string ddSearchValue { get; set; }
        public string txtSearchValue { get; set; }
        public bool userTypeFlag { get; set; }
        public string UserTypeID { get; set; }


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


