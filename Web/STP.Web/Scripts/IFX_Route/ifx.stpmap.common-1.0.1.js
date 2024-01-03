function IfxStpmapCommon() {
}

//function to find the feature of a particular linkID from the feature list
IfxStpmapCommon.getFeatureOfLinkId = function (features, routeLinkId) {
    for (var i = 0; i < features.length; i++) {
        if (features[i].attributes.LINK_ID == routeLinkId) {
            return features[i];
        }
    }
    return null;
}

IfxStpmapCommon.getFeatureOfLinkIdFromPathDetails = function (routePath, linkId) {
    for (var i = 0; i < routePath.routeSegmentList.length; i++) {

        if (routePath.routeSegmentList[i].offRoadGeometry != null)
            continue;

        var feature = IfxStpmapCommon.getFeatureOfLinkId(routePath.routeSegmentList[i].otherinfo.features, linkId);

        if (feature != null) {
            return feature;
        }

        if (routePath.routeSegmentList[i].otherinfo.startSegmentfeature && routePath.routeSegmentList[i].otherinfo.startSegmentfeature.attributes.LINK_ID == linkId)
            return routePath.routeSegmentList[i].otherinfo.startSegmentfeature;

        if (routePath.routeSegmentList[i].otherinfo.endSegmentfeature && routePath.routeSegmentList[i].otherinfo.endSegmentfeature.attributes.LINK_ID == linkId)
            return routePath.routeSegmentList[i].otherinfo.endSegmentfeature;
    }
    return null;
}

//function to create a feature for start/end point
IfxStpmapCommon.getRoutePointSegmentFeature = function (rpPointGeometry, rpFeature, nextSegmentFeature, pointType) {
    if (rpFeature == null || nextSegmentFeature == null) {
        if (rpPointGeometry != null) {
            var pointArr = new Array();
            pointArr.push(new OpenLayers.Geometry.Point(rpPointGeometry.sdo_point.X, rpPointGeometry.sdo_point.Y));
            return { feature: new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(pointArr)) };
        }
        else {
            return null;
        }
    }

    if (rpPointGeometry == null) {
        rpPointGeometry = {};
        rpPointGeometry.sdo_point = {};
        rpPointGeometry.sdo_point.X = rpFeature.geometry.components[0].x;
        rpPointGeometry.sdo_point.Y = rpFeature.geometry.components[0].y;
    }

    var lrsMeasure = LRSMeasure(rpFeature.geometry, new OpenLayers.Geometry.Point(rpPointGeometry.sdo_point.X, rpPointGeometry.sdo_point.Y), { tolerance: 0.5, details: true });

    if (rpFeature.attributes.REF_IN_ID == nextSegmentFeature.attributes.NREF_IN_ID ||
        rpFeature.attributes.REF_IN_ID == nextSegmentFeature.attributes.REF_IN_ID) {
        if (lrsMeasure.measure != 0) {
            var line = lrsMeasure.subString1;
        }
        else {
            var part1Points = [];
            part1Points.push(rpFeature.geometry.components[0]);
            var line = new OpenLayers.Geometry.LineString(part1Points);
        }
        if (pointType == 'STARTPOINT')
            var direction = 0;
        else
            var direction = 1;
    }
    else if (rpFeature.attributes.NREF_IN_ID == nextSegmentFeature.attributes.NREF_IN_ID ||
        rpFeature.attributes.NREF_IN_ID == nextSegmentFeature.attributes.REF_IN_ID) {
        if (lrsMeasure.measure != 1) {
            var line = lrsMeasure.subString2;
        }
        else {
            var part1Points = [];
            part1Points.push(rpFeature.geometry.components[0]);
            var line = new OpenLayers.Geometry.LineString(part1Points);
        }
        if (pointType == 'STARTPOINT')
            var direction = 1;
        else
            var direction = 0;
    }

    return { feature: new OpenLayers.Feature.Vector(line), direction: direction };
};

IfxStpmapCommon.getCQLFilerFromRouteLinkList = function (routeLinkList) {
    var cqlFilters = new Array();

    if (routeLinkList && routeLinkList.length > 0) {
        var strcql = "LINK_ID=" + routeLinkList[routeLinkList.length - 1].linkId;
        var count = 0;
        for (var i = routeLinkList.length - 2; i >= 0; i--) {
            count++;
            if (count >= 30) {
                cqlFilters.push(strcql);
                count = 0;
                strcql = "LINK_ID=" + routeLinkList[i].linkId;
                continue;
            }
            strcql = strcql + "%20OR%20LINK_ID=" + routeLinkList[i].linkId;
        }

        cqlFilters.push(strcql);
    }
    return cqlFilters;


}

IfxStpmapCommon.getCQLFilerFromLinkIDs = function (linkIDs) {
    var cqlFilters = new Array();

    if (linkIDs && linkIDs.length > 0) {
        var strcql = ""
        if (linkIDs[linkIDs.length - 1] != null) {
            strcql = "LINK_ID=" + linkIDs[linkIDs.length - 1];
        }

        var count = 0;
        for (var i = linkIDs.length - 2; i >= 0; i--) {
            if (linkIDs[i] != null) {
                count++;
                if (count >= 30) {
                    cqlFilters.push(strcql);
                    count = 0;
                    strcql = "LINK_ID=" + linkIDs[i];
                    continue;
                }
                strcql = strcql + "%20OR%20LINK_ID=" + linkIDs[i];
            }
        }

        cqlFilters.push(strcql);
    }
    return cqlFilters;
}

//IfxStpmapCommon.getCQLFilerFromRoadNames = function (RoadNames) {//Nithin
//    var cqlFilters = new Array();

//    if (RoadNames && RoadNames.length > 0) {
//        var strcql = "ST_NAME=" + RoadNames[RoadNames.length - 1];
//        var count = 0;
//        for (var i = RoadNames.length - 2; i >= 0; i--) {
//            count++;
//            if (count >= 30) {
//                cqlFilters.push(strcql);
//                count = 0;
//                strcql = "ST_NAME=" + RoadNames[i];
//                continue;
//            }
//            strcql = strcql + "%20OR%20ST_NAME=" + RoadNames[i];
//        }

//        cqlFilters.push(strcql);
//    }
//    return cqlFilters;
//}

IfxStpmapCommon.orderFeatureByDistance = function (features, X, Y) {
    var point = new OpenLayers.Geometry.Point(X, Y);
    for (var index = 0; index < features.length; index++) {
        features[index].retObject = point.distanceTo(features[index].geometry, { details: true, edge: true });
    }

    features.sort(function (a, b) {
        return a.retObject.distance - b.retObject.distance;
    })
}

