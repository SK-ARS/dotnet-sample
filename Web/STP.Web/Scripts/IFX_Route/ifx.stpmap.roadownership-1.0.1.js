function IfxStpmapRoadOwnership() {
}

IfxStpmapRoadOwnership.prototype.csvImportFlag = false;
IfxStpmapRoadOwnership.prototype.roadSegment = {
    startPointGeom: { sdo_point: { X: '', Y: '' } },
    endPointGeom: { sdo_point: { X: '', Y: '' } },
    linkInfoList: [],
    ownedFeatures: [],
    ownedLinkIds: [],
    unassignedFeatures: [],
    selectedFeatures: [],
    removedFeatures: [],
    otherinfo: {
        startPoint: {
            geom: { sdo_point: { X: '', Y: '' } },
            feature: '',
            beginNodeId: '',
            endNodeId: '',
            linkId: '',
            fromLrs: '',
            toLrs: '',
            lrsValue: ''
        },
        endPoint: {
            geom: { sdo_point: { X: '', Y: '' } },
            feature: '',
            beginNodeId: '',
            endNodeId: '',
            linkId: '',
            fromLrs: '',
            toLrs: '',
            lrsValue: ''
        },
        linkIds: [],
        features: [],
        currentfeatures: []
    }
};



IfxStpmapRoadOwnership.prototype.polygonLayer = null;
IfxStpmapRoadOwnership.prototype.fetchUnassignedRoadsTimeFlag = 0;
IfxStpmapRoadOwnership.prototype.fetchOwnedRoadsTimeFlag = 0;
IfxStpmapRoadOwnership.prototype.setUnassignedRoadsFetchStart = false;
IfxStpmapRoadOwnership.prototype.setOwnedRoadsFetchStart = false;
IfxStpmapRoadOwnership.prototype.organisationId = null;
IfxStpmapRoadOwnership.prototype.colorCode = null;
IfxStpmapRoadOwnership.prototype.isSelectAll = null;
IfxStpmapRoadOwnership.prototype.unassignedTimeFlag = 0;
IfxStpmapRoadOwnership.prototype.deselectTimeFlag = 0;
IfxStpmapRoadOwnership.prototype.createRoadDelegationToolBar = function () {
    var self = this;
    roadDelegationToolbarPanel = new OpenLayers.Control.Panel({ displayClass: 'roaddelegationtoolbarpanel horizontalMap-center roadownershipTool' });
    objifxStpMap.olMap.addControl(roadDelegationToolbarPanel);

    btnSelectAndPlan = new OpenLayers.Control.Button({
        displayClass: 'selectAndPlanLinks',
        title: 'Select road links by planning',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](0, "ROADDELEGATION");
                }
            },
            'deactivate': function () { }
        }
    });

    btnSelectByPolygon = new OpenLayers.Control.Button({
        displayClass: 'selectLinksByPolygon',
        title: 'Select road links by polygon',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                var zoom = objifxStpMap.olMap.getZoom();
                if (zoom < 8) {
                    selectByPolygon.deactivate();
                    showNotification('Selecting road links by polygon is allowed only at zoom level 8 or above.');
                }
                else {
                    selectByPolygon.activate();
                }
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](1, "ROADDELEGATION");
                }
            },
            'deactivate': function () { selectByPolygon.deactivate(); }
        }
    });

    btnSelectByLink = new OpenLayers.Control.Button({
        displayClass: 'selectLinks',
        title: 'Select road links randomly',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](2, "ROADDELEGATION");
                }
            },
            'deactivate': function () { }
        }
    });

    btnDeselectByPolygon = new OpenLayers.Control.Button({
        displayClass: 'deselectLinksByPolygon',
        title: 'Deselect road links by polygon',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                deselectByPolygon.activate();
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](3, "ROADDELEGATION");
                }
            },
            'deactivate': function () { deselectByPolygon.deactivate(); }
        }
    });

    btnDeselectLink = new OpenLayers.Control.Button({
        displayClass: 'deselectLinks',
        title: 'Deselect road links randomly',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](4, "ROADDELEGATION");
                }
            },
            'deactivate': function () { }
        }
    });

    btnUndoSelectAndPlan = new OpenLayers.Control.Button({
        displayClass: 'undoSelectLinks',
        title: 'Undo',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](5, "ROADDELEGATION");
                }
                objifxStpmapRoadOwnership.undoSelectLink();
            },
            'deactivate': function () { }
        }
    });

    roadDelegationToolbarPanel.addControls([btnSelectAndPlan, btnSelectByPolygon, btnSelectByLink, btnDeselectByPolygon, btnDeselectLink, btnUndoSelectAndPlan]);
}

IfxStpmapRoadOwnership.prototype.createPolygonDrawControl = function () {
    var self = this;
    var styleMap = new OpenLayers.StyleMap(OpenLayers.Util.applyDefaults(
                        { strokeOpacity: 0.7, strokeColor: "#1919FF", strokeWidth: 3 },
                        OpenLayers.Feature.Vector.style["default"]));

    this.polygonLayer = new OpenLayers.Layer.Vector("Polygon Layer", { styleMap: styleMap });

    objifxStpMap.olMap.addLayers([this.polygonLayer]);
    selectByPolygon = new OpenLayers.Control.DrawFeature(this.polygonLayer, OpenLayers.Handler.Polygon, {
        title: 'Select road links by polygon',
        'featureAdded': function (obj) {
            self.addFeatureForRoads(obj);
            btnSelectByPolygon.deactivate();
            self.polygonLayer.removeFeatures(self.polygonLayer.features);
        }
    });

    deselectByPolygon = new OpenLayers.Control.DrawFeature(this.polygonLayer, OpenLayers.Handler.Polygon, {
        title: 'Deselect road links by polygon',
        'featureAdded': function (obj) {
            self.removeFeatureForArea(obj);
            btnDeselectByPolygon.deactivate();
            self.polygonLayer.removeFeatures(self.polygonLayer.features);
        }
    });

    objifxStpMap.olMap.addControl(selectByPolygon);
    objifxStpMap.olMap.addControl(deselectByPolygon);
}

