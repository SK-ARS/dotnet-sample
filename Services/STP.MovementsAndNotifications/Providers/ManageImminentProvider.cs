using STP.Domain;
using STP.MovementsAndNotifications.Interface;
using STP.MovementsAndNotifications.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.MovementsAndNotifications.Notification;

namespace STP.MovementsAndNotifications.Providers
{
    public class ManageImminentProvider : IManageImminent
    {
        #region ManageImminentProvider Singleton

        private ManageImminentProvider()
        {
        }
        public static ManageImminentProvider Instance
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
            internal static readonly ManageImminentProvider instance = new ManageImminentProvider();
        }

        #region Logger instance

        private const string PolicyName = "ManageImminentProvider";

        #endregion


        #endregion

        #region showImminentMovement(vehicleclass, moveStartDate)

        public int ShowImminentMovement(string moveStartDate, string countryId, int countryIdCount, int vehicleClass)
        {
            return ManageImminentDAO.showImminentMovementDAO(moveStartDate, countryId, countryIdCount, vehicleClass);
        }
        #endregion

        #region Removed Unwanted code by Mahzeer on 04-12-2023
        //public GetImminentChkDetailsDomain GetDetailsToChkImminent(long notificationId, string contentReferenceNo, long revisionId, string userSchema)
        //{
        //    return ManageImminentDAO.getDetailsToChkImminent( notificationId,  contentReferenceNo,  revisionId, userSchema);
        //}
        //public int CheckImminent(int vehicleclass, decimal vehiWidth, decimal vehiLength, decimal rigidLength, decimal GrossWeight, int WorkingDays, decimal FrontPRJ, decimal RearPRJ, decimal LeftPRJ, decimal RightPRJ, GetImminentChkDetailsDomain objImminent, string Notif_type = null)
        //{
        //    return ManageImminentDAO.CheckImminent(vehicleclass, vehiWidth, vehiLength, rigidLength, GrossWeight, WorkingDays, FrontPRJ, RearPRJ, LeftPRJ, RightPRJ, objImminent, Notif_type);
        //}
        #endregion
    }
}