IfxStpmapCommon.findNearestSuitableFeatureIndex = function (features, X, Y, locationDesc) {
    IfxStpmapCommon.orderFeatureByDistance(features, X, Y);
    var found = false;
    var retObject;
    var nonPedIndex = -1;

    for (index = 0; index < features.length; index++) {
        if (features[index].attributes.PEDESTRIAN == 0 && features[index].attributes.FERRY_TYPE != 'B') {
            if (nonPedIndex == -1) {
                retObject = features[index].retObject;
                retObject['index'] = index;
                nonPedIndex = index;
            }
            if (features[index].attributes.ST_NAME != null && features[index].attributes.ST_NAME != undefined && locationDesc.toUpperCase().indexOf(features[index].attributes.ST_NAME.toUpperCase()) > -1) {
                retObject = features[index].retObject;
                retObject['index'] = index;
                found = true;
                break;
            }
        }
    }

    return retObject;
}

IfxStpmapCommon.findNearestSuitableAlternateFeatureIndex = function (features, X, Y, locationDesc) {
    IfxStpmapCommon.orderFeatureByDistance(features, X, Y);
    var found = false;
    var retObject;
    var nonPedIndex = -1;

    for (index = 0; index < features.length - 1; index++) {
        if (features[index].attributes.PEDESTRIAN == 0 && features[index].attributes.FERRY_TYPE != 'B') {
            if (nonPedIndex == -1) {
                retObject = features[index + 1].retObject;
                retObject['index'] = index + 1;
                nonPedIndex = index;
            }
            if (features[index].attributes.ST_NAME != null && features[index].attributes.ST_NAME != undefined && locationDesc.toUpperCase().indexOf(features[index].attributes.ST_NAME.toUpperCase()) > -1) {
                retObject = features[index + 1].retObject;
                retObject['index'] = index + 1;
                found = true;
                break;
            }
        }
    }

    return retObject;
}

IfxStpmapCommon.findNearestFeatureIndex = function (features, X, Y) {
    var distance, retObject;
    var nearest = 0;
    var point = new OpenLayers.Geometry.Point(X, Y);
    var index;

    retObject = point.distanceTo(features[0].geometry, { details: true, edge: true });
    retObject['index'] = 0;
    retObject['linkid'] = features[0].attributes.LINK_ID;
    retObject['refinid'] = features[0].attributes.REF_IN_ID;
    retObject['nrefinid'] = features[0].attributes.NREF_IN_ID;

    try {
        for (index = 1; index < features.length; index++) {
            distance = point.distanceTo(features[index].geometry, { details: true, edge: true });
            distance['index'] = index;
            distance['linkid'] = features[index].attributes.LINK_ID;
            distance['refinid'] = features[index].attributes.REF_IN_ID;
            distance['nrefinid'] = features[index].attributes.NREF_IN_ID;

            if (distance.distance < retObject.distance) {
                retObject = distance;
                //retObject['index'] = index;
            }
        }
    }
    catch (err) {
    }
    return retObject;
}

IfxStpmapCommon.findAlternateNearestFeatureIndex = function (features, X, Y, rPointObj) {
    if (features.length < 2) {
        return rPointObj;
    }

    var distance, retObject;
    var nearest = 0;
    var point = new OpenLayers.Geometry.Point(X, Y);
    var index;

    var startIdx = 0;

    for (index = startIdx; index < features.length; index++) {
        var tempRetObject = point.distanceTo(features[index].geometry, { details: true, edge: true });
        tempRetObject['linkid'] = features[index].attributes.LINK_ID;
        tempRetObject['refinid'] = features[index].attributes.REF_IN_ID;
        tempRetObject['nrefinid'] = features[index].attributes.NREF_IN_ID;
        if (tempRetObject.distance != rPointObj.distance
            && tempRetObject.refinid != rPointObj.refinid && tempRetObject.refinid != rPointObj.nrefinid
            && tempRetObject.nrefinid != rPointObj.refinid && tempRetObject.nrefinid != rPointObj.nrefinid) {
            retObject = tempRetObject;
            retObject['index'] = index;
            startIdx++;
            break;
        }
    }

    for (index = startIdx; index < features.length; index++) {
        var tempRetObject = point.distanceTo(features[index].geometry, { details: true, edge: true });
        tempRetObject['linkid'] = features[index].attributes.LINK_ID;
        tempRetObject['refinid'] = features[index].attributes.REF_IN_ID;
        tempRetObject['nrefinid'] = features[index].attributes.NREF_IN_ID;
        if (tempRetObject.distance != rPointObj.distance
            && tempRetObject.refinid != rPointObj.refinid && tempRetObject.refinid != rPointObj.nrefinid
            && tempRetObject.nrefinid != rPointObj.refinid && tempRetObject.nrefinid != rPointObj.nrefinid) {
            distance = tempRetObject;
            distance['index'] = index;

            if (distance.distance < retObject.distance) {
                retObject = distance;
                //retObject['index'] = index;
            }
        }
    }
    return retObject;
}

IfxStpmapCommon.cloneRoutePoint = function (srcRp) {
    var rpPoint = {};
    rpPoint['pointType'] = srcRp['pointType']
    rpPoint['pointDescr'] = srcRp['pointDescr']
    rpPoint['routePointNo'] = srcRp['routePointNo']
    rpPoint['direction'] = srcRp['direction']
    rpPoint['routeContactList'] = srcRp['routeContactList']
    rpPoint['linkId'] = srcRp['linkId']
    rpPoint['isAnchorPoint'] = srcRp['isAnchorPoint']
    rpPoint['pointGeom'] = { sdo_point: {} };
    rpPoint.pointGeom.sdo_point.X = srcRp.pointGeom.sdo_point.X
    rpPoint.pointGeom.sdo_point.Y = srcRp.pointGeom.sdo_point.Y
    return rpPoint;
}

IfxStpmapCommon.createLineGeometry = function (sdo_geometry) {
    var pointArr = new Array();
    for (var j = 0; j < sdo_geometry.OrdinatesArray.length; j += 2) {
        pointArr.push(new OpenLayers.Geometry.Point(sdo_geometry.OrdinatesArray[j],
            sdo_geometry.OrdinatesArray[j + 1]));
    }

    return new OpenLayers.Geometry.LineString(pointArr);
}

