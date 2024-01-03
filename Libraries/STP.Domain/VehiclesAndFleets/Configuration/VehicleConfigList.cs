using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class VehicleConfigList
    {
        public long VehicleId { get; set; }
        public long ComponentId { get; set; }
        public int SubType { get; set; }
        public int LatPosn { get; set; }
        public int LongPosn { get; set; }
        public double SpaceToFollowing { get; set; }
        public int SpaceToFollowingUnit { get; set; }
        public long RoutePartId { get; set; }
        public long ComponentTypeId { get; set; }
        public string ComponentType { get; set; }
        public string ComponentSubType { get; set; }
        public long ComponentSubTypeId { get; set; }
        public double? GroundClearance { get; set; }
        public double? RedHeight { get; set; }
        public double? FrontOverhang { get; set; }
        public double? RearOverhang { get; set; }
        public decimal IsSteerableAtRear { get; set; }
        public double? WheelBase { get; set; }
        public double? RedGroundClearance { get; set; }
        public double? OutsideTrack { get; set; }
        public double? LeftOverhang { get; set; }
        public double? RightOverhang { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public string VehicleDescription { get; set; }
        public double? GrossWeight { get; set; }
        public double? RigidLength { get; set; }
    }
}