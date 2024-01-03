using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Routes;
using STP.Domain.VehiclesAndFleets.Configuration;
using System.Collections.Generic;

namespace STP.Domain.Workflow
{
    public class PlanMvmntPayLoad
    {
        public string NextActivity { get; set; }
        public bool IsVr1App { get; set; }
        public bool IsApp { get; set; }
        public bool IsSortApp { get; set; }
        public bool IsNotif { get; set; }
        public string OrgName { get; set; }
        public int OrgId { get; set; }
        public long MovementKey { get; set; }
        public long VehicleMoveId { get; set; }
        public bool IsAppClone { get; set; }
        public bool IsNotifClone { get; set; }
        public bool IsRenotify { get; set; }
        public bool IsRevise { get; set; }
        public bool IsNotifyApp { get; set; }
        public bool IsSupplimentarySaved { get; set; }
        public bool IsSoOverView { get; set; }
        public bool IsNotifGeneralSaved { get; set; }
        public bool IsAgreedNotified { get; set; }
        public bool IsApproveNotified { get; set; }
        public string ContenRefNo { get; set; }
        public long VersionId { get; set; }
        public long AnalysisId { get; set; }
        public long RevisionId { get; set; }
        public long NotificationId { get; set; }
        public List<AppVehicleConfigList> VehicleList { get; set; }
        public VehicleMovementType MvmntType { get; set; }
        public List<VehicleAssignment> VehicleAssignmentList { get; set; }
        public bool IsRouteVehicleAssigned { get; set; }
        public double NextAction { get; set; }
        public double ActionCompleted { get; set; }
        public int MovementType { get; set; }
        public int CurrentMovementType { get; set; }
        public int PrevMovType { get; set; }
        public int VehicleClass { get; set; }
        public bool RequireVr1 { get; set; }
        public bool RequireSO { get; set; }
        public bool IsVSO { get; set; }
        public int VSOType { get; set; }
        public List<NotifDispensations> DipesnsationList { get; set; }
        public List<ReturnRouteMapping> ReturnRouteMappings { get; set; }
        public bool IsRouteAssessmentDone{ get; set; }
        public bool IsVehicleAmended { get; set; }
        public int VehiclePurpose { get; set; }
        public int IsRouteSummaryPage { get; set; }
        public int NoticePeriodFlag { get; set; }
        public bool IsTermsAndConditionsAccepted { get; set; }
        public string PreviousContactName { get; set; }
        public string ImminentMessage { get; set; }

        public PlanMvmntPayLoad()
        {
            DipesnsationList = new List<NotifDispensations>();
            ReturnRouteMappings = new List<ReturnRouteMapping>();
        }
    }
}
