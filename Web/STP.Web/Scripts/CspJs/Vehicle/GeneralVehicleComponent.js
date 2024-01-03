    $(document).ready(function () {
if($('#hf_mode').val() ==  'view') {
            VehicleComponentDetails(@ViewBag.ComponentId);
        }

        $(".viewComponentDetail").on('click', ViewComponentDetails);
    });
    function VehicleComponentDetails(VehicleCompId) {
        var ComponentCntrId = "veh_component_details_" + VehicleCompId;
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + VehicleCompId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + VehicleCompId).style.display = "block"
            $('#spnDetailStatus_' + VehicleCompId).text("Show Details");
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + VehicleCompId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + VehicleCompId).style.display = "none"
            $('#spnDetailStatus_' + VehicleCompId).text("Hide Details");
        }
    }
    function ViewComponentDetails(e) {
        var componentId = e.currentTarget.dataset.ComponentId;
        VehicleComponentDetails(componentId);
    }
