using STP.Common.Constants;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.RouteAssessment;
using STP.Domain.Routes;
using STP.Domain.Structures;
using STP.Routes.Interface;
using STP.Routes.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static STP.Domain.Routes.RouteModel;

namespace STP.Routes.Providers
{
    public class RouteManagerProvider : IRouteManager
    {
        #region RouteManagerProvider Singleton
        private RouteManagerProvider()
        {
        }
        public static RouteManagerProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        internal static class Nested
        {
            static Nested()
            {
            }
            internal static readonly RouteManagerProvider instance = new RouteManagerProvider();
        }
        #endregion

        public int SaveMapUsage(int userId, int organisationId, int type)
        {
            return RouteManagerDao.SaveMapUsageInfo(userId, organisationId, type);
        }

        #region Library Route Functions
        public RoutePart GetLibraryRoute(long plannedRouteID, string userSchema)
        {
            return RouteManagerDao.GetLibraryRoute(plannedRouteID, userSchema);
        }
        public List<RoutePartDetails> LibraryRouteList(int organisationID, int pageNumber, int pageSize, int routeType, string serchString, string userSchema, int filterFavouritesRoutes,int presetFilter,int? sortOrder=null)
        {
            return RouteManagerDao.GetLibraryRouteList(organisationID, pageNumber, pageSize, routeType, serchString, userSchema, filterFavouritesRoutes,presetFilter,sortOrder);
        }
        public int DeleteLibraryRoute(long plannedRouteID, string userSchema)
        {
            return RouteManagerDao.DeleteLibraryRoute(plannedRouteID, userSchema);
        }
        public long SaveLibraryRoute(RoutePart routePart, string userSchema)
        {
            return RouteManagerDao.SaveLibraryRoute(routePart, userSchema);
        }
        public long UpdateLibraryRoute(RoutePart routePart, string userSchema)
        {
            return RouteManagerDao.UpdateLibraryRoute(routePart, userSchema);
        }
        public long AddRouteToLibrary(long routePartId, int orgId, string rtType, string userSchema)
        {
            return RouteManagerDao.AddRouteToLibrary(routePartId, orgId, rtType, userSchema);
        }
        public int CheckRouteName(string RouteName, int organisationId, string userSchema)
        {
            return RouteManagerDao.CheckRouteName(RouteName, organisationId, userSchema);
        }
        #endregion

        #region Application/Notification Route Function
        public RoutePart GetApplicationRoute(long routePartId, string userSchema = UserSchema.Portal)
        {
            return RouteManagerDao.GetApplicationRoute(routePartId, userSchema);
        }
        public RoutePart GetHistoricAppRoute(long routeId, string userSchema)
        {
            return RouteManagerDao.GetHistoricAppRoute(routeId, userSchema);
        }
        public List<RoutePart> UpdateCloneHistoricAppRoute(UpdateHistoricCloneRoute updateHistoricClone)
        {
            return RouteManagerDao.UpdateCloneHistoricAppRoute(updateHistoricClone);
        }
        public List<RouteLinkModel> GetHistoricRouteLinkModel(long segmentId, int segmentNo, int tolerance, string userSchema)
        {
            return RouteManagerDao.GetHistoricRouteLinkModel(segmentId, segmentNo, tolerance, userSchema);
        }
        public long SaveApplicationRoute(SaveAppRouteParams saveAppRouteParams)
        {
            return RouteManagerDao.SaveApplicationRoute(saveAppRouteParams);
        }
        public long UpdateApplicationRoute(SaveAppRouteParams updateAppRouteParams)
        {
            return RouteManagerDao.UpdateApplicationRoute(updateAppRouteParams);
        }
        public int DeleteApplicationRoute(long routeId, string routeType, string userSchema)
        {
            return RouteManagerDao.DeleteApplicationRoute(routeId, routeType, userSchema);
        }
        #endregion

        public bool SaveRouteAnnotation(RoutePart routePart, int type, string userSchema)
        {
            return RouteManagerDao.SaveRouteAnnotation(routePart, type, userSchema);
        }

        #region ListRouteImportDetails
        public List<NotifRouteImport> ListRouteImportDetails(string contentReferenceNo)
        {
            return RouteManagerDao.ListRouteImportDetails(contentReferenceNo);
        }
        #endregion

