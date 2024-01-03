//function for insert new waypoints
function insertwaypoint(type) {
    var rowCnt = $('#sortable li').length;
    var rowID;
    var gblstring = "";
    if (rowCnt <= 99) {
        if (rowCnt == 0) {
            rowID = 0;
        }
        else {
            rowID = $('#sortable li:last').attr('id');
        }

        if (rowCnt == 99) {
            $("#spn_WayPoint").hide();
            $("#spn_ViaPoint").hide();
        }

        var name = document.getElementsByName("lblwaypoint");
        rowID = (name.length) + 1;
        if (type == 3) {
            var lblRow = "Viapoint" + (rowID);
            var tooltip = "Viapoint #" + (rowID);
        }
        else {
            var lblRow = "Waypoint" + (rowID);
            var tooltip = "Waypoint #" + (rowID);
        }
        var cnt = parseInt(rowID) + 1;
        if (type == null || type == 1 || type == 2) {
            var str = "<li id=" + rowID + " name='trwaypoint' class='ui-state-default py-1'><div style='display:none;'><span name='lblwaypoint'>" + lblRow + "</span></div> <div class='searchtableleft editor-field d-flex'><div class='sortOrderlist'><span class='waypointicon waypointclass' name='waypointicon'>" + rowID + "</span></div> <div class='flex-grow-1'><input style='color:white' class='serchlefttxt1one AutocompleteElement'  url='../QAS/Search' origin='a2bLeft' pointType ='2' pointNo = '" + cnt + "' placeholder='" + lblRow + "' type='text' id='" + lblRow + "' name='txtwaypoint' /></div> <div class='canwaypoint ifx-waypoint-remove'>×</div></li>";
        }
        else if (type == 3) {
            var str = "<li id=" + rowID + " name='trwaypoint' class='ui-state-default py-1'><div style='display:none;'><span name='lblwaypoint'>" + lblRow + "</span></div> <div class='searchtableleft editor-field d-flex'><div class='sortOrderlist'><span class='waypointicon_green waypointclass' name='waypointicon'>" + rowID + "</span></div> <div class='flex-grow-1'><input style='color:white' class='serchlefttxt1one AutocompleteElement'  url='../QAS/Search' origin='a2bLeft' pointType ='3' pointNo = '" + cnt + "'  placeholder='stopping point " + rowID + "' type='text' id='" + lblRow + "' name='txtwaypoint' /></div> <div class='canwaypoint ifx-waypoint-remove'>×</div></li>";
        }
        $("#sortable").append(str);
        $("#spn_WayPoint").hide();
        $("#spn_ViaPoint").hide();
    }

}
$('body').on('click', '.ifx-waypoint-remove', function (e) {
    removewaypoint(this);
});
//function for inserting intermediate waypoints
//function insertintermediatewaypoint(index) {
//    var rowCnt = $('#sortable li').length;
//    for (var i = index; i < rowCnt; i++) {
//        var id = document.getElementsById("Waypoint" + index + 1);
//        if (id != null) {

//        }
//    }

//    var name = document.getElementsByName("lblwaypoint");
//    var i = len - 1;
//    if (len != 0) {
//        var str = name[i];
//        var id = name[i].innerHTML;
//        var value = $('#' + id).val();
//        if (value == '')
//            $('#sortable').find('li').eq(i).remove();

//    }
//}

//function for remove waypoints
function removewaypoint(a) {
    var li = $(a).closest('li');
    li.remove();
    $("#spn_WayPoint").show();
    $("#spn_ViaPoint").hide();
    var name = document.getElementsByName("lblwaypoint");
    var trname = document.getElementsByName("trwaypoint");
    var txtname = document.getElementsByName("txtwaypoint");
    var waypointicon = document.getElementsByName("waypointicon");

    var i = 0;
    for (i = 0; i < name.length; i++) {

        name[i].innerHTML = "Waypoint" + (i + 1);
        trname[i].id = (i + 1);
        waypointicon[i].innerHTML = i + 1;
        txtname[i].id = "Waypoint" + (i + 1);
        txtname[i].placeholder = "Waypoint" + (i + 1);


    }
    objifxStpMap.backupAnnotationsinRoute();
    deleteWayPoint($(li).attr("id") - 1, getCurrentActiveRoutePathIndex());
    removeDragroutemarkers();
    $('.swapicon').last().hide();

    if ($("#hIs_NEN").val() == "true" && VObj_err != null) {
     
        var Index = li[0].id, errorCount = 0;
        errorCount = VObj_err.length;
        for (var i = 0 ; i < errorCount; i++) {
            if (VObj_err[i].add_index == Index)
                VObj_err[i].isSetCorrect = 1;
            else if (VObj_err[i].add_index > Index) 
                VObj_err[i].add_index = (VObj_err[i].add_index - 1);

            }
        }
    
}

//event occuring after dragg and drop
function WaypointReady() {

    var first_position = 0;
    var current_position = 0;

    $("#sortable").sortable({
        start: function (event, ui) {
            ui.item.css('opacity', 0.50);
            first_position = ui.item.attr('id');
        },
        stop: function (event, ui) {
            ui.item.css('opacity', 1);
            waypoint_ordering();
            current_position = ui.item.attr('id');
            moveWayPointPosition(first_position, current_position);
        }
    });
    //$("#sortable").disableSelection();
    $('li > input').disableSelection();
}

//function for ordering the waypoints
function waypoint_ordering() {
    var pointNo = 2;
    var rowid = 1;
    $('.ui-state-default').each(function (index) {
        $(this).find('.waypointclass').text(rowid);
        $(this).find('input').attr({ "id": 'Waypoint' + rowid, "placeholder": 'Waypoint' + rowid, "pointNo": pointNo });
        $(this).attr({ "id": rowid })
        rowid = rowid + 1;
        pointNo = pointNo + 1;
    });
}

//function for removing waypoint from map
function removewaypoint_map(x) {
    $('#sortable').find('li').eq(x).remove();
    waypoint_ordering();
}
