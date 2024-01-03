///// global variables used to show warning on deleting route
var G_Route_id = 0;
var G_Route_type = "";
var G_Route_flag = "";
var G_delrouteName = "";
////
var plannedRouteId = 0;
var Dele_RouteID = 0;
var CountVal = $("#hf_Count").val();
var NENRouteStatusVal = $("#hf_NENRouteStatus").val();
var NENRouteStatusVBVal = $("#hf_NENRouteStatusVB").val();
var hNENRoute_IdVal = $("#hf_hNENRoute_Id").val();
var pageflagVal = $("#hf_pageflag").val();
var RouteFlagVal = $("#hf_RouteFlag").val();
function ListImportedRouteFromLibraryInit() {
    CountVal = $("#hf_Count").val();
    NENRouteStatusVal = $("#hf_NENRouteStatus").val();
    NENRouteStatusVBVal = $("#hf_NENRouteStatusVB").val();
    hNENRoute_IdVal = $("#hf_hNENRoute_Id").val();
    pageflagVal = $("#hf_pageflag").val();
    RouteFlagVal = $("#hf_RouteFlag").val();

    if (CountVal <= 1) {
        $("#sort").hide();
    }
    var status = NENRouteStatusVal;
    if (CountVal == 1) {
        if (status == "Unplanned" || status == "Planning error") {
            $("#RtStatus").html('Route is in ' + status + ' status, So no visual representation of route available.');
        }
    }
    $('#sort11').hide();

    $('#sort11').hide();
    $("#information").hide();
    var ValHFRouteFlag = $('#HFRouteFlag').val();

    $('#btn_cancel').hide();
    if ($("#hIs_NEN").val() == "true") {
        $("#hNEN_RouteStatus").val(NENRouteStatusVBVal);
        $('#sort').hide();
        $('#hNENRoute_Id').val(hNENRoute_IdVal);

    }

    var NEN_route
    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    var proj_status = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;
    var sort_status = $('#SortStatus').val() ? $('#SortStatus').val() : 0;
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
        $('#btn_edit_route').hide();
        $('#btn_del_route').hide();
    }
    if (chk_status == 301006) {
        if (sort_status == "CreateSO") {
            $('#btn_edit_route').show();
            $('#btn_del_route').show();
        }
        else {
            $('#btn_edit_route').hide();
            $('#btn_del_route').hide();
        }
    }
}
$(document).ready(function () {
    $('body').on('click', '#btn_cancel', function (e) {
        e.preventDefault();
        BacktorouteList(this);
    });
    $('body').on('click', '#btn_hualierdetails', function (e) {
        e.preventDefault();
        view_hualierdetails(this);
    });
    $('#sort').bind('click', sort);
    $('#sort11').click(function () {
        reset();
    });

    $('body').on('click', '#IDEnLargeMapNEN', function (e) {
        e.preventDefault();
        EnLargeMapNEN(this);
    });
    $('body').on('click', '#IDopenRouteFiltersNEN', function (e) {
        e.preventDefault();
        openRouteFilters(this);
    });
});
function sort() {
    $("#sortable").sortable();
    $("#sortable").sortable('enable');
    $('#sort11').show();
    $('#sort').hide();
    $('#btn_cancel').show();
    $("#information").show();
}

$('body').on('click', '#addtolib', function (e) {
    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var RouteName = $(this).data('namer');
    var RouteType = $(this).data('routetype');
    AddToLibrary(RouteID, RouteName, RouteType);
});
function displayList() {

    $(".slidingpanelstructures").removeClass("show").addClass("hide");
    if ($("#hIs_NEN").val() != "true") {                     // back button working flow retained for esdal
        var iscandlastversion = $('#IsCandVersion').val();
        var plannruserid = $('#PlannrUserId').val();
        var appstatuscode = $('#AppStatusCode').val();

        var status = $('#SortStatus').val();
        $('#ShowDetail').hide();
        $('#ShowList').show();
        $("#hdnVRRouteTab").val("True");

        $('#mapTitle').html('');
        var flag = pageflagVal;
        var link = '../Application/SoRoute?pageflag="_pageflag"';
        if (status == "CandidateRT") {
            link = '../SORTApplication/SortRoutes?CheckerId=' + _checkerid + '&CheckerStatus=' + _checkerstatus + '&IsCandLastVersion=' + iscandlastversion + '&planneruserId=' + plannruserid + '&appStatusCode=' + appstatuscode;
        }
        if (status == "ViewProj" || status == "MoveVer") {
            $("#leftpanel").html('');
        } else {
            $("#leftpanel").html('');
            $("#leftpanel").load(link.replace("_pageflag", flag), function () {
                $('#leftpanel_quickmenu').html('');
                $("#leftpanel").show();
                CheckSessionTimeOut();
            });

        }
    }
    else {

        removescroll();
        $('#mapTitle').html('');
        $("map").hide();
        $('#RouteMap').html('');
        PlannNENotification(0);
        var VNENId = $("#hNEN_Id").val(), VInboxID = $("#hinboxId").val();
        //$('#leftpanel').html('');
        //$("#leftpanel").load('../NENNotification/NENotif_leftPanel?inboxId=' + VInboxID + '&NenID=' + VNENId, function () {
        //    $('.loading').hide();
        //    $("#overlay").hide();
        //    addscroll();
        //    CheckSessionTimeOut();
        //});
        addscroll();
    }
}

