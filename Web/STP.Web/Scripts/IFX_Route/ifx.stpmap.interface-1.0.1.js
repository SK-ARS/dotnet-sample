var objifxStpMap;
var objifxStpmapStructures;
var objifxStpMapRoadDelegation;
var objifxStpmapRoadOwnership;
var pageType;
var annotPosition;
var currentMouseOverFeature = null;
var mapfromroutesmenu = false;
var mapcontextMenuOn = false;
var timer;
var returnLeg = false;
var showRouteAssessment = true;
var isViewfromNotif = false;//flag added for indicating the view is from notification 'view map' click
var drawManouevre = null;
var notifrouteedit = false
var mapSearcheasting =0;
var mapSearchnorthing = 0;
var mapsearchflag = 0;
var mapsearchtrigger = 0;
var mapaddresssearch = "";
var InfRtName="";
var tempAnnotations = [];
var viewEditRouteFlagStructures = 0;
var viewEditRouteFlagConstraints = 0;
var vieweditflagStruct = 0;
var vieweditflagconst = 0;
var defaultx = 525168;
var defaulty = 170100;
var _checkerstatus = $('#CheckerStatus').val();
var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
var plannr_UserId = $('#PlannrUserId').val() ? $('#PlannrUserId').val() : 0;
var Ismapenlarged = 0;
var AfStArray = [];
var AfConArray = [];
function setRouteID(id) {
    routeId = id;
}
function setRouteNameEdit(name) {
    InfRtName = name;
}
function getRouteID() {
    return routeId;
}

function resizeMap(width, height) {
    if (objifxStpMap != undefined)
        objifxStpMap.resizeMap();
}

function fillRoutePart(response) {
}

function resetMapSearch() {
     mapSearcheasting = 0;
    mapSearchnorthing = 0;
    mapaddresssearch = "";
}

function zoomIn() {
    objifxStpMap.zoomIn();
}

function getConstraintDetails() {
    return objifxStpmapStructures.getConstraintDetails();
}



function ConstraintCreationflag(flag, obj) {
    if (flag == true) {
        objifxStpmapStructures.markerConstraint[objifxStpmapStructures.markerConstraint.length] =
            objifxStpmapStructures.addConstraint(obj.constraintId, obj.constraintName, obj.constraintCode, IfxStpmapCommon.getConstraintTypeDescription(obj.constraintType),
                obj.topologyType, obj.constSuitability, obj.geometry.OrdinatesArray);
        objifxStpmapStructures.redrawConstraints();
    }
    else {
        objifxStpmapStructures.clearMarkerConstriants();
    }
}

function ConstraintDeletionflag(flag, constId) {
    if (flag == true) {
        var timerVar = setInterval(function () {
            objifxStpmapStructures.deleteConstraint(constId);
            clearTimeout(timerVar);
        }, 500);
        removePopups();
    }
}

function zoomOut() {
    objifxStpMap.zoomOut();
}

function createAnnotation(pix, type) {
    annotPosition = { x: pix.x, y: pix.y, type: type };
    resetdialogue();
    $("#dialogue").load('../Annotation/CreateAnnotation', function () {
        CreateAnnotationInit();
        if (type == 'offroad') {
            toolbarPanel2.div.style.display = "inline";
        }
        CheckSessionTimeOut();
    });
}

var annotationObject = null;
function getAnnotationObject() {
    return annotationObject;
}

function showAnnotation(pathIndex, segmentIndex, arrIndex) {
    annotationObject = objifxStpMap.routeManager.getAnnotation(pathIndex, segmentIndex, arrIndex);
    resetdialogue();
    $("#dialogue").load('../Annotation/CreateAnnotation?editmode=' + 1, function () {
        CreateAnnotationInit();
        CheckSessionTimeOut();
    });
}

function onAnnotationWindowClose(annotText, annotType, annotationContactList) {
    objifxStpMap.setRoutePointAtXY({ pointType: 'ANNOTATION', pointPos: 0, X: annotPosition.x, Y: annotPosition.y, type: annotPosition.type, searchInBbox: false, Zoomin: false }, function (annotObject) {
        if (annotObject != undefined && annotObject != null) {
            annotObject.annotationContactList = annotationContactList;
            annotObject.annotType = annotType;
            annotObject.annotText = annotText;
            objifxStpMap.addAnnotation(annotObject.otherinfo.pathIndex, annotObject.otherinfo.segmentIndex, -1, annotObject);
            if (objifxStpMap.getCurrentPathState() == 'routedisplayed') {
                objifxStpMap.routeManager.setRoutePathState(objifxStpMap.currentActiveRoutePathIndex, 'routeplanned');
                updateUI();
            }
        }
        else {
            ShowErrorPopup('Failed to create annotation');
        }
        stopAnimation();
    });

}

function IsMapnotidle() {
    try {
        var currentstate = objifxStpMap.getCurrentPathState()
        var isidle = true;
        if (currentstate == 'idle' || currentstate == 'routedisplayed') {
            isidle = false;
        }
        return isidle;
    }
    catch (err) {
        return false;
    }
}

function setCenter(position, bLonLat) {
    objifxStpMap.setCenter(position, bLonLat);
}

function setCenterAndZoom(position, bLonLat) {
    objifxStpMap.setCenterAndZoom(position, bLonLat);
}

function setZoomTo(X, Y, zoom) {
    objifxStpMap.setZoomTo(X, Y, zoom);
}

function setCurrentActiveRoutePath(routePathIndex) {
    objifxStpMap.setCurrentActiveRoutePath(routePathIndex);
}

//fired when dragged a route point marker
function onDragCompleteFn(markerObject, pathIndex, isRp) {
    if (isRp == true) {
        if (tempAnnotations.length == 0) {
            backupAnnotations();
        }
        if (pathIndex == 0 && objifxStpMap.getPathCount() > 1) {
            objifxStpMap.clearAllRoutes(false);
            clearselect();
            updatewaypoints();
        }
        else {
            if (pathIndex == 0)
                objifxStpMap.removeReturnLeg();
            objifxStpMap.clearRoutePath(pathIndex);
        }
        updateUI(markerObject);
        if (markerObject != null && markerObject != undefined &&
            (markerObject.pointType == 0 || markerObject.pointType == 1 || markerObject.pointType == 3))
            ShowQASList(markerObject);
    }
    else {
        if (objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION')
            $('#btnmovsaveannotation').show();
        else
            $('#btn_updateRoute').show();
    }}

function moveRoutePointPos(from, to) {
    objifxStpMap.moveRoutePointPos(objifxStpMap.currentActiveRoutePathIndex, parseInt(from), parseInt(to));
}

function toggleCancelToolBar() {
    var count = objifxStpMap.routeManager.getRoutePathCount();
    var state = objifxStpMap.getCurrentPathState();

    if (state == 'routeplanned' || state == 'routedisplayed') {
        toolbarPanel3.div.style.display = "none";
        if (objifxStpMap.pageType != 'DISPLAYONLY' && objifxStpMap.pageType != 'DISPLAYONLY_EDITANNOTATION' && objifxStpMap.pageType != 'SIMPNOTIF')
            toolbarPanel.div.style.display = "inline";
    }
    else {
        if (state == "idle") {
            toolbarPanel3.div.style.display = "none";
            toolbarPanel4.div.style.display = "none";
        }
        else {
            deactivateToolbarControls();
            toolbarPanel.div.style.display = "none";
            if (count > 1)
                toolbarPanel3.div.style.display = "inline";
        }
    }
}

function hideMnvrCancelToolBar() {
    toolbarPanel4.div.style.display = "none";
}

function updateUI(rpPoint, topologyType) {
    //openRoutenav();
    if (rpPoint != undefined && rpPoint != null && objifxStpMap.pageType != 'NOMAPDISPLAY') {
        if (rpPoint.pointType == 0) {
            $('#From_location').val(rpPoint.pointDescr);
            //  document.getElementById('From_location').value = rpPoint.pointDescr;
        }
        else if (rpPoint.pointType == 1) {
            $('#To_location').val(rpPoint.pointDescr);
            // document.getElementById('To_location').value = rpPoint.pointDescr;
        }
        else if (rpPoint.pointType == 2) {

            var wayptlen = document.getElementsByName("lblwaypoint").length;
            Checkprevptstatus(wayptlen);
            if (rpPoint.routePointNo > document.getElementsByName("lblwaypoint").length) {
                insertwaypoint(2);
            }
            var wp = 'Waypoint' + rpPoint.routePointNo;
            var waypoint = '#' + wp;
            $(waypoint).val(rpPoint.pointDescr);
            //   document.getElementById('Waypoint' + rpPoint.routePointNo).value = rpPoint.pointDescr;
            objifxStpMap.clearRoutePath();
        }
        else if (rpPoint.pointType == 3) {
            var wayptlen = document.getElementsByName("lblwaypoint").length;
            Checkprevptstatus(wayptlen);
            if (rpPoint.routePointNo > document.getElementsByName("lblwaypoint").length) {
                insertwaypoint(3);
            }
            var wp = 'Viapoint' + rpPoint.routePointNo;
            var viapoint = '#' + wp;
            $(viapoint).val(rpPoint.pointDescr);

            //  document.getElementById('Viapoint' + rpPoint.routePointNo).value = rpPoint.pointDescr;
            objifxStpMap.clearRoutePath();
        }
        //if ($("#hIs_NEN").val() == "true") {
        //    remove_highlight_error()
        //}
    }
    var state = objifxStpMap.getCurrentPathState();
    hideMnvrCancelToolBar();
    switch (state) {
        case 'idle':
            switch (objifxStpMap.getPageState()) {
                case 'readyidle':
                    clearselect();
                    $('#From_location').val('');
                    $('#To_location').val('');
                    $('#sortable').html('');
                    $('#spn_WayPoint').hide();
                    $('#spn_ViaPoint').hide();
                    clearAllInPanel();
                    buttonsShowAndHide(2, 2, 2, 2, 2, 2, 2);
                    showToolBar(false, "A2BPLANNING");
                    toggleCancelToolBar();
                    break;
                case 'constraintadded':
                    //resetdialogue();
                    startAnimation();
                    var randomNumber = Math.random();
                    $("#dialogue").load('../Constraint/CreateConstraint?topolgyType=' + topologyType + '&isPartialView=true&random=' + randomNumber, function () {
                        //CheckSessionTimeOut();
                        stopAnimation();
                        CreateConstraintInit();
                    });
                    
                    break;
                case 'constraintbydescription':
                    resetdialogue();
                    $("#dialogue").load('../Constraint/CreateConstraintByDescription', function () {
                        CheckSessionTimeOut();
                    });
                    removescroll();
                    $("#dialogue").show();
                    $("#overlay").show();
                    $("#overlay").css('background', 'none');
                    break;
                case 'planninginprogress':
                    var count = objifxStpMap.routeManager.getRoutePathCount();
                    if (count > 1) {
                        deactivateToolbarControls();
                        toolbarPanel.div.style.display = "none";
                        toolbarPanel3.div.style.display = "inline";
                    }
                    if (objifxStpMap.currentActiveRoutePathIndex > 0) {//alternate route path is active // planning in progress then hide the save button 
                        $('#btn_saveRoute').hide();
                    }
                    break;
            }
            break;
        case 'firstpointselected':
            buttonsShowAndHide(0, 0, 0, 1, 0, 0, 0);
            toggleCancelToolBar();
            hideMnvrCancelToolBar();
            clearAllInPanel();
            break;
        case 'secondpointselected':
            if (objifxStpMap.routeManager.RoutePart.routePartDetails.routeType == 'outline') {
                buttonsShowAndHide(2, 1, 2, 1, 2, 2, 2);
            }
            else {
                if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePathType == 0) {
                    if (mapfromroutesmenu == true)
                        buttonsShowAndHide(1, 2, 0, 0, 0, 2, 1);
                    else
                        buttonsShowAndHide(1, 2, 0, 0, 0, 1, 1);
                    showToolBar(false, objifxStpMap.pageType);
                }
                else {
                    buttonsShowAndHide(1, 2, 0, 0, 0, 2, 2);
                }
                if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
                    $('#callFromViewMap').val(false);//clearing unsitable structures/constraints from map
                }
                clearStructures(0);
                clearConstraints(0);
                clearStructFlags();
                toggleCancelToolBar();
                hideMnvrCancelToolBar();
                $('#spn_WayPoint').show();
                $('#spn_ViaPoint').show();
                btnAffectedStructures.deactivate();
                btnAffectedConstraints.deactivate();
                document.getElementById('AffectedLabel').style.opacity = "0.5";
                document.getElementById('Affected').checked = false;
                document.getElementById('Affected').disabled = true;
            }
            break;
        case 'routeplanned':
            buttonsShowAndHide(2, 1, 2, 1, 2, 2, 2);
            if (objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION')
                $('#btnmovsaveannotation').show();
            if (objifxStpMap.getAdvancedEditPath().pathIndex != -1) {
                buttonsShowAndHide(2, 2, 2, 1, 2, 2, 2, 1);
            }
            if (objifxStpMap.getcurrentSelectedRouteType() != 4) {
                showToolBar(true, "A2BPLANNING");
                toggleCancelToolBar();
                hideMnvrCancelToolBar();
            }
            document.getElementById('AffectedLabel').style.opacity = "1";
            if (showRouteAssessment == true)
                document.getElementById('Affected').disabled = false;
            break;
        case 'routedisplayed':
            if ($("#hIs_NEN").val() == "true") {
                highlight_error();
            }
            clearStructFlags();
            //if ($("#IsRouteReplanned").val() == "true") {
            //    buttonsShowAndHide(2, 1, 2, 1, 2, 2, 2);
            //    $('#IsRouteReplanned').val("false");
            //}
            //else
            buttonsShowAndHide(2, 2, 2, 1, 1, 2, 2);
            deactivateToolbarControls();
            showToolBar(true, "A2BPLANNING");
            $('#spn_WayPoint').show();
            $('#spn_ViaPoint').show();
            if (objifxStpMap.getPathCount() > 1) {
                $('#deletepath').show();
            }
            //showToolBar(true, "ROUTEAPPRAISAL");
            toggleCancelToolBar();
            hideMnvrCancelToolBar();
            document.getElementById('AffectedLabel').style.opacity = "1";
            if (showRouteAssessment == true)
                document.getElementById('Affected').disabled = false;
            if (typeof $("#hIs_NEN").val() != "undefined" && $("#hIs_NEN").val() == "true") {
                if (VObj_err != null) {

                    if (VObj_err.length > 0 && ActionForReturnRout != 1 && document.getElementsByName("lblwaypoint").length == VWaypointCount) {
                        SetCorrectElementOnMap(0);
                    }
                    else if (ActionForReturnRout == 1 && IsReturnRestriction == true) {//swap the route for return leg
                        SetReturnLeg(1);
                    }//this will action only if its a return route and it has restrictions
                }
                else if (IsrestrictionsErrror == true && document.getElementsByName("lblwaypoint").length == VWaypointCount) {
                    if (ActionForReturnRout != 1 && mainRouteLegalRestriction == true)
                    { SetCorrectElementOnMap(0); }//this will action only if its main route
                    //else if (ActionForReturnRout == 1 && IsReturnRestriction == true)
                    //{ SetCorrectElementOnMap(0); }//this will action only if its a return route and it has restrictions 
                    else if (returnRouteError == true && mainRouteCheck == false && ActionForReturnRout == 1) {// If Only return route is Unplanned
                        SetCorrectElementOnMap(0);
                    }
                }
                else if (ActionForReturnRout == 1 && IsReturnPlanned == false && returnRouteError == true && mainRouteCheck == false) {// If Only return route is Unplanned
                    SetCorrectElementOnMap(0);
                }
            }
    }
}

