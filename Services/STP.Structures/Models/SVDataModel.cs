using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class SVDataWithLoadModel
    {
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithSV80 { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithSV100 { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithSV150 { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithSVTrain { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithSVTT { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithoutSV80 { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithoutSV100 { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithoutSV150 { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithoutSVTrain { get; set; }
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? WithoutSVTT { get; set; }
        public int? SVDerivation { get; set; }

    }

    public class SVDataWithoutLoadModel
    {
        public double SV80 { get; set; }
        public double SV100 { get; set; }
        public double SV150 { get; set; }
        public double SVTrain { get; set; }
        public double SVTT { get; set; }
    }

    public class SVDataList
    {
        public int VehicleType { get; set; }
        public double? WithLoad { get; set; }
        public double? WithoutLoad { get; set; }
        public int ParameterNo { get; set; }
        public int SVDerivation { get; set; }
        public double? CalculatedFactor { get; set; }
        public double? ManualInputFactor { get; set; }

    }
}