        #region GetRoutePathId
        public long GetRoutePathId(long routeId, int isLib, string userSchema)
        {
            return RouteManagerDao.GetRoutePathId(routeId, isLib, userSchema);
        }
        #endregion

        #region GetRoutePoints
        public List<RoutePoint> GetRoutePoints(long routePathId, string userSchema)
        {
            return RouteManagerDao.GetLibraryRoutePoints(routePathId, userSchema);
        }
        #endregion

        #region UpdateNotifPlanRoute
        public bool UpdateNotifPlanRoute(int RoutePartId, string contentrefno, int RoutePartNo, int ImportVeh = 0, int Flag = 0)
        {
            return RouteManagerDao.UpdateNotifPlanRoute(RoutePartId, contentrefno, RoutePartNo, ImportVeh, Flag);
        }
        #endregion

        #region SaveSNotificationRoute
        public long SaveSNotificationRoute(int routepartId, string ContentRefNo)
        {
            return RouteManagerDao.SaveSNotifRoute(routepartId, ContentRefNo);
        }
        #endregion

        public int VerifyApplicationRouteName(ApplicationRouteNameParams objApplicationRouteNameParams)
        {
            return RouteManagerDao.VerifyApplicationRouteName(objApplicationRouteNameParams);
        }

        #region  public List<RoutePoint> GetRouteDetails(string ContentRefNo)
        public List<RoutePoint> GetRouteDetails(string contentReferenceNo)
        {
            return RouteManagerDao.GetRouteDetails(contentReferenceNo);
        }
        #endregion

        #region GetRoutePartsCount
        public int GetRoutePartsCount(string contentReferenceNo)
        {
            return RouteManagerDao.GetRoutePartsCount(contentReferenceNo);
        }
        #endregion

        #region SaveNotificationRoute
        public long SaveNotificationRoute(int routePartId, int versionId, string contentRefNo, int routeType)
        {
            return RouteManagerDao.SaveNotifRoute(routePartId, versionId, contentRefNo, routeType);
        }
        #endregion

        #region DeleteOldRouteDetails
        public int DeleteOldRouteDetails(long newRoutePartId, string contentRefNo, int oldRoutePartId)
        {
            return RouteManagerDao.DeleteOldRoute(newRoutePartId, contentRefNo, oldRoutePartId);
        }
        #endregion

        #region UpdateRoutePartId
        public int UpdateRoutePartId(int newRoutePartId, int oldRoutePartId, string contentRefNo = "")
        {
            return RouteManagerDao.UpdateRoutePartId(newRoutePartId, oldRoutePartId, contentRefNo);
        }
        #endregion

        #region GetNotifRouteDetails
        public List<ListRouteVehicleId> GetNotifRouteDetails(string contentReferenceNo)
        {
            return RouteManagerDao.GetNotifRouteDetails(contentReferenceNo);
        }
        #endregion

        #region DeleteOldReturnLeg
        public int DeleteOldReturnLeg(string contentReferenceNo)
        {
            return RouteManagerDao.DeleteOldReturnLeg(contentReferenceNo);
        }
        #endregion

        #region GetRoutePointsForReturnLeg
        public List<RoutePoint> GetRoutePointsForReturnLeg(int libraryRouteId, long planRouteId)
        {
            return RouteManagerDao.GetRoutePointsForReturnLeg(libraryRouteId, planRouteId);
        }
        #endregion

        public int DeleteOldRouteDetailsForImport(long newRoutePartId, string contentReferenceNo, int routePartNo)
        {
            return RouteManagerDao.DeleteOldRouteForImport(newRoutePartId, contentReferenceNo, routePartNo);
        }

        #region SaveSOApplicationRoute
        public long SaveSOAppImportRoute(int routePartId, int appRevId, int routeType, string userSchema)
        {
            return RouteManagerDao.SaveSOAppImportRoute(routePartId, appRevId, routeType, userSchema);
        }
        #endregion

