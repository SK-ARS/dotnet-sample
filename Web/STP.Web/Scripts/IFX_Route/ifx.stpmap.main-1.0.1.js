//constructor
function IfxStpMap() {
    this.routeManager = new IfxRouteManager();
    this.advancedEditRouteDetails = {
        pathIndex: -1,
        startSegmentIndex: -1,
        endSegmentIndex: -1,
        terminalPoint: -1
    };
}
IfxStpMap.prototype.setMapURL = function () {
    this.geoserverUrl = objifxStpMap.configGeoserverUrl;
    this.geoserverWfsUrl = objifxStpMap.configGeoserverWfsUrl;
    this.geoserverWmsUrl = objifxStpMap.configGeoserverWmsUrl;
    this.backgroundLayers = "ESDAL4:MapSpatial";
    this.LayerRoadName = "ESDAL4:STREETS";
    this.requestPart1 = "?request=GetFeature&typeName=ESDAL4:STREETS&propertyName=LINK_ID,ST_NAME,REF_IN_ID&outputFormat=application/json&version=1.0.0&CQL_FILTER=";
    this.imageFormat = "image/png8";
    this.defaultZoom = { X: 525168, Y: 170100, zoom: 1 };
}

//IfxStpMap.prototype.defaultZoom = { X: 636236.5482493, Y: 165233.2980473, zoom: 9 };

//variables here
//this parameter indicates the origin.  i.e. the caller of the function
//page 0: A2B, 1: route library, 2: sketched/outline route, 
//3: agreed route, 4: structures, 5: so(outline) creation 
//6: vr1 creation 7: so display 8: vr1 display

IfxStpMap.prototype.pageType = 0;

//pageloading, readyidle, planninginprogress

//firstpointselected, secondpointselected,
//routeplanninginprogress,routeplanned.

IfxStpMap.prototype.pageState = 'readyidle';
//openlayers map object
IfxStpMap.prototype.olMap = null;
IfxStpMap.prototype.addReturnRoute = false;
//vectors layers
IfxStpMap.prototype.vectorLayerMarkers = null;
IfxStpMap.prototype.vectorLayerRoute = null;
IfxStpMap.prototype.vectorLayerOffRoad = null;
IfxStpMap.prototype.vectorLayerSketcheaoute = null;
IfxStpMap.prototype.vectorLayerStructures = null;
IfxStpMap.prototype.vectorLayerStructuresLine = null;
IfxStpMap.prototype.vectorLayerConstraints = null;
IfxStpMap.prototype.vectorLayerDelegRoads = null;

IfxStpMap.prototype.routeManager = null;
IfxStpMap.prototype.returnLegRoute = null;
IfxStpMap.prototype.tempRoutePart = null;

IfxStpMap.prototype.currentActiveRoutePathIndex = 0;
IfxStpMap.prototype.dragMarkerDiff = { X: '', Y: '' }
IfxStpMap.prototype.eventList = {};
IfxStpMap.prototype.bounds = { left: '', bottom: '', right: '', top: '' };

IfxStpMap.prototype.previousActivity = '';

IfxStpMap.prototype.boundaryOffset = 500;

IfxStpMap.prototype.dragActivity = { dragActive: false, panned: false };

IfxStpMap.prototype.mousePosition = { X: '', Y: '' };

IfxStpMap.prototype.currentMouseOverFeature = null;

IfxStpMap.prototype.loadMap = function (pageType, routePart, routePlanUnit, geoRegion) {
    if (pageType == 'NOMAPDISPLAY') {
        this.pageState = 'readyidle';//page is ready
        this.pageType = pageType;
        return;
    }
    this.pageState = 'pageloading';
    this.pageType = pageType;
    this.setMapURL();
    this.createMapObject();
    this.addLayers();
    this.addScaleBar(routePlanUnit);
    this.createVectorLayerObjects();
    //this.createCacheReadWriteControls();
    this.createControls();
    if (pageType != 'DISPLAYONLY' && pageType != 'SORTMAPFILTER_VIEWANDEDIT' && pageType != 'DISPLAYONLY_EDITANNOTATION' && pageType != 'DELEGATION_VIEWONLY' && pageType != 'DELEGATION_VIEWANDEDIT' && pageType != 'ROADOWNERSHIP_VIEWONLY' && pageType != 'ROADOWNERSHIP_VIEWANDEDIT') {
        this.createOffRoadLineControl();
        //this.createDragRouteControl();
        this.createToolbar();
        this.createDragFeature();
        this.createClickControl();
    }
    this.createRouteAppraisalToolbar();
    this.createadv_rtplanCancelToolbar();
    this.createManoeuvreCancelToolbar();

    if (pageType == 'DELEGATION_VIEWANDEDIT' || pageType == 'ROADOWNERSHIP_VIEWANDEDIT' || pageType == 'SORTMAPFILTER_VIEWANDEDIT') {
        this.createClickControl();
        if (pageType == 'DELEGATION_VIEWANDEDIT') {
            objifxStpMapRoadDelegation.createPolygonDrawControl();
            objifxStpMapRoadDelegation.createRoadDelegationToolBar();
        }
        else if (pageType == 'SORTMAPFILTER_VIEWANDEDIT') {
            objifxStpMapRoadDelegation.createPolygonDrawControl();
            objifxStpMapRoadDelegation.createMapfilterToolBar();
        }
        else {
            objifxStpmapRoadOwnership.createPolygonDrawControl();
            objifxStpmapRoadOwnership.createRoadDelegationToolBar();
        }

    }
    this.olMap.updateSize();
    if (pageType == "STRUCTURES") {
        objifxStpmapStructures.createConstraintDrawControl();
        //objifxStpmapStructures.createConstraintToolBar();
        objifxStpmapStructures.constraintbyDescrToolbar();
    }
    if (routePart == undefined || routePart == null || routePart.routePathList[0].routePointList.length == 0) {
        if (geoRegion != undefined && geoRegion != null) {
            var bounds = new OpenLayers.Bounds();
            bounds.extend(new OpenLayers.LonLat(geoRegion.x1, geoRegion.y1));
            bounds.extend(new OpenLayers.LonLat(geoRegion.x2, geoRegion.y2));
            this.olMap.zoomToExtent(bounds, true);
        }
        else if (pageType == 'SORTMAPFILTER_VIEWANDEDIT' && mapSearcheasting != 0 && mapSearchnorthing != 0) {
            this.olMap.setCenter([mapSearcheasting, mapSearchnorthing], 9);
        }
        else if (pageType == 'SORTMAPFILTER_VIEWANDEDIT' && mapSearcheasting == 0 && mapSearchnorthing == 0) {
            this.olMap.setCenter([this.defaultZoom.X, this.defaultZoom.Y], 7);
        }
        else {
            this.olMap.setCenter([this.defaultZoom.X, this.defaultZoom.Y], this.defaultZoom.zoom);
        }
    }
    else
        this.setRoutePart(routePart, null);
    this.pageState = 'readyidle';//page is ready
}

IfxStpMap.prototype.zoomIn = function () {
    this.olMap.zoomIn();
}

IfxStpMap.prototype.zoomOut = function () {
    this.olMap.zoomOut();
}

IfxStpMap.prototype.setCurrentActiveRoutePath = function (routePathIndex) {
    this.currentActiveRoutePathIndex = routePathIndex;
}

IfxStpMap.prototype.setCenter = function (position, bLonLat) {
    var lonlat;
    if (bLonLat) {
        lonlat = new OpenLayers.LonLat(position.x, position.y);
    }
    else {
        lonlat = this.olMap.getLonLatFromPixel(position);
    }
    this.olMap.setCenter(lonlat);
}

IfxStpMap.prototype.setCenterAndZoom = function (position, bLonLat) {
    this.setCenter(position, bLonLat);
    this.zoomIn();
}

IfxStpMap.prototype.setZoomTo = function (X, Y, zoom) {
    var x = parseInt(X);
    var y = parseInt(Y);
    this.olMap.setCenter([x, y], zoom);
}

IfxStpMap.prototype.registerEvent = function (eventName, eventCallback) {
    this.eventList[eventName] = eventCallback;
}

IfxStpMap.prototype.createMapObject = function () {
    var bounds = new OpenLayers.Bounds(
        -372478.393235993, -204735.427371373,
        848548.217114138, 1487331.39223813
    );

    var options = {
        resolutions: [1228.8, 614.4, 307.2, 153.6, 76.8, 38.4, 19.2, 9.6, 4.8, 2.4, 1.2, 0.6, 0.3],
        //controls: [new OpenLayers.Control.PanZoomBar()],
        maxExtent: bounds,
        maxResolution: 1228.8,
        projection: "EPSG:27700",
        units: 'm'
    };
    this.olMap = new OpenLayers.Map('map', options);

    //this.olMap.events.register("zoomend", this, this.zoomChanged);
    //this.olMap.events.register("loadend", this, this.loadEnd);
    this.olMap.events.register("moveend", this, this.panChanged);
    this.olMap.events.register("mousemove", this, this.mouseMoved);
}

IfxStpMap.prototype.deleteMapLayer = function (name) {
    for (var i = 0; i < this.olMap.layers.length; i++) {
        if (this.olMap.layers[i].name == name) {
            this.olMap.removeLayer(this.olMap.layers[i]);
            break;
        }
    }
}

IfxStpMap.prototype.zoomChanged = function () {
    this.eventList['ZOOMCHANGED']();
}

IfxStpMap.prototype.loadEnd = function () {
    this.eventList['LOADMAPEND']();
}

IfxStpMap.prototype.panChanged = function () {
    this.eventList['PANCHANGED']();
}

IfxStpMap.prototype.mouseMoved = function (e) {
    this.mousePosition = { X: e.xy.x, Y: e.xy.y };
}

IfxStpMap.prototype.addLayers = function () {
    var layerBackground = new OpenLayers.Layer.WMS(
        "Geoserver layers", this.geoserverUrl,
        {
            layers: this.backgroundLayers,
            format: this.imageFormat,
            tiled: false,
            tilesorigin: [this.olMap.maxExtent.left, this.olMap.maxExtent.bottom]
        },
        { attribution: "Contains Ordnance Survey data &copy Crown copyright and database right 2021<br>Contains Royal Mail data &copy Royal Mail copyright and database right 2021" },
        { buffer: 0 }
    );

    this.olMap.addLayer(layerBackground);
}

IfxStpMap.prototype.addScaleBar = function (routePlanUnit) {
    var scalebar = new OpenLayers.Control.ScaleBar();
    if (routePlanUnit == 'imperial') {
        scalebar.displaySystem = 'english';
    }
    this.olMap.addControl(scalebar);
}

IfxStpMap.prototype.createVectorLayerObjects = function () {
    var self = this;
    var context = {
        getColor: function (feature) {
            return IfxStpmapCommon.getColor(feature);
        }
    };
    var template = {
        strokeColor: "${getColor}", // using context.getColor(feature)
        strokeOpacity: 0.7,
        strokeWidth: 3
    };
    var style = new OpenLayers.Style(template, { context: context });

    this.vectorLayerRoute = new OpenLayers.Layer.Vector("Route", {
        styleMap: new OpenLayers.StyleMap(style)
    });

    this.vectorLayerMarkers = new OpenLayers.Layer.Vector("Markers", {
        eventListeners: {
            featureover: function (e) {
                if (self.eventList['FEATUREOVER'] != undefined && typeof (self.eventList['FEATUREOVER']) === "function") {
                    self.eventList['FEATUREOVER'](e, 'MARKER');
                }
            },
            featureout: function (e) {
                if (self.eventList['FEATUREOUT'] != undefined && typeof (self.eventList['FEATUREOUT']) === "function") {
                    self.eventList['FEATUREOUT'](e, 'MARKER');
                }
            }
        }
    });
    var styleMap = new OpenLayers.StyleMap(OpenLayers.Util.applyDefaults(
        { strokeOpacity: 0.7, strokeColor: "#8000ff", strokeWidth: 3 },
        OpenLayers.Feature.Vector.style["default"]));

    this.vectorLayerSketchedRoute = new OpenLayers.Layer.Vector("SketchedRoute", {
        styleMap: styleMap
    });

    var styleMapOffRoad = new OpenLayers.StyleMap(OpenLayers.Util.applyDefaults(
        { strokeOpacity: 0.7, strokeColor: "#6FB9C5", strokeWidth: 3 },
        OpenLayers.Feature.Vector.style["default"]));

    this.vectorLayerOffRoad = new OpenLayers.Layer.Vector("OffRoadRoute", {
        styleMap: styleMapOffRoad
    });

    var styleMapDragRouteMarker = new OpenLayers.StyleMap(OpenLayers.Util.applyDefaults({
        externalGraphic: '/Content/Images/alt_startmarker.svg',
        graphicHeight: 12, graphicWidth: 12, graphicXOffset: -6, graphicYOffset: -5
    }));

    this.vectorLayerDragRouteMarker = new OpenLayers.Layer.Vector("DragRouteMarker", {
        styleMap: styleMapDragRouteMarker
    });

    this.vectorLayerStructures = new OpenLayers.Layer.Vector("Structures", {
        eventListeners: {
            featureover: function (e) {
                if (self.eventList['FEATUREOVER'] != undefined && typeof (self.eventList['FEATUREOVER']) === "function") {
                    self.eventList['FEATUREOVER'](e, 'STRUCTURE');
                }
            },
            featureout: function (e) {
                if (self.eventList['FEATUREOUT'] != undefined && typeof (self.eventList['FEATUREOUT']) === "function") {
                    self.eventList['FEATUREOUT'](e, 'STRUCTURE');
                }
            }
        }
    });

    this.vectorLayerConstraints = new OpenLayers.Layer.Vector("Constraints", {
        eventListeners: {
            featureover: function (e) {
                if (self.eventList['FEATUREOVER'] != undefined && typeof (self.eventList['FEATUREOVER']) === "function") {
                    self.eventList['FEATUREOVER'](e, 'CONSTRAINT');
                }
            },
            featureout: function (e) {
                if (self.eventList['FEATUREOUT'] != undefined && typeof (self.eventList['FEATUREOUT']) === "function") {
                    self.eventList['FEATUREOUT'](e, 'CONSTRAINT');
                }
            }
        }
    });

    /*var styleDelegRoad = new OpenLayers.StyleMap(OpenLayers.Util.applyDefaults(
                        { strokeOpacity: 0.7, strokeColor: "#1919FF", strokeWidth: 3 },
                        OpenLayers.Feature.Vector.style["default"]));

    this.vectorLayerDelegRoads = new OpenLayers.Layer.Vector("DelegRoads", {
        styleMap: styleDelegRoad
    });*/

    var contextDeleg = {
        getColorDeleg: function (feature) {
            return IfxStpmapCommon.getDelegRoadColor(feature);
        }
    };
    var templateDeleg = {
        strokeColor: "${getColorDeleg}", // using context.getColor(feature)
        strokeOpacity: 0.7,
        strokeWidth: 3
    };
    var styleDeleg = new OpenLayers.Style(templateDeleg, { context: contextDeleg });

    this.vectorLayerDelegRoads = new OpenLayers.Layer.Vector("DelegRoads", {
        styleMap: new OpenLayers.StyleMap(styleDeleg)
    });

    this.vectorLayerDelegMarkers = new OpenLayers.Layer.Vector("DelegMarkers");

    //this.olMap.addLayers([this.vectorLayerConstraints, this.vectorLayerStructures, this.vectorLayerRoute, this.vectorLayerOffRoad, this.vectorLayerMarkers]);
    this.addVectorLayers();
}

IfxStpMap.prototype.addVectorLayers = function () {
    if (this.pageType == 'NOMAPDISPLAY');
    else if (this.pageType == 'STRUCTURES')
        this.olMap.addLayers([this.vectorLayerConstraints, this.vectorLayerStructures]);
    else
        this.olMap.addLayers([this.vectorLayerMarkers, this.vectorLayerRoute, this.vectorLayerOffRoad, this.vectorLayerDragRouteMarker]);
}

IfxStpMap.prototype.createControls = function () {
    this.olMap.addControl(new OpenLayers.Control.Attribution());

    if (this.pageType === "A2BPLANNING" ||
        this.pageType === "ROUTELIBRARY" ||
        this.pageType === "STRUCTURES" ||
        this.pageType === "SOAPP" ||
        this.pageType === "DISPLAYONLY" ||
        this.pageType === "DISPLAYONLY_EDITANNOTATION" ||
        this.pageType === "VR1APP" ||
        this.pageType === "DELEGATION_VIEWONLY" ||
        this.pageType === "DELEGATION_VIEWANDEDIT" ||
        this.pageType == "ROADOWNERSHIP_VIEWONLY" ||
        this.pageType == "ROADOWNERSHIP_VIEWANDEDIT" ||
        this.pageType == "SORTMAPFILTER_VIEWANDEDIT" ||
        this.pageType == "SOA2BPLANNING") {
        this.olMap.addControl(new OpenLayers.Control.Navigation());
    }

    var zoomStatus = new OpenLayers.Control.ZoomStatus({
        autoActivate: true
    });
    this.olMap.addControl(zoomStatus);

    var keyboardControl = new OpenLayers.Control.KeyboardDefaults({
        autoActivate: true
    });
    this.olMap.addControl(keyboardControl);

    var snap = new OpenLayers.Control.Snapping({ layer: this.vectorLayerOffRoad, targets: [{ layer: this.vectorLayerRoute }] });
    this.olMap.addControl(snap);
    snap.activate();

    var snapDragRoute = new OpenLayers.Control.Snapping({ layer: this.vectorLayerDragRouteMarker, targets: [{ layer: this.vectorLayerRoute }] });
    this.olMap.addControl(snapDragRoute);
    snapDragRoute.activate();
}

IfxStpMap.prototype.createToolbar = function () {
    var self = this;

    toolbarPanel = new OpenLayers.Control.Panel({ displayClass: 'maptoolbarpanel horizontalMap-center' });  //ESDAL4 2022 New displaynone class added

    this.olMap.addControl(toolbarPanel);

    btnAlternateStartRoute = new OpenLayers.Control.Button({
        displayClass: 'alternateStart',
        title: 'Alternate start point',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {
            if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                self.eventList['DEACTIVATECONTROL'](0, "A2BPLANNING");
            }
            self.addRoutePath(1);
            document.getElementById("startflag").style.display = "none";
            document.getElementById("startflagalt").style.display = "block";
            document.getElementById("endflag").style.display = "block";
            document.getElementById("endflagalt").style.display = "none";
        }
    });

    btnAlternateMiddleRoute = new OpenLayers.Control.Button({
        displayClass: 'alternateMiddle',
        title: 'Alternate middle point',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {
            if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                self.eventList['DEACTIVATECONTROL'](1, "A2BPLANNING");
            }
            self.addRoutePath(2);
        }
    });

    btnAlternateEndRoute = new OpenLayers.Control.Button({
        displayClass: 'alternateEnd',
        title: 'Alternate end point',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {
            if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                self.eventList['DEACTIVATECONTROL'](2, "A2BPLANNING");
            }
            self.addRoutePath(3);
            document.getElementById("endflag").style.display = "none";
            document.getElementById("endflagalt").style.display = "block";
            document.getElementById("startflag").style.display = "block";
            document.getElementById("startflagalt").style.display = "none";
        }
    });

    btnOffRoadRoute = new OpenLayers.Control.Button({
        displayClass: 'offRoad',
        title: 'Off-road route',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                    self.eventList['DEACTIVATECONTROL'](3, "A2BPLANNING");
                }
                offRoadLineControl.activate();
            },
            'deactivate': function () {
                offRoadLineControl.deactivate();
            }
        }
    });

    btnReverseMnvr = new OpenLayers.Control.Button({
        displayClass: 'reverseMnvr',
        title: 'Reverse manoeuvre',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                    self.eventList['DEACTIVATECONTROL'](4, "A2BPLANNING");
                }
            },
            'deactivate': function () {
                self.updateManoeuvreRouteSegment(self.currentActiveRoutePathIndex);
            }
        }
    });

    btnUturnMnvr = new OpenLayers.Control.Button({
        displayClass: 'uturnMnvr',
        title: 'U-turn manoeuvre',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                    self.eventList['DEACTIVATECONTROL'](5, "A2BPLANNING");
                }
            },
            'deactivate': function () {
                self.updateManoeuvreRouteSegment(self.currentActiveRoutePathIndex);
            }
        }
    });

    btnConfirmedMnvr = new OpenLayers.Control.Button({
        displayClass: 'confirmedMnvr',
        title: 'Breaking the rules of the road network',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                    self.eventList['DEACTIVATECONTROL'](6, "A2BPLANNING");
                }
            },
            'deactivate': function () {
                self.updateManoeuvreRouteSegment(self.currentActiveRoutePathIndex);
            }
        }
    });

    btnCutRoute = new OpenLayers.Control.Button({
        displayClass: 'cutRoute',
        title: 'Cut route',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                    self.eventList['DEACTIVATECONTROL'](7, "A2BPLANNING");
                }
            },
            'deactivate': function () {
                toolbarPanel4.div.style.display = "none";
            }
        }
    });

    //btnDragRoute = new OpenLayers.Control.Button({
    //    displayClass: 'dragRoute',
    //    title: 'Drag route',
    //    type: OpenLayers.Control.TYPE_TOGGLE,
    //    eventListeners: {
    //        'activate': function () {
    //            if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
    //                self.eventList['DEACTIVATECONTROL'](8, "A2BPLANNING");
    //            }

    //            dragRouteControl.activate();
    //        },
    //        'deactivate': function () {
    //            dragRouteControl.deactivate();
    //        }
    //    }
    //});

    btnAdvancedEdit = new OpenLayers.Control.Button({
        displayClass: 'advancedEdit',
        title: 'Advanced edit',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {
            if (self.eventList['DEACTIVATECONTROL'] != undefined && typeof (self.eventList['DEACTIVATECONTROL']) === "function") {
                self.eventList['DEACTIVATECONTROL'](9, "A2BPLANNING");
            }
            self.addRoutePath(4);
        }
    });

    if (this.pageType == "SOA2BPLANNING") {
        if ($('#PortalType').val() == 696008)
            toolbarPanel.addControls([btnAlternateStartRoute, btnAlternateMiddleRoute, btnAlternateEndRoute, btnOffRoadRoute, btnReverseMnvr, btnUturnMnvr, btnConfirmedMnvr, btnCutRoute,  btnAdvancedEdit]);
        else
            toolbarPanel.addControls([btnAlternateStartRoute, btnAlternateMiddleRoute, btnAlternateEndRoute, btnOffRoadRoute, btnReverseMnvr, btnUturnMnvr, btnConfirmedMnvr, btnCutRoute ]);
    }
    else if (this.pageType == "A2BPLANNING" && ($('#PortalType').val() == 696008)) {
        toolbarPanel.addControls([btnAlternateStartRoute, btnAlternateMiddleRoute, btnAlternateEndRoute, btnOffRoadRoute, btnReverseMnvr, btnUturnMnvr, btnConfirmedMnvr, btnCutRoute, btnAdvancedEdit]);
    }
    else {
        toolbarPanel.addControls([btnAlternateStartRoute, btnAlternateMiddleRoute, btnAlternateEndRoute, btnOffRoadRoute, btnReverseMnvr, btnUturnMnvr, btnConfirmedMnvr, btnCutRoute]);
    }
    toolbarPanel.div.style.display = "none";
}

IfxStpMap.prototype.createRouteAppraisalToolbar = function () {
    var self = this;
    toolbarPanel2 = new OpenLayers.Control.Panel({ displayClass: 'RouteAppraisalToolbar horizontalMap-center  alignCentreClass' });  //ESDAL4 2022 New displaynone class added
    this.olMap.addControl(toolbarPanel2);

    btnAffectedStructures = new OpenLayers.Control.Button({
        displayClass: 'affectedStructures',
        title: 'Affected Structures',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['TOGGLESTRUCTURES'] != undefined && typeof (self.eventList['TOGGLESTRUCTURES']) === "function") {
                    self.eventList['TOGGLESTRUCTURES']('STRUCTURES', true);
                }
            },
            'deactivate': function () {
                if (self.eventList['TOGGLESTRUCTURES'] != undefined && typeof (self.eventList['TOGGLESTRUCTURES']) === "function") {
                    self.eventList['TOGGLESTRUCTURES']('STRUCTURES', false);
                }
            }
        }
    });
    btnAffectedConstraints = new OpenLayers.Control.Button({
        displayClass: 'affectedConstraints',
        title: 'Affected Constraints',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                if (self.eventList['TOGGLESTRUCTURES'] != undefined && typeof (self.eventList['TOGGLESTRUCTURES']) === "function") {
                    self.eventList['TOGGLESTRUCTURES']('CONSTRAINTS', true);
                }
            },
            'deactivate': function () {
                if (self.eventList['TOGGLESTRUCTURES'] != undefined && typeof (self.eventList['TOGGLESTRUCTURES']) === "function") {
                    self.eventList['TOGGLESTRUCTURES']('CONSTRAINTS', false);
                }
            }
        }
    });
    if (this.pageType == "DISPLAYONLY") {
        toolbarPanel2.addControls([btnAffectedStructures, btnAffectedConstraints]);
        toolbarPanel2.div.style.left = "620px";
    }
    if (this.pageType == "SOA2BPLANNING") {
        if ($('#PortalType').val() == 696008 && $('#SortStatus').val() == "CandidateRT")
            toolbarPanel2.addControls([btnAffectedStructures, btnAffectedConstraints]);
        //toolbarPanel2.div.style.left = "291px";
        toolbarPanel2.div.style.display = "none";
    }
    else {
        toolbarPanel2.addControls([btnAffectedStructures, btnAffectedConstraints]);
        //toolbarPanel2.div.style.left = "291px";
        toolbarPanel2.div.style.display = "none";
    }
}

IfxStpMap.prototype.createManoeuvreCancelToolbar = function () {
    var self = this;
    toolbarPanel4 = new OpenLayers.Control.Panel({ displayClass: 'MnvrCancelToolbar horizontalMap-center' });
    this.olMap.addControl(toolbarPanel4);

    btnManoeuvreCancel = new OpenLayers.Control.Button({
        displayClass: 'manoeuvreCancelToolbar',
        title: 'Cancel',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {
            $('#intellizenz-ctxmenu').remove();
            var rPath = self.routeManager.getRoutePath(self.currentActiveRoutePathIndex);
            if (self.previousActivity == 'ROUTEPATHEXTENDEDATSTART')
                var segmentIndex = 0;
            else
                var segmentIndex = rPath.routeSegmentList.length - 1;

            self.deleteAnnotation(self.currentActiveRoutePathIndex, segmentIndex, 0);
            self.removeRouteSegment(self.currentActiveRoutePathIndex, segmentIndex);
            toolbarPanel4.div.style.display = "none";

            for (var i = 4; i < 7; i++) {
                toolbarPanel.controls[i].deactivate();
            }

            if (self.previousActivity == 'ROUTEPATHEXTENDEDATSTART') {
                self.shortenRoute('STARTPOINT');
                self.previousActivity == '';
            }
            else if (self.previousActivity == 'ROUTEPATHEXTENDEDATEND') {
                self.shortenRoute('ENDPOINT');
                self.previousActivity == '';
            }
        }
    });

    toolbarPanel4.addControls([btnManoeuvreCancel]);
    toolbarPanel4.div.style.display = "none";
}

IfxStpMap.prototype.createadv_rtplanCancelToolbar = function () {
    var self = this;
    toolbarPanel3 = new OpenLayers.Control.Panel({ displayClass: 'adv_rtplanCancelToolbar horizontalMap-center' });
    this.olMap.addControl(toolbarPanel3);

    btnAdv_routeplanCancel = new OpenLayers.Control.Button({
        displayClass: 'adv_routeplanCancel',
        title: 'Cancel',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {
            if (self.eventList['ADVRPCANCEL'] != undefined && typeof (self.eventList['ADVRPCANCEL']) === "function") {
                self.eventList['ADVRPCANCEL']();
            }
        }
    });
    toolbarPanel3.addControls([btnAdv_routeplanCancel]);
    toolbarPanel3.div.style.display = "none";
}



