    var onchangeflag = false;

    $(document).ready(function () {
        StepFlag = 6;
        SubStepFlag = 0;
        CurrentStep = "Notification Overview";
        //$('#plan_movement_hdng').text("PLAN MOVEMENT");
        $('#current_step').text(CurrentStep);
        $('#VSOType').val($('#VSO_Type').val());
        $('#VehicleClass').val(@planMovePayLoad.MvmntType.VehicleClass);
        SetWorkflowProgress(6);

        $('#back_btn').hide();
        $('#save_btn').hide();
        $('#apply_btn').hide();
        $('#confirm_btn').hide();
        @if (ConstructionAndUse) {
            @:$('#actingOnBehalf').show();
        }
        if ($('#OverviewInfoSaveFlag').val() == 'true') {
            $('#cancel_overview_info_btn_cntr').show();
        }
        $("#FromDateTime").datepicker({
            dateFormat: "dd/mm/yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),          
            onSelect: function (selected) {
                var frmdt = selected.split("/").reverse().join("-");
                var dt = new Date(frmdt);              
                var endDate = $("#ToDateTime").val();
                var dtnewdate1 = endDate.split("/").reverse().join("-");              
                dt.setDate(dt.getDate());// + 1              
                //  $("#ToDate").datepicker("option", "minDate", dt || 1);

                   $("#ToDateTime").datepicker("option", "minDate", dt);
                if (dt > new Date(dtnewdate1)) {
                  //  $("#ToDateTime").datepicker("setDate", dt);
                 
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
        $("#ToDateTime").datepicker({
            dateFormat: "dd/mm/yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                var todate = selected.split("/").reverse().join("-");
                var dt = new Date(todate);
                dt.setDate(dt.getDate()); //+ 1
                $("#FromDateTime").datepicker("option", "maxDate", new Date(dt))

            },
            beforeShow: function (textbox, instance) {
                var rect = textbox.getBoundingClientRect();
                setTimeout(function () {
                    var scrollTop = $("body").scrollTop();
                    instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                }, 0);
            }
        });
        ViewRouteAndVehicles('viewroutes');
        ViewTermsAndConditions();
        showIndemnity();
        $('#TimeFrom').timepicker({ 'scrollDefault': 'now', 'timeFormat': 'H:i' });
        $('#TimeTo').timepicker({ 'scrollDefault': 'now', 'timeFormat': 'H:i' });
        $("#ViewNotificationOverview").on('click', ViewNotificationOverview);
        $("#btn_view_indemnification").on('click', ViewIndemnityConfirmation);
        $(".onchangefunctionDN").on('change', onchangefunctionDN);
        
        
    });
    $('#FromSummary').keyup(function () {
        $('#spnFromSummary').html('');
    });
    $('#ToSummary').keyup(function () {
        $('#spnToSummary').html('');
    });
    $('#LoadDescription').keyup(function () {
        $('#spnLoadDescription').html('');
    });
    $('#Notes').keyup(function () {
        $('#spnNotes').html('');
    });
    $('#TimeTo').change(function () {
        $('#spnToTime').html('');
    });
    $('#TimeFrom').change(function () {
        $('#spnFromTime').html('');
    });

    function onchangefunctionDN() {
        onchangeflag = true;
    }

    function ValidateGeneralDetails() {
        debugger
        CheckValidationDN();

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
    function ViewRouteAndVehicles(ComponentCntrId) {
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('rou_veh_chevlon-up-icon3').style.display = "none"
            document.getElementById('rou_veh_chevlon-down-icon3').style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('rou_veh_chevlon-up-icon3').style.display = "block"
            document.getElementById('rou_veh_chevlon-down-icon3').style.display = "none"
        }
    }
    function CheckValidationDN() {
        debugger
        $('.validation-summary-errors ul li').remove();
        if (chkvalid()) {
            var count = 0;
            var reClass = /(^|\s)required(\s|$)/;  // Field is required
            var reValue = /^\s*$/;
            //var ele = document.getElementById("formnsd").getElementsByTagName("input");
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
                    if (el.name == 'VSONumber') {
                        $('#spn' + el.name).html('Vehicle special order no. is required');
                    }
                    //else if (el.name == 'SONumbers') {
                    //    $('#spn' + el.name).html('SO reference no. is required');
                    //}
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
                    //else if (el.name == 'Notes') {
                    //    $('#spn' + el.name).html('Notes are required');
                    //}

                    count = count + 1;
                }
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
            //check for my reference
            var ref = $('#MyReference').val();
            var len = ref.length;

            if (len > 35) {
                count = count + 1;
                $('#overview_info_section').find('#spnMyReference').html('My reference should be 35 characters only');
            }
            else {
                $('#overview_info_section').find('#spnMyReference').html('');
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
            else { $('#overview_info_section').find('#spnIndemnifyFlag').html('');}

            if ($('#TimeFrom').val()=='') {
                $('#overview_info_section').find('#spnToTime').html('Date To is required');
            }
            if ($('#TimeTo').val() =='') {
                $('#overview_info_section').find('#spnFromTime').html('Date From is required');
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
            }

            if (count == 0) {
                showWarningImminentMovement();
            }
        }
    }

    var isValid = true;
    function chkvalid() {
        var fromDateOnly = $('#FromDateTime').val();
        var toDateOnly = $('#ToDateTime').val();

        var fromTimeOnly = $('#TimeFrom').val();
        var toTimeOnly = $('#TimeTo').val();

        isValid = DateTimeValidate(fromDateOnly, toDateOnly, fromTimeOnly, toTimeOnly);

        return isValid;
    }

    function DateTimeValidate(startDate, endDate, startTime, endTime) {
        var result = "";
        var res = false;
        //var CurrentDate = new Date();
        var dt = new Date();
        var day = dt.getDate();
        var month = dt.getMonth() + 1;
        var hour = dt.getHours();
        var minute = dt.getMinutes();
        var currentDateTime = (('' + day).length < 2 ? '0' : '') + day + '/' + (('' + month).length < 2 ? '0' : '') + month + '/' + dt.getFullYear() + ' ' +
            (('' + hour).length < 2 ? '0' : '') + hour + ':' + (('' + minute).length < 2 ? '0' : '') + minute;
        var startDateTime = startDate + " " + startTime;
        var endDateTime = endDate + " " + endTime;
        $.ajax
            ({
                type: "POST",
                url: "../Notification/DateTimeValidate",
                async: false,
                data: { startDateTime: startDateTime, endDateTime: endDateTime, currentDateTime: currentDateTime },
                success: function (data) {
                    result = data.res;
                    if (result == 1) {
                        $('#overview_info_section').find('#spnFromDate').html('From date & time must be equal to or greater than today\'s date & time.');
                        $('#overview_info_section').find('#spnToDate').html('');
                        res = false;
                    }
                    else if (result == 2) {
                        $('#overview_info_section').find('#spnToDate').html('To date must be greater than from date.');
                        $('#overview_info_section').find('#spnFromDate').html('');
                        res = false;
                    }
                    else {
                        $('#overview_info_section').find('#spnFromDate').html('');
                        $('#overview_info_section').find('#spnToDate').html('');
                        res = true;
                    }
                },
                complete: function () {
                    stopAnimation();
                }
            });
        return res;
    }

    var imminentmsg = "";
    function showWarningImminentMovement() {

        var vehicleclass = $('#VehicleClass').val();
        var NotifID = $('#NotificationId').val();
        ContentRefno = $('#CRNo').val();
        var moveStartDate = $('#FromDateTime').val();
        var imminentMsg = '';
        if (vehicleclass != "241001") {
            $.ajax
                ({
                    type: "POST",
                    url: "../Notification/ShowImminentMovement",
                    data: { moveStartDate: moveStartDate, contentRefNo: ContentRefno, notificationId: NotifID },
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

                            var msg = "The movement from date is outside the statutory window (you are not notifying on time). Are you sure you want to continue with these movement dates? If Yes, you are required to telephone affected parties to obtain consent. Select Yes to continue with the current movement dates, select No to modify the movement dates.";
                            ShowWarningPopup(msg, 'SubmitNotification');
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("error");
                    },
                    complete: function () {
                        stopAnimation();
                    }
                });
        }
        else {
            $("#ImminentFlag").val("No imminent movement");
            $('#ShowWanringImiinent').css("display", "none");
            $('#warningImminent').html(' ');
            SubmitNotification();

        }
    }

    function SubmitNotification() {
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
            $.ajax({
                type: "POST",
                url: "../Notification/SaveNotificationGeneralDetails?notificationId=" + notifId,
                data: $(forminfod).serialize(),
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    if (data.result) {
                        $('#OverviewInfoSaveFlag').val(true);
                       // var Msg = 'Notification saved successfully.';
                        //ShowSuccessModalPopup(Msg);
                        GenerateNotification();

                        ////check validation
                        //NotifValidation();
                        //// startAnimation();

                        //if (dnbtn == "submit") {
                        //    $('.loading').hide();
                        //    $("#overlay").hide();
                        //    ShowDNRouteAnalysis(NotificationID, Analysisid, ContentRefNo);
                        //}
                        //else {
                        //    showWarningPopDialog('Notification updated successfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        //    $('#btn_GeneralSubmit').show();
                        //    $('.loading').hide();
                        //    $("#overlay").hide();
                        //}
                    }
                    else {
                        var msg1 = 'Notification not saved. Please fill mandatory fields';
                        ShowErrorPopup(msg1);
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
                var Msg = 'Notification submitted successfully. The ESDAL reference is ' + "\"" + data.EsdalRefNo + "\"";
                ShowSuccessModalPopup(Msg, "RedirectToNotificationOverview('" + $('#NotificationId').val() + "','" + data.EsdalRefNo + "')");
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function ViewNotificationOverview() {
        $('#save_btn').hide();
        CloseSuccessModalPopup();
        LoadContentForAjaxCalls("POST", '../Notification/NotificationGeneralDetails', { notificationId: $('#NotificationId').val(), workflowProcess: 'HaulierApplication' }, '#overview_info_section');
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
        //$("#").load("../Application/ViewIndemnityConfirmation?NotificationId=" + ID + "&random=" + random);
        var indemnity = {};

        indemnity.NotificationId = $('#NotificationId').val();
        indemnity.OnBehalfOf = $('#ActingOnBehalfOf').val();
        indemnity.FirstMoveDate = $('#FromDateTime').val();
        indemnity.LastMoveDate = $('#ToDateTime').val();

        $("#dialogue").load("../Application/ViewIndemnityConfirmation", { indemnityConfirmation: indemnity }, function () {/*?NotificationId = " + ID + " & random=" + random + "*/
            $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });

            $('#contactDetails').modal('show');
            stopAnimation();
        });
    }
