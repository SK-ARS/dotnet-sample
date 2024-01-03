using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.MovementsAndNotifications.Interface
{
    public interface IQuickLinks
    {
        string GetContentReferenceNo(int notificationNo);

        int InsertQuickLinkSOA(int organisationId, int inboxId, int userId);

        int InsertQuickLink(InsertQuickLinkParams objInsertQuickLinkParams);

        List<QuickLinksSOA> GetQuickLinksSOAList(int userId);

        List<QuickLinks> GetQuickLinksList(int userId);
    }
}