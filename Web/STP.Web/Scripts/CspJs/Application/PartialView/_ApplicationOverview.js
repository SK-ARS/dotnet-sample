
	var count = 0;
        var onchangeflag = false;

    $(document).ready(function () {
        $("#CancelEditingOverview").on('click', CancelEditingOverview);
        StepFlag = 6;
        SubStepFlag = 0;
        CurrentStep = "Application Overview";
        $('#current_step').text(CurrentStep);
        SetWorkflowProgress(6);

        $('#back_btn').show();
        $('#save_btn').hide();
        $('#apply_btn').show();
        $('#confirm_btn').hide();

        if ($('#OverviewInfoSaveFlag').val() == 'true') {
            $('#cancel_overview_info_btn_cntr').show();
        }
        @*//if ('@Model.Load' != '') {//this check can be removed when the WIP step can be fetched from wrokflow
            //LoadContentForAjaxCalls("POST", '../Application/ViewApplicationOverview', { appRevisionId: $('#AppRevisionId').val() }, '#overview_info_section');
        //}*@
       /* if (($('#IsClone').val() == 1) || ($('#IsRevise').val() == 1)) {*/
            @*$('#IsVR1').val('@isVr1');
            $('#IsSoApp').val('@isSoApp');*@

        if ($('#IsVR1').val() == 'true') {
            $('#apply_btn').text('APPLY FOR VR1');
        }
        else {
            $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
        }
        /*}*/
        if ('@userTypeID' != 696008) {
            $('#NumberofPieces').val('1');
            $('#NumberOfMovements').val('1');
        }
        var ApplicationRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;
        if (ApplicationRevId == 0) {
            ApplicationRevId = '@Model.ApplicationRevId';
        }
        $('#ApplicationRevId').val(ApplicationRevId);

        SupressAlphabets();

        $("#HelpHiddenId").val("CreateSO");
        @*if ($('#CloneRevise').val() == 0) {
            $('#MovementDateFrom').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
            $('#MovementDateTo').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        }
        $('#ApplicationDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        $('#ApplicationDueDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');*@
        if ('@userTypeID' == 696008) {
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
                   var frmdt = selected.split("-").reverse().join("/");
                    var dt = new Date(frmdt);
                    var endDate = $("#MovementDateTo").val();
                    var dtnewdate1 = endDate.split("-").reverse().join("/");
                    dt.setDate(dt.getDate()); //+ 1
                    $("#MovementDateTo").datepicker("option", "minDate", dt);
                    if (dt > new Date(dtnewdate1)) {
                      //  $("#MovementDateTo").datepicker("setDate", dt);
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
                 onSelect: function (selected) {
                   var todate = selected.split("-").reverse().join("/");
                    var dt = new Date(todate);
                    dt.setDate(dt.getDate()); //+ 1
                    $("#MovementDateFrom").datepicker("option", "maxDate", new Date(dt))
                           
                    
                },
                beforeShow: function (textbox, instance) {

                    var rect = textbox.getBoundingClientRect();
                    setTimeout(function () {
                        var scrollTop = $("body").scrollTop();
                        instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                    }, 0);
                }
            });
      
            if ('@userTypeID' == 696008) {
                $('#ApplicationDate').val('@DateTime.Now.ToString("dd-MM-yyyy")');
                $('#ApplicationDueDate').val('@DateTime.Now.ToString("dd-MM-yyyy")');

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
        });
	var isValid1 = true;
        function chkvalid() {
            var isValiddate = ValidateDate();
            return isValiddate;
		//return true;
	}
        function ValidateDate() {

            var isValid = true;

            //if ('@userTypeID' == 696008) {
            var dt1 = $('#MovementDateFrom').val();
            var dt2 = $('#MovementDateTo').val();

            var d = new Date();
            var month = d.getMonth() + 1;
            var day = d.getDate();
            var output = (('' + day).length < 2 ? '0' : '') + day + '/' + (('' + month).length < 2 ? '0' : '') + month + '/' + d.getFullYear();
            var d = dt1.split('-');
            var d1 = d[1] + '/' + d[0] + '/' + d[2];
            var fromdate = Date.parse(d1);
            var dt = dt2.split('-');
            var d2 = dt[1] + '/' + dt[0] + '/' + dt[2];
            var todate = Date.parse(d2);
            var dtt = output.split('/');
            var d3 = dtt[1] + '/' + dtt[0] + '/' + dtt[2];
            var todaydate = Date.parse(d3);
            if (fromdate > todate) {
                $('#overview_info_section').find('#spnMvmntToDate').html('To date must be greater than from date.');
                $('#overview_info_section').find('#spnMvmntFromDate').html('');
                return false;
            }
            else {
                if (fromdate < todaydate) {//|| fromdate == todaydate
                    $('#overview_info_section').find('#spnMvmntFromDate').html('From date must be today\'s date or greater than today\'s date.');
                    $('#overview_info_section').find('#spnMvmntToDate').html('');
                    return false;
                }
                else {
                    $('#overview_info_section').find('#spnMvmntFromDate').html('');
                    $('#overview_info_section').find('#spnMvmntToDate').html('');
                    return true;
                }
            }

            return isValid;
	}
    function OverViewValidation() {
        var count = 0;
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

		if (chkvalid())
		{

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
					// Required field has no value or only whitespace
					// Advise user to fix
					//if (el.name == 'Haul_Description')
					//    $('#lbl' + el.name).html('Description is required');
                    if (el.name == 'FromAddress')
                        $('#overview_info_section').find('#lbl' + el.name).html('From summary is required');
                    else if (el.name == 'ToAddress')
                        $('#overview_info_section').find('#lbl' + el.name).html('To summary is required');
					else if (el.name == 'Load')
                        $('#overview_info_section').find('#lbl' + el.name).html('Load description summary is required');
                    else if ((el.name == 'NumberOfMovements') && ('@userTypeID' != 696008))
                        $('#overview_info_section').find('#lbl' + el.name).html('Number of Movements is required');
                    else if ((el.name == 'NumberofPieces') && ('@userTypeID' != 696008))
                        $('#overview_info_section').find('#lbl' + el.name).html('Number of Pieces is required');
                    else if ((el.name == 'ApplicationNotesFromHA') )//&& ('@userTypeID' != 696008))
                        $('#overview_info_section').find('#lbl' + el.name).html('Haulier notes is required');
					count = count + 1;
				}
			}
			if (count == 0) {
                SubmitApplication();
			}

		}
	}
    function SubmitApplication() {
        CloseWarningPopup();
		var fromdate = $('#MovementDateFrom').val();
		var todate = $('#MovementDateTo').val();
		var infoForm = $("#infoForm");
        var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0; $('#ApplicationRevId').val();
		$.ajax({
            url: "../Application/SaveAppGeneral?ApplicationrevId=" + AppRevId + "&workflowProcess=HaulierApplication",
			type: 'post',
            data: '{"soapplication":' + infoForm.serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {

                if (data.result) {
                    $('#OverviewInfoSaveFlag').val(true);
                    SubmitAndCompleteApplication();
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
	}

	function onchangefunctionSO(){
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
        if ($('#IsVR1').val() == 'true') {
            url = "../Application/SubmitVR1Application";
        }
        else {
            url = "../Application/SubmitSoApplication";
        }

        if ('@userTypeID' == 696008) {
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
                    var msg1 = "\"" + $('#HaulierReference').val() + '\" Application cannot be submitted because of some server issues, please try later.';
                    ShowErrorPopup(msg1);
				}
				else {
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
        //var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;
        //var msg = "";
        //var autoReplanSucess = 0;
        //var brokenRouteCount = 0;
        //if (AppRevId != 0 && deleteFlag != 1) {//deleteflag checks implimented for ignoring broken route chceks on delete a so
        //    var is_replan = CheckIsBroken(0, 0, AppRevId, 0, 0, 0);
        //    for (var i = 0; i < is_replan.length; i++) {
        //        if (is_replan[i].isBroken > 0) {
        //            brokenRouteCount++;
        //            if (is_replan[i].isReplan < 2) {	//no special manouers exists
        //                autoReplanSucess++;
        //            }
        //        }
        //    }
        //}
        //if (brokenRouteCount != 0 && autoReplanSucess > 0) {
        //    msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.';
        //    ShowWarningPopup(msg, 'WarningCancelBtn');
        //}
        //else {
        $('#save_btn').hide();
        $('#apply_btn').show();
        CloseSuccessModalPopup();
        LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val() }, '#overview_info_section');
       // }
    }


    function CancelEditingOverview() {
        $('#save_btn').hide();
        $('#apply_btn').show();
        LoadContentForAjaxCalls("POST", '../Application/ViewApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val() }, '#overview_info_section');
    }

    function ViewSupplimentaryDetails() {
        if (document.getElementById('viewsuplimentaryinfo').style.display !== "none") {
            document.getElementById('viewsuplimentaryinfo').style.display = "none"
            document.getElementById('chevlon-up-icon_supp_info').style.display = "none"
            document.getElementById('chevlon-down-icon_supp_info').style.display = "block"
        }
        else {
            document.getElementById('viewsuplimentaryinfo').style.display = "block"
            document.getElementById('chevlon-up-icon_supp_info').style.display = "block"
            document.getElementById('chevlon-down-icon_supp_info').style.display = "none"
        }
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
    function ViewRouteVehicles(RoutePartId) {
        var ComponentCntrId = "viewroutedetails_" + RoutePartId;
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + RoutePartId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + RoutePartId).style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + RoutePartId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + RoutePartId).style.display = "none"
        }
    }
    function ViewVehicleDetails(VehicleId) {
        var ComponentCntrId = "viewcomponentdetails_" + VehicleId;
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "none"
        }
    }