IfxStpMap.prototype.createOffRoadLineControl = function () {
    var self = this;
    offRoadLineControl = new OpenLayers.Control.DrawFeature(this.vectorLayerOffRoad, OpenLayers.Handler.Path,
        {
            title: 'Plan off-road',
            'featureAdded': function (obj) {
                $('#intellizenz-ctxmenu').remove();
                self.createOffRoadSegment(obj);
                btnOffRoadRoute.deactivate();
                var pix = self.olMap.getPixelFromLonLat(new OpenLayers.LonLat(obj.geometry.components[0].x, obj.geometry.components[0].y));
                self.eventList['MANOEUVREADDED'](pix, 'offroad');
            }
        });

    this.olMap.addControl(offRoadLineControl);
}

IfxStpMap.prototype.createDragRouteControl = function () {
    var self = this;
    dragRouteControl = new OpenLayers.Control.DrawFeature(this.vectorLayerDragRouteMarker, OpenLayers.Handler.Point,
        {
            title: 'Drag route',
            'featureAdded': function (obj) {
                btnDragRoute.deactivate();
                var pix = self.olMap.getPixelFromLonLat(new OpenLayers.LonLat(obj.geometry.x, obj.geometry.y));
                self.eventList['DRAGPOINTADDED'](pix);
            }
        });

    this.olMap.addControl(dragRouteControl);
}

IfxStpMap.prototype.createOffRoadSegment = function (obj) {
    var ordinateArray = [];
    for (var i = 0, j = 0; i < obj.geometry.components.length; i++) {
        ordinateArray[j++] = obj.geometry.components[i].x;
        ordinateArray[j++] = obj.geometry.components[i].y;
    }

    var rPath = this.routeManager.getRoutePath(this.currentActiveRoutePathIndex);
    var rSegment = this.routeManager.createRouteSegmentObject(null, null, null, ordinateArray, rPath.routeSegmentList.length + 1, 'OFFROAD');
    var pix = this.olMap.getPixelFromLonLat(new OpenLayers.LonLat(obj.geometry.components[0].x, obj.geometry.components[0].y));
    var retObject = this.getNearestPathAndSegment(pix.x, pix.y, true);
    rSegment.startLinkId = retObject.feature != '' ? retObject.feature.attributes.LINK_ID : null;
    pix = this.olMap.getPixelFromLonLat(new OpenLayers.LonLat(obj.geometry.components[obj.geometry.components.length - 1].x, obj.geometry.components[obj.geometry.components.length - 1].y));
    retObject = this.getNearestPathAndSegment(pix.x, pix.y, true);
    rSegment.endLinkId = retObject.feature != '' ? retObject.feature.attributes.LINK_ID : null;
    rSegment.otherinfo.completefeatures = [this.vectorLayerOffRoad.features[this.vectorLayerOffRoad.features.length - 1]];
    rPath.routeSegmentList.push(rSegment);

    var extend = this.checkOffRoadExtend(rPath, obj.geometry);
    if (extend.extended == true) {
        this.moveMarkerToExtendedRoad(extend.index, extend.pointType, extend.geometry.x, extend.geometry.y);
        this.routeManager.reArrangeRouteSegmentList(this.currentActiveRoutePathIndex);
        if (extend.pointType == 'STARTPOINT')
            this.previousActivity = 'ROUTEPATHEXTENDEDATSTART';
        else
            this.previousActivity = 'ROUTEPATHEXTENDEDATEND';
    }
    else {
        var bool = this.checkOffRoadInCurrentActivePath(rPath, obj.geometry);
        if (!bool.bool) {
            if (bool.path != -1) {
                //var pathNo = parseInt(bool.path) + 1;
                //showNotification("Trying to create an Off-road on conflicting route path, please select 'Path" + pathNo + "' from the left panel selection and continue...");
                showNotification("Since this route includes alternate route paths, please select the relevant route path by selecting the related Path number from the left panel before creating the off-road route segment.");
            }
            this.removeRouteSegment(this.currentActiveRoutePathIndex, rPath.routeSegmentList.length - 1);
            return;
        }
        else {
            if (bool.distance >= 1) {
                showNotification("Trying to create an Off-road which is not continuous with the route.");
                this.removeRouteSegment(this.currentActiveRoutePathIndex, rPath.routeSegmentList.length - 1);
                return;
            }
        }
    }

    this.routeManager.setRoutePathState(this.currentActiveRoutePathIndex, 'routeplanned');
    if (this.eventList['OFFROADADDED'] != undefined && typeof (this.eventList['OFFROADADDED']) === "function") {
        this.eventList['OFFROADADDED']();
    }
}

IfxStpMap.prototype.checkOffRoadExtend = function (rPath, offRoadGeometry) {
    if (rPath == null || rPath == undefined) {
        rPath = this.routeManager.getRoutePath(this.currentActiveRoutePathIndex);
    }
    var points = this.routeManager.findStartAndEndPointsFromRoutePointList(this.currentActiveRoutePathIndex);
    var retObj = { extended: false, index: -1, pointType: '', geometry: null };

    if (IfxStpmapCommon.compareGeometries(offRoadGeometry.components[0], rPath.routePointList[points.startIndex].pointGeom.sdo_point)) {
        retObj = { extended: true, index: points.startIndex, pointType: 'STARTPOINT', geometry: offRoadGeometry.components[offRoadGeometry.components.length - 1] };
    }
    else if (IfxStpmapCommon.compareGeometries(offRoadGeometry.components[offRoadGeometry.components.length - 1], rPath.routePointList[points.startIndex].pointGeom.sdo_point)) {
        retObj = { extended: true, index: points.startIndex, pointType: 'STARTPOINT', geometry: offRoadGeometry.components[0] };
    }
    else if (IfxStpmapCommon.compareGeometries(offRoadGeometry.components[0], rPath.routePointList[points.endIndex].pointGeom.sdo_point)) {
        retObj = { extended: true, index: points.endIndex, pointType: 'ENDPOINT', geometry: offRoadGeometry.components[offRoadGeometry.components.length - 1] };
    }
    else if (IfxStpmapCommon.compareGeometries(offRoadGeometry.components[offRoadGeometry.components.length - 1], rPath.routePointList[points.endIndex].pointGeom.sdo_point)) {
        retObj = { extended: true, index: points.endIndex, pointType: 'ENDPOINT', geometry: offRoadGeometry.components[0] };
    }

    if (retObj.extended) {
        var pix = this.olMap.getPixelFromLonLat(new OpenLayers.LonLat(retObj.geometry.x, retObj.geometry.y));
        var nearestPath = this.getNearestPathAndSegment(pix.x, pix.y, false);
        if (nearestPath.pathIndex == this.currentActiveRoutePathIndex && nearestPath.distance < 1)
            retObj.extended = false;
    }
    return retObj;
}

IfxStpMap.prototype.checkOffRoadInCurrentActivePath = function (rPath, offRoadGeometry) {
    var retObj = { bool: false, path: -1, distance: 2500 };
    var nearestPath = this.getNearestPathAndSegment(offRoadGeometry.components[0].x, offRoadGeometry.components[0].y, false, true);
    if (nearestPath.pathIndex == this.currentActiveRoutePathIndex) {
        retObj = { bool: true, path: nearestPath.pathIndex, distance: nearestPath.distance };
    }
    else {
        retObj = { bool: false, path: nearestPath.pathIndex, distance: nearestPath.distance };
    }
    nearestPath = this.getNearestPathAndSegment(offRoadGeometry.components[offRoadGeometry.components.length - 1].x, offRoadGeometry.components[offRoadGeometry.components.length - 1].y, false, true);
    if (retObj.bool == false) {
        if (nearestPath.pathIndex == this.currentActiveRoutePathIndex) {
            retObj = { bool: true, path: nearestPath.pathIndex, distance: nearestPath.distance };
        }
        else {
            retObj = { bool: false, path: nearestPath.pathIndex, distance: nearestPath.distance };
        }
    }
    else if (retObj.distance > nearestPath.distance) {
        retObj = { bool: true, path: nearestPath.pathIndex, distance: nearestPath.distance };
    }
    return retObj;
}

IfxStpMap.prototype.shortenRoute = function (pointType) {
    var rPath = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex];
    var points = this.routeManager.findStartAndEndPointsFromRoutePointList(this.currentActiveRoutePathIndex);
    if (pointType == 'STARTPOINT') {
        var geometry = rPath.routeSegmentList[0].startPointGeometry;
        this.moveMarkerToExtendedRoad(points.startIndex, pointType, geometry.sdo_point.X, geometry.sdo_point.Y);
    }
    else {
        var geometry = rPath.routeSegmentList[rPath.routeSegmentList.length - 1].endPointGeometry;
        this.moveMarkerToExtendedRoad(points.endIndex, pointType, geometry.sdo_point.X, geometry.sdo_point.Y);
    }
}


IfxStpMap.prototype.moveMarkerToExtendedRoad = function (index, pointType, x, y) {
    var rPath = this.routeManager.getRoutePath(this.currentActiveRoutePathIndex);
    var rPoint = rPath.routePointList[index];
    rPoint.pointGeom.sdo_point = { X: x, Y: y };
    this.deleteRoutePoint(index);
    var marker = this.setMarker(pointType, x, y, 0);
    rPoint['otherinfo']['marker'] = marker;
    this.routeManager.addRoutePoint(this.currentActiveRoutePathIndex, index, rPoint, false);
}

IfxStpMap.prototype.resizeMap = function (width, height) {
    if (this.olMap == null)
        return;
    var mapDiv = document.getElementById('map');
    mapDiv.style.width = width;
    mapDiv.style.height = height;
    this.olMap.updateSize();
}

IfxStpMap.prototype.updateMapSize = function () {
    this.olMap.updateSize();
}

IfxStpMap.prototype.clearMouseCache = function () {
    this.olMap.events.clearMouseCache();
}

IfxStpMap.prototype.onDragStartCallback = function (feature, pixel) {
    var pixel1 = this.olMap.getPixelFromLonLat(new OpenLayers.LonLat(feature.geometry.x, feature.geometry.y));
    this.dragMarkerDiff['X'] = pixel1.x - pixel.x;
    this.dragMarkerDiff['Y'] = pixel1.y - pixel.y;
    return true;
}

IfxStpMap.prototype.undoMarkerDrag = function (marker, pointAttrib) {
    if (pointAttrib[0] == "ROUTEDRAG") {
        this.vectorLayerDragRouteMarker.removeFeatures(marker);
        var pointIndex = pointAttrib[2] > 0 && this.routeManager.getAllRoutePointCount(this.currentActiveRoutePathIndex) < 2 ? 0 : pointAttrib[2];
        var rpPoint = this.routeManager.getAllRoutePoint(pointAttrib[1], pointIndex);
    }
    else {
        this.vectorLayerMarkers.removeFeatures(marker);
        var pointIndex = pointAttrib[2] > 0 && this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) < 2 ? 0 : pointAttrib[2];
        var rpPoint = this.routeManager.getRoutePoint(pointAttrib[1], pointIndex);
    }

    marker = this.setMarker(pointAttrib[0],
        rpPoint.pointGeom.sdo_point.X, rpPoint.pointGeom.sdo_point.Y,
        pointIndex);

    rpPoint['otherinfo']['marker'] = marker;
}

IfxStpMap.prototype.onDragCompleteCallback = function (feature, pixel, isDragMarkerLayer) {
    if (isDragMarkerLayer == undefined || isDragMarkerLayer == null || isDragMarkerLayer == false) {
        var marker = this.vectorLayerMarkers.getFeaturesByAttribute('name', feature.data.name);
    }
    else {
        var marker = this.vectorLayerDragRouteMarker.getFeaturesByAttribute('name', feature.data.name);
    }
    var self = this;
    var pointAttrib = feature.data.name.split(" ");
    var X = (this.dragActivity.dragActive == true && this.dragActivity.panned == true) ? pixel.x : (pixel.x + this.dragMarkerDiff.X);
    var Y = (this.dragActivity.dragActive == true && this.dragActivity.panned == true) ? pixel.y : (pixel.y + this.dragMarkerDiff.Y);
    var pathType = this.getcurrentSelectedRouteType();
    if (pointAttrib[0] == 'ROUTEDRAG') {
        if (pointAttrib[3] != undefined && pointAttrib[3] != null) {
            pointAttrib[2] = pointAttrib[3];
        }
    }
    if ((pathType == 1 && pointAttrib[0] == 'ENDPOINT')
        || (pathType == 2 && (pointAttrib[0] == 'STARTPOINT' || pointAttrib[0] == 'ENDPOINT'))
        || (pathType == 3 && pointAttrib[0] == 'STARTPOINT') || pointAttrib[0] == 'ANNOTATION') {
        var features = self.getFeaturesOfRoutePath(0);
        self.onDragCompleteSetPoint(X, Y, features, pointAttrib, marker);
    }
    else if (pathType == 4 &&
        (this.advancedEditRouteDetails.terminalPoint == -1 && (pointAttrib[0] == 'STARTPOINT' || pointAttrib[0] == 'ENDPOINT'))
        || (this.advancedEditRouteDetails.terminalPoint == 0 && pointAttrib[0] == 'ENDPOINT')
        || (this.advancedEditRouteDetails.terminalPoint == 1 && pointAttrib[0] == 'STARTPOINT')) {
        if (this.advancedEditRouteDetails.terminalPoint == -1 && (pointAttrib[0] == 'STARTPOINT' || pointAttrib[0] == 'ENDPOINT')) {
            var val = this.validateEditPoints(X, Y, pointAttrib[0] == 'STARTPOINT' ? 7 : 8, -1);
            if (val == true) {
                var features = self.getFeaturesOfRoutePath(this.advancedEditRouteDetails.pathIndex);
                self.onDragCompleteSetPoint(X, Y, features, pointAttrib, marker);
            }
            else {
                self.undoMarkerDrag(marker[0], pointAttrib);
                return;
            }
        }
        else {
            var features = self.getFeaturesOfRoutePath(this.advancedEditRouteDetails.pathIndex);
            self.onDragCompleteSetPoint(X, Y, features, pointAttrib, marker);
        }
    }
    else if (pointAttrib[0] == 'TRIMPOINT') {
        this.vectorLayerMarkers.removeFeatures(marker[0]);
        var retObject = this.getNearestPathAndSegment(X, Y, true);
        this.setMarker('TRIMPOINT', retObject.x1, retObject.y1, retObject.pathIndex + ' ' + retObject.segmentIndex);
    }
    else {

        this.searchFeaturesByXY(X, Y, false, self.boundaryOffset, function (features) {
            if (features == null || features.length <= 0) {
                if (typeof IsAutomaticPlaning != "undefined" && IsAutomaticPlaning == false) {
                    showNotification('No road selected. Select a valid location.');
                }
                self.undoMarkerDrag(marker[0], pointAttrib);
                return;
            }
            else {
                self.onDragCompleteSetPoint(X, Y, features, pointAttrib, marker);
            }
        });
    }
}

IfxStpMap.prototype.onDragCompleteSetPoint = function (X, Y, features, pointAttrib, marker) {
    if (pointAttrib[0] != 'ANNOTATION') {
        this.currentActiveRoutePathIndex = pointAttrib[1];
        var lonlat = this.olMap.getLonLatFromPixel({ x: X, y: Y });
        var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
        var retAlternateObject = IfxStpmapCommon.findAlternateNearestFeatureIndex(features, lonlat.lon, lonlat.lat, retObject);

        if (IfxStpmapCommon.checkForPedestrianRoad(features[retObject.index]) == true) {
            showNotification('You have selected a road which is not accessible for trucks. Select a valid location.');
            this.undoMarkerDrag(marker[0], pointAttrib);
            return;
        }
        var rpPoint = null;
        if (this.currentActiveRoutePathIndex > 0 && retObject.distance > 200) {
            this.undoMarkerDrag(marker[0], pointAttrib);
        }
        else {
            rpPoint = this.createRoutePointObject(pointAttrib[0], features[retObject.index], retObject, pointAttrib[2]);
            if (retAlternateObject != undefined) { //check added for HE-6836
                alternatePoint = this.createRoutePointObject(pointAttrib[0], features[retAlternateObject.index], retAlternateObject, pointAttrib[2]);
            }
            else {
                alternatePoint = this.createRoutePointObject(pointAttrib[0], features[retObject.index], retObject, pointAttrib[2]);
            }
            var isAlternate = this.getcurrentSelectedRouteType();
            if (rpPoint.otherinfo.pointfeature.attributes.ROUNDABOUT == 'Y' && isAlternate != 0)//Added by nithin 7/14/2015
                showNotification('Diverge/merge route points cannot be placed on a roundabout: please adjust relevant point(s). For assistance call the Helpdesk on 0300 470 3733.');
            if (pointAttrib[0] == 'ROUTEDRAG') {
                this.vectorLayerDragRouteMarker.removeFeatures(marker[0]);
            }
            else {
                this.vectorLayerMarkers.removeFeatures(marker[0]);
            }
            marker = this.setMarker(pointAttrib[0], retObject.x1, retObject.y1, pointAttrib[2]);
            rpPoint['otherinfo']['marker'] = marker;

            if (pointAttrib[0] == 'ROUTEDRAG') {
                var pointIndex = pointAttrib[2] > 0 && this.routeManager.getAllRoutePointCount(this.currentActiveRoutePathIndex) < 2 ? 0 : pointAttrib[2];
                this.routeManager.updateAllRoutePoint(pointAttrib[1], pointIndex, rpPoint);
                this.routeManager.updateAllAlternateRoutePoint(pointAttrib[1], pointIndex, alternatePoint);
                this.updateRoutePointMarkers(this.currentActiveRoutePathIndex, pointIndex, this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) - 1);
            }
            else {
                var pointIndex = pointAttrib[2] > 0 && this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) < 2 ? 0 : pointAttrib[2];
                this.routeManager.updateRoutePoint(pointAttrib[1], pointIndex, rpPoint);
                this.routeManager.updateAlternateRoutePoint(pointAttrib[1], pointIndex, alternatePoint);
                this.resetAllRoutePoints(pointAttrib[1]);
            }
        }

        this.reactivateControls();

        if (pointAttrib[0] == 'ROUTEDRAG') {
            this.setPageState('routedragged');
        }
        else {
            this.setPageState('pointselected');
        }

        if (this.eventList['ONDRAGCOMPLETE'] != undefined && typeof (this.eventList['ONDRAGCOMPLETE']) === "function") {
            this.eventList['ONDRAGCOMPLETE'](rpPoint, pointAttrib[1], true);
        }

        //initiate auto planning
        if (pointAttrib[0] == 'ROUTEDRAG') {
            if (this.eventList['ONROUTEDRAGCOMPLETE'] != undefined && typeof (this.eventList['ONROUTEDRAGCOMPLETE']) === "function") {
                this.eventList['ONROUTEDRAGCOMPLETE'](rpPoint, pointAttrib[1], true);
            }
        }
    }
    else {
        var retObject = this.getNearestPathAndSegment(X, Y, true);
        var annotObject = this.createAnnotationObject(retObject.feature, retObject);
        var oldOldAnnot = this.routeManager.getAnnotation(pointAttrib[1], pointAttrib[2], pointAttrib[3]);
        annotObject.annotationContactList = oldOldAnnot.annotationContactList;
        annotObject.annotType = oldOldAnnot.annotType;
        annotObject.annotText = oldOldAnnot.annotText;
        this.deleteAnnotation(pointAttrib[1], pointAttrib[2], pointAttrib[3]);
        objifxStpMap.addAnnotation(pointAttrib[1], pointAttrib[2], pointAttrib[3], annotObject);
        var marker = this.setMarker('ANNOTATION', retObject.x1, retObject.y1, pointAttrib[1] + ' ' + pointAttrib[2] + ' ' + pointAttrib[3]);
        annotObject['otherinfo']['marker'] = marker;
        annotObject['otherinfo']['pathIndex'] = retObject.pathIndex;
        annotObject['otherinfo']['segmentIndex'] = retObject.segmentIndex;
        if (this.eventList['ONDRAGCOMPLETE'] != undefined && typeof (this.eventList['ONDRAGCOMPLETE']) === "function") {
            this.eventList['ONDRAGCOMPLETE'](annotObject, pointAttrib[1], false);
        }
    }
}

IfxStpMap.prototype.panMapForDrag = function (pixel) {
    var currentBounds = this.getBounds();
    var latlon = this.olMap.getLonLatFromPixel(pixel);
    if (latlon.lon <= currentBounds.left + 100) {
        this.olMap.pan(-300, 0);
    }
    else if (latlon.lon >= currentBounds.right - 100) {
        this.olMap.pan(300, 0);
    }
    else if (latlon.lat >= currentBounds.bottom - 100) {
        this.olMap.pan(0, -300);
    }
    else if (latlon.lat <= currentBounds.top + 100) {
        this.olMap.pan(0, 300);
    }
}

IfxStpMap.prototype.createDragFeature = function () {
    var context = this;
    var dragControl = new OpenLayers.Control.DragFeature(this.vectorLayerMarkers, {
        autoActivate: true,
        onStart: function (feature, pixel) {
            context.onDragStartCallback(feature, pixel);
            context.dragActivity.dragActive = true;
        },
        onComplete: function (feature, pixel) {
            context.onDragCompleteCallback(feature, pixel);
            context.dragActivity = { dragActive: false, panned: false };
            this.deactivate();
            this.activate();
        }
    });
    this.olMap.addControl(dragControl);

    var dragControlRoute = new OpenLayers.Control.DragFeature(this.vectorLayerDragRouteMarker, {
        autoActivate: true,
        onStart: function (feature, pixel) {
            context.onDragStartCallback(feature, pixel);
            context.dragActivity.dragActive = true;
        },
        onComplete: function (feature, pixel) {
            context.onDragCompleteCallback(feature, pixel, true);
            context.dragActivity = { dragActive: false, panned: false };
            this.deactivate();
            this.activate();
        }
    });
    this.olMap.addControl(dragControlRoute);

    var selectHoverControl = new OpenLayers.Control.SelectFeature([this.vectorLayerMarkers, this.vectorLayerDragRouteMarker], {
        multiple: true,
        clickout: true,
        hover: true,
        highlightOnly: true,
        renderIntent: 'temporary',
        overFeature: function (feature) {
            context.currentMouseOverFeature = feature;
            if (context.eventList['FEATUREOVER'] != undefined && typeof (context.eventList['FEATUREOVER']) === "function") {
                context.eventList['FEATUREOVER']({ feature: feature }, 'MARKER');
            }
        },
        outFeature: function (feature) {
            context.currentMouseOverFeature = null;
            if (context.eventList['FEATUREOUT'] != undefined && typeof (context.eventList['FEATUREOUT']) === "function") {
                context.eventList['FEATUREOUT']({ feature: feature }, 'MARKER');
            }
        },
        callbacks:
        {
            click: function (feature) {
                context.removeRouteDragPoint(feature);
            }
        }
    });
    //selectHoverControl.handlers.feature.stopDown = true;
    this.olMap.addControl(selectHoverControl);
    selectHoverControl.activate();
}

IfxStpMap.prototype.createCacheReadWriteControls = function () {
    var cacheRead = new OpenLayers.Control.CacheRead();
    this.olMap.addControl(cacheRead);

    var cacheWrite = new OpenLayers.Control.CacheWrite({
        autoActivate: true,
        imageFormat: this.imageFormat
    });
    this.olMap.addControl(cacheWrite);
}

IfxStpMap.prototype.showLinkDetails = function (e) {
    var self = this;
    var lonlat = this.olMap.getLonLatFromViewPortPx(e.xy);
    this.searchFeaturesByXY(e.xy.x, e.xy.y, false, self.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            return;
        }
        else {
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
            var featureSel = features[retObject.index];
            var content = "<div class='text-normal' style='font-size:.8em'><table><tr><td class='text-normal pr-2 paddingCls' style='font-size:16px'>" + 'LINK DETAILS' + "</td></tr></table><br>LINK ID: "
                + featureSel.attributes.LINK_ID
                + "<br>REF IN ID: " + featureSel.attributes.REF_IN_ID
                + "<br>NREF IN ID: " + featureSel.attributes.NREF_IN_ID
                + "<br>PRIVATE: " + featureSel.attributes.PRIVATE
                + "<br>PAVED: " + featureSel.attributes.PAVED
                + "<br>PUB_ACCESS: " + featureSel.attributes.PUB_ACCESS
                + "<br>ST_NAME: " + featureSel.attributes.ST_NAME
                + "<br>TUNNEL: " + featureSel.attributes.TUNNEL
                + "<br>AR_AUTO: " + featureSel.attributes.AR_AUTO
                + "<br>AR_BUS: " + featureSel.attributes.AR_BUS
                + "<br>AR_MOTOR: " + featureSel.attributes.AR_MOTOR
                + "<br>AR_PEDEST: " + featureSel.attributes.AR_PEDEST
                + "<br>AR_TAXIS: " + featureSel.attributes.AR_TAXIS
                + "<br>AR_TRAFF: " + featureSel.attributes.AR_TRAFF
                + "<br>AR_TRUCKS: " + featureSel.attributes.AR_TRUCKS
                + "<br>DIR_TRAVEL: " + featureSel.attributes.DIR_TRAVEL
                + "<br>FERRY_TYPE: " + featureSel.attributes.FERRY_TYPE
                + "<br>FUNC_CLASS: " + featureSel.attributes.FUNC_CLASS
                + "</div>";

            self.olMap.addPopup(new OpenLayers.Popup.FramedCloud(
                "LINK_DETAILS",
                self.olMap.getLonLatFromPixel(e.xy),
                null,
                content,
                null,
                true
            ));
        }
    });
}

