    $(document).ready(function () {
        $(".viewComponentDetail").on('click', ViewComponentDetails);
    });
    function VehicleComponentDetails(VehicleCompId) {
        if (document.getElementById(VehicleCompId).style.display !== "none") {
            document.getElementById(VehicleCompId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + VehicleCompId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + VehicleCompId).style.display = "block"
            $('#spnDetailStatus_' + VehicleCompId).text("Show Details");
        }
        else {
            document.getElementById(VehicleCompId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + VehicleCompId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + VehicleCompId).style.display = "none"
            $('#spnDetailStatus_' + VehicleCompId).text("Hide Details");
            HeaderHeight(VehicleCompId);
        }
    }
    function ViewComponentDetails(e) {
        var componentId = e.currentTarget.dataset.ComponentId;
        VehicleComponentDetails(componentId);
    }
