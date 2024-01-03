function IfxStpmapStructures() {
    this.structureClassList = {};
    this.structureList = null;
    this.markerStructure = [];
    this.unsuitableConst = [];
    this.markerStructureOther = [];
    this.markerStructureLine = [];
    this.markerStructureLineOther = [];
    this.markerConstraint = [];
    this.pointLayer = null;
    this.lineLayer = null;
    this.polygonLayer = null;
    this.constraintDetails = {
        topologyType: '',
        geometry: '',
        ConstraintReferences: ''
    }
}
IfxStpmapStructures.prototype.structureClassList = {};
IfxStpmapStructures.prototype.structureList = null;
IfxStpmapStructures.prototype.markerStructure = [];
IfxStpmapStructures.prototype.markerStructureOther = [];
IfxStpmapStructures.prototype.markerStructureLine = [];
IfxStpmapStructures.prototype.markerStructureLineOther = [];
IfxStpmapStructures.prototype.markerConstraint = [];
IfxStpmapStructures.prototype.markerConstraintOther = [];
IfxStpmapStructures.prototype.pointLayer = null;
IfxStpmapStructures.prototype.lineLayer = null;
IfxStpmapStructures.prototype.polygonLayer = null;
IfxStpmapStructures.prototype.constraintDetails = {
    topologyType: '',
    geometry: '',
    ConstraintReferences: ''
}

IfxStpmapStructures.prototype.showStructures = function (structureList, otherOrg, page, zoom) {
    
    if (structureList.length == 0)
        return;
    var suitabilitycount = 0;
    var setsuitablity='All suitable'
    for (var i = 0; i < structureList.length; i++) {
        if (structureList[i].Suitability == 'Unsuitable') {
            suitabilitycount = suitabilitycount + 1;
        }
    }
    if (suitabilitycount != 0) {
        setsuitablity = 'Mixed';
    }
    

    if (zoom == undefined || zoom == null) {
        zoom = objifxStpMap.olMap.getZoom();
    }

    var markerStructure = [];
    this.clearStructureClassList();
    //clear all features   
    if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
        if (otherOrg == false) { //chceks for affected structure click
            objifxStpMap.vectorLayerStructures.removeFeatures(objifxStpMap.vectorLayerStructures.features);
        }
    }
  
    for (var i = 0; i < structureList.length; i++) {
            if (structureList[i].PointGeometry != null) {
                markerStructure[i] = [];
                markerStructure[i][0] = this.setMarkerStructure(structureList[i].StructureId, structureList[i].StructureName, structureList[i].StructureCode, structureList[i].StructureClass, 'point',
                    structureList[i].PointGeometry.OrdinatesArray[3], structureList[i].PointGeometry.OrdinatesArray[4], null, structureList[i].Suitability, otherOrg, zoom, setsuitablity);
                if (page != 'SORTInbox') {
                    markerStructure[i][1] = this.drawStructuresLine(structureList[i], zoom, otherOrg);
                }

                objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[i][0]);
                
                if (markerStructure[i][1] != undefined) {
                    objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[i][1]);
                }
              

               // if (page != 'SORTInbox') {
                    if (zoom > 8) {
                        markerStructure[i][0].style.display = 'none';
                    }
                    else {
                        if (markerStructure[i][1] != undefined) {
                            for (var j = 0; j < markerStructure[i][1].length; j++) {
                                markerStructure[i][1][j].style.display = 'none';
                            }
                        }
                    }
                //}

        if (checkboxValue(structureList[i].StructureClass, page, otherOrg) == false) {
            markerStructure[i][0].style.display = 'none';
            if (page != 'SORTInbox' && page != undefined) {
                for (var j = 0; j < markerStructure[i][1].length; j++) {
                    markerStructure[i][1][j].style.display = 'none';
                }
            }
            else {
                markerStructure[i][0].style.display = 'block';
			}
        }
               // if (page != 'SORTInbox') {
                    this.setFilterList(structureList[i].StructureClass, i);
                //}
            }
        }

    objifxStpMap.vectorLayerStructures.redraw();

    if (otherOrg == false)
        this.markerStructure = markerStructure;
    else
        this.markerStructureOther = markerStructure;

    this.addVectorLayers("Structures");

    return this.structureClassList;
}

IfxStpmapStructures.prototype.drawStructuresLine = function (structure, zoom, otherOrg) {
    var markerStructure = [];
    var ordinateArrayStart;
    var ordinateArrayEnd;
    var k = 0;

    for (var x = 0; (x * 3) < structure.LineGeometry.ElemArray.length; x++) {

        ordinateArrayStart = structure.LineGeometry.ElemArray[x * 3] - 1;

        if (((x * 3) + 3) < structure.LineGeometry.ElemArray.length) {
            ordinateArrayEnd = structure.LineGeometry.ElemArray[x * 3 + 3] - 1;
        }
        else {
            ordinateArrayEnd = structure.LineGeometry.OrdinatesArray.length;
        }

        var pointArr = new Array();
        for (j = ordinateArrayStart; j < ordinateArrayEnd; j += 2) {
            if (structure.LineGeometry.sdo_srid != 27700 && structure.StructureId < 170277) {
                pointArr.push(new OpenLayers.Geometry.Point(structure.LineGeometry.OrdinatesArray[j] - 2.0,   /*offset value added for handling esdal1 structures whose geometry cordinate system not in 27700*/
                    structure.LineGeometry.OrdinatesArray[j + 1] + 4.8));
            }
            else {
                pointArr.push(new OpenLayers.Geometry.Point(structure.LineGeometry.OrdinatesArray[j],   /*offset value removed for handling structures  whose geometry cordinate system in 27700 or not esdal1 structures*/
                    structure.LineGeometry.OrdinatesArray[j + 1]));
            }
        }

        markerStructure[k] = this.setMarkerStructure(structure.StructureId, structure.StructureName, structure.StructureCode, structure.StructureClass, 'line', 0, 0, pointArr, structure.Suitability, otherOrg, zoom);
        //objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[k]);

        k++;
    }

    return markerStructure;
}

IfxStpmapStructures.prototype.clearStructures = function (org) { //0-markerStructure, 1-markerStructureOther
    if (org != 1) {
        for (var i = 0; i < this.markerStructure.length; i++) {
            if (this.markerStructure[i].length > 0) {
                for (var j = 0; j < this.markerStructure[i].length; j++) {
                    objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructure[i][j]);
                }
            }
            else {
                objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructure[i]);
            }
        }
    }

    if (org != 0) {
        for (var i = 0; i < this.markerStructureOther.length; i++) {
            if (this.markerStructureOther[i].length > 0) {
                for (var j = 0; j < this.markerStructureOther[i].length; j++) {
                    objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructureOther[i][j]);
                }
            }
            else {
                objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructureOther[i]);
            }
        }
    }
}

IfxStpmapStructures.prototype.clearAllStructuresExceptOne = function (org,structurecode) { //0-markerStructure, 1-markerStructureOther
    if (org != 1) {
        for (var i = 0; i < this.markerStructure.length; i++) {
            if (this.markerStructure[i].length > 0) {
                for (var j = 0; j < this.markerStructure[i].length; j++) {
                    objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructure[i][j]);
                }
            }
            else {
                objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructure[i]);
            }
        }
    }

    if (org != 0) {
        for (var i = 0; i < this.markerStructureOther.length; i++) {
            var code = this.markerStructureOther[i][0].data.name.match("CODE (.*) SUITABILITY ");
            if (code[1] != structurecode) {
                if (this.markerStructureOther[i].length > 0) {
                    for (var j = 0; j < this.markerStructureOther[i].length; j++) {
                        objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructureOther[i][j]);
                    }
                }
                else {
                    objifxStpMap.vectorLayerStructures.removeFeatures(this.markerStructureOther[i]);
                }
            }
        }
    }
}

