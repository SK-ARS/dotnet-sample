var routeId;
var routeListCount;
var IsHistory;

function DisplayNotificationRouteVehicleInit() {
    routeId = $('#hf_RouteId').val();
    routeListCount = $('#hf_RouteListCount').val();
    IsHistory = $('#hf_IsHistory').val();
    if (routeListCount == 1) {
        $('#rbtn_' + routeId).attr('checked', true);
        GetRouteVehicleDetails(routeId);
    }
}

$(document).ready(function () {
    $('body').on('click', '.getRouteVehicleDetails', function (e) {
        var RoutePartId = $(this).attr("routepartid");
        GetRouteVehicleDetails(RoutePartId);
    });
});

function GetRouteVehicleDetails(RoutePartId) {

    $("#route_vehicle").html('<span id="vehicle_placeholder" class="blink_span" style="color:#000;padding:10px;">Loading vehicle configurations...</span>');
    var ishist = IsHistory;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetRouteVehicleDetails',
        data: { routePartId: RoutePartId, isHistory: ishist },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            for (var x = 0; x < result.routeDetails.routePathList.length; x++) {
                result.routeDetails.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
            }
            var routeDetails = result.routeDetails;
            var vehicleDetails = result.vehicleDetails;
            $("#agreedRouteMap").html('');
            $("#idstrDesc").html('');
            $("#idEndDesc").html('');
            $("#idViaWay").html('');

            for (var i = 0; i < vehicleDetails.length; i++) {
                VehicleDetails(vehicleDetails[i].VehicleId);
            }
            $("#agreedRouteMap").show();
            $("#route_address_sectn").show();
            if (routeDetails.routePartDetails.routeDescr != null) {
                $('#trHeaderDescription1').show();
                $('#trdesc1').show();
                $('#RouteDesc1').html(routeDetails.routePartDetails.routeDescr);
            }
            $('#RoutePartsDescription').show();
            $("#agreedRouteMap").load('../Routes/A2BPlanning?routeID=0', function () {
                $("#map").addClass("context-wrap olMap");
                loadmap('DISPLAYONLY', routeDetails);
                var count = -1, strTr, Index = 0;
                if (routeDetails != null)
                    count = routeDetails.routePathList[0].routePointList.length;
                for (var i = 0; i < count; i++) {
                    if (routeDetails.routePathList[0].routePointList[i].pointType == 0)
                        $('#idstrDesc').html(routeDetails.routePathList[0].routePointList[0].pointDescr);
                    else if (routeDetails.routePathList[0].routePointList[i].pointType == 1)
                        $('#idEndDesc').html(routeDetails.routePathList[0].routePointList[1].pointDescr);
                    else if (routeDetails.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + routeDetails.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#idViaWay').append(strTr);
                    }
                }
                A2BPlanningInit();
            });

        },
        complete: function () {
            stopAnimation();
        }
    });
}

function VehicleDetails(VehicleId) {
    $.ajax({
        type: 'POST',
        url: '../VehicleConfig/ViewConfigurationGeneral?vehicleID=' + VehicleId + '&isOverviewDisplay=' + true,
        data: {},
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            if ($('#vehicle_placeholder').length > 0)
                $('#vehicle_placeholder').remove();
            $(result).appendTo('#route_vehicle');
            ViewConfigurationGeneralInit(VehicleId);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
