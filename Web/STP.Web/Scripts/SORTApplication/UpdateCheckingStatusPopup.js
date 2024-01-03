var getValue = 0;
var date = 0;
var checkerusers = "";
var workstatus = 0;
var checkername = "";
var atype;
function UpdateCheckingStatusPopupInit() {
    $('.dyntitleConfig').html('' + hauliermnemonic + '/' + esdalref + ' - Complete checking');
    if ($('#IsBrokenCSCheck').val() == 1)
        document.getElementById("cspositive").disabled = true;
    Resize_PopUp(480);
    atype = $('#txtallocationtype').val();
    checkername = $('#Owner').val();
    if (atype == 'checking') {
        $('#cspositive').val(301003);
        $('#csnegative').val(301004);
        $('#spancspos').html('Positive');
        $('#spancsneg').html('Negative');
        getValue = $('#PlannrUserId').val();
        $("#checkerusers").val(getValue);
    }
    else if (atype == 'signoff') {
        $('#cspositive').val(301006);
        $('#csnegative').val(301007);
        $('#spancspos').html('Agreed');
        $('#spancsneg').html('Disagreed');
    }
    else if (atype == 'QAChecking') {
        $('#cspositive').val(301009);
        $('#csnegative').val(301010);
        getValue = $('#PlannrUserId').val();
        $("#checkerusers").val(getValue);
    }
}
$(document).ready(function () {
    $('body').on('click', '#btn-candidate', function (e) {
        CandidateCheckerCSendingok_Click();
    });
    $('body').on('click', '.update-check-stat-close-popup,#btn-close', function (e) {
        ClosePopUpUpdateCheckingStatus();
    });
    $('body').on('change', '#checkerusers', function (e) {
        getValue = $(this).val();
        checkername = $("#checkerusers option:selected").text();
        if (getValue != 0)
            $('#validcehckerusers').text('');
    });
    $('body').on('change', '.rbtnstatus', function (e) {
        workstatus = $(this).val();
        $('#validrbtncehckerusers').text('');
    });
});
function ClosePopUpUpdateCheckingStatus() {
   
    CloseGeneralPopup();
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    $('#IsBrokenCSCheck').val(0);
    $('#generalPopupContent').html('');
    addscroll();
    resetdialogue();
}
function CandidateCheckerCSendingok_Click() {
    $('#IsBrokenCSCheck').val(0);
    var atype = $('#txtallocationtype').val();
    var lastrouteid = $('#LastCandRouteId').val();
    var lastrevisionid = $('#LastCandRevisionId').val();
   // checkername = checkername.replace(/ /g, '%20');
    var appref = hauliermnemonic + "/" + esdalref;
    analysis_id = $('#candAnalysisId').val();
    var isvr1app = $('#VR1Applciation').val();
    var lastrevisionno = $('#revisionno').val();
    var lastversionno = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val():0;
    if (workstatus != 0 && (getValue != 0 || atype == 'signoff')) {
        startAnimation();
        if (isvr1app == "False" || isvr1app == "false") {
            var ver_no = $('#versionno').val();
            var _pstatus = $('#AppStatusCode').val();
            var dataModelPassed = { Projectid: projectid, userId: getValue, Status: workstatus, CandRouteId: lastrouteid, CandRevisionId: lastrevisionid, CheckerName: checkername, AnalysisId: analysis_id, AppRef: appref, PStatus: _pstatus, MovVerNo: versionno, isVr1Application: false, isWorkflow: true, lastrevisionno: lastrevisionno,lastversionno: lastversionno };
            var result = SORTSORouting(8, dataModelPassed);
            if (result != undefined || result != "NOROUTE") {
                $.ajax({
                    url: result.route, //"/SORTApplication/CheckerUpdation",
                    type: 'POST',
                    async: true,
                    data: {
                        checkerUpdationCntrlModel: result.dataJson
                    },
                    beforeSend: function () {

                    },
                    success: function (data) {
                        ClosePopUpUpdateCheckingStatus();
                        var value = JSON.parse(data);
                        stopAnimation();
                        if (value.result == true) {
                            $('#PlannrUserId').val(getValue);
                            $('#Owner').val(checkername);
                            $('#CheckerId').val("");
                            $('#CheckerStatus').val(workstatus);
                            
                            $('#RoutePart').hide();
                            var displaymessage = 'Successfully updated the checking status as';
                            if (workstatus == 301003)
                                displaymessage += '"checked +"';
                            else if (workstatus == 301004)
                                displaymessage += '"checked -"';
                            else if (workstatus == 301006)
                                displaymessage += '"final checked +"';
                            else if (workstatus == 301007)
                                displaymessage += '"final checked -"';
                            else if (workstatus == 301009)
                                displaymessage += '"QA checked +"';
                            else if (workstatus == 301010)
                                displaymessage += '"QA checked -"';
                            
                            ShowSuccessModalPopup(displaymessage, 'Redirect_ProjectOverview');
                        }
                        else if (workstatus == 301006) {
                            if (value.RouteAnalysis == false) {
                                var displaymessage = 'Please generate affected parties before agree from last candidate route version!';
                                ShowInfoPopup(displaymessage);
                            }
                        }
                        if (value.analysisid != 0) {
                            $('#candAnalysisId').val(value.analysisid);
                        }
                    },
                    error: function (data) {
                        stopAnimation();
                        ShowInfoPopup(data);
                    }
                });
            }
        }
        else if (isvr1app == "true" || isvr1app == "True") {
            var dataModelPassed = { Projectid: projectid, userId: getValue, Status: workstatus, OwnerName: checkername, AppRef: appref, MovVerNo: versionno, isVr1Application: true, isWorkflow: true, lastrevisionno: lastrevisionno, lastversionno: lastversionno };
            var result = SORTVR1Routing(1, dataModelPassed);
            if (result == null || result != "NOROUTE") {
                $.ajax({
                    url: result.route, //"/SORTApplication/VR1CheckerUpdation",
                    type: 'POST',
                    async: true,
                    data: {
                        allocateVr1CheckerUserCntrlModel: result.dataJson
                    },
                    beforeSend: function () {

                    },
                    success: function (data) {
                        ClosePopUpUpdateCheckingStatus();
                        var value = JSON.parse(data);
                        stopAnimation();
                        if (value.result == true) {
                            $('#CheckerId').val("");
                            $('#PlannrUserId').val(getValue);
                            $('#Owner').val(checkername);
                            $('#CheckerStatus').val(workstatus);
                            var displaymessage = 'Successfully updated the checking status as';
                            if (workstatus == 301003)
                                displaymessage += '"checked +"';
                            else if (workstatus == 301004)
                                displaymessage += '"checked -"';
                            else if (workstatus == 301006)
                                displaymessage += '"final checked +"';
                            else if (workstatus == 301007)
                                displaymessage += '"final checked -"';

                            //showWarningPopDialog(displaymessage, 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');
                            ShowSuccessModalPopup(displaymessage, 'Redirect_ProjectOverview');
                        }
                        else
                            //showWarningPopDialog('The updation of checking process is not completed., ', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                            ShowInfoPopup('The updation of checking process is not completed');
                    },
                    error: function (data) {
                        stopAnimation();
                        ShowInfoPopup(data);
                    }
                });
            }
        }
    }
    else {
        if (getValue == 0) {
            $('#validcehckerusers').text('User is required');
        }
        if (workstatus == 0) {
            $('#validrbtncehckerusers').text('Checking outcome is required');
            $('#validrbtncehckerusers').show();
        }
    }
}