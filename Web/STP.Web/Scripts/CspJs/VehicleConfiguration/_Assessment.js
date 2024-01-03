    //$(document).ready(function () {
    $('#ConfigTypeId').val(@configId);
    if ($('#MovementTypeId').val() != 0) {
        $('#PreviousMovementTypeId').val($('#MovementTypeId').val());
    }
    $('#MovementTypeId').val(@movementTypeId);
    $('#ChangedMovementTypeId').val(@movementTypeId);
    $('#ChangedMovementTypeName').val('@movementType');
    $('#MaxTractorCount').val(@ViewBag.MaxTractorCount);
    $('#MaxTrailerCount').val(@ViewBag.MaxTrailerCount);
//});