IfxStpmapRoadOwnership.prototype.highlightRoads = function (linkInfoList, byGeom, addToList, callback) {
    startAnimation();
    if (linkInfoList == null || linkInfoList == undefined) {
        showNotification('No data available.');
        return;
    }
    var self = this;

    if (byGeom != true) {   //WFS calls to get features
        var linkIds = [];
        var linkcount;

        for (var i = 0; i < linkInfoList.length; i++) {
            //if (linkInfoList[i].fromLinearRef != null && linkInfoList[i].toLinearRef != null) {
            linkIds.push(linkInfoList[i].linkId);
        }
        //self.roadSegment.linkInfoList = linkInfoList;
        objifxStpMap.searchFeaturesByLinkID(linkIds, function (features) {
            self.roadSegment.linkInfoList = features;
            self.addVectorLayers("DelegRoads");
            objifxStpMap.vectorLayerDelegRoads.addFeatures(features);
            objifxStpMap.olMap.zoomToExtent(objifxStpMap.vectorLayerDelegRoads.getDataExtent());

            if (callback && typeof (callback) === "function") {
                callback();
            }
        });
    }

    else {  //highlight using geometry
        var features = [];

        for (var i = 0; i < linkInfoList.length; i++) {
            if (linkInfoList[i].linkGeom != null) {
                var pointArr = [];
                for (var j = 0; j < linkInfoList[i].linkGeom.OrdinatesArray.length; j += 2) {
                    pointArr.push(new OpenLayers.Geometry.Point(linkInfoList[i].linkGeom.OrdinatesArray[j], linkInfoList[i].linkGeom.OrdinatesArray[j + 1]));
                }
                var feature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(pointArr));
                if (linkInfoList[i].fromLinearRef != null || linkInfoList[i].toLinearRef != null) {
                    var featureLength = IfxStpmapCommon.getLengthOfFeature(feature);
                    if (linkInfoList[i].fromLinearRef == null)
                        var lrsStart = 0;
                    else
                        var lrsStart = linkInfoList[i].fromLinearRef / featureLength;
                    if (linkInfoList[i].toLinearRef == null)
                        var lrsEnd = 1;
                    else
                        var lrsEnd = linkInfoList[i].toLinearRef / featureLength;
                    var line = LRSSubstring(feature.geometry, lrsStart, lrsEnd);
                    feature = new OpenLayers.Feature.Vector(line);
                }
                feature.data = linkInfoList[i].linkStatus;
                feature.attributes = { LINK_ID: linkInfoList[i].linkId };
                features.push(feature);
            }
            if (addToList) {
                self.roadSegment.linkInfoList.push({ linkId: linkInfoList[i].linkId, fromLinearRef: linkInfoList[i].fromLinearRef, toLinearRef: linkInfoList[i].toLinearRef });
                self.showAssignedButton();
            }

        }

        if (addToList) {
            self.roadSegment.otherinfo.currentfeatures = features;
        }
        self.addVectorLayers("DelegRoads");

        if (callback && typeof (callback) === "function") {
            callback(features);
        }
    }
}

IfxStpmapRoadOwnership.prototype.showRoadOwnershipOrganisationOwnedRoads = function (OrgId) {
    objifxStpMap.deleteMapLayer("OWNEDROADS");
    var param = "organisationId:" + OrgId + ";";
    var organisationOwnedRoadsLayer = new OpenLayers.Layer.WMS(
        "OWNEDROADS", objifxStpMap.geoserverWmsUrl,
        {
            layers: 'ESDAL4:OWNEDROADS',
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            viewparams: param,
            tiled: true,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(organisationOwnedRoadsLayer);
    objifxStpMap.olMap.setLayerIndex(organisationOwnedRoadsLayer, 1);
   // objifxStpMap.olMap.zoomToExtent(objifxStpMap.olMap.layers[0].getDataExtent());
    //objifxStpMap.olMap.zoomToExtent(objifxStpMap.olMap.getDataExtent());
    this.zoomToGeoRegion(OrgId, function (geoRegion) {
        if (geoRegion != undefined && geoRegion != null) {
            var bounds = new OpenLayers.Bounds();
            bounds.extend(new OpenLayers.LonLat(geoRegion.x1, geoRegion.y1));
            bounds.extend(new OpenLayers.LonLat(geoRegion.x2, geoRegion.y2));
            objifxStpMap.olMap.zoomToExtent(bounds, true);
        }
    });
    }
IfxStpmapRoadOwnership.prototype.zoomToGeoRegion = function (organisationId, callback) {
            $.ajax({
                url: '/RoadDelegation/FetchOrgGeoRegion',
                type: 'POST',
                cache: true,
                //contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    orgId: organisationId
                }),
                success: function (val) {
                    if (val == "Session timeout") {
                        //location.reload();
                        stopAnimation();
                    }
                    else
                        res = val.result;
                    if (res.OrganisationGeoRegion && res.OrganisationGeoRegion.OrdinatesArray != null) {
                        var x1 = res.OrganisationGeoRegion.OrdinatesArray[0];
                        var y1 = res.OrganisationGeoRegion.OrdinatesArray[1];
                        var x2 = res.OrganisationGeoRegion.OrdinatesArray[2];
                        var y2 = res.OrganisationGeoRegion.OrdinatesArray[3];

                        if (callback && typeof (callback) === "function") {
                            callback({ x1: x1, y1: y1, x2: x2, y2: y2 });
                        }
                        //stopAnimation();
                    }
                    else {
                        stopAnimation();
                        showNotification('No owned roads or delegated roads for selected delegator.');
                    }
                },
                complete: function () {
                    //stopAnimation();
                },
            });
        }

IfxStpmapRoadOwnership.prototype.addVectorLayers = function (layer) {
    for (var i = 0; i < objifxStpMap.olMap.layers.length; i++) {
        if (objifxStpMap.olMap.layers[i].name == layer)
            return;
    }
    if (layer == "DelegRoads") {
        objifxStpMap.olMap.addLayers([objifxStpMap.vectorLayerDelegRoads]);
        objifxStpMap.olMap.setLayerIndex(objifxStpMap.vectorLayerDelegRoads, 1);
    }
    else if (layer == "DelegMarkers") {
        objifxStpMap.olMap.addLayers([objifxStpMap.vectorLayerDelegMarkers]);
        objifxStpMap.olMap.setLayerIndex(objifxStpMap.vectorLayerDelegMarkers, 1);
    }
}