IfxStpmapCommon.createFeatureForWaypoints = function (routePath, routeSegment, obj) {
    if (routePath.otherinfo == undefined) {
        routePath.otherinfo = { completefeatures: [] };
    }
    if (routePath.otherinfo.completefeatures == undefined) {
        routePath.otherinfo.completefeatures = [];
    }

    for (var i = 0; i < routePath.routePointList.length; i++) {
        if (routePath.routePointList[i].pointType > 1) {
            linkDet = obj.searchRouteLinkAndSegmentByLinkID(routePath.pathNo, routeSegment.segmentNo - 1, routePath.routePointList[i].linkId);
            routeSegment = obj.routeManager.getRouteSegment(routePath.pathNo, linkDet.segmentIndex);
            if (routeSegment.otherinfo == undefined || routeSegment.otherinfo.completefeatures == undefined) {
                routeSegment.otherinfo = { completefeatures: [] };
            }
            if (linkDet.link != undefined && linkDet.link.linkId != 0 && linkDet.link.linkNo > 0 && (routeSegment.routeLinkList[linkDet.link.linkNo + 1] != undefined)) {//the extra condition appended for RM #15643
                if (routeSegment.routeLinkList[linkDet.link.linkNo - 1].linkId == routePath.routePointList[i].linkId || routeSegment.routeLinkList[linkDet.link.linkNo + 1].linkId == routePath.routePointList[i].linkId)
                    break;

                var len = routeSegment.routeLinkList.length;

                if (linkDet.link.linkNo <= len - 2 && linkDet.link.linkNo >= 1) {
                    var featureOfprevLinkID = IfxStpmapCommon.getFeatureOfLinkId(routeSegment.otherinfo.features,
                        routeSegment.routeLinkList[linkDet.link.linkNo - 1].linkId);
                    var featureOfnextLinkID = IfxStpmapCommon.getFeatureOfLinkId(routeSegment.otherinfo.features,
                        routeSegment.routeLinkList[linkDet.link.linkNo + 1].linkId);

                    if (featureOfprevLinkID != null && featureOfprevLinkID != undefined
                        && featureOfnextLinkID != null && featureOfnextLinkID != undefined) {
                        if (featureOfprevLinkID.attributes.NREF_IN_ID == featureOfnextLinkID.attributes.NREF_IN_ID ||
                            featureOfprevLinkID.attributes.REF_IN_ID == featureOfnextLinkID.attributes.NREF_IN_ID ||
                            featureOfprevLinkID.attributes.NREF_IN_ID == featureOfnextLinkID.attributes.REF_IN_ID) {
                            if ((routeSegment.routeLinkList[linkDet.link.linkNo + 1] && linkDet.link.linkId == routeSegment.routeLinkList[linkDet.link.linkNo + 1].linkId) ||
                                (routeSegment.routeLinkList[linkDet.link.linkNo - 1] && linkDet.link.linkId == routeSegment.routeLinkList[linkDet.link.linkNo - 1].linkId)) {
                                obj.removeFeaturesFromRoutePath(routePath.pathNo, linkDet.link.linkId);
                                IfxStpmapCommon.getWaypointSegmentFeature(routePath, routeSegment, i, true);
                            }
                        }
                    }
                }
            }
            else {
                IfxStpmapCommon.getWaypointSegmentFeature(routePath, routeSegment, i, false);
            }
        }
    }
}

IfxStpmapCommon.getWaypointSegmentFeature = function (routePath, routeSegment, pointIndex, traversedTwice) {
    if (routePath.routePointList[pointIndex].otherinfo.pointfeature == null || routePath.routePointList[pointIndex].otherinfo.pointfeature == undefined)
        return;
    var lrsMeasure = routePath.routePointList[pointIndex].lrs / IfxStpmapCommon.getLengthOfFeature(routePath.routePointList[pointIndex].otherinfo.pointfeature);
    if (lrsMeasure <= 0.5) {
        if (traversedTwice == false) {
            var line = LRSSubstring(routePath.routePointList[pointIndex].otherinfo.pointfeature.geometry, 0, lrsMeasure);
            var direction = 1;
        }
        else {
            var line = LRSSubstring(routePath.routePointList[pointIndex].otherinfo.pointfeature.geometry, lrsMeasure, 1);
            var direction = 0;
        }
    }
    else {
        if (traversedTwice == false) {
            var line = LRSSubstring(routePath.routePointList[pointIndex].otherinfo.pointfeature.geometry, lrsMeasure, 1);
            var direction = 0;
        }
        else {
            var line = LRSSubstring(routePath.routePointList[pointIndex].otherinfo.pointfeature.geometry, 0, lrsMeasure);
            var direction = 1;
        }
    }
    routePath.routePointList[pointIndex].direction = direction;
    var feature = new OpenLayers.Feature.Vector(line);
    feature.data = 'NORMAL';
    // feature.color = 4;
    routePath.otherinfo.completefeatures.push(feature);
    routeSegment.otherinfo.completefeatures.push(feature);
}

IfxStpmapCommon.createFeatureForRouteSegment = function (routeSegment) {
    if (routeSegment.otherinfo == undefined) {
        routeSegment.otherinfo = { completefeatures: [], features: [] };
    }
    if (routeSegment.offRoadGeometry != null) {
        routeSegment.otherinfo.completefeatures.push(new OpenLayers.Feature.Vector(IfxStpmapCommon.createLineGeometry(routeSegment.offRoadGeometry)));
        return;
    }

    var segmentFeature;
    var featureOfNextLinkID;
    if (routeSegment.otherinfo.features != null)
        routeSegment.otherinfo.completefeatures = routeSegment.otherinfo.features.slice(0);

    if (routeSegment.routeLinkList.length > 0) {
        featureOfNextLinkID = IfxStpmapCommon.getFeatureOfLinkId(routeSegment.otherinfo.features,
            routeSegment.routeLinkList[0].linkId);

        if (featureOfNextLinkID == null)
            featureOfNextLinkID = routeSegment.otherinfo.startSegmentfeature;

        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(routeSegment.startPointGeometry, routeSegment.otherinfo.startSegmentfeature, featureOfNextLinkID, 'STARTPOINT');
        routeSegment.startPointDirection = segmentFeature.direction;
        if (segmentFeature.feature.geometry != null) {
            if (routeSegment.otherinfo.startSegmentfeature)
                segmentFeature.feature.attributes = routeSegment.otherinfo.startSegmentfeature.attributes;
            routeSegment.otherinfo.completefeatures.push(segmentFeature.feature);
        }
        else {
            routeSegment.otherinfo.completefeatures.push(routeSegment.otherinfo.startSegmentfeature);
        }

        featureOfNextLinkID = IfxStpmapCommon.getFeatureOfLinkId(routeSegment.otherinfo.features,
            routeSegment.routeLinkList[routeSegment.routeLinkList.length - 1].linkId);

        if (featureOfNextLinkID == null)
            featureOfNextLinkID = routeSegment.otherinfo.endSegmentfeature;

        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(routeSegment.endPointGeometry, routeSegment.otherinfo.endSegmentfeature, featureOfNextLinkID, 'ENDPOINT');
        routeSegment.endPointDirection = segmentFeature.direction;
        if (segmentFeature.feature.geometry != null) {
            if (routeSegment.otherinfo.endSegmentfeature)
                segmentFeature.feature.attributes = routeSegment.otherinfo.endSegmentfeature.attributes;
            routeSegment.otherinfo.completefeatures.push(segmentFeature.feature);
        }
        else {
            routeSegment.otherinfo.completefeatures.push(routeSegment.otherinfo.endSegmentfeature);
        }
    }
    else {
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(routeSegment.startPointGeometry,
            routeSegment.otherinfo.startSegmentfeature, routeSegment.otherinfo.endSegmentfeature, 'STARTPOINT');
        routeSegment.startPointDirection = segmentFeature.direction;
        if (segmentFeature.feature.geometry != null) {
            if (routeSegment.otherinfo.startSegmentfeature)
                segmentFeature.feature.attributes = routeSegment.otherinfo.startSegmentfeature.attributes;
        }
        routeSegment.otherinfo.completefeatures.push(segmentFeature.feature);
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(routeSegment.endPointGeometry,
            routeSegment.otherinfo.endSegmentfeature, routeSegment.otherinfo.startSegmentfeature, 'ENDPOINT');
        routeSegment.endPointDirection = segmentFeature.direction;
        if (segmentFeature.feature.geometry != null) {
            if (routeSegment.otherinfo.endSegmentfeature)
                segmentFeature.feature.attributes = routeSegment.otherinfo.endSegmentfeature.attributes;
        }
        routeSegment.otherinfo.completefeatures.push(segmentFeature.feature);
    }

    var segType = IfxStpmapCommon.getSegmentTypeName(routeSegment.segmentType);
    for (var i = 0; i < routeSegment.otherinfo.completefeatures.length; i++) {
        routeSegment.otherinfo.completefeatures[i].data = segType;
    }
}

