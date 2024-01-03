using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IVehiConfigGetVehicleConfig
    {
        List<VehicleConfigList> GetVR1VehicleConfigVhclID(int vehicleId, string userSchema);
        List<VehicleConfigList> GetAppVehicleConfigVhclID(int vehicleId, string userSchema);
        List<VehicleConfigList> GetVehicleConfigVhclID(int vehicleId, string userSchema);
        ConfigurationModel GetNotifVehicleConfigByID(int vehicleId);
        ConfigurationModel GetConfigInfo(int componentId, string userSchema);
        ConfigurationModel GetConfigInfoApplication(int vehicleId, string userSchema);
        ConfigurationModel GetConfigInfoVR1(int componentId, string schemaType);
        List<ComponentModel> GetComponentsInConfiguration(string componentIds, string userSchema, int flag = 0);
        ComponentModel GetComponentsConfigurationDetails(string componentIds, bool isMovement, string userSchema);
        List<MovementVehicleConfig> InsertMovementVehicle(InsertMovementVehicle movementVehicle);
        List<MovementVehicleConfig> SelectMovementVehicle(long movementId, string userSchema);
        ConfigurationModel GetMovementConfigInfo(long vehicleId, string userSchema);
        List<VehicleConfigList> GetMovementVehicleConfig(long vehicleId, string userSchema);
        bool AssignMovementVehicle(VehicleAssignementParams vehicleAssignement);
        bool DeleteMovementVehicle(long movementId, long vehicleId, string userSchema);
        List<VehicleList> GetFavouriteVehicles(int organisationId, int movementId, string userSchema);
        ConfigurationModel GetConfigDimensions(string GUID, int configTypeId, string userSchema);
        ConfigurationModel GetVehicleDimensions(long VehicleId, int configTypeId, string userSchema);
        bool InsertVehicleConfigPosnTemp(string GUID, int vehicleId, string userSchema);
        ConfigurationModel GetVehicleDetails(int vehicleId, bool movement, string userSchema);
        bool InsertMovementConfigPosnTemp(string GUID, int vehicleId, string userSchema);
        ConfigurationModel GetMovementConfigDimensions(int vehicleId, string userSchema);
        ConfigurationModel GetMovementVehicleDetails(int vehicleId, int isRoute, string userSchema);
        List<AppVehicleConfigList> GetSortMovementVehicle(long revisionId, int rListType);
        List<VehicleConfigurationGridList> GetFilteredVehicleCombinations(ConfigurationModel configurationModel);
        long ImportFleetVehicleToRoute(long configId, string userSchema, int applnRev);
        long ChekcVehicleIsValid(long vehicleId, int flag, string userSchema);
        List<AutoFillModel> AutoFillVehicles(string vehicleIds, int vehicleCount, string userSchema);
    }
}
