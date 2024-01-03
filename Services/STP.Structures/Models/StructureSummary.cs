using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Structures.Models
{
    public class StructureSummary
    {
        //Structure Number
        public double StructureId { get; set; }
        //Refference Number
        public string StructCode { get; set; }
        //Structure Name
        public string StructureName { get; set; }
        //Structure Class
        public string StructureClass { get; set; }
        //Structure section number
        public int SectionId { get; set; }
        //Struction Section class
        public string SectionClass { get; set; }

        /// <summary>
        /// Store multiple StructCode
        /// Added by - NetWeb
        /// </summary>
        public string StructCodes { get; set; }
        /// <summary>
        /// store character(s) to bind a string.
        /// </summary>
        public string BindBy { get; set; }

        /// <summary>
        /// Struct code selection
        /// </summary>
        public bool StructCodeSelected { get; set; }

        /// <summary>
        /// Store previouse selected structure checkbox value
        /// </summary>
        public bool PreStructCodeValue { get; set; }
        /// <summary>
        /// store previouse selected structure codes.
        /// </summary>
        public string PreviousStructCodes { get; set; }

        public decimal TOTAL_RECORD_COUNT { get; set; }

        public int TotalCount { get; set; }

        public string ButtonName { get; set; }
    }

    public class DelegationArrangment
    {
        public string name { get; set; }
        public long arrangmentID { get; set; }
    }
}