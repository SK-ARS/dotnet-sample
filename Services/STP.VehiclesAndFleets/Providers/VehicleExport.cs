using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System.Collections.Generic;
using System.Diagnostics;
using STP.Domain.VehiclesAndFleets.External;
using STP.Domain.ExternalAPI;

namespace STP.VehiclesAndFleets.Providers
{
    public sealed class VehicleExport : IVehicleExport
    {
        private VehicleExport()
        {
        }
        public static VehicleExport Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly VehicleExport instance = new VehicleExport();
        }
        public VehicleListDetails GetVehicleList(int organisationId, int pageNumber, int pageSize)
        {
            return VehicleDao.GetVehicleList(organisationId, pageNumber, pageSize);
        }
        public int DeleteVehicle(int vehicleid)
        {
            return VehicleDao.DeleteVehicle(vehicleid);
        }
        public int CheckVehicleExists(int vehicleId, int organisationId)
        {
            return VehicleDao.CheckVehicleExists(vehicleId, organisationId);
        }
        public List<Domain.ExternalAPI.Vehicle> ExportVehicleList(GetVehicleExportList vehicleExportList)
        {
            return VehicleDao.ExportVehicleList(vehicleExportList);
        }
        public VehicleExportExternal GetFleetVehicle(long vehicleId)
        {
            return VehicleDao.GetFleetVehicle(vehicleId);
        }
    }
}