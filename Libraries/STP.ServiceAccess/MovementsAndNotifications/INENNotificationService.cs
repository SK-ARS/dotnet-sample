using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.MovementsAndNotifications.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.MovementsAndNotifications
{
    public interface INENNotificationService
    {
        List<NeHaulierList> GetNeHaulier(int pageNo, int pageSize, string searchString, int isVal, int presetFilter,int sortorder);
        List<NeHaulierList> EditNeUser(long AuthKeyId);
        int SaveNeUser(string Haulname, string authKey, string OrgName, long NeLimit, long KeyId);
        int EnableUser(string authKey, long keyId);
        int HaulierValid(string haulierName, string organisationName);
        List<NENUpdateRouteDet> GetNENRouteList(long NENInboxId, int organisationId);             
        List<OrganisationUser> GetOrg_UserList(long OrgID, int SOA_UserTypeID, long inBoxId = 0, long NEN_ID = 0);
        bool SP_SAVE_NEN_USER_FOR_SCRUTINY(MovementModel movement);
        long GetNENId(int notificationNo);
        List<NENSOAReportModel> GetNENSOAReportHistory(int month, int year, long orgId);
        List<NENHelpdeskReportModel> GetNENHelpdeskReportHistory(int month, int year, string vehicleCat, int requiresVr1, int vehicleCount);
        int VerifyRouteIdWithOtherOrg(int NENId, int organisationId, int routePartId);
        long GetNENRouteID(int NENId, int inboxItemID, int organisationId, char returnVal);
        long SaveNotificationAutoResponseAuditLog(SaveAutoResponseParams saveAutoResponseParams);
        
        List<NENRouteStatusList> GetNENBothRouteStatus(int inboxId, int NENId, int userId, long organisationId);
        int GetRouteStatus(int inboxItemId, int NENId, int userId);
        List<NENGeneralDetails> GetNENRouteDescription(int NENId, long inboxId, long organisationId);
        List<NENGeneralDetails> GetRouteFromAndToDescp(long routepartId);
        NENHaulierRouteDesc GetHualierRouteDesc(int NENId, int inboxItemId, string routeDesc = "", string fromAddress = "", string toAddress = "");
        long GetNENDocumentIdFromInbox(long NENId, long inboxId, long organisationId);
        int UpdateInboxTypeFirstTime(long InboxId, long organisationId);
        NENGeneralDetails GetGeneralDetail(int NENId, int RouteId);
        NotificationGeneralDetails GetNENNotifInboundDet(long NotifId, long NENId);
        int UpdateRouteStatus(int InboxId, int UserId, int RouteId, int RouteStatus, long OrganisationId);
        bool InsertInboxEditRouteForNewUser(int InboxId, long NENId, int NotificationId, int NewUserId, long EditedRouteId, long NewRouteId, long OrganisationId);
        long GetNENReturnRouteID(int InboxItemId, int orgId);
        int UpdateNENICAStatusInboxItem(int InboxId, int IcaStatus, long OrganisationId);
    }
}
