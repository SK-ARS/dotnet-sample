using NetSdoGeometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            MAX_LIST_ITEMS = 20;
            userSchema = "portal";
        }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [JsonProperty("FirstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("LastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("LastLogged", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastLogged { get; set; }
        [JsonProperty("IsValidUser", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsValidUser { get; set; }
        [JsonProperty("userTypeId", NullValueHandling = NullValueHandling.Ignore)]
        public Int32 userTypeId { get; set; }
        [JsonProperty("organisationId", NullValueHandling = NullValueHandling.Ignore)]
        public Int64 organisationId { get; set; }

        //public Int32 PortalType { get; set; }
        [JsonProperty("IsAdmin", NullValueHandling = NullValueHandling.Ignore)]
        public int IsAdmin { get; set; }
        [JsonProperty("ProjectId", NullValueHandling = NullValueHandling.Ignore)]
        public Int32 ProjectId { get; set; }
        [JsonProperty("LastLogin", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastLogin { get; set; }

        //for User Preferences
        [JsonProperty("VehicleUnits", NullValueHandling = NullValueHandling.Ignore)]
        public Int32 VehicleUnits { get; set; }
        [JsonProperty("RoutePlanUnits", NullValueHandling = NullValueHandling.Ignore)]
        public Int32 RoutePlanUnits { get; set; }
        [JsonProperty("MAX_LIST_ITEMS ", NullValueHandling = NullValueHandling.Ignore)]
        public int MAX_LIST_ITEMS { get; set; }

        [JsonProperty("Email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("NumOfAttempts", NullValueHandling = NullValueHandling.Ignore)]
        public int NumOfAttempts { get; set; }
        [JsonProperty("IsEnabled", NullValueHandling = NullValueHandling.Ignore)]
        public int IsEnabled { get; set; }

        [JsonProperty("organisationName", NullValueHandling = NullValueHandling.Ignore)]
        public string organisationName { get; set; }
        [JsonProperty("userSchema", NullValueHandling = NullValueHandling.Ignore)]
        public string userSchema { get; set; }//portal or stp_sort

        //SORT preferences based on Roles
        [JsonProperty("SORTCreateJob", NullValueHandling = NullValueHandling.Ignore)]
        public string SORTCreateJob { get; set; }
        [JsonProperty("SORTAllocateJob", NullValueHandling = NullValueHandling.Ignore)]
        public string SORTAllocateJob { get; set; }
        [JsonProperty("SORTCanApproveSignVR1", NullValueHandling = NullValueHandling.Ignore)]
        public string SORTCanApproveSignVR1 { get; set; }
        [JsonProperty("SORTCanAgreeUpto150", NullValueHandling = NullValueHandling.Ignore)]
        public string SORTCanAgreeUpto150 { get; set; }
        [JsonProperty("SORTCanAgreeAllSO", NullValueHandling = NullValueHandling.Ignore)]
        public string SORTCanAgreeAllSO { get; set; }
        [JsonProperty("ContactId", NullValueHandling = NullValueHandling.Ignore)]
        public long ContactId { get; set; }
        [JsonProperty("SortUserId", NullValueHandling = NullValueHandling.Ignore)]
        public long SortUserId { get; set; }
        public sdogeometry GeoRegion { get; set; }

        [JsonProperty("isTermsAccepted", NullValueHandling = NullValueHandling.Ignore)]
        public int isTermsAccepted { get; set; }

        [JsonProperty("IsDeleted", NullValueHandling = NullValueHandling.Ignore)]
        public int IsDeleted { get; set; }

        [JsonProperty("Helpdest_redirect", NullValueHandling = NullValueHandling.Ignore)]
        public string Helpdest_redirect { get; set; }
        [JsonProperty("Helpdesk_userID", NullValueHandling = NullValueHandling.Ignore)]
        public string Helpdesk_userID { get; set; }
        [JsonProperty("helpdesk_username", NullValueHandling = NullValueHandling.Ignore)]
        public string helpdesk_username { get; set; }
        [JsonProperty("helpdestLoginAsAnotherUser", NullValueHandling = NullValueHandling.Ignore)]
        public bool helpdestLoginAsAnotherUser { get; set; }
        [JsonProperty("Logged_In", NullValueHandling = NullValueHandling.Ignore)]
        public int Logged_In { get; set; }
        [JsonProperty("LoginStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginStatus { get; set; }

    }
}