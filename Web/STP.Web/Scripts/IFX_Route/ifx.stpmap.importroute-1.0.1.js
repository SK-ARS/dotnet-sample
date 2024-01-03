var PageFlag = $('#Pageflag').val(); // PageFlag for So app and VR1 app
var AppRouteType = "planned";
var origin = "";
//function for document ready()
function RouteDetailsReady(returnlegflag) {
    origin = $('#origin').val();
    PageFlag = $('#Pageflag').val(); // PageFlag for So app and VR1 app
    if ($('#Routetype').val() == 'outline') {
        AppRouteType = $('#Routetype').val();
    }
    else {
        AppRouteType = "planned";
    }
    var id = $('#RouteID').val() || $('#RouteId').val();
    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        var id = $('#showRouteIdMap').val();
    }
    
    if (PageFlag == 1 || PageFlag == 3 || PageFlag == 2)
        if (id == 0 || String(id) == "Undefined")
            id = $('#showRouteIdMap').val(); ;
    if (id == !null || id > 0) {
        if (PageFlag == 1 || PageFlag == 3 || PageFlag == 2)
            AppRouteType = $("#HfRouteType").val();
        if (typeof AppRouteList !== 'undefined') {
            var routeexistingFlag = 0;
            for (var i = 0; i < AppRouteList.length - 1; i++) {
                if (AppRouteList[i].RouteID == id) {
                    routeexistingFlag = 1;
                    break;
                }
            }
            if (routeexistingFlag == 0 && AppRouteList.length > 0) {
                id = AppRouteList[AppRouteList.length - 1].RouteID
            }
        }
        setRouteID(id);
        try {
            if ($('#IsNotif').val().toLowerCase() == "true" && $('#PortalType').val() == '696001') {
                document.getElementById('Notificationwarndiv').style.display = "block";
            }
        }
        catch (err) {}
        ShowForUpdate(id, returnlegflag);
    }
    else {
        setRouteID(0);
        if (PageFlag == 2) // for So app
            loadmap('SOA2BPLANNING', null);
        else if (PageFlag == 3) // for VR1 app
            loadmap('A2BPLANNING', null);
        else if (PageFlag == 1 || PageFlag == 3) // for VR1 app
            loadmap('A2BPLANNING', null);
        else {
            loadmap('A2BPLANNING', null, returnlegflag);
        }
    }
}
//var pageFlag = 0;
function ShowForUpdate(id, returnlegflag) {
    var VIsNEN = false;
    if ($("#hIs_NEN").val() == "true")
        VIsNEN = true;
    var routePart = null;
    var variable = "RouteID";
    if (PageFlag == 2 || PageFlag == 1 || PageFlag == 3) {

        variable = "plannedRouteId";
    }
    var IsAuthorizeMovementGeneral = false;
    if ($('#AuthorizeMovementGeneral').length > 0 && $('#AuthorizeMovementGeneral').val().toLowerCase() == "true")
        IsAuthorizeMovementGeneral = true;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "../Routes/GetPlannedRoute",
        data: { RouteID: id, routeType: AppRouteType, IsNEN: VIsNEN, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView(), IsAuthorizeMovementGeneral: IsAuthorizeMovementGeneral },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            for (var x = 0; x < result.result.routePathList.length; x++) {
                result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
            }
            stopAnimation();
            routePart = result.result;

            //Check the route is broken or not
            if ($('#hf_IsPlanMovmentGlobal').length > 0) {
                CheckIsBroken({ RoutePartId: id, IsReplanRequired: false, IsViewOnly: true }, function (response) {
                    if (response && response.Result && response.Result.length > 0) { //check in the existing route is broken
                        var res = response.Result[0];
                        if (res.IsBroken > 0 && res.IsReplan == 1) 
                            $('#btn_re_plan').show();
                    }
                });
            }
            
        }
    }).done(function (result) {
        
        if (routePart != null && (routePart.routePathList.length > 0 || routePart.routePathList.Count > 0)) {

            if (routePart.routePathList[0].routePointList.length > 0) {
                var fromIndex = -1, toIndex = -1;
                for (var i = 0; i < routePart.routePathList[0].routePointList.length; i++) {
                    if (fromIndex == -1 && routePart.routePathList[0].routePointList[i].pointType == 0) {
                        $("#From_location").val(routePart.routePathList[0].routePointList[i].pointDescr);
                        fromIndex = i;
                    }
                    if (toIndex == -1 && routePart.routePathList[0].routePointList[i].pointType == 1) {
                        $("#To_location").val(routePart.routePathList[0].routePointList[i].pointDescr);
                        toIndex = i;
                    }
                }
                if ((fromIndex != -1 && toIndex != -1) && (routePart.routePathList[0].routePointList[fromIndex].pointDescr != "" || routePart.routePathList[0].routePointList[toIndex].pointDescr != "")) {
                    var RName = routePart.routePartDetails.routeName + " " + routePart.routePathList[0].routePointList[fromIndex].pointDescr + " to " + routePart.routePathList[0].routePointList[toIndex].pointDescr;
                    if (RName.length > 130) {
                        RName = RName.substring(0, 129);
                        RName = RName + "...";
                    }
                    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
                        var routeId = $('#showRouteIdMap').val();;
                        $('.EditRouteCLS').removeClass('active-route');
                        $('.EditRouteCLS[data-routeid=' + routeId + ']').addClass('active-route');
                    }
                    else {
                        $("#pageheader").find("h3").text(RName);
                        $("#RoteHeading").text(routePart.routePartDetails.routeName);//ESDAL4 2022
                    }
                }
            }

            var array = null;
            try {
                    array = routePart.routePathList.length > 0 ? routePart.routePathList[0].routeSegmentList[0].routeLinkList : null;
            }
            catch (e) {
            }

            if (array == null || array == "null" || array == "[]") {
                var routeSegmentCount = 0;
                routeSegmentCount = routePart.routePathList[0].routeSegmentList.length;
                if (routePart.routePathList[0].routePointList.length > 0 && routePart.routePathList[0].routePointList[0].pointGeom != null) {
                    if (PageFlag == 2 || PageFlag == 0) {
                        if (AppRouteType == "outline" || routeSegmentCount == 0) {
                            if ($('#PortalType').val() == 696008) {
                                loadmap('SOA2BPLANNING', result.result);
                            }
                            else {
                                loadmap('A2BPLANNING', result.result);
                            }
                            //loadmap('A2BPLANNING');
                            //showSketchedRoute(result.result);
                            addWaypoint(0, routePart);
                            $('#btn_clear').show();

                        }
                        else {
                            loadmap('A2BPLANNING', routePart);
                            addWaypoint(0, routePart);
                            $("#btn_saveRoute").show();
                        }
                        // showOutlineRoute(routePart);
                    }
                    else {
                        if (routePart.routePartDetails.routeType == "outline") {
                            loadmap('A2BPLANNING');
                            showSketchedRoute(result.result);
                            addWaypoint(0, routePart);
                            $('#btn_clear').show();
                        }
                        else {
                            loadmap('A2BPLANNING', routePart);
                            // showSketchedRoute(routePart);
                            addWaypoint(0, routePart);
                        }
                    }
                }
                else {
                    loadmap('A2BPLANNING', null);
                }
            }
            else {
                if (PageFlag == "U" && origin == "Lib") {
                    loadmap('ROUTELIBRARY', routePart);
                    addWaypoint(0, routePart);
                }
                else if (PageFlag == "U" && origin == "App") {
                    loadmap('A2BPLANNING', routePart, returnlegflag);
                    addWaypoint(0, routePart);
                }
                else if ((PageFlag == 2 || PageFlag == 4) && AppRouteType.toLowerCase() == "planned") {
                    if ($('#CandidateModifyflag').val() == 'False') {
                        loadmap('DISPLAYONLY', routePart, null, true);
                    }
                    else {
                        loadmap('SOA2BPLANNING', routePart, null, true);
					}
                    addWaypoint(0, routePart);
                }
                else if ($('#IsAgreedNotify').val() == "True" || $('#IsAgreedNotify').val() == "true") {
                    //var deactivateControls = "olControlDrawFeature,olControlDragFeature,olControlSelectFeature,olControl,maptoolbarpanel horizontalMap-center";
                    //DeactivateControls(deactivateControls);
                    loadmap('DISPLAYONLY', routePart, null, true);
                }
                else if ($('#IsCandVersion').val()=="True") {
                    if (($('#CheckerStatus').val() == '301002' && $('#SortUserId').val() != $('#CheckerId').val()) || $('#CheckerStatus').val() == '301003' ) {
                        loadmap('DISPLAYONLY', routePart);
                        
                    }
                    else {
                        loadmap('A2BPLANNING', routePart);
                        
                    }
                    addWaypoint(0, routePart);
                }
                else {
                    loadmap('A2BPLANNING', routePart);
                    addWaypoint(0, routePart);
                }
            }
            if (routePart.routePathList.length > 1) {
                for (var i = 2; i <= routePart.routePathList.length; i++) {
                    var x = "Path" + i;
                    $(".root").append("<li><a class='active'>" + x + "</a></li>");
                }
                clear_selection();
                $(".account").html('Path1');
                $(".root > li:first > a").addClass("active");
                $('.dropdown11').show();
            }

        }
        else {
            loadmap('A2BPLANNING', null);
			//Uncomment with conditions when map upgrade is up.
            //ShowErrorPopup('Unfortunately this route is incomplete due to the map upgrade and cannot be used - please re-create the route and save to library');
        }
    }).fail(function (error, a, b) {
    }).always(function (xhr) {
        if (typeof mapInputValuesGlobal != 'undefined') {
            mapInputValuesGlobal = getMapInputValues();
        }
    });

}

function close_alert() {
    $("#dialogue").hide();
    $("#pop-warning").hide();
    $("#overlay").hide();
    return false;
}