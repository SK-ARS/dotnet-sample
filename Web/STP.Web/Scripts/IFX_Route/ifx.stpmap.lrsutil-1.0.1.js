function extendClass(child, supertype) {
    child.prototype.__proto__ = supertype.prototype;
}

/**
* APIFunction: multiSort
*      for OLNetwork
*  sort multi dimension array on the any index given
* Parameters:
*      array - {Array}
*      i - {integer}  column index for which sorting to be done.
* Returns:
*      {Array}
*/
var multiSort = function (array, index) {
    // TODO : it rerurns index column as first column, should retain the column index
    // Written By: WillyDuitt@hotmail.com | 03-10-2005 \\;
    for (var i = 0; i < array.length; i++) {
        var temp = array[i].splice(index, 1);
        array[i].unshift(temp);
    } return array.sort();
};

/**
* APIFunction: removeDuplicate
*      for OLNetwork
*  reomve the duplicate from the array
* Parameters:
*      array - {Array}
* Returns:
*      {Array}
*/
removeDuplicate = function (oldArray) {
    var r = new Array();
    o: for (var i = 0, n = oldArray.length; i < n; i++) {
        for (var x = 0, y = r.length; x < y; x++) {
            if (r[x] == oldArray[i]) {
                continue o;
            }
        }
        r[r.length] = oldArray[i];
    }
    return r;
};

/**
* APIMethod: intersection
*      for OLNetwork, TODO should be part of Geometry
* Returns the intersection of linestring geometry
* Parameters:
*      line - OpenLayers.Geometry.LineString
*      geom - OpenLayers.Geometry with which intersetion is calculated
*      options - Valid options
*              tolerance: float
* Returns:
*      Array of {<OpenLayers.Geometry.Point>}
*/
intersection = function (line, geom, options) {
    if (options) {
        options.point = true;
    } else {
        options = {};
    }
    var intersections = [];
    if (geom instanceof OpenLayers.Geometry.LineString) {
        if (line.intersects(geom)) {
            var targetParts, sourceParts;
            var seg1, seg2;
            var point;
            targetParts = line.getVertices();
            sourceParts = geom.getVertices();
            for (var i = 0; i < targetParts.length - 1; i++) {
                seg1 = { x1: targetParts[i].x, y1: targetParts[i].y, x2: targetParts[i + 1].x, y2: targetParts[i + 1].y };
                for (var j = 0; j < sourceParts.length - 1; j++) {
                    seg2 = { x1: sourceParts[j].x, y1: sourceParts[j].y, x2: sourceParts[j + 1].x, y2: sourceParts[j + 1].y };
                    point = OpenLayers.Geometry.segmentsIntersect(seg1, seg2, options);
                    if (point) {
                        if (point == true) { // geometry coincident
                            var p1 = new OpenLayers.Geometry.Point(seg1.x1, seg1.y1);
                            var p2 = new OpenLayers.Geometry.Point(seg1.x2, seg1.y2);
                            intersections.push(p1, p2);
                        } else {
                            intersections.push(point);
                        }
                    }
                }
            }
        }
    }
    return intersections;
};

/**
* APIMethod: LRSMeasure
*      to measure of point on linestring
* Parameters:
*      line - <OpenLayers.Geometry.LineString>
*      point - <OpenLayers.Geometry.Point>
* Returns:
*      {float}, between 0 and 1
*/
LRSMeasure = function (line, point, options) {
    var details = options && options.details;
    var tolerance = options && options.tolerance ? options.tolerance : 0.0;
    var seg = {};
    var length = 0.0;
    var dist = 0.0;
    var measureCalculated = false;
    var part1Points = [];
    var part2Points = [];
    var totalLength = line.getLength();
    var result = {};
    for (var i = 0; i < line.components.length - 1; i++) {
        seg = {
            x1: line.components[i].x,
            y1: line.components[i].y,
            x2: line.components[i + 1].x,
            y2: line.components[i + 1].y
        };
        dist = OpenLayers.Geometry.distanceToSegment(point, seg).distance;
        if ((dist < tolerance) && !measureCalculated) {
            length += line.components[i].distanceTo(point);
            measureCalculated = true;
            //return length/totalLength;                           
            part1Points.push(line.components[i], point);
            part2Points.push(point, line.components[i + 1]);
        } else if (!measureCalculated) {
            length += line.components[i].distanceTo(line.components[i + 1]);
            part1Points.push(line.components[i]);
        } else {
            part2Points.push(line.components[i + 1]);
        }
    }
    length = parseInt(length);
    if (details) {
        result = {
            length: length,
            measure: length / totalLength,
            subString1: new OpenLayers.Geometry.LineString(part1Points),
            subString2: new OpenLayers.Geometry.LineString(part2Points)
        };
        return result;
    } else {
        return length / totalLength;
    }
};

