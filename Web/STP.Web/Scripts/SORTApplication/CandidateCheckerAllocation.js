var getValue = 0;
var date = 0;
var checkername = "";
$(document).ready(function () {
    $('body').on('click', '#checkerusers', function (e) {
        getValue = $(this).val();
        checkername = $("#checkerusers option:selected").text();
        if (getValue != 0)
            $('#validsender').text('');
    });
    $('body').on('click', '#btnCandidateCheckerSendingClick', CandidateCheckerSending_Click);
    $('body').on('click', '#spanCloseGeneralPopup, #btnCloseGeneralPopup', CloseGeneralPopup);
});
function CandidateCheckerAllocationInit() {
    $('.dyntitleConfig').html('' + hauliermnemonic + '/' + esdalref + ' - Send for checking');
    Resize_PopUp(460);
}
function ClosePopUp() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    addscroll();
    resetdialogue();
}
function CandidateCheckerSending_Click() {
    
    if (getValue != 0) {
        
        startAnimation();
        var lastrouteid = $('#LastCandRouteId').val();
        var lastrevisionid = $('#LastCandRevisionId').val();
        var candversionno = $('#LastCandVersion').val();
        var isvr1app = $('#VR1Applciation').val();
        var revisionno = $('#revisionno').val();
        var lastversionno = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
        var cstatus = 0;
        if ($('#hf_AllocationType').val() == "checking") {
            cstatus = 301002;
        }
        if ($('#hf_AllocationType').val() == "signoff") {
            cstatus = 301005;
        }
        if ($('#hf_AllocationType').val() == "QAChecking") {
            cstatus = 301008;
        }
        var appref = hauliermnemonic + "/" + esdalref;
        if (isvr1app == "False" || isvr1app == "false") {
            var _pstatus = $('#AppStatusCode').val();
            var vr1Application = false;
            if ($('#VR1Applciation').val() == "True") {
                vr1Application = true;
            }
            var dataModelPassed = { Projectid: projectid, userId: getValue, Status: cstatus, CandRouteId: lastrouteid, CandRevisionId: lastrevisionid, CandVersiono: candversionno, CheckerName: checkername, AppRef: appref, PStatus: _pstatus, MovVerNo: versionno, isVr1Application: vr1Application, isWorkflow: true, lastrevisionno: revisionno, lastversionno: lastversionno };

            var result = SORTSORouting(3, dataModelPassed);
            if (result == null || result != "NOROUTE") {
                $.ajax({
                    url: result.route, /*Expected route :  /SORTApplication/CheckerUpdation*/
                    type: 'POST',
                    data: {
                        checkerUpdationCntrlModel: result.dataJson
                    },
                    dataType: "json",
                    beforeSend: function () {

                    },
                    success: function (result) {
                        
                        var value = JSON.parse(result);
                        CloseGeneralPopup();
                        if (value.result == true) {
                            $('#CheckerId').val(getValue);
                            $('#CheckerStatus').val(cstatus);
                            if (cstatus == 301002)
                                ShowSuccessModalPopup('Checker allocation is successfully completed', 'Redirect_ProjectOverview');
                            else if (cstatus == 301005)
                                ShowSuccessModalPopup('Final checker allocation is successfully completed', 'Redirect_ProjectOverview');
                            else if (cstatus == 301008)
                                ShowSuccessModalPopup('QA checker allocation is successfully completed', 'Redirect_ProjectOverview');
                        }
                        else
                            ShowDialogWarningPop('The checking process is not completed., ', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

                        if (value.analysisid != 0) {
                            $('#candAnalysisId').val(value.analysisid);
                        }
                        stopAnimation();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        CloseGeneralPopup();
                    }
                });
            }
        }
        else if (isvr1app == "true" || isvr1app == "True") {
            var vr1Application = true;
            var dataModelPassed = { Projectid: projectid, userId: getValue, Status: cstatus, CheckerName: checkername, AppRef: appref, MovVerNo: versionno, isVr1Application: vr1Application, isWorkflow: true, lastrevisionno: revisionno, lastversionno: lastversionno };

            var result = SORTVR1Routing(1, dataModelPassed);
            if (result == null || result != "NOROUTE") {
                $.ajax({
                    url: result.route, /*Expected route : /SORTApplication/VR1CheckerUpdation*/
                    type: 'POST',
                    data: {
                        allocateVr1CheckerUserCntrlModel: result.dataJson
                    },
                    dataType: "json",
                    beforeSend: function () {

                    },
                    success: function (result) {
                        
                        CloseGeneralPopup();
                        var value = JSON.parse(result);
                        if (value.result == true) {
                            $('#CheckerId').val(getValue);
                            $('#CheckerStatus').val(cstatus);
                            if (cstatus == 301002)
                                ShowSuccessModalPopup('Checker allocation is successfully completed', 'Redirect_ProjectOverview');
                            else if (cstatus == 301005)
                                ShowSuccessModalPopup('Final checker allocation is successfully completed', 'Redirect_ProjectOverview');
                        }
                        else
                            ShowSuccessModalPopup('The checking processes is not completed., ', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

                        stopAnimation();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        CloseGeneralPopup();
                    }
                });
            }
        }
    }
    else {
        $("#validsender").show();
        $('#validsender').text('Checker is required');
    }
}
