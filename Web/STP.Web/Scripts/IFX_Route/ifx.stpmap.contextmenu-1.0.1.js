function getOffset(el) {
    var _x = 0;
    var _y = 0;
    while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
        _x += el.offsetLeft;// - el.scrollLeft;
        _y += el.offsetTop;// - el.scrollTop;
        el = el.offsetParent;
    }
    return { top: _y, left: _x };
}

function createContextMenu() {
    
    $('#map').IntellizenzContext({
        columnType: [
            //0 - start flag
            {
                colDetails: [{ type: 'image', name: 'image', src: '/Content/assets/Icons/map-startpoint.svg' }, { type: 'label', name: 'Direction from here', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 0);
                }
            },
            //1 - end flag
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/map-endpoint.svg' }, { type: 'label', name: 'Direction to here', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 1);
                }
            },
            //2 - alternate start flag
            {
                colDetails: [{ type: 'image', name: 'image', src: '/Content/assets/Icons/map-startpoint-alt.svg' }, { type: 'label', name: 'Direction from here', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 0);
                }
            },
            //3 - alternate end flag
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/map-endpoint-alt.svg' }, { type: 'label', name: 'Direction to here', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 1);
                }
            },
            //4 - add waypoint
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/images/circular-add-icon.svg' }, { type: 'label', name: 'Add waypoint', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 2);
                }
            },
            //5 - add viapoint
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/images/anchorpointW.svg' }, { type: 'label', name: 'Add stopping point', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 3);
                }
            },
            //6 - delete waypoint
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/delete-map.svg' }, { type: 'label', name: 'Delete waypoint', src: '' }],
                seperator: true,
                func: function (e) {
                    deleteMouseOverWaypoint();
                }
            },
            //7 - delete viapoint
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/delete-map.svg' }, { type: 'label', name: 'Delete viapoint', src: '' }],
                seperator: true,
                func: function (e) {
                    deleteMouseOverWaypoint();
                }
            },
            //8 - diverge point
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/diverge from.svg' }, { type: 'label', name: 'Diverge from', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 5);
                }
            },
            //9 - merge point
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/Merge to.svg' }, { type: 'label', name: 'Merge to', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 4);
                }
            },
            //10 - edit point A
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/Images/adv_edit_30x30.png' }, { type: 'label', name: 'Edit point A', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 7);
                }
            },
            //11 - edit point B
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/Images/adv_edit_30x30.png' }, { type: 'label', name: 'Edit point B', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    searchSegmentByXY((parseInt(e.xcord) - parseInt(offset.left)).toString(), (parseInt(e.ycord) - parseInt(offset.top)).toString(), 8);
                }
            },
            //12 - add annotation
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/images/Path 380.svg' }, { type: 'label', name: ' Add annotation', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    var pix = { x: (parseInt(e.xcord) - parseInt(offset.left)).toString(), y: (parseInt(e.ycord) - parseInt(offset.top)).toString() };
                    createAnnotation(pix);
                }
            },
            //13 - delete annotation
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/Images/delete.png' }, { type: 'label', name: 'Delete annotation ', src: '' }],
                seperator: false,
                func: function (e) {
                    deleteAnnotation();
                }
            },
            //14 - view details
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/Images/details.svg' }, { type: 'label', name: 'View details', src: '' }],
                func: function (e) {
                    showDetails();
                }
            },
            
            //15 - show structure contacts
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/images/Group 294.svg' }, { type: 'label', name: 'Show structure contacts', src: '' }],
                func: function (e) {
                    showStructureContact();
                }
            },
            //16 - show road contacts
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/images/Group 294.svg' }, { type: 'label', name: 'Show Road contacts', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    var pix = { x: (parseInt(e.xcord) - parseInt(offset.left)).toString(), y: (parseInt(e.ycord) - parseInt(offset.top)).toString() };
                    ShowRoadContacts(pix);

                }
            },
            //17 - show road owner
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/road-owner-details.svg' }, { type: 'label', name: 'Road owner details', src: '' }],
                seperator: true,
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    var pix = { x: (parseInt(e.xcord) - parseInt(offset.left)).toString(), y: (parseInt(e.ycord) - parseInt(offset.top)).toString() };
                    ShowRoadOwnerContacts(pix);
                }
            },
            //18 - zoom in
            {
                colDetails: [{
                    type: 'image', name: 'image', src: '../../Content/assets/Icons/zoom-in.svg' }, { type: 'label', name: 'Zoom in', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    var pix = { x: (parseInt(e.xcord) - parseInt(offset.left)).toString(), y: (parseInt(e.ycord) - parseInt(offset.top)).toString() };
                    setCenterAndZoom(pix, false);
                }
            },
            //19 - zoom out
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/zoom-out.svg' }, { type: 'label', name: 'Zoom out', src: '' }],
                func: function (e) {
                    zoomOut();
                }
            },
            //20 - center here
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/assets/Icons/center-here.svg' }, { type: 'label', name: 'Centre here', src: '' }],
                func: function (e) {
                    var offset = getOffset(document.getElementById('map'));
                    var pix = { x: (parseInt(e.xcord) - parseInt(offset.left)).toString(), y: (parseInt(e.ycord) - parseInt(offset.top)).toString() };
                    setCenter(pix, false);
                },
            },
            //21 - Show related movement list
            {
                colDetails: [{ type: 'image', name: 'image', src: '../../Content/Images/details.svg' }, { type: 'label', name: 'Show related movements', src: '' }],
                func: function (e) {
                    ShowRelatedMovm();
                }
            },
           
            
        ]
    });

}