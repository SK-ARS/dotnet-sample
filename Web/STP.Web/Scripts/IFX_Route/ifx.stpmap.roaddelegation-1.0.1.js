function IfxStpmapRoadDelegation() {
}

IfxStpmapRoadDelegation.prototype.roadSegment = {
    startPointGeom: { sdo_point: { X: '', Y: '' } },
    endPointGeom: { sdo_point: { X: '', Y: '' } },
    linkInfoList: [],
    delegatedFeatures: [],
    ownedFeatures: [],
    managedFeatures: [],
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
        completefeatures: []
    }
};
IfxStpmapRoadDelegation.prototype.MapfilterGeom = {
    OrdinatesArray: [],
    ElemArray: [1, 1003, 1],
    sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
    sdo_srid: IfxStpmapCommon.getSdo_srid()
};
IfxStpmapRoadDelegation.prototype.polygonLayer = null;
IfxStpmapRoadDelegation.prototype.fetchAllRoadsTimeFlag = 0;
IfxStpmapRoadDelegation.prototype.fetchDelegRoadsTimeFlag = 0;
IfxStpmapRoadDelegation.prototype.setAllRoadsFetchStart = false;
IfxStpmapRoadDelegation.prototype.setDelegRoadsFetchStart = false;
IfxStpmapRoadDelegation.prototype.currentArrangementId = null;

IfxStpmapRoadDelegation.prototype.createRoadDelegationToolBar = function () {
    var self = this;
    roadDelegationToolbarPanel = new OpenLayers.Control.Panel({ displayClass: 'roaddelegationtoolbarpanel horizontalMap-center tool-center tool-set p-2' });
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
                objifxStpMapRoadDelegation.undoSelectLink();
            },
            'deactivate': function () { }
        }
    });

    roadDelegationToolbarPanel.addControls([btnSelectAndPlan, btnSelectByPolygon, btnSelectByLink, btnDeselectByPolygon, btnDeselectLink, btnUndoSelectAndPlan]);
}

IfxStpmapRoadDelegation.prototype.createMapfilterToolBar = function () {
    var self = this;
    roadDelegationToolbarPanel = new OpenLayers.Control.Panel({ displayClass: 'roaddelegationtoolbarpanel horizontalMap-center roaddelegleft ' });
    objifxStpMap.olMap.addControl(roadDelegationToolbarPanel);

    

    btnSelectByPolygon = new OpenLayers.Control.Button({
        displayClass: 'selectLinksByPolygon',
        title: 'Select road links by polygon',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                var zoom = objifxStpMap.olMap.getZoom();
                if (zoom < 7) {
                    selectByPolygon.deactivate();
                    showNotification('Selecting road links by polygon is allowed only at zoom level 7 or above.');
                }
                else if (document.getElementById('cbStructurefilter').checked == false && document.getElementById('cbRoadfilter').checked == false) {
                    selectByPolygon.deactivate();
                    showNotification('Please check structures or roadworks checkbox');
                }
                else {
                    selectByPolygon.activate();
                }
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](0, "ROADDELEGATION");
                }
            },
            'deactivate': function () { selectByPolygon.deactivate(); }
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
                    objifxStpMap.eventList['DEACTIVATECONTROL'](1, "ROADDELEGATION");
                }
            },
            'deactivate': function () { deselectByPolygon.deactivate(); }
        }
    });
roadDelegationToolbarPanel.addControls([ btnSelectByPolygon,  btnDeselectByPolygon]);
}