/**
* APIMethod: LRSSubstring
* Returns the part of line for given start and end measure
* Parameters:
* line - {<OpenLayers.Geometry.LineString>}
* start - start measure between 0-1
* end - end measure between 0-1
* Retruns:
* {<OpenLayers.Geometry.LineString>}
*/
LRSSubstring = function (line, start, end) {
    var length = line.getLength();
    var startPos = start * length;
    var endPos = end * length;
    var points = [];
    var curPos = 0;
    var subString = false;
    reverseFlag = false;
    if (start == 0 && end == 1) {
        subString = line;
    } else if (start >= 0 && end <= 1 && start != end) {
        if (start > end) {
            var tmp = end;
            end = start;
            start = tmp;
            reverseFlag = true;
        }
        for (var i = 0; i < line.components.length; i++) {
            var lastPos = curPos;
            if (i > 0) {
                curPos += line.components[i].distanceTo(line.components[i - 1]);
            }
            if (curPos > startPos && curPos < endPos) {
                if (points.length == 0) {
                    var segLength = line.components[i].distanceTo(line.components[i - 1]);
                    var firstPoint = LRSLocatePointOnSegment(
                            line.components[i - 1],
                            line.components[i],
                            (startPos - lastPos) / segLength
                    );
                    points.push(firstPoint);
                }
                points.push(line.components[i]);
            } else if (curPos >= endPos) {
                var segLength = line.components[i].distanceTo(line.components[i - 1]);
                if (points.length == 0) {
                    var firstPoint = LRSLocatePointOnSegment(
                            line.components[i - 1],
                            line.components[i],
                            (startPos - lastPos) / segLength
                    );
                    points.push(firstPoint);
                }
                var endPoint = LRSLocatePointOnSegment(
                    line.components[i - 1],
                    line.components[i],
                    (endPos - lastPos) / segLength
                );
                points.push(endPoint);
                break;
            }
        }
        if (reverseFlag) {
            points.reverse();
        }
        subString = new OpenLayers.Geometry.LineString(points);
    }
    return subString;
};

/**
* Method: LRSSubstringBetweenPoints
* Get the part line between two points
* Parameters:
* line - {<OpenLayers.Geometry.LineString>}
* startPoint - {<OpenLayers.Geometry.Point>}
* endPoint - {<OpenLayers.Geometry.Point>}
* tolerance - float
* Retruns:
* {<OpenLayers.Geometry.LineString>}
*/
LRSSubstringBetweenPoints = function (line, startPoint, endPoint, tolerance) {
    var start = LRSMeasure(line, startPoint, { tolerance: tolerance });
    var end = LRSMeasure(line, endPoint, { tolerance: tolerance });
    var traceLine;
    if (start < end) {
        traceLine = LRSSubstring(line, start, end);
    } else {
        traceLine = LRSSubstring(line, end, start);
        traceLine.components.reverse();
    }
    return traceLine;
};


LRSLocatePointOnSegment = function (point1, point2, position) {
    if (position > 1 && position - 1 < 0.00000000002) {
        position = 1;
    }
    var point = false;
    if (position >= 0 && position <= 1) {
        x1 = point1.x;
        y1 = point1.y;
        x2 = point2.x;
        y2 = point2.y;
        x = x1 + (x2 - x1) * position;
        y = y1 + (y2 - y1) * position;
        point = new OpenLayers.Geometry.Point(x, y);
    }
    return point;
};
LRSLocatePoint = function (line, position) {
    var curPos = 0;
    var point = false;
    for (var i = 0; i < line.components.length; i++) {
        lastPos = curPos;
        curPos += line.componets[i].distanceto(line.components[i + 1]);
        if (curPos > position) {
            point = LRSLocatePointOnSegment(
                        line.componets[i],
                        line.componets[i + 1],
                        position - lastPos
                );
        }
    }
    return point;
};

Array.prototype.isKey = function () {
    for (i in this) {
        if (this[i] === arguments[0])
            return true;
    };
    return false;
};

Array.prototype.getIndexOf = function () {
    for (i in this) {
        if (this[i] === arguments[0])
            return i;
    };
    return false;
};

