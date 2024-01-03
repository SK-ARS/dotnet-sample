//script to manage RoutePart objectz

function IfxRouteManager() {
    this.RoutePart = {
        routePathList: [{
            routeSegmentList: [],
            routePointList: [],
            alternatePointList: [],
            allRoutePointList: [],
            allAlternatePointList: [],
            otherinfo: { features: [], routeType: 0, state: 'idle' },
            routePathType: 0,
            pathNo: 0,
            routePathNo: 1
        }],
        routePartDetails: {}
    };
}

IfxRouteManager.prototype.RoutePart = null;

IfxRouteManager.prototype.getSegmentCount = function (pathIndex) {
    return this.RoutePart.routePathList[pathIndex].routeSegmentList.length;
};

IfxRouteManager.prototype.getRoutePathCount = function () {
    return this.RoutePart.routePathList.length;
};

IfxRouteManager.prototype.getRoutePointCount = function (pathIndex) {
    return this.RoutePart.routePathList[pathIndex].routePointList.length;
};

IfxRouteManager.prototype.getAllRoutePointCount = function (pathIndex) {
    return this.RoutePart.routePathList[pathIndex].allRoutePointList.length;
};

IfxRouteManager.prototype.getAllAlternateRoutePointCount = function (pathIndex) {
    return this.RoutePart.routePathList[pathIndex].allAlternatePointList.length;
};

IfxRouteManager.prototype.setRoutePart = function (routePart) {
    return this.RoutePart = routePart;
};

IfxRouteManager.prototype.clearRoutePart = function (deleteMainRoute) {

    if (deleteMainRoute == true) {
        this.RoutePart = {
            routePathList: [{
                routeSegmentList: [],
                routePointList: [],
                alternatePointList: [],
                allRoutePointList: [],
                allAlternatePointList: [],
                otherinfo: { features: [], routeType: 0, state: 'idle' },
                routePathType: 0,
                pathNo: 0,
                routePathNo: 1
            }],
            routePartDetails: {}
        };
    }
    else {
        this.RoutePart.routePathList.splice(1, this.RoutePart.routePathList.length);
    }
};

IfxRouteManager.prototype.addRouteSegment = function (routeSegment, pathIndex) {
    this.RoutePart.routePathList[pathIndex].routeSegmentList.push(routeSegment);
};

IfxRouteManager.prototype.addAnnotation = function (pathIndex, segmentIndex, insertat, annotation) {
    if (insertat >= 0) {
        this.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList.splice(insertat, 0, annotation);
    }
    else {
        this.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList.push(annotation);
    }
};

IfxRouteManager.prototype.getAnnotation = function (pathIndex, segmentIndex, arrIndex) {
    return this.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList[arrIndex];
};

IfxRouteManager.prototype.getAnnotationCount = function (pathIndex, segmentIndex) {
    return this.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList.length;
};

IfxRouteManager.prototype.deleteAnnotation = function (pathIndex, segmentIndex, arrIndex) {
    this.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList.splice(arrIndex, 1);
}

IfxRouteManager.prototype.addRoutePath = function (routeType) {
    var routePath = {
        routeSegmentList: [],
        routePointList: [],
        alternatePointList: [],
        allRoutePointList: [],
        allAlternatePointList: [],
        otherinfo: { features: [], routeType: routeType, state: 'idle' }
    };
    routePath.routePathType = routeType;
    routePath.pathNo = this.RoutePart.routePathList.length;
    routePath.routePathNo = (routeType == 0 || routeType == 1) ? 1 : routeType;
    this.RoutePart.routePathList.push(routePath);
    return this.RoutePart.routePathList.length;
};

IfxRouteManager.prototype.removeRoutePath = function (routePathIndex) {
    this.RoutePart.routePathList.splice(routePathIndex, 1);
}



IfxRouteManager.prototype.getRoutePath = function (pathIndex) {
    return this.RoutePart.routePathList[pathIndex];
};

IfxRouteManager.prototype.getRouteSegment = function (pathIndex, segmentIndex) {
    return this.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex];
};

IfxRouteManager.prototype.getStateFromPointList = function (pathIndex) {
    var ptList = this.RoutePart.routePathList[pathIndex].routePointList;
    if (ptList == undefined || ptList.length == 0) {
        return 'idle';
    }
    if (ptList.length == 1) {
        if (ptList[0].pointType == 0 || ptList[0].pointType == 1)
            return 'firstpointselected';
    }
    else {
        if (ptList[1].pointType == 1)
            return 'secondpointselected';
        else
            if (ptList[0].pointType == 0)
                return 'firstpointselected';
    }

    return 'idle';
};