function Checkprevptstatus(len) {
    var name = document.getElementsByName("lblwaypoint");
    var i = len - 1;
    if (len != 0) {
        var str = name[i];
        var id = name[i].innerHTML;
        var value = $('#' + id).val();
        if (value == '')
            $('#sortable').find('li').eq(i).remove();

    }
}
function showToolBar(show, page) {
    if (page == "A2BPLANNING") {
        if (show == false) {
            if (typeof toolbarPanel!='undefined') { toolbarPanel.div.style.display = "none"; }
            if (typeof toolbarPanel2 != 'undefined') { toolbarPanel2.div.style.display = "none"; }
            if (typeof toolbarPanel3!='undefined'){toolbarPanel3.div.style.display = "none";}
            if (typeof toolbarPanel4!='undefined'){toolbarPanel4.div.style.display = "none";}
        }
        else {
            if (showRouteAssessment == true) {
                toolbarPanel2.div.style.display = "inline";
            }

        }
    }
    else if (page == "SOA2BPLANNING") {
        if (show == false) {
            toolbarPanel.div.style.display = "none";
            toolbarPanel2.div.style.display = "none";
            toolbarPanel3.div.style.display = "none";
            toolbarPanel4.div.style.display = "none";
        }
        else {
            if (showRouteAssessment == true)
                toolbarPanel2.div.style.display = "inline";
        }
    }
    else if (page == "STRUCTURES") {
        if (show == false) {
            constraintToolbarPanel.div.style.display = "none";
        }
        else {
            constraintToolbarPanel.div.style.display = "inline";
        }
    }
    else if (page == "ROUTEAPPRAISAL" && objifxStpMap.pageType == "DISPLAYONLY") {
        if (show == false) {
            toolbarPanel2.div.style.display = "none";
        }
        else {
            if (showRouteAssessment == true)
                toolbarPanel2.div.style.display = "inline";
        }
    }
}

function moveWayPointPosition(from, to) {
    if (from == to) return;
    objifxStpMap.moveRoutePointPos(Number(from), Number(to));
    objifxStpMap.clearRoutePath();
    updateUI();
}

function searchSegmentByXY(x, y, pointType) {		// pointType = 0 Start, 1 End, 2 Waypoint, 3 Viapoint, 4 Merge, 5 Diverge, 6 Manoeuvre, 7 Edit start, 8 Edit end
    startAnimation();

    if (pointType == 7 || pointType == 8) {
        var terminalPoint = -1;
        if (objifxStpMap.getPageState() == 'mouseover') {
            var pix = objifxStpMap.olMap.getPixelFromLonLat(new OpenLayers.LonLat(currentMouseOverFeature.geometry.x, currentMouseOverFeature.geometry.y));
            x = pix.x;
            y = pix.y;
            if (pointType == 7) {
                terminalPoint = 0;
            }
            else {
                terminalPoint = 1;
            }
        }

        var val = objifxStpMap.validateEditPoints(x, y, pointType, terminalPoint);
        if (val == false) {
            stopAnimation();
            return 0;
        }
    }

    objifxStpMap.setRoutePointAtXY({ pointType: IfxStpmapCommon.getPointTypeName(pointType), pointPos: -1, X: x, Y: y, searchInBbox: false, Zoomin: false }, function (rpPoint) {

        if (rpPoint != undefined && rpPoint != null) {
            var isAlternate = objifxStpMap.getcurrentSelectedRouteType();
            if (rpPoint.otherinfo.pointfeature.attributes.ROUNDABOUT == 'Y' && isAlternate != 0)//Added by nithin 7/14/2015
                showNotification('Diverge/merge route points cannot be placed on a roundabout: please adjust relevant point(s). For assistance call the Helpdesk on 0300 470 3733');
            if (getCurrentActiveRoutePathIndex() == 0 && objifxStpMap.getPathCount() > 1 && rpPoint.pointType != 9) {
                objifxStpMap.clearAllRoutes(false);
                clearselect();
            }
            updateUI(rpPoint);

            if (rpPoint.pointType == 0 || rpPoint.pointType == 1 || rpPoint.pointType == 3) {
                ShowQASList(rpPoint);
            }
            else {
                stopAnimation();
            }

        }
        else {
            stopAnimation();
        }

    });
}

function selectRoute(routePart) {
    objifxStpMap.setRoutePart(routePart);
}

function backupAnnotations() {
    objifxStpMap.backupAnnotationsinRoute();
}
function showSketchedRoute(routePart) {
    objifxStpMap.drawSketchedRoute(routePart);
}

function showOutlineRoute(routePart) {
    objifxStpMap.drawSketchedRoute(routePart);
}

function showStructures(structureList, otherOrg, page, zoom) {
    return objifxStpmapStructures.showStructures(structureList, otherOrg, page, zoom);
}

function clearStructures(clearAll) {
    objifxStpmapStructures.clearStructures(clearAll);
}

function clearStructuresExceptone(clearAll, structurecode) {
    objifxStpmapStructures.clearAllStructuresExceptOne(clearAll, structurecode);
}

function clearConstraints(clearAll) {
    objifxStpmapStructures.clearConstraints(clearAll);
}

function filterStructures(structureClassList, show, otherOrg, zoom) {
    objifxStpmapStructures.filterStructures(structureClassList, show, otherOrg, zoom);
}

function showOrHideAffectedStructures(show) {
    objifxStpmapStructures.showOrHideAffectedStructures(show);
}

function checkboxValue(type, page, otherOrg) {
    switch (type) {
        case 'CONSTRAINT':
            if (document.getElementById('Constraints').checked == true) {
                if (page == 'AFFECTED') {
                    if ((otherOrg == false && document.getElementById('Affected').checked == true) || (otherOrg == true && document.getElementById('All').checked == true))
                        return true;
                }
                else {
                    if ((otherOrg == false && document.getElementById('OwnedByMe').checked == true) || (otherOrg == true && document.getElementById('OwnedByOtherOrganisations').checked == true))
                        return true;
                }
            }
            return false;
        default:
            if (document.getElementById('Structs').checked == true &&
                (type == 'underbridge' && document.getElementById('Underbridge').checked == true) ||
                (type == 'overbridge' && document.getElementById('Overbridge').checked == true) ||
                (type == 'under and over bridge' && document.getElementById('UnderAndOverbridge').checked == true) ||
                (type == 'level crossing' && document.getElementById('LevelCrossing').checked == true)) {
                if (page == 'AFFECTED') {
                    if ((otherOrg == false && document.getElementById('Affected').checked == true) || (otherOrg == true && document.getElementById('All').checked == true))
                        return true;
                }
                else {
                    if ((otherOrg == false && document.getElementById('OwnedByMe').checked == true) || (otherOrg == true && document.getElementById('OwnedByOtherOrganisations').checked == true))
                        return true;
                }
            }
            return false;
    }
}

function showConstraints(constraintList, otherOrg) {
    objifxStpmapStructures.showConstraints(constraintList, otherOrg);
}

function ShowRoadownershipRoads(OrgId) {
    objifxStpmapRoadOwnership.showRoadOwnershipOrganisationOwnedRoads(OrgId);
    
}

function ShowDelegationOwnedRoads(OrgId) {
    objifxStpMapRoadDelegation.showDelegationOwnedRoads(OrgId);
}

function DeleteDelegationOwnedRoads() {
    objifxStpMapRoadDelegation.DeleteOwnedRoads();
}

function DeleteDelegationManagedRoads() {
    objifxStpMapRoadDelegation.DeleteManagedRoads(); 
}

function ShowDelegationManagedRoads(OrgId) {
    objifxStpMapRoadDelegation.showDelegationManagedRoads(OrgId);
}

function filterConstraints(show, otherOrg) {
    objifxStpmapStructures.filterConstraints(show, otherOrg);
}
function ShowHideUnsuitableconstraints(show, otherOrg) {
    objifxStpmapStructures.filterUnsuitableConstraints(show, otherOrg);
}

function confirmClearRoute() {
    //showWarningPopDialog('You have unsaved changes. Do you want to continue?', 'No', 'Yes', 'close_alert', 'clearRoute', 1, 'warning');
    
        if (typeof CheckMapInputHasChanges != 'undefined' && CheckMapInputHasChanges()) {
        ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "clearRoute")
    }
    else {
        clearRoute();
    }
}

function finishEdit() {
    objifxStpMap.processEditRoute();
    updateUI();
}

function clearRoute() {
    if (typeof $("#hIs_NEN").val() == "undefined" || $("#hIs_NEN").val() != "true") {
        if (objifxStpMap.pageType == 'ROUTELIBRARY')
            $('#btn_showRoute').show();
    }
    close_alert();
    CloseWarningPopup();
    objifxStpMap.clearAllRoutes(true);
    clearConstraints(0);
    clearStructures(0);
    clearStructFlags();
    document.getElementById('Affected').checked = false;
    updateUI();
    setZoomTo(defaultx, defaulty, 1);
    IsRoutePlanned = false;
    isUnsavedRouteExist = false;
    mapInputValuesGlobal = [];
    $('#btn_re_plan').hide();
}

function clearRouteReplan(callBackFn) {
    objifxStpMap.clearAllRoutes(true);
    setPageState('replanninginprogress');
    clearConstraints(0);
    clearStructures(0);
    clearStructFlags();
    document.getElementById('Affected').checked = false;
    if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
        callBackFn();
    }
}
function notifClearRoute() {
    objifxStpMap.clearRoutePart();
}

function debugSearch(txt) {
    if (txt[0] == '$') {
        if (txt[1] != '(') {
            txt = txt.substring(1);
            var linkIds = txt.split(',');
            objifxStpMap.searchFeaturesByLinkID(linkIds, function (features) {
                if (features != null && features.length > 0) {
                    objifxStpMap.highlightFeatures(features);
                }
            });
        }
        else {
            txt = txt.substring(2);
            txt = txt.substring(0, txt.length - 1).replace(/[,\s]/, '*');
            var coord = txt.split('*');
            if (coord.length == 2)
                objifxStpMap.setLocationIndicatorAtXY(coord[0], coord[1]);
        }
    }
}


function searchAddress(txt) {
    
    $.ajax({
        url: '../QAS/Search',
        type: 'POST',
        async: true,
        data: { searchKeyword: txt },
        beforeSend: function () {
            // startAnimation();
           
        },
        success: function (addrList) {
            if (addrList.length > 0) {
                var x = 0;
            }
            $.ajax({
                url: '../QAS/GetAddress',
                type: 'POST',
                async: true,
                data: { moniker: addrList[0].Moniker },
                beforeSend: function () {
                    // startAnimation();
                    
                },
                success: function (addrList) {
                    if (addrList != null) {

                        mapSearcheasting = addrList.Easting;
                        mapSearchnorthing = addrList.Northing;
                        mapaddresssearch = txt;
                        mapsearchtrigger = 1;
                    }
                    
                },
                complete: function () {
                   

                }
            });
           
        },
        complete: function () {
            
        }
    });

}



function searchLinks(txt) {
    startAnimation();
    $('.bs-canvas-overlay').remove();
    txt = txt.replace("'", "''");
    var keyValues = txt.split(',');
    //var isLink = false;
    //var i = 0;
    //while (i < (keyValues.length)) {
    //    var a = isNaN(keyValues[i]);
    //    if (isNaN(keyValues[i]) == false)
    //        isLink = true;
    //    else
    //        keyValues[i] = "'" + keyValues[i].toUpperCase() + "'";
    //    i++;
    //}
    objifxStpMap.clearAllFeaturesFromLayer(objifxStpMap.vectorLayerRoute);
    // if (isLink == true) {
    objifxStpMap.searchFeaturesByLinkID(keyValues, function (features) {
        if (objifxStpMap.getPageType() == "ROADOWNERSHIP_VIEWANDEDIT") {
            objifxStpmapRoadOwnership.searchAndSelectLinks(features);
        }
        else if (objifxStpMap.getPageType() == "DELEGATION_VIEWANDEDIT") {
            objifxStpMapRoadDelegation.searchAndSelectLinks(features);
        }
        else {
            stopAnimation();
        }
        //if (features != null && features.length > 0) {
        //    objifxStpMap.highlightFeatures(features, true);
        //}

    });
    // }
    //else {
    //    objifxStpMap.searchFeaturesByRoadName(keyValues, function (features) {
    //        if (features != null && features.length > 0) {
    //            objifxStpMap.highlightFeatures(features, true);
    //        }
    //        stopAnimation();
    //    });

    //}
}
function isSessionActive() {
    var res = false;
    $.ajax({
        async: false,
        type: "POST",
        url: "../Routes/IsSessionActive",
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ rand: Math.random() }),
        success: function (reply, textStatus, jqXHR) {
            if (reply.Success == true)
                res = true;
        }
    });

    return res;
}