IfxStpMap.prototype.createClickControl = function () {
    var self = this;
    OpenLayers.Control.Click = OpenLayers.Class(OpenLayers.Control, {
        defaultHandlerOptions: {
            'single': true,
            'double': true,
            'pixelTolerance': 0,
            'stopSingle': false,
            'stopDouble': false
        },

        initialize: function (options) {
            this.handlerOptions = OpenLayers.Util.extend(
                {}, this.defaultHandlerOptions
            );
            OpenLayers.Control.prototype.initialize.apply(
                this, arguments
            );
            this.handler = new OpenLayers.Handler.Click(
                this, {
                'click': this.trigger,
                'dblclick': this.dbClick
            }, this.handlerOptions
            );
        },
        dbClick: function (e) {
            $('#intellizenz-ctxmenu').remove();

        },

        trigger: function (e) {
            if (e.ctrlKey == true && e.shiftKey == true) {
                var lonlat = self.olMap.getLonLatFromViewPortPx(e.xy);
                showNotification("X, Y: " + lonlat.lon + ", "
                    + lonlat.lat);
                return;
            }
            else {
                if (e.ctrlKey == true) {
                    if (self.pageType == 'DELEGATION_VIEWANDEDIT' || self.pageType == 'ROADOWNERSHIP_VIEWANDEDIT') {
                        if (btnSelectAndPlan.active == true) {
                            if (self.pageType == 'DELEGATION_VIEWANDEDIT')
                                objifxStpMapRoadDelegation.selectAndPlanLink(e, 'ENDPOINT');
                            else
                                objifxStpmapRoadOwnership.selectAndPlanLink(e, 'ENDPOINT');
                        }
                        else {
                            self.showLinkDetails(e);
                        }
                    }
                    else {
                        self.showLinkDetails(e);
                    }
                    return;
                }
                else {
                    if (e.altKey == true && e.shiftKey == true) {
                        if (self.pageType == 'STRUCTURES')
                            constraintbydescToolbar.div.style.display = "inline";
                    }
                }
            }
            $('#intellizenz-ctxmenu').remove();
            //mapcontextMenuOn = false;
            //currentMouseOverFeature = null;
            if (self.pageType != 'DELEGATION_VIEWANDEDIT' && self.pageType != 'ROADOWNERSHIP_VIEWANDEDIT') {
                if (btnReverseMnvr.active == true) {
                    self.drawManoeuvreSet(e, 'OVERRIDE');
                }
                else if (btnUturnMnvr.active == true) {
                    self.drawManoeuvreSet(e, 'UTURN');
                }
                else if (btnConfirmedMnvr.active == true) {
                    self.drawManoeuvreSet(e, 'CONFIRMED');
                }
                else if (btnCutRoute.active == true) {
                    self.cutRoute(e);
                }
                //else if (btnTrimRoute.active == true) {
                //    self.trimRoute(e);
                //}
                else if (btnAffectedStructures.active == true) {

                }
                else if (btnAffectedConstraints.active == true) {

                }
            }
            else {
                if (btnSelectAndPlan.active == true) {
                    if (self.pageType == 'DELEGATION_VIEWANDEDIT')
                        objifxStpMapRoadDelegation.selectAndPlanLink(e, 'STARTPOINT');
                    else
                        objifxStpmapRoadOwnership.selectAndPlanLink(e, 'STARTPOINT');
                }
                else if (btnSelectByLink.active == true) {
                    if (self.pageType == 'DELEGATION_VIEWANDEDIT')
                        objifxStpMapRoadDelegation.selectByLink(e);
                    else
                        objifxStpmapRoadOwnership.selectByLink(e);
                }
                else if (btnDeselectLink.active == true) {
                    if (self.pageType == 'DELEGATION_VIEWANDEDIT')
                        objifxStpMapRoadDelegation.deselectLink(e);
                    else
                        objifxStpmapRoadOwnership.deselectLink(e);

                }
            }
        }
    });

    var click = new OpenLayers.Control.Click();
    this.olMap.addControl(click);
    click.activate();
}

IfxStpMap.prototype.setLocationIndicatorAtXY = function (X, Y) {
    return this.setMarker('LOCATIONIDICATOR', X, Y);
}

IfxStpMap.prototype.addAnnotation = function (pathIndex, segmentIndex, insertat, annotation) {
    this.routeManager.addAnnotation(pathIndex, segmentIndex, insertat, annotation);
}

IfxStpMap.prototype.deleteAnnotation = function (pathIndex, segmentIndex, arrIndex) {
    var annotation = this.routeManager.getAnnotation(pathIndex, segmentIndex, arrIndex);
    if (annotation != undefined) {
        this.vectorLayerMarkers.removeFeatures(annotation.otherinfo.marker);
        this.routeManager.deleteAnnotation(pathIndex, segmentIndex, arrIndex);
        this.updateAnnotationMarkers(pathIndex, segmentIndex, arrIndex);
        this.reactivateControls();
        this.setPageState('routeplanned');
    }
}

IfxStpMap.prototype.clearAnnotationsofRoutePath = function (pathIndex) {
    for (var i = this.vectorLayerMarkers.features.length - 1; i >= 0; i--) {
        var pointAttrib = this.vectorLayerMarkers.features[i].data.name.split(" ");
        if (pointAttrib[0] == 'ANNOTATION' && pointAttrib[1] == pathIndex) {
            this.vectorLayerMarkers.removeFeatures(this.vectorLayerMarkers.features[i]);
        }
    }
}

IfxStpMap.prototype.deleteAnnotationsofRouteSegment = function (pathIndex, segmentIndex) {
    var rSegment = this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex];
    if (rSegment.routeAnnotationsList != null) {
        for (var i = 0; i < rSegment.routeAnnotationsList.length; i++) {
            this.deleteAnnotation(pathIndex, segmentIndex, 0);
        }
    }
}

//PARAMETERS
//pointType: START,END,WAYPOINT,VIAPOINT
//Zoomin: true,false
//X: longitude
//Y: latitude
//callback: optional callback function
//pointPos: specifies at which position to insert
IfxStpMap.prototype.setRoutePointAtXY = function (args, callback) {
    //openRoutenav();//ESDAL 4 2022
    if (args.pointType != 'ANNOTATION' && args.pointPos != -1) {
        if (args.pointType == 'STARTPOINT' || args.pointType == 'ENDPOINT')
            this.deleteRoutePointByType(args.pointType)
        else {
            this.deleteRoutePoint(args.pointPos);
        }
    }
    var self = this;
    var pathType = this.getcurrentSelectedRouteType();
    if (args.pointType == 'ANNOTATION') {
        self.setRoutePoint({ pointType: args.pointType, pointPos: -1, X: args.X, Y: args.Y, type: args.type, setMarker: true, bLonLat: false, features: null }, callback);
    }
    else if ((pathType == 1 && args.pointType == 'ENDPOINT')
        || (pathType == 2 && (args.pointType == 'STARTPOINT' || args.pointType == 'ENDPOINT'))
        || (pathType == 3 && args.pointType == 'STARTPOINT')) {
        var features = self.getFeaturesOfRoutePath(0);
        self.setRoutePoint({ pointType: args.pointType, pointPos: -1, X: args.X, Y: args.Y, setMarker: true, bLonLat: false, features: features }, callback);
    }
    else if (args.pointType == 'ROUTEDRAG') {
        var features = self.getFeaturesOfRoutePath(self.currentActiveRoutePathIndex);
        self.setRoutePoint({ pointType: args.pointType, pointPos: -1, X: args.X, Y: args.Y, setMarker: true, bLonLat: false, features: features }, callback);
        dragRouteControl.deactivate();
    }
    else if (pathType == 4 &&
        (this.advancedEditRouteDetails.terminalPoint == -1 && (args.pointType == 'STARTPOINT' || args.pointType == 'ENDPOINT'))
        || (this.advancedEditRouteDetails.terminalPoint == 0 && args.pointType == 'ENDPOINT')
        || (this.advancedEditRouteDetails.terminalPoint == 1 && args.pointType == 'STARTPOINT')) {
        var features = self.getFeaturesOfRoutePath(this.advancedEditRouteDetails.pathIndex);
        self.setRoutePoint({ pointType: args.pointType, pointPos: -1, X: args.X, Y: args.Y, setMarker: true, bLonLat: false, features: features }, callback);
    }
    else {
        this.searchFeaturesByXY(args.X, args.Y, args.searchInBbox, self.boundaryOffset, function (features) {
            self.pageState = 'planninginprogress';
            if (features == null || features.length <= 0) {
                self.searchFeaturesByXY(args.X, args.Y, args.searchInBbox, 1000, function (features) {
                    if (features == null || features.length <= 0) {
                        if (typeof IsAutomaticPlaning == "undefined" || IsAutomaticPlaning == null || IsAutomaticPlaning == false) {
                            showToastMessage({
                                message: 'No road selected. Select a valid location',
                                type: "error"
                            })
                        }
                        if (callback && typeof (callback) === "function") {
                            callback();
                        }
                    }
                    else {
                        var rPoint = self.setRoutePoint({
                            pointType: args.pointType,
                            pointPos: args.pointPos,
                            X: args.X,
                            Y: args.Y,
                            setMarker: true,
                            bLonLat: args.searchInBbox,
                            features: features,
                            locationDesc: args.locationDesc
                        }, callback);

                        if (rPoint != null && self.pageType != 'NOMAPDISPLAY' && args.Zoomin == true) {
                            self.olMap.zoomToExtent(self.vectorLayerMarkers.getDataExtent());
                        }
                    }
                });
            }
            else {
                var rPoint = self.setRoutePoint({
                    pointType: args.pointType,
                    pointPos: args.pointPos,
                    X: args.X,
                    Y: args.Y,
                    setMarker: true,
                    bLonLat: args.searchInBbox,
                    features: features,
                    locationDesc: args.locationDesc
                }, callback);

                if (rPoint != null && self.pageType != 'NOMAPDISPLAY' && args.Zoomin == true) {
                    self.olMap.zoomToExtent(self.vectorLayerMarkers.getDataExtent());
                }
            }
        });
    }
}

//called when QAS is invoked
IfxStpMap.prototype.setRoutePoint = function (args, callback) {
    if (args.bLonLat || args.setMarker == false) {
        var lonlat = new OpenLayers.LonLat(args.X, args.Y);
    }
    else {
        var lonlat = this.olMap.getLonLatFromPixel({ x: args.X, y: args.Y });
    }
    var pointIndex;
    if (args.pointType == 'ANNOTATION') {
        pointIndex = 0;
    }
    else {
        pointIndex = (args.pointPos != -1) ? args.pointPos : this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex);
    }
    var retObject, retAlternateObject;
    if (args.pointType == 'ANNOTATION') {
        retObject = this.getNearestPathAndSegment(args.X, args.Y, true, null, args.type);
        if (retObject.pathIndex == -1 || retObject.segmentIndex == -1) {
            if (callback && typeof (callback) === "function") {
                callback(null);
            }
            return null;
        }
    }
    else {
        if (args.locationDesc != undefined && args.locationDesc != null) {
            retObject = IfxStpmapCommon.findNearestSuitableFeatureIndex(args.features, lonlat.lon, lonlat.lat, args.locationDesc);
            retAlternateObject = IfxStpmapCommon.findNearestSuitableAlternateFeatureIndex(args.features, lonlat.lon, lonlat.lat, args.locationDesc);
            if (retAlternateObject == undefined || retAlternateObject == null) {
                retAlternateObject = retObject;
            }
        }
        else {
            retObject = IfxStpmapCommon.findNearestFeatureIndex(args.features, lonlat.lon, lonlat.lat);
            retAlternateObject = IfxStpmapCommon.findAlternateNearestFeatureIndex(args.features, lonlat.lon, lonlat.lat, retObject);
        }
        if (args.pointType == 'ROUTEDRAG') {
            pointIndex = this.identifyWaypointPos(args.features[retObject.index]);
        }
        if (retObject == undefined || retObject == null) {
            if (callback && typeof (callback) === "function") {
                callback(null);
            }
            return null;
        }
        if (IfxStpmapCommon.checkForPedestrianRoad(args.features[retObject.index]) == true) {
            ShowInfoPopup('You have selected a road which is not accessible for trucks. Select a valid location');
            if (callback && typeof (callback) === "function") {
                callback(null);
            }
            return null;
        }
    }

    var pointObj = null;
    var alternatePointObj = null;
    if (args.pointType != 'ANNOTATION' && args.pointType != 'ROUTEDRAG') {
        pointObj = this.createRoutePointObject(args.pointType, args.features[retObject.index], retObject, this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex), args.locationDesc);
        if (retAlternateObject != undefined) { // check added for HE-6836
            alternatePointObj = this.createRoutePointObject(args.pointType, args.features[retAlternateObject.index], retAlternateObject, this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex), args.locationDesc);
        }
        else {
            alternatePointObj = this.createRoutePointObject(args.pointType, args.features[retObject.index], retObject, this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex), args.locationDesc);
        }
        if (this.pageType != 'NOMAPDISPLAY' && args.setMarker == true) {
            var marker = this.setMarker(args.pointType, retObject.x1, retObject.y1, pointIndex);
            pointObj['otherinfo']['marker'] = marker;
        }
        this.routeManager.addRoutePoint(this.currentActiveRoutePathIndex, pointIndex, pointObj, true, alternatePointObj);
        this.resetAllRoutePoints(this.currentActiveRoutePathIndex);
        if ((args.pointType == 'WAYPOINT' || args.pointType == 'VIAPOINT') && objifxStpMap.routeManager.RoutePart.routePathList[0] != []) {
            this.backupAnnotationsinRoute();
        }
        if ((args.pointType == 'WAYPOINT' || args.pointType == 'VIAPOINT') && pointIndex < this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) && this.pageType != 'NOMAPDISPLAY')
            this.updateRoutePointMarkers(this.currentActiveRoutePathIndex, pointIndex, this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) - 1);
    }
    else if (args.pointType == 'ANNOTATION') {
        pointObj = this.createAnnotationObject(retObject.feature, retObject);
        if (args.setMarker == true) {
            var marker = this.setMarker(args.pointType, retObject.x1, retObject.y1, retObject.pathIndex + ' ' + retObject.segmentIndex + ' ' + this.routeManager.getAnnotationCount(retObject.pathIndex, retObject.segmentIndex));
        }
        pointObj['otherinfo']['marker'] = marker;
        pointObj['otherinfo']['pathIndex'] = retObject.pathIndex;
        pointObj['otherinfo']['segmentIndex'] = retObject.segmentIndex;
    }
    else {
        if (args.pointType == 'ROUTEDRAG') {
            pointObj = this.createRoutePointObject(args.pointType, args.features[retObject.index], retObject, this.routeManager.getAllRoutePointCount(this.currentActiveRoutePathIndex), args.locationDesc);
            alternatePointObj = this.createRoutePointObject(args.pointType, args.features[retAlternateObject.index], retAlternateObject, this.routeManager.getAllAlternateRoutePointCount(this.currentActiveRoutePathIndex), args.locationDesc);
            if (this.pageType != 'NOMAPDISPLAY' && args.setMarker == true) {
                var marker = this.setMarker(args.pointType, retObject.x1, retObject.y1, pointIndex);
                pointObj['otherinfo']['marker'] = marker;
            }
            this.routeManager.addAllRoutePoint(this.currentActiveRoutePathIndex, pointIndex, pointObj, true, alternatePointObj);
            for (var i = this.vectorLayerDragRouteMarker.features.length - 1; i >= 0; i--) {
                if (this.vectorLayerDragRouteMarker.features[i].data.name == undefined) {
                    this.vectorLayerDragRouteMarker.removeFeatures(this.vectorLayerDragRouteMarker.features[i]);
                }
            }
            this.updateRoutePointMarkers(this.currentActiveRoutePathIndex, pointIndex, this.routeManager.getAllRoutePointCount(this.currentActiveRoutePathIndex) - 1, args.pointType);
        }
        else {
            this.updateRoutePointMarkers(this.currentActiveRoutePathIndex, pointIndex, this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) - 1);
        }
    }

    if (callback && typeof (callback) === "function") {
        callback(pointObj);
    }
    return pointObj;
}

IfxStpMap.prototype.getLocationDescription = function (feature) {
    var strRes = '';
    if (feature.attributes.ST_NAME)
        strRes = feature.attributes.ST_NAME;

    if (strRes == '') {
        if (feature.attributes.ST_NM_BASE)
            strRes = feature.attributes.ST_NM_BASE;
    }
    return strRes;
}

IfxStpMap.prototype.createAnnotationObject = function (feature, retObject) {
    var rSegment = this.routeManager.RoutePart.routePathList[retObject.pathIndex].routeSegmentList[retObject.segmentIndex];
    var routeAnnotation = { geometry: { sdo_point: {} }, annotationContactList: [], otherinfo: {} };
    routeAnnotation.geometry.sdo_point.X = retObject.x1;
    routeAnnotation.geometry.sdo_point.Y = retObject.y1;
    routeAnnotation.easting = retObject.x1;
    routeAnnotation.northing = retObject.y1;
    if (feature.attributes.LINK_ID != undefined)
        routeAnnotation.linkId = feature.attributes.LINK_ID;
    else
        routeAnnotation.linkId = rSegment.startLinkId;
    routeAnnotation.linearRef = IfxStpmapCommon.getLRSLength(feature.geometry, new OpenLayers.Geometry.Point(retObject.x1, retObject.y1));
    routeAnnotation.direction = IfxStpmapCommon.getDirectionOfRouteLink(rSegment, retObject, feature, this);
    return routeAnnotation;
}

IfxStpMap.prototype.createRoutePointObject = function (pointType, feature, retObject, pointCount, pointDesc) {
    var rpPoint = {};
    var nPointType = IfxStpmapCommon.getPointTypeID(pointType);
    var locationDescr = this.getLocationDescription(feature);

    var pointNumber = 0;
    if (nPointType == 0 || nPointType == 1) {
        pointNumber = parseInt(nPointType) + 1;
    }
    else {
        pointNumber = parseInt(pointCount) - 1;
    }

    rpPoint['pointType'] = nPointType;
    rpPoint['otherinfo'] = {};

    if (pointDesc) {
        rpPoint['pointDescr'] = pointDesc;
        rpPoint['otherinfo'].isFullAddress = true;
    }
    else if (locationDescr) {
        rpPoint['pointDescr'] = locationDescr + ', ' + feature.attributes.R_POSTCODE;
        rpPoint['otherinfo'].isFullAddress = false;
    }
    else {
        rpPoint['pointDescr'] = Math.round(retObject.x1) + ',' + Math.round(retObject.y1) + ', ' + feature.attributes.R_POSTCODE;
        rpPoint['otherinfo'].isFullAddress = false;
    }

    rpPoint['routePointNo'] = pointNumber;
    rpPoint['direction'] = null;
    rpPoint['routeContactList'] = [];
    rpPoint['linkId'] = feature.attributes.LINK_ID;
    rpPoint['isAnchorPoint'] = nPointType == 4 ? 1 : 0;
    rpPoint['pointGeom'] = { sdo_point: { X: '', Y: '' } };
    rpPoint.pointGeom.sdo_point.X = retObject.x1;
    rpPoint.pointGeom.sdo_point.Y = retObject.y1;
    var lrsMeasure = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(retObject.x1, retObject.y1), { tolerance: 0.5, details: true });
    rpPoint['lrs'] = Math.round(lrsMeasure.length);
    rpPoint.showRoutePoint = 1;

    rpPoint['otherinfo'].pointfeature = feature;
    rpPoint['otherinfo'].dir_travel = feature.attributes.DIR_TRAVEL;
    rpPoint['otherinfo'].func_class = feature.attributes.FUNC_CLASS;

    if (feature.attributes.DIR_TRAVEL == 'B') {
        if (nPointType == 0 || nPointType == 1) {
            if (lrsMeasure.measure <= 0.5) {
                rpPoint['otherinfo'].beginNodeId = feature.attributes.REF_IN_ID;
                rpPoint['otherinfo'].linkId = feature.attributes.LINK_ID;
                rpPoint['otherinfo'].endNodeId = feature.attributes.NREF_IN_ID;
            }
            else {
                rpPoint['otherinfo'].beginNodeId = feature.attributes.NREF_IN_ID;
                rpPoint['otherinfo'].linkId = feature.attributes.LINK_ID;
                rpPoint['otherinfo'].endNodeId = feature.attributes.REF_IN_ID;
            }
        }
        else {
            rpPoint['otherinfo'].beginNodeId = feature.attributes.REF_IN_ID;
            rpPoint['otherinfo'].linkId = feature.attributes.LINK_ID;
            rpPoint['otherinfo'].endNodeId = feature.attributes.NREF_IN_ID;
        }
    }
    else {
        if ((pointType == 'STARTPOINT' && feature.attributes.DIR_TRAVEL == 'F') || (pointType == 'ENDPOINT' && feature.attributes.DIR_TRAVEL == 'T')) {
            rpPoint['otherinfo'].beginNodeId = feature.attributes.NREF_IN_ID;
            rpPoint['otherinfo'].linkId = feature.attributes.LINK_ID;
            rpPoint['otherinfo'].endNodeId = feature.attributes.REF_IN_ID;
        }
        else {
            rpPoint['otherinfo'].beginNodeId = feature.attributes.REF_IN_ID;
            rpPoint['otherinfo'].linkId = feature.attributes.LINK_ID;
            rpPoint['otherinfo'].endNodeId = feature.attributes.NREF_IN_ID;
        }
        this.updateNodeInfo(nPointType, pointNumber, rpPoint, feature);
    }
    return rpPoint;
}

//function to add a marker on map.  marker can be of any of the following types
//START,END,WAYPOINT,VIAPOINT
IfxStpMap.prototype.setMarker = function (pointType, X, Y, label, pathno, pathtype) {
    var markerStyle;
    var markerName;

    if (pointType == 'STARTPOINT') {
        markerName = 'STARTPOINT' + " " + this.currentActiveRoutePathIndex + " " + 0;
        if (pathno != undefined) {
            var imgFile = IfxStpmapCommon.getMarkerStartEndImage(pointType, pathtype, pathno);
        }
        else {
            var imgFile = IfxStpmapCommon.getMarkerImage(this.getcurrentSelectedRouteType(), pointType);
        }
        if (imgFile == '/Content/assets/Icons/map-startpoint.svg' || imgFile == '/Content/assets/Icons/map-startpoint.svg') {
            markerStyle = { externalGraphic: imgFile, graphicHeight: 40, graphicWidth: 35, graphicXOffset: -9, graphicYOffset: -24 };
        }
        else if (imgFile == '/Content/Images/editA.png') {
            markerStyle = { externalGraphic: imgFile, graphicHeight: 35, graphicWidth: 35, graphicXOffset: -18, graphicYOffset: -35 };
        }
        else {
            markerStyle = { externalGraphic: imgFile, graphicHeight: 40, graphicWidth: 35 };
        }
    }
    else if (pointType == 'ENDPOINT') {
        markerName = 'ENDPOINT' + " " + this.currentActiveRoutePathIndex + " " + 1;
        if (pathno != undefined) {
            var imgFile = IfxStpmapCommon.getMarkerStartEndImage(pointType, pathtype, pathno);
        }
        else {
            var imgFile = IfxStpmapCommon.getMarkerImage(this.getcurrentSelectedRouteType(), pointType);
        }
        if (imgFile == '/Content/assets/Icons/map-endpoint.svg' || imgFile == '/Content/assets/Icons/map-endpoint.svg') {
            markerStyle = { externalGraphic: imgFile, graphicHeight: 40, graphicWidth: 30, graphicXOffset: -9, graphicYOffset: -30 };
        }
        else if (imgFile == '/Content/Images/editB.png') {
            markerStyle = { externalGraphic: imgFile, graphicHeight: 35, graphicWidth: 35, graphicXOffset: -18, graphicYOffset: -35 };
        }
        else {
            markerStyle = { externalGraphic: imgFile, graphicHeight: 40, graphicWidth: 30 };
        }
    }
    else if (pointType == 'WAYPOINT') {
        markerName = 'WAYPOINT' + " " + this.currentActiveRoutePathIndex + " " + label;
        markerStyle = {
            externalGraphic: '/Content/Images/waymarker.svg', graphicHeight: 30, graphicWidth: 30, label: (label - 1).toString(),
            labelXOffset: 0,
            labelYOffset: 0,
            fontColor: "black",
            fontSize: "14px",
            fontFamily: "Arial",
            fontWeight: "bold",
            labelAlign: "cm"
        };
    }
    else if (pointType == 'VIAPOINT') {
        markerName = 'VIAPOINT' + " " + this.currentActiveRoutePathIndex + " " + label;
        markerStyle = {
            externalGraphic: '/Content/Images/anchorpoint.svg', graphicHeight: 30, graphicWidth: 30, label: (label - 1).toString(),
            labelXOffset: 0,
            labelYOffset: 0,
            fontColor: "white",
            fontSize: "14px",
            fontFamily: "Arial",
            fontWeight: "bold",
            labelAlign: "cm"
        };
    }
    else if (pointType == 'LOCATIONIDICATOR') {
        markerName = 'LOCATIONIDICATOR' + " " + this.currentActiveRoutePathIndex;
        markerStyle = {
            externalGraphic: '/Content/Images/location.png', graphicHeight: 40, graphicWidth: 40,
            fontColor: "white",
            fontSize: "12px",
            fontFamily: "Arial",
            fontWeight: "bold",
            labelAlign: "cm"
        };
    }
    else if (pointType == 'ANNOTATION') {
        markerName = 'ANNOTATION' + " " + label;
        markerStyle = {
            externalGraphic: '/Content/Images/annotindicator.svg', graphicHeight: 40, graphicWidth: 37, graphicXOffset: -12, graphicYOffset: -38,
            labelXOffset: 0,
            labelYOffset: -2,
            fontColor: "white",
            fontSize: "12px",
            fontFamily: "Arial",
            fontWeight: "bold",
            labelAlign: "cm"
        };
    }
    else if (pointType == 'TRIMPOINT') {
        markerName = 'TRIMPOINT' + " " + X + " " + Y + " " + label;
        markerStyle = {
            externalGraphic: '/Content/Images/trimpoint.png', graphicHeight: 20, graphicWidth: 20
        };
    }
    else if (pointType == 'ROUTEDRAG') {
        markerName = 'ROUTEDRAG' + " " + this.currentActiveRoutePathIndex + " " + 0 + " " + label;
        markerStyle = { externalGraphic: '/Content/Images/route-start-icon.svg', graphicHeight: 12, graphicWidth: 12, graphicXOffset: -6, graphicYOffset: -5, title: 'Drag to change route, click to remove.' };
    }

    var markerFlag = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(X, Y),
        { name: markerName },
        markerStyle);
    if (pointType == 'LOCATIONIDICATOR') {
        this.vectorLayerMarkers.addFeatures(markerFlag);
        this.olMap.setCenter([X, Y], 9);
    }
    else if (pointType == 'ROUTEDRAG') {
        this.vectorLayerDragRouteMarker.addFeatures(markerFlag);
    }
    else {
        this.vectorLayerMarkers.addFeatures(markerFlag);
    }
    return markerFlag;
}

IfxStpMap.prototype.highlightFeatures = function (features, visibleAll) {
    this.vectorLayerRoute.addFeatures(features);
    if (visibleAll)
        this.olMap.zoomToExtent(this.vectorLayerRoute.getDataExtent());
    else
        this.olMap.zoomToExtent(features[0].geometry.getBounds());
}

IfxStpMap.prototype.clearAllFeaturesFromLayer = function (layer) {
    layer.removeFeatures(layer.features);
}

IfxStpMap.prototype.searchFeaturesInBBox = function (X, Y, boundaryOffset, callback) {
    var cqlFilters = new Array();
    cqlFilters.push('BBOX(GEOM,' + (Number(X) - boundaryOffset).toString() + ',' + (Number(Y) + boundaryOffset).toString() + ',' + (Number(X) + boundaryOffset).toString() + ',' + (Number(Y) - boundaryOffset).toString() + ')');
    this.searchFeaturesByCQL(cqlFilters, callback);
}

IfxStpMap.prototype.searchFeaturesByXY = function (X, Y, searchInBbox, boundaryOffset, callback) {
    if (searchInBbox != undefined && searchInBbox == true) {
        this.searchFeaturesInBBox(X, Y, boundaryOffset, callback);
        return;
    }

    var prms = {
        REQUEST: "GetFeatureInfo",
        EXCEPTIONS: "application/vnd.ogc.se_xml",
        BBOX: this.olMap.getExtent().toBBOX(),
        VERSION: '1.1.1',
        SERVICE: "WMS",
        INFO_FORMAT: 'application/json',
        QUERY_LAYERS: this.LayerRoadName,
        FEATURE_COUNT: 50,
        Layers: this.LayerRoadName,
        WIDTH: this.olMap.size.w,
        HEIGHT: this.olMap.size.h,
        format: this.imageFormat,
        styles: this.olMap.layers[0].params.STYLES,
        srs: this.olMap.layers[0].params.SRS,
        buffer: this.calculateBufferForFeatureSearch()
    };

    prms.x = parseInt(X);
    prms.y = parseInt(Y);

    OpenLayers.Request.GET({
        url: this.geoserverWfsUrl,
        params: prms,
        headers: { 'Accept': 'application/json' },
        success: function (reply) {
            if (callback && typeof (callback) === "function") {
                var features;
                try {
                    var format = new OpenLayers.Format.GeoJSON();
                    features = format.read(reply.responseText);
                    callback(features);
                }
                catch (err) {
                    callback(null);
                }
            }
        },
        failure: function () {
            if (callback && typeof (callback) === "function") {
                callback(null);
            }
        }
    });
}

