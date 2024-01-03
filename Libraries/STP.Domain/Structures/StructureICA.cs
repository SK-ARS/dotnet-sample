using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class StructureICA
    {
        public string ICAMethod { get; set; }
        public double BandLimit { get; set; }
        public bool GrossWeight { get; set; }
        public bool AxelWeight { get; set; }
        public bool WeightOverDist { get; set; }
        public bool AWR { get; set; }
    }

    public class StructureICA2
    {
        public string ICAMethod { get; set; }
        public double BandLimit { get; set; }
        public bool SV80 { get; set; }
        public bool SV100 { get; set; }
        public bool SV150 { get; set; }
        public bool SVTrain { get; set; }
    }

    public class StructureICA3
    {
        public string ICAMethod { get; set; }
        public double BandLimit { get; set; }

    }

    //class for managing Structure ICA
    public class ManageStructureICA
    {
        public int StructureId { get; set; }
        public string StructureCode { get; set; }
        public int? SectionId { get; set; }

        //For Weight Screening BandLimit 
        public double DefaultWSBandLimitMin { get; set; }
        public double DefaultWSBandLimitMax { get; set; }
        public double DefaultSVBandLimitMin { get; set; }
        public double DefaultSVBandLimitMax { get; set; }

        //For SV Screening BandLimit       
       // [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
      //  [MaxLength(5)]
        public double? CustomWSBandLimitMin { get; set; }


        //[RegularExpression(@"^\d{1,2}([.]\d{1})$", ErrorMessage = "Only numbers and . are allowed")]

        public double? CustomWSBandLimitMax { get; set; }
       // [MaxLength(5)]
        public double? CustomSVBandLimitMin { get; set; }
       // [StringLength(5)]
        public double? CustomSVBandLimitMax { get; set; }

        //For the fields from STRUCTURE_SECTION_ICA table 
        public decimal? EnableGrossWeight { get; set; }
        public decimal? EnableAxleWeight { get; set; }
        public decimal? EnableWeightOverDist { get; set; }
        public decimal? EnableAWR { get; set; }
        public decimal? EnableSV80 { get; set; }
        public decimal? EnableSV100 { get; set; }
        public decimal? EnableSV150 { get; set; }
        public decimal? EnableSVTrain { get; set; }

        //For the checkboxes
        public int Default { get; set; }
        public int Custom { get; set; }
        public string UserName { get; set; }
    }

    public class SvReserveFactors
    {

        //for SV reserve factors with load
        public double? SVLiveLoad { get; set; }

        //for SV reserve factors without load
        public double? SVNoLoad { get; set; }

        //for HB rating
        public double? HBRatingLiveLoad { get; set; }
        public double? HBRatingNoLoad { get; set; }

        //for determining SV Assessment vehicle type
        public long ParamNumber { get; set; }
        public int VehicleType { get; set; }

        //for Claculated and Manual HB to SV conversion factors
        public int SVDerivation { get; set; }
        public double? CalculatedFactor { get; set; }
        public double? ManualInputFactor { get; set; }


    }

    public class ICACalculation
    {
        public int Status { get; set; }
    }
}