using STP.Common.Logger;
using STP.Domain.StructureUpdate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.StructureUpdates 
{
   public  class StructureUpdateService : IStructureUpdateService
    {
        private readonly HttpClient _httpClient;

        public StructureUpdateService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public List<SUProject> GetSUProjectList(SUProjectListParams suListParam)
        {
            List<SUProject> summaryObjList = new List<SUProject>();
            try
            {
                //SUProjectListParams structureListParams = new SUProjectListParams
                //{
                //    DataOwnerName = suListParam.orgId,
                //    PageNumber = pageNum,
                //    PageSize = pageSize,
                //    ObjSearchStructure = objSearchStruct
                //};
                HttpResponseMessage response = _httpClient.PostAsJsonAsync(
                           $"{ConfigurationManager.AppSettings["StructureUpdate"]}" +
                           $"/StructureUpdates/GetSUProjectList",
                           suListParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    summaryObjList = response.Content.ReadAsAsync<List<SUProject>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureListSearch, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Structures/GetStructureListSearch, Exception: {0}", ex);
            }
            return summaryObjList;
        }
    }
}
