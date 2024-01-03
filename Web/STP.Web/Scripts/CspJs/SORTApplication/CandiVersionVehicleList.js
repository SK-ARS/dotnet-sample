    $(document).ready(function () {

        $("#btnBactoGen").on('click', BactoGen);
        $('body').on('click', '#btnImportAppVehicle', function (e) {
            e.preventDefault();
            var routeId = $(this).data('btnImportAppVehicleRoutePartId');
            var vehicleId = $(this).data('btnImportAppVehicleVehicleId');
            var vehicleName = $(this).data('btnImportAppVehicleVehicleName');
            var applnVehicle = $(this).data('btnImportAppVehicleVehicleName');
            importappVehicle(routeId, vehicleId, vehicleName, applnVehicle);
        });
        $('body').on('click', '#btnImportVehicle', function (e) {
            e.preventDefault();
            var vehicleId = $(this).data('btnImportVehicleVehicleId');
            var vehicleName = $(this).data('btnImportVehicleVehicleName');
            var vrList = $(this).data('btnImportVehicleVrList');
            ImportVehicle(vehicleId, vehicleName, vrList);
        });

    });
    function BactoGen() {
        $('#tab_3').html('');
        $('#generalDetailDiv').show();
    }
