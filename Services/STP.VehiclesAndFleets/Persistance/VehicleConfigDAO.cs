using Oracle.DataAccess.Client;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Configuration;
using STP.Common.Constants;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Applications;
using STP.Domain.Routes;
using System.Text;
using STP.Domain.MovementsAndNotifications.Movements;

namespace STP.VehiclesAndFleets.Persistance
{
    public static class VehicleConfigDAO
    {
        public static double InsertConfiguration(NewConfigurationModel ConfigurationModel)
        {
            double result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_INSERT_VEHICLECONFIGURATION",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_organisation_id", ConfigurationModel.OrganisationId, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_UNIT", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_MTR", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("VEHICLE_ID");
                }
            );
            return result;
        }
        public static int CreateVehicleRegistration(int vehicleId, string registrationValue, string fleetId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_INSERT_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENCE_PLATE", registrationValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLEET_NO", fleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt16OrDefault("ID_NO");
                }
            );
            return result;
        }
        public static bool UpdateVehicle(NewConfigurationModel ConfigurationModel)
        {
            bool result = false;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                     count,
                   UserSchema.Portal + ".SP_EDIT_VEHICLE",
                   parameter =>
                   {
                       parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_organisation_id", ConfigurationModel.OrganisationId, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_HEIGHT_UNIT", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_HEIGHT_MTR", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_VEHICLE_ID", ConfigurationModel.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   record =>
                   {
                       count = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                   }
               );
            if (count > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        

        public static List<VehicleRegistration> GetRegistration(int vehicleId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".GET_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("LICENSE_PLATE");
                    result.FleetId = records.GetStringOrDefault("FLEET_NO");
                    result.IdNumber = records.GetInt16OrDefault("ID_NO");
                }
            );
            return listVehclRegObj;
        }
        public static List<VehicleRegistration> GetVR1ApplRegistration(int vehicleId, string userSchema)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   listVehclRegObj,
                   userSchema + ".SP_ROUTE_GET_VEHICLE_ID",
                   parameter =>
                   {
                       parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   (records, result) =>
                   {
                       result.RegistrationId = records.GetStringOrDefault("LICENSE_PLATE");
                       result.FleetId = records.GetStringOrDefault("FLEET_NO");
                       result.IdNumber = records.GetInt16OrDefault("ID_No");
                   }
               );
            return listVehclRegObj;
        }
        public static List<VehicleRegistration> GetApplRegistration(int vehicleId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".GET_APPL_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("LICENSE_PLATE");
                    result.FleetId = records.GetStringOrDefault("FLEET_NO");
                    result.IdNumber = records.GetInt16OrDefault("ID_NO");
                }
            );
            return listVehclRegObj;
        }
        public static List<VehicleConfigList> GetVR1VehicleConfigPosn(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".SP_ROUTE_GET_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    result.ComponentTypeId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    result.SubType = records.GetInt32OrDefault("SUB_TYPE");
                    result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                    result.LongPosn = records.GetInt16OrDefault("LAT_POSN");
                    result.ComponentSubTypeId = Convert.ToInt64(records["SUB_TYPE"]);
                }
            );
            return listVehclRegObj;
        }
        public static List<VehicleConfigList> GetAppVehicleConfigPosn(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".SP_APPL_GET_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    result.ComponentTypeId = (int)records.GetDecimalOrDefault("COMPONENT_TYPE");
                    result.LongPosn = records.GetInt16OrDefault("LAT_POSN");
                    result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                }
            );
            return listVehclRegObj;
        }
        public static List<VehicleConfigList> GetVehicleConfigPosn(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".GET_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                    result.LongPosn = records.GetInt16OrDefault("LAT_POSN");
                    result.ComponentTypeId = Convert.ToInt64(records["COMPONENT_TYPE"]);
                    result.ComponentSubTypeId = Convert.ToInt64(records["SUB_TYPE"]);
                }
            );
            return listVehclRegObj;
        }
        //function to select all component regardless of show flag
        public static List<ComponentGridList> GetAllComponentDetailsByID(int organisationId, long componentId)
        {
            List<ComponentGridList> componentGridObj = new List<ComponentGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                UserSchema.Portal + ".SELECT_ALL_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                        instance.ComponentName = records.GetStringOrDefault("COMPONENT_NAME");
                        instance.FleetList = records.GetStringOrDefault("FLEET_ID");
                        instance.ComponentDescription = records.GetStringOrDefault("COMPONENT_SUMMARY");
                        instance.VehicleIntent = records.GetStringOrDefault("VEHICLE_INTENT");
                        instance.ComponentType = records.GetStringOrDefault("COMPONENT_SUBTYPE");
                        instance.AxleSpacing = records.GetDecimalOrDefault("SPACE_TO_FOLLOWING");
                    }
            );
            return componentGridObj;
        }
        public static VehicleConfigList CreateVehicleConfigPosn(VehicleConfigList ConfigList)
        {
            VehicleConfigList InsertedConPos = new VehicleConfigList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               InsertedConPos,
                UserSchema.Portal + ".SP_INSERT_VEHICLE_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", ConfigList.VehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", ConfigList.ComponentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LAT_POS", ConfigList.LatPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONG_POS", ConfigList.LongPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOWING", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOWING_UNIT", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("P_COMPONENT_TYPE", ConfigList.SubType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    InsertedConPos.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    InsertedConPos.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    InsertedConPos.LatPosn = records.GetInt16OrDefault("LAT_POSN");
                    InsertedConPos.LongPosn = records.GetInt16OrDefault("LONG_POSN");
                }
            );
            return InsertedConPos;
        }
        public static bool UpdateVehicleDetailsOnFinish(int configId, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false)
        {
            bool result = false;
            int output = 0;
            int isNotifFlag = 0;
            int isVR1Flag = 0;
            if (isNotif)
            {
                isNotifFlag = 1;
            }
            if (isVR1)
            {
                isVR1Flag = 1;
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".SP_UPDATE_VEHICLE_WITH_COMP",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHILCE_ID", configId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_APPLN_REV_ID", applnRev, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ISNOTIF", isNotifFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ISVR1", isVR1Flag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    output = (int)record.GetDecimalOrDefault("VAR_OUTPUT");
                }
            );
            if (output == 1)
            {
                result = true;
            }
            return result;
        }
        public static List<ComponentIdModel> GetVR1ComponentIDList(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<ComponentIdModel> listCompId = new List<ComponentIdModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listCompId,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_LIST_COMPONENT_TYPE",//to be changed
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.ComponentId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    result.LatPos = records.GetInt16OrDefault("long_posn");
                    result.LongPos = records.GetInt16OrDefault("lat_posn");
                }
            );
            return listCompId;
        }
        public static List<ComponentIdModel> GetComponentIDList(int vhclID)
        {
            List<ComponentIdModel> listCompId = new List<ComponentIdModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listCompId,
                UserSchema.Portal + ".LIST_COMPONENT_TYPE",//to be changed
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.ComponentId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    result.LongPos = records.GetInt16OrDefault("lat_posn");
                    result.LatPos = records.GetInt16OrDefault("long_posn");
                }
                );
            return listCompId;
        }
        //for getting component list from application veh comp
        public static List<ComponentIdModel> GetAppComponentIDList(int vhclID, string userSchema = UserSchema.Portal)
        {
            List<ComponentIdModel> listCompId = new List<ComponentIdModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listCompId,
                userSchema + ".SP_APPL_LIST_COMPONENT_TYPE",//to be changed
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.ComponentId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    result.LatPos = records.GetInt16OrDefault("long_posn");
                    result.LongPos = records.GetInt16OrDefault("lat_posn");
                }
            );
            return listCompId;
        }
        public static int CheckFormalNameExists(int componentId, int organisationId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".SP_VERIFY_FORMAL_NAME",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                 }
            );
            return result;
        }
        public static int CheckVR1FormalNameExists(int componentId, int organisationId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_CHECK_NAME",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                 }
            );
            return result;
        }
        public static int CheckAppFormalNameExists(int componentId, int organisationId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".SP_APPL_CHECK_NAME",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                 }
            );
            return result;
        }
        public static int SaveVR1VehicleRegistrationId(int vehicleId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENCE_PLATE", registrationValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLEET_NO", fleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt16OrDefault("ID_NO");
                }
            );
            return result;
        }
        //for deleting the VR1 vehicle registration details
        public static int DeleteVR1RegConfig(int vehicleId, int IdNumber, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_DELETE_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ID_NO", IdNumber, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        public static int DeleteVehicleconfig(int vehicleId, int latpos, int longpos)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_DELETE_VEHICLECONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LATPOS", longpos, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONGPOS", latpos, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        //Delete component from Application Component table
        public static int DeleteApplVehicleconfig(int vehicleId, int latpos, int longpos, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".SP_APPL_DELETE_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LATPOS", longpos, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONGPOS", latpos, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        //Delete component from Route vehicle Component table
        public static int DeleteVR1Vehicleconfig(int vehicleId, int latpos, int longpos, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".SP_ROUTE_DELETE_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LATPOS", longpos, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONGPOS", latpos, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        public static int DeleteVehicleRegistrationConfiguration(int vehicleId, int idno, bool isMovement = false)
        {
            int rowsAffected = 0;
            string sp = UserSchema.Portal + ".SP_DELETE_VEHICLE_ID";
            if (isMovement)
            {
                sp = UserSchema.Portal + ".STP_VEHICLE.SP_DELETE_VEHICLE_ID_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                sp,
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ID_NO", idno, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                 }
            );
            return rowsAffected;
        }
        public static int DeleteAppRegConfig(int vehicleId, int IdNumber, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".SP_APPL_DELETE_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ID_NO", IdNumber, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                 }
            );
            return rowsAffected;
        }
        public static int Disablevehicle(int vehicleId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_DISABLE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        public static bool UpdateApplicationVehicle(NewConfigurationModel ConfigurationModel, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  count,
                 userSchema + ".SP_APPL_EDIT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", ConfigurationModel.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_UNIT", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_MTR", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = Convert.ToInt32(record.GetDecimalOrDefault("STATUS_1"));
                }
            );
            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public static bool UpdateVR1ApplicationVehicle(NewConfigurationModel ConfigurationModel, string userschema = UserSchema.Portal)
        {
            bool result = false;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  count,
                userschema + ".STP_ROUTE_VEHICLES.SP_ROUTE_EDIT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", ConfigurationModel.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_type", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_max_height_unit", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_max_height_mtr", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTE_PART_ID", ConfigurationModel.RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = Convert.ToInt32(record.GetDecimalOrDefault("STATUS_1"));
                }
            );
            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        //CreateConfigPosn
        public static VehicleConfigList CreateApplVehConfigPosn(VehicleConfigList configList, string userSchema = UserSchema.Portal)
        {
            VehicleConfigList InsertedConPos = new VehicleConfigList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               InsertedConPos,
                userSchema + ".SP_APPL_INSERT_VHCL_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", configList.VehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", configList.ComponentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LAT_POS", configList.LatPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONG_POS", configList.LongPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMPONENT_TYPE", configList.SubType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    InsertedConPos.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    InsertedConPos.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    InsertedConPos.LatPosn = records.GetInt16OrDefault("LAT_POSN");
                    InsertedConPos.LongPosn = records.GetInt16OrDefault("LONG_POSN");
                }
            );
            return InsertedConPos;
        }
        //CreateAppVehicleConfigPosn for Aplication Component table
        public static VehicleConfigList CreateAppVehicleConfigPosn(VehicleConfigList configList, int isImportFromFleet, string userSchema = UserSchema.Portal)
        {
            VehicleConfigList InsertedConPos = new VehicleConfigList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               InsertedConPos,
                userSchema + ".SP_APPL_INSERT_VHCL_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", configList.VehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767); //Vehicle id from appl_vehicle
                    parameter.AddWithValue("p_COMPONENT_ID", configList.ComponentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767); //comp_id from vehicle_components (imported)
                    parameter.AddWithValue("p_LAT_POSN", configList.LatPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LONG_POSN", configList.LongPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_TYPE", configList.SubType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FROM_FLEET_FLAG", isImportFromFleet, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    InsertedConPos.VehicleId = records.GetLongOrDefault("VEHICLE_ID"); //The vehicle id of the vehicle of appl_vehicle
                    InsertedConPos.ComponentId = records.GetLongOrDefault("COMPONENT_ID"); //Comp id from Appl_components
                    InsertedConPos.LatPosn = records.GetInt16OrDefault("LAT_POSN");
                    InsertedConPos.LongPosn = records.GetInt16OrDefault("LONG_POSN");
                }
            );
            return InsertedConPos;
        }
        //  Position of components in a VR1 vehicle configuration 
        public static VehicleConfigList CreateVR1VehicleConfigPosn(VehicleConfigList configList, string userSchema = UserSchema.Portal)
        {
            VehicleConfigList InsertedConPos = new VehicleConfigList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               InsertedConPos,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_VHCL_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", configList.VehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767); //Vehicle id from appl_vehicle
                    parameter.AddWithValue("p_COMPONENT_ID", configList.ComponentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767); //comp_id from vehicle_components (imported)
                    parameter.AddWithValue("p_LAT_POSN", configList.LatPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LONG_POSN", configList.LongPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_TYPE", configList.SubType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    InsertedConPos.VehicleId = records.GetLongOrDefault("VEHICLE_ID"); //The vehicle id of the vehicle of appl_vehicle
                    InsertedConPos.ComponentId = records.GetLongOrDefault("COMPONENT_ID"); //Comp id from Appl_components
                    InsertedConPos.LatPosn = records.GetInt16OrDefault("LAT_POSN");
                    InsertedConPos.LongPosn = records.GetInt16OrDefault("LONG_POSN");
                    InsertedConPos.SubType = records.GetInt32OrDefault("COMPONENT_TYPE");
                }
            );
            return InsertedConPos;
        }
        public static long InsertApplicationVehicleConfiguration(NewConfigurationModel ConfigurationModel, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_APPL_INSERT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REVISION_ID", ConfigurationModel.ApplicationRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    if (ConfigurationModel.PartId == null)
                        parameter.AddWithValue("p_PART_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    else
                        parameter.AddWithValue("p_PART_ID", ConfigurationModel.PartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_UNIT", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_MTR", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("VEHICLE_ID");
                }
            );
            return result;
        }
        public static long CopyVehicle(long configId, string userSchema, int applnRev = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", int IsCandidate = 0)
        {
            int isNotifFlag = 0;
            int isVR1Flag = 0;
            if (IsCandidate == 0)
            {
                if (isNotif)
                {
                    isNotifFlag = 1;
                }
                if (isVR1)
                {
                    isVR1Flag = 1;
                }
            }
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_COPY_VEHICLE_FROM_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", configId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_APPLN_REV_ID", applnRev, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF_NO", ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ISNOTIF", isNotifFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ISVR1", isVR1Flag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    if (userSchema.ToUpper() == UserSchema.Sort.ToUpper())
                        parameter.AddWithValue("P_ISCANDIDATE", IsCandidate, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("VEHICLE_ID");
                }
            );
            return result;
        }
        public static NewConfigurationModel InsertVR1ApplicationVehicleConfiguration(NewConfigurationModel ConfigurationModel, string userSchema = UserSchema.Portal)
        {
            NewConfigurationModel config = new NewConfigurationModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                config,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_width_unit", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Width_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_max_height_unit", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_max_height_mtr", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_ID", ConfigurationModel.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTENT_REF_NO", ConfigurationModel.ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    //p_ISDELETED
                    //p_IsNotif
                    parameter.AddWithValue("p_ISDELETED", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IsNotif", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    if (userSchema == UserSchema.Sort)
                        parameter.AddWithValue("p_CAND_REVISION_ID", ConfigurationModel.CandRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    config.VehicleId = Convert.ToInt32(records.GetLongOrDefault("VEHICLE_ID"));
                    config.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                }
            );
            return config;
        }
        public static long ImportVehicle(long configId, string userSchema, int applnRev = 0, bool isNotif = false, bool isVR1 = false, string ContentRefNo = "0", int IsCandidate = 0, string VersionType = "A")
        {
            int isNotifFlag = 0;
            int isVR1Flag = 0;
            if (IsCandidate == 0)
            {
                if (isNotif)
                {
                    isNotifFlag = 1;
                }
                if (isVR1)
                {
                    isVR1Flag = 1;
                }
            }
            long result = 0;
            string Procname = ".SP_IMPORT_CANDIDATE_VEHICLE";
            if (IsCandidate == 1)
            {
                if (VersionType == "A")
                {
                    Procname = ".STP_CANDIDATE_ROUTE_PKG.IMPORT_APPL_VEH_TO_ROUTE_VEH";
                }
                else if (VersionType == "C" || VersionType == "M")
                {
                    Procname = ".SP_IMPORT_CANDIDATE_VEHICLE";
                }
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + Procname, //".SP_COPY_VEHICLE_FROM_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", configId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_APPLN_REV_ID", applnRev, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF_NO", ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ISNOTIF", isNotifFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ISVR1", isVR1Flag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    if (userSchema.ToUpper() == UserSchema.Sort.ToUpper())
                        parameter.AddWithValue("P_ISCANDIDATE", IsCandidate, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("VEHICLE_ID");
                }
            );
            return result;
        }
        public static ConfigurationModel GetNotifVehicleConfigByID(int RoutePartId)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_GET_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_RoutePart_ID", RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VHCL_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        VehicleConfig.FormalName = records.GetStringOrDefault("VEHICLE_NAME");
                        VehicleConfig.Description = records.GetStringOrDefault("VEHICLE_INT_DESC");
                        VehicleConfig.InternalName = records.GetStringOrDefault("VEHICLE_DESC");
                        VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MovementClassificationId = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN");
                        VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                        VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                        VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                        VehicleConfig.MaxHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                        VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                        VehicleConfig.ReducedHeight = records.GetDoubleOrDefault("RED_HEIGHT_MTR");
                        VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                        VehicleConfig.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        VehicleConfig.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                        VehicleConfig.TravellingSpeed = Convert.ToDouble(records.GetSingleOrDefault("SPEED"));
                        VehicleConfig.TravellingSpeedUnit = records.GetInt32OrDefault("SPEED_UNIT");
                        VehicleConfig.TyreSpacing = records.GetDoubleOrDefault("SIDE_BY_SIDE");
                        VehicleConfig.TyreSpacingUnit = records.GetInt32OrDefault("SIDE_BY_SIDE_UNIT");
                        VehicleConfig.MaxAxleWeight = Convert.ToDouble(records["MAX_AXLE_WEIGHT"]);
                        VehicleConfig.AxleCount = Convert.ToInt32(records["AXLE_COUNT"]);
                        VehicleConfig.NotifLeftOverhang = Convert.ToDouble(records["left_overhang"]);
                        VehicleConfig.NotifRightOverhang = Convert.ToDouble(records["right_overhang"]);
                        VehicleConfig.NotifFrontOverhang = Convert.ToDouble(records["front_overhang"]);
                        VehicleConfig.NotifRearOverhang = Convert.ToDouble(records["rear_overhang"]);
                        VehicleConfig.AxleCount = Convert.ToInt32(records["AXLE_COUNT"]);
                        //VehicleConfig.TrailerAxleCount = Convert.ToInt32(records["trailer_axle_count"]);
                        VehicleConfig.TrainWeight = Convert.ToDouble(records["TRAIN_WEIGHT"]);
                        VehicleConfig.ReducedHeight = Convert.ToDouble(records["REDUCIBLE_HEIGHT"]);
                    }
            );
            return VehicleConfig;
        }
        public static ConfigurationModel GetConfigDetails(int componentId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".GET_VEHICLE_CONFIGURATION",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Is_Simple", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MovementClassificationId = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.FormalName = records.GetStringOrDefault("VEHICLE_DESC");
                        VehicleConfig.Description = records.GetStringOrDefault("VEHICLE_INT_DESC");
                        VehicleConfig.InternalName = records.GetStringOrDefault("VEHICLE_NAME");
                        VehicleConfig.ComponentType = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MaxHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                        VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN"); //swapped with LEN below
                        VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                        VehicleConfig.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        VehicleConfig.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                        VehicleConfig.TyreSpacing = records.GetDoubleOrDefault("SIDE_BY_SIDE");
                        VehicleConfig.TyreSpacingUnit = records.GetInt32OrDefault("SIDE_BY_SIDE_UNIT");
                        VehicleConfig.TravellingSpeed = Convert.ToDouble(records.GetSingleOrDefault("SPEED"));
                        VehicleConfig.TravellingSpeedUnit = records.GetInt32OrDefault("SPEED_UNIT");
                        VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                        VehicleConfig.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                        VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_Weight_UNIT");
                        VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                    }
            );
            return VehicleConfig;
        }
        public static ConfigurationModel GetConfigDetailsApplication(int vehicleId, string userSchema)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".SP_GET_APPL_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MovementClassificationId = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.InternalName = records.GetStringOrDefault("VEHICLE_NAME");
                        VehicleConfig.Description = records.GetStringOrDefault("VEHICLE_INT_DESC");
                        VehicleConfig.FormalName = records.GetStringOrDefault("VEHICLE_DESC");
                        VehicleConfig.ComponentType = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MaxHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                        VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN"); //swapped with LEN below
                        VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                        VehicleConfig.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        VehicleConfig.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                        VehicleConfig.TyreSpacing = records.GetDoubleOrDefault("SIDE_BY_SIDE");
                        VehicleConfig.TyreSpacingUnit = records.GetInt32OrDefault("SIDE_BY_SIDE_UNIT");
                        VehicleConfig.TravellingSpeed = Convert.ToDouble(records.GetSingleOrDefault("SPEED"));
                        VehicleConfig.TravellingSpeedUnit = records.GetInt32OrDefault("SPEED_UNIT");
                        VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                        VehicleConfig.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                        VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_Weight_UNIT");
                        VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                    }
            );
            return VehicleConfig;
        }
        public static ConfigurationModel GetConfigDetailsVR1(int componentId, string schemaType = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                schemaType + ".STP_ROUTE_VEHICLES.SP_ROUTE_GET_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_RoutePart_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VHCL_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        VehicleConfig.InternalName = records.GetStringOrDefault("VEHICLE_NAME");
                        VehicleConfig.Description = records.GetStringOrDefault("VEHICLE_INT_DESC");
                        VehicleConfig.FormalName = records.GetStringOrDefault("VEHICLE_DESC");
                        VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MovementClassificationId = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN");
                        VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                        VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                        VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                        VehicleConfig.MaxHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                        VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                        VehicleConfig.ReducedHeight = records.GetDoubleOrDefault("RED_HEIGHT_MTR");
                        VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                        VehicleConfig.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        VehicleConfig.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                        VehicleConfig.TravellingSpeed = Convert.ToDouble(records.GetSingleOrDefault("SPEED"));
                        VehicleConfig.TravellingSpeedUnit = records.GetInt32OrDefault("SPEED_UNIT");
                        VehicleConfig.TyreSpacing = records.GetDoubleOrDefault("SIDE_BY_SIDE");
                        VehicleConfig.TyreSpacingUnit = records.GetInt32OrDefault("SIDE_BY_SIDE_UNIT");
                        VehicleConfig.MaxAxleWeight = Convert.ToDouble(records["MAX_AXLE_WEIGHT"]);
                        VehicleConfig.AxleCount = Convert.ToInt32(records["AXLE_COUNT"]);
                        VehicleConfig.NotifLeftOverhang = Convert.ToDouble(records["left_overhang"]);
                        VehicleConfig.NotifRightOverhang = Convert.ToDouble(records["right_overhang"]);
                        VehicleConfig.NotifFrontOverhang = Convert.ToDouble(records["front_overhang"]);
                        VehicleConfig.NotifRearOverhang = Convert.ToDouble(records["rear_overhang"]);
                        VehicleConfig.AxleCount = Convert.ToInt32(records["AXLE_COUNT"]);
                        //VehicleConfig.TrailerAxleCount = Convert.ToInt32(records["trailer_axle_count"]);
                        VehicleConfig.TrainWeight = Convert.ToDouble(records["TRAIN_WEIGHT"]);
                        VehicleConfig.ReducedHeight = Convert.ToDouble(records["REDUCIBLE_HEIGHT"]);
                    }
            );
            return VehicleConfig;
        }
        public static int SaveAppVehicleRegistrationId(int vehicleId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_APPL_INSERT_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENCE_PLATE", registrationValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLEET_NO", fleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt16OrDefault("ID_NO");
                }
            );
            return result;
        }
        public static List<VehicleConfigurationGridList> GetComponentDetailsByID(int organisationId, int movclass, int movtype, string userSchema = UserSchema.Portal, int filterFavouritesVehConfig = 0,int presetFilter=1,int? sortOrder=null)
        {
            //Creating new object for VehicleConfigurationGridList
            List<VehicleConfigurationGridList> VehicleConfigGridObj = new List<VehicleConfigurationGridList>();
            //Setup Procedure LIST_VEHICLE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                VehicleConfigGridObj,
                userSchema + ".LIST_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVMNT_CLASSIFICATION", movclass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVMNT_TYPE", movtype, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FILTER_FAVOURITES_FLAG", filterFavouritesVehConfig, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    
                },
                    (records, instance) =>
                    {
                        instance.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        instance.FormalName = records.GetStringOrDefault("VEHICLE_NAME");
                        instance.ConfigurationName = records.GetStringOrDefault("INTERNAL_NAME");
                        instance.IndendedUse = records.GetStringOrDefault("NAME");
                        instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                        instance.IsFavourites = Convert.ToInt32(records["Is_Favourite"]);
                        instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        instance.width = Convert.ToDouble(Math.Round (records.GetDecimalOrDefault("WIDTH"),2));
                        instance.height = Convert.ToDouble(Math.Round (records.GetDecimalOrDefault("MAX_HEIGHT"),2));
                        instance.grossWeight = Convert.ToDouble(Math.Round(records.GetDecimalOrDefault("GROSS_WEIGHT"),2));
                    }
            );
            return VehicleConfigGridObj;
        }
        public static int AddToFleet(int componentid, int organisationid)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".SP_ADD_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetLongOrDefault("COMPONENT_ID"));
                 }
            );
            return result;
        }
        // For Application Component Add to fleet
        #region  For Application Component Add to fleet
        public static int AddApplCompToFleet(int componentid, int organisationid, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_APPL_COMP_ADD_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGID", organisationid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("TODO", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetLongOrDefault("COMPONENT_ID"));
                 }
            );
            return result;
        }
        #endregion
        // For VR1 Application Component Add to fleet
        public static int AddVR1ApplCompToFleet(int componentid, int organisationid, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_COMP_ADD_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORGID", organisationid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("TODO", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetLongOrDefault("COMPONENT_ID"));
                 }
            );
            return result;
        }
        #region Vehicle Managemnet in Application
        #region To import vehicle from fleet into a SO application
        public static long SaveSOApplicationvehicleconfig(int vehicleId, int apprevisionId, int routepartid = 0, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_CLONE_APPLICATION_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REV_ID ", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PART_ID", routepartid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Result_Set", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        result = record.GetLongOrDefault("VEHICLE_ID");
                    }
            );
            return result;
        }
        #endregion
        #region To import vehicle from fleet into a VR1 application
        public static NewConfigurationModel SaveVR1Applicationvehicleconfig(NewConfigurationModel configurationModel)
        {
            NewConfigurationModel config = new NewConfigurationModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                config,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_IMPORT_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("P_SIMPLE", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VHCL_ID", configurationModel.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_PART_ID", configurationModel.RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", configurationModel.ApplicationRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENTREF_NO‏", configurationModel.ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        config.VehicleId = Convert.ToInt32(record.GetLongOrDefault("VEHICLE_ID"));
                        config.RoutePartId = record.GetLongOrDefault("ROUTE_PART_ID");
                    }
            );
            return config;
        }
        #endregion
        #region Applicationvehiclelist
        public static List<VehicleDetailSummary> Applicationvehiclelist(int partId, int flagSOAppVeh, string routeType = "", string userSchema = UserSchema.Portal)
        {
            List<VehicleDetailSummary> objComponentModelList = new List<VehicleDetailSummary>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            objComponentModelList,
                userSchema + ".SP_APPL_LIST_VEHICLE_PART_ID ",
            parameter =>
            {
                parameter.AddWithValue("P_FlagSOAppVeh", flagSOAppVeh, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_PART_ID", partId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ROUTE_TYPE", routeType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    if (flagSOAppVeh == 2)
                    {
                        if (routeType == "outline" || routeType == "planned")
                        {
                            instance.VehicleType = "ApplVehicle";
                        }
                        else
                        {
                            instance.VehicleType = "RouteVehicle";
                        }
                        if (userSchema == UserSchema.Portal)
                        {
                            instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                        }
                        else
                        {
                            instance.RoutePartId = records.GetLongOrDefault("PART_ID");
                        }
                    }
                    else
                    {
                        instance.RoutePartId = records.GetLongOrDefault("PART_ID");
                    }
                    instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
                    instance.FormalName = records.GetStringOrDefault("vehicle_desc");
                }
            );
            return objComponentModelList;
        }
        #endregion
        #region Import vehicle from pre movement in application
        public static int PrevMove_ImportApplVeh(int vehicleId = 0, int apprevisionId = 0, int routepartId = 0, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                    UserSchema.Sort + ".STP_APPLICATIONS.SP_IMPRT_APPL_TO_APPL_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("C_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("C_REVISION_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("C_ROUTE_PART_ID", routepartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("C_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = Convert.ToInt32(records.GetLongOrDefault("VEHICLE_ID"));
                }
            );
            return result;
        }
        #endregion
        #region ImportRouteVehicleToAppVehicle
        public static int ImportRouteVehicleToAppVehicle(int vehicleId = 0, int apprevisionId = 0, int routepartId = 0, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                    userSchema + ".STP_APPLICATIONS.SP_IMPRT_ROUTE_TO_APPL_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("B_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("B_REVISION_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("B_ROUTE_PART_ID", routepartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("B_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (int)records.GetDecimalOrDefault("B_NEW_VHCL_ID");
                }
            );
            return result;
        }
        #endregion
        #region VR1 Application vehicle movemnt list
        public static int VR1AppVehicle_MovementList(int vehicleId = 0, int apprevisionId = 0, int routepartid = 0, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                    userSchema + ".SP_CLONE_APPL_VR1_VEHICLE_DET",
                parameter =>
                {
                    parameter.AddWithValue("P_VHCL_ID‏", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_REV_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULT_SET‏", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = record.GetLongOrDefault("VEHICLE_ID") == null ? 0 : Convert.ToInt32(record.GetLongOrDefault("VEHICLE_ID"));
                }
            );
            return result;
        }
        #endregion
        #region SO Application vehicle movemnt list
        public static int AppVehicle_MovementList(int vehicleId = 0, int apprevisionId = 0, int routepartId = 0, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            if (userSchema == UserSchema.Portal)
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                        userSchema + ".STP_APPLICATIONS.SP_IMPRT_ROUTE_TO_APPL_VEHICLE",
                    parameter =>
                    {
                        parameter.AddWithValue("B_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("B_REVISION_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("B_ROUTE_PART_ID", routepartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("B_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        result = (int)records.GetDecimalOrDefault("B_NEW_VHCL_ID");
                    }
                );
            }
            else
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                        userSchema + ".SP_CLONE_APPL_VEHICLE_DET",
                    parameter =>
                    {
                        parameter.AddWithValue("P_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_REV_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_PART_ID", routepartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        result = records.GetLongOrDefault("VEHICLE_ID") == null ? 0 : Convert.ToInt32(records.GetLongOrDefault("VEHICLE_ID"));
                    }
                );
            }
            return result;
        }
        #endregion
        #region Delete Selected VehicleComponent from so application
        public static int DeleteSelectedVehicleComponent(int vehicleId, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".SP_R_DELE_APPL_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEH_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        #endregion
        #region  Delete Selected VehicleComponent from vr1 application
        public static int DeleteSelectedVR1VehicleComponent(int vehicleId, string userschema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userschema + ".STP_ROUTE_VEHICLES.SP_ROUTE_DELETE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTEPART_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEH_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            return rowsAffected;
        }
        #endregion
        #region ViewVehicleSummaryByID
        public static List<VehicleDetailSummary> ViewVehicleSummaryByID(long rPartId, int vr1, string userSchema)
        {
            List<VehicleDetailSummary> VehicleSummarygridList = new List<VehicleDetailSummary>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                VehicleSummarygridList,
                userSchema + ".SP_ROUTE_COMPONENT_SUMMARY",
                parameter =>
                {
                    parameter.AddWithValue("P_RPART_ID", rPartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VR1", vr1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.VehicleName = records.GetStringOrDefault("VEHICLE_DESC");
                }
            );
            return VehicleSummarygridList;
        }
        #endregion
        #region CheckFormalNameInApplicationVeh
        public static int CheckFormalNameInApplicationVeh(string vehicleName, int organisationId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_CHECK_VEH_INT_NAME",
                parameter =>
                {
                    parameter.AddWithValue("ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("INT_NAME", vehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        result = Convert.ToInt32(record.GetDecimalOrDefault("CNT"));
                    }
            );
            return result;
        }
        #endregion
        #region Add Vehicle To Fleet
        public static int AddVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_ADD_APP_VEHICLE_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("VEH_ID", vehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("OVERRITE", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        result = Convert.ToInt32(record.GetLongOrDefault("VEHICLE_ID"));
                    }
            );
            return result;
        }
        #endregion
        #region Add VR1 Vehicle To Fleet
        public static int AddVR1VhclToFleet(int vehicleId, int organisationId, int flag)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_ADD_CONFIG_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("P_VHCL_ID", vehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_OVERRITE", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        result = Convert.ToInt32(record.GetLongOrDefault("VEHICLE_ID"));
                    }
            );
            return result;
        }
        #endregion
        #region Get the list of application vehicle list
        public static List<AppVehicleConfigList> AppVehicleConfigList(long apprevisionId, string userSchema = UserSchema.Portal)
        {
            List<AppVehicleConfigList> objAppVehicleConfigList = new List<AppVehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            objAppVehicleConfigList,
                userSchema + ".SP_R_LIST_APPL_VEHICLE",
            parameter =>
            {
                parameter.AddWithValue("P_REV_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    var vehiclejson = records.GetStringOrDefault("VEHICLE_JSON");
                    objAppVehicleConfigList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AppVehicleConfigList>>(vehiclejson);
                }
            );
            return objAppVehicleConfigList;
        }
        #endregion
        #region Get the list of VR1 application vehicle list
        public static List<AppVehicleConfigList> AppVehicleConfigListVR1(long routePartId, long versionId, string contentRefNo, string userSchema)
        {
            List<AppVehicleConfigList> objAppVehicleConfigList = new List<AppVehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            objAppVehicleConfigList,
            userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_LIST_VR1_VEHICLE",
            parameter =>
            {
                parameter.AddWithValue("P_ROUTE_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_CONTENT_REF_NO", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                var vehiclejson = records.GetStringOrDefault("VEHICLE_JSON");
                objAppVehicleConfigList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AppVehicleConfigList>>(vehiclejson);
            });
            return objAppVehicleConfigList;
        }
        #endregion
        #region Get the list of NEN vehicle list
        public static List<AppVehicleConfigList> GetNEN_VehicleList(long routePartId)
        {
            List<AppVehicleConfigList> VehicleRouteList = new List<AppVehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                VehicleRouteList,
                    UserSchema.Portal + ".STP_NEN_NOTIFICATION.SP_NENVEHICLE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleId = records.GetLongOrDefault("vehicle_id");
                        instance.VehicleName = records.GetStringOrDefault("vehicle_name");
                        instance.VehicleDescription = records.GetStringOrDefault("vehicle_int_desc");
                        instance.RoutePart = records.GetStringOrDefault("ROUTE_PART_NAME");
                        instance.RouteType = "Planned";
                    }
            );
            return VehicleRouteList;
        }
        #endregion
        #region ListVehCompDetails
        public static List<ApplVehiclComponents> ListVehCompDetails(int revisionId, string userschema = UserSchema.Portal)
        {
            List<ApplVehiclComponents> objlistveh = new List<ApplVehiclComponents>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objlistveh,
                    userschema + ".STP_APPLICATIONS.SP_CHECK_VEHICLE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("A_REVISION_ID ", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("A_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleName = records.GetStringOrDefault("A_VEHICLENAME");
                    }
            );
            return objlistveh;
        }
        #endregion
        #region ListLengthVehCompDetails
        public static List<ApplVehiclComponents> ListLengthVehCompDetails(int revisionId,int vehicleId, string userschema = UserSchema.Portal)
        {
            List<ApplVehiclComponents> objlistveh = new List<ApplVehiclComponents>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objlistveh,
                    //userschema + ".STP_APPLICATIONS.SP_CHECK_VEHICLE_LEN",
                    userschema + ".STP_VEHICLE.SP_CHECK_VEHICLE_LEN",
                parameter =>
                {                    
                    //parameter.AddWithValue("A_REVISION_ID ", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("A_VEHICLE_ID ", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("A_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleName = records.GetStringOrDefault("V_OUTPUT");
                    }
            );
            return objlistveh;
        }
        #endregion
        #region Checking vehicle weight against axle weight validation
        public static List<ApplVehiclComponents> ListVehWeightDetails(int revisionId,int vehicleId, string userschema, int isVR1 = 0)
        {
            List<ApplVehiclComponents> objlistveh = new List<ApplVehiclComponents>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objlistveh,
                    //userschema + ".STP_APPLICATIONS.SP_CHECK_VEHICLE_WEIGHT",
                    userschema + ".STP_VEHICLE.SP_CHECK_VEHICLE_WEIGHT",
                parameter =>
                {
                    //parameter.AddWithValue("P_REVISION_ID ", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    //parameter.AddWithValue("P_IS_VR1", isVR1, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("A_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleName = records.GetStringOrDefault("VW_VEHICLENAME");
                    }
            );
            return objlistveh;
        }
        #endregion
        #region ListVR1VehCompDetails
        public static List<ApplVehiclComponents> ListVR1VehCompDetails(int versionId, string contentref)
        {
            List<ApplVehiclComponents> objlistveh = new List<ApplVehiclComponents>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objlistveh,
                    UserSchema.Portal + ".STP_APPLICATIONS.SP_CHECK_VR1_VEHICLE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSIONID ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENTREFNO ", contentref, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleName = records.GetStringOrDefault("A_VEHICLENAME");
                    }
            );
            return objlistveh;
        }
        #endregion
        #region ListLengthVR1VehDetails
        public static List<ApplVehiclComponents> ListLengthVR1VehDetails(int versionId, string contentref)
        {
            List<ApplVehiclComponents> objlistveh = new List<ApplVehiclComponents>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objlistveh,
                    UserSchema.Portal + ".STP_APPLICATIONS.SP_CHECK_VR1_VEHICLE_LEN",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSIONID ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENTREFNO ", contentref, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleName = records.GetStringOrDefault("V_OUTPUT");
                    }
            );
            return objlistveh;
        }
        #endregion
        #endregion
        #region Vehicle Managemnet in Notification
        #region ImportFleetRouteVehicle
        public static ListRouteVehicleId ImportFleetRouteVehicle(int VehicleID, string ContentRefNo, int simple, int RoutePartId)
        {
            ListRouteVehicleId objListRouteVehicleId = new ListRouteVehicleId();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objListRouteVehicleId,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_IMPORT_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("P_SIMPLE", simple, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VHCL_ID", VehicleID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_PART_ID", RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENTREF_NO‏", ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.VehicleId = Convert.ToInt32(records.GetLongOrDefault("VEHICLE_ID"));
                    instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.ComponentIdList = records.GetStringOrDefault("V_COMP_ID_LIST");
                    instance.ComponentTypeList = records.GetStringOrDefault("V_COMP_TYPE_LIST");
                }
            );
            return objListRouteVehicleId;
        }
        #endregion
        #region ImportReturnRouteVehicle
        public static long ImportReturnRouteVehicle(int routePartId, string contentRefNo)
        {
            long routePartID = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                routePartID,
                UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_IMPORT_RETURNROUTE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("RV_ROUTEPART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("RV_CONTENTREF_NO", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("RV_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                },
                record =>
                {
                    routePartID = (long)record.GetDecimalOrDefault(":B1");
                }
            );
            return routePartID;
        }
        #endregion
        #region UpdateNotifRouteVehicle
        public static int UpdateNotifRouteVehicle(NotificationGeneralDetails obj, int RoutePartId, int vehicleUnits)
        {
            int status = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                   RoutePartId,
                   UserSchema.Portal + ".STP_NOTIFICATION.SP_UPDATE_ROUTE_VEHICLE",
                   parameter =>
                   {
                       parameter.AddWithValue("P_ROUTEPART_ID", RoutePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       if (vehicleUnits == 692001)
                       {
                           parameter.AddWithValue("P_LEN", obj.VehicleLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_RIGID_LEN", obj.RigidLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_WIDTH", obj.VehicleWidth, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_GROSS_WEIGHT", obj.GrossWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_MAX_HEIGHT", obj.MaximamHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_REDUCIBLE_HGT", obj.ReducibleHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_VHCL_REG", obj.RegisterNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_AXLE_WEIGHT", obj.AxelWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_FRONT_PROJTN", obj.FrontProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_REAR_PROJTN", obj.RearProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_RIGHT_PROJTN", obj.RightProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_LEFT_PROJTN", obj.LeftProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                       }
                       else
                       {
                           parameter.AddWithValue("P_LEN", obj.MetricVehicleLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_RIGID_LEN", obj.MetricRigidLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_WIDTH", obj.MetricVehicleWidth, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_GROSS_WEIGHT", obj.GrossWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_MAX_HEIGHT", obj.MetricMaximamHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_REDUCIBLE_HGT", obj.MetricReducibleHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_VHCL_REG", obj.RegisterNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_AXLE_WEIGHT", obj.AxelWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_FRONT_PROJTN", obj.MetricFrontProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_REAR_PROJTN", obj.MetricRearProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_RIGHT_PROJTN", obj.MetricRightProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                           parameter.AddWithValue("P_LEFT_PROJTN", obj.MetricLeftProjection, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                       }
                       parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
                   },
                   record =>
                   {
                       status = (int)record.GetDecimalOrDefault("STATUS_1");
                   }
                );
            return status;
        }
        #endregion
        #region InsertRouteVehicleConfiguration
        public static double InsertRouteVehicleConfiguration(NewConfigurationModel ConfigurationModel, string contentRefNo, int isNotif = 0)
        {
            double VehicleId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleId,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_type", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", ConfigurationModel.RigidLength, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_width_unit", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Width_MTR", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_KG", ConfigurationModel.GrossWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_max_height_unit", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_max_height_mtr", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_KG", ConfigurationModel.MaxAxleWeight, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VERSION_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONTENT_REF_NO", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("p_ISDELETED", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IsNotif", isNotif, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                }
            );
            return VehicleId;
        }
        #endregion
        #region SaveNotifVehicleRegistrationId
        public static int SaveNotifVehicleRegistrationId(int vehicleId, string registrationValue, string fleetId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENCE_PLATE", registrationValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLEET_NO", fleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt16OrDefault("ID_NO");
                }
            );
            return result;
        }
        #endregion
        #region SaveNotifVehicleConfiguration
        public static int SaveNotifVehicleConfiguration(int vehicleId, int componentId, int componentType)
        {
            int result = 0;
            int latPosn = 0;
            int longPosn = 0;
            if (componentType == 234002)
            {
                latPosn = 1;
                longPosn = 1;
            }
            else if (componentType == 234005)
            {
                latPosn = 1;
                longPosn = 2;
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_NOTIFICATION.SP_INSERT_NOTIF_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LAT_POSN", latPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONG_POSN", longPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMP_TYPE", componentType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = 1;
                }
            );
            return result;
        }
        #endregion
        #region SaveNotifAxel
        public static bool SaveNotifAxel(AxleDetails axle)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_AXLE",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", axle.ComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_NO", axle.AxleNumberId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WEIGHT", axle.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYRE_SIZE", axle.TyreSize, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NEXT_AXLE_DIST", axle.AxleSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_COUNT", axle.NoOfWheels, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_LIST", axle.TyreCenters, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                },
                record =>
                {
                    result = true;
                }
            );
            return result;
        }
        #endregion
        #region UpdateMaxAxleWeight
        public static bool UpdateMaxAxleWeight(long vehicleId)
        {
            bool result = false;

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_UPDATE_MAX_AXLE_WEIGHT",
                parameter =>
                {
                    parameter.AddWithValue("MAW_VEHICLE_ID", vehicleId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                },
                record =>
                {
                    result = true;
                }
            );
            return result;
        }
        #endregion
        #region ListCloneAxelDetails
        public static List<AxleDetails> ListCloneAxelDetails(int VehicleId)
        {
            List<AxleDetails> objlistaxel = new List<AxleDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objlistaxel,
                    UserSchema.Portal + ".STP_NOTIFICATION.SP_GET_NOTIF_CLONE_AXEL",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID ", VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ComponentId = (int)records.GetLongOrDefault("COMPONENT_ID");
                        instance.AxleNo = records.GetInt16OrDefault("AXLE_NO");
                        instance.AxleWeight = (decimal)records.GetDoubleOrDefault("WEIGHT");
                        instance.AxleSpacing = records.GetDecimalOrDefault("V_NEXT_AXLE_DIST");
                        instance.NoOfWheels = records.GetInt16OrDefault("WHEEL_COUNT");
                        instance.ComponentType = records.GetDecimalOrDefault("COMP_TYPE");
                    }
            );
            return objlistaxel;
        }
        #endregion
        #region UpdateNewAxleDetails
        public static bool UpdateNewAxleDetails(AxleDetails axle)
        {
            bool result = false;
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_UPDATE_NEW_AXLES",
                parameter =>
                {
                    parameter.AddWithValue("UA_COMPONENT_ID", axle.ComponentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("UA_AXLE_NO", axle.AxleNo, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("UA_NO_OF_WHEEL", axle.NoOfWheels, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("UA_AXLE_WEIGHT", axle.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("UA_AXLE_SPACING", axle.AxleSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = Convert.ToInt16(record["AFFECTEDROW"]);
                }
            );
            if (rowsAffected > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion
        #region GetNotificationVehicle
        public static List<VehicleDetailSummary> GetNotificationVehicle(long partId)
        {
            List<VehicleDetailSummary> objApplicVeh = new List<VehicleDetailSummary>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                objApplicVeh,
                UserSchema.Portal + ".SP_GET_NOTIF_VEHICLE",
            parameter =>
            {
                parameter.AddWithValue("P_ROUTEPARTID", partId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
                    instance.FormalName = records.GetStringOrDefault("vehicle_desc");
                }
            );
            return objApplicVeh;
        }
        #endregion
        #region ImportRouteVehicle
        public static long ImportRouteVehicle(int routePartId, int vehicleId, string contentRefNo, int simple = 0)
        {
            long rPartId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    routePartId,
                    UserSchema.Portal + ".STP_NOTIFICATION.SP_IMPORT_PREV_MOVE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_SIMPLE‏", simple, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_VHCL_ID‏", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_CONTENTREF_NO‏", contentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULT_SET‏", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    try
                    {
                        rPartId = Convert.ToInt32(record.GetLongOrDefault("VEHICLE_ID"));
                    }
                    catch
                    {
                        rPartId = vehicleId;
                    }
                }
            );
            return routePartId;
        }
        #endregion
        #region checking vehicle registraion validation
        public static List<NotifVehicleRegistration> ListVehRegDetails(string contentReferenceNo)
        {
            List<NotifVehicleRegistration> objlistveh = new List<NotifVehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEH_REGISTRATION",
               parameter =>
               {
                   parameter.AddWithValue("RG_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                   parameter.AddWithValue("RG_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                       instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking vehicle import validation
        public static List<NotifVehicleImport> ListVehImportDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_IMPORT",
               parameter =>
               {
                   parameter.AddWithValue("VI_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                   parameter.AddWithValue("VI_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VI_VEHICLENAME");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking vehicle weight against axle weight validation
        public static List<NotifVehicleWeight> ListNotiVehWeightDetails(string contentReferenceNo)
        {
            List<NotifVehicleWeight> objlistveh = new List<NotifVehicleWeight>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_WEIGHT",
               parameter =>
               {
                   parameter.AddWithValue("VW_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                   parameter.AddWithValue("VW_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VW_VEHICLENAME");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking vehicle length validation
        public static List<NotifVehicleImport> ListVehLenDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_LENGTH",
               parameter =>
               {
                   parameter.AddWithValue("P_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VL_OUTPUT");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking vehicle gross weight validation based on vehicle category
        public static List<NotifVehicleImport> ListVehGrossWgtDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_GROSS_WGT",
               parameter =>
               {
                   parameter.AddWithValue("VW_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("VW_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VW_VEHICLELIST");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking vehicle width validation based on vehicle category
        public static List<NotifVehicleImport> ListVehWdhDetails(string contentReferenceNo, int ReqVR1 = 0)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_WIDTH",
               parameter =>
               {
                   parameter.AddWithValue("VWD_CONTENT_REF_NO", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("VWD_REQ_VR1", ReqVR1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("VWD_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VWD_VEHICLELIST");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking Axle Weight validation based on vehicle category
        public static List<NotifVehicleImport> ListVehAxleWgtDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_AXLE_WEIGHT",
               parameter =>
               {
                   parameter.AddWithValue("VAW_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("VAW_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VAW_VEHICLELIST");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region checking Rigid Length validation based on vehicle category
        public static List<NotifVehicleImport> ListVehRigidLenDetails(string contentReferenceNo)
        {
            List<NotifVehicleImport> objlistveh = new List<NotifVehicleImport>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objlistveh,
                 UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_VEHICLE_RIGID_LENGTH",
               parameter =>
               {
                   parameter.AddWithValue("VRL_CONTENT_REF_NO ", contentReferenceNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("VRL_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output, 32767);
               },
                   (records, instance) =>
                   {
                       instance.VehicleName = records.GetStringOrDefault("VRL_VEHICLELIST");
                   }
           );
            return objlistveh;
        }
        #endregion
        #region DeletePrevVehicle
        public static int DeletePrevVehicle(int routePartId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_DELETE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("p_ROUTEPART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEH_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                 }
            );
            return rowsAffected;
        }
        #endregion
        #endregion

        public static List<VehicleConfigurationGridList> GetSimilarVehicleCombinations(SearchVehicleCombination configDimensions)
        {
            List<VehicleConfigurationGridList> vehicleConfigList = new List<VehicleConfigurationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleConfigList,
                UserSchema.Portal + ".GET_SIMILAR_VEHICLES",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_NAME", configDimensions.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FLEET_NO", configDimensions.FleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT", configDimensions.GrossWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RIGID_LEN", configDimensions.RigidLength, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LEN", configDimensions.OverAllLength, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", configDimensions.OverAllWidth, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REAR_OVERHANG", configDimensions.RearOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LEFT_OVERHANG", configDimensions.LeftOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FRONT_OVERHANG", configDimensions.FrontOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RIGHT_OVERHANG", configDimensions.RightOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TRACTOR_AXLE_CNT", configDimensions.NoOfAxlesTractor, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TRAILOR_AXLE_CNT", configDimensions.NoOfAxlesTrailer, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", configDimensions.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_TYPE", configDimensions.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.ConfigurationName = records.GetStringOrDefault("VEHICLE_NAME");
                }
            );
            return vehicleConfigList;
        }
        #region Get components in configuration based on vehicle id
        public static List<ComponentModel> GetComponentsInConfiguration(string componentIds, string userSchema, int flag = 0)
        {
            List<ComponentModel> componentList = new List<ComponentModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentList,
                userSchema + ".STP_VEHICLE_ASSESSMENT.GET_VEHICLE_COMPONENTS_CONFIG",//".GET_VEHICLE_COMPONENTS_IN_CONFIGURATION",
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENT_IDS", componentIds, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FLAG", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.IntendedName = records.GetStringOrDefault("COMPONENT_NAME");
                    instance.FormalName = records.GetStringOrDefault("COMPONENT_SUMMARY");
                    instance.Description = records.GetStringOrDefault("COMPONENT_DESC");
                    instance.ComponentSubType = records.GetInt32OrDefault("COMPONENT_SUBTYPE");
                    instance.CouplingType = records.GetInt32OrDefault("COUPLING_TYPE");
                    instance.VehicleIntent = records.GetInt32OrDefault("VEHICLE_INTENT");
                    instance.ComponentType = records.GetInt32OrDefault("COMPONENT_TYPE");
                    instance.MaxHeight = records.GetDoubleNullable("MAX_HEIGHT");
                    instance.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                    instance.ReducableHeight = records.GetDoubleNullable("RED_HEIGHT");
                    instance.ReducableHeightUnit = records.GetInt32OrDefault("RED_HEIGHT_UNIT");
                    instance.RigidLength = records.GetDoubleNullable("RIGID_LEN");
                    instance.RigidLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                    instance.Width = records.GetDoubleNullable("WIDTH");
                    instance.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                    instance.WheelBase = records.GetDoubleNullable("WHEELBASE");
                    instance.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                    instance.LeftOverhang = records.GetDoubleNullable("LEFT_OVERHANG");
                    instance.LeftOverhangUnit = records.GetInt32OrDefault("LEFT_OVERHANG_UNIT");
                    instance.RightOverhang = records.GetDoubleNullable("RIGHT_OVERHANG");
                    instance.RightOverhangUnit = records.GetInt32OrDefault("RIGHT_OVERHANG_UNIT");
                    instance.FrontOverhang = records.GetDoubleNullable("FRONT_OVERHANG");
                    instance.FrontOverhangUnit = records.GetInt32OrDefault("FRONT_OVERHANG_UNIT");
                    instance.RearOverhang = records.GetDoubleNullable("REAR_OVERHANG");
                    instance.RearOverhangUnit = records.GetInt32OrDefault("REAR_OVERHANG_UNIT");
                    instance.GroundClearance = records.GetDoubleNullable("GROUND_CLEARANCE");
                    instance.GroundClearanceUnit = records.GetInt32OrDefault("GROUND_CLEARANCE_UNIT");
                    instance.OutsideTrack = records.GetDoubleNullable("OUTSIDE_TRACK");
                    instance.OutsideTrackUnit = records.GetInt32OrDefault("OUTSIDE_TRACK_UNIT");
                    instance.GrossWeight = records.GetDoubleNullable("GROSS_WEIGHT");
                    instance.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                    instance.MaxAxleWeight = records.GetDoubleNullable("MAX_AXLE_WEIGHT");
                    instance.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                    instance.AxleWeightUnit = records.GetInt32OrDefault("AXLE_WEIGHT_UNIT");
                    instance.MaxAxleWeightUnit = records.GetInt32OrDefault("AXLE_SPACING_UNIT");
                    instance.WheelSpacingUnit = records.GetInt32OrDefault("WHEEL_SPACING_UNIT");
                    instance.SpacingToFollowing = records.GetDoubleNullable("SPACE_TO_FOLLOWING");
                    instance.SpacingToFollowingUnit = records.GetInt32OrDefault("SPACE_TO_FOLLOWING_UNIT");
                    instance.OrganisationId = records["ORGANISATION_ID"].GetType().Name == "Decimal" ?(long)Convert.ToDecimal(records["ORGANISATION_ID"]) : records.GetLongOrDefault("ORGANISATION_ID");
                    instance.RedGroundClearance = records.GetDoubleNullable("RED_GROUND_CLEARANCE");
                    instance.RedGroundClearanceUnit = records.GetInt32OrDefault("RED_GROUND_CLEARANCE_UNIT");
                    instance.AxleCount = records.GetInt16Nullable("AXLE_COUNT");
                    instance.IsSteerable = records.GetInt16Nullable("IS_STEERABLE_AT_REAR");
                }
            );
            return componentList;
        }
        #endregion

        #region Get configuration details of components
        public static ComponentModel GetComponentsConfigurationDetails(string componentIds, bool isMovement, string userSchema)
        {
            ComponentModel component = new ComponentModel();
            string procedure = ".STP_VEHICLE_ASSESSMENT.GET_VEHICLE_COMPONENTS_DETAILS";//".GET_VEHICLE_COMPONENTS_CONFIGURATION_DETAILS"
            if (isMovement)
                procedure = ".STP_VEHICLE_ASSESSMENT.GET_MOVE_VHCL_COMP_DETAILS";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                component,
                userSchema + procedure,
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENT_IDS", componentIds, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RigidLength = records.GetDecimalNullable("RIGID_LEN") != null ? (double)records.GetDecimalNullable("RIGID_LEN") : 0;
                    //instance.RigidLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                    instance.Width = records.GetDecimalNullable("WIDTH") != null ? (double)records.GetDecimalNullable("WIDTH") : 0;
                    //instance.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                    instance.GrossWeight = records.GetDecimalNullable("GROSS_WEIGHT") != null ? (double)records.GetDecimalNullable("GROSS_WEIGHT") : 0;
                    //instance.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                }
            );
            return component;
        }
        #endregion

        #region ViewVehicleConfigByID
        public static List<VehicleDetail> GetVehicleConfigByPartID(string esdalRef, int vr1Vehicle)
        {
            List<VehicleDetail> VehicleConfiggridobj = new List<VehicleDetail>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                VehicleConfiggridobj,
                UserSchema.Portal + ".STP_ROUTE_VEHICLES.SP_ROUTE_VEHICLE_CONFIGURATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDAL_REF", esdalRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VR1_VEH", vr1Vehicle, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                (records, instance) =>
                {
                    instance.Veh_Id = records.GetLongOrDefault("VEHICLE_ID");
                    instance.Rpart_Id = records.GetLongOrDefault("ROUTE_PART_ID"); 
                    instance.Vehicle_Name = records.GetStringOrDefault("VEHICLE_DESC");
                    instance.Height = records.GetDoubleOrDefault("MAX_HEIGHT");
                    instance.Registration = records.GetStringOrDefault("LICENSE_PLATE");
                    instance.Rigid_Length = records.GetDoubleOrDefault("RIGID_LEN");
                    instance.Width = records.GetDoubleOrDefault("WIDTH");
                    instance.Rear_Overhang = records.GetDoubleOrDefault("REAR_OVERHANG");
                    instance.Length = records.GetDoubleOrDefault("LEN");
                    instance.Gross_Weight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                    instance.Max_Axle_Weight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                    instance.Wheelbase = records.GetDoubleOrDefault("WHEELBASE");
                    instance.Ground_Clearence = records.GetDoubleOrDefault("GROUND_CLEARANCE");
                    instance.Outside_Track = records.GetDoubleOrDefault("OUTSIDE_TRACK");
                }
                );
            return VehicleConfiggridobj;
        }
        #endregion
        #region VehiclecomponentListByID


        public static List<ComponentGroupingModel> ApplicationcomponentList(int routePartId)
        {
            List<ComponentGroupingModel> objComponentModelList = new List<ComponentGroupingModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           objComponentModelList,
             UserSchema.Portal + ".GET_VEHICLE_COMPONENT_LIST",
           parameter =>
           {
               parameter.AddWithValue("P_RPARTID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.VehicleDesc = records.GetStringOrDefault("VEHICLE_DESC");
                   instance.VehicleName = records.GetStringOrEmpty("VEHICLE_NAME");
                   instance.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                   instance.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                   instance.AxleWeight = records.GetDoubleOrDefault("AXLE_WEIGHT");
                   instance.TyreSize = records.GetStringOrDefault("TYRE_SIZE");
                   instance.TyreCentreSpacing = records.GetStringOrDefault("WHEEL_SPACING_LIST");
                   instance.WheelsPerAxle = records.GetInt16OrDefault("WHEEL_COUNT");
                   instance.AxleSpacing = records.GetDoubleOrDefault("NEXT_AXLE_DIST");
                   instance.AxleSpacing2 = records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
                   instance.Wheelbase = records.GetDoubleOrDefault("WHEELBASE");
                   instance.RearOverhang = records.GetDoubleOrDefault("REAR_OVERHANG");
                   instance.OutsideTrack = records.GetDoubleOrDefault("OUTSIDE_TRACK");


               }
          );
            return objComponentModelList;
        }

        #endregion VehiclecomponentListID

        #region Movement Vehicle
        public static List<MovementVehicleConfig> InsertMovementVehicle(InsertMovementVehicle movementVehicle)
        {
            List<MovementVehicleConfig> movementVehicleConfig = new List<MovementVehicleConfig>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movementVehicleConfig,
                movementVehicle.UserSchema + ".STP_MOVEMENT.SP_INSERT_MOVEMENT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", movementVehicle.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", movementVehicle.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FLAG", movementVehicle.Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_ID", movementVehicle.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REV_ID", movementVehicle.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_VR1", movementVehicle.IsVr1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    var vehiclejson = records.GetStringOrDefault("VEHICLE_JSON");
                    movementVehicleConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MovementVehicleConfig>>(vehiclejson);
                }
            );
            return movementVehicleConfig;
        }

        public static List<MovementVehicleConfig> SelectMovementVehicle(long movementId, string userSchema)
        {
            List<MovementVehicleConfig> movementVehicle = new List<MovementVehicleConfig>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                movementVehicle,
                userSchema + ".STP_MOVEMENT.SP_SELECT_MOVEMENT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_MOVEMENT_ID", movementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    var vehiclejson = records.GetStringOrDefault("VEHICLE_JSON");
                    movementVehicle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MovementVehicleConfig>>(vehiclejson);
                }
            );
            return movementVehicle;
        }
        public static ConfigurationModel GetMovementConfigDetails(long vehicleId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".STP_MOVEMENT.GET_MVMNT_VEHICLE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MovementClassificationId = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.FormalName = records.GetStringOrDefault("VEHICLE_DESC");
                        VehicleConfig.Description = records.GetStringOrDefault("VEHICLE_INT_DESC");
                        VehicleConfig.InternalName = records.GetStringOrDefault("VEHICLE_NAME");
                        VehicleConfig.ComponentType = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MaxHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                        VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN"); //swapped with LEN below
                        VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                        VehicleConfig.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        VehicleConfig.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                        VehicleConfig.TyreSpacing = records.GetDoubleOrDefault("SIDE_BY_SIDE");
                        VehicleConfig.TyreSpacingUnit = records.GetInt32OrDefault("SIDE_BY_SIDE_UNIT");
                        VehicleConfig.TravellingSpeed = Convert.ToDouble(records.GetSingleOrDefault("SPEED"));
                        VehicleConfig.TravellingSpeedUnit = records.GetInt32OrDefault("SPEED_UNIT");
                        VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                        VehicleConfig.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                        VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_Weight_UNIT");
                        VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                        VehicleConfig.AxleCount = Convert.ToInt32(records["TRACTOR_AXLE_CNT"]);
                        VehicleConfig.TrailerAxleCount = Convert.ToInt32(records["TRAILOR_AXLE_CNT"]);
                    }
            );
            return VehicleConfig;
        }
        public static List<VehicleConfigList> GetMovementVehicleConfigPosn(long vhclID, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".STP_MOVEMENT.GET_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                    result.LongPosn = records.GetInt16OrDefault("LAT_POSN");
                    result.ComponentTypeId = Convert.ToInt64(records["COMPONENT_TYPE"]);
                    result.ComponentType= records.GetStringOrDefault("COMPONENT_TYPE_NAME");
                    result.ComponentSubTypeId = Convert.ToInt64(records["SUB_TYPE"]);
                }
            );
            return listVehclRegObj;
        }
        public static List<VehicleRegistration> GetMovementRegistration(long vehicleId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".STP_MOVEMENT.GET_VEHICLE_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("LICENSE_PLATE");
                    result.FleetId = records.GetStringOrDefault("FLEET_NO");
                    result.IdNumber = records.GetInt16OrDefault("ID_NO");
                }
            );
            return listVehclRegObj;
        }
        public static bool AssignMovementVehicle(VehicleAssignementParams vehicleAssignement)
        {
            bool status = false;
            int result = 0;
            var routeVehicleJson = Newtonsoft.Json.JsonConvert.SerializeObject(vehicleAssignement.VehicleAssignments);
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                vehicleAssignement.UserSchema + ".STP_MOVEMENT.SP_UPDATE_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_VEHICLE_JSON", routeVehicleJson, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_REVISION_ID", vehicleAssignement.RevsionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_VERSION_ID", vehicleAssignement.VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_CONTENT_REF_NUM", vehicleAssignement.ContentRefNum, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_AFFECT_ROW", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt32("P_AFFECT_ROW");
                });
            if (result > 0)
                status = true;
            return status;
        }
        public static bool DeleteMovementVehicle(long movementId, long vehicleId, string userSchema)
        {
            bool status = false;
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".STP_MOVEMENT.SP_DELETE_MOVEMENT_VEHICLE",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_MOVEMENT_ID", movementId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_AFFECT_ROW", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt32("P_AFFECT_ROW");
                }
            );
            if (result > 0)
                status = true;
            return status;
        }
        public static List<MovementVehicleList> GetRouteVehicleList(long revisionId, long versionId, string cont_Ref_No, string userSchema = UserSchema.Portal, int isHistoric=0)
        {
            List<MovementVehicleList> prevMovementVehicleLists = new List<MovementVehicleList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                prevMovementVehicleLists,
                userSchema + ".STP_MOVEMENT.SP_GET_ROUTE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_REF_NO", cont_Ref_No, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", isHistoric, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RoutePartId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RoutePartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                    instance.RoutePartName = records.GetStringOrDefault("PART_NAME");
                    instance.FromAddress = records.GetStringOrDefault("FROM_ADDRESS");
                    instance.ToAddress = records.GetStringOrDefault("TO_ADDRESS");
                    instance.IsSort = (int)records.GetDecimalOrDefault("IS_SORT");
                    instance.VehicleList = GetVehicleList(instance.RoutePartId, userSchema, isHistoric);

                });
            return prevMovementVehicleLists;
        }
        public static List<VehicleDetails> GetVehicleList(long routePartId, string userSchema, int isHistoric = 0)
        {
            List<VehicleDetails> vehicleList = new List<VehicleDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleList,
                userSchema + ".STP_MOVEMENT.SP_GET_VEHICLE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ROUTE_PART_ID", routePartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", isHistoric, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
                    instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                    instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                    instance.ParentVehicleId = records.GetLongOrDefault("PARENT_VEHICLE_ID");
                    instance.VehicleCompList = GetComponentList(instance.VehicleId, userSchema, isHistoric);
                });
            return vehicleList;
        }

        private static List<VehicleConfigList> GetComponentList(long vehicleId, string userSchema, int isHistoric = 0)
        {
            List<VehicleConfigList> compList = new List<VehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                compList,
                userSchema + ".STP_MOVEMENT.SP_GET_COMPONENT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", isHistoric, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                    instance.ComponentTypeId = records.GetInt32OrDefault("COMPONENT_TYPE");
                    instance.ComponentSubTypeId = records.GetInt32OrDefault("component_subtype");
                });
            return compList;
        }
        public static List<VehicleList> GetFavouriteVehicles(int organisationId,int movementId, string userSchema)
        {
            List<VehicleList> vehicleList = new List<VehicleList>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleList,
                userSchema + ".GET_VEHICLE_FAV_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOVEMENT_ID", movementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                        instance.InternalName = records.GetStringOrDefault("INTERNAL_NAME");
                        instance.IndendedUse = records.GetStringOrDefault("NAME");
                    }
            );

            return vehicleList;
        }
        #endregion

        #region Vehicle workflow TEMP table implementation
        #region Get vehicle dimensions based on component id in TEMP table
        public static ConfigurationModel GetConfigDimensions(string GUID, int configTypeId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".STP_VEHICLE.GET_VEHICLE_CONFIG_DIMENSIONS",
                parameter =>
                {
                    parameter.AddWithValue("p_GUID", GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONFIG_TYPE", configTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.RigidLength = records["RIGID_LNGTH"].ToString() == "" ? 0 : Convert.ToDouble(records["RIGID_LNGTH"]);
                        VehicleConfig.OverallLength = records["RIGID_LNGTH"].ToString() == "" ? 0 : Convert.ToDouble(records["RIGID_LNGTH"]); //swapped with LEN below
                        VehicleConfig.GrossWeight = records["GROSS_WIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["GROSS_WIGHT"]);
                        VehicleConfig.Width = records["OVR_WIDTH"].ToString() == "" ? 0 : Convert.ToDouble(records["OVR_WIDTH"]);
                        VehicleConfig.MaxHeight = records["MAX_HIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["MAX_HIGHT"]);
                        VehicleConfig.MaxAxleWeight = records["MAX_AXLE"].ToString() == "" ? 0 : Convert.ToDouble(records["MAX_AXLE"]);
                        VehicleConfig.ReducedHeight = records["RED_HIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["RED_HIGHT"]);
                        VehicleConfig.WheelBase = records["AXL_NXT_SUM"].ToString() == "" ? 0 : Convert.ToDouble(records["AXL_NXT_SUM"]);
                    }
            );
            return VehicleConfig;
        }
        #endregion
        #region Get vehicle dimensions based on vehicle id
        public static ConfigurationModel GetVehicleDimensions(long VehicleId, int configTypeId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".STP_VEHICLE.GET_VEHICLE_DIMENSIONS",
                parameter =>
                {
                    parameter.AddWithValue("p_VehicleId", VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_CONFIG_TYPE", configTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.RigidLength = records["RIGID_LNGTH"].ToString() == "" ? 0 : Convert.ToDouble(records["RIGID_LNGTH"]);
                        VehicleConfig.OverallLength = records["RIGID_LNGTH"].ToString() == "" ? 0 : Convert.ToDouble(records["RIGID_LNGTH"]); //swapped with LEN below
                        VehicleConfig.GrossWeight = records["GROSS_WIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["GROSS_WIGHT"]);
                        VehicleConfig.Width = records["OVR_WIDTH"].ToString() == "" ? 0 : Convert.ToDouble(records["OVR_WIDTH"]);
                        VehicleConfig.MaxHeight = records["MAX_HIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["MAX_HIGHT"]);
                        VehicleConfig.MaxAxleWeight = records["MAX_AXLE"].ToString() == "" ? 0 : Convert.ToDouble(records["MAX_AXLE"]);
                        VehicleConfig.ReducedHeight = records["RED_HIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["RED_HIGHT"]);
                        VehicleConfig.WheelBase = records["AXL_NXT_SUM"].ToString() == "" ? 0 : Convert.ToDouble(records["AXL_NXT_SUM"]);
                    }
            );
            return VehicleConfig;
        }
        #endregion
        #region Insert components from TEMP table to component table and insert vehicle config posn
        public static bool InsertVehicleConfigPosnTemp(string GUID, int vehicleId, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int configId = 0;
            string spName = userSchema + ".STP_VEHICLE.SP_INSERT_VEHICLE_CONFIG_POSN_TEMP";
            if (userSchema == UserSchema.Sort)
            {
                spName = userSchema + ".STP_VEHICLE.SP_INSERT_ROUTE_VEHICLE";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                configId, spName,
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GUID", GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        configId = Convert.ToInt16(records["VEHICLE_ID"]);
                    }
            );
            if (configId > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion
        public static ConfigurationModel GetVehicleDetails(int vehicleId, bool movement, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            string sp = ".STP_VEHICLE.SP_GET_VEHICLE_CONFIGURATION";
            if (movement)
            {
                sp = ".STP_VEHICLE.SP_GET_VEHICLE_CONFIGURATION_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + sp,
                parameter =>
                {
                    parameter.AddWithValue("p_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Is_Simple", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                        VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                        VehicleConfig.MovementClassificationId = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.FormalName = records.GetStringOrDefault("VEHICLE_DESC");
                        VehicleConfig.Description = records.GetStringOrDefault("VEHICLE_INT_DESC");
                        VehicleConfig.InternalName = records.GetStringOrDefault("VEHICLE_NAME");
                        VehicleConfig.ComponentType = records.GetInt32OrDefault("VEHICLE_TYPE");
                        if (records["MAX_HEIGHT"].ToString() == "")
                            VehicleConfig.MaxHeight = null;
                        else
                            VehicleConfig.MaxHeight = Convert.ToDouble(records["MAX_HEIGHT"]);
                        VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN"); //swapped with LEN below
                        VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                        if (records["WHEELBASE"].ToString() == "0" || records["WHEELBASE"].ToString() == "")
                            VehicleConfig.WheelBase = null;
                        else
                            VehicleConfig.WheelBase = Convert.ToDouble(records["WHEELBASE"]);
                        VehicleConfig.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                        VehicleConfig.TyreSpacing = records.GetDoubleOrDefault("SIDE_BY_SIDE");
                        VehicleConfig.TyreSpacingUnit = records.GetInt32OrDefault("SIDE_BY_SIDE_UNIT");
                        VehicleConfig.TravellingSpeed = Convert.ToDouble(records.GetSingleOrDefault("SPEED"));
                        VehicleConfig.TravellingSpeedUnit = records.GetInt32OrDefault("SPEED_UNIT");
                        //VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                        VehicleConfig.GrossWeight = records.GetFieldType("GROSS_WEIGHT") != null ? Convert.ToDouble(records["GROSS_WEIGHT"]) : 0;
                        if (records["MAX_AXLE_WEIGHT"].ToString() == "0"|| records["MAX_AXLE_WEIGHT"].ToString() == "")
                            VehicleConfig.MaxAxleWeight = null;
                        else
                            VehicleConfig.MaxAxleWeight = Convert.ToDouble(records["MAX_AXLE_WEIGHT"]);
                        VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_Weight_UNIT");
                        VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                        if (records["FRONT_OVERHANG"].ToString() == "")
                            VehicleConfig.NotifFrontOverhang = null;
                        else
                            VehicleConfig.NotifFrontOverhang = Convert.ToDouble(records["FRONT_OVERHANG"]);
                        if (records["REAR_OVERHANG"].ToString() == "")
                            VehicleConfig.NotifRearOverhang = null;
                        else
                            VehicleConfig.NotifRearOverhang = Convert.ToDouble(records["REAR_OVERHANG"]);
                        if (records["RIGHT_OVERHANG"].ToString() == "")
                            VehicleConfig.NotifRightOverhang = null;
                        else
                            VehicleConfig.NotifRightOverhang = Convert.ToDouble(records["RIGHT_OVERHANG"]);
                        if (records["LEFT_OVERHANG"].ToString() == "")
                            VehicleConfig.NotifLeftOverhang = null;
                        else
                            VehicleConfig.NotifLeftOverhang = Convert.ToDouble(records["LEFT_OVERHANG"]);
                        VehicleConfig.AxleCount = records["AXLE_COUNT"].ToString() == "" ? 0 : Convert.ToInt32(records["AXLE_COUNT"]); 
                        //VehicleConfig.TrailerAxleCount = Convert.ToInt32(records["trailer_axle_count"]);

                        VehicleConfig.TrainWeight = records.GetFieldType("TRAIN_WEIGHT")!=null?Convert.ToDouble(records["TRAIN_WEIGHT"]):0;
                        if (records["REDUCIBLE_HEIGHT"].ToString() == "")
                            VehicleConfig.ReducedHeight = null;
                        else
                            VehicleConfig.ReducedHeight = Convert.ToDouble(records["REDUCIBLE_HEIGHT"]); 
                    }
            );
            return VehicleConfig;
        }
        #region check formal name in Temp table
        public static int CheckFormalNameExistsTemp(int componentId, int organisationId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".STP_VEHICLE.SP_VERIFY_FORMAL_NAME_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                 }
            );
            return result;
        }
        #endregion
        #region Insert config to movement vehicle temp table
        public static List<MovementVehicleConfig> InsertConfigurationTemp(NewConfigurationModel ConfigurationModel, string userSchema = UserSchema.Portal)
        {
            List<MovementVehicleConfig> movementVehicle = new List<MovementVehicleConfig>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementVehicle,
                userSchema + ".STP_VEHICLE.SP_INSERT_VEHICLECONFIGURATION_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("p_organisation_id", ConfigurationModel.OrganisationId, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.WidthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_UNIT", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_HEIGHT_MTR", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVEMENT_ID", ConfigurationModel.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.MovementId = records.GetLongOrDefault("MOVEMENT_ID");
                }
            );
            return movementVehicle;
        }
        #endregion
        #region Insert vehicle registration to movement vehicle temp table
        public static int CreateVehicleRegistrationTemp(int vehicleId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_VEHICLE.SP_INSERT_VEHICLE_ID_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENCE_PLATE", registrationValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLEET_NO", fleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt16OrDefault("ID_NO");
                }
            );
            return result;
        }
        #endregion
        #region Insert vehicle config posn from application
        public static bool InsertMovementConfigPosnTemp(string GUID, int vehicleId, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int configId = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                configId,
                userSchema + ".STP_VEHICLE.SP_INSERT_VEHICLE_POSN_MVMT_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GUID", GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        configId = Convert.ToInt16(records["VEHICLE_ID"]);
                    }
            );
            if (configId > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion

        public static List<VehicleRegistration> GetVehicleRegistrationTemp(int vehicleId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                UserSchema.Portal + ".STP_VEHICLE.GET_VEHICLE_ID_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("LICENSE_PLATE");
                    result.FleetId = records.GetStringOrDefault("FLEET_NO");
                    result.IdNumber = records.GetInt16OrDefault("ID_NO");
                }
            );
            return listVehclRegObj;
        }

        public static bool UpdateMovementVehicle(NewConfigurationModel ConfigurationModel, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                     count,
                   userSchema + ".STP_VEHICLE.SP_MVMT_EDIT_VEHICLE",
                   parameter =>
                   {
                       parameter.AddWithValue("p_VEHICLE_NAME", ConfigurationModel.VehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_vehicle_int_desc", ConfigurationModel.VehicleDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_vehicle_desc", ConfigurationModel.VehicleIntDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_VEHICLE_TYPE", ConfigurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_vehicle_purpose", ConfigurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       //parameter.AddWithValue("p_organisation_id", ConfigurationModel.OrganisationId, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_LEN", ConfigurationModel.Length, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_LEN_UNIT", ConfigurationModel.LengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_LEN_MTR", (ConfigurationModel.LengthMtr == 0 || ConfigurationModel.LengthMtr == null) ? ConfigurationModel.Length : ConfigurationModel.LengthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_RIGID_LEN", ConfigurationModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_Regid_LEN_Unit", ConfigurationModel.RigidLengthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_RIGID_LEN_MTR", (ConfigurationModel.RigidLengthMtr == 0 || ConfigurationModel.RigidLengthMtr == null) ? ConfigurationModel.RigidLength : ConfigurationModel.RigidLengthMtr, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WIDTH", ConfigurationModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WIDTH_UNIT", ConfigurationModel.WidthUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WIDTH_MTR", (ConfigurationModel.WidthMtr == 0 || ConfigurationModel.WidthMtr == null) ? ConfigurationModel.Width : ConfigurationModel.WidthMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_GROSS_WEIGHT", ConfigurationModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_GROSS_WEIGHT_UNIT", ConfigurationModel.GrossWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_GROSS_WEIGHT_KG", (ConfigurationModel.GrossWeightKg == 0 || ConfigurationModel.GrossWeightKg == null) ? ConfigurationModel.GrossWeight : ConfigurationModel.GrossWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_HEIGHT", ConfigurationModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_HEIGHT_UNIT", ConfigurationModel.MaxHeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_HEIGHT_MTR", (ConfigurationModel.MaxHeightMtr == 0 || ConfigurationModel.MaxHeightMtr == null) ? ConfigurationModel.MaxHeight : ConfigurationModel.MaxHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_RED_HEIGHT_MTR", ConfigurationModel.RedHeightMtr, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_AXLE_WEIGHT", ConfigurationModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_AXLE_WEIGHT_UNIT", ConfigurationModel.MaxAxleWeightUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_MAX_AXLE_WEIGHT_KG", (ConfigurationModel.MaxAxleWeightKg == 0 || ConfigurationModel.MaxAxleWeightKg == null) ? ConfigurationModel.MaxAxleWeight : ConfigurationModel.MaxAxleWeightKg, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WHEELBASE", ConfigurationModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_WHEELBASE_UNIT", ConfigurationModel.WheelBaseUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SPEED", ConfigurationModel.Speed, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SPEED_UNIT", ConfigurationModel.SpeedUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SIDE_BY_SIDE", ConfigurationModel.TyreSpacing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("P_SIDE_BY_SIDE_UNIT", ConfigurationModel.TyreSpacingUnit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_TRACTOR_AXLE_COUNT", ConfigurationModel.TractorAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_TRAILER_AXLE_COUNT", ConfigurationModel.TrailerAxleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_VEHICLE_ID", ConfigurationModel.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                       parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                   record =>
                   {
                       count = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                   }
               );
            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        #region Add Vehicle To Fleet form movement temp
        public static int AddMovementVehicleToFleet(int vehicleId, int organisationId, int flag, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_VEHICLE.SP_ADD_MVMT_VEHICLE_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("VEH_ID", vehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("ORG_ID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("OVERRITE", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    record =>
                    {
                        result = Convert.ToInt32(record.GetLongOrDefault("VEHICLE_ID"));
                    }
            );
            return result;
        }
        #endregion
        #region Get vehicle dimensions based on component id in movement TEMP table
        public static ConfigurationModel GetMovementConfigDimensions(int vehicleId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".STP_VEHICLE.GET_MVMT_VEHICLE_CONFIG_DIMENSIONS",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.RigidLength = records["RIGID_LNGTH"].ToString() == "" ? 0 : Convert.ToDouble(records["RIGID_LNGTH"]);
                        VehicleConfig.OverallLength = records["RIGID_LNGTH"].ToString() == "" ? 0 : Convert.ToDouble(records["RIGID_LNGTH"]); //swapped with LEN below
                        VehicleConfig.GrossWeight = records["GROSS_WIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["GROSS_WIGHT"]);
                        VehicleConfig.Width = records["OVR_WIDTH"].ToString() == "" ? 0 : Convert.ToDouble(records["OVR_WIDTH"]);
                        VehicleConfig.MaxHeight = records["MAX_HIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["MAX_HIGHT"]);
                        VehicleConfig.MaxAxleWeight = records["MAX_AXLE"].ToString() == "" ? 0 : Convert.ToDouble(records["MAX_AXLE"]);
                        VehicleConfig.ReducedHeight = records["RED_HIGHT"].ToString() == "" ? 0 : Convert.ToDouble(records["RED_HIGHT"]);
                        VehicleConfig.WheelBase = records["AXL_NXT_SUM"].ToString() == "" ? 0 : Convert.ToDouble(records["AXL_NXT_SUM"]);
                    }
            );
            return VehicleConfig;
        }
        #endregion

        public static int AddMovementComponentToFleet(int componentid, int organisationid, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".STP_VEHICLE.SP_MVMT_COMPONENT_ADD_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationid, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetLongOrDefault("COMPONENT_ID"));
                 }
            );
            return result;
        }

        public static int MovementCheckFormalNameExists(int componentId, int organisationId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + ".STP_VEHICLE.SP_MVMT_VERIFY_FORMAL_NAME",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGID", organisationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                 }
            );
            return result;
        }
        #endregion
        public static ConfigurationModel GetMovementVehicleDetails(int vehicleId, int isRoute, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                VehicleConfig,
                userSchema + ".STP_VEHICLE_ASSESSMENT.GET_VEHICLE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ROUTE_VEHICLE", isRoute, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        VehicleConfig.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                        VehicleConfig.GrossWeight = (double)records.GetDecimalOrDefault("GROSS_WEIGHT");
                        VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                        VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                        VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN");
                        VehicleConfig.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                        VehicleConfig.AxleCount = (int)records.GetDecimalOrDefault("AXLE_COUNT");
                        VehicleConfig.NotifLeftOverhang = (double)records.GetDecimalOrDefault("LEFT_OVERHANG");
                        VehicleConfig.NotifRightOverhang = (double)records.GetDecimalOrDefault("RIGHT_OVERHANG");
                        VehicleConfig.NotifFrontOverhang = (double)records.GetDecimalOrDefault("FRONT_OVERHANG");
                        VehicleConfig.NotifRearOverhang = (double)records.GetDecimalOrDefault("REAR_OVERHANG");
                        VehicleConfig.WheelBase = records.GetDoubleOrDefault("WHEELBASE");
                        VehicleConfig.TrailerWeight = (double)records.GetDecimalOrDefault("TRAILER_WEIGHT");
                        VehicleConfig.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                    }
            );
            return VehicleConfig;
        }

        #region SORT Previous/Current Movement VehicleList
        public static List<AppVehicleConfigList> GetSortMovementVehicle(long revisionId, int rListType)
        {
            List<AppVehicleConfigList> appVehicleConfigList = new List<AppVehicleConfigList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            appVehicleConfigList,
            UserSchema.Sort + ".STP_MOVEMENT.SP_PREVIOUS_MOVE_VEHICLES",
               parameter =>
               {
                   parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_TYPE", rListType, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                       instance.VehicleName = records.GetStringOrDefault("VEHICLE_NAME");
                       instance.VehicleDescription = records.GetStringOrDefault("VEHICLE_DESC");
                       instance.VehicleType = records.GetInt32OrDefault("VEHICLE_TYPE");
                       instance.VehiclePurpose = records.GetInt32OrDefault("VEHICLE_PURPOSE");
                       instance.VehicleCompList = GetComponentList(instance.VehicleId, UserSchema.Sort);
                   }
              );
            return appVehicleConfigList;
        }
        #endregion

        public static List<VehicleConfigurationGridList> GetFilteredVehicleCombinations(ConfigurationModel configurationModel)
        {
            List<VehicleConfigurationGridList> vehicleConfigList = new List<VehicleConfigurationGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                vehicleConfigList,
                UserSchema.Portal + ".STP_VEHICLE.GET_FILTERED_VEHICLES",
                parameter =>
                {
                    parameter.AddWithValue("P_INTERNAL_NAME", configurationModel.InternalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GROSS_WEIGHT", configurationModel.GrossWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RIGID_LEN", configurationModel.RigidLength, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LEN", configurationModel.OverallLength, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_WIDTH", configurationModel.Width, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REAR_OVERHANG", configurationModel.NotifRearOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LEFT_OVERHANG", configurationModel.NotifLeftOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FRONT_OVERHANG", configurationModel.NotifFrontOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RIGHT_OVERHANG", configurationModel.NotifRightOverhang, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TRACTOR_AXLE_CNT", configurationModel.AxleCount, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TRAILOR_AXLE_CNT", configurationModel.TrailerAxleCount, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HEIGHT", configurationModel.MaxHeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MAX_AXLE_WEIGHT", configurationModel.MaxAxleWeight, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_ID", configurationModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_TYPE", configurationModel.VehicleType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_PURPOSE", configurationModel.VehiclePurpose, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                    instance.ConfigurationName = records.GetStringOrDefault("VEHICLE_NAME");
                }
            );
            return vehicleConfigList;
        }

        public static long ImportFleetVehicleToRoute(long configId, string userSchema, int applnRev = 0)
        {
            long result = 0;
            string Procname = ".STP_VEHICLE.IMPORT_FLEET_VEH_TO_ROUTE_VEH";
            
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + Procname, 
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", configId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_APPLN_REV_ID", applnRev, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("VEHICLE_ID");
                }
            );
            return result;
        }

        public static long ChekcVehicleIsValid(long vehicleId, int flag, string userSchema)
        {
            long result = 0;
            string Procname = ".STP_VEHICLE.SP_CHECK_VEHICLE_VALID";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + Procname,
                parameter =>
                {
                    parameter.AddWithValue("p_vehicle_id", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_flag", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = Convert.ToInt32(records["COMP_COUNT"]);
                }
            );
            return result;
        }

        #region check vehicle validation
        public static ImportVehicleValidations CheckVehicleValidations(int vehicleId, string userschema = UserSchema.Portal)
        {
            ImportVehicleValidations vehicleValidations  = new ImportVehicleValidations();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                vehicleValidations,
                    userschema + ".STP_VEHICLE.SP_CHECK_VEHICLE_VALIDATIONS",
                parameter =>
                {
                    parameter.AddWithValue("A_VEHICLE_ID ", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("A_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 records =>
                {
                    vehicleValidations.AxleLength =Convert.ToInt16(records["CHK_AXLE_LENGTH"]);
                    vehicleValidations.Weight = Convert.ToInt16(records["CHK_WEIGHT"]);
                }
            );
            return vehicleValidations;
        }
        #endregion

        #region AutoFillVehicles
        public static List<AutoFillModel> AutoFillVehicles(string vehicleIds, int vehicleCount, string userSchema)
        {
            List<AutoFillModel> autoFillModel = new List<AutoFillModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                autoFillModel,
                    userSchema + ".SP_AUTOFILL_VEHILCE",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_IDS ", vehicleIds, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_VEHICLE_COUNT ", vehicleCount, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 records =>
                 {
                     var autoFillRecords = records.GetStringOrDefault("AUTO_FILL_VEHICLE_JSON");
                     autoFillModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AutoFillModel>>(autoFillRecords);
                 }
            );
            return autoFillModel;
        }
        #endregion

        #region GetNenApiVehiclesList
        public static List<VehicleConfigration> GetNenApiVehiclesList(long notificationId, long organisationId, string userschema)
        {
            List<VehicleConfigration> objVehicleConfigration = new List<VehicleConfigration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objVehicleConfigration,
                    userschema + ".STP_NON_ESDAL_ROUTES.SP_GET_VEHICLES",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIFICATION_ID ", notificationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_ORG_ID ", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 records =>
                 {
                     var objVehicleConfigrations = records.GetStringOrDefault("VEHICLE_JSON");
                     objVehicleConfigration = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehicleConfigration>>(objVehicleConfigrations);
                 }
            );
            return objVehicleConfigration;
        }
        #endregion
    }
}