IfxStpmapStructures.prototype.filterStructures = function (structureClassList, show, otherOrg, zoom) {
    if (otherOrg == false)
        var markerStructure = this.markerStructure;
    else
        var markerStructure = this.markerStructureOther;

    if (show == true) {
        var display = 'block';
    }
    else {
        var display = 'none';
    }
    if (show == true) {
        if (zoom > 8) {
            for (var i = 0; i < structureClassList.length; i++) {
                if (markerStructure[structureClassList[i]]) {
                    for (var j = 0; j < markerStructure[structureClassList[i]][1].length; j++) {
                        markerStructure[structureClassList[i]][1][j].style.display = display;
                    }
                }
            }
        }
        else {
            for (var i = 0; i < structureClassList.length; i++) {
                if (markerStructure[structureClassList[i]]) {
                    markerStructure[structureClassList[i]][0].style.display = display;
                }
            }
        }
    }
    else {
        for (var i = 0; i < structureClassList.length; i++) {
            if (markerStructure[structureClassList[i]]) {
                for (var j = 0; j < markerStructure[structureClassList[i]][1].length; j++) {
                    markerStructure[structureClassList[i]][1][j].style.display = display;
                }
            }
        }
        for (var i = 0; i < structureClassList.length; i++) {
            if (markerStructure[structureClassList[i]]) {
                markerStructure[structureClassList[i]][0].style.display = display;
            }
        }
	}

    objifxStpMap.vectorLayerStructures.redraw();
}

IfxStpmapStructures.prototype.showOrHideAffectedStructures = function (show) {
    if (show == true) {
        var display = '';
    }
    else {
        var display = 'none';
    }

    for (var i = 0; i < this.markerStructure.length; i++) {
        this.markerStructure[i][0].style.display = display
        for (var j = 0; j < this.markerStructure[i][1].length; j++) {
            this.markerStructure[i][1][j].style.display = display;
        }
    }

    objifxStpMap.vectorLayerStructures.redraw();
}

IfxStpmapStructures.prototype.clearStructureClassList = function () {
    this.structureClassList = {
        underBridge: [],
        overBridge: [],
        underAndOverBridge: [],
        levelCrossing: []
    };
}

IfxStpmapStructures.prototype.setMarkerStructure = function (structureId, structureName, structureCode, structureClass, markerType, X, Y, pointArray, suitability, otherOrg, zoom, setsuitablity) {
    if (suitability != undefined) {
        if ((setsuitablity == undefined || setsuitablity == 'All suitable') && (suitability[0] != 'Unsuitable' && suitability[0] != 'Marginally suitable' && suitability[0] != 'Not Specified' && suitability[0] != 'Not Structure Specified')) {
            suitability[0] = 'Default';
        }
    }
    var finalSuitability = IfxStpmapCommon.getFinalSuitability(suitability);
    var markerName = "STRUCTURE " + structureId + " NAME " + structureName + " CODE " + structureCode + " SUITABILITY " + finalSuitability + " OTHERORG " + otherOrg + " TYPE " + structureClass;
    if (markerType == 'point') {
        var imgFile = IfxStpmapCommon.getStructureImage(structureClass, finalSuitability);
        var markerStyle = { externalGraphic: imgFile, graphicHeight: 18, graphicWidth: 18, title: structureName };
        var marker = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(X, Y),
                                                    { name: markerName },
                                                    markerStyle);
    }
    else {
        var markerStyle = { strokeColor: IfxStpmapCommon.getSuitabilityColor(finalSuitability, 0), strokeWidth: (zoom - 5) * 2, title: structureName };
        var marker = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(pointArray),
                                                    { name: markerName },
                                                    markerStyle);
    }
    return marker;
}

IfxStpmapStructures.prototype.setFilterList = function (structureClass, index) {
    switch (structureClass.toLowerCase()) {
        case 'underbridge':
            this.structureClassList.underBridge.push(index);
            break;
        case 'overbridge':
            this.structureClassList.overBridge.push(index);
            break;
        case 'under and over bridge':
            this.structureClassList.underAndOverBridge.push(index);
            break;
        default:
            this.structureClassList.levelCrossing.push(index);
    }
}

IfxStpmapStructures.prototype.showConstraints = function (constraintList, otherOrg) {
    var markerConstraint = [];
    //clear all features   
    var setsuitability = 0;
    for (var m = 0; m < constraintList.length; m++) {
        if (constraintList[m].ConstraintSuitability == 'Unsuitable') {
            setsuitability = setsuitability + 1;
        }
    }
    if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
        if (otherOrg == false) { //chceks for affected constraints click
            objifxStpMap.vectorLayerConstraints.removeFeatures(objifxStpMap.vectorLayerConstraints.features);
        }
    }
    for (var i = 0, j = 0; i < constraintList.length; i++) {
        if (constraintList[i].ConstraintGeometry != null) {
            markerConstraint[j] = this.addConstraint(constraintList[i].ConstraintId, constraintList[i].ConstraintName, constraintList[i].ConstraintCode, constraintList[i].ConstraintType,
                constraintList[i].TopologyType, constraintList[i].ConstraintSuitability, constraintList[i].ConstraintGeometry.OrdinatesArray, otherOrg);
            if (markerConstraint[j] == undefined || markerConstraint[j] == null)
                markerConstraint.length--;
            else
                j++;
        }
    }
    if (otherOrg == false)
        this.markerConstraint = markerConstraint;
    else
        this.markerConstraintOther = markerConstraint;
    objifxStpMap.vectorLayerConstraints.redraw();

    this.addVectorLayers("Constraints");
    //objifxStpMap.olMap.addLayer(objifxStpMap.vectorLayerConstraints);
}
IfxStpmapStructures.prototype.showConstraintsOnNotif = function (constraintList, otherOrg) {
    //filter added for showing unsuitable structures on notification viw map click #1257 #4th point
    var markerConstraint = [];
    var k = 0;
   
    for (var i = 0, j = 0; i < constraintList.length; i++) {
        var suitability = constraintList[i].constSuitability;
        if (suitability!=undefined) {
            if (suitability != null && suitability != undefined)
                suitability = suitability.toLowerCase();
            if (suitability == 'unsuitable' || suitability == 'not specified' ) {
                if (constraintList[i].constGeom != null) {
                    markerConstraint[k] = this.addConstraint(constraintList[i].constId, constraintList[i].consName, constraintList[i].constCode, constraintList[i].constType,
                        constraintList[i].topologyType, constraintList[i].constSuitability, constraintList[i].constGeom.OrdinatesArray, otherOrg);
                    if (markerConstraint[k] == undefined || markerConstraint[k] == null)
                        markerConstraint.length--;
                    else
                        j++;
                    k++;
                }
            }
        } else {
            suitability = constraintList[i].ConstraintSuitability;
            if (suitability != null && suitability != undefined)
                suitability = suitability.toLowerCase();
            if (suitability == 'unsuitable' || suitability == 'not specified') {
                if (constraintList[i].ConstraintGeometry != null) {
                    markerConstraint[k] = this.addConstraint(constraintList[i].ConstraintId, constraintList[i].ConstraintName, constraintList[i].ConstraintCode, constraintList[i].ConstraintType,
                        constraintList[i].TopologyType, constraintList[i].ConstraintSuitability, constraintList[i].ConstraintGeometry.OrdinatesArray, otherOrg);
                    if (markerConstraint[k] == undefined || markerConstraint[k] == null)
                        markerConstraint.length--;
                    else
                        j++;
                    k++;
                }
            }
        }
    }
    this.unsuitableConst = markerConstraint;
    if (otherOrg == false)
        this.markerConstraint = markerConstraint;
    else
        this.markerConstraintOther = markerConstraint;
    objifxStpMap.vectorLayerConstraints.redraw();

    this.addVectorLayers("Constraints");
}

