var userTypeIDVal = $('#hf_userTypeID').val();
var RouteFlagVal = $('#hf_RouteFlag').val();
$(document).ready(function () {
    ViewAppGeneralDetails();
});
function OnBackButtonClick() {

    if (userTypeIDVal == 696008) {
        window.location.href = '/SORTApplication/SORTInbox';
    }
    else {
        window.location.href = '/Movements/MovementList';
    }
}

function ViewAppGeneralDetails() {
    if ($('#viewappgeneral').is(":visible")) {
        $('#viewappgeneral').css("display", "none");
        $('#chevlon-up-icon1').css("display","none");
        $('#chevlon-down-icon1').css("display","block");
    }
    else {
        $('#viewappgeneral').css("display","block");
        $('#chevlon-up-icon1').css("display","block");
        $('#chevlon-down-icon1').css("display","none");
    }
}
function ViewSupplimentaryDetails() {
    if ($('#viewsuplimentaryinfo').is(":visible")) {
        $('#viewsuplimentaryinfo').css("display","none");
        $('#chevlon-up-icon2').css("display","none");
        $('#chevlon-down-icon2').css("display", "block");
    }
    else {
        $('#viewsuplimentaryinfo').css("display","block");
        $('#chevlon-up-icon2').css("display","block");
        $('#chevlon-down-icon2').css("display","none");
    }
}
function ViewRouteAndVehiclesVA(ComponentCntrId) {
    if ($(ComponentCntrId).is(":visible")) {
        $(ComponentCntrId).css("display","none");
        $('#rou_veh_chevlon-up-icon3').css("display","none");
        $('#rou_veh_chevlon-down-icon3').css("display","block");
    }
    else {
        $(ComponentCntrId).css("display","block");
        $('#rou_veh_chevlon-up-icon3').css("display","block");
        $('#rou_veh_chevlon-down-icon3').css("display","none");
    }
}
function ViewRouteVehicles(RoutePartId) {
    var ComponentCntrId = "#viewroutedetails_" + RoutePartId;
    if ($(ComponentCntrId).is(":visible")) {
        $(ComponentCntrId).css("display","none");
        $('#chevlon-up-icon_' + RoutePartId).css("display","none");
        $('#chevlon-down-icon_' + RoutePartId).css("display","block");
    }
    else {
        $(ComponentCntrId).css("display","block");
        $('#chevlon-up-icon_' + RoutePartId).css("display","block");
        $('#chevlon-down-icon_' + RoutePartId).css("display","none");
    }
}
function ViewVehicleDetails(VehicleId) {
    var ComponentCntrId = "#viewcomponentdetails_" + VehicleId;
    if ($(ComponentCntrId).is(":visible")) {
        $(ComponentCntrId).css("display","none");
        $('#chevlon-up-icon_' + VehicleId).css("display","none");
        $('#chevlon-down-icon_' + VehicleId).css("display","block");
    }
    else {
        $(ComponentCntrId).css("display","block");
        $('#chevlon-up-icon_' + VehicleId).css("display","block");
        $('#chevlon-down-icon_' + VehicleId).css("display","none");
    }
}
function ViewRouteOnMap(RouteId, RouteType) {
    $.ajax({
        url: '../Routes/GetSoRouteDescMap',
        type: 'post',
        data: { plannedRouteId: RouteId, routeType: RouteType },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (typeof result.result.routePathList[0].routeSegmentList != "undefined" && String(typeof result.result.routePathList[0].routeSegmentList) != "[]") {
                var viewRouteCntr = '#viewmap_' + RouteId;
                $(viewRouteCntr).html('');
                $(viewRouteCntr).show();
                $(viewRouteCntr).load('../Routes/A2BPlanning?routeID = 0', function () {
                    if (RouteType == "outline") {
                        loadmap('DISPLAYONLY');
                        showSketchedRoute(result.result);
                    }
                    else {
                        $("#map").html('');
                        if (RouteFlagVal == 2)
                            loadmap('DISPLAYONLY', result.result, null, false);
                        else if (RouteFlagVal == 4)
                            loadmap('DISPLAYONLY', result.result, null, true);
                        else
                            loadmap('DISPLAYONLY', result.result);
                    }
                });
            }
            else {
                $(viewRouteCntr).load('/Routes/A2BPlanning?routeID=0', function () {

                    loadmap('DISPLAYONLY');
                    showSketchedRoute(result.result);
                });
            }
            $('#viewroutedetails_' + RouteId).show();
            $('#viewRouteBtn_' + RouteId).hide();
            $("#map").css("height !important", 450);
        },
        error: function (jqXHR, textStatus, errorThrown) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}