IfxStpmapRoadDelegation.prototype.createPolygonDrawControl = function () {
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

IfxStpmapRoadDelegation.prototype.highlightRoads = function (linkInfoList, byGeom, addToList, callback) {
    startAnimation();
    if (linkInfoList == null || linkInfoList == undefined) {
        showNotification('No data available.');
        return;
    }
    var self = this;

    if (byGeom != true) {   //WFS calls to get features
        var linkIds = [];
        for (var i = 0; i < linkInfoList.length; i++) {
            //if (linkInfoList[i].fromLinearRef != null && linkInfoList[i].toLinearRef != null) {
            linkIds.push(linkInfoList[i].LinkId);
            //}
        }
        self.roadSegment.linkInfoList = linkInfoList;
        objifxStpMap.searchFeaturesByLinkID(linkIds, function (features) {
            self.roadSegment.delegatedFeatures = features;
            self.addVectorLayers("DelegRoads");
            objifxStpMap.vectorLayerDelegRoads.addFeatures(features);
            objifxStpMap.olMap.zoomToExtent(objifxStpMap.vectorLayerDelegRoads.getDataExtent());
            objifxStpMap.olMap.setLayerIndex(objifxStpMap.vectorLayerDelegRoads, 1);

            if (callback && typeof (callback) === "function") {
                callback();
            }
        });
    }

    else {  //highlight using geometry
        var features = [];
        for (var i = 0; i < linkInfoList.length; i++) {
            if (linkInfoList[i].LinkGeometry != null) {
                var pointArr = [];
                for (var j = 0; j < linkInfoList[i].LinkGeometry.OrdinatesArray.length; j += 2) {
                    pointArr.push(new OpenLayers.Geometry.Point(linkInfoList[i].LinkGeometry.OrdinatesArray[j], linkInfoList[i].LinkGeometry.OrdinatesArray[j + 1]));
                }
                var feature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(pointArr));
                if (linkInfoList[i].FromLinearRef != null || linkInfoList[i].ToLinearRef != null) {
                    var featureLength = IfxStpmapCommon.getLengthOfFeature(feature);
                    if (linkInfoList[i].FromLinearRef == null)
                        var lrsStart = 0;
                    else
                        var lrsStart = linkInfoList[i].FromLinearRef / featureLength;
                    if (linkInfoList[i].ToLinearRef == null)
                        var lrsEnd = 1;
                    else
                        var lrsEnd = linkInfoList[i].ToLinearRef / featureLength;
                    var line = LRSSubstring(feature.geometry, lrsStart, lrsEnd);
                    feature = new OpenLayers.Feature.Vector(line);
                }
                feature.data = linkInfoList[i].LinkManageStatus;
                feature.attributes = { LINK_ID: linkInfoList[i].LinkId };
                features.push(feature);
            }
            if (addToList) {
                self.roadSegment.linkInfoList.push({ linkId: linkInfoList[i].LinkId, fromLinearRef: linkInfoList[i].FromLinearRef, toLinearRef: linkInfoList[i].ToLinearRef });
                self.roadSegment.delegatedFeatures.push(feature);
            }
           
            else {
                switch (linkInfoList[i].LinkManageStatus) {
                    case 'a':
                        self.roadSegment.ownedFeatures.push(feature);
                        break;
                    case 'b':
                    case 'c':
                        self.roadSegment.managedFeatures.push(feature);
                        break;
                }
            }
        }

        if (addToList) {
            self.roadSegment.otherinfo.completefeatures = features;
        }
        self.addVectorLayers("DelegRoads");

        if (callback && typeof (callback) === "function") {
            callback(features);
        }
    }
}