IfxStpMap.prototype.searchFeaturesByCQL = function (cqlFilters, callback) {
    this.requestPart1 = "?request=GetFeature&typeName=ESDAL4:STREETS&propertyName=LINK_ID,ST_NAME,REF_IN_ID&outputFormat=application/json&version=1.0.0&CQL_FILTER=";
    this.geoserverWfsUrl = objifxStpMap.configGeoserverWfsUrl;
    var totalCount = cqlFilters.length;
    var featuresCollction = [];
    if (totalCount > 0) {
        for (var i = cqlFilters.length - 1; i >= 0; i--) {

            var reqString = this.geoserverWfsUrl + this.requestPart1 + cqlFilters[i];
            var request = OpenLayers.Request.GET({
                url: reqString,
                headers: { 'Accept': 'application/json' },
                async: true,
                success: function (reply) {
                    totalCount--;

                    if (reply.responseText.length > 0) {
                        var format = new OpenLayers.Format.GeoJSON();
                        var features;
                        try {
                            features = format.read(reply.responseText);
                            features.splice(features.length / 2, features.length / 2);
                            Array.prototype.push.apply(featuresCollction, features)
                            if (features.length == 0) {
                                var x = 0;
                            }
                        }
                        catch (err) {
                            if (totalCount == 0 && callback && typeof (callback) === "function") {
                                callback();
                            }
                        }
                    }

                    if (totalCount == 0) {
                        if (callback && typeof (callback) === "function") {
                            callback(featuresCollction);
                        }
                    }
                },
                failure: function () {
                    if (totalCount == 0 && callback && typeof (callback) === "function") {
                        callback();
                    }
                }
            });
        }
    }
    else {
        if (callback && typeof (callback) === "function") {
            callback();
        }
    }
}

IfxStpMap.prototype.searchFeaturesByLinkID = function (linkIDs, callback) {
    var cqlFilters = IfxStpmapCommon.getCQLFilerFromLinkIDs(linkIDs);
    this.searchFeaturesByCQL(cqlFilters, callback);
}
//IfxStpMap.prototype.searchFeaturesByRoadName = function (RoadNames, callback) {  //Nithin
//    var cqlFilters = IfxStpmapCommon.getCQLFilerFromRoadNames(RoadNames);
//   this.searchFeaturesByCQL(cqlFilters, callback);
//}

IfxStpMap.prototype.searchFeaturesOfRouteLinkList = function (routeLinkList, callback) {
    var cqlFilters = IfxStpmapCommon.getCQLFilerFromRouteLinkList(routeLinkList);
    this.searchFeaturesByCQL(cqlFilters, callback);
}

IfxStpMap.prototype.swapStartEndRoutePoints = function (pathIndex) {
    var rp1 = jQuery.extend(true, {}, this.routeManager.RoutePart.routePathList[pathIndex].routePointList[0]);
    var rp2 = jQuery.extend(true, {}, this.routeManager.RoutePart.routePathList[pathIndex].routePointList[1]);

    this.deleteRoutePointByType('STARTPOINT');
    this.deleteRoutePointByType('ENDPOINT');

    this.setRoutePoint({ pointType: 'STARTPOINT', pointPos: -1, X: rp2.pointGeom.sdo_point.X, Y: rp2.pointGeom.sdo_point.Y, locationDesc: rp2.pointDescr, setMarker: true, bLonLat: true, features: [rp2.otherinfo.pointfeature] }, null);
    this.setRoutePoint({ pointType: 'ENDPOINT', pointPos: -1, X: rp1.pointGeom.sdo_point.X, Y: rp1.pointGeom.sdo_point.Y, locationDesc: rp1.pointDescr, setMarker: true, bLonLat: true, features: [rp1.otherinfo.pointfeature] }, null);
}

IfxStpMap.prototype.swapRoutePoint = function (pointIndex1, pointIndex2) {
    var pathIndex = this.currentActiveRoutePathIndex;
    var state = this.routeManager.getStateFromPointList(pathIndex);
    if (state == 'firstpointselected' || state == 'idle') {
        return false;
    }

    this.swapStartEndRoutePoints(pathIndex);
    this.vectorLayerMarkers.redraw();
    if (this.eventList['ONDRAGCOMPLETE'] != undefined && typeof (this.eventList['ONDRAGCOMPLETE']) === "function") {
        this.eventList['ONDRAGCOMPLETE'](null, pathIndex, true);
    }
    return true;
}

IfxStpMap.prototype.updateRoutePointMarkers = function (pathIndex, fromIndex, toIndex, pointType) {
    for (var i = fromIndex; i <= toIndex; i++) {
        if (pointType == 'ROUTEDRAG') {
            var rp1 = this.routeManager.getAllRoutePoint(pathIndex, i);
        }
        else {
            var rp1 = this.routeManager.getRoutePoint(pathIndex, i);
        }
        var marker1 = rp1['otherinfo']['marker'];
        if (marker1 != undefined) {
            var tLable = (i - 1).toString();
            var tName = marker1.attributes.name;
            var pointAttrib = tName.split(" ");
            pointAttrib[2] = i;
            marker1.data.name = marker1.attributes.name = pointAttrib[0] + " " + pointAttrib[1] + " " + pointAttrib[2];
            if (pointType != 'ROUTEDRAG') {
                marker1.style.label = tLable;
            }
        }
        rp1.routePointNo = i - 1;
    }
    if (pointType == 'ROUTEDRAG') {
        this.vectorLayerDragRouteMarker.redraw();
    }
    else {
        this.vectorLayerMarkers.redraw();
    }
}
IfxStpMap.prototype.updateAnnotationMarkers = function (pathIndex, segmentIndex, index) {
    var anntCnt = this.routeManager.getAnnotationCount(pathIndex, segmentIndex) - 1;
    for (var i = index; i <= anntCnt; i++) {
        var Annot = this.routeManager.getAnnotation(pathIndex, segmentIndex, i);
        var marker1 = Annot['otherinfo']['marker'];
        if (marker1 != undefined) {
            var tName = marker1.attributes.name;
            var pointAttrib = tName.split(" ");
            marker1.data.name = marker1.attributes.name = pointAttrib[0] + " " + pointAttrib[1] + " " + pointAttrib[2] + " " + i;
        }
    }
    this.vectorLayerMarkers.redraw();
}

IfxStpMap.prototype.deleteRoutePointByType = function (pointType) {
    var pathIndex = this.currentActiveRoutePathIndex;
    pointType = IfxStpmapCommon.getPointTypeID(pointType);
    if (pointType == 0 || pointType == 1) {
        var rp = this.routeManager.getRoutePoint(pathIndex, 0);
        if (rp != null && rp.pointType == pointType) {
            this.deleteRoutePoint(0, pathIndex);
        }
        else {
            rp = this.routeManager.getRoutePoint(pathIndex, 1);
            if (rp != null && rp.pointType == pointType) {
                this.deleteRoutePoint(1, pathIndex);
            }
        }
    }
}

IfxStpMap.prototype.deleteRoutePoint = function (indexToDel, pathIndex, callback) {
    if (pathIndex == null || pathIndex == undefined) {
        pathIndex = this.currentActiveRoutePathIndex;
    }

    var rp = this.routeManager.getRoutePoint(pathIndex, indexToDel);
    if (rp != undefined /*&& rp['otherinfo']['marker']*/) {
        var marker = rp['otherinfo']['marker'];
        this.vectorLayerMarkers.removeFeatures(marker);
        this.routeManager.removeRoutePoint(pathIndex, indexToDel);
        this.routeManager.removeAllRoutePoint(pathIndex, indexToDel);
        if (rp.pointType > 1)
            this.updateRoutePointMarkers(pathIndex, indexToDel, this.routeManager.getRoutePointCount(pathIndex) - 1);
    }

    if (callback && typeof (callback) === "function") {
        this.reactivateControls();
        this.setPageState('secondpointselected');
        callback();
    }
}

IfxStpMap.prototype.deleteAllRoutePoint = function (indexToDel, pathIndex, callback) {
    if (pathIndex == null || pathIndex == undefined) {
        pathIndex = this.currentActiveRoutePathIndex;
    }

    var rp = this.routeManager.getAllRoutePoint(pathIndex, indexToDel);
    if (rp != undefined /*&& rp['otherinfo']['marker']*/) {
        var marker = rp['otherinfo']['marker'];
        this.vectorLayerDragRouteMarker.removeFeatures(marker);
        this.routeManager.removeAllRoutePoint(pathIndex, indexToDel);
        if (rp.pointType > 1)
            this.updateRoutePointMarkers(pathIndex, indexToDel, this.routeManager.getAllRoutePointCount(pathIndex) - 1, 'ROUTEDRAG');
    }

    if (callback && typeof (callback) === "function") {
        this.reactivateControls();
        this.setPageState('secondpointselected');
        callback();
    }
}

IfxStpMap.prototype.moveRoutePointPos = function (from, to) {
    pathIndex = this.currentActiveRoutePathIndex;
    var frmIndex = from < to ? from : to;
    var toIndex = from < to ? to : from;
    this.routeManager.moveRoutePointPos(pathIndex, from + 1, to + 1);
    this.updateRoutePointMarkers(pathIndex, frmIndex + 1, toIndex + 1);
}

//function to plan the route
//if multiple paths are present, current active paths route points will be considered
IfxStpMap.prototype.planRoute = function (planReturn, callback) {
    var mainRoute = this.currentActiveRoutePathIndex;
    if (mainRoute == 0) {
        this.routeManager.RoutePart.Esdal2Broken = 1;   // check added for a route planing. Some times we can save a route with out planning(annotation create/move)
    }
    this.routeManager.RoutePart.IsAutoReplan = 0;
    if (this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex].routePathType != 0) {
        planReturn = false;
    }
    if (planReturn == true) {
        this.initialiseReturnRoutePart();
    }

    var routeRequest = this.routeManager.formatRouteRequest(this.currentActiveRoutePathIndex);
    var plannedRouteLinksCount = 0;

    var self = this;

    self.routeManager.setRoutePathState(self.currentActiveRoutePathIndex, 'routeplanninginprogress');
    if ((routeRequest.BeginPointLinkId == routeRequest.EndPointLinkId) && (routeRequest.WayPoints.length == 0)) { // code added by afsal #4728
        var rPath = self.routeManager.getRoutePath(self.currentActiveRoutePathIndex);
        var rpStart = jQuery.extend(true, {}, self.routeManager.getRoutePoint(self.currentActiveRoutePathIndex, 0));
        var rpEnd = jQuery.extend(true, {}, self.routeManager.getRoutePoint(self.currentActiveRoutePathIndex, 1));
        var rSegment = self.routeManager.createRouteSegmentObject(routeRequest.BeginPointLinkId, rpStart, rpEnd, null, rPath.routeSegmentList.length + 1, 'NORMAL');
        rPath.routeSegmentList.push(rSegment);
        self.drawPartial(routeRequest.BeginPointLinkId, function (features) {
            var DirectionFlag = features.data;
            var StartDir = rpStart.otherinfo.dir_travel;
            var EndDir = rpEnd.otherinfo.dir_travel;
            if ((DirectionFlag == 1) && ((StartDir == 'B' || StartDir == 'F') || (EndDir == 'B' || EndDir == 'F'))) { // direction from start to end
                self.RouteDrawSingleLink(features, rSegment, rPath, DirectionFlag, planReturn, self);
            }
            else if ((DirectionFlag == 0) && ((StartDir == 'B' || StartDir == 'T') || (EndDir == 'B' || EndDir == 'T'))) {  //direction from end to start
                self.RouteDrawSingleLink(features, rSegment, rPath, DirectionFlag, planReturn, self);
            }
            else if (StartDir == undefined && EndDir == undefined)     // the check is for planned single link id route opens first time then plans with out move any flag
            {
                self.RouteDrawSingleLink(features, rSegment, rPath, DirectionFlag, planReturn, self);
            }
            else {
                self.setCurrentPathState('secondpointselected');
                showNotificationMap("No route can be planned. Which may be due to legal restrictions on the route. Please change the start/end/way points and plan the route.");
            }
            if (callback && typeof (callback) === "function") {
                callback(true);
            }
        });

    }
    else {
        this.routeManager.doProcessPlanRouteRequest(routeRequest, function (routeLinkIds) {
            if (routeLinkIds != null && routeLinkIds.length > 0) {
                plannedRouteLinksCount = routeLinkIds.length;
                //add start and end route linkd ID if not present in the list
                var initialCount = routeLinkIds.length;

                var rPath = self.routeManager.getRoutePath(self.currentActiveRoutePathIndex);

                var rpStart = jQuery.extend(true, {}, self.routeManager.getRoutePoint(self.currentActiveRoutePathIndex, 0));
                if (rPath.routePathType != 4 && rpStart.linkId == routeLinkIds[0]) {
                    routeLinkIds.splice(0, 1);
                }
                else if (rPath.routePathType == 4 && rpStart.linkId != routeLinkIds[0]) {
                    routeLinkIds.splice(0, 0, rpStart.linkId);
                }

                var rpEnd = jQuery.extend(true, {}, self.routeManager.getRoutePoint(self.currentActiveRoutePathIndex, 1));
                if (rPath.routePathType != 4 && rpEnd.linkId == routeLinkIds[routeLinkIds.length - 1]) {
                    routeLinkIds.splice(routeLinkIds.length - 1, 1);
                }
                else if (rPath.routePathType == 4 && rpEnd.linkId != routeLinkIds[routeLinkIds.length - 1]) {
                    routeLinkIds.push(rpEnd.linkId);
                }

                if (rPath.routeSegmentList.length > 0) {
                    if (rPath.routeSegmentList[0].segmentNo == 1) {
                        if (IfxStpmapCommon.compareGeometries(rPath.routePointList[0].pointGeom.sdo_point, rPath.routeSegmentList[0].startPointGeometry.sdo_point)) {
                            rpStart.pointGeom = jQuery.extend(true, {}, rPath.routeSegmentList[0].endPointGeometry);
                        }
                        else {
                            rpStart.pointGeom = jQuery.extend(true, {}, rPath.routeSegmentList[0].startPointGeometry);
                        }
                    }
                    else {
                        if (IfxStpmapCommon.compareGeometries(rPath.routePointList[1].pointGeom.sdo_point, rPath.routeSegmentList[0].startPointGeometry.sdo_point)) {
                            rpEnd.pointGeom = jQuery.extend(true, {}, rPath.routeSegmentList[0].endPointGeometry);
                        }
                        else {
                            rpEnd.pointGeom = jQuery.extend(true, {}, rPath.routeSegmentList[0].startPointGeometry);
                        }
                    }
                }

                var rSegment = self.routeManager.createRouteSegmentObject(routeLinkIds, rpStart, rpEnd, null, rPath.routeSegmentList.length + 1, 'NORMAL');
                rPath.routeSegmentList.push(rSegment);
                self.searchFeaturesOfRouteLinkList(rSegment.routeLinkList, function (features) {
                    if (initialCount > 0) {
                        rSegment.otherinfo.features = features;
                        IfxStpmapCommon.createFeatureForRouteSegment(rSegment);
                        self.routeManager.updateRoutePointField(self.currentActiveRoutePathIndex, 0, 'direction', rSegment.startPointDirection);
                        self.routeManager.updateRoutePointField(self.currentActiveRoutePathIndex, 1, 'direction', rSegment.endPointDirection);
                        IfxStpmapCommon.createFeatureForWaypoints(rPath, rSegment, self);
                        self.updateDirectionOfRouteLinks(self.currentActiveRoutePathIndex, rPath.routeSegmentList.length - 1);

                        if (self.pageType != 'NOMAPDISPLAY' && planReturn == false) {
                            self.drawRoute(self.currentActiveRoutePathIndex, false);
                            if (self.routeManager.getSegmentCount(self.currentActiveRoutePathIndex) > 1)
                                self.routeManager.reArrangeRouteSegmentList(self.currentActiveRoutePathIndex);
                            //self.routeManager.setRoutePathState(self.currentActiveRoutePathIndex, 'routeplanned'); //set path state to routeplanned
                            self.olMap.zoomToExtent(self.vectorLayerRoute.getDataExtent());   //get extent after planning  HE #2319
                        }
                        else if (planReturn == true) {
                            self.setReturnRoutePart();
                        }
                        if (tempAnnotations.length > 0) {
                            self.getNearestRouteSegmentToRetainAnnotation();
                        }
                        self.routeManager.setRoutePathState(self.currentActiveRoutePathIndex, 'routeplanned'); //set path state to routeplanneds
                        self.routeManager.RoutePart.routePartDetails.routeType = 'planned';
                        if (callback && typeof (callback) === "function") {
                            callback(true);
                        }
                    }
                    else {
                        if (callback && typeof (callback) === "function") {
                            callback(false);
                        }
                        self.routeManager.setRoutePathState(self.currentActiveRoutePathIndex, 'secondpointselected');
                    }
                });
            }
            else {
                if ($("#hIs_NEN").val() == "true") {
                    if (planReturn == true) {
                        self.setReturnRoutePart();
                    }
                }
                if (callback && typeof (callback) === "function") {
                    callback(false);
                }
                self.routeManager.setRoutePathState(self.currentActiveRoutePathIndex, 'secondpointselected');
            }
        });
    }
}

IfxStpMap.prototype.illogicalWaypointIdentification = function () {
    if (this.routeManager.getRoutePathState(this.currentActiveRoutePathIndex) == 'routeplanned') {
        //check for illogical waypoints
        var rPointCount = this.routeManager.getAllRoutePointCount(this.currentActiveRoutePathIndex);
        var plannedRouteLinksCount = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex].routeSegmentList[0].routeLinkList.length;
        var pointPassed = true;
        for (var pointIndex = 2; pointIndex < rPointCount; pointIndex++) {
            if (pointPassed) {
                var rPoint = this.routeManager.getAllRoutePoint(this.currentActiveRoutePathIndex, pointIndex);
                if (rPoint.pointType != 9 && rPoint.otherinfo.func_class != "5") {
                    var alternateRouteRequest = this.routeManager.formatRouteRequest(this.currentActiveRoutePathIndex, true, pointIndex);
                    this.routeManager.doProcessPlanRouteRequest(alternateRouteRequest, function (alternateRouteLinkIds) {
                        if (plannedRouteLinksCount > alternateRouteLinkIds.length + 12) {
                            showNotification('System has identified illogically placed waypoint(s) on the route');
                            pointPassed = false;
                        }
                    });
                }
            }
        }
    }
}

IfxStpMap.prototype.setSegmentTypeToFeatures = function (routePath, segmentIndex) {
    var segType = routePath.routeSegmentList[segmentIndex].segmentType;
    for (var j = 0; j < routePath.routeSegmentList[segmentIndex].otherinfo.completefeatures.length; j++) {
        routePath.routeSegmentList[segmentIndex].otherinfo.completefeatures[j].data = segType;
    }
    return segType;
}

IfxStpMap.prototype.doProcessDrawRoute = function (routePath, drawMarkers) {
    for (var i = 0; i < routePath.routeSegmentList.length; i++) {
        var segType = this.setSegmentTypeToFeatures(routePath, i);
        if (routePath.routeSegmentList[i].otherinfo.completefeatures != undefined) {
            if (segType != 3) {
                if (routePath.routeSegmentList[i].routeLinkList.length == 0 && routePath.routeSegmentList[i].LinkGeometry != null) {
                    routePath.routeSegmentList[i].otherinfo.completefeatures = [];
                    routePath.routeSegmentList[i].otherinfo.completefeatures.push(new OpenLayers.Feature.Vector(IfxStpmapCommon.createLineGeometry(routePath.routeSegmentList[i].LinkGeometry)));
                    routePath.routeSegmentList[i].otherinfo.completefeatures[0].data = "NORMAL";
                }
                this.vectorLayerRoute.addFeatures(routePath.routeSegmentList[i].otherinfo.completefeatures);
            }
            else
            {
                this.vectorLayerOffRoad.addFeatures(routePath.routeSegmentList[i].otherinfo.completefeatures);
            }
        }

    }

    if (routePath.otherinfo && routePath.otherinfo.completefeatures.length > 0) {
        this.vectorLayerRoute.addFeatures(routePath.otherinfo.completefeatures);
    }

    if (drawMarkers == true) {
        //draw start/end/waypoint markers
        this.drawMarkers(routePath);
    }
}

IfxStpMap.prototype.drawMarkers = function (routePath) {
    //if (this.getPageState() == 'routedragged') {
    var rPointList = routePath.routePointList;
    //}
    //else {
    //    var rPointList = routePath.allRoutePointList;
    //}
    for (var i = 0; i < rPointList.length; i++) {
        var rp = rPointList[i];
        if (rp.pointGeom != null && rp.showRoutePoint == 1) {
            var marker = this.setMarker(IfxStpmapCommon.getPointTypeName(rp.pointType),
                rp.pointGeom.sdo_point.X,
                rp.pointGeom.sdo_point.Y,
                rp.routePointNo + 1, routePath.pathNo, routePath.routePathNo);
            if (rp.otherinfo == undefined) {
                rp.otherinfo = { completefeatures: [], features: [] };
            }
            //set marker in other info here
            rp.otherinfo.marker = marker;
        }
    }
}

IfxStpMap.prototype.drawRoute = function (index, drawMarkers) {
    if (index == -1) {
        for (var i = 0; i < this.routeManager.getRoutePathCount(); i++) {
            var rPath = this.routeManager.getRoutePath(i);
            this.currentActiveRoutePathIndex = i;
            this.doProcessDrawRoute(rPath, drawMarkers);
        }
    }
    else {
        var rPath = this.routeManager.getRoutePath(index);
        this.doProcessDrawRoute(rPath, drawMarkers);
    }
}

//add empty route path object
IfxStpMap.prototype.addRoutePath = function (routeType) {
    if (this.getCurrentPathState() != 'idle') {
        this.currentActiveRoutePathIndex = this.routeManager.addRoutePath(routeType) - 1;
        addtoselect('Path' + Number(this.currentActiveRoutePathIndex + 1));
        if (this.eventList['ADDPATH'] != undefined && typeof (this.eventList['ADDPATH']) === "function") {
            this.eventList['ADDPATH'](null);
        }
    }
}

IfxStpMap.prototype.clearAnnotations = function (routeAnnotationsList) {
    if (routeAnnotationsList != null) {
        for (var i = 0; i < routeAnnotationsList.length; i++) {
            this.vectorLayerMarkers.removeFeatures(routeAnnotationsList[i].otherinfo.marker);
        }
    }
    routeAnnotationsList = [];
}

IfxStpMap.prototype.clearRoutePart = function () {
    this.routeManager.clearRoutePart(true);
    this.currentActiveRoutePathIndex = 0;
}

IfxStpMap.prototype.removeRouteSegment = function (pathIndex, segmentIndex) {
    var rSegment = this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex];
    this.deleteAnnotationsofRouteSegment(pathIndex, segmentIndex);
    if (IfxStpmapCommon.getSegmentTypeName(rSegment.segmentType) != 'OFFROAD') {
        this.vectorLayerRoute.removeFeatures(rSegment.otherinfo.completefeatures);
    }
    else {
        this.vectorLayerOffRoad.removeFeatures(rSegment.otherinfo.completefeatures);
    }
    this.routeManager.removeRouteSegment(pathIndex, segmentIndex);
}

IfxStpMap.prototype.insertRouteSegment = function (pathIndex, segmentIndex, obj, removeAndInsert) {
    this.routeManager.insertRouteSegment(pathIndex, segmentIndex, obj, removeAndInsert);
}

IfxStpMap.prototype.clearSpecificRouteSegment = function (rSegment) {
    this.vectorLayerRoute.removeFeatures(rSegment.otherinfo.completefeatures);
}

IfxStpMap.prototype.clearAllRouteSegmentsInPath = function (rPath) {
    for (var i = 0; i < rPath.routeSegmentList.length; i++) {
        if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[i].segmentType) != 'OFFROAD') {
            this.vectorLayerRoute.removeFeatures(rPath.routeSegmentList[i].otherinfo.completefeatures);
        }
        else {
            this.vectorLayerOffRoad.removeFeatures(rPath.routeSegmentList[i].otherinfo.completefeatures);
        }
        if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[i].segmentType) == 'OFFROAD') {
            if (rPath.routeSegmentList[i].segmentNo == 1 &&
                (IfxStpmapCommon.compareGeometries(rPath.routePointList[0].pointGeom.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routePointList[0].pointGeom.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))) {
                continue;
            }
            else if (rPath.routeSegmentList[i].segmentNo == rPath.routeSegmentList.length &&
                (IfxStpmapCommon.compareGeometries(rPath.routePointList[1].pointGeom.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                    IfxStpmapCommon.compareGeometries(rPath.routePointList[1].pointGeom.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))) {
                continue;
            }
        }
        rPath.routeSegmentList[i].otherinfo.completefeatures = [];
        rPath.routeSegmentList[i].otherinfo.features = [];
        this.clearAnnotations(rPath.routeSegmentList[i].routeAnnotationsList);
    }
    //this.vectorLayerRoute.removeFeatures(rPath.otherinfo.completefeatures);
}

IfxStpMap.prototype.clearRoutePath = function (routePathIndex) {
    if (routePathIndex == undefined || routePathIndex == null) {
        routePathIndex = this.currentActiveRoutePathIndex;
    }

    if (this.routeManager.getRoutePathState(routePathIndex) == 'routeplanned' || this.routeManager.getRoutePathState(routePathIndex) == 'routedisplayed') {
        var rPath = this.routeManager.getRoutePath(routePathIndex);
        this.clearAllRouteSegmentsInPath(rPath);
        var segListLength = rPath.routeSegmentList.length;
        for (var i = segListLength - 1; i >= 0; i--) {
            if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[i].segmentType) == 'OFFROAD') {
                if (rPath.routeSegmentList[i].segmentNo == 1 &&
                    (IfxStpmapCommon.compareGeometries(rPath.routePointList[0].pointGeom.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                        IfxStpmapCommon.compareGeometries(rPath.routePointList[0].pointGeom.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))) {
                    continue;
                }
                else if (rPath.routeSegmentList[i].segmentNo == segListLength &&
                    (IfxStpmapCommon.compareGeometries(rPath.routePointList[1].pointGeom.sdo_point, rPath.routeSegmentList[i].startPointGeometry.sdo_point) ||
                        IfxStpmapCommon.compareGeometries(rPath.routePointList[1].pointGeom.sdo_point, rPath.routeSegmentList[i].endPointGeometry.sdo_point))) {
                    continue;
                }
            }
            this.routeManager.removeRouteSegment(routePathIndex, i);
        }
        this.routeManager.setRoutePathState(routePathIndex, 'secondpointselected');
        rPath.otherinfo.completefeatures = [];
    }
}

