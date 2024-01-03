$(document).ready(function () {
    $(".gross_weights1").hide();
    //$('body').on('click', '#table-head', function (e) { e.preventDefault(); viewSORTAdvHaulier(); });
    $('body').on('click', '#SORTAddOption', function (e) { e.preventDefault(); SortAddVehicleFilter(this); });

    $('body').on('click', '#FilterMoveInboxSORT .filters', function (e) { e.preventDefault(); showFilterDiv(this); });
    $('body').on('click', '.Sort-VehicleDimension', function (e) { SortVehicleDimension(this); });
    $('body').on('click', '.Sort-OperatorCount', function (e) { SortOperatorCountRange(this); });
    $('body').on('click', '#SORTRemoveOption', function (e) {e.preventDefault(); SortRemoveVehicleFilter(this); });

});
function showFilterDiv(_this) {
    
    var divId = $(_this).data('targetid');
    switch (divId) {
        case 'viewstatus':
            viewStatus();
            break;
        case 'viewotheroptions':
            viewmovements();
            break;
        case 'viewSORTAdvHaulier':
            if ($('#hf_IsPlanMovmentGlobal').length== 0) {
                viewSORTAdvHaulier(_this);
            }
            break;
        case 'viewMapFilter':
            viewMapFilter(_this);
            break;
    }
    //$('#' + divId).toggle();
}

function AllowNumericOnly() {
    // isnumeric
    $('.isnumeric').keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
}
$(function () {
    AllowNumericOnly();
});



function SortAddVehicleFilter(_this) {

    var divId = $(_this).closest('.VehicleFilter').attr('id');
    var condition = $('#' + divId).find('#sortoperator').val();
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if (condition == "1" || condition == "2") {
        $('#' + divId).find('#errormsg').hide();
        var div = $('#viewSORTAdvHaulier').html();
        if ($("#VehicleFilterData_" + vehicleFiltercount).length > 0) {
            vehicleFiltercount++;
        }
        var newFilterDiv = "#VehicleFilterData_" + vehicleFiltercount;
        var newDiv = $(div).find("#" + divId).attr("id", "VehicleFilterData_" + vehicleFiltercount);
        $('#VehicleFilterDiv').append(newDiv);
        $('#' + 'VehicleFilterData_' + vehicleFiltercount).find(".gross_weights1").hide();
        $('#' + 'VehicleFilterData_' + vehicleFiltercount).find('#RemoveOption').show();
        $('#' + divId).find('#SORTAddOption').hide();
        $('#' + divId).find('#SORTRemoveOption').show();

        $(newFilterDiv).find('.gross_weightUnit').text('kg');
        $(newFilterDiv).find('.gross_weights1Unit').text('kg');

        $(newFilterDiv).find('.vehicletextboxs').attr("id", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextboxs').attr("name", "gross_weight_max_kg");
        $(newFilterDiv).find('.vehicletextboxs1').attr("id", "gross_weight_max_kg1");
        $(newFilterDiv).find('.vehicletextboxs1').attr("name", "gross_weight_max_kg1");
        $(newFilterDiv).find('#OperatorCount').attr('style', 'width: 12rem;');
        $(newFilterDiv).find('.vehicletextbox').attr('style', 'width:60%; padding-left: 0rem;');
    }
    else {
        $('#' + divId).find('#errormsg').show();
    }
}

function SortOperatorCountRange(_this) {

    var OperatorCount = $(_this).val();
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    if (OperatorCount == "between") {
        $('#' + divId).find(".gross_weights1").show();
        $(_this).attr('style', 'width: 8rem;');
        $(_this).closest('.VehicleFilter').find(".vehicletextbox").attr('style', 'width: 90%; padding-left: 0rem;');
    }
    else {
        $(_this).attr('style', 'width: 15rem;');
        $('#' + divId).find(".gross_weights1").hide();
    }
}


function SortVehicleDimension(_this) {

    var fieldData = $(_this).val();
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    $('#' + divId).find("#gross_weight_max_kg").attr("name", fieldData);
    $('#' + divId).find("#gross_weight_max_kg").attr("id", fieldData);
    $('#' + divId).find("#gross_weight_max_kg1").attr("name", fieldData + "1");
    $('#' + divId).find("#gross_weight_max_kg1").attr("id", fieldData + "1");
    if (fieldData != "gross_weight_max_kg" && fieldData != "max_axle_weight_max_kg") {
        $('#' + divId).find('.gross_weightUnit').text('m');
        $('#' + divId).find('.gross_weights1Unit').text('m');
    }
    else {
        $('#' + divId).find('.gross_weightUnit').text('kg');
        $('#' + divId).find('.gross_weights1Unit').text('kg');
    }
}
function SortRemoveVehicleFilter(_this) {
    var divId = $(_this).closest('.VehicleFilter').attr('id');
    var vehicleFiltercount = $("div[class*='VehicleFilter']").length;
    if (vehicleFiltercount > 1) {
        $('#' + divId).remove();
        if (vehicleFiltercount == 2) {
            $('.VehicleFilter').find('#SORTAddOption').show();
            $('.VehicleFilter').find('#SORTRemoveOption').hide();
        }
    }
}
