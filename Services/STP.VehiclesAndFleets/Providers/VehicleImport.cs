using STP.VehiclesAndFleets.Interface;
using STP.VehiclesAndFleets.Persistance;
using System.Diagnostics;
using System.Collections.Generic;
using STP.Domain.NonESDAL;
using STP.Domain.ExternalAPI;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;
using System;

namespace STP.VehiclesAndFleets.Providers
{
    public sealed class VehicleImport : IVehicleImport
    {
        #region Vehicle Import SingleTon
        private VehicleImport()
        {
        }
        public static VehicleImport Instance
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
            internal static readonly VehicleImport instance = new VehicleImport();
        }
        #endregion

        public int CheckFormalName(string vehicleName, long organisationId)
        {
            return VehicleDao.CheckFormalNameExists(vehicleName, organisationId);
        }
        public VehicleImportOutput InsertTempVehicle(VehicleImportModel vehicleImportModel)
        {
            return VehicleImportDao.InsertTempVehicle(vehicleImportModel);
        }

        public List<long> InsertNonEsdalVehicle(NEVehicleImport neVehicleImport)
        {
            return VehicleImportDao.InsertNonEsdalVehicle(neVehicleImport);
        }
        public long InsertFleetVehicle(VehicleImportOutput vehicleImport, int organisationId)
        {
            return VehicleImportDao.InsertFleetVehicle(vehicleImport, organisationId);
        }

        public bool UpdateTempVehicle(long movementId, int vehicleCategory, long vehicleId)
        {
            return VehicleImportDao.UpdateTempVehicle(movementId, vehicleCategory, vehicleId);
        }
        public bool DeleteTempMovementVehicle(long movementId, string userSchema)
        {
            return VehicleImportDao.DeleteTempMovementVehicle(movementId, userSchema);
        }
    }
}
