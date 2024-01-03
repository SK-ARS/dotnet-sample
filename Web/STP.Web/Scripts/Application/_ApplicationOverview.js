var count;
var onchangeflag;
var userTypeIDVal;
var ApplicationRevIdVal;
var DateTimeVal;
var isValid1 = true;
function ApplicationOverviewInit() {
    isVehicleHasChanged = false;
    count = 0;
    onchangeflag = false;
    userTypeIDVal = $('#hf_userTypeID').val();
    ApplicationRevIdVal = $('#hf_ApplicationRevId').val();
    DateTimeVal = $('#hf_DateTime').val();
    StepFlag = 6;
    SubStepFlag = 0;
    CurrentStep = "Application Overview";
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(6);

    $('#backbutton').show();
    $('#back_btn').show();
    $('#save_btn').hide();
    $('#apply_btn').show();
    $('#confirm_btn').hide();

    if ($('#OverviewInfoSaveFlag').val() == 'true') {
        $('#cancel_overview_info_btn_cntr').show();
    }
    if ($('#IsVR1').val() == 'true') {
        $('#apply_btn').text('APPLY FOR VR1');
    }
    else {
        $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
    }
    if (userTypeIDVal != 696008) {//SORT
        $('#NumberofPieces').val('1');
        $('#NumberOfMovements').val('1');
    }
    var ApplicationRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;
    if (ApplicationRevId == 0) {
        ApplicationRevId = ApplicationRevIdVal;
    }
    $('#ApplicationRevId').val(ApplicationRevId);

    SupressAlphabets();

    $("#HelpHiddenId").val("CreateSO");
    if (userTypeIDVal == 696008) {
        $('#OrgHaulierContactName').val($('#HdnHaulierContactName').val());
        $('#OrgFax').val($('#HdnOrgFax').val());
        $('#OrgEmailId').val($('#HdnOrgEmailId').val());
        //$('#OrgId').val($('#HdnOrgIDSORT').val());
        //$('#HAContactId').val($('#SOHndContactID').val());
        $('#OrgName').val($('#HdnOrgNameSORT').val());
    }

    $("#MovementDateFrom").datepicker({
        dateFormat: "dd-mm-yy",
        changeYear: true,
        changeMonth: true,
        minDate: new Date(),
        onSelect: function (selected) {
            var startDate = selected.split("-").reverse().join("/");
            var fromDate = new Date(startDate);
            var toDate = $("#MovementDateTo").datepicker('getDate');
            if (fromDate > toDate) {
                $("#MovementDateTo").datepicker("setDate", fromDate);
            }
        },
        beforeShow: function (textbox, instance) {
            var rect = textbox.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            }, 0);
        }
    });
    $("#MovementDateTo").datepicker({
        dateFormat: "dd-mm-yy",
        changeYear: true,
        changeMonth: true,
        minDate: new Date(),
        beforeShow: function (textbox, instance) {
            var startDate = $("#MovementDateFrom").datepicker('getDate');
            $("#MovementDateTo").datepicker("option", "minDate", startDate);
            var rect = textbox.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            }, 0);
        }
    });

    if (userTypeIDVal == 696008) {
        $('#ApplicationDate').val(DateTimeVal);
        $('#ApplicationDueDate').val(DateTimeVal);

        $("#ApplicationDate").datepicker({
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                var dt = new Date(selected);
                var endDate = $("#ApplicationDueDate").datepicker('getDate');
                dt.setDate(dt.getDate());// + 1
                if (dt > endDate) {
                    $("#ApplicationDueDate").datepicker("setDate", dt);
                }
            }
        });
        $("#ApplicationDueDate").datepicker({
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                var dt = new Date(selected);
                dt.setDate(dt.getDate());// - 1

            }
        });
    }
    scrolltotop();
    ApplicationRouteListInit();
}
function chkvalidAppOverView() {
    var isValiddate;
    var fromDate = $('#MovementDateFrom').val();
    var toDate = $('#MovementDateTo').val();
    var result = ValidateMomentDateTime(fromDate, toDate, false);
    if (result == 1) {
        $('#overview_info_section').find('#spnMvmntFromDate').html('From date must be today\'s date or greater than today\'s date.');
        $('#overview_info_section').find('#spnMvmntToDate').html('');
        invalidElemAppOverview = invalidElemAppOverview == undefined ? $('#MovementDateFrom') : invalidElemAppOverview;
        isValiddate = false;
    }
    else if (result == 2) {
        $('#overview_info_section').find('#spnMvmntToDate').html('To date must be greater than from date.');
        $('#overview_info_section').find('#spnMvmntFromDate').html('');
        invalidElemAppOverview = invalidElemAppOverview == undefined ? $('#MovementDateTo') : invalidElemAppOverview;
        isValiddate = false;
    }
    else {
        $('#overview_info_section').find('#spnMvmntFromDate').html('');
        $('#overview_info_section').find('#spnMvmntToDate').html('');
        isValiddate = true;
    }
    return isValiddate;
}
function OverViewValidation(IsCompleted = true, NavigateToLink = '') {

    var count = 0;
    var isvalid = true;
    $('.error').html('');
    invalidElemAppOverview = undefined;
    $('#dateValidate').html('');
    //check for my reference
    var ref = $('#HaulierReference').val();
    var len = ref.length;

    if (len > 35) {
        count = count + 1;
        $('#overview_info_section').find('#spnHaulier_Reference').html('My reference should be 35 characters only');
    }
    else {
        $('#overview_info_section').find('#spnHaulier_Reference').html('');
    }

    if (chkvalidAppOverView()) {

        var reClass = /(^|\s)required(\s|$)/;  // Field is required
        var reValue = /^\s*$/;
        var elements = infoForm.elements;
        var el;
        for (var i = 0, iLen = elements.length; i < iLen; i++) {
            el = elements[i];

            if (reClass.test(el.className)) {
                $('#overview_info_section').find('#lbl' + el.name).html('');
            }
        }

        for (var i = 0, iLen = elements.length; i < iLen; i++) {
            el = elements[i];
            if (reClass.test(el.className) && reValue.test(el.value)) {
                if (el.name == 'HaulierReference')
                    $('#overview_info_section').find('#spnHaulier_Reference').html('Reference number is required');
                else if (el.name == 'FromAddress')
                    $('#overview_info_section').find('#lbl' + el.name).html('From summary is required');
                else if (el.name == 'ToAddress')
                    $('#overview_info_section').find('#lbl' + el.name).html('To summary is required');
                else if (el.name == 'Load')
                    $('#overview_info_section').find('#lbl' + el.name).html('Load description summary is required');
                else if ((el.name == 'NumberOfMovements') && (userTypeIDVal != 696008))
                    $('#overview_info_section').find('#lbl' + el.name).html('Number of Movements is required');
                else if ((el.name == 'NumberofPieces') && (userTypeIDVal != 696008))
                    $('#overview_info_section').find('#lbl' + el.name).html('Number of Pieces is required');
                else if ((el.name == 'ApplicationNotesFromHA'))//&& (userTypeIDVal != 696008))
                    $('#overview_info_section').find('#lbl' + el.name).html('Haulier notes is required');
                count = count + 1;                
            }            
        }
        isvalid = validateLengthAppOverview('#overview_info_section #HaulierReference', isvalid, '#spnHaulier_Reference');
        isvalid = validateLengthAppOverview('#overview_info_section #FromAddress', isvalid, '#lblFromAddress');
        isvalid = validateLengthAppOverview('#overview_info_section #ToAddress', isvalid, '#lblToAddress');
        isvalid = validateLengthAppOverview('#overview_info_section #NumberOfMovements', isvalid, '#lblNumberOfMovements');
        isvalid = validateLengthAppOverview('#overview_info_section #NumberofPieces', isvalid, '#lblNumberofPieces');
        isvalid = validateLengthAppOverview('#overview_info_section #LoadDescription', isvalid, '#lblLoad');
        isvalid = validateLengthAppOverview('#overview_info_section #NotesWithAppl', isvalid, '#lblApplicationNotesFromHA');

        //Other organisation
        var isOnBehalfChecked = $('#overview_info_section').find('#chkOnBehalfOf').is(':checked');
        if (isOnBehalfChecked) {
            if ($('#OnBehalOfContactName').val() == "") {
                $('#overview_info_section').find('#spContactName').html('Contact name is required');
                invalidElemAppOverview = invalidElemAppOverview == undefined ? $('#OnBehalOfContactName') : invalidElemAppOverview;
                isvalid = false;
            }
            if ($('#OnBehalOfHaulierOrgName').val() == "") {
                $('#overview_info_section').find('#spHaulierOrgName').html('Haulier organisation name is required');
                invalidElemAppOverview = invalidElemAppOverview == undefined ? $('#OnBehalOfHaulierOrgName') : invalidElemAppOverview;
                isvalid = false;
            }
            if ($('#OnBehalOfCountryID').val() == "0" ||$('#OnBehalOfCountryID').val() == "" || $('#OnBehalOfCountryID').val() == undefined) {
                $('#overview_info_section').find('#spHaulierCountry').html('Country is required');
                invalidElemAppOverview = invalidElemAppOverview == undefined ? $('#OnBehalOfCountryID') : invalidElemAppOverview;
                isvalid = false;
            }

            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfContactName', isvalid, '#spContactName');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulierOrgName', isvalid, '#spHaulierOrgName');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulierAddress1', isvalid, '#spHaulierAddress1');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulierAddress2', isvalid, '#spHaulierAddress2');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulierAddress3', isvalid, '#spHaulierAddress3');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulierAddress4', isvalid, '#spHaulierAddress4');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulierAddress5', isvalid, '#spHaulierAddress5');

            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulPostCode', isvalid, '#spOnBehalOfHaulPostCode');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulOperatorLicens', isvalid, '#spOnBehalOfHaulOperatorLicens');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulEmailID', isvalid, '#spOnBehalOfHaulEmailID');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulFaxNo', isvalid, '#spOnBehalOfHaulFaxNo');
            isvalid = validateLengthAppOverview('#overview_info_section #OnBehalOfHaulTelephoneNo', isvalid, '#spOnBehalOfHaulTelephoneNo');

        }

        if (!isvalid && invalidElemAppOverview!=undefined) {
            $('body,html').animate({ scrollTop: invalidElemAppOverview.position().top }, 800);
        }
        if (count == 0 && isvalid) {
            SubmitApplication(IsCompleted, NavigateToLink);
        } else if (NavigateToLink != '' && NavigateToLink != null)
            location.href = NavigateToLink;

    } else {
        if (invalidElemAppOverview != undefined) {
            $('body,html').animate({ scrollTop: invalidElemAppOverview.position().top }, 800);
        }
    }
}
var invalidElemAppOverview;
function validateLengthAppOverview(elem, currentStatus, errorSpanElem) {
    if ($(elem).length<=0)
        return currentStatus;
    var elemVal = $(elem).val();
    if (elemVal == '')
        return currentStatus;
    var elemLength = elemVal.length;
    var numberOfLineBreaks = (elemVal.match(/\n/g) || []).length || 0;
    elemLength = elemLength + numberOfLineBreaks;
    var elemMaxLength = $(elem).data('maxlength') || 500;
    if (elemLength > 0 && elemLength > elemMaxLength) {
        $(errorSpanElem).html('Maximum ' + elemMaxLength + ' characters allowed.');
        invalidElemAppOverview = invalidElemAppOverview == undefined ? $(elem) : invalidElemAppOverview;
        currentStatus = false;
    }
    return currentStatus;
}
$(document).ready(function () {
    $('body').on('click', '#CancelEditingOverview', function (e) {
        e.preventDefault();
        CancelEditingOverview(this);
    });
    $('body').on('change', '.app-over-view-on-change', function (e) {
        onchangefunctionSO(this);
    });
    $('body').off('focus', '#overview_info_section .edit-normal,#overview_info_section .form-control');
    $('body').on('focus', '#overview_info_section .edit-normal,#overview_info_section .form-control', function (e) {
        $(this).closest('.input-field').find('.error').html('');
    });
    $('body').on('click', '#chkOnBehalfOf', function (e) {
        if ($(this).is(':checked')) {
            $('#divVr1Details').show();
        } else {
            $('#divVr1Details').hide();
            $("#divVr1Details input").each(function () {
                this.value = "";
            });
            $('#divVr1Details select').val('0');
            $('#divVr1Details .error').text('');
        }
    });
});

