using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace STP.Domain.Structures
{
    public class ImposedConstraints
    {
        public int? SignedGrossWeight { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public int? GrossWeight { get; set; }
        public int? AxleWeight { get; set; }
        public double? SkewAngle { get; set; }
        public MaximumWeightOverMinimumDistance MaxWeightOverMinDistance { get; set; }
        public int? HALoading { get; set; }
        public double? HBWithLiveLoad { get; set; }
        public double? HBWithoutLiveLoad { get; set; }
        public SignedHeight SignedHeight { get; set; }
        public SignedWidth SignedWidth { get; set; }
        public SignedLength SignedLength { get; set; }
        public SignedGrossWeight SignedGrossWeightObj { get; set; }
        public SignedAxleWeight SignedAxleWeight { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public HeightEnvelopes HeightEnvelopes { get; set; }

        public int SignedHeightStatus { get; set; }
        public int SignedWidthStatus { get; set; }
        public int SignedLengthStatus { get; set; }
        public int SignedGrossWeightStatus { get; set; }
        public int SignedAxleWeightStatus { get; set; }
        public double? HighwaySkew { get; set; }
        public string UserName { get; set; }
        public long OrgId { get; set; }

        public bool SignedHeightStatusBool { get; set; }
        public bool SignedWidthStatusBool { get; set; }
        public bool SignedLengthStatusBool { get; set; }
        public bool SignedGrossWeightStatusBool { get; set; }
        public bool SignedAxcelWeightStatusBool { get; set; }
        public int? SVRating { get; set; }
        public string SignedHeightRadio { get; set; }
        public string SignedWidthRadio { get; set; }
        public string SignedLengthRadio { get; set; }
        public string SignedGrossWeightRadio { get; set; }
        public string SignedAxleWeightRadio { get; set; }
        public int? SignedSingleAxleWeight { get; set; }
        public int? SignedDoubleAxleWeight { get; set; }
        public int? SignedTripleAxleWeight { get; set; }

        public int? SignedAxleGroupWeight { get; set; }

        public string MaxWeightOverMinDistanceWeight { get; set; }
        public string MaxWeightOverMinDistanceDistance { get; set; }
        
        public int EnableSV_80 { get; set; }
        public int EnableSV_100 { get; set; }
        public int EnableSV_150 { get; set; }
        public int EnableSV_Train { get; set; }
        public int? SectionId { get; set; }
    }

    public class SignedHeight
    {
        public double? HeightMeter { get; set; }
        public double? HeightFeet { get; set; }
        public int Constraints { get; set; }
        public double? HeightInches { get; set; }
    }
    public class SignedWidth
    {
        public double? WidthMeter { get; set; }
        public double? WidthFeet { get; set; }
        public int Constraints { get; set; }
        public double? WidthInches { get; set; }
    }
    public class SignedLength
    {
        public double? LengthMeter { get; set; }
        public double? LengthFeet { get; set; }
        public int Constraints { get; set; }
        public double? LengthInches { get; set; }
    }
    public class SignedGrossWeight
    {
        public int? GrossWeight { get; set; }
        public int Constraints { get; set; }
    }
    public class SignedAxleWeight
    {
        public int? AxleWeight { get; set; }
        public int Constraints { get; set; }
    }

    public class VerticalAlignment
    {
        public double? EntryDistance { get; set; }
        public double? EntryHeight { get; set; }
        public double? MaxDistance { get; set; }
        public double? MaxHeight { get; set; }
        public double? ExitDistance { get; set; }
        public double? ExitHeight { get; set; }
    }

    public class MaximumWeightOverMinimumDistance
    {
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        // [Required(ErrorMessage = "The height envelopes is required")]
        public string TonnesOver { get; set; }

        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        // [Required(ErrorMessage = "The height envelopes is required")]
        public string Meter { get; set; }
    }

    public class HeightEnvelopes
    {
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        // [Required(ErrorMessage = "The height envelopes is required")]
        public string HeightEnvelopeOffset { get; set; }

        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        public string HeightEnvelopeWidth { get; set; }

        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        public string HeighttEnvelopeHeight { get; set; }
    }
}