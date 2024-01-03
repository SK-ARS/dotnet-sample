var DateTime = $('#hf_DateTime').val();

$(document).ready(function () {
    if ($('#hf_IsPlanMovmentGlobal').length <= 0) {
        SOAdvancedSearchInitDatePicker();
    }
    $('body').on('click', '#HaulAddOption', function (e) { e.preventDefault(); HaulAddVehicleFilter(this); });
    $('body').on('click', '#HaulRemoveOption', function (e) { e.preventDefault(); HaulRemoveVehicleFilter(this); })
    $('body').on('click', '.haul-vehicle-dimension', function (e) { haulVehicleDimension(this); });
    $('body').on('click', '.haul-operator-count', function (e) { OperatorCountRange(this); });
    $('body').on('click', '.viewAdvhaulier', function () { window['viewAdvHaulier'](); });
    $('.vehicletextbox1').attr('style', 'width:75%;');
    //$('body').on('dragend', '.folder-item', function (e) {
    //    dragEnd(e);
    //});
});
function OperatorCountRange(_this) {

    var OperatorCount = $(_this).val();
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    if ($('div#VehicleFilterData').length > 1) {
        $('div#VehicleFilterData:first').remove();
    }
    if (OperatorCount == "between") {
        $('#' + divId).find(".vehicletextbox").val('');
        $('#' + divId).find(".gross_weight1").val('');
        $('#' + divId).find(".gross_weight1").show();
        $(_this).closest('.VehicleFilter').find(".gross_weight1").show();
        $('#' + divId).find('.vehicletextbox1').attr('style', 'width:75%;');
    }
    else {
        $('#' + divId).find(".gross_weight1").hide();
        $('#' + divId).find(".gross_weight1").val('');
    }
}

