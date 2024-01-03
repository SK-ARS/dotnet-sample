var CandVersionNo = $('#hf_candversionno').val();
var CandName = $('#hf_candname').val();
$('body').on('click', '#btn-importroute', function (e) {
    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var RouteName = $(this).data('routename');
    ImportRoute(RouteID, RouteName);
});
function ImportRoute(RouteId, RNam) {
    var RevId = $('#revisionId').val();
    $.ajax({
        url: "../SORTApplication/ImportRouret",
        type: 'post',
        data: { routepartId: RouteId, AppRevId: RevId, routeType: "planned" },
        success: function (result) {
            CheckIsBroken({ RoutePartId: result.result, IsReplanRequired: true }, function (response) {
                BrokenSortCandadteRouteReplan(response, RNam);
            });
        },
        error: function () {
        }
    });
}
function BrokenSortCandadteRouteReplan(response, RNam) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        if (res.IsReplan > 1) {
            GetSortCandadteRouteReplanMessage(false);
        }
        else {
            GetSortCandadteRouteReplanMessage(true, response.autoReplanSuccess > 0);
        }
    }
    else {
        ShowSuccessModalPopup("'" + RNam + "' route imported successfully", "ShowRouteList");
    }
}
function GetSortCandadteRouteReplanMessage(isReplan, isReplanSuccess = false) {
    var msg = "";
    var isPreviousMvmnt = $("#IsPreviousMvmnt").val() == 1;
    if (!isReplan) {
        msg = isPreviousMvmnt ? BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_DETAILS_IS_REPLAN_NOT_POSSIBLE_FROM_PREV_MVMNT : BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_DETAILS_IS_REPLAN_NOT_POSSIBLE_NOT_FROM_CURRENT_MVMNT;
    }
    else {
        if (isReplanSuccess) {
            $('#IsRouteReplanned').val("true");
            msg = isPreviousMvmnt ? BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_DETAILS_IS_REPLAN_SUCCESS_FROM_PREV_MVMNT : BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_DETAILS_IS_REPLAN_SUCCESS_NOT_FROM_CURRENT_MVMNT;
        }
        else {
            msg = isPreviousMvmnt ? BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_DETAILS_IS_REPLAN_FAILED_FROM_PREV_MVMNT : BROKEN_ROUTE_MESSAGES.CANDIDATE_ROUTE_DETAILS_IS_REPLAN_FAILED_NOT_FROM_CURRENT_MVMNT;
        }
    }

    ShowWarningPopupMapupgarde(msg, function () {
        $('#WarningPopup').modal('hide');
        ShowRouteList();
    });
}
function ShowRouteList() {
    if ($('#SortStatus').val() == "CreateSO")
        SelectedRouteFromLibraryForVR1();
    else {
        BindRouteParts();
    }
}
//Candidate route binding.
function BindRouteParts() {
    CloseSuccessModalPopup();
    $('#StruRelatedMov').hide();
    $('#RoutePart').show();
    $('#divCurrentMovement').hide();
    $("#back_currentmovmnt").hide();
    $('#SelectCurrentMovements1').hide();
    $('#SelectCurrentMovements2').hide();
    $("#divCandiRouteDeatils").html('');
    $('#route').hide(); //hide map
    $('#back_btn_Rt').hide();
    startAnimation();
    $("li[id='4']").show();
    $("li[id='4']").addClass('t');
    var rtrevisionId = $('#revisionId').val();
    var iscandlastversion = $('#IsCandVersion').val();
    var plannruserid = $('#PlannrUserId').val();
    var appstatuscode = $('#AppStatusCode').val();
    var movversionno = $('#versionno').val();
    var movdistributed = $('#IsMovDistributed').val();
    var sonumber = $('#SONumber').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var prjstatus = $('#Proj_Status').val();

    //_checkerid = 15;
    if (rtrevisionId == 0) {
        $('#leftpanel').hide();
    }
    else {
        $(".tab_content1").each(function () {
            $(this).hide();
        });
        $.ajax({
            url: "../SORTApplication/ShowCandidateRoutes",
            type: 'post',
            async: false,
            data: { routerevision_id: rtrevisionId, CheckerId: _checkerid, CheckerStatus: _checkerstatus, IsCandLastVersion: iscandlastversion, planneruserId: plannruserid, appStatusCode: appstatuscode, SONumber: sonumber },
            success: function (data) {
                $('#tab_4').show();
                $('#RoutePart').html('');
                $('#RoutePart').html(data);

                //$("#leftpanel_quickmenu").html('');
                $('#leftpanel').html('');
                $('#leftpanel').hide();
                $('#sort11').hide();
                $('#information').hide();
                $('#btn_cancel').hide();
                stopAnimation();

                var prjheader = hauliermnemonic + "/" + esdalref + " - SO - " + prjstatus + " Candidate route(" + CandName + " - Version " + CandVersionNo + ") - Route";
                $('div#pageheader').html('');
                $('div#pageheader').append('<h3>' + prjheader + '</h3>');

            },
            error: function () {
            }
        });
    }
}
