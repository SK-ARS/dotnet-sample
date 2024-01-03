      $(document).ready(function () {
          ViewAppGeneralDetails();
    });
    function OnBackButtonClick() {
       
        if ('@userTypeID' == 696008) {
            window.location.href = '/SORTApplication/SORTInbox';
        }
        else {
            window.location.href = '/Movements/MovementList';
        }
    }

    function ViewAppGeneralDetails() {
        if (document.getElementById('viewappgeneral').style.display !== "none") {
            document.getElementById('viewappgeneral').style.display = "none"
            document.getElementById('chevlon-up-icon1').style.display = "none"
            document.getElementById('chevlon-down-icon1').style.display = "block"
        }
        else {
            document.getElementById('viewappgeneral').style.display = "block"
            document.getElementById('chevlon-up-icon1').style.display = "block"
            document.getElementById('chevlon-down-icon1').style.display = "none"
        }
    }
    function ViewSupplimentaryDetails() {
        if (document.getElementById('viewsuplimentaryinfo').style.display !== "none") {
            document.getElementById('viewsuplimentaryinfo').style.display = "none"
            document.getElementById('chevlon-up-icon2').style.display = "none"
            document.getElementById('chevlon-down-icon2').style.display = "block"
        }
        else {
            document.getElementById('viewsuplimentaryinfo').style.display = "block"
            document.getElementById('chevlon-up-icon2').style.display = "block"
            document.getElementById('chevlon-down-icon2').style.display = "none"
        }
    }
    function ViewRouteAndVehicles(ComponentCntrId) {
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('rou_veh_chevlon-up-icon3').style.display = "none"
            document.getElementById('rou_veh_chevlon-down-icon3').style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('rou_veh_chevlon-up-icon3').style.display = "block"
            document.getElementById('rou_veh_chevlon-down-icon3').style.display = "none"
        }
    }
    function ViewRouteVehicles(RoutePartId) {
        var ComponentCntrId = "viewroutedetails_" + RoutePartId;
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + RoutePartId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + RoutePartId).style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + RoutePartId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + RoutePartId).style.display = "none"
        }
    }
    function ViewVehicleDetails(VehicleId) {
        var ComponentCntrId = "viewcomponentdetails_" + VehicleId;
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "none"
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
                    $(viewRouteCntr).load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                            if (RouteType == "outline") {
                                loadmap('DISPLAYONLY');
                                showSketchedRoute(result.result);
                            }
                            else {
                                $("#map").html('');
                                if ('@Session["RouteFlag"]' == 2)
                                    loadmap('DISPLAYONLY', result.result, null, false);
                                else if ('@Session["RouteFlag"]' == 4)
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
