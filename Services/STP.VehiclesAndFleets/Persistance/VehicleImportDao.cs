using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.ExternalAPI;
using STP.Domain.NonESDAL;
using System;
using System.Collections.Generic;
using static STP.Domain.ExternalAPI.VehicleImportModelExternal;

namespace STP.VehiclesAndFleets.Persistance
{
    public static class VehicleImportDao
    {
        #region Import vehicle to temp table
        public static VehicleImportOutput InsertTempVehicle(VehicleImportModel vehicleImportModel)
        {
            VehicleImportOutput vehicleImport = new VehicleImportOutput();

            vehicleImportModel.VehicleConfigDetails.VehicleRegistrationArray.RegistrationsObj = vehicleImportModel.VehicleConfigDetails.VehicleRegistration.Count > 0 ?
                vehicleImportModel.VehicleConfigDetails.VehicleRegistration.ToArray() : null;

            vehicleImportModel.VehicleComponentDetails.ForEach(
                component =>
                {
                    component.AxleArray.AxlesObj = (component.Axles != null && component.Axles.Count > 0) ? component.Axles.ToArray() : null;
                    component.ComponentRegistrationArray.RegistrationsObj = (component.ComponentRegistration.Count > 0) ? component.ComponentRegistration.ToArray() : null;
                });

            VehicleComponentDetailsArray componentObj = new VehicleComponentDetailsArray()
            {
                ComponentObj = vehicleImportModel.VehicleComponentDetails.ToArray()
            };

            OracleCommand cmd = new OracleCommand();

            OracleParameter vehicleConfigParam = cmd.CreateParameter();
            vehicleConfigParam.OracleDbType = OracleDbType.Object;
            vehicleConfigParam.UdtTypeName = "PORTAL.CONFIG_IMPORT";
            vehicleConfigParam.ParameterName = "P_VEHICLE_CONFIG";
            vehicleConfigParam.Value = vehicleImportModel.VehicleConfigDetails;

            OracleParameter vehicleComponentParam = cmd.CreateParameter();
            vehicleComponentParam.OracleDbType = OracleDbType.Object;
            vehicleComponentParam.UdtTypeName = "PORTAL.COMPONENT_IMPORT_ARRAY";
            vehicleComponentParam.ParameterName = "P_COMPONENT_ARRAY";
            vehicleComponentParam.Value = vehicleImportModel.VehicleComponentDetails.Count > 0 ? componentObj : null;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                vehicleImport,
                UserSchema.Portal + ".STP_IMPORT_VEHICLE_EXTERNAL.SP_INSERT_TEMP_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", vehicleImportModel.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_NAME", vehicleImportModel.RouteName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(vehicleConfigParam);
                    parameter.Add(vehicleComponentParam);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.MovementId = records.GetLongOrDefault("MOVEMENT_ID");
                });
            return vehicleImport;
        }

        public static bool UpdateTempVehicle(long movementId, int vehicleCategory, long vehicleId)
        {
            bool isSuccess = false;
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".STP_IMPORT_VEHICLE_EXTERNAL.SP_UPDATE_TEMP_VEHICLE_CAT",
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", movementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CAT", vehicleCategory, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECT_ROW", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt32("P_AFFECT_ROW");
                });
            if (result > 0)
                isSuccess = true;
            return isSuccess;
        }

        public static bool DeleteTempMovementVehicle(long movementId, string userSchema)
        {
            bool isSuccess = false;
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".STP_DUPLICATE_VEHICLES.SP_DELETE_TEMP_VEHICLE_DATA",
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", movementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECT_ROW", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt32("P_AFFECT_ROW");
                });
            if (result > 0)
                isSuccess = true;
            return isSuccess;
        }

        #endregion

        #region InsertNonEsdalVehicle

        public static List<long> InsertNonEsdalVehicle(NEVehicleImport neVehicleImport)
        {
            long result = 0;
            List<long> vehicleIds = new List<long>();
            string neaVehicleProc = "SP_INSERT_APP_VEHICLE";
            if (neVehicleImport.IsVr1 || neVehicleImport.IsNotif)
                neaVehicleProc = "SP_INSERT_ROUTE_VEHICLE";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleIds,
                UserSchema.Portal + ".STP_IMPORT_VEHICLE_EXTERNAL." + neaVehicleProc,
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", neVehicleImport.MovementId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_ID", neVehicleImport.NotificationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RVISION_ID", neVehicleImport.RevisionId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    result = records.GetLongOrDefault("VEHICLE_ID");
                    vehicleIds.Add(result);
                }
            );
            return vehicleIds;
        }

        #endregion

        #region InsertFleetVehicle
        public static long InsertFleetVehicle(VehicleImportOutput vehicleImport, int organisationId)
        {
            long vehicleId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                vehicleId,
                UserSchema.Portal + ".STP_IMPORT_VEHICLE_EXTERNAL.SP_INSERT_FLEET_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", vehicleImport.MovementId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleImport.VehicleId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    vehicleId = records.GetLongOrDefault("V_VEHICLE_ID");
                }
            );
            return vehicleId;
        }
        #endregion
    }
}