IfxStpmapRoadOwnership.prototype.selectAndPlanLink = function (e, pointType) {
    if (pointType == 'STARTPOINT')
        startAnimation("Getting location. Please wait...");
    else
        startAnimation("Planning route. Please wait...");

    if (pointType == 'ENDPOINT' && this.roadSegment.otherinfo.startPoint.linkId == '') {
        stopAnimation();
        return;
    }

    var self = this;
    objifxStpMap.searchFeaturesByXY(e.xy.x, e.xy.y, false, objifxStpMap.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            stopAnimation();
            showNotification('No road selected. Select a valid location.');
        }
        else {
            var lonlat = objifxStpMap.olMap.getLonLatFromPixel({ x: e.xy.x, y: e.xy.y });
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
            var lrsMeasure = LRSMeasure(features[retObject.index].geometry, new OpenLayers.Geometry.Point(retObject.x1, retObject.y1), { tolerance: 0.5, details: true });

            if (pointType == 'STARTPOINT') {
                self.removeDelegationMarker(1);
                self.removeDelegationMarker(0);
                self.roadSegment.startPointGeom.sdo_point = { X: retObject.x1, Y: retObject.y1 };
                self.setMarkerPoint(0, retObject.x1, retObject.y1);
                self.fillPointDetails(0, lrsMeasure, features[retObject.index], { x: retObject.x1, y: retObject.y1 });
            }
            else if (pointType = 'ENDPOINT') {
                if (self.roadSegment.otherinfo.endPoint.linkId == '') {
                    self.roadSegment.endPointGeom.sdo_point = { X: retObject.x1, Y: retObject.y1 };
                    self.setMarkerPoint(1, retObject.x1, retObject.y1);
                    self.fillPointDetails(1, lrsMeasure, features[retObject.index], { x: retObject.x1, y: retObject.y1 });
                }
                else {
                    self.removeDelegationMarker(1);
                    self.roadSegment.endPointGeom.sdo_point = { X: retObject.x1, Y: retObject.y1 };
                    self.setMarkerPoint(1, retObject.x1, retObject.y1);
                    self.roadSegment.otherinfo.startPoint = jQuery.extend(true, {}, self.roadSegment.otherinfo.endPoint);
                    self.fillPointDetails(1, lrsMeasure, features[retObject.index], { x: retObject.x1, y: retObject.y1 });
                }
            }
        }

        if (pointType == 'ENDPOINT') {
            var routeRequest = self.formatRouteRequest(false);
            if (routeRequest.BeginPointLinkId == routeRequest.EndPointLinkId) {
                var feature = self.drawPartial(routeRequest.BeginPointLinkId, function (feature) {
                    if (feature != undefined) {
                        if (self.roadSegment.otherinfo.startPoint.lrsValue < self.roadSegment.otherinfo.endPoint.lrsValue)
                            self.roadSegment.linkInfoList.push({ linkId: routeRequest.BeginPointLinkId, fromLinearRef: self.roadSegment.otherinfo.startPoint.lrsValue, toLinearRef: self.roadSegment.otherinfo.endPoint.lrsValue });
                        else
                            self.roadSegment.linkInfoList.push({ linkId: routeRequest.BeginPointLinkId, fromLinearRef: self.roadSegment.otherinfo.endPoint.lrsValue, toLinearRef: self.roadSegment.otherinfo.startPoint.lrsValue });
                        self.roadSegment.otherinfo.features = feature;

                        self.showAssignedButton();

                        self.addVectorLayers("DelegRoads");
                        self.roadSegment.otherinfo.currentfeatures.push(feature);
                        for (var i = 0; i < self.roadSegment.otherinfo.currentfeatures.length; i++) {
                            self.roadSegment.selectedFeatures.push(self.roadSegment.otherinfo.currentfeatures[i]);
                        }
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.otherinfo.currentfeatures);
                        stopAnimation();
                    }
                    else {
                        stopAnimation();
                    }
                });
            }
            else {
                objifxStpMap.doProcessPlanRouteRequest(routeRequest, function (linkIds) {
                    if (linkIds != null && linkIds.length > 0) {
                        self.highlightPlannedLinks(linkIds, false);
                    }
                    else {
                        var returnRouteRequest = self.formatRouteRequest(true);
                        objifxStpMap.doProcessPlanRouteRequest(returnRouteRequest, function (linkIds) {
                            self.highlightPlannedLinks(linkIds, true);
                        });
                    }
                });
            }
        }
        else {
            stopAnimation();
        }
    });
}

IfxStpmapRoadOwnership.prototype.highlightPlannedLinks = function (linkIds, isReturn) {
    var self = this;
    self.roadSegment.otherinfo.linkIds = [];
    for (var i = 0; i < linkIds.length; i++)
        self.roadSegment.otherinfo.linkIds.push(linkIds[i]);

    if (linkIds[0] == self.roadSegment.otherinfo.startPoint.feature.attributes.LINK_ID)
        self.roadSegment.otherinfo.linkIds.splice(0, 1);
    if (linkIds[linkIds.length - 1] == self.roadSegment.otherinfo.endPoint.feature.attributes.LINK_ID)
        self.roadSegment.otherinfo.linkIds.splice(self.roadSegment.otherinfo.linkIds.length - 1, 1);

    if (self.roadSegment.otherinfo.linkIds.length == 0) {
        self.addVectorLayers("DelegRoads");
        self.createFeatureForRoadSegment(isReturn);
        self.updateLinkInfoList();
        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.otherinfo.currentfeatures);
        stopAnimation();
        return;
    }

    objifxStpMap.searchFeaturesByLinkID(self.roadSegment.otherinfo.linkIds, function (features) {
        if (features != undefined) {
            self.roadSegment.otherinfo.features = features;
            self.addVectorLayers("DelegRoads");
            self.createFeatureForRoadSegment(isReturn);
            self.updateLinkInfoList();
            for (var i = 0; i < self.roadSegment.otherinfo.currentfeatures.length; i++) {
                self.roadSegment.selectedFeatures = self.roadSegment.selectedFeatures.concat(self.roadSegment.otherinfo.currentfeatures[i]);
            }
            objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.otherinfo.currentfeatures);
            stopAnimation();
        }
        else {
            stopAnimation();
        }

        for (var i = 0; i < self.roadSegment.selectedFeatures.length; i++) {
            for (var j = 0; j < objifxStpMap.vectorLayerRoute.features.length; j++) {
                if (self.roadSegment.selectedFeatures[i].attributes.LINK_ID == objifxStpMap.vectorLayerRoute.features[j].attributes.LINK_ID)
                    objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features[j]);
            }
        }

    });
}

IfxStpmapRoadOwnership.prototype.selectByLink = function (e) {
    startAnimation("Selecting road. Please wait...");
    var self = this;
    self.organisationId = $('#hdnorganisationId').val();
    objifxStpMap.searchFeaturesByXY(e.xy.x, e.xy.y, false, objifxStpMap.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            stopAnimation();
            showNotification('No road selected. Select a valid location.');
        }
        else {
            var lonlat = objifxStpMap.olMap.getLonLatFromPixel({ x: e.xy.x, y: e.xy.y });
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
            self.roadSegment.otherinfo.currentfeatures = [features[retObject.index]];
            self.addVectorLayers("DelegRoads");
            objifxStpMap.vectorLayerDelegRoads.addFeatures(features[retObject.index]);
            self.roadSegment.linkInfoList.push({ linkId: features[retObject.index].attributes.LINK_ID, fromLinearRef: null, toLinearRef: null });
            self.roadSegment.selectedFeatures = self.roadSegment.selectedFeatures.concat(features[retObject.index]);
            for (var i = 0; i < self.roadSegment.selectedFeatures.length; i++) {
                for (var j = 0; j < objifxStpMap.vectorLayerRoute.features.length; j++) {
                    if (self.roadSegment.selectedFeatures[i].attributes.LINK_ID == objifxStpMap.vectorLayerRoute.features[j].attributes.LINK_ID)
                        objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features[j]);
                }
            }
            self.showAssignedButton();
            stopAnimation();
        }
    });
}

