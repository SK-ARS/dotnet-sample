using STP.Domain;
using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.HelpdeskTools
{
    public interface IHolidaysService
    {
        List<HolidaysDomain> GetAllHolidays(int pageNumber, int pageSize,int flag, string MonthYear, string searchType, int sortOrder, int presetFilter);

        int InsertHolidayInfo(string HolidayDate, string DESCRIPTION, int COUNTRY_ID);

        bool DeleteHolidayDetails(long holidayId);

        int EditholidayInfo(long HOLIDAY_ID, string HolidayDate, string DESCRIPTION, int COUNTRY_ID);
    }
}
