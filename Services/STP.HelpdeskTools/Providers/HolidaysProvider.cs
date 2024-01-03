using STP.Domain.HelpdeskTools;
using STP.HelpdeskTools.Interface;
using STP.HelpdeskTools.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Providers
{
    public class HolidaysProvider : IHolidays
    {
        #region HolidaysProvider Singleton

        private HolidaysProvider()
        {
        }
        public static HolidaysProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly HolidaysProvider instance = new HolidaysProvider();
        }
        #endregion

        #region Get All Holidays
        public List<HolidaysDomain> GetAllHolidays(int pageNumber, int pageSize,int flag, string monthYear, string searchType, int sortOrder, int presetFilter)
        {
            return HolidaysDAO.GetAllHoliday(pageNumber, pageSize,flag, monthYear, searchType, sortOrder, presetFilter);
        }
        #endregion

        #region  Insert Holiday Information
        public int InsertHolidayInfo(string holidayDate, string description, int countryId)
        {
            return HolidaysDAO.InsertHolidaysInfo(holidayDate, description, countryId);
        }
        #endregion

        #region Delete Holiday Details
        public int DeleteHolidayDetails(long holidayId)
        {
            return HolidaysDAO.DeleteHolidayInfo(holidayId);
        }
        #endregion

        #region Edit holiday Info
        public int EditholidayInfo(HolidayParams objHolidayParams)
        {
            return HolidaysDAO.EditHolidaysInfo(objHolidayParams);
        }
        #endregion
    }
}