IfxStpmapCommon.createPartialFeatureForRouteSegment = function (feature, direction, X, Y, rSegment, pointType) {
    var lrsMeasure = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(X, Y), { tolerance: 0.5, details: true });
    if (direction == 1) {
        var line = lrsMeasure.subString1;
    }
    else {
        var line = lrsMeasure.subString2;
    }
    var segmentFeature = new OpenLayers.Feature.Vector(line);
    if (pointType == 'STARTPOINT') {
        if (rSegment.otherinfo.startSegmentfeature)
            segmentFeature.attributes = rSegment.otherinfo.startSegmentfeature.attributes;
    }
    else if (pointType == 'ENDPOINT') {
        if (rSegment.otherinfo.endSegmentfeature)
            segmentFeature.attributes = rSegment.otherinfo.endSegmentfeature.attributes;
    }
    rSegment.otherinfo.completefeatures = [];
    rSegment.otherinfo.completefeatures.push(segmentFeature);
    rSegment.otherinfo.completefeatures[0].data = 'NORMAL';
    return new OpenLayers.Feature.Vector(line);
}

IfxStpmapCommon.getPartialFeature = function (feature, rSegment, seg) {
    if (seg != 1) {
        var lrsMeasure = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(rSegment.startPointGeometry.sdo_point.X, rSegment.startPointGeometry.sdo_point.Y), { tolerance: 0.5, details: true });
        var direction = rSegment.startPointDirection;
    }
    else {
        var lrsMeasure = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(rSegment.endPointGeometry.sdo_point.X, rSegment.endPointGeometry.sdo_point.Y), { tolerance: 0.5, details: true });
        var direction = !rSegment.endPointDirection;
    }

    if (direction == 0) {
        var line = lrsMeasure.subString1;
    }
    else {
        var line = lrsMeasure.subString2;
    }
    var retObj = new OpenLayers.Feature.Vector(line);
    retObj.attributes = feature.attributes;

    return retObj;
}

//function to convert point type to its corresponding number used in DB
//START, END, WAYPOINT, VIAPOINT
IfxStpmapCommon.getPointTypeID = function (pointType) {
    if (pointType == 'STARTPOINT')
        return 0;
    if (pointType == 'ENDPOINT')
        return 1;
    if (pointType == 'WAYPOINT')
        return 2;
    if (pointType == 'VIAPOINT')
        return 3;
    if (pointType == 'ROUTEDRAG')
        return 9;

    return 0;
}

IfxStpmapCommon.getPointTypeName = function (pointTypeID) {
    switch (pointTypeID) {
        case 0:
        case 5:
        case 7:
            return 'STARTPOINT';
        case 1:
        case 4:
        case 8:
            return 'ENDPOINT';
        case 2:
            return 'WAYPOINT';
        case 3:
            return 'VIAPOINT';
        case 9:
            return 'ROUTEDRAG';
    }
}

IfxStpmapCommon.getPathTypeName = function (pathTypeID) {
}

IfxStpmapCommon.getPathTypeID = function (pathTypeName) {
}

IfxStpmapCommon.getSegmentTypeName = function (segmentTypeID) {
    switch (segmentTypeID) {
        case 1:
        case 'NORMAL':
            return 'NORMAL';
        case 2:
        case 'OVERRIDE':
            return 'OVERRIDE';
        case 3:
        case 'OFFROAD':
            return 'OFFROAD';
        case 4:
        case 'SHUNT':
            return 'SHUNT';
        case 5:
        case 'UTURN':
            return 'UTURN';
        case 6:
        case 'BROKEN':
            return 'BROKEN';
        case 7:
        case 'ASSUMED':
            return 'ASSUMED';
        default:
            return 'CONFIRMED';
    }
}

IfxStpmapCommon.getSegmentTypeID = function (segmentType) {
    switch (segmentType) {
        case 'NORMAL':
        case 1:
            return 1;
        case 'OVERRIDE':
        case 2:
            return 2;
        case 'OFFROAD':
        case 3:
            return 3;
        case 'SHUNT':
        case 4:
            return 4;
        case 'UTURN':
        case 5:
            return 5;
        case 'BROKEN':
        case 6:
            return 6;
        case 'ASSUMED':
        case 7:
            return 7;
        default:
            return 8;
    }
}

IfxStpmapCommon.getColor = function (feature) {
    switch (feature.data) {
        case 'NORMAL':
        case 1:
            return '#8000FF';
        case 'OVERRIDE':
        case 2:
            return '#FF3535';
        case 'UTURN':
        case 5:
            return '#4C6160';
        case 'CONFIRMED':
        case 8:
            return '#A35024';
        default:
            return '#6FB9C5';

        //default:
        //    var col = Math.floor(Math.random() * 16777215);
        //    var color = '#' + col.toString(16);
        //    return '#' + col.toString(16);
    }
}