IfxRouteManager.prototype.addRoutePoint = function (pathIndex, pointPos, routePoint, insertAtLoc, alternateRoutePoint) {
    //check whether the same type point already exists
    if (routePoint.pointType < 2/*start or end*/) {
        if (this.RoutePart.routePathList[pathIndex].routePointList.length > routePoint.pointType) {
            if (this.RoutePart.routePathList[pathIndex].routePointList[routePoint.pointType].pointType != routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].routePointList.splice(routePoint.pointType, 0, routePoint);
                return;
            }
            else {
                this.RoutePart.routePathList[pathIndex].routePointList.push(routePoint);
                return;
            }
        }
    }

    if (pointPos != -1 && pointPos < this.RoutePart.routePathList[pathIndex].routePointList.length) {
        if (insertAtLoc == false)
            this.RoutePart.routePathList[pathIndex].routePointList[pointPos] = routePoint;
        else
            this.RoutePart.routePathList[pathIndex].routePointList.splice(pointPos, 0, routePoint);
    }
    else {
        if (this.RoutePart.routePathList[pathIndex].routePointList.length == 1) {
            if (this.RoutePart.routePathList[pathIndex].routePointList[0].pointType == routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].routePointList[0] = routePoint;
            }
            else {
                this.RoutePart.routePathList[pathIndex].routePointList.push(routePoint);
            }
        }
        else {
            this.RoutePart.routePathList[pathIndex].routePointList.push(routePoint);
        }
    }

    if (this.RoutePart.routePathList[0].routePointList.length >= 2) {
        if ((this.RoutePart.routePathList[0].routePointList[this.RoutePart.routePathList[0].routePointList.length - 1].pointType == this.RoutePart.routePathList[0].routePointList[this.RoutePart.routePathList[0].routePointList.length - 2].pointType)
            && (this.RoutePart.routePathList[0].routePointList[this.RoutePart.routePathList[0].routePointList.length - 1].routePointNo == this.RoutePart.routePathList[0].routePointList[this.RoutePart.routePathList[0].routePointList.length - 2].routePointNo)) {
            this.RoutePart.routePathList[0].routePointList.splice(this.RoutePart.routePathList[0].routePointList.length - 1, 1);
        }
    }

    this.addAlternateRoutePoint(pathIndex, pointPos, alternateRoutePoint, insertAtLoc);
    //this.addAllRoutePoint(pathIndex, pointPos, alternateRoutePoint, insertAtLoc, alternateRoutePoint);
};

IfxRouteManager.prototype.addAlternateRoutePoint = function (pathIndex, pointPos, routePoint, insertAtLoc) {
    //check whether the same type point already exists
    if (routePoint.pointType < 2/*start or end*/) {
        if (this.RoutePart.routePathList[pathIndex].alternatePointList.length > routePoint.pointType) {
            if (this.RoutePart.routePathList[pathIndex].alternatePointList[routePoint.pointType].pointType != routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].alternatePointList.splice(routePoint.pointType, 0, routePoint);
                return;
            }
            else {
                this.RoutePart.routePathList[pathIndex].alternatePointList.push(routePoint);
                return;
            }
        }
    }

    if (pointPos != -1 && pointPos < this.RoutePart.routePathList[pathIndex].alternatePointList.length) {
        if (insertAtLoc == false)
            this.RoutePart.routePathList[pathIndex].alternatePointList[pointPos] = routePoint;
        else
            this.RoutePart.routePathList[pathIndex].alternatePointList.splice(pointPos, 0, routePoint);
    }
    else {
        if (this.RoutePart.routePathList[pathIndex].alternatePointList.length == 1) {
            if (this.RoutePart.routePathList[pathIndex].alternatePointList[0].pointType == routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].alternatePointList[0] = routePoint;
            }
            else {
                this.RoutePart.routePathList[pathIndex].alternatePointList.push(routePoint);
            }
        }
        else {
            this.RoutePart.routePathList[pathIndex].alternatePointList.push(routePoint);
        }
    }
};

IfxRouteManager.prototype.addAllRoutePoint = function (pathIndex, pointPos, routePoint, insertAtLoc, alternateRoutePoint) {
    //check whether the same type point already exists
    if (routePoint.pointType < 2/*start or end*/) {
        if (this.RoutePart.routePathList[pathIndex].allRoutePointList.length > routePoint.pointType) {
            if (this.RoutePart.routePathList[pathIndex].allRoutePointList[routePoint.pointType].pointType != routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].allRoutePointList.splice(routePoint.pointType, 0, routePoint);
                return;
            }
            else {
                this.RoutePart.routePathList[pathIndex].allRoutePointList.push(routePoint);
                return;
            }
        }
    }

    if (pointPos != -1 && pointPos < this.RoutePart.routePathList[pathIndex].allRoutePointList.length) {
        if (insertAtLoc == false)
            this.RoutePart.routePathList[pathIndex].allRoutePointList[pointPos] = routePoint;
        else
            this.RoutePart.routePathList[pathIndex].allRoutePointList.splice(pointPos, 0, routePoint);
    }
    else {
        if (this.RoutePart.routePathList[pathIndex].allRoutePointList.length == 1) {
            if (this.RoutePart.routePathList[pathIndex].allRoutePointList[0].pointType == routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].allRoutePointList[0] = routePoint;
            }
            else {
                this.RoutePart.routePathList[pathIndex].allRoutePointList.push(routePoint);
            }
        }
        else {
            this.RoutePart.routePathList[pathIndex].allRoutePointList.push(routePoint);
        }
    }

    if (this.RoutePart.routePathList[0].allRoutePointList.length >= 2) {
        if ((this.RoutePart.routePathList[0].allRoutePointList[this.RoutePart.routePathList[0].allRoutePointList.length - 1].pointType == this.RoutePart.routePathList[0].allRoutePointList[this.RoutePart.routePathList[0].allRoutePointList.length - 2].pointType)
            && (this.RoutePart.routePathList[0].allRoutePointList[this.RoutePart.routePathList[0].allRoutePointList.length - 1].routePointNo == this.RoutePart.routePathList[0].allRoutePointList[this.RoutePart.routePathList[0].allRoutePointList.length - 2].routePointNo)) {
            this.RoutePart.routePathList[0].allRoutePointList.splice(this.RoutePart.routePathList[0].allRoutePointList.length - 1, 1);
        }
    }

    this.addAllAlternateRoutePoint(pathIndex, pointPos, alternateRoutePoint, insertAtLoc);
};

