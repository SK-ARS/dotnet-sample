using Newtonsoft.Json;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.RoutePlannerInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.RoutePlannerInterface
{
    public class RoutePlannerInterfaceService: IRoutePlannerInterfaceService
    {
        private readonly HttpClient httpClient;
        public RoutePlannerInterfaceService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region GetRoutedata
        public RouteData GetRouteData(RouteViaWaypointEx routeViaPointEx)
        {
            string json = JsonConvert.SerializeObject(routeViaPointEx);
            RouteData routeData = null;
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/RoutePlanner/PostEx", routeViaPointEx).Result;
            if (response.IsSuccessStatusCode)
            {
                routeData = response.Content.ReadAsAsync<RouteData>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"RoutePlanner/PostEx, Error: {(int)response.StatusCode} - ​​​​​​​{response.ReasonPhrase}");
            }
            var jsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(routeViaPointEx);
            return routeData;
        }
        #endregion
    }
}
