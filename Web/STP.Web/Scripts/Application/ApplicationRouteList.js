var RoutePartId;
var RouteType;

function ApplicationRouteListInit() {
    RoutePartId = $("#hf_RoutePartId_RList").val();
    RouteType = $('#RouteType').val();
    var radioButtonId = '#rdb_' + '' + RoutePartId + '';
    $(radioButtonId).attr('checked', true);
    if (RoutePartId != "") {
        ViewRouteOnMap(RoutePartId, RouteType, true);
    }
    if ($('#hf_IsPlanMovmentGlobal').length > 0)
        CreateLocalObject(StepFlag, true);
    stopAnimation();
}
$(document).ready(function () {
    $('body').on('click', '.btn-arl-view-route-on-map', function () {
        var RouteID = $(this).data('routeid');
        var RouteType = $(this).data('routetype');
        var radioButtonId = '#rdb_' + '' + RouteID + '';
        $(radioButtonId).attr('checked', true);
        ViewRouteOnMap(RouteID, RouteType);
    });

    $('body').on('change', '#gpxRoute', function () {
        ViewGPXRoute();
    });

    $('body').on('click', '.app-route-vehicle-view', function (e) {
        ViewRouteAndVehicles("viewroutes");
    });
});
function ViewGPXRoute() {
    var routeId = $("#RouteId").val();
    var revisionId = $('#ApplicationrevId').val();
    var movementType = 207001;
    if ($('#VR1Applciation').val() == 'True') {
        movementType = 207002;
    }
    if ($("#gpxRoute").prop('checked') == true) {
        //New function to show GPX geometry using Geoserver
        ShowGpxRoute(revisionId, routeId, movementType);
    }
    else if ($("#gpxRoute").prop('checked') == false) {
        //New function to hide GPX geometry using Geoserver
        HideGpxRoute(revisionId, routeId);
    }
    
}
function ViewRouteOnMap(RouteId, RouteType, hideRoute = false) {
    if ($('#hauliermnemonic').val() == 'NEA') {
        $("#showGPXRoute").show();
        $("#movementMapFilterIcon").addClass('mapFilterIconMargin');
    }
    $("#RouteId").val(RouteId); 
    var isHistoric = $("#Historical").length > 0 ? $("#Historical").val() : 0;
    $.ajax({
        url: '../Routes/GetRouteVehicleDetails',
        type: 'post',
        data: { routePartId: RouteId, routeType: RouteType, isHistory: isHistoric},
        beforeSend: function () {
            //startAnimation();
            openContentLoader('#appRouteMap');
        },
        success: function (result) {
            var routeDetails = result.routeDetails;
            var vehicleDetails = result.vehicleDetails;
            $("#appRouteMap").show();
            $("#appRouteMap").html('');
            $('#route_vehicle').html('');
            $("#appRouteMap").load('../Routes/A2BPlanning?routeID=0', function () {
                $("#map").addClass("context-wrap olMap");
                A2BPlanningInit();
                var count = -1, strTr, Index = 0;
                if (routeDetails != null)
                    count = routeDetails.routePathList[0].routePointList.length;
                $('#RouteAdd').show();
                $('#StartingAdd').html('');
                $('#EndingAdd').html('');
                $('#TabviaAdd').html('');
                for (var i = 0; i < count; i++) {
                    if (routeDetails.routePathList[0].routePointList[i].pointType == 0)
                        $('#StartingAdd').html(routeDetails.routePathList[0].routePointList[i].pointDescr);
                    else if (routeDetails.routePathList[0].routePointList[i].pointType == 1)
                        $('#EndingAdd').html(routeDetails.routePathList[0].routePointList[i].pointDescr);
                    else if (routeDetails.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + routeDetails.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#TabviaAdd').append(strTr);
                    }
                }
                if (hideRoute == true) {
                    document.getElementById('viewroutes').style.display = "block"
                }
                if (RouteType == 'outline') {
                    loadmap('DISPLAYONLY');
                    showSketchedRoute(routeDetails);
                }
                else {
                    loadmap('DISPLAYONLY', routeDetails);
                }
                if (hideRoute == true) {
                    document.getElementById('viewroutes').style.display = "none"
                }
            });
            for (var i = 0; i < vehicleDetails.length; i++) {
                VehicleDetails(vehicleDetails[i].VehicleId);
            }
        }
    });
}
function VehicleDetails(VehicleId) {
    $('#IsApplication').val(1);
    $('#vehicleList').show();
    $.ajax({
        type: 'POST',
        url: '../VehicleConfig/ViewConfigurationGeneral',
        data: {
            vehicleID: VehicleId,
            isOverviewDisplay: true
        },
        beforeSend: function (xhr) {
            //startAnimation();
            openContentLoader('#vehicleList');
        },
        success: function (result) {
            
            $(result).appendTo('#route_vehicle');
            $('#route_vehicle').find('#route-entry1').removeClass('pt-8');
            ViewConfigurationGeneralInit(VehicleId);
            closeContentLoader('#vehicleList');
        }
    });
}
function ViewRouteAndVehicles(ComponentCntrId) {
    var targetElem = ComponentCntrId;
    if ($('#' + targetElem).is(":visible")) {
        $('#' + targetElem).css("display", "none");
        $("#rou_veh_chevlon-up-icon3").css("display", "none");
        $("#rou_veh_chevlon-down-icon3").css("display", "block");
    }
    else {
        $('#' + targetElem).css("display", "block");
        $("#rou_veh_chevlon-up-icon3").css("display", "block");
        $("#rou_veh_chevlon-down-icon3").css("display", "none");
    }
}