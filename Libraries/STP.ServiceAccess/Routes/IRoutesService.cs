using STP.Common.Constants;
using STP.Domain;
using STP.Domain.Applications;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.NonESDAL;
using STP.Domain.Routes;
using System;
using System.Collections.Generic;
using static STP.Domain.Routes.RouteModel;

namespace STP.ServiceAccess.Routes
{
    public interface IRoutesService
    {
        List<RoutePartDetails> LibraryRouteList(int organisationID, int pageNumber, int pageSize, int routeType, string serchString, string userSchema,int filterFavouritesRoutes,int presetFilter,int? sortOrder=null);
        RoutePart GetLibraryRoute(long plannedRouteID, string userSchema);
        long SaveLibraryRoute(RoutePart routePart, string userSchema);
        long UpdateLibraryRoute(RoutePart routePart, string userSchema);
        int DeleteLibraryRoute(long plannedRouteID, string userSchema);
        long AddRouteToLibrary(long routePartId, int orgId, string routeType, string userSchema);
        int CheckRouteName(string routeName, int organisationId, string userSchema);
        RoutePart GetApplicationRoute(long routePartId, string userSchema);
        RoutePart GetHistoricAppRoute(long routePartId, string userSchema);
        RoutePart GetCandidateOutlineRoute(long routePartId, string userSchema);
        RoutePart GetApplicationRoutePartGeometry(long partId, string userSchema);
        long SaveApplicationRoute(RoutePart routePart, long versionId, long revisionId, string contRefNum, int dockFlag, long routeRevisionid, string userSchema, bool IsReturnRoute);
        long UpdateApplicationRoute(RoutePart routePart, long versionId, long revisionId, string contRefNum, int dockFlag, long routeRevisionid, string userSchema);
        int DeleteApplicationRoute(long routeId, string routeType, string userSchema);
        List<NotifRouteImport> ListRouteImportDetails(string contentRefNo);
        bool SaveRouteAnnotation(RoutePart routePart, int type, string userSchema);
        long GetRoutePathId(long routeId, int isLib, string userSchema = UserSchema.Portal);
        List<RoutePoint> GetRoutePoints(long routePathId, string userSchema);
        bool UpdateNotifPlanRoute(int RoutePartId, string contentrefno, int RoutePartNo, int ImportVeh = 0, int Flag = 0);
        long SaveSNotificationRoute(int routepartId, string ContentRefNo);
        int VerifyApplicatiponRouteNameValidation(string RouteName, int RevisionId, string ContentRefNo, int RouteFor, string UserSchema);
        List<RoutePoint> GetRouteDetails(string ContentRefNo);
        int GetRoutePartsCount(string ContentRefNo);
        long SaveNotificationRoute(int routePartId, int versionId, string contentRefNo, int routeType);
        int DeleteOldRouteDetails(long newRoutePartId, string contentRefNo, int oldRoutePartId);
        int UpdateRoutePartId(long newRoutePartId, int oldRoutePartId, string contentRefNo);
        List<ListRouteVehicleId> GetNotifRouteDetails(string contentRefNo);
        int DeleteOldReturnLeg(string contentRefNo);
        List<RoutePoint> GetRoutePointsForReturnLeg(int libraryRouteId, long planRouteId);
        int DeleteOldRouteDetailsForImport(long newRoutePartId, string contentRefNo, int routePartNo);
        long SaveSOAppImportRoute(int routePartId, int appRevId, int routeType, string userSchema);
        long ImportRouteFromLibrary(int routePartId, int versionId, int appRevId, int routeType, string contentRef, string userSchema);
        long SaveRouteInRouteParts(int routePartId, int appRevId, int versionId, string contentRef, string userSchema);
        long SaveRouteInAppParts(int routePartId, int appRevId, string userSchema);
        List<RoutePoint> GetRoutePointsDetails(int PlanRouteID);
        long GetRoutePartId(string conRefNumber, string userSchema);
        List<AppRouteList> GetAuthorizedRoutePartList(long versionId, string userSchema);
        List<AppRouteList> GetPlannedNenRouteList(long nenId, int userId, long inboxItemId, int orgId);
        List<AppRouteList> GetSoAppRouteList(long revisionId, string userSchema);
        List<AppRouteList> NotifVR1RouteList(long revisionId, string contRefNum, long versionId, string userSchema);
        List<NotifRouteImport> ListBrokenRouteDetails(string contentReferenceNumber = null, string userSchema = UserSchema.Portal, long appRevisionID = 0, long revisionID = 0, long movementVersionID = 0);
        List<RoutePoint> GetBrokenRoutePoints(long routePathId = 0, int isLib = 0, string userSchema = "");
        List<RouteAnnotation> GetBrokenRouteAnnotations(long segmentId, int is_lib, string userSchema);
        List<BrokenRouteList> GetBrokenRouteIds(GetBrokenRouteList getBrokenRouteList);
        int CheckIsBroken(long routePartId, string userSchema);
        bool UpdateBrokenRoutePath(List<RoutePath> routePathList, int is_lib, string userSchema = UserSchema.Portal);
        int SetVerificationStatus(int routeId, int is_lib, int replanStatus, string userSchema);
        List<RoutePartDetails> GetFavouriteRoutes(int organisationId, string userSchema);
        int SaveMapUsage(int userId, int organisationId, int type);
        List<AppRouteList> GetSortMovementRoute(long revisionId, int rListType);
        long SaveNERoute(SaveNERouteParams saveNERoute);
        List<RoutePartDetails> GetRouteDetailForAnalysis(long versionId, string contentRefNo, long revisionId, int isCandidate, string userSchema);
        int CheckRouteVehicleAttach(long routePartId);
        List<Domain.ExternalAPI.Route> ExportRouteDetails(Domain.ExternalAPI.GetRouteExportList routeExportList);
        int SaveAnnottationTextToLibrary(long organisationId, string userId, long annotationType, string annotationText, long structureId = 0, string userSchema = UserSchema.Portal);
        List<long> UpdateCloneHistoricRoute(string contentRefNum, long appRevisionId, long versionId, string userSchema);
        List<AnnotationTextLibrary> GetAnnottationTextListLibrary(int pageNumber,int pageSize,long organisationId, long userId, long annotationType, string annotationText, long structureId = 0, string userSchema = UserSchema.Portal);
        int ReOrderRoutePart(string routePartIds, string userSchema = UserSchema.Portal);
        List<RoutePartDetails> GetRoutePartDetails(string notificationidVal, int? isNenViaPdf, int? isHistoric, int orgId, string userSchema);
        List<NenRouteList> CloneNenRoute(CloneNenRoute cloneNenRoute);
        List<AppRouteList> GetPlannedNenAPIRouteList(string contRefNum, int orgId);
        List<RoutePartDetails> GetNenApiRoutesForAnalysis(string contRefNum, int orgId, string userSchema);
        List<RoutePartDetails> GetNenPdfRoutesForAnalysis(int inboxItemId, int orgId, string userSchema);
    }
}