IfxStpmapRoadOwnership.prototype.searchAndSelectLinks = function (features) {
    startAnimation("Selecting road. Please wait...");
    if (features == null || features.length <= 0) {
        stopAnimation();
        showNotification('No road selected. Select a valid location.');
    }
    else {
        var uniqueFeatures = [];
        for (var i = 0; i < features.length; i++) {
            var uniqueFlag = true;
            for (var j = 0; j < uniqueFeatures.length; j++) {
                if (features[i].attributes.LINK_ID == uniqueFeatures[j].attributes.LINK_ID) {
                    uniqueFlag = false;
                    break;
                }
            }
            if (uniqueFlag) {
                uniqueFeatures.push(features[i]);
            }
        }
        this.roadSegment.otherinfo.currentfeatures = uniqueFeatures;
        this.addVectorLayers("DelegRoads");
        objifxStpMap.vectorLayerDelegRoads.addFeatures(uniqueFeatures);
        for (var i = 0; i < uniqueFeatures.length; i++) {
            this.roadSegment.linkInfoList.push({ linkId: uniqueFeatures[i].attributes.LINK_ID, fromLinearRef: null, toLinearRef: null });
            this.roadSegment.selectedFeatures = this.roadSegment.selectedFeatures.concat(uniqueFeatures[i]);
        }
        for (var i = 0; i < this.roadSegment.selectedFeatures.length; i++) {
            for (var j = 0; j < objifxStpMap.vectorLayerRoute.features.length; j++) {
                if (this.roadSegment.selectedFeatures[i].attributes.LINK_ID == objifxStpMap.vectorLayerRoute.features[j].attributes.LINK_ID)
                    objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features[j]);
            }
        }
        objifxStpMap.olMap.zoomToExtent(objifxStpMap.vectorLayerDelegRoads.getDataExtent());
        this.showAssignedButton();
        stopAnimation();
    }
}

IfxStpmapRoadOwnership.prototype.fillPointDetails = function (pointType, lrsMeasure, feature, geom) {
    var point;
    if (pointType == 0) {
        point = this.roadSegment.otherinfo.startPoint;
        this.roadSegment.otherinfo.endPoint = {
            geom: { sdo_point: { X: '', Y: '' } },
            feature: '',
            beginNodeId: '',
            endNodeId: '',
            linkId: '',
            fromLrs: '',
            toLrs: '',
            lrsValue: ''
        }
    }
    else {
        point = this.roadSegment.otherinfo.endPoint;
    }

    point.lrsValue = Math.round(lrsMeasure.length);
    point.linkId = feature.attributes.LINK_ID;
    point.geom.sdo_point = { X: geom.x, Y: geom.y };
    point.feature = feature;

    if (feature.attributes.DIR_TRAVEL == 'B') {
        if (lrsMeasure.measure <= 0.5) {
            point.beginNodeId = feature.attributes.REF_IN_ID;
            point.endNodeId = feature.attributes.NREF_IN_ID;
        }
        else {
            point.beginNodeId = feature.attributes.NREF_IN_ID;
            point.endNodeId = feature.attributes.REF_IN_ID;
        }
    }
    else if ((feature.attributes.DIR_TRAVEL == 'F' && pointType == 0) || (feature.attributes.DIR_TRAVEL == 'T' && pointType == 1)) {
        point.beginNodeId = feature.attributes.NREF_IN_ID;
        point.endNodeId = feature.attributes.REF_IN_ID;
    }
    else {
        point.beginNodeId = feature.attributes.REF_IN_ID;
        point.endNodeId = feature.attributes.NREF_IN_ID;
    }
}

IfxStpmapRoadOwnership.prototype.formatRouteRequest = function (isReturn) {
    var routeViaPoint = {
        WayPoints: [],
        MaxHeight: "0",
        MaxWeight: "0",
        MaxLength: "0",
        MaxWidth: "0",
        MaxNormAxleLoad: "0",
        MaxShutAxleLoad: "0"
    };

    if (isReturn != true) {
        routeViaPoint.BeginStartNode = this.roadSegment.otherinfo.startPoint.beginNodeId;
        routeViaPoint.BeginPointLinkId = this.roadSegment.otherinfo.startPoint.linkId;
        routeViaPoint.BeginPointEndNode = this.roadSegment.otherinfo.startPoint.endNodeId;

        routeViaPoint.EndPointStartNode = this.roadSegment.otherinfo.endPoint.beginNodeId;
        routeViaPoint.EndPointLinkId = this.roadSegment.otherinfo.endPoint.linkId;
        routeViaPoint.EndPointEndNode = this.roadSegment.otherinfo.endPoint.endNodeId;
    }
    else {
        routeViaPoint.BeginStartNode = this.roadSegment.otherinfo.endPoint.beginNodeId;
        routeViaPoint.BeginPointLinkId = this.roadSegment.otherinfo.endPoint.linkId;
        routeViaPoint.BeginPointEndNode = this.roadSegment.otherinfo.endPoint.endNodeId;

        routeViaPoint.EndPointStartNode = this.roadSegment.otherinfo.startPoint.beginNodeId;
        routeViaPoint.EndPointLinkId = this.roadSegment.otherinfo.startPoint.linkId;
        routeViaPoint.EndPointEndNode = this.roadSegment.otherinfo.startPoint.endNodeId;
    }

    return routeViaPoint;
}

IfxStpmapRoadOwnership.prototype.createFeatureForRoadSegment = function (isReturn) {
    var segmentFeature;
    var featureOfNextLinkID;
    if (isReturn != true) {
        var startPoint = jQuery.extend(true, {}, this.roadSegment.otherinfo.startPoint);
        var endPoint = jQuery.extend(true, {}, this.roadSegment.otherinfo.endPoint);
    }
    else {
        var startPoint = jQuery.extend(true, {}, this.roadSegment.otherinfo.endPoint);
        var endPoint = jQuery.extend(true, {}, this.roadSegment.otherinfo.startPoint);
    }

    if (this.roadSegment.otherinfo.features != null)
        this.roadSegment.otherinfo.currentfeatures = this.roadSegment.otherinfo.features.slice(0);

    if (this.roadSegment.otherinfo.linkIds.length > 0) {
        featureOfNextLinkID = IfxStpmapCommon.getFeatureOfLinkId(this.roadSegment.otherinfo.features,
            this.roadSegment.otherinfo.linkIds[0]);

        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(startPoint.geom, startPoint.feature, featureOfNextLinkID, 'STARTPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (startPoint.feature)
                segmentFeature.feature.attributes = startPoint.feature.attributes;
            this.roadSegment.otherinfo.currentfeatures.push(segmentFeature.feature);

            this.updatePointLrs(segmentFeature.feature, 'STARTPOINT');
        }
        else {
            this.roadSegment.otherinfo.currentfeatures.push(startPoint.feature);
        }

        featureOfNextLinkID = IfxStpmapCommon.getFeatureOfLinkId(this.roadSegment.otherinfo.features,
           this.roadSegment.otherinfo.linkIds[this.roadSegment.otherinfo.linkIds.length - 1]);

        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(endPoint.geom, endPoint.feature, featureOfNextLinkID, 'ENDPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (endPoint.feature)
                segmentFeature.feature.attributes = endPoint.feature.attributes;
            this.roadSegment.otherinfo.currentfeatures.push(segmentFeature.feature);

            this.updatePointLrs(segmentFeature.feature, 'ENDPOINT');
        }
        else {
            this.roadSegment.otherinfo.currentfeatures.push(endPoint.feature);
        }
    }
    else {
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(startPoint.geom,
            startPoint.feature, endPoint.feature, 'STARTPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (startPoint.feature)
                segmentFeature.feature.attributes = startPoint.feature.attributes;
        }
        this.roadSegment.otherinfo.currentfeatures.push(segmentFeature.feature);
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(endPoint.geom,
                        endPoint.feature, startPoint.feature, 'ENDPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (endPoint.feature)
                segmentFeature.feature.attributes = endPoint.feature.attributes;
        }
        this.roadSegment.otherinfo.currentfeatures.push(segmentFeature.feature);
    }
}

