var hf_ImportFrm;
var hf_isApplicationVehicle;
var hf_IsFavourite;
var hf_IsSort;
var hf_BackToMovementPreviousList;
var getBrokenRouteList = {
    RoutePartId: 0,
    VersionId: 0,
    AppRevisonId: 0,
    LibraryRouteId: 0,
    ConteRefNo: "",
    CandRevisionId: 0
};
function SelectRouteByImportInit() {
    IsRoutePlanned = false;
    hf_ImportFrm = $('#hf_ImportFrm').val() || $('#hf_importFrm').val();
    hf_isApplicationVehicle = $('#hf_isApplicationVehicle').val();
    hf_IsFavourite = $('#hf_IsFavourite').val();
    hf_IsSort = $('#hf_IsSort').val();
    hf_BackToMovementPreviousList = $('#hf_BackToMovementPreviousList').val();
    StepFlag = 4;
    SubStepFlag = 4.1;
    CurrentStep = "Route Details";
    SetWorkflowProgress(4);
    $('#confirm_btn').hide();
    $('#back_btn').show();
    $('#IsVehicle').val(false);
    $('#ImportFrom').val(hf_ImportFrm);
    $('#IsFavouriteRoute').val(hf_IsFavourite);
    if ($('#hf_ImportFrm').val() == 'library') {
        var clearSearch = true;
        $('#list_heading').text("Select route from library");
        SelectRouteFromLibrary(clearSearch);
    }
    if (hf_ImportFrm == 'prevMov') {
        $('#IsVehicle').val(false);
        $('#list_heading').text("Select route from previous movements");
        UseMovement($('#IsSortUser').val(), hf_BackToMovementPreviousList);
    }
}

function ImportRouteInAppLibrary(routeID, routetype) {

    var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
    $('#IsRouteModify').val(1);
    $('#IsReturnRoute').val(0);
    $('#IsReturnRouteAvailable_Flag').val(false);
    var vr1contrefno = $('#CRNo').val() == undefined ? $('#VR1ContentRefNo').val() : $('#CRNo').val();
    var vr1versionid = $('#AppVersionId').val();
    $.ajax({
        url: '../Routes/ImportRouteFromLibrary',
        type: 'POST',
        async: true,
        cache: false,
        data: { routepartId: routeID, routetype: routetype, AppRevId: AppRevId, CONTENT_REF: vr1contrefno, VersionId: vr1versionid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {

            if (result.result != 0) {
                $('#viewHfRouteId').val(result.result);
                CheckIsBroken({ RoutePartId: result.result, IsReplanRequired: true }, function (response) {
                    BrokenRouteReplan(response, true);
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function BrokenRouteReplan(response, isLib = false) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        if (res.IsReplan > 1) {
            GetReplanMessage(false, isLib);
        }
        else {
            GetReplanMessage(true, isLib, response.autoReplanSuccess > 0);
        }
    }
    else {
        $('#back_btn').show();
        $('#back_btn_Rt_prv').hide();
        ReloadToMovementRoute();
    }
}

function GetReplanMessage(isReplan, isLib, isReplanSuccess = false) {
    var msg = "";
    if (!isReplan)
        msg = isLib ? BROKEN_ROUTE_MESSAGES.RI_NOT_IS_REPLAN_IS_LIB : BROKEN_ROUTE_MESSAGES.RI_NOT_IS_REPLAN_IS_NOT_LIB;
    else {
        if (isReplanSuccess) {
            $('#IsRouteReplanned').val("true");
            msg = isLib ? BROKEN_ROUTE_MESSAGES.RI_IS_REPLAN_IS_LIB : BROKEN_ROUTE_MESSAGES.RI_IS_REPLAN_IS_NOT_LIB;
        }
        else 
            msg = isLib ? BROKEN_ROUTE_MESSAGES.RI_IS_REPLAN_FAIL_IS_LIB : BROKEN_ROUTE_MESSAGES.RI_IS_REPLAN_FAIL_IS_NOT_LIB;
    }
    ShowWarningPopupMapupgarde(msg, function () {
        $('#WarningPopup').modal('hide');
        ReloadToMovementRoute();
    });
}

function ImportRouteInAppParts(RouteId, RouteType) {
    if (RouteType != 'planned') {
        ShowErrorPopup('An outline route cannot be imported to the movement');
    }
    else {
        $('#IsRouteModify').val(1);
        $('#IsReturnRoute').val(0);
        $('#IsReturnRouteAvailable_Flag').val(false);
        var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        var SOVersionID = $('#RevisionID').val() ? $('#RevisionID').val() : 0; //Previous Movement version id
        var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
        var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRT").val();
        //added by poonam (14.8.14)
        var vr1contrefno = $('#CRNo').val() ? $('#CRNo').val() : null;
        var vr1versionid = $('#AppVersionId').val();

        $.ajax({
            url: '../Routes/ImportRouteFromPrevious',
            type: 'POST',
            cache: false,
            data: { routepartId: RouteId, routeType: RouteType, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                //Uncommented on 22-03-2023
                if (result != 0) {
                    $('#viewHfRouteId').val(result);
                    CheckIsBroken({ RoutePartId: result, IsReplanRequired: true }, function (brokenRouteList) {
                        BrokenRouteReplan(brokenRouteList);
                    });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}
function ReloadToMovementRoute() {
    var vr1versionid = $('#AppVersionId').val();
    var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
    stopAnimation(); openContentLoader('html');
    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { workflowProcess: 'HaulierApplication', apprevisionId: AppRevId, versionId: vr1versionid, contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section', '', function () {
        MovementRouteInit();
    });
}

