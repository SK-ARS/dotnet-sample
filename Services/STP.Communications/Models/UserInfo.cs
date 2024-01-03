using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.CommunicationsInterface.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            MAX_LIST_ITEMS = 20;
            userSchema = "portal";
        }

        public string UserID { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLogged { get; set; }
        public bool IsValidUser { get; set; }
        public Int32 userTypeId { get; set; }
        public Int64 organisationId { get; set; }

        //public Int32 PortalType { get; set; }
        public int IsAdmin { get; set; }
        public Int32 ProjectId { get; set; }
        public DateTime LastLogin { get; set; }

        //for User Preferences
        public Int32 VehicleUnits { get; set; }
        public Int32 RoutePlanUnits { get; set; }
        public int MAX_LIST_ITEMS { get; set; }

        public string Email { get; set; }

        public int NumOfAttempts { get; set; }
        public int IsEnabled { get; set; }

        public string organisationName { get; set; }
        public string userSchema { get; set; }//portal or stp_sort

        //SORT preferences based on Roles
        public string SORTCreateJob { get; set; }
        public string SORTAllocateJob { get; set; }
        public string SORTCanApproveSignVR1 { get; set; }
        public string SORTCanAgreeUpto150 { get; set; }
        public string SORTCanAgreeAllSO { get; set; }
        public long ContactId { get; set; }
        public long SortUserId { get; set; }
        //public sdogeometry GeoRegion { get; set; }

        public int isTermsAccepted { get; set; }

        public int IsDeleted { get; set; }

        public string Helpdest_redirect { get; set; }
        public string Helpdesk_userID { get; set; }
        public string helpdesk_username { get; set; }
        public bool helpdestLoginAsAnotherUser { get; set; }
        public int Logged_In { get; set; }
        public string responseContent { get; set; }

    }
}