using STP.Common.Constants;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Routes;
using System.Collections.Generic;
using static STP.Domain.Routes.RouteModel;

namespace STP.Routes.Interface
{
    public interface IRouteManager
    {
        List<RoutePartDetails> LibraryRouteList(int organisationID, int pageNumber, int pageSize, int routeType, string serchString, string userSchema, int filterFavouritesRoutes,int presetFilter,int? sortOrder=null);
        RoutePart GetLibraryRoute(long plannedRouteID, string userSchema);
        long SaveLibraryRoute(RoutePart routePart, string userSchema);
        long UpdateLibraryRoute(RoutePart routePart, string userSchema);
        int DeleteLibraryRoute(long plannedRouteID, string userSchema);
        long AddRouteToLibrary(long routePartId, int orgId, string rtType, string userSchema);
        RoutePart GetApplicationRoute(long routePartId, string userSchema = UserSchema.Portal);
        RoutePart GetHistoricAppRoute(long routeId, string userSchema);
        List<RoutePart> UpdateCloneHistoricAppRoute(UpdateHistoricCloneRoute updateHistoricClone);
        RoutePart GetCandidateOutlineRoute(long routePartId, string userSchema);
        RoutePart GetApplicationRoutePartGeometry(long partId, string userSchema);
        long SaveApplicationRoute(SaveAppRouteParams saveAppRouteParams);
        long UpdateApplicationRoute(SaveAppRouteParams updateAppRouteParams);
        int DeleteApplicationRoute(long routeId, string routeType, string userSchema);
        int SaveMapUsage(int userId, int organisationId, int type);
        bool SaveRouteAnnotation(RoutePart routePart, int type, string userSchema);
        List<NotifRouteImport> ListRouteImportDetails(string contentReferenceNo);
        int CheckRouteName(string RouteName, int organisationId, string userSchema);
        long GetRoutePathId(long routeId, int isLib, string userSchema);
        List<RoutePoint> GetRoutePoints(long routePathId, string userSchema);
        bool UpdateNotifPlanRoute(int RoutePartId, string contentrefno, int RoutePartNo, int ImportVeh = 0, int Flag = 0);
        long SaveSNotificationRoute(int routepartId, string ContentRefNo);
        int VerifyApplicationRouteName(ApplicationRouteNameParams objApplicationRouteNameParams);
        List<RoutePoint> GetRouteDetails(string contentReferenceNo);
		int GetRoutePartsCount(string contentReferenceNo);
        long SaveNotificationRoute(int routePartId, int versionId, string contentRefNo, int routeType);
        int DeleteOldRouteDetails(long newRoutePartId, string contentRefNo, int oldRoutePartId);
        int UpdateRoutePartId(int newRoutePartId, int oldRoutePartId, string contentRefNo = "");
        List<ListRouteVehicleId> GetNotifRouteDetails(string contentReferenceNo);
        int DeleteOldReturnLeg(string contentReferenceNo);
        List<RoutePoint> GetRoutePointsForReturnLeg(int libraryRouteId, long planRouteId);
        int DeleteOldRouteDetailsForImport(long newRoutePartId, string contentReferenceNo, int routePartNo);
		long SaveSOAppImportRoute(int routePartId, int appRevId, int routeType, string userSchema);
        long ImportRouteFromLibrary(int routePartId, int versionId, int appRevId, int routeType, string contentRef, string userSchema);
        long SaveRouteInRouteParts(int routePartId, int appRevId, int versionId, string contentRef, string userSchema);
        long SaveRouteInAppParts(int routePartId, int appRevId, string userSchema);
        long SaveAnnotationInLibrary(int organisationId, int userId, long annotationType, string annotationText, int structureId = 0, string userSchema = UserSchema.Portal);
        List<AnnotationTextLibrary> GetAnnotationsFromLibrary(int organisationId, int userId, int pageNumber, int pageSize, long annotationType, string annotationText, int structureId = 0, string userSchema = UserSchema.Portal);
        long GetRoutePartId(string conRefNumber, string userSchema);
        List<RoutePoint> GetRoutePointsDetails(int PlanRouteID);
        List<AppRouteList> GetAuthorizedRoutePartList(long versionId, string userSchema);
        List<AppRouteList> GetPlannedNenRouteList(long nenId, int userId, long inboxItemId, int orgId);
        List<AppRouteList> GetSoAppRouteList(long revisionId, string userSchema);
        List<AppRouteList> NotifVR1RouteList(long revisionId, string contRefNum, long versionId, string userSchema);
        List<NotifRouteImport> ListBrokenRouteDetails(string contentReferenceNumber, string userSchema, long appRevisionID, long revisionID, long movementVersionID);
        List<RouteAnnotation> GetBrokenRouteAnnotations(long segmentId, int is_lib, string userSchema);
        List<RoutePoint> GetBrokenRoutePoints(long routePathId = 0, int isLib = 0, string userSchema = "");
        List<BrokenRouteList> GetBrokenRouteIds(GetBrokenRouteList getBrokenRouteList);
        bool UpdateBrokenRoutePath(UpdateBrokenRoutePathParam objUpdateBrokenRoutePathParam);
        int SetVerificationStatus(VerificationStatusParams objVerificationStatusParams);
        List<RoutePartDetails> GetFavouriteRoutes(int organisationId, string userSchema);
        long SaveNERoute(SaveNERouteParams saveNERoute, bool isAutoPlan);
        List<AppRouteList> GetSortMovementRoute(long revisionId, int rListType);
        List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema);
        int CheckRouteVehicleAttach(long routePartId);
        List<RouteLinkModel> GetHistoricRouteLinkModel(long segmentId, int segmentNo, int tolerance, string userSchema);
        List<RoutePartDetails> GetRoutePartDetails(string notificationidVal, int? isNenViaPdf, int? isHistoric, int orgId, string userSchema);
        List<NenRouteList> CloneNenRoute(CloneNenRoute cloneNenRoute);
        List<AppRouteList> GetPlannedNenAPIRouteList(string contRefNum, int orgId, string userSchema);
        List<RoutePartDetails> GetNenApiRoutesForAnalysis(string contRefNum, int orgId, string userSchema);
        List<RoutePartDetails> GetNenPdfRoutesForAnalysis(int inboxItemId, int orgId, string userSchema);
    }
}
