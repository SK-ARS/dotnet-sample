using STP.Common.Constants;
using System.Collections.Generic;
using STP.Domain.Structures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Applications;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.Applications.Interface
{
    public interface IApplication
    {
        ApplyForVR1 GetVR1General(string userSchema, long revisionId, long versionId, long organisationId, int historic);
        SOApplicationTabs GetSOApplicationTabDetails(long revisionId, long versionId, string userSchema, int historic);
        int ResetNeedAttention(long projectID, long revisionID, long versionID);
        int UpdateNeedsAttention(int notificationID, int revisionID, int naFlag);
        ProjectFolderModel GetFolderDetails(ProjectFolderModelParams objProjectFolderModelParams);
        List<ProjectFolderModel> GetProjectFolderList(int organisationId);
        List<AffStructureGeneralDetails> GetStructureDetailList(string structureCode, int sectionId = 0);
        HAContact GetHAContactDetails(decimal ContactId);
        int UpdatePartId(UpdatePartIdInputParams updatePartIdInputParams);
        List<AffStructureSectionList> ViewAffStructureSections(string structureCode);
        SOApplication GetSOGeneralWorkInProcessByRevisionId(string userSchema, long revisionId, long versionId, int orgId);
        SOApplication GetSOGeneralDetails(long revisionId, long versionId, string userSchema, int historic);
        string GetSONumberStatus(int projectID, string userSchema = UserSchema.Portal);
        bool VR1SupplementaryInfo(SupplimentaryInfoParams objSupplimentaryInfoParams);
        int DeleteApplication(long apprevisionID, string userSchema);
        SOApplication CheckLatestAppStatus(long projectId);
        ApplicationWithdraw WithdrawApplication(long projectId, long appRevId);
        SOApplication ReviseSOApplication(long apprevId, string userSchema);
        SOApplication CloneSOApplication(long apprevId, int organisationId, int userId);
        SOApplication CloneSOHistoryApplication(long apprevId, int organisationId, int userId, string userSchema);
        ApplyForVR1 ReviseVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema);
        bool UpdateVR1Application(ApplyForVR1 vr1Application, int organisationId, int userId, long applicationRevId);
        ApplyForVR1 SaveVR1Application(ApplyForVR1 vr1Application, int organisationId, int userId);
        bool SaveAppGeneral(SOApplication soApplication, int organisationId, int userId, string userSchema, long applicationRevId);
        long SaveSOApplication(SOApplication soApplication, int organisationId, int userId);
        ApplyForVR1 CheckSOValidation(int apprevisionId, string userSchema = UserSchema.Portal);
        int GetApplicationStatus(int versionno, int RevisionNo, long ProjectId, string userSchema, int historic);
        HAContact GetHAContDetFromInboundDoc(string esdalReference);
        ApplyForVR1 CheckVR1Validation(int versionId, int ShowVehicle, string contentref, int apprevisionId, string userSchema = UserSchema.Portal);
        List<VR1RouteImport> ListVR1RouteDetails(string contentref);
        SOApplication SubmitSoApplication(int apprevisionId, int userId);
        ApplyForVR1 SubmitVR1Application(int apprevisionId, int reducedDetails);
        ApplyForVR1 GetVR1VehicleDetails(VR1VehicleDetailsParams vr1VehicleDetailsParams);
        SOHaulierApplication GetSOHaulierDetails(long revisionId, long versionId, int historic);
        List<AffectedStructures> GetHaulierApplRouteParts(int revisionID, int appFlag, int sortRouteVehicleFlag, string userSchema);
        List<AffectedStructures> GetSORTHaulierAppRouteParts(int versionID, string vr1ContentRefNo, string userSchema);
        AppGeneralDetails InsertApplicationType(PlanMovementType saveAppType);
        AppGeneralDetails UpdateApplicationType(PlanMovementType updateAppType);
        SupplimentaryInfo VR1GetSupplementaryInfo(int apprevisionId = 0, string userSchema = UserSchema.Portal, int historic = 0);
        List<AffectedStructures> GetNotifRouteParts(int notificationId, int rpFlag);
        long IMP_CondidateRoue(int routepartId, int appRevId, int versionId, string contentRef, string userSchema);
        string EsdalRefNum(int SOVersionID);
        List<AffectedStructures> GetAgreedRoutePart(int VersionId, int revisionid, string userSchema, string ContentRefNo);
        PlanMovementType GetApplicationDetails(long revisionId, string userSchema);
        PlanMovementType GetNotificationDetails(long notificationId, string userSchema);
        Domain.ExternalAPI.ExportAppGeneralDetails ExportApplicationData(string ESDALReferenceNumber, string userSchema);
    }
}