IfxStpmapRoadDelegation.prototype.addVectorLayers = function (layer) {
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

IfxStpmapRoadDelegation.prototype.selectAndPlanLink = function (e, pointType) {
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
            var DelegateOwnedFeature = self.roadSegment;;
            var SelectLink = [];
            var routeRequest = self.formatRouteRequest(false);
            if (routeRequest.BeginPointLinkId == routeRequest.EndPointLinkId) {
                SelectLink[0] = routeRequest.BeginPointLinkId;
                var DelegFlag = self.checkDelegOwn(DelegateOwnedFeature, SelectLink,0);
                if (DelegFlag == 1) {
                    var feature = self.drawPartial(routeRequest.BeginPointLinkId, function (feature) {
                        if (feature != undefined) {
                            if (self.roadSegment.otherinfo.startPoint.lrsValue < self.roadSegment.otherinfo.endPoint.lrsValue)
                                self.roadSegment.linkInfoList.push({ linkId: routeRequest.BeginPointLinkId, fromLinearRef: self.roadSegment.otherinfo.startPoint.lrsValue, toLinearRef: self.roadSegment.otherinfo.endPoint.lrsValue });
                            else
                                self.roadSegment.linkInfoList.push({ linkId: routeRequest.BeginPointLinkId, fromLinearRef: self.roadSegment.otherinfo.endPoint.lrsValue, toLinearRef: self.roadSegment.otherinfo.startPoint.lrsValue });
                            self.roadSegment.otherinfo.features = feature;

                            self.addVectorLayers("DelegRoads");
                            self.roadSegment.otherinfo.completefeatures.push(feature);
                            for (var i = 0; i < self.roadSegment.otherinfo.completefeatures.length; i++) {
                                self.roadSegment.delegatedFeatures.push(self.roadSegment.otherinfo.completefeatures[i]);
                            }
                            objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.otherinfo.completefeatures);
                            stopAnimation();
                        }
                        else {
                            stopAnimation();
                        }
                    });
                }
                else {
                    showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                    stopAnimation();
                }
            }
            else {
                objifxStpMap.doProcessPlanRouteRequest(routeRequest, function (linkIds) {
                    if (linkIds != null && linkIds.length > 0) {

                        //var RetLink = self.checkDelegOwn(DelegateOwnedFeature, linkIds, 1);
                        ////console.log(RetLink);
                        //if (RetLink.length > 0) {
                        self.highlightPlannedLinks(linkIds, false);
                            //if (RetLink.length != linkIds.length) {
                            //    showNotification('The roads you have selected contain some roads not owned by the delegator. Those roads have been excluded and the roads owned by the delegator are highlighted green.');
                            //    stopAnimation();
                            //}
                           
                        //}
                        //else {
                        //    showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                        //    stopAnimation();
                        //}

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

IfxStpmapRoadDelegation.prototype.highlightPlannedLinks = function (linkIds, isReturn) {
    var self = this;
    self.checkLinkOwnership(linkIds, null, function (linkIds) {
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
            objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.otherinfo.completefeatures);
            stopAnimation();
            return;
        }

        objifxStpMap.searchFeaturesByLinkID(self.roadSegment.otherinfo.linkIds, function (features) {
            if (features != undefined) {
                self.roadSegment.otherinfo.features = features;
                self.addVectorLayers("DelegRoads");
                self.createFeatureForRoadSegment(isReturn);
                for (var cflength = self.roadSegment.otherinfo.completefeatures.length - 1; cflength >= 0; cflength--) {
                    var removeflag = 0;
                    for (var linkidslength = 0; linkidslength < linkIds.length - 1; linkidslength++) {
                        if (self.roadSegment.otherinfo.completefeatures[cflength].attributes.LINK_ID == linkIds[linkidslength]) {
                            removeflag = 1;
                            break;
                        }
                    }
                    if (removeflag == 0) {
                        self.roadSegment.otherinfo.completefeatures.splice(cflength, 1);
                    }

                }
                self.updateLinkInfoList();
                for (var i = 0; i < self.roadSegment.otherinfo.completefeatures.length; i++) {
                    self.roadSegment.delegatedFeatures.push(self.roadSegment.otherinfo.completefeatures[i]);
                }
                objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.otherinfo.completefeatures);
                stopAnimation();
            }
            else {
                stopAnimation();
            }
        });
    });
}

IfxStpmapRoadDelegation.prototype.selectByLink = function (e) {
    var DelegateOwnedFeature = this.roadSegment;
    startAnimation("Selecting road. Please wait...");
    var self = this;
    objifxStpMap.searchFeaturesByXY(e.xy.x, e.xy.y, false, objifxStpMap.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            stopAnimation();
            showNotification('No road selected. Select a valid location.');
        }
        else {
            //var SelectLink = [];
            //for (var i = 0; i < features.length; i++) {
            //    SelectLink[i]= features[i].attributes.LINK_ID;
            //} 
            //var DelegFlag = self.checkDelegOwn(DelegateOwnedFeature, SelectLink,0);
            //if (DelegFlag == 0) {
                var lonlat = objifxStpMap.olMap.getLonLatFromPixel({ x: e.xy.x, y: e.xy.y });
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
                self.checkLinkOwnership(features[retObject.index].attributes.LINK_ID, null, function (linkId) {
                    if (linkId.length > 0) {
                        self.roadSegment.otherinfo.completefeatures = features[retObject.index];
                        self.addVectorLayers("DelegRoads");
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(features[retObject.index]);
                        self.roadSegment.linkInfoList.push({ linkId: features[retObject.index].attributes.LINK_ID, fromLinearRef: null, toLinearRef: null });
                        self.roadSegment.delegatedFeatures.push(features[retObject.index]);
                        self.deleteFromRemovedFeatures(features[retObject.index].attributes.LINK_ID);
                        stopAnimation();
                    }
                    else {
                        showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                        stopAnimation();
                    }
                });
            //}
            //else {
            //    showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
            //    stopAnimation();
            //}
    }
    });
}

IfxStpmapRoadDelegation.prototype.searchAndSelectLinks = function (features) {
    startAnimation("Selecting road. Please wait...");
    var self = this;
    if (features == null || features.length <= 0) {
        stopAnimation();
        showNotification('No road selected. Select a valid location.');
    }
    else {
        var uniqueFeatures = [], uniqueLinks = [];
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
                uniqueLinks.push(features[i].attributes.LINK_ID);
            }
        }
        self.checkLinkOwnership(uniqueLinks, null, function (linkId) {
            if (linkId.length > 0) {
                var returnFeatures = [];
                for (var i = 0; i < uniqueFeatures.length; i++) {
                    for (var j = 0; j < linkId.length; j++) {
                        if (uniqueFeatures[i].attributes.LINK_ID == linkId[j]) {
                            returnFeatures.push(uniqueFeatures[i]);
                            break;
                        }
                    }
                }

                self.roadSegment.otherinfo.completefeatures = returnFeatures;
                self.addVectorLayers("DelegRoads");
                objifxStpMap.vectorLayerDelegRoads.addFeatures(returnFeatures);
                for (var i = 0; i < returnFeatures.length; i++) {
                    self.roadSegment.linkInfoList.push({ linkId: returnFeatures[i].attributes.LINK_ID, fromLinearRef: null, toLinearRef: null });
                    self.roadSegment.delegatedFeatures.push(returnFeatures[i]);
                }
                objifxStpMap.olMap.zoomToExtent(objifxStpMap.vectorLayerDelegRoads.getDataExtent());
                stopAnimation();
            }
            else {
                stopAnimation();
            }
        });
    }
}