function Updatenewroute(RouteId, RouteName) {
    //function for resize map
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

    var page = pageflagVal;

    $('#leftpanel').html('');
    $("#mapTitle").show();
    $('#RoutePart').show();

    $.ajax({
        url: '../Routes/LibraryRoutePartDetails',
        data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, plannedRouteName: RouteName, plannedRouteId: RouteId, PageFlag: "U", routeType: "outline" },
        type: 'GET',
        success: function (page) {
            //$('#tab_3').show();
            $('#RoutePart').html('');

            $('#RoutePart').append($(page).find('#CreateRoute').html());
            LibraryRoutePartDetailsInit();
            CheckSessionTimeOut();
            Map_size_fit();
            $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btn1-BackToImportRouteList"  type="button" data-icon="" aria-hidden="true">Back</button>');
            $("#mapTitle").html('');
            addscroll();
            // $('#tab_4').html('');
        }
    });
}
function UpdateOutlineDetails(RouteId) {
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + RouteId + '&routeType=' + 'outline&ApplicationRevId=' + ApplicationRevId + '&PageFlag=' + 'updateOutline', function () {
        //    removescroll();
        LibraryRoutePartDetailsInit();
        $("#dialogue").show();
        $("#overlay").show();
        //    CheckSessionTimeOut();
        //    return;
    });

    //  stopAnimation();
}
$('body').on('click', '#edit-route1', function (e) {
    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var rtname1 = $(this).data('rtname1');
    var planned = $(this).data('planned');
    EditRouteApplication(RouteID, rtname1, planned);
});
$('body').on('click', '#btn_edit_route', function (e) {

    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var rtname1 = $(this).data('rtname1');
    var RouteType = $(this).data('routetype');
    EditRouteApplication(RouteID, rtname1, RouteType);
});
function EditRouteApplication(RouteId, RouteName, RouteType) {
    var IsReturnLeg = false;  // added a check for indicating the route opened is return/main
    if ($("#hIs_NEN").val()) {
        var routeEdit = true;
        view_hualierdetails(routeEdit);  // automatically calls the haulier description when a nen route is edited
    }
    $('#RouteIDApp').val(RouteId);
    $("#HfRouteType").val(RouteType);
    startAnimation();
    if (RouteFlagVal == 4) {
        editCandidatRoute(RouteId, RouteType, RouteName);
    }
    else {
        if (RouteType == "planned" && $("#hIs_NEN").val() == "true") {      // handling conditions for nen
            $(".generalOptions").hide();
            $(".generalOptions").removeClass("d-flex");
            EditPlannedRoute(RouteId, RouteName);//function to edit the route with planned route type status
            //editeRoute(RouteId, RouteType);
        }
        else if (RouteType == "planned") {    // normal esdal sort route flow
            newroute1(RouteId, RouteName);
        }
        else {
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '../Routes/GetSoRouteDescMap',
                data: { plannedRouteId: RouteId, routeType: RouteType },
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    var flag = 0;
                    var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                    if (count == 0) { flag = 2; }
                    for (var i = 0; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointGeom != null || (result.result.routePathList[0].routePointList[i].linkId != null && result.result.routePathList[0].routePointList[i].linkId != 0))
                            flag = 1;
                        break;
                    }
                    if (result.result.routePathList[0].routeSegmentList == null || result.result.routePathList[0].routeSegmentList == "[]" || result.result.routePathList[0].routeSegmentList.count == 0)
                        flag = 0;

                    if (flag != 1) {
                        for (var i = 0; i < count; i++) {
                            if (result.result.routePathList[0].routePointList[i].pointType != 0 || result.result.routePathList[0].routePointList[i].pointType != 1) {
                                if (result.result.routePathList[0].routePointList[i].pointDescr == "")
                                    flag = 2;
                            }
                        }
                    }
                    if (flag == 0 && RouteFlagVal == 2) {
                        $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + RouteId + '&routeType=' + 'outline&ApplicationRevId=' + ApplicationRevId + '&IsStartAndEndPointOnly=true', function () {
                            removescroll();
                            $("#dialogue").show();
                            $("#overlay").show();
                            LibraryRoutePartDetailsInit();
                            CheckSessionTimeOut();
                        });
                        $("#dialogue").show();
                        $("#overlay").show();
                    }
                    else if (flag == 2 && RouteFlagVal == 2) {
                        $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + RouteId + '&routeType=' + 'outline&ApplicationRevId=' + ApplicationRevId + '&IsTextualRouteType=true', function () {
                            removescroll();
                            $("#dialogue").show();
                            $("#overlay").show();
                            LibraryRoutePartDetailsInit();
                            CheckSessionTimeOut();
                        });
                        $("#dialogue").show();
                        $("#overlay").show();
                    }
                    else if (flag == 1 && RouteFlagVal == 2) {
                        Updatenewroute(RouteId, RouteName);
                        // newroute1();

                    }
                },
                complete: function () {
                }
            });
        }
    }
    stopAnimation();
    addscroll();
    var url = geturl(location.href);

    $('#URLKEY').val("RoutesLibraryRoutePartDetails");
    if (CheckLoginUserCount())
        // help();
        if (RouteFlagVal == 2) { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for SO  - Route</h3>'); }
        else if (RouteFlagVal == 1 || RouteFlagVal == 3) { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for VR-1 - Route</h3>'); }
}


function editCandidatRoute(RouteId, RouteType, RouteName) {
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetCandidateRouteDescMap',
        data: { plannedRouteId: RouteId, routeType: RouteType },
        beforeSend: function (xhr) {
        },
        success: function (result) {
            if (result.result != null) {
                var flag = 0;
                var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                if (count == 0) { flag = 2; }
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointGeom != null || (result.result.routePathList[0].routePointList[i].linkId != null && result.result.routePathList[0].routePointList[i].linkId != 0))
                        flag = 1;
                    break;
                }
                if (result.result.routePathList[0].routeSegmentList == null || result.result.routePathList[0].routeSegmentList == "[]" || result.result.routePathList[0].routeSegmentList.count == 0)
                    flag = 0;

                if (flag != 1) {
                    for (var i = 0; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointType != 0 || result.result.routePathList[0].routePointList[i].pointType != 1) {
                            if (result.result.routePathList[0].routePointList[i].pointDescr == "")
                                flag = 2;
                        }
                    }
                }
                if (flag == 0 && RouteFlagVal == 4) {
                    $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + RouteId + '&routeType=' + 'outline&ApplicationRevId=' + ApplicationRevId + '&IsStartAndEndPointOnly=true', function () {
                        removescroll();
                        $("#dialogue").show();
                        $("#overlay").show();
                        LibraryRoutePartDetailsInit();
                        CheckSessionTimeOut();
                    });
                    $("#dialogue").show();
                    $("#overlay").show();
                }
                else if (flag == 2 && RouteFlagVal == 4) {
                    $('#hfCandidatRouteID').val(RouteId);
                    $('#hfCandidatDescription').val(result.result.routePartDetails.routeDescr);
                    $('#hfCandidatName').val(result.result.routePartDetails.routeName);

                    $('#leftpanel').html('');
                    $("#mapTitle").show();
                    $('#tab_4').show();
                    $('#RoutePart').show();
                    $('#RoutePart').html('');
                    var page = pageflagVal;
                    $.ajax({
                        url: '../Routes/LibraryRoutePartDetails',
                        data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, plannedRouteName: RouteName, plannedRouteId: RouteId, PageFlag: "U", routeType: RouteType },
                        type: 'GET',
                        success: function (page) {
                            $('#tab_4').show();
                            $('#RoutePart').html('');

                            $('#RoutePart').append($(page).find('#CreateRoute').html());
                            LibraryRoutePartDetailsInit();
                            CheckSessionTimeOut();
                            Map_size_fit();
                            $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btn2-BackToImportRouteList"  type="button" data-icon="" aria-hidden="true">Back</button>');
                            $("#mapTitle").html('');
                            addscroll();
                        }
                    });

                }
                else if (flag == 1 && RouteFlagVal == 4) {
                    // Updatenewroute(RouteId, RouteName);  /* function commented for avoiding duplicate call to load map */
                    // newroute1();
                    $('#leftpanel').html('');
                    $("#mapTitle").show();
                    $('#tab_4').show();
                    $('#RoutePart').show();
                    $('#RoutePart').html('');
                    var page = pageflagVal;
                    $.ajax({
                        url: '../Routes/LibraryRoutePartDetails',
                        data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, plannedRouteName: RouteName, plannedRouteId: RouteId, PageFlag: "U", routeType: RouteType },
                        type: 'GET',
                        success: function (page) {
                            $('#tab_4').show();
                            $('#RoutePart').html('');

                            $('#RoutePart').append($(page).find('#CreateRoute').html());
                            LibraryRoutePartDetailsInit();
                            CheckSessionTimeOut();
                            Map_size_fit();
                            $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btn3-BackToImportRouteList"  type="button" data-icon="" aria-hidden="true">Back</button>');
                            $("#mapTitle").html('');
                            addscroll();
                        }
                    });

                }
            }
        },
        complete: function () {
        }
    });

}


