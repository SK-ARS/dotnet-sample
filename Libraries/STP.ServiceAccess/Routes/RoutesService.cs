using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;
using STP.Domain.Routes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using static STP.Domain.Routes.RouteModel;
using static STP.Domain.Routes.RouteSerialization;

namespace STP.ServiceAccess.Routes
{
    public class RoutesService : IRoutesService
    {
        private readonly HttpClient httpClient;
        private readonly string Routes;
        public RoutesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            Routes = ConfigurationManager.AppSettings["Routes"];
        }
        #region Library Route Scenario

        #region Library Route List
        public List<RoutePartDetails> LibraryRouteList(int organisationID, int pageNumber, int pageSize, int routeType, string serchString, string userSchema, int filterFavouritesRoutes, int presetFilter,int? sortOrder=null)
        {
            LibraryRouteListParams listParams = new LibraryRouteListParams
            {
                OrganisationId = organisationID,
                PageNumber = pageNumber,
                PageSize = pageSize,
                RouteType = routeType,
                SerchString = serchString,
                UserSchema = userSchema,
                FilterFavouritesRoutes = filterFavouritesRoutes,
                presetFilter = presetFilter,
                sortOrder=sortOrder
            };
            List<RoutePartDetails> libraryRouteList = new List<RoutePartDetails>();
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/LibraryRouteList", listParams).Result;
            if (response.IsSuccessStatusCode)
            {
                libraryRouteList = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/LibraryRouteList, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return libraryRouteList;
        }
        #endregion

        #region Get Library Route
        public RoutePart GetLibraryRoute(long plannedRouteID, string userSchema)
        {
            RoutePart routePart = new RoutePart();
            string urlParameters = "?routeId=" + plannedRouteID + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetLibraryRoute{ urlParameters}").Result;
            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                routePart = response.Content.ReadAsAsync<RoutePart>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetLibraryRoute, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routePart;
        }
        #endregion

        #region Save Library Route
        public long SaveLibraryRoute(RoutePart routePart, string userSchema)
        {
            long routeId = 0;
            Serialization serialization = new Serialization();
            routePart = serialization.SerializeRoutePart(routePart);
            SaveLibraryRouteParams plannedRoutePath = new SaveLibraryRouteParams
            {
                RoutePart = routePart,
                UserSchema = userSchema
            };
            httpClient.Timeout = TimeSpan.FromMinutes(10); 
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Routes/SaveLibraryRoute, Starting the SaveLibraryRoute call for route :" + routePart.RoutePartDetails.RouteName);
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/SaveLibraryRoute", plannedRoutePath).Result;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Routes/SaveLibraryRoute, Received response for SaveLibraryRoute call for route :" + routePart.RoutePartDetails.RouteName);
            if (response.IsSuccessStatusCode)
            {
                routeId = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveLibraryRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routeId;
        }
        #endregion

        #region Update Library Route
        public long UpdateLibraryRoute(RoutePart routePart, string userSchema)
        {
            long routeId = 0;
            Serialization serialization = new Serialization();
            routePart = serialization.SerializeRoutePart(routePart);
            SaveLibraryRouteParams plannedRoutePath = new SaveLibraryRouteParams
            {
                RoutePart = routePart,
                UserSchema = userSchema
            };
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Routes/UpdateLibraryRoute, Starting the UpdateLibraryRoute call for route :" + routePart.RoutePartDetails.RouteName);
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/UpdateLibraryRoute", plannedRoutePath).Result;
            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, @"Routes/UpdateLibraryRoute, Received response for UpdateLibraryRoute call for route :" + routePart.RoutePartDetails.RouteName);
            if (response.IsSuccessStatusCode)
            {
                routeId = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/UpdateLibraryRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routeId;
        }
        #endregion

        #region Delete Library Route
        public int DeleteLibraryRoute(long plannedRouteID, string userSchema)
        {
            int result = 0;
            string urlParameters = "?routeId=" + plannedRouteID + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.DeleteAsync(
                $"{ConfigurationManager.AppSettings["Routes"]}" +
                $"/Routes/DeleteLibraryRoute{urlParameters}").Result;

            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/DeleteLibraryRoute, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region Add Route To Library
        public long AddRouteToLibrary(long routePartId, int orgId, string routeType, string userSchema)
        {
            long routeId = 0;
            AddToLibraryParams routeParams = new AddToLibraryParams
            {
                OrganisationId = orgId,
                RouteId = routePartId,
                RouteType = routeType,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/AddRouteToLibrary", routeParams).Result;
            if (response.IsSuccessStatusCode)
            {
                routeId = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/AddRouteToLibrary, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routeId;
        }
        public int CheckRouteName(string routeName, int organisationId, string userSchema)
        {
            int result = 0;
            CheckRouteName chkRouteName = new CheckRouteName
            {
                OrganisationId = organisationId,
                RouteName = routeName,
                UserSchema = userSchema
            };

            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/CheckRouteName", chkRouteName).Result;

            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/CheckRouteName, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #endregion

        #region Application Route Scenario

        #region Get Notif/Application Route
        public RoutePart GetApplicationRoute(long routePartId, string userSchema)
        {
            string strUrlParameter = "?routeId=" + routePartId + "&userSchema=" + userSchema;
            RoutePart routePart = new RoutePart();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["Routes"]}" +
                     $"/Routes/GetApplicationRoute{strUrlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    routePart = response.Content.ReadAsAsync<RoutePart>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetApplicationRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetApplicationRoute, Exception: {ex}");
            }

            return routePart;
        }
        #endregion

        #region Get Historic Notif/Application Route
        public RoutePart GetHistoricAppRoute(long routePartId, string userSchema)
        {
            string strUrlParameter = "?routeId=" + routePartId + "&userSchema=" + userSchema;
            RoutePart routePart = new RoutePart();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["Routes"]}" +
                     $"/Routes/GetHistoricAppRoute{strUrlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    routePart = response.Content.ReadAsAsync<RoutePart>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetHistoricAppRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetHistoricAppRoute, Exception: {ex}");
            }