IfxStpmapRoadDelegation.prototype.fillPointDetails = function (pointType, lrsMeasure, feature, geom) {
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

IfxStpmapRoadDelegation.prototype.formatRouteRequest = function (isReturn) {
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

IfxStpmapRoadDelegation.prototype.createFeatureForRoadSegment = function (isReturn) {
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
        this.roadSegment.otherinfo.completefeatures = this.roadSegment.otherinfo.features.slice(0);

    if (this.roadSegment.otherinfo.linkIds.length > 0) {
        featureOfNextLinkID = IfxStpmapCommon.getFeatureOfLinkId(this.roadSegment.otherinfo.features,
            this.roadSegment.otherinfo.linkIds[0]);
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(startPoint.geom, startPoint.feature, featureOfNextLinkID, 'STARTPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (startPoint.feature)
                segmentFeature.feature.attributes = startPoint.feature.attributes;
            this.roadSegment.otherinfo.completefeatures.push(segmentFeature.feature);

            this.updatePointLrs(segmentFeature.feature, 'STARTPOINT');
        }
        else {
            this.roadSegment.otherinfo.completefeatures.push(startPoint.feature);
        }
        featureOfNextLinkID = IfxStpmapCommon.getFeatureOfLinkId(this.roadSegment.otherinfo.features,
           this.roadSegment.otherinfo.linkIds[this.roadSegment.otherinfo.linkIds.length - 1]);
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(endPoint.geom, endPoint.feature, featureOfNextLinkID, 'ENDPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (endPoint.feature)
                segmentFeature.feature.attributes = endPoint.feature.attributes;
            this.roadSegment.otherinfo.completefeatures.push(segmentFeature.feature);

            this.updatePointLrs(segmentFeature.feature, 'ENDPOINT');
        }
        else {
            this.roadSegment.otherinfo.completefeatures.push(endPoint.feature);
        }
    }
    else {
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(startPoint.geom,
            startPoint.feature, endPoint.feature, 'STARTPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (startPoint.feature)
                segmentFeature.feature.attributes = startPoint.feature.attributes;
        }
        this.roadSegment.otherinfo.completefeatures.push(segmentFeature.feature);
        segmentFeature = IfxStpmapCommon.getRoutePointSegmentFeature(endPoint.geom,
                        endPoint.feature, startPoint.feature, 'ENDPOINT');
        if (segmentFeature.feature.geometry != null) {
            if (endPoint.feature)
                segmentFeature.feature.attributes = endPoint.feature.attributes;
        }
        this.roadSegment.otherinfo.completefeatures.push(segmentFeature.feature);
    }
}

IfxStpmapRoadDelegation.prototype.undoSelectLink = function () {
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

IfxStpmapRoadDelegation.prototype.setMarkerPoint = function (pointType, X, Y) {
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

IfxStpmapRoadDelegation.prototype.removeDelegationMarker = function (pointType) {
    objifxStpMap.vectorLayerDelegMarkers.removeFeatures(objifxStpMap.vectorLayerDelegMarkers.features[pointType]);
}

IfxStpmapRoadDelegation.prototype.updateLinkInfoList = function () {
    var linkUpdateFlag = false;
    for (var i = this.roadSegment.linkInfoList.length - 1; i > 0; i--) {
        if (this.roadSegment.linkInfoList[i].linkId == this.roadSegment.otherinfo.startPoint.linkId) {
            this.roadSegment.linkInfoList[i].FromLinearRef = null;
            this.roadSegment.linkInfoList[i].ToLinearRef = null;
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
}

IfxStpmapRoadDelegation.prototype.deletePreviousLinks = function () {
    for (var i = 0; i < this.roadSegment.otherinfo.completefeatures.length; i++) {
        for (var j = this.roadSegment.linkInfoList.length - 1; j >= 0 ; j--) {
            if (this.roadSegment.otherinfo.completefeatures[i].attributes.LINK_ID == this.roadSegment.linkInfoList[j].linkId) {
                this.roadSegment.linkInfoList.splice(j, 1);
            }
        }
        for (var k = this.roadSegment.delegatedFeatures.length - 1; k >= 0 ; k--) {
            if (this.roadSegment.otherinfo.completefeatures[i].attributes.LINK_ID == this.roadSegment.delegatedFeatures[k].attributes.LINK_ID) {
                this.roadSegment.delegatedFeatures.splice(k, 1);
            }
        }
    }
    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.otherinfo.completefeatures);
}

IfxStpmapRoadDelegation.prototype.deleteFromLinkInfoList = function (linkId) {
    ;
    for (var i = this.roadSegment.linkInfoList.length - 1; i >= 0; i--) {
        if (this.roadSegment.linkInfoList[i].linkId == linkId) {
            this.roadSegment.linkInfoList.splice(i, 1);
           // break;//commented since multiple rcord are there to remove
        }
    }
}
IfxStpmapRoadDelegation.prototype.deleteFromRemovedFeatures = function (linkId) {
    for (var k = 0; k < this.roadSegment.removedFeatures.length; k++) {
        if (this.roadSegment.removedFeatures[k].attributes.LINK_ID == linkId) {
            this.roadSegment.removedFeatures.splice(k, 1);
            break;
        }
    }
}

IfxStpmapRoadDelegation.prototype.getLinkInfoList = function () {
    return this.roadSegment.linkInfoList;
}

IfxStpmapRoadDelegation.prototype.addFeatureForRoads = function (obj) {
    startAnimation("Adding roads. Please wait...");
    if (objifxStpMap.pageType == 'DELEGATION_VIEWANDEDIT') {
        this.getFeatureForRoads(obj, function () {
            stopAnimation();
        });
    }
    else if (objifxStpMap.pageType == 'SORTMAPFILTER_VIEWANDEDIT') {
        this.getFeatureForRoadsForMapfilter(obj, function () {
           // stopAnimation();
        });
	}
}

IfxStpmapRoadDelegation.prototype.getFeatureForRoads = function (obj, callback) {
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

    var fromOrgId = parseInt($('#hdnFromOrgId').val());
    $.ajax({
        url: '/RoadDelegation/GetRoadDelegationDetails',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            arrangementId: 0, fetchFlag: 7, areaGeom: geom, zoomLevel: boundsAndZoom.zoom,
            searchParam: { fromOrgId: fromOrgId }
        }),
        success: function (val) {
            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            if (val.length > 0) {
                var allLinks = [];
                for (var i = 0; i < val.length; i++) {
                    allLinks.push(val[i].LinkId);
				}
                self.checkLinkOwnership(allLinks, fromOrgId, function (linkId) {
                    for (var j = val.length - 1; j >= 0; j--) {
                       
                        var removeflag = 1;
                        for (var k = 0; k < linkId.length; k++) {
                            if (val[j].LinkId == linkId[k]) {
                                
                                removeflag = 0;
                                break;
							}
                        }
                        if (removeflag == 1) {
                            val.splice(j, 1);
                            
						}
					}
                    for (var i = 0; i < val.length; i++) {
                        self.deleteFromRemovedFeatures(val[i].LinkId);
                    }
                    self.highlightRoads(val, true, true, function () {
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
                        self.removeFeatureFromDelegated();
                        stopAnimation();
                    });
                });
            }
            else {
                showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                stopAnimation();
            }
        },
        complete: function () {
            stopAnimation();
        },
    });
}

IfxStpmapRoadDelegation.prototype.getFeatureForRoadsForMapfilter = function (obj, callback) {


    this.MapfilterGeom.OrdinatesArray = [];
    for (var i = 0, j = 0; i < obj.geometry.components[0].components.length; i++) {
        this.MapfilterGeom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].x;
        this.MapfilterGeom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].y;
    }
    $.ajax({
        url: '/SORTApplication/SetSearchGeometry',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            areaGeom: this.MapfilterGeom
        }),
        success: function () {
           
            
            
        },
        complete: function () {
          //  stopAnimation();
        },
    });


    if (document.getElementById('cbStructurefilter').checked == true) {
        $.ajax({
            url: '/Movements/GetStructuresAndRoad',
            type: 'POST',
            cache: true,
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                structureFlag: 1, areaGeom: this.MapfilterGeom
            }),
            beforeSend: function () {
                startAnimation();
            },
            success: function (val) {
                if (val == "Session timeout") {
                    //location.reload();
                    stopAnimation();
                }
                if (val.length > 0) {

                    
                    showStructures(val, false, 'SORTInbox');
                    //stopAnimation();
                }
                else {
                    //showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                    stopAnimation();
                }
            },
            error: function () {
                stopAnimation();
            },

            complete: function () {
                if (document.getElementById('cbRoadfilter').checked == false) {
                    stopAnimation();
                }
               // 
            },
        });
    }
    if (document.getElementById('cbRoadfilter').checked == true) {
        $.ajax({
            url: '/Movements/GetStructuresAndRoad',
            type: 'POST',
            cache: true,
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                structureFlag: 2, areaGeom: this.MapfilterGeom
            }),
            beforeSend: function () {
                startAnimation();
            },
            success: function (val) {
                if (val == "Session timeout") {
                    //location.reload();
                    stopAnimation();
                }
                if (val.length > 0) {
                    //for (var i = 0; i < val.length; i++) {
                    //    self.deleteFromRemovedFeatures(val[i].LinkId);
                    //}
                    self.highlightRoads(val, true, true, function () {
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
                        self.removeFeatureFromDelegated();
                        stopAnimation();
                    });
                }
                else {
                    //showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                    stopAnimation();
                }
            },
            error: function () {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            },
        });
    }



    //stopAnimation();
}





