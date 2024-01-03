    $(document).ready(function () {
        $(".deleteConfigReg").on('click', DeleteConfigReg);
        $(".btn_add_new").on('click', AddConfigReg);
        $(".deleteConfigRow").on('click', DeleteConfigRegRow);
        $(".clearSpanConfig").keypress(function () {
            ClearSpanConfig();
        });
    });
    function add_row(_this) {

        var registerId = document.getElementById("txt_register_config").value;
        var fleetId = document.getElementById("txt_fleet_config").value;
        if (validateConfigDataV1(_this)) {
            if (registerId == '' && fleetId == '') {
                $('#div_reg_config_vehicle').find('#error_msg').text('Please select either registration plate or fleet id');
            }
            else {
                var table = document.getElementById("vehicle-reg-table");
                var table_len = (table.rows.length) - 1;
                var row = table.insertRow(table_len).outerHTML = "<tr id='row" + table_len + "' class='tr_config_Registration'><td id='registerId" + table_len + "' class='cls_regId txt_register_config tblregcomponent'>" + registerId + "</td><td id='fleetId" + table_len + "' class='cls_fleetId txt_fleet_config tblregcomponent'>" + fleetId + "</td><td><a href='#' RowId='" + table_len + "' class='delete btngrad btnrds btnbdr tdbutton deleteConfigRow'></a ></td></tr>";

                document.getElementById("txt_register_config").value = "";
                document.getElementById("txt_fleet_config").value = "";
            }
        }
    }

    function delete_row(no) {
        document.getElementById("row" + no + "").outerHTML = "";
    }

    //validate the fleetid and registrationid
    function validateConfigDataV1(_this) {

        var isvalid = true;
        var this_regId = $('#txt_register_config').val();
        var this_fleetId = $('#txt_fleet_config').val();


        $('#div_reg_config_vehicle').find('.cls_regId').each(function () {
            var prv_reg_txt = $(this).text();
            if (prv_reg_txt == this_regId && this_regId != '') {
                $('#div_reg_config_vehicle').find('#error_msg').text('Vehicle Registration already in use');
                isvalid = false;
            }
        });

        if (this_fleetId != '') {
            $('#div_reg_config_vehicle').find('.cls_fleetId').each(function () {
                var prv_fleet_txt = $(this).text();
                if (prv_fleet_txt == this_fleetId) {
                    $('#div_reg_config_vehicle').find('#error_msg').text('Fleet ID already in use');
                    isvalid = false;
                }


            });
        }
        return isvalid;
    }
    function ClearSpanConfig() {
        $('#div_reg_config_vehicle').find('#error_msg_config').text('');
    }


    function DeleteConfigRegistration(_this) {
        var IdNum = $(_this).closest('td').find('input:hidden').val();
        var planMovement = false;
        if ($('#IsMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() === 'true') {
            planMovement = true;
        }
        var vehicleId = $('#vehicleConfigId').val();
        if (vehicleId == undefined || vehicleId == "") {
            vehicleId = $('#vehicleId').val();
        }
        $.ajax({
            url: "../VehicleConfig/DeleteVehicleRegistration",
            type: 'POST',
            cache: false,
            data: { configId: vehicleId, IdNumber: IdNum, isMovement: planMovement },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.Success) {
                    $(_this).closest('tr').remove();
                }
            },
            error: function (data) {
                alert("error");
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function DeleteConfigReg(e) {
        DeleteConfigRegistration(e);
    }

    function AddConfigReg(e) {
        add_row(e);
    }
    function DeleteConfigRegRow(e) {
        var rowId = e.currentTarget.dataset.RowId;
        delete_row(rowId);
    }