function checkForReturnLeg() {
    if (isSessionActive() == false) {
        //location.reload();
        return;
    }
    var wayptlen = document.getElementsByName("lblwaypoint").length;
    Checkprevptstatus(wayptlen);
    if ($("#hIs_NEN").val() == "true" || $("#hIs_NEN").val() == "True") {
        if (IsReturnPlanned == false)
            document.getElementById('ReturnLeg').checked = true;
        if (IsReturnRestriction == true)
            document.getElementById('ReturnLeg').checked = false;
    }
    if (document.getElementById('ReturnLeg').checked == true) {
        planRoute(true);
    }
    else {
        planRoute(false);
    }
}
//        var Istrue = true;
//        var errorCount;
//        if (VObj_err != null) {
//            errorCount = VObj_err.length;
//            for (var i = 0 ; i < errorCount; i++) {
//                if (VObj_err[i].isSetCorrect == 0) {
//                    Istrue = false;
//                    showWarningPopDialogBig('The route could not be planned due to the following reason: </br></br> 1. Please correct all addresses. If address could not easily editable then delete/change the address.'
//                 , 'Ok', '', 'closecheckingErrors', '', 1, 'info');
//                    break;
//                }
//            }
//        }
//        if (Istrue == true) {
//            if (IsReturnPlanned == false) {
//                document.getElementById('ReturnLeg').checked = true;
//                planRoute(true);
//            }
//            if (IsReturnRestriction == true) {
//                document.getElementById('ReturnLeg').checked = false;
//                planRoute(false);
//            }
//        }
//    }
//    else {
//        if (document.getElementById('ReturnLeg').checked == true) {
//            planRoute(true);
//        }
//        else {
//            planRoute(false);
//        }
//    }
//}

function planRoute(planReturn, callback) {
    startAnimation();
    objifxStpMap.planRoute(false, function (result) {
        if (planReturn == true && result == true) {
            mainRouteLegalRestriction = false;//this indicates there is no legal restriction for main route.
            objifxStpMap.planRoute(true, function (result) {
                if (result == false)
                    IsReturnRestriction = true; //here return leg of non esdal automatic planning has resulted in error 
                planRouteCallback(result, callback);
            });
        }
        else {
            if (typeof IsAutomaticPlaning != "undefined" && IsAutomaticPlaning == true)
                mainRouteLegalRestriction = true;//this indicates there is legal restriction for main route
            planRouteCallback(result, callback);
        }
    });
}

function planRouteCallback(result, callback) {
    if (objifxStpMap.pageType != 'NOMAPDISPLAY') {
        updateUI();
        stopAnimation();
        objifxStpMap.illogicalWaypointIdentification();
        if (result == false) {
            if ($("#hIs_NEN").val() == "true") {
                $('.pop-message').css({ 'margin-top': '56px' });
                IsrestrictionsErrror = true;
                if (IsReturnRestriction == true)
                    showWarningPopDialogBig('The route(s) could not be planned due to the following reasons: </br></br> 1. Due to legal restrictions on the route.</br>Please change the start/end/waypoints and plan the route.'
                        , 'Ok', '', 'close_alert', '', 1, 'info');
                else
                    showWarningPopDialogBig('The route(s) could not be planned due to the following reasons: </br></br> 1. Due to legal restrictions on the route.</br>Please change the start/end/waypoints and plan the route.'
                        , 'Ok', '', 'close_alert', '', 1, 'info');
            }
            else {
                showNotification("No route can be planned. Which may be due to legal restrictions on the route. Please change the start/end/way points and plan the route.");
            }
        }
    }
    else {
        if (result == false) {
            if ($("#hIs_NEN").val() == "true") {
                $('.pop-message').css({ 'margin-top': '56px' });
                IsrestrictionsErrror = true;
                if (IsReturnRestriction == true && mainRouteLegalRestriction == false) {//here return route is having the legal restrictions error, since main route has been planned without errors for non esdal
                    routeId_forChStatua = $("#hreturnLeg_routeID").val(); // changing return routes status with planning error.
                    SetRouteStatus_PlanningError($("#hreturnLeg_routeID").val()); //setting the route status to planning error for return route
                    //showWarningPopDialogBig('The route(s) could not be planned due to the following reasons: </br></br> 1. Due to legal restrictions on the return route.</br>Please change the start/end and plan the route. If problem continues sign out and then sign back in.'
                    //, 'Ok', '', 'close_alert', '', 1, 'info');
                }
                else {
                    // stopAnimation();
                    routeId_forChStatua = $("#hNENRoute_Id").val();
                    SetRouteStatus_PlanningError($("#hNENRoute_Id").val());
                    showWarningPopDialogBig('The route(s) could not be planned due to the following reasons: </br></br> 1. Due to legal restrictions on the route.</br>Please change the start/end/waypoints and plan the route.'
                        , 'Ok', '', 'close_alert', '', 1, 'info');
                }

            }
            else {
                stopAnimation();
                showToastMessage({
                    message: "No route can be planned, which may be due to legal restrictions on the route. Please change the start/end/waypoints and plan the route.",
                    type: "warning"
                });
                //showNotification("No route can be planned, which may be due to legal restrictions on the route. Please change the start/end/waypoints and plan the route.");
            }

        }
        if (callback && typeof (callback) === "function") {
            callback(result);
        }
    }
}

function showContextMenu() {
    var x = ($('#hf_AllocationType').val());
    if (objifxStpMap.pageType == 'SORTMAPFILTER_VIEWANDEDIT') {
        return false;
    }
    else {
        if (objifxStpMap.getPageState() != 'mouseover') {
            mapcontextMenuOn = false;
            currentMouseOverFeature = null;
        }
        else {
            var mouseOverFeature = getCurrentMouseOverFeature();
            var featureType = mouseOverFeature[0];
            if (featureType == 'LOCATIONIDICATOR') {
                return false;
            }
            else if (featureType == 'ANNOTATION' || featureType == 'STRUCTURE' || featureType == 'CONSTRAINT') {
                mapcontextMenuOn = true;
                $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
            }
            else if (featureType == 'WAYPOINT') {
                $('#ctxtable').find('tr:eq(' + 6 + ')').show();
                $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
            }
            else if (featureType == 'VIAPOINT') {
                $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                $('#ctxtable').find('tr:eq(' + 7 + ')').show();
                $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
            }
            else if (featureType == 'STARTPOINT' || featureType == 'ENDPOINT') {
                for (var i = 0; i < 16; i++) {
                    $('#ctxtable').find('tr:eq(' + i + ')').hide();
                }
                $('#ctxtable').find('tr:eq(' + 21 + ')').hide();
                switch (objifxStpMap.pageType) {
                    case 'A2BPLANNING':
                    case 'SOA2BPLANNING':
                    case 'ROUTELIBRARY':
                    case 'SIMPNOTIF':
                        var state = objifxStpMap.getCurrentPathState();
                        switch (state) {
                            case 'idle':
                                if (objifxStpMap.getcurrentSelectedRouteType() == 4) {
                                    if (featureType == 'STARTPOINT') {
                                        $('#ctxtable').find('tr:eq(' + 10 + ')').show();
                                    }
                                    else {
                                        $('#ctxtable').find('tr:eq(' + 11 + ')').show();
                                    }
                                }
                                break;
                            case 'firstpointselected':
                                if (objifxStpMap.getcurrentSelectedRouteType() == 4) {
                                    if (featureType == 'STARTPOINT'
                                        && objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 1
                                        && objifxStpMap.getAdvancedRouteDetails().terminalPoint != 1) {
                                        $('#ctxtable').find('tr:eq(' + 10 + ')').show();
                                    }
                                    else if (featureType == 'ENDPOINT'
                                        && objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 0
                                        & objifxStpMap.getAdvancedRouteDetails().terminalPoint != 0) {
                                        $('#ctxtable').find('tr:eq(' + 11 + ')').show();
                                    }
                                }
                                break;
                        }
                }
                return true;
            }
            else {
                if ((featureType != 'WAYPOINT' && featureType != 'VIAPOINT') || (objifxStpMap.pageType == 'DISPLAYONLY' || objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION')) {
                    for (var i = 0; i < 16; i++) {
                        $('#ctxtable').find('tr:eq(' + i + ')').hide();
                    }
                    return;
                }
            }
        }
        switch (objifxStpMap.pageType) {
            case 'A2BPLANNING':
            case 'SOA2BPLANNING':
            case 'ROUTELIBRARY':
            case 'SIMPNOTIF':
                var state = objifxStpMap.getCurrentPathState();
                switch (state) {
                    case 'idle':
                        if (objifxStpMap.getcurrentSelectedRouteType() == 0) {
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 1) {
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 2) {
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();

                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 3) {
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 4) {
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                        }
                        $('#ctxtable').find('tr:eq(' + 4 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 5 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                        if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0] == undefined) {
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
						}
                        break;
                    case 'firstpointselected':
                        if (objifxStpMap.getcurrentSelectedRouteType() == 0) {
                            if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 0) {
                                $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                                $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            }
                            else {
                                $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                                $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            }
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();

                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 1) {
                            if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 0) {
                                $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                                $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            }
                            else {
                                $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            }
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 2) {
                            if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 0) {
                                $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            }
                            else {
                                $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            }
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 3) {
                            if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 0) {
                                $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            }
                            else {
                                $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                                $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            }
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                        }
                        else if (objifxStpMap.getcurrentSelectedRouteType() == 4) {
                            if (objifxStpMap.routeManager.RoutePart.routePathList[objifxStpMap.currentActiveRoutePathIndex].routePointList[0].pointType == 0) {
                                $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                            }
                            else {
                                $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                            }
                            $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                        }
                        $('#ctxtable').find('tr:eq(' + 4 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 5 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                        break;
                    case 'secondpointselected':
                    case 'routeplanned':
                    case 'routedisplayed':
                        $('#ctxtable').find('tr:eq(' + 0 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 1 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 2 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 3 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 8 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 10 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 11 + ')').hide();
                        if (state == 'secondpointselected') {
                            $('#ctxtable').find('tr:eq(' + 9 + ')').hide();
                            if (featureType == 'WAYPOINT') {
                                $('#ctxtable').find('tr:eq(' + 6 + ')').show();
                                $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                            }
                            else if (featureType == 'VIAPOINT') {
                                $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                                $('#ctxtable').find('tr:eq(' + 7 + ')').show();
                            }
                        }
                        break;
                    default:
                        for (var i = 0; i < 16; i++) {
                            $('#ctxtable').find('tr:eq(' + i + ')').hide();
                        }
                }
                var pageState = objifxStpMap.getPageState();
                switch (pageState) {
                    case 'mouseover':
                        $('#ctxtable').find('tr:eq(' + 4 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 5 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                        var mouseOverFeature = getCurrentMouseOverFeature();
                        var featureType = mouseOverFeature[0];
                        if (featureType == 'ANNOTATION') {
                            $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 21 + ')').hide();

                        }
                        else if (featureType == 'WAYPOINT' || featureType == 'VIAPOINT') {
                            $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 14 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 21 + ')').hide();

                        }
                        else if (featureType == 'STRUCTURE' || featureType == 'CONSTRAINT') {
                            if (featureType != 'STRUCTURE') {
                                $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                                $('#ctxtable').find('tr:eq(' + 21 + ')').hide();
                            }
                            $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                        }
                        else {
                            $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 14 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                            $('#ctxtable').find('tr:eq(' + 21 + ')').hide();
                        }
                        break;
                    default:
                        $('#ctxtable').find('tr:eq(' + 6 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 7 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 14 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 17 + ')').hide();
                        //if (_checkerstatus == 301002 ) {
                        //    $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                        //}
                        //301002- checking , 301008 - QA Checking , 301005-Checking final
                        try {
                            if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005)) {
                                if (sort_user_id != checker_id) {
                                    $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                                }
                            } else {
                                if (plannr_UserId > 0 && sort_user_id != plannr_UserId) {
                                    $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                                }
                            }
                        }
                        catch (err) {}
                }
                $('#ctxtable').find('tr:eq(' + 17 + ')').hide();
                break;

            case 'DISPLAYONLY':
            case 'STRUCTURES':
            case 'DISPLAYONLY_EDITANNOTATION':
                for (var i = 0; i < 14; i++) {
                    $('#ctxtable').find('tr:eq(' + i + ')').hide();
                }
                if (objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION') {
                    var pageState = objifxStpMap.getPageState();
                    switch (pageState) {
                        case 'mouseover':
                            $('#ctxtable').find('tr:eq(' + 13 + ')').show();
                            if (featureType == 'WAYPOINT' || featureType == 'VIAPOINT') {
                                $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                            }
                            break;
                        default:
                            $('#ctxtable').find('tr:eq(' + 12 + ')').show();
                    }
                }
                if (objifxStpMap.getPageState() != 'mouseover') {
                    $('#ctxtable').find('tr:eq(' + 14 + ')').hide();
                    $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                    $('#ctxtable').find('tr:eq(' + 21 + ')').hide();
                }
                if (featureType != 'STRUCTURE') {
                    $('#ctxtable').find('tr:eq(' + 15 + ')').hide();
                    $('#ctxtable').find('tr:eq(' + 21 + ')').hide();
                }
                $('#ctxtable').find('tr:eq(' + 17 + ')').hide();
                break;
            case 'DELEGATION_VIEWONLY':
            case 'DELEGATION_VIEWANDEDIT':
                for (var i = 0; i < 16; i++) {
                    $('#ctxtable').find('tr:eq(' + i + ')').hide();
                }
                $('#ctxtable').find('tr:eq(' + 17 + ')').hide();
                break;
            case 'ROADOWNERSHIP_VIEWANDEDIT':
            case 'ROADOWNERSHIP_VIEWONLY':
                for (var i = 0; i < 17; i++) {
                    $('#ctxtable').find('tr:eq(' + i + ')').hide();
                }
                break;
        }
        if ($('#PortalType').val() != '696008')
            $('#ctxtable').find('tr:eq(' + 21 + ')').hide();

        //Condition for showing annotation on affected structures and constraints feature over
        var Routestate = objifxStpMap.getCurrentPathState();
        var overState = objifxStpMap.getPageState();
        if ((Routestate == 'routeplanned' || Routestate == 'routedisplayed') && (overState == 'mouseover')) {
            var mouseOverFeature = getCurrentMouseOverFeature();
            var featureType = mouseOverFeature[0];
            if (featureType == 'STRUCTURE' || featureType == 'CONSTRAINT') {
                $('#ctxtable').find('tr:eq(' + 12 + ')').show();
            }
        }
        if ((Routestate == 'routeplanned' || Routestate == 'routedisplayed')) {
            if (status != undefined) {
                if (status == 'MoveVer' && (_checkerstatus != 301002)) {
                    $('#ctxtable').find('tr:eq(' + 12 + ')').show();
                    if (objifxStpMap.getPageState() == 'mouseover') {
                        var mouseOverFeature = getCurrentMouseOverFeature();
                        var featureType = mouseOverFeature[0];
                        if (featureType == 'ANNOTATION') {
                            $('#ctxtable').find('tr:eq(' + 13 + ')').show();
                        }
                    }
                }
                //if (status == 'MoveVer' && (sort_user_id=checker_id))
                //    $('#ctxtable').find('tr:eq(' + 12 + ')').show();
                try {
                    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005 || chk_status == 301009 || chk_status == 301006) && (sort_user_id != checker_id) && ((MovLatestVer == MovVersion) || MovLatestVer == 0)) {
                        $('#ctxtable').find('tr:eq(' + 12 + ')').hide();
                        $('#ctxtable').find('tr:eq(' + 13 + ')').hide();
                    }
                }
                catch (err) {
                    // resuming the flow without showing if error is thrown.
                }
            }
        }
        if (objifxStpMap.getPageState() != 'mouseover') {
            $('#ctxtable').find('tr:eq(' + 21 + ')').hide();
        }
        return true;
       
        
    }
      
}

