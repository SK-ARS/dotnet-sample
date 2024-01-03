using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class ICAVehicleModel
    {
        //for vehicle configuration
        [Required(ErrorMessage = "Gross weight is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double VhclGrossWeight { get; set; }

        [Required(ErrorMessage = "Length is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double VhclLength { get; set; }

        [Required(ErrorMessage = "Width is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double VhclWidth { get; set; }

        [Required(ErrorMessage = "Maximum axle weight is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double VhclMaxAxleWeight { get; set; }

        [Required(ErrorMessage = "Minimum axle spacing is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double VhclMinAxleSpacing { get; set; }

        public int VhclCount { get; set; }

        //for tractors
        [Required(ErrorMessage = "Gross weight is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double TractorGrossWeight { get; set; }

        [Required(ErrorMessage = "Maximum axle weight is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double TractorMaxAxleWeight { get; set; }

        [Required(ErrorMessage = "Minimum axle spacing is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double TractorMinAxleSpacing { get; set; }

        public int TractorAxleCount { get; set; }

        //for trailers
        [Required(ErrorMessage = "Gross weight is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double TrailerGrossWeight { get; set; }

        [Required(ErrorMessage = "Maximum axle weight is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double TrailerMaxAxleWeight { get; set; }

        [Required(ErrorMessage = "Minimum axle spacing is required")]
        [RegularExpression(@"[\d*\.?\d*]+$", ErrorMessage = "Only numeric characters and '.' are allowed")]
        public double TrailerMinAxleSpacing { get; set; }
    }
}