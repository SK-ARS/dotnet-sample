$(document).ready(function () {

   /* $("#btnBactoGen").on('click', BactoGen);*/

    $('body').on('click', '#btnBactoGenVeh', function (e) {
        e.preventDefault();
        BactoGen(this);
    });
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
        var vehicleId = $(this).data('btnimportvehiclevehicleid');
        var vehicleName = $(this).data('btnimportvehiclevehiclename');
        var vrList = $(this).data('btnimportvehiclevrlist');
        ImportVehicle(vehicleId, vehicleName, vrList);
    });

});





function BactoGen() {
    $('#tab_3').html('');
    $('#generalDetailDiv').show();
}
