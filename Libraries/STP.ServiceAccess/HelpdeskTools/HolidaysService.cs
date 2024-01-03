using STP.Common.Logger;
using STP.Domain;
using STP.Domain.HelpdeskTools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.HelpdeskTools
{
    public class HolidaysService : IHolidaysService
    {
        private readonly HttpClient httpClient;

        public HolidaysService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region public Object GetAllHolidays()
        public List<HolidaysDomain> GetAllHolidays(int pageNumber, int pageSize, int flag, string MonthYear, string searchType, int sortOrder, int presetFilter)
        {
            List<HolidaysDomain> holidaysDomainsList = new List<HolidaysDomain>();
            try
            {
                string urlParameters = "?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&flag=" + flag + "&MonthYear=" + MonthYear + "&searchType=" + searchType + "&sortOrder=" + sortOrder + "&presetFilter=" + presetFilter;
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(urlParameters);
                
                                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["HelpdeskTools"]}" +
                                                    $"/Holidays/GetAllHolidays{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    holidaysDomainsList = response.Content.ReadAsAsync<List<HolidaysDomain>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/GetAllHolidays, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/GetAllHolidays, Exception: {ex}");
            }

            return holidaysDomainsList;
        }
        #endregion

        #region public int InsertHolidayInfo()
        public int InsertHolidayInfo(string holidayDate, string description, int countryId)
        {
            int result = 0;
            try
            {
                string urlParameters = "?holidayDate=" + holidayDate + "&description=" + description + "&countryId=" + countryId;
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["HelpdeskTools"]}" +
                                                   $"/Holidays/InsertHolidayInfo{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/InsertHolidayInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/InsertHolidayInfo, Exception: {ex}");
            }

            return result;
        }
        #endregion

        #region public int DeleteHolidayDetails()
        public bool DeleteHolidayDetails(long holidayId)
        {
            bool success = false;
            try
            {
                string urlParameters = "?holidayId=" + holidayId;
                HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["HelpdeskTools"]}" +
                                   $"/Holidays/DeleteHolidayDetails{urlParameters}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    success = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/DeleteHolidayDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/DeleteHolidayDetails, Exception: {ex}");
            }

            return success;
        }
        #endregion

        #region public int EditholidayInfo()
        public int EditholidayInfo(long HOLIDAY_ID, string HolidayDate, string DESCRIPTION, int COUNTRY_ID)
        {
            int result = 0;
            try
            {
                HolidayParams objHolidayParams = new HolidayParams
                {
                    HolidayId = HOLIDAY_ID,
                    HolidayDate = HolidayDate,
                    Description = DESCRIPTION,
                    CountryId = COUNTRY_ID
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["HelpdeskTools"]}" +
                    $"/Holidays/EditholidayInfo",
                    objHolidayParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/EditholidayInfo, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }


            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Holidays/EditholidayInfo, Exception: {ex}");
            }
            return result;
        }
        #endregion
    }
}