function getRouteDetails(isReturnLeg) {
    return objifxStpMap.getRoutePart(isReturnLeg);
}

function getOutlineRouteDetails() {
    return objifxStpMap.getRoutePart();

}

function confirmRemoveRoutePath(pathIndex) {
    showWarningPopDialog('You have unsaved changes. Do you want to continue?', 'No', 'Yes', 'close_alert', 'removeRoutePath(pathIndex)', 1, 'warning');
}

function removeRoutePath(pathIndex) {
    close_alert();
    objifxStpMap.removeRoutePath(pathIndex - 1);
    //if (objifxStpMap.getCurrentPathState() == 'routedisplayed')
    //    $('#btn_updateRoute').show();
    objifxStpMap.setCurrentPathState('routeplanned');
    updateUI();
}

function deleteWayPoint(pos, pathIndex) {
    var timerVar = setInterval(function () {
        objifxStpMap.deleteRoutePoint(pos + 2, pathIndex, function () {
            clearTimeout(timerVar);
        });
    }, 100);

    //objifxStpMap.clearRoutePath(pathIndex);
    if (pathIndex == 0 && objifxStpMap.getPathCount() > 1) {
        objifxStpMap.clearAllRoutes(false);
        clearselect();
    }
    else {
        objifxStpMap.clearRoutePath(pathIndex);
    }
    updateUI();
}

function getCurrentActiveRoutePathIndex() {
    return objifxStpMap.currentActiveRoutePathIndex;
}

function isQASAllowed(pathType, pointType) {
    switch (pathType) {
        case 1: if (pointType == 'ENDPOINT') return false; break;
        case 2: if (pointType == 'STARTPOINT' || pointType == 'ENDPOINT') return false; break;
        case 3: if (pointType == 'STARTPOINT') return false; break;
    }
    return true;
}

function setRoutePoint(locationDesc, pointNo, easting, northing, pointType, callback) {
    
    pointType = IfxStpmapCommon.getPointTypeName(parseInt(pointType));
    var pathType = objifxStpMap.getcurrentSelectedRouteType();

    if (!isQASAllowed(pathType, pointType)) {
        showNotification('Address search is not allowed. Please select the location from the main route');
        objifxStpMap.setLocationIndicatorAtXY(easting, northing);
        var pix = { x: easting, y: northing };
        setCenter(pix, true);
        return;
    }

    startAnimation();
    objifxStpMap.setRoutePointAtXY({ pointType: pointType, pointPos: pointNo, X: easting, Y: northing, searchInBbox: true, Zoomin: true, locationDesc: locationDesc }, function (rpPoint) {
        if (objifxStpMap.pageType != 'NOMAPDISPLAY') {
            var map = document.getElementById('map');
            map.click();

            if (rpPoint != undefined && rpPoint != null) {

                if (getCurrentActiveRoutePathIndex() == 0 && objifxStpMap.getPathCount() > 1) {
                    objifxStpMap.clearAllRoutes(false);
                    clearselect();
                }
                else {

                    rpPoint.pointDescr = locationDesc;
                    objifxStpMap.clearRoutePath();
                }
                updateUI(rpPoint);
            }
            else {
                if ($("#hIs_NEN").val() == "true") {
                    var VRouteId = $("#hNENRoute_Id").val();
                    routeId_forChStatua = VRouteId;
                    SetRouteStatus_PlanningError($("#hNENRoute_Id").val());
                }
                showNotification('No road found at the location. Please select the location from the map');
                //var pix = { x: easting, y: northing };
                //setCenter(pix, true);
                setZoomTo(easting, northing, 9);
            }
            var isNEN = $("#hIs_NEN").val();
            if (callback && typeof (callback) === "function") {
                callback();
            }
        }
        stopAnimation();
        if (objifxStpMap.pageType == 'NOMAPDISPLAY') {
            if (callback && typeof (callback) === "function") {
                if (rpPoint != null) {
                    rpPoint.pointDescr = locationDesc;
                }
                else {
                    if (pointType == 'STARTPOINT')
                        ShowInfoPopup('No road found for the start address. Please change the address or plan the route from map');
                    else if (pointType == 'ENDPOINT') {
                        if ($("#hIs_NEN").val() == "true") {
                            IsrestrictionsErrror = true;
                            routeId_forChStatua = $("#hNENRoute_Id").val();
                            SetRouteStatus_PlanningError($("#hNENRoute_Id").val());
                        }

                        ShowInfoPopup('No road found for the end address. Please change the address or plan the route from map');
                    }
                }
                callback(rpPoint != null);
            }
        }
    });
}

function setRouteDetails(rPart) {

}

function buttonsShowAndHide(plan, save, update, clear, details, saveOutline, chkReturnLeg, finishEdit) {
    if (objifxStpMap.pageType == 'DISPLAYONLY' || objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION') {
        return;
    }

    var fromRLib = !$('#btn_saveRoute').length;
    if (plan == 1) {
        $('#btn_plan').show();
        $('#div_viapoint').show();
        $('#div_waypoint').show();
        $('#btn_re_plan').hide();
        if ($("#hIs_NEN").val() == "true" && VObj_err != null) {
            if (objifxStpMap.routeManager.RoutePart.routePathList[0].routePointList[0].linkId == 0 || objifxStpMap.routeManager.RoutePart.routePathList[0].routePointList[1].linkId == 0) {
                $('#btn_plan').hide();
                $('#div_viapoint').hide();
                $('#div_waypoint').hide();
                $('#btn_details').hide();
            } else {
                $('#btn_plan').show();
                $('#div_viapoint').show();
                $('#div_waypoint').show();
                $('#btn_details').show();
            }
        }
        else
            $('#btn_details').show();
    }
    else if (plan == 2) {
        $('#btn_plan').hide();
        if (clear == 2) {
            $('#div_viapoint').hide();
            $('#div_waypoint').hide();
        }
        if (clear == 1) {
            $('#div_viapoint').show();
            $('#div_waypoint').show();
        }
        $('#btn_details').hide();
    }
    if (save == 1 || update == 1) {
        if (!fromRLib) {
            $('#btn_saveRoute').show();
            try {
                if (IsRoutePlanned != undefined) {
                    IsRoutePlanned = true;
                }
            } catch (err) {

            }
        }
        else
            $('#btn_updateRoute').show();
    }
    else if (save == 2 || update == 2) {
        if (!fromRLib) {
            $('#btn_saveRoute').hide();
        }
        else
            $('#btn_updateRoute').hide();
    }
    if (clear == 1)
        $('#btn_clear').show();
    else if (clear == 2)
        $('#btn_clear').hide();
    if (details == 1)
        $('#btn_details').show();
    else if (details == 2)
        $('#btn_details').hide();

    if (saveOutline == 1)
        $('#Outline_Save').show();
    else if (saveOutline == 2)
        $('#Outline_Save').hide();

    var PageFlag = $('#Pageflag').val();
    if (PageFlag != 2)
        $('#Outline_Save').hide();

    if (chkReturnLeg == 1 && returnLeg == true) {
        var page = objifxStpMap.pageType;
        document.getElementById('ReturnLeg').checked = false;
        //if (returnLeg == true && page != "A2BPLANNING" && page != "ROUTELIBRARY")
        if ($("#hIs_NEN").length == 0 ||
            ($("#hIs_NEN").length > 0 && $("#hIs_NEN").val() != "true" && $("#hIs_NEN").val() != "True")) {
            $('#ReturnLegDiv').show();
        }
    }
    else if (chkReturnLeg == 2) {
        $('#ReturnLegDiv').hide();
    }

    if (finishEdit == 1) {
        $('#btn_finishedit').show();
    }
    else {
        $('#btn_finishedit').hide();
    }
}

function deactivateOtherControl(index, page) {
   
    $('#intellizenz-ctxmenu').remove();
    if (page == "A2BPLANNING") {
        if (index == 0 || index == 1 || index == 2) {

            document.getElementById("From_location").value = '';
            document.getElementById("To_location").value = '';
            //var start = objifxStpMap.getcurrentSelectedRouteType();
            //if (start==1) {
            //    document.getElementById("startflag").style.display = "none";
            //    document.getElementById("startflagalt").style.display = "block";
            //    document.getElementById("endflag").style.display = "none";
            //    document.getElementById("endflagalt").style.display = "block";
            //}
            
        }
        for (var i = 0; i < toolbarPanel.controls.length; i++) {
            if (i == index) {
                toolbarPanel.controls[i].activate();
            }
            else {
                toolbarPanel.controls[i].deactivate();
            }
        }

        if (index == -1 || (index >= 4 && index <= 6)) {
            toolbarPanel4.div.style.display = "inline";
            toolbarPanel2.div.style.display = "inline";
        }
        else {
            toolbarPanel2.div.style.display = "none";
            toolbarPanel4.div.style.display = "none";
        }
    }
    //else if (page == "STRUCTURES") {
    //    for (var i = 0; i < constraintToolbarPanel.controls.length; i++) {
    //        if (i == index) {
    //            constraintToolbarPanel.controls[i].activate();
    //        }
    //        else {
    //            constraintToolbarPanel.controls[i].deactivate();
    //        }
    //    }
    //}
    else if (page == "ROUTEAPPRAISAL") {
        for (var i = 0; i < toolbarPanel2.controls.length; i++) {
            if (i == index) {
                toolbarPanel2.controls[i].activate();
            }
            else {
                toolbarPanel2.controls[i].deactivate();
            }
        }
    }
    else if (page == "ROADDELEGATION") {
        for (var i = 0; i < roadDelegationToolbarPanel.controls.length; i++) {
            if (i == index) {
                roadDelegationToolbarPanel.controls[i].activate();
            }
            else {
                roadDelegationToolbarPanel.controls[i].deactivate();
            }
        }
    }
}

function deactivateToolbarControls() {
    if (objifxStpMap.pageType == 'DISPLAYONLY' || objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION') {
        return;
    }
    for (var i = 0; i < toolbarPanel.controls.length; i++) {
        toolbarPanel.controls[i].deactivate();
    }
    toolbarPanel4.div.style.display = "none";
}

function swapStartEnd() {
    if (objifxStpMap.swapRoutePoint(0, 1)) {
        var tVal = document.getElementById('To_location').value;
        document.getElementById('To_location').value = document.getElementById('From_location').value;
        document.getElementById('From_location').value = tVal;
        objifxStpMap.clearRoutePath();
        updateUI();
    }
}

function setBounds(bounds) {
    objifxStpMap.setBounds(bounds);
}

function showStructOnTimer(page, bounds) {
    clearTimeout(timer);
    timer = setInterval(function () {
        if (page == 'STRUCTURES') {
           structPanChanged();
           showStructBounds();
        }
        else {
            structPanChangedA2B();
            showStructBoundsA2B();
        }
        setBounds(bounds);
        clearTimeout(timer);
    }, 1000);
}

function fetchRoadsOnTimer() {
    clearTimeout(timer);
    timer = setInterval(function () {
        objifxStpMapRoadDelegation.fetchDelegRoadsOnZoomChange(function () {
            objifxStpMapRoadDelegation.fetchDelegRoadsTimeFlag = 1;
            if (document.getElementById('owned') != null) {
                if (roadDelegationCheckValue('owned') || roadDelegationCheckValue('managed')) {
                    objifxStpMapRoadDelegation.fetchAllRoadsOnZoomChange(function () {
                        objifxStpMapRoadDelegation.fetchAllRoadsTimeFlag = 1;
                    });
                }
            }
            else {
                objifxStpMapRoadDelegation.fetchAllRoadsOnZoomChange(function () {
                    objifxStpMapRoadDelegation.fetchAllRoadsTimeFlag = 1;
                });
            }
        });
        clearTimeout(timer);
    }, 3000);
}

function fetchOwnedRoadsOnTimer() {
    clearTimeout(timer);
    timer = setInterval(function () {
        objifxStpmapRoadOwnership.fetchOwnedRoadsOnZoomChange(function () {
            objifxStpmapRoadOwnership.fetchOwnedRoadsTimeFlag = 1;
            if (roadOwnershipCheckValue('unassigned')) {
                objifxStpmapRoadOwnership.fetchUnassignedRoadsOnZoomChange(function () {
                    objifxStpmapRoadOwnership.fetchUnassignedRoadsTimeFlag = 1;
                    stopAnimation();
                });
            }
            else {
                stopAnimation();
            }
            //if (roadOwnershipCheckValue('owned')) {
            //    objifxStpmapRoadOwnership.fetchOwnedRoadsOnZoomChange(function () {
            //        objifxStpmapRoadOwnership.fetchUnassignedRoadsTimeFlag = 1;
            //    });
            //}
        });
        clearTimeout(timer);
    }, 3000);
}

function loadEnd() {
    if (objifxStpMap.getPageType() == "ROADOWNERSHIP_VIEWANDEDIT") {
        setInterval(function () {
            //document.getElementById("searchOrganisation").style.display = "inline";
        }, 5000);
    }
    handleZoomButtonAlignment();
}

