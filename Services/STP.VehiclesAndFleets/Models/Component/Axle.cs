using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.VehiclesAndFleets.Models.Component
{
    public class Axle
    {
        //Axle Number
        public int AxleNumId { get; set; }
        //number of wheels
        public int NoOfWheels { get; set; }
        //Axle weight
        public double AxleWeight { get; set; }
        //Distance to next axle
        public double DistToNextAxle { get; set; }
        //Description
        public string TeXtDesription { get; set; }
        //Axle driven
        public int AxleDriven { get; set; }
        //Bogie Axle
        public int BogieAxle { get; set; }
        //Tyre centers
        public string TyreCenters { get; set; }
        //Tyre Size
        [RegularExpression(@"[a-z0-9/ ]+$", ErrorMessage = "Only alphanumeric characters and '/' are allowed")]
        public string TyreSize { get; set; }
        //component id
        public int CompID { get; set; }

    }
}