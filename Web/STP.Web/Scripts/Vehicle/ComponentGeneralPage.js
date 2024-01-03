$(document).ready(function () {
    vehicleId = $('#hf_VehicleConfigId').val();
    $('body').on('change', '.axledrop', function (e) {
        e.preventDefault();
        $("#btn_cancel").show();
        $('#componentBtn').show();
        var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
        var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
        //if (configurableAxles == 'True') {
            loadAxles(numberOfAxles);
        
        //}
    });
    $('body').on('change', '.pageKeyUpValidation', function (e) {
        keyUpValidationFn(this);
    });
    $('body').on('change', '.pageKeyUpSpeedValidation', function (e) {
        validationSpeedFn(this);
    });
    $('body').on('change', '#Weight', function (e) {
        AxleValidationCalculation();
    });
    $('body').on('click', '#btn_comp_finish', function (e) {
        startAnimation();
        var componentId = $('#Component_Id').val();
        var unit = $('#UnitValue').val();
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var isVR1 = $('#vr1appln').val();
        var isNotify = $('#ISNotif').val();
        if (isNotify == 'True' || isNotify == 'true') {
            isVR1 = 'True';
        }
        var vd = validation();
        var isTableValid = true;
        var vehicleTypeId = $('#vehicleTypeValue').val();
        //if (vehicleTypeId != TypeConfiguration.DRAWBAR_TRAILER && vehicleTypeId != TypeConfiguration.SEMI_TRAILER)
            //isTableValid=validateRegTable();
        var axleweightsum=0;
        $('#divCreateComponent').find('.axleweight').each(function () {
            var _thisVal = $(this).val();
            axleweightsum = axleweightsum + parseFloat(_thisVal);
        });
        var result = checkaxle_weight(axleweightsum);

        if (vd && result) {
            if (componentId == '') {
                SaveComponent(this);
            }
            else {
                UpdateData(this);
            }
        }
        else { stopAnimation(); }
        return false;
    });

});

function validateRegTable() {
    var isTableValid = true;
    var tablevehicletable = $('#vehicle-table.RegistrationComponent');
    if (tablevehicletable.length > 0) {
        console.log('tablevehicletable', true);
        var allTrs = $("#vehicle-table.RegistrationComponent > tbody > tr");
        var registrationDetails = [];
        allTrs.each(function (index, value) {//loop thorugh all tbody>tr items and insert all itemsinto an array
            
            var regIdComp = $(this).find('.txt_register').length > 0 ? $(this).find('.txt_register') : $(this).find('.txt_register');
            var fleetIdComp = $(this).find('.txt_fleet').length > 0 ? $(this).find('.txt_fleet') : $(this).find('.txt_fleet ');
            var regId = regIdComp.val() != "" ? regIdComp.val() : regIdComp.text();
            var fleetId = fleetIdComp.val() != "" ? fleetIdComp.val() : fleetIdComp.text();
            var id = $(this).find('td .hdId').length > 0 ? $(this).find('td .hdId').val() : 0;
            if (regId || fleetId) {
                var obj = { RegId: regId, FleetId: fleetId, Id: id };
                registrationDetails.push(obj);
            }
        }).promise().done(function () { // after loop through all tbody tr items, we need to check the table has atleast one entry
            console.log('registrationDetails', registrationDetails);
            if (registrationDetails.length == 0) {
                $('#error_msg').text('Please enter at least one registration plate or fleet id.');
                //setTimeout(function () {
                //    $('#error_msg').text('');
                //}, 5000);
                isTableValid = false;
            }
        });
    }
    return isTableValid;
}
function keyUpValidationFn(e) {
    keyUpValidation(e);
}
function validationSpeedFn(e) {
    validationSpeed(e);
}
$(function () {
    $(".axledrop").change(function () {

        $("#btn_cancel").show();
        $('#componentBtn').show();
        //$("#btn_cancel_axle").show();
        //$('#btn_cancel_axle').css('display', 'block');
        var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
        var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
        if (configurableAxles == 'True') {

            loadAxles(numberOfAxles);

            //var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            //var isVR1 = $('#vr1appln').val();
            //var isNotify = $('#ISNotif').val();

            //if (isNotify == 'True' || isNotify == 'true') {
            //    loadVR1vehAxles(numberOfAxles);
            //}
            //else if (ApplicationRevId == 0 && !isVR1) {
            //    loadAxles(numberOfAxles);
            //}
            //else if (isVR1 == 'True') {
            //    loadVR1vehAxles(numberOfAxles);
            //}
            //else {
            //    loadAppvehAxles(numberOfAxles);
            //}
        }
    });
});

function AxleValidationCalculation() {
    var weight = $('#Weight').val();
    if (weight == "") {
        weight = null;
    }
    $.ajax({
        url: '../Vehicle/AxleValidationCalculation',
        type: 'POST',
        data: { "weight": weight },
        success: function (response) {
            $('#axleweightRange').val(response);
        },
        error: function (response) {

        }
    });

}

