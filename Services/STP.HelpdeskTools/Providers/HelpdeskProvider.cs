using STP.Domain.HelpdeskTools;
using STP.HelpdeskTools.Interface;
using STP.HelpdeskTools.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Providers
{
    public sealed class HelpdeskProvider : IHelpdesk
    {
        #region HelpdeskProvider Singleton

        private HelpdeskProvider()
        {
        }
        public static HelpdeskProvider Instance
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
            internal static readonly HelpdeskProvider instance = new HelpdeskProvider();
        }
        #endregion

        #region Get Distribution Alerts
        public List<DistributionAlerts> GetSORTDistributionAlerts(int pageNum, int pageSize, DistributionAlerts objDistributionAlert, int portalType,int? presetFilter=null,int? sortOrder=null)
        {
            return HelpdeskDAO.GetSORTDistributionAlerts(pageNum, pageSize, objDistributionAlert, portalType, (int)presetFilter,sortOrder);
        }
        #endregion
    }
}