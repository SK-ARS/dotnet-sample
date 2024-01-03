using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.MovementsAndNotifications.Interface
{
   public interface IMovementInboxSubcontent
    {
        /// <summary>
        /// Interface to get movement inbox subcontent list
        /// </summary>
        /// <returns></returns>
        List<InboxSubContent> GetInboxSubContent(int pageNumber, int pageSize,int versionId, int orgId, int notifhistory);
        /// <summary>
        /// GetSORTHistoryDetails
        /// </summary>
        /// <param name="esdalref"></param>
        /// <returns></returns>
        List<InboxSubContent> GetSORTHistoryDetails(string esdalref, int versionno);
    }
}