function haulVehicleDimension(_this) {

    var fieldData = $(_this).val();
    if ($('div#VehicleFilterData').length > 1) {
        $('div#VehicleFilterData:first').remove();
    }
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    $('#' + divId).find("#gross_weight_max_kg").attr("name", fieldData);
    $('#' + divId).find("#gross_weight_max_kg").attr("id", fieldData);
    $('#' + divId).find("#gross_weight_max_kg1").attr("name", fieldData + "1");
    $('#' + divId).find("#gross_weight_max_kg1").attr("id", fieldData + "1");
    if ((fieldData != "gross_weight_max_kg") && (fieldData != "max_axle_weight_max_kg")) {
        $('#' + divId).find('.gross_weightUnit').text('m');
        $('#' + divId).find('.gross_weight1Unit').text('m');
    }
    else {
        $('#' + divId).find('.gross_weightUnit').text('kg');
        $('#' + divId).find('.gross_weight1Unit').text('kg');
    }
}
function HaulRemoveVehicleFilter(_this) {
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if (vehicleFiltercount > 1) {
        $('#' + divId).remove();
        if (vehicleFiltercount == 2) {
            $('.VehicleFilter').find('#HaulAddOption').show();
            $('.VehicleFilter').find('#HaulRemoveOption').hide();
        }
    }
}
function HaulAddVehicleFilter(_this) {

    var divId = $(_this).closest('.VehicleFilter').attr('id');
    var condition = $('#' + divId).find('.vehicleOperator').val();
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if ($('div#VehicleFilterData').length > 1) {
        $('div#VehicleFilterData:first').remove();
    }
    if ($('div#viewAdvHaulier #VehicleFilterDiv').length > 1) {
        $('div#viewAdvHaulier #VehicleFilterDiv:first').remove();
    }
    if (condition == "1" || condition == "2") {
        $('#' + divId).find('#errormsg').hide();
        var div = $('#viewAdvHaulier').html();
        if ($("#VehicleFilterData_" + vehicleFiltercount).length > 0) {
            vehicleFiltercount++;
        }
        var newFilterDiv = "#VehicleFilterData_" + vehicleFiltercount;
        var newDiv = $(div).find("#" + divId).attr("id", "VehicleFilterData_" + vehicleFiltercount);
        $('#VehicleFilterDiv').append(newDiv);
        $('#' + 'VehicleFilterData_' + vehicleFiltercount).find(".gross_weight1").hide();
        $('#' + 'VehicleFilterData_' + vehicleFiltercount).find('#HaulRemoveOption').show();
        $('#' + divId).find('#HaulAddOption').hide();
        $('#' + divId).find('#HaulRemoveOption').show();

        $(newFilterDiv).find('.gross_weightUnit').text('kg');
        $(newFilterDiv).find('.gross_weight1Unit').text('kg');

        $(newFilterDiv).find('.vehicletextbox').attr("id", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextbox').attr("name", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextbox1').attr("id", "gross_weight_max_kg1");
        $(newFilterDiv).find('.vehicletextbox1').attr("name", "gross_weight_max_kg1");
    }
    else {
        $('#' + divId).find('#errormsg').show();
    }
}
function SOAdvancedSearchInitDatePicker() {
    DateTime = $('#hf_DateTime').val();
    $("#MovementFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#MovementToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#MovementToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#MovementToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#MovementToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#MovementFromDate").datepicker("option", "maxDate", dt);
        }
    });

    $("#ApplicationFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#ApplicationToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#ApplicationToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#ApplicationToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#ApplicationToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#ApplicationFromDate").datepicker("option", "maxDate", dt);
        }
    });

    $("#NotificationFromDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#NotificationToDate").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#NotificationToDate").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#NotificationToDate").datepicker("setDate", dt);
            }
        }
    });
    $("#NotificationToDate").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#NotificationFromDate").datepicker("option", "maxDate", dt);
        }
    });
}
$('body').on('click', '#MovementDate', function (e) {
    if ($('#MovementDate').is(':checked')) {
        $('#MovementFromDate').attr('disabled', false);
        $('#MovementToDate').attr('disabled', false);
        $('#MovementFromDate').val(DateTime);
        $('#MovementToDate').val(DateTime);
    } else {
        $('#MovementFromDate').attr('disabled', 'disabled');
        $('#MovementFromDate').val('');
        $('#MovementToDate').attr('disabled', 'disabled');
        $('#MovementToDate').val('');
    }
});
$('body').on('click', '#ApplicationDate', function (e) {
    if ($('#ApplicationDate').is(':checked')) {
        $('#ApplicationFromDate').attr('disabled', false);
        $('#ApplicationToDate').attr('disabled', false);
        $('#ApplicationFromDate').val(DateTime);
        $('#ApplicationToDate').val(DateTime);
    } else {
        $('#ApplicationFromDate').attr('disabled', 'disabled');
        $('#ApplicationFromDate').val('');
        $('#ApplicationToDate').attr('disabled', 'disabled');
        $('#ApplicationToDate').val('');
    }
});
$('body').on('click', '#NotifyDate', function (e) {
    if ($('#NotifyDate').is(':checked')) {
        $('#NotificationFromDate').attr('disabled', false);
        $('#NotificationToDate').attr('disabled', false);
        $('#NotificationFromDate').val(DateTime);
        $('#NotificationToDate').val(DateTime);
    } else {
        $('#NotificationFromDate').attr('disabled', 'disabled');
        $('#NotificationFromDate').val('');
        $('#NotificationToDate').attr('disabled', 'disabled');
        $('#NotificationToDate').val('');
    }
});


//function EnableDateFields() {
//    $('#viewAdvHaulier input:checkbox').on('click', function () {
//        if ($(this).is(':checked')) {
//            $(this).closest('.row').find('input:text').attr('disabled', false);
//        }
//        else {
//            $(this).closest('.row').find('input:text').attr('disabled', true);
//            $(this).closest('.row').find('input:text').val("");
//            $(this).closest('.row').find('.field-validation-error').html("");
//        }
//    });
//}
function AllowNumericOnly() {
    // isnumeric
    $('.isnumeric').keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
}

$(function () {
    //EnableDateFields();
    AllowNumericOnly();
});

