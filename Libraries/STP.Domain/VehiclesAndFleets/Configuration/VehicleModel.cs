using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using STP.Domain.VehicleAndFleets.Component;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class VehicleModel
    {
        #region Component
        [DataContract]
        public class VehicleComponentModel : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("COMPONENT_ID")]
            public double ComponentId { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_NAME")]
            public string IntendedName { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_DESC")]
            public string Description { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_SUMMARY")]
            public string FormalName { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_TYPE")]
            public int ComponentType { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_SUBTYPE")]
            public int ComponentSubType { get; set; }
            [DataMember]
            [OracleObjectMapping("COUPLING_TYPE")]
            public int? CouplingType { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_STANDARD_C_AND_U")]
            public int? StandardCU { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_TRACKED")]
            public int? IsTracked { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_STEERABLE_AT_REAR")]
            public int? IsSteerable { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT")]
            public double? MaxHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT_UNIT")]
            public int? MaxHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_HEIGHT")]
            public double? ReducableHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_HEIGHT_UNIT")]
            public int? ReducableHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN")]
            public double? RigidLength { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN_UNIT")]
            public int? RigidLengthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH")]
            public double? Width { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH_UNIT")]
            public int? WidthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE")]
            public double? WheelBase { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE_UNIT")]
            public int? WheelBaseUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("LEFT_OVERHANG")]
            public double? LeftOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("LEFT_OVERHANG_UNIT")]
            public int? LeftOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGHT_OVERHANG")]
            public double? RightOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGHT_OVERHANG_UNIT")]
            public int? RightOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("FRONT_OVERHANG")]
            public double? FrontOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("FRONT_OVERHANG_UNIT")]
            public int? FrontOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("REAR_OVERHANG")]
            public double? RearOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("REAR_OVERHANG_UNIT")]
            public int? RearOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROUND_CLEARANCE")]
            public double? GroundClearance { get; set; }
            [DataMember]
            [OracleObjectMapping("GROUND_CLEARANCE_UNIT")]
            public int? GroundClearanceUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("OUTSIDE_TRACK")]
            public double? OutsideTrack { get; set; }
            [DataMember]
            [OracleObjectMapping("OUTSIDE_TRACK_UNIT")]
            public int? OutsideTrackUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT")]
            public double? GrossWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT_UNIT")]
            public int? GrossWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT")]
            public double? MaxAxleWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT_UNIT")]
            public int? MaxAxleWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_COUNT")]
            public int? AxleCount { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_WEIGHT_UNIT")]
            public int? AxleWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_SPACING_UNIT")]
            public int? AxleSpacingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEEL_SPACING_UNIT")]
            public int? WheelSpacingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("SPACE_TO_FOLLOWING")]
            public double? SpaceToFollowing { get; set; }
            [DataMember]
            [OracleObjectMapping("SPACE_TO_FOLLOWING_UNIT")]
            public int? SpaceToFollowingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("ORGANISATION_ID")]
            public double? OrganisationId { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_GROUND_CLEARANCE")]
            public double? RedGroundClearance { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_GROUND_CLEARANCE_UNIT")]
            public int? RedGroundClearanceUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("VEHICLE_INTENT")]
            public int VehicleIntent { get; set; }
            [DataMember]
            [OracleObjectMapping("SHOW_COMPONENT")]
            public int ShowComponent { get; set; }
            [DataMember]
            [OracleObjectMapping("GUID")]
            public string GUID { get; set; }
            [DataMember]
            public List<ComponentAxle> ComponentAxleList { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_AXLE_ARRAY")]
            public ComponentAxleArray ComponentAxleArray { get; set; }
            [DataMember]
            public List<ComponentRegistration> ComponentRegistrationList { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_REGISTRATION_ARRAY")]
            public ComponentRegistrationArray ComponentRegistrationArray { get; set; }
            public VehicleComponentModel()
            {
                ComponentAxleList = new List<ComponentAxle>();
                ComponentAxleArray = new ComponentAxleArray();
                ComponentRegistrationList = new List<ComponentRegistration>();
                ComponentRegistrationArray = new ComponentRegistrationArray();
            }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static VehicleComponentModel Null
            {
                get
                {
                    VehicleComponentModel p = new VehicleComponentModel
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "COMPONENT_ID", ComponentId);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_NAME", IntendedName);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_DESC", Description);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_SUMMARY", FormalName);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_TYPE", ComponentType);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_SUBTYPE", ComponentSubType);
                OracleUdt.SetValue(con, pUdt, "COUPLING_TYPE", CouplingType);
                OracleUdt.SetValue(con, pUdt, "IS_STANDARD_C_AND_U", StandardCU);
                OracleUdt.SetValue(con, pUdt, "IS_TRACKED", IsTracked);
                OracleUdt.SetValue(con, pUdt, "IS_STEERABLE_AT_REAR", IsSteerable);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT", MaxHeight);
                OracleUdt.SetValue(con, pUdt, "MAX_HEIGHT_UNIT", MaxHeightUnit);
                OracleUdt.SetValue(con, pUdt, "RED_HEIGHT", ReducableHeight);
                OracleUdt.SetValue(con, pUdt, "RED_HEIGHT_UNIT", ReducableHeightUnit);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN", RigidLength);
                OracleUdt.SetValue(con, pUdt, "RIGID_LEN_UNIT", RigidLengthUnit);
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
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT", GrossWeight);
                OracleUdt.SetValue(con, pUdt, "GROSS_WEIGHT_UNIT", GrossWeightUnit);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT", MaxAxleWeight);
                OracleUdt.SetValue(con, pUdt, "MAX_AXLE_WEIGHT_UNIT", MaxAxleWeight);
                OracleUdt.SetValue(con, pUdt, "AXLE_COUNT", AxleCount);
                OracleUdt.SetValue(con, pUdt, "AXLE_WEIGHT_UNIT", AxleWeightUnit);
                OracleUdt.SetValue(con, pUdt, "AXLE_SPACING_UNIT", AxleSpacingUnit);
                OracleUdt.SetValue(con, pUdt, "WHEEL_SPACING_UNIT", WheelSpacingUnit);
                OracleUdt.SetValue(con, pUdt, "SPACE_TO_FOLLOWING", SpaceToFollowing);
                OracleUdt.SetValue(con, pUdt, "SPACE_TO_FOLLOWING_UNIT", SpaceToFollowingUnit);
                OracleUdt.SetValue(con, pUdt, "ORGANISATION_ID", OrganisationId);
                OracleUdt.SetValue(con, pUdt, "RED_GROUND_CLEARANCE", RedGroundClearance);
                OracleUdt.SetValue(con, pUdt, "RED_GROUND_CLEARANCE_UNIT", RedGroundClearanceUnit);
                OracleUdt.SetValue(con, pUdt, "VEHICLE_INTENT", VehicleIntent);
                OracleUdt.SetValue(con, pUdt, "SHOW_COMPONENT", ShowComponent);
                OracleUdt.SetValue(con, pUdt, "GUID", GUID);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_AXLE_ARRAY", ComponentAxleArray);
                OracleUdt.SetValue(con, pUdt, "COMPONENT_REGISTRATION_ARRAY", ComponentRegistrationArray);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                ComponentId = (double)OracleUdt.GetValue(con, pUdt, "COMPONENT_ID");
                IntendedName = (string)OracleUdt.GetValue(con, pUdt, "COMPONENT_NAME");
                Description = (string)OracleUdt.GetValue(con, pUdt, "COMPONENT_DESC");
                FormalName = (string)OracleUdt.GetValue(con, pUdt, "COMPONENT_SUMMARY");
                ComponentType = (int)OracleUdt.GetValue(con, pUdt, "COMPONENT_TYPE");
                ComponentSubType = (int)OracleUdt.GetValue(con, pUdt, "COMPONENT_SUBTYPE");
                CouplingType = (int?)OracleUdt.GetValue(con, pUdt, "COUPLING_TYPE");
                StandardCU = (int?)OracleUdt.GetValue(con, pUdt, "IS_STANDARD_C_AND_U");
                IsTracked = (int?)OracleUdt.GetValue(con, pUdt, "IS_TRACKED");
                IsSteerable = (int?)OracleUdt.GetValue(con, pUdt, "IS_STEERABLE_AT_REAR");
                MaxHeight = (double?)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT");
                MaxHeightUnit = (int?)OracleUdt.GetValue(con, pUdt, "MAX_HEIGHT_UNIT");
                ReducableHeight = (double?)OracleUdt.GetValue(con, pUdt, "RED_HEIGHT");
                ReducableHeightUnit = (int?)OracleUdt.GetValue(con, pUdt, "RED_HEIGHT_UNIT");
                RigidLength = (double?)OracleUdt.GetValue(con, pUdt, "RIGID_LEN");
                RigidLengthUnit = (int?)OracleUdt.GetValue(con, pUdt, "RIGID_LEN_UNIT");
                Width = (double?)OracleUdt.GetValue(con, pUdt, "WIDTH");
                WidthUnit = (int?)OracleUdt.GetValue(con, pUdt, "WIDTH_UNIT");
                WheelBase = (double?)OracleUdt.GetValue(con, pUdt, "WHEELBASE");
                WheelBaseUnit = (int?)OracleUdt.GetValue(con, pUdt, "WHEELBASE_UNIT");
                LeftOverhang = (double?)OracleUdt.GetValue(con, pUdt, "LEFT_OVERHANG");
                LeftOverhangUnit = (int?)OracleUdt.GetValue(con, pUdt, "LEFT_OVERHANG_UNIT");
                RightOverhang = (double?)OracleUdt.GetValue(con, pUdt, "RIGHT_OVERHANG");
                RightOverhangUnit = (int?)OracleUdt.GetValue(con, pUdt, "RIGHT_OVERHANG_UNIT");
                FrontOverhang = (double?)OracleUdt.GetValue(con, pUdt, "FRONT_OVERHANG");
                FrontOverhangUnit = (int?)OracleUdt.GetValue(con, pUdt, "FRONT_OVERHANG_UNIT");
                RearOverhang = (double?)OracleUdt.GetValue(con, pUdt, "REAR_OVERHANG");
                RearOverhangUnit = (int?)OracleUdt.GetValue(con, pUdt, "REAR_OVERHANG_UNIT");
                GroundClearance = (double?)OracleUdt.GetValue(con, pUdt, "GROUND_CLEARANCE");
                GroundClearanceUnit = (int?)OracleUdt.GetValue(con, pUdt, "GROUND_CLEARANCE_UNIT");
                OutsideTrack = (double?)OracleUdt.GetValue(con, pUdt, "OUTSIDE_TRACK");
                OutsideTrackUnit = (int?)OracleUdt.GetValue(con, pUdt, "OUTSIDE_TRACK_UNIT");
                GrossWeight = (double?)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT");
                GrossWeightUnit = (int?)OracleUdt.GetValue(con, pUdt, "GROSS_WEIGHT_UNIT");
                MaxAxleWeight = (double?)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT");
                MaxAxleWeightUnit = (int?)OracleUdt.GetValue(con, pUdt, "MAX_AXLE_WEIGHT_UNIT");
                AxleCount = (int?)OracleUdt.GetValue(con, pUdt, "AXLE_COUNT");
                AxleWeightUnit = (int?)OracleUdt.GetValue(con, pUdt, "AXLE_WEIGHT_UNIT");
                AxleSpacingUnit = (int?)OracleUdt.GetValue(con, pUdt, "AXLE_SPACING_UNIT");
                WheelSpacingUnit = (int?)OracleUdt.GetValue(con, pUdt, "WHEEL_SPACING_UNIT");
                SpaceToFollowing = (double?)OracleUdt.GetValue(con, pUdt, "SPACE_TO_FOLLOWING");
                SpaceToFollowingUnit = (int?)OracleUdt.GetValue(con, pUdt, "SPACE_TO_FOLLOWING_UNIT");
                OrganisationId = (double?)OracleUdt.GetValue(con, pUdt, "ORGANISATION_ID");
                RedGroundClearance = (double)OracleUdt.GetValue(con, pUdt, "RED_GROUND_CLEARANCE");
                RedGroundClearanceUnit = (int?)OracleUdt.GetValue(con, pUdt, "RED_GROUND_CLEARANCE_UNIT");
                VehicleIntent = (int)OracleUdt.GetValue(con, pUdt, "VEHICLE_INTENT");
                ShowComponent = (int)OracleUdt.GetValue(con, pUdt, "SHOW_COMPONENT");
                GUID = (string)OracleUdt.GetValue(con, pUdt, "GUID");
                ComponentAxleArray = (ComponentAxleArray)OracleUdt.GetValue(con, pUdt, "COMPONENT_AXLE_ARRAY");
                ComponentRegistrationArray = (ComponentRegistrationArray)OracleUdt.GetValue(con, pUdt, "COMPONENT_REGISTRATION_ARRAY");
            }
        }
        [OracleCustomTypeMapping("PORTAL.COMPONENTS")]
        public class VehicleComponentModelFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new VehicleComponentModel();
            }
        }
        public class VehicleComponentModelArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public VehicleComponentModel[] ComponentObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static VehicleComponentModelArray Null
            {
                get
                {
                    VehicleComponentModelArray p = new VehicleComponentModelArray
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
                ComponentObj = (VehicleComponentModel[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }
        
        [OracleCustomTypeMapping("PORTAL.COMPONENTSARRAY")]
        public class VehicleComponentModelArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new VehicleComponentModelArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new VehicleComponentModel[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new VehicleComponentModel[numElems];
            }
            #endregion
        }

        #endregion

        #region ComponentAxle
        [DataContract]
        public class ComponentAxle : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("AXLE_NO")]
            public int AxleNumId { get; set; }
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
            public string TextDescription { get; set; }
            [DataMember]
            public int AxleDriven { get; set; }
            [DataMember]
            public int BogieAxle { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEEL_SPACING_LIST")]
            public string TyreCenters { get; set; }
            [DataMember]
            [OracleObjectMapping("TYRE_SIZE")]
            public string TyreSize { get; set; }
            [DataMember]
            public double ComponentId { get; set; }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static ComponentAxle Null
            {
                get
                {
                    ComponentAxle p = new ComponentAxle
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }

            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "AXLE_NO", AxleNumId);
                OracleUdt.SetValue(con, pUdt, "WEIGHT", AxleWeight);
                OracleUdt.SetValue(con, pUdt, "TYRE_SIZE", TyreSize);
                OracleUdt.SetValue(con, pUdt, "NEXT_AXLE_DIST", DistanceToNextAxle);
                OracleUdt.SetValue(con, pUdt, "WHEEL_COUNT", NoOfWheels);
                OracleUdt.SetValue(con, pUdt, "WHEEL_SPACING_LIST", TyreCenters);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                AxleNumId = (int)OracleUdt.GetValue(con, pUdt, "AXLE_NO");
                AxleWeight = (double)OracleUdt.GetValue(con, pUdt, "WEIGHT");
                TyreSize = (string)OracleUdt.GetValue(con, pUdt, "TYRE_SIZE");
                DistanceToNextAxle = (double)OracleUdt.GetValue(con, pUdt, "NEXT_AXLE_DIST");
                NoOfWheels = (int)OracleUdt.GetValue(con, pUdt, "WHEEL_COUNT");
                TyreCenters = (string)OracleUdt.GetValue(con, pUdt, "WHEEL_SPACING_LIST");
            }
        }
        [OracleCustomTypeMapping("PORTAL.COMPONENTAXLE")]
        public class ComponentAxleFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new ComponentAxle();
            }
        }
        public class ComponentAxleArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public ComponentAxle[] ComponentAxleObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static ComponentAxleArray Null
            {
                get
                {
                    ComponentAxleArray p = new ComponentAxleArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, ComponentAxleObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                ComponentAxleObj = (ComponentAxle[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.COMPONENTAXLEARRAY")]
        public class ComponentAxleArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new ComponentAxleArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new ComponentAxle[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new ComponentAxle[numElems];
            }
            #endregion
        }
        #endregion

        #region ComponentRegistration
        [DataContract]
        public class ComponentRegistration : INullable, IOracleCustomType
        {
            private bool m_bIsNull;
            [DataMember]
            [OracleObjectMapping("LICENSE_PLATE")]
            public string RegistrationValue { get; set; }
            [DataMember]
            [OracleObjectMapping("FLEET_NO")]
            public string FleetId { get; set; }
            [DataMember]
            public double ComponentId { get; set; }
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static ComponentRegistration Null
            {
                get
                {
                    ComponentRegistration p = new ComponentRegistration
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, "LICENSE_PLATE", RegistrationValue);
                OracleUdt.SetValue(con, pUdt, "FLEET_NO", FleetId);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                RegistrationValue = (string)OracleUdt.GetValue(con, pUdt, "LICENSE_PLATE");
                FleetId = (string)OracleUdt.GetValue(con, pUdt, "FLEET_NO");
            }
        }
        [OracleCustomTypeMapping("PORTAL.COMPONENTREGISTRATION")]
        public class ComponentRegistrationFactory : IOracleCustomTypeFactory
        {
            public IOracleCustomType CreateObject()
            {
                // Return a new custom object
                return new ComponentRegistration();
            }
        }
        public class ComponentRegistrationArray : INullable, IOracleCustomType
        {
            [OracleArrayMapping()]
            public ComponentRegistration[] ComponentRegistrationObj { get; set; }

            private bool m_bIsNull;
            public virtual bool IsNull
            {
                get
                {
                    return m_bIsNull;
                }
            }
            public static ComponentRegistrationArray Null
            {
                get
                {
                    ComponentRegistrationArray p = new ComponentRegistrationArray
                    {
                        m_bIsNull = true
                    };
                    return p;
                }
            }
            public void FromCustomObject(OracleConnection con, IntPtr pUdt)
            {
                OracleUdt.SetValue(con, pUdt, 0, ComponentRegistrationObj);
            }

            public void ToCustomObject(OracleConnection con, IntPtr pUdt)
            {
                ComponentRegistrationObj = (ComponentRegistration[])OracleUdt.GetValue(con, pUdt, 0);
            }
        }

        [OracleCustomTypeMapping("PORTAL.COMPONENTREGISTRATIONARRAY")]
        public class ComponentRegistrationArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
        {
            #region IOracleCustomTypeFactory Members
            public IOracleCustomType CreateObject()
            {
                return new ComponentRegistrationArray();
            }

            #endregion

            #region IOracleArrayTypeFactory Members
            public Array CreateArray(int numElems)
            {
                return new ComponentRegistration[numElems];
            }
            public Array CreateStatusArray(int numElems)
            {
                return new ComponentRegistration[numElems];
            }
            #endregion
        }
        #endregion

        #region VehicleConfigDetail
        [DataContract]
        public class VehicleConfigDetail
        {
            [DataMember]
            public int ConfigurationId { get; set; }
            [DataMember]
            public int MovementTypeId { get; set; }
            [DataMember]
            public int Flag { get; set; }
            [DataMember]
            public List<VehicleComponentModel> componentList { get; set; }
            [DataMember]
            public string UserSchema { get; set; }

            public VehicleConfigDetail()
            {
                componentList = new List<VehicleComponentModel>();
            }
        }
        #endregion

        #region ComponentNewModel
        public class ComponentNewModel 
        {
            [DataMember]
            [OracleObjectMapping("COMPONENT_ID")]
            public double ComponentId { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_NAME")]
            public string IntendedName { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_DESC")]
            public string Description { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_SUMMARY")]
            public string FormalName { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_TYPE")]
            public int ComponentType { get; set; }
            [DataMember]
            [OracleObjectMapping("COMPONENT_SUBTYPE")]
            public int ComponentSubType { get; set; }
            [DataMember]
            [OracleObjectMapping("COUPLING_TYPE")]
            public int? CouplingType { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_STANDARD_C_AND_U")]
            public int? StandardCU { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_TRACKED")]
            public int? IsTracked { get; set; }
            [DataMember]
            [OracleObjectMapping("IS_STEERABLE_AT_REAR")]
            public int? IsSteerable { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT")]
            public double? MaxHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_HEIGHT_UNIT")]
            public int? MaxHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_HEIGHT")]
            public double? ReducableHeight { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_HEIGHT_UNIT")]
            public int? ReducableHeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN")]
            public double? RigidLength { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGID_LEN_UNIT")]
            public int? RigidLengthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH")]
            public double? Width { get; set; }
            [DataMember]
            [OracleObjectMapping("WIDTH_UNIT")]
            public int? WidthUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE")]
            public double? WheelBase { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEELBASE_UNIT")]
            public int? WheelBaseUnit { get; set; }
            [OracleObjectMapping("LEFT_OVERHANG")]
            public double? LeftOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("LEFT_OVERHANG_UNIT")]
            public int? LeftOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGHT_OVERHANG")]
            public double? RightOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("RIGHT_OVERHANG_UNIT")]
            public int? RightOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("FRONT_OVERHANG")]
            public double? FrontOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("FRONT_OVERHANG_UNIT")]
            public int? FrontOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("REAR_OVERHANG")]
            public double? RearOverhang { get; set; }
            [DataMember]
            [OracleObjectMapping("REAR_OVERHANG_UNIT")]
            public int? RearOverhangUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROUND_CLEARANCE")]
            public double? GroundClearance { get; set; }
            [DataMember]
            [OracleObjectMapping("GROUND_CLEARANCE_UNIT")]
            public int? GroundClearanceUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("OUTSIDE_TRACK")]
            public double? OutsideTrack { get; set; }
            [DataMember]
            [OracleObjectMapping("OUTSIDE_TRACK_UNIT")]
            public int? OutsideTrackUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT")]
            public double? GrossWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("GROSS_WEIGHT_UNIT")]
            public int? GrossWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT")]
            public double? MaxAxleWeight { get; set; }
            [DataMember]
            [OracleObjectMapping("MAX_AXLE_WEIGHT_UNIT")]
            public int? MaxAxleWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_COUNT")]
            public int? AxleCount { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_WEIGHT_UNIT")]
            public int? AxleWeightUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("AXLE_SPACING_UNIT")]
            public int? AxleSpacingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("WHEEL_SPACING_UNIT")]
            public int? WheelSpacingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("SPACE_TO_FOLLOWING")]
            public double? SpaceToFollowing { get; set; }
            [DataMember]
            [OracleObjectMapping("SPACE_TO_FOLLOWING_UNIT")]
            public int? SpaceToFollowingUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("ORGANISATION_ID")]
            public double? OrganisationId { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_GROUND_CLEARANCE")]
            public double? RedGroundClearance { get; set; }
            [DataMember]
            [OracleObjectMapping("RED_GROUND_CLEARANCE_UNIT")]
            public int? RedGroundClearanceUnit { get; set; }
            [DataMember]
            [OracleObjectMapping("VEHICLE_INTENT")]
            public int VehicleIntent { get; set; }
            [DataMember]
            [OracleObjectMapping("SHOW_COMPONENT")]
            public int ShowComponent { get; set; }
            [DataMember]
            [OracleObjectMapping("GUID")]
            public string GUID { get; set; }

        }
        #endregion

        #region ComponentDetail
        [DataContract]
        public class ComponentDetail
        {
            [DataMember]
            public int ComponentId { get; set; }
            [DataMember]
            public int ComponentTypeId { get; set; }
            [DataMember]
            public int ComponentSubTypeId { get; set; }
            [DataMember]
            public string Guid { get; set; }
            [DataMember]
            public VehicleComponent vehicleComponent { get; set; }
            [DataMember]
            public List<ComponentAxle> ComponentAxleList { get; set; }
            [DataMember]
            public List<ComponentRegistration> ComponentRegistrationList { get; set; }
            public ComponentDetail()
            {
                ComponentAxleList = new List<ComponentAxle>();
                ComponentRegistrationList = new List<ComponentRegistration>();
                vehicleComponent = new VehicleComponent();
            }
        }
        #endregion
        
        #region VehicleComponentDetail
        [DataContract]
        public class VehicleComponentDetail
        {
            [DataMember]
            public int ConfigurationId { get; set; }
            [DataMember]
            public int MovemnetTypeId { get; set; }
            [DataMember]
            public List<ComponentDetail> ComponentDetailList { get; set; }
            public string UserSchema { get; set; }

            public VehicleComponentDetail()
            {
                ComponentDetailList = new List<ComponentDetail>();
            }
        }
        #endregion


    }
}
