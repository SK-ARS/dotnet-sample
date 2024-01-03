    SelectMenu(5);
    $(document).ready(function () {
        $("#ComponentType").on('change', VehicleTypeChangeFn);
        $("#VehicleSubType").on('change', VehicleSubTypeChangeFn);
        $("#movementTypeId").on('change', VehicleClassificationChangeFn);
    });

    function VehicleTypeChangeFn(e) {
        VehicleTypeChange(e);
    }
    function VehicleSubTypeChangeFn(e) {
        VehicleSubTypeChange(e);
    }
    function VehicleClassificationChangeFn(e) {
        VehicleClassificationChange(e);
    }