IfxStpmapCommon.getMarkerImage = function (routePathType, pointTypeID) {
    if (routePathType == undefined || routePathType == 0) {
        if (pointTypeID == 'STARTPOINT') {
            return '/Content/assets/Icons/map-startpoint.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '/Content/assets/Icons/map-endpoint.svg';
        }
    }
    else if (routePathType == 1) {

        if (pointTypeID == 'STARTPOINT') {

            return '/Content/assets/Icons/map-startpoint-alt.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '../../Content/assets/Icons/Merge to.svg';
        }
    }
    else if (routePathType == 2) {
        if (pointTypeID == 'STARTPOINT') {
            return '../../Content/assets/Icons/diverge from.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '../../Content/assets/Icons/Merge to.svg';
        }
    }
    else if (routePathType == 3) {
        if (pointTypeID == 'STARTPOINT') {
            return '../../Content/assets/Icons/diverge from.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '/Content/assets/Icons/map-endpoint-alt.svg';
        }
    }
    else if (routePathType == 4) {
        if (pointTypeID == 'STARTPOINT') {
            return '/Content/Images/editA.png';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '/Content/Images/editB.png';
        }
    }
}

IfxStpmapCommon.getMarkerStartEndImage = function (pointTypeID, routePathType, pathno) {
    if (routePathType == undefined || (routePathType == 1 && pathno == 0)) {
        if (pointTypeID == 'STARTPOINT') {
            return '/Content/assets/Icons/map-startpoint.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '/Content/assets/Icons/map-endpoint.svg';
        }
    }
    else if (routePathType == 1) {

        if (pointTypeID == 'STARTPOINT') {

            return '/Content/assets/Icons/map-startpoint-alt.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '../../Content/assets/Icons/Merge to.svg';
        }
    }
    else if (routePathType == 2) {
        if (pointTypeID == 'STARTPOINT') {
            return '../../Content/assets/Icons/diverge from.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '../../Content/assets/Icons/Merge to.svg';
        }
    }
    else if (routePathType == 3) {
        if (pointTypeID == 'STARTPOINT') {
            return '../../Content/assets/Icons/diverge from.svg';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '/Content/assets/Icons/map-endpoint-alt.svg';
        }
    }
    else if (routePathType == 4) {
        if (pointTypeID == 'STARTPOINT') {
            return '/Content/Images/editA.png';
        }
        else if (pointTypeID == 'ENDPOINT') {
            return '/Content/Images/editB.png';
        }
    }
}

IfxStpmapCommon.getFinalSuitability = function (suitability) {
    var ret_suitability = 'Default';
    if (suitability) {
        for (var i = 0; i < suitability.length; i++) {
            if (suitability[i] == 'Unsuitable')
                return 'Unsuitable';
            else if (suitability[i] == 'Marginally suitable' && ret_suitability != 'Marginally suitable')
                ret_suitability = 'Marginally suitable';
            else if (suitability[i] == 'Suitable')
                ret_suitability = 'Suitable';
            else if (suitability[i] == 'Not Specified' || suitability[i] == 'Not Structure Specified')
                ret_suitability = 'Not Specified'
        }
    }
    return ret_suitability;
}

IfxStpmapCommon.getStructureImage = function (structureClass, suitability) {

    if (suitability == null || suitability == undefined)
        suitability = '';
    else
        suitability = suitability.toLowerCase();
    switch (structureClass) {
        case 'underbridge':
        case 'overbridge':
        case 'under and over bridge':
        case 'level crossing':
            switch (suitability) {
                case 'unsuitable':
                    return '/Content/assets/Icons/structure-not-suitable.svg';
                case 'marginally suitable':
                    return '/Content/assets/Icons/structure-marginal-suitability.svg';
                case 'suitable':
                    return '/Content/assets/Icons/structure-suitable.svg';
                case 'not specified':
                    return '/Content/assets/Icons/structure-unspecified-caution.svg';
                default:
                    return '/Content/Images/Group 539.svg';
            }
    }
}

IfxStpmapCommon.getSuitabilityColor = function (suitability, checkFor) {

    if (suitability == null || suitability == undefined)
        suitability = '';
    else
        suitability = suitability.toLowerCase();
    switch (checkFor) {
        case 0:
            switch (suitability) {
                case 'unsuitable':
                    return '#f70012';
                case 'marginally suitable':
                    return '#ffdb17';
                default:
                    return '#003060';
            }
        default:
            switch (suitability) {
                case 'unsuitable':
                    return '#ff0000';
                default:
                    return '#6a6c95';
            }
    }
}

IfxStpmapCommon.getConstraintTypeDescription = function (constraintTypeID) {
    switch (constraintTypeID) {
        case "253002":
            return 'height';
        case "253003":
            return 'width';
        case "253004":
            return 'length';
        case "253005":
            return 'weight';
        case "253006":
            return 'oneway';
        case "253007":
            return 'roadworks';
        case "253008":
            return 'incline';
        case "253009":
            return 'tram';
        case "253010":
            return 'tight bend';
        case "253011":
            return 'event';
        case "253012":
            return 'risk of grounding';
        case "253013":
            return 'unmade';
        case "253014":
            return 'natural void';
        case "253015":
            return 'manmade void';
        case "253016":
            return 'tunnel';
        case "253017":
            return 'tunnel void';
        case "253018":
            return 'pipes and ducts';
        case "retaining wall":
        case "253019":
            return 'retaining wall';
        case "253020":
            return 'traffic calming';
        case "253021":
            return 'overhead building';
        case "253022":
            return 'overhead pipes and utilities';
        case "253023":
            return 'adjacent retaining wall';
        case "253024":
            return 'power cable';
        case "253025":
            return 'telecomms cable';
        case "253026":
            return 'gantry road furniture';
        case "253027":
            return 'cantilever road furniture';
        case "253028":
            return 'catenary road furniture';
        case "253029":
            return 'electrification cable';
        case "253030":
            return 'bollard';
        case "253031":
            return 'removable bollard';
        default:
            return 'Generic';
    }
}