IfxStpmapRoadOwnership.prototype.undoSelectLink = function () {
    this.deletePreviousLinks();

    if (this.roadSegment.otherinfo.startPoint.geom.sdo_point.X == this.roadSegment.startPointGeom.sdo_point.X &&
        this.roadSegment.otherinfo.startPoint.geom.sdo_point.Y == this.roadSegment.startPointGeom.sdo_point.Y) {
        this.roadSegment.otherinfo.startPoint = {
            geom: { sdo_point: { X: '', Y: '' } },
            feature: '',
            beginNodeId: '',
            endNodeId: '',
            linkId: '',
            fromLrs: '',
            toLrs: '',
            lrsValue: ''
        };
        this.roadSegment.otherinfo.endPoint = {
            geom: { sdo_point: { X: '', Y: '' } },
            feature: '',
            beginNodeId: '',
            endNodeId: '',
            linkId: '',
            fromLrs: '',
            toLrs: '',
            lrsValue: ''
        }
        this.removeDelegationMarker(1);
        this.removeDelegationMarker(0);
    }
    else {
        this.roadSegment.otherinfo.endPoint = jQuery.extend(true, {}, this.roadSegment.otherinfo.startPoint);
        this.removeDelegationMarker(1);
        this.setMarkerPoint(1, this.roadSegment.otherinfo.endPoint.geom.sdo_point.X, this.roadSegment.otherinfo.endPoint.geom.sdo_point.Y);
    }
}

IfxStpmapRoadOwnership.prototype.setMarkerPoint = function (pointType, X, Y) {
    if (pointType == 0) {
        markerName = 'DELEGSTARTPOINT';
        var imgFile = IfxStpmapCommon.getDelegationMarkerImage(pointType);
        markerStyle = { externalGraphic: imgFile, graphicHeight: 40, graphicWidth: 24, graphicXOffset: -12, graphicYOffset: -40 };
    }
    else {
        markerName = 'DELEGENDPOINT';
        var imgFile = IfxStpmapCommon.getDelegationMarkerImage(pointType);
        markerStyle = { externalGraphic: imgFile, graphicHeight: 40, graphicWidth: 24, graphicXOffset: -12, graphicYOffset: -40 };
    }

    var markerFlag = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(X, Y),
                                                    { name: markerName },
                                                  markerStyle);
    this.addVectorLayers("DelegMarkers");
    objifxStpMap.vectorLayerDelegMarkers.addFeatures(markerFlag);
}

IfxStpmapRoadOwnership.prototype.removeDelegationMarker = function (pointType) {
    objifxStpMap.vectorLayerDelegMarkers.removeFeatures(objifxStpMap.vectorLayerDelegMarkers.features[pointType]);
}

IfxStpmapRoadOwnership.prototype.updateLinkInfoList = function () {
    var linkUpdateFlag = false;
    for (var i = this.roadSegment.linkInfoList.length - 1; i > 0; i--) {
        if (this.roadSegment.linkInfoList[i].linkId == this.roadSegment.otherinfo.startPoint.linkId) {
            this.roadSegment.linkInfoList[i].fromLinearRef = null;
            this.roadSegment.linkInfoList[i].toLinearRef = null;
            linkUpdateFlag = true;
            break;
        }
    }

    if (!linkUpdateFlag) {
        var startLink = {
            linkId: this.roadSegment.otherinfo.startPoint.linkId,
            fromLinearRef: this.roadSegment.otherinfo.startPoint.fromLrs,
            toLinearRef: this.roadSegment.otherinfo.startPoint.toLrs
        };
        this.roadSegment.linkInfoList.push(startLink);
    }

    var link;
    for (var i = 0; i < this.roadSegment.otherinfo.linkIds.length; i++) {
        link = {
            linkId: this.roadSegment.otherinfo.linkIds[i],
            fromLinearRef: null,
            toLinearRef: null
        };
        this.roadSegment.linkInfoList.push(link);
    }

    var endLink = {
        linkId: this.roadSegment.otherinfo.endPoint.linkId,
        fromLinearRef: this.roadSegment.otherinfo.endPoint.fromLrs,
        toLinearRef: this.roadSegment.otherinfo.endPoint.toLrs
    };
    this.roadSegment.linkInfoList.push(endLink);
    this.showAssignedButton();
}

IfxStpmapRoadOwnership.prototype.deletePreviousLinks = function () {
    for (var i = 0; i < this.roadSegment.otherinfo.currentfeatures.length; i++) {
        for (var j = this.roadSegment.linkInfoList.length - 1; j >= 0 ; j--) {
            if (this.roadSegment.otherinfo.currentfeatures[i].attributes.LINK_ID == this.roadSegment.linkInfoList[j].linkId) {
                this.roadSegment.linkInfoList.splice(j, 1);
            }
        }
        for (var k = this.roadSegment.selectedFeatures.length - 1; k >= 0 ; k--) {
            if (this.roadSegment.otherinfo.currentfeatures[i].attributes.LINK_ID == this.roadSegment.selectedFeatures[k].attributes.LINK_ID) {
                this.roadSegment.selectedFeatures.splice(k, 1);
            }
        }
    }
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.otherinfo.currentfeatures);
    this.showAssignedButton();
}

IfxStpmapRoadOwnership.prototype.deleteFromLinkInfoList = function (linkId) {
    for (var i = this.roadSegment.linkInfoList.length - 1; i >= 0; i--) {
        if (this.roadSegment.linkInfoList[i].linkId == linkId) {
            this.roadSegment.linkInfoList.splice(i, 1);
            break;
        }
    }
    this.showAssignedButton();
}

IfxStpmapRoadOwnership.prototype.getOwnerLinkInfoList = function () {
    return this.roadSegment.linkInfoList;
}

