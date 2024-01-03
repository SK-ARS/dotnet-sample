using STP.Domain.DocumentsAndContents;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.MovementsAndNotifications.Providers
{
    public class NotificationDocumentProvider: INotificationDocument
    {

        #region Singleton Pattern
        private NotificationDocumentProvider()
        {
        }
        internal static NotificationDocumentProvider Instance
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
            internal static readonly NotificationDocumentProvider instance = new NotificationDocumentProvider();
        }
        #region Logger instance

        private const string PolicyName = "NotificationDocumentProvider";

        #endregion

        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023
        /*public NotificationXSD.OutboundNotificationStructure GenerateOutboundNotificationStructureData(long NotificationId)
        {
            return NotificationDocumentDAO.GetOutboundNotificationDetailsForNotification(NotificationId);
        }

        public OutboundDocuments GetOutboundDoc(int notificationId)
        {
            OutboundDocuments outbounddocs = NotificationDocumentDAO.GetNotificationDetails(notificationId);
            return outbounddocs;
        }*/
        #endregion
    }
}