using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Applications;
using STP.Domain.VehiclesAndFleets.Vehicles;

namespace STP.VehiclesAndFleets.Providers
{
    public class ApplicationVehicleProvider: IApplicationVehicleProvider
    {

        #region ApplicationVehicleProvider Singleton
        private ApplicationVehicleProvider()
        {
        }
        public static ApplicationVehicleProvider Instance
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
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly ApplicationVehicleProvider instance = new ApplicationVehicleProvider();
        }
        #region Logger instance
        private const string PolicyName = "ApplicationVehicleProvider";
        #endregion
        #endregion

        #region To import vehicle from fleet into a SO application
        public long SaveSOApplicationvehicleconfig(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            return VehicleConfigDAO.SaveSOApplicationvehicleconfig(vehicleId, apprevisionId, routepartid, userSchema);
        }
        #endregion
        #region To import vehicle from fleet into a VR1 application
        public NewConfigurationModel SaveVR1Applicationvehicleconfig(NewConfigurationModel configurationModel)
        {
            return VehicleConfigDAO.SaveVR1Applicationvehicleconfig(configurationModel);
        }
        #endregion
        #region Applicationvehiclelist
        public List<VehicleDetailSummary> Applicationvehiclelist(int partId, int flagSOAppVeh, string routeType, string userSchema)
        {
            return VehicleConfigDAO.Applicationvehiclelist(partId, flagSOAppVeh, routeType, userSchema);
        }
        #endregion
        #region Import vehicle from pre movement in application
        public int PrevMove_ImportApplVeh(int vehicleId, int apprevisionId, int routepartid, string userSchema)
        {
            return VehicleConfigDAO.PrevMove_ImportApplVeh(vehicleId, apprevisionId, routepartid, userSchema);
        }
        #endregion
        #region ImportRouteVehicleToAppVehicle
        public int ImportRouteVehicleToAppVehicle(int vehicleId, int apprevisionId, int routepartId, string userSchema)
        {
            return VehicleConfigDAO.ImportRouteVehicleToAppVehicle(vehicleId, apprevisionId, routepartId, userSchema);
        }
        #endregion
        #region VR1 Application vehicle movemnt list
        public int VR1AppVehicle_MovementList(int vehicleId, int apprevisionId, int routepartId, string userSchema)
        {
            return VehicleConfigDAO.VR1AppVehicle_MovementList(vehicleId, apprevisionId, routepartId, userSchema);
        }
        #endregion
        #region SO Application vehicle movemnt list
        public int AppVehicle_MovementList(int vehicleId, int apprevisionId, int routepartId, string userSchema)
        {
            return VehicleConfigDAO.AppVehicle_MovementList(vehicleId, apprevisionId, routepartId, userSchema);
        }
        #endregion
        #region Delete selected vehicleComponent from so application
        public int DeleteSelectedVehicleComponent(int vehicleId, string userSchema )
        {
            return VehicleConfigDAO.DeleteSelectedVehicleComponent(vehicleId, userSchema);
        }
        #endregion 
        #region Delete selected vehicleComponent from vr1 application
        public int DeleteSelectedVR1VehicleComponent(int vehicleId, string userSchema)
        {
            return VehicleConfigDAO.DeleteSelectedVR1VehicleComponent(vehicleId, userSchema);
        }
        #endregion
        #region ViewVehicleSummaryByID
        public List<VehicleDetailSummary> ViewVehicleSummaryByID(long rPartId, int vr1, string userSchema)
        {
            return VehicleConfigDAO.ViewVehicleSummaryByID(rPartId, vr1, userSchema);
        }
        #endregion
        #region CheckFormalNameInApplicationVeh
        public int CheckFormalNameInApplicationVeh(string vehicleName, int organisationId, string userSchema)
        {
            return VehicleConfigDAO.CheckFormalNameInApplicationVeh(vehicleName, organisationId, userSchema);
        }
        #endregion
        #region Add Vehicle To Fleet
        public int AddVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema)
        {
            return VehicleConfigDAO.AddVehicleToFleet(vehicleId, organisationId, flag, userSchema);
        }
        #endregion
        #region Add VR1 Vehicle To Fleet
        public int AddVR1VhclToFleet(int vehicleId, int organisationId, int flag)
        {
            return VehicleConfigDAO.AddVR1VhclToFleet(vehicleId, organisationId, flag);
        }
        #endregion
        #region Get the list of application vehicle list
        public List<AppVehicleConfigList> AppVehicleConfigList(long apprevisionId, string userSchema)
        {
            return VehicleConfigDAO.AppVehicleConfigList(apprevisionId, userSchema);
        }
        #endregion
        #region Get the list of VR1 application vehicle list
        public List<AppVehicleConfigList> AppVehicleConfigListVR1(long routePartId, long versionId, string contentRefNo, string userSchema)
        {
            return VehicleConfigDAO.AppVehicleConfigListVR1(routePartId, versionId, contentRefNo, userSchema);
        }
        #endregion
        #region Get the list of NEN vehicle list
        public List<AppVehicleConfigList> GetNenVehicleList(long routePartId)
        {
            return VehicleConfigDAO.GetNEN_VehicleList(routePartId);
        }
        #endregion
        #region ListVehCompDetails
        public List<ApplVehiclComponents> ListVehCompDetails(int revisionId, string userschema)
        {
            return VehicleConfigDAO.ListVehCompDetails(revisionId, userschema);
        }
        #endregion
        #region ListLengthVehCompDetails
        public List<ApplVehiclComponents> ListLengthVehCompDetails(int revisionId, int vehicleId, string userschema)
        {
            return VehicleConfigDAO.ListLengthVehCompDetails(revisionId,vehicleId, userschema);
        }
        #endregion
        #region Checking vehicle weight against axle weight validation
        public List<ApplVehiclComponents> ListVehWeightDetails(int revisionId, int vehicleId, string userschema, int isVR1)
        {
            return VehicleConfigDAO.ListVehWeightDetails(revisionId, vehicleId, userschema,isVR1);
        }
        #endregion
        #region ListVR1VehCompDetails
        public List<ApplVehiclComponents> ListVR1VehCompDetails(int versionId, string contentref)
        {
            return VehicleConfigDAO.ListVR1VehCompDetails(versionId, contentref);
        }
        #endregion
        #region ListLengthVR1VehDetails
        public List<ApplVehiclComponents> ListLengthVR1VehDetails(int versionId, string contentref)
        {
            return VehicleConfigDAO.ListLengthVR1VehDetails(versionId, contentref);
        }
        #endregion
        public List<VehicleDetails> GetSORTMovVehicle(int partID, string userSchema)
        {
            return VehicleDao.GetSORTMovVehicle(partID, userSchema);
        }
        public List<VehicleConfigurationGridList> GetSimilarVehicleCombinations(SearchVehicleCombination configDimensions)
        {
            return VehicleConfigDAO.GetSimilarVehicleCombinations(configDimensions);
        }
        public List<VehicleDetail> GetVehicleConfigByPartID(string ESDALRef, int vr1Vehicle)
        {
            return VehicleConfigDAO.GetVehicleConfigByPartID(ESDALRef, vr1Vehicle);
        }
        public List<ComponentGroupingModel> ApplicationcomponentList(int routePartId)
        {
            return VehicleConfigDAO.ApplicationcomponentList(routePartId);
        }
        #region GetApplVehicle
        public List<VehicleDetails> GetApplVehicle(int PartID, int revisionId, bool IsVRVeh, string userSchema)
        {
            return VehicleDao.GetApplicationVehicle(PartID, revisionId, IsVRVeh, userSchema);
        }

        #endregion
        #region Add Vehicle To Fleet form movement temp
        public int AddMovementVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema)
        {
            return VehicleConfigDAO.AddMovementVehicleToFleet(vehicleId, organisationId, flag, userSchema);
        }
        #endregion

        #region check vehicle validation
        public ImportVehicleValidations CheckVehicleValidations(int vehicleId, string userschema)
        {
            return VehicleConfigDAO.CheckVehicleValidations(vehicleId, userschema);
        }
        #endregion
    }
}