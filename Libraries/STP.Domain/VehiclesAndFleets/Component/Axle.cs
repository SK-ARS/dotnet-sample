using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.VehiclesAndFleets.Component
{
    public partial class Axle
    {
        public int AxleNumId { get; set; }
        public int NoOfWheels { get; set; }
        public double AxleWeight { get; set; }
        public double DistanceToNextAxle { get; set; }
        public string TextDescription { get; set; }
        public int AxleDriven { get; set; }
        public int BogieAxle { get; set; }
        public string TyreCenters { get; set; }
        [RegularExpression(@"[a-z0-9/ ]+$", ErrorMessage = "Only alphanumeric characters and '/' are allowed")]
        public string TyreSize { get; set; }
        public int ComponentId { get; set; }

    }
}