IfxStpMap.prototype.removeRoutePath = function (routePathIndex) {
    this.clearRoutePath(routePathIndex);

    var rPath = this.routeManager.getRoutePath(routePathIndex);
    if (rPath.routePointList && rPath.routePointList.length > 0) {
        for (var i = 0; i < rPath.routePointList.length; i++) {
            if (rPath.routePointList[i].showRoutePoint == 1 && this.vectorLayerMarkers.features.length > 0)
                this.vectorLayerMarkers.removeFeatures([rPath.routePointList[i]['otherinfo']['marker']]);
        }
    }
    if (rPath.allRoutePointList && rPath.allRoutePointList.length > 0) {
        for (var i = 0; i < rPath.allRoutePointList.length; i++) {
            if (this.vectorLayerDragRouteMarker.features.length > 0 &&
                rPath.allRoutePointList[i]['otherinfo']['marker'] != null && rPath.allRoutePointList[i]['otherinfo']['marker'] != undefined) {
                this.vectorLayerDragRouteMarker.removeFeatures([rPath.allRoutePointList[i]['otherinfo']['marker']]);
            }
        }
    }

    if (rPath.routePathType == 4) {
        if (this.advancedEditRouteDetails.terminalPoint == 0) {
            var rp = this.routeManager.getRoutePoint(this.advancedEditRouteDetails.pathIndex, 0);
            if (rp != undefined) {
                var marker = rp['otherinfo']['marker'];
                this.vectorLayerMarkers.addFeatures(marker);
            }
        }
        else if (this.advancedEditRouteDetails.terminalPoint == 1) {
            var rp = this.routeManager.getRoutePoint(this.advancedEditRouteDetails.pathIndex, 1);
            if (rp != undefined) {
                var marker = rp['otherinfo']['marker'];
                this.vectorLayerMarkers.addFeatures(marker);
            }
        }
        this.advancedEditRouteDetails = {
            pathIndex: -1,
            startSegmentIndex: -1,
            endSegmentIndex: -1,
            terminalPoint: -1
        };
    }

    this.routeManager.removeRoutePath(routePathIndex);
    this.currentActiveRoutePathIndex = 0;
}

IfxStpMap.prototype.getPathCount = function () {
    return this.routeManager.getRoutePathCount();
}

IfxStpMap.prototype.clearAllRoutes = function (deleteMainRoute) {
    this.routeManager.RoutePart.Esdal2Broken = 0;  //when cleared a route broken route status changed from unplanned 
    if (deleteMainRoute == true) {
        this.removeReturnLeg();
    }

    var routePartDetails = jQuery.extend(true, {}, this.routeManager.RoutePart.routePartDetails);

    var endCond = deleteMainRoute == true ? 0 : 1;//whether to delete the main route or not
    for (var i = this.routeManager.getRoutePathCount() - 1; i >= endCond; i--) {
        this.removeRoutePath(i);
    }
    this.routeManager.clearRoutePart(deleteMainRoute);
    if (deleteMainRoute == true) {
        this.routeManager.setRoutePathState(this.currentActiveRoutePathIndex, 'idle');
    }
    else {
        this.clearRoutePath(0);
        this.routeManager.setRoutePathState(this.currentActiveRoutePathIndex, 'secondpointselected');
    }

    this.routeManager.RoutePart.routePartDetails = routePartDetails;

    this.pageState = 'readyidle';
}

IfxStpMap.prototype.getPageType = function () {
    return this.pageType;
}
IfxStpMap.prototype.clearoffroad = function () {//path index set as 0 becuase always clear the main route
    objifxStpMap.vectorLayerOffRoad.removeFeatures(objifxStpMap.vectorLayerOffRoad.features);
    objifxStpMap.vectorLayerRoute.removeFeatures(objifxStpMap.vectorLayerRoute.features);

}

IfxStpMap.prototype.setPageType = function (type) {
    this.pageType = type;
}

IfxStpMap.prototype.getPageState = function () {
    return this.pageState;
}

IfxStpMap.prototype.setPageState = function (state) {
    this.pageState = state;
}

IfxStpMap.prototype.getCurrentPathState = function () {
    return this.routeManager.getRoutePathState(this.currentActiveRoutePathIndex);
}

IfxStpMap.prototype.setCurrentPathState = function (state) {
    return this.routeManager.setRoutePathState(this.currentActiveRoutePathIndex, state);
}

IfxStpMap.prototype.getcurrentSelectedRouteType = function () {
    return this.routeManager.getRoutePath(this.currentActiveRoutePathIndex).routePathType;
}

IfxStpMap.prototype.getFeaturesOfRoutePath = function (routePathIndex) {
    var features = [];
    for (var i = 0; i < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList.length; i++) {
        for (var j = 0; j < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].otherinfo.completefeatures.length; j++) {
            features.push(this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].otherinfo.completefeatures[j]);
        }
    }
    return features;
}

IfxStpMap.prototype.getFeaturesOfRouteSegment = function (routePathIndex, routeSegmentIndex) {
    return this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].otherinfo.completefeatures;
}

IfxStpMap.prototype.getNearestPathAndSegment = function (X, Y, bAll, isLonLat, type) {
    if (isLonLat != true) {
        var pixel = { x: X, y: Y };
        var lonlat = this.olMap.getLonLatFromPixel(pixel);
    }
    else {
        var lonlat = new OpenLayers.LonLat(X, Y)
    }
    var features;
    var retObject;
    var retObjectThis = { distance: 2500, pathIndex: -1, segmentIndex: -1, feature: '' };
    for (var pathIndex = 0; pathIndex < this.routeManager.RoutePart.routePathList.length; pathIndex++) {
        for (var segmentIndex = 0; segmentIndex < this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList.length; segmentIndex++) {
            if ((bAll == false && IfxStpmapCommon.getSegmentTypeName(this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].segmentType) != 'NORMAL') ||
                (type == 'offroad' && IfxStpmapCommon.getSegmentTypeName(this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].segmentType) != 'OFFROAD'))
                continue;
            features = [];
            features = this.getFeaturesOfRouteSegment(pathIndex, segmentIndex);
            if (features.length > 0) {
                retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
                if (retObject.distance < retObjectThis.distance) {
                    retObjectThis = retObject;
                    retObjectThis.pathIndex = pathIndex;
                    retObjectThis.segmentIndex = segmentIndex;
                    retObjectThis.feature = features[retObject.index];
                }
            }
        }
    }
    return retObjectThis;
}
//Function to identify adjacent route segments to annotation in case of route redraw
IfxStpMap.prototype.getNearestRouteSegmentToRetainAnnotation = function () {
    for (var tempAnnotCount = 0; tempAnnotCount < tempAnnotations.length; tempAnnotCount++) {
        var lonlat = new OpenLayers.LonLat(tempAnnotations[tempAnnotCount].geometry.sdo_point.X, tempAnnotations[tempAnnotCount].geometry.sdo_point.Y);

        var features;
        var retObject;
        var retObjectThis = { distance: 100, pathIndex: -1, segmentIndex: -1, feature: '' };
        for (var pathIndex = 0; pathIndex < this.routeManager.RoutePart.routePathList.length; pathIndex++) {
            for (var segmentIndex = 0; segmentIndex < this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList.length; segmentIndex++) {
                features = [];
                features = this.getFeaturesOfRouteSegment(pathIndex, segmentIndex);
                if (features.length > 0) {
                    retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
                    if (retObject.distance < retObjectThis.distance) {
                        retObjectThis = retObject;
                        retObjectThis.pathIndex = pathIndex;
                        retObjectThis.segmentIndex = segmentIndex;
                        retObjectThis.feature = features[retObject.index];
                    }
                }
            }
        }
        if (retObjectThis.pathIndex != -1) {
            this.routeManager.RoutePart.routePathList[retObjectThis.pathIndex].routeSegmentList[retObjectThis.segmentIndex].routeAnnotationsList.push(tempAnnotations[tempAnnotCount]);
            this.drawAnnotationsForSegment(this.routeManager.RoutePart.routePathList[retObjectThis.pathIndex], retObjectThis.pathIndex, retObjectThis.segmentIndex)
        }
    }
    tempAnnotations = [];

}

IfxStpMap.prototype.divideRouteSegment = function (pathIndex, segmentIndex, startPoint, endPoint, endSegmentIndex) {
    var rPath = this.routeManager.getRoutePath(pathIndex);

    if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) == 'OVERRIDE') {
        var features1 = this.searchFeaturesBetweenLinkNo(0, startPoint.linkNo - 1, pathIndex, segmentIndex);
        var features2 = this.searchFeaturesBetweenLinkNo(endPoint.linkNo - 1, rPath.routeSegmentList[segmentIndex].routeLinkList.length - 1, pathIndex, segmentIndex);
    }
    else {
        var features1 = this.searchFeaturesBetweenLinkNo(0, startPoint.linkNo - 2, pathIndex, segmentIndex);
        var features2 = this.searchFeaturesBetweenLinkNo(endPoint.linkNo, rPath.routeSegmentList[segmentIndex].routeLinkList.length - 1, pathIndex, segmentIndex);
    }

    var tempSegment = jQuery.extend(true, {}, rPath.routeSegmentList[segmentIndex]);
    var tempRouteLinkLength = rPath.routeSegmentList[segmentIndex].routeLinkList.length;

    var rLinkList = [];
    var direction = [];
    var annotObj = [];

    if (!IfxStpmapCommon.compareGeometries(tempSegment.startPointGeometry.sdo_point, startPoint.pointGeom.sdo_point)) {
        this.clearSpecificRouteSegment(rPath.routeSegmentList[segmentIndex]);

        for (var i = 0; i < startPoint.linkNo - 1; i++) {
            rLinkList.push(tempSegment.routeLinkList[i].linkId);
            direction.push(tempSegment.routeLinkList[i].direction);
        }

        if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) == 'OVERRIDE') {
            rLinkList.push(startPoint.linkId);
            direction.push(startPoint.direction);
        }
        if (tempSegment.routeAnnotationsList != null) {
            for (var i = 0; i < tempSegment.routeAnnotationsList.length; i++) {
                if (tempSegment.startLinkId == tempSegment.routeAnnotationsList[i].linkId)
                    annotObj.push(tempSegment.routeAnnotationsList[i]);
                for (var j = 0; j < rLinkList.length; j++) {
                    if (rLinkList[j] == tempSegment.routeAnnotationsList[i].linkId)
                        annotObj.push(tempSegment.routeAnnotationsList[i]);
                }
            }
        }

        rpStart = {
            linkId: tempSegment.startLinkId,
            pointGeom: tempSegment.startPointGeometry,
            lrs: tempSegment.startLrs,
            direction: tempSegment.startPointDirection,
            otherinfo: { pointfeature: tempSegment.otherinfo.startSegmentfeature }
        };
        if (startPoint.lrs == 0 || startPoint.lrs == 1 || IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) != 'OFFROAD') {
            startPoint.linkId = rLinkList[rLinkList.length - 1];
        }
        //else {
        //    startPoint.lrs = IfxStpmapCommon.getLRSLength(startPoint.otherinfo.pointfeature.geometry, new OpenLayers.Geometry.Point(startPoint.pointGeom.sdo_point.X, startPoint.pointGeom.sdo_point.Y));
        //}
        rpEnd = startPoint;

        var rSegment = this.routeManager.createRouteSegmentObject(rLinkList, rpStart, rpEnd, null, tempSegment.segmentNo, IfxStpmapCommon.getSegmentTypeName(tempSegment.segmentType), direction);
        rSegment.otherinfo.features = features1;
        this.insertRouteSegment(pathIndex, segmentIndex, rSegment, true);
        if (rSegment.routeLinkList.length != 0 || startPoint.linkNo == 1) {
            IfxStpmapCommon.createFeatureForRouteSegment(rSegment);
            this.vectorLayerRoute.addFeatures(rSegment.otherinfo.completefeatures);
        }
        else {
            rSegment.endPointDirection = rpStart.direction;
            var startFeature = IfxStpmapCommon.getPartialFeature(rSegment.otherinfo.startSegmentfeature, rSegment);
            IfxStpmapCommon.createPartialFeatureForRouteSegment(startFeature, rpStart.direction, rpEnd.pointGeom.sdo_point.X, rpEnd.pointGeom.sdo_point.Y, rSegment, 'STARTPOINT');
            this.vectorLayerRoute.addFeatures(rSegment.otherinfo.completefeatures);
        }

        for (var i = 0; i < annotObj.length; i++) {
            this.addAnnotation(pathIndex, segmentIndex, -1, annotObj[i]);
        }
    }
    else {
        this.removeRouteSegment(pathIndex, segmentIndex);
    }

    if (!IfxStpmapCommon.compareGeometries(tempSegment.endPointGeometry.sdo_point, endPoint.pointGeom.sdo_point)) {
        rLinkList = [];
        direction = [];
        annotObj = [];

        if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) == 'OVERRIDE') {
            rLinkList.push(endPoint.linkId);
            direction.push(endPoint.direction);
        }

        for (var i = endPoint.linkNo; i < tempSegment.routeLinkList.length; i++) {
            rLinkList.push(tempSegment.routeLinkList[i].linkId);
            direction.push(tempSegment.routeLinkList[i].direction);
        }
        if (tempSegment.routeAnnotationsList != null) {
            for (var i = 0; i < tempSegment.routeAnnotationsList.length; i++) {
                if (tempSegment.endLinkId == tempSegment.routeAnnotationsList[i].linkId)
                    annotObj.push(tempSegment.routeAnnotationsList[i]);
                for (var j = 0; j < rLinkList.length; j++) {
                    if (rLinkList[j] == tempSegment.routeAnnotationsList[i].linkId)
                        annotObj.push(tempSegment.routeAnnotationsList[i]);
                }
            }
        }

        if (endPoint.lrs == 0 || endPoint.lrs == 1 || IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) != 'OFFROAD') {
            endPoint.linkId = rLinkList[0];
        }
        //else {
        //    endPoint.lrs = IfxStpmapCommon.getLRSLength(endPoint.otherinfo.pointfeature.geometry, new OpenLayers.Geometry.Point(endPoint.pointGeom.sdo_point.X, endPoint.pointGeom.sdo_point.Y));
        //}
        rpStart = endPoint;
        rpEnd = {
            linkId: tempSegment.endLinkId,
            pointGeom: tempSegment.endPointGeometry,
            lrs: tempSegment.endLrs,
            direction: tempSegment.endPointDirection,
            otherinfo: { pointfeature: tempSegment.otherinfo.endSegmentfeature }
        };

        var rSegment = this.routeManager.createRouteSegmentObject(rLinkList, rpStart, rpEnd, null, endSegmentIndex + 2, IfxStpmapCommon.getSegmentTypeName(tempSegment.segmentType), direction);
        rSegment.otherinfo.features = features2;
        this.insertRouteSegment(pathIndex, endSegmentIndex + 1, rSegment, false);

        if (rSegment.routeLinkList.length != 0 || endPoint.linkNo == tempRouteLinkLength) {
            IfxStpmapCommon.createFeatureForRouteSegment(rSegment);
            this.vectorLayerRoute.addFeatures(rSegment.otherinfo.completefeatures);
        }
        else {
            rSegment.startPointDirection = rpEnd.direction;
            var endFeature = IfxStpmapCommon.getPartialFeature(rSegment.otherinfo.startSegmentfeature, rSegment);
            IfxStpmapCommon.createPartialFeatureForRouteSegment(endFeature, rpEnd.direction, rpEnd.pointGeom.sdo_point.X, rpEnd.pointGeom.sdo_point.Y, rSegment, 'ENDPOINT');
            this.vectorLayerRoute.addFeatures(rSegment.otherinfo.completefeatures);
        }

        for (var i = 0; i < annotObj.length; i++) {
            this.addAnnotation(pathIndex, rPath.routeSegmentList.length - 1, -1, annotObj[i]);
        }
    }

    this.routeManager.reArrangeRouteSegmentList(pathIndex);

    this.clearAnnotationsofRoutePath(pathIndex);
    this.drawAnnotations(rPath, pathIndex);

    if (this.routeManager.getRoutePathState(pathIndex) == 'routedisplayed') {
        this.routeManager.setRoutePathState(this.currentActiveRoutePathIndex, 'routeplanned');
        if (this.eventList['PATHSTATECHANGED'] != undefined && typeof (this.eventList['PATHSTATECHANGED']) === "function") {
            this.eventList['PATHSTATECHANGED']();
        }
    }
}

IfxStpMap.prototype.searchRouteLinkByLinkID = function (routePathIndex, routeSegmentIndex, linkId) {
    for (var i = 0; i < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].routeLinkList.length; i++) {
        if (this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].routeLinkList[i].linkId == linkId) {
            return this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].routeLinkList[i];
        }
    }
}

IfxStpMap.prototype.searchRouteLinkAndSegmentByLinkID = function (routePathIndex, routeSegmentIndex, linkId) {
    for (i = 0; i < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList.length; i++) {
        if (this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].routeLinkList != null) {
            for (var j = 0; j < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].routeLinkList.length; j++) {
                if (this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].routeLinkList[j].linkId == linkId ||
                    this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].startLinkId == linkId ||
                    this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].endLinkId == linkId) {
                    return {
                        link: this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].routeLinkList[j],
                        segmentIndex: i
                    };
                }
            }
        }
    }
    return {
        link: { linkId: linkId, linkNo: 0, direction: 0 },
        segmentIndex: 0
    };
}

IfxStpMap.prototype.searchFeatureInRouteByLinkId = function (routePathIndex, routeSegmentIndex, linkId) {
    for (var i = 0; i < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].otherinfo.completefeatures.length; i++) {
        if (this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].otherinfo.completefeatures[i].attributes.LINK_ID == linkId) {
            return this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex].otherinfo.completefeatures[i];
        }
    }
}

IfxStpMap.prototype.searchFeaturesBetweenLinkNo = function (startLinkNo, endLinkNo, routePathIndex, routeSegmentIndex) {
    var rSegment = this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex];
    var features = [];
    for (var i = startLinkNo; i <= endLinkNo; i++) {
        for (var j = 0; j < rSegment.otherinfo.completefeatures.length; j++) {
            if (rSegment.routeLinkList[i].linkId == rSegment.otherinfo.completefeatures[j].attributes.LINK_ID) {
                features.push(rSegment.otherinfo.completefeatures[j]);
            }
        }
    }
    return features;
}

IfxStpMap.prototype.drawManoeuvreSet = function (e, type) {
    var rPath = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex];
    if (rPath.routeSegmentList[rPath.routeSegmentList.length - 1].direction == 1 && rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.isComplete == false) {
        this.drawManoeuvre(e, type);
    }
    else {
        var self = this;
        if (this.eventList['MANOEUVRESELECT'] != undefined && typeof (this.eventList['MANOEUVRESELECT']) === "function") {
            this.eventList['MANOEUVRESELECT'](e, function (result) {
                self.drawManoeuvre(e, type);
            });
        }
    }
}

IfxStpMap.prototype.drawManoeuvre = function (e, type) {
    var self = this;
    self.searchFeaturesByXY(e.xy.x, e.xy.y, false, self.boundaryOffset, function (features) {
        if (features == null || features.length <= 0) {
            return;
        }
        else {
            self.setManoeuvreSegment(self.currentActiveRoutePathIndex, features, e.xy.x, e.xy.y, type);
        }
    });
}

IfxStpMap.prototype.setManoeuvreSegment = function (routePathIndex, features, X, Y, type) {
    var rPath = this.routeManager.RoutePart.routePathList[routePathIndex];
    var lonlat = this.olMap.getLonLatFromPixel({ x: X, y: Y });
    var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, lonlat.lon, lonlat.lat);
    var selectFeature = features[retObject.index];

    if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) != 'NORMAL'
        && IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) != 'OFFROAD'
        && rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.isComplete == false);
    else {
        var rSegment = this.routeManager.createRouteSegmentObject(null, null, null, null, rPath.routeSegmentList.length + 1, type);
        rPath.routeSegmentList.push(rSegment);

        if (this.eventList['MANOEUVREADDED'] != undefined && typeof (this.eventList['MANOEUVREADDED']) === "function") {
            var pix = this.olMap.getPixelFromLonLat(lonlat);
            this.eventList['MANOEUVREADDED'](pix);
        }
    }

    if (rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.completefeatures.length > 0)
        var continuity = this.checkContinuity(selectFeature, rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.completefeatures,
            rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType, rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.endnode);
    if (rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.completefeatures.length == 0 || continuity != false) {
        selectFeature.data = type;
        this.vectorLayerRoute.addFeatures(selectFeature);
        rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.startfrom = selectFeature;
        rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.endnode = continuity;
        rPath.routeSegmentList[rPath.routeSegmentList.length - 1].otherinfo.completefeatures.push(selectFeature);
    }
    else {
        showNotification('Select a continuous road');
    }
}

IfxStpMap.prototype.checkContinuity = function (selectFeature, routeSegmentFeatures, segmentType, endNode) {
    if (IfxStpmapCommon.getSegmentTypeName(segmentType) == 'OVERRIDE') {
        for (var i = 0; i < routeSegmentFeatures.length; i++) {
            if (routeSegmentFeatures[i].attributes.REF_IN_ID == selectFeature.attributes.REF_IN_ID || routeSegmentFeatures[i].attributes.REF_IN_ID == selectFeature.attributes.NREF_IN_ID ||
                routeSegmentFeatures[i].attributes.NREF_IN_ID == selectFeature.attributes.REF_IN_ID || routeSegmentFeatures[i].attributes.NREF_IN_ID == selectFeature.attributes.NREF_IN_ID) {
                return true;
            }
        }
    }
    else {
        if (endNode == 'REF_NODE') {
            if (routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.REF_IN_ID == selectFeature.attributes.REF_IN_ID)
                return 'NREF_NODE';
            else if (routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.REF_IN_ID == selectFeature.attributes.NREF_IN_ID)
                return 'REF_NODE';
        }
        else if (endNode == 'NREF_NODE') {
            if (routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.NREF_IN_ID == selectFeature.attributes.REF_IN_ID)
                return 'NREF_NODE';
            else if (routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.NREF_IN_ID == selectFeature.attributes.NREF_IN_ID)
                return 'REF_NODE';
        }
        else {
            if (routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.REF_IN_ID == selectFeature.attributes.REF_IN_ID ||
                routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.NREF_IN_ID == selectFeature.attributes.REF_IN_ID)
                return 'NREF_NODE';
            else if (routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.REF_IN_ID == selectFeature.attributes.NREF_IN_ID ||
                routeSegmentFeatures[routeSegmentFeatures.length - 1].attributes.NREF_IN_ID == selectFeature.attributes.NREF_IN_ID)
                return 'REF_NODE';
        }
    }
    return false;
}

IfxStpMap.prototype.updateManoeuvreRouteSegment = function (routePathIndex) {
    toolbarPanel4.div.style.display = "none";
    var routeSegmentIndex = this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList.length - 1;
    var rSegment = this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[routeSegmentIndex];
    if (rSegment.otherinfo.isAdded && !rSegment.otherinfo.isComplete) {
        rSegment.otherinfo.isComplete = true;
        var linkIds = [];
        for (var i = 0; i < rSegment.otherinfo.completefeatures.length; i++) {
            linkIds[i] = rSegment.otherinfo.completefeatures[i].attributes.LINK_ID;
        }
        rSegment.routeLinkList = this.routeManager.createRouteLinkList(linkIds, 1);
        rSegment.otherinfo.isComplete = true;

        rSegment.startLinkId = linkIds[0];
        rSegment.endLinkId = linkIds[linkIds.length - 1];

        var geom = this.findEndGeometriesOfManoeuvre(rSegment);
        rSegment.startPointGeometry.sdo_point = { X: geom.startGeom.x, Y: geom.startGeom.y };
        rSegment.endPointGeometry.sdo_point = { X: geom.endGeom.x, Y: geom.endGeom.y };

        rSegment.otherinfo.startSegmentfeature = jQuery.extend(true, {}, rSegment.otherinfo.completefeatures[0]);
        rSegment.otherinfo.endSegmentfeature = jQuery.extend(true, {}, rSegment.otherinfo.completefeatures[rSegment.otherinfo.completefeatures.length - 1]);

        this.updateDirectionOfRouteLinks(routePathIndex, routeSegmentIndex);

        rSegment.startPointDirection = rSegment.routeLinkList[0].direction;
        rSegment.endPointDirection = rSegment.routeLinkList[rSegment.routeLinkList.length - 1].direction;

        var lrsMeasure = LRSMeasure(rSegment.otherinfo.startSegmentfeature.geometry, new OpenLayers.Geometry.Point(rSegment.startPointGeometry.sdo_point.X, rSegment.startPointGeometry.sdo_point.Y), { tolerance: 0.5, details: true });
        rSegment.startLrs = Math.round(lrsMeasure.length);
        var endlrsMeasure = LRSMeasure(rSegment.otherinfo.endSegmentfeature.geometry, new OpenLayers.Geometry.Point(rSegment.endPointGeometry.sdo_point.X, rSegment.endPointGeometry.sdo_point.Y), { tolerance: 0.5, details: true });
        rSegment.endLrs = Math.round(endlrsMeasure.length);

        if (IfxStpmapCommon.getSegmentTypeName(rSegment.segmentType) == 'OVERRIDE') {
            var e = { xy: { x: geom.startGeom.x, y: geom.startGeom.y } };
            this.cutRoute(e, true, geom.startGeom, false);
        }


        this.routeManager.reArrangeRouteSegmentList(routePathIndex);


    }
}

IfxStpMap.prototype.findEndGeometriesOfManoeuvre = function (rSegment) {
    var midLinks = rSegment.otherinfo.completefeatures;

    if (IfxStpmapCommon.getSegmentTypeName(rSegment.segmentType) != 'OVERRIDE') {
        var endLinks = [rSegment.otherinfo.completefeatures[0], rSegment.otherinfo.completefeatures[rSegment.otherinfo.completefeatures.length - 1]];

        var startGeom, endGeom;

        if (endLinks[0].attributes.LINK_ID == endLinks[1].attributes.LINK_ID) {
            startGeom = jQuery.extend(true, {}, endLinks[0].geometry.components[0]);
            endGeom = jQuery.extend(true, {}, endLinks[0].geometry.components[endLinks[0].geometry.components.length - 1]);
        }

        else {
            var startNodeFlag = endNodeFlag = 0;

            for (var i = 0; i < midLinks.length; i++) {
                if ((endLinks[0] != midLinks[i]) && (endLinks[0].attributes.REF_IN_ID == midLinks[i].attributes.REF_IN_ID || endLinks[0].attributes.REF_IN_ID == midLinks[i].attributes.NREF_IN_ID)) {
                    startNodeFlag = 1;
                    break;
                }
            }
            for (var i = 0; i < midLinks.length; i++) {
                if ((endLinks[1] != midLinks[i]) && (endLinks[1].attributes.REF_IN_ID == midLinks[i].attributes.REF_IN_ID || endLinks[1].attributes.REF_IN_ID == midLinks[i].attributes.NREF_IN_ID)) {
                    endNodeFlag = 1;
                    break;
                }
            }

            if (startNodeFlag == 0) {
                startGeom = jQuery.extend(true, {}, endLinks[0].geometry.components[0]);
            }
            else {
                startGeom = jQuery.extend(true, {}, endLinks[0].geometry.components[endLinks[0].geometry.components.length - 1]);
            }
            if (endNodeFlag == 0) {
                endGeom = jQuery.extend(true, {}, endLinks[1].geometry.components[0]);
            }
            else {
                endGeom = jQuery.extend(true, {}, endLinks[1].geometry.components[endLinks[1].geometry.components.length - 1]);
            }
        }

        return { startGeom: startGeom, endGeom: endGeom };
    }
    else {
        var retObj;
        var rPath = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex];
        for (var i = 0; i < midLinks.length; i++) {
            for (var j = 0; j < rPath.routeSegmentList.length - 1; j++) {
                retObj = IfxStpmapCommon.findNearestFeatureIndex(rPath.routeSegmentList[j].otherinfo.completefeatures, midLinks[i].geometry.components[0].x, midLinks[i].geometry.components[0].y);
                if (retObj.distance == 0) {
                    return { startGeom: midLinks[i].geometry.components[0], endGeom: midLinks[i].geometry.components[0] };
                }
                retObj = IfxStpmapCommon.findNearestFeatureIndex(rPath.routeSegmentList[j].otherinfo.completefeatures,
                    midLinks[i].geometry.components[midLinks[i].geometry.components.length - 1].x, midLinks[i].geometry.components[midLinks[i].geometry.components.length - 1].y);
                if (retObj.distance == 0) {
                    return { startGeom: midLinks[i].geometry.components[midLinks[i].geometry.components.length - 1], endGeom: midLinks[i].geometry.components[midLinks[i].geometry.components.length - 1] };
                }
            }
        }
    }
}

