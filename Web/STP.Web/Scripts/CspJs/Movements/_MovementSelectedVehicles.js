    debugger
    StepFlag = 2;
    SubStepFlag = 0;
    CurrentStep = "Vehicle Details";
    SetWorkflowProgress(2);
    if ($('#plan_movement_hdng').text() == '')
        $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);

    $('#save_btn').hide();
    $('#apply_btn').hide();
    $('#confirm_btn').removeClass('blur-button');
    $('#confirm_btn').attr('disabled', false);
    $('#confirm_btn').show();

    $('#IsVR1').val('@isVr1');
    $('#IsNotif').val('@isNotif');
    $('#IsSoApp').val('@isSoApp');
    $('#MovementId').val(@ViewBag.MovementId);

    @foreach(var item in VehicleList)
    {
        @:LoadSelectedVehicle(@item.VehicleId);
    }

    if (@IsAddVehicleError == 1) {
        SelectedVehicles.pop();
        ShowErrorPopup("The vehicle is not added to the movement, as it is not compatible with the original movement type.", 'CloseErrorPopup');
    }
    else if (@IsAddVehicleError == 2) {
        $('#IsAddVehicleError').val(@IsAddVehicleError);
        SelectedVehicles.pop();
        ShowErrorPopup("The vehicles in this movement are in different category. So we are unable to proceed further. Please edit the vehicle and proceed.", 'CloseErrorPopup');
    }
    else if (@IsAddVehicleError == 3) {
    $('#IsAddVehicleError').val(@IsAddVehicleError);
    //SelectedVehicles.pop();
    $('.btnAddVehicle').removeAttr('data-bs-toggle');
        ShowErrorPopup("Axle weight is more than gross weight for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');

    }
    else if (@IsAddVehicleError == 4) {
    $('#IsAddVehicleError').val(@IsAddVehicleError);
        //SelectedVehicles.pop();
        ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');

    }
    else if (@IsAddVehicleError == 5) {
    $('#IsAddVehicleError').val(@IsAddVehicleError);
        ShowErrorPopup("Based on the vehicle details you entered, system assess the movement as a Notification and thus cannot proceed further. Please edit the vehicle dimensions to match with an application.", 'CloseErrorPopup');
    }
    else if (@IsAddVehicleError == 6) {
    $('#IsAddVehicleError').val(@IsAddVehicleError);
        ShowErrorPopup("Construction and use vehicle. Does not need a notification.", 'CloseErrorPopup');
    }
    else if (@IsAddVehicleError == 7) {
    $('#IsAddVehicleError').val(@IsAddVehicleError);
        ShowErrorPopup("Based on the details you’ve filled in system couldn't identified the movement. Please edit the vehicle dimensions to match with an application.", 'CloseErrorPopup');
    }
    else{
    $('#IsAddVehicleError').val(@IsAddVehicleError);
    }
    @if (VehicleList == null)
    {
        @:LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
    }

    function ValidationMessage() {
        if (@IsAddVehicleError == 3) {
            ShowErrorPopup("Axle weight is more than gross weight for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');

        }
        else if (@IsAddVehicleError == 4) {
            ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');

        }
    }

    function LoadSelectedVehicle(vehicleId) {
        if (!SelectedVehicles.includes(vehicleId))
        {
            SelectedVehicles.push(vehicleId);
        }
    }

            //-------------------------------AMEND START
               $(document).ready(function () {
           
           
                   $('body').on('click','.create_vehicle', function() { window['CreateNewConfiguration'](); });
                   $('body').on('click','.choose_from_fleet', function() { window['ImportVehicle']('fleet'); });
                   $('body').on('click','.choose_from_prvmovement', function() { window['ImportVehicle']('prevMov'); });
                   $('body').on('click','.choosefromsimilarcombinations', function() { window['ChooseFromSimilarCombinations'](); });
                    $('body').on('click', '#spnclose', closeAmendmentPopup);
                    $('body').on('click', '#btnclose', closeAmendmentPopup);
        });

            var VehicleIdAmend = 0;
            var planMovement = false;
            if ($('#IsMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() === 'true') {
                planMovement = true;
            }
            function AmendVehicle(VehicleId) {

                VehicleIdAmend = VehicleId;
                $('#vehicleConfigId').val(VehicleIdAmend);
                $.ajax({
                    url: '../VehicleConfig/ConfigurationRegistration',
                    type: 'GET',
                    cache: false,
                    async: false,
                    data: { vehicleId: VehicleId, RegBtn: true, planMovement: planMovement },
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (response) {
                        $('#amendmentPopup').modal('show');
                        $('#amendmentPopup .modal-body').html(response);
                    },
                    complete: function () {
                        stopAnimation();
                    }
                });

            }

            $('#btnSaveAmend').click(function (e) {
                e.preventDefault();
                //get vehicle details from modal
                var listObj = { VehicleId: VehicleIdAmend, RegistrationDetails: [] };
                var allTrs = $("#amendmentPopup table.tbl_registration > tbody > tr");
                var loader = $("#amendmentPopup .modal-footer #AmendLoader");
                var buttons = $("#amendmentPopup .modal-footer .btn");
                buttons.hide();
                loader.show();
                allTrs.each(function (index, value) {
                    var isLastElement = index == allTrs.length - 1;
                    if (!isLastElement) {
                        var regIdComp = $(this).find('td span.cls_regId_config').length > 0 ? $(this).find('td span.cls_regId_config') : $(this).find('td.cls_regId');
                        var fleetIdComp = $(this).find('td span.cls_fleetId_config').length > 0 ? $(this).find('td span.cls_regId_config') : $(this).find('td.cls_fleetId');
                        var regId = regIdComp.text();
                        var fleetId = fleetIdComp.text();
                        var id = $(this).find('td .hdId').length > 0 ? $(this).find('td .hdId').val() : 0;
                        var obj = { RegId: regId, FleetId: fleetId, Id: id };
                        listObj.RegistrationDetails.push(obj);
                    }
                }).promise().done(function () {
                    console.log('listObj', listObj);
                    $.ajax({
                        url: '../VehicleConfig/AmendVehicleConfiguration',
                        type: 'POST',
                        cache: false,
                        async: false,
                        data: { amendRegistration: listObj },
                        beforeSend: function () {
                            //startAnimation();
                        },
                        success: function (response) {
                            buttons.show();
                            loader.hide();
                            if (response.success) {
                                $('#amendmentPopup').modal('hide');
                                ShowSuccessModalPopup("Successfully updated!", 'ReplaceUpdatedSection');
                            } else {
                                if (response.error && response.error != '')
                                    ShowErrorPopup(response.error);
                            }
                        },
                        complete: function () {
                            //stopAnimation();
                        },
                        error: function () {
                            buttons.show();
                            loader.hide();
                        }
                    });
                });;
            });

            function ReplaceUpdatedSection(vehicleId) {
                if (vehicleId == null || vehicleId == undefined || vehicleId == 0)
                    vehicleId = VehicleIdAmend;

                CloseInfoPopup('SuccessPopupAction');
                $.ajax({
                    url: '../VehicleConfig/ViewConfigurationGeneral',
                    type: 'GET',
                    cache: false,
                    async: false,
                    data: { vehicleId: vehicleId, isMovement: planMovement },
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (response) {
                        $('#viewcomponentdetails_' + VehicleIdAmend).html(response);
                    },
                    complete: function () {
                        stopAnimation();
                    }
                });
            }

            function closeAmendmentPopup() {
                ReplaceUpdatedSection(VehicleIdAmend);
                VehicleIdAmend = 0;
                $('#vehicleConfigId').val(VehicleIdAmend);
                $('#amendmentPopup .modal-body').html('');
                $('#amendmentPopup').modal('hide');
    }
    $('body').on('click', '.vehicleformovement', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var flag = $(this).data("flag");
     

        UseVehicleForMovement(vehicleId, flag);
    });
    $('body').on('click', '.validationmsg', function (e) {
      
        ValidationMessage();
    });                                               //-----------------------------------------------------------AMEND END
