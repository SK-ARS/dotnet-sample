using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.MovementsAndNotifications.Providers
{
    public class MovementInboxSubcontent : IMovementInboxSubcontent
    {
        #region MovementInboxSubcontent Singleton

        private MovementInboxSubcontent()
        {
        }
        public static MovementInboxSubcontent Instance
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
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly MovementInboxSubcontent instance = new MovementInboxSubcontent();
        }

        #region Logger instance

        private const string PolicyName = "MovementInboxSubcontent";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion


        #endregion

        #region MovementInboxSubcontent implementation

        public List<InboxSubContent> GetInboxSubContent(int pageNumber, int pageSize, int versionId, int orgId, int notifhistory)
        {
            return MovementsDAO.GetMovementInboxSubContent(pageNumber,pageSize,versionId, orgId, notifhistory);
        }

        #endregion

        #region
        public List<InboxSubContent> GetSORTHistoryDetails(string esdalref, int versionno)
        {
            return MovementsDAO.GetSORTHistoryDetails(esdalref, versionno);
        }
        #endregion
    }
}