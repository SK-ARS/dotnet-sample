using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Models
{
    public class HolidayParams
    {
        public long HolidayId { get; set; }

        public string HolidayDate { get; set; }

        public string Description { get; set; }

        public int CountryId { get; set; }
    }
}