var route_id = 0;
var rt_type = '';
var rt_Name;
$(document).ready(function () {
    $('body').on('click', '#btnGotoRouteLibrary', gotoRouteLibrary);
    $('body').on('click', '#span-close', ClosePopUp);
    $('body').on('click', '#span-help', help_poup);
    $('body').on('click', '#btnBactoGen', BactoGenRoute);
    $('body').on('click', '#spanShowRouteDetails', function (e) {
        var routeId = $(this).data('routeId');
        var routeType = $(this).data('routeType');
        ShowRouteDetails(routeId, routeType);
    });
    $('body').on('click', '#btnImportRouteInApp', function (e) {
        var routeId = $(this).data('btnImportrouteId');
        var routeType = $(this).data('btnImportrouteType');
        Importrouteinapp(routeId, routeType);
    });
    $('body').on('click', '#btnImportRoute', function (e) {
        var routeId = $(this).data('btnImportRouterouteId');
        var routeName = $(this).data('btnImportRouterouteName');
        ImportRoute(routeId, routeName);
    });
});
function BactoGenRoute() {
    $("#divCandiRouteDeatils").hide();
    $('#generalDetailDiv').show();
    $('#route').hide(); //hide if the route div is open
    $('#back_btn_Rt').hide();
}
function ShowRouteList() {
    if ($('#SortStatus').val() == "CreateSO")
        SelectedRouteFromLibraryForVR1();
    else {
        WarningCancelBtn();
        BindRouteParts();
    }
}
function Importrouteinapp(routeID, routetype) {
    WarningCancelBtn();
    var routename = $('#btnrouteimport_' + routeID).data('name');
    if (routename == null)
        routename = $("#RouteName1").text();
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var SOVersionID = $('#RevisionID').val(); //Previous Movement version id
    var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRT").val();
    var vr1contrefno = $('#VR1ContentRefNo').val();
    var vr1versionid = $('#VersionId').val();
    $("#divCurrentMovement").hide();
    $.ajax({
        url: '../Application/SaveRouteInAppParts',
        type: 'POST',
        data: { routepartId: routeID, routeType: routetype, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result != 0) {
                CheckIsBroken({ RoutePartId: result, IsReplanRequired: true }, function (response) {
                    CandidateBrokenRouteReplan(response);
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function CandidateBrokenRouteReplan(response) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        if (res.IsReplan > 1) {
            GetCandidateReplanMessage(false);
        }
        else {
            GetCandidateReplanMessage(true, response.autoReplanSuccess > 0);
        }
    }
    else {
        $('#back_btn').show();
        $('#back_btn_Rt_prv').hide();
        ShowRouteList();
    }
}
function GetCandidateReplanMessage(isReplan, isReplanSuccess = false) {
    var msg = "";
    var isPreviousMvmnt = $("#IsPreviousMvmnt").val() == 1;
    if (!isReplan) {
        msg = isPreviousMvmnt ? BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_LIST_IS_REPLAN_NOT_POSSIBLE_FROM_PREV_MVMNT : BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_LIST_IS_REPLAN_NOT_POSSIBLE_FROM_CURRENT_MVMNT;
    }
    else {
        if (isReplanSuccess) {
            $('#IsRouteReplanned').val("true");
            msg = isPreviousMvmnt ? BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_LIST_IS_REPLAN_SUCCESS_FROM_PREV_MVMNT : BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_LIST_IS_REPLAN_SUCCESS_FROM_CURRENT_MVMNT;
        }
        else {
            msg = isPreviousMvmnt ? BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_LIST_IS_REPLAN_FAILED_FROM_PREV_MVMNT : BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_LIST_IS_REPLAN_FAILED_FROM_CURRENT_MVMNT;
        }
    }
    ShowWarningPopupMapupgarde(msg, function () {
        $('#WarningPopup').modal('hide');
        ShowRouteList();
        $("#RoutePart").show();
        $("#ShowDetail").hide();
        $("#RouteMap").html('');
        if ($('#UserTitle').html() == "SORT Portal") {
            CloneRoutesSort();
        }
        else {
            CloneRoutes();
        }
    });
}