/**
* APIMethod: createFormat
* create a format object given a format name
* Parameters:
* format - {string}
* Returns:
* {OpenLayers.Format}
*/
createFormat = function (format) {
    if (format.toLowerCase() == "geojson") {
        return new OpenLayers.Format.GeoJSON();
    } else if (format.toLowerCase == "gml") {
        return new OpenLayers.Format.GML();
    } else {
        return null;
    }
};

/**
* APIMethod: createProtocol
* create a protocol object for a given protocol name
* Parameters:
* protocol - {string}
* url - {string}
* fromat - {OpenLayers.Format}
* Retruns:
* {OpenLayers.Protocol}
*/
createProtocol = function (protocol, url, format, params) {
    if (protocol.toLowerCase() == "http") {
        return new OpenLayers.Protocol.HTTP({ url: url, format: format, params: params });
    } else {
        return null;
    }
};

/**
* APIMethod: createStrategy
* create a format object given a strategy name
* Parameters:
* strategy - {string}
* Returns:
* {OpenLayers.Strategy}
*/
createStrategy = function (strategy) {
    if (strategy.toLowerCase() == "fixed") {
        return new OpenLayers.Strategy.Fixed();
    } else if (strategy.toLowerCase == "bbox") {
        return new OpenLayers.Strategy.BBOX();
    } else {
        return null;
    }
};

/**
* APIMethod: getStrategyByClassName
* Paramaeters:
* layer - OpenLayers.Layer.Vector
* className - {string}
* Returns:
* {OpenLayers.Strategy}
*/
getStrategyByClassName = function (layer, className) {
    if (layer.strategies && layer.strategies.length > 0) {
        for (var i = 0; i < layer.strategies.length; i++) {
            if (layer.strategies[i].CLASS_NAME == className) {
                return layer.strategies[i];
            }
        }
    }
    return null;
};

/**
* APIMethod: clone
* Parameters:
* obj - {object}
* Returns: {object}
*/
clone = function (obj) {
    if (obj == null || typeof (obj) != 'object')
        return obj;
    var temp = obj.constructor(); // changed
    for (var key in obj)
        temp[key] = clone(obj[key]);
    return temp;
};

/**
* APIMethod: cloneFeature
* as OpenLayers clone is shallow copy, this create a copy of geometry,
* attrubutes, fid, layer and state also
* Parameters:
* feature - {<OpenLayers.Feature.Vector>}
* Returns: {<OpenLayers.Feature.Vector>}
*/
cloneFeature = function (feature) {
    newFeature = feature.clone();
    newFeature.fid = feature.fid;
    newFeature.layer = feature.layer;
    newFeature.state = feature.state;
    wktGeometry = feature.geometry.toString();
    newFeature.geometry = new OpenLayers.Geometry.fromWKT(wktGeometry);
    newFeature.attributes = clone(feature.attributes);
    return newFeature;
};

/**
* APIMethod: offset
* offset the arc by creating a new geometry parallel to the given geometry
* Parameters:
* geometry - {<OpenLayers.Geometry>}
* distance - {float}
* side - {string} "left", "right"
* Returns: {<OpenLayers.Geometry.LineString>}
*/

GeomLib = {};
GeomLib.offset = function (geometry, distance) {
    var glib = GeomLib;
    var offset = false;
    if (geometry instanceof OpenLayers.Geometry.LineString) {
        var segments = glib.getSegments(geometry);
        var segmentCount = segments.length;
        var segmentOffsets = [];
        for (var i = 0; i < segmentCount ; i++) {
            segmentOffsets.push(glib.offsetSegment(segments[i], distance));
        }
        var segmentIntersections = [];
        for (var i = 0; i < segmentCount - 1 ; i++) {
            segmentIntersections.push(
            glib.segmentLinesIntersection(segmentOffsets[i], segmentOffsets[i + 1])
            );
        }
        //add start and end point
        segmentIntersections.unshift(new OpenLayers.Geometry.Point(
        segmentOffsets[0].x1, segmentOffsets[0].y1));
        segmentIntersections.push(new OpenLayers.Geometry.Point(
        segmentOffsets[segmentCount - 1].x2, segmentOffsets[segmentCount - 1].y2));
        offset = new OpenLayers.Geometry.LineString(segmentIntersections);
    }
    return offset;
};

GeomLib.getSegments = function (line) {
    var numSeg = line.components.length - 1;
    var segments = new Array(numSeg), point1, point2;
    for (var i = 0; i < numSeg; ++i) {
        point1 = line.components[i];
        point2 = line.components[i + 1];
        segments[i] = {
            x1: point1.x,
            y1: point1.y,
            x2: point2.x,
            y2: point2.y
        };
    }
    return segments;
};