IfxStpmapCommon.getConstraintImage = function (constraintType, suitability) {
    var suitable = "";
    if (suitability != null && suitability != undefined) {
        suitability = suitability.toLowerCase();
        try {
            if (Notificationflag != undefined && CurrentAssessmentstatus != undefined) {
                if (Notificationflag == 'True' && CurrentAssessmentstatus == "") {
                    suitability = "Default";
                }
            }
        }
        catch (err) {
            suitability = suitability.toLowerCase();
        }
    }

    switch (suitability) {

        /*case "height":
        case "253002":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_height.png';
        case "width":
        case "253003":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_width.png';
        case "length":
        case "253004":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_length.png';
        case "weight":
        case "253005":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_weight.png';
        case "oneway":
        case "253006":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_oneway.png';
        case "roadworks":
        case "253007":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_roadworks.png';
        case "incline":
        case "253008":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_incline.png';
        case "tram":
        case "253009":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_tram.png';
        case "tight bend":
        case "253010":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_tightbends.png';
        case "risk of grounding":
        case "253012":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_riskofgrouding.png';
        case "unmade":
        case "253013":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_unmade.png';
        case "natural void":
        case "253014":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_naturalvoid.png';
        case "manmade void":
        case "253015":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_manmadevoid.png';
        case "tunnel":
        case "253016":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_tunnel.png';
        case "tunnel void":
        case "253017":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_tunnelvoid.png';
        case "pipes and ducts":
        case "253018":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_pipes&ducts.png';
        case "retaining wall":
        case "253019":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_retainingwall.png';
        case "traffic calming":
        case "253020":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_trafficcalming.png';
        case "overhead building":
        case "253021":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_overheadbuilding.png';
        case "overhead pipes and utilities":
        case "253022":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_overheadpipes.png';
        case "adjacent retaining wall":
        case "253023":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_adjacentretainingwall.png';
        case "power cable":
        case "253024":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_powercable.png';
        case "telecomms cable":
        case "253025":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_telecommscable.png';
        case "gantry road furniture":
        case "253026":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_gantry.png';
        case "cantilever road furniture":
        case "253027":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_cantiliver.png';
        case "catenary road furniture":
        case "253028":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_catenary.png';
        case "electrification cable":
        case "253029":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_electriccable.png';
        case "bollard":
        case "253030":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_bollards.png';
        case "removable bollard":
        case "253031":
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_removablebollards.png';
        default:
            return '/Content/Images/constraint_icons/' + suitable + 'constraint_generic.png';*/
        case "unsuitable":
            return '/Content/assets/Icons/constraint-not-suitable.svg';
        case "not specified":
            return '/Content/assets/Icons/constraint.svg';
        case "suitable":
            return '/Content/assets/Icons/constraint-suitable.svg';
        //case "unsuitable":
        //    return '/Content/assets/images/unsuitableConstraint.png';
        default:
            return '/Content/assets/Icons/constraint.svg';
    }
}

IfxStpmapCommon.getRoutePlanUnit = function (unit) {
    switch (unit) {
        case "692002":
            return 'imperial';
        default:
            return 'metric';
    }
}

IfxStpmapCommon.getIntersectedFeatures = function (obj, features, topologyType) {
    var intersectedFeaturesArr = new Array();
    if (topologyType == 'linear') {
        for (var i = 0; i < features.length; i++) {
            for (var j = 0; j < features[i].geometry.components.length - 1; j++) {
                if (features[i].geometry.components[j].x < features[i].geometry.components[j + 1].x) {
                    var seg1 = {
                        x1: features[i].geometry.components[j].x, y1: features[i].geometry.components[j].y,
                        x2: features[i].geometry.components[j + 1].x, y2: features[i].geometry.components[j + 1].y
                    };
                }
                else {
                    var seg1 = {
                        x1: features[i].geometry.components[j + 1].x, y1: features[i].geometry.components[j + 1].y,
                        x2: features[i].geometry.components[j].x, y2: features[i].geometry.components[j].y
                    };
                }
                for (var k = 0; k < obj.geometry.components.length - 1; k++) {
                    if (obj.geometry.components[k].x < obj.geometry.components[k + 1].x) {
                        var seg2 = {
                            x1: obj.geometry.components[k].x, y1: obj.geometry.components[k].y,
                            x2: obj.geometry.components[k + 1].x, y2: obj.geometry.components[k + 1].y
                        };
                    }
                    else {
                        var seg2 = {
                            x1: obj.geometry.components[k + 1].x, y1: obj.geometry.components[k + 1].y,
                            x2: obj.geometry.components[k].x, y2: obj.geometry.components[k].y
                        };
                    }

                    var intersect = OpenLayers.Geometry.segmentsIntersect(seg1, seg2, { point: true, tolerance: 5.0 });
                    if (intersect) {
                        intersectedFeaturesArr.push(features[i]);
                        break;
                    }
                }
                if (intersect)
                    break;
            }
        }
    }
    else {
        for (var i = 0; i < features.length; i++) {
            var intersect = obj.geometry.intersects(features[i].geometry);
            if (intersect) {
                intersectedFeaturesArr.push(features[i]);
            }
        }
    }
    return intersectedFeaturesArr;
}

IfxStpmapCommon.capitaliseString = function (string) {
    return string.charAt(0).toUpperCase() + string.substring(1);
}

IfxStpmapCommon.getRouteAppraisalParams = function (page) {
    switch (page) {
        case 'ROUTELIBRARY':
            return { routeType: 0, checkAppraisal: false };
        default:
            return { routeType: 1, checkAppraisal: true };
    }
}

IfxStpmapCommon.setFeatureDataForRoute = function (features, data) {
    for (var i = 0; i < features.length; i++) {
        features[i].data = data;
    }
    return features;

}

IfxStpmapCommon.getLRSLength = function (line, point) {
    var lrsMeasure = LRSMeasure(line, point, { tolerance: 0.5, details: true });
    return Math.round(lrsMeasure.length);
}

IfxStpmapCommon.getLengthOfFeature = function (feature) {
    return feature.geometry.getLength();
}

IfxStpmapCommon.checkForPedestrianRoad = function (feature) {
    if (feature.attributes.AR_AUTO == 'N' && feature.attributes.AR_BUS == 'N' && feature.attributes.AR_TAXIS == 'N' && feature.attributes.AR_CARPOOL == 'N' && feature.attributes.AR_TRUCKS == 'N' &&
        feature.attributes.AR_TRAFF == 'N' && feature.attributes.AR_DELIV == 'N' && feature.attributes.AR_EMERVEH == 'N' && feature.attributes.AR_MOTOR == 'N' && feature.attributes.AR_PEDEST == 'Y')
        return true;
    return false;
}

IfxStpmapCommon.findIsNearest = function (X, Y, p1, p2) {
    var point = new OpenLayers.Geometry.Point(X, Y);
    var point1 = new OpenLayers.Geometry.Point(p1.x, p1.y);
    var point2 = new OpenLayers.Geometry.Point(p2.x, p2.y);
    var retObject = point.distanceTo(point1, { details: true, edge: true });
    var distance = retObject.distance;
    retObject = point.distanceTo(point2, { details: true, edge: true });
    if (distance < retObject.distance)
        return true;
    return false;
}

