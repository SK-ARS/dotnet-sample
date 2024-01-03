using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class ImposedConstraints
    {
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public int? GrossWeight { get; set; }
        public int? AxcelWeight { get; set; }
        public MaximumWeightOverMinimumDistance MaxWeightOverminDisttonnes { get; set; }
        public int? HALoading { get; set; }
        public double? HBWithLiveLoad { get; set; }
        public double? HBWithouthLiveLoad { get; set; }
        public SignedHeight Signed_Height { get; set; }
        public SignedWidth Signed_Width { get; set; }
        public SignedLength Signed_Length { get; set; }
        public SignedGrossWeight Signed_Gross_Weight { get; set; }
        public SignedAxcelWeight Signed_Axcel_Weight { get; set; }
        public VerticalAlignment Vertical_Alignment { get; set; }
        public HeightEnvelopes Height_Envelopes { get; set; }

        public int SignedHeightStatus { get; set; }
        public int SignedWidthStatus { get; set; }
        public int SignedLengthStatus { get; set; }
        public int SignedGrossWeightStatus { get; set; }
        public int SignedAxcelWeightStatus { get; set; }
        public double? HighwaySkew { get; set; }
        public string UserName { get; set; }
        public long OrgID { get; set; }

        public bool SignedHeightStatusbool { get; set; }
        public bool SignedWidthStatusbool { get; set; }
        public bool SignedLengthStatusbool { get; set; }
        public bool SignedGrossWeightStatusbool { get; set; }
        public bool SignedAxcelWeightStatusbool { get; set; }

        public string SignedHeightRadio { get; set; }
        public string SignedWidthRadio { get; set; }
        public string SignedLenRadio { get; set; }
        public string SignedGrossWeightRadio { get; set; }
        public string SignedAxleWeightRadio { get; set; }

        public string MaxWeightOverMinDistanceWeight { get; set; }
        public string MaxWeightOverMinDistanceDistance { get; set; }       
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
        public int? Grossweight { get; set; }
        public int Constraints { get; set; }
    }
    public class SignedAxcelWeight
    {
        public int? Axcelweight { get; set; }
        public int Constraints { get; set; }
    }

    public class VerticalAlignment
    {
        public double? EntryDistance { get; set; }
        public double? EntryHeight { get; set; }
        public double? MaxHeighDistance { get; set; }
        public double? MaxHeighHeight { get; set; }
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
        public string HtEnvelopOffset { get; set; }

        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        public string HtEnvelopWidth { get; set; }

        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid value")]
        public string HtEnvelopHeight { get; set; }
    }
}