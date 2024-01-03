using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SOApplicationTabs
    {
        public int VersionStatus { get; set; }
        public long VersionID { get; set; }
        public string TabStatus { get; set; }
        public long AnalysisID { get; set; }
        public int ApplicationStatus { get; set; }
        public string ESDAL_Reference { get; set; }
        public long ApplicationrevId { get; set; }
        public bool IsVR1Applciation { get; set; }
        public bool Reduceddetailed { get; set; }
        public int EnterBySort { get; set; }
        public int IsDistributed { get; set; }
        public long FolderID { get; set; }
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
            if (VersionID != 0)
            {
                /*  General, Contacted parties, Proposed route, Route assessment, Hauliers application, Notification history tab disabled */
                TabStatus = "ShowCollaboration";
            }
            if (VersionStatus == 305004 || VersionStatus == 305005 || VersionStatus == 305006)
            {
                /*General, Contacted parties, Agreed route (changed from Proposed route), Route assessment, Hauliers application, Notification history tab disabled*/
                TabStatus = "ShowCollaborationAgreed";
            }
            if (VersionStatus == 305004 || VersionID != 0)
            {
                TabStatus = "ShowNotification";
            }
            return TabStatus;
        }
        #endregion
    }
}