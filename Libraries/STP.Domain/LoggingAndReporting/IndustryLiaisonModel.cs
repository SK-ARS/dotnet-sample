using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
    public class IndustryLiaisonModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRecordCount { get; set; }
        public int NumericMonth { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal NoofRegEnabledHauAcc { get; set; }
        public decimal NoofRegHaulierOrganisation { get; set; }
        public decimal NoofHaulierSession { get; set; }
        public decimal NoofPlanRouteSO { get; set; }
        public decimal NoofPlanRouteVR1 { get; set; }
        public decimal NoofPlanRouteNotification { get; set; }
        public decimal NoofRegEnabledPoliceAcc { get; set; }
        public decimal NoofRegPoliceOrg { get; set; }
        public decimal NoPolicePrefInboxOnly { get; set; }
        public decimal NoofPoliceSession { get; set; }
        public decimal NoofRegSOAAcc { get; set; }
        public decimal NoofRegSOAOrg { get; set; }
        public decimal NoSOAPrefInboxOnly { get; set; }
        public decimal NoofSOASession { get; set; }
        public decimal SOAppSubEnterByHauForSO { get; set; }
        public decimal SOAppSubEnterByHauForVR1 { get; set; }
        public decimal SOAppSubEnterBySortForSO { get; set; }
        public decimal SOAppSubEnterBySortForVR1 { get; set; }
        public decimal SOProcssedAgreed { get; set; }
        public decimal SOProcssedDeclined { get; set; }
        public decimal SOProcssedWithdrawn { get; set; }
        public decimal NoPropoAgreedSentEmail { get; set; }
        public decimal NoPropoAgreedSentFax { get; set; }
        public decimal NoPropoAgreedSentInbox { get; set; }

        public decimal NoGuestUsers { get; set; }
        public decimal NoUserSessions { get; set; }

        public decimal HaulierOnlineEntered { get; set; }
        public decimal SORTEnteredOnBehalfOfHaulier { get; set; }
        public decimal SpecialOrders { get; set; }
        public decimal AllSTGONotified { get; set; }

        public decimal STGOCategory1 { get; set; }
        public decimal STGOCategory2 { get; set; }
        public decimal STGOCategory3 { get; set; }

        public decimal MobileCraneCategoryA1 { get; set; }
        public decimal MobileCraneCategoryA2 { get; set; }
        public decimal MobileCraneCategoryA3 { get; set; }

        public decimal STGOOtherDetails { get; set; }

        public decimal CAndUNotification { get; set; }
        public decimal OtherNotification { get; set; }

        public decimal TotalNumberOfSOANotificationToShow { get; set; }
        public decimal TotalNumberOfSOANotificationToDisplay { get; set; }

        public decimal TotalNumberOfPoliceNotificationToShow { get; set; }
        public decimal TotalNumberOfPoliceNotificationToDisplay { get; set; }

        public decimal TotalNumberOfSOANotificationOpenToShow { get; set; }
        public decimal TotalNumberOfSOANotificationOpenToDisplay { get; set; }

        public decimal TotalNumberOfPoliceNotificationOpenToShow { get; set; }
        public decimal TotalNumberOfPoliceNotificationOpenToDisplay { get; set; }

        public decimal EmailNotification { get; set; }
        public decimal FaxNotification { get; set; }
        public decimal InBoxNotification { get; set; }

        public decimal NotificationAndReNotificationReceived { get; set; }
       
    }
}