IfxStpMap.prototype.removeFeaturesFromRoutePath = function (routePathIndex, linkId) {
    var features = [];
    for (var i = 0; i < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList.length; i++) {
        for (var j = 0; j < this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].otherinfo.completefeatures.length; j++) {
            if (this.routeManager.RoutePart.routePathList[routePathIndex].routeSegmentList[i].otherinfo.completefeatures[j].attributes.LINK_ID == linkId) {
                this.rearrangeFeaturesOfRouteSegment(routePathIndex, i, j);
                j--;
            }
        }
    }
}

IfxStpMap.prototype.rearrangeFeaturesOfRouteSegment = function (pathIndex, segmentIndex, from) {
    for (var i = from; i < this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].otherinfo.completefeatures.length; i++) {
        this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].otherinfo.completefeatures[i] = this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].otherinfo.completefeatures[i + 1];
    }
    this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].otherinfo.completefeatures.length--;
}

IfxStpMap.prototype.removeFeaturesFromRoute = function (features) {
    this.vectorLayerRoute.removeFeatures(features);
}

IfxStpMap.prototype.getTerminalPoints = function (pathIndex, segmentIndex) {
    var terminalPoints = [];
    var rPath = this.routeManager.RoutePart.routePathList[pathIndex];

    for (var i = 0; i < rPath.routeSegmentList.length; i++) {
        if (i != segmentIndex) {
            if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[i].segmentType) == 'OFFROAD') {
                terminalPoints.push(new OpenLayers.Geometry.Point(rPath.routeSegmentList[i].offRoadGeometry.OrdinatesArray[0], rPath.routeSegmentList[i].offRoadGeometry.OrdinatesArray[1]));
                terminalPoints.push(new OpenLayers.Geometry.Point(rPath.routeSegmentList[i].offRoadGeometry.OrdinatesArray[rPath.routeSegmentList[i].offRoadGeometry.OrdinatesArray.length - 2], rPath.routeSegmentList[i].offRoadGeometry.OrdinatesArray[rPath.routeSegmentList[i].offRoadGeometry.OrdinatesArray.length - 1]));
            }
            else {
                terminalPoints.push(new OpenLayers.Geometry.Point(rPath.routeSegmentList[i].startPointGeometry.sdo_point.X, rPath.routeSegmentList[i].startPointGeometry.sdo_point.Y));
                terminalPoints.push(new OpenLayers.Geometry.Point(rPath.routeSegmentList[i].endPointGeometry.sdo_point.X, rPath.routeSegmentList[i].endPointGeometry.sdo_point.Y));
            }
        }
    }

    return terminalPoints;
}

IfxStpMap.prototype.getIntersectionFeatures = function (pathIndex, segmentIndex, terminalPoints) {
    var intersectionFeatures = [];
    var rPath = this.routeManager.RoutePart.routePathList[pathIndex];

    for (var i = 0; i < rPath.routeSegmentList[segmentIndex].otherinfo.completefeatures.length; i++) {
        for (var j = 0; j < terminalPoints.length; j++) {
            var intersect = IfxStpmapCommon.findNearestFeatureIndex([rPath.routeSegmentList[segmentIndex].otherinfo.completefeatures[i]], terminalPoints[j].x, terminalPoints[j].y);
            if (intersect.distance < 1) {
                intersectionFeatures[intersectionFeatures.length] = { feature: '', point: '', terminal: '' };
                intersectionFeatures[intersectionFeatures.length - 1].feature = rPath.routeSegmentList[segmentIndex].otherinfo.completefeatures[i];
                intersectionFeatures[intersectionFeatures.length - 1].point = terminalPoints[j];
                intersectionFeatures[intersectionFeatures.length - 1].terminal = j;
            }
        }
    }

    return intersectionFeatures;
}

IfxStpMap.prototype.getNextIntersectionLink = function (linkNo, intersectionFeatures, nearestPath) {
    for (var i = 0; i < intersectionFeatures.length; i++) {
        link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, intersectionFeatures[i].feature.attributes.LINK_ID);
        if (link.linkNo == linkNo + 1) {
            return { linkNo: link.linkNo, index: i };
        }
    }
    return { linkNo: linkNo, index: -2 };
}

IfxStpMap.prototype.getPreviousIntersectionLink = function (linkNo, intersectionFeatures, nearestPath) {
    for (var i = 0; i < intersectionFeatures.length; i++) {
        link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, intersectionFeatures[i].feature.attributes.LINK_ID);
        if (link.linkNo == linkNo - 1) {
            return { linkNo: link.linkNo, index: i };
        }
    }
    return { linkNo: linkNo, index: -2 };
}

IfxStpMap.prototype.getNearestIntersectionLinks = function (nearestPath, intersectionFeatures) {
    var rSegment = this.routeManager.RoutePart.routePathList[nearestPath.pathIndex].routeSegmentList[nearestPath.segmentIndex];
    var clickedLink = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, nearestPath.feature.attributes.LINK_ID);
    var link;
    var near1 = { linkNo: 0, index: -2 };
    var near2 = { linkNo: rSegment.routeLinkList.length + 1, index: -2 };
    var temp = { linkNo: -1, index: -2 };

    var intersectionSegment = this.routeManager.RoutePart.routePathList[nearestPath.pathIndex].routeSegmentList[this.routeManager.RoutePart.routePathList[nearestPath.pathIndex].routeSegmentList.length - 1];
    if (intersectionFeatures.length == 2 && intersectionSegment != undefined && IfxStpmapCommon.getSegmentTypeName(intersectionSegment.segmentType) == 'OVERRIDE') {
        var link1 = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, intersectionFeatures[0].feature.attributes.LINK_ID);
        var link2 = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, intersectionFeatures[1].feature.attributes.LINK_ID);
        if (link1 != undefined && link2 != undefined) {
            if (link1.linkNo < link2.linkNo) {
                near1.linkNo = link1.linkNo;
                near1.index = 0;
                near2.linkNo = link2.linkNo;
                near2.index = 1;
            }
            else {
                near1.linkNo = link2.linkNo;
                near1.index = 1;
                near2.linkNo = link1.linkNo;
                near2.index = 0;
            }
        }
        return { near1: near1, near2: near2 };
    }

    if (clickedLink == undefined) {
        if (nearestPath.feature.attributes.LINK_ID == rSegment.startLinkId)
            clickedLink = { linkNo: 0 };
        else if (nearestPath.feature.attributes.LINK_ID == rSegment.endLinkId)
            clickedLink = { linkNo: rSegment.routeLinkList.length + 1 };
        else
            return null;
    }

    for (var i = 0; i < intersectionFeatures.length; i++) {
        link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, intersectionFeatures[i].feature.attributes.LINK_ID);
        if (link == undefined || link == null) {
            if (intersectionFeatures[i].feature.attributes.LINK_ID == rSegment.startLinkId &&
                near1.linkNo == 0 && clickedLink.linkNo >= 0) {
                var bool = IfxStpmapCommon.findIsNearestWithDirection(rSegment, nearestPath, intersectionFeatures[i], intersectionFeatures[near1.index], this, 1);
                if (bool) {
                    near1.linkNo = 0;
                    near1.index = i;
                }
            }
            else if (intersectionFeatures[i].feature.attributes.LINK_ID == rSegment.endLinkId &&
                clickedLink.linkNo == rSegment.routeLinkList.length + 1) {
                var bool = IfxStpmapCommon.findIsNearestWithDirection(rSegment, nearestPath, intersectionFeatures[i], intersectionFeatures[near1.index], this, 1);
                if (bool) {
                    near1.linkNo = rSegment.routeLinkList.length + 1;
                    near1.index = i;
                }
            }
        }
        else if (link.linkNo >= near1.linkNo) {
            if (link.linkNo == clickedLink.linkNo) {
                var bool = IfxStpmapCommon.findIsNearestWithDirection(rSegment, nearestPath, intersectionFeatures[i], intersectionFeatures[temp.index], this, 1);
                if (bool) {
                    temp.linkNo = link.linkNo;
                    temp.index = i;
                }
            }
            if (link.linkNo == near1.linkNo) {
                var bool = IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeatures[i].point, intersectionFeatures[near1.index].point);
                if (bool) {
                    near1.linkNo = link.linkNo;
                    near1.index = i;
                }
            }
            else if (link.linkNo < clickedLink.linkNo) {
                near1.linkNo = link.linkNo;
                near1.index = i;
            }
        }
    }

    if (near1.linkNo < temp.linkNo && temp.linkNo <= clickedLink.linkNo) {
        near1 = jQuery.extend(true, {}, temp);
    }
    temp.linkNo = -2;
    temp.index = -2;

    for (var i = 0; i < intersectionFeatures.length; i++) {
        link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, intersectionFeatures[i].feature.attributes.LINK_ID);
        if (link == undefined || link == null) {
            if (intersectionFeatures[i].feature.attributes.LINK_ID == rSegment.endLinkId &&
                near2.linkNo == rSegment.routeLinkList.length + 1 && clickedLink.linkNo <= rSegment.routeLinkList.length + 1) {
                var bool = IfxStpmapCommon.findIsNearestWithDirection(rSegment, nearestPath, intersectionFeatures[i], intersectionFeatures[near2.index], this, 2);
                if (bool) {
                    near2.linkNo = rSegment.routeLinkList.length + 1;
                    near2.index = i;
                }
            }
            else if (intersectionFeatures[i].feature.attributes.LINK_ID == rSegment.startLinkId &&
                clickedLink.linkNo == 0) {
                var bool = IfxStpmapCommon.findIsNearestWithDirection(rSegment, nearestPath, intersectionFeatures[i], intersectionFeatures[near2.index], this, 2);
                if (bool) {
                    near2.linkNo = 0;
                    near2.index = i;
                }
            }
        }
        else if (link.linkNo <= near2.linkNo) {
            if (link.linkNo == clickedLink.linkNo) {
                var bool = IfxStpmapCommon.findIsNearestWithDirection(rSegment, nearestPath, intersectionFeatures[i], intersectionFeatures[temp.index], this, 2);
                if (bool) {
                    temp.linkNo = link.linkNo;
                    temp.index = i;
                }
            }
            if (link.linkNo == near2.linkNo) {
                var bool = IfxStpmapCommon.findIsNearest(nearestPath.x1, nearestPath.y1, intersectionFeatures[i].point, intersectionFeatures[near2.index].point);
                if (bool) {
                    near2.linkNo = link.linkNo;
                    near2.index = i;
                }
            }
            else if (link.linkNo > clickedLink.linkNo) {
                near2.linkNo = link.linkNo;
                near2.index = i;
            }
        }
    }
    if (near2.linkNo > temp.linkNo && temp.linkNo >= clickedLink.linkNo) {
        near2 = temp;
    }

    return { near1: near1, near2: near2 };
}

IfxStpMap.prototype.checkParallelSegmentsContinuity = function (pathIndex, segIndex, intersectionFeatures, intersectionPoints, startIndex, endIndex, startPoint, endPoint) {
    var rPath = this.routeManager.getRoutePath(pathIndex);
    if (startPoint == null || startPoint == undefined || endPoint == null || endPoint == undefined) {
        if (intersectionFeatures != null) {
            startPoint = jQuery.extend(true, {}, intersectionFeatures[startIndex].point);
            endPoint = jQuery.extend(true, {}, intersectionFeatures[endIndex].point);
        }
        else if (intersectionPoints != null) {
            startPoint = jQuery.extend(true, {}, intersectionPoints[startIndex]);
            endPoint = jQuery.extend(true, {}, intersectionPoints[endIndex]);
        }
    }
    for (var i = 0; i < rPath.routeSegmentList.length; i++) {
        if (i != segIndex) {
            var flag = 0;
            if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[i].startPointGeometry.sdo_point, startPoint)) {
                startPoint.x = rPath.routeSegmentList[i].endPointGeometry.sdo_point.X;
                startPoint.y = rPath.routeSegmentList[i].endPointGeometry.sdo_point.Y;
                flag = 1;
            }
            else if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[i].endPointGeometry.sdo_point, startPoint)) {
                startPoint.x = rPath.routeSegmentList[i].startPointGeometry.sdo_point.X;
                startPoint.y = rPath.routeSegmentList[i].startPointGeometry.sdo_point.Y;
                flag = 1;
            }
            if (flag == 1) {
                if (IfxStpmapCommon.compareGeometries(startPoint, endPoint))
                    return true;
                if (intersectionFeatures != null) {
                    for (var j = 0; j < intersectionFeatures.length; j++) {
                        if (IfxStpmapCommon.compareGeometries(intersectionFeatures[j], startPoint))
                            return false;
                    }
                    return this.checkParallelSegmentsContinuity(pathIndex, i, jQuery.extend(true, [], intersectionFeatures), null, null, null, startPoint, endPoint);
                }
                else if (intersectionPoints != null) {
                    for (var j = 0; j < intersectionPoints.length; j++) {
                        if (IfxStpmapCommon.compareGeometries(intersectionPoints[j], startPoint))
                            return false;
                    }
                    return this.checkParallelSegmentsContinuity(pathIndex, i, null, jQuery.extend(true, [], intersectionPoints), null, null, startPoint, endPoint);
                }
            }
        }
    }
    return false;
}

IfxStpMap.prototype.findSegmentHavingEndPoint = function (pathIndex, X, Y) {
    var rPath = this.routeManager.getRoutePath(pathIndex);
    for (var i = 0; i < rPath.routeSegmentList.length; i++) {
        var point = new OpenLayers.Geometry.Point(X, Y);
        if (IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[i].startPointGeometry, point) || IfxStpmapCommon.compareGeometries(rPath.routeSegmentList[i].endPointGeometry, point)) {
            return i;
        }
    }
}

IfxStpMap.prototype.getIntersectionPointsInFeature = function (feature, points) {
    var intersectionPoints = [];
    for (var i = 0; i < points.length; i++) {
        if (IfxStpmapCommon.findIfIntersects(feature, points[i].x, points[i].y))
            intersectionPoints.push(points[i]);
    }
    return intersectionPoints;
}

IfxStpMap.prototype.cutALink = function (nearestPath, intersectionFeature, terminalPoints, link) {
    var rSegment = this.routeManager.RoutePart.routePathList[nearestPath.pathIndex].routeSegmentList[nearestPath.segmentIndex];
    var direction = IfxStpmapCommon.getDirectionOfRouteLink(rSegment, nearestPath, intersectionFeature.feature, this);
    var intersectionPoints = this.getIntersectionPointsInFeature(intersectionFeature.feature, terminalPoints);
    var nearestIntersectionPoints = IfxStpmapCommon.getNearestIntersectionPoints(intersectionFeature.feature, direction, intersectionPoints, nearestPath.x1, nearestPath.y1);
    var continuous = this.checkParallelSegmentsContinuity(nearestPath.pathIndex, nearestPath.segmentIndex, null, jQuery.extend(true, [], intersectionPoints), nearestIntersectionPoints.near1.index, nearestIntersectionPoints.near2.index);
    if (continuous) {
        this.setDivideRouteSegment(nearestPath.pathIndex, nearestPath.segmentIndex, nearestIntersectionPoints, intersectionFeature.feature, intersectionPoints, link);
        return;
    }
    showNotification('Route segment cannot be cut');
}

IfxStpMap.prototype.setDivideRouteSegment = function (pathIndex, segmentIndex, nearestIntersection, intersectionFeatures, intersectionPoints, link) {
    if (intersectionPoints == null || intersectionPoints == undefined) {
        var startPoint = {
            linkId: intersectionFeatures[nearestIntersection.near1.index].feature.attributes.LINK_ID,
            linkNo: nearestIntersection.near1.linkNo,
            pointGeom: { sdo_point: { X: intersectionFeatures[nearestIntersection.near1.index].point.x, Y: intersectionFeatures[nearestIntersection.near1.index].point.y } },
            lrs: IfxStpmapCommon.getLRSLength(intersectionFeatures[nearestIntersection.near1.index].feature.geometry, new OpenLayers.Geometry.Point(intersectionFeatures[nearestIntersection.near1.index].point.x, intersectionFeatures[nearestIntersection.near1.index].point.y)),
            otherinfo: { pointfeature: intersectionFeatures[nearestIntersection.near1.index].feature }
        };
        var endPoint = {
            linkId: intersectionFeatures[nearestIntersection.near2.index].feature.attributes.LINK_ID,
            linkNo: nearestIntersection.near2.linkNo,
            pointGeom: { sdo_point: { X: intersectionFeatures[nearestIntersection.near2.index].point.x, Y: intersectionFeatures[nearestIntersection.near2.index].point.y } },
            lrs: IfxStpmapCommon.getLRSLength(intersectionFeatures[nearestIntersection.near2.index].feature.geometry, new OpenLayers.Geometry.Point(intersectionFeatures[nearestIntersection.near2.index].point.x, intersectionFeatures[nearestIntersection.near2.index].point.y)),
            otherinfo: { pointfeature: intersectionFeatures[nearestIntersection.near2.index].feature }
        };
    }
    else {
        var startPoint = {
            linkId: intersectionFeatures.attributes.LINK_ID,
            linkNo: link,
            pointGeom: { sdo_point: { X: intersectionPoints[nearestIntersection.near1.index].x, Y: intersectionPoints[nearestIntersection.near1.index].y } },
            lrs: nearestIntersection.near1.lrs,
            otherinfo: { pointfeature: intersectionFeatures }
        };
        var endPoint = {
            linkId: intersectionFeatures.attributes.LINK_ID,
            linkNo: link,
            pointGeom: { sdo_point: { X: intersectionPoints[nearestIntersection.near2.index].x, Y: intersectionPoints[nearestIntersection.near2.index].y } },
            lrs: nearestIntersection.near2.lrs,
            otherinfo: { pointfeature: intersectionFeatures }
        };
    }

    var endSegmentIndex = this.findSegmentHavingEndPoint(pathIndex, endPoint.pointGeom.sdo_point.X, endPoint.pointGeom.sdo_point.Y);

    this.divideRouteSegment(pathIndex, segmentIndex, startPoint, endPoint, endSegmentIndex);
}

IfxStpMap.prototype.checkIfWaypointIsBypassed = function (nearestPath, startLinkNo, endLinkNo) {
    var rPath = this.routeManager.RoutePart.routePathList[nearestPath.pathIndex];
    if (rPath.routePointList.length < 3)
        return true;

    var link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, rPath.routePointList[2].linkId);
    if (link == undefined || link.linkNo > endLinkNo)
        return true;
    link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, rPath.routePointList[rPath.routePointList.length - 1].linkId);
    if (link == undefined || link.linkNo < startLinkNo)
        return true;

    for (var i = 2; i < rPath.routePointList.length; i++) {
        link = this.searchRouteLinkByLinkID(nearestPath.pathIndex, nearestPath.segmentIndex, rPath.routePointList[i].linkId);
        if (link.linkNo > startLinkNo && link.linkNo < endLinkNo)
            return false;
    }

    return true;
}

IfxStpMap.prototype.cutRoute = function (e, isLonLat, terminalPoint, bAll) {
    if (bAll == undefined)
        var bAll = true;
    var nearestPath = this.getNearestPathAndSegment(e.xy.x, e.xy.y, bAll, isLonLat);
    var rPath = this.routeManager.getRoutePath(nearestPath.pathIndex);
    if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[nearestPath.segmentIndex].segmentType) != 'NORMAL') {
        this.deleteRouteSegment(nearestPath, rPath);
        return;
    }
    if (terminalPoint == undefined) {
        var terminalPoints = this.getTerminalPoints(nearestPath.pathIndex, nearestPath.segmentIndex);
    }
    else {
        var terminalPoints = [terminalPoint];
    }

    if (terminalPoints.length > 0) {
        var intersectionFeatures = this.getIntersectionFeatures(nearestPath.pathIndex, nearestPath.segmentIndex, terminalPoints);
        if (intersectionFeatures.length > 0) {
            var nearestIntersectionLinks = this.getNearestIntersectionLinks(nearestPath, intersectionFeatures);
            if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[rPath.routeSegmentList.length - 1].segmentType) == 'OVERRIDE') {
                if (nearestIntersectionLinks.near1.index != -2 && nearestIntersectionLinks.near2.index == -2) {
                    nearestIntersectionLinks.near2 = this.getNextIntersectionLink(nearestIntersectionLinks.near1.linkNo, intersectionFeatures, nearestPath);
                }
                else if (nearestIntersectionLinks.near1.index == -2 && nearestIntersectionLinks.near2.index != -2) {
                    nearestIntersectionLinks.near1 = this.getPreviousIntersectionLink(nearestIntersectionLinks.near2.linkNo, intersectionFeatures, nearestPath);
                }
            }
            if (nearestIntersectionLinks != null && nearestIntersectionLinks.near1.index != -2 && nearestIntersectionLinks.near2.index != -2) {
                if (nearestIntersectionLinks.near1.index != nearestIntersectionLinks.near2.index) {
                    var wayPointBypassed = this.checkIfWaypointIsBypassed(nearestPath, nearestIntersectionLinks.near1.linkNo, nearestIntersectionLinks.near2.linkNo);
                    if (wayPointBypassed) {
                        var continuous = this.checkParallelSegmentsContinuity(nearestPath.pathIndex, nearestPath.segmentIndex, jQuery.extend(true, [], intersectionFeatures), null, nearestIntersectionLinks.near1.index, nearestIntersectionLinks.near2.index);
                        if (continuous)
                            this.setDivideRouteSegment(nearestPath.pathIndex, nearestPath.segmentIndex, nearestIntersectionLinks, intersectionFeatures);
                        else
                            showNotification("Route segment cannot be cut since it is not continuous.");
                    }
                    else {
                        showNotification('Cannot bypass a waypoint');
                    }
                    return;
                }
                else {
                    this.cutALink(nearestPath, intersectionFeatures[nearestIntersectionLinks.near1.index], terminalPoints, nearestIntersectionLinks.near1.linkNo);



                    return;
                }
            }
        }
        if (this.routeManager.getRoutePathCount() > 1)
            showNotification("Route segment cannot be removed since it is part of the main route of the clicked route path");
        else
            showNotification("Route segment cannot be removed since it is part of the main route");
        return;
    }
    else {
        if (this.routeManager.getRoutePathCount() > 1)
            showNotification("Route segment cannot be cut since no alternate segments are found related to the clicked route path");
        else
            showNotification("Route segment cannot be cut since no alternate segments are found");
        return;
    }
    showNotification('Route segment cannot be cut');
}


IfxStpMap.prototype.removeduplicatesegment = function (routePathIndex) {
    this.routeManager.RemoveduplicateRoutesegments(routePathIndex);
}

IfxStpMap.prototype.getRoutePart = function (isReturnLeg) {
    if (isReturnLeg == true && (this.returnLegRoute == null || this.returnLegRoute == undefined)) {
        return null;
    }
    return this.routeManager.getRoutePart(isReturnLeg, this.returnLegRoute);
}

IfxStpMap.prototype.setSegmentFeatureInternal = function (callback, features, routeSegment, context) {
    var self = this;
    this.searchFeaturesByLinkID([routeSegment.startLinkId, routeSegment.endLinkId], function (featuresStEnd) {
        if (featuresStEnd != undefined) {
            routeSegment.otherinfo = {};
            routeSegment.otherinfo.startSegmentfeature = IfxStpmapCommon.getFeatureOfLinkId(featuresStEnd, routeSegment.startLinkId);
            routeSegment.otherinfo.endSegmentfeature = IfxStpmapCommon.getFeatureOfLinkId(featuresStEnd, routeSegment.endLinkId);
            routeSegment.otherinfo.features = features == null ? [] : features;
            routeSegment.otherinfo.completefeatures = routeSegment.otherinfo.features;
            if (features != null) {
                IfxStpmapCommon.createFeatureForRouteSegment(routeSegment);
            }
            else {
                if (routeSegment.otherinfo.startSegmentfeature && routeSegment.otherinfo.endSegmentfeature && routeSegment.otherinfo.startSegmentfeature != routeSegment.otherinfo.endSegmentfeature) {
                    var startFeature = IfxStpmapCommon.getPartialFeature(routeSegment.otherinfo.startSegmentfeature, routeSegment);
                    var endFeature = IfxStpmapCommon.getPartialFeature(routeSegment.otherinfo.endSegmentfeature, routeSegment, 1);
                    routeSegment.otherinfo.features.push(startFeature);
                    routeSegment.otherinfo.features.push(endFeature);
                    routeSegment.otherinfo.completefeatures = routeSegment.otherinfo.features;
                    IfxStpmapCommon.createFeatureForRouteSegment(routeSegment);
                }
                else if (routeSegment.otherinfo.startSegmentfeature) {
                    var startFeature = IfxStpmapCommon.getPartialFeature(routeSegment.otherinfo.startSegmentfeature, routeSegment, 'STARTPOINT');
                    IfxStpmapCommon.createPartialFeatureForRouteSegment(startFeature, routeSegment.startPointDirection, routeSegment.endPointGeometry.sdo_point.X, routeSegment.endPointGeometry.sdo_point.Y, routeSegment);
                }

            }
            if (callback && typeof (callback) === "function") {
                callback(true, context);
            }
        }
    });
}

IfxStpMap.prototype.setSegmentFeature = function (routeSegment, context, callback) {
    routeSegment.otherinfo = { completefeatures: [], features: [] };
    if (routeSegment.offRoadGeometry != null) {
        IfxStpmapCommon.createFeatureForRouteSegment(routeSegment);
        if (callback && typeof (callback) === "function") {
            callback(true, context);
        }
        return;
    }
    else {
        var self = this;

        /* if (routeSegment.routeLinkList != null) {*/

        if (routeSegment.routeLinkList.length != 0) {
            if (routeSegment.startLinkId == 0) {
                routeSegment.startLinkId = routeSegment.routeLinkList[0].linkId;
            }
            if (routeSegment.endLinkId == 0) {
                routeSegment.endLinkId = routeSegment.routeLinkList[routeSegment.routeLinkList.length - 1].linkId;
            }

            if (routeSegment.routeLinkList.length >= 1) {
                if (IfxStpmapCommon.getSegmentTypeName(routeSegment.segmentType) == 'NORMAL') {
                    if (routeSegment.routeLinkList[0].linkId == routeSegment.startLinkId) {
                        routeSegment.routeLinkList.splice(0, 1);
                    }
                    if (routeSegment.routeLinkList.length != 0 && routeSegment.routeLinkList[routeSegment.routeLinkList.length - 1].linkId == routeSegment.endLinkId) {
                        routeSegment.routeLinkList.splice(routeSegment.routeLinkList.length - 1, 1);
                    }
                }
                else {
                    for (var i = 0; i < routeSegment.routeLinkList.length; i++) {
                        routeSegment.routeLinkList[i].linkNo = i + 1;
                    }
                }
            }
        }
        /*}*/
        this.searchFeaturesOfRouteLinkList(routeSegment.routeLinkList, function (features) {
            if (features == null || features == undefined || features.length <= 0) {
                self.setSegmentFeatureInternal(callback, null, routeSegment, context);
            }
            else {
                self.setSegmentFeatureInternal(callback, features, routeSegment, context);
            }
        });
    }
}