IfxStpmapCommon.findIsNearestWithDirection = function (rSegment, nearestPath, intersectionFeature1, intersectionFeature2, obj, near) {
    var lrs = LRSMeasure(intersectionFeature1.feature.geometry, new OpenLayers.Geometry.Point(nearestPath.x1, nearestPath.y1), { tolerance: 0.5, details: true });
    var a = Math.round(lrs.measure).toFixed(2);
    if (lrs.measure == 1 || Math.round(lrs.measure * 1000) == 1000) {
        if (intersectionFeature2 == undefined)
            return true;
        return IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeature1.point, intersectionFeature2.point);
    }
    var dir = IfxStpmapCommon.getDirectionOfRouteLink(rSegment, nearestPath, intersectionFeature1.feature, obj);
    if (dir == 1) {
        var tempLrs = LRSMeasure(intersectionFeature1.feature.geometry, new OpenLayers.Geometry.Point(intersectionFeature1.point.x, intersectionFeature1.point.y), { tolerance: 0.5, details: true });
        if (near == 1 && tempLrs.measure < lrs.measure) {
            if (intersectionFeature2 != undefined) {
                var bool = IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeature1.point, intersectionFeature2.point);
                return bool;
            }
            return true;
        }
        else if (near == 2 && tempLrs.measure > lrs.measure) {
            if (intersectionFeature2 != undefined) {
                var bool = IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeature1.point, intersectionFeature2.point);
                return bool;
            }
            return true;
        }
        return false;
    }
    else {
        var tempLrs = LRSMeasure(intersectionFeature1.feature.geometry, new OpenLayers.Geometry.Point(intersectionFeature1.point.x, intersectionFeature1.point.y), { tolerance: 0.5, details: true });
        if (near == 1 && tempLrs.measure > lrs.measure) {
            if (intersectionFeature2 != undefined) {
                var bool = IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeature1.point, intersectionFeature2.point);
                return bool;
            }
            return true;
        }
        else if (near == 2 && tempLrs.measure < lrs.measure) {
            if (intersectionFeature2 != undefined) {
                var bool = IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeature1.point, intersectionFeature2.point);
                return bool;
            }
            return true;
        }
        return false;
    }
}

IfxStpmapCommon.getDirectionOfRouteLink = function (rSegment, nearestPath, feature, obj, link) {
    var direction = 1;
    if (link == undefined)
        link = obj.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, feature.attributes.LINK_ID);
    if (link != undefined) {
        var feature = obj.searchFeatureInRouteByLinkId(nearestPath.pathIndex, nearestPath.segmentIndex, link.linkId);
        if (feature != undefined) {
            if (rSegment.routeLinkList.length > 1) {
                if (link.linkNo != rSegment.routeLinkList.length) {
                    var nextLink = rSegment.routeLinkList[link.linkNo];
                    var nextFeature;
                    if (nextLink != undefined)
                        nextFeature = obj.searchFeatureInRouteByLinkId(nearestPath.pathIndex, nearestPath.segmentIndex, nextLink.linkId);
                    if (nextFeature != undefined) {
                        if (feature.attributes.REF_IN_ID == nextFeature.attributes.REF_IN_ID || feature.attributes.REF_IN_ID == nextFeature.attributes.NREF_IN_ID)
                            direction = 0;
                        else
                            direction = 1;
                    }
                    else {
                        var prevLink = rSegment.routeLinkList[link.linkNo - 2];
                        var prevFeature;
                        if (prevLink != undefined)
                            prevFeature = obj.searchFeatureInRouteByLinkId(nearestPath.pathIndex, nearestPath.segmentIndex, prevLink.linkId);
                        if (prevFeature != undefined) {
                            if (feature.attributes.REF_IN_ID == prevFeature.attributes.REF_IN_ID || feature.attributes.REF_IN_ID == prevFeature.attributes.NREF_IN_ID)
                                direction = 1;
                            else
                                direction = 0;
                        }
                    }
                }
                else {
                    var prevLink = rSegment.routeLinkList[link.linkNo - 2];
                    var prevFeature;
                    if (prevLink != undefined)
                        prevFeature = obj.searchFeatureInRouteByLinkId(nearestPath.pathIndex, nearestPath.segmentIndex, prevLink.linkId);
                    if (prevFeature != undefined) {
                        if (feature.attributes.REF_IN_ID == prevFeature.attributes.REF_IN_ID || feature.attributes.REF_IN_ID == prevFeature.attributes.NREF_IN_ID)
                            direction = 1;
                        else
                            direction = 0;
                    }
                    else {
                        var nextLink = rSegment.routeLinkList[link.linkNo];
                        var nextFeature;
                        if (nextLink != undefined)
                            nextFeature = obj.searchFeatureInRouteByLinkId(nearestPath.pathIndex, nearestPath.segmentIndex, nextLink.linkId);
                        if (nextFeature != undefined) {
                            if (feature.attributes.REF_IN_ID == nextFeature.attributes.REF_IN_ID || feature.attributes.REF_IN_ID == nextFeature.attributes.NREF_IN_ID)
                                direction = 0;
                            else
                                direction = 1;
                        }
                    }
                }
            }
            else {
                var nextFeature = rSegment.otherinfo.endSegmentfeature;
                if (feature.attributes.REF_IN_ID == nextFeature.attributes.REF_IN_ID || feature.attributes.REF_IN_ID == nextFeature.attributes.NREF_IN_ID)
                    direction = 0;
                else
                    direction = 1;
            }
        }
    }
    else {
        if (feature.attributes.LINK_ID == rSegment.startLinkId) {
            direction = rSegment.startPointDirection;
        }
        else if (feature.attributes.LINK_ID == rSegment.endLinkId) {
            direction = rSegment.endPointDirection;
        }
    }
    return direction;
}

IfxStpmapCommon.findIfIntersects = function (feature, X, Y) {
    var point = new OpenLayers.Geometry.Point(X, Y);
    var retObject = point.distanceTo(feature.geometry, { details: true, edge: true });
    if (retObject.distance < 5)
        return true;
    return false;
}

IfxStpmapCommon.getNearestIntersectionPoints = function (feature, direction, intersectionPoints, X, Y) {
    var clickedLRS = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(X, Y), { tolerance: 0.5, details: true });
    var near1 = { lrs: 0, index: -1 };
    var near2 = { lrs: 1, index: -1 };
    for (var i = 0; i < intersectionPoints.length; i++) {
        var lrs = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(intersectionPoints[i].x, intersectionPoints[i].y), { tolerance: 0.5, details: true });
        if (lrs.measure > near1.lrs && lrs.measure < clickedLRS.measure) {
            near1.lrs = lrs.measure;
            near1.index = i;
        }
        if (lrs.measure < near2.lrs && lrs.measure > clickedLRS.measure) {
            near2.lrs = lrs.measure;
            near2.index = i;
        }
    }
    if (direction == 0) {
        var temp = near1;
        near1 = near2;
        near2 = temp;
    }
    return { near1: near1, near2: near2 };
}

