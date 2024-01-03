using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Interface
{
    public interface IHolidays
    {
        List<HolidaysDomain> GetAllHolidays(int pageNumber, int pageSize, int flag, string monthYear, string searchType, int sortOrder, int presetFilter);

        int InsertHolidayInfo(string holidayDate, string description, int countryId);

        int DeleteHolidayDetails(long holidayId);

        int EditholidayInfo(HolidayParams objHolidayParams);
    }
}