function handleZoomButtonAlignment(timeOut=1000) {
    setTimeout(function () {        
        var $zoomElem = $('.olControlZoom.olControlNoSelect');
        var footerHeight = $('#footer').height() || 72;
        $zoomElem.css('bottom', (footerHeight + 5) + 'px');//Adjust zoom element bottom
        var olControlNoSelectHeight = $zoomElem.height() || 48;
        $('.olControlZoomStatus.olControlNoSelect').css('bottom', (footerHeight + olControlNoSelectHeight + 5) + 'px');//Adjust zoom number element bottom
        $('.olControlZoomStatus.olControlNoSelect').css('right', '16px');//Adjust zoom number element right
    }, timeOut);
}

function panChanged() {
    clearTimeout(timer);
    var boundsAndZoom = getCurrentBoundsAndZoom();
    var prevBounds = objifxStpMap.getBounds();
    if (boundsAndZoom.zoom < 9) {
        for (var i = 0; i < objifxStpMap.olMap.layers.length; i++) {
            if (objifxStpMap.olMap.layers[i].name == 'Structures') {
                for (var k = 0; k < objifxStpMap.olMap.layers[i].features.length; k++) {
                    if (objifxStpMap.olMap.layers[i].features[k].style.strokeWidth != undefined && objifxStpMap.olMap.layers[i].features[k].style.graphicWidth == undefined) { //extra condition added to confirm onlt line is hidden and no icons
                        objifxStpMap.olMap.layers[i].features[k].style.display = 'none';
                    }

                }
                break;
            }
        }
    }
    

    if (objifxStpMap.pageType == 'DELEGATION_VIEWANDEDIT' && boundsAndZoom.zoom < 8) {
        if (btnSelectByPolygon != undefined) {
            if (btnSelectByPolygon.active == true) {
                selectByPolygon.deactivate();
			}
		}
    }
    
    setBounds(boundsAndZoom.bounds);

    objifxStpMap.setDragandPan();

    $('#intellizenz-ctxmenu').remove();
    if (objifxStpMap.getPageType() == 'STRUCTURES' ||
        objifxStpMap.getPageType() == 'A2BPLANNING' ||
        objifxStpMap.getPageType() == 'ROUTELIBRARY' ||
        objifxStpMap.getPageType() == 'SOAPP' ||
        objifxStpMap.getPageType() == 'DISPLAYONLY' ||
        objifxStpMap.getPageType() == 'DISPLAYONLY_EDITANNOTATION' ||
        objifxStpMap.getPageType() == 'VR1APP' ||
        objifxStpMap.getPageType() == 'SOA2BPLANNING'
    ) {

        if (boundsAndZoom.bounds.left >= prevBounds.left && boundsAndZoom.bounds.bottom >= prevBounds.bottom && boundsAndZoom.bounds.right <= prevBounds.right && boundsAndZoom.bounds.top <= prevBounds.top) {
            if (objifxStpMap.getPageType() == 'STRUCTURES') {
                var timeFlag = getTimeFlag();
                if (timeFlag.myStruct == 0 && document.getElementById('OwnedByMe').checked == true && document.getElementById('Structs').checked == true) {
                    showStructOnTimer('STRUCTURES', boundsAndZoom.bounds);
                }
                else if (timeFlag.myStruct == 1) {
                    objifxStpmapStructures.structuresOnZoomChange(boundsAndZoom.zoom, 'MYSTRUCTURES', false);
                }
                if (timeFlag.otherStruct == 0 && document.getElementById('OwnedByOtherOrganisations').checked == true && document.getElementById('Structs').checked == true) {
                    showStructOnTimer('STRUCTURES', boundsAndZoom.bounds);
                }
                else if (timeFlag.otherStruct == 1) {
                    objifxStpmapStructures.structuresOnZoomChange(boundsAndZoom.zoom, 'MYSTRUCTURES', true);
                }
                if (timeFlag.myConst == 0 && document.getElementById('OwnedByMe').checked == true && document.getElementById('Constraints').checked == true) {
                    showStructOnTimer('STRUCTURES', boundsAndZoom.bounds);
                }
                else if (timeFlag.myConst == 1) {
                    objifxStpmapStructures.constraintsOnZoomChange(boundsAndZoom.zoom, 'MYSTRUCTURES', false);
                }
                if (timeFlag.otherConst == 0 && document.getElementById('OwnedByOtherOrganisations').checked == true && document.getElementById('Constraints').checked == true) {
                    showStructOnTimer('STRUCTURES', boundsAndZoom.bounds);
                }
                else if (timeFlag.otherConst == 1) {
                    objifxStpmapStructures.constraintsOnZoomChange(boundsAndZoom.zoom, 'MYSTRUCTURES', true);
                }
            }
            else {
                var timeFlag = getTimeFlagA2B();
                if (timeFlag.affectedStruct == 0 && document.getElementById('Affected').checked == true && document.getElementById('Structs').checked == true) {
                    showStructOnTimer('A2B', boundsAndZoom.bounds);
                }
                else if (timeFlag.affectedStruct == 1) {
                    objifxStpmapStructures.structuresOnZoomChange(boundsAndZoom.zoom, 'AFFECTED', false);
                }
                if (timeFlag.allStruct == 0 && document.getElementById('All').checked == true && document.getElementById('Structs').checked == true) {
                    showStructOnTimer('A2B', boundsAndZoom.bounds);
                }
                else if (timeFlag.allStruct == 1) {
                    objifxStpmapStructures.structuresOnZoomChange(boundsAndZoom.zoom, 'AFFECTED', true);
                }
                if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
                    if (timeFlag.affectedStruct == 1 && document.getElementById('All').checked == false && document.getElementById('Structs').checked == false) {
                        showStructOnTimer('A2B', boundsAndZoom.bounds);
                    }
                    else if (timeFlag.unSuitStruct == 1 && timeFlag.affectedStruct == 0) {
                        objifxStpmapStructures.structuresOnZoomChangeUnsuitable(boundsAndZoom.zoom, 'AFFECTED', false);
                    }
                }

                if (timeFlag.affectedConst == 0 && document.getElementById('Affected').checked == true && document.getElementById('Constraints').checked == true) {
                    showStructOnTimer('A2B', boundsAndZoom.bounds);
                }
                else if (timeFlag.affectedConst == 1) {
                    objifxStpmapStructures.constraintsOnZoomChange(boundsAndZoom.zoom, 'AFFECTED', false);
                }
                if (timeFlag.allConst == 0 && document.getElementById('All').checked == true && document.getElementById('Constraints').checked == true) {
                    showStructOnTimer('A2B', boundsAndZoom.bounds);
                }
                else if (timeFlag.allConst == 1) {
                    objifxStpmapStructures.constraintsOnZoomChange(boundsAndZoom.zoom, 'AFFECTED', true);
                }
                if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
                    if (timeFlag.affectedConst == 1 && document.getElementById('Affected').checked == false && document.getElementById('Constraints').checked == false) {
                        showStructOnTimer('A2B', boundsAndZoom.bounds);
                    }
                    else if (timeFlag.unSuitConst == 1 && timeFlag.affectedConst == 0) {
                        objifxStpmapStructures.constraintsOnZoomChangeUnsuite(boundsAndZoom.zoom, 'AFFECTED', false);
                    }
                }
            }
        }
        else {
            if (objifxStpMap.getPageType() == 'STRUCTURES') {
                showStructOnTimer('STRUCTURES', boundsAndZoom.bounds);
            }
            else {
                showStructOnTimer('A2B', boundsAndZoom.bounds);
            }
        }
    }
    else if (objifxStpMap.getPageType() == 'DELEGATION_VIEWANDEDIT' || objifxStpMap.getPageType() == 'DELEGATION_VIEWONLY') {
        objifxStpMapRoadDelegation.fetchAllRoadsTimeFlag = 0;
        objifxStpMapRoadDelegation.fetchDelegRoadsTimeFlag = 0;
        fetchRoadsOnTimer();
    }
    else if (objifxStpMap.getPageType() == 'ROADOWNERSHIP_VIEWANDEDIT' || objifxStpMap.getPageType() == 'ROADOWNERSHIP_VIEWONLY') {
        objifxStpmapRoadOwnership.fetchOwnedRoadsTimeFlag = 0;
        objifxStpmapRoadOwnership.fetchUnassignedRoadsTimeFlag = 0;
        fetchOwnedRoadsOnTimer();
    }

    handleZoomButtonAlignment(0);
}

function zoomChanged() {
}

function Structurearrayempty() {
    emptyStructureArray();
}

function checkBoxStatus() {
    var myStructures = false, otherStructures = false, constraints = false;
    if (document.getElementById('MyStructures').checked == true) {
        myStructures = true;
    }
    if (document.getElementById('OtherStructures').checked == true) {
        otherStructures = true;
    }
    if (document.getElementById('Constraints').checked == true) {
        constraints = true;
    }
    return { myStructures: myStructures, otherStructures: otherStructures, constraints: constraints };
}

function onRouteLoaded() {
    var pageState = objifxStpMap.getPageState();
    if (pageState == "replanninginprogress") {
        setPageState('planninginprogress');
        stopAnimation();
    }
    updateUI();
}

function getCurrentMouseOverFeature() {
    return currentMouseOverFeature.data.name.split(" ");
}

function routeexist() {
    try {
        if (objifxStpMap.routeManager.RoutePart.routePathList[0].routeSegmentList.length > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        return false;
    }
}

function deleteAnnotation() {
    if (objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION')
        $('#btnmovsaveannotation').show();
    else {
        buttonsShowAndHide(2, 1, 2, 1, 2, 2, 2);
        try {
            if (IsRoutePlanned != undefined) {
                IsRoutePlanned = true;
            }
        }
        catch (err) {}
    }
    
    if (document.getElementById('MoveVerVehicleandRoutes') != null && document.getElementById('MoveVerVehicleandRoutes') != undefined) {
        let rtv = document.getElementById('MoveVerVehicleandRoutes');
        if (rtv.className.includes('active-card')) {
            $('#btnmovsaveannotation').show();
        }
    }
    var mouseOverFeature = getCurrentMouseOverFeature();
    var featureType = mouseOverFeature[0];
    if (featureType == 'ANNOTATION') {

        var self = this;
        var timerVar = setInterval(function () {
            objifxStpMap.deleteAnnotation(mouseOverFeature[1], mouseOverFeature[2], mouseOverFeature[3]);
            clearTimeout(timerVar);
        }, 500);
    }
}

function deleteMouseOverWaypoint() {
    var mouseOverFeature = getCurrentMouseOverFeature();
    var pathIndex = parseInt(mouseOverFeature[1]);
    var pos = parseInt(mouseOverFeature[2]) - 2;
    currentMouseOverFeature = null;
    objifxStpMap.backupAnnotationsinRoute();
    deleteWayPoint(pos, pathIndex);
    removewaypoint_map(pos);
    objifxStpMap.resetAllRoutePoints(pathIndex);
}

function removeDragroutemarkers() {

    objifxStpMap.resetAllRoutePoints(objifxStpMap.currentActiveRoutePathIndex);
}

function showDetails() {
    var mouseOverFeature = getCurrentMouseOverFeature();
    var featureType = mouseOverFeature[0];
    if (featureType != 'ANNOTATION') {
        var featureId = parseInt(mouseOverFeature[1]);
        if (featureType == 'STRUCTURE') {
            var structureId = currentMouseOverFeature.data.name.match("STRUCTURE (.*) NAME");
            var structureCode = currentMouseOverFeature.data.name.match("CODE (.*) SUITABILITY");
            var otherOrg = currentMouseOverFeature.data.name.match("OTHERORG (.*) TYPE");
            if (objifxStpMap.getPageType() == 'A2BPLANNING' || objifxStpMap.getPageType() == 'ROUTELIBRARY' ||
                objifxStpMap.getPageType() == 'DISPLAYONLY' || objifxStpMap.getPageType() == 'DISPLAYONLY_EDITANNOTATION') {
                showStructureDetails("true", structureId[1]);
            }
            else if (otherOrg[1] == "true") {
                showStructureDetails(otherOrg[1], structureId[1]);
            }
            else
                showStructureDetails(otherOrg[1], structureId[1]);
        }
        else if (featureType == 'CONSTRAINT') {
            var otherOrg = currentMouseOverFeature.data.name.match("OTHERORG (.*) TYPE");

            viewConstraintDetails(featureId, otherOrg[1]);
        }
    }
    else {
        showAnnotation(mouseOverFeature[1], mouseOverFeature[2], mouseOverFeature[3]);
    }
}


$('body').on('click', '.ifx-stp-map-bak-to-map', function (e) {
    BackTomap();
});
$('body').on('click', '.ifx-stp-map-select-from-qas', function (e) {
    selectFromQASList(this);
});
function BackTomap() {
    $('#StruRelatedMov').html('');
    $('#StruRelatedMov').hide();
    $("#IsMovListAvalabl").val("No");
    $('#RoutePart').show();

    $('#leftpanel').show();
    $('#divMap').show();
    $(".slidingpanelstructures").removeClass("hide").addClass("show");
    $("#header-fixed").hide();
    $("#tableheader").hide();

}
function ShowRoadContacts(pix) {
    var self = this;
    objifxStpMap.searchFeaturesByXY(pix.x, pix.y, false, objifxStpMap.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            if ($("#hIs_NEN").val() == "true") {
                routeId_forChStatua = $("#hNENRoute_Id").val();
                SetRouteStatus_PlanningError($("#hNENRoute_Id").val());
            }
            ShowErrorPopup('No road found');
            return;
        }
        else {

            var lonlat = this.objifxStpMap.olMap.getLonLatFromPixel({ x: pix.x, y: pix.y });
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
            var featureSel = features[retObject.index];
            var lrsMeasure = LRSMeasure(featureSel.geometry, new OpenLayers.Geometry.Point(retObject.x1, retObject.y1), { tolerance: 0.5, details: true });
            ShowRoadContactsDetails(featureSel.attributes.LINK_ID, Math.round(lrsMeasure.length));
        }
    });
}

