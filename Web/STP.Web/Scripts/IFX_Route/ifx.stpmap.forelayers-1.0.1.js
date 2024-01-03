function IfxStpmapForeLayers() {
}

IfxStpmapForeLayers.prototype.PoliceBoundaries = function (show) {
    if (show) {
        this.showPoliceBoundaries();
    }
    else {
        this.hidePoliceBoundaries();
    }
}

IfxStpmapForeLayers.prototype.LABoundaries = function (show) {
    if (show) {
        this.showLABoundaries();
    }
    else {
        this.hideLABoundaries();
    }
}

IfxStpmapForeLayers.prototype.NHBoundaries = function (show) {
    if (show) {
        this.showNHBoundaries();
    }
    else {
        this.hideNHBoundaries();
    }
}

IfxStpmapForeLayers.prototype.TfLRoads = function (show) {
    if (show) {
        this.showTfLRoads();
    }
    else {
        this.hideTfLRoads();
    }
}

IfxStpmapForeLayers.prototype.WelshTrunkRoads = function (show) {
    if (show) {
        this.showWelshTrunkRoads();
    }
    else {
        this.hideWelshTrunkRoads();
    }
}

IfxStpmapForeLayers.prototype.ScottishTrunkRoads = function (show) {
    if (show) {
        this.showScottishTrunkRoads();
    }
    else {
        this.hideScottishTrunkRoads();
    }
}

