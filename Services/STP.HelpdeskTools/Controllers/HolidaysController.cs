using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.HelpdeskTools;
using STP.HelpdeskTools.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace STP.HelpdeskTools.Controllers
{
    public class HolidaysController : ApiController
    {
        #region Get All Holidays
        [HttpGet]
        [Route("Holidays/GetAllHolidays")]
        public IHttpActionResult  GetAllHolidays(int pageNumber,int pageSize,int flag, string monthYear, string searchType, int sortOrder, int presetFilter)
        {
            List<HolidaysDomain> holidaysDomainsList = new List<HolidaysDomain>();
            try
            {
                holidaysDomainsList = HolidaysProvider.Instance.GetAllHolidays(pageNumber, pageSize, flag, monthYear, searchType, sortOrder, presetFilter);
                return Content(HttpStatusCode.OK, holidaysDomainsList);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Holidays/GetAllHolidays,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  Insert Holiday Information
        [HttpGet]
        [Route("Holidays/InsertHolidayInfo")]
        public IHttpActionResult InsertHolidayInfo(string holidayDate, string description, int countryId)
        {
            int result = 0;
            try
            {
                result = HolidaysProvider.Instance.InsertHolidayInfo(holidayDate, description, countryId);
                  return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Holidays/InsertHolidayInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }

        }
        #endregion

        #region Delete Holiday Details
        [HttpDelete]
        [Route("Holidays/DeleteHolidayDetails")]
        public IHttpActionResult DeleteHolidayDetails(long holidayId)
        {
            int status = 0;
            try
            {
                status = HolidaysProvider.Instance.DeleteHolidayDetails(holidayId);
                if(status > -1)
                    return Content(HttpStatusCode.OK, status);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
               
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Holidays/DeleteHolidayDetails,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Edit holiday Info
        [HttpPost]
        [Route("Holidays/EditholidayInfo")]
        public IHttpActionResult EditholidayInfo(HolidayParams objHolidayParams)
        {
            int result = 0;
            try
            {
                result = HolidaysProvider.Instance.EditholidayInfo(objHolidayParams);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Holidays/EditholidayInfo,Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