IfxStpmapStructures.prototype.addConstraint = function (constraintId, constraintName, constraintCode, constraintType, topologyType, suitability, OrdinatesArray, otherOrg) {
    if (topologyType == 'point') {
        var markerConstraint = this.setMarkerConstraint(constraintId, constraintName, constraintCode, constraintType, topologyType, suitability, OrdinatesArray[0], OrdinatesArray[1], null, null, otherOrg);
        objifxStpMap.vectorLayerConstraints.addFeatures(markerConstraint);
    }
    else if (topologyType == 'linear' || topologyType == 'area') {
        var markerConstraint = [];
        var pointArr = new Array();
        for (var k = 0; k < OrdinatesArray.length; k += 2) {
            pointArr.push(new OpenLayers.Geometry.Point(OrdinatesArray[k], OrdinatesArray[k + 1]));
        }
        if (topologyType == 'linear') {
            markerConstraint[0] = this.setMarkerConstraint(constraintId, constraintName, constraintCode, constraintType, topologyType, suitability, 0, 0, pointArr, null, otherOrg);

            var middle = Math.floor((OrdinatesArray.length - ((OrdinatesArray.length / 2) % 2)) / 2);
            markerConstraint[1] = this.setMarkerConstraint(constraintId, constraintName, constraintCode, constraintType, 'point', suitability, OrdinatesArray[middle], OrdinatesArray[middle + 1], null, null, otherOrg);
        }
        else {
            var linearRing = new OpenLayers.Geometry.LinearRing(pointArr);
            markerConstraint[0] = this.setMarkerConstraint(constraintId, constraintName, constraintCode, constraintType, topologyType, suitability, 0, 0, null, linearRing, otherOrg);

            var centroid = linearRing.getCentroid();
            markerConstraint[1] = this.setMarkerConstraint(constraintId, constraintName, constraintCode, constraintType, 'point', suitability, centroid.x, centroid.y, null, null, otherOrg);
        }
        for (var l = 0; l < 2; l++) {
            objifxStpMap.vectorLayerConstraints.addFeatures(markerConstraint[l]);
        }
        this.checkZoomForConstraints(markerConstraint);
    }
    return markerConstraint;
}

IfxStpmapStructures.prototype.deleteConstraint = function (constraintId) {
    var marker;
    for (var i = 0; i < this.markerConstraint.length; i++) {
        if (this.markerConstraint[i].length == 2) {
            marker = this.markerConstraint[i][0];
        }
        else {
            marker = this.markerConstraint[i];
        }
        if (parseInt(marker.data.name.split(" ")[1]) == constraintId) {
            objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraint[i]);
            this.reArrangeConstraints(i, false);
            return;
        }
    }
    for (var i = 0; i < this.markerConstraintOther.length; i++) {
        if (this.markerConstraintOther[i].length == 2) {
            marker = this.markerConstraintOther[i][0];
        }
        else {
            marker = this.markerConstraintOther[i];
        }
        if (parseInt(marker.data.name.split(" ")[1]) == constraintId) {
            objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraintOther[i]);
            this.reArrangeConstraints(i, true);
            return;
        }
    }
}

IfxStpmapStructures.prototype.clearConstraints = function (org) {
    if (org != 1) {
        for (var i = 0; i < this.markerConstraint.length; i++) {
            if (this.markerConstraint[i] != undefined) {
                if (this.markerConstraint[i].length == 2) {
                    objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraint[i][0]);
                    objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraint[i][1]);
                }
                else {
                    objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraint[i]);
                }
            }
        }
    }

    if (org != 0) {
        for (var i = 0; i < this.markerConstraintOther.length; i++) {
            if (this.markerConstraintOther[i].length == 2) {
                objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraintOther[i][0]);
                objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraintOther[i][1]);
            }
            else {
                objifxStpMap.vectorLayerConstraints.removeFeatures(this.markerConstraintOther[i]);
            }
        }
    }
}

IfxStpmapStructures.prototype.reArrangeConstraints = function (from, otherOrg) {
    if (otherOrg == false)
        var markerConstraint = this.markerConstraint;
    else
        var markerConstraint = this.markerConstraintOther;

    for (var i = from; i < markerConstraint.length - 1; i++) {
        markerConstraint[i] = markerConstraint[i + 1];
    }
    markerConstraint.length--;
    objifxStpMap.vectorLayerConstraints.redraw();
}

IfxStpmapStructures.prototype.filterConstraints = function (show, otherOrg) {
    if (otherOrg == false)
        var markerConstraint = this.markerConstraint;
    else
        var markerConstraint = this.markerConstraintOther;
    if (show == false) {
        for (var i = 0; i < markerConstraint.length; i++) {
            if (markerConstraint[i].length == 2) {
                markerConstraint[i][0].style.display = 'none';
                markerConstraint[i][1].style.display = 'none';
            }
            else {
                markerConstraint[i].style.display = 'none';
            }
        }
    }
    else {
        var zoom = objifxStpMap.olMap.getZoom();
        for (var i = 0; i < markerConstraint.length; i++) {
            if (markerConstraint[i].length == 2) {
                if (zoom > 6) {
                    markerConstraint[i][0].style.display = '';
                }
                else {
                    markerConstraint[i][1].style.display = '';
                }
            }
            else {
                markerConstraint[i].style.display = '';
            }
        }
    }
    objifxStpMap.vectorLayerConstraints.redraw();
}
//show/hide unsuitable constraints
IfxStpmapStructures.prototype.filterUnsuitableConstraints = function (show, otherOrg) {
    if (otherOrg == false)
        var markerConstraint = this.unsuitableConst;
    else
        var markerConstraint = this.markerConstraintOther;
    if (show == false) {
        for (var i = 0; i < markerConstraint.length; i++) {
            if (markerConstraint[i].length == 2) {
                markerConstraint[i][0].style.display = 'none';
                markerConstraint[i][1].style.display = 'none';
            }
            else {
                markerConstraint[i].style.display = 'none';
            }
        }
    }
    else {
        var zoom = objifxStpMap.olMap.getZoom();
        for (var i = 0; i < markerConstraint.length; i++) {
            if (markerConstraint[i].length == 2) {
                if (zoom > 6) {
                    markerConstraint[i][0].style.display = '';
                }
                else {
                    markerConstraint[i][1].style.display = '';
                }
            }
            else {
                markerConstraint[i].style.display = '';
            }
        }
    }
    objifxStpMap.vectorLayerConstraints.redraw();
}

IfxStpmapStructures.prototype.setMarkerConstraint = function (constraintId, constraintName, constraintCode, constraintType, topologyType, suitability, X, Y, pointArray, linearRing, otherOrg) {
    var markerName = "CONSTRAINT " + constraintId + " NAME " + constraintName + " CODE " + constraintCode + " SUITABILITY " + suitability + " OTHERORG " + otherOrg + " TYPE " + constraintType;
    if (topologyType == 'point') {
        var imgFile = IfxStpmapCommon.getConstraintImage(constraintType, suitability);
        var markerStyle = { externalGraphic: imgFile, graphicHeight: 18, graphicWidth: 18, title: constraintName };
        var marker = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(X, Y),
                                                    { name: markerName },
                                                    markerStyle);
    }
    else if (topologyType == 'linear') {
        var markerStyle = { strokeColor: IfxStpmapCommon.getSuitabilityColor(suitability, 1), strokeWidth: (objifxStpMap.olMap.getZoom() - 5), title: constraintName };
        var marker = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.LineString(pointArray),
                                                    { name: markerName },
                                                    markerStyle);
    }
    else if (topologyType == 'area') {
        var color = IfxStpmapCommon.getSuitabilityColor(suitability, 1);
        var markerStyle = { strokeColor: color, strokeWidth: 1, fillColor: color, fillOpacity: 0.4, title: constraintName };
        var marker = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Polygon([linearRing]),
                                                    { name: markerName },
                                                    markerStyle);
    }
    return marker;
}

IfxStpmapStructures.prototype.checkZoomForConstraints = function (markerConstraint) {
    var zoom = objifxStpMap.olMap.getZoom();
    if (zoom >= 9) {
        markerConstraint[1].style.display = 'none';
    }
    else {
        markerConstraint[0].style.display = 'none';
    }
}

IfxStpmapStructures.prototype.checkZoomForConstraintsPoint = function (markerConstraint) {
        markerConstraint[0].style.display = 'none';
}

