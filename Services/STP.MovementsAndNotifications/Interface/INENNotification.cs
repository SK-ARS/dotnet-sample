using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.LoggingAndReporting;
using STP.Domain.NonESDAL;

namespace STP.MovementsAndNotifications.Interface
{
    public interface INENNotification
    {
        long GetNENId(int notificationNo);
        long SaveNotificationAuditLog(AuditLogIdentifiersParams objAuditLogIdentifiersParams);
        List<NeHaulierList> GetNeHaulier(int pageNo, int pageSize, string searchString, int isVal = 0, int presetFilter=1,int sortorder=1);
        List<NeHaulierList> EditNENUser(long authKeyId);
        int SaveNeuse(NeHaulierParams objNeHaulierParams);

        int EnableUser(string authKey, long keyId);
        int HaulierValid(string haulierName, string organisationName);
        List<NENUpdateRouteDet> GetNENRouteList(long NENInboxId, int organisationId);      
        List<OrganisationUser> GetOrgUserList(long organisationId, int SOA_UserTypeID, long inBoxId = 0, long NEN_ID = 0);
        bool SAVENENUSERFORSCRUTINY(MovementModel movement);
        List<NENSOAReportModel> GetNENSOAReportHistory(Int32 month, Int32 year, long organisationId);
        List<NENHelpdeskReportModel> GetNENHelpdeskReportHistory(int month, int year, string vehicleCat, int requiresVr1, int vehicleCount);
        long GetNENRouteID(int NENId, int inboxItemID, int organisationId, char returnVal);
        int VerifyRouteIdWithOtherOrg(int NENId, int organisationId, int routePartId);
        long SaveNotificationAutoResponseAuditLog(string logMessage, int userId, long organisationId);
        List<NENRouteStatusList> GetNENBothRouteStatus(int inboxId, int NENId, int userId, long organisationId);
        int GetRouteStatus(int inboxItemId, int NENId, int userId);
        List<NENGeneralDetails> GetNENRouteDescription(int NENId, long inboxId, long organisationId);
        List<NENGeneralDetails> GetRouteFromAndToDescp(long routepartId);
        NENHaulierRouteDesc GetHualierRouteDesc(int NENId, int inboxItemId);
        long GetNENDocumentIdFromInbox(long NENId, long inboxId, long organisationId);
        int UpdateInboxTypeFirstTime(long InboxId, long organisationId);
        NENGeneralDetails GetGeneralDetail(int NENId, int RouteId);
        NotificationGeneralDetails GetNENNotifInboundDet(long NotifId, long NENId);
        int UpdateRouteStatus(int InboxId, int UserId, int RouteId, int RouteStatus, long OrganisationId);
        bool InsertInboxEditRouteForNewUser(int InboxId, long NENId, int NotificationId, int NewUserId, long EditedRouteId, long NewRouteId, long OrganisationId);
        long GetNENReturnRouteID(int InboxItemId, int orgId);
        int UpdateNENICAStatusInboxItem(int InboxId, int IcaStatus, long OrganisationId);
        int UpdateNenApiIcaStatus(UpdateNENICAStatusParams updateNENICA);
    }
}