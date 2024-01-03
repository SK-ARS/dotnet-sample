using STP.Domain.VehiclesAndFleets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace STP.Domain.DocumentsAndContents
{
    public class OutboundDocuments
    {
        public byte[] DocumentInBytes { get; set; }

        public int DocType { get; set; }

        public int NotificationID { get; set; }

        public long OrganisationID { get; set; }

        public int ContactID { get; set; }

        public string EsdalReference { get; set; }

    }
    public class OutBoundDoc
    {

        public long RoutePartId { get; set; }

        public long VehicleId { get; set; }

        public string VehicleDesc { get; set; }

        public string PlannedContentRefNo { get; set; }

        [XmlIgnore]
        public double? RigidLength { get; set; }

        [XmlIgnore]
        public double? Length { get; set; }

        [XmlIgnore]
        public double? Width { get; set; }

        [XmlIgnore]
        public double? MaximumHeight { get; set; }

        [XmlIgnore]
        public double? GrossWeight { get; set; }

        [XmlIgnore]
        public double? MaximumAxleWeight { get; set; }

        [XmlIgnore]
        public double? RedHeight { get; set; }

        [XmlIgnore]
        public double? FrontOverhang { get; set; }

        [XmlIgnore]
        public double? RearOverhang { get; set; }

        public decimal IsSteerableAtRear { get; set; }

        [XmlIgnore]
        public double? WheelBase { get; set; }

        [XmlIgnore]
        public double? GroundClearance { get; set; }

        [XmlIgnore]
        public double? OutsideTrack { get; set; }

        [XmlIgnore]
        public double? LeftOverhang { get; set; }

        [XmlIgnore]
        public double? RightOverhang { get; set; }

        public double ComparisonId { get; set; }

        public decimal RoutePartNo { get; set; }

        public string PartDescr { get; set; }

        public int TransportMode { get; set; }

        public string PartName { get; set; }

        public string Name { get; set; }

        public string ComponentType { get; set; }

        public string VehicleType { get; set; }

        public string ComponentSubtype { get; set; }

        public short InclueDockWayCaution { get; set; }

        [XmlIgnore]
        public double? RedGroundClearance { get; set; }

        [XmlIgnore]
        public double? SpaceToFollowing { get; set; }

        public short LongPosn { get; set; }
        [XmlIgnore]
        public double? GrossTrainWeight { get; set; }
    }
    public class AxleFollowParams
    {
        public List<VehComponentAxles> vehicleComponentAxlesList { get; set; }
        public double firstComponentAxleSpaceToFollow { get; set; }
    }

}