IfxStpmapStructures.prototype.createConstraintDrawControl = function () {
    var self = this;
    var styleMap = new OpenLayers.StyleMap(OpenLayers.Util.applyDefaults(
                        { strokeOpacity: 0.7, strokeColor: "#6a6c95", strokeWidth: 3 },
                        OpenLayers.Feature.Vector.style["default"]));
    this.pointLayer = new OpenLayers.Layer.Vector("Point Layer", { styleMap: styleMap });
    this.lineLayer = new OpenLayers.Layer.Vector("Line Layer", { styleMap: styleMap });
    this.polygonLayer = new OpenLayers.Layer.Vector("Polygon Layer", { styleMap: styleMap });

    objifxStpMap.olMap.addLayers([this.pointLayer, this.lineLayer, this.polygonLayer]);

    createPointConstraint = new OpenLayers.Control.DrawFeature(this.pointLayer, OpenLayers.Handler.Point, {
        title: 'Create point constraint',
        'featureAdded': function (obj) {
            this.deactivate();
            //btnPointConstraint.deactivate();
            self.addFeatureForConstraints(obj, 'point');
        }
    });
    createLinearConstraint = new OpenLayers.Control.DrawFeature(this.lineLayer, OpenLayers.Handler.Path, {
        title: 'Create linear constraint',
        'featureAdded': function (obj) {
            this.deactivate();
            self.addFeatureForConstraints(obj, 'linear');

        }
    });
    createAreaConstraint = new OpenLayers.Control.DrawFeature(this.polygonLayer, OpenLayers.Handler.Polygon, {
        title: 'Create area constraint',
        'featureAdded': function (obj) {
            this.deactivate();
            self.addFeatureForConstraints(obj, 'area');
        }
    });

    objifxStpMap.olMap.addControl(createPointConstraint);
    objifxStpMap.olMap.addControl(createLinearConstraint);
    objifxStpMap.olMap.addControl(createAreaConstraint);
}

IfxStpmapStructures.prototype.createConstraintToolBar = function () {
    constraintToolbarPanel = new OpenLayers.Control.Panel({ displayClass: 'mapconstrainttoolbarpanel' });
    objifxStpMap.olMap.addControl(constraintToolbarPanel);

    btnPointConstraint = new OpenLayers.Control.Button({
        displayClass: 'pointConstraint',
        title: 'Create point constraint',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                createPointConstraint.activate();
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](0, "STRUCTURES");
                }
            },
            'deactivate': function () { createPointConstraint.deactivate(); }
        }
    });

    btnLinearConstraint = new OpenLayers.Control.Button({
        displayClass: 'lineConstraint',
        title: 'Create linear constraint',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {

                var zoom = objifxStpMap.olMap.getZoom();
                if (zoom <= 8) {
                    createLinearConstraint.deactivate();
                    $('#lineconstraintwarningpopup').addClass('show').removeClass('fade');
                    $('#lineconstraintwarningpopup').show();
                    //showWarningPopDialog('Constraint creation is allowed only at zoom level 9 or above.', 'Ok', '', 'close_alert', '', 1, 'info');
                }
                else {
                    createLinearConstraint.activate();
                    $("#MakingcontraintsInfo").show();

                    //alert('INSTRUCTIONS\n\n1) While creating lines ensure to follow the roads as much as possible.\n2) Ensure that all points and connecting lines are always on road.\n3) Select the highest zoom level for better accuracy.\n\nNOTE\nYou can pan the map even though line constraint creation tool is active by holding left mouse button down and moving.');
                }
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](1, "STRUCTURES");
                }
            },
            'deactivate': function () { createLinearConstraint.deactivate(); }
        }
    });

    btnAreaConstraint = new OpenLayers.Control.Button({
        displayClass: 'areaConstraint',
        title: 'Create area constraint',
        type: OpenLayers.Control.TYPE_TOGGLE,
        eventListeners: {
            'activate': function () {
                createAreaConstraint.activate();
                if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
                    objifxStpMap.eventList['DEACTIVATECONTROL'](2, "STRUCTURES");
                }
            },
            'deactivate': function () { createAreaConstraint.deactivate(); }
        }
    });
    constraintToolbarPanel.addControls([btnPointConstraint, btnLinearConstraint, btnAreaConstraint]);
    //constraintToolbarPanel.div.style.display = "none";
}
IfxStpmapStructures.prototype.constraintbyDescrToolbar = function () {
    constraintbydescToolbar = new OpenLayers.Control.Panel({ displayClass: 'mapconstrainttoolbarbydesc' });
    objifxStpMap.olMap.addControl(constraintbydescToolbar);

    btnDescConstraint = new OpenLayers.Control.Button({
        displayClass: 'descConstraint',
        title: 'Create constraint by geometric description',
        type: OpenLayers.Control.TYPE_BUTTON,
        trigger: function () {

            objifxStpMap.setPageState('constraintbydescription');
            if (objifxStpMap.eventList['CONSTRAINTBYDESCRIPTION'] != undefined && typeof (objifxStpMap.eventList['CONSTRAINTBYDESCRIPTION']) === "function") {
                objifxStpMap.eventList['CONSTRAINTBYDESCRIPTION']();
            }

        }
    });

    constraintbydescToolbar.addControls([btnDescConstraint]);
    constraintbydescToolbar.div.style.display = "none";
}

IfxStpmapStructures.prototype.checkLinkOwnerShip = function (constRefrences, allLinks) {
    var res;
    $.ajax({
        url: '/Constraint/CheckLinkOwnerShip',
        type: 'POST',
        cache: true,
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ constRefrences: constRefrences, allLinks: allLinks }),
        success: function (val) {
            //if (val == "Session timeout")
                 //location.reload();
            res = val.Success;
        },
        complete: function () { },
    });
    return res;
}

IfxStpmapStructures.prototype.createConstraint = function (topologyType, geometry, ConstraintReferences, constraintID) {
    this.constraintDetails.topologyType = topologyType;
    this.constraintDetails.geometry = geometry;
    this.constraintDetails.ConstraintReferences = ConstraintReferences;

    var res;
    if ('bydescription' == topologyType) {
        $.ajax({
            url: '/Constraint/SaveLinkDetails',
            type: 'POST',
            cache: true,
            async: false,
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ CONSTRAINT_ID: constraintID, constRefrences: ConstraintReferences }),
            success: function (val) {
                if (val == "Session timeout")
                     //location.reload();
                res = val.Success;
            },
            complete: function () {
                alert('success');
                stopAnimation();
            },
        });
        return;
    }
    else {
        if (topologyType == 'area')
            res = true;
        else
            res = this.checkLinkOwnerShip(ConstraintReferences, true);
    }

    if (res == true) {

        objifxStpMap.setPageState('constraintadded');

        if (objifxStpMap.eventList['CONSTRAINTADDED'] != undefined && typeof (objifxStpMap.eventList['CONSTRAINTADDED']) === "function") {
            objifxStpMap.eventList['CONSTRAINTADDED'](null, topologyType);
        }
    }
    else {
        //stopAnimation();
        $('#ConstraintcreationfailedPopup').show();
        $('#ConstraintcreationfailedPopup').addClass('show').removeClass('fade');


        //showWarningPopDialog('Constraint creation failed. You do not have sufficient privilege to create constraints in the selected roads.', 'Ok', '', 'close_alert', '', 1, 'info');
        objifxStpmapStructures.clearMarkerConstriants();
    }
}

