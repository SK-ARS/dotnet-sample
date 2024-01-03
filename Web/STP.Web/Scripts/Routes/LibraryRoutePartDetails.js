// $('.dropdown-toggle').dropdown();
$('body').on('click', '#btnbackroute', function (e) {
    e.preventDefault();
    BackToPreviousPage();
});
function BackToPreviousPage() {
    window.location = "../Routes/RoutePartLibrary";
}
var RouteFlag = $('#hf_RouteFlag').val();
var RouteID = parseInt($('#hf_RouteID').val());
var PageFlag = $('#hf_PageFlag').val();

function LibraryRoutePartDetailsInit(routeId, routeFlag) {
    if (routeId != null && routeId != undefined) {
        RouteID = routeId;
    }
    else {
        RouteID = parseInt($('#hf_RouteID').val());
    }
    if (routeFlag != null && routeFlag != undefined) {
        RouteFlag = routeFlag;
    }
    else {
        RouteFlag = $('#hf_RouteFlag').val();
    }
    PageFlag = $('#hf_PageFlag').val();
    $("#SearchPanelForm").empty();
    if (document.getElementById("RouteFilter") !== null) {
        var filterData = $('#planRouteFilterDiv').html();
        $('#planRouteFilterDiv').html('');
        $('#planRouteFilter').html(filterData);
    }

    document.getElementById('All').checked = true;
    document.getElementById('Affected').checked = false;
    document.getElementById('Affected').disabled = true;
    $("#toggleAllAffected").prop('checked', true);
    $("#All").change();
    $("#Affected").change();
    if (RouteFlag != 2 && RouteFlag != 1 && RouteFlag != 3) {
        if ($('#PortalType').val() == 696001) {
            SelectMenu(4);
        }
        if ($('#PortalType').val() == 696008) {
            SelectMenu(2);
        }
        mapfromroutesmenu = true;
    }
    removescroll();
    $("#map").html('');
    $("#map").load('../Routes/A2BPlanning?routeID=' + RouteID, function () {
        LoadLeftPanel();
        A2BPlanningInit();
        try {
            if ($('#IsNotif').val().toLowerCase() != "true" && $('#IsRenotify').val().toLowerCase() != "true") {

                document.getElementById('Notificationwarndiv').style.display = "none";
            }
            else {
                if ($('#PortalType').val() == '696001') {
                    if (routeexist() || $('#IsRenotify').val().toLowerCase() == "true") {
                        document.getElementById('Notificationwarndiv').style.display = "block";
                    }
                }
            }
        }
        catch (err) {}
        if (typeof document.getElementsByClassName('sortsubhead') != undefined && document.getElementsByClassName('sortsubhead').length >0) {
            if (document.getElementsByClassName('sortsubhead')[0].textContent.includes("Candidate route")) {
                document.querySelector('#map').classList.add('candmap-height');
            }
        }
        setTimeout(function () {
            if ($('#IsAgreedNotify').val() == "True" || $('#IsAgreedNotify').val() == "true" && $('.hp-map .RouteAppraisalToolbar').length > 0) {//--HE-7990
                $('.hp-map .RouteAppraisalToolbar').addClass("AgreedSOBtnCont");
            }
        }, 500);
    });

    if ($('#IsAgreedNotify').val() == "True" || $('#IsAgreedNotify').val() == "true") {//--HE-7990
        $('.DeleteRouteCls').remove();
        $('#IDswap').remove();
        $('#IDswap').remove();
        $('#From_location').attr('readonly', true);
        $('#To_location').attr('readonly', true);
        $('#divRouteActionsContainer .dropdwon_new').remove();
        $('#divRouteActionsContainer #btn_continue_to_route_summary').remove();
        $('.hp-map .RouteAppraisalToolbar').addClass("AgreedSOBtnCont");
    }

}
$(document).ready(function () {
    if (window.location.pathname.indexOf('/Routes/LibraryRoutePartDetails') >= 0) {
        LibraryRoutePartDetailsInit();
    }
    $('body').on('click', '#IDEnLargeMap', function (e) {
       
        e.preventDefault();
        EnLargeMap(this);
    });
    $('body').on('click', '#IDopenRouteFilters', function (e) {
        e.preventDefault();
        openRouteFilters(this);
    });
    $('body').on('click', '#card-swipe1', function (e) {
        e.preventDefault();
        openRoutenav(this);
    });
    //$('input[type="checkbox"]').change(function (event) {
    //    $('.bs-canvas-overlay').remove();
    //});
    $('body').on('click', '#card-swipe2', function (e) {
        e.preventDefault();
        closeRoutenav(this);
    });
    $('body').on('click', '#deletepath', function (e) {
        e.preventDefault();
        confirmRemovePath(this);
    });
    $('body').on('click', '#IDswap', function (e) {
        e.preventDefault();
        swap(this);
    });
    $('body').on('click', '#IDClearall', function (e) {
        e.preventDefault();
        Clearall(this);
    });
    $('body').on('click', '#IDviewMapFeatures', function (e) {
        e.preventDefault();
        viewMapFeatures(this);
    });
    $('body').on('click', '#IDviewRoads', function (e) {
        e.preventDefault();
        viewRoads(this);
    });
    $('body').on('click', '#IDviewBoundaries', function (e) {
        e.preventDefault();
        viewBoundaries(this);
    });
    $('body').on('click', '#IDviewStructuresConstraints', function (e) {
        e.preventDefault();
        viewStructuresConstraints(this);
    });
    $('body').on('click', '#btn_back', function (e) {
        location.href = '../Routes/RoutePartLibrary';
    });
    $('body').on('click', '#btn_finishedit', function (e) {
        finishEdit();
    });
    $('body').on('click', '#btn_plan', function (e) {
        viewEditRouteFlagStructures = 0;
        viewEditRouteFlagConstraints = 0;
        checkForReturnLeg();
        if ($('#hf_IsPlanMovmentGlobal').length > 0) {
            IsRoutePlanned=true;
        }
    });
    $('body').on('click', '#btn_clear', function (e) {
        confirmClearRoute();
    });
    $('body').on('click', '#btn_re_plan', function (e) {
        var isLibrary = 0;
        if ($('#hf_IsLibrary').val() == 'True') {
            isLibrary = 1;
        }
        var routeIdReplan = RouteID != 0 && RouteID != '' && RouteID != undefined ? RouteID : $('#showRouteIdMap').val();
        ReplanBrokenRoutes(routeIdReplan, isLibrary, true, function (response) {
            if (response.isSuccess) {
                var routePart = response.result;
                clearRouteReplan(function () { selectRoute(routePart); });
                routePart.Esdal2Broken = 1;//indication this field is a broken and auto replanned
                routePart.IsAutoReplan = 1;//set 1 for auto replanned route using replan button
            }
            else {
                var msg = 'No route can be planned. Which may be due to legal restrictions on the route. Please change the start/end/way points and plan the route.';
                ShowErrorPopup(msg);
            }
            $('#btn_re_plan').hide();
        });
        
    });
    $('body').on('click', '#closebtn', function (e) {
        closeRouteFilters();
    });

    $('body').on('click', '#btn_addviapoint', function (e) {
        insertwaypoint(3);
    });

    $('body').on('click', '#btn_addwaypoint', function (e) {
        insertwaypoint(2);
    });

    $('body').on('change', '#PoliceBoundaries', function (e) {
        startAnimation();
       // $('.bs-canvas-overlay').remove();
        if (this.checked) {
            PoliceBoundaries(true);
        }
        else {
            PoliceBoundaries(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#LABoundaries', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            LABoundaries(true);
        }
        else {
            LABoundaries(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#NHBoundaries', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();

        if (this.checked) {
            NHBoundaries(true);
        }
        else {
            NHBoundaries(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#TfLRoads', function (e) {
        startAnimation();
       // $('.bs-canvas-overlay').remove();

        if (this.checked) {
            TfLRoads(true);
        }
        else {
            TfLRoads(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
   
    $('body').on('change', '#WelshTrunkRoads', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            WelshTrunkRoads(true);
        }
        else {
            WelshTrunkRoads(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
   
        $('body').on('change', '#ScottishTrunkRoads', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            ScottishTrunkRoads(true);
        }
        else {
            ScottishTrunkRoads(false);
        }
            stopAnimation();
            $('#overlay').show();
            $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#Restaurants', function (e) {
        
        startAnimation();
       // $('.bs-canvas-overlay').remove();
        if (this.checked) {
            Restaurants(true);
        }
        else {
            Restaurants(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#Hospital', function (e) {

        startAnimation();
       //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            Hospital(true);
        }
        else {
            Hospital(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#Parkbay', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            Parkbay(true);
        }
        else {
            Parkbay(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#FiInst', function (e) {
        startAnimation();
       // $('.bs-canvas-overlay').remove();
        if (this.checked) {
            FiInst(true);
        }
        else {
            FiInst(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#Entertainment', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            Entertainment(true);
        }
        else {
            Entertainment(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#BuisFeci', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            BuisFeci(true);
        }
        else {
            BuisFeci(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#CMSServ', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            CMSServ(true);
        }
        else {
            CMSServ(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#Shopping', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            Shopping(true);
        }
        else {
            Shopping(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#EduIn', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            EduIn(true);
        }
        else {
            EduIn(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#AutMob', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            AutMob(true);
        }
        else {
            AutMob(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#TransHub', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            TransHub(true);
        }
        else {
            TransHub(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#TravelDest', function (e) {
        startAnimation();
        if (this.checked) {
            TravelDest(true);
        }
        else {
            TravelDest(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });
    $('body').on('change', '#ParkAndRec', function (e) {
        startAnimation();
        //$('.bs-canvas-overlay').remove();
        if (this.checked) {
            ParkAndRec(true);
        }
        else {
            ParkAndRec(false);
        }
        stopAnimation();
        $('#overlay').show();
        $('.bs-canvas-overlay').remove();
    });

    $('body').on('change', '#toggleAllAffected', function (e) {
        ToggleAllAffected();
    });
    
});
function myFunction(item) {
    if (item.style.backgroundColor === "lightgrey") {
        item.style.backgroundColor = "white";
    }
    else {
        item.style.backgroundColor = "lightgrey";
    }
}
function onremove() {
    if (document.getElementById("onfocus1").style.backgroundColor === "red" ||
        document.getElementById("onfocus2").style.backgroundColor === "red" ||
        document.getElementById("onfocus2").style.backgroundColor === "red") {
        document.getElementById("onfocus1").style.backgroundColor === "white"
        document.getElementById("onfocus2").style.backgroundColor === "white"
        document.getElementById("onfocus3").style.backgroundColor === "white"
    }
}

// showing user-setting inside vertical menu
function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}
// showing filter-settings
function openRouteFilters() {
    if ($(".planRouteFilter").length>0) {
        $('.planRouteFilter').css("width", "400px");//$('.planRouteFilter').css("margin-right", "0px");
        $('.planRouteFilter').css("margin", "0 0 0 0");//$('.planRouteFilter').css("width", "450px");

    }
    else {
        $('#filters').css("width", "400px");//$('#filters').css("width", "450px");
        $('#filters').css("margin", "0 0 0 0");//$("#filters").css('margin-right', "0");
    }

    $('body').prepend('<div class="bs-canvas-overlay bg-dark position-fixed w-100 h-100"></div>');

    //$("#overlay").show();
    //$("#overlay").css("background", "rgba(0, 0, 0, 0)");
    //$("#overlay").css("z-index", "0");
    //$("#banner").css('filter', "brightness(0.5)");
    //$("#banner").css('background', "white");
    //$("#navbar").css('filter', "brightness(0.5)");

    //$("#navbar").css('background', "white");
    function myFunction(x) {
        if (x.matches) { // If media query matches

            if (document.getElementById("planRouteFilter") !== null) {
                $('.planRouteFilter').css("width", "200px");
            }
            else {
                $("#filters").css('width', "200px");
            }
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeRouteFilters() {
    $('.bs-canvas-overlay').remove();
    if ($(".planRouteFilter").length > 0) 
        $('.planRouteFilter').css("margin", "0 -400px 0 0");
    else 
        $('#filters').css("margin", "0 -400px 0 0");
}
//  on save in add-caution
function onSave() {
    // if (document.getElementById('check-caution').style.display !== "none") {
    document.getElementById('check-caution').style.display = "block"
    document.getElementById('add-caution').style.display = "none"
}
// on save in check-caution
function closecaution() {
    document.getElementById('check-caution').style.display = "none"
    document.getElementById('add-caution').style.display = "block"
}
function failed() {
    document.getElementById('add-caution').style.display = "block"
    document.getElementById('check-caution').style.display = "none"
}
function opennewNav() {
    document.getElementById("mySidenav1").style.width = "400px";
    document.getElementById("mySidenav1").style.display = "block"
    document.getElementById("route-content-1").style.display = "block"
    document.getElementById("route-content-2").style.display = "none"
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
function closenewNav() {
    document.getElementById("mySidenav1").style.width = "0";
    document.getElementById("route-content-1").style.display = "none"
    document.getElementById("card-swipe2").style.display = "none";
    document.getElementById("card-swipe1").style.display = "block";
}
function closeRoutenav() {
    document.getElementById("mySidenav1").style.width = "0";
    //document.getElementById("route-content-2").style.display = "none"
    // document.getElementById("esdaldclaimer").style.display = "none"
    document.getElementById("card-swipe2").style.display = "none";
    //document.getElementById("card-swipe1").style.display = "block";
}
// search structures/constraints in filter
function viewStructuresConstraints() {
    if (document.getElementById('viewStructuresConstraints').style.display !== "none") {
        document.getElementById('viewStructuresConstraints').style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById('viewStructuresConstraints').style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}
// search boundaries in filter
function viewBoundaries() {
    if (document.getElementById('viewBoundaries').style.display !== "none") {
        document.getElementById('viewBoundaries').style.display = "none"
        document.getElementById('chevlon-up-icon2').style.display = "none"
        document.getElementById('chevlon-down-icon2').style.display = "block"
    }
    else {
        document.getElementById('viewBoundaries').style.display = "block"
        document.getElementById('chevlon-up-icon2').style.display = "block"
        document.getElementById('chevlon-down-icon2').style.display = "none"
    }
}
// search roads in filter
function viewRoads() {
    if (document.getElementById('viewroads').style.display !== "none") {
        document.getElementById('viewroads').style.display = "none"
        document.getElementById('chevlon-up-icon21').style.display = "none"
        document.getElementById('chevlon-down-icon21').style.display = "block"

    }
    else {
        document.getElementById('viewroads').style.display = "block"
        document.getElementById('chevlon-up-icon21').style.display = "block"
        document.getElementById('chevlon-down-icon21').style.display = "none"

    }
}
//search map features
function viewMapFeatures() {
    if (document.getElementById('viewmapfeat').style.display !== "none") {
        document.getElementById('viewmapfeat').style.display = "none"
        document.getElementById('chevlon-up-icon212').style.display = "none"
        document.getElementById('chevlon-down-icon212').style.display = "block"

    }
    else {
        document.getElementById('viewmapfeat').style.display = "block"
        document.getElementById('chevlon-up-icon212').style.display = "block"
        document.getElementById('chevlon-down-icon212').style.display = "none"

    }
}
// search organisation in filter
function viewAllstructures() {
    if (document.getElementById('viewAllstructures').style.display !== "none") {
        document.getElementById('viewAllstructures').style.display = "none"
        document.getElementById('chevlon-up-icon211').style.display = "none"
        document.getElementById('chevlon-down-icon211').style.display = "block"
    }
    else {
        document.getElementById('viewAllstructures').style.display = "block"
        document.getElementById('chevlon-up-icon211').style.display = "block"
        document.getElementById('chevlon-down-icon211').style.display = "none"
    }
}
function Clearall() {
   
    startAnimation();
    $("#Affected").prop('checked',false);
    $("#toggleAllAffected").prop('checked', true);
    $("#All").prop('checked',true);
    $("#Structs").prop('checked',false);
    $("#Underbridge").prop('checked',false);
    $("#Overbridge").prop('checked',false);
    $("#UnderAndOverbridge").prop('checked',false);
    $("#LevelCrossing").prop('checked',false);
    $("#Constraints").prop('checked',false);
    $("#PoliceBoundaries").prop('checked',false);
    $("#LABoundaries").prop('checked',false);
    $("#NHBoundaries").prop('checked',false);
    $("#TfLRoads").prop('checked',false);
    $("#WelshTrunkRoads").prop('checked',false);
    $("#ScottishTrunkRoads").prop('checked',false);
    $("#ParkAndRec").prop('checked',false);
    $("#TravelDest").prop('checked',false);
    $("#TransHub").prop('checked',false);
    $("#AutMob").prop('checked',false);
    $("#EduIn").prop('checked',false);
    $("#Shopping").prop('checked',false);
    $("#CMSServ").prop('checked',false);
    $("#BuisFeci").prop('checked',false);
    $("#Entertainment").prop('checked',false);
    $("#FiInst").prop('checked',false);
    $("#Parkbay").prop('checked',false);
    $("#Hospital").prop('checked',false);
    $("#Restaurants").prop('checked',false);
    $("#AILStructs").prop('checked',false);
    NHBoundaries(false);
    PoliceBoundaries(false);
    LABoundaries(false);
    TfLRoads(false);
    ParkAndRec(false);
    TravelDest(false);
    TransHub(false);
    AutMob(false);
    EduIn(false);
    Shopping(false);
    CMSServ(false);
    BuisFeci(false);
    Entertainment(false);
    FiInst(false);
    Parkbay(false);
    Hospital(false);
    Restaurants(false);
    showStructBoundsA2B();
    closeRouteFilters();
    stopAnimation();
}
function ApplyFilterMap() {
    startAnimation();
    if (document.getElementById('NHBoundaries').checked == true) {
        NHBoundaries(true);
    }
    else {
        NHBoundaries(false);
    }
    if (document.getElementById('PoliceBoundaries').checked == true) {
        PoliceBoundaries(true);
    }
    else {
        PoliceBoundaries(false);
    }
    if (document.getElementById('LABoundaries').checked == true) {
        LABoundaries(true);
    }
    else {
        LABoundaries(false);
    }
    if (document.getElementById('TfLRoads').checked == true) {
        TfLRoads(true);
    }
    else {
        TfLRoads(false);
    }
    showStructBoundsA2B();
    stopAnimation();
}
function ToggleAllAffected() {
    if ($("#toggleAllAffected").prop('checked') == true) {
        $('#toggleAllAffected').val(true);
        document.getElementById('All').checked = true;
        document.getElementById('Affected').checked = false;
    }
    else if ($("#toggleAllAffected").prop('checked') == false) {
        if (document.getElementById('Affected').disabled == false) {
            $('#toggleAllAffected').val(false);
            document.getElementById('Affected').checked = true;
            document.getElementById('All').checked = false;
        }
        else {
            $("#toggleAllAffected").prop('checked', true);
            $('#toggleAllAffected').val(true);
            document.getElementById('All').checked = true;
            document.getElementById('Affected').checked = false;
        }
    }
    showStructBoundsA2B();
}
function LoadLeftPanel() {
    var retlegflag = false;
    if ($('#hf_ShowReturnLeg').val() == 2) { retlegflag = true; }
    var url = '../Routes/A2BLeftPanel?routeID=' + RouteID + '&val=' + PageFlag;
    $.ajax({
        url: url,
        type: 'POST',
        data: { ShowReturnLegFlag: retlegflag },
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html(page);
            $('#leftpanel').show();
            A2BLeftPanelInit();
            if ($('#hf_ShowReturnLeg').val() == 1) {
                $('#ReturnLegForNotif').hide();
            }
            else {
                $('#ReturnLegForNotif').show();
            }
            CheckSessionTimeOut();
            openRoutenav();
        },
        complete: function () {
            stopAnimation();
            if (typeof $("#hIs_NEN").val() != "undefined" && $("#hIs_NEN").val() == "true") {
                $("#tabA2Bpanel").css('margin-top', '10px');
            }
        }
    });
}
function EnLargeMap() {
   
    var url = geturl(location.href);
    if (url == 'SORTApplicationSORTListMovemnets') {
        EnLargeMapCandidate();
    }
    else if (url == 'NENNotificationNE_Notification') {
        NENRouteFullScreen();
    }
    else {
        EnLargeMapNotification();
    }

}
function NENRouteFullScreen() {
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
        $("#back_btn").hide();
        $("#progressBar").hide();
        $("#RoteHeading").hide();
        $("#sort-menu-list").hide();
        $("#NEHeading").hide();
        $("#main-sort-content").removeClass("main-sort-content");
        $("#main-sort-content").addClass("nenfulls");
        $('body').css('overflow-y', 'hidden');
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
        $("#back_btn").show();
        $("#RoteHeading").show();
        $("#sort-menu-list").show();
        $("#NEHeading").show();
        document.getElementById('veh').id = 'vehicles';
        document.getElementById('div').id = 'banner-container';
        $('body').css('overflow-y', 'scroll');
        $('map').css('height', '650px');
        $("#main-sort-content").addClass("main-sort-content");
        $("#main-sort-content").removeClass("nenfulls");
        $(".tableHead").show()

    }

}
function scroll() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
function EnLargeMapCandidate() {
    
    if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen  

        $("#navbar").hide();
        $("#Minimzeicon").hide();
        $("#MaxmizeIocn").show();
        $("#banner-container").hide();
        $("#banner").hide();
        $("#back_btn_Rt").hide();
        $("#sort-flow").hide();
        $("#progressBar").hide();
        $("#RoteHeading").hide();
        $("#vehicles").show();
        $("#backbutton").hide();
        $("#footerdiv").hide();
       
        $("#banner-container").removeClass("container-fluid");
        $("#banner1").removeClass("map-icons");
        $("#banner-container").addClass("bannercfulls");
        $("#banner1").addClass("banner1");
        $('body').css('overflow-y', 'hidden');
        scroll();
        mapResize();
    } else { //mode changes to minimze

        $("#navbar").show();
        $("#Minimzeicon").show();
        $("#MaxmizeIocn").hide();
        $("#banner-container").show();
        $("#banner").show();
        $("#progressBar").show();
        $("#backbutton").show();
        $("#banner-container").addClass("container-fluid");
      
        $("#banner1").addClass("map-icons");
        $("#banner-container").removeClass("bannercfulls");
        $("#banner1").removeClass("banner1");
        $("#footerdiv").show();
        $("#back_btn_Rt").show();
        $("#sort-flow").show();
        $("#RoteHeading").show();
        $('body').css('overflow-y', 'scroll');
    }
}
function EnLargeMapNotification() {
    
    if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
        Ismapenlarged = 1;
        $("body").addClass('mapClassl');
        $("#navbar").hide();
        mapResize();
        $("#footer").hide();
        $("#back_btn_Altered").hide();
        $("#RoteHeading").hide();
        $("#divRouteActionsContainer").hide();

        mapResize();
        $("#sort-inner-map").removeClass("inner-map-sort-tab");
        $("#sort-inner-map").addClass("inner-map-sort-tab-expand");
        $("#Minimzeicon").hide();
        $("#MaxmizeIocn").show();
        $('#btnbackroute').hide();
        $("#banner-container").removeClass("container-fluid");
        $("#banner").removeClass("vehicle-setting");
        $("#vehicles").removeClass("flowComponent");
        $("#vehicles").removeClass("vehicles");
        $('#vehicles').attr('id', 'veh');
        $('#banner-container').attr('id', 'div');
        $("#footerdiv").hide();
        $("#back_btn").hide();
        $("#backbuttondiv").hide();
        $("#progressBar").hide();
        $("#RoteHeading").hide();
        $('body').css('overflow-y', 'hidden');
        mapResize();
        $("#sort-menu-list").hide();
        $("#title").hide();
        scroll();
        document.querySelector('#map').classList.remove('candmap-height');



    } else { //mode changes to minimze
        //closeFullscreen();
        Ismapenlarged = 0;
        $("body").removeClass('mapClassl');
        $("#navbar").show();
        mapResize();
        $("#footer").show();
        $("#back_btn_Altered").show();
        $("#RoteHeading").show();
        $("#Minimzeicon").show();
        $("#MaxmizeIocn").hide();
        $("#divRouteActionsContainer").show();
        $("#banner-container").addClass("container-fluid");
        var portalType = $('#PortalType').val();
        if (portalType == '696001' || portalType == '696008') {//added condition so that this will apply in haulier and sort portal only
            $("#banner").addClass("vehicle-setting");
        }
        $("#vehicles").addClass("flowComponent");
        $("#progressBar").show();
        $("#footerdiv").show();
        //$("#back_btn").show();
        $("#RoteHeading").show();
         $("#sort-inner-map").removeClass("inner-map-sort-tab-expand");
        $("#sort-inner-map").addClass("inner-map-sort-tab");
        $('body').css('overflow-y', 'scroll');
        $('map').css('height', '650px');
        $("#sort-menu-list").show();
        $("#title").show();
        $('#btnbackroute').show();
        $("#backbuttondiv").show();
        if (document.getElementById('veh') != null) {
            document.getElementById('veh').id = 'vehicles';
        }
        if (document.getElementById('div') != null) {
            document.getElementById('div').id = 'banner-container';
        }
        if (typeof document.getElementsByClassName('sortsubhead') != undefined && document.getElementsByClassName('sortsubhead').length > 0) {
            if (document.getElementsByClassName('sortsubhead')[0].textContent.includes("Candidate route")) {
                document.querySelector('#map').classList.add('candmap-height');
            }
        }
    }
}