using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Models
{
    public class HolidaysDomain
    {
        public int listCount { get; set; }

        public int COUNTRY_ID { get; set; }

        public string DESCRIPTION { get; set; }

        public long HOLIDAY_ID { get; set; }

        public string HolidayDate { get; set; }

        public string searchType { get; set; }

        public string CountryName { get; set; }
    }
}