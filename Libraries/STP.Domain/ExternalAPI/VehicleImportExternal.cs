using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using STP.Domain.NonESDAL;

namespace STP.Domain.ExternalAPI
{
    public class VehicleImportModelExternal
    {
        #region Vehicle
        public partial class VehicleImportModel
        {
            public string UnitSystem { get; set; }
            public VehicleConfigDetails VehicleConfigDetails { get; set; }
            public List<VehicleComponentDetails> VehicleComponentDetails { get; set; }
            public string RouteName { get; set; }
            public long MovementId { get; set; }
            public ValidationError VehicleError { get; set; }
        }
        #endregion

        #region Vehicle Configuration
        [DataContract]
        public partial class VehicleConfigDetails : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("VEHICLE_DESC")]
            public string FormalName { get; set; }
            [DataMember]
            [OracleObjectMapping("VEHICLE_NAME")]
            public string InternalName { get; set; }
            [DataMember]
            [OracleObjectMapping("VEHICLE_INT_DESC")]
            public string Description { get; set; }
            [DataMember]
            [OracleObjectMapping("VEHICLE_PURPOSE")]
            public int MovementClassification { get; set; }
            [DataMember]
            [OracleObjectMapping("VEHICLE_TYPE")]
            public int VehicleType { get; set; }
            [DataMember]
            [OracleObjectMapping("LEN")]
            public double? OverallLength { get; set; }
            [DataMember]
            [OracleObjectMapping("LEN_UNIT")]
            public int LengthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("LEN_MTR")]
            public double? OverallLengthMtr { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN")]
            public double? RigidLength { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN_UNIT")]
            public int RigidLengthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN_MTR")]
            public double? RigidLengthMtr { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH")]
            public double? Width { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH_UNIT")]
            public int WidthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH_MTR")]
            public double? WidthMtr { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT")]
            public double? MaxHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT_UNIT")]
            public int MaxHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT_MTR")]
            public double? MaxHeightMtr { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT")]
            public double? GrossWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT_UNIT")]
            public int GrossWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT_KG")]
            public double? GrossWeightKg { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT")]
            public double? MaxAxleWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT_UNIT")]
            public int MaxAxleWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT_KG")]
            public double? MaxAxleWeightKg { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE")]
            public double? WheelBase { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE_UNIT")]
            public int WheelBaseUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("SPEED")]
            public double? Speed { get; set; }
            [DataMember]
            [OracleObjectMapping("SPEED_UNIT")]
            public int SpeedUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("SIDE_BY_SIDE")]
            public double? TyreSpacing { get; set; }
            [DataMember]
            [OracleObjectMapping("SIDE_BY_SIDE_UNIT")]
            public int TyreSpacingUnit { get; set; }
            [DataMember]
            public List<Registrations> VehicleRegistration { get; set; }
            [DataMember]
            [OracleObjectMapping("CONFIG_REG_IMPORT")]
            public RegistrationsArray VehicleRegistrationArray { get; set; }
            public ValidationError VehicleConfigError { get; set; }

            public VehicleConfigDetails()
            {
                VehicleRegistration = new List<Registrations>();
                VehicleRegistrationArray = new RegistrationsArray();
            }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static VehicleConfigDetails Null
            {
                get
                {
                    VehicleConfigDetails p = new VehicleConfigDetails
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "VEHICLE_DESC", FormalName);
                OracleUdt.SetValue(con, pUdt, "VEHICLE_NAME", InternalName);
                OracleUdt.SetValue(con, pUdt, "VEHICLE_INT_DESC", Description);
                OracleUdt.SetValue(con, pUdt, "VEHICLE_PURPOSE", MovementClassification);
                OracleUdt.SetValue(con, pUdt, "VEHICLE_TYPE", VehicleType);
                OracleUdt.SetValue(con, pUdt, "LEN", OverallLength);
                OracleUdt.SetValue(con, pUdt, "LEN_UNIT", LengthUnit);
                OracleUdt.SetValue(con, pUdt, "LEN_MTR", OverallLengthMtr);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN", RigidLength);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN_UNIT", RigidLengthUnit);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN_MTR", RigidLengthMtr);
                OracleUdt.SetValue(con, pUdt, "WIDTH", Width);
                OracleUdt.SetValue(con, pUdt, "WIDTH_UNIT", WidthUnit);
                OracleUdt.SetValue(con, pUdt, "WIDTH_MTR", WidthMtr);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT", MaxHeight);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT_UNIT", MaxHeightUnit);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT_MTR", MaxHeightMtr);
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT", GrossWeight);
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT_UNIT", GrossWeightUnit);
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT_KG", GrossWeightKg);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT", MaxAxleWeight);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT_UNIT", MaxAxleWeightUnit);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT_KG", MaxAxleWeightKg);
                OracleUdt.SetValue(con, pUdt, "WHEELBASE", WheelBase);
                OracleUdt.SetValue(con, pUdt, "WHEELBASE_UNIT", WheelBaseUnit);
                OracleUdt.SetValue(con, pUdt, "SPEED", Speed);
                OracleUdt.SetValue(con, pUdt, "SPEED_UNIT", SpeedUnit);
                OracleUdt.SetValue(con, pUdt, "SIDE_BY_SIDE", TyreSpacing);
                OracleUdt.SetValue(con, pUdt, "SIDE_BY_SIDE_UNIT", TyreSpacingUnit);
                OracleUdt.SetValue(con, pUdt, "CONFIG_REG_IMPORT", VehicleRegistrationArray);
            }
            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                FormalName = (string)OracleUdt.GetValue(con, pUdt, "VEHICLE_DESC");
                InternalName = (string)OracleUdt.GetValue(con, pUdt, "VEHICLE_NAME");
                Description = (string)OracleUdt.GetValue(con, pUdt, "VEHICLE_INT_DESC");
                MovementClassification = (int)OracleUdt.GetValue(con, pUdt, "VEHICLE_PURPOSE");
                VehicleType = (int)OracleUdt.GetValue(con, pUdt, "VEHICLE_TYPE");
                OverallLength = (double?)OracleUdt.GetValue(con, pUdt, "LEN");
                LengthUnit = (int)OracleUdt.GetValue(con, pUdt, "LEN_UNIT");
                OverallLengthMtr = (double?)OracleUdt.GetValue(con, pUdt, "LEN_MTR");
                RigidLength = (double?)OracleUdt.GetValue(con, pUdt, "RIGID_LEN");
                RigidLengthUnit = (int)OracleUdt.GetValue(con, pUdt, "RIGID_LEN_UNIT");
                RigidLengthMtr = (double?)OracleUdt.GetValue(con, pUdt, "RIGID_LEN_MTR");
                Width = (double?)OracleUdt.GetValue(con, pUdt, "WIDTH");
                WidthUnit = (int)OracleUdt.GetValue(con, pUdt, "WIDTH_UNIT");
                WidthMtr = (double?)OracleUdt.GetValue(con, pUdt, "WIDTH_MTR");
                MaxHeight = (double?)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT");
                MaxHeightUnit = (int)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT_UNIT");
                MaxHeightMtr = (double?)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT_MTR");
                GrossWeight = (double?)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT");
                GrossWeightUnit = (int)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT_UNIT");
                GrossWeightKg = (double?)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT_KG");
                MaxAxleWeight = (double?)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT");
                MaxAxleWeightUnit = (int)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT_UNIT");
                MaxAxleWeightKg = (double?)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT_KG");
                WheelBase = (double?)OracleUdt.GetValue(con, pUdt, "WHEELBASE");
                WheelBaseUnit = (int)OracleUdt.GetValue(con, pUdt, "WHEELBASE_UNIT");
                Speed = (double?)OracleUdt.GetValue(con, pUdt, "SPEED");
                SpeedUnit = (int)OracleUdt.GetValue(con, pUdt, "SPEED_UNIT");
                TyreSpacing = (double?)OracleUdt.GetValue(con, pUdt, "SIDE_BY_SIDE");
                TyreSpacingUnit = (int)OracleUdt.GetValue(con, pUdt, "SIDE_BY_SIDE_UNIT");
                VehicleRegistrationArray = (RegistrationsArray)OracleUdt.GetValue(con, pUdt, "REGISTRATION_IMPORT_ARRAY");
            }
        }
        [OracleCustomTypeMapping("PORTAL.CONFIG_IMPORT")]
        public class VehicleConfigDetailsFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new VehicleConfigDetails();
            }
        }
        public class VehicleConfigDetailsArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public VehicleConfigDetails[] VehicleConfigDetailObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static VehicleConfigDetailsArray Null
            {
                get
                {
                    VehicleConfigDetailsArray p = new VehicleConfigDetailsArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, VehicleConfigDetailObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                VehicleConfigDetailObj = (VehicleConfigDetails[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.CONFIG_IMPORT_ARRAY")]
        public class VehicleConfigDetailsArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new VehicleConfigDetailsArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new VehicleConfigDetails[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new VehicleConfigDetails[numElems];
            }
            #endregion
        }
        #endregion

        #region Components
        public partial class VehicleComponentDetails : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("COMPONENT_SUMMARY")]
            public string FormalName { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_NAME")]
            public string InternalName { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_DESC")]
            public string Description { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_TYPE")]
            public int ComponentType { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_SUBTYPE")]
            public int ComponentSubType { get; set; }
            [DataMember]
            [OracleObjectMapping("COUPLING_TYPE")]
            public int CouplingType { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_STEERABLE_AT_REAR")]
            public int RearSteerable { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT")]
            public double? MaxHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT_UNIT")]
            public int MaxHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_HEIGHT")]
            public double? ReducibleHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_HEIGHT_UNIT")]
            public int ReducibleHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN")]
            public double? RigidLength { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN_UNIT")]
            public int RigidLengthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH")]
            public double? Width { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH_UNIT")]
            public int WidthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE")]
            public double? WheelBase { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE_UNIT")]
            public int WheelBaseUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("LEFT_OVERHANG")]
            public double? LeftOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("LEFT_OVERHANG_UNIT")]
            public int LeftOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGHT_OVERHANG")]
            public double? RightOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGHT_OVERHANG_UNIT")]
            public int RightOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("FRONT_OVERHANG")]
            public double? FrontOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("FRONT_OVERHANG_UNIT")]
            public int FrontOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("REAR_OVERHANG")]
            public double? RearOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("REAR_OVERHANG_UNIT")]
            public int RearOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROUND_CLEARANCE")]
            public double? GroundClearance { get; set; }
            [DataMember]
            [OracleObjectMapping("GROUND_CLEARANCE_UNIT")]
            public int GroundClearanceUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("OUTSIDE_TRACK")]
            public double? OutsideTrack { get; set; }
            [DataMember]
            [OracleObjectMapping("OUTSIDE_TRACK_UNIT")]
            public int OutsideTrackUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_COUNT")]
            public int? AxleCount { get; set; }
            [DataMember]
            [OracleObjectMapping("SPACE_TO_FOLLOWING")]
            public double? AxleSpacingToFollowing { get; set; }
            [DataMember]
            [OracleObjectMapping("SPACE_TO_FOLLOWING_UNIT")]
            public double? AxleSpacingToFollowingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_GROUND_CLEARANCE")]
            public double? ReducibleGroundClearance { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_GROUND_CLEARANCE_UNIT")]
            public int ReducibleGroundClearanceUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT")]
            public double? GrossWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT_UNIT")]
            public int GrossWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT")]
            public double? MaxAxleWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT_UNIT")]
            public int MaxAxleWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("COMP_REG_IMPORT_ARRAY")]
            public RegistrationsArray ComponentRegistrationArray { get; set; }
            public List<Registrations> ComponentRegistration { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLES_ARRAY")]
            public AxlesArray AxleArray { get; set; }
            [DataMember]
            public List<Axles> Axles { get; set; }
            public ValidationError VehicleComponentError { get; set; }
            public VehicleComponentDetails()
            {
                ComponentRegistration = new List<Registrations>();
                ComponentRegistrationArray = new RegistrationsArray();
                Axles = new List<Axles>();
                AxleArray = new AxlesArray();
            }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static VehicleComponentDetails Null
            {
                get
                {
                    VehicleComponentDetails p = new VehicleComponentDetails
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "COMPONENT_SUMMARY", FormalName);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_NAME", InternalName);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_DESC", Description);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_TYPE", ComponentType);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_SUBTYPE", ComponentSubType);
                OracleUdt.SetValue(con, pUdt, "COUPLING_TYPE", CouplingType);
                OracleUdt.SetValue(con, pUdt, "IS_STEERABLE_AT_REAR", RearSteerable);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT", MaxHeight);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT_UNIT", MaxHeightUnit);
                OracleUdt.SetValue(con, pUdt, "RED_HEIGHT", ReducibleHeight);
                OracleUdt.SetValue(con, pUdt, "RED_HEIGHT_UNIT", RigidLengthUnit);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN", RigidLength);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN_UNIT", ReducibleHeightUnit);
                OracleUdt.SetValue(con, pUdt, "WIDTH", Width);
                OracleUdt.SetValue(con, pUdt, "WIDTH_UNIT", WidthUnit);
                OracleUdt.SetValue(con, pUdt, "WHEELBASE", WheelBase);
                OracleUdt.SetValue(con, pUdt, "WHEELBASE_UNIT", WheelBaseUnit);
                OracleUdt.SetValue(con, pUdt, "LEFT_OVERHANG", LeftOverhang);
                OracleUdt.SetValue(con, pUdt, "LEFT_OVERHANG_UNIT", LeftOverhangUnit);
                OracleUdt.SetValue(con, pUdt, "RIGHT_OVERHANG", RightOverhang);
                OracleUdt.SetValue(con, pUdt, "RIGHT_OVERHANG_UNIT", RightOverhangUnit);
                OracleUdt.SetValue(con, pUdt, "FRONT_OVERHANG", FrontOverhang);
                OracleUdt.SetValue(con, pUdt, "FRONT_OVERHANG_UNIT", FrontOverhangUnit);
                OracleUdt.SetValue(con, pUdt, "REAR_OVERHANG", RearOverhang);
                OracleUdt.SetValue(con, pUdt, "REAR_OVERHANG_UNIT", RearOverhangUnit);
                OracleUdt.SetValue(con, pUdt, "GROUND_CLEARANCE", GroundClearance);
                OracleUdt.SetValue(con, pUdt, "GROUND_CLEARANCE_UNIT", GroundClearanceUnit);
                OracleUdt.SetValue(con, pUdt, "OUTSIDE_TRACK", OutsideTrack);
                OracleUdt.SetValue(con, pUdt, "OUTSIDE_TRACK_UNIT", OutsideTrackUnit);
                OracleUdt.SetValue(con, pUdt, "AXLE_COUNT", AxleCount);
                OracleUdt.SetValue(con, pUdt, "SPACE_TO_FOLLOWING", AxleSpacingToFollowing);
                OracleUdt.SetValue(con, pUdt, "SPACE_TO_FOLLOWING_UNIT", AxleSpacingToFollowingUnit);
                OracleUdt.SetValue(con, pUdt, "RED_GROUND_CLEARANCE", ReducibleGroundClearance);
                OracleUdt.SetValue(con, pUdt, "RED_GROUND_CLEARANCE_UNIT", ReducibleGroundClearanceUnit);
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT", GrossWeight);
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT_UNIT", GrossWeightUnit);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT", MaxAxleWeight);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT_UNIT", MaxAxleWeightUnit);
                OracleUdt.SetValue(con, pUdt, "COMP_REG_IMPORT_ARRAY", ComponentRegistrationArray);
                OracleUdt.SetValue(con, pUdt, "AXLES_ARRAY", AxleArray);
            }
            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                FormalName = (string)OracleUdt.GetValue(con, pUdt, "COMPONENT_SUMMARY");
                InternalName = (string)OracleUdt.GetValue(con, pUdt, "COMPONENT_NAME");
                Description = (string)OracleUdt.GetValue(con, pUdt, "COMPONENT_DESC");
                ComponentType = (int)OracleUdt.GetValue(con, pUdt, "COMPONENT_TYPE");
                ComponentSubType = (int)OracleUdt.GetValue(con, pUdt, "COMPONENT_SUBTYPE");
                CouplingType = (int)OracleUdt.GetValue(con, pUdt, "COUPLING_TYPE");
                RearSteerable = (int)OracleUdt.GetValue(con, pUdt, "IS_STEERABLE_AT_REAR");
                MaxHeight = (double?)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT");
                MaxHeightUnit = (int)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT_UNIT");
                ReducibleHeight = (double?)OracleUdt.GetValue(con, pUdt, "RED_HEIGHT");
                ReducibleHeightUnit = (int)OracleUdt.GetValue(con, pUdt, "RED_HEIGHT_UNIT");
                RigidLength = (double?)OracleUdt.GetValue(con, pUdt, "RIGID_LEN");
                RigidLengthUnit = (int)OracleUdt.GetValue(con, pUdt, "RIGID_LEN_UNIT");
                Width = (double?)OracleUdt.GetValue(con, pUdt, "WIDTH");
                WidthUnit = (int)OracleUdt.GetValue(con, pUdt, "WIDTH_UNIT");
                WheelBase = (double?)OracleUdt.GetValue(con, pUdt, "WHEELBASE");
                WheelBaseUnit = (int)OracleUdt.GetValue(con, pUdt, "WHEELBASE_UNIT");
                LeftOverhang = (double?)OracleUdt.GetValue(con, pUdt, "LEFT_OVERHANG");
                LeftOverhangUnit = (int)OracleUdt.GetValue(con, pUdt, "LEFT_OVERHANG_UNIT");
                RightOverhang = (double?)OracleUdt.GetValue(con, pUdt, "RIGHT_OVERHANG");
                RightOverhangUnit = (int)OracleUdt.GetValue(con, pUdt, "RIGHT_OVERHANG_UNIT");
                FrontOverhang = (double?)OracleUdt.GetValue(con, pUdt, "FRONT_OVERHANG");
                FrontOverhangUnit = (int)OracleUdt.GetValue(con, pUdt, "FRONT_OVERHANG_UNIT");
                RearOverhang = (double?)OracleUdt.GetValue(con, pUdt, "REAR_OVERHANG");
                RearOverhangUnit = (int)OracleUdt.GetValue(con, pUdt, "REAR_OVERHANG_UNIT");
                GroundClearance = (double?)OracleUdt.GetValue(con, pUdt, "GROUND_CLEARANCE");
                GroundClearanceUnit = (int)OracleUdt.GetValue(con, pUdt, "GROUND_CLEARANCE_UNIT");
                OutsideTrack = (double?)OracleUdt.GetValue(con, pUdt, "OUTSIDE_TRACK");
                OutsideTrackUnit = (int)OracleUdt.GetValue(con, pUdt, "OUTSIDE_TRACK_UNIT");
                AxleCount = (int?)OracleUdt.GetValue(con, pUdt, "AXLE_COUNT");
                AxleSpacingToFollowing = (double?)OracleUdt.GetValue(con, pUdt, "SPACE_TO_FOLLOWING");
                AxleSpacingToFollowingUnit = (int)OracleUdt.GetValue(con, pUdt, "SPACE_TO_FOLLOWING_UNIT");
                ReducibleGroundClearance = (double?)OracleUdt.GetValue(con, pUdt, "RED_GROUND_CLEARANCE");
                ReducibleGroundClearanceUnit = (int)OracleUdt.GetValue(con, pUdt, "RED_GROUND_CLEARANCE_UNIT");
                GrossWeight = (double?)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT");
                GrossWeightUnit = (int)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT_UNIT");
                MaxAxleWeight = (double?)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT");
                MaxAxleWeightUnit = (int)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT_UNIT");
                ComponentRegistrationArray = (RegistrationsArray)OracleUdt.GetValue(con, pUdt, "COMP_REG_IMPORT_ARRAY");
                AxleArray = (AxlesArray)OracleUdt.GetValue(con, pUdt, "AXLES_ARRAY");
            }
        }
        [OracleCustomTypeMapping("PORTAL.COMPONENT_IMPORT")]
        public class VehicleComponentModelFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new VehicleComponentDetails();
            }
        }
        public class VehicleComponentDetailsArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public VehicleComponentDetails[] ComponentObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static VehicleComponentDetailsArray Null
            {
                get
                {
                    VehicleComponentDetailsArray p = new VehicleComponentDetailsArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, ComponentObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                ComponentObj = (VehicleComponentDetails[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.COMPONENT_IMPORT_ARRAY")]
        public class VehicleComponentModelArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new VehicleComponentDetailsArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new VehicleComponentDetails[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new VehicleComponentDetails[numElems];
            }
            #endregion
        }
        #endregion

        #region Axles
        public partial class Axles : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("AXLE_NO")]
            public int AxleNumber { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEEL_COUNT")]
            public int NoOfWheels { get; set; }
            [DataMember]
            [OracleObjectMapping("WEIGHT")]
            public double AxleWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("NEXT_AXLE_DIST")]
            public double DistanceToNextAxle { get; set; }
            [DataMember]
            [OracleObjectMapping("TYRE_SIZE")]
            public string TyreSize { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEEL_SPACING_LIST")]
            public string TyreSpacing { get; set; }

            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static Axles Null
            {
                get
                {
                    Axles p = new Axles
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "AXLE_NO", AxleNumber);
                OracleUdt.SetValue(con, pUdt, "WEIGHT", AxleWeight);
                OracleUdt.SetValue(con, pUdt, "TYRE_SIZE", TyreSize);
                OracleUdt.SetValue(con, pUdt, "NEXT_AXLE_DIST", DistanceToNextAxle);
                OracleUdt.SetValue(con, pUdt, "WHEEL_COUNT", NoOfWheels);
                OracleUdt.SetValue(con, pUdt, "WHEEL_SPACING_LIST", TyreSpacing);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                AxleNumber = (int)OracleUdt.GetValue(con, pUdt, "AXLE_NO");
                AxleWeight = (double)OracleUdt.GetValue(con, pUdt, "WEIGHT");
                TyreSize = (string)OracleUdt.GetValue(con, pUdt, "TYRE_SIZE");
                DistanceToNextAxle = (double)OracleUdt.GetValue(con, pUdt, "NEXT_AXLE_DIST");
                NoOfWheels = (int)OracleUdt.GetValue(con, pUdt, "WHEEL_COUNT");
                TyreSpacing = (string)OracleUdt.GetValue(con, pUdt, "WHEEL_SPACING_LIST");
            }
        }
        [OracleCustomTypeMapping("PORTAL.AXLE_IMPORT")]
        public class AxlesFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new Axles();
            }
        }
        public class AxlesArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public Axles[] AxlesObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static AxlesArray Null
            {
                get
                {
                    AxlesArray p = new AxlesArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, AxlesObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                AxlesObj = (Axles[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.AXLE_IMPORT_ARRAY")]
        public class ComponentAxleArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new AxlesArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new Axles[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new Axles[numElems];
            }
            #endregion
        }
        #endregion

        #region Registration
        public class Registrations : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("ID_NO")]
            public int SerialNumber { get; set; }
            [DataMember]
            [OracleObjectMapping("FLEET_NO")]
            public string FleetId { get; set; }
            [DataMember]
            [OracleObjectMapping("LICENSE_PLATE")]
            public string Registration { get; set; }

            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static Registrations Null
            {
                get
                {
                    Registrations p = new Registrations
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "ID_NO", SerialNumber);
                OracleUdt.SetValue(con, pUdt, "LICENSE_PLATE", Registration);
                OracleUdt.SetValue(con, pUdt, "FLEET_NO", FleetId);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                SerialNumber = (int)OracleUdt.GetValue(con, pUdt, "ID_NO");
                Registration = (string)OracleUdt.GetValue(con, pUdt, "LICENSE_PLATE");
                FleetId = (string)OracleUdt.GetValue(con, pUdt, "FLEET_NO");
            }
        }
        [OracleCustomTypeMapping("PORTAL.REG_IMPORT")]
        public class RegistrationsFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new Registrations();
            }
        }
        public class RegistrationsArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public Registrations[] RegistrationsObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static RegistrationsArray Null
            {
                get
                {
                    RegistrationsArray p = new RegistrationsArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, RegistrationsObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RegistrationsObj = (Registrations[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.REG_IMPORT_ARRAY")]
        public class RegistrationsArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new RegistrationsArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new Registrations[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new Registrations[numElems];
            }
            #endregion
        }

        #endregion

    }
}