function ShowRoadOwnerContacts(pix) {
    var self = this;
    objifxStpMap.searchFeaturesByXY(pix.x, pix.y, false, objifxStpMap.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            if ($("#hIs_NEN").val() == "true") {
                routeId_forChStatua = $("#hNENRoute_Id").val();
                SetRouteStatus_PlanningError($("#hNENRoute_Id").val());
            }
            ShowErrorPopup('No road found');
            return;
        }
        else {
            var lonlat = this.objifxStpMap.olMap.getLonLatFromPixel({ x: pix.x, y: pix.y });
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
            var featureSel = features[retObject.index];
            var lrsMeasure = LRSMeasure(featureSel.geometry, new OpenLayers.Geometry.Point(retObject.x1, retObject.y1), { tolerance: 0.5, details: true });
            ShowRoadOwnerContactsDetails(featureSel.attributes.LINK_ID, Math.round(lrsMeasure.length));
        }
    });
}

function ShowRoadContactsDetails(link_ID, length) {
    resetdialogue();
    pageType = objifxStpMap.getPageType();
    $("#dialogue").load('/Structures/RoadContact?linkID=' + link_ID + '&length=' + length + '&pageType=' + pageType, function () {
        CheckSessionTimeOut();
        RoadContactInit();
    });
    $("#dialogue").show();
    document.getElementById("overlay").style.display = "block";
}

function ShowRoadOwnerContactsDetails(link_ID, length) {
    resetdialogue();
    pageType = objifxStpMap.getPageType();
    $("#dialogue").load('/RoadOwnership/RoadOwnersContact?linkID=' + link_ID + '&length=' + length + '&pageType=' + pageType, function () {
        CheckSessionTimeOut();
        RoadOwnersContactInit();
    });
    $("#dialogue").show();
    document.getElementById("overlay").style.display = "block";
}

function showStructureContact() {
    var mouseOverFeature = getCurrentMouseOverFeature();
    var featureType = mouseOverFeature[0];
    if (featureType == 'STRUCTURE') {
        var featureId = parseInt(mouseOverFeature[1]);

        showStructContactDetails(featureId)

    }
}

function showStructContactDetails(struct_id) {
    resetdialogue();
    $("#dialogue").load('../Structures/StructureContact?structureId=' + struct_id, function () {
        CheckSessionTimeOut();
        StructureContactInit();
    });
    removescroll();
    $("#dialogue").show();
    $("#overlay").show();

}

function viewConstraintDetails(constraintId, flageditmode) {
    var randomNumber = Math.random();
    flageditmode = flageditmode == "true" ? false : true;
    startAnimation();
    $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + constraintId + '&flageditmode=' + flageditmode + '&random=' + randomNumber, function () {
        CheckSessionTimeOut();
        stopAnimation();
        $("#dialogue").show();
        $("#overlay").show();
        $("#dialogue").css("margin-left","auto");
        ViewConstraintInit();
    });
}

function showStructureDetails(otherOrg, struct_id) {
    //ADDED BY POONAM 24-3-15 RM#3840
    var orgid = $('#OrgId').val();
    var isAuthorizeMovementGeneral = $('#AuthorizeMovementGeneral').length>0 && $('#AuthorizeMovementGeneral').val()=="True";
    if (isAuthorizeMovementGeneral) {
        viewStructureDetails(struct_id);
    } else if (orgid != 0) {
        $.ajax({
            type: "POST",
            url: "../Structures/GetStructureOwner",
            data: { StructureId: struct_id, OrganisationId: orgid },
            beforeSend: function () {
                // startAnimation();
            },
            success: function (data) {
                if (data.result == 0) {
                    viewStructureDetails(struct_id);
                }
                else {
                    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + struct_id);
                }
            },
            error: function (xhr, status, error) {
                alert("error");
            },
            complete: function () {
                //$('.loading').hide();
                //$("#overlay").hide();
            }
        });

    }
    else {
        if (otherOrg == "true") {
            viewStructureDetails(struct_id);
        }
        else {
            window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + struct_id);
        }
    }
}

function viewStructureDetails(struct_id) {
    resetdialogue();
    if (typeof $("#chkRouteID").val() != "undefined" && $("#chkRouteID").val() != 0) {
        var contref = $('#CRNo').val();
        var route_id = $('#chkRouteID').val();
        var isRouteAssessment = $('#IsRouteAssessment').val();
        startAnimation();
        $("#dialogue").load('../Structures/UnsuitableStructureSummary?structureId=' + struct_id + '&route_part_id=' + route_id + '&section_id=' + 0 + '&cont_ref_num=' + contref + '&isRouteAssessment=' + isRouteAssessment, function () {
            CheckSessionTimeOut();
            removescroll();
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
            UnsuitableStructureSummaryInit();
            ViewContactDetails_Obj.ContactParent = { StructureId: struct_id, Method: 'viewStructureDetails' };//this
        });
    }
    else {
        startAnimation();
        $("#dialogue").load('../Structures/ReviewSummaryPopup?structureId=' + struct_id, function () {
            CheckSessionTimeOut();
            stopAnimation();
            removescroll();
            $("#dialogue").show();
            $("#overlay").show();
            ReviewSummaryPopupInit();
        });
    }

    //    $("#overlay").show();

}



function ShowStructuredetailspopup(structurecode, sectionId) {
    startAnimation();
    $("#dialogue").load('../Structures/AffectedstructureSummaryPopup?structurecode=' + structurecode + '&sectionId='+ sectionId, function () {
        //CheckSessionTimeOut();
        //stopAnimation();
        //removescroll();
        //$("#overlay").show();
        stopAnimation();
        $("#dialogue").show();
        $('#dialogue').modal('show');
        $("#overlay").css("display", "block");
        ViewContactDetails_Obj.ContactParent = { StructureId: structurecode, Method: 'ShowStructuredetailspopup' };
        AffectedstructureSummaryPopupInit();
    });
}

function calculateRelativePosition(component1, component2) {
    var pos;
    if (component1.y < component2.y)
        pos = 'b';
    else
        pos = 't';
    if (component1.x < component2.x)
        pos = pos.concat('l');
    else
        pos = pos.concat('r');
    return pos;
}

function createConstraintByDescription(ordArray, topologyType, constraintID) {
    objifxStpmapStructures.createConstraintByDescription(ordArray, topologyType, constraintID);
}

function featureOver(e, featureType) {
    objifxStpMap.setPageState('mouseover');
    currentMouseOverFeature = e.feature;
    if (featureType == 'STRUCTURE' || featureType == 'CONSTRAINT') {
        if (e.feature.geometry.components) {
            if (e.feature.geometry.components[0].components) {
                createPopup(featureType, e.feature, e.feature.geometry.components[0].components[0]);
            }
            else {
                var popupPosition = calculateRelativePosition(e.feature.geometry.components[0], e.feature.geometry.components[e.feature.geometry.components.length - 1]);
                createPopup(featureType, e.feature, e.feature.geometry.components[0], popupPosition);
            }
        }
        else {
            createPopup(featureType, e.feature, e.feature.geometry);
        }
    }
}

function featureOut(e, featureType) {
    objifxStpMap.setPageState('readyidle');
    if (featureType == 'STRUCTURE' || featureType == 'STRUCTURELINE' || featureType == 'CONSTRAINT') {
        removePopup(e.feature);
    }
    if (mapcontextMenuOn == false && objifxStpMap.getPageState() == 'readyidle')
        currentMouseOverFeature = null;
}

function createPopup(featureType, feature, geometry, pos) {
    if (featureType == 'STRUCTURE') {
        var name = feature.data.name.match("NAME (.*) CODE");
        var code = feature.data.name.match("CODE (.*) SUITABILITY");
        var suitability = feature.data.name.match("SUITABILITY (.*) OTHERORG");
        var otherOrg = feature.data.name.match("OTHERORG (.*) TYPE");
        var type = feature.data.name.split("TYPE ").pop();
        if (suitability[1] == 'Default' || suitability[1] == 'Not Specified')
            suitability[1] = "   ";
        else if (suitability[1] == 'Unsuitable')
            suitability[1] = "Unsuitable for selected vehicle(s)";
        else if (suitability[1] == 'Marginally suitable')
            suitability[1] = "Marginally suitable for selected vehicle(s)";
        var content = "<div class='card' style='font-size:1.0em'><table><tr><td><b>" + name[1] + "</b></td></tr></table><br>" + suitability[1] + "<br>Code: " + code[1] + "<br>Class: " + IfxStpmapCommon.capitaliseString(type) + "</div>";
        var content1 = "<div ><div style='padding-top: 10px; padding-left: 10px;'><div class='card-text pb-2'>" + name[1] + "</div> <div class='card-text2'><div>" + suitability[1] + "</div><div class='card-text2 pl-0'>Code: " + code[1] + "</div><div class='card-text2 pl-0'>Class: " + IfxStpmapCommon.capitaliseString(type) + "</div></div>";
    }
    else if (featureType == 'CONSTRAINT') {
        var name = feature.data.name.match("NAME (.*) CODE");
        var code = feature.data.name.match("CODE (.*) SUITABILITY");
        if (code[1].substring(0, 2) == "C-") {
            code[1] = "C1-" + feature.data.name.match("CONSTRAINT (.*) NAME")[1] + "-P1";
        }
        var suitability = feature.data.name.match("SUITABILITY (.*) OTHERORG");
        var otherOrg = feature.data.name.match("OTHERORG (.*) TYPE");
        var type = feature.data.name.split("TYPE ").pop();
        if (suitability[1] != null && suitability[1] != undefined)
            suitability[1] = suitability[1].toLowerCase();
        if (suitability[1] == 'unsuitable')
            suitability[1] = "Unsuitable for selected vehicle(s)";
        else
            suitability[1] = "   ";
        var content = "<div class='card' style='font-size:1.0em'><table><tr><td><b>" + name[1] + "</b></td></tr></table><br>" + suitability[1] + "<br>Code: " + code[1] + "<br>Type: " + IfxStpmapCommon.capitaliseString(type) + "</div>";
        var content1 = "<div ><div style='padding-top: 10px; padding-left: 10px;'><div class='card-text pb-2'>" + name[1] + "</div> <div class='card-text2'><div>" + suitability[1] + "</div><div class='card-text2 pl-0'>Code: " + code[1] + "</div><div class='card-text2 pl-0'>Type: " + IfxStpmapCommon.capitaliseString(type) + "</div></div>";
    }



    var popup;
    var pix;
    var boundsAndZoom = getCurrentBoundsAndZoom();
    if (boundsAndZoom.zoom >= 9 && featureType == 'CONSTRAINT') {
        //new code
        var Xpos = objifxStpMap.mousePosition.X;
        var Ypos = objifxStpMap.mousePosition.Y;
        pix = objifxStpMap.olMap.getPixelFromLonLat(new OpenLayers.LonLat(Xpos, Ypos));
        
        var popupPixel = { x: Xpos, y: Ypos };
        popup = new OpenLayers.Popup.FramedCloud("popup",
            objifxStpMap.olMap.getLonLatFromPixel({ x: popupPixel.x, y: popupPixel.y }),
            null,
            content1,
            new OpenLayers.Icon("", new OpenLayers.Size(8, 8), new OpenLayers.Pixel(-4, -4)),
            false
        );
      
    }
    else {
         popup = new OpenLayers.Popup.FramedCloud("popup",
            OpenLayers.LonLat.fromString(geometry.toShortString()),
            null,
            content1,
            new OpenLayers.Icon("", new OpenLayers.Size(6, 6), new OpenLayers.Pixel(-8, -8)),
            false,

        );
    }

   
    if (pos != undefined) {
        popup.calculateRelativePosition = function () {
            return pos;
        }
    }

    popup.autoSize = true;
    feature.popup = popup;
    objifxStpMap.olMap.addPopup(popup);
    //$('#popup_GroupDiv').css('height', '50%');

}

function removePopup(feature) {
    objifxStpMap.olMap.removePopup(feature.popup);
    feature.popup.destroy();
    feature.popup = null;
}

function removePopups() {
    while (objifxStpMap.olMap.popups.length) {
        objifxStpMap.olMap.removePopup(objifxStpMap.olMap.popups[0]);
}
}

function setMapUsageCount(type) {
    $.ajax({
        url: '../Routes/SetMapUsage',
        type: 'POST',
        datatype: 'json',
        async: true,
        data: { type: type },
        success: function (value) {
            closeContentLoader('html')
        },
        error: function () {
            //location.reload();
        },
        complete: function () {
            //stopAnimation();
        }
    });
}

function remPath() {
    $('#intellizenz-ctxmenu').remove();
    removepath(objifxStpMap.currentActiveRoutePathIndex);
    updateUI();
}

function setPageType(type) {
    objifxStpMap.setPageType(type);
}

function setPageState(state) {
    objifxStpMap.setPageState(state);
}

function getCurrentBoundsAndZoom() {
    return objifxStpMap.getCurrentBoundsAndZoom();
}

function showAffectedStructures(routeId, callback) {
    //routeId = getRouteID();
    if (routeId == undefined || routeId == '0' || routeId == '')
        routeId = objifxStpMap.getRouteId();

    if (objifxStpMap.getCurrentPathState() == 'routeplanned') {
        if (routeId == undefined || routeId == '0' || routeId == '')
            routeId = 0;
        objifxStpmapStructures.getInstantAnalysis(routeId, 0, function (result) {
            if (callback && typeof (callback) === "function") {
                callback(result);
            }
        });
    }
    else {
        var sortFlag = false;
        if ($('#HFSortFlag').val() != undefined && ($('#HFSortFlag').val() == "true" || $('#HFSortFlag').val() == "True"))
            sortFlag = true;
        objifxStpmapStructures.showAffectedStructures(routeId, objifxStpMap.getPageType(), sortFlag, function (result) {
            if (callback && typeof (callback) === "function") {
                callback(result);
            }
        });
    }
}
//function for show unsitablestructures on notification 
function showUnsuitableStructures(routeId, callback) {
    if (vieweditflagStruct == 1) {
        vieweditflagStruct = 0;
        objifxStpmapStructures.showAffectedStructures(routeId, "NotificationViewOnMap", false, function (result) {
            if (callback && typeof (callback) === "function") {
                callback(result);
            }
        });
    }
    vieweditflagStruct = 0;
}
function showUnsuitableConstraints(routeId) {
    if (vieweditflagconst == 1) {
        vieweditflagconst = 0;
        objifxStpmapStructures.showAffectedConstraints(routeId, "NotificationViewOnMap", false);
    }
    vieweditflagconst = 0;
}

