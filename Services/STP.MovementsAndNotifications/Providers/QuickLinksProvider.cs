using STP.Domain.MovementsAndNotifications.Notification;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.MovementsAndNotifications.Providers
{
    public sealed class QuickLinksProvider : IQuickLinks
    {
        #region MovementProvider Singleton

        private QuickLinksProvider()
        {
        }
        public static QuickLinksProvider Instance
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
            internal static readonly QuickLinksProvider instance = new QuickLinksProvider();
        }
        #endregion

        #region Get Content Reference No
        public string GetContentReferenceNo(int notificationNo)
        {
            return QuickLinksDAO.Get_CONTENT_REF_NO(notificationNo);
        }
        #endregion

        #region Insert QuickLink SOA
        public int InsertQuickLinkSOA(int organisationId, int inboxId, int userId)
        {
            return QuickLinksDAO.InsertQuickLinksSOA(organisationId, inboxId, userId);
        }
        #endregion

        #region Insert Quick Link
        public int InsertQuickLink(InsertQuickLinkParams objInsertQuickLinkParams)
        {
            return QuickLinksDAO.InsertQuickLinks(objInsertQuickLinkParams);
        }
        #endregion

        #region Get QuickLinks SOA List    
        public List<QuickLinksSOA> GetQuickLinksSOAList(int userId)
        {
            return QuickLinksDAO.GetQuickLinksSOA(userId);
        }
        #endregion

        #region
        public List<QuickLinks> GetQuickLinksList(int userId)
        {
            return QuickLinksDAO.GetQuickLinks(userId);
        }
        #endregion
    }
}