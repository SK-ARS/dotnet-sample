function MovementTypeConfirmationInit() {
    StepFlag = 3;
    SubStepFlag = 0;
    CurrentStep = "Movement Type";
    if ($('#plan_movement_hdng').text() == '')
        $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(3);
    $('#save_btn').hide();
    $('#apply_btn').hide();
    $('#confirm_btn').removeClass('blur-button');
    $('#confirm_btn').attr('disabled', false);
    $('#confirm_btn').show();
    $('#backbutton').show();
    MovementAssessDetailsInit();
    hf_Vr1SoExistingPopUp = false;

    if (typeof showImminentMovementBanner != 'undefined') {
        showImminentMovementBanner($('#FromDate').val(), '#movement_type_confirmation ');
    }
}
$(document).ready(function () {
    $('body').on('click', '#ApplicationRadio', appln);
    $('body').on('click', '#NotificationRadio', notif);
});
function InsertMovementType() {
    var isSort = $('#IsSortUser').val();
    var allocateUserId = 0;
    var radioValue = $("input[name='applicationType']:checked").val();
    if (isSort == 'True') {
        allocateUserId = $('#dropSort').val();
    }
    var dataModelPassed = {
        movementId: $('#MovementId').val(),
        startApplicationProcess: true,
        haulierRefNo: $('#RefNo').val(),
        fromDate: $('#FromDate').val(),
        toDate: $('#ToDate').val(),
        fromSummary: $('#FromSummary').val(),
        toSummary: $('#ToSummary').val(),
        imminentMessage: $('#movement_type_confirmation #imminentBannerMsg').text(),
        applicationType: radioValue,
        allocateUserId: allocateUserId,
        startTime: "00:00:00",
        endTime: "00:00:00"
    };

    var result = ApplicationRouting(1, dataModelPassed);
    if (result != undefined || result != "NOROUTE") {
        $.ajax({
            url: result.route,
            type: 'POST',
            data: {
                insertMovementTypeCntrlModel: result.dataJson
            },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {

                $('#MvmtConfirmFlag').val(true);
                if (AmendFlag >= 1) {
                    $('#IsNotif').val(false);
                    $('#hf_IsNotif').val(false);
                    $('#hf_IsVr1App').val(false);
                    $('#hf_IsApp').val(false);
                    $('#hf_IsSortApp').val(false);
                    if (response.movementType == 207003) {
                        $('#NotificationId').val(response.data.NotificationId);
                        $('#NotifVersionId').val(response.data.VersionId);
                        $('#NotifProjectId').val(response.data.ProjectId);
                        $('#NotifAnalysisId').val(response.data.AnalysisId);
                        $('#CRNo').val(response.data.ContentRefNum);
                        $('#IsNotif').val(true);
                        $('#hf_IsNotif').val(true);
                        $('#apply_btn').text('SUBMIT NOTIFICATION');
                        $('#plan_movement_hdng').text("NOTIFICATION");
                        if ($('#IsSortUser').val() == 'True')
                            $('#step6 p').text('Route Assessment');
                        else
                            $('#step5 p').text('Route Assessment');
                        if ($('#IsNotifyApplication').val() == 1) {

                            LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: response.data.ContentRefNum, isNotif: $('#IsNotif').val(), workflowProcess: "HaulierApplication", IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section', '', function () {
                                MovementRouteInit();
                            });
                        }
                        else {
                            ReloadToRoutePage();
                        }
                    }
                    else {
                        $('#AppRevisionId').val(response.data.RevisionId);
                        $('#IsVR1').val(response.data.IsVr1);
                        $('#AppVersionId').val(response.data.VersionId);
                        $('#CRNo').val(response.data.ContentRefNum);
                        if ($('#IsVR1').val() == 'true') {
                            $('#apply_btn').text('APPLY FOR VR1');
                            $('#plan_movement_hdng').text("APPLY FOR VR1");
                            $('#hf_IsVr1App').val(true);
                        }
                        else {
                            $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
                            $('#plan_movement_hdng').text("APPLY FOR SPECIAL ORDER");
                            $('#hf_IsApp').val(true);
                        }
                        if ($('#IsSortUser').val() == 'True') {
                            $('#step6 p').text('Supplementary Information');
                            $('#hf_IsSortApp').val(true);
                        }
                        else
                            $('#step5 p').text('Supplementary Information');

                        ReloadToRoutePage();
                    }
                }
            },
            error: function (result) {
                ShowErrorPopup("Something went wrong");
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}
function UpdateMovementType(NavigateToNextPage,NavLink='') {
    var isSort = $('#IsSortUser').val();
    var radioValue = $("input[name='applicationType']:checked").val();
    var allocateUserId = 0;

    if (isSort == 'True') {
        allocateUserId = $('#dropSort').val();
    }

    var dataModelPassed = {
        movementId: $('#MovementId').val(),
        notificationId: $('#NotificationId').val(),
        appRevisionId: $('#AppRevisionId').val(),
        startApplicationProcess: true, // THIS FLAG WILL START A NEW HAULIER APPLICATION NOTIFICATION WORKFLOW
        haulierRefNo: $('#RefNo').val(),
        fromDate: $('#FromDate').val(),
        toDate: $('#ToDate').val(),
        fromSummary: $('#FromSummary').val(),
        toSummary: $('#ToSummary').val(),
        imminentMessage: $('#movement_type_confirmation #imminentBannerMsg').text(),
        applicationType: radioValue,
        allocateUserId: allocateUserId,
        startTime: $('#StartTime').val(),
        endTime: $('#EndTime').val()
    };
    $.ajax({
        type: "POST",
        url: "../Movements/UpdateMovementType",

        data: {
            updateMovementTypeCntrlModel: dataModelPassed
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#MvmtConfirmFlag').val(true);
            if (NavLink) {
                location.href = NavLink;
            }
            if (NavigateToNextPage) {
                if (AmendFlag >= 1) {

                    $('#IsNotif').val(false);
                    $('#hf_IsNotif').val(false);
                    $('#hf_IsVr1App').val(false);
                    $('#hf_IsApp').val(false);
                    $('#hf_IsSortApp').val(false);
                    $('#IsVR1').val(false);

                    if (response.movementType == 207003) {
                        $('#NotificationId').val(response.data.NotificationId);
                        $('#NotifVersionId').val(response.data.VersionId);
                        $('#NotifProjectId').val(response.data.ProjectId);
                        $('#NotifAnalysisId').val(response.data.AnalysisId);
                        $('#CRNo').val(response.data.ContentRefNum);
                        $('#IsNotif').val(true);
                        $('#hf_IsNotif').val(true);
                        $('#apply_btn').text('SUBMIT NOTIFICATION');
                        $('#plan_movement_hdng').text("NOTIFICATION");
                        $('#AppRevisionId').val(0);
                        $('#IsVR1').val(false);
                        $('#AppVersionId').val(0);
                        if ($('#IsSortUser').val() == 'True')
                            $('#step6 p').text('Route Assessment');
                        else
                            $('#step5 p').text('Route Assessment');

                        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", NotificationEditFlag: $('#NotificationEditFlag').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section', '', function () {
                            MovementRouteInit();
                        });
                    }
                    else {
                        $('#AppRevisionId').val(response.data.RevisionId);
                        $('#IsVR1').val(response.data.IsVr1);
                        $('#AppVersionId').val(response.data.VersionId);
                        $('#NotificationId').val(0);
                        $('#NotifVersionId').val(0);
                        $('#NotifProjectId').val(0);
                        $('#NotifAnalysisId').val(0);
                        $('#CRNo').val("");
                        $('#IsNotif').val(false);
                        if ($('#IsVR1').val() == 'true') {
                            $('#apply_btn').text('APPLY FOR VR1');
                            $('#plan_movement_hdng').text("APPLY FOR VR1");
                            $('#hf_IsVr1App').val(true);
                        }
                        else {
                            $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
                            $('#plan_movement_hdng').text("APPLY FOR SPECIAL ORDER");
                            $('#hf_IsApp').val(true);
                        }
                        if ($('#IsSortUser').val() == 'True') {
                            $('#step6 p').text('Supplementary Information');
                            $('#hf_IsSortApp').val(true);
                        }
                        else {
                            $('#step5 p').text('Supplementary Information');
                            $('#hf_IsSortApp').val(false);
                        }
                        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: response.data.RevisionId, versionId: response.data.VersionId, contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", NotificationEditFlag: $('#NotificationEditFlag').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section', '', function () {
                            MovementRouteInit();
                        });
                    }
                }
            }
        },
        error: function (result) {
            ShowErrorPopup("Something went wrong");
        },
        complete: function () {
            //stopAnimation();
        }
    });
}
function MovementTypeValidation() {
    var count = true;
    $('#dateValidate').html('');
    var ref = $('#RefNo').val();
    var len = ref.length;

    if (len > 35) {
        count = false;
        $('#spnHaulier_Reference').html('My reference should be 35 characters only');
    }
    else {
        $('#spnHaulier_Reference').html('');
    }

    if ($('#RefNo').val().trim() == "") {
        $('#spnHaulier_Reference').html('Reference number is required');
        count = false;
    }
    if ($('#FromSummary').val().trim() == "") {
        $('#lblFromSummary').html('From summary is required');
        count = false;
    }
    if ($('#ToSummary').val().trim() == "") {
        $('#lblToSummary').html('To summary is required');
        count = false;
    }

    if (!chkvalidMoveTypeConfirm()) {
        count = false;
    }
    return count;
}
function chkvalidMoveTypeConfirm() {
    var isValiddate;
    var fromDate = $('#FromDate').val();
    var toDate = $('#ToDate').val();
    var result = ValidateMomentDateTime(fromDate, toDate, false);
    if (result == 1) {
        $('#spnFromDate').html('Date must be today\'s date or greater than today\'s date.');
        $('#spnToDate').html('');
        isValiddate = false;
    }
    else if (result == 2) {
        $('#spnToDate').html('To date must be greater than from date.');
        $('#spnFromDate').html('');
        isValiddate = false;
    }
    else {
        $('#spnFromDate').html('');
        $('#spnToDate').html('');
        isValiddate = true;
    }
    return isValiddate;
}
function ReloadToRoutePage() {
    var data;
    if ($('#NotificationId').val() > 0) {
        data = { contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), workflowProcess: "HaulierApplication", IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() };
    }
    else {
        data = { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), workflowProcess: "HaulierApplication", isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() };
    }
    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', data, '#select_route_section', '', function () {
        MovementRouteInit();
        showToastMessage({
            message: "Movement is now saved as WIP.",
            type: "success"
        });
    })
}
function AllocatePOP(_this,valueObj) {
    startAnimation();
    $('#AllocateUser').load('../SORTApplication/SORTAllocateUser', {},
        function () {
            $("#overlay").hide();
            $('#AllocateUser').find('#table-head').remove();
            $('#AllocateUser').find('.pb-3').remove();
            $('#AllocateUser').show();
            if (valueObj) {
                $('#dropSort').val(valueObj);
            }
            stopAnimation();
        });
}
function appln() {
    $('.effe').hide();
    $('.notifassess').show();
    $('.notifassessin').hide();
}
function notif() {
    $('.effe').show();
    $('.eff').show();
}