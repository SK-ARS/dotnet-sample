using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow.Models
{
    public class AllocateSORTUserCntrlModel
    {
        public long projectId { get; set; } = 0;
        public long pln_user_id { get; set; } = 0;
        public string due_date { get; set; } = "";
        public int revisionNo { get; set; } = 0;
        public string DropSort { get; set; } = "";
        public string EsdalRef { get; set; } = "";
        public string ProjectOwner { get; set; } = "";
        public bool firstAllocate { get; set; } = false;
        public bool isVr1Application { get; set; } = false;
        public bool isWorkflow { get; set; } = false;
        public int versionNo { get; set; } = 0;
    }
    public class SaveCandidateRouteCntrlModel
    {
        public string RouteType { get; set; }
        public string name { get; set; }
        public int ProjectId { get; set; } = 0;
        public int AnalysisId { get; set; } = 0;
        public int LatestRevId { get; set; } = 0;
        public int AppRevisionId { get; set; } = 0;
        public string EsdalRef { get; set; } = "";
        public string EsdalReferenceWorkflow { get; set; } = "";
        public bool isVr1Application { get; set; } = false;
        public bool isWorkflow { get; set; } = false;
        public int lastrevisionno { get; set; } = 0;
        public int lastversionno { get; set; } = 0;
    }
    public class CheckerUpdationCntrlModel
    {
        public int Projectid { get; set; }
        public int Userid { get; set; }
        public int Status { get; set; }
        public int CandRouteId { get; set; } = 0;
        public int CandRevisionId { get; set; } = 0;
        public int CandVersiono { get; set; } = 0;
        public string CheckerName { get; set; } = "";
        public int AnalysisId { get; set; } = 0;
        public string AppRef { get; set; } = "";
        public long PStatus { get; set; } = 0;
        public int MovVerNo { get; set; } = 0;
        public bool isVr1Application { get; set; } = false;
        public bool isWorkflow { get; set; } = false;
        public int lastrevisionno { get; set; } = 0;
        public int lastversionno { get; set; } = 0;
    }
    public class CreateMovementVersionCntrlModel
    {
        public int ProjectId { get; set; } = 0;
        public int AppRevisionId { get; set; } = 0;
        public int AnalysisId { get; set; } = 0;
        public int RouteRevisionId { get; set; } = 0;
        public string AppRef { get; set; } = "";
        public int MovVersionNo { get; set; } = 0;
        public string Haulnemonic { get; set; } = "";
        public int Esdalrefno { get; set; } = 0;
        public bool isVr1Application { get; set; } = false;
        public bool isWorkflow { get; set; } = false;
        public int lastrevisionno { get; set; } = 0;
    }
    public class SaveDistributionCommentsCntrlModel
    {
        public string EsdalReference { get; set; } = "";
        public string HaulierMnemonic { get; set; } = "";
        public string EsdalRef { get; set; } = "";
        public int VersionNo { get; set; } = 1;
        public int VersionId { get; set; } = 0;
        public string HaJobFileRef { get; set; } = "";
        public long ProjectStatus { get; set; } = 0;
        public decimal PreVersionDistr { get; set; } = 0;
        public int lastrevisionno { get; set; } = 0;
        public int ProjectId { get; set; } = 0;
    }
    public class CreateCandidateVersionCntrlModel
    {
        public int CandRouteId { get; set; } = 0;
        public int CandRevisionId { get; set; } = 0;
        public string CloneType { get; set; } = "";
        public int AppRevId { get; set; } = 0;
        public string EsdalRef { get; set; } = "";
        public string EsdalReferenceWorkflow { get; set; } = "";
        public int LastRevisionNo { get; set; } = 0;
        public int LastversionNo { get; set; } = 0;
    }
    public class MovementAgreeUnagreeWithdrawCntrlModel
    {
        public long Version_Id { get; set; } = 0;
        public int flag { get; set; } = 0;
        public string esdalRef { get; set; } = "";
        public int ProjectId { get; set; } = 0;
        public string AppRef { get; set; } = "";
        public int AnalysisId { get; set; } = 0;
        public int VersionNo { get; set; } = 0;
        public string EsdalReferenceWorkflow { get; set; } = "";
        public bool isVr1Application { get; set; } = false;
        public bool isWorkflow { get; set; } = false;
        public int RevisionNo { get; set; } = 0;
    }
    public class CreateSpecialOrderCntrlModel
    {
        public string sedalno { get; set; }
        public int ProjectId { get; set; }
        public int VersionId { get; set; }
        public string SONumber { get; set; }
        public long PlannerId { get; set; }
        public string ProjectStatus { get; set; }
        public string EsdalReferenceWorkflow { get; set; } = "";
    }
    public class AllocateVr1CheckerUserCntrlModel
    {
        public int Projectid { get; set; }
        public int Userid { get; set; }
        public int Status { get; set; }
        public string CheckerName { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public int MovVerNo { get; set; } = 0;
        public string AppRef { get; set; } = "";
        public bool isWorkflow { get; set; } = false;
        public int lastrevisionno { get; set; } = 0;
        public int lastversionno { get; set; } = 0;
    }
    public class ApproveVr1CntrlModel
    {
        public long ProjectId { get; set; } = 0;
        public int Rev_No { get; set; } = 0;
        public string EsdalRef { get; set; } = "";
        public int VR1_No { get; set; } = 0;
        public int VersionNo { get; set; } = 0;
        public int RevisionNo { get; set; } = 0;
    }
    public class InsertMovementTypeCntrlModel
    {
        public long movementId { get; set; }
        public bool startApplicationProcess { get; set; } = false;
        public string haulierRefNo { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string fromSummary { get; set; }
        public string toSummary { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string ImminentMessage { get; set; }
        public int applicationType { get; set; }
        public long allocateUserId { get; set; }
    }
    public class MovementRouteCntrlModel
    {
        public long apprevisionId { get; set; } = 0;
        public long versionId { get; set; } = 0;
        public string contRefNum { get; set; } = "";
        public bool isNotif { get; set; } = false;
        public string workflowProcess { get; set; } = "";
        public int NotificationEditFlag { get; set; } = 0;
        public int IsReturnAvailable { get; set; }
        public int IsRouteModify { get; set; }
        public int IsRouteSummaryPage { get; set; }
    }
    

    public class MovementSelectRouteByImportCntrlModel
    {
        public string importFrm { get; set; } = "";
        public int isFavourite { get; set; } = 0;
        public string workflowProcess { get; set; } = "";
        public bool Iscandidiate { get; set; } = false;
        public long NotificationId { get; set; }
        public long ApplicationId { get; set; }
        public bool IsVr1Appln { get; set; }
        public bool BackToMovementPreviousList { get; set; } = false;
    }
    public class SetNotificationGeneralDetailsCntrlModel
    {       
        public long notificationId { get; set; } = 0;
        public string workflowProcess { get; set; } = "";
    }

    public class UpdateMovementTypeCntrlModel
    {
        public long movementId { get; set; }
        public long notificationId { get; set; }
        public long appRevisionId { get; set; }
        public bool startApplicationProcess { get; set; } = false;
        public string haulierRefNo { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string fromSummary { get; set; }
        public string toSummary { get; set; }
        public int applicationType { get; set; }
        public long allocateUserId { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string ImminentMessage { get; set; }
    }
}
