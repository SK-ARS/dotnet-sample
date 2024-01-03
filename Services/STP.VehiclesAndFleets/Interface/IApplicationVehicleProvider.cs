using STP.Domain;
using STP.Domain.Applications;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IApplicationVehicleProvider
    {
        long SaveSOApplicationvehicleconfig(int vehicleId, int apprevisionId, int routepartid , string userSchema);
        NewConfigurationModel SaveVR1Applicationvehicleconfig(NewConfigurationModel configurationModel);
        List<VehicleDetailSummary> Applicationvehiclelist(int partId, int flagSOAppVeh, string routeType, string userSchema);
        int PrevMove_ImportApplVeh(int vehicleId, int apprevisionId, int routepartid, string userSchema );
        int ImportRouteVehicleToAppVehicle(int vehicleId, int apprevisionId, int routepartId, string userSchema);
        int VR1AppVehicle_MovementList(int vehicleId, int apprevisionId, int routepartId, string userSchema);
        int AppVehicle_MovementList(int vehicleId, int apprevisionId, int routepartId, string userSchema);
        int DeleteSelectedVehicleComponent(int vehicleId, string userSchema);
        int DeleteSelectedVR1VehicleComponent(int vehicleId, string userSchema);
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
        List<VehicleConfigurationGridList> GetSimilarVehicleCombinations(SearchVehicleCombination configDimensions);
        List<VehicleDetail> GetVehicleConfigByPartID(string ESDALRef, int vr1Vehicle);
        List<ComponentGroupingModel> ApplicationcomponentList(int RoutePartId);
        int AddMovementVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema);
        List<VehicleDetails> GetApplVehicle(int PartID, int revisionId, bool IsVRVeh, string userSchema);
        List<VehicleDetails> GetSORTMovVehicle(int partID, string userSchema);
        ImportVehicleValidations CheckVehicleValidations(int vehicleId, string userschema);
    }
}
