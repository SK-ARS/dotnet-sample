
    $(document).ready(function () {
        $("#btn_back_config").on('click', OnBackBtnClick);
        $("#btn_saveConfig").on('click', SaveVehicleConfigurationFn);
        $("#VehicleTypeConfig").on('change', movementNext);
    });
    function SaveVehicleConfigurationFn(e) {
        SaveVehicleConfiguration(e);
    }