function showAffectedConstraints(routeId) {
    
    if (routeId == undefined || routeId == '0' || routeId == '')
        routeId = objifxStpMap.getRouteId();

    if (objifxStpMap.getCurrentPathState() == 'routeplanned') {
        if (routeId == undefined || routeId == '0' || routeId == '')
            routeId = 0;
        objifxStpmapStructures.getInstantAnalysis(routeId, 1, null);
    }
    else {
        var sortFlag = false;
        if ($('#HFSortFlag').val() != undefined && ($('#HFSortFlag').val() == "true" || $('#HFSortFlag').val() == "True"))
            sortFlag = true;
        
        objifxStpmapStructures.showAffectedConstraints(routeId, objifxStpMap.getPageType(), sortFlag);
    }
}

function showAllStructures(animationText, callback) {
    objifxStpmapStructures.showAllStructures(animationText, function (result) {
        if (callback && typeof (callback) === "function") {
            callback(result);
        }
    });
}

function showAllConstraints(animationText) {
    objifxStpmapStructures.showAllConstraints(animationText);
}

function toggleStructures(type, activate) {
    
    $('#intellizenz-ctxmenu').remove();
    if (type == 'STRUCTURES') {
        if (activate == true) {
            if (document.getElementById("toggleAllAffected") != null) {
            $("#toggleAllAffected").prop('checked', false);
            ToggleAllAffected();
            }
            
            // if ($(".slidingpanelstructuresopen").length < 1)
            //   $(".slidingpanelnav").trigger("click");
            document.getElementById('Affected').checked = true;
            document.getElementById('Structs').checked = true;
            if (document.getElementById('Underbridge').checked == false && document.getElementById('Overbridge').checked == false
                && document.getElementById('UnderAndOverbridge').checked == false && document.getElementById('LevelCrossing').checked == false) {
                document.getElementById('Underbridge').checked = true;
                document.getElementById('Overbridge').checked = true;
                document.getElementById('UnderAndOverbridge').checked = true;
                document.getElementById('LevelCrossing').checked = true;
            }
        }
        else {
            if (document.getElementById('All').checked == false) {
                if (btnAffectedConstraints.active != true) {
                    document.getElementById('Affected').checked = false;
                    if ($(".slidingpanelstructuresopen").length >= 1 && $(".slidingpanelnav").is(':visible'))
                        $(".slidingpanelnav").trigger("click");
                }
                document.getElementById('Structs').checked = false;
                document.getElementById('Underbridge').checked = false;
                document.getElementById('Overbridge').checked = false;
                document.getElementById('UnderAndOverbridge').checked = false;
                document.getElementById('LevelCrossing').checked = false;
            //    if (document.getElementById("toggleAllAffected") != null && document.getElementById('Constraints').checked == false && document.getElementById('Structs').checked == false) {
            //    $("#toggleAllAffected").prop('checked', true);
               ToggleAllAffected();
            //}
            }
            else {
                if (document.getElementById('Constraints').checked == true)
                    document.getElementById('Affected').checked = false;
            }
        }
        if (viewEditRouteFlagStructures == 1) {
            viewEditRouteFlagStructures = 0;
            objifxStpMap.vectorLayerStructures.removeFeatures(objifxStpMap.vectorLayerStructures.features);
            objifxStpMap.vectorLayerStructures.redraw();
        }
        showStructBoundsA2B();
    }
    else {
        if (activate == true) {
            if (document.getElementById("toggleAllAffected") != null) {
            $("#toggleAllAffected").prop('checked', false);
            ToggleAllAffected();
            }
            if ($(".slidingpanelstructuresopen").length < 1 && $(".slidingpanelnav").is(':visible'))
                $(".slidingpanelnav").trigger("click");
            document.getElementById('Affected').checked = true;
            
            document.getElementById('Constraints').checked = true;
            
        }
        else {
            if (document.getElementById('All').checked == false) {
                if (btnAffectedStructures.active != true) {
                    document.getElementById('Affected').checked = false;
                    if ($(".slidingpanelstructuresopen").length >= 1 && $(".slidingpanelnav").is(':visible'))
                        $(".slidingpanelnav").trigger("click");
                }
                document.getElementById('Constraints').checked = false;
                if (document.getElementById("toggleAllAffected") != null && document.getElementById('Constraints').checked == false && document.getElementById('Structs').checked == false) {
                $("#toggleAllAffected").prop('checked', true);
                ToggleAllAffected();
            }
            }
            else {
                if (document.getElementById('Structs').checked == true)
                    document.getElementById('Affected').checked = false;
            }
        }
        if (viewEditRouteFlagConstraints == 1) {
            viewEditRouteFlagConstraints = 0;
            objifxStpMap.vectorLayerConstraints.removeFeatures(objifxStpMap.vectorLayerConstraints.features);
            objifxStpMap.vectorLayerConstraints.redraw();
        }
        showStructBoundsA2B();
    }
}

function offRoadAdded() {
    updateUI();
    toolbarPanel4.div.style.display = "inline";
    toolbarPanel2.div.style.display = "none";
}

function dragPointAdded(pix) {
    searchSegmentByXY(pix.x, pix.y, 9);
}

function flashRoute(index) {
    var counter = 0;
    var timerVar = setInterval(function () {
        objifxStpMap.flashRoutePath(index, counter);
        if (counter == 7)
            clearTimeout(timerVar);
        counter++;
    }, 85);
}

function setManoeuvre(e, callback) {
    drawManouevre = callback;
    var nearestPath = objifxStpMap.getNearestPathAndSegment(e.xy.x, e.xy.y, false);
    if (nearestPath.pathIndex != objifxStpMap.currentActiveRoutePathIndex && nearestPath.distance < 1000) {
        ShowWarningPopup('Trying to select a road segment which is closer to another route path. Do you want to continue?', 'drawManouevre');
    }
    else {
        callback();
    }
}

function setTrimPoint(pix) {
    objifxStpMap.setTrimPoint(pix);
}

function trimRoute(pix) {
    objifxStpMap.trimRoute(pix);
}

$(document).keydown(function (e) {
    if (e.keyCode == 27) {
        if ($(document).find('#route_search_popup').length > 0) {
            $(document).find('#route_search_popup').hide();
            $(document).find("#Map_View").css("z-index", 0);
            $(document).find("#map").css("z-index", 0);
            $(document).find("#wraper_leftpanel_content").css("overflow", 'auto');
        }
    }
});
var isUnsavedRouteExist = false;
function ShowQASList(rpPoint, field) {
    if (rpPoint == null || rpPoint == undefined)
        return;
    var searchKeyword = rpPoint.pointDescr;
    var url = "../QAS/Search";
    var field = getTextField(rpPoint);
    x = $(field);
    x.parent().append("<div id='search_anim' class='search_anim'></div>");
    var offsettop = x.offset().top;
    var offsetleft = x.offset().left;
    var Element_height = x.css("height");
    offsettop = (parseInt(offsettop) + parseInt(Element_height) + parseInt(72));

    //add flag to check unsaved changes
    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        isUnsavedRouteExist = true;
    }

    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        data: { searchKeyword: searchKeyword },
        beforeSend: function () {
            startAnimation();

        },
        success: function (result) {
            if (result.length > 0) {
                $(x).parent().append("<div id='route_search_popup' class='route_search_popup'></div>");
                $('#route_search_popup').append("<ul class='qas-autofill-ul' id='newList'></ul>");
                for (var cnt = 0; cnt < result.length; cnt++) {
                    var String = result[cnt].AddressLine;
                    $("#newList").append("<li class='pl-4 pr-2 pt-2 pb-2 ifx-stp-map-select-from-qas'><div > <span class='class='text-normal''>" + String + "</span></div></li>");
                }

                x.parent().find('#route_search_popup').css({ "top": offsettop - 61, "left": offsetleft, "width": 350 });

                x.parent().find('#newList').css({ "top": offsettop - 65 });
                x.parent().find('#route_search_popup').show();
                $('body').addClass('removeScroll');

            }
        },
        error: function (xhr, textStatus, errorThrown) {
            Error_showNotification(x, errorThrown);
        },
        complete: function () {
            $('#search_anim').remove();

            stopAnimation();

        }
    });
    if (notifrouteedit == false) {
        $(document).find("#Map_View").css("z-index", -1);
        //$(document).find("#map").css("z-index", -1);
    }
    $(document).find("#wraper_leftpanel_content").css("overflow", 'auto');
}

function selectFromQASList(_this) {
    var a = $(_this).parent().parent().parent().find("input[type=text]").attr('id');

    var text = jQuery.trim($(_this).text())
    setRoutePointDesc(a, text);

    var input_id = '#' + a;
    $(input_id).val(text);
    $('body').removeClass('removeScroll');
}

function getTextField(rpPoint) {
    if (rpPoint.pointType == 0) {
        return '#From_location';
    }
    else if (rpPoint.pointType == 1) {
        return '#To_location';
    }
    else if (rpPoint.pointType == 2) {
        var wp = 'Waypoint' + rpPoint.routePointNo;
        var waypoint = '#' + wp;
        return waypoint;
    }
    else if (rpPoint.pointType == 3) {
        var wp = 'Viapoint' + rpPoint.routePointNo;
        var viapoint = '#' + wp;
        return viapoint;
    }
}

function setRoutePointDesc(field, desc) {
    switch (field) {
        case 'From_location':
            objifxStpMap.setRoutePointDescription(0, desc);
            break;
        case 'To_location':
            objifxStpMap.setRoutePointDescription(1, desc);
            break;
        default:
            var strIndex = "";
            for (var i = 8; i < field.length; i++) {
                strIndex += field.charAt(i);
            }
            var pointIndex = parseInt(strIndex) + 1;
            objifxStpMap.setRoutePointDescription(pointIndex, desc);
    }
}

function validateRoutePoints() {
    return objifxStpMap.validateRoutePoints();
}

function updateFullAddress(pathIndex, pointIndex, pointDesc) {
    objifxStpMap.updateFullAddress(pathIndex, pointIndex, pointDesc);
}

function highlightRoads(linkInfoList) {
    startAnimation();
    objifxStpMapRoadDelegation.highlightRoads(linkInfoList, false, function () {
        stopAnimation();
    });
}

function getDelegLinkInfoList() {
    return objifxStpMapRoadDelegation.getLinkInfoList();
}
function getOwnerLinkInfoList() {
    return objifxStpmapRoadOwnership.getOwnerLinkInfoList();
}
function fetchRoadLinks(arrangementId, linkInfoList) {
    objifxStpMapRoadDelegation.fetchRoadLinks(arrangementId, linkInfoList);
}

function showAllRoads(zoomToOrg) {
    objifxStpMapRoadDelegation.showAllRoads(zoomToOrg);
}
function showUnassignedRoads() {//nithin
    objifxStpmapRoadOwnership.showUnassignedRoads();
}
function showOrganisationRoads(organisationId) {
    objifxStpmapRoadOwnership.showOrganisationRoads(organisationId);
}
function hideUnassignedRoads() {//nithin
    objifxStpmapRoadOwnership.hideUnassignedRoads();
}
function selectAllOwnedRoads(addtolistflag) {//nithin
    startAnimation();
    objifxStpmapRoadOwnership.selectAllOwnedRoads(addtolistflag);
}
function deSelectAllOwnedRoads(RemoveFromlistflag) {//nithin
    objifxStpmapRoadOwnership.deSelectAllOwnedRoads(RemoveFromlistflag);
}
function clearAllOrgData() {//nithin
    objifxStpmapRoadOwnership.clearAllOrgData();
   
}
function clearAllSelectedRoads() {//nithin
    objifxStpmapRoadOwnership.clearAllSelectedRoads();
}
function clearSearchSegments() {
    
    objifxStpmapRoadOwnership.clearSearchSegments();
    document.getElementById('vieworganisation').style.display = "none";
    document.getElementById('viewroads').style.display = "none";
    document.getElementById('chevlon-up-icon').style.display = "none";
    document.getElementById('chevlon-down-icon').style.display = "block";
    document.getElementById('chevlon-up-icon1').style.display = "none";
    document.getElementById('chevlon-down-icon1').style.display = "block";
    $('#searchID').hide();
    $('#txtOrgSearch').val('');
    deleteroad();
    closeOwnershipFilter();
    
}
function clearOffRoad() {
    objifxStpMap.clearoffroad();
}

function zoomInToDelegRoad(link, arrangementId, callback) {
    objifxStpMapRoadDelegation.zoomInToDelegRoad(link, arrangementId, callback);
}

function zoomInToOwnedRoad(LinkIds, organisationId, callback) {//nithin
    objifxStpmapRoadOwnership.zoomInToOwnedRoad(LinkIds, organisationId, callback);
}

function zoomInToSelectedLinkId(LinkId, callback) {//nithin
    objifxStpmapRoadOwnership.zoomInToSelectedLinkId(LinkId, callback);
}
//function Display() {
//    objifxStpmapRoadOwnership.Display()
//}

function addToDelegatedRoadLinks(linkInfo) {
    objifxStpMapRoadDelegation.addToRoadLinks(linkInfo);
}

function showAndHideRoads(linkMangeStatus, boolShow) {
    if (boolShow && objifxStpMapRoadDelegation.fetchAllRoadsTimeFlag == 0) {
        objifxStpMapRoadDelegation.fetchAllRoadLinks(function () {
            objifxStpMapRoadDelegation.fetchAllRoadsTimeFlag = 1;
        });
    }
    else {
        objifxStpMapRoadDelegation.showAndHideRoads(linkMangeStatus, boolShow);
    }
}

function roadDelegationCheckValue(item) {
    switch (item) {
        case 'owned':
            return document.getElementById('owned').checked;
        case 'managed':
            return document.getElementById('managed').checked;
    }
}
function roadOwnershipCheckValue() {
    return document.getElementById('unassigned').checked;
}

function mapResize() {
    objifxStpMap.updateMapSize();
}

function PoliceBoundaries(show) {
    objifxStpmapForeLayers.PoliceBoundaries(show);
}

function LABoundaries(show) {
    objifxStpmapForeLayers.LABoundaries(show);
}

function NHBoundaries(show) {
    objifxStpmapForeLayers.NHBoundaries(show);
}

function TfLRoads(show) {
    objifxStpmapForeLayers.TfLRoads(show);
}

