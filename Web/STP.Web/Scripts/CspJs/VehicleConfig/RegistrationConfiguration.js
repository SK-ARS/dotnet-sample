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
        if (validateConfigData(_this)) {
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

    function DeleteConfigReg(e) {
        deleteRowConfig(e);
    }
    function AddConfigReg(e) {
        add_row(e);
    }
    function DeleteConfigRegRow(e) {
        var rowId = e.currentTarget.dataset.RowId;
        delete_row(rowId);
    }
