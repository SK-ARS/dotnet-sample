using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class DrivingInstructionModel
    {
        public List<DrivingInstructionModel> ListDrivingInstructionModel { get; set; }

        public int ComparisonId { get; set; }

        public int Id { get; set; }

        public string LegNumber { get; set; }

        public string Name { get; set; }

        public string Instruction { get; set; }

        public int DisplayMetricDistance { get; set; }

        public int MeasuredMetricDistance { get; set; }

        public int DisplayImperialDistance { get; set; }

        public string PointType { get; set; }

        public string Description { get; set; }

        public ulong GridRefX { get; set; }

        public ulong GridRefY { get; set; }

        public Byte[] DrivingInstructions { get; set; }

        public string DockCautionDescription { get; set; }

        public string MotorwayCautionDescription { get; set; }

        public bool MotorwayCaution { get; set; }

        //RM#3646 - Start
        public string AnnotationType { get; set; }
        public int AnnotationId { get; set; }
        public string Text { get; set; }
        public bool IsDrivingInstructionAnnotation { get; set; }
        //RM#3646 - End
        //RM#3646 - Start
        public string Action { get; set; }

        public string CautionedEntity { get; set; }

        public int CautionId { get; set; }

        public bool CautionIdSpecified { get; set; }

        public string Conditions { get; set; }

        public string ConstrainingAttribute { get; set; }

        public string Contact { get; set; }

        public bool IsApplicable { get; set; }

        public string ECRN { get; set; }

        public string Type { get; set; }

        public string ConstraintName { get; set; }

        public string OrganisationName { get; set; }

        public int AlternativeId { get; set; }
        public string AlternativeName { get; set; }
        //RM#3646 - End


        public string ESRN { get; set; }

        public int SectionId { get; set; }

        public string StructureName { get; set; }

        public int OrganisationId { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }

        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }

        public decimal EncounteredMetric { get; set; }
        public decimal EncounteredImperial { get; set; }
        public decimal EncounterMeasureMetric { get; set; }

        public Byte[] AffectedStructures { get; set; }
    }

    public class RouteAnalysisModel
    {
        public Byte[] DrivingInstructions { get; set; }
        public Byte[] AffectedStructures { get; set; }
        public Byte[] AffectedRoads { get; set; }
        public Byte[] RouteDescription { get; set; }
        public Byte[] AffectedParties { get; set; }
    }

}