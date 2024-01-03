    $(document).ready(function () {
        $(".viewVehicleDetail").on('click', VehicleDetails);
    });
    function VehicleDetails(e) {
        var vehicleId = e.currentTarget.dataset.VehicleId;
        ViewVehicleDetails(vehicleId);
    }
