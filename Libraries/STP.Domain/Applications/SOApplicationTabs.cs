using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SOApplicationTabs
    {
        public int VersionStatus { get; set; }
        public long VersionId { get; set; }
        public string TabStatus { get; set; }
        public long AnalysisId { get; set; }
        public int ApplicationStatus { get; set; }
        public string ESDALReference { get; set; }
        public long ApplicationRevisionId { get; set; }
        public bool IsVR1Applciation { get; set; }
        public bool ReducedDetailed { get; set; }
        public int EnterBySORT { get; set; }
        public int IsDistributed { get; set; }
        public long FolderId { get; set; }
        //1)The haulier creates a SO application.
        /*General
         Vehicles
         Route  
        * 
        * (All Tabs are editable)
       */
        #region WorkInProgress
        public string ShowWorkInProgress()
        {
            if (VersionStatus == 305001)
            {
                TabStatus = "ShowWorkInProgress";
            }
            return TabStatus;
        }
        #endregion


        //1)The haulier submits the application to SORT
        //2)Allocate the application to SORT team member
        //3)SORT route planning and checking


        #region ShowTab
        public string ShowTab()
        {
            if (ApplicationStatus == 308002 || ApplicationStatus == 308003)
            {
                /* General, Haulier application (and in this tab one can go to route and vehicle) */
                TabStatus = "ShowSubmitted";
            }
            if (VersionId != 0)
            {
                /*  General, Contacted parties, Proposed route, Route assessment, Hauliers application, Notification history tab disabled */
                TabStatus = "ShowCollaboration";
            }
            if (VersionStatus == 305004 || VersionStatus == 305005 || VersionStatus == 305006)
            {
                /*General, Contacted parties, Agreed route (changed from Proposed route), Route assessment, Hauliers application, Notification history tab disabled*/
                TabStatus = "ShowCollaborationAgreed";
            }
            if (VersionStatus == 305004 || VersionId != 0)
            {
                TabStatus = "ShowNotification";
            }
            return TabStatus;
        }
        #endregion
    }
}