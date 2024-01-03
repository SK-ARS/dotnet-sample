    SelectMenu(5);
    SubStepFlag = 2.5;
    $(document).ready(function () {
        $(".btn_back_config_edit").on('click', OnBackEditBtnClick);
        $("#btn_saveConfig").on('click', SaveVehicleConfigurationFn);
    });

    function SetRangeForSpacing() {
        var movementId = $('#movementTypeId').val();
        var componentId = parseInt($('#vehicleTypeValue').val());
        var vehicleTypeId = $('#VehicleTypeConfig').val();
        var unit = $('#UnitValue').val();

        if (movementId != 270001 && movementId != 270008 && movementId != 270003) {

            if (vehicleTypeId == 244002 && componentId == 234002) {
                $('#Axle_Spacing_To_Following').attr('range', '2.5,100');
                if (unit == 692002) {
                    ConvertRangeToFeet($('#Axle_Spacing_To_Following'));
                }

            }
            else if (vehicleTypeId == 244001 && componentId == 234001) {
                $('#Axle_Spacing_To_Following').attr('range', '1,50');
                if (unit == 692002) {
                    ConvertRangeToFeet($('#Axle_Spacing_To_Following'));
                }

            }
            else if ((vehicleTypeId == 244006 || vehicleTypeId == 244007)) {//&& (componentId == 234001 || componentId == 234002 || componentId == 234003) code commented for Bug #5114 axle spacing to following missing
                $('#Axle_Spacing_To_Following').attr('range', '0.5,100');
                if (unit == 692002) {
                    ConvertRangeToFeet($('#Axle_Spacing_To_Following'));
                }

            }
        }


    }

    function OnBackEditBtnClick() {
        $.ajax({
            url: '../Vehicle/BackButtonToConfig',
            type: 'POST',
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                
                var vehicleId = $('#vehicleConfigId').val();
                if (vehicleId == undefined || vehicleId == "") {
                    vehicleId = $('#vehicleId').val();
                }
                /*if (response != "") {*/
                if ($('div').find('#vehicle_Component_edit_section').length != 0) {
                    $.ajax({
                        type: "GET",
                        url: '../VehicleConfig/EditConfiguration',
                        data: { vehicleId: vehicleId, isApplication: true },
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (response) {
                            $('#vehicle_edit_section').html('');
                            $('#vehicle_Component_edit_section').html('');
                            $('#vehicle_Component_edit_section').html(response);
                            $('#vehicle_Component_edit_section').show();
                            $('.createConfig').unwrap('#banner-container');
                        },
                        error: function (result) {
                        },
                        complete: function () {
                            stopAnimation();
                        }
                    });
                }
                else {
                    window.location = "../vehicleConfig/EditConfiguration" + EncodedQueryString("vehicleId=" + vehicleId);
                }
            },
            complete: function () {
                stopAnimation();
            }
            //else {
            //    window.location = "../vehicleConfig/CreateConfiguration";
            //}
            //}
        });
    }

    function SaveVehicleConfigurationFn(e) {
        SaveVehicleConfiguration(e);
    }
