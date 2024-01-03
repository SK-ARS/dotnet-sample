var getValue = 0;
$(document).ready(function () {
    $('body').on('change', '#ddlroutes', function (e) {
        getValue = $(this).val();
        if (getValue != 0) {
            $('#trvalidcandrt').hide();
        }
    });
    $('body').on('click', '#txtcandidateroutename', function (e) {
        $('#trvalidcandname').css("display", "none");
    });
    $('body').on('click', '#btnValidate', function (e) {
        validate(this);
    });
    $('body').on('click', '#btnRemoveCandidateButton', function (e) {
        removeCandidateButton(this);
    });
});
function ClosePopUp() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    addscroll();
}
function validate() {
    var routeType = $('#ddlroutes option:selected').val();
    if (routeType == "0") {
        $("#trvalidcandrt").show();
        return false;
    }
    else {
        Createnewversion();
    }
}
function Createnewversion() {
    var lastrevisionno = parseInt($('#revisionno').val());
    var LastversionNo = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
    var lastcandrouteid = $('#LastCandRouteId').val();
    var lastcandrtreversionid = $('#LastCandRevisionId').val();
    var projectid = $('#projectid').val();
    var revisionId = $('#revisionId').val();
    var esdalref = hauliermnemonic + "/" + $('#esdalref').val() + "-" + revisionno;
    var esdalReferenceWorkflow = hauliermnemonic + "_" + $('#esdalref').val()
    var Msg = "";
    if (getValue == "application") {
        $.post('/SORTApplication/CheckVehicleForApplication?PROJ_ID=' + projectid + '&Rev_Id=' + revisionId, function (data) {
            if (data == true) {
                startAnimation();
                $.post('/SORTApplication/CreateCandidateVersion?CandRouteId=' + lastcandrouteid + '&CandRevisionId=' + lastcandrtreversionid + '&CloneType=' + getValue + '&AppRevId=' + ApprevId + '&EsdalRef=' + esdalref + '&LastRevisionNo=' + lastrevisionno + '&LastversionNo=' + LastversionNo, function (data) {
                    if (data != null) {
                        var result = JSON.parse(data);
                        if (result.newrivisionId != 0 && result.newversionNo != 0 && result.analysisid != 0) {
                            $('#candAnalysisId').val(result.analysisid);
                            $('#LastCandRevisionId').val(result.newrivisionId);
                            $('#LastCandVersion').val(result.newversionNo);
                            var candRevisionId = $('#LastCandRevisionId').val();
                            //Uncommented on 22-03-2023
                            CheckIsBroken({ CandRevisionId: candRevisionId }, function (brokenRouteList) {
                                CandidateVersionBrokenRouteReplan(brokenRouteList);
                            });
                        }
                        else {
                            stopAnimation();
                            Msg = 'The new version is not created.';
                            ShowErrorPopup(Msg,'WarningCancelBtn');
                        }
                    }
                });
            }
            else {
                stopAnimation();
                Msg = 'Submit the edited application before proceeding';
                ShowInfoPopup(Msg, 'WarningCancelBtn');
            }
        });
    }
    else {
        var dataModelPassed = { CandRouteId: lastcandrouteid, CandRevisionId: lastcandrtreversionid, CloneType: getValue, AppRevId: ApprevId, EsdalRef: esdalref, EsdalReferenceWorkflow: esdalReferenceWorkflow,LastRevisionNo:lastrevisionno,LastversionNo:LastversionNo };
        var result = SORTSORouting(6, dataModelPassed);
        if (result != undefined || result != "NOROUTE") {
            $.ajax({
                url: result.route, //"",
                type: 'POST',
                async: true,
                data: {
                    createCandidateVersionCntrlModel: result.dataJson
                },
                beforeSend: function () {

                },
                success: function (data) {
                    if (data != null) {
                        startAnimation();
                        var result = JSON.parse(data);
                        if (result.newrivisionId != 0 && result.newversionNo != 0 && result.analysisid != 0) {
                            $('#candAnalysisId').val(result.analysisid);
                            $('#LastCandRevisionId').val(result.newrivisionId);
                            $('#LastCandVersion').val(result.newversionNo);
                            var lastcandrtreversionid = $('#LastCandRevisionId').val();
                            
                            CheckIsBroken({ CandRevisionId: lastcandrtreversionid, IsReplanRequired: true }, function (response) {
                                CandidateVersionBrokenRouteReplan(response, true);
                            });
                        }
                        else {
                            stopAnimation();
                            Msg = 'The new version is not created.';
                            ShowErrorPopup(Msg, 'Redirect_ProjectOverview');
                        }
                    }
                },
                error: function () {
                }
            });
        }
    }
}
function WarningBrokenPopUp() {
    WarningCancelBtn();
    stopAnimation();
    var msg = "";
    var res = false;
    res = UpdateRouteAnalysis();
    if (getValue == "application") {
        msg = 'New candidate route version is successfully created from application routes';
        ShowSuccessModalPopup(msg, 'Redirect_ProjectOverview');
    }
    else if (getValue == "lastcandidate") {
        msg = 'New candidate route version is successfully created from last candidate route version';
        ShowSuccessModalPopup(msg, 'Redirect_ProjectOverview');
    }
}
function UpdateRouteAnalysis() {
    var rtRevision = $('#LastCandRevisionId').val();
    var res = false;
    $.ajax({
        url: '../SORTApplication/ClearRouteAssessment',
        type: 'POST',
        async: false,
        data: { RouteRevisionId: rtRevision },
        success: function (data) {
            res = data.result;
        }
    });
    return res;
}

