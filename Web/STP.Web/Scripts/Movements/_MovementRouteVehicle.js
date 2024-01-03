$(document).ready(function () {
    $('body').off('click', '.vehicleformovement');
    $('body').on('click', '.btnviewroute', function (e) {
        var RoutePartId = $(this).data("routepartid");
        var RoutePartName = $(this).data("routepartname");
        ViewMapRoute(RoutePartId, RoutePartName);
    });
    $('body').on('click', '#btnImport', function (e) {
        var RoutePartId = $(this).data("routepartid");
        var RouteType = $(this).data("routetype");
        ImportRouteInAppParts(RoutePartId, RouteType);
    });
    $('body').on('click', '.filterbtnimport', function (e) {
        var RoutePartId = $(this).data("routepartid");
        var RouteType = $(this).data("routetype");
        ImportRouteInAppParts(RoutePartId, RouteType);
    });
    $('body').on('click', '.vehicleformovement', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var flag = $(this).data("flag");
        UseVehicleForMovement(vehicleId, flag);
    });
});

