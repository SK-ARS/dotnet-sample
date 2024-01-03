var hf_ComponentId;
$(document).ready(function () {
    GeneralVehicCompInit();
    $('body').on('click', '.viewComponentDetail', function (e) {
        e.preventDefault();
        ViewComponentDetails(this);
    });
});

function GeneralVehicCompInit() {
    hf_ComponentId = $('#hf_ComponentId').val();
    if ($('#hf_mode').val() == 'view') {
        VehicleComponentDetails(hf_ComponentId);
    }
}
function VehicleComponentDetails(componentid) {
    var targetElem = "veh_component_details_" + componentid;
    if ($('#' + targetElem).is(":visible")) {
        $('#' + targetElem).css("display", "none");
        $("#chevlon-up-icon_" + componentid).css("display", "none");
        $("#chevlon-down-icon_" + componentid).css("display", "block");
        $('#spnDetailStatus_' + componentid).text("Show Details");
    }
    else {
        $('#' + targetElem).css("display", "block");
        $("#chevlon-up-icon_" + componentid).css("display", "block");
        $("#chevlon-down-icon_" + componentid).css("display", "none");
        $('#spnDetailStatus_' + componentid).text("Hide Details");
    }
}

function ViewComponentDetails(e) {
    var componentId = $(e).attr("componentid");
    VehicleComponentDetails(componentId);
}