IfxStpmapStructures.prototype.constraintFillDetails = function (obj, feature, topologyType, constraintID) {
    objifxStpmapStructures.clearMarkerConstriants();
    if (topologyType != 'area') {
        var constRef = this.createConstraintReference(obj, feature, topologyType);
        if (constRef == null) {
            $("#Poinsandlinesnotonroad").show();

            $('#Poinsandlinesnotonroad').addClass('show').removeClass('fade');
            //showWarningPopDialog('Points or/and joining lines are not on road. Please make sure created points and joining lines are on road.', 'Ok', '', 'close_alert', '', 1, 'error');
            objifxStpmapStructures.clearMarkerConstriants();
            return false;
        }
    }
    else {
        if (feature.length == 0) {
            objifxStpmapStructures.clearMarkerConstriants();
            return false;
        }
    }
    var geom = this.createConstraintGeometry();

    if (topologyType == 'point') {
        geom.OrdinatesArray[0] = obj.geometry.x;
        geom.OrdinatesArray[1] = obj.geometry.y;
    }
    else if (topologyType == 'linear') {
        for (var i = 0, j = 0; i < obj.geometry.components.length; i++) {
            geom.OrdinatesArray[j++] = obj.geometry.components[i].x;
            geom.OrdinatesArray[j++] = obj.geometry.components[i].y;
        }
    }
    else if (topologyType == 'area') {
        for (var i = 0, j = 0; i < obj.geometry.components[0].components.length; i++) {
            geom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].x;
            geom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].y;
        }
    }
    this.createConstraint(topologyType, geom, constRef, constraintID);
    return true;
}

IfxStpmapStructures.prototype.createConstraintReference = function (obj, features, topologyType) {
    var constRef = [];
    var x, y, _x, _y;
    var nearestFeatures = [];
    if (topologyType == 'point') {
        var isPoint = true;
        var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, obj.geometry.x, obj.geometry.y);
        x = _x = retObject.x1;
        y = _y = retObject.y1;
        nearestFeatures.push(features[retObject.index]);
    }
    else {
        var isPoint = false;
        if (topologyType == 'linear') {
            if (features.length == 0)
                return null;
            var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, obj.geometry.components[0].x, obj.geometry.components[0].y);
            if (retObject.distance > 5)
                return null;
            x = retObject.x1;
            y = retObject.y1;
            nearestFeatures.push(features[retObject.index]);
            retObject = IfxStpmapCommon.findNearestFeatureIndex(features, obj.geometry.components[obj.geometry.components.length - 1].x, obj.geometry.components[obj.geometry.components.length - 1].y);
            if (retObject.distance > 5)
                return null;
            _x = retObject.x1;
            _y = retObject.y1;
            nearestFeatures.push(features[retObject.index]);
        }
        else {
            x = y = _x = _y = null;
        }
    }

    if (topologyType != 'area') {
        var linRefCount = 0;
        for (var i = 0; i < features.length; i++) {
            var linearRef = this.getLinearRefForConstraints(features[i], nearestFeatures, x, y, _x, _y);
            if (linearRef.fromLinearRef != null || linearRef.toLinearRef != null) {
                linRefCount++;
            }

            if (features.length == 1 && linearRef.fromLinearRef != null && linearRef.toLinearRef != null) {
                linRefCount = 2;
            }

            constRef[i] = {
                constLink: features[i].attributes.LINK_ID,
                direction: null,
                easting: Math.round(features[i].geometry.components[0].x),
                fromEasting: Math.round(features[i].geometry.components[0].x),
                fromLinearRef: linearRef.fromLinearRef,
                fromNorthing: Math.round(features[i].geometry.components[0].y),
                isPoint: isPoint,
                linearRef: linearRef.fromLinearRef,
                northing: Math.round(features[i].geometry.components[0].y),
                toEasting: Math.round(features[i].geometry.components[features[i].geometry.components.length - 1].x),
                toLinearRef: linearRef.toLinearRef,
                toNorthing: Math.round(features[i].geometry.components[features[i].geometry.components.length - 1].y)
            };
        }

        if (topologyType == 'linear') {
            if (linRefCount == 2) {
                return constRef;
            }
            else {
                return null;
            }
        }
        return constRef;
    }
}

IfxStpmapStructures.prototype.getLinearRefForConstraints = function (feature, nearestFeatures, x, y, _x, _y) {
    for (var i = 0; i < nearestFeatures.length; i++) {
        if (feature == nearestFeatures[i]) {
            var lrsMeasure = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(x, y), { tolerance: 0.5, details: true });
            if (lrsMeasure == undefined || lrsMeasure == null || lrsMeasure.measure == 1)
                var fromLinearRef = null;
            else
                var fromLinearRef = Math.round(lrsMeasure.length);

            lrsMeasure = LRSMeasure(feature.geometry, new OpenLayers.Geometry.Point(_x, _y), { tolerance: 0.5, details: true });
            if (lrsMeasure == undefined || lrsMeasure == null || lrsMeasure.measure == 1)
                var toLinearRef = null;
            else
                var toLinearRef = Math.round(lrsMeasure.length);

            return { fromLinearRef: fromLinearRef, toLinearRef: toLinearRef };
        }
    }
    return { fromLinearRef: null, toLinearRef: null };
}

IfxStpmapStructures.prototype.createConstraintGeometry = function () {
    return {
        OrdinatesArray: []
    };
}

IfxStpmapStructures.prototype.getConstraintDetails = function () {
    return this.constraintDetails;
}

IfxStpmapStructures.prototype.redrawConstraints = function () {
    this.clearMarkerConstriants();
    objifxStpMap.vectorLayerConstraints.redraw();
}

IfxStpmapStructures.prototype.clearMarkerConstriants = function () {
    this.pointLayer.removeFeatures(this.pointLayer.features);
    this.lineLayer.removeFeatures(this.lineLayer.features);
    this.polygonLayer.removeFeatures(this.polygonLayer.features);
}

IfxStpmapStructures.prototype.addFeatureForConstraints = function (obj, topologyType, constraintID) {
    //startAnimation();

    this.getFeatureForConstraints(obj, topologyType, constraintID, function () {
        //stopAnimation();
    });
}

IfxStpmapStructures.prototype.filterContinuousLinks = function (line, features) {
    if (features.length == 1)
        return features;
    var retObj = IfxStpmapCommon.findNearestFeatureIndex(features, line.geometry.components[0].x, line.geometry.components[0].y);
    if (retObj.index != 0) {
        var temp = jQuery.extend(true, {}, features[0]);
        features[0] = jQuery.extend(true, {}, features[retObj.index]);
        features[retObj.index] = temp;
    }
    retObj = IfxStpmapCommon.findNearestFeatureIndex(features, line.geometry.components[line.geometry.components.length - 1].x, line.geometry.components[line.geometry.components.length - 1].y);
    if (retObj.index == 0) {
        features.push(features[0]);
    }
    else if (retObj.index != features.length - 1) {
        var temp = jQuery.extend(true, {}, features[features.length - 1]);
        features[features.length - 1] = jQuery.extend(true, {}, features[retObj.index]);
        features[retObj.index] = temp;
    }
    var startFeature = jQuery.extend(true, {}, features[0]);
    var endFeature = jQuery.extend(true, {}, features[features.length - 1]);
    var traversedList = [];

    for (var i = 1; i < features.length; i++) {
        var traversedFlag = false;
        for (var j = 0; j < traversedList.length; j++) {
            if (i == traversedList[j]) {
                traversedFlag = true;
                break;
            }
        }
        if (traversedFlag == false) {
            if (features[i].attributes.REF_IN_ID == startFeature.attributes.REF_IN_ID || features[i].attributes.REF_IN_ID == startFeature.attributes.NREF_IN_ID) {
                var bool = IfxStpmapCommon.checkLinkContinuity(i, features, 'NREF_NODE', line);
                if (bool || i == features.length - 1) {
                    startFeature = jQuery.extend(true, {}, features[i]);
                    traversedList.push(i);
                    i = 0;
                }
            }
            else if (features[i].attributes.NREF_IN_ID == startFeature.attributes.REF_IN_ID || features[i].attributes.NREF_IN_ID == startFeature.attributes.NREF_IN_ID) {
                var bool = IfxStpmapCommon.checkLinkContinuity(i, features, 'REF_NODE', line);
                if (bool || i == features.length - 1) {
                    startFeature = jQuery.extend(true, {}, features[i]);
                    traversedList.push(i);
                    i = 0;
                }
            }
            if (traversedList.length > 0 && startFeature.attributes.LINK_ID == endFeature.attributes.LINK_ID) {
                var retFeatures = [features[0]];
                for (var i = 0; i < traversedList.length; i++)
                    retFeatures.push(features[traversedList[i]]);
                return retFeatures;
            }
        }
    }
    return [];
}

