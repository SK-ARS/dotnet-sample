using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain.VehiclesAndFleets.External;
using System;
using System.Collections.Generic;
using System.Configuration;
using STP.Common.Constants;
using static STP.Common.Enums.ExternalApiEnums;
using STP.Domain.ExternalAPI;
using System.Linq;

namespace STP.VehiclesAndFleets.Persistance
{
    public static class VehicleDao
    {
        private static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];

        public static VehicleListDetails GetVehicleList(int organisationId, int pageNumber, int pageSize)
        {
            VehicleListDetails vehicleListDetails = new VehicleListDetails();
            List<Domain.VehiclesAndFleets.External.Vehicle> vehicleList = new List<Domain.VehiclesAndFleets.External.Vehicle>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleList,
                UserSchema.Portal + ".GET_VEHICLE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pagesize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    vehicleListDetails.TotalRecords = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.Name = records.GetStringOrDefault("VEHICLE_NAME");
                    instance.MovementClassification = records.GetStringOrDefault("VEHICLE_PURPOSE"); 
                    instance.VehicleType = records.GetStringOrDefault("VEHICLE_TYPE"); 
                });
            vehicleListDetails.Vehicles = vehicleList;
            
            //To get the Page Count
            if (vehicleListDetails.TotalRecords > 0)
            {
                int Pages = vehicleListDetails.TotalRecords / pageSize;
                int RemainingRecords = vehicleListDetails.TotalRecords % pageSize;
                vehicleListDetails.NumberOfPages = RemainingRecords >= 1 ? Pages + 1: Pages;
            }
            vehicleListDetails.PageSize = vehicleList.Count;
            vehicleListDetails.PageNumber = pageNumber;
            return vehicleListDetails;
        }
        public static int CheckVehicleExists(int vehicleId, int organisationId)
        {
            int result = 0;
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                       result,
                      UserSchema.Portal + ".SP_CHECK_VEHICLE_EXISTS",
                       parameter =>
                       {
                           parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                       },
                        record =>
                        {
                            result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                        }
                   );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/GetVehicleDetails, Exception: " + ex​​​​);
                result = -1;
            }
            return result;
        }
        public static int CheckFormalNameExists(string vehicleName, long organisationId)
        {
            int result = 0;
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                   UserSchema.Portal + ".SP_CHECK_CONFIG_NAME_EXISTS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_CONFIG_NAME", vehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     record =>
                     {
                         result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                     }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/InsertVehicleDetails, Exception: " + ex​​​​​);
            }
            return result;
        }
        public static int DeleteVehicle(int vehicleid)
        {
            int count = 0;
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                    UserSchema.Portal + ".SP_DISABLE_VEHICLE",
                    parameter =>
                    {
                        parameter.AddWithValue("p_VEHICLE_ID", vehicleid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        count = record.GetInt32("p_AFFECTED_ROWS");
                    }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/DeleteVehicle, Exception: " + ex​​​​​);
                throw;
            }
            return count;
        }
        public static List<Domain.VehiclesAndFleets.Configuration.VehicleDetails> GetSORTMovVehicle(int partID, string userSchema)
        {
            List<Domain.VehiclesAndFleets.Configuration.VehicleDetails> vehicleList = new List<Domain.VehiclesAndFleets.Configuration.VehicleDetails>();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               vehicleList,
                  userSchema + ".SP_SORT_GETMOV_VEHICLE_DET",
               parameter =>
               {
                   parameter.AddWithValue("P_RPARTID", partID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                       instance.VehicleName = records.GetStringOrDefault("VEHICLE_DESC");
                       instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                       instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                       instance.VehicleCompList = GetComponentList(instance.VehicleId, userSchema, 1);
                   }
              );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, LogInstance + @" - Vehicle/GetSORTMovVehicle, Exception: " + ex​​​​​);
            }
            return vehicleList;

        }

        #region GetApplicationVehicle
        public static List<Domain.VehiclesAndFleets.Configuration.VehicleDetails> GetApplicationVehicle(int PartId, int revisionId, bool IsVRVeh, string userSchema)
        {
            int vr = 0;
            if (IsVRVeh) { vr = 1; }
            List<Domain.VehiclesAndFleets.Configuration.VehicleDetails> objApplicVeh = new List<Domain.VehiclesAndFleets.Configuration.VehicleDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            objApplicVeh,
                userSchema + ".SP_GET_VEHICLE_DET",
            parameter =>
            {
                parameter.AddWithValue("P_PARTID", PartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("REV_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("VRVeh", vr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.VehicleName = records.GetStringOrDefault("vehicle_desc");
                    instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                    instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                    instance.VehicleCompList = GetComponentList(instance.VehicleId, userSchema, vr);

                }
            );
            return objApplicVeh;
        }
        #endregion
        private static List<Domain.VehiclesAndFleets.Configuration.VehicleConfigList> GetComponentList(long vehicleId, string userSchema, int vr = 0)
        {
            List<Domain.VehiclesAndFleets.Configuration.VehicleConfigList> compList = new List<Domain.VehiclesAndFleets.Configuration.VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                compList,
                userSchema + ".SP_GET_COMPONENT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VRVeh", vr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    instance.ComponentTypeId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    instance.ComponentSubTypeId = records.GetInt32OrDefault("COMPONENT_SUBTYPE");
                });
            return compList;
        }

        #region Export Application/Notification Vehicle Details
        public static List<Domain.ExternalAPI.Vehicle> ExportVehicleList(GetVehicleExportList vehicleExportList)
        {
            #region Vehicle configuration Details
            List<Domain.ExternalAPI.Vehicle> vehicleList = new List<Domain.ExternalAPI.Vehicle>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               vehicleList, vehicleExportList.UserSchema + ".STP_EXPORT_DETAILS.SP_GET_ROUTE_VEHICLE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSION_ID", vehicleExportList.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONT_REF_NUM", vehicleExportList.ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", vehicleExportList.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", vehicleExportList.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_TYPE", vehicleExportList.NotificationType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    var vehiclejson = records.GetStringOrDefault("VEHICLE_JSON");
                    vehicleList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Domain.ExternalAPI.Vehicle>>(vehiclejson);
                    vehicleList.ForEach(
                        vehicle =>
                        {
                            if (vehicle.VehicleConfiguration.VehicleType == "VT001" || vehicle.VehicleConfiguration.VehicleType == "VT008")
                            {
                                vehicle.VehicleConfiguration.GrossTrainWeight = vehicle.VehicleComponents.Sum(item => item.GrossWeight);
                            }
                        });
                });
            return vehicleList;
            #endregion
        }
        #endregion

        #region Get Fleet Vehicle
        public static VehicleExportExternal GetFleetVehicle(long vehicleId)
        {
            VehicleExportExternal vehicleExportExternal = new VehicleExportExternal();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               vehicleExportExternal,
               UserSchema.Portal + ".STP_EXPORT_DETAILS.SP_GET_FLEET_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    var vehiclejson = records.GetStringOrDefault("VEHICLE_JSON");
                    vehicleExportExternal = Newtonsoft.Json.JsonConvert.DeserializeObject<VehicleExportExternal>(vehiclejson);
                    if (vehicleExportExternal.VehicleConfiguration.VehicleType == "VT001" || vehicleExportExternal.VehicleConfiguration.VehicleType == "VT008")
                    {
                        vehicleExportExternal.VehicleConfiguration.GrossTrainWeight = vehicleExportExternal.VehicleComponents.Sum(item => item.GrossWeight);
                    }
                });
            return vehicleExportExternal;
        }
        #endregion
    }
}