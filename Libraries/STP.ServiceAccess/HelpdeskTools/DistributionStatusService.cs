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
    public class DistributionStatusService : IDistributionStatusService
    {
        private readonly HttpClient httpClient;

        public DistributionStatusService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region public List<Object> GetDistributionAlert()
        public List<DistributionAlerts> GetDistributionAlert(int pageNumber, int? pageSize, DistributionAlerts objDistributionAlert, int portalType
            , int? presetFilter = null, int? sortOrder = null)
        {

            List<DistributionAlerts> objListDistAlerts = new List<DistributionAlerts>();
            try
            {
                DistributionAlertsParams objDistributionParams = new DistributionAlertsParams
                {
                    PageNo = pageNumber,
                    PageSize = (int)pageSize,
                    PortalType = portalType,
                    //PresetFilter = (int)presetFilter,
                    PresetFilter = presetFilter== null ? 0 : (int)presetFilter,
                    SortOrder = sortOrder == null ? 0 : (int)sortOrder,
                    ObjDistributionAlert = new DistributionAlerts()
                    {
                        StartDate = objDistributionAlert.StartDate,
                        EndDate = objDistributionAlert.EndDate,
                        ESDALReference = objDistributionAlert.ESDALReference,
                        MovementData = objDistributionAlert.MovementData,
                        ShowAlert = objDistributionAlert.ShowAlert,
                        ToOrganisationName = objDistributionAlert.ToOrganisationName,
                        SearchFlag = objDistributionAlert.SearchFlag,
                        //PresetFilter = (int)presetFilter,
                        PresetFilter = presetFilter == null ? 0 : (int)presetFilter,
                        SortOrder = objDistributionAlert.SortOrder


                    }
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                    $"{ConfigurationManager.AppSettings["HelpdeskTools"]}" +
                    $"/DistributionStatus/GetDistributionAlerts",
                    objDistributionParams).Result;
               
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    objListDistAlerts = response.Content.ReadAsAsync<List<DistributionAlerts>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"DistributionStatus/ViewDistributionStatus, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
               

            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"DistributionStatus/ViewDistributionStatus, Exception: {ex}");
            }
            return objListDistAlerts;
        }
        #endregion
    }
}
