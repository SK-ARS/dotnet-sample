    $(document).ready(function () {
        FillComponentFav();

        $(".a_createComponent").on('click', CreateComponentForVehicle);
        $(".a_selectComponent").on('click', SelectVehiclecomponentFromFleet);
        $("#BoatMastException").on('change', AssessConfigType);
    });

