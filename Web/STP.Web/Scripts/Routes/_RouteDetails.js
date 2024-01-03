$(document).ready(function () {
    //Add new route dropdown
    $('body').on('click', '#IDCreateRoute', function (e) {
        e.preventDefault();
        var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
        if (isUnsavedChange) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "clearRoute", "CloseWarningPopupRef");
        }
        else {
            clearRoute();
            ClearRouteSessionFlag();
            $('.EditRouteCLS').removeClass('active-route');
            setRouteID(0);
            if (typeof mapInputValuesGlobal != 'undefined') mapInputValuesGlobal = [];
        }
    });
    //Select existing route from dropdown
    $('body').on('click', '.EditRouteCLS', function (e) {
        e.preventDefault();
        var isUnsavedChange = ValidateUnSavedChange("map");
        var isRouteSummaryFlag = $('#IsRouteSummary_Flag').val();
        var currentmapstate = IsMapnotidle();
        if ((isUnsavedChange || currentmapstate) && isRouteSummaryFlag!="1") {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "EditRoute1", "CloseWarningPopupRef", this);
        }
        else {
            EditRoute1(this);
        }
    });
    $('body').on('click', '.AddRouteLibCls', function (e) {
        e.preventDefault();
        var routeId = $(this).data('routeid');
        var routeType = $(this).data('routetype');
        var routeName = $(this).data('routename');
        AddRouteToLibrary(routeId, routeType, routeName);
    });
    $('body').on('click', '.DeleteRouteCls', function (e) {
        e.preventDefault();
        var routeId = $(this).data('routeid');
        var routeType = $(this).data('routetype');
        var routeName = $(this).data('routename');
        var isUnsavedChange = ValidateUnSavedChange("map");
        var currentmapstate = IsMapnotidle();
        if (isUnsavedChange || currentmapstate) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "DeleteRoute", "CloseWarningPopupRef", routeId, routeType, routeName);
        }
        else {
            DeleteRoute(routeId, routeType, routeName);
        }
    });
    $('body').on('click', '.AddReturnRouteCLS', function (e) {
        e.preventDefault();
        var routeId = $(this).data('routeid');
        var routeName = $(this).data('routename');
        AddReturnRoute(routeId, routeName);
    });
});
function EditRoute1(_this) {
    var routeId = $(_this).data('routeid');
    var routeName = $(_this).data('routename');
    var routeType = $(_this).data('routetype');
    setRouteNameEdit(routeName);
    $('#hf_RouteID').val(routeId);
    $('#chkRouteID').val(routeId);
    if ($('#IsRouteSummary_Flag').val() != "1") {
        var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
        if (isUnsavedChange) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "EditRoute", "CloseWarningPopupRef", routeId, routeName, routeType);
        }
        else {
            EditRoute(routeId, routeName, routeType);
        }
    }
    else {
        $('#viewHfRouteId').val(routeId);
        setRouteNameEdit(routeName);
        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', {
            apprevisionId: RevisionIdVal, versionId: VersionIdVal, contRefNum: ContenRefNoVal, isNotif: IsNotifVal, workflowProcess: "HaulierApplication",
            IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), IsRouteSummaryPage: 0
        }, '#select_route_section', '', function () {
            MovementRouteInit();
        });
    }
}
function AddReturnRoute(mainRouteId, mainRouteName) {
    $.ajax({
        type: "POST",
        url: "../Routes/AddReturnRoute",
        data: { routePartId: mainRouteId },
        beforeSend: function () {
            startAnimation('Planning return leg');
        },
        success: function (response) {
            AddReturnRoutePart(response, mainRouteName,0, function (returnRouteId) {
                setReturnRouteWorkFlowPayload(response.result.RouteId, returnRouteId, function () {
                    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication', IsRouteSummaryPage: 1 }, '#select_route_section', '', function () {
                        MovementRouteInit(true);
                    });
                });
            });
        }
    });
}
function setReturnRouteWorkFlowPayload(mainRouteId, returnRouteId, callbackFn) {
    $.ajax({
        type: "POST",
        url: "../Routes/SetWorkFlowPayload",
        data: { mainRoutePartId: mainRouteId, returnRoutePartId: returnRouteId },
        success: function () {
            callbackFn();
        }
    });
}