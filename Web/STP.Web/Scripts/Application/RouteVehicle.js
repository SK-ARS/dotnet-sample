var CountVal;
var RouteIdVal;

function RouteVehicleInit() {
    CountVal = $('#hf_Count').val();
    RouteIdVal = $('#hf_RouteId').val();
    if (CountVal == 1) {
        $('#rbtn_' + RouteIdVal).attr('checked', true);
        GetRouteDetails(RouteIdVal);
    }
}
$(document).ready(function () {
    $('body').on('click', '.getroutedetails', function (e) {
        GetRoutedetails(this);
    });
});
function GetRoutedetails(e) {
    var arg1 = $(e).attr("arg1");
    GetRouteDetails(arg1);
}
function GetRouteDetails(RoutePartId) {
    var isHistoric = $('#Historical').length > 0 ? $("#Historical").val() : 0;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetRouteVehicleDetails',
        data: { routePartId: RoutePartId, isHistory: isHistoric },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            var routeDetails = result.routeDetails;
            var vehicleDetails = result.vehicleDetails;
            $("#agreedRouteMap").html('');
            $("#idstrDesc").html('');
            $("#idEndDesc").html('');
            $("#idViaWay").html('');

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
                createContextMenu();
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
            });
            for (var i = 0; i < vehicleDetails.length; i++) {
                VehicleDetails1(vehicleDetails[i].VehicleId);
            }
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function VehicleDetails1(VehicleId) {
    $('#route_vehicle').html('');
    $.ajax({
        type: 'POST',
        url: '../VehicleConfig/ViewConfigurationGeneral?vehicleID=' + VehicleId + '&flag=vr1' + '&isOverviewDisplay='+ true,
        data: {},
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            $('#vehicleList').show();
            $(result).appendTo('#route_vehicle');
            $('#route_vehicle').find('#route-entry1').removeClass('pt-8');
            ViewConfigurationGeneralInit();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