GeomLib.offsetSegment = function (segment, distance) {
    // the approach is taken from JTS
    var sideSign = distance < 0 ? -1 : 1; //-1 is right and +1 is left
    var dx = (segment.x2 - segment.x1);
    var dy = (segment.y2 - segment.y1);
    var length = Math.sqrt(dx * dx + dy * dy);
    var offsetX = sideSign * dy / length * distance;
    var offsetY = sideSign * dx / length * distance;
    var offset = {
        x1: segment.x1 - offsetX,
        y1: segment.y1 + offsetY,
        x2: segment.x2 - offsetX,
        y2: segment.y2 + offsetY
    };
    return offset;
};

GeomLib.segmentLinesIntersection = function (seg1, seg2) {
    var glib = GeomLib;
    var m1 = glib._segSlope(seg1);
    var m2 = glib._segSlope(seg2);
    var c1 = glib._segLineIntercept(seg1);
    var c2 = glib._segLineIntercept(seg2);
    var x, y;
    if (m1 == Number.POSITIVE_INFINITY && m2 == Number.POSITIVE_INFINITY) {
        return false;
    } else if (m1 != Number.POSITIVE_INFINITY && m2 != Number.POSITIVE_INFINITY) {
        x = (c2 - c1) / (m1 - m2);
        y = m1 * x + c1;
    } else {
        if (m1 == Number.POSITIVE_INFINITY) {
            y = c1;
            x = (y - c2) / m2;
        } else {
            y = c2;
            x = (y - c1) / m1;
        }
    }
    return new OpenLayers.Geometry.Point(x, y);
}

GeomLib._segSlope = function (seg) {
    if ((seg.y2 - seg.y1) != 0) {
        return (seg.y2 - seg.y1) / (seg.x2 - seg.x1)
    } else {
        return Number.POSITIVE_INFINITY;
    }
};

GeomLib._segLineIntercept = function (seg) {
    return seg.y1 - GeomLib._segSlope(seg) * seg.x1;
};

GeomLib.geometryWithDefinition = function (geom, definition) {
    var outGeom = false;
    if (definition.shape && definition.shape.process) {
        switch (definition.shape.process) {
            case "Buffer":
                break;
            case "FixedRectangle":
                outGeom = GeomLib.rectangleWithDefinition(geom, definition);
                break;
            case "Offset":
                outGeom = GeomLib.offsetWithDefinition(geom, definition);
                break;
            default:
                outGeom = false;
        }
    }
    return outGeom;
};

GeomLib.offsetWithDefinition = function (geom, definition) {
    var offsetGeom;
    if (definition.shape && definition.shape.distanceX && definition.shape.distanceX != 0) {
        offsetGeom = GeomLib.offset(geom, definition.shape.distanceX);
    } else if (definition.shape.distanceX == 0) {
        offsetGeom = geom;
    } else {
        return false;
    }
    return offsetGeom;
};

GeomLib.rectangleWithDefinition = function (geom, definition) {
    var rectangle;
    var points = [];
    if (!(geom instanceof OpenLayers.Geometry.Point)) {
        var center = geom.getCentroid();
    } else {
        center = geom;
    }
    var dx, dy, x, y;
    dx = definition.position && definition.position.deltaX ? definition.position.deltaX : 0;
    dy = definition.position && definition.position.deltaY ? definition.position.deltaY : 0;
    x = center.x + dx;
    y = center.y + dy;
    points.push(new OpenLayers.Geometry.Point((x - definition.shape.distanceX), (y - definition.shape.distanceY)));
    points.push(new OpenLayers.Geometry.Point((x - definition.shape.distanceX), (y + definition.shape.distanceY)));
    points.push(new OpenLayers.Geometry.Point((x + definition.shape.distanceX), (y + definition.shape.distanceY)));
    points.push(new OpenLayers.Geometry.Point((x + definition.shape.distanceX), (y - definition.shape.distanceY)));
    points.push(new OpenLayers.Geometry.Point((x - definition.shape.distanceX), (y - definition.shape.distanceY)));
    rectangle = new OpenLayers.Geometry.Polygon(new OpenLayers.Geometry.LinearRing(points));
    if (definition.position.rotation && definition.position.rotation != 0) {
        rectangle.rotate(definition.position.rotation, new OpenLayers.Geometry.Point(x, y));
    }
    return rectangle;
}