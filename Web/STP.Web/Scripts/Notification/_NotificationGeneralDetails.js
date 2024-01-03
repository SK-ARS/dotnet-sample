var onchangeflag = false;
var VehicleClassVal;
var ConstructionAndUseVal;
var imminentmsg = "";
var isValid = true;

function NotificationGeneralDetailsInit() {
    StepFlag = 6;
    SubStepFlag = 0;
    localStorage.removeItem('isMovementTypeHasMajorChanges');
    VehicleClassVal = $('#hf_VehicleClass').val();
    ConstructionAndUseVal = $('#hf_ConstructionAndUse').val();
    CurrentStep = "Notification Overview";
    //$('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);
    $('#VSOType').val($('#VSO_Type').val());
    $('#VehicleClass').val(VehicleClassVal);
    SetWorkflowProgress(6);
    $('#back_btn').hide();
    $('#save_btn').hide();
    $('#apply_btn').hide();
    $('#confirm_btn').hide();
    if (ConstructionAndUseVal == "True" || ConstructionAndUseVal == "true") {
        $('#actingOnBehalf').show();
    }
    if ($('#OverviewInfoSaveFlag').val() == 'True' || $('#OverviewInfoSaveFlag').val() == 'true') {
        $('#cancel_overview_info_btn_cntr').show();
    }
    var fromDateExisting = $("#FromDateTime").val();//From Movement Type Page
    var toDateExisting = $("#ToDateTime").val();//From Movement Type Page
    $("#FromDateTime").datepicker({
        dateFormat: "dd/mm/yy",
        changeYear: true,
        changeMonth: true,
        minDate: new Date(),
        onSelect: function (selected) {
            var startDate = selected.split("/").reverse().join("-");
            var fromDate = new Date(startDate);
            var toDate = $("#ToDateTime").datepicker('getDate');
            if (fromDate > toDate) {
                $("#ToDateTime").datepicker("setDate", fromDate);
            }
            var toDateSelected = $("#ToDateTime").val();
            if (selected != fromDateExisting || toDateSelected != toDateExisting) {
                localStorage.setItem('isMovementTypeHasMajorChanges', 2);
                showToastMessage({
                    message: "There are changes in dates. Please re-generate the route assessment.",
                    type: "error"
                });
            } else {
                localStorage.removeItem('isMovementTypeHasMajorChanges');
            }
            if (typeof showImminentMovementBanner != 'undefined') {
                showImminentMovementBanner(selected, '#overview_info_section ');
            }

        },
        beforeShow: function (textbox, instance) {
            var rect = textbox.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            }, 0);
            $("body").addClass('date-picker-pos-cl');
        },
        onClose: function () {
             setTimeout(function () {
                    $("body").removeClass('date-picker-pos-cl');
             }, 500)
        }
    });
    
    $("#ToDateTime").datepicker({
        dateFormat: "dd/mm/yy",
        changeYear: true,
        changeMonth: true,
        minDate: 0,
        onSelect: function (selected) {
            var fromDate = $("#FromDateTime").val();
            if (fromDate != fromDateExisting || selected != toDateExisting) {
                localStorage.setItem('isMovementTypeHasMajorChanges', 2);
                showToastMessage({
                    message: "There are changes in dates. Please re-generate the route assessment.",
                    type: "error"
                });
            } else {
                localStorage.removeItem('isMovementTypeHasMajorChanges');
            }

        },
        beforeShow: function (textbox, instance) {
            var startDate = $("#FromDateTime").datepicker('getDate');
            var rect = textbox.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            }, 0);
            $("#ToDateTime").datepicker("option", "minDate", startDate);
            $("body").addClass('date-picker-pos-cl');
        },
        onClose: function () {
            setTimeout(function () {
                $("body").removeClass('date-picker-pos-cl');
            }, 500)
        }
    });
    ApplicationRouteListInit();
    //GetLocalObject(StepFlag);
    $('#TimeFrom').timepicker({ 'scrollDefault': 'now', 'timeFormat': 'H:i' });
    $('#TimeTo').timepicker({ 'scrollDefault': 'now', 'timeFormat': 'H:i' });
   
    stopAnimation();
    if (typeof showImminentMovementBanner != 'undefined') {
        showImminentMovementBanner($('#FromDateTime').val(), '#overview_info_section ');
    }
};
$(document).ready(function () {
    $('body').on('click', '#btn_view_indemnification', function (e) {
        e.preventDefault();
        ViewIndemnityConfirmation(this);
    });
    $('body').on('change', '.onchangefunctionDN', function (e) {
        e.preventDefault();
        onchangeflag = true;
    });
    $('body').on('change', '#idemnity', function (e) {
        e.preventDefault();
        showIndemnity(this);
    });
    $('body').on('click', '.notif-term-condtn', function (e) {
        ViewTermsAndConditions();
    });
    $('body').on('click', '#back_btns', function (e) {
        e.preventDefault();
        PlanMoveNavigateFlow(StepFlag, true);
    });
    $('body').on('click', '#apply_btn1', function (e) {
        var isMajorChangesExist = localStorage.getItem('isMovementTypeHasMajorChanges');
        if (isMajorChangesExist == "2") {
            //If there are changes in date, user should be notified to re run route assesment
            showToastMessage({
                message: "There are changes in dates. Please re-generate the route assessment.",
                type: "error"
            });
        } else if (BackAndForthNavMethods.IsNotification() && typeof isRouteAssessmentRequiredForWIP != 'undefined' && isRouteAssessmentRequiredForWIP == true) {
            var fromDateOverview = $('#FromDateTime').val();
            var toDateOverview = $('#ToDateTime').val();
            var timeFromOverview = $('#TimeFrom').val();
            var timeToOverview = $('#TimeTo').val();
            var isValid = true;
            if (fromDateOverview != "" && toDateOverview != "" && timeFromOverview != "" && timeToOverview != "") {
                var result = ValidateMomentDateTime(fromDateOverview + " " + timeFromOverview, toDateOverview + " " + timeToOverview, true, "DD/MM/YYYY HH:mm");
                if (result == 1) {
                    $('#overview_info_section').find('#spnFromDate').html('From date & time must be equal to or greater than today\'s date & time.');
                    $('#overview_info_section').find('#spnToDate').html('');
                    isValid = false;
                }
                else if (result == 2) {
                    $('#overview_info_section').find('#spnToDate').html('To date & time must be equal to or greater than today\'s date & time and movement start date & time.');
                    $('#overview_info_section').find('#spnFromDate').html('');
                    isValid = false;
                }
                else {
                    $('#overview_info_section').find('#spnFromDate').html('');
                    $('#overview_info_section').find('#spnToDate').html('');
                }
            }
            if (isValid) {
                showToastMessage({
                    message: "Please re-generate the route assessment.",
                    type: "error"
                });
            } else {
                $('html, body').animate({
                    scrollTop: $("#FromDateTime").offset().top
                }, 2000);
            }
        } else {
            onApply();
        }
    });
    $('body').on('keyup', '#FromSummary', function (e) {
        e.preventDefault();
        $('#spnFromSummary').html('');
    });
    $('body').on('keyup', '#ToSummary', function (e) {
        e.preventDefault();
        $('#spnToSummary').html('');
    });
    $('body').on('keyup', '#LoadDescription', function (e) {
        e.preventDefault();
        $('#spnLoadDescription').html('');
    });
    $('body').on('keyup', '#Notes', function (e) {
        e.preventDefault();
        $('#spnNotes').html('');
    });
    $('body').on('change', '#TimeTo', function (e) {
        e.preventDefault();
        $('#spnToTime').html('');
    });
    $('body').on('change', '#TimeFrom', function (e) {
        e.preventDefault();
        $('#spnFromTime').html('');
    });
    $('body').on('keyup', '#ToDateTime', function (e) {
        e.preventDefault();
        $('#spnToDate').html('');
    });
});
function onchangefunctionDN() {
    onchangeflag = true;
}
function ValidateGeneralDetails(IsCompleted = true, NavigateToLink = '') {
    CheckValidationDN(IsCompleted,NavigateToLink);
}
function CheckIsNotificationSubmited(NotifId) {
    var res = 0;
    $.ajax({
        url: '../Notification/IsNotifSubmitCheck',
        type: 'POST',
        async: false,
        data: { NotificationID: NotifId },
        success: function (result) {
            res = result;
        }
    });
    return res;
}
function CheckValidationDN(IsCompleted = true, NavigateToLink = '') {
    $('.validation-summary-errors ul li').remove();
    if (IsCompleted) {
        var count = 0;
        var reClass = /(^|\s)required(\s|$)/;  // Field is required
        var reValue = /^\s*$/;
        var ele = $("#formnsd").find("input.required:text,textarea");
        var el;

        for (var i = 0, iLen = ele.length; i < iLen; i++) {
            el = ele[i];
            if (reClass.test(el.className)) {
                $('#overview_info_section').find('#spn' + el.name).html('');
            }
        }

        $('#overview_info_section').find('#spnIndemnifyFlag').html('');
        $('#overview_info_section').find('#spnMyReference').html('');           

        for (var i = 0, iLen = ele.length; i < iLen; i++) {
            el = ele[i];

            if (reClass.test(el.className) && reValue.test(el.value)) {
                // Required field has no value or only whitespace
                // Advise user to fix
                if (el.name == 'MyReference') {
                    $('#spn' + el.name).html('Reference number is required');
                }
                else if (el.name == 'VSONumber') {
                    $('#spn' + el.name).html('Vehicle special order no. is required');
                }
                else if (el.name == 'FromSummary') {
                    $('#overview_info_section').find('#spn' + el.name).html('From summary is required');
                }
                else if (el.name == 'ToSummary') {
                    $('#overview_info_section').find('#spn' + el.name).html('To summary is required');
                }
                else if (el.name == 'LoadDescription') {
                    $('#overview_info_section').find('#spn' + el.name).html('Load description summary is required');
                }
                else if (el.name == 'HaulierOprLicence') {
                    $('#overview_info_section').find('#spn' + el.name).html('Haulier operator licence is required');
                }
                else if (el.name == 'Notes') {
                    $('#overview_info_section').find('#spn' + el.name).html('Haulier notes is required');
                }
                count = count + 1;
            }
        }
        //check for my reference
        var ref = $('#MyReference').val();
        var len = ref.length;
        if (len > 35) {
            count = count + 1;
            $('#overview_info_section').find('#spnMyReference').html('My reference should be 35 characters only');
        }

        var numMvmnt = $('#NoOfMoves').val();
        var numPieces = $('#MaxPieces').val();

        if (numMvmnt == "") {

            $('#overview_info_section').find('#spnNoOfMoves').html('No of movements is required');
            $('.validation-summary-errors ul').append('<li>No of movements is required</li>');
            $('.validation-summary-errors').show();
        }
        if (numPieces == "") {
            $('#overview_info_section').find('#spnMaxPieces').html('Max pieces movement is required');
            $('.validation-summary-errors ul').append('<li>Max pieces movement is required</li>');
            $('.validation-summary-errors').show();
        }
            
        //check for my indemnity
        if ($('#IsIndemnityNeeded').val() == 'true' || $('#IsIndemnityNeeded').val() == 'True') {
            if (!($("#idemnity").is(':checked')) == true) {
                count = count + 1;
                $('#overview_info_section').find('#spnIndemnifyFlag').html('Indemnity is required');
            }
            else {
                $('#overview_info_section').find('#spnIndemnifyFlag').html('');
            }
        }
        else {
            $('#overview_info_section').find('#spnIndemnifyFlag').html('');
        }

        if ($('#TimeFrom').val() == '') {
            count = count + 1;
            $('#overview_info_section').find('#spnFromTime').html('Time From is required');
        }
        if ($('#TimeTo').val() == '') {
            count = count + 1;
            $('#overview_info_section').find('#spnToTime').html('Time To is required');
        }
        var fromDateOverview = $('#FromDateTime').val();
        var toDateOverview = $('#ToDateTime').val();
        var timeFromOverview = $('#TimeFrom').val();
        var timeToOverview = $('#TimeTo').val();
        if (fromDateOverview != "" && toDateOverview != "" && timeFromOverview != "" && timeToOverview != "") {
            var result = ValidateMomentDateTime(fromDateOverview + " " + timeFromOverview, toDateOverview + " " + timeToOverview, true, "DD/MM/YYYY HH:mm");
            if (result == 1) {
                $('#overview_info_section').find('#spnFromDate').html('From date & time must be equal to or greater than today\'s date & time.');
                $('#overview_info_section').find('#spnToDate').html('');
                count = count + 1;
            }
            else if (result == 2) {
                $('#overview_info_section').find('#spnToDate').html('To date & time must be equal to or greater than today\'s date & time and movement start date & time.');
                $('#overview_info_section').find('#spnFromDate').html('');
                count = count + 1;
            }
            else if (result == 3) {
                count = count + 1;
                $('#overview_info_section').find('#spnToDate').html('Movement Start Date/Time must be before Movement End Date/Time');
            }
            else {
                $('#overview_info_section').find('#spnFromDate').html('');
                $('#overview_info_section').find('#spnToDate').html('');
            }
        }

        if (count > 0) {
            $('.error').each(function () {
                var _error = $(this).text();
                if (_error != '') {
                    $('.validation-summary-errors ul').append('<li>' + _error + '</li>');
                    $('.validation-summary-errors').show();

                    $(this).closest('td').find('input:text').focus();
                    //return false;
                }
            });

            $('html, body').animate({
                scrollTop: $("#formnsd").offset().top
            }, 2000);
        }

        if (count == 0) {
            showWarningImminentMovement(IsCompleted);
        }
    } else {
        //no validation for temp store
        showWarningImminentMovement(IsCompleted, NavigateToLink);
    }
}
function showWarningImminentMovement(IsCompleted = true, NavigateToLink = '') {
    var vehicleclass = $('#VehicleClass').val();
    var NotifID = $('#NotificationId').val();
    ContentRefno = $('#CRNo').val();
    var moveStartDate = $('#FromDateTime').val();
    var imminentMsg = '';
    if (vehicleclass != "241001" && IsCompleted == true) {
        $.ajax
            ({
                type: "POST",
                url: "../Notification/ShowImminentMovement",
                data: { moveStartDate: moveStartDate, contentRefNo: ContentRefno, notificationId: NotifID, vehicleClass: vehicleclass },
                beforeSend: function () {
                    startAnimation('Checking whether imminent movement');
                },
                success: function (data) {
                    var ids = data.result.strContryID;//country containing the route
                    //1 - Imminent movement.
                    //2 - Imminent movement for police.
                    //3 - Imminent movement for SOA.
                    //4 - Imminent movement for SOA and police.
                    //5 - No imminent movement.

                    if (data.result.imminentStatus == 5) {
                        $("#ImminentFlag").val("No imminent movement");
                        $('#ShowWanringImiinent').css("display", "none");
                        $('#warningImminent').html(' ');

                        SubmitNotification();
                    }
                    else {
                        if (data.result.imminentStatus == 1) { imminentMsg = "Imminent movement." }
                        else if (data.result.imminentStatus == 2) { imminentMsg = "Imminent movement for police." }
                        else if (data.result.imminentStatus == 3) { imminentMsg = "Imminent movement for SOA." }
                        else if (data.result.imminentStatus == 4) { imminentMsg = "Imminent movement for SOA and police." }

                        $("#ImminentFlag").val(imminentMsg + "," + ids);
                        $('#ShowWanringImiinent').css("display", "block");
                        $('#warningImminent').html(imminentMsg);
						stopAnimation();

                        var msg = "The movement from date is outside the statutory window (you are not notifying on time). Are you sure you want to continue with these movement dates? If Yes, you are required to telephone affected parties to obtain consent. Select Yes to continue with the current movement dates, select No to modify the movement dates.";
                        ShowWarningPopup(msg, 'SubmitNotification');
                    }
                },
                error: function (xhr, status, error) {
                    alert("error");
                },
                complete: function () {
                    //stopAnimation();
                }
            });
    }
    else {
        $("#ImminentFlag").val("No imminent movement");
        $('#ShowWanringImiinent').css("display", "none");
        $('#warningImminent').html(' ');
        
        if (IsCompleted == true) {
            var input=CheckBrokenRouteExistBeforeSubmit();
            CheckIsBroken(input, function (response) {
                if (response && response.brokenRouteCount > 0) {//Check broken route exist and show message
                    ShowWarningPopupMapupgarde(BROKEN_ROUTE_MESSAGES.PLAN_MOV_ON_MAP_PAGE_CONFIRM, function () {
                        $('#WarningPopup').modal('hide');
                    });
                    return false;
                } else {
                    SubmitNotification(IsCompleted, NavigateToLink);
                }
            });
        } else {
            SubmitNotification(IsCompleted, NavigateToLink);
        }

    }
}
function SubmitNotification(IsCompleted = true, NavigateToLink = '') {
    var backFlag = false;
    if (IsCompleted == false) {
        backFlag = true;
    }
    RemoveLocalObject(NavigationEnum.MOVEMENT_TYPE);
    RemoveLocalObject(NavigationEnum.OVERVIEW);
    CloseWarningPopup();
    var start = $("#tempFrom").val();
    var end = $("#tempTo").val();
    if (start == " " && end == " ") {
        start = $('#StartRenCln').val();
        end = $('#ToRenCln').val();
    }
    var routeObj;
    if (objifxStpMap != undefined) {
        var routeObj = getRouteDetails();
        if (routeObj != null && routeObj != undefined && routeObj.routePathList[0].routePointList.length > 1) {
            $('#FromAddress').val(routeObj.routePathList[0].routePointList[0].pointDescr);
            $('#ToAddress').val(routeObj.routePathList[0].routePointList[1].pointDescr);
        }
    }
    else {
        if (start != " " && end != " ") {
            $('#FromAddress').val(start);
            $('#ToAddress').val(end);
        }
    }

    var vehicleclass = $('#VehicleClass').val();
    var notifId = $('#NotificationId').val();
    ContentRefno = $('#ContentRefNo').val();
    //forminfod = document.getElementById("formnsd").getElementsByTagName("input");
    forminfod = $("#formnsd").find("input,textarea");
    $('#TextChangeFlagDN').val(false);
    var inputData = $(forminfod).serialize();
    inputData += "&imminentMessage=" + $('#overview_info_section #imminentBannerMsg').text();
    $.ajax({
        type: "POST",
        url: "../Notification/SaveNotificationGeneralDetails?notificationId=" + notifId +"&backflag="+ backFlag,
        data: inputData,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (IsCompleted == true) {
                if (data.result) {
                    $('#OverviewInfoSaveFlag').val(true);
                    GenerateNotification();
                }
                else {
                    var msg1 = 'Notification not saved. Please fill mandatory fields';
                    ShowErrorPopup(msg1);
                }
            } else if (NavigateToLink != '' && NavigateToLink != null) {
                location.href = NavigateToLink;
            }

        },
        error: function (xhr, status, error) {
            alert("error");
        },
        complete: function () {
        }
    });
}
function IsApplyForNotification() {
    let Msg = "Do you want to submit notification?";
    if ($('#MyReference').val() != '')
        Msg = "Do you want to submit notification \"" + $('#MyReference').val() + '\" ?';
    ShowWarningPopup(Msg, "SubmitNotification");
}
function GenerateNotification() {
    CloseWarningPopup();
    $.ajax({
        url: '../Notification/GenerateNotification',
        type: 'POST',
        datatype: 'json',
        data: { NotificationId: $('#NotificationId').val(), GenerateCode: 1, ImminentMovestatus: $('#ImminentFlag').val(), ProjectId: $('#NotifProjectId').val(), workflowProcess: 'HaulierApplication' },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            stopAnimation();
            isWorkFlowCompleted = true;
            var Msg = 'Notification submitted successfully. The ESDAL reference is ' + "\"" + data.EsdalRefNo + "\"";
            var param1 = $('#NotificationId').val();
            var param2 = data.EsdalRefNo;
            ShowSuccessModalPopup(Msg, "RedirectToNotificationOverview", param1, param2);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function showIndemnity() {
    let checkBox = document.getElementById('idemnity');
    if (checkBox.checked) {
        document.getElementById('actingOnBehalf').style.display = "block";
        document.getElementById('show-idemnity').style.display = "block";
        $('#overview_info_section').find('#spnIndemnifyFlag').html('');
    }
    else {
        document.getElementById('actingOnBehalf').style.display = "none";
        document.getElementById('show-idemnity').style.display = "none";
    }
}
function ViewIndemnityConfirmation() {
    startAnimation();
    var random = Math.random();
    var indemnity = {};

    indemnity.NotificationId = $('#NotificationId').val();
    indemnity.OnBehalfOf = $('#ActingOnBehalfOf').val();
    indemnity.FirstMoveDate = $('#FromDateTime').val();
    indemnity.LastMoveDate = $('#ToDateTime').val();

    $("#dialogue").load("../Application/ViewIndemnityConfirmation", { indemnityConfirmation: indemnity }, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $('#contactDetails').modal('show');
        IndemntityConfirmationInit();
    });
}
function ViewTermsAndConditions() {
    var targetElem = 'terms-and-conditions';
    if ($('#' + targetElem).is(":visible")) {
        $('#' + targetElem).slideUp(1000);
        $("#chevlon-up-icon").css("display", "none");
        $("#chevlon-down-icon").css("display", "block");
        UpdateTermsAndConditionsStatusInWorkFlow(false);
    }
    else {
        $('#' + targetElem).slideDown(1000);
        $("#chevlon-up-icon").css("display", "block");
        $("#chevlon-down-icon").css("display", "none");
        UpdateTermsAndConditionsStatusInWorkFlow(true);
    }
}
function UpdateTermsAndConditionsStatusInWorkFlow(status) {
    $.ajax({
        url: '../Notification/UpdateTermsAndConditions',
        type: 'POST',
        datatype: 'json',
        data: { status },
        beforeSend: function () {
        },
        success: function (data) {
            
        },
        complete: function () {
        }
    });
}
function IndemntityConfirmationInit() {
    stopAnimation();
    Resize_PopUp(540);
    $('body').on('click', '#IDcloseMp', function (e) {
        e.preventDefault();
        closeMp(this);
    });
    $('body').on('click', '#span-close', function (e) {
        e.preventDefault();
        $('#overlay').hide();
        addscroll(this);
        resetdialogue(this);
    });
    $('body').on('click', '#PrintPage', function (e) {
        e.preventDefault();
        var link = "../Movements/IndemnityConfirmation?notificationId=" + $('#NotificationId').val() + "";
        window.open(link, '_blank');
    });
    removescroll();
    $("#dialogue").show();
    $("#overlay").show();
}
function closeSpan() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function closeMp() {
    $('#contactDetails').modal('hide');
    $("#overlay").hide();

}
