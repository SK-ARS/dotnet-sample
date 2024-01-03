var newMap = "";
var mapLoadContainer = "";
var routeDiv = "";
$(document).ready(function () {
    $("#map").html('');
    $("#map").load('../Routes/A2BPlanning',{routeID : 0}, function () {
        loadmap('DISPLAYONLY');
        RouteMapDetail();

        //******Check Broken Route
        var inputDataObj = {};
        var revisionId = $('#REVISION_ID').val();
        var versionId = $('#hfVERSION_ID').val();
        var contentRefNum = $('#hfContentRefNum').val();
        var HFNEN_ID = $('#HFNEN_ID').val();
        var isNen = 0;
        if (HFNEN_ID > 0) {//PDF-1
            isNen = 1;
        }
        else if ($("#RouteName").val().toLowerCase() == 'ne notification api' ||
            $("#RouteName").val().toLowerCase() == 'ne renotification api') {//API-2
            isNen = 2;
        }
        
        inputDataObj = (revisionId != '0') ? { VersionId: versionId, IsReplanRequired: false, IsSort: true, IsNen: isNen } : { ConteRefNo: contentRefNum, IsReplanRequired: false, IsNen: isNen };
        CheckIsBroken(inputDataObj, function (response) {
            if (response && response.Result && response.Result.length > 0) {//isBroken[0].isBroken > 0  //check in the existing route is broken   Extra condition added for handling to ESDAL4 once the Mapupgarde service activated then the condition can be moved
                if (response.brokenRouteCount != 0) {
                    var routeNameHtml = "";
                    $.each(response.Result, function (key, valueObj) {
                        if (valueObj.IsBroken > 0) {
                            var routePartName = $('.map-name-' + valueObj.PlannedRouteId).text();
                            routeNameHtml += "<li>" + routePartName + "</li>";
                        }
                    });
                    routeNameHtml = "<ul style='text-align: left;list-style: auto;padding-top: 10px;'>" + routeNameHtml + "</ul>"
                    var msg = revisionId != '0' ? BROKEN_ROUTE_MESSAGES.SOA_POLICE_SO_APPLICATIONS + routeNameHtml : BROKEN_ROUTE_MESSAGES.SOA_POLICE_NOTIFICATIONS + routeNameHtml;
                    ShowWarningPopupMapupgarde(msg, function () {
                        $('#WarningPopup').modal('hide');
                    });
                }
            }
        });
        //******
    });
    $('body').on('click', '#btnenlarge', EnLargeMap);
    $('body').on('change', '#gpxRoute', ViewGPXRoute);
    $('body').on('click', '#movementMapFilterIcon', openAuthorizeMovFilters);
    $('body').on('click', '#generalmap #flexRadioDefault11', function (e) {
        var RouteId = $(this).data("routeid");
        var RouteType = $(this).data("routetype");
        var VIsSortPortal = $(this).data("vissortportal");
        $('#divViaPoint').css("display", "none");
        $('#generalmap').find("#up-chevlon").css("display", "none");
        $('#generalmap').find("#down-chevlon").css("display", "block");
        RouteView(RouteId, RouteType, VIsSortPortal);
        if ($("#RouteName").val().toLowerCase() == 'ne notification api' ||
            $("#RouteName").val().toLowerCase() == 'ne renotification api') {
            $(".editroutebtncls").hide();
            $("#editnenroute-" + RouteId).show();
        }
    });
    $('body').on('click', '.editroutebtncls', EditNENRoute);
    $('body').on('click', '#card-swipe1', function (e) {
        e.preventDefault();
        openRoutenav(this);
    });
    $('body').on('click', '#card-swipe2', function (e) {
        e.preventDefault();
        closeRoutenav(this);
    });
    $('body').on('click', '#editnenrouteback', EditRouteBackAction);
});
function EnLargeMap() {
    if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
        $('#movementMapFilterIcon').show();
        $("#navbar").hide();
        $("#generalmap").hide();
        $("#listitem").hide();
        $("#printbutton").hide();
        $("#movement-details").hide();
        $("#footerdiv").hide();
        $("#Minimzeicon").hide();
        $("#MaxmizeIocn").show();
        $("#banner-container").addClass("bannercfulls");
        $("#container-sub").addClass("containerfulls");
        $("#helpdeskDelegation").addClass("helpdeskfulls");
        $("#movement-map").addClass("movementfulls");
        $("#map-load-container").removeClass("p-3");
        $("#map").removeClass("olMap");
        $("#map").addClass("mapfulls");
        $("#map").removeClass("maponload");
        $("#btnenlarge").addClass("expand");
        $("#OpenLayers_Control_Panel_200").addClass("horizontalMapRoad");
        $("#btnmap").addClass("iconfulls");
        $("#helpdeskDelegation").removeClass("row");
        $("#banner-container").removeClass("container-fluid");
        $("#movement-map").removeClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
        mapResize();
    }
    else { //mode changes to minimze
        $('#movementMapFilterIcon').hide();
        $("#navbar").show();
        $("#Minimzeicon").show();
        $("#MaxmizeIocn").hide();
        $("#printbutton").show();
        $("#listitem").show();
        $("#movement-details").show();
        $("#map").addClass("maponload");
        $("#map-load-container").addClass("p-3");
        $("#map").removeClass("mapfulls");
        $("#map").removeClass("olMap");
        $("#btnenlarge").removeClass("expand");
        $("#container-sub").removeClass("containerfulls");
        $("#helpdeskDelegation").removeClass("helpdeskfulls");
        $("#movement-map").removeClass("movementfulls");
        $("#mvpmap").removeClass("mvpmapfulls");
        $("#OpenLayers_Control_Panel_200").removeClass("horizontalMapRoad");
        $("#banner-container").removeClass("bannercfulls");
        $("#helpdeskDelegation").addClass("row");
        $("#banner-container").addClass("container-fluid");
        $("#movement-map").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
        $("#overlay_load").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
        $("#btnmap").removeClass("iconfulls");
        $("#generalmap").show();
        $("#footerdiv").show();
        functionMapRefresh();  
    }
}
function functionMapRefresh()
{
    var contRefNum = $('#hf_Content_ref_no').val();
    var versionId = $('#hfVERSION_ID').val();
    var nenId = $('#HFNEN_ID').val();
    var isNen = $('#hf_IsNen').val();
    var isNenViaApi = Boolean($('#hf_IsNenViaApi').val().toLowerCase() == "true");
   

    $.ajax({
        url: '../Movements/MovementMap',
        data: { contRefNum: contRefNum, versionId: versionId, nenId: nenId, isNen: isNen, isNenApi: isNenViaApi },
        async: false,
        success: function (data) {
            $('#MapData').html('')
            $('#MapData').html(data);
            $("#flexRadioDefault11").trigger("click");
            
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        }
    });
}
function openAuthorizeMovFilters() {
    $('#authorizeMovFilterDiv #filters').css("width", "450px");//$('#filters').css("width", "450px");
    $('#authorizeMovFilterDiv #filters').css("margin", "0 0 0 0");//$("#filters").css('margin-right', "0");
    $('body').prepend('<div class="bs-canvas-overlay bg-dark position-fixed w-100 h-100"></div>');
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeAuthorizeMovFilters() {
    $('.bs-canvas-overlay').remove();
    $('#authorizeMovFilterDiv #filters').css("margin", "0 -450px 0 0");
}
function RouteMapDetail() {
    if ($('#HfRouteIdMap').val() != 0) {
        var routeId = $('#HfRouteIdMap').val();
        var value = ".move-map-" + routeId;
        $(value).attr('checked', 'checked').trigger('click');
    }
}
function EditNENRoute() {
    var routeId = $(this).data("routeid");
    var routeName = $(this).data("routename");
    var routeDesc = $(this).data("routedesc");
    var routeFromAddress = $(this).data("fromaddress");
    var routeToAddress = $(this).data("toaddress");
    var isReturnRoute = $(this).data("isreturnroute");
    var nenId = $('#HFNEN_ID').val();
    var vInboxId = $('#HdnInboxID').val();
    var contentRefNo = $('#hfContentRefNum').val();
    setRouteNameEdit(routeName);
    $.ajax({
        url: '../Routes/LibraryRoutePartDetails',
        data: { ApplicationRevId: contentRefNo },
        type: 'POST',
        success: function (page) {
            newMap = page;
            mapLoadContainer = $('#map-load-container').html();
            routeDiv = $('#route').html();
            $('#banner-container').find('div#filters').remove();
            $('#map').html('');
            $('#map-load-container').html('');
            $('#route').show();
            $('#route').append($(page).find('#CreateRoute').html());
            LibraryRoutePartDetailsInit(routeId, 3);
            CheckSessionTimeOut();
            Map_size_fit();
            addscroll();
            EnLargeMap();
            $('#btnenlarge').hide();
            $('#IDEnLargeMap').hide();
            $('#movementMapFilterIcon').hide();
            $('#editnenrouteback').show();
            $("#RoteHeading").addClass("routeEditHeading");
            $('#gpxRoute').prop('checked', false);
            ViewRouteDetails(nenId, vInboxId, isReturnRoute, routeDesc, routeFromAddress, routeToAddress);
        }
    });
}
function ViewRouteDetails(nenId, vInboxId, isReturnRoute, routeDesc, fromAddress, toAddress) {
    var link = '../NENNotification/hualierRouteInfo?InboxitemId=' + vInboxId
        + '&NEN_ID=' + nenId
        + '&IsReturleg=' + isReturnRoute
        + '&routeDesc=' + routeDesc
        + '&fromAddress=' + fromAddress
        + '&toAddress=' + toAddress;
    var myWindow = window.open(link, 'myWindow', "width=500,height=300,scrollbars=yes,resizable =yes");
}
function EditRouteBackAction() {
    $('#movementMapFilterIcon').hide();
    $("#navbar").show();
    $("#Minimzeicon").show();
    $("#MaxmizeIocn").hide();
    $("#printbutton").show();
    $("#listitem").show();
    $("#movement-details").show();
    $("#map").addClass("maponload");
    $("#map-load-container").addClass("p-3");
    $("#map").removeClass("mapfulls");
    $("#map").removeClass("olMap");
    $("#map").removeClass("map-edit");
    $("#btnenlarge").removeClass("expand");
    $("#container-sub").removeClass("containerfulls");
    $("#helpdeskDelegation").removeClass("helpdeskfulls");
    $("#movement-map").removeClass("movementfulls");
    $("#mvpmap").removeClass("mvpmapfulls");
    $("#OpenLayers_Control_Panel_200").removeClass("horizontalMapRoad");
    $("#banner-container").removeClass("bannercfulls");
    $("#helpdeskDelegation").addClass("row");
    $("#banner-container").addClass("container-fluid");
    $("#movement-map").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
    $("#overlay_load").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
    $("#btnmap").removeClass("iconfulls");
    $("#generalmap").show();
    $("#footerdiv").show();
    $('#map').html('');
    $('#route').html(routeDiv);
    $('#route').hide();
    $('#map-load-container').html(mapLoadContainer);
    $('#route').hide();
    $('#gpxRoute').prop('checked', false);
    functionMapRefresh();
}
function openRoutenav() {
    document.getElementById("mySidenav1").style.width = "350px";
    document.getElementById("mySidenav1").style.display = "block"
    document.getElementById("route-content-2").style.display = "block"
    document.getElementById("route-content-1").style.display = "none"
    document.getElementById("card-swipe1").style.display = "block"
    document.getElementById("card-swipe2").style.display = "block"
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("mySidenav1").style.width = "auto";
        }
    }
    var x = window.matchMedia("(max-width: 410px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeRoutenav() {
    document.getElementById("mySidenav1").style.width = "0";
    document.getElementById("route-content-2").style.display = "none"
    document.getElementById("card-swipe2").style.display = "none";
    document.getElementById("card-swipe1").style.display = "block";
}
function NENRouteSaveCompletion() {
    GenerateNENRouteAssessment();
    ResetMapState();
}
function ResetMapState() {
    setPathState('routedisplayed');
    setPageState('readyidle');
    updateUI();
}
function GenerateNENRouteAssessment() {
    var isRenotif = $("#RouteName").val().toLowerCase() == "ne renotification api" ? 1 : 0;
    var isNenViaApi = Boolean($('#hf_IsNenViaApi').val().toLowerCase() == "true");
    var generateRouteAssessment = {
        ContentRefNo: $('#hfContentRefNum').val(),
        NotificationId: $('#hf_Notificationid').val(),
        AnalysisId: $('#StructAnalysisID').val(),
        VSoType: 0,
        IsRenotify: isRenotif,
        NoGenerateAffectedParties: true,
        IsNenApi: isNenViaApi
    }
    $.ajax({
        url: '../RouteAssessment/GenerateRouteAssessment',
        type: 'POST',
        data: generateRouteAssessment,
        beforeSend: function () {
            startAnimation();
            showToastMessage({
                message: "Route assessment in progress, please wait...",
                type: "warning"
            });
        },
        success: function (data) {
            stopAnimation();
            if (data) {
                showToastMessage({
                    message: "Route assessment updated.",
                    type: "success"
                });
            }
            else {
                showToastMessage({
                    message: "Route assessment updation failed.",
                    type: "error"
                });
            }
        },
        error: function () {
            showToastMessage({
                message: "Route assessment updation failed.",
                type: "error"
            });
        },
        complete: function () {
            RefreshDetailsOnReAssessment();
        }
    });
}
function RefreshDetailsOnReAssessment() {
    if (document.getElementById('routeDescription').style.display !== "none") {
        document.getElementById('routeDescription').style.display = "none";
        document.getElementById('up-chevlon12').style.display = "none";
        document.getElementById('down-chevlon12').style.display = "block";
        $(".routeOverview").trigger("click");
    }
    if (document.getElementById('RoadsRoute').style.display !== "none") {
        document.getElementById('RoadsRoute').style.display = "none";
        document.getElementById('up-chevlon13').style.display = "none";
        document.getElementById('down-chevlon13').style.display = "block";
        $(".routeRoads").trigger("click");
    }
    if (document.getElementById('affectedstructure').style.display !== "none") {
        document.getElementById('affectedstructure').style.display = "none";
        document.getElementById('up-chevlon10').style.display = "none";
        document.getElementById('down-chevlon10').style.display = "block";
        $(".affectedstructure").trigger("click");
    }
    if (document.getElementById('drivingInstruction').style.display !== "none") {
        document.getElementById('drivingInstruction').style.display = "none";
        document.getElementById('up-chevlon14').style.display = "none";
        document.getElementById('down-chevlon14').style.display = "block";
        $(".drivingInstruction").trigger("click");
    }
}