function editeRoute(RouteId, RouteType) {
    var g = RouteId;
    // displaySoRouteDescMap(RouteId, RouteType);
    $('#leftpanel').html('');
    var url = '../Routes/A2BLeftPanel?routeID = "_ID"';
    $.ajax({
        url: url.replace("_ID", RouteId),
        type: 'POST',
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html(page);
            $('#leftpanel').show();
            CheckSessionTimeOut();
        },
        complete: function () {
            stopAnimation();
        }
    });

}

function CheckLoginUserCount() {
    var res = false;
    $.ajax({
        url: '../Notification/CheckLoginUserCount',
        type: 'POST',
        async: false,
        success: function (result) {
            res = result.Status;
        }
    });
    return res;
}
$('body').on('click', '#displaySoRouteDescMap', function (e) {

    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var RouteType = $(this).data('routetype');
    displaySoRouteDescMap(RouteID, RouteType);
});
function displaySoRouteDescMap(RouteId, RouteType) {
    if ($("#hIs_NEN").val()) {
        //view_hualierdetails();  // automatically calls the haulier description when a nen route is added
        var rowID2 = "#status_" + $('#hNENRoute_Id').val(), rowID1 = "#status_" + $('#hreturnLeg_routeID').val();
        if (RouteId == $('#hreturnLeg_routeID').val()) {
            ActionForReturnRout = 1;
            if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error")     // check added for return route is not planned set IsReturnPlanned to 0
            { IsReturnPlanned = false; }
            else { IsReturnPlanned = true; }
        }
        else {
            ActionForReturnRout = 0;
            if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error") { IsReturnPlanned = false; }
            else { IsReturnPlanned = true; }
        }

    }
    setRouteID(RouteId);
    $('#leftpanel').html('');
    $('#leftpanel_quickmenu').html('');
    $("#RouteMap").html('');
    $("#map").html('');
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetSoRouteDescMap',
        data: { plannedRouteId: RouteId, routeType: RouteType },
        beforeSend: function (xhr) {
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {

            $('#ShowDetail').show();
            $(".slidingpanelstructures").removeClass("hide").addClass("show");
            var count = -1, strTr, flag = 0, Index = 0;
            if (result.result != null && result.result.routePathList.length > 0) {
                count = result.result.routePathList[0].routePointList.length;

                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointGeom != null || (result.result.routePathList[0].routePointList[i].linkId != null && result.result.routePathList[0].routePointList[i].linkId != 0)) { flag = 1; break; }
                    if (result.result.routePathList[0].routePointList[i].pointGeom == null) { flag = 0; break; }
                }
            }
            if (flag == 1 || RouteFlagVal == 1 || RouteFlagVal == 3) {

                $('#Tabvia').html('');
                $('#ShowList').hide();
                $("#RoutePartName").html(result.result.routePartDetails.routeName);
                if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "") {
                    var StrDes = result.result.routePartDetails.routeDescr;
                    if (StrDes.length > 149) {
                        $("#RouteDescr").html(StrDes.substring(0, 149) + "...");
                        document.getElementById('RouteDescr').title = StrDes;
                    }
                    else $("#RouteDescr").html(StrDes);
                }
                var LI = "";
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointType == 0)
                        $('#Starting').html(result.result.routePathList[0].routePointList[i].pointDescr);
                    else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                        $('#Ending').html(result.result.routePathList[0].routePointList[i].pointDescr);

                    else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        if ($("#hIs_NEN").val() == "true") {
                            if (ISCordinets(result.result.routePathList[0].routePointList[i].pointDescr) == true)
                                LI = " (Easting,Northing)";
                            else
                                LI = " (Address)";
                        }
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + LI + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }

                if (result.result.routePathList.length > 0) {
                    $("#RouteMap").load('../Routes/A2BPlanning?routeID = 0', function () {

                        if (RouteType == "outline") {
                            loadmap('DISPLAYONLY');
                            showSketchedRoute(result.result);
                        }
                        else {
                            $("#map").html('');
                            if (RouteFlagVal == 2)
                                loadmap('DISPLAYONLY', result.result, null, false);
                            else if (RouteFlagVal == 4)
                                loadmap('DISPLAYONLY', result.result, null, true);
                            else
                                loadmap('DISPLAYONLY', result.result);
                        }
                    });
                }
                else {
                    $("#RouteMap").load('../Routes/A2BPlanning?routeID=0', function () {

                        loadmap('DISPLAYONLY');
                        showSketchedRoute(result.result);
                    });
                }
            }
            else {
                $("#RouteMap").html('');
                $('#ShowDetail').show();
                $('#ShowList').hide();
                $("#RoutePartName").html(result.result.routePartDetails.routeName);
                $('#Tabvia').html('');
                if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "") {
                    var StrDes = result.result.routePartDetails.routeDescr;
                    if (StrDes.length > 149) {
                        $("#RouteDescr").html(StrDes.substring(0, 149) + "...");
                        document.getElementById('RouteDescr').title = StrDes;
                    }
                    else $("#RouteDescr").html(StrDes);
                }
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointType == 0)
                        $('#Starting').html(result.result.routePathList[0].routePointList[0].pointDescr);
                    else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                        $('#Ending').html(result.result.routePathList[0].routePointList[1].pointDescr);

                    else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }
                $("#RouteMap").html('No visual representation of route available.');
            }
            $("#RouteName1").html('Route not available.');

        },
        complete: function () {
            if ($('#Starting').html() == '') {
                $('#trRoute').hide();
                $('#trStarting').hide();
                $('#trVia').hide();
                $('#trEnding').hide();
            }
            else {
                $('#trRoute').show();
                $('#trStarting').show();
                $('#trVia').show();
                $('#trEnding').show();
            }
            if ($("#RouteDescr").html() != "") {
                $('#trHeaderDescription').show();
                $('#trdesc').show();
            }
            else {
                $('#trHeaderDescription').hide();
                $('#trdesc').hide();
            }

            $('#URLKEY').val("RoutesLibraryRoutePartDetails");
            if (CheckLoginUserCount()) {
                $('#scroll-btns').show();

                var url = geturl(location.href);
                var urlkey = $("#URLKEY").val();
                if (urlkey == "RoutesLibraryRoutePartDetails") {
                    url = urlkey;
                }
                else {
                    if (urlkey != "") {
                        url = url + urlkey;
                    }
                }
                $('#url_html_page').val(url);
                $('#fflag').val(0);
                $.ajax({
                    url: '/Generic/GetHtmlPage',
                    type: 'POST',
                    data: { fileName: url, flag: 0 },
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (page) {
                        $("#exampleModalCenter2").html(page);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        GetPageException(errorThrown);
                    },
                    complete: function () {
                        stopAnimation();
                    }
                });
            }
            stopAnimation();
        }
    });
    addscroll();
    if ($("#hIs_NEN").val() == "true") {
        $('#leftpanel').append("<button id='btn_hualierdetails' style='float: left;margin-left: 45px !important;margin-top: 20px' class='create btngrad btnrds btnbdr pointerCursor' aria-hidden='true'  >Haulier route description</button>");
        if (RouteId == $('#hreturnLeg_routeID').val())
            ActionForReturnRout = 1;
        else
            ActionForReturnRout = 0;
    }
}
//View candidate route
$('body').on('click', '#displayCandidateRouteDescMap', function (e) {

    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var RouteType = $(this).data('routetype');
    displayCandidateRouteDescMap(RouteID, RouteType);
});
function displayCandidateRouteDescMap(RouteId, RouteType) {
    $("#RouteMap").html('');
    $("#map").html('');
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetCandidateRouteDescMap',
        data: { plannedRouteId: RouteId, routeType: RouteType },
        beforeSend: function (xhr) {
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#ShowDetail').show();
            var count = -1, strTr, flag = 0, Index = 0;
            if (result.result != null) {
                count = result.result.routePathList[0].routePointList.length;

                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointGeom != null || (result.result.routePathList[0].routePointList[i].linkId != null && result.result.routePathList[0].routePointList[i].linkId != 0)) { flag = 1; break; }
                    if (result.result.routePathList[0].routePointList[i].pointGeom == null) { flag = 0; break; }
                }
                $('#Tabvia').html('');

                $('#ShowList').hide();
                $("#RoutePartName").html(result.result.routePartDetails.routeName);
                if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                    $("#RouteDescr").html(result.result.routePartDetails.routeDescr);
                for (var i = 0; i < count; i++) {
                    if (result.result.routePathList[0].routePointList[i].pointType == 0)
                        $('#Starting').html(result.result.routePathList[0].routePointList[i].pointDescr);
                    else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                        $('#Ending').html(result.result.routePathList[0].routePointList[i].pointDescr);

                    else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }
                if (typeof result.result.routePathList[0].routeSegmentList != "undefined" && String(typeof result.result.routePathList[0].routeSegmentList) != "[]") {
                    $("#RouteMap").load('../Routes/A2BPlanning?routeID = 0', function () {

                        if (RouteType == "outline" || RouteType == "Outline") {
                            loadmap('DISPLAYONLY');
                            showSketchedRoute(result.result);
                            // loadmap(10, result.result);
                        }
                        else {
                            $("#map").html('');
                            //  loadmap(7, result.result);
                            //loadmap('DISPLAYONLY', result.result);
                            loadmap('DISPLAYONLY', result.result, null, true);
                            //Changed by sali.
                            /*loadmap(10);
                            showSketchedRoute(result.result);*/
                        }
                    });
                }
                else {
                    $("#RouteMap").load('/Routes/A2BPlanning?routeID=0', function () {

                        loadmap('DISPLAYONLY');
                        showSketchedRoute(result.result);
                    });
                }
                $("#RouteName1").html('Route not available.');
            }
            else if (RouteType == 'planned') {
                displayCandidateRouteDescMap(RouteId, 'outline');
            }

        },
        complete: function () {
            if ($('#Starting').html() == '') {
                $('#trRoute').hide();
                $('#trStarting').hide();
                $('#trVia').hide();
                $('#trEnding').hide();
            }
            else {
                $('#trRoute').show();
                $('#trStarting').show();
                $('#trVia').show();
                $('#trEnding').show();
            }
            if ($("#RouteDescr").html() != "") {
                $('#trHeaderDescription').show();
                $('#trdesc').show();
            }
            else {
                $('#trHeaderDescription').hide();
                $('#trdesc').hide();
            }
            stopAnimation();
        }
    });
    addscroll();
}
//function to delete route
$('body').on('click', '#btn_del_route', function (e) {

    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var RouteType = $(this).data('routetype');
    DeleteSelectedRoute(this, RouteID, RouteType);
});
$('body').on('click', '#btn-DeleteSelectedRoute', function (e) {

    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var RouteType = $(this).data('routetype');
    DeleteSelectedRoute(this, RouteID, RouteType);
});
function DeleteSelectedRoute(_this, routeId, RouteType) {
    Dele_RouteID = routeId;
    V_RouteType = RouteType;
    delrouteName = $(_this).attr('name');
    G_delrouteName = $(_this).attr('name');
    RouteID = routeId;
    var warning = 0;
    ////varialbles used to show warning on deleting route
    G_Route_id = routeId;
    G_Route_type = RouteType;
    G_Route_flag = RouteFlagVal;
    ////
    var Msg = "Do you want to delete '" + "" + "'" + delrouteName + "'" + "" + "' ?";
    showWarningPopDialog(Msg, 'No', 'Yes', 'WarningCancelBtnDelete', 'OnDeleteShow_Warning', 1, 'warning');
}
function OnDeleteShow_Warning() {
    routeId = G_Route_id;
    RouteType = G_Route_type;

    $.ajax({
        url: '../Routes/OnDeleteRouteShowWarning',
        type: 'POST',
        cache: false,
        async: false,
        data: { RouteID: routeId, RouteType: RouteType, RouteFlag: RouteFlagVal },
        beforeSend: function () {
        },
        success: function (data) {

            if (data.success == 1) {
                showWarningPopDialog('"' + G_delrouteName + '" route is attached to a vehicle. Please delete the vehicle in the vehicle tab or reassign it to available routes (if not automatically assigned by the system).', 'ok', '', 'Delete_Route_After_Warning', '', 1, 'warning');
            }
            else if (data.success == 2) {
                showWarningPopDialog('As one route remains in the route tab, all vehicles will be assigned to the available route. Please delete the vehicle if not needed.', 'ok', '', 'Delete_Route_After_Warning', '', 1, 'warning');
            }
            else {
                Delete_Route_After_Warning();
            }

        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {

        }
    });
}

function Delete_Route_After_Warning() {
    if (RouteFlagVal == 3)//notification
        Delete_Noti_Route();
    else if (RouteFlagVal == "4")
        DeleteCandidateRoute();
    else //SO / VR1
        DeleteRoute();
    cntTr = $(_this).closest("tr");
}




function Delete_Noti_Route() {
    var notifid = $('#NotificatinId').val();
    var VConten_Ref_ID = $('#CRNo').val() ? $('#CRNo').val() : 0;
    if (VConten_Ref_ID == 0)
        VConten_Ref_ID = $('#ContentRefNo').val() ? $('#ContentRefNo').val() : 0;
    //added by poonam (18.8.14)
    var vr1contrefno = $('#VR1ContentRefNo').val();
    var vr1versionid = $('#VersionId').val();
    //-----------
    if (VConten_Ref_ID == 0) { VConten_Ref_ID = vr1contrefno; }
    $.ajax({
        url: "../Application/DeleteSelectedSOAppRoute",
        type: 'POST',
        async: false,
        data: { RouteID: Dele_RouteID, apprevisionId: 0, routeType: V_RouteType, Conten_Ref_ID: VConten_Ref_ID, versionId: vr1versionid, NotificationId: notifid },
        beforeSend: function () {
            startAnimation();
            WarningCancelBtn();
        },
        success: function (result) {
            if (result.Success) {
                //method
                var msg = 'Route  "' + delrouteName + '"  deleted successfully';
                showWarningPopDialog(msg, 'Ok', '', 'ShowUpdatedRouteList', '', 1, 'info');
                NotifValidation();
            }
            else {
                alert("Error");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            // location.reload();
        },
        complete: function () {
            stopAnimation();
            //   cntTr.remove();
        }

    });
}

function DeleteRoute() {
    var PageFlag = $('#Pageflag').val();
    //added by poonam (18.8.14)
    var vr1contrefno = $('#VR1ContentRefNo').val();
    var vr1versionid = $('#VersionId').val();
    //-----------
    if (PageFlag == 3) { Delete_Noti_Route(); }
    else {
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        //  $('#pop-warning').hide();
        $.ajax({
            url: "../Application/DeleteSelectedSOAppRoute",
            type: 'POST',
            cache: false,
            async: false,
            data: { RouteID: Dele_RouteID, apprevisionId: ApplicationRevId, routeType: V_RouteType, Conten_Ref_ID: vr1contrefno, versionId: vr1versionid },
            beforeSend: function () {
                startAnimation();
                WarningCancelBtn();
            },
            success: function (result) {
                if (result.Success) {

                    var msg = 'Route  "' + delrouteName + '"  deleted successfully';
                    showWarningPopDialog(msg, 'Ok', '', 'ShowUpdatedRouteList', '', 1, 'info');

                }
                else {
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
                // cntTr.remove();
            }

        });
    }
}

function DeleteCandidateRoute() {
    var revisionid = $('#revisionId').val();
    //  $('#pop-warning').hide();
    $.ajax({
        url: "../Application/DeleteSelectedSOAppRoute",
        type: 'POST',
        cache: false,
        async: false,
        data: { RouteID: Dele_RouteID, routeType: V_RouteType, RouteRevisionId: revisionid },
        beforeSend: function () {
            startAnimation();
            WarningCancelBtn();
        },
        success: function (result) {
            if (result.Success) {
                var msg = 'Route  "' + delrouteName + '"  deleted successfully';
                showWarningPopDialog(msg, 'Ok', '', 'BindRouteParts', '', 1, 'info');

            }
            else {
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
            // cntTr.remove();
        }

    });
}

function ISCordinets(Key) {
    var is0Numeric = true, is1Numeric = true;
    var AddresArr = Key.split(',');;
    if (AddresArr.length == 2) {
        if (isNaN(parseInt(AddresArr[0], 10)))
            is0Numeric = false;
        if (isNaN(parseInt(AddresArr[1], 10)))
            is1Numeric = false;

        if (is0Numeric == true && is1Numeric == true)
            return true;
        else return false;
    }
    return false;

}
function view_hualierdetails(routeEdit) {
    var VNEN_id = $("#hNEN_Id").val(), VIsReturnleg = false;
    if (($('#hIsReturnLeg').val() == 'true' || $('#hIsReturnLeg').val() == 'True')) { VIsReturnleg = true }
    var VInboxID = $("#hinboxId").val();
    var link = '../NENNotification/hualierRouteInfo?InboxitemId=' + VInboxID + '&NEN_ID=' + VNEN_id + '&IsReturleg=' + VIsReturnleg;
    var myWindow = window.open(link, 'myWindow', "width=500,height=300,scrollbars=yes,resizable =yes");
    stopAnimation();
    if (routeEdit == undefined) {
         ShowGeneralDetails();
    }
   
}
$('body').on('click', '#btn1-viewroute', function (e) {
    e.preventDefault();
    var RouteID = $(this).data('routeid');
    var planned = $(this).data('planned');
    var NENRouteStatus = $(this).data('nenroutestatus');
    ViewRouteOnMapNEN(RouteID, planned, NENRouteStatus);
});
function ViewRouteOnMapNEN(RouteId, RouteType, rtStatus, hideRoute = false) {
    //if (rtStatus == "Unplanned" || rtStatus == "Planning error") {
    //    $("#RtStatus").html('Route is in ' + rtStatus + ' status, So no visual representation of route available.');
    //}
    //else {
        $(".generalOptions").hide();
        $(".generalOptions").removeClass("d-flex");
        if ($("#hIs_NEN").val()) {
            var rowID2 = "#status_" + $('#hNENRoute_Id').val(), rowID1 = "#status_" + $('#hreturnLeg_routeID').val();
            if (RouteId == $('#hreturnLeg_routeID').val()) {
                ActionForReturnRout = 1;
                if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error")     // check added for return route is not planned set IsReturnPlanned to 0
                { IsReturnPlanned = false; }
                else { IsReturnPlanned = true; }
            }
            else {
                ActionForReturnRout = 0;
                if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error") { IsReturnPlanned = false; }
                else { IsReturnPlanned = true; }
            }
        }
       
        setRouteID(RouteId);
        $.ajax({
            url: '../Routes/GetRouteVehicleDetails',
            type: 'post',
            data: { routePartId: RouteId, routeType: RouteType },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                //extra code
                if ($('#hf_sessionRouteFlag').val() == 3) {
                    $('#RoutePart').append('<div class="row mt-4" style="float: right; ">' + '<div class="mt-2 pl-0 ml-0 DivCSSLisTImported">' + '<button title="Go back to the route list" class="btn outline- btn-primary SOAButtonHelper ml2 mb-2 btn-ne-display-list" aria-hidden="true" type="button">BACK</button>' + '</div >' + '</div >');
                    //$('#belowMap').append('<div class="row mt-4" style="float: right; ">' + '<div class= ' + "mt-2 pl-0 ml-0 DivCSSLisTImported" + '>' + '<button title="Go back to the route list" class="btn outline- btn-primary SOAButtonHelper ml2 mb-2 btn-ne-display-list" aria-hidden="true" type="button">BACK</button>' + '</div >' + '</div >');
                
                }
                else {
                    $('#RoutePart').append('<button id="back_btn" class="btn_reg_back next btngrad btnrds btnbdr btn-ne-bind-route-parts" type="button" data-icon="" aria-hidden="true">Back</button>');
                }

                var LI = "";
                var rtDetails = result.routeDetails.routePartDetails;
                var count = -1, strTr, flag = 0, Index = 0;
                if (rtDetails != null && result.routeDetails.routePathList.length > 0) {
                    count = result.routeDetails.routePathList[0].routePointList.length;

                    for (var i = 0; i < count; i++) {
                        if (result.routeDetails.routePathList[0].routePointList[i].pointGeom != null || (result.routeDetails.routePathList[0].routePointList[i].linkId != null && result.routeDetails.routePathList[0].routePointList[i].linkId != 0)) { flag = 1; break; }
                        if (result.routeDetails.routePathList[0].routePointList[i].pointGeom == null) { flag = 0; break; }
                    }
                }

                $('#ShowDetail').show();
                $('#Tabvia').html('');
                $('#ShowList').hide();

                $("#RoutePartName").html(rtDetails.routeName);
                if (rtDetails.routeDescr != null && rtDetails.routeDescr != "") {
                    var StrDes = rtDetails.routeDescr;
                    if (StrDes.length > 149) {
                        $("#RouteDescr").html(StrDes.substring(0, 149) + "...");
                        document.getElementById('RouteDescr').title = StrDes;
                    }
                    else $("#RouteDescr").html(StrDes);
                }

                for (var i = 0; i < count; i++) {
                    if (result.routeDetails.routePathList[0].routePointList[i].pointType == 0)
                        $('#Starting').html(result.routeDetails.routePathList[0].routePointList[i].pointDescr);
                    else if (result.routeDetails.routePathList[0].routePointList[i].pointType == 1)
                        $('#Ending').html(result.routeDetails.routePathList[0].routePointList[i].pointDescr);

                    else if (result.routeDetails.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        if ($("#hIs_NEN").val() == "true") {
                            if (ISCordinets(result.routeDetails.routePathList[0].routePointList[i].pointDescr) == true)
                                LI = " (Easting,Northing)";
                            else
                                LI = " (Address)";
                        }
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.routeDetails.routePathList[0].routePointList[i].pointDescr + LI + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }

                if (rtStatus != "Unplanned" && rtStatus != "Planning error") {
                    var routeDetails = result.routeDetails;
                    $("#appRouteMap").show();
                    $("#appRouteMap").html('');
                    $("#appRouteMap").load('../Routes/A2BPlanning?routeID=0', function () {
                        $("#map").addClass("context-wrap olMap mapHeight");
                        $("#IDEnLargeMapNEN").show();
                        $("#IDopenRouteFiltersNEN").show();
                        createContextMenu();
                        var count = -1, strTr, Index = 0;
                        if (routeDetails != null)
                            count = routeDetails.routePathList[0].routePointList.length;
                        $('#StartingAdd').html('');
                        $('#EndingAdd').html('');
                        $('#TabviaAdd').html('');
                        for (var i = 0; i < count; i++) {
                            if (routeDetails.routePathList[0].routePointList[i].pointType == 0)
                                $('#StartingAdd').html(routeDetails.routePathList[0].routePointList[i].pointDescr);
                            else if (routeDetails.routePathList[0].routePointList[i].pointType == 1)
                                $('#EndingAdd').html(routeDetails.routePathList[0].routePointList[i].pointDescr);
                            else if (routeDetails.routePathList[0].routePointList[i].pointType >= 2) {
                                Index = Index + 1;
                                strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + routeDetails.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                $('#TabviaAdd').append(strTr);
                            }
                        }
                        if (hideRoute == true) {
                            document.getElementById('viewroutes').style.display = "block"
                        }
                        if (RouteType == 'outline') {
                            loadmap('DISPLAYONLY');
                            showSketchedRoute(routeDetails);
                        }
                        else {
                            loadmap('DISPLAYONLY', routeDetails);
                        }
                        if (hideRoute == true) {
                            document.getElementById('viewroutes').style.display = "none"
                        }
                    });
                }
                else {
                    $("#RouteMap").html('<span class="text-normal">Route is in ' + rtStatus + ' status, So no visual representation of route available.</span>');
                    $(".DivCSSLisTImported").attr('style', 'margin-top: 0px !important');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            complete: function () {
                stopAnimation();
            }
        });
    //}


}


function EnLargeMapNEN() {
    if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
        $("#navbar").hide();
        $("#Minimzeicon").hide();
        $("#MaxmizeIocn").show();
        $("#banner-container").removeClass("container-fluid");
        $("#banner").removeClass("vehicle-setting");
        $("#vehicles").removeClass("flowComponent");
        $("#vehicles").removeClass("vehicles");
        document.getElementById('vehicles').id = 'veh';
        document.getElementById('banner-container').id = 'div';
        $("#footerdiv").hide();
        $(".DivCSSLisTImported").hide();
        $("#progressBar").hide();
        $("#RoteHeading").hide();
        $("#sort-menu-list").hide();
        $("#NEHeading").hide();
        $("#ShowDetail").hide();     
        $("#main-sort-content").removeClass("main-sort-content");
        $("#main-sort-content").addClass("nenfulls");
        $('body').css('overflow-y', 'hidden');
        
        $('div.mapHeight').css('height', '1250px');
        $(".tableHead").hide()
        scroll();


    } else { //mode changes to minimze
        //closeFullscreen();
        $("#navbar").show();
        $("#Minimzeicon").show();
        $("#MaxmizeIocn").hide();
        $("#banner-container").addClass("container-fluid");
        $("#banner").addClass("vehicle-setting");
        $("#vehicles").addClass("flowComponent");
        $("#progressBar").show();
        $("#footerdiv").show();
        $(".DivCSSLisTImported").show();
        $("#RoteHeading").show();
        $("#sort-menu-list").show();
        $("#NEHeading").show();
        document.getElementById('veh').id = 'vehicles';
        document.getElementById('div').id = 'banner-container';
        $('body').css('overflow-y', 'scroll');
        $('div.mapHeight').css('height', '650px');
      
        $("#main-sort-content").addClass("main-sort-content");
        $("#main-sort-content").removeClass("nenfulls");
        $("#ShowDetail").show();
        $(".tableHead").show()

    }

}






