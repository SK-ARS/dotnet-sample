using STP.Common.Constants;
using System.Collections.Generic;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Applications;
using STP.Domain.Routes;
using System;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.ServiceAccess.VehiclesAndFleets
{
   public interface IVehicleConfigService
    {

        #region  vehicle insert-Update
        double CreateConfiguration(NewConfigurationModel configurationModel);
        bool Updatevehicleconfig(NewConfigurationModel configurationModel);
        bool Updateappvehicleconfig(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal);
        NewConfigurationModel InsertVR1VehicleConfiguration(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal);
        double InsertApplicationVehicleConfiguration(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal);
        bool UpdateVR1vehicleconfig(NewConfigurationModel configurationModel, string userSchema = UserSchema.Portal);
        VehicleConfigList CreateConfigPosn(VehicleConfigList ConfigList);
        int CreateVehicleRegistration(int vhclId, string registrationValue, string fleetId);
        int SaveVR1VehicleRegistration(int vhclId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal);
        int SaveAppVehicleRegistrationId(int vhclId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal);
        bool UpdateVehicleConfigDetails(int configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false);
        VehicleConfigList CreateApplVehConfigPosn(VehicleConfigList ConfigList, string userSchema = UserSchema.Portal);
        VehicleConfigList CreateAppConfigPosn(VehicleConfigList ConfigList, int isImportFromFleet, string userSchema = UserSchema.Portal);
        VehicleConfigList CreateVR1ConfigPosn(VehicleConfigList ConfigList, string userSchema = UserSchema.Portal);

        #endregion

        #region AddToFleet

        int AddComponentToFleet(int componentid, int organisationid);
        int AddApplicationComponentToFleet(int componentid, int organisationid, int flag, string userSchema = UserSchema.Portal);
        int AddVR1ComponentToFleet(int componentid, int organisationid, int flag, string userSchema = UserSchema.Portal);
        #endregion
        long ImportVehicleFromList(long configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", int IsCandidate = 0, string VersionType = "A");
        long CopyVehicleFromList(long configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", int IsCandidate = 0);
        ConfigurationModel GetRouteConfigInfo(int componentId, string userSchema);
        ConfigurationModel GetRouteConfigInfoForVR1(int componentId, string userSchema, int isEdit = 0);
        ConfigurationModel GetNotifVehicleConfiguration(int vhclId, int isSimple);
        bool CheckNotifValidVehicle(int vhclId);
        #region CHeckNameExists
        int CheckFormalName(int componentid, int organisationid);
        int CheckVR1FormalName(int componentid, int organisationid, string userSchema = UserSchema.Portal);
        int CheckAppFormalName(int componentid, int organisationid, string userSchema = UserSchema.Portal);
        #endregion
        #region Delete Vehicle
        bool DeletVehicleRegisterConfiguration(int vehicleid, int IdNumber, bool isMovement = false);
        int DeleteVehicleConfigPosn(int vehicleid, int latpos, int longpos);

        int DeleteVR1VehicleConfigPosn(int vehicleid, int latpos, int longpos, string userSchema = UserSchema.Portal);
        int DeleteApplicationVehicleConfigPosn(int vehicleid, int latpos, int longpos, string userSchema = UserSchema.Portal);

        bool DisableVehicleApi(int vehicleid);//DisableVehicle
        bool DeleteVR1RegConfig(int vehicleid, int IdNumber, string userSchema = UserSchema.Portal);
        bool DeleteAppRegConfig(int vehicleid, int IdNumber, string userSchema = UserSchema.Portal);
        #endregion

        List<VehicleRegistration> GetVehicleRegistrationDetails(int vhclId, string userSchema = UserSchema.Portal);
        List<VehicleRegistration> GetVR1VehicleRegistrationDetails(int vhclId, string userSchema = UserSchema.Portal);
        List<VehicleRegistration> GetApplVehicleRegistrationDetails(int vhclId, string userSchema = UserSchema.Portal);
        List<VehicleConfigList> GetVR1VehicleConfigVhclID(int vhclID, string userSchema);
        List<ComponentGridList> GetAllVR1ComponentByOrganisationId(long componentId, string userSchema);
        List<VehicleConfigList> GetAppVehicleConfigVhclID(int vhclID, string userSchema = UserSchema.Portal);
        List<VehicleConfigList> GetVehicleConfigVhclID(int vehicleId, string userSchema);
        List<ComponentGridList> GetAllAppComponentByOrganisationId(long componentId, string userSchema = UserSchema.Portal);
        List<ComponentGridList> GetAllComponentByOrganisationId(int organisationId, long componentId);
        List<ComponentIdModel> GetVR1ListComponentId(int vehicleId, string userSchema = UserSchema.Portal);
        List<ComponentIdModel> GetAppListComponentId(int vehicleId, string userSchema = UserSchema.Portal);
        List<ComponentIdModel> GetListComponentId(int vehicleId);
        List<VehicleConfigList> GetRouteVehicleConfigVhclID(int vhclID, string userSchema);
        long CheckValidVehicleCreated(int vhclID);
        List<VehicleRegistration> GetRouteVehicleRegistrationDetails(int vhclId, string userSchema);
        List<VehicleConfigurationGridList> GetConfigByOrganisationId(int organisationId, int movtype, int movetype1, string userSchema, int filterFavouritesVehConfig, int presetFilter = 1, int? sortOrder = null);
        ConfigurationModel GetNotifVehicleConfigByID(int VehicleId);
        ConfigurationModel GetConfigInfo(int componentId, string userSchema);
        ConfigurationModel GetConfigInfoApplication(int vehicleId, string userSchema = UserSchema.Portal);
        ConfigurationModel GetConfigInfoVR1(int componentId, string schematype = UserSchema.Portal);
        bool CheckWheelWithSumOfAxel(int vhclId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false);


        #region Vehicle Managemnet in Application
        long SaveSOApplicationvehicleconfig(int vehicleId, int apprevisionId, int routepartid, string userSchema);
        NewConfigurationModel SaveVR1Applicationvehicleconfig(NewConfigurationModel configurationModel);
        List<VehicleDetailSummary> Applicationvehiclelist(int PartID, int FlagSOAppVeh, string RouteType, string userSchema);
        int ImportApplnVehicleFromPreMove(int vehicleId, int apprevisionId, int routepartid, string userSchema);
        int ImportRouteVehicleToAppVehicle(int vehicleId, int apprevisionId, int routepartid, string userSchema);
        int VR1AppVehicleMovementList(int vehicleId, int apprevisionId, int routepartid, string userSchema);
        int AppVehicleMovementList(int vehicleId, int apprevisionId, int routepartid, string userSchema);
        bool DeleteSelectedVehicleComponent(int vehicleId, string userSchema);
        bool DeleteSelectedVR1VehicleComponent(int vehicleId, string userSchema);
        List<VehicleDetailSummary> ViewVehicleSummaryByID(long rPartId, int vr1, string userSchema);
        int CheckFormalNameInApplicationVeh(string vehicleName, int organisationId, string userSchema);
        int AddVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema);
        int AddVR1VhclToFleet(int vehicleId, int organisationId, int flag);
        List<AppVehicleConfigList> AppVehicleConfigList(long apprevisionId, string userSchema);
        List<AppVehicleConfigList> AppVehicleConfigListVR1(long routePartId, long versionId, string contentRefNo, string userSchema);
        List<AppVehicleConfigList> GetNenVehicleList(long routePartId);
        List<ApplVehiclComponents> ListVehCompDetails(int revisionId, string userschema);
        List<ApplVehiclComponents> ListLengthVehCompDetails(int revisionId, int vehicleId, string userschema);
        List<ApplVehiclComponents> ListVehWeightDetails(int revisionId, int vehicleId, string userschema, int isVR1);
        List<ApplVehiclComponents> ListVR1VehCompDetails(int versionId, string contentref);
        List<ApplVehiclComponents> ListLengthVR1VehDetails(int versionId, string contentref);
        #endregion

        #region Vehicle Managemnet in Notification
        ListRouteVehicleId ImportFleetRouteVehicle(int VehicleID, string ContentRefNo, int simple, int RoutePartId = 0);
        long ImportReturnRouteVehicle(int routePartId, string contentRefNo);
        int UpdateNotifRouteVehicle(NotificationGeneralDetails obj, int RoutePartId, int vehicleUnits);
        double CreateNotifVehicleConfiguration(NewConfigurationModel configurationModel, string contentRefNo, int isNotif);
        int SaveNotifVehicleRegistrationId(int vhclId, string registrationValue, string fleetId);
        int SaveNotifVehicleConfiguration(int vhclId, int compId, int compType);
        bool SaveNotifAxel(AxleDetails axle);
        bool UpdateMaxAxleWeight(long vehicleId);
        List<AxleDetails> ListCloneAxelDetails(int VehicleID);
        bool UpdateNewAxleDetails(AxleDetails axle);
        List<VehicleDetailSummary> GetNotificationVehicle(long partId);
        long ImportRouteVehicle(int routePartID, int vehicleID, string contentRefNo, int simple = 0);
        List<NotifVehicleRegistration> ListVehRegDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleImportDetails(string contentReferenceNo);
        List<NotifVehicleWeight> ListNotiVehWeightDetails(string ContentRefNo);
        List<NotifVehicleImport> ListVehicleLengthDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleGrossWeightDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleWidthDetails(string contentReferenceNo, int reqVR1);
        List<NotifVehicleImport> ListVehicleAxleWeightDetails(string contentReferenceNo); 
         List<NotifVehicleImport> ListVehicleRigidLengthDetails(string contentReferenceNo);
        bool DeletePrevVehicle(int routePartId);
        #endregion

        List<AutoAssessingParams> AutoVehicleConfigType(List<ComponentIdList> componentIds);
        List<AutoAssessingMovementType> AutoVehicleMovementType(List<ComponentIdList> componentIds);
        List<VehicleConfigurationGridList> GetSimilarVehicleCombinations(SearchVehicleCombination configDimensions);
        List<uint> AssessConfigurationType(List<int> componentIds, bool boatMastFlag = false, string userSchema = UserSchema.Portal, int flag = 0);
        List<uint> AssessMovementClassificationType(List<Int64> componentIds, int configurationType, bool isMovement, string userSchema = UserSchema.Portal);
        List<VehicleDetail> GetVehicleConfigByPartID(string ESDALRef, int vr1Vehicle);
        List<ComponentGroupingModel> ApplicationcomponentList(int routePartId);
        List<MovementVehicleConfig> InsertMovementVehicle(InsertMovementVehicle movementVehicle);
        List<MovementVehicleConfig> SelectMovementVehicle(long movementId, string userSchema);
        List<VehicleConfigList> GetMovementVehicleConfig(long vehicleId, string userSchema);
        ConfigurationModel GetMovementConfigInfo(long vehicleId, string userSchema);
        List<VehicleRegistration> GetMovementVehicleRegDetails(long vehicleId, string userSchema = UserSchema.Portal);
        List<MovementVehicleList> GetRouteVehicleList(long revisionId, long versionId, string cont_Ref_No, string userSchema, int isHistoric=0);
        bool AssignMovementVehicle(List<VehicleAssignment> vehicleAssignment, long revisionId, long versionId, string contRefNum, string userSchema);
        bool DeleteMovementVehicle(long movementId, long vehicleId, string userSchema);
        List<VehicleList> GetFavouriteVehicles(int organisationId, int movementId, string userSchema);
        ConfigurationModel GetConfigDimensions(string GUID, int configTypeId, string userSchema);
        ConfigurationModel GetVehicleDimensions(long VehicleId, int configTypeId, string userSchema);
        bool InsertVehicleConfigPosnTemp(string GUID, int vehicleId, string userSchema = UserSchema.Portal);
        ConfigurationModel GetVehicleDetails(int componentId,bool movement, string userSchema);
        VehicleMovementType AssessMovementType(AssessMoveTypeParams moveTypeParams);
        int CheckFormalNameExistsTemp(int componentId, int organisationId);
        List<MovementVehicleConfig> InsertConfigurationTemp(NewConfigurationModel configurationModel, string userSchema);
        int CreateVehicleRegistrationTemp(int vhclId, string registrationValue, string fleetId, string userSchema);
        bool InsertMovementConfigPosnTemp(string GUID, int vehicleId, string userSchema = UserSchema.Portal);
        bool UpdateMovementVehicle(NewConfigurationModel configurationModel, string userSchema);
        List<VehicleRegistration> GetVehicleRegistrationTemp(int vhclId, string userSchema = UserSchema.Portal);
        int AddMovementVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema = UserSchema.Portal);
        ConfigurationModel GetMovementConfigDimensions(int vehicleId, string userSchema);
        int AddMovementComponentToFleet(int componentid, int organisationid, string userSchema);
        int MovementCheckFormalNameExists(int componentid, int organisationid, string userSchema);
        List<VehicleDetails> GetSORTMovVehicle(int PartId, string userSchema);
        List<VehicleDetails> GetApplVehicle(int PartID, int revisionId, bool IsVRVeh, string userSchema);
        List<VehicleDetails> GetVehicleList(long routePartId, string userSchema, int isHistoric = 0);
        List<AppVehicleConfigList> GetSortMovementVehicle(long revisionId, int rListType);
        List<long> SaveNonEsdalVehicle(Domain.NonESDAL.NEVehicleImport neVehicleImport);
        Domain.ExternalAPI.VehicleImportOutput ImportVehicleExternal(VehicleImportModel vehicleImportModel, long movementId);
        List<Domain.ExternalAPI.Vehicle> ExportVehicleDetails(Domain.ExternalAPI.GetVehicleExportList vehicleExportList);
        long SaveVehicleComponents(VehicleConfigDetail vehicleDetail);
        long UpdateVehicleComponents(VehicleConfigDetail vehicleDetail);
        bool UpdateTempVehicle(long movementId, int vehicleCategory, long vehicleId);
        bool DeleteTempMovementVehicle(long movementId, string userSchema);
        long AddComponentToFleetLibrary(List<VehicleComponentModel> componentList);
        List<VehicleConfigurationGridList> GetFilteredVehicleCombinations(ConfigurationModel configurationModel);
        long ImportFleetVehicleToRoute(long configId, string userSchema, int applnRev);
        long ChekcVehicleIsValid(long vehicleId, int flag, string userSchema);
        ImportVehicleValidations CheckVehicleValidations(int vehicleId, string userschema);
        List<AutoFillModel> AutoFillVehicles(string vehicleIds, int vehicleCount, string userSchema);
        List<VehicleConfigration> GetNenApiVehiclesList(long notificationId, long organisationId, string userschema);
    }
}