function CandidateVersionBrokenRouteReplan(response, isFromCandidate) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        GetCandidateVersionMessageBrokenRoute(response, isFromCandidate);
    }
    else {
        stopAnimation();
        if (!isFromCandidate) {
            Msg = 'New candidate route version is successfully created from application routes';
            ShowSuccessModalPopup(Msg, 'Redirect_ProjectOverview');
        }
        else if (getValue == "lastcandidate") {
            Msg = 'New candidate route version is successfully created from last candidate route version';
            ShowSuccessModalPopup(Msg, 'Redirect_ProjectOverview');
        }
    }
}
function GetCandidateVersionMessageBrokenRoute(response,isFromCandidate) {
    var Msg = "";
    var LastCandVersion = $('#LastCandVersion').val();
    var revisionno = parseInt($('#revisionno').val());
    if (response.brokenRouteCount != 0) {
        if (response.specialManouer > 0) {//single route contains special manour
            Msg = !isFromCandidate ? BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_SPECIAL_MANOUER_IS_FROM_APPLICATION_ROUTES.replace("##*##", revisionno) : BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_SPECIAL_MANOUER_IS_FROM_CANDIDATE.replace("##*##", LastCandVersion) ;
        }
        else if (response.autoReplanFail > 0) { //if auto replan fails
            if (response.brokenRouteCount == 1) { //single broken route
                Msg = !isFromCandidate ? BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_SINGLE_BROKEN_ROUTE_IS_FROM_APPLICATION_ROUTES.replace("##*##", revisionno) : BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_SINGLE_BROKEN_ROUTE_IS_FROM_CANDIDATE.replace("##*##", LastCandVersion);
            }
            else if (response.brokenRouteCount > 1) { //multiple broken route
                Msg = !isFromCandidate ? BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_MULTIPLE_BROKEN_ROUTE_IS_FROM_APPLICATION_ROUTES.replace("##*##", revisionno) : BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_MULTIPLE_BROKEN_ROUTE_IS_FROM_CANDIDATE.replace("##*##", LastCandVersion);
            }
        }
        else if (response.autoReplanSuccess > 0) { // if all routes is replanned
            Msg = !isFromCandidate ? BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_ALL_ROUTES_IS_REPLANNED_IS_FROM_APPLICATION_ROUTES.replace("##*##", revisionno) : BROKEN_ROUTE_MESSAGES.CREATE_CANDIDATE_VERSION_ALL_ROUTES_IS_REPLANNED_IS_FROM_CANDIDATE.replace("##*##", LastCandVersion);
        }
        ShowWarningPopupMapupgarde(Msg, function () {
            $('#WarningPopup').modal('hide');
            stopAnimation();
            Redirect_ProjectOverview();
        });
    }
}