IfxStpmapRoadDelegation.prototype.deselectLink = function (e) {
    ;
    var lonlat = objifxStpMap.olMap.getLonLatFromPixel({ x: e.xy.x, y: e.xy.y });
    var retObject = IfxStpmapCommon.findNearestFeatureIndex(this.roadSegment.delegatedFeatures, lonlat.lon, lonlat.lat);

    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.delegatedFeatures[retObject.index]);
    this.roadSegment.removedFeatures.push(this.roadSegment.delegatedFeatures[retObject.index]);
    this.deleteFromLinkInfoList(this.roadSegment.delegatedFeatures[retObject.index].attributes.LINK_ID);
    this.roadSegment.delegatedFeatures.splice(retObject.index, 1);
}

IfxStpmapRoadDelegation.prototype.showDelegationOwnedRoads = function (OrgId) {
    objifxStpMap.deleteMapLayer("RDOWNEDROADS");
    var param = "organisationId:" + OrgId + ";";
    var organisationOwnedRoadsLayer = new OpenLayers.Layer.WMS(
        "RDOWNEDROADS", objifxStpMap.geoserverWmsUrl,
        {
            layers: 'ESDAL4:RDOWNEDROADS',
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            viewparams: param,
            tiled: true,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(organisationOwnedRoadsLayer);
    objifxStpMap.olMap.setLayerIndex(organisationOwnedRoadsLayer, 2);
    // objifxStpMap.olMap.zoomToExtent(objifxStpMap.olMap.layers[0].getDataExtent());
    //objifxStpMap.olMap.zoomToExtent(objifxStpMap.olMap.getDataExtent());
}

IfxStpmapRoadDelegation.prototype.showDelegationManagedRoads = function (OrgId) {
    objifxStpMap.deleteMapLayer("RDMANAGEDROADS");
    var param = "organisationId:" + OrgId + ";";
    var organisationManagedRoadsLayer = new OpenLayers.Layer.WMS(
        "RDMANAGEDROADS", objifxStpMap.geoserverWmsUrl,
        {
            layers: 'ESDAL4:RDMANAGEDROADS',
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            viewparams: param,
            tiled: true,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(organisationManagedRoadsLayer);
    objifxStpMap.olMap.setLayerIndex(organisationManagedRoadsLayer, 2);
    // objifxStpMap.olMap.zoomToExtent(objifxStpMap.olMap.layers[0].getDataExtent());
    //objifxStpMap.olMap.zoomToExtent(objifxStpMap.olMap.getDataExtent());
}

IfxStpmapRoadDelegation.prototype.fetchAllRoadLinks = function (callback) {
    var self = this;
    var fromOrgId = parseInt($('#hdnFromOrgId').val());
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();
    if ($('#txtLinkSearch').val() != undefined) {
        var letters = /^[A-Za-z]+$/;
        if ($('#txtLinkSearch').val().match(letters)) {
            $('#txtLinkSearch').val('');
        }
    }

    var geom = {
        OrdinatesArray: [boundsAndZoom.bounds.left, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right,
            boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.top],
        ElemArray: [1, 1003, 1],
        sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
        sdo_srid: IfxStpmapCommon.getSdo_srid()
    };

    $.ajax({
        url: '/RoadDelegation/GetRoadDelegationDetails',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            arrangementId: 0, fetchFlag: 5, areaGeom: geom, zoomLevel: boundsAndZoom.zoom,
            searchParam: { fromOrgId: fromOrgId }
        }),
        beforeSend: function () {
            startAnimation("Fetching roads. Please wait...");
            objifxStpMap.vectorLayerDelegRoads.removeFeatures(objifxStpMap.vectorLayerDelegRoads.features);
            //objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
            self.roadSegment.ownedFeatures = [];
            self.roadSegment.managedFeatures = [];
        },
        success: function (val) {
            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            else if (val.length > 0) {
                self.highlightRoads(val, true, false, function () {
                    if (roadDelegationCheckValue('owned'))
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.ownedFeatures);
                    if (roadDelegationCheckValue('managed'))
                        objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.managedFeatures);
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
                    self.removeFeatureFromDelegated();
                    stopAnimation();
                });
            }
            else {
                objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
                self.removeFeatureFromDelegated();
                stopAnimation();
            }
            if (callback && typeof (callback) === "function") {
                callback();
            }
        },
        complete: function () {
            stopAnimation();
        },
    });
}

