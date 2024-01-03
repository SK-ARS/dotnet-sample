using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.LoggingAndReporting;
using STP.Domain.NonESDAL;

namespace STP.MovementsAndNotifications.Providers
{
    public sealed class NENNotificationProvider : INENNotification
    {
        #region NENNotificationProvider Singleton

        private NENNotificationProvider()
        {
        }
        public static NENNotificationProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly NENNotificationProvider instance = new NENNotificationProvider();
        }
        #endregion

        #region Get NEN Id
        public long GetNENId(int notificationNo)
        {
            return NENNotificationDAO.Get_SP_NEN_ID(notificationNo);
        }
        #endregion

        #region Save Notification AuditLog
        public long SaveNotificationAuditLog(AuditLogIdentifiersParams objAuditLogIdentifiersParams)
        {
            return NENNotificationDAO.SaveNotifAuditLog(objAuditLogIdentifiersParams);
        }
        #endregion

        #region Get Ne Haulier
        public List<NeHaulierList> GetNeHaulier(int pageNo, int pageSize, string searchString, int isVal = 0, int presetFilter=1,int sortorder=1)
        {
            return NENNotificationDAO.GetNeHaulier(pageNo, pageSize, searchString,isVal,presetFilter,sortorder);
        }
        #endregion

        #region Edit Ne User
        public List<NeHaulierList> EditNENUser(long authKeyId)
        {
            return NENNotificationDAO.EditNeUser(authKeyId);
        }
        #endregion

        #region Save Ne User
        public int SaveNeuse(NeHaulierParams objNeHaulierParams)
        {
            return NENNotificationDAO.SaveNeuse(objNeHaulierParams);
        }
        #endregion

        #region Enable/Disable User
        public int EnableUser(string authKey, long keyId)
        {
            return NENNotificationDAO.EnableUser(authKey, keyId);
        }
        #endregion

        #region Haulier Validation
        public int HaulierValid(string haulierName, string organisationName)
        {
            return NENNotificationDAO.HaulierValid(haulierName, organisationName);
        }
        #endregion

        #region GetNENRouteList
        public List<NENUpdateRouteDet> GetNENRouteList(long NENinboxId, int organisationId)
        {
            return NENNotificationDAO.GetNENRouteList(NENinboxId, organisationId);
        }
        #endregion

        public List<OrganisationUser> GetOrgUserList(long organisationId, int SOA_UserTypeID, long inBoxId = 0, long NEN_ID = 0)
        {
            return NENNotificationDAO.GetOrgUserList(organisationId,  SOA_UserTypeID, inBoxId,  NEN_ID );
       }

        public  bool SAVENENUSERFORSCRUTINY(MovementModel movement)
        {
            return NENNotificationDAO.SAVENENUSERFORSCRUTINY(movement);
        }

        #region GetNENSOAReportHistory
        public List<NENSOAReportModel> GetNENSOAReportHistory(int month, int year, long organisationId)
        {
            return NENNotificationDAO.GetNENSOAReportHistory(month, year, organisationId);
        }
        #endregion

        #region GetNENHelpdeskReportHistory
        public List<NENHelpdeskReportModel> GetNENHelpdeskReportHistory(int month, int year, string vehicleCat, int requiresVr1, int vehicleCount)
        {
            return NENNotificationDAO.GetNENHelpdeskReportHistory(month, year, vehicleCat, requiresVr1, vehicleCount);
        }
        #endregion

        #region GetNENRouteID
        public long GetNENRouteID(int NENId, int inboxItemID, int organisationId, char returnVal)
        {
            return NENNotificationDAO.GetNENRouteID(NENId, inboxItemID, organisationId, returnVal);
        }
        #endregion

        #region Veri_RouteIdWithOtherOrg
        public int VerifyRouteIdWithOtherOrg(int NENId, int organisationId, int routePartId)
        {
            return NENNotificationDAO.VerifyRouteIdWithOtherOrg(NENId, organisationId, routePartId);
        }
        #endregion
        #region SaveNotifAutoResponseAuditLog
        public long SaveNotificationAutoResponseAuditLog(string logMessage, int userId, long organisationId)
        {
            return NENNotificationDAO.SaveNotificationAutoResponseAuditLog(logMessage,userId,organisationId);
        }
        #endregion
        public List<NENRouteStatusList> GetNENBothRouteStatus(int inboxId, int NENId, int userId, long organisationId)
        {
            return NENNotificationDAO.GetNENBothRouteStatus(inboxId, NENId, userId, organisationId);
        }

        public int GetRouteStatus(int inboxItemId, int NENId, int userId)
        {
            return NENNotificationDAO.GetRouteStatus(inboxItemId, NENId, userId);

        }

        public List<NENGeneralDetails> GetNENRouteDescription(int NENId, long inboxId, long organisationId)
        {
            return NENNotificationDAO.GetNENRouteDescription(NENId, inboxId, organisationId);
        }

        public List<NENGeneralDetails> GetRouteFromAndToDescp(long routepartId)
        {
            return NENNotificationDAO.GetRouteFromAndToDescp(routepartId);
        }

        public NENHaulierRouteDesc GetHualierRouteDesc(int NENId, int inboxItemId)
        {

            return NENNotificationDAO.GetHualierRouteDesc(NENId, inboxItemId);
        }

        public long GetNENDocumentIdFromInbox(long NENId, long inboxId, long organisationId)
        {

            return NENNotificationDAO.GetNENDocumentIdFromInbox(NENId, inboxId, organisationId);
        }
        public int UpdateInboxTypeFirstTime(long InboxId, long organisationId)
        {

            return NENNotificationDAO.UpdateInboxTypeFirstTime(InboxId, organisationId);
        }

        public NENGeneralDetails GetGeneralDetail(int NENId, int RouteId)
        {

            return NENNotificationDAO.GetGeneralDetail(NENId, RouteId);
        }
        public NotificationGeneralDetails GetNENNotifInboundDet(long NotifId, long NENId)
        {

            return NENNotificationDAO.GetNENNotifInboundDet(NotifId, NENId);
        }
        public int UpdateRouteStatus(int InboxId, int UserId, int RouteId, int RouteStatus, long OrganisationId)
        {

            return NENNotificationDAO.UpdateRouteStatus(InboxId, UserId, RouteId, RouteStatus, OrganisationId);
        }
        public bool InsertInboxEditRouteForNewUser(int InboxId, long NENId, int NotificationId, int NewUserId, long EditedRouteId, long NewRouteId, long OrganisationId)
        {
            return NENNotificationDAO.InsertInboxEditRouteForNewUser(InboxId, NENId, NotificationId, NewUserId, EditedRouteId, NewRouteId, OrganisationId);
        }
        public long GetNENReturnRouteID(int InboxItemId, int orgId)
        {
            return NENNotificationDAO.GetNENReturnRouteID(InboxItemId, orgId);
        }
        public int UpdateNENICAStatusInboxItem(int InboxId, int IcaStatus, long OrganisationId)
        {
            return NENNotificationDAO.UpdateNENICAStatusInboxItem(InboxId, IcaStatus, OrganisationId);
        }

        public int UpdateNenApiIcaStatus(UpdateNENICAStatusParams updateNENICA)
        {
            return NENNotificationDAO.UpdateNenApiIcaStatus(updateNENICA);
        }
    }
}