IfxStpmapRoadOwnership.prototype.addFeatureForRoads = function (obj) {
    startAnimation("Adding roads. Please wait...");
    this.getFeatureForRoads(obj, function () {
        stopAnimation();
    });
}

IfxStpmapRoadOwnership.prototype.getFeatureForRoads = function (obj, callback) {
    var self = this;
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();
    var geom = {
        OrdinatesArray: [],
        ElemArray: [1, 1003, 1],
        sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
        sdo_srid: IfxStpmapCommon.getSdo_srid()
    };
    for (var i = 0, j = 0; i < obj.geometry.components[0].components.length; i++) {
        geom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].x;
        geom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].y;
    }

    $.ajax({
        url: '/RoadOwnership/GetRoadOwnedDetails',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ fetchFlag: 4, areaGeom: geom, zoomLevel: boundsAndZoom.zoom }),
        success: function (val) {
            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            if (val.length > 0) {
                self.highlightRoads(val, true, true, function (features) {
                    self.roadSegment.selectedFeatures = self.roadSegment.selectedFeatures.concat(features);
                    self.roadSegment.otherinfo.currentfeatures = features;
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.selectedFeatures);
                    self.removeFeatureFromDelegated();

                    for (var i = 0; i < self.roadSegment.selectedFeatures.length; i++) {
                        for (var j = 0; j < objifxStpMap.vectorLayerRoute.features.length; j++) {
                            if (self.roadSegment.selectedFeatures[i].attributes.LINK_ID == objifxStpMap.vectorLayerRoute.features[j].attributes.LINK_ID)
                                objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features[j]);
                        }
                    }
                    stopAnimation();
                });
            }
            else {
                showNotification('No road selected.Select a valid location');
                stopAnimation();
            }
        },
        complete: function () {
            stopAnimation();
        },
    });
}

IfxStpmapRoadOwnership.prototype.deselectLink = function (e) {
    var lonlat = objifxStpMap.olMap.getLonLatFromPixel({ x: e.xy.x, y: e.xy.y });
    var retObject = IfxStpmapCommon.findNearestFeatureIndex(this.roadSegment.selectedFeatures, lonlat.lon, lonlat.lat);

    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.selectedFeatures[retObject.index]);
    this.roadSegment.removedFeatures.push(this.roadSegment.selectedFeatures[retObject.index]);
    this.deleteFromLinkInfoList(this.roadSegment.selectedFeatures[retObject.index].attributes.LINK_ID);
    this.roadSegment.selectedFeatures.splice(retObject.index, 1);
}

IfxStpmapRoadOwnership.prototype.viewUnassignedLayer = function () {
    objifxStpMap.deleteMapLayer("UnassignedRoads");
    var unassignedRoadsLayer = new OpenLayers.Layer.WMS(
        "UnassignedRoads", objifxStpMap.geoserverWmsUrl,
        {
            layers: 'ESDAL3Q32020:UNASSIGNEDROADS',
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(unassignedRoadsLayer);
    objifxStpMap.olMap.setLayerIndex(unassignedRoadsLayer, 1);
}

IfxStpmapRoadOwnership.prototype.showUnassignedRoads = function () {
    if (this.csvImportFlag == true)
        return;
    startAnimation("Fetching roads. Please wait...");
    this.viewUnassignedLayer();
    stopAnimation();
    closeOwnershipFilter();
}

IfxStpmapRoadOwnership.prototype.hideUnassignedRoads = function () {
    objifxStpMap.deleteMapLayer("UnassignedRoads");
    var self = this;
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.unassignedFeatures);
    // self.showOwnedRoads('owned');
}
IfxStpmapRoadOwnership.prototype.viewOrganisationRoadsLayer = function (organisationId) {
    objifxStpMap.deleteMapLayer("OrganisationRoads");
    var param = "organisationId:" + organisationId + ";";
    var organisationRoadsLayer = new OpenLayers.Layer.WMS(
        "OrganisationRoads", objifxStpMap.geoserverWmsUrl,
        {
            layers: 'ESDAL4:ORGANISATIONROADS',
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            viewparams: param,
            tiled: true,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }, { singleTile: true }
    );
    objifxStpMap.olMap.addLayer(organisationRoadsLayer);
    objifxStpMap.olMap.setLayerIndex(organisationRoadsLayer, 1);
}

IfxStpmapRoadOwnership.prototype.showOrganisationRoads = function (organisationId) {
    if (this.csvImportFlag == true)
        return;
   // this.viewOrganisationRoadsLayer(organisationId); //uncomment once dev completed
}

IfxStpmapRoadOwnership.prototype.removeOrganisationRoads = function () {
    objifxStpMap.deleteMapLayer("OrganisationRoads");
}

IfxStpmapRoadOwnership.prototype.showAssignedButton = function () {
    if (this.roadSegment.linkInfoList.length == 0 || this.roadSegment.linkInfoList == undefined) {
        document.getElementById("assignroads").style.display = 'none';
        document.getElementById("clearSelectedRoads").style.display = 'none';
    }
    else {
        document.getElementById("assignroads").style.display = 'block';
        document.getElementById("clearSelectedRoads").style.display = 'block';
    }
}

IfxStpmapRoadOwnership.prototype.clearAllOrgData = function () {
    this.deSelectAllOwnedRoads(true);
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.ownedFeatures);
    this.roadSegment.ownedFeatures = [];
    this.roadSegment.ownedLinkIds = [];
    $('#pageheader').find('h3').text("Road ownership");
    $('#clearOrgData').hide();
    document.getElementById("owned").checked = false;
    $("#showowned").hide();
    this.showAssignedButton();
    this.setOwnedRoadsFetchStart = false;
    this.organisationId = null; 
}

IfxStpmapRoadOwnership.prototype.clearAllSelectedRoads = function () {
    // objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.ownedFeatures);
    //objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features);
    //$('#pageheader').find('h3').text("Road ownership");
    // $("#showowned").hide();
    //this.roadSegment.ownedFeatures = [];
    //this.roadSegment.ownedLinkIds = [];
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.selectedFeatures);
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.unassignedFeatures);
    //this.organisationId = null;  commented for #9430
    document.getElementById("txtLinkSearch").value = '';
    document.getElementById("clearsearchsegments").style.display = 'none';
    document.getElementById("owned").checked = false;
    document.getElementById("unassigned").checked = false;
    this.roadSegment.selectedFeatures = [];
    this.roadSegment.removedFeatures = [];
    this.roadSegment.linkInfoList = [];
    this.roadSegment.otherinfo.currentfeatures = [];
    this.removeDelegationMarker(1);
    this.removeDelegationMarker(0);
    this.showAssignedButton();
    this.isSelectAll = false;
   // this.setOwnedRoadsFetchStart = false;     commented for #9430

    // self.showOwnedRoads('owned');//fetch all unasigned road links
}

IfxStpmapRoadOwnership.prototype.clearSearchSegments = function () {
    objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features);
    document.getElementById("txtLinkSearch").value = '';
    document.getElementById("clearsearchsegments").style.display = 'none';
}