IfxStpmapStructures.prototype.getFeatureForConstraints = function (obj, topologyType, constraintID, callback) {
    var self = this;
    if (topologyType == 'point') {
        bounds = (Number(obj.geometry.x) - 100).toString() + ',' + (Number(obj.geometry.y) - 100).toString() + ',' + (Number(obj.geometry.x) + 100).toString() + ',' + (Number(obj.geometry.y) + 100).toString();
    }
    else {
        bounds = obj.geometry.getBounds();
        bounds.left -= 50;
        bounds.bottom -= 50;
        bounds.right += 50;
        bounds.top += 50;
    }
    var cqlFilters = new Array();
    cqlFilters.push('BBOX(GEOM,' + bounds + ')');

    if (topologyType != 'area') {
        objifxStpMap.searchFeaturesByCQL(cqlFilters, function (features) {
            if (features.length > 0) {
                var feat = [];
                if (topologyType == 'point') {
                    var retObject = IfxStpmapCommon.findNearestFeatureIndex(features, obj.geometry.x, obj.geometry.y);
                    obj.geometry.x = retObject.x1;
                    obj.geometry.y = retObject.y1;
                    feat[0] = features[retObject.index];
                }
                else if (topologyType == 'linear') {
                    feat = IfxStpmapCommon.getIntersectedFeatures(obj, features, topologyType);
                    feat = self.filterContinuousLinks(obj, feat);
                }
                if (self.constraintFillDetails(obj, feat, topologyType, constraintID) == false) {
                    if (callback && typeof (callback) === "function") {
                        callback();
                    }
                }
            }
            else {
                //alert('line 961');
                $('#ConstraintcreationfailedPopup').show();
                $('#ConstraintcreationfailedPopup').addClass('show').removeClass('fade');
                if (callback && typeof (callback) === "function") {
                    callback();
                }
            }

        });
    }
    else {
        var res;
        var geom = {
            OrdinatesArray: [],
            ElemArray: [1, 1003, 1],
            sdo_gtype: IfxStpmapCommon.getSdo_gtype('POLYGON'),
            sdo_srid: IfxStpmapCommon.getSdo_srid()
        }
        for (var i = 0, j = 0; i < obj.geometry.components[0].components.length; i++) {
            geom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].x;
            geom.OrdinatesArray[j++] = obj.geometry.components[0].components[i].y;
        }

        $.ajax({
            url: '/Constraint/FindLinksOfAreaConstraint',
            type: 'POST',
            cache: true,
            //async: false,
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ polygonGeometry: geom }),
            success: function (val) {
                if (val == "Session timeout") {
                     //location.reload();
                    stopAnimation();
                }
                else
                    res = val.Success;
                if (res == true) {

                    if (topologyType == 'linear') {

                        $("#mySidenav1").css("display", "none");
                        $("#card-swipe1").css("display", "none");
                        $("#card-swipe2").css("display", "none");
                    }

                    self.createConstraint(topologyType, geom, null, constraintID);
                }
                else {
                    $('#ConstraintcreationfailedPopup').show();

                    $('#ConstraintcreationfailedPopup').addClass('show').removeClass('fade');

                    //showWarningPopDialog('Constraint creation failed. You do not have sufficient privilege to create constraints in the selected roads.', 'Ok', '', 'close_alert', '', 1, 'info');
                    objifxStpmapStructures.clearMarkerConstriants();
                    //alert('Constraint creation failed. You do not have sufficient privilege to create constraints in the selected roads.');
                    //stopAnimation();


                }
            },
            complete: function () {
                //stopAnimation();
            },
        });
    }
}

IfxStpmapStructures.prototype.showAffectedStructures = function (routeId, page, BSortFlag, callback) {
    var Is_NeN = $("#hIs_NEN").val();   //check added for its an nen movment
    var Is_NotfView = false;//chcek added for indicating the call from notification page #12507
    if (Is_NeN == "true" && page =="ROUTELIBRARY") {
        page = "DISPLAYONLY";    //set page value for showing affected structures on the map in edit tab  
    }
    if (page == "NotificationViewOnMap")//check added for detc
    {
        page = "A2BPLANNING";
        Is_NotfView = true;
    }
    if (Is_NotfView) {
        $('#overlay').show();
        startAnimation("Loading unsuitable structures / constraints, please wait...");

    }
    else {
        $('#overlay').show();
        startAnimation();
    }
    //$('.loading').show();
    
    var self = this;
    //var analysisId = $('#StructAnalysisID').val() || 0;
    var analysisId = $('#hf_AnalysisId').val() || 0;
    if (analysisId == 0) {
        try {
            analysisId = $('#NotifAnalysisId').val() || 0;
        }
        catch (err) {
            analysisId = 0;
        }
    }
    var routeParams = IfxStpmapCommon.getRouteAppraisalParams(page);
    $.ajax({
        url: "/RouteAssessment/GetAffectedStructureInfoList",
        type: "POST",
        dataType: "json",
        cache: true,
        data: { routeId: routeId, routeType: routeParams.routeType, checkAppAppraisal: routeParams.checkAppraisal, sortFlag: BSortFlag, analysisId: analysisId },
        success: function (result) {
            structureList = result.result;
            if (Is_NotfView) {
                var structureArray = self.showStructuresOnViewMap(structureList, false, 'AFFECTED');
            }
            else {
                var structureArray = self.showStructures(structureList, false, 'AFFECTED');    
            }
            stopAnimation();
            if (callback && typeof (callback) === "function") {
                callback(structureArray);
            }
        },
        complete: function (result) {
            var str = '<div>' + result.responseText + '</div>';
            var islogin = $(str).find('#isloginPage').val();
            if (islogin == 1) {
                location.href = '../Account/Login';
            }
            stopAnimation();
            $('#overlay').hide();
            //$('.loading').hide();
        }
    });
}

IfxStpmapStructures.prototype.showAffectedConstraints = function (routeId, page, sortFlag) {
    
    var self = this;
    var routeParams;
    var Is_NeN = $("#hIs_NEN").val();   //check added for  nen movment
    var Is_NotfView = false;//chcek added for indicating the call from notification page #12507
    if (page == "NotificationViewOnMap")//check added for detc
    {
        page = "A2BPLANNING";
        Is_NotfView = true;
    }
    if (Is_NeN == "true" && page == "ROUTELIBRARY") {
        routeParams = 0;  //set to 0 for showing affected constraints on the map in edit tab
    }
    else {
        routeParams = IfxStpmapCommon.getRouteAppraisalParams(page);
    }
    if (Is_NotfView) {
        $('#overlay').show();
        startAnimation("Loading unsuitable structures / constraints, please wait...");
        
    }
    else {
        $('#overlay').show();
        startAnimation("Loading affected constraints, please wait...");
        
    }
    var analysisId = 0;
    if ($('#hf_AnalysisId').length > 0)
    {
        analysisId = $('#hf_AnalysisId').val();
    }
    else
    if ($('#NotifAnalysisId').length > 0) {
        analysisId = $('#NotifAnalysisId').val();
    }
    else
    if ($('#StructAnalysisID').length > 0)
    {
        analysisId = $('#StructAnalysisID').val();
    }

    $.ajax({
        url: "/RouteAssessment/GetAffectedConstraintInfoList",
        type: "POST",
        dataType: "json",
        data: { routeId: routeId, routeType: routeParams.routeType, BSortFlag: sortFlag, analysisId: analysisId },
        success: function (result) {
            var constraintList = result.result;
            if (Is_NotfView) {
                self.showConstraintsOnNotif(constraintList, false);
            }
            else {
                self.showConstraints(constraintList, false);
            }
            stopAnimation();
        },
        complete: function (result) {
            var str = '<div>' + result.responseText + '</div>';
            var islogin = $(str).find('#isloginPage').val();
            if (islogin == 1) {
                location.href = '../Account/Login';
            }
            stopAnimation();  
            $('#overlay').hide();
            //$('.loading').hide();
        }
    });
}