IfxStpmapRoadDelegation.prototype.showAllRoads = function (zoomToOrg) {
    var self = this;
    var orgId = parseInt($('#hdnFromOrgId').val());

    if (zoomToOrg) {
        this.zoomToGeoRegion(orgId, function (geoRegion) {
            if (geoRegion != undefined && geoRegion != null) {
                var bounds = new OpenLayers.Bounds();
                bounds.extend(new OpenLayers.LonLat(geoRegion.x1, geoRegion.y1));
                bounds.extend(new OpenLayers.LonLat(geoRegion.x2, geoRegion.y2));
                objifxStpMap.olMap.zoomToExtent(bounds, true);
            }
            ShowDelegationManagedRoads(orgId);

            ShowDelegationOwnedRoads(orgId);
            

            //self.fetchAllRoadLinks(function () {
            //    self.fetchAllRoadsTimeFlag = 1;
            //    self.setAllRoadsFetchStart = true;
            //});

        });
    }
    else {
       ShowDelegationManagedRoads(orgId);

            ShowDelegationOwnedRoads(orgId);
    }
}
IfxStpmapRoadDelegation.prototype.DeleteManagedRoads = function () {
    objifxStpMap.deleteMapLayer("RDMANAGEDROADS");
}

IfxStpmapRoadDelegation.prototype.DeleteOwnedRoads = function () {
    objifxStpMap.deleteMapLayer("RDOWNEDROADS");
}

