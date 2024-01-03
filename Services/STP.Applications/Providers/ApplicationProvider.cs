using STP.Applications.Interface;
using STP.Applications.Persistance;
using STP.Common.Constants;
using System.Collections.Generic;
using System.Diagnostics;
using STP.Domain.Structures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Applications;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.Applications.Providers
{
    public sealed class ApplicationProvider : IApplication
    {
        #region ApplicationProvider Singleton

        private ApplicationProvider()
        {
        }
        public static ApplicationProvider Instance
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
            internal static readonly ApplicationProvider instance = new ApplicationProvider();
        }
        #endregion

        #region Get VR1 General
        public ApplyForVR1 GetVR1General(string userSchema, long revisionId, long versionId, long organisationId, int historic)
        {
            return ApplicationDAO.GetVR1General(userSchema, revisionId, versionId, organisationId, historic);
        }
        #endregion

        #region Get SO Application Details
        public SOApplicationTabs GetSOApplicationTabDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            return ApplicationDAO.GetSOApplTabDetails(revisionId, versionId, userSchema, historic);
        }
        #endregion

        #region Reset Need Attention
        public int ResetNeedAttention(long projectID, long revisionID, long versionID)
        {
            return ApplicationDAO.ResetNeedAttention(projectID, revisionID, versionID);
        }
        #endregion

        #region Update_Needs_Attention
        public int UpdateNeedsAttention(int notificationID, int revisionID, int naFlag)
        {
            return ApplicationDAO.UpdateNeedsAttention(notificationID, revisionID, naFlag);
        }
        #endregion

        #region Get Project Folder Details
        public ProjectFolderModel GetFolderDetails(ProjectFolderModelParams objProjectFolderModelParams)
        {
            return ApplicationDAO.GetProjectFolderDetails(objProjectFolderModelParams);
        }
        #endregion

        #region Get Project Folder List
        public List<ProjectFolderModel> GetProjectFolderList(int organisationId)
        {
            return ApplicationDAO.GetProjectFolderList(organisationId);
        }
        #endregion

        #region GetStructureDetails
        /// <summary>
        /// getting structure general detail list from route assessment of affected structure's
        /// </summary>
        /// <param name="structureCode"></param>
        /// <returns></returns>
        public List<AffStructureGeneralDetails> GetStructureDetailList(string structureCode, int sectionId = 0)
        {
            return ApplicationDAO.GetStructureGeneralDetailList(structureCode, sectionId);
        }
        #endregion

        #region GetStructureDetails
        public HAContact GetHAContactDetails(decimal ContactId)
        {
            return ApplicationDAO.GetHAContactDetails(ContactId);
        }
        #endregion

        #region viewAffStructureSections
        public List<AffStructureSectionList> ViewAffStructureSections(string structureCode)
        {
            return ApplicationDAO.ViewAffStructureSections(structureCode);
        }
        #endregion

        #region UpdatePartId
        public int UpdatePartId(UpdatePartIdInputParams updatePartIdInputParams)
        {
            return ApplicationDAO.UpdatePartId(updatePartIdInputParams);
        }
        #endregion

        #region UpdateVR1Application
        public bool UpdateVR1Application(ApplyForVR1 vr1Application, int organisationId, int userId, long applicationRevId)
        {
            return ApplicationDAO.UpdateVR1Application(vr1Application, organisationId, userId, applicationRevId);
        }
        #endregion

        #region SaveVR1Application
        public ApplyForVR1 SaveVR1Application(ApplyForVR1 vr1Application, int organisationId, int userId)
        {
            return ApplicationDAO.SaveVR1Application(vr1Application, organisationId, userId);
        }
        #endregion

        #region GetSOGeneralWorkInProcessByRevisionId
        public SOApplication GetSOGeneralWorkInProcessByRevisionId(string userSchema, long revisionId, long versionId, int orgId)
        {
            return ApplicationDAO.GetSOGeneralWorkInProcessByRevisionId(userSchema, revisionId, versionId, orgId);
        }
        #endregion

        #region GetSOGeneralDetails
        public SOApplication GetSOGeneralDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            return ApplicationDAO.GetSOGeneralDetails(revisionId, versionId, userSchema, historic);
        }
        #endregion

        #region GetSONumberStatus
        public string GetSONumberStatus(int projectID, string userSchema = UserSchema.Portal)
        {
            return ApplicationDAO.GetSONumberStatus(projectID, userSchema);
        }
        #endregion

        #region VR1SupplementaryInfo
        public bool VR1SupplementaryInfo(SupplimentaryInfoParams objSupplimentaryInfoParams)
        {
            return ApplicationDAO.VR1SupplementaryInfo(objSupplimentaryInfoParams);
        }
        #endregion

        #region Delete Application
        public int DeleteApplication(long apprevisionID, string userSchema)
        {
            return ApplicationDAO.DeleteApplication(apprevisionID, userSchema);
        }
        #endregion

        #region CheckLatestAppStatus
        public SOApplication CheckLatestAppStatus(long projectId)
        {
            return ApplicationDAO.CheckLatestAppStatus(projectId);
        }
        #endregion

        #region WithdrawApplication
        public ApplicationWithdraw WithdrawApplication(long projectId, long appRevId)
        {
            return ApplicationDAO.WithdrawApplication(projectId, appRevId);
        }
        #endregion

        #region Revise SO Application
        public SOApplication ReviseSOApplication(long apprevId, string userSchema)
        {
            return ApplicationDAO.ReviseSOApplication(apprevId, userSchema);
        }
        #endregion

        #region Clone SO Application
        public SOApplication CloneSOApplication(long apprevId, int organisationId, int userId)
        {
            return ApplicationDAO.CloneSOApplication(apprevId, organisationId, userId);
        }
        #endregion

        #region Clone SO History Application
        public SOApplication CloneSOHistoryApplication(long apprevId, int organisationId, int userId, string userSchema)
        {
            return ApplicationDAO.CloneSOHistoryApplication(apprevId, organisationId, userId,userSchema);
        }
        #endregion

        #region ReviseVR1Application
        public ApplyForVR1 ReviseVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema)
        {
            return ApplicationDAO.ReviseVR1Application(apprevId, reducedDet, cloneApp, versionId, userSchema);
        }
        #endregion

        #region CloneHistoryVR1Application
        public ApplyForVR1 CloneHistoryVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema)
        {
            return ApplicationDAO.CloneHistoryVR1Application(apprevId, reducedDet, cloneApp, versionId, userSchema);
        }
        #endregion

        #region UpdateSOApplication
        public bool SaveAppGeneral(SOApplication soApplication, int organisationId, int userId, string userSchema, long applicationRevId)
        {
            return ApplicationDAO.SaveAppGeneral(soApplication, organisationId, userId, userSchema, applicationRevId);
        }
        #endregion UpdateSOApplication

        #region SaveSOApplication
        public long SaveSOApplication(SOApplication soApplication, int organisationId, int userId)
        {
            return ApplicationDAO.SaveSOApplication(soApplication, organisationId, userId);
        }
        #endregion SaveSOApplication

        #region CheckSOValidation
        public ApplyForVR1 CheckSOValidation(int apprevisionId, string userSchema = UserSchema.Portal)
        {
            return ApplicationDAO.CheckSOValidation(apprevisionId, userSchema);
        }
        #endregion

        #region GetApplicationStatus
        public int GetApplicationStatus(int versionno, int RevisionNo, long ProjectId, string userSchema, int historic)
        {
            return ApplicationDAO.GetApplStatus(versionno, RevisionNo, ProjectId, userSchema, historic);
        }
        #endregion

        #region GetHAContDetFromInboundDoc
        public HAContact GetHAContDetFromInboundDoc(string esdalReference)
        {
            return ApplicationDAO.GetHAContDetFromInboundDoc(esdalReference);
        }
        #endregion

        #region CheckVR1Validation
        public ApplyForVR1 CheckVR1Validation(int versionId, int ShowVehicle, string contentref, int apprevisionId, string userSchema = UserSchema.Portal)
        {
            return ApplicationDAO.CheckVR1Validation(versionId, ShowVehicle, contentref, apprevisionId, userSchema);
        }
        #endregion

        #region ListVR1RouteDetails
        public List<VR1RouteImport> ListVR1RouteDetails(string contentref)
        {
            return ApplicationDAO.ListVR1RouteDetails(contentref);
        }
        #endregion

        #region SubmitSoApplication
        public SOApplication SubmitSoApplication(int apprevisionId, int userId)
        {
            return ApplicationDAO.SubmitSoApplication(apprevisionId, userId);
        }
        #endregion SubmitSoApplication

        #region SubmitVR1Application
        public ApplyForVR1 SubmitVR1Application(int apprevisionId, int reducedDetails)
        {
            return ApplicationDAO.SubmitVR1Application(apprevisionId, reducedDetails);
        }
        #endregion SubmitSoApplication

        #region GetVR1VehicleDEtails
        public ApplyForVR1 GetVR1VehicleDetails(VR1VehicleDetailsParams vr1VehicleDetailsParams)
        {
            return ApplicationDAO.GetVR1VehicleDEtails(vr1VehicleDetailsParams);
        }
        #endregion

        #region GetSOHaulierDetails
        public SOHaulierApplication GetSOHaulierDetails(long revisionId, long versionId, int historic)
        {
            return ApplicationDAO.GetSOHaulApplDetails(revisionId, versionId, historic);
        }
        #endregion

        #region GetSORTVR1GeneralDetails(ProjectID)
        public ApplyForVR1 GetSORTVR1GeneralDetails(int ProjectID, string userSchema)
        {
            return ApplicationDAO.GetSORTVR1GeneralDetails(ProjectID, userSchema);
        }
        #endregion

        #region GetSORTHaulierAppRouteParts
        public List<AffectedStructures> GetSORTHaulierAppRouteParts(int versionID, string vr1ContentRefNo, string userSchema)
        {
            return ApplicationDAO.GetSORTHaulierAppRouteParts(versionID, vr1ContentRefNo, userSchema);
        }
        #endregion

        #region GetHaulierApplRouteParts
        public List<AffectedStructures> GetHaulierApplRouteParts(int revisionID, int appFlag, int sortRouteVehicleFlag, string userSchema)
        {
            return ApplicationDAO.GetHaulierApplRouteParts(revisionID, appFlag, sortRouteVehicleFlag, userSchema);
        }
        #endregion

        #region Insert Plan Movemnt App
        public AppGeneralDetails InsertApplicationType(PlanMovementType saveAppType)
        {
            return ApplicationDAO.InsertApplicationType(saveAppType);
        }
        public AppGeneralDetails UpdateApplicationType(PlanMovementType updateAppType)
        {
            return ApplicationDAO.UpdateApplicationType(updateAppType);
        }
        #endregion

        #region VR1GetSupplementaryInfo
        public SupplimentaryInfo VR1GetSupplementaryInfo(int apprevisionId=0, string userSchema= UserSchema.Portal, int historic = 0)
        {
            return ApplicationDAO.VR1GetSupplementaryInfo(apprevisionId, userSchema, historic);
        }
        #endregion

        #region GetNotifRouteParts
        public List<AffectedStructures> GetNotifRouteParts(int notificationId, int rpFlag)
        {
            return ApplicationDAO.GetNotifRoutePart(notificationId, rpFlag);
        }
        #endregion

        #region IMP_CondidateRoue
        public long IMP_CondidateRoue(int routepartId, int appRevId, int versionId, string contentRef, string userSchema)
        {
            return ApplicationDAO.IMP_CondidateRoue(routepartId, appRevId, versionId, contentRef, userSchema);
        }
        #endregion

        #region EsdalRefNum
        public string EsdalRefNum(int SOVersionID)
        {
            return ApplicationDAO.EsdalRefNum(SOVersionID);
        }
        #endregion

        #region GetAgreedRouteParts
        public List<AffectedStructures> GetAgreedRoutePart(int VersionId, int revisionid, string userSchema, string ContentRefNo)
        {
            return ApplicationDAO.GetAgreedRoutePart(VersionId, revisionid, userSchema, ContentRefNo);
        }
        #endregion

        #region GetApplicationDetails
        public PlanMovementType GetApplicationDetails(long revisionId, string userSchema)
        {
            return ApplicationDAO.GetApplicationDetails(revisionId, userSchema);
        }
        #endregion

        #region  Get Application Data
        public PlanMovementType GetNotificationDetails(long notificationId, string userSchema)
        {
            return ApplicationDAO.GetNotificationDetails(notificationId, userSchema);
        }

        #endregion

        #region ExportApplicationData
        public Domain.ExternalAPI.ExportAppGeneralDetails ExportApplicationData(string ESDALReferenceNumber, string userSchema)
        {
            return ApplicationDAO.ExportApplicationData(ESDALReferenceNumber, userSchema);
        }
        #endregion
    }
}