        #region SaveVR1ApplicationRoute
        public long ImportRouteFromLibrary(int routePartId, int versionId, int appRevId, int routeType, string contentRef, string userSchema)
        {
            return RouteManagerDao.ImportRouteFromLibrary(routePartId, versionId, appRevId, routeType, contentRef, userSchema);
        }
        #endregion

        #region SaveRouteInRouteParts
        public long SaveRouteInRouteParts(int routePartId, int appRevId, int versionId, string contentRef, string userSchema)
        {
            return RouteManagerDao.SaveRouteInRouteParts(routePartId, appRevId, versionId, contentRef, userSchema);
        }
        #endregion

        #region SaveRouteInAppParts
        public long SaveRouteInAppParts(int routePartId, int appRevId, string userSchema)
        {
            return RouteManagerDao.SaveRouteInAppParts(routePartId, appRevId, userSchema);
        }
        #endregion

        #region GetRoutePartId
        public long GetRoutePartId(string conRefNumber, string userSchema)
        {
            return RouteManagerDao.GetRoutePartId(conRefNumber, userSchema);
        }
        #endregion

        #region GetRoutePointsDetails
        public List<RoutePoint> GetRoutePointsDetails(int PlanRouteID)
        {
            return RouteManagerDao.GetRoutePointsDetails(PlanRouteID);
        }
        #endregion

        #region Authorized Route Part List
        public List<AppRouteList> GetAuthorizedRoutePartList(long versionId, string userSchema)
        {
            return RouteManagerDao.GetAuthorizedRoutePartList(versionId, userSchema);
        }
        #endregion

        #region Get Planned NEN Route List for SOA/Police
        public List<AppRouteList> GetPlannedNenRouteList(long nenId, int userId, long inboxItemId, int orgId)
        {
            return RouteManagerDao.GetPlannedNenRouteList(nenId, userId, inboxItemId, orgId);
        }
        #endregion

        #region SO App Route List
        public List<AppRouteList> GetSoAppRouteList(long revisionId, string userSchema)
        {
            return RouteManagerDao.GetSoAppRouteList(revisionId, userSchema);
        }
        #endregion

        #region Notif VR1 RouteList
        public List<AppRouteList> NotifVR1RouteList(long revisionId, string contRefNum, long versionId, string userSchema)
        {
            return RouteManagerDao.NotifVR1RouteList(revisionId, contRefNum, versionId, userSchema);
        }
        #endregion AppVR1RouteList

        #region Get Outline Candidate Route
        public RoutePart GetCandidateOutlineRoute(long routePartId, string userSchema)
        {
            return RouteManagerDao.GetCandidateOutlineRoute(routePartId, userSchema);
        }
        #endregion

        #region Outline Notif/App Route
        public RoutePart GetApplicationRoutePartGeometry(long partId, string userSchema)
        {
            return RouteManagerDao.GetApplicationRoutePartGeometry(partId, userSchema);
        }
        #endregion

        #region  Broken Route Scenarios
        public List<RoutePoint> GetBrokenRoutePoints(long routePathId = 0, int isLib = 0, string userSchema = "")
        {
            return RouteManagerDao.GetBrokenRoutePoints(routePathId, isLib, userSchema);
        }
        public List<RouteAnnotation> GetBrokenRouteAnnotations(long segmentId, int is_lib, string userSchema)
        {
            return RouteManagerDao.GetBrokenRouteAnnotations(segmentId, is_lib, userSchema);
        }
        public List<BrokenRouteList> GetBrokenRouteIds(GetBrokenRouteList getBrokenRouteList)
        {
            return RouteManagerDao.GetBrokenRouteIds(getBrokenRouteList);
        }
        public int CheckIsBroken(GetBrokenRouteList getBrokenRouteList)
        {
            return RouteManagerDao.CheckIsBroken(getBrokenRouteList);
        }
        public bool UpdateBrokenRoutePath(UpdateBrokenRoutePathParam objUpdateBrokenRoutePathParam)
        {
            return RouteManagerDao.UpdateBrokenRoutePath(objUpdateBrokenRoutePathParam);
        }
        public List<NotifRouteImport> ListBrokenRouteDetails(string contentReferenceNumber, string userSchema, long appRevisionID, long revisionID, long movementVersionID)
        {
            return RouteManagerDao.ListBrokenRouteDetails(contentReferenceNumber, userSchema, appRevisionID, revisionID, movementVersionID);
        }
        public int SetVerificationStatus(VerificationStatusParams objVerificationStatusParams)
        {
            return RouteManagerDao.SetVerificationStatusBrknRts(objVerificationStatusParams);
        }

