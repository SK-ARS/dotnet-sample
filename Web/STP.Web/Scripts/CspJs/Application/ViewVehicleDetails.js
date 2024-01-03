    $(document).ready(function () {
        $("#closevehicle").on('click', closeVehicleDetails);
    });
    function closeVehicleDetails() {
        $('#vehicleDetails').modal('hide');
    }
