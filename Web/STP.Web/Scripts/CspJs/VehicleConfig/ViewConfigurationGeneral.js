    $(document).ready(function () {
if($('#hf_IsOverviewDisplay').val() ==  'True') {            
             VehicleDetailsShowHide('@ViewBag.OverviewDisplayVehicleId');
        }
        $(".VehicleShowHide").on('click', VehicleShowHide);
    });
    function VehicleDetailsShowHide(displayToggleVehicleId) {

        var overviewDisplayDiv = "applnOverviewDisplayDetails_" + displayToggleVehicleId;
        if (document.getElementById(overviewDisplayDiv).style.display !== "none") {
            document.getElementById(overviewDisplayDiv).style.display = "none"
            document.getElementById('chevlon-up-icon_overviewDisplayDiv_' + displayToggleVehicleId).style.display = "none"
            document.getElementById('chevlon-down-icon_overviewDisplayDiv_' + displayToggleVehicleId).style.display = "block"
            $('#applnOverviewDisplay_' + displayToggleVehicleId).text("Show Details");
        }
        else {
            document.getElementById(overviewDisplayDiv).style.display = "block"
            document.getElementById('chevlon-up-icon_overviewDisplayDiv_' + displayToggleVehicleId).style.display = "block"
            document.getElementById('chevlon-down-icon_overviewDisplayDiv_' + displayToggleVehicleId).style.display = "none"
            $('#applnOverviewDisplay_' + displayToggleVehicleId).text("Hide Details");
        }
    }
    function VehicleShowHide(e) {
        var vehicleId = e.currentTarget.dataset.VehicleId;
        VehicleDetailsShowHide(vehicleId);
    }
