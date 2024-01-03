using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.HelpdeskTools
{
    public class HolidaysDomain
    {
        public int ListCount { get; set; }

        public int CountryId { get; set; }

        public string Descripton { get; set; }

        public long HolidayId { get; set; }

        public string HolidayDate { get; set; }

        public string SearchType { get; set; }

        public string CountryName { get; set; }

        public string HighlightedDate { get; set; }

    }
}