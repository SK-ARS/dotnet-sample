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
   
    function InsertMovementType() {
        debugger
        var isSort = $('#IsSortUser').val();
        var allocateUserId = 0;
        var radioValue = $("input[name='applicationType']:checked").val();
        if (isSort == 'True') {
            allocateUserId = $('#dropSort').val();
           // radioValue = 1;
        }
        var dt = new Date();
        var hour = (('' + dt.getHours()).length < 2 ? '0' : '') + dt.getHours();
        var minute = (('' + dt.getMinutes()).length < 2 ? '0' : '') + dt.getMinutes();
        var second = (('' + dt.getSeconds()).length < 2 ? '0' : '') + dt.getSeconds();
        var currentTime = hour + ':' + minute + ':' + second;
        var dataModelPassed = {
            movementId: $('#MovementId').val(),
            startApplicationProcess: true,
            haulierRefNo: $('#RefNo').val(),
            fromDate: $('#FromDate').val(),
            toDate: $('#ToDate').val(),
            fromSummary: $('#FromSummary').val(),
            toSummary: $('#ToSummary').val(),
            applicationType: radioValue,
            allocateUserId: allocateUserId,
            time: currentTime
        };

        var result = ApplicationRouting(1, dataModelPassed);
        if (result != undefined || result != "NOROUTE") {
            $.ajax({
                url: result.route, //"../Movements/InsertMovementType",
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
                        if (response.movementType == 207003) {
                            $('#NotificationId').val(response.data.NotificationId);
                            $('#NotifVersionId').val(response.data.VersionId);
                            $('#NotifProjectId').val(response.data.ProjectId);
                            $('#NotifAnalysisId').val(response.data.AnalysisId);
                            $('#CRNo').val(response.data.ContentRefNum);
                            $('#IsNotif').val(true);
                            $('#apply_btn').text('SUBMIT NOTIFICATION');
                            $('#plan_movement_hdng').text("NOTIFICATION");
                            if ($('#IsSortUser').val() == 'True')
                                $('#step6 p').text('Route Assessment');
                            else
                                $('#step5 p').text('Route Assessment');
                           // ShowSuccessModalPopup("movement is now saved as WIP", 'CloseSuccessModalPopup');
                            if ($('#IsNotifyApplication').val() == 1) {
                                
                                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: response.data.ContentRefNum, isNotif: $('#IsNotif').val(), workflowProcess: "HaulierApplication", IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
                                
                                        }
                            else {
                                ShowInfoPopup("Movement is now saved as WIP.", LoadContentForAjaxCalls("POST", '../ Routes / MovementRoute', { contRefNum: response.data.ContentRefNum, isNotif: $('#IsNotif').val(), workflowProcess: "HaulierApplication", IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section'));
                                //ShowInfoPopup("movement is now saved as WIP",LoadContentForAjaxCalls("POST", '../ Routes / MovementRoute', { contRefNum: response.data.ContentRefNum, isNotif: $('#IsNotif').val(), workflowProcess: "HaulierApplication", IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section'));
                                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { contRefNum: response.data.ContentRefNum, isNotif: $('#IsNotif').val(), workflowProcess: "HaulierApplication", IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
                            }
                        }
                        else {
                            $('#AppRevisionId').val(response.data.RevisionId);
                            $('#IsVR1').val(response.data.IsVr1);
                            $('#AppVersionId').val(response.data.VersionId);
                            if ($('#IsVR1').val() == 'true') {
                                $('#apply_btn').text('APPLY FOR VR1');
                                $('#plan_movement_hdng').text("APPLY FOR VR1");
                            }
                            else {
                                $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
                                $('#plan_movement_hdng').text("APPLY FOR SPECIAL ORDER");
                            }
                            if ($('#IsSortUser').val() == 'True')
                                $('#step6 p').text('Supplementary Information');
                            else
                                $('#step5 p').text('Supplementary Information');
                            ShowInfoPopup("Movement is now saved as WIP.", LoadContentForAjaxCalls("POST", '../ Routes/MovementRoute', { apprevisionId: response.data.RevisionId, versionId: response.data.VersionId, contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section'));
             
                          
                            LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: response.data.RevisionId, versionId: response.data.VersionId, contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
                        }
                    }
                    //else {
                    //    ShowInfoPopup('The vehicle registration details are missing for the vehicle. Go to vehicle details page and click on amend vehicle button to enter the mandatory registration details.');
                    //}
                },
                error: function (result) {
                    ShowErrorPopup("Something went wrong.");
                },
                complete: function () {
                    //stopAnimation();
                }
            });
        }
    }

    function UpdateMovementType() {
        var isSort = $('#IsSortUser').val();
        var radioValue = $("input[name='applicationType']:checked").val();
        var allocateUserId = 0;

        if (isSort == 'True') {
            allocateUserId = $('#dropSort').val();
            //radioValue = 1;
        }
        var dt = new Date();
        var hour = (('' + dt.getHours()).length < 2 ? '0' : '') +  dt.getHours();
        var minute = (('' + dt.getMinutes()).length < 2 ? '0' : '') + dt.getMinutes();
        var second = (('' + dt.getSeconds()).length < 2 ? '0' : '') + dt.getSeconds();
        var currentTime = hour + ':' + minute + ':' + second;
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
            applicationType: radioValue,
            allocateUserId: allocateUserId,
            time: currentTime
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

                if (AmendFlag >= 1) {
                    if (response.movementType == 207003) {
                        $('#NotificationId').val(response.data.NotificationId);
                        $('#NotifVersionId').val(response.data.VersionId);
                        $('#NotifProjectId').val(response.data.ProjectId);
                        $('#NotifAnalysisId').val(response.data.AnalysisId);
                        $('#CRNo').val(response.data.ContentRefNum);
                        $('#IsNotif').val(true);
                        $('#apply_btn').text('SUBMIT NOTIFICATION');
                        $('#plan_movement_hdng').text("NOTIFICATION");
                        $('#AppRevisionId').val(0);
                        $('#IsVR1').val(false);
                        $('#AppVersionId').val(0);
                        if ($('#IsSortUser').val() == 'True')
                            $('#step6 p').text('Route Assessment');
                        else
                            $('#step5 p').text('Route Assessment');
                        
                        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", NotificationEditFlag: $('#NotificationEditFlag').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
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
                        }
                        else {
                            $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
                            $('#plan_movement_hdng').text("APPLY FOR SPECIAL ORDER");
                        }
                        if ($('#IsSortUser').val() == 'True')
                            $('#step6 p').text('Supplementary Information');
                        else
                            $('#step5 p').text('Supplementary Information');
                        //ShowSuccessModalPopup("movement is now saved as WIP", 'CloseSuccessModalPopup');
                        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: response.data.RevisionId, versionId: response.data.VersionId, contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", NotificationEditFlag: $('#NotificationEditFlag').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
                    }
                }
                //else {
                //    ShowInfoPopup('The vehicle registration details are missing for the vehicle. Go to vehicle details page and click on amend vehicle button to enter the mandatory registration details.');
                //}
            },
            error: function (result) {
                ShowErrorPopup("Something went wrong.");
            },
            complete: function () {
                //stopAnimation();
            }
        });
    }
    function MovementTypeValidation() {
        var count = true;
        $('#dateValidate').html('');
        //check for my reference
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

        if (!chkvalid()) {
            count = false;
        }
        //if (chkvalid()) {
        //    var reClass = /(^|\s)required(\s|$)/;  // Field is required
        //    var reValue = /^\s*$/;
        //    var elements = infoForm.elements;
        //    var el;
        //    for (var i = 0, iLen = elements.length; i < iLen; i++) {
        //        el = elements[i];

        //        if (reClass.test(el.className)) {
        //            $('#lbl' + el.name).html('');
        //        }
        //    }

        //}


        return count;
    }


	var isValid1 = true;
    function chkvalid() {
        var isValiddate = ValidateDate();
        return isValiddate;
        //return true;
    }

    function ValidateDate() {

        var isValid = true;

            //if ('@userTypeID' == 696008) {
        var dt1 = $('#FromDate').val();
        var dt2 = $('#ToDate').val();
        var dtnewdate1 = dt1.split("-").join("/");
        var dtnewdate2 = dt2.split("-").join("/");

                var d = new Date();
                var month = d.getMonth() + 1;
                var day = d.getDate();
                var hour = d.getHours();
                var minute = d.getMinutes();
                var second = d.getSeconds();
                var output = (('' + day).length < 2 ? '0' : '') + day + '/' +
                    (('' + month).length < 2 ? '0' : '') + month + '/' +
                    d.getFullYear() + ' ' +
                    (('' + hour).length < 2 ? '0' : '') + hour + ':' +
                    (('' + minute).length < 2 ? '0' : '') + minute;


        var ArrayDateTime1 = dtnewdate1.split(' ');
                var ArrayDMY1 = ArrayDateTime1[0].split('/');
                var NewDate1 = new Date(ArrayDMY1[2], ArrayDMY1[1], ArrayDMY1[0], 0, 0, 0, 0);
                var TimeOfDate1 = NewDate1.getTime();

        var ArrayDateTime2 = dtnewdate2.split(' ');
                var ArrayDMY2 = ArrayDateTime2[0].split('/');
                var NewDate2 = new Date(ArrayDMY2[2], ArrayDMY2[1], ArrayDMY2[0], 0, 0, 0, 0);
                var TimeOfDate2 = NewDate2.getTime();

                var ArrayDateTime3 = output.split(' ');
                var ArrayDMY3 = ArrayDateTime3[0].split('/');
                var NewTodayDate = new Date(ArrayDMY3[2], ArrayDMY3[1], ArrayDMY3[0], 0, 0, 0, 0);
                var TimeOfTodayDate = NewTodayDate.getTime();
                if (ArrayDMY1[0] == "31" && ArrayDMY2[0] == "01") {
                    if (ArrayDMY1[1] < ArrayDMY2[1]) {
                        $('#spnFromDate').html('');
                        $('#spnToDate').html('');

                    }
                }
                else {
                    // ensure both evaluate to true
                    if (dt1 && dt2) {
                        if (TimeOfDate1 > TimeOfDate2) {
                            $('#spnToDate').html('To date must be greater than from date.');
                            $('#spnFromDate').html('');
                            isValid = false;
                        }
                        else {
                            if (TimeOfDate1 < TimeOfTodayDate) {
                                $('#spnFromDate').html('Date must be today\'s date or greater than today\'s date.');
                                $('#spnToDate').html('');
                                isValid = false;
                            }
                            else {
                                $('#spnFromDate').html('');
                                $('#spnToDate').html('');

                            }
                        }
                    }
                }
            //}

            return isValid;
	}
    //function wip() {
    //    debugger
    //    // LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: response.data.RevisionId, versionId: response.data.VersionId, contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
    //    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: response.data.RevisionId, versionId: response.data.VersionId, contRefNum: response.data.ContentRefNum, workflowProcess: "HaulierApplication", isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
    //}

