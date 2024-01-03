var CandidateRouteId;
var LastVersion;
var LastRevisionId;
var IsVehicleCurrentMovenet;
var ViewLastRevisionId;

function SORTAppCandidateRouteVersionInit() {
    CandidateRouteId = $('#hf_CandidateRouteId').val();
    LastVersion = $('#hf_LastVersion').val();
    LastRevisionId = $('#hf_LastRevisionId').val();
    IsVehicleCurrentMovenet = $('#hf_IsVehicleCurrentMovenet').val();
    ViewLastRevisionId = $('#hf_ViewLastRevisionId').val();

    $('#LastCandRouteId').val(CandidateRouteId);
    $('#LastCandVersion').val(LastVersion);
    $('#LastCandRevisionId').val(LastRevisionId);
    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
        $('#li_edit_cndRt').hide();
    }
    if (chk_status == 301006) {
        $('#li_edit_cndRt').hide();
    }

    $('.tree li').each(function () {
        if ($(this).children('ol').length > 0) {
            $(this).addClass('parent');
        }
    });
    $('.tree li').each(function () {
        $(this).toggleClass('active');
        $(this).children('ol').slideToggle('fast');
    });
    $('.tree li.parent > a').click(function () {
        $(this).parent().toggleClass('active');
        $(this).parent().children('ol').slideToggle('fast');
    });

    if (typeof isCandidateApiCallCompleted != 'undefined')
        isCandidateApiCallCompleted = true;
}
$(document).ready(function () {
    $('body').on('click', '#ahrefSortCreateCandidateRoute', function (e) {
        e.preventDefault();
        SortCreateCandidateRoute(this);
    });
    $('body').on('click', '#li_edit_cndRt', function (e) {
        e.preventDefault();
        var CnRouteId = $(this).data('cnrouteid');
        EditCandiRouteName(CnRouteId);
    });
    //$('body').on('click', '#ahrefVersinRevision', function (e) {
    //    e.preventDefault();
    //    var VersinRevisionId = $(this).data('versinrevisionid');
    //    var VersinVrListType = $(this).data('versinvrlisttype');
    //    CurreMovemenRouteList(VersinRevisionId, VersinVrListType);
    //});
    $('body').on('click', '#ahrefShowCandidateRoutes', function (e) {
        e.preventDefault();
        var ReviosionId = $(this).data('reviosionid');
        var candname = $(this).data('candname');
        var RevisionNo = $(this).data('revisionno');
        var RouteId = $(this).data('routeid');
        var AnalysisId = $(this).data('analysisid');
        ShowCandidateRoutes(ReviosionId, candname, RevisionNo, RouteId, AnalysisId);
    });
    $('body').on('click', '#ahrefCurreMovemenVehicleList', function (e) {
        e.preventDefault();
        var ReviosionId = $(this).data('reviosionid');
        var SecondParameter = $(this).data('secondparameter');
        CurreMovemenVehicleList(ReviosionId, SecondParameter);
    });
});
function LoadAppAndMoveVersions(Vhauliermnemonic, Vesdalref, Vprojectid) {
    if ($('#IsMyStructure').val() == "Yes" || $('#StruRelatedMove').val() == "Yes") {
        StruApplRevisions_SelectCurrMovmt(IsVehicleCurrentMovenet);
        StruMovementVersion__SelectCurrMovmt(IsVehicleCurrentMovenet);
    }
    else {
        ApplRevisions_SelectCurrMovmt(IsVehicleCurrentMovenet, Vhauliermnemonic, Vesdalref, Vprojectid);
        MovementVersion__SelectCurrMovmt(IsVehicleCurrentMovenet, Vhauliermnemonic, Vesdalref, Vprojectid);
    }
}
function ApplRevisions_SelectCurrMovmt(V_IsVehicleCurrentMovenet, haulierMnemonic, esdalRef, VprojectId) {
    var OwnerName = '', plannruserid;
    OwnerName = '';
    plannruserid = $('#PlannrUserId').val();
    checkingstatus = $('#CheckerStatus').val();
    _isvr1 = $('#VR1Applciation').val();
    Enter_BY_SORT = $('#EnterBySort').val();
    var N_IsRouteCurrentMovenet = true;
    if (V_IsVehicleCurrentMovenet == 'True')
        N_IsRouteCurrentMovenet = false;
    startAnimation()
    Checker = "";
    var _pstatus = $('#AppStatusCode').val();
    $('#SelectCurrentMovementsVehicle1').load('../SORTApplication/SORTApplRevisions?ProjectID=' + VprojectId + '&hauliermnemonic=' + haulierMnemonic + '&esdalref=' + esdalRef + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
        function () {
            $('#SelectCurrentMovementsVehicle1').show();
            stopAnimation()
        });
}
function MovementVersion__SelectCurrMovmt(V_IsVehicleCurrentMovenet, haulierMnemonic, esdalRef, VprojectId) {
    var OwnerName = '';
    var _isvr1 = $('#VR1Applciation').val();
    var Enter_BY_SORT = $('#EnterBySort').val();

    var N_IsRouteCurrentMovenet = true;
    if (V_IsVehicleCurrentMovenet == 'True')
        N_IsRouteCurrentMovenet = false;
    startAnimation()

    if ($("#IsPrevMoveOpion").val() == 'true' || $("#IsPrevMoveOpion").val() == 'True') {
        $('#previousMovementImportBack').show();
        if (N_IsRouteCurrentMovenet) {
            $('#previousMovementImportBack').hide();//hide back button for imprting a route from prev movment
        }
    }
    Checker = Checker.replace(/ /g, '%20');
    $('#SelectCurrentMovementsVehicle2').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + VprojectId + '&hauliermnemonic=' + haulierMnemonic + '&esdalref=' + esdalRef + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&IsChooseCurrMovmOption=' + N_IsRouteCurrentMovenet + '&IsVehicleCurrentMovenet=' + V_IsVehicleCurrentMovenet, {},
        function () {
            $('#SelectCurrentMovementsVehicle2').show();
            stopAnimation()
        });
}
function ShowRouteVer() {
    showWarningPopDialog('Functionality is not implemented', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
}
function CandRoute_Click(e) {
    var routeid = $(e).attr("data-rid");
    if ($('#' + routeid).css("display") == "none") {
        $('#' + routeid).fadeIn("slow", function () {
        });
    }
    else {
        $('#' + routeid).fadeOut("slow", function () {
        });
    }
}
function CandVersion_Click(e) {
    var versionId = $(e).attr("data-vid");
    if ($('#' + versionId).css("display") == "none") {
        $('#' + versionId).fadeIn("slow", function () {
        });
    }
    else {
        $('#' + versionId).fadeOut("slow", function () {
        });
    }
}
function SortCreateCandidateRoute() {
    var projectid = $('#projectid').val();
    var revisionId = $('#revisionId').val();
    startAnimation();
    $.post('/SORTApplication/CheckVehicleForApplication?PROJ_ID=' + projectid + '&Rev_Id=' + revisionId, function (data) {
        if (data == true) {
            stopAnimation();
            $("#overlay").show();
            $('.loading').show();
            var options = { "backdrop": "static", keyboard: true };
            $.ajax({
                type: "GET",
                url: "../SORTApplication/CreateCandidateRoute",
                //contentType: "application/json; charset=utf-8",
                data: {},
                datatype: "json",
                success: function (data) {
                    $('#generalPopupContent').html(data);
                    $('#generalPopup').modal(options);
                    $('#generalPopup').modal('show');
                    $('.loading').hide();
                    CreateCandidateRouteInit();
                },
                error: function () {

                }
            });
            stopAnimation();
        }
        else {
            stopAnimation();
            ShowDialogWarningPop('Submit the edited application before proceeding', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
    });
}
function ShowCandidateRoutes(rev_Id, candname, candversionno, routeId, analysisId) {
    startAnimation();
    if (LastRevisionId == rev_Id)
        isLastVersion = true;
    else
        isLastVersion = false;
    var sowner = $('#Owner').val();
    sowner = sowner.replace(/ /g, '%20');
    window.location.href = '../SORTApplication/SORTListMovemnets' + EncodedQueryString('SORTStatus=CandidateRT&projecid=' + ProjectID + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionId=' + rev_Id + '&movementId=' + movementId + '&apprevid=' + ApprevId + '&revisionno=' + revisionno + '&OrganisationId=' + OrgID + '&versionno=' + versionno + '&versionId=' + versionId + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + false + '&pageflag=' + Pageflag + '&esdal_history=' + esdal_history + '&candName=' + candname + '&candVersionno=' + candversionno + '&CandRouteId=' + routeId + '&LatestRevisionId=' + ViewLastRevisionId + '&analysisId=' + analysisId + '&IsLastVersion=' + isLastVersion + '&EnterBySORT=' + Enter_BY_SORT + '&Owner=' + sowner);
    stopAnimation();
}
function EditCandiRouteName(CnRouteID) {
    removescroll();
    // $("#dialogue").html('');
    //$("#dialogue").load("../SORTApplication/SORTAppEditCandiRouteName?CnRouteID=" + CnRouteID);
    // $("#dialogue").show();
    $("#overlay").show();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: "../SORTApplication/SORTAppEditCandiRouteName",
        //contentType: "application/json; charset=utf-8",
        data: { CnRouteID: CnRouteID },
        datatype: "json",
        success: function (data) {
            $('#generalPopupContent').html(data);
            $('#generalPopup').modal(options);
            $('#generalPopup').modal('show');
            $('#edithead').html('Update Candidate Route Name');
            document.getElementById("edithead").style.color = "black";
            document.getElementById("edithead").style.userSelect = "none";
            $('.loading').hide();
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}