IfxStpmapRoadOwnership.prototype.getAllOwnedRoads = function (callback) {
    var fetchFlag;
    var self = this;
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();
    var geom = {
        OrdinatesArray: [boundsAndZoom.bounds.left, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right,
            boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.top],
        ElemArray: [1, 1003, 1],
        sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
        sdo_srid: IfxStpmapCommon.getSdo_srid()
    };
    if (self.organisationId == null || self.organisationId == undefined)
        return;
    fetchFlag = 1;
    $.ajax({
        url: '/RoadOwnership/GetRoadOwnedDetails',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            organisationId: self.organisationId, fetchFlag: fetchFlag, areaGeom: geom, zoomLevel: boundsAndZoom.zoom
        }),
        beforeSend: function () {
            startAnimation("Selecting roads. Please wait...");
        },
        success: function (val) {
            //objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.ownedFeatures);
            //objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.unassignedFeatures);
            // objifxStpMap.vectorLayerDelegRoads.removeFeatures(objifxStpMap.vectorLayerDelegRoads.features);
            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            else if (val.length > 0) {
                self.highlightRoads(val, true, false, function (features) {
                    if (callback && typeof (callback) === "function") {
                        callback(features);
                    }
                    /*else {
                        self.roadSegment.unassignedFeatures = features;
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.ownedFeatures);
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.unassignedFeatures);
                        self.setUnassignedRoadsFetchStart = true;
                        self.unassignedTimeFlag = 1;
                    }*/


                    stopAnimation();
                });
            }

        },
        complete: function () {
            stopAnimation();
        },
    });

}

IfxStpmapRoadOwnership.prototype.selectAllOwnedRoads = function (addtolistflag, callback) {
    var self = this;
    this.getAllOwnedRoads(function (features) {
        self.isSelectAll = true;
        for (var i = 0; i < features.length; i++) {
            features[i].data = 'selected';
        }
        self.roadSegment.selectedFeatures = self.roadSegment.selectedFeatures.concat(features);
        for (var i = self.roadSegment.selectedFeatures.length - 1; i >= 0; i--) {
            for (var j = 0; j < self.roadSegment.removedFeatures.length; j++) {
                if (self.roadSegment.selectedFeatures[i].attributes.LINK_ID == self.roadSegment.removedFeatures[j].attributes.LINK_ID) {
                    self.roadSegment.selectedFeatures.splice(i, 1);
                    break;
                }
            }
        }
        if (addtolistflag) {
            for (var i = 0; i < self.roadSegment.ownedLinkIds.length; i++) {
                self.roadSegment.linkInfoList.push({ linkId: self.roadSegment.ownedLinkIds[i].linkId, fromLinearRef: null, toLinearRef: null });//add all owned linkIds to LinkInfoList
            }
            self.showAssignedButton();
        }

        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.selectedFeatures);

        if (callback && typeof (callback) === "function") {
            callback();
        }
        //stopAnimation();
    });
    //self.roadSegment.selectedFeatures = self.roadSegment.selectedFeatures.concat(self.roadSegment.ownedFeatures);

}

IfxStpmapRoadOwnership.prototype.deSelectAllOwnedRoads = function (RemoveFromlistflag) {
    startAnimation("Please wait...");
    var self = this;
    self.isSelectAll = false;
    //self.roadSegment.removedFeatures = self.roadSegment.removedFeatures.concat(self.roadSegment.selectedFeatures);

    var tempFeatures = [];
    for (var i = self.roadSegment.selectedFeatures.length - 1; i >= 0; i--) {
        for (var j = 0; j < self.roadSegment.ownedLinkIds.length; j++) {
            if (self.roadSegment.selectedFeatures[i].attributes.LINK_ID == self.roadSegment.ownedLinkIds[j].linkId) {
                tempFeatures.push(self.roadSegment.selectedFeatures[i]);
                self.roadSegment.selectedFeatures.splice(i, 1);
                break;
            }
        }
    }
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(tempFeatures);
    if (RemoveFromlistflag == true)
        self.deselectAllOwnedLinkIds();
}

IfxStpmapRoadOwnership.prototype.deselectAllOwnedLinkIds = function () {
    for (var i = this.roadSegment.linkInfoList.length - 1; i >= 0 ; i--) {
        for (var j = 0; j < this.roadSegment.ownedLinkIds.length; j++) {
            if (this.roadSegment.linkInfoList[i].linkId == this.roadSegment.ownedLinkIds[j].linkId) {
                this.roadSegment.linkInfoList.splice(i, 1);
                break;
            }
        }
    }
    this.showAssignedButton();
    stopAnimation();
}

IfxStpmapRoadOwnership.prototype.fetchUnassignedRoadsOnZoomChange = function (callback) {
    if (this.csvImportFlag == true)
        return;
    if (document.getElementById('unassigned').checked) {
        // this.showOwnedRoads('unassigned', callback, true);
        this.viewUnassignedLayer();
    }
    else {
        if (callback && typeof (callback) === "function") {
            callback();
        }
    }
}

IfxStpmapRoadOwnership.prototype.fetchOwnedRoadsOnZoomChange = function (callback) {
    if (this.csvImportFlag == true)
        return;
    if (this.setOwnedRoadsFetchStart) {
        this.showOwnedRoads('owned', callback);
    }

    else {
        if (callback && typeof (callback) === "function") {
            //callback();
        }
    }
}