IfxRouteManager.prototype.addAllAlternateRoutePoint = function (pathIndex, pointPos, routePoint, insertAtLoc) {
    //check whether the same type point already exists
    if (routePoint.pointType < 2/*start or end*/) {
        if (this.RoutePart.routePathList[pathIndex].allAlternatePointList.length > routePoint.pointType) {
            if (this.RoutePart.routePathList[pathIndex].allAlternatePointList[routePoint.pointType].pointType != routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].allAlternatePointList.splice(routePoint.pointType, 0, routePoint);
                return;
            }
            else {
                this.RoutePart.routePathList[pathIndex].allAlternatePointList.push(routePoint);
                return;
            }
        }
    }

    if (pointPos != -1 && pointPos < this.RoutePart.routePathList[pathIndex].allAlternatePointList.length) {
        if (insertAtLoc == false)
            this.RoutePart.routePathList[pathIndex].allAlternatePointList[pointPos] = routePoint;
        else
            this.RoutePart.routePathList[pathIndex].allAlternatePointList.splice(pointPos, 0, routePoint);
    }
    else {
        if (this.RoutePart.routePathList[pathIndex].allAlternatePointList.length == 1) {
            if (this.RoutePart.routePathList[pathIndex].allAlternatePointList[0].pointType == routePoint.pointType) {
                this.RoutePart.routePathList[pathIndex].allAlternatePointList[0] = routePoint;
            }
            else {
                this.RoutePart.routePathList[pathIndex].allAlternatePointList.push(routePoint);
            }
        }
        else {
            this.RoutePart.routePathList[pathIndex].allAlternatePointList.push(routePoint);
        }
    }
};

IfxRouteManager.prototype.removeRouteSegment = function (pathIndex, segmentIndex) {
    this.RoutePart.routePathList[pathIndex].routeSegmentList.splice(segmentIndex, 1);
};

IfxRouteManager.prototype.insertRouteSegment = function (pathIndex, segmentIndex, obj, removeAndInsert) {
    if (removeAndInsert)
        this.RoutePart.routePathList[pathIndex].routeSegmentList.splice(segmentIndex, 1, obj);
    else
        this.RoutePart.routePathList[pathIndex].routeSegmentList.splice(segmentIndex, 0, obj);
};

IfxRouteManager.prototype.getRoutePoint = function (pathIndex, pointIndex) {
    if (pointIndex < this.RoutePart.routePathList[pathIndex].routePointList.length) {
        return this.RoutePart.routePathList[pathIndex].routePointList[pointIndex];
    }
    else
        return null;
};

IfxRouteManager.prototype.getAllRoutePoint = function (pathIndex, pointIndex) {
    if (pointIndex < this.RoutePart.routePathList[pathIndex].allRoutePointList.length) {
        return this.RoutePart.routePathList[pathIndex].allRoutePointList[pointIndex];
    }
    else
        return null;
};

IfxRouteManager.prototype.removeRoutePoint = function (pathIndex, pointIndex) {
    this.RoutePart.routePathList[pathIndex].routePointList.splice(pointIndex, 1);
    this.removeAlternateRoutePoint(pathIndex, pointIndex);
};

IfxRouteManager.prototype.removeAlternateRoutePoint = function (pathIndex, pointIndex) {
    this.RoutePart.routePathList[pathIndex].alternatePointList.splice(pointIndex, 1);
};

IfxRouteManager.prototype.removeAllRoutePoint = function (pathIndex, pointIndex) {
    this.RoutePart.routePathList[pathIndex].allRoutePointList.splice(pointIndex, 1);
    this.removeAllAlternateRoutePoint(pathIndex, pointIndex);
};

IfxRouteManager.prototype.removeAllAlternateRoutePoint = function (pathIndex, pointIndex) {
    this.RoutePart.routePathList[pathIndex].allAlternatePointList.splice(pointIndex, 1);
};

IfxRouteManager.prototype.swapRoutePoint = function (pathIndex, pointIndex1, pointIndex2) {
    var tmp = this.RoutePart.routePathList[pathIndex].routePointList[pointIndex1];
    this.RoutePart.routePathList[pathIndex].routePointList[pointIndex1] = this.RoutePart.routePathList[pathIndex].routePointList[pointIndex2];
    this.RoutePart.routePathList[pathIndex].routePointList[pointIndex2] = tmp;
    this.swapAlternateRoutePoint(pathIndex, pointIndex1, pointIndex2);
};