function WelshTrunkRoads(show) {
    objifxStpmapForeLayers.WelshTrunkRoads(show);
}

function ScottishTrunkRoads(show) {
    objifxStpmapForeLayers.ScottishTrunkRoads(show);
}

function Restaurants(show) {
    objifxStpmapForeLayers.Restaurants(show);
} 
function Hospital(show) {
    objifxStpmapForeLayers.Hospital(show);
} 
function Parkbay(show) {
    objifxStpmapForeLayers.Parkbay(show);
} 
function FiInst(show) {
    objifxStpmapForeLayers.FiInst(show);
}
function Entertainment(show) {
    objifxStpmapForeLayers.Entertainment(show);
}
function BuisFeci(show) {
    objifxStpmapForeLayers.BuisFeci(show);
}
function CMSServ(show) {
    objifxStpmapForeLayers.CMSServ(show);
}
function Shopping(show) {
    objifxStpmapForeLayers.Shopping(show);
}
function EduIn(show) {
    objifxStpmapForeLayers.EduIn(show);
}
function AutMob(show) {
    objifxStpmapForeLayers.AutMob(show);
}
function TransHub(show) {
    objifxStpmapForeLayers.TransHub(show);
}
function TravelDest(show) {
    objifxStpmapForeLayers.TravelDest(show);
}
function ParkAndRec(show) {
    objifxStpmapForeLayers.ParkAndRec(show);
}
function clearMouseCache() {
    objifxStpMap.clearMouseCache();
}

function routeDragComplete() {
    planRoute(false);
}

function changePathSelect(pathIndex) {
    change_select(pathIndex, false);
}

//page 0: A2B, 1: route library, 2: sketched/outline route,
//3: agreed route, 4: structures, 5: so(outline) creation 
//6: vr1 creation 7: so display 8: vr1 display
//10: display only mode 11: notification creation
function loadmap(pageType, routePart, showReturnLeg, flagShowRouteAssessment, geoRegion) {
    objifxStpMap = new IfxStpMap();
    objifxStpmapStructures = new IfxStpmapStructures();
    objifxStpMapRoadDelegation = new IfxStpmapRoadDelegation();
    objifxStpmapRoadOwnership = new IfxStpmapRoadOwnership();
    objifxStpmapForeLayers = new IfxStpmapForeLayers();
    objifxStpMap.registerEvent('ONDRAGCOMPLETE', onDragCompleteFn);
    objifxStpMap.registerEvent('ADDPATH', updateUI);
    objifxStpMap.registerEvent('ROUTELOADED', onRouteLoaded);
    objifxStpMap.registerEvent('CONSTRAINTADDED', updateUI);
    objifxStpMap.registerEvent('MANOEUVRESELECT', setManoeuvre);
    objifxStpMap.registerEvent('MANOEUVREADDED', createAnnotation);
    objifxStpMap.registerEvent('CONSTRAINTBYDESCRIPTION', updateUI);
    objifxStpMap.registerEvent('DEACTIVATECONTROL', deactivateOtherControl);
    objifxStpMap.registerEvent('ZOOMCHANGED', zoomChanged);
    objifxStpMap.registerEvent('PANCHANGED', panChanged);
    objifxStpMap.registerEvent('FEATUREOVER', featureOver);
    objifxStpMap.registerEvent('FEATUREOUT', featureOut);
    objifxStpMap.registerEvent('ADVRPCANCEL', remPath);
    objifxStpMap.registerEvent('OFFROADADDED', offRoadAdded);
    objifxStpMap.registerEvent('TOGGLESTRUCTURES', toggleStructures);
    objifxStpMap.registerEvent('PATHSTATECHANGED', updateUI);
    objifxStpMap.registerEvent('ROUTEPOINTADDED', updateUI);
    objifxStpMap.registerEvent('DRAGPOINTADDED', dragPointAdded);
    objifxStpMap.registerEvent('ONROUTEDRAGCOMPLETE', routeDragComplete);
    objifxStpMap.registerEvent('CHANGEPATHSELECT', changePathSelect);
    // objifxStpMap.registerEvent('LOADMAPEND', loadEnd);

    if (showReturnLeg == true || (showReturnLeg != undefined && showReturnLeg.toLowerCase() == 'true'))
        returnLeg = true;

    if (flagShowRouteAssessment != undefined) {
        if (flagShowRouteAssessment == false || (typeof flagShowRouteAssessment === "string" && flagShowRouteAssessment.toLowerCase() == 'false'))
            showRouteAssessment = false;
    }

    if (pageType != 'NOMAPDISPLAY') {//no map display
        setMapUsageCount(0);
        if (pageType == 'DISPLAYONLY' || pageType == 'DISPLAYONLY_EDITANNOTATION')
            structureslidingpanel_show();
    }
    closeContentLoader('html');

    if (pageType != 'STRUCTURES') {
        //call for showing structure sliding panel
        load_Structureslidingpanel();
        init_structureslidingpanel();
        structureslidingpanel_show();
    }

    if (routePart == 'DEFAULT')
        routePart = null;

    var routePlanUnit = IfxStpmapCommon.getRoutePlanUnit($("#HdnRoutePlanUnit").val());
    objifxStpMap.loadMap(pageType, routePart, routePlanUnit, geoRegion);
    try {
        if ($('#hIs_NEN').val() == "true" && $('.hp-map .RouteAppraisalToolbar').length > 0) {
            $('.hp-map .RouteAppraisalToolbar').removeClass("AgreedSOBtnCont");
            $('.hp-map .RouteAppraisalToolbar').addClass("NenAffect");
        }
    }
    catch (err) {}
    loadEnd();
    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        ShowUnSuitableStructuresOrConstraintsOnMapFromRouteAssessment();
        if (Ismapenlarged == 0) {
            $("#MaxmizeIocn").hide();
        }
    }
}

/*function for set a return leg in the case of legal restriction RM#10629*/
function SetReturnLeg(i) {
    if (i != -1) {
        setRoutePoint(Vobj_NonError[i].address, Vobj_NonError[i].point, Vobj_NonError[i].Esting, Vobj_NonError[i].Northing, Vobj_NonError[i].point, function () {  // pinned start and end flag on the map
            i = i - 1;
            SetReturnLeg(i);
        });

    }
    else {
        swapStartEnd()
    }
}
/*function to get broken route points and replanning*/
function ReplanBrokenRoutes(routeId, is_library, isView, callBackFn) {
   var requestAjax= $.ajax({
        url: '../BrokenRoutesReplanner/ReplanBrokenRoutes',
        type: 'POST',
        data: { routePartId: routeId, isLib: is_library, isViewOnly: isView },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (isView) {
                if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
                    callBackFn(data);
                }
            }
            else {
                if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
                    callBackFn(data.result);
                }
            }
        },
        complete: function () {
            stopAnimation();
        }
   });
    return requestAjax;
}
/*function to check whether the route is broken*/
function CheckIsBroken(getBrokenRouteList, callBackFn) {
    $.ajax({
        url: '../BrokenRoutesReplanner/Is_BrokenRouteCheck',
        type: 'POST',
        datatype: "json",
        data: getBrokenRouteList,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (typeof callBackFn != 'undefined' && callBackFn != null && callBackFn != "") {
                callBackFn(data);
            }
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function ShowGpxRoute(movementId, routeId, movementType) {
    objifxStpmapForeLayers.ShowGpxRoute(movementId, routeId, movementType);
}

function HideGpxRoute() {
    objifxStpmapForeLayers.HideGpxRoute();
}
function ShowRouteViewMap(routePart, callback) {
    AddRoutePathAndPoints(routePart);
    objifxStpMap.setRoutePart(routePart, callback);
    if ($('#IsAgreedNotify').val() == "True" || $('#IsAgreedNotify').val() == "true") {
        var deactivateControls = "olControlDrawFeature,olControlDragFeature,olControlSelectFeature,olControl,maptoolbarpanel horizontalMap-center";
        DeactivateControls(deactivateControls);
        objifxStpMap.setPageType('DISPLAYONLY');
    }
}
function DeactivateControls(deactivateControls) {
    var array = deactivateControls.split(',');
    objifxStpMap.olMap.controls.forEach(function (control) {
        if (array.includes(control.displayClass)) {
            control.deactivate();
        }
    });
}
function AddRoutePathAndPoints(routePart) {
    for (var i = 0; i < routePart.routePathList[0].routePointList.length; i++) {
        if (routePart.routePathList[0].routePointList[i].pointType == 0) {
            $("#From_location").val(routePart.routePathList[0].routePointList[i].pointDescr);
        }
        if (routePart.routePathList[0].routePointList[i].pointType == 1) {
            $("#To_location").val(routePart.routePathList[0].routePointList[i].pointDescr);
        }
    }
    addWaypoint(0, routePart);
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

    //Keep existing data in temp variable
    if (typeof mapInputValuesGlobal != 'undefined') {
        mapInputValuesGlobal = getMapInputValues();
    }
}

var pageSizeMS = 10;
var pageNumMS = 1;
function ShowRelatedMovm(isSort = false) {
    var structureId;
    var StrucID=0;
    try {
        structureId = currentMouseOverFeature.data.name.match("STRUCTURE (.*) NAME");

        StrucID = structureId[1], structureNM = structureId[0];
    }
    catch (err) {
        StrucID = hf_StructureId;
    }
    var params = isSort ? "page=" + pageNumMS : "page=1";
    params += "&pageSize=" + pageSizeMS;
    $.ajax({
        url: '../Movements/ClearInboxFilter',
        type: 'POST',
        success: function (data) {
            //ClearAdvancedData();
        },
        complete: function () {


            $.ajax({
                type: 'POST',
                cache: false,
                url: '../SORTApplication/SORTInbox?' + params,
                data: { structID: StrucID/*, sectionID: 0, structureNm: structureNM, ESRN: structureCode[1], IsRelatedMov: true*/ },
                beforeSend: function () {
                    //$('#StruRelatedMov').html('');
                    startAnimation('Loading related movements...');
                    //if ($('#SearchPrevMoveVeh').val() == "No") {
                    //}
                    //$("#StruRelatedMove").val("Yes");
                    //$('#RoutePart').hide();
                    //// $("#RoutePart").children().attr("disabled", "disabled");
                    //$("#dcacl").children().prop('disabled', true);
                    //$('#divMap').hide();
                },
                success: function (result) {
                    //$('#StruRelatedMov').show();
                    //$(".slidingpanelstructures").removeClass("show").addClass("hide");
                    //$('#StruRelatedMov').html($(result).find('#div_movement_list').html());
                    //$('#StruRelatedMov').append("<button type='button' class='btn_reg_back next btngrad btnrds btnbdr ifx-stp-map-bak-to-map'>Back</button>");
                    //CheckSessionTimeOut();
                    stopAnimation();

                    $('#dialogueSORTRelatedItems .modal-body').html($(result).find('#banner-container').html(), function () {
                        event.preventDefault();
                    });
                    $('#dialogueSORTRelatedItems .modal-body').find('div#filters').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('.table-filter-header').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#btnback').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#outsideFilterMoveInboxSORT').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#div_MoveList_MapSearch').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#folder-nav').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('tr td.no-records-found').attr('colspan', '10');
                    $('#dialogueSORTRelatedItems .modal-body').find('tr th span.typeimg').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('.relatedmov').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#filterimageprojectstatus').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#filterimageERNSort').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('#sortInboxTotalCount .absolute').remove();
                    $('#dialogueSORTRelatedItems .modal-body').find('.sorting').removeClass('sorting');
                    $('#dialogueSORTRelatedItems .modal-body').find('#sortInboxTotalCount .col-lg-3:first').addClass('col-lg-6');
                    $('#dialogueSORTRelatedItems .modal-body').find('#sortInboxTotalCount .col-lg-3:first').removeClass('col-lg-3');
                    $('#dialogueSORTRelatedItems').modal('show');

                },
                complete: function () {
                    $('#BackRevStructSummary').hide();
                    $("#IsMovListAvalabl").val("Yes");

                }
            });
        }
    });
}

function AddReturnRoutePart(data, mainRouteName, Animationflag, callBackFn) {

    var startDesc = data.result.StartDescr;
    var startEasting = data.result.StartEasting;
    var startNorthing = data.result.StartNorthing;
    var endDesc = data.result.EndDescr;
    var endEasting = data.result.EndEasting;
    var endNorthing = data.result.EndNorthing;

    loadmap('NOMAPDISPLAY');
    notifClearRoute();
    setReturnRouteType(true);
    setRoutePoint(startDesc, 0, startEasting, startNorthing, 0, function (result) {
        if (result == true) {
            setRoutePoint(endDesc, 1, endEasting, endNorthing, 1, function (result) {
                if (result == true) {
                    planRoute(false, function (result) {
                        if (result == true) {
                            routePartObj = getRouteDetails();
                            if ($('#PortalType').val() == '696008') {
                                try {
                                    if ($("#chkdockcaution").is(':checked')) {
                                        routePartObj.routePartDetails.Dockcaution = "True";
                                    }
                                }
                                catch (err) {}
                            }
                            routePartObj.routePartDetails.routeType = "planned";
                            routePartObj.routePartDetails.routeName = mainRouteName + " (Return)";
                            // Compress the data and convert to base64 for transmission
                            var compressedRoutePart = pako.gzip(JSON.stringify({ RoutePart: routePartObj }));
                            var base64RoutePartData = btoa(String.fromCharCode.apply(null, compressedRoutePart));
                            $.ajax({
                                url: '/Routes/SaveCompressedRoute',
                                type: 'POST',
                                async: true,
                                beforeSend: function () {
                                    startAnimation('Planning return leg');
                                },
                                data: JSON.stringify({ compressedRoutePart: base64RoutePartData, PlannedRouteId: 0, RouteFlag: 3, IsReturnRoute: true, VR1ContentRefNo: $('#CRNo').val() }),
                                success: function (val) {
                                    setReturnRouteType(false);
                                    callBackFn(val.value);
                                },
                                complete: function () {
                                    if (Animationflag != 1) {
                                        stopAnimation();
                                    }
                                }
                            });
                        }
                    });
                }
            });
        }
    });
}

function setReturnRouteType(type) {
    objifxStpMap.setReturnRouteType(type);
}

function setPathState(state) {
    objifxStpMap.setCurrentPathState(state);
}