IfxStpMap.prototype.drawAnnotations = function setAnnotationFeatures(routePath, pathIndex) {
    var routeSegment;
    for (var j = 0; j < routePath.routeSegmentList.length; j++) {
        routeSegment = routePath.routeSegmentList[j];
        if (routeSegment.routeAnnotationsList != null) {
            for (var i = 0; i < routeSegment.routeAnnotationsList.length; i++) {
                var annotObject = routeSegment.routeAnnotationsList[i]; annotObject.otherinfo = {};
                var marker = this.setMarker('ANNOTATION', annotObject.geometry.sdo_point.X, annotObject.geometry.sdo_point.Y,
                    pathIndex + ' ' + j + ' ' + i);
                annotObject['otherinfo']['marker'] = marker;
                annotObject['otherinfo']['pathIndex'] = pathIndex;
                annotObject['otherinfo']['segmentIndex'] = j;
            }
        }
    }
}

IfxStpMap.prototype.drawAnnotationsForSegment = function (routePath, pathIndex, segmentIndex) {
    var routeSegment = routePath.routeSegmentList[segmentIndex];
    if (routeSegment.routeAnnotationsList != null) {
        for (var i = 0; i < routeSegment.routeAnnotationsList.length; i++) {
            var annotObject = routeSegment.routeAnnotationsList[i]; annotObject.otherinfo = {};
            var marker = this.setMarker('ANNOTATION', annotObject.geometry.sdo_point.X, annotObject.geometry.sdo_point.Y,
                pathIndex + ' ' + segmentIndex + ' ' + i);
            annotObject['otherinfo']['marker'] = marker;
            annotObject['otherinfo']['pathIndex'] = pathIndex;
            annotObject['otherinfo']['segmentIndex'] = segmentIndex;
        }
    }
}

IfxStpMap.prototype.setPathFeature = function (routePath, callback) {
    var callbackCount = 0;
    var invalidePathArray = new Array();
    var self = this;
    for (var i = 0; i < routePath.routeSegmentList.length; i++) {
        this.setSegmentFeature(routePath.routeSegmentList[i], i, function (result, context) {
            if (result == false) {
                invalidePathArray.push(context);
            }
            callbackCount++;

            if (callbackCount == routePath.routeSegmentList.length) {

                self.setRoutePathWaypointFeatures(routePath, function (result) {

                    if (result == true) {
                        //remove all invalid segments from path list
                        for (var i = invalidePathArray.length - 1; i >= 0; i--) {
                            routePath.routeSegmentList.splice(i, 1);
                        }

                    }
                    callback(result);
                });
            }
        });
    }
    if (routePath.routeSegmentList.length <= 0) {
        callback(null);
    }
}

IfxStpMap.prototype.getOutlineRoutePointFeatures = function (routePath, routePointIndex) {
    var self = this;
    this.olMap.setCenter([routePath.routePointList[routePointIndex].pointGeom.sdo_point.X, routePath.routePointList[routePointIndex].pointGeom.sdo_point.Y], 10);
    var pix = this.olMap.getPixelFromLonLat(new OpenLayers.LonLat(routePath.routePointList[routePointIndex].pointGeom.sdo_point.X, routePath.routePointList[routePointIndex].pointGeom.sdo_point.Y));
    this.searchFeaturesByXY(pix.x, pix.y, false, 1000, function (features) {
        retObject = IfxStpmapCommon.findNearestFeatureIndex(features, routePath.routePointList[routePointIndex].pointGeom.sdo_point.X, routePath.routePointList[routePointIndex].pointGeom.sdo_point.Y);
        if (retObject == undefined || retObject == null) {
            if (callback && typeof (callback) === "function") {
                callback(null);
            }
            return null;
        }
        else {
            var feature = features[retObject.index];
            if (feature != null) {
                routePath.routePointList[routePointIndex] = self.createRoutePointObject(IfxStpmapCommon.getPointTypeName(routePath.routePointList[routePointIndex].pointType), feature, retObject, routePath.routePointList[routePointIndex].routePointNo + 1, routePath.routePointList[routePointIndex].pointDescr);
            }
            else if (routeType != 'planned') {
                this.olMap.setCenter([routePath.routePointList[routePointIndex].pointGeom.sdo_point.X, routePath.routePointList[routePointIndex].pointGeom.sdo_point.Y], 10);
                var pix = this.olMap.getPixelFromLonLat(new OpenLayers.LonLat(routePath.routePointList[routePointIndex].pointGeom.sdo_point.X, routePath.routePointList[routePointIndex].pointGeom.sdo_point.Y));
                var pointType = routePath.routePointList[i].pointType;
                nullLinks.push(i);
                this.setRoutePointAtXY({ pointType: IfxStpmapCommon.getPointTypeName(pointType), pointPos: routePath.routePointList[i].routePointNo + 1, X: pix.x, Y: pix.y, searchInBbox: false, Zoomin: false }, function (rPoint) {
                    self.olMap.zoomToExtent(self.vectorLayerMarkers.getDataExtent());
                    if (self.pageType != 'DISPLAYONLY') {
                        if (self.eventList['ROUTEPOINTADDED'] != undefined && typeof (self.eventList['ROUTEPOINTADDED']) === "function") {
                            self.eventList['ROUTEPOINTADDED'](rPoint);
                        }
                    }
                });
            }
        }
    });
}

IfxStpMap.prototype.setRoutePointDetails = function (routePath, routeType) {
    var self = this;
    var nullLinks = [];
    routePath.alternatePointList = routePath.routePointList.map(a => Object.assign({}, a));
    routePath.allRoutePointList = routePath.routePointList.map(a => Object.assign({}, a));
    routePath.allAlternatePointList = routePath.routePointList.map(a => Object.assign({}, a));
    for (var i = 0; i < routePath.routePointList.length; i++) {
        if (routePath.routePointList[i].otherinfo == undefined) {
            routePath.routePointList[i].otherinfo = { completefeatures: [], features: [] };
        }
        if (routePath.alternatePointList[i].otherinfo == undefined) {
            routePath.alternatePointList[i].otherinfo = { completefeatures: [], features: [] };
        }
        if (routePath.allRoutePointList[i].otherinfo == undefined) {
            routePath.allRoutePointList[i].otherinfo = { completefeatures: [], features: [] };
        }
        if (routePath.allAlternatePointList[i].otherinfo == undefined) {
            routePath.allAlternatePointList[i].otherinfo = { completefeatures: [], features: [] };
        }

        if (routePath.routePointList[i].linkId || $('#hIs_NEN').val() == 'true') {
            var feature = IfxStpmapCommon.getFeatureOfLinkIdFromPathDetails(routePath, routePath.routePointList[i].linkId);

            if (feature != null) {
                routePath.routePointList[i].otherinfo.beginNodeId = feature.attributes.REF_IN_ID;
                routePath.routePointList[i].otherinfo.linkId = feature.attributes.LINK_ID;
                routePath.routePointList[i].otherinfo.endNodeId = feature.attributes.NREF_IN_ID;
                routePath.routePointList[i].otherinfo.pointfeature = feature;
                routePath.routePointList[i].otherinfo.isFullAddress = true;

                routePath.alternatePointList[i].otherinfo.beginNodeId = feature.attributes.REF_IN_ID;
                routePath.alternatePointList[i].otherinfo.linkId = feature.attributes.LINK_ID;
                routePath.alternatePointList[i].otherinfo.endNodeId = feature.attributes.NREF_IN_ID;
                routePath.alternatePointList[i].otherinfo.pointfeature = feature;
                routePath.alternatePointList[i].otherinfo.isFullAddress = true;

                routePath.allRoutePointList[i].otherinfo.beginNodeId = feature.attributes.REF_IN_ID;
                routePath.allRoutePointList[i].otherinfo.linkId = feature.attributes.LINK_ID;
                routePath.allRoutePointList[i].otherinfo.endNodeId = feature.attributes.NREF_IN_ID;
                routePath.allRoutePointList[i].otherinfo.pointfeature = feature;
                routePath.allRoutePointList[i].otherinfo.isFullAddress = true;

                routePath.allAlternatePointList[i].otherinfo.beginNodeId = feature.attributes.REF_IN_ID;
                routePath.allAlternatePointList[i].otherinfo.linkId = feature.attributes.LINK_ID;
                routePath.allAlternatePointList[i].otherinfo.endNodeId = feature.attributes.NREF_IN_ID;
                routePath.allAlternatePointList[i].otherinfo.pointfeature = feature;
                routePath.allAlternatePointList[i].otherinfo.isFullAddress = true;
            }
        }
        else {
            this.getOutlineRoutePointFeatures(routePath, i);
        }
    }
    if (nullLinks.length > 0) {
        for (var i = nullLinks.length - 1; i >= 0; i--)
            this.deleteRoutePoint(nullLinks[i], this.currentActiveRoutePathIndex);
    }
}

IfxStpMap.prototype.setRoutePathWaypointFeatures = function (routePath, callback) {
    var wayPointLinkList = [];

    for (var i = 2; i < routePath.routePointList.length; i++) {
        wayPointLinkList.push(routePath.routePointList[i].linkId);
    }

    if (wayPointLinkList.length > 0) {
        this.searchFeaturesByLinkID(wayPointLinkList, function (features) {
            for (var i = 2; i < routePath.routePointList.length; i++) {
                if (routePath.routePointList[i].otherinfo == undefined) {
                    routePath.routePointList[i].otherinfo = {};
                }
                routePath.routePointList[i].otherinfo.pointfeature = IfxStpmapCommon.getFeatureOfLinkId(features, routePath.routePointList[i].linkId);
            }
            callback(true);
        });
    }
    else {
        callback(false);
    }
}

IfxStpMap.prototype.setRoutePart = function (routePart, callback) {
    this.routeManager.setRoutePart(routePart);
    var callbackCount = 0;
    var self = this;
    var pageState = objifxStpMap.getPageState();
    for (var i = 0; i < routePart.routePathList.length; i++) {
        this.setPathFeature(routePart.routePathList[i], function (result) {
            callbackCount++;
            if (callbackCount == routePart.routePathList.length) {
                for (var j = 0; j < routePart.routePathList.length; j++) {
                    self.setRoutePointDetails(routePart.routePathList[j], routePart.routePartDetails.routeType);
                    self.routeManager.setPathNo(j, j);
                    if (routePart.routePathList[j].routeSegmentList[0] != null && routePart.routePathList[j].routeSegmentList[0] != undefined) {
                        IfxStpmapCommon.createFeatureForWaypoints(routePart.routePathList[j], routePart.routePathList[j].routeSegmentList[0], self);
                        if (pageState == "replanninginprogress") {
                            self.routeManager.setRoutePathState(j, "routeplanned");
                        }
                        else {
                            self.routeManager.setRoutePathState(j, "routedisplayed");
                        }

                        //display annotations
                        self.drawAnnotations(routePart.routePathList[j], j);
                    }

                    if (j == routePart.routePathList.length - 1 && callback != undefined && callback != null && callback != "" && typeof (callback) === "function") {
                        callback();
                    }
                }

                self.drawRoute(-1, true);
                if (self.vectorLayerRoute.features.length != 0)
                    self.olMap.zoomToExtent(self.vectorLayerRoute.getDataExtent());
                else
                    self.olMap.zoomToExtent(self.vectorLayerMarkers.getDataExtent());
                self.currentActiveRoutePathIndex = 0;
                //if ($('#PortalType').val() == '696001' && $("#hf_isNotification").val() != undefined && $("#hf_isNotification").val() =='True') {
                //    ShowInfoPopup('Please note: To view only the unsuitable affected structures and constraints on the map you must choose the route from the Route Assessment page');
                //}
                if (self.eventList['ROUTELOADED'] != undefined && typeof (self.eventList['ROUTELOADED']) === "function") {
                    self.eventList['ROUTELOADED'](null);
                }
            }
        });
    }
}

IfxStpMap.prototype.drawSketchedRoute = function (routePart) {
    var i;
    var points = [];

    this.routeManager.setRoutePart(routePart, null);

    for (i = 0; i < this.routeManager.getRoutePathCount(); i++) {
        var rPath = this.routeManager.getRoutePath(i);
        this.currentActiveRoutePathIndex = i;
        this.drawMarkers(rPath);
    }
    this.olMap.zoomToExtent(this.vectorLayerMarkers.getDataExtent());

    var geom = this.routeManager.RoutePart.routePartDetails.partGeometry;
    if (geom != null) {
        var ordinatesArray = this.routeManager.RoutePart.routePartDetails.partGeometry.OrdinatesArray;

        for (i = 0; i < ordinatesArray.length / 2; i += 2) {
            var p = new OpenLayers.Geometry.Point(routePart.routePartDetails.partGeometry.OrdinatesArray[i], routePart.routePartDetails.partGeometry.OrdinatesArray[i + 1]);
            points.push(p);
        }
        var feature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(points));
        this.vectorLayerRoute.addFeatures(feature);

        this.olMap.zoomToExtent(this.vectorLayerRoute.getDataExtent());
    }
}

IfxStpMap.prototype.initialiseReturnRoutePart = function () {
    this.tempRoutePart = jQuery.extend(true, {}, this.routeManager);
    this.routeManager.clearRoutePart(true);

    var rPointList = this.tempRoutePart.RoutePart.routePathList[0].routePointList;
    for (var i = 0; i < rPointList.length; i++) {
        if (rPointList[i].pointType == 0) {
            this.setRoutePoint({ pointType: 'ENDPOINT', pointPos: -1, X: rPointList[i].pointGeom.sdo_point.X, Y: rPointList[i].pointGeom.sdo_point.Y, locationDesc: rPointList[i].pointDescr, setMarker: false, bLonLat: null, features: [rPointList[i].otherinfo.pointfeature] }, null);
        }
        else if (rPointList[i].pointType == 1) {
            this.setRoutePoint({ pointType: 'STARTPOINT', pointPos: -1, X: rPointList[i].pointGeom.sdo_point.X, Y: rPointList[i].pointGeom.sdo_point.Y, locationDesc: rPointList[i].pointDescr, setMarker: false, bLonLat: null, features: [rPointList[i].otherinfo.pointfeature] }, null);
        }
    }
}

IfxStpMap.prototype.setReturnRoutePart = function () {
    this.returnLegRoute = jQuery.extend(true, {}, this.routeManager);
    this.routeManager = jQuery.extend(true, {}, this.tempRoutePart);
}

IfxStpMap.prototype.removeReturnLeg = function () {
    if (this.returnLegRoute != null) {
        var rPath = this.returnLegRoute.RoutePart.routePathList[0];
        this.clearAllRouteSegmentsInPath(rPath);
        this.returnLegRoute = null;
    }
}

IfxStpMap.prototype.getCurrentBoundsAndZoom = function () {
    return { bounds: this.olMap.getExtent(), zoom: this.olMap.getZoom() };
}

IfxStpMap.prototype.setBounds = function (bounds) {
    this.bounds = bounds;
}

IfxStpMap.prototype.getBounds = function () {
    return this.bounds;
}

IfxStpMap.prototype.getRouteId = function () {
    return this.routeManager.getRouteId();
}

IfxStpMap.prototype.flashRoutePath = function (pathIndex, counter) {
    var rPath = this.routeManager.getRoutePath(pathIndex);
    if (counter % 2 == 0) {
        for (var i = 0; i < rPath.routeSegmentList.length; i++) {
            this.vectorLayerRoute.removeFeatures(rPath.routeSegmentList[i].otherinfo.completefeatures);
        }
    }
    else {
        for (var i = 0; i < rPath.routeSegmentList.length; i++) {
            this.vectorLayerRoute.addFeatures(rPath.routeSegmentList[i].otherinfo.completefeatures);
        }
    }
}

IfxStpMap.prototype.deleteRouteSegment = function (nearestPath, rPath) {
    var extend = 0;
    if (IfxStpmapCommon.getSegmentTypeName(rPath.routeSegmentList[nearestPath.segmentIndex].segmentType) == 'OFFROAD') {
        extend = IfxStpmapCommon.checkOffRoadExtend(rPath, nearestPath.segmentIndex)
    }
    var bool = IfxStpmapCommon.checkRouteContinuity(rPath, nearestPath.segmentIndex);
    if (bool) {
        this.removeRouteSegment(nearestPath.pathIndex, nearestPath.segmentIndex);
        if (extend == 1)
            this.shortenRoute('STARTPOINT');
        else if (extend == 2)
            this.shortenRoute('ENDPOINT');
    }
    else {
        showNotification("Route segment cannot be removed since it is part of the main route or it is connected to other segments.");
    }
}

IfxStpMap.prototype.updateDirectionOfRouteLinks = function (pathIndex, segmentIndex) {
    var rSegment = this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex];
    for (var i = 0; i < rSegment.routeLinkList.length; i++) {
        var direction = IfxStpmapCommon.getDirectionOfRouteLink(rSegment, { pathIndex: pathIndex, segmentIndex: segmentIndex }, null, this, rSegment.routeLinkList[i]);
        rSegment.routeLinkList[i].direction = direction;
    }
}

IfxStpMap.prototype.getControlByIndex = function (control) {
    for (var i = 0; i < this.olMap.controls.length; i++) {
        if (this.olMap.controls[i].displayClass == control) {
            return i;
        }
    }
    return -1;
}

IfxStpMap.prototype.setRoutePointDescription = function (pointIndex, description, returnLeg) {
    pointIndex = this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex) == 1 ? 0 : pointIndex;
    var rpPoint = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex].routePointList[pointIndex];
    rpPoint.pointDescr = description;
    rpPoint.otherinfo.isFullAddress = true;
}

IfxStpMap.prototype.isRoutePointAddressValid = function (pathIndex, pointIndex) {
    var rpPoint = this.routeManager.RoutePart.routePathList[pathIndex].routePointList[pointIndex];
    return rpPoint.otherinfo.isFullAddress;
}

IfxStpMap.prototype.validateRoutePoints = function () {
    var string = "";
    for (var i = 0; i < this.routeManager.RoutePart.routePathList.length; i++) {
        var pathString = "";
        for (var j = 0; j < this.routeManager.RoutePart.routePathList[i].routePointList.length; j++) {
            if (this.routeManager.RoutePart.routePathList[i].routePointList[j].pointType != 2) {
                var isValid = this.isRoutePointAddressValid(i, j);
                if (!isValid) {
                    if (pathString == "") {
                        if (this.routeManager.RoutePart.routePathList.length > 1)
                            pathString = "Path" + (i + 1) + " - ";
                    }
                    else {
                        pathString += ", ";
                    }
                    if (j == 0)
                        pathString += "Start point";
                    else if (j == 1)
                        pathString += "End point";
                    else
                        //pathString += "Viapoint" + (j - 1);     // cmd by nk HE-8764 on 6.12.2023
                        pathString += "Stopping point" + (j - 1);// added by nk HE-8764 on 6.12.2023
                }
            }
        }
        if (string != "")
            string += ", ";
        string += pathString;
    }
    return string;
}

IfxStpMap.prototype.updateFullAddress = function (pathIndex, pointIndex, pointDesc) {
    var rpPoint = this.routeManager.RoutePart.routePathList[pathIndex].routePointList[pointIndex];
    rpPoint.otherinfo.isFullAddress = true;
    rpPoint.pointDescr = pointDesc;
}

IfxStpMap.prototype.updateNodeInfo = function (pointType, pointNo, rPoint, feature) {
    if (pointType == 9) {
        var pointCount = this.routeManager.getAllRoutePointCount(this.currentActiveRoutePathIndex);
    }
    else {
        var pointCount = this.routeManager.getRoutePointCount(this.currentActiveRoutePathIndex);
    }
    if (pointCount == 0)
        return;

    var prev_rPoint;
    var rPath = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex];

    switch (pointType) {
        case 0:
            if (pointCount > 2)
                prev_rPoint = rPath.routePointList[2];
            else if (pointCount > 1)
                prev_rPoint = rPath.routePointList[1];
            else if (pointCount == 1)
                prev_rPoint = rPath.routePointList[0];

            this.changeNodeDetails(rPoint, prev_rPoint, feature);
            break;
        case 1:
            if (pointCount > 2)
                prev_rPoint = rPath.routePointList[rPath.routePointList.length - 1];
            else if (pointCount >= 1)
                prev_rPoint = rPath.routePointList[0];

            this.changeNodeDetails(rPoint, prev_rPoint, feature);
            break;
        case 2:
        case 3:
            var next_rPoint;
            if (pointNo == 1) {
                prev_rPoint = rPath.routePointList[0];
                if (pointCount == 2)
                    next_rPoint = rPath.routePointList[1];
                else
                    next_rPoint = rPath.routePointList[pointNo + 1];
            }
            else {
                prev_rPoint = rPath.routePointList[pointNo];
                if (pointCount == 3)
                    next_rPoint = rPath.routePointList[1];
                else if (pointCount > 3)
                    next_rPoint = rPath.routePointList[pointNo + 1];
                if (next_rPoint == null)// added by Nithin 7/2/15
                    next_rPoint = rPath.routePointList[1];
            }

            if (prev_rPoint.otherinfo.beginNodeId == rPoint.otherinfo.beginNodeId) {
                this.changeNodeDetails(rPoint, prev_rPoint, feature);
            }
            if (next_rPoint.linkId == rPoint.linkId || (next_rPoint.otherinfo == null && next_rPoint.otherinfo.pointfeature.attributes.REF_IN_ID == rPoint.otherinfo.pointfeature.attributes.NREF_IN_ID)
                || (next_rPoint.otherinfo == null && next_rPoint.otherinfo.pointfeature.attributes.NREF_IN_ID == rPoint.otherinfo.pointfeature.attributes.REF_IN_ID)) {
                if ((next_rPoint.otherinfo.dir_travel == 'F' && rPoint.otherinfo.dir_travel == 'F') || (next_rPoint.otherinfo.dir_travel == 'T' && rPoint.otherinfo.dir_travel == 'T')) {
                    this.swapNodeDetails(next_rPoint);
                }
            }
            break;
        case 9:
            var next_rPoint;
            if (pointNo == 1) {
                prev_rPoint = rPath.allRoutePointList[0];
                if (pointCount == 2)
                    next_rPoint = rPath.allRoutePointList[1];
                else
                    next_rPoint = rPath.allRoutePointList[pointNo + 1];
            }
            else {
                prev_rPoint = rPath.allRoutePointList[pointNo];
                if (pointCount == 3)
                    next_rPoint = rPath.allRoutePointList[1];
                else if (pointCount > 3)
                    next_rPoint = rPath.allRoutePointList[pointNo + 1];
                if (next_rPoint == null)// added by Nithin 7/2/15
                    next_rPoint = rPath.allRoutePointList[1];
            }

            if (prev_rPoint.otherinfo.beginNodeId == rPoint.otherinfo.beginNodeId) {
                this.changeNodeDetails(rPoint, prev_rPoint, feature);
            }
            if (next_rPoint.linkId == rPoint.linkId || (next_rPoint.otherinfo == null && next_rPoint.otherinfo.pointfeature.attributes.REF_IN_ID == rPoint.otherinfo.pointfeature.attributes.NREF_IN_ID)
                || (next_rPoint.otherinfo == null && next_rPoint.otherinfo.pointfeature.attributes.NREF_IN_ID == rPoint.otherinfo.pointfeature.attributes.REF_IN_ID)) {
                if ((next_rPoint.otherinfo.dir_travel == 'F' && rPoint.otherinfo.dir_travel == 'F') || (next_rPoint.otherinfo.dir_travel == 'T' && rPoint.otherinfo.dir_travel == 'T')) {
                    this.swapNodeDetails(next_rPoint);
                }
            }
            break;
    }
}

IfxStpMap.prototype.changeNodeDetails = function (currentRoutePoint, prevRoutePoint, feature) {
    if (prevRoutePoint.linkId == currentRoutePoint.linkId) {
        this.swapNodeDetails(currentRoutePoint);
        this.swapNodeDetails(prevRoutePoint);
    }
    else if (prevRoutePoint.otherinfo.pointfeature != null && (prevRoutePoint.otherinfo.pointfeature.attributes.REF_IN_ID == currentRoutePoint.otherinfo.pointfeature.attributes.NREF_IN_ID
        || prevRoutePoint.otherinfo.pointfeature.attributes.NREF_IN_ID == currentRoutePoint.otherinfo.pointfeature.attributes.REF_IN_ID)) {
        if ((prevRoutePoint.otherinfo.dir_travel == 'F' && currentRoutePoint.otherinfo.dir_travel == 'F') || (prevRoutePoint.otherinfo.dir_travel == 'T' && currentRoutePoint.otherinfo.dir_travel == 'T')) {
            this.swapNodeDetails(currentRoutePoint);
            this.swapNodeDetails(prevRoutePoint);
        }
    }
}

IfxStpMap.prototype.swapNodeDetails = function (rPoint) {
    var temp = rPoint['otherinfo'].beginNodeId;
    rPoint['otherinfo'].beginNodeId = rPoint['otherinfo'].endNodeId;
    rPoint['otherinfo'].endNodeId = temp;
}

IfxStpMap.prototype.doProcessPlanRouteRequest = function (routeRequest, callback) {
    this.routeManager.doProcessPlanRouteRequest(routeRequest, callback);
}

IfxStpMap.prototype.drawPartial = function (linkId, callback) {
    var self = this;
    var StrtEndFlag = 0;
    // calculating lrs value for the route having start and end are same link id..for exclude extra lines in planned route by afsal
    objifxStpMap.searchFeaturesByLinkID([linkId], function (feature) {
        var lrsStart = LRSMeasure(feature[0].geometry, new OpenLayers.Geometry.Point(self.routeManager.RoutePart.routePathList[0].routePointList[0].pointGeom.sdo_point.X, self.routeManager.RoutePart.routePathList[0].routePointList[0].pointGeom.sdo_point.Y), { tolerance: 2.0 });
        var lrsEnd = LRSMeasure(feature[0].geometry, new OpenLayers.Geometry.Point(self.routeManager.RoutePart.routePathList[0].routePointList[1].pointGeom.sdo_point.X, self.routeManager.RoutePart.routePathList[0].routePointList[1].pointGeom.sdo_point.Y), { tolerance: 2.0 });
        if (lrsStart < lrsEnd) {
            var line = LRSSubstring(feature[0].geometry, lrsStart, lrsEnd);
            StrtEndFlag = 1;
        }
        else {
            var line = LRSSubstring(feature[0].geometry, lrsEnd, lrsStart);
            StrtEndFlag = 0;
        }
        callback(new OpenLayers.Feature.Vector(line, StrtEndFlag));
    });
}
//function for plan a route on single link id by afsal
IfxStpMap.prototype.RouteDrawSingleLink = function (features, rSegment, rPath, DirectionFlag, planReturn, self) {
    //draw route for start and end are same link id
    features.data = 'NORMAL'; // for single linkid route
    rSegment.otherinfo.features = features;
    rSegment.otherinfo.completefeatures = features;
    rSegment.endPointDirection = DirectionFlag;
    rSegment.startPointDirection = DirectionFlag;
    rPath.routePointList[0].direction = DirectionFlag;
    rPath.routePointList[1].direction = DirectionFlag;
    rPath.routeSegmentList[0].endPointDirection = DirectionFlag;
    rPath.routeSegmentList[0].startPointDirection = DirectionFlag;
    IfxStpmapCommon.createFeatureForWaypoints(rPath, rSegment, self);
    self.updateDirectionOfRouteLinks(self.currentActiveRoutePathIndex, rPath.routeSegmentList.length - 1);

    if (self.pageType != 'NOMAPDISPLAY' && planReturn == false) {
        self.drawRoute(self.currentActiveRoutePathIndex, false);
        if (self.routeManager.getSegmentCount(self.currentActiveRoutePathIndex) > 1)
            self.routeManager.reArrangeRouteSegmentList(self.currentActiveRoutePathIndex);
        self.routeManager.setRoutePathState(self.currentActiveRoutePathIndex, 'routeplanned'); //set path state to routeplanned
    }
    else if (planReturn == true) {
        self.setReturnRoutePart();
    }
}