        public List<StructureInfo> GetAffectedStructures(int routePartId, RoutePart routePart, string userSchema)
        {
            return RouteManagerDao.GetAffectedStructureList(routePartId, routePart, userSchema);
        }

        public List<RouteConstraints> GetAffectedConstraints(int routePartId, RoutePart routePart, string userSchema)
        {
            return RouteManagerDao.GetAffectedConstraintList(routePartId, routePart, userSchema);
        }
        #endregion
        public List<RoutePartDetails> GetFavouriteRoutes(int organisationId, string userSchema)
        {
            return RouteManagerDao.GetFavouriteRoutes(organisationId, userSchema);
        }
        public long SaveNERoute(SaveNERouteParams saveNERoute, bool isAutoPlan)
        {
            return RouteManagerDao.SaveNERoute(saveNERoute, isAutoPlan);
        }
        public List<AppRouteList> GetSortMovementRoute(long revisionId, int rListType)
        {
            return RouteManagerDao.GetSortMovementRoute(revisionId, rListType);
        }
        public List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema)
        {
            return RouteManagerDao.GetRouteDetailForAnalysis(versionId, contentRefNo, revisionId, isCandidate, userSchema);
        }
        public int CheckRouteVehicleAttach(long routePartId)
        {
            return RouteManagerDao.CheckRouteVehicleAttach(routePartId);
        }

        #region SaveAnnotation
        public long SaveAnnotationInLibrary(int organisationId, int userId, long annotationType, string annotationText, int structureId = 0,  string userSchema = UserSchema.Portal)
        {
            return RouteManagerDao.SaveAnnotationInLibrary(organisationId, userId,annotationType, annotationText, structureId, userSchema = UserSchema.Portal);
        }
        #endregion

        #region GetAnnotationFromLibrary
        public List<AnnotationTextLibrary> GetAnnotationsFromLibrary(int organisationId, int userId, int pageNumber,int pageSize, long annotationType, string annotationText, int structureId = 0, string userSchema = UserSchema.Portal)
        {
            return RouteManagerDao.GetAnnotationsFromLibrary(organisationId, userId, pageNumber, pageSize , annotationType, annotationText, structureId, userSchema = UserSchema.Portal);
        }


        #endregion
        #region ReOrderRoutePart
        public int ReOrderRoutePart(string routePartIds, string userSchema = UserSchema.Portal)
        {
            return RouteManagerDao.ReOrderRoutePart(routePartIds, userSchema);
        }
        #endregion

        #region GetRoutePartDetails
        public List<RoutePartDetails> GetRoutePartDetails(string notificationidVal, int? isNenViaPdf, int? isHistoric, int orgId, string userSchema)
        {
            return RouteManagerDao.GetRoutePartDetails(notificationidVal, isNenViaPdf, isHistoric, orgId, userSchema);
        }
        #endregion

        #region NEN Via API route functions
        public List<NenRouteList> CloneNenRoute(CloneNenRoute cloneNenRoute)
        {
            return RouteManagerDao.CloneNenRoute(cloneNenRoute);
        }
        public List<AppRouteList> GetPlannedNenAPIRouteList(string contRefNum, int orgId, string userSchema)
        {
            return RouteManagerDao.GetPlannedNenAPIRouteList(contRefNum, orgId, userSchema);
        }
        public List<RoutePartDetails> GetNenApiRoutesForAnalysis(string contRefNum, int orgId, string userSchema)
        {
            return RouteManagerDao.GetNenApiRoutesForAnalysis(contRefNum, orgId, userSchema);
        }
        #endregion

        #region NEN PDF Route details
        public List<RoutePartDetails> GetNenPdfRoutesForAnalysis(int inboxItemId, int orgId, string userSchema)
        {
            return RouteManagerDao.GetNenPdfRoutesForAnalysis(inboxItemId, orgId, userSchema);
        }
        #endregion
    }
}