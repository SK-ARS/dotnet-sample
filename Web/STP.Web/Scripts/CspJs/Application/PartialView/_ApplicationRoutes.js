    $(document).ready(function () {
        $("#ViewRouteOnMap").on('click', ViewRouteOnMapFunction);
        $(".viewrouteparts").on('click', viewroutepartsFunction);
    });

    function ViewRouteOnMapFunction(data) {
        var RoutePartId = data.currentTarget.attributes.RoutePartId.value;
        var RouteType = data.currentTarget.attributes.RouteType.value;
        ViewRouteOnMap(RoutePartId, RouteType);
    }
    function viewroutepartsFunction(data) {
        var RoutePartNo = data.currentTarget.attributes.RoutePartNo.value;
        viewrouteparts(RoutePartNo);
    }