IfxStpmapRoadDelegation.prototype.zoomToGeoRegion = function (organisationId, callback) {
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

IfxStpmapRoadDelegation.prototype.fetchAllRoadsOnZoomChange = function (callback) {
    if (this.setAllRoadsFetchStart) {
        this.fetchAllRoadLinks(callback);
    }
    else {
        if (callback && typeof (callback) === "function") {
            callback();
        }
    }
}

IfxStpmapRoadDelegation.prototype.fetchDelegRoadsOnZoomChange = function (callback) {
    if (this.setDelegRoadsFetchStart) {
        this.showDelegatedRoads(callback);
    }
    else {
        if (callback && typeof (callback) === "function") {
            callback();
        }
    }
}

IfxStpmapRoadDelegation.prototype.showDelegatedRoads = function (callback) {
    if (this.currentArrangementId == null || this.currentArrangementId == undefined)
        return;
    var self = this;
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();

    var geom = {
        OrdinatesArray: [boundsAndZoom.bounds.left, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right, boundsAndZoom.bounds.top, boundsAndZoom.bounds.right,
            boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.bottom, boundsAndZoom.bounds.left, boundsAndZoom.bounds.top],
        ElemArray: [1, 1003, 1],
        sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
        sdo_srid: IfxStpmapCommon.getSdo_srid()
    };

    $.ajax({
        url: '/RoadDelegation/GetRoadDelegationDetails',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            arrangementId: self.currentArrangementId, fetchFlag: 6, areaGeom: geom, zoomLevel: boundsAndZoom.zoom
        }),
        beforeSend: function () {
            startAnimation("Fetching roads. Please wait...");
        },
        success: function (val) {
            objifxStpMap.vectorLayerDelegRoads.removeFeatures(objifxStpMap.vectorLayerDelegRoads.features);
            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            else if (val.length > 0) {
                self.highlightRoads(val, true, false, function (features) {
                    //if (self.roadSegment.delegatedFeatures.length == 0) {
                        self.roadSegment.delegatedFeatures = features;
                    //}
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
                    self.removeFeatureFromDelegated();
                    stopAnimation();
                });
            }
            else {
                objifxStpMap.vectorLayerDelegRoads.addFeatures(self.roadSegment.delegatedFeatures);
                self.removeFeatureFromDelegated();
                stopAnimation();
            }
            if (callback && typeof (callback) === "function") {
                callback();
            }
        },
        complete: function () {
            stopAnimation();
        },
    });
}

IfxStpmapRoadDelegation.prototype.zoomInToDelegRoad = function (link, arrangementId, callback) {
    var self = this;
    this.currentArrangementId = arrangementId;
    objifxStpMap.searchFeaturesByLinkID([link.LinkId], function (feature) {
        objifxStpMap.setZoomTo(feature[0].geometry.components[0].x, feature[0].geometry.components[0].y, 8);
        self.showDelegatedRoads(callback);
        self.fetchDelegRoadsTimeFlag = 1;
        self.setDelegRoadsFetchStart = true;
    });
}

IfxStpmapRoadDelegation.prototype.addToRoadLinks = function (linkInfoList) {
    for (var i = 0; i < linkInfoList.length; i++) {
        this.roadSegment.linkInfoList.push({ linkId: linkInfoList[i].LinkId, fromLinearRef: linkInfoList[i].FromLinearRef, toLinearRef: linkInfoList[i].ToLinearRef });
    }
}

IfxStpmapRoadDelegation.prototype.showAndHideRoads = function (linkMangeStatus, boolShow) {
    switch (linkMangeStatus) {
        case 'owned':
            switch (boolShow) {
                case true:
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(this.roadSegment.ownedFeatures);
                    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.delegatedFeatures);
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(this.roadSegment.delegatedFeatures);
                    break;
                case false:
                    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.ownedFeatures);
                    break;
            }
            break;
        case 'managed':
            switch (boolShow) {
                case true:
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(this.roadSegment.managedFeatures);
                    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.delegatedFeatures);
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(this.roadSegment.delegatedFeatures);
                    break;
                case false:
                    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.managedFeatures);
                    break;
            }
            break;
        default:
            switch (boolShow) {
                case true:
                    objifxStpMap.vectorLayerDelegRoads.addFeatures(this.roadSegment.delegatedFeatures);
                    break;
                case false:
                    objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.delegatedFeatures);
                    break;
            }
    }
}

