    $(document).ready(function () {
        $(".deleteCompReg").on('click', DeleteCompRegRow); 
        $(".btn_add_new").on('click', AddCompRegRow);
        $(".deleteCompRegNew").on('click', DeleteCompReg);
        $(".clearSpan").keypress(function () {
            ClearSpan();
        });
    });
    function RegistrationSaveClick(_this) {
        
        CloneRow(_this);
        var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
        var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
        if (configurableAxles == 'True') {
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();

            if (isNotify == 'True' || isNotify == 'true') {
                loadVR1vehAxles(numberOfAxles);
            }
            else if (ApplicationRevId == 0 && !isVR1) {
                loadAxles(numberOfAxles);
            }
            else if (isVR1 == 'True') {
                loadVR1vehAxles(numberOfAxles);
            }
            else {
                loadAppvehAxles(numberOfAxles);
            }
        }
        else {
            location.reload();
        }
    }

    function add_row_component(_this, id) {

        var table = document.getElementsByClassName("vh_" + id)[0];
        var registerId = $(table).find('#txt_register').val();
        var fleetId = $(table).find('#txt_fleet').val();

        //var registerId = document.getElementById("txt_register").value;
        //var fleetId = document.getElementById("txt_fleet").value;
        if (validatData(_this)) {
            if (registerId == '' && fleetId == '') {
                $('#div_reg_component_vehicle_' + id).find('#error_msg').text('Please select either registration plate or fleet id.');
            }
            else {               
                var table = document.getElementsByClassName("vh_" + id)[0];  
                var table_len = (table.rows.length) - 1;
                var row = table.insertRow(table_len).outerHTML = "<tr id='row" + table_len + "' class='tr_Registration'><td id='registerId" + table_len + "' class='cls_regId txt_register tblregcomponent'>" + registerId + "</td><td id='fleetId" + table_len + "' class='cls_fleetId txt_fleet tblregcomponent'>" + fleetId + "</td><td><a href='#' RowId=" + table_len + " class='delete btngrad btnrds btnbdr tdbutton deleteCompRegNew'></a ></td></tr>";

                $(table).find('#txt_register').val('');
                $(table).find('#txt_fleet').val('');

            }

        }
    }

    function delete_row_component(no) {
        document.getElementById("row" + no + "").outerHTML = "";
    }

    function DeleteCompRegRow(e) {
        var vehicleElementId = e.currentTarget.dataset.VehicleElementId;
        deleteRow(e,vehicleElementId);
    }
    function AddCompRegRow(e) {
        var vehicleElementId = e.currentTarget.dataset.VehicleElementId;
        add_row(e, vehicleElementId);
    }

    function DeleteCompReg(e) {
        var rowId = e.currentTarget.dataset.RowId;
        delete_row(rowId);
    }