IfxStpmapCommon.compareGeometries = function (geom1, geom2, tolerance) {
    if (tolerance == undefined || tolerance == null)
        tolerance = 1;
    if (geom1.x != undefined && geom1.x != null) {
        if (geom2.x != undefined && geom2.x != null) {
            if (Math.abs(Math.round(geom1.x) - Math.round(geom2.x)) <= tolerance && Math.abs(Math.round(geom1.y) - Math.round(geom2.y)) <= tolerance)
                return true;
            return false;
        }
        else {
            if (Math.abs(Math.round(geom1.x) - Math.round(geom2.X)) <= tolerance && Math.abs(Math.round(geom1.y) - Math.round(geom2.Y)) <= tolerance)
                return true;
            return false;
        }
    }
    else {
        if (geom2.x != undefined && geom2.x != null) {
            if (Math.abs(Math.round(geom1.X) - Math.round(geom2.x)) <= tolerance && Math.abs(Math.round(geom1.Y) - Math.round(geom2.y)) <= tolerance)
                return true;
            return false;
        }
        else {
            if (Math.abs(Math.round(geom1.X) - Math.round(geom2.X)) <= tolerance && Math.abs(Math.round(geom1.Y) - Math.round(geom2.Y)) <= tolerance)
                return true;
            return false;
        }
    }
}

IfxStpmapCommon.checkLinkContinuity = function (currentIndex, features, checkOn, line) {
    if (checkOn == 'REF_NODE') {
        for (var i = 0; i < features.length; i++) {
            if (i != currentIndex && (features[i].attributes.REF_IN_ID == features[currentIndex].attributes.REF_IN_ID || features[i].attributes.NREF_IN_ID == features[currentIndex].attributes.REF_IN_ID)) {
                if (features[currentIndex].geometry.components.length == 2) {
                    var _x = (features[currentIndex].geometry.components[0].x + features[currentIndex].geometry.components[1].x) / 2;
                    var _y = (features[currentIndex].geometry.components[0].y + features[currentIndex].geometry.components[1].y) / 2;
                    var point = new OpenLayers.Geometry.Point(_x, _y);
                }
                else {
                    var len = Math.floor(features[currentIndex].geometry.components.length / 2);
                    var point = features[currentIndex].geometry.components[len];
                }
                var distance = point.distanceTo(line.geometry);
                if (distance > 5)
                    return false;
                return true;
            }
        }
    }
    else {
        for (var i = 0; i < features.length; i++) {
            if (i != currentIndex && (features[i].attributes.REF_IN_ID == features[currentIndex].attributes.NREF_IN_ID || features[i].attributes.NREF_IN_ID == features[currentIndex].attributes.NREF_IN_ID)) {
                if (features[currentIndex].geometry.components.length == 2) {
                    var _x = (features[currentIndex].geometry.components[0].x + features[currentIndex].geometry.components[1].x) / 2;
                    var _y = (features[currentIndex].geometry.components[0].y + features[currentIndex].geometry.components[1].y) / 2;
                    var point = new OpenLayers.Geometry.Point(_x, _y);
                }
                else {
                    var len = Math.floor(features[currentIndex].geometry.components.length / 2);
                    var point = features[currentIndex].geometry.components[len];
                }
                var distance = point.distanceTo(line.geometry);
                if (distance > 5)
                    return false;
                return true;
            }
        }
    }
    return false;
}

IfxStpmapCommon.checkRouteContinuity = function (rPath, segmentIndex) {
    if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routePointList[0].pointGeom.sdo_point) ||
        IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routePointList[0].pointGeom.sdo_point)) {
        for (var i = 0; i < rPath.routeSegmentList.length; i++) {
            if (i != segmentIndex) {
                if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))
                    return false;
            }
        }
    }
    else if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routePointList[1].pointGeom.sdo_point) ||
        IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routePointList[1].pointGeom.sdo_point)) {
        for (var i = 0; i < rPath.routeSegmentList.length; i++) {
            if (i != segmentIndex) {
                if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point))
                    return false;
            }
        }
    }
    else {
        for (var i = 0; i < rPath.routeSegmentList.length; i++) {
            if (i != segmentIndex) {
                if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))
                    return false;
            }
        }
    }
    return true;
}

IfxStpmapCommon.checkOffRoadExtend = function (rPath, segmentIndex) {
    if (rPath.routeSegmentList[segmentIndex].segmentNo == 1)
        return 1;
    else if (rPath.routeSegmentList[segmentIndex].segmentNo = rPath.routeSegmentList.length) {
        if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routePointList[1].pointGeom.sdo_point)) {
            for (var i = 0; i < rPath.routeSegmentList.length; i++) {
                if (i != segmentIndex) {
                    if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                        IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))
                        return 2;
                }
            }
        }
        else if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].endPointGeometry.sdo_point, rPath.routePointList[1].pointGeom.sdo_point)) {
            for (var i = 0; i < rPath.routeSegmentList.length; i++) {
                if (i != segmentIndex) {
                    if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                        IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[segmentIndex].startPointGeometry.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))
                        return 2;
                }
            }
        }
    }
    return 0;
}

IfxStpmapCommon.getDelegRoadColor = function (feature) {
    switch (feature.data) {
        case 'a'://owned by me
        case 'owned':
            return '#1919FF';
        // return color[feature.color];
        case 'b':   //managed by me sa
        case 'c':   //managed by me sna
        case 'unassigned':
            return '#3F1201';
        //case 'c':   //managed by me sna
        //    return '#FF3535';
        default:
            return '#44FF00';
    }
}

IfxStpmapCommon.getDelegationMarkerImage = function (pointType) {
    switch (pointType) {
        case 0:
            return '/Content/Images/delegationstart.png';
        case 1:
            return '/Content/Images/delegationend.png';
    }
}

IfxStpmapCommon.getSdo_gtype = function (geometryType) {
    switch (geometryType) {
        case 'POINT':
            return 2001;
        case 'LINE':
            return 2002;
        case 'POLYGON':
            return 2003;
    }
}

IfxStpmapCommon.getSdo_srid = function () {
    return 27700;
}

IfxStpmapCommon.getZoomStatusPos = function (zoom) {
    if (zoom >= 10) {
        return { bottom: "93px", right: "14px", color: "#343a40" }
    }
    else {
        return { bottom: "93px", right: "18px", color: "#343a40" }
    }

}
