using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Applications
{
    public class ComponentGroupingModel
    {
        public string VehicleDesc { get; set; }
        public string VehicleName { get; set; }
        public double GrossWeight { get; set; }
        public double? MaxAxleWeight { get; set; }
        public double? AxleWeight { get; set; }
        public Int32? WheelsPerAxle { get; set; }
        public double AxleSpacing { get; set; }
        public double? AxleSpacing2 { get; set; }
        public string TyreSize { get; set; }
        public string TyreCentreSpacing { get; set; }
        public double Wheelbase { get; set; }
        public double? RearOverhang { get; set; }
        public double? GroundClearance { get; set; }
        public double? OutsideTrack { get; set; }
    }
    public class ComponentObjListModelToreturn
    {
        public string VehicleDesc { get; set; }
        public string VehicleName { get; set; }
        public double GrossWeight { get; set; }
        public double? MaxAxleWeight { get; set; }
        public string AxleWeight { get; set; }
        public string WheelsPerAxle { get; set; }
        public string AxleSpacing { get; set; }
        public string AxleSpacing2 { get; set; }
        public string TyreSize { get; set; }
        public string TyreCentreSpacing { get; set; }
        public double WheelBase { get; set; }
        public double? RearOverhang { get; set; }
        public double GroundClearance { get; set; }
        public double? OutsideTrack { get; set; }
    }
}