IfxRouteManager.prototype.swapAlternateRoutePoint = function (pathIndex, pointIndex1, pointIndex2) {
    var tmp = this.RoutePart.routePathList[pathIndex].alternatePointList[pointIndex1];
    this.RoutePart.routePathList[pathIndex].alternatePointList[pointIndex1] = this.RoutePart.routePathList[pathIndex].alternatePointList[pointIndex2];
    this.RoutePart.routePathList[pathIndex].alternatePointList[pointIndex2] = tmp;
};

/*----changes for drag route----*/
/*IfxRouteManager.prototype.updateRoutePoint = function (pathIndex, pointIndex1, routePoint) {
    
    if (parseInt(pointIndex1) >= this.RoutePart.routePathList[pathIndex].routePointList.length) {
        this.RoutePart.routePathList[pathIndex].routePointList[pointIndex1] = routePoint;
    }
    else {
        this.RoutePart.routePathList[pathIndex].routePointList.splice(pointIndex1, 0, routePoint);
        for (var i = pointIndex1 + 1; i < this.RoutePart.routePathList[pathIndex].routePointList.length; i++) {
            this.RoutePart.routePathList[pathIndex].routePointList[i].routePointNo++;
        }
    }
};*/

IfxRouteManager.prototype.updateRoutePoint = function (pathIndex, pointIndex1, routePoint) {
    this.RoutePart.routePathList[pathIndex].routePointList[pointIndex1] = routePoint;
};

IfxRouteManager.prototype.updateAlternateRoutePoint = function (pathIndex, pointIndex1, routePoint) {
   this.RoutePart.routePathList[pathIndex].alternatePointList[pointIndex1] = routePoint;
};

IfxRouteManager.prototype.updateAllRoutePoint = function (pathIndex, pointIndex1, routePoint) {
    //this.RoutePart.routePathList[pathIndex].allRoutePointList.splice(pointIndex1, 1, routePoint);
    this.RoutePart.routePathList[pathIndex].allRoutePointList[pointIndex1] = routePoint;
};

IfxRouteManager.prototype.updateAllAlternateRoutePoint = function (pathIndex, pointIndex1, routePoint) {
    this.RoutePart.routePathList[pathIndex].allAlternatePointList[pointIndex1] = routePoint;
};

IfxRouteManager.prototype.resetAllRoutePoint = function (pathIndex) {
    if (this.RoutePart.routePathList[pathIndex].routePointList.length > 0) {
        this.RoutePart.routePathList[pathIndex].allRoutePointList =
            this.RoutePart.routePathList[pathIndex].routePointList.map(a => Object.assign({}, a));
    }
};

IfxRouteManager.prototype.resetAllAlternateRoutePoint = function (pathIndex) {
    if (this.RoutePart.routePathList[pathIndex].alternatePointList.length > 0) {
        this.RoutePart.routePathList[pathIndex].allAlternatePointList =
            this.RoutePart.routePathList[pathIndex].alternatePointList.map(a => Object.assign({}, a));
    }
};

IfxRouteManager.prototype.updateRoutePointField = function (pathIndex, pointIndex, field, value) {
    this.RoutePart.routePathList[pathIndex].routePointList[pointIndex][field] = value;
}

IfxRouteManager.prototype.updateAlternateRoutePointField = function (pathIndex, pointIndex, field, value) {
    this.RoutePart.routePathList[pathIndex].alternatePointList[pointIndex][field] = value;
}

IfxRouteManager.prototype.moveRoutePointPos = function (pathIndex, from, to) {
    this.RoutePart.routePathList[pathIndex].routePointList.splice(to, 0, this.RoutePart.routePathList[pathIndex].routePointList.splice(from, 1)[0]);
    this.RoutePart.routePathList[pathIndex].allRoutePointList.splice(to, 0, this.RoutePart.routePathList[pathIndex].allRoutePointList.splice(from, 1)[0]);
};

IfxRouteManager.prototype.moveAlternateRoutePointPos = function (pathIndex, from, to) {
    this.RoutePart.routePathList[pathIndex].alternatePointList.splice(to, 0, this.RoutePart.routePathList[pathIndex].alternatePointList.splice(from, 1)[0]);
};

