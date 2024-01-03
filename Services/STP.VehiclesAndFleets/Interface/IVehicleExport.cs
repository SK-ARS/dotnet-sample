using STP.Domain.ExternalAPI;
using STP.Domain.VehiclesAndFleets.External;
using System.Collections.Generic;

namespace STP.VehiclesAndFleets.Interface
{
    public interface IVehicleExport
    {
        VehicleListDetails GetVehicleList(int organisationId, int pageNumber, int pageSize);
        int DeleteVehicle(int vehicleid);
        int CheckVehicleExists(int vehicleId, int organisationId);
        List<Domain.ExternalAPI.Vehicle> ExportVehicleList(GetVehicleExportList vehicleExportList);
        VehicleExportExternal GetFleetVehicle(long vehicleId);
    }
}