IfxStpmapRoadDelegation.prototype.removeFeatureForArea = function (obj) {
   
    if (objifxStpMap.pageType == 'SORTMAPFILTER_VIEWANDEDIT') {
        //for (var i = 0; i < objifxStpMap.vectorLayerStructures.features.length; i++) {
        for (var i = objifxStpMap.vectorLayerStructures.features.length - 1; i >= 0; i--) {
            if (obj.geometry.intersects(objifxStpMap.vectorLayerStructures.features[i].geometry)) {
                objifxStpMap.vectorLayerStructures.removeFeatures(objifxStpMap.vectorLayerStructures.features[i]);
               
                
			}
        }
        for (var k = 0;k< objifxStpmapStructures.markerStructure.length;k++) {
            if (obj.geometry.intersects(objifxStpmapStructures.markerStructure[k][0].geometry)) {
                objifxStpmapStructures.markerStructure.splice(k, 1);
                k--;
            }
		}
        
            for (var j = objifxStpMap.vectorLayerDelegRoads.features.length - 1; j >= 0;j--) {
            if (obj.geometry.intersects(objifxStpMap.vectorLayerDelegRoads.features[j].geometry)) {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(objifxStpMap.vectorLayerDelegRoads.features[j]);
            }
        }
    }
    else {
        for (var i = this.roadSegment.delegatedFeatures.length - 1; i >= 0; i--) {
            if (obj.geometry.intersects(this.roadSegment.delegatedFeatures[i].geometry)) {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.delegatedFeatures[i]);
                this.roadSegment.removedFeatures.push(this.roadSegment.delegatedFeatures[i]);
                this.deleteFromLinkInfoList(this.roadSegment.delegatedFeatures[i].attributes.LINK_ID);
                this.roadSegment.delegatedFeatures.splice(i, 1);
            }
        }
    }
    
}

IfxStpmapRoadDelegation.prototype.checkLinkOwnership = function (linkIds, orgId, callback) {
    var self = this;
    if (orgId == null) {
        orgId = parseInt($('#hdnFromOrgId').val());
    }
    $.ajax({
        url: '/RoadDelegation/GetLinksAllowedForDelegation',
        type: 'POST',
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            linkIdList: linkIds, fromOrgId: orgId
        }),
        success: function (val) {
            if (val == "Session timeout") {
                 //location.reload();
                stopAnimation();
            }
            if (val.result.length > 0) {
                if (callback && typeof (callback) === "function") {
                    callback(val.result);
                }
            }
            else {
                showNotification('The selected road link(s) is(are) not owned by the delegator. Please select the road links that are highlighted blue for delegation.');
                //callback([25422932, 25422876]);
                //self.removeDelegationMarker(1);
                //self.removeDelegationMarker(0);
                //showWarningPopDialog('Selection failed. The selected road(s) cannot be delegated.', 'Ok', '', 'close_alert', '', 1, 'info');
            }
        },
        complete: function () {
            stopAnimation();
        },
    });
}

IfxStpmapRoadDelegation.prototype.removeFeatureFromDelegated = function () {
    for (var i = 0; i < this.roadSegment.removedFeatures.length; i++) {
        for (var j = 0; j < this.roadSegment.delegatedFeatures.length; j++) {
            if (this.roadSegment.removedFeatures[i].attributes.LINK_ID == this.roadSegment.delegatedFeatures[j].attributes.LINK_ID) {
                objifxStpMap.vectorLayerDelegRoads.removeFeatures(this.roadSegment.delegatedFeatures[j]);
                break;
            }
        }
    }
}

IfxStpmapRoadDelegation.prototype.drawPartial = function (linkId, callback) {
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

IfxStpmapRoadDelegation.prototype.updatePointLrs = function (feature, pointType) {
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
IfxStpmapRoadDelegation.prototype.checkDelegOwn = function (DelegateOwnedFeature, SelectLink,Flag) {    // function for checking delegate ownership when select a link by afsal
    var DelegFlag = 0, RetLinkNO = -1;
    var RetLink = [];  // contains only owned links 
        for (var i = 0; i < DelegateOwnedFeature.ownedFeatures.length; i++) {
            for (var j = 0; j < SelectLink.length; j++) {
                if ((DelegateOwnedFeature.ownedFeatures[i].attributes.LINK_ID) == SelectLink[j]) {
                    DelegFlag = 1;
                    RetLinkNO++;
                    RetLink[RetLinkNO] = SelectLink[j];
                }
            }
        }
        if (Flag == 0) {
            return DelegFlag;    //if it is only single link id then returns flag variable(select road links randomly)
        }
        else {
            return RetLink;    // more than one links returns links that are owned by organisation(select road links by planning)
        }
}