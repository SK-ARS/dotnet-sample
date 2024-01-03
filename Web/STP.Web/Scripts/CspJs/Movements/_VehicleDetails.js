    var vehicleId;
    //$('#returncopyvehicleid').val(@VehicleDetails.VehicleId);
    function RemoveVehicle(VehicleId) {
        vehicleId = VehicleId;
        ShowWarningPopup("Do you want to remove the Vehicle?", 'RemoveMovementVehicle');
    }
    function RemoveMovementVehicle() {
        $.ajax({
            type: "POST",
            url: "../VehicleConfig/DeleteMovementVehicle",
            data: { movementId: $('#MovementId').val(), vehicleId: vehicleId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                if (response.Success) {
                    CloseWarningPopup();
                    SelectedVehicles.pop();
                    var msg = 'The vehicle removed successfully.';
                    @if(isAgreedNotif)
                    {
                        @:msg = 'The vehicle deleted successfully. Please delete the route assigned to this vehicle. ';
                    }
                    ShowSuccessModalPopup(msg, "ReloadVehicleList('" + 3 + "')");
                }
            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function ReloadVehicleList(flag) {
        CloseSuccessModalPopup();
        if (flag == 2 || flag == 3) {
            $('#IsVehicleAutoAssigned_Flag').val(false);
        }
        LoadContentForAjaxCalls("POST", '../Movements/MovementSelectedVehicles', { movementId: $('#MovementId').val(), isVehicleModify:flag}, '#vehicle_details_section');
    }
    function CopyVehicle(VehicleId, Flag) {
        if ($('#IsAddVehicleError').val() == 3) {
            ShowErrorPopup("Axle weight is more than gross weight for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
        }
        else if ($('#IsAddVehicleError').val() == 4) {
            ShowErrorPopup("Total axle distance exceeds vehicle length for the vehicle. Please edit the vehicle and proceed.", 'CloseErrorPopup');
        }
        else {
            if (!SelectedVehicles.includes(VehicleId)) {
                SelectedVehicles.push(VehicleId);
            }
            LoadContentForAjaxCalls("POST", '../VehicleConfig/InsertMovementVehicle', { movementId: $('#MovementId').val(), vehicleId: VehicleId, flag: Flag }, '#vehicle_details_section');
        }
    }

    //function EditComponents(VehicleId) {
    //    LoadContentForAjaxCalls("GET", '../VehicleConfig/EditConfiguration', { vehicleId: VehicleId, isApplication: true }, '#vehicle_Component_edit_section');
    //}

    function AddMovementVehicleToFleet(vehhId, vehName) {
        CheckFormalNameExist(vehName, vehhId);
    }


    function EditComponents(VehicleId) {
        $.ajax({
            url: '../VehicleConfig/EditConfiguration',
            type: 'GET',
            cache: false,
            async: false,
            data: { vehicleId: VehicleId, isApplication: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                SubStepFlag = 2.4;
                $('#haulier_details_section').hide();
                $('#select_vehicle_section').hide();
                $('#vehicle_details_section').hide();
                $('#movement_type_confirmation').hide();
                $('#select_route_section').hide();
                $('#route').html('');
                $('#route').hide();
                $('#route_vehicle_assign_section').hide();
                $('#supplimentary_info_section').hide();
                $('#overview_info_section').hide();
                $('#vehicle_edit_section').hide
                $('#vehicle_Component_edit_section').hide();
                $('#vehicle_Create_section').hide();

                $('#vehicle_Component_edit_section').show();
                $('#vehicle_Component_edit_section').html(response);
                $('.new-vehicle').unwrap('#banner-container');
                $('.new-vehicle').attr("style", "padding-left:0px !important");
                $('#confirm_btn').hide();
                $("#back_btn").prop('disabled', false);
                SelectMenu(2);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    //function to check the formal name exists during add to fleet
    function CheckFormalNameExist(vehName, vehhId) {

        var flag = 0;
        $.ajax({
            url: '../VehicleConfig/CheckConfigFormalName',
            type: 'POST',
            cache: false,
            async: false,
            data: { VehicleName: vehName },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {

                if (data.success > 0) {
                    flag = 1;
                    ShowWarningPopup("Configuration already exists. Do you want to over write? <br/><br/>  Note : If you want to save it as a new configuration, edit the internal name and then add to fleet.", 'AddVehicleToFleet(' + vehhId + ',' + flag + ')');
                }
                else {
                    flag = data.success;
                    AddVehicleToFleet(vehhId, flag);
                }
            },
            complete: function () {
                stopAnimation();
            },
            error: function (data) {

            }
        });
    }


    //function to add to fleet
    function AddVehicleToFleet(vehicleappId, flag) {


        $.ajax({
            url: '../VehicleConfig/AddConfigToFleet',
            type: 'POST',
            cache: false,
            async: false,
            data: { VehicleId: vehicleappId, flag: flag },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.success > 0) {
                    CloseWarningPopup();
                    ShowSuccessModalPopup('Configuration added to fleet', 'ReloadVehicleList')
                }
                else {
                    ShowModalPopup('Not saved');
                }
            },
            complete: function () {
                stopAnimation();
            },
            error: function (data) {

            }
        });
    }

    $('body').on('click', '#imgcopyvehicle', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var flag = $(this).data("flag");


        CopyVehicle(vehicleId, flag);
    });
    $('body').on('click', '#imgaddmovement', function (e) {
        var vehhId = $(this).data("vehicleid");
        var vehName = $(this).data("vehiclename");


        AddMovementVehicleToFleet(vehhId, vehName);
    });
    $('body').on('click', '#imgeditmovement', function (e) {
        var VehicleId = $(this).data("vehicleid");
      
        EditComponents(VehicleId);
    });
    $('body').on('click', '#imgdltmovement', function (e) {
        var VehicleId = $(this).data("vehicleid");

        RemoveVehicle(VehicleId);
    });
    $('body').on('click', '#imgamendmovement', function (e) {
        var VehicleId = $(this).data("vehicleid");

        AmendVehicle(VehicleId);
    });
   
    $('body').on('click', '#spnviewvehicle', function (e) {
        var VehicleId = $(this).data("vehicleid");

        ViewVehicleDetails(VehicleId);
    });