IfxRouteManager.prototype.formatRouteRequest = function (pathIndex, isAlternate, alternatePointIndex) {
    if (this.getAllRoutePointCount(pathIndex) < 2)
        return null;

    var routeViaPoint = {
        WayPoints: [],
        MaxHeight: "0",
        MaxWeight: "0",
        MaxLength: "0",
        MaxWidth: "0",
        MaxNormAxleLoad: "0",
        MaxShutAxleLoad: "0"
    };

    routeViaPoint.BeginStartNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[0].otherinfo.beginNodeId;
    routeViaPoint.BeginPointLinkId = this.RoutePart.routePathList[pathIndex].allRoutePointList[0].otherinfo.linkId;
    routeViaPoint.BeginPointEndNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[0].otherinfo.endNodeId;

    routeViaPoint.EndPointStartNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[1].otherinfo.beginNodeId;
    routeViaPoint.EndPointLinkId = this.RoutePart.routePathList[pathIndex].allRoutePointList[1].otherinfo.linkId;
    routeViaPoint.EndPointEndNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[1].otherinfo.endNodeId;

    var WayPoints = [];

    //add waypoints
    if (isAlternate == undefined || isAlternate == null || !isAlternate) {
        for (var i = 2; i < this.RoutePart.routePathList[pathIndex].allRoutePointList.length; i++) {
            var WayPoint = {};
            WayPoint.WayPointBeginNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[i].otherinfo.beginNodeId;
            WayPoint.WayPointLinkId = this.RoutePart.routePathList[pathIndex].allRoutePointList[i].otherinfo.linkId;
            WayPoint.WayPointEndNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[i].otherinfo.endNodeId;
            routeViaPoint.WayPoints.push(WayPoint);
        }
    }
    else {
        for (var i = 2; i < this.RoutePart.routePathList[pathIndex].allAlternatePointList.length; i++) {
            var WayPoint = {};
            if (i == alternatePointIndex) {
                WayPoint.WayPointBeginNode = this.RoutePart.routePathList[pathIndex].allAlternatePointList[i].otherinfo.beginNodeId;
                WayPoint.WayPointLinkId = this.RoutePart.routePathList[pathIndex].allAlternatePointList[i].otherinfo.linkId;
                WayPoint.WayPointEndNode = this.RoutePart.routePathList[pathIndex].allAlternatePointList[i].otherinfo.endNodeId;
            }
            else {
                WayPoint.WayPointBeginNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[i].otherinfo.beginNodeId;
                WayPoint.WayPointLinkId = this.RoutePart.routePathList[pathIndex].allRoutePointList[i].otherinfo.linkId;
                WayPoint.WayPointEndNode = this.RoutePart.routePathList[pathIndex].allRoutePointList[i].otherinfo.endNodeId;
            }
            routeViaPoint.WayPoints.push(WayPoint);
        }
    }

    return routeViaPoint;
};

