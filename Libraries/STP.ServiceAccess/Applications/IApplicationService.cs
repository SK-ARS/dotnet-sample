using STP.Common.Constants;
using System.Collections.Generic;
using STP.Domain.Structures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.Applications
{
    public interface IApplicationService
    {
        string GetEsdalRefNum(int SOVersionID);
        ApplyForVR1 GetVR1General(string userSchema, long revisionid = 0, long versionid = 0, int organisationId = 0, int historic = 0);
        SOApplicationTabs GetSOApplicationTabDetails(long revisionId, long versionId, string userSchema, int historic);
        int ResetNeedAttention(long projectID, long revisionID, long versionID);
        int UpdateNeedsAttention(int notificationID = 0, int revisionID = 0, int naFlag = 0);
        List<ProjectFolderModel> GetProjectFolderList(int orgId);
        int UpdatePartId(int VehicleId, int PartId, bool VR1Appl, bool Notif, string RType,bool Iscand, string userSchema = UserSchema.Portal);
        ProjectFolderModel GetFolderDetails(int flag, long folderID, long projectId, string hauliermnemonic, int esdalref, long notificationId, long revisionID);
        List<AffStructureGeneralDetails> GetStructureDetailList(string StructureCode, int SectionId = 0);
        HAContact GetHAContactDetails(decimal ContactId);
        List<AffStructureSectionList> ViewAffStructureSections(string structureCode);
        bool DeleteApplication(long apprevisionId, string userSchema);
        SOApplication CheckLatestAppStatus(long projectId);
        ApplicationWithdraw WithdrawApplication(long projectId, long appRevId);
        SOApplication ReviseSOApplication(long apprevId, string userSchema);
        SOApplication CloneSOApplication(long apprevId, int organisationId, int userId);
        SOApplication CloneSOHistoryApplication(long apprevId, int organisationId, int userId, string userSchema);
        ApplyForVR1 ReviseVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema);
        ApplyForVR1 CloneHistoryVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema);
        SOApplication GetSOGeneralDetails(long revisionId, long versionId, string userSchema, int historic);
        SOApplication GetSOGeneralWorkinProcessbyrevisionid(string userSchema, long revisionId = 0, long versionId = 0, int Org_id = 0);
        string GetSONumberStatus(int project_id, string userSchema = UserSchema.Portal);
        bool UpdateVR1Application(ApplyForVR1 vr1application, int organisationId, int userId, long apprevid);
        ApplyForVR1 SaveVR1Application(ApplyForVR1 vr1application, int organisationId, int userId);
        bool SaveAppGeneral(SOApplication soapplication, int organisationId, int userId, long applicationrevId,string userSchema);
        long SaveSOApplication(SOApplication soapplication, int organisationId, int userId);
        ApplyForVR1 CheckSOValidation(int apprevisionId, string userSchema = UserSchema.Portal);
        int GetApplicationStatus(int versionNo, int revisionNo, long projectId, string userSchema, int historic);
        HAContact GetHAContDetFromInboundDoc(string EsdalRef);
        ApplyForVR1 CheckVR1Validation(int versionId, int showVehicle, string contentRef, int apprevisionId, string userSchema = UserSchema.Portal);
        List<VR1RouteImport> ListVR1RouteDetails(string contentref);
        SOApplication SubmitSoApplication(int apprevisionId, int userId);
        ApplyForVR1 SubmitVR1Application(int apprevisionId, int reducedDet);
        ApplyForVR1 GetVR1VehicleDEtails(VR1VehicleDetailsParams vr1VehicleDetailsParams);
        SOHaulierApplication GetSOHaulierDetails(long revisionId, long versionId, int historic);
        bool SOVR1SupplementaryInfo(SupplimentaryInfo objSupplimentaryInfo, string userSchema, int applicationRevisionId);
        AppGeneralDetails InsertApplicationType(PlanMovementType saveApplication);
        AppGeneralDetails UpdateApplicationType(PlanMovementType updateApplication);
        SupplimentaryInfo VR1GetSupplementaryInfo(int apprevisionId = 0, string userSchema = UserSchema.Portal, int historic = 0);
        List<AffectedStructures> GetSORTHaulierAppRouteParts(int versionID, string vr1ContentRefNo, string userSchema);
        List<AffectedStructures> GetHaulierApplRouteParts(int revisionID, string appFlag, string sortRouteVehicleFlag, string userSchema);
        List<AffectedStructures> GetNotifRouteParts(int notificationid, int rpFlag);
        long IMP_CondidateRoue(int RoutePartId, int AppRevId, int VersionId, string ContentRef, string userSchema);
        List<AffectedStructures> GetAgreedRouteParts(int VersionId, int revisionid, string userSchema, string ContentRefNo = "");
        PlanMovementType GetApplicationDetails(long revisionId, string userSchema);
        PlanMovementType GetNotificationDetails(long notificationId, string userSchema);
    }
}
