var IsVehicleComponentTypeChanged = false;
var isVehicleEditOnPlanMovement = false;
function CreateComponentInit() {
    //$('#dropdown_div').hide();
    $('#ComponentCount').val($('#hf_compCount').val());
    if ($('#hf_compCount').val() > 3) {
        if ($('#vehicle_Create_section').length > 0)
            $('#vehicle_Create_section #scroll-btns').show();
        else
            $('#scroll-btns').show();
    }
    else {
        if ($('#vehicle_Create_section').length > 0)
            $('#vehicle_Create_section #scroll-btns').hide();
        else
            $('#scroll-btns').hide();
    }

    var vehicleType = $('#vehicleType_Id').val();
    if ($('#hf_compType1').val() == TypeConfiguration.CONVENTIONAL_TRACTOR && $('#hf_compType2').val() == TypeConfiguration.SEMI_TRAILER ||
        $('#hf_compType1').val() == TypeConfiguration.RIGID_VEHICLE && $('#hf_compType2').val() == TypeConfiguration.DRAWBAR_TRAILER) {
        $('#divBoatMast').show();
        if (vehicleType == "244008")
            $('#BoatMastException').prop('checked', true);
        else
            $('#BoatMastException').prop('checked', false);

        if ($('#hf_compType1').val() == TypeConfiguration.CONVENTIONAL_TRACTOR && $('#hf_compType2').val() == TypeConfiguration.SEMI_TRAILER) {
            $('#divBoatMast .semi-trailer').show();
            $('#divBoatMast .rigid').hide();
        }
        if ($('#hf_compType1').val() == TypeConfiguration.RIGID_VEHICLE && $('#hf_compType2').val() == TypeConfiguration.DRAWBAR_TRAILER) {
            $('#divBoatMast .semi-trailer').hide();
            $('#divBoatMast .rigid').show();
        }

    }
    else {
        $('#BoatMastException').val('false');
        $('#BoatMastException').prop('checked', false);
        $('#divBoatMast').hide();
    }

    if ($('#ComponentCount').val() == 0) {
        $('#vehicle_back_btn').hide();
        $('#vehicle_next_btn').hide();
    }
    else {
        $('#vehicle_next_btn').show();
        $('#vehicle_back_btn').hide();
    }

    if (!isVehicleEditOnPlanMovement) {
        IsVehicleComponentTypeChanged = true;
    }
}
$(document).ready(function () {
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        CreateComponentInit();
    }
});