IfxStpmapStructures.prototype.createConstraintByDescription = function (ordArray, topologyType, constraintID) {
    switch (topologyType) {
        case 'area':
            var pointArr = new Array();
            for (var i = 0; i < ordArray.length; i += 2) {
                pointArr.push(new OpenLayers.Geometry.Point(parseFloat(ordArray[i]), parseFloat(ordArray[i + 1])));
            }
            var linearRing = new OpenLayers.Geometry.LinearRing(pointArr);
            var obj = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Polygon([linearRing]));

            this.addFeatureForConstraints(obj, "bydescription", constraintID);
            break;
    }
}

IfxStpmapStructures.prototype.addVectorLayers = function (layer) {
    for (var i = 0; i < objifxStpMap.olMap.layers.length; i++) {
        if (objifxStpMap.olMap.layers[i].name == layer)
            return;
    }
    if (layer == "Structures") {
        objifxStpMap.olMap.addLayers([objifxStpMap.vectorLayerStructures]);
        objifxStpMap.olMap.setLayerIndex(objifxStpMap.vectorLayerStructures, 1);
    }
    else {
        objifxStpMap.olMap.addLayers([objifxStpMap.vectorLayerConstraints]);
        objifxStpMap.olMap.setLayerIndex(objifxStpMap.vectorLayerConstraints, 1);
    }
}

IfxStpmapStructures.prototype.structuresOnZoomChange = function (zoom, page, otherOrg) {
    if (zoom <= 6 && page == 'AFFECTED' && otherOrg == false)
        return;

    if (zoom >= 9) {
        var strokeWidth = (zoom - 5) * 2;
        var graphicHeight = 0;
        var graphicWidth = 0;
        var pointDisplay = 'none';
        var lineDisplay = 'block';
    }
    else if (zoom >= 7) {
        var strokeWidth = 0;
        var graphicHeight = zoom + 13;
        var graphicWidth = zoom + 13;
        var pointDisplay = '';
        var lineDisplay = 'none';
    }
    else {
        var strokeWidth = 0;
        var graphicHeight = 0;
        var graphicWidth = 0;
        var pointDisplay = 'none';
        var lineDisplay = 'none';
    }

    if (otherOrg == false) {
        for (var i = 0; i < this.markerStructure.length; i++) {
            var type = this.markerStructure[i][0].data.name.split("TYPE ").pop();
            if (checkboxValue(type, page, false) == true) {
                this.markerStructure[i][0].style.display = pointDisplay;
                for (var j = 0; j < this.markerStructure[i][1].length; j++) {
                    this.markerStructure[i][1][j].style.strokeWidth = strokeWidth;
                    this.markerStructure[i][1][j].style.display = lineDisplay;
                }
            }
        }
    }
    else {
        for (var i = 0; i < this.markerStructureOther.length; i++) {
            var type = this.markerStructureOther[i][0].data.name.split("TYPE ").pop();
            if (checkboxValue(type, page, true) == true) {
                this.markerStructureOther[i][0].style.display = pointDisplay;
                for (var j = 0; j < this.markerStructureOther[i][1].length; j++) {
                    this.markerStructureOther[i][1][j].style.strokeWidth = strokeWidth;
                    this.markerStructureOther[i][1][j].style.display = lineDisplay;
                }
            }
        }
    }
    objifxStpMap.vectorLayerStructures.redraw();
}
IfxStpmapStructures.prototype.structuresOnZoomChangeUnsuitable = function (zoom, page, otherOrg) {
    if (zoom <= 6 && page == 'AFFECTED' && otherOrg == false)
        return;

    if (zoom >= 9) {
        var strokeWidth = (zoom - 5) * 2;
        var graphicHeight = 0;
        var graphicWidth = 0;
        var pointDisplay = 'none';
        var lineDisplay = '';
    }
    else if (zoom >= 7) {
        var strokeWidth = 0;
        var graphicHeight = zoom + 13;
        var graphicWidth = zoom + 13;
        var pointDisplay = '';
        var lineDisplay = 'none';
    }
    else {
        var strokeWidth = 0;
        var graphicHeight = 0;
        var graphicWidth = 0;
        var pointDisplay = 'none';
        var lineDisplay = 'none';
    }

    if (otherOrg == false) {
        for (var i = 0; i < this.markerStructure.length; i++) {
            if (this.markerStructure[i] != undefined) {
                var type = this.markerStructure[i][0].data.name.split("TYPE ").pop();
                    this.markerStructure[i][0].style.display = pointDisplay;
                    for (var j = 0; j < this.markerStructure[i][1].length; j++) {
                        this.markerStructure[i][1][j].style.strokeWidth = strokeWidth;
                        this.markerStructure[i][1][j].style.display = lineDisplay;
                    }
            }
        }
    }

    objifxStpMap.vectorLayerStructures.redraw();
}

IfxStpmapStructures.prototype.constraintsOnZoomChange = function (zoom, page, otherOrg) {
    if (zoom <= 6 && page == 'AFFECTED' && otherOrg == false)
        return;

    if (zoom >= 9) {
        var strokeWidth = zoom - 5;
        var graphicHeight = 40;
        var graphicWidth = 40;
        var pointDisplay = 'none';
        var lineDisplay = '';
    }
    else if (zoom >= 7) {
        var strokeWidth = 0;
        var graphicHeight = 30;
        var graphicWidth = 30;
        var pointDisplay = '';
        var lineDisplay = 'none';
    }
    else {
        var strokeWidth = 0;
        var graphicHeight = 0;
        var graphicWidth = 0;
        var pointDisplay = 'none';
        var lineDisplay = 'none';
    }

    if (otherOrg == false) {
        for (var i = 0; i < this.markerConstraint.length; i++) {
            if (checkboxValue('CONSTRAINT', page, false) == true) {
                if (this.markerConstraint[i].length == 2) {
                    this.markerConstraint[i][0].style.strokeWidth = strokeWidth;
                    this.markerConstraint[i][0].style.display = lineDisplay;
                    this.markerConstraint[i][1].style.display = pointDisplay;
                }
                else {
                    this.markerConstraint[i].style.graphicHeight = graphicHeight;
                    this.markerConstraint[i].style.graphicWidth = graphicWidth;
                }
            }
        }
    }

    else {
        for (var i = 0; i < this.markerConstraintOther.length; i++) {
            if (checkboxValue('CONSTRAINT', page, true) == true) {
                if (this.markerConstraintOther[i].length == 2) {
                    this.markerConstraintOther[i][0].style.strokeWidth = strokeWidth;
                    this.markerConstraintOther[i][0].style.display = lineDisplay;
                    this.markerConstraintOther[i][1].style.display = pointDisplay;
                }
                else {
                    this.markerConstraintOther[i].style.graphicHeight = graphicHeight;
                    this.markerConstraintOther[i].style.graphicWidth = graphicWidth;
                }
            }
        }
    }

    objifxStpMap.vectorLayerConstraints.redraw();
}
IfxStpmapStructures.prototype.constraintsOnZoomChangeUnsuite = function (zoom, page, otherOrg) {
    if (zoom <= 6 && page == 'AFFECTED' && otherOrg == false)
        return;

    if (zoom >= 9) {
        var strokeWidth = zoom - 5;
        var graphicHeight = 40;
        var graphicWidth = 40;
        var pointDisplay = 'none';
        var lineDisplay = '';
    }
    else if (zoom >= 7) {
        var strokeWidth = 0;
        var graphicHeight = 30;
        var graphicWidth = 30;
        var pointDisplay = '';
        var lineDisplay = 'none';
    }
    else {
        var strokeWidth = 0;
        var graphicHeight = 0;
        var graphicWidth = 0;
        var pointDisplay = 'none';
        var lineDisplay = 'none';
    }

    if (otherOrg == false) {
        for (var i = 0; i < this.markerConstraint.length; i++) {
            if (this.markerConstraint[i] != undefined) {
                if (this.markerConstraint[i].length == 2) {
                    this.markerConstraint[i][0].style.strokeWidth = strokeWidth;
                    this.markerConstraint[i][0].style.display = lineDisplay;
                    this.markerConstraint[i][1].style.display = pointDisplay;
                }
                else {
                    this.markerConstraint[i].style.graphicHeight = graphicHeight;
                    this.markerConstraint[i].style.graphicWidth = graphicWidth;
                }
            }
        }
    }

    objifxStpMap.vectorLayerConstraints.redraw();
}