IfxStpmapForeLayers.prototype.Restaurants = function (show) {
    if (show) {
        this.showRestaurants();
    }
    else {
        this.hideRestaurants();
    }
}
IfxStpmapForeLayers.prototype.Hospital = function (show) {
    if (show) {
        this.showHospital();
    }
    else {
        this.hideHospital();
    }
}
IfxStpmapForeLayers.prototype.Parkbay = function (show) {
    if (show) {
        this.showParkbay();
    }
    else {
        this.hideParkbay();
    }
} 
IfxStpmapForeLayers.prototype.FiInst = function (show) {
    if (show) {
        this.showFiInst();
    }
    else {
        this.hideFiInst();
    }
}
IfxStpmapForeLayers.prototype.Entertainment = function (show) {
    if (show) {
        this.showEntertainment();
    }
    else {
        this.hideEntertainment();
    }
}
IfxStpmapForeLayers.prototype.BuisFeci = function (show) {
    if (show) {
        this.showBuisFeci();
    }
    else {
        this.hideBuisFeci();
    }
}
IfxStpmapForeLayers.prototype.CMSServ = function (show) {
    if (show) {
        this.showCMSServ();
    }
    else {
        this.hideCMSServ();
    }
}
IfxStpmapForeLayers.prototype.Shopping = function (show) {
    if (show) {
        this.showShopping();
    }
    else {
        this.hideShopping();
    }
}
IfxStpmapForeLayers.prototype.EduIn = function (show) {
    if (show) {
        this.showEduIn();
    }
    else {
        this.hideEduIn();
    }
}
IfxStpmapForeLayers.prototype.AutMob = function (show) {
    if (show) {
        this.showAutMob();
    }
    else {
        this.hideAutMob();
    }
}
IfxStpmapForeLayers.prototype.TransHub = function (show) {
    if (show) {
        this.showTransHub();
    }
    else {
        this.hideTransHub();
    }
}
IfxStpmapForeLayers.prototype.TravelDest = function (show) {
    if (show) {
        this.showTravelDest();
    }
    else {
        this.hideTravelDest();
    }
}
IfxStpmapForeLayers.prototype.ParkAndRec = function (show) {
    if (show) {
        this.showParkAndRec();
    }
    else {
        this.hideParkAndRec();
    }
}
IfxStpmapForeLayers.prototype.showPoliceBoundaries = function () {
    objifxStpMap.deleteMapLayer("PoliceBoundaries");
    var policeBoundariesLayer = new OpenLayers.Layer.WMS(
        "PoliceBoundaries", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:POLICE_BOUNDARIES",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(policeBoundariesLayer);
}

IfxStpmapForeLayers.prototype.hidePoliceBoundaries = function () {
    objifxStpMap.deleteMapLayer("PoliceBoundaries");
}

IfxStpmapForeLayers.prototype.showLABoundaries = function () {
    objifxStpMap.deleteMapLayer("LABoundaries");
    var laBoundariesLayer = new OpenLayers.Layer.WMS(
        "LABoundaries", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:LA_BOUNDARIES",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(laBoundariesLayer);
}

IfxStpmapForeLayers.prototype.hideLABoundaries = function () {
    objifxStpMap.deleteMapLayer("LABoundaries");
}

IfxStpmapForeLayers.prototype.showNHBoundaries = function () {
    objifxStpMap.deleteMapLayer("NHBoundaries");
    var nhBoundariesLayer = new OpenLayers.Layer.WMS(
        "NHBoundaries", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL3Q32020:NHBOUNDARIES",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(nhBoundariesLayer);
}

IfxStpmapForeLayers.prototype.hideNHBoundaries = function () {
    objifxStpMap.deleteMapLayer("NHBoundaries");
}

IfxStpmapForeLayers.prototype.showTfLRoads = function () {
    objifxStpMap.deleteMapLayer("TfLRoads");
    var tflRoadsLayer = new OpenLayers.Layer.WMS(
        "TfLRoads", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:TFLROADS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(tflRoadsLayer);
}

IfxStpmapForeLayers.prototype.hideTfLRoads = function () {
    objifxStpMap.deleteMapLayer("TfLRoads");
}

IfxStpmapForeLayers.prototype.showWelshTrunkRoads = function () {
    objifxStpMap.deleteMapLayer("WelshTrunkRoads");
    var WelshTrunkRoadsLayer = new OpenLayers.Layer.WMS(
        "WelshTrunkRoads", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL3Q32020:WELSHTRUNKROADS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(WelshTrunkRoadsLayer);
}

IfxStpmapForeLayers.prototype.hideWelshTrunkRoads = function () {
    objifxStpMap.deleteMapLayer("WelshTrunkRoads");
}

IfxStpmapForeLayers.prototype.showScottishTrunkRoads = function () {
    objifxStpMap.deleteMapLayer("ScottishTrunkRoads");
    var ScottishTrunkRoadsLayer = new OpenLayers.Layer.WMS(
        "ScottishTrunkRoads", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL3Q32020:SCOTTISHTRUNKROADS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(ScottishTrunkRoadsLayer);
}

IfxStpmapForeLayers.prototype.hideScottishTrunkRoads = function () {
    objifxStpMap.deleteMapLayer("ScottishTrunkRoads");
}


IfxStpmapForeLayers.prototype.showRestaurants = function () {
    objifxStpMap.deleteMapLayer("RESTAURANTS");
    var RESTAURANTSLayer = new OpenLayers.Layer.WMS(
        "RESTAURANTS", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:RESTAURANTS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(RESTAURANTSLayer);
}
IfxStpmapForeLayers.prototype.hideRestaurants = function () {
    objifxStpMap.deleteMapLayer("RESTAURANTS");
}
IfxStpmapForeLayers.prototype.showHospital = function () {
    objifxStpMap.deleteMapLayer("HOSPITAL");
    var HOSPITALLayer = new OpenLayers.Layer.WMS(
        "HOSPITAL", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:HOSPITAL",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(HOSPITALLayer);
}
IfxStpmapForeLayers.prototype.hideHospital = function () {
    objifxStpMap.deleteMapLayer("HOSPITAL");
}
IfxStpmapForeLayers.prototype.showParkbay = function () {
    objifxStpMap.deleteMapLayer("PARKING");
    var PARKINGLayer = new OpenLayers.Layer.WMS(
        "PARKING", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:PARKING",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(PARKINGLayer);
}
IfxStpmapForeLayers.prototype.hideParkbay = function () {
    objifxStpMap.deleteMapLayer("PARKING");
}
IfxStpmapForeLayers.prototype.showFiInst = function () {
    objifxStpMap.deleteMapLayer("FINANCIAL_INSTITUTIONS");
    var FINANCIAL_INSTITUTIONSLayer = new OpenLayers.Layer.WMS(
        "FINANCIAL_INSTITUTIONS", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:FINANCIAL_INSTITUTIONS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(FINANCIAL_INSTITUTIONSLayer);
}
IfxStpmapForeLayers.prototype.hideFiInst = function () {
    objifxStpMap.deleteMapLayer("FINANCIAL_INSTITUTIONS");
}
IfxStpmapForeLayers.prototype.showEntertainment = function () {
    objifxStpMap.deleteMapLayer("ENTERTAINMENT");
    var ENTERTAINMENTLayer = new OpenLayers.Layer.WMS(
        "ENTERTAINMENT", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:ENTERTAINMENT",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(ENTERTAINMENTLayer);
}
IfxStpmapForeLayers.prototype.hideEntertainment = function () {
    objifxStpMap.deleteMapLayer("ENTERTAINMENT");
}
IfxStpmapForeLayers.prototype.showBuisFeci = function () {
    objifxStpMap.deleteMapLayer("BUSINESS_FACILITIES");
    var BUSINESS_FACILITIESLayer = new OpenLayers.Layer.WMS(
        "BUSINESS_FACILITIES", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:BUSINESS_FACILITIES",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(BUSINESS_FACILITIESLayer);
}
IfxStpmapForeLayers.prototype.hideBuisFeci = function () {
    objifxStpMap.deleteMapLayer("BUSINESS_FACILITIES");
}
IfxStpmapForeLayers.prototype.showCMSServ = function () {
    objifxStpMap.deleteMapLayer("COMMUNITY_SERVICES");
    var COMMUNITY_SERVICESLayer = new OpenLayers.Layer.WMS(
        "COMMUNITY_SERVICES", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:COMMUNITY_SERVICES",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(COMMUNITY_SERVICESLayer);
}
IfxStpmapForeLayers.prototype.hideCMSServ = function () {
    objifxStpMap.deleteMapLayer("COMMUNITY_SERVICES");
}
IfxStpmapForeLayers.prototype.showShopping = function () {
    objifxStpMap.deleteMapLayer("SHOPPING");
    var SHOPPINGLayer = new OpenLayers.Layer.WMS(
        "SHOPPING", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:SHOPPING",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(SHOPPINGLayer);
}
IfxStpmapForeLayers.prototype.hideShopping = function () {
    objifxStpMap.deleteMapLayer("SHOPPING");
}
IfxStpmapForeLayers.prototype.showEduIn = function () {
    objifxStpMap.deleteMapLayer("EDUCATIONAL_INSTITUTIONS");
    var EDUCATIONAL_INSTITUTIONSLayer = new OpenLayers.Layer.WMS(
        "EDUCATIONAL_INSTITUTIONS", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:EDUCATIONAL_INSTITUTIONS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(EDUCATIONAL_INSTITUTIONSLayer);
}
IfxStpmapForeLayers.prototype.hideEduIn = function () {
    objifxStpMap.deleteMapLayer("EDUCATIONAL_INSTITUTIONS");
}
IfxStpmapForeLayers.prototype.showAutMob = function () {
    objifxStpMap.deleteMapLayer("AUTOMOBILE_MAINTENANCE");
    var AUTOMOBILE_MAINTENANCELayer = new OpenLayers.Layer.WMS(
        "AUTOMOBILE_MAINTENANCE", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:AUTOMOBILE_MAINTENANCE",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(AUTOMOBILE_MAINTENANCELayer);
}
IfxStpmapForeLayers.prototype.hideAutMob= function () {
    objifxStpMap.deleteMapLayer("AUTOMOBILE_MAINTENANCE");
}
IfxStpmapForeLayers.prototype.showTransHub = function () {
    objifxStpMap.deleteMapLayer("TRANSPORTATION_HUBS");
    var TRANSPORTATION_HUBSLayer = new OpenLayers.Layer.WMS(
        "TRANSPORTATION_HUBS", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:TRANSPORTATION_HUBS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(TRANSPORTATION_HUBSLayer);
}
IfxStpmapForeLayers.prototype.hideTransHub = function () {
    objifxStpMap.deleteMapLayer("TRANSPORTATION_HUBS");
}
IfxStpmapForeLayers.prototype.showTravelDest = function () {
    objifxStpMap.deleteMapLayer("TRAVEL_DESTINATIONS");
    var TTRAVEL_DESTINATIONSLayer = new OpenLayers.Layer.WMS(
        "TRAVEL_DESTINATIONS", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:TRAVEL_DESTINATIONS",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(TTRAVEL_DESTINATIONSLayer);
}
IfxStpmapForeLayers.prototype.hideTravelDest = function () {
    objifxStpMap.deleteMapLayer("TRAVEL_DESTINATIONS");
}
IfxStpmapForeLayers.prototype.showParkAndRec = function () {
    objifxStpMap.deleteMapLayer("PARKS_RECREATION");
    var PARKS_RECREATIONLayer = new OpenLayers.Layer.WMS(
        "PARKS_RECREATION", objifxStpMap.geoserverWmsUrl,
        {
            layers: "ESDAL4:PARKS_RECREATION",
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            //viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(PARKS_RECREATIONLayer);
}
IfxStpmapForeLayers.prototype.hideParkAndRec = function () {
    objifxStpMap.deleteMapLayer("PARKS_RECREATION");
}


IfxStpmapForeLayers.prototype.ShowGpxRoute = function (movementId, routeId, movementType) {
    var param = "movementId:" + movementId + ";routeId:" + routeId + ";movementType:" + movementType
    objifxStpMap.deleteMapLayer("HISTORICROUTES");
    var gpxRouteLayer = new OpenLayers.Layer.WMS(
        "HISTORICROUTES", objifxStpMap.geoserverWmsUrl,
        {
            layers: 'ESDAL4:HISTORICROUTES',
            format: objifxStpMap.imageFormat,
            transparent: 'true',
            viewparams: param,
            tiled: false,
            tilesorigin: [objifxStpMap.olMap.maxExtent.left, objifxStpMap.olMap.maxExtent.bottom],
            data: Math.random()
        }
    );
    objifxStpMap.olMap.addLayer(gpxRouteLayer);
    objifxStpMap.olMap.setLayerIndex(gpxRouteLayer, 1);
}

IfxStpmapForeLayers.prototype.HideGpxRoute = function () {
    objifxStpMap.deleteMapLayer("HISTORICROUTES");
}
