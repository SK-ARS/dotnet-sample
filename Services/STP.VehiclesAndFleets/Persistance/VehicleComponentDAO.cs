using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;

namespace STP.VehiclesAndFleets.Persistance
{
    public static class VehicleComponentDAO
    {
        #region Function for checking vehicle exists
        public static int CheckConfigurationExists(string vehicleName, int organisationId)
        {
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                UserSchema.Portal + ".SP_CHECK_CONFIG_NAME_EXISTS",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_NAME", vehicleName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_organisation_id", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                }
            );
            return count;
        }
        #endregion
        #region Create component in fleet managemnet
        public static double InsertComponent(ComponentModel componentModel)
        {
            double result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".INSERT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_TYPE", componentModel.ComponentType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_SUBTYPE", componentModel.ComponentSubType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", componentModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_INTENT", componentModel.VehicleIntent, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHOW_COMPONENT", componentModel.ShowComponent, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        //p_SHOW_COMPONENT
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("COMPONENT_ID");
                }
            );
            return result;
        }
        #endregion
        #region InsertAppVehicleComponent implementation
        public static double InsertAppVehicleComponent(ComponentModel componentModel, string userSchema = UserSchema.Portal)
        {
            double result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 userSchema + ".SP_APPL_INSERT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_TYPE", componentModel.ComponentType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_SUBTYPE", componentModel.ComponentSubType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_INTENT", componentModel.VehicleIntent, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                        //p_SHOW_COMPONENT
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("COMPONENT_ID");
                }
            );
            return result;
        }
        #endregion
        #region InsertVR1VehicleComponent implementation
        public static double InsertVR1VehicleComponent(ComponentModel componentModel, string userSchema = UserSchema.Portal)
        {
            double result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_TYPE", componentModel.ComponentType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", componentModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_INTENT", componentModel.VehicleIntent, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_SUBTYPE", componentModel.ComponentSubType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHOW_COMPONENT", componentModel.ShowComponent, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetLongOrDefault("COMPONENT_ID");
                }
            );
            return result;
        }
        #endregion
        #region For inserting the vehicle and component into vehicle position table with lat and long position as 1
        public static VehicleConfigList CreateVehicleConPosnForComp(VehicleConfigList configList)
        {
            VehicleConfigList InsertedConPos = new VehicleConfigList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               InsertedConPos,
                UserSchema.Portal + ".SP_INSERT_POSN_MAKE_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", configList.VehicleId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", configList.ComponentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LAT_POS", configList.LatPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LONG_POS", configList.LongPosn, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("P_COMPONENT_TYPE", configList.SubType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        #endregion
        #region Get component based on component id
        public static ComponentModel GetComponent(int componentId)
        {
            ComponentModel componentModelObj = new ComponentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                componentModelObj,
                UserSchema.Portal + ".GET_VEHICLE_COMPONENT",
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
                    componentModelObj.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                    componentModelObj.RedGroundClearance = records.GetDoubleNullable("RED_GROUND_CLEARANCE");
                    componentModelObj.RedGroundClearanceUnit = records.GetInt32OrDefault("RED_GROUND_CLEARANCE_UNIT");
                    componentModelObj.AxleCount = records.GetInt16Nullable("AXLE_COUNT");
                    componentModelObj.IsSteerable = records.GetInt16Nullable("IS_STEERABLE_AT_REAR");
                }
            );
            return componentModelObj;
        }
        #endregion
        # region Function to check configuration already created
        public static VehicleConfigList GetConfigByComponent(int componentId)
        {
            VehicleConfigList InsertedConPos = new VehicleConfigList();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
               InsertedConPos,
                UserSchema.Portal + ".SP_GET_VEHICLE_FROM_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    InsertedConPos.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
                    InsertedConPos.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                }
            );
            return InsertedConPos;
        }
        #endregion
        # region Function for getting registartion details
        public static List<VehicleRegistration> GetRegistration(int componentId)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                UserSchema.Portal + ".SELECT_COMP_REG",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("license_plate");
                    result.FleetId = records.GetStringOrDefault("fleet_no");
                    result.IdNumber = records.GetInt16OrDefault("id_no");
                }
            );
            return listVehclRegObj;
        }
        #endregion
        # region Function for getting VR1 registartion details
        public static List<VehicleRegistration> GetVR1Registration(int componentId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_SELECT_COMP_REG",
                parameter =>
                {
                    parameter.AddWithValue("p_COMP_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Result_Set", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("license_plate");
                    result.FleetId = records.GetStringOrDefault("fleet_no");
                    result.IdNumber = records.GetInt16OrDefault("id_no");
                }
            );
            return listVehclRegObj;
        }
        #endregion
        #region Function for getting application registartion details
        public static List<VehicleRegistration> GetApplRegistration(int componentId, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                userSchema + ".SELECT_APPL_COMP_REG",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("license_plate");
                    result.FleetId = records.GetStringOrDefault("fleet_no");
                    result.IdNumber = records.GetInt16OrDefault("id_no");
                }
            );
            return listVehclRegObj;
        }
        #endregion
        #region Function for create registration
        public static int CreateRegistration(int componentId, string registrationValue, string fleetId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".INSERT_COMPONENT_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        #region Function for create VR1 registration
        public static int CreateVR1CompRegistration(int componentId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_INS_COMP_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LICENCE_PLATE", registrationValue, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLEET_NO", fleetId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt16OrDefault("ID_No");
                }
            );
            return result;
        }
        #endregion
        #region Function for create application registration
        public static int CreateAppCompRegistration(int componentId, string registrationValue, string fleetId, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 userSchema + ".SP_APPL_INS_COMP_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        #region Update component in fleet managemnet
        public static bool UpdateComponent(ComponentModel componentModel)
        {
            bool result;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                UserSchema.Portal + ".SP_EDIT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", componentModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", componentModel.ComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
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
        #endregion
        #region CreateVehicleRegFromComp
        public static int CreateVehicleRegFromComp(int componentId, int vehicleId)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_CLONE_COMP_REG_TO_CONFIG",
                parameter =>
                {
                    parameter.AddWithValue("P_COMP_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONFIG_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                },
                records =>
                {
                    result = 1;
                }
            );
            return result;
        }
        #endregion
        #region Update VR1 vehicle component in fleet managemnet
        public static bool UpdateVR1VehComponent(ComponentModel componentModel, string userSchema = UserSchema.Portal)
        {
            bool result;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_EDIT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", componentModel.ComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
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
        #endregion
        #region Update application component
        public static bool UpdateAppVehComponent(ComponentModel componentModel, string userSchema = UserSchema.Portal)
        {
            bool result;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                userSchema + ".SP_APPL_EDIT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENT_ID", componentModel.ComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
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
        #endregion
        #region Getting component from route vehicle component table for VR1 appln vehicle
        public static ComponentModel GetVR1VehComponent(int componentId, string userSchema = UserSchema.Portal)
        {
            ComponentModel componentModelObj = new ComponentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                componentModelObj,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_GET_VEHICLE_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENTID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    componentModelObj.IntendedName = records.GetStringOrDefault("COMPONENT_SUMMARY");
                    componentModelObj.FormalName = records.GetStringOrDefault("COMPONENT_SUMMARY");
                    //componentModelObj.Description = records.GetStringOrDefault("COMPONENT_DESC");
                    componentModelObj.ComponentSubType = records.GetInt32OrDefault("COMPONENT_SUBTYPE");
                    componentModelObj.VehicleIntent = records.GetInt32OrDefault("VEHICLE_INTENT");
                    componentModelObj.ComponentType = records.GetInt32OrDefault("COMPONENT_TYPE");
                    componentModelObj.CouplingType = records.GetInt32OrDefault("COUPLING_TYPE");
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
                    componentModelObj.SpacingToFollowing = records.GetDoubleNullable("space_to_following");
                    componentModelObj.SpacingToFollowingUnit = records.GetInt32OrDefault("SPACE_TO_FOLLOWING_UNIT");
                    componentModelObj.RedGroundClearance = records.GetDoubleNullable("RED_GROUND_CLEARANCE");
                    componentModelObj.RedGroundClearanceUnit = records.GetInt32OrDefault("RED_GROUND_CLEARANCE_UNIT");
                    componentModelObj.AxleCount = records.GetInt16Nullable("AXLE_COUNT");
                    componentModelObj.IsSteerable = records.GetInt16Nullable("IS_STEERABLE_AT_REAR");
                }
            );
            return componentModelObj;
        }
        #endregion
        #region Getting component from application component
        public static ComponentModel GetApplVehComponent(int componentId, string userSchema = UserSchema.Portal)
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
                    componentModelObj.SpacingToFollowing = records.GetDoubleNullable("space_to_following");
                    componentModelObj.SpacingToFollowingUnit = records.GetInt32OrDefault("SPACE_TO_FOLLOWING_UNIT");
                    componentModelObj.RedGroundClearance = records.GetDoubleNullable("RED_GROUND_CLEARANCE");
                    componentModelObj.RedGroundClearanceUnit = records.GetInt32OrDefault("RED_GROUND_CLEARANCE_UNIT");
                    componentModelObj.AxleCount = records.GetInt16Nullable("AXLE_COUNT");
                    componentModelObj.IsSteerable = records.GetInt16Nullable("IS_STEERABLE_AT_REAR");
                }
            );
            return componentModelObj;
        }
        #endregion
        #region Insert axle details
        public static bool InsertAxleDetails(Axle axle, int componentId)
        {
            bool result = false;
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".INSERT_AXLE",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_NO", axle.AxleNumId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WEIGHT", axle.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYRE_SIZE", axle.TyreSize, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NEXT_AXLE_DIST", axle.DistanceToNextAxle, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_COUNT", axle.NoOfWheels, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_LIST", axle.TyreCenters, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            if (rowsAffected > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion
        #region Insert VR1 axle details
        public static bool InsertVR1VehAxleDetails(Axle axle, int componentId, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_INSERT_AXLE",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_NO", axle.AxleNumId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WEIGHT", axle.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYRE_SIZE", axle.TyreSize, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NEXT_AXLE_DIST", axle.DistanceToNextAxle, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_COUNT", axle.NoOfWheels, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_LIST", axle.TyreCenters, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
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
        #region Insert Appl axle details
        public static bool InsertAppVehAxleDetails(Axle axle, int componentId, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                userSchema + ".SP_APPL_INSERT_AXLE",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_NO", axle.AxleNumId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WEIGHT", axle.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYRE_SIZE", axle.TyreSize, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NEXT_AXLE_DIST", axle.DistanceToNextAxle, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_COUNT", axle.NoOfWheels, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_LIST", axle.TyreCenters, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
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
        #region List of axles from vehicle
        internal static List<Axle> ListAxle(int componentId)
        {
            var axleC = new List<Axle>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                axleC,
                UserSchema.Portal + ".LIST_AXLE",
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
        #region List of axles from vr1 app vehicle
        internal static List<Axle> ListVR1vehAxle(int componentId, string userSchema = UserSchema.Portal)
        {
            var axleC = new List<Axle>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                axleC,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_LIST_AXLE",
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
        #region List of axles from app vehicle
        internal static List<Axle> ListAppvehAxle(int componentId, string userSchema = UserSchema.Portal)
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
                    instance.DistanceToNextAxle = records.GetDoubleOrDefault("NEXT_AXLE_DIST");
                    instance.NoOfWheels = records.GetInt16OrDefault("WHEEL_COUNT");
                    instance.TyreCenters = records.GetStringOrDefault("WHEEL_SPACING_LIST");
                }
            );
            return axleC;
        }
        #endregion
        #region Delete vehicle component in fleet management
        public static int DeleteVehComponent(int componentId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".DELETE_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        #region Delete vehicle registration in component
        public static int DeleteRegistrationComponent(int componentId, int idNumber,int flag=0)
        {
            int rowsAffected = 0;
            string sp = UserSchema.Portal + ".DELETE_COMPONENT_ID";
            if (flag == 1)// delete reg from temp table
            {
                sp = UserSchema.Portal + ".STP_VEHICLE.DELETE_COMPONENT_ID_TEMP";
            } 
            else if (flag == 2)// delete reg from movement temp table
            {
                sp = UserSchema.Portal + ".STP_VEHICLE.DELETE_MVMT_COMPONENT_ID_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                sp,
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ID_NO", idNumber, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
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
        #region Delete VR1 application Component Registration 
        public static int DeleteVR1VehRegistrationComponent(int componentId, int idNumber, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_DELETE_COMPONENT_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ID_NO", idNumber, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
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
        #region  Delete Application Component Registration 
        public static int DeleteAppVehRegistrationComponent(int componentId, int idNumber, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                userSchema + ".SP_APPL_DELETE_COMPONENT_ID",
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ID_NO", idNumber, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
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
        //-------------------susmitha---------------------------------------------------------------------------------------
        public static List<ComponentGridList> GetAllVR1ComponentDetailsByID(long componentId, string userSchema = UserSchema.Portal)
        {
            List<ComponentGridList> componentGridObj = new List<ComponentGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                userSchema + ".STP_ROUTE_VEHICLES.SP_ROUTE_SELECT_COMP",
                parameter =>
                {
                    parameter.AddWithValue("P_COMP_ID", componentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Result_Set", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                        instance.ComponentName = records.GetStringOrDefault("COMPONENT_NAME");
                        instance.ComponentDescription = records.GetStringOrDefault("COMPONENT_DESC");
                        instance.AxleSpacing = (decimal)records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
                    }
            );
            return componentGridObj;
        }
        //function to select all component regardless of show flag
        public static List<ComponentGridList> GetAllAppComponentDetailsByID(long componentId, string userSchema = UserSchema.Portal)
        {
            List<ComponentGridList> componentGridObj = new List<ComponentGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                userSchema + ".SP_APPL_SELECT_COMP",
                parameter =>
                {
                    parameter.AddWithValue("P_COMP_ID", componentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Result_Set", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                        instance.ComponentName = records.GetStringOrDefault("COMPONENT_NAME");
                        instance.ComponentDescription = records.GetStringOrDefault("COMPONENT_SUMMARY");
                        instance.ComponentType = records.GetStringOrDefault("COMPONENT_DESC");
                        instance.AxleSpacing = (decimal)records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
                    }
            );
            return componentGridObj;
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
        public static List<ComponentGridList> GetComponentDetailsByID(int organisationId, int pageNumber, int pageSize, string componentName, string componentType, string vehicleIntent, int filterFavourites, string userSchema = UserSchema.Portal,int presetFilter=1,int? sortOrder=null)
        {
            List<ComponentGridList> componentGridObj = new List<ComponentGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentGridObj,
                userSchema + ".SELECT_COMPONENT",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMPONENT_NAME", componentName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMPONENT_TYPE", componentType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_INTENT", vehicleIntent, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FILTER_FAVOURITES_FLAG", filterFavourites, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
                        instance.FleetList = records.GetStringOrDefault("FLEET_ID");
                        instance.ComponentName = records.GetStringOrDefault("COMPONENT_NAME");
                        instance.ComponentDescription = records.GetStringOrDefault("COMPONENT_SUMMARY");
                        instance.VehicleIntent = records.GetStringOrDefault("VEHICLE_INTENT");
                        instance.ComponentType = records.GetStringOrDefault("COMPONENT_TYPE");
                        instance.TotalRecordCount = (long)Convert.ToInt32(records["TOTALRECORDCOUNT"]);
                        instance.IsFavourites =Convert.ToInt32(records["Is_Favourite"]);
                    }
            );
            return componentGridObj;
        }
        //-----------------------------------------------------------------------------------------------------------------------

        #region Vehicle workflow TEMP table implementation
        #region Insert component to TEMP table
        public static double InsertComponentToTemp(ComponentModel componentModel)
        {
            double result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_VEHICLE.INSERT_COMPONENT_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_GUID", componentModel.GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_TYPE", componentModel.ComponentType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_SUBTYPE", componentModel.ComponentSubType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", componentModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_VEHICLE_INTENT", componentModel.VehicleIntent, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHOW_COMPONENT", componentModel.ShowComponent, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    //p_SHOW_COMPONENT
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = Convert.ToDouble(records["COMPONENT_ID"]);
                }
            );
            return result;
        }
        #endregion

        #region Update component subtype to TEMP table
        public static int UpdateComponentSubTypeToTemp(ComponentModel componentModel)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".STP_VEHICLE.UPDATE_COMPONENT_SUBTYPE_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_ComponentId", componentModel.ComponentId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_TYPE", componentModel.ComponentType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMP_SUBTYPE", componentModel.ComponentSubType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_Affected_Rows", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = records.GetInt32("p_Affected_Rows");
                }
            );
            return result;
        }
        #endregion

        #region Insert registration to TEMP table
        public static int CreateRegistrationTemp(int componentId, string registrationValue, string fleetId, bool movement = false, string userSchema = UserSchema.Portal)
        {
            int result = 0;
            string sp = UserSchema.Portal + ".STP_VEHICLE.INSERT_COMPONENT_ID_TEMP";
            if (movement)
            {
                sp = userSchema + ".STP_VEHICLE.INSERT_MVMT_COMPONENT_ID_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                sp,
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENTID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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
        #region Insert axle details to TEMP table
        public static bool InsertAxleDetailsTemp(Axle axle, int componentId, bool movement=false, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            int rowsAffected = 0;
            string sp = UserSchema.Portal + ".STP_VEHICLE.INSERT_AXLE_TEMP";
            if (movement)
            {
                sp = userSchema + ".STP_VEHICLE.INSERT_MVMT_AXLE_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                sp,
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENTID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_NO", axle.AxleNumId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", 1, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WEIGHT", axle.AxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_TYRE_SIZE", axle.TyreSize, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_NEXT_AXLE_DIST", axle.DistanceToNextAxle, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_COUNT", axle.NoOfWheels, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_LIST", axle.TyreCenters, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            if (rowsAffected > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion
        #region Update component in TEMP table
        public static bool UpdateComponentTemp(ComponentModel componentModel)
        {
            bool result;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                UserSchema.Portal + ".STP_VEHICLE.SP_EDIT_COMPONENT_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ORGANISATION_ID", componentModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GUID", componentModel.GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENTID", componentModel.ComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    
                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
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
        #endregion
        #region Get component ids from TEMP table
        public static List<VehicleConfigList> GetComponentIdTemp(string GUID,int vehicleId, string userSchema = UserSchema.Portal)
        {
            List<VehicleConfigList> componentIdList = new List<VehicleConfigList>();
            if (vehicleId != 0)
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    componentIdList,
                    userSchema + ".STP_VEHICLE.SP_GET_MVMT_COMPONENT_IDS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_vehicleId", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.ComponentId = Convert.ToInt32(records["COMPONENT_ID"]);
                        instance.LatPosn = records.GetInt16OrDefault("LONG_POSN");
                        instance.LongPosn = records.GetInt16OrDefault("LAT_POSN");
                        instance.ComponentTypeId = Convert.ToInt32(records["COMPONENT_TYPE"]);
                        instance.ComponentSubTypeId = Convert.ToInt32(records["COMPONENT_SUBTYPE"]);
                    }
                );
            }
            else
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    componentIdList,
                    UserSchema.Portal + ".STP_VEHICLE.SP_GET_COMPONENT_IDS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_GUID", GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.ComponentId = Convert.ToInt16(records["COMPONENT_ID"]);
                        instance.ComponentTypeId = Convert.ToInt32(records["COMPONENT_TYPE"]);
                        instance.ComponentSubTypeId = Convert.ToInt32(records["COMPONENT_SUBTYPE"]);
                    }
                );
            }
            return componentIdList;
        }
        #endregion
        #region Get component details based on component id and guid from TEMP table
        public static ComponentModel GetComponentTemp(int componentId,string GUID, string userSchema = UserSchema.Portal)
        {
            ComponentModel componentModelObj = new ComponentModel();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                componentModelObj,
                userSchema + ".STP_VEHICLE.GET_VEHICLE_COMPONENT_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENTID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_GUID", GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
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
                    componentModelObj.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                    componentModelObj.RedGroundClearance = records.GetDoubleNullable("RED_GROUND_CLEARANCE");
                    componentModelObj.RedGroundClearanceUnit = records.GetInt32OrDefault("RED_GROUND_CLEARANCE_UNIT");
                    componentModelObj.AxleCount = records.GetInt16Nullable("AXLE_COUNT");
                    componentModelObj.IsSteerable = records.GetInt16Nullable("IS_STEERABLE_AT_REAR");
                }
            );
            return componentModelObj;
        }
        #endregion
        #region Function for getting registartion details from TEMP table
        public static List<VehicleRegistration> GetRegistrationTemp(int componentId,bool movement = false, string userSchema = UserSchema.Portal)
        {
            List<VehicleRegistration> listVehclRegObj = new List<VehicleRegistration>();
            string sp = UserSchema.Portal + ".STP_VEHICLE.SELECT_COMP_REG_TEMP";
            if (movement)
            {
                sp = userSchema + ".STP_VEHICLE.SELECT_MVMT_COMP_REG_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                listVehclRegObj,
                sp,
                parameter =>
                {
                    parameter.AddWithValue("p_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, result) =>
                {
                    result.RegistrationId = records.GetStringOrDefault("license_plate");
                    result.FleetId = records.GetStringOrDefault("fleet_no");
                    result.IdNumber = records.GetInt16OrDefault("id_no");
                }
            );
            return listVehclRegObj;
        }
        #endregion
        #region List of axles from component Temp
        public static List<Axle> ListAxleTemp(int componentId, bool movement = false, string userSchema = UserSchema.Portal)
        {
            var axleC = new List<Axle>();
            string sp = UserSchema.Portal + ".STP_VEHICLE.LIST_AXLE_TEMP";
            if (movement)
            {
                sp = userSchema + ".STP_VEHICLE.LIST_MVMT_AXLE_TEMP";
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                axleC,
                sp,
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
                    instance.DistanceToNextAxle = Convert.ToDouble(records["NEXT_AXLE_DIST"]); ;
                    instance.NoOfWheels = records.GetInt16OrDefault("WHEEL_COUNT");
                    instance.TyreCenters = records.GetStringOrDefault("WHEEL_SPACING_LIST");
                }
            );
            return axleC;
        }
        #endregion
        #region Insert component to vehicle config
        public static int InsertComponentConfigPosn(int componentId, int vehicleId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 "STP_VEHICLE.SP_COMPONENT_CONFIG_POSN",
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        rowsAffected = records.GetInt32("p_AFFECTED_ROWS");
                    }
            );
            return rowsAffected;
        }
        #endregion
        #region add to fleet using temp table
        public static int AddToFleetTemp(string GUID, int componentId, int vehicleId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_VEHICLE.SP_COMPONENT_ADD_TO_FLEET",
                parameter =>
                {
                    parameter.AddWithValue("P_GUID", GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        rowsAffected = records.GetInt32("p_AFFECTED_ROWS");
                    }
            );
            return rowsAffected;
        }
        #endregion
        #region Delete component from temp table
        public static int DeleteComponentTemp( int componentId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 "STP_VEHICLE.SP_DELETE_COMPONENT_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        rowsAffected = records.GetInt32("P_AFFECTED_ROWS");
                    }
            );
            return rowsAffected;
        }
        #endregion
        #region Delete component in config posn
        public static int DeleteComponentConfig( int componentId, int vehicleId, bool movement = false, string userSchema = UserSchema.Portal)
        {
            int rowsAffected = 0;
            string sp = "STP_VEHICLE.SP_DELETE_COMPONENTCONFIG";
            if (movement)
            {
                sp = userSchema + ".STP_VEHICLE.SP_DELETE_MVMT_COMPONENT_TEMP";
            }
            
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 sp,
                parameter =>
                {
                    parameter.AddWithValue("P_COMPONENT_ID", componentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                    records =>
                    {
                        rowsAffected = records.GetInt32("p_AFFECTED_ROWS");
                    }
            );
            return rowsAffected;
        }
        #endregion
        #region Get fav
        public static List<ComponentGridList> GetComponentFavourite(int organisationId,int movementId)
        {
            List<ComponentGridList> componentIdList = new List<ComponentGridList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                componentIdList,
                UserSchema.Portal + ".SELECT_COMPONENT_FAV",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVEMENT_ID", movementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ComponentId = Convert.ToInt32(records["COMPONENT_ID"]);
                    instance.ComponentName = Convert.ToString(records["COMPONENT_NAME"]);
                }
            );
            return componentIdList;
        }
        #endregion
        #region Update component in movement TEMP table
        public static bool UpdateMovementComponentTemp(ComponentModel componentModel, string userSchema = UserSchema.Portal)
        {
            bool result;
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                userSchema + ".STP_VEHICLE.SP_EDIT_MVMT_COMPONENT_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("p_NAME", componentModel.IntendedName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_DESCRIPTION", componentModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SUMMARY", componentModel.FormalName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COUPLING_TYPE", componentModel.CouplingType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STANDARD_C_AND_U", componentModel.StandardCU, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_TRACKED", componentModel.IsTracked, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_IS_STEERABLE_AT_REAR", componentModel.IsSteerable, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT", componentModel.MaxHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_HEIGHT_UNIT", componentModel.MaxHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT", componentModel.ReducableHeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_HEIGHT_UNIT", componentModel.ReducableHeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN", componentModel.RigidLength, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGID_LEN_UNIT", componentModel.RigidLengthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH", componentModel.Width, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WIDTH_UNIT", componentModel.WidthUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE", componentModel.WheelBase, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEELBASE_UNIT", componentModel.WheelBaseUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG", componentModel.LeftOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_LEFT_OVERHANG_UNIT", componentModel.LeftOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG", componentModel.RightOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RIGHT_OVERHANG_UNIT", componentModel.RightOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG", componentModel.FrontOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FRONT_OVERHANG_UNIT", componentModel.FrontOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG", componentModel.RearOverhang, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_REAR_OVERHANG_UNIT", componentModel.RearOverhangUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE", componentModel.GroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROUND_CLEARANCE_UNIT", componentModel.GroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK", componentModel.OutsideTrack, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_OUTSIDE_TRACK_UNIT", componentModel.OutsideTrackUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT", componentModel.GrossWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_GROSS_WEIGHT_UNIT", componentModel.GrossWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT", componentModel.MaxAxleWeight, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MAX_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_COUNT", componentModel.AxleCount, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_WEIGHT_UNIT", componentModel.MaxAxleWeightUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AXLE_SPACING_UNIT", componentModel.AxleSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_WHEEL_SPACING_UNIT", componentModel.WheelSpacingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("p_ORGANISATION_ID", componentModel.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE", componentModel.RedGroundClearance, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_RED_GROUND_CLEARANCE_UNIT", componentModel.RedGroundClearanceUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("p_GUID", componentModel.GUID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_COMPONENTID", componentModel.ComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_SPACE_TO_FOLLOW", componentModel.SpacingToFollowing, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SPACE_UNIT", componentModel.SpacingToFollowingUnit, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
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
        #endregion
        #endregion

        #region Save All Components
        public static long SaveVehicleComponents(VehicleConfigDetail vehicleDetail, string userSchema = UserSchema.Portal)
        {
            long result = 0;

            vehicleDetail.componentList = ProcessVehicleComponent(vehicleDetail.componentList);
            VehicleComponentModelArray componentObj = new VehicleComponentModelArray()
            {
                ComponentObj = vehicleDetail.componentList.ToArray()
            };
            int flag = vehicleDetail.Flag;
            if (vehicleDetail.Flag == 2&& userSchema== UserSchema.Sort)
            {
                flag = 4;
            }
            OracleCommand cmd = new OracleCommand();
            OracleParameter componentObjParam = cmd.CreateParameter();
            componentObjParam.OracleDbType = OracleDbType.Object;
            componentObjParam.UdtTypeName = "PORTAL.COMPONENTSARRAY";
            componentObjParam.Value = vehicleDetail.componentList.Count > 0 ? componentObj : null;
            componentObjParam.ParameterName = "P_COMPONENT_ARRAY";
            //Function needs to be updated with SP variables
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_VEHICLE.SP_SAVE_VEHICLE_COMPONENTS",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleDetail.ConfigurationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVEMENT_TYPE_ID", vehicleDetail.MovementTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLAG", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(componentObjParam);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = record.GetInt32("p_AFFECTED_ROWS");
                });

            return result;
        }


        private static List<VehicleComponentModel> ProcessVehicleComponent(List<VehicleComponentModel> componentList)
        {
            componentList.ForEach(
                component =>
                {
                    component.ComponentAxleArray.ComponentAxleObj = (component.ComponentAxleList.Count > 0) ? component.ComponentAxleList.ToArray() : null;
                    component.ComponentRegistrationArray.ComponentRegistrationObj = (component.ComponentRegistrationList.Count > 0) ? component.ComponentRegistrationList.ToArray() : null;
                });
            return componentList;
        }
        #endregion

        #region Update All Components
        public static long UpdateVehicleComponents(VehicleConfigDetail vehicleDetail, string userSchema = UserSchema.Portal)
        {
            long result = 0;

            vehicleDetail.componentList = ProcessVehicleComponent(vehicleDetail.componentList);
            VehicleComponentModelArray componentObj = new VehicleComponentModelArray()
            {
                ComponentObj = vehicleDetail.componentList.ToArray()
            };

            int flag = vehicleDetail.Flag;
            if (vehicleDetail.Flag == 2 && userSchema == UserSchema.Sort)
            {
                flag = 4;
            }
            OracleCommand cmd = new OracleCommand();
            OracleParameter componentObjParam = cmd.CreateParameter();
            componentObjParam.OracleDbType = OracleDbType.Object;
            componentObjParam.UdtTypeName = "PORTAL.COMPONENTSARRAY";
            componentObjParam.Value = vehicleDetail.componentList.Count > 0 ? componentObj : null;
            componentObjParam.ParameterName = "P_COMPONENT_ARRAY";
            //Function needs to be updated with SP variables
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_VEHICLE.SP_UPDATE_VEHICLE_COMPONENTS",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", vehicleDetail.ConfigurationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_MOVEMENT_TYPE_ID", vehicleDetail.MovementTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLAG", flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.Add(componentObjParam);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = record.GetInt32("p_AFFECTED_ROWS");
                });

            return result;
        }

        #endregion

        #region Function for checking component exists
        public static int CheckComponentInternalnameExists(string componentName, int organisationId)
        {
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                UserSchema.Portal + ".STP_VEHICLE.SP_CHECK_COMPONENT_NAME_EXISTS",
                parameter =>
                {
                    parameter.AddWithValue("p_component_name", componentName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_organisation_id", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = Convert.ToInt32(record.GetDecimalOrDefault("COUNT"));
                }
            );
            return count;
        }
        #endregion

        #region Function for add component to fleet
        public static long AddComponentToFleet(List<VehicleComponentModel> componentList)
        {
            long result = 0;

            componentList = ProcessVehicleComponent(componentList);
            VehicleComponentModelArray componentObj = new VehicleComponentModelArray()
            {
                ComponentObj = componentList.ToArray()
            };
            OracleCommand cmd = new OracleCommand();
            OracleParameter componentObjParam = cmd.CreateParameter();
            componentObjParam.OracleDbType = OracleDbType.Object;
            componentObjParam.UdtTypeName = "PORTAL.COMPONENTSARRAY";
            componentObjParam.Value = componentList.Count > 0 ? componentObj : null;
            componentObjParam.ParameterName = "P_COMPONENT_ARRAY";
            //Function needs to be updated with SP variables
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                 UserSchema.Portal + ".STP_VEHICLE.SP_ADD_COMPONENT_TO_FLEET",
                parameter =>
                {
                    parameter.Add(componentObjParam);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = record.GetInt32("p_AFFECTED_ROWS");
                });

            return result;
        }
        #endregion

        #region Function to update axle count for semi trailer - conventional tractor
        public static int UpdateConventionalTractorAxleCount(int axleCount, int vehicleId, int fromComponentId, int toComponentId, string userSchema)
        {
            int count = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                count,
                userSchema + ".STP_VEHICLE.SP_REASSIGN_AXLES_TEMP",
                parameter =>
                {
                    parameter.AddWithValue("P_VEHICLE_ID", vehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FROM_COMPONENT", fromComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_COMPONENT", toComponentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AXLE_COUNT", axleCount, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    count = Convert.ToInt32(record.GetDecimalOrDefault("FLAG"));
                }
            );
            return count;
        }
        #endregion
    }
}