IfxRouteManager.prototype.doProcessPlanRouteRequest = function (routeRequest, callback) {
    $.ajax({
        type: "POST",
        datatype: "json",
        url: "/RoutePlannerInterface/PostEx",
        //contentType: 'application/json; charset=utf-8',

        data: JSON.stringify({ routeViaPointEx: routeRequest }),

        success: function (reply, textStatus, jqXHR) {
            if (reply != "") {
                if (callback && typeof (callback) === "function") {
                    callback(reply.ListSegments);
                }
            }
            else {
                if (callback && typeof (callback) === "function") {
                    callback(null);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (callback && typeof (callback) === "function") {
                callback(null);
            }
        }
    });
}

IfxRouteManager.prototype.getCQLFilerFromLinkIds = function (response) {
    var strcql = "";
    var cqlFilters = new Array();

    if (response && response.length > 0) {
        var strcql = "LINK_ID=" + response[response.length - 1];
        var count = 0;

        for (var i = response.length - 2; i >= 0; i--) {
            count++;
            if (count >= 50) {
                cqlFilters.push(strcql);
                count = 0;
                strcql = "LINK_ID=" + response[i];
                continue;
            }

            strcql = strcql + "%20OR%20LINK_ID=" + response[i];
        }

        cqlFilters.push(strcql);
    }
    return cqlFilters;
}
function MakeData(routePart) {
    for (var i = 0; i < routePart.routePathList[0].routePointList.length;) {

        if (routePart.routePathList[0].routePointList[i].pointGeom == null) {
            routePart.routePathList[0].routePointList.splice(i, 1);
            $('#Pointtype option[value=' + i + ']').remove();
        }
        else i++;
    }
}

IfxRouteManager.prototype.processRoutePart = function (isReturnLeg, routeObj) {
    if (isReturnLeg != undefined && isReturnLeg == true) {
        var routePart = jQuery.extend(true, {}, routeObj.RoutePart);
    }
    else {
        var routePart = jQuery.extend(true, {}, this.RoutePart);
    }
    delete routePart.otherinfo;
    if ($("#hIs_NEN").val() == "true") { MakeData(routePart); }

    for (var i = 0; i < routePart.routePathList.length; i++) {
        delete routePart.routePathList[i].alternatePointList;
        delete routePart.routePathList[i].allRoutePointList;
        delete routePart.routePathList[i].allAlternatePointList;
        delete routePart.routePathList[i].otherinfo;
        delete routePart.routePathList[i].pathNo;

        for (var j = 0; j < routePart.routePathList[i].routePointList.length; j++) {
            delete routePart.routePathList[i].routePointList[j].otherinfo;
            routePart.routePathList[i].routePointList[j].pointGeom = {
                sdo_point: {
                    X: routePart.routePathList[i].routePointList[j].pointGeom.sdo_point.X,
                    Y: routePart.routePathList[i].routePointList[j].pointGeom.sdo_point.Y
                },
                sdo_gtype: IfxStpmapCommon.getSdo_gtype('POINT'),
                sdo_srid: IfxStpmapCommon.getSdo_srid()
            };
        }

        for (var j = 0; j < routePart.routePathList[i].routeSegmentList.length; j++) {
            delete routePart.routePathList[i].routeSegmentList[j].otherinfo;
            routePart.routePathList[i].routeSegmentList[j].startPointGeometry = {
                sdo_point: {
                    X: routePart.routePathList[i].routeSegmentList[j].startPointGeometry.sdo_point.X,
                    Y: routePart.routePathList[i].routeSegmentList[j].startPointGeometry.sdo_point.Y
                },
                sdo_gtype: IfxStpmapCommon.getSdo_gtype('POINT'),
                sdo_srid: IfxStpmapCommon.getSdo_srid()
            };
            routePart.routePathList[i].routeSegmentList[j].endPointGeometry = {
                sdo_point: {
                    X: routePart.routePathList[i].routeSegmentList[j].endPointGeometry.sdo_point.X,
                    Y: routePart.routePathList[i].routeSegmentList[j].endPointGeometry.sdo_point.Y
                },
                sdo_gtype: IfxStpmapCommon.getSdo_gtype('POINT'),
                sdo_srid: IfxStpmapCommon.getSdo_srid()
            };
            routePart.routePathList[i].routeSegmentList[j].segmentType = IfxStpmapCommon.getSegmentTypeID(routePart.routePathList[i].routeSegmentList[j].segmentType);

            if (routePart.routePathList[i].routeSegmentList[j].segmentType == 3) {
                routePart.routePathList[i].routeSegmentList[j].offRoadGeometry = {
                    OrdinatesArray: routePart.routePathList[i].routeSegmentList[j].offRoadGeometry.OrdinatesArray,
                    ElemArray: [1, 2, 1],
                    sdo_gtype: IfxStpmapCommon.getSdo_gtype('LINE'),
                    sdo_srid: IfxStpmapCommon.getSdo_srid()
                };
            }
            if (routePart.routePathList[i].routeSegmentList[j].routeAnnotationsList != null) {
                for (var k = 0; k < routePart.routePathList[i].routeSegmentList[j].routeAnnotationsList.length; k++) {
                    delete routePart.routePathList[i].routeSegmentList[j].routeAnnotationsList[k].otherinfo;
                    routePart.routePathList[i].routeSegmentList[j].routeAnnotationsList[k].geometry = {
                        sdo_point: {
                            X: routePart.routePathList[i].routeSegmentList[j].routeAnnotationsList[k].geometry.sdo_point.X,
                            Y: routePart.routePathList[i].routeSegmentList[j].routeAnnotationsList[k].geometry.sdo_point.Y
                        },
                        sdo_gtype: IfxStpmapCommon.getSdo_gtype('POINT'),
                        sdo_srid: IfxStpmapCommon.getSdo_srid()
                    }
                }
            }

            this.updateRouteLinkList(routePart.routePathList[i].routeSegmentList[j]);
        }
    }

    return routePart;
}

IfxRouteManager.prototype.getRoutePart = function (isReturnLeg, routeObj) {
    var routePart = this.processRoutePart(isReturnLeg, routeObj);
    return routePart;
}

IfxRouteManager.prototype.getRouteId = function () {
    return this.RoutePart.routePartDetails.routeID;
}

IfxRouteManager.prototype.createRouteLinkList = function (routeLinkIds, direction) {
    var routeLinkList = [];
    for (i = 0; i < routeLinkIds.length; i++) {
        var routeLink = {};

        //routeLink['linkId'] = (Number(routeLinkIds[i]) * 10) + 100000000000;
        routeLink['linkId'] = Number(routeLinkIds[i]);
        routeLink['linkNo'] = i + 1;
        if (direction != undefined && direction != null && direction[i] != undefined && direction[i] != null)
            routeLink['direction'] = direction[i];
        else
            routeLink['direction'] = 1;

        routeLinkList.push(routeLink);
    }
    return routeLinkList;
}

IfxRouteManager.prototype.updateRouteLinkList = function (routeSegment) {
    if (routeSegment.routeLinkList.length > 0)// sorting routeLinkList based on linkNo 
    {
        routeSegment.routeLinkList.sort((a, b) => a.linkNo - b.linkNo);
    }
    if (routeSegment.routeLinkList.length > 0 && routeSegment.routeLinkList[0].linkId == routeSegment.startLinkId) {
        for (var i = 0; i < routeSegment.routeLinkList.length; i++) {
            routeSegment.routeLinkList[i].linkNo = i;
        }
    }
    else {
        var routeLinkObj = {
            linkId: routeSegment.startLinkId,
            linkNo: 0,
            direction: routeSegment.startPointDirection
        };
        if (routeSegment.routeLinkList.length > 0)
            routeSegment.routeLinkList.splice(0, 0, routeLinkObj);
        else
            routeSegment.routeLinkList.push(routeLinkObj);
    }
    if (routeSegment.routeLinkList.length > 0 && routeSegment.routeLinkList[routeSegment.routeLinkList.length - 1].linkId != routeSegment.endLinkId) {
        routeLinkObj = {
            linkId: routeSegment.endLinkId,
            linkNo: routeSegment.routeLinkList.length,
            direction: routeSegment.endPointDirection
        };
        routeSegment.routeLinkList.push(routeLinkObj);
    }
    return routeSegment;
}

IfxRouteManager.prototype.createRouteSegmentObject = function (routeLinkIds, rpStart, rpEnd, offRoadGeometry, segmentNo, segmentType, direction) {
    var routeSegment = { routeLinkList: [], otherinfo: {} };

    routeSegment.segmentNo = segmentNo;
    routeSegment.segmentType = segmentType;
    routeSegment.routeAnnotationsList = [];
    if (IfxStpmapCommon.getSegmentTypeID(segmentType) == 1) {
        routeSegment.startLinkId = rpStart.linkId;
        if (routeSegment.startLinkId == undefined || routeSegment.startLinkId=="") {
            routeSegment.startLinkId = rpEnd.linkId;
        }
        routeSegment.endLinkId = rpEnd.linkId;
        if (routeSegment.endLinkId == undefined || routeSegment.endLinkId =="") {
            routeSegment.endLinkId = rpStart.linkId;
        }
        routeSegment.startPointGeometry = jQuery.extend(true, {}, rpStart.pointGeom);
        routeSegment.endPointGeometry = jQuery.extend(true, {}, rpEnd.pointGeom);
        routeSegment.startLrs = rpStart.lrs;
        routeSegment.endLrs = rpEnd.lrs;
        routeSegment.startPointDirection = rpStart.direction;
        routeSegment.endPointDirection = rpEnd.direction;
        routeSegment.routeLinkList = this.createRouteLinkList(routeLinkIds, direction);
        routeSegment.otherinfo.startSegmentfeature = rpStart.otherinfo.pointfeature;
        routeSegment.otherinfo.endSegmentfeature = rpEnd.otherinfo.pointfeature;
        routeSegment.otherinfo.completefeatures = [];
    }
    else if (IfxStpmapCommon.getSegmentTypeID(segmentType) == 3) {
        if (offRoadGeometry != null) {
            routeSegment.offRoadGeometry = { OrdinatesArray: [] };
            routeSegment.offRoadGeometry.OrdinatesArray = offRoadGeometry;
            routeSegment.startPointGeometry = { sdo_point: { X: '', Y: '' } };
            routeSegment.endPointGeometry = { sdo_point: { X: '', Y: '' } };
            routeSegment.startPointGeometry.sdo_point.X = routeSegment.offRoadGeometry.OrdinatesArray[0];
            routeSegment.startPointGeometry.sdo_point.Y = routeSegment.offRoadGeometry.OrdinatesArray[1];
            routeSegment.endPointGeometry.sdo_point.X = routeSegment.offRoadGeometry.OrdinatesArray[routeSegment.offRoadGeometry.OrdinatesArray.length - 2];
            routeSegment.endPointGeometry.sdo_point.Y = routeSegment.offRoadGeometry.OrdinatesArray[routeSegment.offRoadGeometry.OrdinatesArray.length - 1];
        }
    }
    else {
        if (routeLinkIds != undefined && routeLinkIds != null) {
            routeSegment.startLinkId = rpStart.linkId;
            routeSegment.endLinkId = rpEnd.linkId;
            routeSegment.startPointGeometry = jQuery.extend(true, {}, rpStart.pointGeom);
            routeSegment.endPointGeometry = jQuery.extend(true, {}, rpEnd.pointGeom);
            routeSegment.startLrs = rpStart.lrs;
            routeSegment.endLrs = rpEnd.lrs;
            routeSegment.startPointDirection = rpStart.direction;
            routeSegment.endPointDirection = rpEnd.direction;
            routeSegment.routeLinkList = this.createRouteLinkList(routeLinkIds, direction);
            routeSegment.otherinfo.startSegmentfeature = rpStart.otherinfo.pointfeature;
            routeSegment.otherinfo.endSegmentfeature = rpEnd.otherinfo.pointfeature;
            routeSegment.otherinfo.completefeatures = [];
        }
        else {
            routeSegment.routeLinkList = [];
            routeSegment.startPointGeometry = { sdo_point: { X: '', Y: '' } };
            routeSegment.endPointGeometry = { sdo_point: { X: '', Y: '' } };
            routeSegment.otherinfo.isAdded = true;
            routeSegment.otherinfo.completefeatures = [];
            routeSegment.otherinfo.isComplete = false;
        }
    }
    return routeSegment;
}

IfxRouteManager.prototype.setRoutePathState = function (pathIndex, pathState) {
    if (this.RoutePart.routePathList[pathIndex].otherinfo == undefined) {
        this.RoutePart.routePathList[pathIndex].otherinfo = { completefeatures: [], features: [] };
    }
    this.RoutePart.routePathList[pathIndex].otherinfo.state = pathState;
}

IfxRouteManager.prototype.setPathNo = function (pathIndex, pathNo) {
    this.RoutePart.routePathList[pathIndex].pathNo = pathNo;
}

IfxRouteManager.prototype.getRoutePathState = function (pathIndex) {
    if (this.RoutePart.routePathList[pathIndex].otherinfo && this.RoutePart.routePathList[pathIndex].otherinfo.state &&
        (this.RoutePart.routePathList[pathIndex].otherinfo.state == 'routeplanned' ||
        this.RoutePart.routePathList[pathIndex].otherinfo.state == 'routeplanninginprogress' ||
        this.RoutePart.routePathList[pathIndex].otherinfo.state == 'routedisplayed'))
        return this.RoutePart.routePathList[pathIndex].otherinfo.state;
    return this.getStateFromPointList(pathIndex);
}

IfxRouteManager.prototype.setRoutePart = function (routePart) {
    
        if (routePart.routePathList[0].alternatePointList == null) {
            routePart.routePathList[0].alternatePointList = routePart.routePathList[0].routePointList;
        }
   
    this.RoutePart = routePart;
}

IfxRouteManager.prototype.findStartAndEndPointsFromRoutePointList = function (pathIndex) {
    var rPath = this.RoutePart.routePathList[pathIndex];
    var startIndex = null, endIndex = null;
    for (var i = 0; i < rPath.routePointList.length; i++) {
        if (startIndex == null && rPath.routePointList[i].pointType == 0) {
            startIndex = i;
        }
        else if (endIndex == null && rPath.routePointList[i].pointType == 1) {
            endIndex = i;
        }
        if (startIndex != null && endIndex != null) {
            break;
        }
    }
    return { startIndex: startIndex, endIndex: endIndex };
}

IfxRouteManager.prototype.reArrangeRouteSegmentList = function (pathIndex) {
    var rSegmentList = [];
    var rPath = this.RoutePart.routePathList[pathIndex];
    var points = this.findStartAndEndPointsFromRoutePointList(pathIndex);
    var startRoutePoint = rPath.routePointList[points.startIndex].pointGeom.sdo_point;
    var endRoutePoint = rPath.routePointList[points.endIndex].pointGeom.sdo_point;
    var traversedList = [];
    var reverseSeg = [];

    var tempStartPoint = jQuery.extend(true, {}, startRoutePoint);
    var temprsegment = [];
    for (var k = rPath.routeSegmentList.length - 1;k>=0; k--) {
        
        if (IfxStpmapCommon.getSegmentTypeID(rPath.routeSegmentList[k].segmentType) == 2) {
            temprsegment.push(rPath.routeSegmentList[k]);
            rPath.routeSegmentList.splice(k, 1);
        }
    }
    if (temprsegment.length > 0) {
        for (var l = 0; l < temprsegment.length; l++) {
            rPath.routeSegmentList.push(temprsegment[l]);
        }

    }

    for (var i = 0; i < rPath.routeSegmentList.length; i++) {
        var flag = 0;
        for (var j = 0; j < traversedList.length; j++) {
            if (traversedList[j] == i) {
                flag = 1;
                break;
            }
        }
        if (IfxStpmapCommon.getSegmentTypeID(rPath.routeSegmentList[i].segmentType) == 2) {
            reverseSeg.push(i);
            traversedList.push(i);
            if (reverseSeg.length==temprsegment.length)
            break;
        }
        if (flag == 0) {
            if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[i].startPointGeometry.sdo_point, tempStartPoint)) {
                rSegmentList.push(rPath.routeSegmentList[i]);
                rSegmentList[rSegmentList.length - 1].segmentNo = rSegmentList.length;
                tempStartPoint = jQuery.extend(true, {}, rPath.routeSegmentList[i].endPointGeometry.sdo_point);
                traversedList.push(i);
                i = -1;
            }
            else if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[i].endPointGeometry.sdo_point, tempStartPoint)) {
                rSegmentList.push(rPath.routeSegmentList[i]);
                rSegmentList[rSegmentList.length - 1].segmentNo = rSegmentList.length;
                tempStartPoint = jQuery.extend(true, {}, rPath.routeSegmentList[i].startPointGeometry.sdo_point);
                traversedList.push(i);
                i = -1;
            }
        }
    }

    for (var i = 0; i < reverseSeg.length; i++) {
        for (var j = 0; j < rSegmentList.length; j++) {
            if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[reverseSeg[i]].startPointGeometry.sdo_point, rSegmentList[j].endPointGeometry.sdo_point)) {
                rSegmentList.splice(j + 1, 0, jQuery.extend(true, {}, rPath.routeSegmentList[reverseSeg[i]]));
                break;
            }
        }
    }

    if (rSegmentList.length != rPath.routeSegmentList.length) {
        for (var i = 0; i < rPath.routeSegmentList.length; i++) {
            var flag = 0;
            for (var j = 0; j < rSegmentList.length; j++) {
                if (rSegmentList[j].startPointGeometry.sdo_point.X == rPath.routeSegmentList[i].startPointGeometry.sdo_point.X && rSegmentList[j].endPointGeometry.sdo_point.X == rPath.routeSegmentList[i].endPointGeometry.sdo_point.X
                    && rSegmentList[j].startPointGeometry.sdo_point.Y == rPath.routeSegmentList[i].startPointGeometry.sdo_point.Y && rSegmentList[j].endPointGeometry.sdo_point.Y == rPath.routeSegmentList[i].endPointGeometry.sdo_point.Y) {
                    flag = 1;
                    break;
                }
            }
            if (flag == 0) {
                rSegmentList.push(rPath.routeSegmentList[i]);
                rSegmentList[rSegmentList.length - 1].segmentNo = rSegmentList.length;
            }
        }
    }

    if (reverseSeg.length > 0) {
        for (var i = 0; i < rSegmentList.length; i++) {
            rSegmentList[i].segmentNo = i + 1;
        }
    }

    rPath.routeSegmentList = [];
    rPath.routeSegmentList = rSegmentList;
}