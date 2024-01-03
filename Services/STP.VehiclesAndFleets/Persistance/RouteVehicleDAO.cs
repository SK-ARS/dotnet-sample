using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
namespace STP.VehiclesAndFleets.Persistance
{
    public static class RouteVehicleDAO
    {
        #region Vehicle Configuration
        public static ConfigurationModel GetRouteVehicleConfigDetails(int componentId, string userSchema = UserSchema.Portal)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
                //Setup Procedure View Configuration
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    VehicleConfig,
                    userSchema + ".SP_APPL_GET_VEHICLE_CONFIG",
                    parameter =>
                    {
                        parameter.AddWithValue("p_VHCL_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
                            VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                            if (records["MAX_AXLE_WEIGHT"].ToString() == "0" || records["MAX_AXLE_WEIGHT"].ToString() == "")
                                VehicleConfig.MaxAxleWeight = null;
                            else
                                VehicleConfig.MaxAxleWeight = Convert.ToDouble(records["MAX_AXLE_WEIGHT"]);
                            VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
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
                            if (records["RED_HEIGHT_MTR"].ToString() == "")
                                VehicleConfig.ReducedHeight = null;
                            else
                                VehicleConfig.ReducedHeight = Convert.ToDouble(records["RED_HEIGHT_MTR"]);
                            VehicleConfig.AxleCount = records["AXLE_COUNT"].ToString() == "" ? 0 : Convert.ToInt32(records["AXLE_COUNT"]);
                            if (records["TRAIN_WEIGHT"].ToString() == "")
                                VehicleConfig.TrainWeight = null;
                            else
                                VehicleConfig.TrainWeight = Convert.ToDouble(records["TRAIN_WEIGHT"]);
                        }
                );
            return VehicleConfig;
        }
        public static ConfigurationModel GetRouteVehicleConfigDetailsForVR1(int componentId, string userSchema,int isEdit=0)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
            //Setup Procedure View Configuration

            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                        VehicleConfig,
                        userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_GET_VEHICLE_CONFIG",
                        parameter =>
                        {
                            parameter.AddWithValue("p_RoutePart_ID", 0, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_VHCL_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("p_VHCL_EDIT", isEdit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
                                if(records["GROSS_WEIGHT"].ToString() != "0" && records["GROSS_WEIGHT"].ToString() != "")  
                                    VehicleConfig.GrossWeight = records.GetFieldType("GROSS_WEIGHT").Name == "Decimal"?Convert.ToDouble(records.GetDecimalOrDefault("GROSS_WEIGHT")):records.GetDoubleOrDefault("GROSS_WEIGHT");
                                if (records["MAX_AXLE_WEIGHT"].ToString() == "0"|| records["MAX_AXLE_WEIGHT"].ToString() == "")
                                    VehicleConfig.MaxAxleWeight = null;
                                else
                                    VehicleConfig.MaxAxleWeight = Convert.ToDouble(records["MAX_AXLE_WEIGHT"]);
                                //VehicleConfig.MaxAxleWeight = records.GetFieldType("MAX_AXLE_WEIGHT") != null && records.GetFieldType("MAX_AXLE_WEIGHT").Name == "Decimal" ? Convert.ToDouble(records.GetDecimalOrDefault("MAX_AXLE_WEIGHT")) : records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                                VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                                VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                                VehicleConfig.AxleCount = records["AXLE_COUNT"].ToString() == "" ? 0 : Convert.ToInt32(records["AXLE_COUNT"]);

                                if (records["front_overhang"].ToString() == "")
                                    VehicleConfig.NotifFrontOverhang = null;
                                else
                                    VehicleConfig.NotifFrontOverhang = Convert.ToDouble(records["front_overhang"]);
                                if (records["rear_overhang"].ToString() == "")
                                    VehicleConfig.NotifRearOverhang = null;
                                else
                                    VehicleConfig.NotifRearOverhang = Convert.ToDouble(records["rear_overhang"]);
                                if (records["right_overhang"].ToString() == "")
                                    VehicleConfig.NotifRightOverhang = null;
                                else
                                    VehicleConfig.NotifRightOverhang = Convert.ToDouble(records["right_overhang"]);
                                if (records["left_overhang"].ToString() == "")
                                    VehicleConfig.NotifLeftOverhang = null;
                                else
                                    VehicleConfig.NotifLeftOverhang = Convert.ToDouble(records["left_overhang"]);

                                VehicleConfig.TrainWeight = Convert.ToDouble(records["TRAIN_WEIGHT"]);
                                if (records["REDUCIBLE_HEIGHT"].ToString() == "")
                                    VehicleConfig.ReducedHeight = null;
                                else
                                    VehicleConfig.ReducedHeight = Convert.ToDouble(records["REDUCIBLE_HEIGHT"]);
                            }
                    );
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return VehicleConfig;
        }
        public static List<VehicleConfigList> GetRouteVehicleConfigPosn(int vehicleId, string userSchema)
        {
            List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    listVehclRegObj,
                    userSchema + ".SP_APPL_GET_CONFIG_POSN",
                    parameter =>
                    {
                        parameter.AddWithValue("p_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, result) =>
                    {
                        result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                        result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                        result.ComponentTypeId = (int)records.GetDecimalOrDefault("COMPONENT_TYPE");
                        result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                        result.LongPosn = records.GetInt16OrDefault("LAT_POSN"); 
                        result.ComponentSubTypeId = Convert.ToInt64(records["SUB_TYPE"]);
                    }
                );
            return listVehclRegObj;
        }
        public static List<VehicleRegistration> GetVehicleConfigRegistration(int vehicleId, string userSchema)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    listVehclRegObj,
                    userSchema + ".SP_APPL_GET_VEHICLE_ID",
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
        public static ConfigurationModel GetNotifVehicleConfigByVehID(int vehicleId, int isSimple)
        {
            ConfigurationModel VehicleConfig = new ConfigurationModel();
                //Setup Procedure View Configuration
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    VehicleConfig,
                    UserSchema.Portal+".GET_VEHICLE_CONFIGURATION",
                    parameter =>
                    {
                        parameter.AddWithValue("p_VHCL_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_Is_Simple", isSimple, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                            VehicleConfig.ConfigurationId = records.GetLongOrDefault("VEHICLE_ID");
                            VehicleConfig.ConfigurationTypeId = records.GetInt32OrDefault("VEHICLE_TYPE");
                            VehicleConfig.MaxHeight =  records.GetDoubleOrDefault("MAX_HEIGHT");
                            VehicleConfig.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                            VehicleConfig.ReducedHeight = (double)records.GetDecimalOrDefault("RED_HEIGHT_MTR");
                            VehicleConfig.RigidLength = records.GetDoubleOrDefault("RIGID_LEN");//swapped with LEN below
                            VehicleConfig.RigidLengthUnit = records.GetInt32OrDefault("LEN_UNIT");
                            VehicleConfig.Width = records.GetDoubleOrDefault("WIDTH");
                            VehicleConfig.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                            VehicleConfig.OverallLength = records.GetDoubleOrDefault("LEN");
                            VehicleConfig.OverallLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                            VehicleConfig.GrossWeight = records.GetDoubleOrDefault("GROSS_WEIGHT");
                            VehicleConfig.MaxAxleWeight = records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
                            VehicleConfig.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                            VehicleConfig.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                            VehicleConfig.NotifFrontOverhang = (double)records.GetDecimalOrDefault("FRONT_OVERHANG");
                            VehicleConfig.NotifRearOverhang = (double)records.GetDecimalOrDefault("REAR_OVERHANG");
                            VehicleConfig.NotifRightOverhang = (double)records.GetDecimalOrDefault("RIGHT_OVERHANG");
                            VehicleConfig.NotifLeftOverhang = (double)records.GetDecimalOrDefault("LEFT_OVERHANG");
                            if (isSimple == 1)
                            {
                                VehicleConfig.MaxReducedHeight = (double)records.GetDecimalOrDefault("MAX_RED_HGT");
                            }
                        }
                );
            return VehicleConfig;
        }
        public static bool CheckValidVehilceForImport(int VehID )
        {
            int output = 0;
            bool result = false;
                //Setup Procedure View Configuration
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    output,
                     UserSchema.Portal + ".SP_CHECK_VALID_IMPORT",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ConfigId", VehID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                            output = (int)records.GetDecimalOrDefault("VAR_RESULT");
                        }
                );
                if (output == 1)
                {
                    result = true;
                }
           return result;
        }
        #endregion
        #region Vehicle Components
        #region Function to obtain component general details
        public static ComponentModel GetRouteVehicleComponent(int componentId, string userSchema = UserSchema.Portal)
        {
            ComponentModel componentModelObj = new ComponentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                componentModelObj,
                userSchema + ".SP_APPL_GET_VEHICLE_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENTID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    componentModelObj.IntendedName = records.GetStringOrDefault("COMPONENT_NAME");
                    componentModelObj.FormalName = records.GetStringOrDefault("COMPONENT_SUMMARY");
                    componentModelObj.Description = records.GetStringOrDefault("COMPONENT_DESC");
                    componentModelObj.ComponentSubType = records.GetInt32OrDefault("COMPONENT_SUBTYPE");
                    componentModelObj.CouplingType = records.GetInt32OrDefault("COUPLING_TYPE");
                    componentModelObj.VehicleIntent = records.GetInt32OrDefault("VEHICLE_INTENT");
                    componentModelObj.ComponentType = records.GetInt32OrDefault("COMPONENT_TYPE");
                    componentModelObj.StandardCU = records.GetShortOrDefault("IS_STANDARD_C_AND_U");
                    componentModelObj.IsTracked = records.GetShortOrDefault("IS_TRACKED");
                    componentModelObj.IsSteerable = records.GetShortOrDefault("IS_STEERABLE_AT_REAR");
                    componentModelObj.MaxHeight = records.GetDoubleNullable("MAX_HEIGHT");
                    componentModelObj.MaxHeightUnit = records.GetInt32OrDefault("MAX_HEIGHT_UNIT");
                    componentModelObj.ReducableHeight = records.GetDoubleNullable("RED_HEIGHT");
                    componentModelObj.ReducableHeightUnit = records.GetInt32OrDefault("RED_HEIGHT_UNIT");
                    componentModelObj.RigidLength = records.GetDoubleNullable("RIGID_LEN");
                    componentModelObj.RigidLengthUnit = records.GetInt32OrDefault("RIGID_LEN_UNIT");
                    componentModelObj.Width = records.GetDoubleNullable("WIDTH");
                    componentModelObj.WidthUnit = records.GetInt32OrDefault("WIDTH_UNIT");
                    componentModelObj.WheelBase = records.GetDoubleNullable("WHEELBASE");
                    componentModelObj.WheelBaseUnit = records.GetInt32OrDefault("WHEELBASE_UNIT");
                    componentModelObj.LeftOverhang = records.GetDoubleNullable("LEFT_OVERHANG");
                    componentModelObj.LeftOverhangUnit = records.GetInt32OrDefault("LEFT_OVERHANG_UNIT");
                    componentModelObj.RightOverhang = records.GetDoubleNullable("RIGHT_OVERHANG");
                    componentModelObj.RightOverhangUnit = records.GetInt32OrDefault("RIGHT_OVERHANG_UNIT");
                    componentModelObj.FrontOverhang = records.GetDoubleNullable("FRONT_OVERHANG");
                    componentModelObj.FrontOverhangUnit = records.GetInt32OrDefault("FRONT_OVERHANG_UNIT");
                    componentModelObj.RearOverhang = records.GetDoubleNullable("REAR_OVERHANG");
                    componentModelObj.RearOverhangUnit = records.GetInt32OrDefault("REAR_OVERHANG_UNIT");
                    componentModelObj.GroundClearance = records.GetDoubleNullable("GROUND_CLEARANCE");
                    componentModelObj.GroundClearanceUnit = records.GetInt32OrDefault("GROUND_CLEARANCE_UNIT");
                    componentModelObj.OutsideTrack = records.GetDoubleNullable("OUTSIDE_TRACK");
                    componentModelObj.OutsideTrackUnit = records.GetInt32OrDefault("OUTSIDE_TRACK_UNIT");
                    componentModelObj.GrossWeight = records.GetDoubleNullable("GROSS_WEIGHT");
                    componentModelObj.GrossWeightUnit = records.GetInt32OrDefault("GROSS_WEIGHT_UNIT");
                    componentModelObj.MaxAxleWeight = records.GetDoubleNullable("MAX_AXLE_WEIGHT");
                    componentModelObj.MaxAxleWeightUnit = records.GetInt32OrDefault("MAX_AXLE_WEIGHT_UNIT");
                    componentModelObj.AxleWeightUnit = records.GetInt32OrDefault("AXLE_WEIGHT_UNIT");
                    componentModelObj.MaxAxleWeightUnit = records.GetInt32OrDefault("AXLE_SPACING_UNIT");
                    componentModelObj.WheelSpacingUnit = records.GetInt32OrDefault("WHEEL_SPACING_UNIT");
                    componentModelObj.SpacingToFollowing = records.GetDoubleNullable("SPACE_TO_FOLLOWING");
                    componentModelObj.SpacingToFollowingUnit = records.GetInt32OrDefault("SPACE_TO_FOLLOWING_UNIT");
                    componentModelObj.RedGroundClearance = records.GetDoubleNullable("RED_GROUND_CLEARANCE");
                    componentModelObj.RedGroundClearanceUnit = records.GetInt32OrDefault("RED_GROUND_CLEARANCE_UNIT");
                    componentModelObj.AxleCount = records.GetInt16Nullable("AXLE_COUNT")==null?0:records.GetInt16OrDefault("AXLE_COUNT");
                }
            );
            return componentModelObj;
        }
        #endregion
        #region Function to obtain component registration details
        public static List<VehicleRegistration> GetRouteComponentRegistration(int compId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".SP_APPL_SELECT_COMP_REG",
                parameter =>
                {
                    parameter.AddWithValue("p_COMP_ID", compId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Result_Set", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("license_plate");
                    result.FleetId = records.GetStringOrDefault("fleet_no");
                    result.IdNumber = records.GetInt16OrDefault("id_NO");
                }
            );
            return listVehclRegObj;
        }
        #endregion
        #region Function to obtain the component axle details
        internal static List<Axle> ListRouteComponentAxle(int componentId, string userSchema = UserSchema.Portal)
        {
            var axleC = new List<Axle>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                axleC,
                userSchema + ".SP_APPL_LIST_AXLE",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.AxleNumId = records.GetInt16OrDefault("AXLE_NO");
                    instance.AxleWeight = records.GetDoubleOrDefault("WEIGHT");
                    instance.TyreSize = records.GetStringOrDefault("TYRE_SIZE");
                    instance.DistanceToNextAxle = Convert.ToDouble(records["NEXT_AXLE_DIST"]);
                    instance.NoOfWheels = records.GetInt16OrDefault("WHEEL_COUNT");
                    instance.TyreCenters = records.GetStringOrDefault("WHEEL_SPACING_LIST");
                }
            );
            return axleC;
        }
        #endregion
        #endregion
        public static long CheckValidVehilceCreated(int vehicleId)
        {
            long output = 0;
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                output,
                UserSchema.Portal+".SP_VALID_NOTIF_VHCL_CREATED",
                parameter =>
                {
                    parameter.AddWithValue("p_ConfigId", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        output = (int)records.GetDecimalOrDefault("VAR_RESULT");
                    }
            );
            return output;
        }
        public static bool CheckWheelWithSumOfAxel(int VehID, string userSchema = UserSchema.Portal, int applnRev = 0, bool isNotif = false, bool isVR1 = false)
        {
            bool result = true;
            int data = 0;
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
            //Setup Procedure View Configuration
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_CHK_OVRLEN_EQLS_SUMAXEL",
                parameter =>
                {
                    parameter.AddWithValue("p_ConfigId", VehID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_APPLN_REV_ID", applnRev, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ISNOTIF", isNotifFlag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ISVR1", isVR1Flag, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        data = (int)records.GetDecimalOrDefault("VAR_RESULT");
                    }
            );
            if (data == 1)
            {
                result = false;
            }
            return result;
        }
        #region public static bool CheckNotifValidVehicleComp(int VehID)
        public static bool CheckNotifValidVehicleComp(int VehID)
        {
            long output = 0;
            bool result = false;
            try
            {
                //Setup Procedure View Configuration
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    output,
                   UserSchema.Portal + ".SP_VALID_NOTIF_COMP_CREATED",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ConfigId", VehID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        records =>
                        {
                            output = (int)records.GetDecimalOrDefault("VAR_RESULT");
                        }
                );
                if (output > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RouteVehicle/CheckNotifValidVehicleComp,  Exception: " + ex​​​​);
            }
            return result;
        }
        #endregion

    }
}