function SubmitApplication(IsCompleted = true, NavigateToLink = '') {
    RemoveLocalObject(NavigationEnum.MOVEMENT_TYPE);
    RemoveLocalObject(NavigationEnum.OVERVIEW);
    CloseWarningPopup();
    var fromdate = $('#MovementDateFrom').val();
    var todate = $('#MovementDateTo').val();
    var infoForm = $("#infoForm");
    var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0; $('#ApplicationRevId').val();
    analysisId = $('#AnalysisId').val();
    $.ajax({
        url: "../Application/SaveAppGeneral?ApplicationrevId=" + AppRevId + "&workflowProcess=HaulierApplication",
        type: 'post',
        data: '{"soapplication":' + infoForm.serialize(),
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data.result) {
                if (IsCompleted) {
                    $('#OverviewInfoSaveFlag').val(true);
                    if ($('#IsVR1').val() == 'true') {
                        GenerateRouteAssessment($('#AppVersionId').val(), 0, analysisId, 0, AppRevId, function () {
                            SubmitAndCompleteApplication();
                        });
                    }
                    else {
                        SubmitAndCompleteApplication();
                    }

                }
                else if (NavigateToLink != '' && NavigateToLink != null) {
                    location.href = NavigateToLink;
                }
            }
            else {
                var msg1 = 'Application not saved.';
                ShowErrorPopup(msg1);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            var msg1 = 'Application not saved';
            ShowErrorPopup(msg1);
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });

}
//function to validate data on keypress
function SupressAlphabets() {
    $('#NumberofPieces').keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode > 31 && (charCode < 48 || charCode > 57));
    });

    $('#NumberOfMovements').keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode > 31 && (charCode < 48 || charCode > 57));
    });

    $('#OnBehalOfHaulFaxNo,#OnBehalOfHaulTelephoneNo').keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode > 31 && (charCode < 48 || charCode > 57));
    });
}
function onchangefunctionSO() {
    onchangeflag = true;
    $('#TextChangeFlagSO').val(onchangeflag);
}
function viewrouteparts(RoutePartNo) {
    if (document.getElementById(RoutePartNo).style.display !== "none") {
        document.getElementById(RoutePartNo).style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById(RoutePartNo).style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}
function SubmitAndCompleteApplication() {
    CloseWarningPopup();
    var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;
    var url = "";
    var workflowApplicationStatus = "HaulierApplication";
    if ($('#IsVR1').val() == 'true') {//Haulier
        url = "../Application/SubmitVR1Application";
    }
    else {
        url = "../Application/SubmitSoApplication";
    }

    if (userTypeIDVal == 696008) {//SORT
        if ($('#IsVR1').val() == 'true') {
            url = "../Application/SubmitSORTVR1Application"
        }
        else {
            url = "../Application/SubmitSORTSoApplication";
        }

    }
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        async: true,
        data: {
            apprevisionId: AppRevId,
            workflowProcess: workflowApplicationStatus
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {

            var strResult = result.Success;
            if (strResult == false) {
                var msg1 = "\"" + $('#HaulierReference').val() + '\" Application cannot be submitted because of some server issues, please try later';
                ShowErrorPopup(msg1);
            }
            else {
                isWorkFlowCompleted = true;
                var Msg = 'Application submitted successfully. The ESDAL reference is ' + "\"" + strResult + "\"";
                ShowSuccessModalPopup(Msg, "RedirectToMovementInbox");
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
function NavigateToOverviewConfirmButton() {
    $('#save_btn').hide();
    $('#apply_btn').show();
    $('#backbutton').show();
    CloseSuccessModalPopup();
    LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), isVr1: $('#IsVR1').val() }, '#overview_info_section', '', function () {
        ApplicationOverviewInit();
    });
}