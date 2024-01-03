var configId;
var movementTypeId;
var movementType;
var MaxTractorCount;
var MaxTrailerCount;
//$(document).ready(function () {
//    VehicleConfigurationAssessmentInit();
//});
function VehicleConfigurationAssessmentInit(divId) {
    divId = '#' + divId;
    configId = $(divId).find('#hf_configId').val();
    movementTypeId = $(divId).find('#hf_movementTypeId').val();
    movementType = $(divId).find('#hf_movementType').val();
    MaxTractorCount = $(divId).find('#hf_MaxTractorCount').val();
    MaxTrailerCount = $(divId).find('#hf_MaxTrailerCount').val();
    if (configId != 0) {
        $('#ConfigTypeId').val(configId);
    }
    if ($('#MovementTypeId').val() != 0) {
        $('#PreviousMovementTypeId').val($('#MovementTypeId').val());
    }
    $('#MovementTypeId').val(movementTypeId);
    $(divId).find('#ChangedMovementTypeId').val(movementTypeId);
    $(divId).find('#ChangedMovementTypeName').val(movementType);
    if (MaxTractorCount != "")
        $('#MaxTractorCount').val(MaxTractorCount);
    if (MaxTrailerCount != "")
        $('#MaxTrailerCount').val(MaxTrailerCount);

    if (movementTypeId != "270006") {
        $('.div_Speed').hide();
    }
    else {
        $('.div_Speed').show();
    }
    $('.div_AxleWeight').show();

    $('#dropdown_div').show();
    //Hide Add Component button
    if (typeof vehicleTypeIdGlobal != 'undefined' &&
        (vehicleTypeIdGlobal == TypeConfiguration.TRACKED_VEHICLE || vehicleTypeIdGlobal == TypeConfiguration.MOBILE_CRANE ||
         vehicleSubTypeIdGlobal == SubTypeConfiguration.ENGPLANT_RIGID || vehicleSubTypeIdGlobal == SubTypeConfiguration.ENGPLANT_TRACKED)) {
        $('#dropdown_div').hide();
    }
}
