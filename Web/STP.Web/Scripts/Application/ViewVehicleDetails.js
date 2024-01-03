

$(document).ready(function () {
    $('body').on('click', '#closevehicle', function (e) {
        closeVehicleDetails();
    });
});
function closeVehicleDetails() {
    $('#vehicleDetails').modal('hide');
}