IfxStpmapRoadOwnership.prototype.showOwnedRoads = function (fetchValue, callback, fetchOnZoom) {//nithin
    //if (this.csvImportFlag == true)
    //    return;
    var fetchFlag;
    var self = this;
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();
    var geom = {
        OrdinatesArray: [boundsAndZoom.bounds.left, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right,
            boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.top],
        ElemArray: [1, 1003, 1],
        sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
        sdo_srid: IfxStpmapCommon.getSdo_srid()
    };

    if (fetchValue == 'owned') {
        if (self.organisationId == null || self.organisationId == undefined)
            return;
        fetchFlag = 1;
    }

    else {
        if (fetchOnZoom)
            fetchFlag = 3;
        else
            fetchFlag = 2;
    }
    $.ajax({
        url: '/RoadOwnership/GetRoadOwnedDetails',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            organisationId: self.organisationId, fetchFlag: fetchFlag, areaGeom: geom, zoomLevel: boundsAndZoom.zoom
        }),
        beforeSend: function () {
            startAnimation("Fetching roads. Please wait...");
        },
        success: function (val) {
            //objifxStpMap.vectorLayerDelegRoads.removeFeatures(objifxStpMap.vectorLayerDelegRoads.features);
            if (fetchValue == 'owned') {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.ownedFeatures);
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.selectedFeatures);
            }
            else if (fetchValue == 'selected') {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.selectedFeatures);
            }
            else {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.unassignedFeatures);
            }

            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            else if (val.length > 0) {
                if (fetchFlag == 2) {
                    objifxStpMap.searchFeaturesByLinkID([val[Math.round(val.length / 2) - 1].linkId], function (feature) {
                        objifxStpMap.setZoomTo(feature[0].geometry.components[0].x, feature[0].geometry.components[0].y, 9);
                        //self.showOwnedRoads('unassigned', callback, true);
                        return;
                    });
                }
                self.highlightRoads(val, true, false, function (features) {
                    if (fetchValue == 'owned') {
                        self.roadSegment.ownedFeatures = [];
                        self.roadSegment.ownedFeatures = features;
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.ownedFeatures);
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.selectedFeatures);
                        self.setOwnedRoadsFetchStart = true;
                    }
                    else if (fetchValue == 'selected') {
                        self.roadSegment.selectedFeatures = [];
                        if (callback && typeof (callback) === "function") {
                            callback(features);
                        }
                    }
                    else {
                        self.roadSegment.unassignedFeatures = [];
                        self.roadSegment.unassignedFeatures = features;
                        //self.removeFerryRoads(self.roadSegment.ownedFeatures);
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.unassignedFeatures);
                        //objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.selectedFeatures);
                        self.setUnassignedRoadsFetchStart = true;
                        self.unassignedTimeFlag = 1;

                    }
                    if (fetchValue == 'owned' && self.isSelectAll == true) {
                        //objifxStpMap.vectorLayerDelegRoads.removeFeatures(self.roadSegment.selectedFeatures);
                        self.deSelectAllOwnedRoads(false);
                        self.selectAllOwnedRoads(false, function () {
                            if (callback && typeof (callback) === "function") {
                                callback();
                            }
                        });

                    }
                    else {
                        if (callback && typeof (callback) === "function") {
                            callback();
                        }
                    }
                });
            }
            //if (callback && typeof (callback) === "function") {
            //    callback();
            //}
        },
        complete: function () {
            self.showAssignedButton();
            stopAnimation();
        },
    });
}

IfxStpmapRoadOwnership.prototype.zoomInToSelectedLinkId = function (linkId, callback) {//nithin
    if (this.csvImportFlag == true)
        return;
    objifxStpMap.searchFeaturesByLinkID([linkId], function (feature) {
        objifxStpMap.setZoomTo(feature[0].geometry.components[0].x, feature[0].geometry.components[0].y, 12);
        if (callback && typeof (callback) === "function") {
            callback();
        }
    });
}

IfxStpmapRoadOwnership.prototype.zoomInToOwnedRoad = function (LinkInfo, organisationId, callback) {//nithin
    if (this.csvImportFlag == true)
        return;
    var self = this;
    this.roadSegment.ownedLinkIds = LinkInfo;
    var linkIndex = Math.round(LinkInfo.length / 2);
    this.organisationId = organisationId;
    this.roadSegment.linkInfoList = [];
    if (LinkInfo && LinkInfo.length > 0) {
        objifxStpMap.searchFeaturesByLinkID([LinkInfo[linkIndex].linkId], function (feature) {
            objifxStpMap.setZoomTo(feature[0].geometry.components[0].x, feature[0].geometry.components[0].y, 8);
            self.showOwnedRoads('owned', callback);
        });
    }
}

IfxStpmapRoadOwnership.prototype.removeFeatureForArea = function (obj) {
    for (var i = this.roadSegment.selectedFeatures.length - 1; i >= 0; i--) {
        if (obj.geometry.intersects(this.roadSegment.selectedFeatures[i].geometry)) {
            objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.selectedFeatures[i]);
            this.roadSegment.removedFeatures.push(this.roadSegment.selectedFeatures[i]);
            this.deleteFromLinkInfoList(this.roadSegment.selectedFeatures[i].attributes.LINK_ID);
            this.roadSegment.selectedFeatures.splice(i, 1);
        }
    }
}

IfxStpmapRoadOwnership.prototype.removeFeatureFromDelegated = function () {
    for (var i = 0; i < this.roadSegment.removedFeatures.length; i++) {
        for (var j = 0; j < this.roadSegment.ownedFeatures.length; j++) {
            if (this.roadSegment.removedFeatures[i].attributes.LINK_ID == this.roadSegment.ownedFeatures[j].attributes.LINK_ID) {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.ownedFeatures[j]);
                break;
            }
        }
    }
}

IfxStpmapRoadOwnership.prototype.drawPartial = function (linkId, callback) {
    var self = this;

    objifxStpMap.searchFeaturesByLinkID([linkId], function (feature) {
        var lrsStart = LRSMeasure(feature[0].geometry, new OpenLayers.Geometry.Point(self.roadSegment.startPointGeom.sdo_point.X, self.roadSegment.startPointGeom.sdo_point.Y), { tolerance: 2.0 });
        var lrsEnd = LRSMeasure(feature[0].geometry, new OpenLayers.Geometry.Point(self.roadSegment.endPointGeom.sdo_point.X, self.roadSegment.endPointGeom.sdo_point.Y), { tolerance: 2.0 });

        if (lrsStart < lrsEnd)
            var line = LRSSubstring(feature[0].geometry, lrsStart, lrsEnd);
        else
            var line = LRSSubstring(feature[0].geometry, lrsEnd, lrsStart);

        callback(new OpenLayers.Feature.Vector(line));
    });
}

IfxStpmapRoadOwnership.prototype.updatePointLrs = function (feature, pointType) {
    if (pointType == 'STARTPOINT') {
        if (IfxStpmapCommon.compareGeometries(this.roadSegment.startPointGeom.sdo_point, feature.geometry.components[0])) {
            this.roadSegment.otherinfo.startPoint.fromLrs = this.roadSegment.otherinfo.startPoint.lrsValue;
            this.roadSegment.otherinfo.startPoint.toLrs = null;
        }
        else if (IfxStpmapCommon.compareGeometries(this.roadSegment.startPointGeom.sdo_point, feature.geometry.components[feature.geometry.components.length - 1])) {
            this.roadSegment.otherinfo.startPoint.fromLrs = null;
            this.roadSegment.otherinfo.startPoint.toLrs = this.roadSegment.otherinfo.startPoint.lrsValue;
        }
    }
    else {
        if (IfxStpmapCommon.compareGeometries(this.roadSegment.endPointGeom.sdo_point, feature.geometry.components[0])) {
            this.roadSegment.otherinfo.endPoint.fromLrs = this.roadSegment.otherinfo.endPoint.lrsValue;
            this.roadSegment.otherinfo.endPoint.toLrs = null;
        }
        else if (IfxStpmapCommon.compareGeometries(this.roadSegment.endPointGeom.sdo_point, feature.geometry.components[feature.geometry.components.length - 1])) {
            this.roadSegment.otherinfo.endPoint.fromLrs = null;
            this.roadSegment.otherinfo.endPoint.toLrs = this.roadSegment.otherinfo.endPoint.lrsValue;
        }
    }
}