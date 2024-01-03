using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NetSdoGeometry;
using STP.Common.Constants;

namespace STP.Domain.SecurityAndUsers
{
    public class UserInfo
    {
		public int organisationId;

		public UserInfo()
        {
            MaxListItem = 20;
            UserSchema = Common.Constants.UserSchema.Portal;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLogged { get; set; }
        public bool IsValidUser { get; set; }
        public Int32 UserTypeId { get; set; }
        public Int64 OrganisationId { get; set; }

        public int IsAdmin { get; set; }
        public Int32 ProjectId { get; set; }
        public DateTime LastLogin { get; set; }

        //for User Preferences
        public Int32 VehicleUnits { get; set; }
        public Int32 RoutePlanUnits { get; set; }
        public int MaxListItem { get; set; }

        public string Email { get; set; }

        public int NumOfAttempts { get; set; }
        public int IsEnabled { get; set; }

        public string OrganisationName { get; set; }
        public Int32 AccessToALSAT { get; set; }
        public string UserSchema { get; set; }//portal or stp_sort

        //SORT preferences based on Roles
        public string SORTCreateJob { get; set; }
        public string SORTAllocateJob { get; set; }
        public string SORTCanApproveSignVR1 { get; set; }
        public string SORTCanAgreeUpto150 { get; set; }
        public string SORTCanAgreeAllSO { get; set; }
        public long ContactId { get; set; }
        public long SortUserId { get; set; }
        public sdogeometry GeoRegion { get; set; }
        public int IsTermsAccepted { get; set; }
        public int IsDeleted { get; set; }
        public string HelpdeskRedirect { get; set; }
        public string HelpdeskUserId { get; set; }
        public string HelpdeskUserName { get; set; }
        public bool HelpdeskLoginAsAnotherUser { get; set; }
        public int LoggedIn { get; set; }
        public string ResponseContent { get; set; }

        public int PasswordStatus { get; set; }//To distingush password and OTP
        public DateTime PasswordUpdatedOn { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

    }
}