IfxStpMap.prototype.getAdvancedRouteDetails = function () {
    return this.advancedEditRouteDetails;
}

IfxStpMap.prototype.getAdvancedEditPath = function () {
    return this.advancedEditRouteDetails;
}

IfxStpMap.prototype.removeMarkerFeature = function (marker, callback) {
    this.vectorLayerMarkers.removeFeatures(marker);

    if (callback && typeof (callback) === "function") {
        this.reactivateControls();
        callback();
    }
}

IfxStpMap.prototype.setAdvancedEditPath = function (pointType, selectedPath, terminalPoint) {
    var self = this;
    this.advancedEditRouteDetails.pathIndex = selectedPath.pathIndex;
    if (pointType == 7) {
        this.advancedEditRouteDetails.startSegmentIndex = selectedPath.segmentIndex;
    }
    else {
        this.advancedEditRouteDetails.endSegmentIndex = selectedPath.segmentIndex;
    }
    if (terminalPoint != -1) {
        this.advancedEditRouteDetails.terminalPoint = terminalPoint;
        if (pointType == 7) {
            var rp = this.routeManager.getRoutePoint(selectedPath.pathIndex, 0);
            if (rp != undefined) {
                var marker = rp['otherinfo']['marker'];
                var timerVar = setInterval(function () {
                    self.removeMarkerFeature(marker, function () {
                        clearTimeout(timerVar);
                    });
                }, 100);
            }
        }
        else {
            var rp = this.routeManager.getRoutePoint(selectedPath.pathIndex, 1);
            if (rp != undefined) {
                var marker = rp['otherinfo']['marker'];
                var timerVar = setInterval(function () {
                    self.removeMarkerFeature(marker, function () {
                        clearTimeout(timerVar);
                    });
                }, 100);
            }
        }
    }
}

IfxStpMap.prototype.processEditRoute = function () {
    this.mergeRoutePaths(this.advancedEditRouteDetails.pathIndex, this.advancedEditRouteDetails.startSegmentIndex, this.advancedEditRouteDetails.endSegmentIndex, this.currentActiveRoutePathIndex, 0);
}

IfxStpMap.prototype.mergeRoutePaths = function (routePathIndex1, routeSegmentIndex1_1, routeSegmentIndex1_2, routePathIndex2, routeSegmentIndex2) {
    var startIdx = -1, endIdx, delLinkCount = 0;
    var pointIdx = 2;
    var newLinkList = [];
    var newPointList = [];
    var delLinkList = [];
    var delPointList = [];
    var routePath1 = this.routeManager.getRoutePath(routePathIndex1);
    var routePath2 = this.routeManager.getRoutePath(routePathIndex2);
    if (this.advancedEditRouteDetails.terminalPoint != 0) {
        for (var i = 0; i < routePath1.routeSegmentList[routeSegmentIndex1_1].routeLinkList.length; i++) {
            //insert route links along first section of route to new link list
            newLinkList.push(routePath1.routeSegmentList[routeSegmentIndex1_1].routeLinkList[i]);

            //break when edit section starts
            if (routePath1.routeSegmentList[routeSegmentIndex1_1].routeLinkList[i].linkId == routePath2.routePointList[0].linkId) {
                startIdx = i;
                break;
            }
        }
    }

    if (startIdx == -1 && routePath1.routeSegmentList[routeSegmentIndex1_1].endLinkId == routePath2.routePointList[0].linkId) {
        startIdx = routePath1.routeSegmentList[routeSegmentIndex1_1].routeLinkList.length - 1;
    }

    var nextLinkNo = startIdx + 1;

    //insert route links along edit section of route to new link list
    for (var i = 0; i < routePath2.routeSegmentList[routeSegmentIndex2].routeLinkList.length; i++) {
        var flag = false;
        for (var j = 0; j < newLinkList.length; j++) {
            if (newLinkList[j].linkId == routePath2.routeSegmentList[routeSegmentIndex2].routeLinkList[i].linkId) {
                flag = true;
                break;
            }
        }
        if (flag == false) {
            routePath2.routeSegmentList[routeSegmentIndex2].routeLinkList[i].linkNo = ++nextLinkNo;
            newLinkList.push(routePath2.routeSegmentList[routeSegmentIndex2].routeLinkList[i]);
        }
    }

    //record (store) links to be deleted
    for (var i = routeSegmentIndex1_1; i <= routeSegmentIndex1_2; i++) {
        if (i == routeSegmentIndex1_1) {
            loopFrom = startIdx + 1;
            if (this.advancedEditRouteDetails.terminalPoint == 0) {
                delLinkCount++;
                delLinkList.push(routePath1.routeSegmentList[i].startLinkId);
            }

        }
        else if (i == routeSegmentIndex1_2) {
            loopFrom = 0;
            delLinkCount++;
            delLinkList.push(routePath1.routeSegmentList[i].startLinkId);
            if (this.advancedEditRouteDetails.terminalPoint == 1) {
                delLinkCount++;
                delLinkList.push(routePath1.routeSegmentList[i].endLinkId);
            }
        }
        else {
            loopFrom = 0;
            delLinkCount++;
            delLinkList.push(routePath1.routeSegmentList[i].startLinkId);
            delLinkCount++;
            delLinkList.push(routePath1.routeSegmentList[i].endLinkId);
        }
        for (var j = loopFrom; j < routePath1.routeSegmentList[i].routeLinkList.length; j++) {
            if (i == routeSegmentIndex1_2 &&
                routePath2.routePointList[1].linkId == routePath1.routeSegmentList[routeSegmentIndex1_2].startLinkId) {
                endIdx = loopFrom;
                break;
            }
            delLinkCount++;
            delLinkList.push(routePath1.routeSegmentList[i].routeLinkList[j].linkId);
            if (routePath1.routeSegmentList[i].routeLinkList[j].linkId == routePath2.routePointList[1].linkId) {
                endIdx = j;
                break;
            }
            if (j == routePath1.routeSegmentList[i].routeLinkList.length - 1) {
                endIdx = j;
                delLinkList.push(routePath1.routeSegmentList[i].endLinkId);
            }
        }
    }

    if (this.advancedEditRouteDetails.terminalPoint != 1) {
        //insert route links along end section of route to new link list
        for (var i = endIdx; i < routePath1.routeSegmentList[routeSegmentIndex1_2].routeLinkList.length; i++) {
            var flag = false;
            var cnt = newLinkList.length;
            for (var j = cnt - 1; j >= 0; j--) {
                if (newLinkList[j].linkId == routePath1.routeSegmentList[routeSegmentIndex1_2].routeLinkList[i].linkId) {
                    flag = true;
                    break;
                }
            }
            if (flag == false) {
                routePath1.routeSegmentList[routeSegmentIndex1_2].routeLinkList[i].linkNo = ++nextLinkNo;
                newLinkList.push(routePath1.routeSegmentList[routeSegmentIndex1_2].routeLinkList[i]);
            }
        }
    }

    //add start and end points to new point list by default
    if (this.advancedEditRouteDetails.terminalPoint != 0) {
        newPointList.push(routePath1.routePointList[0]);
    }
    else {
        newPointList.push(routePath2.routePointList[0]);
    }
    if (this.advancedEditRouteDetails.terminalPoint != 1) {
        newPointList.push(routePath1.routePointList[1]);
    }
    else {
        newPointList.push(routePath2.routePointList[1]);
    }

    //insert route points along first section of route to new point list 
    for (var i = 0; i <= routeSegmentIndex1_1; i++) {
        if (i == routeSegmentIndex1_1) {
            var loopTill = startIdx;
        }
        else {
            var loopTill = routePath1.routeSegmentList[i].routeLinkList.length;
        }
        for (var j = 0; j < loopTill; j++) {
            if (routePath1.routePointList[pointIdx] && routePath1.routeSegmentList[i].routeLinkList[j].linkId == routePath1.routePointList[pointIdx].linkId) {
                newPointList.push(routePath1.routePointList[pointIdx]);
                pointIdx++;
            }
        }
    }

    //bypass obsolete route points
    for (var i = routeSegmentIndex1_1; i <= routeSegmentIndex1_2; i++) {
        if (pointIdx >= routePath1.routePointList.length)
            break;
        var rSegment = routePath1.routeSegmentList[i];
        if (IfxStpmapCommon.getSegmentTypeName(rSegment.segmentType) == 'NORMAL') {
            if (routeSegmentIndex1_1 == routeSegmentIndex1_2) {
                var loopFrom = startIdx;// + 1;
                var loopTill = endIdx;
            }
            else {
                if (i == routeSegmentIndex1_1) {
                    var loopFrom = startIdx;// + 1;
                    var loopTill = rSegment.routeLinkList.length;
                }
                else if (i == routeSegmentIndex1_2) {
                    var loopFrom = 0;
                    var loopTill = endIdx;
                }
                else {
                    var loopFrom = 0;
                    var loopTill = rSegment.routeLinkList.length;
                }
            }
            for (var j = loopFrom; j < loopTill; j++) {
                if (pointIdx >= routePath1.routePointList.length)
                    break;
                if (rSegment.routeLinkList[j] != undefined) {
                    if (rSegment.routeLinkList[j].linkId == routePath1.routePointList[pointIdx].linkId) {
                        var rp = this.routeManager.getRoutePoint(routePathIndex1, pointIdx);
                        if (rp != undefined) {
                            var marker = rp['otherinfo']['marker'];
                            this.vectorLayerMarkers.removeFeatures(marker);
                        }
                        pointIdx++;
                    }
                }
            }
        }
    }

    //insert route points along edit section of route to new point list 
    for (var i = 2; i < routePath2.routePointList.length; i++) {
        routePath2.routePointList[i].routePointNo = newPointList.length - 1;
        newPointList.push(routePath2.routePointList[i]);
    }

    //insert remaining route points of path1 to new point list
    for (var i = routeSegmentIndex1_2; i < routePath1.routeSegmentList.length; i++) {
        if (i == routeSegmentIndex1_2) {
            var loopFrom = endIdx;
        }
        else {
            var loopFrom = 0;
        }
        for (var j = loopFrom; j < routePath1.routeSegmentList[i].routeLinkList.length; j++) {
            if (routePath1.routePointList[pointIdx] && routePath1.routeSegmentList[i].routeLinkList[j].linkId == routePath1.routePointList[pointIdx].linkId) {
                routePath1.routePointList[pointIdx].routePointNo = newPointList.length - 1;
                newPointList.push(routePath1.routePointList[pointIdx]);
                pointIdx++;
            }
        }
    }

    //remove start/end markers for start/end edit sections
    if (this.advancedEditRouteDetails.terminalPoint == 0) {
        var rp = this.routeManager.getRoutePoint(routePathIndex1, 0);
        if (rp != undefined) {
            var marker = rp['otherinfo']['marker'];
            this.vectorLayerMarkers.removeFeatures(marker);
        }
    }
    else if (this.advancedEditRouteDetails.terminalPoint == 1) {
        var rp = this.routeManager.getRoutePoint(routePathIndex1, 1);
        if (rp != undefined) {
            var marker = rp['otherinfo']['marker'];
            this.vectorLayerMarkers.removeFeatures(marker);
        }
    }

    //delete annotations along the obsolete segment
    for (var i = routeSegmentIndex1_1; i <= routeSegmentIndex1_2; i++) {
        for (var j = routePath1.routeSegmentList[i].routeAnnotationsList.length - 1; j >= 0; j--) {
            for (var k = 0; k < delLinkList.length; k++) {
                if (delLinkList[k] == routePath1.routeSegmentList[i].routeAnnotationsList[j].linkId) {
                    this.deleteAnnotation(routePathIndex1, i, j);
                    break;
                }
            }
        }
    }

    //add annotations along edit section of route
    if (routePath2.routeSegmentList[0].routeAnnotationsList != null) {
        for (var i = 0; i < routePath2.routeSegmentList[0].routeAnnotationsList.length; i++) {
            routePath1.routeSegmentList[routeSegmentIndex1_1].routeAnnotationsList.push(routePath2.routeSegmentList[0].routeAnnotationsList[i]);
        }
    }

    routePath1.routePointList = newPointList;
    routePath1.routeSegmentList[routeSegmentIndex1_1].endLinkId = routePath1.routeSegmentList[routeSegmentIndex1_2].endLinkId;
    routePath1.routeSegmentList[routeSegmentIndex1_1].endLrs = routePath1.routeSegmentList[routeSegmentIndex1_2].endLrs;
    routePath1.routeSegmentList[routeSegmentIndex1_1].endPointDirection = routePath1.routeSegmentList[routeSegmentIndex1_2].endPointDirection;
    routePath1.routeSegmentList[routeSegmentIndex1_1].endPointGeometry = routePath1.routeSegmentList[routeSegmentIndex1_2].endPointGeometry;
    routePath1.routeSegmentList[routeSegmentIndex1_1].routeLinkList = newLinkList;

    if (this.advancedEditRouteDetails.terminalPoint == 0) {
        routePath1.routeSegmentList[routeSegmentIndex1_1].startLinkId = routePath2.routeSegmentList[0].startLinkId;
        routePath1.routeSegmentList[routeSegmentIndex1_1].startLrs = routePath2.routeSegmentList[0].startLrs;
        routePath1.routeSegmentList[routeSegmentIndex1_1].startPointDirection = routePath2.routeSegmentList[0].startPointDirection;
        routePath1.routeSegmentList[routeSegmentIndex1_1].startPointGeometry = routePath2.routeSegmentList[0].startPointGeometry;
    }
    else if (this.advancedEditRouteDetails.terminalPoint == 1) {
        routePath1.routeSegmentList[routeSegmentIndex1_1].endLinkId = routePath2.routeSegmentList[0].endLinkId;
        routePath1.routeSegmentList[routeSegmentIndex1_1].endLrs = routePath2.routeSegmentList[0].endLrs;
        routePath1.routeSegmentList[routeSegmentIndex1_1].endPointDirection = routePath2.routeSegmentList[0].endPointDirection;
        routePath1.routeSegmentList[routeSegmentIndex1_1].endPointGeometry = routePath2.routeSegmentList[0].endPointGeometry;
    }

    this.removeFeaturesFromExistingRoute(routePathIndex1, routeSegmentIndex1_1, routeSegmentIndex1_2, delLinkList);

    if (routeSegmentIndex1_1 != routeSegmentIndex1_2) {
        this.copyFeaturesFromSegment(routePathIndex1, routeSegmentIndex1_1, routeSegmentIndex1_2);
    }
    this.addFeaturesToExistingRoute(routePathIndex1, routeSegmentIndex1_1, routePathIndex2, 0);

    //remove all obsolete route segments
    for (var i = routeSegmentIndex1_2; i > routeSegmentIndex1_1; i--) {
        this.removeRouteSegment(routePathIndex1, i);
    }

    //arrange route segment numbers
    for (var i = 0; i < routePath1.routeSegmentList.length; i++) {
        routePath1.routeSegmentList[i].segmentNo = i + 1;
    }

    this.advancedEditRouteDetails = {
        pathIndex: -1,
        startSegmentIndex: -1,
        endSegmentIndex: -1,
        terminalPoint: -1
    };

    removepath(routePathIndex2, 0);
    this.currentActiveRoutePathIndex = routePathIndex1;
    this.drawRoute(routePathIndex1, true);

    this.routeManager.setRoutePathState(routePathIndex1, 'routeplanned'); //set path state to routeplanneds
}

IfxStpMap.prototype.addFeaturesToExistingRoute = function (pathIndex1, segmentIndex1, pathIndex2, segmentIndex2) {
    var rSegment1 = this.routeManager.getRouteSegment(pathIndex1, segmentIndex1);
    var rSegment2 = this.routeManager.getRouteSegment(pathIndex2, segmentIndex2);

    for (var i = 0; i < rSegment2.otherinfo.completefeatures.length; i++) {
        rSegment1.otherinfo.completefeatures.push(rSegment2.otherinfo.completefeatures[i]);
    }
    if (this.advancedEditRouteDetails.terminalPoint != 0) {
        rSegment1.otherinfo.completefeatures.push(rSegment2.otherinfo.startSegmentfeature);
    }
    if (this.advancedEditRouteDetails.terminalPoint != 1) {
        rSegment1.otherinfo.completefeatures.push(rSegment2.otherinfo.endSegmentfeature);
    }

    for (var i = 0; i < rSegment2.otherinfo.features.length; i++) {
        rSegment1.otherinfo.features.push(rSegment2.otherinfo.features[i]);
    }
}

IfxStpMap.prototype.removeFeaturesFromExistingRoute = function (pathIndex, segmentIndex_1, segmentIndex_2, delLinkList) {
    var rPath = this.routeManager.getRoutePath(pathIndex);

    for (var i = segmentIndex_1; i <= segmentIndex_2; i++) {
        var rSegment = this.routeManager.getRouteSegment(pathIndex, i);

        for (var j = rSegment.otherinfo.completefeatures.length - 1; j >= 0; j--) {
            for (var k = 0; k < delLinkList.length; k++) {
                if (rSegment.otherinfo.completefeatures[j].attributes.LINK_ID == delLinkList[k]) {
                    this.vectorLayerRoute.removeFeatures([rSegment.otherinfo.completefeatures[j]]);
                    this.vectorLayerOffRoad.removeFeatures([rSegment.otherinfo.completefeatures[j]]);
                    rSegment.otherinfo.completefeatures.splice(j, 1);
                    break;
                }
            }
        }

        if (rSegment.otherinfo.features) {
            for (var j = rSegment.otherinfo.features.length - 1; j >= 0; j--) {
                for (var k = 0; k < delLinkList.length; k++) {
                    if (rSegment.otherinfo.features[j].attributes.LINK_ID == delLinkList[k]) {
                        this.vectorLayerRoute.removeFeatures([rSegment.otherinfo.features[j]]);
                        this.vectorLayerOffRoad.removeFeatures([rSegment.otherinfo.features[j]]);
                        rSegment.otherinfo.features.splice(j, 1);
                        break;
                    }
                }
            }
        }
    }

    for (var i = 0; i < rPath.routePointList.length; i++) {
        this.vectorLayerMarkers.removeFeatures(rPath.routePointList[i].otherinfo.marker);
    }
}

IfxStpMap.prototype.copyFeaturesFromSegment = function (pathIndex, segmentIndex_1, segmentIndex_2) {
    var rSegment1 = this.routeManager.getRouteSegment(pathIndex, segmentIndex_1);
    var rSegment2 = this.routeManager.getRouteSegment(pathIndex, segmentIndex_2);

    for (var i = 0; i < rSegment2.otherinfo.completefeatures.length; i++) {
        rSegment1.otherinfo.completefeatures.push(rSegment2.otherinfo.completefeatures[i]);
    }

    for (var i = 0; i < rSegment2.otherinfo.features.length; i++) {
        rSegment1.otherinfo.completefeatures.push(rSegment2.otherinfo.features[i]);
    }
}

IfxStpMap.prototype.validateEditPoints = function (x, y, pointType, terminalPoint) {
    var nearestPath = this.getNearestPathAndSegment(x, y, false);
    var advancedRouteDetails = this.getAdvancedRouteDetails();

    if (advancedRouteDetails.pathIndex != -1) {
        if (nearestPath.pathIndex != advancedRouteDetails.pathIndex) {
            showNotification('Cannot place edit points on different route paths');
            return false;
        }
    }

    if (pointType == 7) {
        if (advancedRouteDetails.endSegmentIndex != -1) {
            if (nearestPath.segmentIndex > advancedRouteDetails.endSegmentIndex) {
                showNotification('Cannot place edit point A after edit point B');
                return false;
            }
            else if (nearestPath.segmentIndex == advancedRouteDetails.endSegmentIndex) {
                var ret = this.validateEditPointsByLinkNo(nearestPath.pathIndex, nearestPath.segmentIndex, nearestPath.feature, 1);
                if (ret == -1) {
                    showNotification('Cannot place edit point A after edit point B');
                    return false;
                }
                if (ret == 0) {
                    showNotification('Cannot place edit point A and edit point B on same link');
                    return false;
                }
            }
        }
        this.setAdvancedEditPath(pointType, nearestPath, terminalPoint);
        return true;
    }
    else if (pointType == 8) {
        if (advancedRouteDetails.startSegmentIndex != -1) {
            if (nearestPath.segmentIndex < advancedRouteDetails.startSegmentIndex) {
                showNotification('Cannot place edit point B before edit point A');
                return false;
            }
            else if (nearestPath.segmentIndex == advancedRouteDetails.startSegmentIndex) {
                var ret = this.validateEditPointsByLinkNo(nearestPath.pathIndex, nearestPath.segmentIndex, nearestPath.feature, 0);
                if (ret == -1) {
                    showNotification('Cannot place edit point B before edit point A');
                    return false;
                }
                if (ret == 0) {
                    showNotification('Cannot place edit point A and edit point B on same link');
                    return false;
                }
            }
        }
        this.setAdvancedEditPath(pointType, nearestPath, terminalPoint);
        return true;
    }
}

IfxStpMap.prototype.backupAnnotationsinRoute = function () {
    var annotationList = [];
    for (var pathIndex = 0; pathIndex < this.routeManager.RoutePart.routePathList.length; pathIndex++) {
        for (var segmentIndex = 0; segmentIndex < this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList.length; segmentIndex++) {
            annotationList = this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList;
            if (annotationList != null && annotationList.length > 0) {
                for (var annotIndex = 0; annotIndex < annotationList.length; annotIndex++) {
                    tempAnnotations.push(this.routeManager.RoutePart.routePathList[pathIndex].routeSegmentList[segmentIndex].routeAnnotationsList[annotIndex]);
                }
            }
        }
    }
}

IfxStpMap.prototype.validateEditPointsByLinkNo = function (pathIndex, segmentIndex, feature, existingPoint) {
    var existingLinkNo, newLinkNo, pointIdx = 0;
    var rSegment1 = this.routeManager.getRouteSegment(pathIndex, segmentIndex);
    var rPath2 = this.routeManager.getRoutePath(this.currentActiveRoutePathIndex);

    if (existingPoint == 1 && rPath2.routePointList.length > 1) {
        pointIdx = 1;
    }

    for (var i = 0; i < rSegment1.routeLinkList.length; i++) {
        if (rSegment1.routeLinkList[i].linkId == rPath2.routePointList[pointIdx].linkId) {
            existingLinkNo = i + 1;
            break;
        }
    }

    for (var i = 0; i < rSegment1.routeLinkList.length; i++) {
        if (rSegment1.routeLinkList[i].linkId == feature.attributes.LINK_ID) {
            newLinkNo = i + 1;
        }
    }

    if (existingLinkNo == newLinkNo) {
        return 0;
    }
    else if ((existingPoint == 0 && existingLinkNo > newLinkNo)
        || (existingPoint == 1 && existingLinkNo < newLinkNo)) {
        return -1;
    }
    return 1;
}

IfxStpMap.prototype.setDragandPan = function () {
    if (this.dragActivity.dragActive == true) {
        this.dragActivity.panned = true;
    }
}

IfxStpMap.prototype.identifyWaypointPos = function (feature) {
    //attributes.LINK_ID
    var routePointList = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex].allRoutePointList;
    var routeLinkList = this.routeManager.RoutePart.routePathList[this.currentActiveRoutePathIndex].routeSegmentList[0].routeLinkList;
    if (routePointList.length < 3) {
        return 2;
    }
    var routePointCounter = 2;
    for (var i = 0; i < routeLinkList.length; i++) {
        if (routeLinkList[i].linkId == feature.attributes.LINK_ID) {
            //insertwaypoint(2);
            return routePointCounter;
        }
        if (routeLinkList[i].linkId == routePointList[routePointCounter].linkId) {
            routePointCounter++;
            if (routePointCounter == routePointList.length) {
                return routePointCounter;
            }
        }
    }
    return -1;
}

IfxStpMap.prototype.deleteRouteDragPoint = function (routePathIndex, pointNo) {
    var self = this;
    var timerVar = setInterval(function () {
        self.deleteAllRoutePoint(pointNo, routePathIndex, function () {
            if (self.eventList['ONDRAGCOMPLETE'] != undefined && typeof (self.eventList['ONDRAGCOMPLETE']) === "function") {
                self.eventList['ONDRAGCOMPLETE'](null, routePathIndex, true);
            }

            if (self.eventList['CHANGEPATHSELECT'] != undefined && typeof (self.eventList['CHANGEPATHSELECT']) === "function") {
                self.eventList['CHANGEPATHSELECT'](routePathIndex);
            }

            if (self.eventList['ONROUTEDRAGCOMPLETE'] != undefined && typeof (self.eventList['ONROUTEDRAGCOMPLETE']) === "function") {
                self.eventList['ONROUTEDRAGCOMPLETE']();
            }

            clearTimeout(timerVar);
        });
    }, 100);
}

IfxStpMap.prototype.resetAllRoutePoints = function (pathIndex) {
    var rPath = this.routeManager.getRoutePath(pathIndex);

    for (var i = 0; i < rPath.allRoutePointList.length; i++) {
        if (rPath.allRoutePointList[i].pointType == 9 && this.vectorLayerDragRouteMarker.features.length > 0) {
            this.vectorLayerDragRouteMarker.removeFeatures([rPath.allRoutePointList[i]['otherinfo']['marker']]);
        }
    }

    this.routeManager.resetAllRoutePoint(pathIndex);
    this.routeManager.resetAllAlternateRoutePoint(pathIndex);
    if (this.addReturnRoute == false) {
        if (this.vectorLayerMarkers != null) this.vectorLayerMarkers.redraw();
        if (this.vectorLayerDragRouteMarker != null) this.vectorLayerDragRouteMarker.redraw();
    }
}

IfxStpMap.prototype.calculateBufferForFeatureSearch = function () {
    var zoom = this.olMap.getZoom();
    return zoom * 10;
}

IfxStpMap.prototype.reactivateControls = function () {
    var self = this;
    var controlIndex = 0;
    this.olMap.controls.forEach(function (control) {
        if (control.displayClass == "olControlDragFeature" || control.displayClass == "olControlSelectFeature") {
            self.olMap.controls[controlIndex].deactivate();
            self.olMap.controls[controlIndex].activate();
        }
        controlIndex++;
    });
    /*var controlIndexDrag = this.getControlByIndex("olControlDragFeature");
    if (controlIndexDrag != -1) {
        this.olMap.controls[controlIndexDrag].deactivate();
        this.olMap.controls[controlIndexDrag].activate();
    }
    var controlIndexSelect = this.getControlByIndex("olControlSelectFeature");
    if (controlIndexSelect != -1) {
        this.olMap.controls[controlIndexSelect].deactivate();
        this.olMap.controls[controlIndexSelect].activate();
    }*/
}

IfxStpMap.prototype.removeRouteDragPoint = function (feature) {
    //check mouse over route drag point
    if (feature != null) {
        var pointAttrib = feature.data.name.split(" ");
        if (pointAttrib[0] == 'ROUTEDRAG') {
            this.deleteRouteDragPoint(pointAttrib[1], pointAttrib[2] == "0" ? parseInt(pointAttrib[3]) : parseInt(pointAttrib[2]));
        }
    }
}

IfxStpMap.prototype.setReturnRouteType = function (type) {
    this.addReturnRoute = type;
}

IfxStpMap.prototype.checkAndUpdateHistoricMapRoute = function (routepart) {
    if (routepart.routePathList != null) {
        var checkFlag = 0;
        for (var i = 0; i < routepart.routePathList[0].routeSegmentList.length; i++) {
            if (objifxStpMap.routeManager.RoutePart.routePathList[0].routeSegmentList[i].segmentType != 3 &&
                objifxStpMap.routeManager.RoutePart.routePathList[0].routeSegmentList[i].routeLinkList != null) {
                //rest code
            }
        }

    }
}
