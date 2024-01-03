function GenerateNotificationRouteAssessment(contentRefNo, notificationId, analysisId, VSOType, isRenotify, prevContacName, callbackFn) {
    stopAnimation();
    openContentLoader('body');
    var generateRouteAssessment = {
        ContentRefNo: contentRefNo,
        NotificationId: notificationId,
        AnalysisId: analysisId,
        VSoType: VSOType,
        IsRenotify: isRenotify,
        PreviousContactName: prevContacName
    }
    $.ajax({
        url: '../RouteAssessment/GenerateRouteAssessment',
        type: 'POST',
        data: generateRouteAssessment,
        success: function () {
            if (typeof callbackFn != 'undefined' && callbackFn != null) {
                callbackFn();
            }
        }
    });
}

function GenerateRouteAssessment(versionId, revisionId, analysisId, prevAnalysisId, appRevisionId, callbackFn) {
    var generateRouteAssessment = {
        VersionId: versionId,
        AnalysisId: analysisId,
        RevisionId: revisionId,
        AppRevId: appRevisionId,
        PrevAnalysisId: prevAnalysisId
    }
    $.ajax({
        url: '../RouteAssessment/GenerateRouteAssessment',
        type: 'POST',
        data: generateRouteAssessment,
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (typeof callbackFn != 'undefined' && callbackFn != null) {
                callbackFn();
            }
        }
    });
}