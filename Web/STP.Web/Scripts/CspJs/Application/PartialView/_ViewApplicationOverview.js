    $(document).ready(function () {
        StepFlag = 6;
        SubStepFlag = 0;
        CurrentStep = "Application Overview";
        $('#current_step').text(CurrentStep);
        SetWorkflowProgress(6);
        $("#IDEditApplicationOverview").on('click', EditApplicationOverview);
        $('#save_btn').hide();
        $('#confirm_btn').hide();
        $('#apply_btn').show();
        @*if ('@isVr1' == 'True') {
            $('#apply_btn').text('APPLY FOR VR1');
        }
        else {
            $('#apply_btn').text('APPLY FOR SPECIAL ORDER');
        }*@
    });

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



    function EditApplicationOverview() {
        LoadContentForAjaxCalls("POST", '../Application/ApplicationOverview', { appRevisionId: $('#AppRevisionId').val(), workflowProcess: "HaulierApplication" }, '#overview_info_section');
    }
     function SubmitApplication() {
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
