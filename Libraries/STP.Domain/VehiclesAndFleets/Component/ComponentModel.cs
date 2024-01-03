using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public class ComponentModel
    {
        public double ComponentId { get; set; }
        public int ComponentSubType { get; set; }
        public string FormalName { get; set; }
        public string IntendedName { get; set; }
        public string Description { get; set; }
        public int? CouplingType { get; set; }
        public int? StandardCU { get; set; }
        public int? IsTracked { get; set; }
        public int? IsSteerable { get; set; }
        public int ComponentType { get; set; }
        public double? MaxHeight { get; set; }
        public int? MaxHeightUnit { get; set; }
        public double? ReducableHeight { get; set; }
        public int? ReducableHeightUnit { get; set; }
        public double? RigidLength { get; set; }
        public int? RigidLengthUnit { get; set; }
        public double? Width { get; set; }
        public int? WidthUnit { get; set; }
        public double? WheelBase { get; set; }
        public int? WheelBaseUnit { get; set; }
        public double? LeftOverhang { get; set; }
        public int? LeftOverhangUnit { get; set; }
        public double? RightOverhang { get; set; }
        public int? RightOverhangUnit { get; set; }
        public double? FrontOverhang { get; set; }
        public int? FrontOverhangUnit { get; set; }
        public double? RearOverhang { get; set; }
        public int? RearOverhangUnit { get; set; }
        public double? GroundClearance { get; set; }
        public int? GroundClearanceUnit { get; set; }
        public double? OutsideTrack { get; set; }
        public int? OutsideTrackUnit { get; set; }
        public double? GrossWeight { get; set; }
        public int? GrossWeightUnit { get; set; }
        public double? MaxAxleWeight { get; set; }
        public int? MaxAxleWeightUnit { get; set; }
        public int? AxleCount { get; set; }
        public int? AxleWeightUnit { get; set; }
        public int? AxleSpacingUnit { get; set; }
        public int? WheelSpacingUnit { get; set; }
        public double? SpaceToFollowing { get; set; }
        public int? SpaceToFollowingUnit { get; set; }
        public double? SpacingToFollowing { get; set; }
        public int? SpacingToFollowingUnit { get; set; }
        public double? OrganisationId { get; set; }
        public double? RedGroundClearance { get; set; }
        public int? RedGroundClearanceUnit { get; set; }
        public int VehicleIntent { get; set; }
        public int ShowComponent { get; set; }
        public string GUID { get; set; }
    }

    public class UpdateComponentParams
    {
        public ComponentModel ComponentDetails { get; set; }
        public string UserSchema { get; set; }
    }
}