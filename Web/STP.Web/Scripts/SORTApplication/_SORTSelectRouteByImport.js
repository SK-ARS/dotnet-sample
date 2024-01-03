var ImportFrmVal = $('#hf_ImportFrm').val() || $('#hf_importFrm').val();
var IsFavouriteVal = $('#hf_IsFavourite').val();
$(document).ready(function () {
    StepFlag = 4;
    SubStepFlag = 4.1;
    CurrentStep = "Route Details";
    SelectMenu(2);
    $('#confirm_btn').hide();
    $('#back_btn').show();
    $('#IsVehicle').val(false);
    $('#ImportFrom').val(ImportFrmVal);
    $('#IsFavouriteRoute').val(IsFavouriteVal);

    if ($('#hf_ImportFrm').val() == 'library') {
        $('#list_heading').text("Select route from library");
        SelectRouteFromLibrary();
    }
    if ($('#hf_ImportFrm').val() == 'prevMov') {
        $('#IsVehicle').val(false);
        $('#list_heading').text("Select route from previous movements");
        UseMovement();
    }
});


$('body').on('click', '#importroutein', function (e) {
    e.preventDefault();
    var routeid = $(this).data('routeid');
    var routetype = $(this).data('routetype');
    ImportRouteInAppParts(routeid, routetype);
});
function ImportRouteInAppLibrary(routeID, routetype) {
    var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
    $('#IsRouteModify').val(1);
    $('#IsReturnRoute').val(0);
    $('#IsReturnRouteAvailable_Flag').val(false);
    //added by poonam (13.8.14)
    if ($('#CRNo').val() == undefined) {
        var vr1contrefno = $('#VR1ContentRefNo').val();
    }
    else {
        var vr1contrefno = $('#CRNo').val();// $('#VR1ContentRefNo').val();
    }
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
                CheckIsBroken({ RoutePartId: result.result, IsReplanRequired: true }, function (brokenRouteList) {
                    BrokenRouteReplanSortImport(brokenRouteList, true);
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
        var vr1contrefno = $('#VR1ContentRefNo').val() ? $('#VR1ContentRefNo').val() : null;
        var vr1versionid = $('#AppVersionId').val();
        //-----------

        $.ajax({
            url: '../Routes/ImportRouteFromPrevious',
            type: 'POST',
            cache: false,
            data: { routepartId: RouteId, routeType: RouteType, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result != 0) {
                    $('#viewHfRouteId').val(result);
                    CheckIsBroken({ RoutePartId: result, IsReplanRequired: true }, function (response) {
                        BrokenRouteReplanSortImport(response);
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
function GetReplanMessageSortImport(isReplan, isLib, isReplanSuccess = false) {
    var msg = "";
    if (!isReplan) {
        msg = isLib ? BROKEN_ROUTE_MESSAGES.SORT_ROUTE_BY_IMPORT_IS_REPLAN_NOT_POSSIBLE_FROM_LIBRARY : BROKEN_ROUTE_MESSAGES.SORT_ROUTE_BY_IMPORT_IS_REPLAN_NOT_POSSIBLE_NOT_FROM_LIBRARY;
    }
    else {
        if (isReplanSuccess) {
            $('#IsRouteReplanned').val("true");
            msg = isLib ? BROKEN_ROUTE_MESSAGES.SORT_ROUTE_BY_IMPORT_IS_REPLAN_SUCCESS_FROM_LIBRARY : BROKEN_ROUTE_MESSAGES.SORT_ROUTE_BY_IMPORT_IS_REPLAN_SUCCESS_NOT_FROM_LIBRARY;
        }
        else {
            msg = isLib ? BROKEN_ROUTE_MESSAGES.SORT_ROUTE_BY_IMPORT_IS_REPLAN_FAILED_FROM_LIBRARY : BROKEN_ROUTE_MESSAGES.SORT_ROUTE_BY_IMPORT_IS_REPLAN_FAILED_NOT_FROM_LIBRARY;
        }
    }
    ShowWarningPopupMapupgarde(msg, function () {
        $('#WarningPopup').modal('hide');
        ReloadToMovementRouteSortImport();
    });
}
function BrokenRouteReplanSortImport(response, isLib = false) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        if (res.IsReplan > 1) {
            GetReplanMessageSortImport(false, isLib);
        }
        else {
            GetReplanMessageSortImport(true, isLib, response.autoReplanSuccess > 0);
        }
    }
    else {
        $('#back_btn').show();
        $('#back_btn_Rt_prv').hide();
        ReloadToMovementRouteSortImport();
    }
}
function ReloadToMovementRouteSortImport() {
    WarningCancelBtn();
    $('#back_btn').show();
    $('#back_btn_Rt_prv').hide();
    var vr1versionid = $('#AppVersionId').val();
    var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { workflowProcess: 'HaulierApplication', apprevisionId: AppRevId, versionId: vr1versionid, isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section', '', function () {
        MovementRouteInit();
    });
}

