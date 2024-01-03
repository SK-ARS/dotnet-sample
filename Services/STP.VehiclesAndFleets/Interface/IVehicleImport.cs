using STP.Domain.ExternalAPI;
using STP.Domain.NonESDAL;
using System.Collections.Generic;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IVehicleImport
    {
        int CheckFormalName(string vehicleName, long organisationId);
        VehicleImportOutput InsertTempVehicle(VehicleImportModel vehicleImportModel);
        List<long> InsertNonEsdalVehicle(NEVehicleImport neVehicleImport);
        long InsertFleetVehicle(VehicleImportOutput vehicleImport, int organisationId);
        bool UpdateTempVehicle(long movementId, int vehicleCategory, long vehicleId);
        bool DeleteTempMovementVehicle(long movementId, string userSchema);
    }
}
