using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class StructureNotification
    {
        public long RevisionId { get; set; }

        public int RevisionNo { get; set; }

        public long AnalysisId { get; set; }

        public long VersionId { get; set; }

        public DateTime ReceivedDate { get; set; }

        public DateTime MovementStartDate { get; set; }

        public DateTime MovementEndDate { get; set; }

        public string HaulierName { get; set; }

        public string HaulierContact { get; set; }

        public string ESDALReference { get; set; }

        public decimal CountRec { get; set; }

        public int WorkInProgress { get; set; }

        public int ApplicationStatus { get; set; }

        public string HaulierReference { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Address4 { get; set; }

        public string Address5 { get; set; }

        public string PostalCode { get; set; }

        public string HaulierTelNumber { get; set; }

        public string HaulierFaxNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string HaulierEmail { get; set; }

        public string HaulierLicenceNumber { get; set; }

        public string HaulierCountry { get; set; }

        public double OverallLength { get; set; }

        public double OverallWidth { get; set; }

        public double OverallHeight { get; set; }

        public double ReducibleHeight { get; set; }

        public double RigidLength { get; set; }

        public int GrossWeight { get; set; }

        public double MaxAxleWeight { get; set; }

        public double RearOverHang { get; set; }

        public double SemiGrossWeight { get; set; }

        public double SemiMaxAxleWeight { get; set; }

        public string SemiRearOverHang { get; set; }

        public long GroundClearance { get; set; }

        public string StructureName { get; set; }

        public string StructureType { get; set; }

        public string OnBehalfOf { get; set; }

        public double WheelBase { get; set; }

        public string AxleComponent { get; set; }

        public Byte[] AffectedStucturesXML { get; set; }

        public string AxleWeight { get; set; }

        public string WheelsPerAxle { get; set; }

        public string AxleSpacing { get; set; }

        public int ICA { get; set; }

        public long RouteId { get; set; }

        public decimal RoutePartId { get; set; }
    }

    public class StructureOnerousSearch
    {
        public string MovementStartDate { get; set; }

        public string MovementEndDate { get; set; }

        public string SortCriteria { get; set; }
        public string SortValue { get; set; }

        public string SearchStatus { get; set; }
        public string SearchStatusValue { get; set; }

        public bool StartDateFlag { get; set; }
        public bool EndDateFlag { get; set; }
    }
}