            return routePart;
        }
        #endregion

        #region SaveApplicationRoute
        public long SaveApplicationRoute(RoutePart routePart, long versionId, long revisionId, string contRefNum, int dockFlag, long routeRevisionid, string userSchema, bool IsReturnRoute = false)
        {
            long routeId = 0;
            Serialization serialization = new Serialization();
            routePart = serialization.SerializeRoutePart(routePart);
            SaveAppRouteParams saveAppRouteParams = new SaveAppRouteParams
            {
                RoutePart = routePart,
                VersionId = versionId,
                RevisionId = revisionId,
                ContRefNumber = contRefNum,
                DockFlag = dockFlag,
                RouteRevisionId = routeRevisionid,
                UserSchema = userSchema,
                IsReturnRoute = IsReturnRoute,
                IsAutoPlan = false
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/SaveApplicationRoute", saveAppRouteParams).Result;
            if (response.IsSuccessStatusCode)
            {
                routeId = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveApplicationRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routeId;
        }
        #endregion

        #region UpdateApplicationRoute
        public long UpdateApplicationRoute(RoutePart routePart, long versionId, long revisionId, string contRefNum, int dockFlag, long routeRevisionid, string userSchema)
        {
            long routeId = 0;
            Serialization serialization = new Serialization();
            routePart = serialization.SerializeRoutePart(routePart);
            SaveAppRouteParams saveAppRouteParams = new SaveAppRouteParams
            {
                RoutePart = routePart,
                VersionId = versionId,
                RevisionId = revisionId,
                ContRefNumber = contRefNum,
                DockFlag = dockFlag,
                RouteRevisionId = routeRevisionid,
                UserSchema = userSchema,
                IsAutoPlan = false
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/UpdateApplicationRoute", saveAppRouteParams).Result;
            if (response.IsSuccessStatusCode)
            {
                routeId = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/UpdateApplicationRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routeId;
        }
        #endregion

        #region Delete Application Route
        public int DeleteApplicationRoute(long routeId, string routeType, string userSchema)
        {
            int result = 0;
            string urlParameters = "/Routes/DeleteApplicationRoute?routeId=" + routeId + "&routeType=" + routeType.ToLower() + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.DeleteAsync(Routes + urlParameters).Result;

            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/DeleteApplicationRoute, StatusCode:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region Update Clone Historic Route
        public List<long> UpdateCloneHistoricRoute(string contentRefNum, long appRevisionId, long versionId, string userSchema)
        {
            List<long> routeIds = new List<long>();
            UpdateHistoricCloneRoute historicCloneRoute = new UpdateHistoricCloneRoute
            {
                VersionId = versionId,
                RevisionId = appRevisionId,
                ContRefNumber = contentRefNum,
                UserSchema = userSchema,
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/UpdateCloneHistoricAppRoute", historicCloneRoute).Result;
            if (response.IsSuccessStatusCode)
            {
                routeIds = response.Content.ReadAsAsync<List<long>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/UpdateCloneHistoricAppRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routeIds;
        }
        #endregion

        #endregion

        #region ListRouteImportDetails
        public List<NotifRouteImport> ListRouteImportDetails(string contentRefNo)
        {
            List<NotifRouteImport> result = new List<NotifRouteImport>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}"
                                                                         + $"/Routes/ListRouteImportDetails?contentRefNo=" + contentRefNo).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<NotifRouteImport>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/ListRouteImportDetails, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/ListRouteImportDetails, Exception: {ex}");
            }
            return result;
        }
        #endregion        

        #region bool SaveRouteAnnotation()
        public bool SaveRouteAnnotation(RoutePart routePart, int type, string userSchema)
        {
            bool saveFlag = false;
            try
            {
                Serialization serialization = new Serialization();
                routePart = serialization.SerializeRoutePart(routePart);
                SaveRouteAnnotationParams objSaveRouteAnnotation = new SaveRouteAnnotationParams
                {
                    routePart = routePart,
                    type = type,
                    userSchema = userSchema
                };
              
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                  $"{ConfigurationManager.AppSettings["Routes"]}" +
                  $"/Routes/SaveRouteAnnotation",
                  objSaveRouteAnnotation).Result;
                var jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(objSaveRouteAnnotation);
                if (response.IsSuccessStatusCode)
                {
                    saveFlag = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveRouteAnnotation, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("Routes/SaveRouteAnnotation, Exception: {0}", ex));
            }
            return saveFlag;
        }
        #endregion

        #region  public  long GetRoutePathId()
        public long GetRoutePathId(long routeId, int isLib, string userSchema = UserSchema.Portal)
        {
            long result = 0;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}"
                                                                         + $"/Routes/GetRoutePathId?routeId=" + routeId + "&isLib=" + isLib + "&userSchema=" + userSchema).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetRoutePathId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetRoutePathId, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region List<RoutePoint> GetRoutePoints()
        public List<RoutePoint> GetRoutePoints(long routePathId, string userSchema)
        {
            List<RoutePoint> result = new List<RoutePoint>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}"
                                                                         + $"/Routes/GetRoutePoints?routePathId=" + routePathId + "&userSchema=" + userSchema).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<RoutePoint>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetRoutePoints, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetRoutePoints, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  SaveSNotificationRoute()
        public long SaveSNotificationRoute(int routepartId, string ContentRefNo)
        {
            long result = 0;
            try
            {
                SaveSNotificationRouteParam saveSNotificationRouteParam = new SaveSNotificationRouteParam()
                {
                    routepartId = routepartId,
                    ContentRefNo = ContentRefNo
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/SaveSNotificationRoute", saveSNotificationRouteParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveSNotificationRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveSNotificationRoute, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  UpdateNotifPlanRoute()
        public bool UpdateNotifPlanRoute(int RoutePartId, string contentrefno, int RoutePartNo, int ImportVeh = 0, int Flag = 0)
        {
            bool result = false;
            try
            {
                UpdateNotifPlanRouteParam updateNotifPlanRouteParam = new UpdateNotifPlanRouteParam()
                {
                    RoutePartId = RoutePartId,
                    ContentReferenceNo = contentrefno,
                    RoutePartNo = RoutePartNo,
                    ImportVehicle = ImportVeh,
                    Flag = Flag
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/UpdateNotifPlanRoute", updateNotifPlanRouteParam).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/UpdateNotifPlanRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/UpdateNotifPlanRoute, Exception: {ex}");
            }
            return result;
        }
        #endregion     

        #region  VerifyApplicatiponRouteNameValidation()
        public int VerifyApplicatiponRouteNameValidation(string RouteName, int RevisionId, string ContentRefNo, int RouteFor, string UserSchema)
        {
            int count = 0;
            try
            {
                ApplicationRouteNameParams objApplicationRouteNameParams = new ApplicationRouteNameParams()
                {
                    RouteName = RouteName,
                    RevisionId = RevisionId,
                    ContentRefNo = ContentRefNo,
                    RouteFor = RouteFor,
                    UserSchema = UserSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/verifyApplicationRouteName", objApplicationRouteNameParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    count = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/VerifyApplicationRouteName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/VerifyApplicationRouteName, Exception: {ex}");
            }
            return count;
        }
        #endregion

        #region GetRouteDetails
        public List<RoutePoint> GetRouteDetails(string ContentRefNo)
        {

            List<RoutePoint> rp = new List<RoutePoint>();
            string urlParameters = "?contentReferenceNo=" + ContentRefNo;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetRouteDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                rp = response.Content.ReadAsAsync<List<RoutePoint>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetRouteDetails, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return rp;
        }
        #endregion

        #region GetRoutePartsCount
        public int GetRoutePartsCount(string ContentRefNo)
        {

            int count = 0;
            string urlParameters = "?contentReferenceNo=" + ContentRefNo;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetRoutePartsCount{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                count = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetRoutePartsCount, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return count;
        }
        #endregion

        #region SaveNotificationRoute
        public long SaveNotificationRoute(int routePartId, int versionId, string contentRefNo, int routeType)
        {
            long routePartID = 0;
            SaveNotificationRouteParam saveNotificationRouteParam = new SaveNotificationRouteParam()
            {
                routePartId = routePartId,
                versionId = versionId,
                contentRefNo = contentRefNo,
                routeType = routeType
            };

            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/SaveNotificationRoute", saveNotificationRouteParam).Result;

            if (response.IsSuccessStatusCode)
            {
                routePartID = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SaveNotificationRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routePartID;
        }
        #endregion

        #region  SaveSOApplicationRoute
        public long SaveSOAppImportRoute(int routePartId, int appRevId, int routeType, string userSchema)
        {
            long result = 0;
            try
            {
                SaveApplicationRouteParams saveApplicationRouteParams = new SaveApplicationRouteParams()
                {
                    routePartId = routePartId,
                    appRevId = appRevId,
                    routeType = routeType,
                    userSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/SaveSOAppImportRoute", saveApplicationRouteParams).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveSOAppImportRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveSOAppImportRoute, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  ImportRouteFromLibrary
        public long ImportRouteFromLibrary(int routePartId, int versionId, int appRevId, int routeType, string contentRef, string userSchema)
        {
            long result = 0;
            try
            {
                SaveApplicationRouteParams saveApplicationRoute = new SaveApplicationRouteParams()
                {
                    routePartId = routePartId,
                    versionId = versionId,
                    appRevId = appRevId,
                    routeType = routeType,
                    contentRef = contentRef,
                    userSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/ImportRouteFromLibrary", saveApplicationRoute).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveVR1AppImportRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveVR1AppImportRoute, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  SaveRouteInRouteParts
        public long SaveRouteInRouteParts(int routePartId, int appRevId, int versionId, string contentRef, string userSchema)
        {
            long result = 0;
            try
            {
                SaveApplicationRouteParams saveApplicationRoute = new SaveApplicationRouteParams()
                {
                    routePartId = routePartId,
                    versionId = versionId,
                    appRevId = appRevId,
                    contentRef = contentRef,
                    userSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/SaveRouteInRouteParts", saveApplicationRoute).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveRouteInRouteParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveRouteInRouteParts, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  SaveRouteInAppParts
        public long SaveRouteInAppParts(int routePartId, int appRevId, string userSchema)
        {
            long result = 0;
            try
            {
                SaveApplicationRouteParams saveApplicationRoute = new SaveApplicationRouteParams()
                {
                    routePartId = routePartId,
                    appRevId = appRevId,
                    userSchema = userSchema
                };

                HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/SaveRouteInAppParts", saveApplicationRoute).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<long>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveRouteInAppParts, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveRouteInAppParts, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region GetRoutePointsDetails
        public List<RoutePoint> GetRoutePointsDetails(int PlanRouteID)
        {

            List<RoutePoint> rp = new List<RoutePoint>();
            string urlParameters = "?PlanRouteID=" + PlanRouteID;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetRoutePointsDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                rp = response.Content.ReadAsAsync<List<RoutePoint>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetRoutePointsDetails, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return rp;
        }
        #endregion

        #region GetRoutePartId
        public long GetRoutePartId(string conRefNumber, string userSchema)
        {

            long result = 0;
            string urlParameters = "?conRefNumber=" + conRefNumber + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetRoutePartId{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetRoutePartId, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region DeleteOldRouteDetails
        public int DeleteOldRouteDetails(long newRoutePartId, string contentRefNo, int oldRoutePartId)
        {
            int result = 0;
            string urlParameters = "?newRoutePartId=" + newRoutePartId + "&contentRefNo=" + contentRefNo + "&oldRoutePartId = " + oldRoutePartId;
            HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/DeleteOldRouteDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/DeleteOldRouteDetails, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region UpdateRoutePartId
        public int UpdateRoutePartId(long newRoutePartId, int oldRoutePartId, string contentRefNo)
        {

            int result = 0;
            string urlParameters = "?newRoutePartId=" + newRoutePartId + "&contentRefNo=" + contentRefNo + "&oldRoutePartId = " + oldRoutePartId;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/UpdateRoutePartId{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/DeleteOldRouteDetails, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region GetNotifRouteDetails
        public List<ListRouteVehicleId> GetNotifRouteDetails(string contentRefNo)
        {

            List<ListRouteVehicleId> result = new List<ListRouteVehicleId>();
            string urlParameters = "?contentRefNo=" + contentRefNo;

            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetNotifRouteDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<List<ListRouteVehicleId>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetNotifRouteDetails, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region DeleteOldReturnLeg
        public int DeleteOldReturnLeg(string contentRefNo)
        {
            int result = 0;
            string urlParameters = "?contentRefNo=" + contentRefNo;
            HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/DeleteOldReturnLeg{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/DeleteOldReturnLeg, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region GetRoutePointsForReturnLeg
        public List<RoutePoint> GetRoutePointsForReturnLeg(int libraryRouteId, long planRouteId)
        {

            List<RoutePoint> result = new List<RoutePoint>();
            string urlParameters = "?libraryRouteId=" + libraryRouteId + "&planRouteId=" + planRouteId;

            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetRoutePointsForReturnLeg{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<List<RoutePoint>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetRoutePointsForReturnLeg, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region DeleteOldRouteDetailsForImport
        public int DeleteOldRouteDetailsForImport(long newRoutePartId, string contentRefNo, int routePartNo)
        {

            int result = 0;
            string urlParameters = "?newRoutePartId=" + newRoutePartId + "&contentRefNo=" + contentRefNo + "&routePartNo = " + routePartNo;
            HttpResponseMessage response = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/DeleteOldRouteDetailsForImport{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/DeleteOldRouteDetailsForImport, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion                    

        #region Authorized Route Part List
        public List<AppRouteList> GetAuthorizedRoutePartList(long versionId, string userSchema)
        {
            List<AppRouteList> soAppRouteList = new List<AppRouteList>();
            string urlParameters = "?versionId=" + versionId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetAuthorizedRoutePartList{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                soAppRouteList = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetAuthorizedRoutePartList, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return soAppRouteList;
        }
        #endregion

        #region Get Planned NEN Route List for SOA/Police
        public List<AppRouteList> GetPlannedNenRouteList(long nenId, int userId, long inboxItemId, int orgId)
        {
            List<AppRouteList> nenRouteList = new List<AppRouteList>();
            string urlParameters = "?nenId=" + nenId + "&userId=" + userId + "&inboxItemId=" + inboxItemId + "&orgId=" + orgId;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetPlannedNenRouteList{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                nenRouteList = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetPlannedNenRouteList, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return nenRouteList;
        }
        #endregion

        #region SO App Route List
        public List<AppRouteList> GetSoAppRouteList(long revisionId, string userSchema)
        {
            List<AppRouteList> soRouteList = new List<AppRouteList>();
            string urlParameters = "?revisionId=" + revisionId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetSoAppRouteList{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                soRouteList = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetSoAppRouteList, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return soRouteList;
        }
        #endregion

        #region VR1/Notification Route List
        public List<AppRouteList> NotifVR1RouteList(long revisionId, string contRefNum, long versionId, string userSchema)
        {
            List<AppRouteList> result = new List<AppRouteList>();
            //api call to new service
            string urlParameters = "?revisionId=" + revisionId + "&contRefNum=" + contRefNum + "&versionId=" + versionId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/NotifVR1RouteList{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                result = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"/Routes/NotifVR1RouteList, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region Get Outline Candidate Route
        public RoutePart GetCandidateOutlineRoute(long routePartId, string userSchema)
        {
            RoutePart outlineCandRoutepart = new RoutePart();
            string urlParameters = "?routePartId=" + routePartId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetCandidateOutlineRoute{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                outlineCandRoutepart = response.Content.ReadAsAsync<RoutePart>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetCandidateOutlineRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return outlineCandRoutepart;
        }
        #endregion

        #region Outline Notif/App Route
        public RoutePart GetApplicationRoutePartGeometry(long partId, string userSchema)
        {
            RoutePart outlineAppNotifRoutePart = new RoutePart();
            string urlParameters = "?partId=" + partId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetOutlinRoute{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                outlineAppNotifRoutePart = response.Content.ReadAsAsync<RoutePart>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetApplicationRoutePartGeometry, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return outlineAppNotifRoutePart;
        }
        #endregion

        #region Broken Route Scenarios

        #region ListBrokenRouteDetails
        public List<NotifRouteImport> ListBrokenRouteDetails(string contentReferenceNumber = null, string userSchema = UserSchema.Portal, long appRevisionID = 0, long revisionID = 0, long movementVersionID = 0)
        {
            List<NotifRouteImport> responseList = new List<NotifRouteImport>();
            string urlParameters = $"?userSchema={userSchema}&appRevisionID={appRevisionID}&revisionID={revisionID}&movementVersionID={movementVersionID}";
            urlParameters += contentReferenceNumber != null ? $"&contentReferenceNumber={contentReferenceNumber}" : string.Empty;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/ListBrokenRouteDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseList = response.Content.ReadAsAsync<List<NotifRouteImport>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/ListBrokenRouteDetails, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return responseList;
        }
        #endregion

        #region Get Broken RouteIds
        public List<BrokenRouteList> GetBrokenRouteIds(GetBrokenRouteList getBrokenRouteList)
        {
            List<BrokenRouteList> brokenRouteList = new List<BrokenRouteList>();
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/GetBrokenRouteId", getBrokenRouteList).Result;
            if (response.IsSuccessStatusCode)
            {
                brokenRouteList = response.Content.ReadAsAsync<List<BrokenRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetBrokenRouteId, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return brokenRouteList;
        }
        #endregion

        #region Get Broken RouteIds
        public int CheckIsBroken(long routePartId,string userSchema)
        {
            // List<BrokenRouteList> brokenRouteList = new List<BrokenRouteList>();
            int IsBroken = 0;
            GetBrokenRouteList getBrokenRouteList = new GetBrokenRouteList
            {
                RoutePartId = routePartId,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/CheckIsBroken", getBrokenRouteList).Result;
            if (response.IsSuccessStatusCode)
            {
                //IsBroken = response.Content;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/CheckIsBroken, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return IsBroken;
        }
        #endregion

        #region GetBrokenRoutePoints
        public List<RoutePoint> GetBrokenRoutePoints(long routePathId = 0, int isLib = 0, string userSchema = "")
        {

            List<RoutePoint> rp = new List<RoutePoint>();
            string urlParameters = "?routePathId=" + routePathId + "&isLib=" + isLib + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetBrokenRoutePoints{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                rp = response.Content.ReadAsAsync<List<RoutePoint>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetBrokenRoutePoints, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return rp;
        }
        #endregion

        #region GetBrokenRouteAnnotations
        public List<RouteAnnotation> GetBrokenRouteAnnotations(long segmentId, int is_lib, string userSchema)
        {

            List<RouteAnnotation> rp = new List<RouteAnnotation>();
            string urlParameters = "?segmentId=" + segmentId + "&is_lib=" + is_lib + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetBrokenRouteAnnotations{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                rp = response.Content.ReadAsAsync<List<RouteAnnotation>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetBrokenRouteAnnotations, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return rp;
        }
        #endregion

        #region UpdateBrokenRoutePath
        public bool UpdateBrokenRoutePath(List<RoutePath> routePathList, int is_lib, string userSchema = UserSchema.Portal)
        {
            bool result = false;
            Serialization serialization = new Serialization();
            routePathList = serialization.SerializeRoutePath(routePathList);
            UpdateBrokenRoutePathParam brokenRoutePathParam = new UpdateBrokenRoutePathParam()
            {
                RoutePathList = routePathList,
                IsLib = is_lib,
                UserSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/Routes/UpdateBrokenRoutePath", brokenRoutePathParam).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<bool>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/UpdateBrokenRoutePath, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #region SetVerificationStatus
        public int SetVerificationStatus(int routeId, int is_lib, int replanStatus, string userSchema)
        {
            int result = 0;
            VerificationStatusParams objVerificationStatusParams = new VerificationStatusParams()
            {
                routeId = routeId,
                isLib = is_lib,
                replanStatus = replanStatus,
                userSchema = userSchema
            };
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/SetVerificationStatus", objVerificationStatusParams).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/SetVerificationStatus, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }
        #endregion

        #endregion

        #region  SaveMapUsage
        public int SaveMapUsage(int userId, int organisationId, int type)
        {
            int result = 0;
            try
            {

                string urlParameters = "?userId=" + userId + "&organisationId=" + organisationId + "&type=" + type;
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/SaveMapUsage" + urlParameters, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveMapUsage, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveMapUsage, Exception: {ex}");
            }
            return result;
        }


        #endregion

        #region Get favorite routes
        public List<RoutePartDetails> GetFavouriteRoutes(int organisationId, string userSchema)
        {
            string strUrlParameter = "?organisationId=" + organisationId + "&userSchema=" + userSchema;
            List<RoutePartDetails> favRoutes = new List<RoutePartDetails>();
            try
            {
                HttpResponseMessage response = httpClient.GetAsync
                    ($"{ConfigurationManager.AppSettings["Routes"]}" +
                     $"/Routes/GetFavouriteRoutes{strUrlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    favRoutes = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetFavouriteRoutes, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/GetFavouriteRoutes, Exception: {ex}");
            }

            return favRoutes;
        }
        #endregion

        #region SORT Previous/Current Movement RouteList
        public List<AppRouteList> GetSortMovementRoute(long revisionId, int rListType)
        {
            List<AppRouteList> appRouteList = new List<AppRouteList>();
            string urlParameter = "?revisionId=" + revisionId + "&rListType=" + rListType;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetSortMovementRoute{urlParameter}").Result;
                if (response.IsSuccessStatusCode)
                {
                    appRouteList = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetSortMovementRoute, Error:" + (int)response.StatusCode + "-" + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetSortMovementRoute, Exception:" + ex);
            }
            return appRouteList;
        }
        #endregion

        #region SaveNERoute
        public long SaveNERoute(SaveNERouteParams saveNERoute)
        {
            long routeId;
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/RouteImport/InsertNERoute", saveNERoute).Result;
            if (response.IsSuccessStatusCode)
            {
                routeId = response.Content.ReadAsAsync<long>().Result;
            }
            else
            {
                routeId = 0;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- RouteImport/InsertNERoute, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return routeId;
        }
        #endregion

        #region GetRouteDetailForAnalysis
        public List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema)
        {
            string urlParameter = "?versionId=" + versionId + "&contentRefNo=" + contentRefNo + "&revisionId=" + revisionId + "&isCandidate=" + isCandidate + "&userSchema=" + userSchema;
            List<RoutePartDetails> responseData = new List<RoutePartDetails>();
            HttpResponseMessage response = httpClient.GetAsync(Routes + $"/Routes/GetRouteDetailForAnalysis{urlParameter}").Result;
            if (response.IsSuccessStatusCode)
            {
                responseData = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- Routes/GetRouteDetailForAnalysis, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return responseData;
        }
        #endregion

        #region CheckRouteVehicleAttach
        public int CheckRouteVehicleAttach(long routePartId)
        {
            int result = 0;
            string urlParameters = "?routePartId=" + routePartId;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/CheckRouteVehicleAttach{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/CheckRouteVehicleAttach, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;

        }
        #endregion

        #region GetNERouList
        public List<Domain.ExternalAPI.Route> ExportRouteDetails(Domain.ExternalAPI.GetRouteExportList routeExportList)
        {
            List<Domain.ExternalAPI.Route> neRouteList = new List<Domain.ExternalAPI.Route>();
            HttpResponseMessage response = httpClient.PostAsJsonAsync(Routes + $"/RouteExport/ExportRouteList", routeExportList).Result;
            if (response.IsSuccessStatusCode)
            {
                neRouteList = response.Content.ReadAsAsync<List<Domain.ExternalAPI.Route>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance, @"- RouteExport/ExportRouteList, Error:" + (int)response.StatusCode + "ReasonPhrase:" + response.ReasonPhrase);
            }
            return neRouteList;
        }
        #endregion

        #region  SaveAnnottationTextToLibrary
        public int SaveAnnottationTextToLibrary(long organisationId, string userId, long annotationType, string annotationText, long structureId = 0, string userSchema = "portal")
        {
            int result = 0;
            try
            {

                string urlParameters = "?organisationId=" + organisationId + "&userId=" + userId + "&annotationType=" + annotationType + "&annotationText=" + annotationText + "&structureId="+ structureId + "&userSchema=" + "portal";
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/SaveAnnotationToLibrary" + urlParameters, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveMapUsage, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveMapUsage, Exception: {ex}");
            }
            return result;
        }
        #endregion

        #region  GetAnnottationTextListLibrary
        public List<AnnotationTextLibrary> GetAnnottationTextListLibrary(int pageNumber, int pageSize, long organisationId, long userId, long annotationType, string annotationText, long structureId = 0, string userSchema ="portal")
        {
            List<AnnotationTextLibrary> result = new List<AnnotationTextLibrary>();
            try
            {

                string urlParameters = "?organisationId=" + organisationId + "&userId=" + userId + "&pageNumber=" + pageNumber+ "&pageSize="+ pageSize +  "&annotationType=" + annotationType + "&annotationText=" + annotationText + "&structureId=" + structureId + "&userSchema=" + "portal";
                HttpResponseMessage response = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                             $"/Routes/GetAnnotationsFromLibrary" + urlParameters, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<List<AnnotationTextLibrary>>().Result;
                }
                else
                {
                    //do exception handling here
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveMapUsage, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"Routes/SaveMapUsage, Exception: {ex}");
            }
            return result;
        }

        #endregion

        public int ReOrderRoutePart(string routePartIds, string userSchema = UserSchema.Portal)
        {
            int result =0;
            string urlParameters = $"?routePartIds={ routePartIds}&userSchema={userSchema}";

            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/ReOrderRoutePart{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/ReOrderRoutePart, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return result;
        }

        #region GetRoutePartDetails
        public List<RoutePartDetails> GetRoutePartDetails(string notificationidVal, int? isNenViaPdf, int? isHistoric, int orgId, string userSchema)
        {

            List<RoutePartDetails> routePartDetails = new List<RoutePartDetails>();
            string urlParameters = "?notificationidVal=" + notificationidVal + "&isNenViaPdf=" + isNenViaPdf + "&isHistoric=" + isHistoric + "&orgId=" + orgId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetRoutePartDetails{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                routePartDetails = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"Routes/GetRoutePartId, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routePartDetails;
        }

        #endregion

        #region NEN Via API route functions
        public List<NenRouteList> CloneNenRoute(CloneNenRoute cloneNenRoute)
        {
            List<NenRouteList> nenRouteLists = new List<NenRouteList>();
            HttpResponseMessage response = httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/RouteImport/CloneNenRoute",cloneNenRoute).Result;
            if (response.IsSuccessStatusCode)
            {
                nenRouteLists = response.Content.ReadAsAsync<List<NenRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteImport/CloneNenRoute, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return nenRouteLists;
        }

        #endregion

        #region GetPlannedNenAPIRouteList
        public List<AppRouteList> GetPlannedNenAPIRouteList(string contRefNum, int orgId)
        {
            List<AppRouteList> appRouteLists = new List<AppRouteList>();
            string urlParameters = "?contRefNum=" + contRefNum + "&orgId=" + orgId + "&userSchema=" + UserSchema.Portal;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetPlannedNenAPIRouteList{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                appRouteLists = response.Content.ReadAsAsync<List<AppRouteList>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteImport/GetPlannedNenAPIRouteList, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return appRouteLists;
        }

        public List<RoutePartDetails> GetNenApiRoutesForAnalysis(string contRefNum, int orgId, string userSchema)
        {
            List<RoutePartDetails> routePartDetails = new List<RoutePartDetails>();
            string urlParameters = "?contRefNum=" + contRefNum + "&orgId=" + orgId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetNenApiRoutesForAnalysis{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                routePartDetails = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteImport/GetNenApiRoutesForAnalysis, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routePartDetails;
        }
        public List<RoutePartDetails> GetNenPdfRoutesForAnalysis(int inboxItemId, int orgId, string userSchema)
        {
            List<RoutePartDetails> routePartDetails = new List<RoutePartDetails>();
            string urlParameters = "?inboxItemId=" + inboxItemId + "&orgId=" + orgId + "&userSchema=" + userSchema;
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Routes"]}" +
                    $"/Routes/GetNenPdfRoutesForAnalysis{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                routePartDetails = response.Content.ReadAsAsync<List<RoutePartDetails>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, @"RouteImport/GetNenPdfRoutesForAnalysis, Error: " + (int)response.StatusCode + "-" + response.ReasonPhrase);
            }
            return routePartDetails;
        }
        #endregion


    }
}
