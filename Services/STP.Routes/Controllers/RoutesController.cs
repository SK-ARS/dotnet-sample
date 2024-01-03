using NetSdoGeometry;
using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.RouteAssessment;
using STP.Domain.Routes;
using STP.Domain.Structures;
using STP.Routes.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using static STP.Domain.Routes.RouteModel;

namespace STP.Routes.Controllers
{
    public class RoutesController : ApiController
    {
        const string m_RouteName = " - RouteManager";

        #region Library Route Scenarios

        #region List Library Route
        [HttpPost]
        [Route("Routes/LibraryRouteList")]
        public IHttpActionResult LibraryRouteList(LibraryRouteListParams listParams)
        {
            try
            {
                List<RoutePartDetails> GridListObj1 =RouteManagerProvider.Instance.LibraryRouteList(listParams.OrganisationId, listParams.PageNumber, listParams.PageSize, listParams.RouteType, listParams.SerchString, listParams.UserSchema, listParams.FilterFavouritesRoutes,listParams.presetFilter,listParams.sortOrder);
                return Content(HttpStatusCode.OK, GridListObj1);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/LibraryRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Library Route
        [HttpGet]
        [Route("Routes/GetLibraryRoute")]
        public IHttpActionResult GetLibraryRoute(long routeId, string userSchema)
        {
            try
            {
                RoutePart rp = RouteManagerProvider.Instance.GetLibraryRoute(routeId, userSchema);
                return rp.RoutePartDetails.RouteId > 0 ? Content(HttpStatusCode.OK, rp) : (IHttpActionResult)Content(HttpStatusCode.OK, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetLibraryRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Save Library Route
        [HttpPost]
        [Route("Routes/SaveLibraryRoute")]
        public IHttpActionResult SaveLibraryRoute(SaveLibraryRouteParams saveLibRoute)
        {
            long routeId;
            try
            {
                routeId = RouteManagerProvider.Instance.SaveLibraryRoute(saveLibRoute.RoutePart, saveLibRoute.UserSchema);
                return Content(HttpStatusCode.Created, routeId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveLibraryRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Update Library Route
        [HttpPost]
        [Route("Routes/UpdateLibraryRoute")]
        public IHttpActionResult UpdateLibraryRoute(SaveLibraryRouteParams updateLibRoute)
        {
            long routeId;
            try
            {
                routeId = RouteManagerProvider.Instance.UpdateLibraryRoute(updateLibRoute.RoutePart, updateLibRoute.UserSchema);
                return Content(HttpStatusCode.OK, routeId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/UpdateLibraryRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Delete Library Route
        [HttpDelete]
        [Route("Routes/DeleteLibraryRoute")]
        public IHttpActionResult DeleteLibraryRoute(long routeId, string userSchema)
        {
            int result;
            try
            {
                result = RouteManagerProvider.Instance.DeleteLibraryRoute(routeId, userSchema);
                if (result > 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/DeleteLibraryRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Add Route to Library
        [HttpPost]
        [Route("Routes/AddRouteToLibrary")]
        public IHttpActionResult AddRouteToLibrary(AddToLibraryParams routeParams)
        {
            try
            {
                long routeId =
                    RouteManagerProvider.Instance.AddRouteToLibrary(routeParams.RouteId, routeParams.OrganisationId, routeParams.RouteType, routeParams.UserSchema);
                return Content(HttpStatusCode.Created, routeId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/AddRouteToLibrary, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Routes/CheckRouteName")]
        public IHttpActionResult CheckRouteName(CheckRouteName chkRouteName)
        {
            try
            {
                int isExist =
                    RouteManagerProvider.Instance.CheckRouteName(chkRouteName.RouteName, chkRouteName.OrganisationId, chkRouteName.UserSchema);
                return Content(HttpStatusCode.OK, isExist);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/CheckRouteNameExistInLibrary, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #endregion

        #region Application/Notification Route Scenarios

        #region  Get Notif/ApplicationRoute
        [HttpGet]
        [Route("Routes/GetApplicationRoute")]
        public IHttpActionResult GetApplicationRoute(long routeId, string userSchema)
        {
            try
            {
                RoutePart routePart = RouteManagerProvider.Instance.GetApplicationRoute(routeId, userSchema);
                if (routePart != null)
                    return Content(HttpStatusCode.OK, routePart);
                else
                    return Content(HttpStatusCode.OK, StatusMessage.NotFound);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetApplicationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Save Application Route
        [HttpPost]
        [Route("Routes/SaveApplicationRoute")]
        public IHttpActionResult SaveApplicationRoute(SaveAppRouteParams saveAppRouteParams)
        {
            long routeId;
            try
            {
                routeId = RouteManagerProvider.Instance.SaveApplicationRoute(saveAppRouteParams);
                return Content(HttpStatusCode.Created, routeId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveApplicationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Update Application Route
        [HttpPost]
        [Route("Routes/UpdateApplicationRoute")]
        public IHttpActionResult UpdateApplicationRoute(SaveAppRouteParams updateAppRouteParams)
        {
            long routeId;
            try
            {
                routeId = RouteManagerProvider.Instance.UpdateApplicationRoute(updateAppRouteParams);
                return Content(HttpStatusCode.OK, routeId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/UpdateApplicationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Delete Application Route
        [HttpDelete]
        [Route("Routes/DeleteApplicationRoute")]
        public IHttpActionResult DeleteApplicationRoute(long routeId, string routeType, string userSchema)
        {
            int result;
            try
            {
                result = RouteManagerProvider.Instance.DeleteApplicationRoute(routeId, routeType, userSchema);
                if (result > 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/DeleteApplicationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #endregion

        #region Save Route Annotation
        [HttpPost]
        [Route("Routes/SaveRouteAnnotation")]
        public IHttpActionResult SaveRouteAnnotation(SaveRouteAnnotationParams objSaveRouteAnnotation)
        {
            bool saveFlag;
            try
            {
                saveFlag = RouteManagerProvider.Instance.SaveRouteAnnotation(objSaveRouteAnnotation.routePart, objSaveRouteAnnotation.type, objSaveRouteAnnotation.userSchema);
                return Content(HttpStatusCode.OK, saveFlag);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveRouteAnnotation, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  List Route Import Details
        [HttpGet]
        [Route("Routes/ListRouteImportDetails")]
        public IHttpActionResult ListRouteImportDetails(string contentReferenceNo)
        {
            try
            {
                List<NotifRouteImport> result = RouteManagerProvider.Instance.ListRouteImportDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/ListRouteImportDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRoutePathId
        [HttpGet]
        [Route("Routes/GetRoutePathId")]
        public IHttpActionResult GetRoutePathId(long routeId, int isLib, string userSchema=UserSchema.Portal)
        {
            try
            {
                long iRouteId= RouteManagerProvider.Instance.GetRoutePathId(routeId, isLib,userSchema);
                return Content(HttpStatusCode.OK, iRouteId);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePathId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRoutePoints
        [HttpGet]
        [Route("Routes/GetRoutePoints")]
        public IHttpActionResult GetRoutePoints(long routePathId, string userSchema)
        {
            try
            {
                List<RoutePoint> rp = RouteManagerProvider.Instance.GetRoutePoints(routePathId, userSchema);
                return Content(HttpStatusCode.OK, rp);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePoints, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region UpdateNotifPlanRoute
        [HttpPost]
        [Route("Routes/UpdateNotifPlanRoute")]
        public IHttpActionResult UpdateNotifPlanRoute(UpdateNotifPlanRouteParam updateNotifPlanRouteParam)
        {
            try
            {
                bool rp = RouteManagerProvider.Instance.UpdateNotifPlanRoute(updateNotifPlanRouteParam.RoutePartId, updateNotifPlanRouteParam.ContentReferenceNo, updateNotifPlanRouteParam.RoutePartNo, updateNotifPlanRouteParam.ImportVehicle, updateNotifPlanRouteParam.Flag);
                return Content(HttpStatusCode.OK, rp);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/UpdateNotifPlanRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  SaveSNotificationRoute
        [HttpPost]
        [Route("Routes/SaveSNotificationRoute")]
        public IHttpActionResult SaveSNotificationRoute(SaveSNotificationRouteParam saveSNotificationRouteParam)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.SaveSNotificationRoute(saveSNotificationRouteParam.routepartId, saveSNotificationRouteParam.ContentRefNo);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveSNotificationRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Verify Application RouteName
        [HttpPost]
        [Route("Routes/verifyApplicationRouteName")]
        public IHttpActionResult VerifyApplicationRouteName(ApplicationRouteNameParams objApplicationRouteNameParams)
        {
            int count = 1;
            try
            {
                count = RouteManagerProvider.Instance.VerifyApplicationRouteName(objApplicationRouteNameParams);
                return Content(HttpStatusCode.OK, count); 
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/VerifyApplicationRouteName, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  GetRouteDetails
        [HttpGet]
        [Route("Routes/GetRouteDetails")]
        public IHttpActionResult GetRouteDetails(string contentReferenceNo)
        {
            try
            {
                List<RoutePoint> objDetails = RouteManagerProvider.Instance.GetRouteDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, objDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRouteDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

		#region  GetRoutePartsCount
        [HttpGet]
        [Route("Routes/GetRoutePartsCount")]
        public IHttpActionResult GetRoutePartsCount(string contentReferenceNo)
        {
            int count;
            try
            {
                count = RouteManagerProvider.Instance.GetRoutePartsCount(contentReferenceNo);
                return Content(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePartsCount, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  SaveNotificationRoute
        [HttpPost]
        [Route("Routes/SaveNotificationRoute")]
        public IHttpActionResult SaveNotificationRoute(SaveNotificationRouteParam saveNotificationRouteParam)
        {
            long routePartID;
            try
            {
                routePartID = RouteManagerProvider.Instance.SaveNotificationRoute(saveNotificationRouteParam.routePartId, saveNotificationRouteParam.versionId, saveNotificationRouteParam.contentRefNo, saveNotificationRouteParam.routeType);
                return Content(HttpStatusCode.OK, routePartID);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveNotificationRoute, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion        

        #region  DeleteOldRouteDetails
        [HttpDelete]
        [Route("Routes/DeleteOldRouteDetails")]
        public IHttpActionResult DeleteOldRouteDetails(long newRoutePartId, string contentRefNo, int oldRoutePartId)
        {
            int result;
            try
            {
                result = RouteManagerProvider.Instance.DeleteOldRouteDetails(newRoutePartId, contentRefNo, oldRoutePartId);
                if(result > 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/DeleteOldRouteDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  UpdateRoutePartId
        [HttpGet]
        [Route("Routes/UpdateRoutePartId")]
        public IHttpActionResult UpdateRoutePartId(int newRoutePartId, int oldRoutePartId, string contentRefNo = "")
        {
            int result;
            try
            {
                result = RouteManagerProvider.Instance.UpdateRoutePartId(newRoutePartId, oldRoutePartId, contentRefNo);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/UpdateRoutePartId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  GetNotifRouteDetails
        [HttpGet]
        [Route("Routes/GetNotifRouteDetails")]
        public IHttpActionResult GetNotifRouteDetails(string contentReferenceNo)
        {
            try
            {
                List<ListRouteVehicleId> routeVehicleIds = RouteManagerProvider.Instance.GetNotifRouteDetails(contentReferenceNo);
                return Content(HttpStatusCode.OK, routeVehicleIds);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetNotifRouteDetails, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  DeleteOldReturnLeg
        [HttpDelete]
        [Route("Routes/DeleteOldReturnLeg")]
        public IHttpActionResult DeleteOldReturnLeg(string contentReferenceNo)
        {
            int result;
            try
            {
                result = RouteManagerProvider.Instance.DeleteOldReturnLeg(contentReferenceNo);
                if (result >= 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/DeleteOldReturnLeg, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  GetRoutePointsForReturnLeg
        [HttpGet]
        [Route("Routes/GetRoutePointsForReturnLeg")]
        public IHttpActionResult GetRoutePointsForReturnLeg(int libraryRouteId, long planRouteId)
        {
            try
            {
                List<RoutePoint> routepoint = RouteManagerProvider.Instance.GetRoutePointsForReturnLeg(libraryRouteId, planRouteId);
                return Content(HttpStatusCode.OK, routepoint);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePointsForReturnLeg, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  DeleteOldRouteDetailsForImport
        [HttpDelete]
        [Route("Routes/DeleteOldRouteDetailsForImport")]
        public IHttpActionResult DeleteOldRouteDetailsForImport(long newRoutePartId, string contentReferenceNo, int routePartNo)
        {
            int result;
            try
            {
                result = RouteManagerProvider.Instance.DeleteOldRouteDetailsForImport(newRoutePartId, contentReferenceNo, routePartNo);
                if (result >= 0)
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.InternalServerError, StatusMessage.DeletionFailed);
            }
            catch(Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/DeleteOldRouteDetailsForImport, Exception:" + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  ImportRouteFromLibrary
        [HttpPost]
        [Route("Routes/ImportRouteFromLibrary")]
        public IHttpActionResult ImportRouteFromLibrary(SaveApplicationRouteParams saveApplicationRouteParams)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.ImportRouteFromLibrary(saveApplicationRouteParams.routePartId, saveApplicationRouteParams.versionId, saveApplicationRouteParams.appRevId, saveApplicationRouteParams.routeType, saveApplicationRouteParams.contentRef, saveApplicationRouteParams.userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/ImportRouteFromLibrary, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  SaveRouteInRouteParts
        [HttpPost]
        [Route("Routes/SaveRouteInRouteParts")]
        public IHttpActionResult SaveRouteInRouteParts(SaveApplicationRouteParams saveApplicationRouteParams)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.SaveRouteInRouteParts(saveApplicationRouteParams.routePartId, saveApplicationRouteParams.appRevId, saveApplicationRouteParams.versionId, saveApplicationRouteParams.contentRef, saveApplicationRouteParams.userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveRouteInRouteParts, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  SaveRouteInAppParts
        [HttpPost]
        [Route("Routes/SaveRouteInAppParts")]
        public IHttpActionResult SaveRouteInAppParts(SaveApplicationRouteParams saveApplicationRouteParams)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.SaveRouteInAppParts(saveApplicationRouteParams.routePartId, saveApplicationRouteParams.appRevId,saveApplicationRouteParams.userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveRouteInAppParts, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  SaveSOAppImportRoute
        [HttpPost]
        [Route("Routes/SaveSOAppImportRoute")]
        public IHttpActionResult SaveSOAppImportRoute(SaveApplicationRouteParams saveApplicationRouteParams)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.SaveSOAppImportRoute(saveApplicationRouteParams.routePartId, saveApplicationRouteParams.appRevId, saveApplicationRouteParams.routeType, saveApplicationRouteParams.userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveSOAppImportRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRoutePartId
        [HttpGet]
        [Route("Routes/GetRoutePartId")]
        public IHttpActionResult GetRoutePartId(string conRefNumber, string userSchema)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.GetRoutePartId(conRefNumber, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePartId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRoutePointsDetails
        [HttpGet]
        [Route("Routes/GetRoutePointsDetails")]
        public IHttpActionResult GetRoutePointsDetails(int PlanRouteID)
        {
            try
            {
                List<RoutePoint> RLObj = RouteManagerProvider.Instance.GetRoutePointsDetails(PlanRouteID);
                return Content(HttpStatusCode.OK, RLObj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePointsDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Authorized Route Part List
        [HttpGet]
        [Route("Routes/GetAuthorizedRoutePartList")]
        public IHttpActionResult GetAuthorizedRoutePartList(long versionId, string userSchema)
        {
            try
            {
                List<AppRouteList> soAppRouteList = RouteManagerProvider.Instance.GetAuthorizedRoutePartList(versionId, userSchema);
                return Content(HttpStatusCode.OK, soAppRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetAuthorizedRoutePartList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Planned NEN Route List for SOA/Police
        [HttpGet]
        [Route("Routes/GetPlannedNenRouteList")]
        public IHttpActionResult GetPlannedNenRouteList(long nenId, int userId, long inboxItemId, int orgId)
        {
            try
            {
                List<AppRouteList> nenRouteList = RouteManagerProvider.Instance.GetPlannedNenRouteList(nenId, userId, inboxItemId, orgId);
                return Content(HttpStatusCode.OK, nenRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetPlannedNenRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get SO App Route Part List
        [HttpGet]
        [Route("Routes/GetSoAppRouteList")]
        public IHttpActionResult GetSoAppRouteList(long revisionId, string userSchema)
        {
            try
            {
                List<AppRouteList> soRouteList = RouteManagerProvider.Instance.GetSoAppRouteList(revisionId, userSchema);
                return Content(HttpStatusCode.OK, soRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetSoAppRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region NotifVR1RouteList
        [HttpGet]
        [Route("Routes/NotifVR1RouteList")]
        public IHttpActionResult NotifVR1RouteList(long revisionId, string contRefNum, long versionId, string userSchema)
        {
            try
            {
                List<AppRouteList> routeList = RouteManagerProvider.Instance.NotifVR1RouteList(revisionId, contRefNum, versionId, userSchema);
                return Content(HttpStatusCode.OK, routeList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Routes/NotifVR1RouteList, Exception: ", ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Outline Candidate Route
        [HttpGet]
        [Route("Routes/GetCandidateOutlineRoute")]
        public IHttpActionResult GetCandidateOutlineRoute(long routePartId, string userSchema)
        {
            try
            {
                RoutePart outlineCandRoute = RouteManagerProvider.Instance.GetCandidateOutlineRoute(routePartId, userSchema);
                return Content(HttpStatusCode.OK, outlineCandRoute);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetCandidateOutlineRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Get Outline Notif/App Route
        [HttpGet]
        [Route("Routes/GetOutlinRoute")]
        public IHttpActionResult GetApplicationRoutePartGeometry(long partId, string userSchema)
        {
            try
            {
                RoutePart outlineAppRoute = RouteManagerProvider.Instance.GetApplicationRoutePartGeometry(partId, userSchema);
                return Content(HttpStatusCode.OK, outlineAppRoute);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetOutlinRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Broken Route Scenarios

        #region  List Broken Route Details            
        [HttpGet]
        [Route("Routes/ListBrokenRouteDetails")]
        public IHttpActionResult ListBrokenRouteDetails(string userSchema = UserSchema.Portal, long appRevisionID = 0, long revisionID = 0, long movementVersionID = 0, string contentReferenceNumber = null)
        {
            try
            {
                List<NotifRouteImport> listNotifRouteImport = RouteManagerProvider.Instance.ListBrokenRouteDetails(contentReferenceNumber, userSchema, appRevisionID, revisionID, movementVersionID);
                return Content(HttpStatusCode.Created, listNotifRouteImport);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $" {m_RouteName}/ListBrokenRouteDetails, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  GetBrokenRouteIds
        [HttpPost]
        [Route("Routes/GetBrokenRouteId")]
        public IHttpActionResult GetBrokenRouteId(GetBrokenRouteList getBrokenRouteList)
        {
            try
            {
                List<BrokenRouteList> brokenRouteList = RouteManagerProvider.Instance.GetBrokenRouteIds(getBrokenRouteList);
                return Content(HttpStatusCode.OK, brokenRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetBrokenRouteId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  CheckIsBroken
        [HttpPost]
        [Route("Routes/CheckIsBroken")]
        public IHttpActionResult CheckIsBroken(GetBrokenRouteList getBrokenRouteList)
        {
            try
            {
                int IsBroken = RouteManagerProvider.Instance.CheckIsBroken(getBrokenRouteList);
                return Content(HttpStatusCode.OK, IsBroken);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/CheckIsBroken, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion


        #region  GetBrokenRoutePoints
        [HttpGet]
        [Route("Routes/GetBrokenRoutePoints")]
        public IHttpActionResult GetBrokenRoutePoints(long routePathId = 0, int isLib = 0, string userSchema = "")
        {
            try
            {
                List<RoutePoint> routePoint = RouteManagerProvider.Instance.GetBrokenRoutePoints(routePathId, isLib, userSchema);
                return Content(HttpStatusCode.OK, routePoint);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetBrokenRoutePoints, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  GetBrokenRouteAnnotations
        [HttpGet]
        [Route("Routes/GetBrokenRouteAnnotations")]
        public IHttpActionResult GetBrokenRouteAnnotations(long segmentId, int is_lib, string userSchema)
        {
            try
            {
                List<RouteAnnotation> routeAnnotation = RouteManagerProvider.Instance.GetBrokenRouteAnnotations(segmentId, is_lib, userSchema);
                return Content(HttpStatusCode.OK, routeAnnotation);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetBrokenRouteAnnotations, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region So Update BrokenRoute Path
        [HttpPost]
        [Route("Routes/UpdateBrokenRoutePath")]
        public IHttpActionResult UpdateBrokenRoutePath(UpdateBrokenRoutePathParam objUpdateBrokenRoutePathParam)
        {
            bool result;
            try
            {
                result = RouteManagerProvider.Instance.UpdateBrokenRoutePath(objUpdateBrokenRoutePathParam);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/UpdateBrokenRoutePath, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region Set Verification Status
        [HttpPost]
        [Route("Routes/SetVerificationStatus")]
        public IHttpActionResult SetVerificationStatus(VerificationStatusParams objVerificationStatusParams)
        {
            int val;
            try
            {
                val= RouteManagerProvider.Instance.SetVerificationStatus(objVerificationStatusParams);
                return Content(HttpStatusCode.OK, val);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SetVerificationStatus, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #endregion

        #region Affected structures and constraints on route
        [HttpGet]
        [Route("Routes/AffectedStructuresOnRoute")]
        public List<StructureInfo> GetAffectedStructures(int routePartId, RoutePart routePart, string userSchema)
        {
            List<StructureInfo> Affectedstructures = new List<StructureInfo>();
            try
            {
                Affectedstructures = RouteManagerProvider.Instance.GetAffectedStructures(routePartId, routePart, userSchema);
                return Affectedstructures;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SetVerificationStatus, Exception: " + ex​​​​);
                return Affectedstructures;
            }

        }

        [HttpGet]
        [Route("Routes/AffectedConstraintsOnRoute")]
        public List<RouteConstraints> GetAffectedConstraints(int routePartId, RoutePart routePart, string userSchema)
        {
            List<RouteConstraints> AffectedConstraints = new List<RouteConstraints>();
            try
            {
                AffectedConstraints = RouteManagerProvider.Instance.GetAffectedConstraints(routePartId, routePart, userSchema);
                return AffectedConstraints;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SetVerificationStatus, Exception: " + ex​​​​);
                return AffectedConstraints;
            }

        }

        #endregion

        #region  SaveMapUsage
        [HttpPost]
        [Route("Routes/SaveMapUsage")]
        public IHttpActionResult SaveMapUsage(int userId, int organisationId, int type)
        {
            try
            {
                int result = RouteManagerProvider.Instance.SaveMapUsage(userId, organisationId,  type);
               
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveMapUsage, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region List Library Route
        [HttpGet]
        [Route("Routes/GetFavouriteRoutes")]
        public IHttpActionResult GetFavouriteRoutes(int organisationId, string userSchema)
        {
            try
            {
                List<RoutePartDetails> favRoutesLst = RouteManagerProvider.Instance.GetFavouriteRoutes(organisationId, userSchema);
                return Content(HttpStatusCode.OK, favRoutesLst);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetFavouriteRoutes, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region SORT Previous/Current Movement RouteList
        [HttpGet]
        [Route("Routes/GetSortMovementRoute")]
        public IHttpActionResult GetSortMovementRoute(long revisionId, int rListType)
        {
            try
            {
                List<AppRouteList> appRouteList = RouteManagerProvider.Instance.GetSortMovementRoute(revisionId, rListType);
                return Content(HttpStatusCode.OK, appRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetSortMovementRoute, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRouteDetailForAnalysis
        [HttpGet]
        [Route("Routes/GetRouteDetailForAnalysis")]
        public IHttpActionResult GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema)
        {
            try
            {
                List<RoutePartDetails> routeDetails = RouteManagerProvider.Instance.GetRouteDetailForAnalysis(versionId, contentRefNo, revisionId, isCandidate, userSchema);
                return Content(HttpStatusCode.OK, routeDetails);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRouteDetailForAnalysis, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region CheckRouteName
        [HttpGet]
        [Route("Routes/CheckRouteVehicleAttach")]
        public IHttpActionResult CheckRouteVehicleAttach(long routePartId)
        {
            try
            {
                int isAttach =RouteManagerProvider.Instance.CheckRouteVehicleAttach(routePartId);
                return Content(HttpStatusCode.OK, isAttach);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"Routes/CheckRouteVehicleAttach, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  SaveAnnotationToLibrary
        [HttpPost]
        [Route("Routes/SaveAnnotationToLibrary")]
        public IHttpActionResult SaveAnnotationToLibrary(int organisationId, int userId, long annotationType, string annotationText, int structureId = 0, string userSchema = UserSchema.Portal)
        {
            long result;
            try
            {
                result = RouteManagerProvider.Instance.SaveAnnotationInLibrary(organisationId,  userId, annotationType,  annotationText,  structureId,  userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveRouteInAppParts, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  GetAnnotationFromLibrary
        [HttpPost]
        [Route("Routes/GetAnnotationsFromLibrary")]
        public IHttpActionResult GetAnnotationsFromLibrary(int organisationId,int userId,int pageNumber,int pageSize,  long annotationType, string annotationText, int structureId = 0, string userSchema = UserSchema.Portal)
        {
            List<AnnotationTextLibrary> result = new List<AnnotationTextLibrary>();
            try
            {
                result = RouteManagerProvider.Instance.GetAnnotationsFromLibrary(organisationId, userId, pageNumber, pageSize, annotationType, annotationText, structureId, userSchema);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/SaveRouteInAppParts, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region  ReOrderRoutePart
        [HttpGet]
        [Route("Routes/ReOrderRoutePart")]
        public IHttpActionResult ReOrderRoutePart(string routePartIds, string userSchema = UserSchema.Portal)
        {
            try
            {
                int routepoint = RouteManagerProvider.Instance.ReOrderRoutePart(routePartIds, userSchema);
                return Content(HttpStatusCode.OK, routepoint);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/ReOrderRoutePart, Exception: " + ex);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region GetRoutePartId
        [HttpGet]
        [Route("Routes/GetRoutePartDetails")]
        public IHttpActionResult GetRoutePartDetails(string notificationidVal, int? isNenViaPdf, int? isHistoric, int orgId, string userSchema)
        {
            List<RoutePartDetails> result = new List<RoutePartDetails>();
            try
            {
                result = RouteManagerProvider.Instance.GetRoutePartDetails(notificationidVal, isNenViaPdf, isHistoric,orgId, userSchema);
                
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetRoutePartId, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region NEN Via API route functions
        [HttpGet]
        [Route("Routes/GetPlannedNenAPIRouteList")]
        public IHttpActionResult GetPlannedNenAPIRouteList(string contRefNum, int orgId, string userSchema)
        {
            try
            {
                List<AppRouteList> nenRouteList = RouteManagerProvider.Instance.GetPlannedNenAPIRouteList(contRefNum, orgId, userSchema);
                return Content(HttpStatusCode.OK, nenRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetPlannedNenRouteList, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        [HttpGet]
        [Route("Routes/GetNenApiRoutesForAnalysis")]
        public IHttpActionResult GetNenApiRoutesForAnalysis(string contRefNum, int orgId, string userSchema)
        {
            try
            {
                List<RoutePartDetails> nenRouteList = RouteManagerProvider.Instance.GetNenApiRoutesForAnalysis(contRefNum, orgId, userSchema);
                return Content(HttpStatusCode.OK, nenRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetNenApiRoutesForAnalysis, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion

        #region NEN PDF Route details
        [HttpGet]
        [Route("Routes/GetNenPdfRoutesForAnalysis")]
        public IHttpActionResult GetNenPdfRoutesForAnalysis(int inboxItemId, int orgId, string userSchema)
        {
            try
            {
                List<RoutePartDetails> nenRouteList = RouteManagerProvider.Instance.GetNenPdfRoutesForAnalysis(inboxItemId, orgId, userSchema);
                return Content(HttpStatusCode.OK, nenRouteList);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + m_RouteName + @"/GetNenPdfRoutesForAnalysis, Exception: " + ex​​​​);
                return Content(HttpStatusCode.InternalServerError, StatusMessage.InternalServerError);
            }
        }
        #endregion
    }
}
