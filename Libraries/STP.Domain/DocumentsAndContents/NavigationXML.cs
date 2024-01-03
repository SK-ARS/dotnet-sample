using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class NavigationXML
    {
        public string Instruction { get; set; }
        public decimal MeasuredMetric { get; set; }
        public decimal DisplayMetric { get; set; }
        public decimal DisplayImperial { get; set; }
        public string YardMiles { get; set; }
        public string Direction { get; set; }
    }
}