IfxStpmapStructures.prototype.showAllStructures = function (animationText, callback) {
    var self = this;
    var isSuccessInvoked = false;
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();
    if (boundsAndZoom.zoom < 7) {
        showNotification("Zoom-in to view structures and constraints");
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Structures/MyStructureInfoList',
        data: { otherOrg: -1, page: 0, left: Math.floor(boundsAndZoom.bounds.left), right: Math.ceil(boundsAndZoom.bounds.right), bottom: Math.floor(boundsAndZoom.bounds.bottom), top: Math.ceil(boundsAndZoom.bounds.top) },
        beforeSend: function (xhr) {
            if (animationText != null)
                startAnimation(animationText);
        },
        complete: function (jqXHR, textStatus) {
            if (isSuccessInvoked == false) {
                stopAnimation();
                window.location.href = '../Account/Login'
            }
        },
        success: function (result) {
            isSuccessInvoked = true;
            length = result.result;
            var resultArr = new Array();
            for (var i = 0; i <= length / 1000; i++) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    async: false,
                    cache: true,
                    url: '../Structures/MyStructureInfoList',
                    data: { otherOrg: -1, page: i + 1, left: Math.floor(boundsAndZoom.bounds.left), right: Math.ceil(boundsAndZoom.bounds.right), bottom: Math.floor(boundsAndZoom.bounds.bottom), top: Math.ceil(boundsAndZoom.bounds.top) },
                    beforeSend: function (xhr) {
                    },
                    success: function (result) {
                        resultArr = resultArr.concat(result.result);
                        stopAnimation();
                        if (document.getElementById("filters") != undefined) {
                            if (document.getElementById("filters").style.width != "0") {
                                $("#overlay").show();
                            }
                        }
                    },
                    error: function () { stopAnimation(); },
                    complete: function (result) {
                        //stopAnimation();
                        $("#overlay").hide();
                    }
                });

            }
            var structureArray = self.showStructures(resultArr, true, 'AFFECTED', boundsAndZoom.zoom);
            if (callback && typeof (callback) === "function") {
                callback(structureArray);
            }
        }
    });
}

IfxStpmapStructures.prototype.showAllConstraints = function (animationText) {
    var self = this;
    var boundsAndZoom = objifxStpMap.getCurrentBoundsAndZoom();
    if (boundsAndZoom.zoom < 7) {
        showNotification("Zoom-in to view structures and constraints");
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Constraint/GetConstraintListForOrg',
        data: { otherOrg: -1, left: Math.floor(boundsAndZoom.bounds.left), right: Math.ceil(boundsAndZoom.bounds.right), bottom: Math.floor(boundsAndZoom.bounds.bottom), top: Math.ceil(boundsAndZoom.bounds.top) },
        beforeSend: function (xhr) {
            if (animationText != null)
                startAnimation(animationText);
        },
        success: function (result) {
            if (result.Success == false) {
                window.location.href = '../Account/Login'
            }
            else {
                self.showConstraints(result.result, true);
            }
            stopAnimation();
        },
        complete: function (result) {
            stopAnimation();
        }
    });
}
//show affected structures in notification map 
IfxStpmapStructures.prototype.showStructuresOnViewMap = function (structureList, otherOrg, page, zoom) {
   
    if (structureList.length == 0)
        return;

    if (zoom == undefined || zoom == null) {
        zoom = objifxStpMap.olMap.getZoom();
    }

    var markerStructure = [];
    this.clearStructureClassList();
    var k = 0;//initializing variable to append correct markers
    for (var i = 0; i < structureList.length; i++) {
        //filter added for showing unsuitable structures on notification viw map click #1257 #4th point
        if (structureList[i].suitability != undefined) {
            var finalSuitability = IfxStpmapCommon.getFinalSuitability(structureList[i].suitability);
            if (finalSuitability == null || finalSuitability == undefined)
                finalSuitability = '';
            else
                finalSuitability = finalSuitability.toLowerCase();
            if (finalSuitability.toLowerCase() == 'unsuitable' || finalSuitability.toLowerCase()=='not specified') {
                if (structureList[i].pointGeometry != null) {
                    markerStructure[k] = [];
                    markerStructure[k][0] = this.setMarkerStructure(structureList[i].structureId, structureList[i].structureName, structureList[i].structureCode, structureList[i].structureClass, 'point',
                        structureList[i].pointGeometry.OrdinatesArray[3], structureList[i].pointGeometry.OrdinatesArray[4], null, structureList[i].suitability, otherOrg, zoom);
                    markerStructure[k][1] = this.drawStructuresLine(structureList[i], zoom, otherOrg);

                    objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[k][0]);
                    objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[k][1]);

                    if (zoom > 8) {
                        markerStructure[k][0].style.display = 'none';
                    }
                    else {
                        for (var j = 0; j < markerStructure[k][1].length; j++) {
                            markerStructure[k][1][j].style.display = 'none';
                        }
                    }
                    this.setFilterList(structureList[i].structureClass, i);
                    k++;
                }
            }
        } else {
            var finalSuitability = IfxStpmapCommon.getFinalSuitability(structureList[i].Suitability);
            if (finalSuitability == null || finalSuitability == undefined)
                finalSuitability = '';
            else
                finalSuitability = finalSuitability.toLowerCase();
            if (finalSuitability.toLowerCase() == 'unsuitable' || finalSuitability.toLowerCase() == 'not specified') {
                if (structureList[i].PointGeometry != null) {
                    markerStructure[k] = [];
                    markerStructure[k][0] = this.setMarkerStructure(structureList[i].StructureId, structureList[i].StructureName, structureList[i].StructureCode, structureList[i].StructureClass, 'point',
                        structureList[i].PointGeometry.OrdinatesArray[3], structureList[i].PointGeometry.OrdinatesArray[4], null, structureList[i].Suitability, otherOrg, zoom);
                    markerStructure[k][1] = this.drawStructuresLine(structureList[i], zoom, otherOrg);

                    objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[k][0]);
                    objifxStpMap.vectorLayerStructures.addFeatures(markerStructure[k][1]);

                    if (zoom > 8) {
                        markerStructure[k][0].style.display = 'none';
                    }
                    else {
                        for (var j = 0; j < markerStructure[k][1].length; j++) {
                            markerStructure[k][1][j].style.display = 'none';
                        }
                    }
                    this.setFilterList(structureList[i].StructureClass, i);
                    k++;
                }
            }
        }
    }
    objifxStpMap.vectorLayerStructures.redraw();

    if (otherOrg == false)
        this.markerStructure = markerStructure;
    else
        this.markerStructureOther = markerStructure;

    this.addVectorLayers("Structures");

    return this.structureClassList;
}
IfxStpmapStructures.prototype.getInstantAnalysis = function (routeId, checkFor, callback) {
    startAnimation("Loading affected " + (checkFor == 0 ? "structures" : "constraints") + ", please wait...");
    var self = this;
    var routePartObj = objifxStpMap.getRoutePart();
    $.ajax({
        url: "/RouteAssessment/GetInstantAnalysis",
        type: "POST",
        dataType: "json",
        cache: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ routePartId: routeId, routePart: routePartObj, CheckFor: checkFor }),
        success: function (result) {
            if (checkFor == 0) {
                structureList = result.result;
                var structureArray = self.showStructures(structureList, false, 'AFFECTED');
                if (callback && typeof (callback) === "function") {
                    callback(structureArray);
                }
            }
            else {
                var constraintList = result.result;
                self.showConstraints(constraintList, false);
            }
            stopAnimation();
        },
        complete: function (result) {
            var str = '<div>' + result.responseText + '</div>';
            var islogin = $(str).find('#isloginPage').val();
            if (islogin == 1) {
                location.href = '../Account/Login';
            }
            stopAnimation();
        }
    });
}