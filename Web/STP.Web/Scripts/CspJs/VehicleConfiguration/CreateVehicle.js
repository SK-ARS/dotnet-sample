    SelectMenu(5);
    $(document).ready(function () {       
        LoadSelectVehicleComponentV1();
        startAnimation();
        //VehicleOverviewPage(20165);
        @if (IsEdit == 1)
        {
            foreach (var compId in Model.OrderBy(s => s.LatPosn).ThenBy(s => s.LongPosn))
            {
                 @:ImportComponentToConfig(@compId.ComponentId);
                 
             }

         }
        stopAnimation();

        $("#back_btn_inVehiclePage").on('click', BackButtonVehiclePagefn);
        $("#vehicle_back_btn").on('click', PreviousPage);
        $("#vehicle_config_assessment_btn").on('click', VehicleConfigPage);
        $("#vehicle_save_btn").on('click', SaveVehicle);
    });

    document.getElementById("scoll-right-btn").addEventListener("click", function () {
        $('#widgetsContent').animate({ scrollLeft: "+=300px" }, "medium");
    });

    document.getElementById("scoll-left-btn").addEventListener("click", function () {
        $('#widgetsContent').animate({ scrollLeft: "-=300px" }, "medium");
    });
   

