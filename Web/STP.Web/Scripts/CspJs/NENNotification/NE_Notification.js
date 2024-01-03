    var getValue = 0, routePartData;
    var HdnInboxID = $('#hinboxId').val();
    var HdnNENID = $('#hNEN_Id').val();
    var IsmyWindowOpen = 0;

    $("#sort-menu-list .card").click(function () {
        $("#sort-menu-list .active-card").each(function () {
            $(this).removeClass('active-card');
        });
        $(this).addClass('active-card');
    });

    function GetRouteDetails() {

        //LodingText_Change("Verify route details...");
        var VRouteId = $("#hNENRoute_Id").val(), RouteName = "NEN Route";
        $.ajax({
            url: '../NENNotification/Get_Route_Points',
            data: {PlanRouteID: VRouteId },

            type: 'POST',
            cache: false,
            success: function (result) {

                verifyRouteDetails(result);
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                //LodingText_Change('Automatic route planning in progress...');
            }
        });
    }

    $(document).ready(function () {
        SelectMenu(2);
        $("#btnShowRouteTab").css("display", "none");
        var myWindow;
        var result1, VVehicleID = 0;
        var element_pos = 0;    // POSITION OF THE NEWLY CREATED ELEMENTS.
        var iCnt = 0;
        $('#dyntitleConfig').hide();
if($('#hf_User_id').val() ==  '') {
                    $("#OrganisationUserId").val('@ViewBag.User_id');
                    $('#hdnScrutinyUser').val(OrganisationUserId.options[OrganisationUserId.selectedIndex].text);
        }
        ShowGeneralDetails(1);

        if (window.history.forward(1) != null)
            window.history.forward(1);

    });
    //Show general details
    function ShowGeneralDetails(parm) {
        $(".generalOptions").show();
        $(".generalOptions").addClass("d-flex");
        $('#tab_1').show();
        var VNEN_RouteStatus = $("#hNEN_RouteStatus").val();
        if (VNEN_RouteStatus == 911001 || VNEN_RouteStatus == 911003 || VNEN_RouteStatus == 911005) {
            $("#btnShowRouteTab").show();
            $("#btnShowRouteTab").css("display", "block");
        }
        else
        $("#btnShowRouteTab").css("display", "none");
        //var VNEN_Id =  $("#hNEN_Id").val();
        var VRoutePartID = $('#hNENRoute_Id').val();
        var VehicleID = $('#hdnNENVehicleID').val();
        $('#pageheader').html('');
        $('#pageheader').html('<h3> NENotification - General</h3>');
        $.ajax({
            type: "GET",
            //url: '@Url.Action("Display_NENotification", "NENNotification")',
            url: "../NENNotification/Display_NENotification",
            data: { NEN_ID: HdnNENID, Route_Id: VRoutePartID, Notif_Id: '@ViewBag.NotificationID', Inbox_ItemID: HdnInboxID, NENVehicleID: VehicleID },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {

                $('#tab_1').find('#div_general').html('');
                //$('#tab_1').find('#div_general').html($(result).find('#GeneralDetails').html());
                $('#tab_1').find('#div_general').html(result);
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                NENViewConfiguration(VehicleID);
                //    $('#leftpanel').html('');
                //$("#leftpanel").load('../NENNotification/NENotificationAssignPopUp?inboxId=' + HdnInboxID + '&NenID=' + HdnNENID, function () {
                //        NENViewConfiguration(VehicleID);
                //        CheckSessionTimeOut();
                //        //stopAnimation();
                //    });

            }
        });
    }

    // Display automatic route planning details
    function PlannNENotification(parm)
    {
        $(".generalOptions").show();
        $(".generalOptions").addClass("d-flex");
        $("li[id=1]").addClass("nonactive");
        $("li[id=3]").removeClass("nonactive");
        $("li[id=3]").addClass("t");
        $("#btnShowRouteTab").css("display", "none");
        $('#overlay').show();
        $("#dialogue").hide();
        $('.loading').show();
        $("#mapTitle").show();
        $('#tab_1').hide();
        $('#tab_3').show();
        $('#RoutePart').show();
        $('#RoutePart').html('');

        var VNEN_Id = $("#hNEN_Id").val();
        $.ajax({
            url: "../Application/ListImportedRouteFromLibrary",
            type: 'POST',
            data: { NEN_ID: VNEN_Id, IsNEN: true , nenInboxId: $('#hdnNENInboxID').val() },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                CheckSessionTimeOut();
                $('#tab_3').show();
                $('#RoutePart').html('');
                $('#RoutePart').html(data);
                $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
                CheckSessionTimeOut();
            },
            error: function () {
            },
            complete: function () {

            }
        });
        $("#mapTitle").html('');
        $("#btnShowRouteTab").css("display", "none");
        if (parm == 1) {

            var VNEN_RouteStatus = $("#hNEN_RouteStatus").val();
            if (VNEN_RouteStatus == 911001 || VNEN_RouteStatus == 911003 || VNEN_RouteStatus == 911005) {
                $('#overlay').show();
                $("#dialogue").hide();
                startAnimation();
                GetRouteDetails();
            }
            else {
                stopAnimation();
            }
        }
        else {
            stopAnimation();
        }
        $('#pageheader').html('');
        $('#pageheader').html('<h3> NENotification - Route</h3>');
        $("#btnShowRouteTab").css("display", "none");
    }

    function EditPlannedRoute(RouteId, RN)
    {
        var rowID2 = "#status_" + $('#hNENRoute_Id').val(), rowID1 = "#status_" + $('#hreturnLeg_routeID').val();
        IsAutomaticPlaning = false;
        returnRouteError = false;//setting it to false by default
        if ($('#hNENRoute_Id').val() == RouteId)
            mainRouteCheck = true;//setting it true since main route is being checked.
        else
            mainRouteCheck = false;//setting it false since return route is being checked

        if (($(rowID2).html() == "Planned" || $(rowID2).html() == "Replanned") && ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error")) {
            returnRouteError = true;//setting it to true since return route has planning error or is unplanned.
        } else {
            returnRouteError = false;
        }

        if (($(rowID1).html() == "Unplanned" || ($(rowID2).html() == "Planning error")) && $('#hreturnLeg_routeID').val() == RouteId) {
            returnRouteError = false;//setting it to false again since main route isnt planned.
            ShowErrorPopup('Main route has planning  errors, so please plan the main route to clear the errors', 'CloseErrorPopup');
        }
        else {
            if (RouteId == $('#hreturnLeg_routeID').val()) {
                ActionForReturnRout = 1;
                if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error")     // check added for return route is not planned set IsReturnPlanned to 0
                { IsReturnPlanned = false; }
                else
                { IsReturnPlanned = true; }
            }
            else {
                ActionForReturnRout = 0;
                if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error")
                { IsReturnPlanned = false; }
                else
                { IsReturnPlanned = true; }
            }
            NENroute = false;
            LodingText_Change("Fetching route details...");
            var VRouteId = RouteId, RouteName = RN;
            //function for resize map
            var page = 0;

            $('#leftpanel').html('');
            $("#mapTitle").show();
            var showretleg = 0;

            showretleg = 2;

            $('#RoutePart').show();
            $('#RoutePart').html('');
            if (returnRouteError == true && mainRouteCheck == false) {
                getReturnRouteNorthingEsting(page, RouteName, VRouteId, showretleg);
            }
            else {
                $.ajax({
                    url: '../Routes/LibraryRoutePartDetails',
                    data: { RouteFlag: page, ApplicationRevId: 0, plannedRouteName: RouteName, plannedRouteId: VRouteId, PageFlag: "U", ShowReturnLeg: showretleg, IsNEN: true },
                    beforeSend: function () {
                        startAnimation();
                    },
                    type: 'GET',
                    cache: false,
                    success: function (page) {
                        $('#tab_3').show();
                        $('#RoutePart').html('');
                        $('#RoutePart').append($(page).find('#CreateRoute').html());

                        CheckSessionTimeOut();
                        Map_size_fit();
                        if ('@Session["RouteFlag"]' == 3) {
                            $('#RoutePart').append('<div class="row mt-4" style="float: right; ">' + '<div class= ' +"mt-2 pl-0 ml-0"+'>'+'<button title="Go back to the route list" class="btn outline- btn-primary SOAButtonHelper ml2 mb-2" aria-hidden="true" type="button" onclick="displayList()">BACK</button>'+'</div >'+'</div >');
                        }
                        else {
                            $('#RoutePart').append('<button id="back_btn" class="btn_reg_back next btngrad btnrds btnbdr" onclick="BindRouteParts()" type="button" data-icon="" aria-hidden="true">Back</button>');
                        }
                        $("#mapTitle").html('');
                        addscroll();

                    },
                    error: function (xhr, textStatus, errorThrown) {
                        location.reload();
                    },
                    complete: function () {
                        stopAnimation();
                    }
                });;
            }
        }
    }
    $('body').on('click', '#span1-NENViewConfiguration', function (e) {
        e.preventDefault();
        var VehicleId = $(this).data('VehicleId');
        NENViewConfiguration(VehicleId);
    });
    $('body').on('click', '#span2-NENViewConfiguration', function (e) {
        e.preventDefault();
        var VehicleId = $(this).data('VehicleId');
        NENViewConfiguration(VehicleId);
    });
    function NENViewConfiguration(id)
    {
        if(id != 0){
            var isImportConfig = false;
            var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            if (AppRevId != 0) {
                isImportConfig = true
            }
            $.ajax({
                url: '../VehicleConfig/ViewNENConfiguration',
                type: 'GET',
                cache: false,
                data: { vehicleID: id, isRoute: true, movementId: 0, isImportConfiguration: false, flag: "VR1" },//, IsNEN: true },

                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                    $('#tab_1').find('#div_vehicle').html('');
                    $('#tab_1').find('#div_vehicle').html($(result).find('#vehicles'));
                    $('#tab_1').find('#btn_back_to_config').remove();
                    $('#tab_1').find('#View_Component').find('.form bdr_config').css('width', '');
                    $('#tab_1').show();
                    $('.vehiclecomp_div').css('float', 'none');
                    $('#div_general').css({ 'margin-top': 0 });
                    $('#dyntitleConfig').show();
                },
                error: function (xhr, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {
                    $("#overlay").hide();
                    $('.loading').hide();
                    stopAnimation();
                }
            });

            //});
        }
    }

    //Assign User For Scrutiny
    function btn_AssignUser() {

        getValue = $('#OrganisationUserId').val();
        @*var NewStatus = 911005;
        switch ('@ViewBag.NEN_RouteStatus') {
            case '911001'://Unplanned
                NewStatus = 911005;//Assign User For Scrutiny Unplanned Route Status
                break;
            case '911002'://Planned
                NewStatus = 911010;//Assign User For Scrutiny planned Route Status
                break;
            case '911003'://Planning Error
                NewStatus = 911005;//Assign User For Scrutiny Unplanned Route Status
                break;
            case '911004'://Replanned
                NewStatus = 911010;//Assign User For Scrutiny planned Route Status
                break;
            case '911005'://Assigned for scrutiny unplanned
                NewStatus = 911005;//Assign User For Scrutiny Unplanned Route Status
                break;
            case '911010'://Assigned for scrutiny planned
                NewStatus = 911010;//Assign User For Scrutiny planned Route Status
                break;
        }*@
        var FromUserDet = "--Select User--";
        if ($('#hdnScrutinyUser').val() != "--Select user--") {
            FromUserDet = $('#hdnScrutinyUser').val();
        }
        if ($('#OrganisationUserId').val() != "" && $('#OrganisationUserId').val() != undefined) {

            if ($('#hdnScrutinyUser').val() != OrganisationUserId.options[OrganisationUserId.selectedIndex].text) {
                var encryptNotificationId = "@TempData["EncryptNotiId"]";
                var paramList = {
                    InboxId: "@TempData["InBoxId"]",
                    UserId: getValue,
                    NotificationId: encryptNotificationId,
                    NenId: $('#hNEN_Id').val(),
                    //ROUTE_STATUS: NewStatus,
                    NenProcess: 1,
                    UserName: OrganisationUserId.options[OrganisationUserId.selectedIndex].text,
                    FromUserName: FromUserDet,
                    ESDALReference: "@TempData["EncryptEsdalRef"]"
                }
                $.ajax({
                    type: "POST",
                    //url: '@Url.Action("NENAssignUserForScrutiny", "NENNotification")',
                    url: '../NENNotification/NENAssignUserForScrutiny',
                    dataType: "json",
                    //contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(paramList),
                    processdata: true,
                    beforeSend: function () {
                        startAnimation();
                    },
                    success: function (result) {
                        if (result.result == true) {
                            $('#generalPopup').modal('hide');
                            ShowModalPopup('Notification assigned to "' + $("#OrganisationUserId option:selected").text() + '"');
                        } else {
                            ShowInfoPopup('Error while assigning.', 'WarningCancelBtn');
                        }
                        $('#hNENRoute_Id').val(result.NENRute_ID);
                        if (result.returnLeg_routeID != 0)
                            $('#hreturnLeg_routeID').val(result.returnLeg_routeID);
                    },
                    error: function () {
                    },
                    complete: function () {

                        $("#overlay").hide();
                        $('.loading').hide();

                        stopAnimation();

                    }
                });
            } else {
                $('#err_user_exists').text('Already assigned to user.');
            }
        } else {
            ShowInfoPopup('Please select user for scrutiny.', 'WarningCancelBtn');
        }
    }

    function BindRouteParts() {
        PlannNENotification(0);
        $(".slidingpanelstructures").removeClass("show").addClass("hide");
        $(".slidingpanelstructures").html('');
    }

    function btn_confirm() {
        //showWarningPopDialog('Functionality under process!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        //
        //Check oute status in Route_assessment table
        getValue = $('#OrgUserId').val() ? $('#OrgUserId').val() : 0;
        //if ($('#OrgUserId').val() != "") {
            // var encryptNotificationId = "@TempData["EncryptNotiId"]";
            var paramList = {
                InboxId: "@TempData["InBoxId"]",
                USER_ID: getValue,
                NEN_ID: $('#hNEN_Id').val()
                //NOTIFICATION_ID: encryptNotificationId,
                //ROUTE_STATUS: 911005,
                // NEN_PROCESS: 1
            }
        var Rt_Id_main = $('#hNENRoute_Id').val();
        var Rt_Id_return = $('#hreturnLeg_routeID').val();

        var isBroken = [];
        var isBrokenRoute = 0;
        var isBrokenReplan = 0;

        var isReturnBroken = [];
        var isBrokenReturnRoute = 0;
        var isBrokenReturnReplan = 0;
            $.ajax({
                //async: false,
                type: "POST",
                //url: '@Url.Action("GetRouteAssessmentStatus", "NENNotification")',
                //url: '../NENNotification/GetRouteAssessmentStatus',//Please uncomment this in case for Old changes required
                url: '../NENNotification/GetBothRouteAssessmentStatus',//Please comment this for old changes
                dataType: "json",
                //contentType: "application/json; charset=utf-8",
                data: JSON.stringify(paramList),
                processdata: true,
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {

                    //New changes done on 17-11-2017 (If any problem occurs please comment new changes and uncomment old changes)
                    //--------------NEW Changes----------------
                    var dataCollection = result;
                    var Rt_Id_main = $('#hNENRoute_Id').val();
                    var Rt_Id_return = $('#hreturnLeg_routeID').val();
                    if (dataCollection.result.length > 0) {
                        if (dataCollection.result.length > 1) {// Checking Main route status and return route status
                            if (dataCollection.result[0].RouteStatus == 911001 || dataCollection.result[1].RouteStatus == 911001) {//Unplanned
                                ShowInfoPopup('For ESDAL to assess the movement you need to plan all routes - please click on the Route tab', 'WarningCancelBtn');
                            }
                            else if ((dataCollection.result[0].RouteStatus == 911002 && dataCollection.result[1].RouteStatus == 911002) || (dataCollection.result[0].RouteStatus == 911010 && dataCollection.result[1].RouteStatus == 911010) || (dataCollection.result[0].Route_Status == 911011 && dataCollection.result[1].RouteStatus == 911011)) {//Planned & Replanned with scrutiny
                                //implimentation for showing broken routes after migration
                                isBroken = CheckIsBroken(Rt_Id_main, 0, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_main);
                                if (isBroken.length > 0) {
                                    isBrokenRoute = isBroken[0].isBroken;
                                    isBrokenReplan = isBroken[0].isReplan
                                }

                                if (Rt_Id_return > 0) {
                                    isReturnBroken = CheckIsBroken(Rt_Id_return, 0, 0, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_return);
                                    if (isReturnBroken.length > 0) {
                                        isBrokenReturnRoute = isReturnBroken[0].isBroken;
                                        isBrokenReturnReplan = isReturnBroken[0].isReplan
                                    }
                                }
                                if (isBrokenRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReplan<=1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.', 'WarningCancelBtn');
                                    }
                                }
                                else if (isBrokenReturnRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReturnReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else{
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.', 'WarningCancelBtn');
                                    }
                                }
                                else {
                                    ShowWarningPopup('By confirming the notification details you will no longer be able to edit the route. Select ‘yes’ to proceed and ‘no’ to return.', 'RedirectToAuthorisedMovement', 'WarningCancelBtn');
                                }
                            }
                            else if ((dataCollection.result[0].RouteStatus == 911002 && dataCollection.result[1].RouteStatus == 911004) || (dataCollection.result[0].RouteStatus == 911004 && dataCollection.result[1].RouteStatus == 911002)) {//Planned and Replanned
                                //implimentation for showing broken routes after migration
                                isBroken = CheckIsBroken(Rt_Id_main, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_main);
                                if (isBroken.length > 0) {
                                    isBrokenRoute = isBroken[0].isBroken;
                                    isBrokenReplan = isBroken[0].isReplan
                                }
                                if (Rt_Id_return > 0) {
                                    isReturnBroken = CheckIsBroken(Rt_Id_return, 0, 0, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_return);
                                    if (isReturnBroken.length > 0) {
                                        isBrokenReturnRoute = isReturnBroken[0].isBroken;
                                        isBrokenReturnReplan = isReturnBroken[0].isReplan
                                    }
                                }
                                if (isBrokenRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.', 'WarningCancelBtn');
                                    }
                                }
                                else if (isBrokenReturnRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReturnReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.', 'WarningCancelBtn');
                                    }
                                }
                                else {
                                    ShowWarningPopup('By confirming the notification details you will no longer be able to edit the route. Select ‘yes’ to proceed and ‘no’ to return.', 'RedirectToAuthorisedMovement', 'WarningCancelBtn');
                                }
                            }
                            else if ((dataCollection.result[0].RouteStatus == 911003 || dataCollection.result[1].RouteStatus == 911003)) {//Planning error
                                ShowInfoPopup('For ESDAL to assess the movement you need to plan all routes - please click on the Route tab', 'WarningCancelBtn');
                            }
                            else if ((dataCollection.result[0].RouteStatus == 911004 && dataCollection.result[1].RouteStatus == 911004)) {//Replanned
                                //implimentation for showing broken routes after migration
                                isBroken = CheckIsBroken(Rt_Id_main, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_main);
                                if (isBroken.length > 0) {
                                    isBrokenRoute = isBroken[0].isBroken;
                                    isBrokenReplan = isBroken[0].isReplan
                                }
                                if (Rt_Id_return > 0) {
                                    isReturnBroken = CheckIsBroken(Rt_Id_return, 0, 0, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_return);
                                    if (isReturnBroken.length > 0) {
                                        isBrokenReturnRoute = isReturnBroken[0].isBroken;
                                        isBrokenReturnReplan = isReturnBroken[0].isReplan
                                    }
                                }
                                if (isBrokenRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.','WarningCancelBtn');
                                    }
                                }
                                else if (isBrokenReturnRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReturnReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.', 'WarningCancelBtn');
                                    }
                                }
                                else {
                                    ShowWarningPopup('By confirming the notification details you will no longer be able to edit the route. Select ‘yes’ to proceed and ‘no’ to return.', 'RedirectToAuthorisedMovement', 'WarningCancelBtn');
                                }
                            }
                            else if ((dataCollection.result[0].RouteStatus == 911005 && dataCollection.result[1].RouteStatus == 911005)) {//Under scrutiny with unplanned
                                var username = $("#OrgUserId option:selected").text() != "--Select user--" ? $("#OrgUserId option:selected").text() : "";
                                //showWarningPopDialog('NE Notification is assigned for scrutiny "' + username + '",so before proceeding please plan route from route tab.', 'Ok', '', 'RedirectToRouteTab', '', 2, 'warning');
                                ShowInfoPopup('For ESDAL to assess the movement you need to plan all routes - please click on the Route tab',  'WarningCancelBtn');
                            }
                        }
                        else {//Checking only Main route status
                            if (dataCollection.result[0].RouteStatus == 911001) {//Unplanned
                                ShowInfoPopup('For ESDAL to assess this movement you need to plan the route - please click on the Route tab', 'WarningCancelBtn');
                            }
                            else if (dataCollection.result[0].RouteStatus == 911002 || dataCollection.result[0].RouteStatus == 911010 || dataCollection.result[0].RouteStatus == 911011) {//Planned & Replanned
                                //implimentation for showing broken routes after migration
                                isBroken = CheckIsBroken(Rt_Id_main, 0, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_main);
                                if (isBroken.length > 0) {
                                    isBrokenRoute = isBroken[0].isBroken;
                                    isBrokenReplan = isBroken[0].isReplan
                                }
                                if (isBrokenRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',  'WarningCancelBtn');
                                    }
                                }
                                else {
                                    ShowWarningPopup('By confirming the notification details you will no longer be able to edit the route. Select ‘yes’ to proceed and ‘no’ to return.', 'RedirectToAuthorisedMovement', 'WarningCancelBtn');
                                }
                            }
                            else if (dataCollection.result[0].RouteStatus == 911003) {//Planning error
                                ShowInfoPopup('For ESDAL to assess this movement you need to plan the route - please click on the Route tab', 'WarningCancelBtn');
                            }
                            else if (dataCollection.result[0].RouteStatus == 911004) {//Replanned
                                //implimentation for showing broken routes after migration
                                var Rt_Id_main = $('#hNENRoute_Id').val();
                                isBroken = CheckIsBroken(Rt_Id_main, 0, 0, 0, 0, 0);//CheckIsBroken(Rt_Id_main);
                                if (isBroken.length > 0) {
                                    isBrokenRoute = isBroken[0].isBroken;
                                    isBrokenReplan = isBroken[0].isReplan
                                }
                                if (isBrokenRoute > 0) {
                                    ChnagePopupstyle();
                                    if (isBrokenReplan <= 1) {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the ‘Replan’ button to automatically re-plan the route and then save it again.', 'WarningCancelBtn');
                                    }
                                    else {
                                        ShowInfoPopup('Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.', 'WarningCancelBtn');
                                    }
                                    }
                                    else {
                                    ShowWarningPopup('By confirming the notification details you will no longer be able to edit the route. Select ‘yes’ to proceed and ‘no’ to return.', 'RedirectToAuthorisedMovement', 'WarningCancelBtn');
                                }
                            }
                            else if (dataCollection.result[0].RouteStatus == 911005) {//Under scrutiny with unplanned
                                var username = $("#OrgUserId option:selected").text() != "--Select user--" ? $("#OrgUserId option:selected").text() : "";
                                //showWarningPopDialog('NE Notification is assigned for scrutiny "' + username + '",so before proceeding please plan route from route tab.', 'Ok', '', 'RedirectToRouteTab', '', 2, 'warning');
                                ShowInfoPopup('For ESDAL to assess this movement you need to plan the route - please click on the Route tab', 'WarningCancelBtn');
                            }
                        }
                    }

                },
                error: function () {
                },
                complete: function () {
                    stopAnimation();
                    //$("#overlay").hide();
                    //$('.loading').hide();
                }
            });
        //}
    }

    function RedirectToAuthorisedMovement() {
        var encryptNotificationId = "@TempData["EncryptNotiId"]";
        var Inbox_Id= "@TempData["InBoxId"]";
        var EsdalRefNo = "@TempData["EncryptEsdalRef"]";
        var NENroute = $('#hdnNENroute').val();
        var NENInboxID = $('#hdnNENInboxID').val();
        var NENEsdal_Ref = $('#hdnNENEsdal_ref').val();
        var NENNotifID = $('#hdnNENNotifID').val();
        //update inbox_asssessment table
        getValue = $('#OrgUserId').val();
        //if ($('#OrgUserId').val() != "") {
        // var encryptNotificationId = "@TempData["EncryptNotiId"]";
        var VRoutePartID = $('#hNENRoute_Id').val();

        // WarningCancelBtn();
        CloseWarningPopup();
        var paramList = {
            InboxId: Inbox_Id,
            USER_ID: getValue,
            Route_ID: VRoutePartID,
            routeStatus: 911006
            //NOTIFICATION_ID: encryptNotificationId,
            //ROUTE_STATUS: 911005,
            // NEN_PROCESS: 1
        }
        var analysisParamList = {
            NotificationId: NENNotifID,
            Inbox_ID: Inbox_Id,
            AnalysisId: 0, //not possible to pass
            ContentRefNo: "" //not possible to pass
        }
        var RAPerformed = false;
        $.ajax({
            type: "POST",
            url: '@Url.Action("NENRouteAnalysis","NENNotification")',
                        dataType: "json",
                        //contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(analysisParamList),
                        processdata: true,
                        beforeSend: function () {
                            startAnimation('Performing route analysis');
                        },
                        success: function (result) {

                            var RAPerformed = result;
                            if (RAPerformed.result==true)
                            {
                                $.ajax({
                                    //async: false,
                                    type: "POST",
                                    url: '@Url.Action("UpdateRouteAssessmentStatus", "NENNotification")',
                                    dataType: "json",
                                    //contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify(paramList),
                                    processdata: true,
                                    cache: false,
                                    beforeSend: function () {
                                        startAnimation('Updating route assessment status');
                                    },
                                    success: function (result) {

                                    },
                                    error: function () {
                                    },
                                    complete: function () {

                                        stopAnimation();
                                        window.location.href = '../Movements/AuthorizeMovementGeneral' + EncodedQueryString('Notificationid=' + NENNotifID + '&esdal_ref=' + NENEsdal_Ref + '&route=' + NENroute + '&inboxId=' + NENInboxID);

                                    }
                                 });
                            }
                            else {
                                stopAnimation();
                                showWarningPopDialog('Route analysis is not performed, please try after some time.', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
                            }
                        },
                        error: function () {
                        },
                        complete: function () {

                        }
         });
       @* var result = 1;//NEN_RouteAnalysis();
        if (result == 1) {
            WarningCancelBtn();
            var paramList = {
                InboxId: Inbox_Id,
                USER_ID: getValue,
                Route_ID: VRoutePartID,
                routeStatus: 911006
                //NOTIFICATION_ID: encryptNotificationId,
                //ROUTE_STATUS: 911005,
                // NEN_PROCESS: 1
            }
            var analysisParamList={
                NotificationId: NENNotifID,
                Inbox_ID: Inbox_Id,
                AnalysisId:0, //not possible to pass
                ContentRefNo: "", //not possible to pass
                ESDALRefNo: NENEsdal_Ref
            }
            var RAPerformed = false;

            $.ajax({
                //async: false,
                type: "POST",
                url: '@Url.Action("UpdateRouteAssessmentStatus", "NENNotification")',
                dataType: "json",
                //contentType: "application/json; charset=utf-8",
                data: JSON.stringify(paramList),
                processdata: true,
                cache: false,
                beforeSend: function () {
                    startAnimation('Performing route analysis');
                },
                success: function (result) {
                    $.ajax({
                        type: "POST",
                        url: '@Url.Action("NENRouteAnalysis","NENNotification")',
                        dataType: "json",
                        //contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(analysisParamList),
                        processdata: true,
                        beforeSend: function () {
                            startAnimation('Performing route analysis');
                        },
                        success: function (result) {

                            RAPerformed = result;
                        },
                        error: function () {
                        },
                        complete: function () {
                            //if (RAPerformed) {
                            //    startAnimation('Opening authorize general page ...');
                            //}
                            //else {

                            //}
                            startAnimation('Performing route analysis');
                        }
                    });
                },
                error: function () {
                },
                complete: function () {

                    if (RAPerformed) {
                        stopAnimation();
                        window.location.href = '../Movements/AuthorizeMovementGeneral?Notificationid=' + NENNotifID + '&esdal_ref=' + NENEsdal_Ref + '&route=' + NENroute + '&inboxId=' + NENInboxID;
                    }
                    else {
                        showWarningPopDialog('Route analysis is not performed, please try after some time.', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
                    }
                }
            });
        } else {
            showWarningPopDialog('Route analysis not performed.', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
        }*@
       // }
        //window.location.href = '../Movements/AuthorizeMovementGeneral?Notificationid=' + encryptNotificationId + '&esdal_ref=' + EsdalRefNo + '&inboxId=' + InboxId;
    }

    function NEN_RouteAnalysis() {
        var Inbox_ID = $('#hdnNENInboxID').val();
        var Notif_ID = $('#hdnNENNotifID').val();
        //var NENAnalysis_ID = $('#htnNENAna_ID').val();
        //var ContentRef_NO = $('#htnContentRef_NO').val();
        $.ajax({
            type: "POST",
            url: '../NENNotification/NENRouteAnalysis',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: { NotificationId: Notif_ID, InboxId: Inbox_ID },
            processdata: true,
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.result == true) {
                    return 1;//
                } else {
                    return 0;
                }
            },
            error: function () {
            },
            complete: function () {
                $("#overlay").hide();
                $('.loading').hide();
            }
        });
    }

    function RedirectToRouteTab() {
        WarningCancelBtn();
        //$("#tab_1").hide();
        //$("#tab_3").show();
    }

    function btn_collab() {
        var paramList = {
            InboxId: "@TempData["InBoxId"]",
            USER_ID:  $('#hUserId').val(),
            NEN_ID: $('#hNEN_Id').val()
        }
        $.ajax({
            //async: false,
            type: "POST",
            url: '../NENNotification/GetRouteAssessmentStatus',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramList),
            processdata: true,
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.result == 911001 || result.result == 911003 || result.result == 911005) {
                    ShowWarningPopup('The route has not been viewed, edited or saved. Are you sure you want to Accept / Reject the notification now?', 'NENCollaborationView', 'WarningCancelBtn');
                } else {
                    NENCollaborationView();
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });


    }
    function ChnagePopupstyle() {
        //$('.popup111 .message111').css({ 'height': '170px' });
        //$('.popup111').css({ 'height': '221px' });
    }

    function NENCollaborationView() {
        startAnimation();
        WarningCancelBtn();
        var randomNumber = Math.random();
        removescroll();
        //$("#dialogue").html('');
        var loggedInContactId = $('#hdnContactId').val() != 0 ? $('#hdnContactId').val() : 0;
        var loggedInOrgId = $('#hdnOrgId').val();
        var IS_MOST_RECENT = $('#hdnIsMostRecent').val();
        var NextNoLongerAffected  = $('#hf_NextNoLongerAffected').val(); 
        var Email  = $('#hf_NENEmail').val(); 
        var esdalRef  = $('#hf_EsdalRefNum'.replace("#", "~').val(); 
        var notifid  = $('#hf_NotificationID').val(); 
        var doc_id  = $('#hf_DOCUMENT_ID').val(); 
        var inbox_id  = $('#hf_inboxId').val(); 
        var rt_type  = $('#hf_routeType.Replace(" ", "').val(); 
        startAnimation();
        $("#popupDialogue").load("../Movements/ViewCollaborationStatusAndNotes?Notificationid=" + notifid + "&documentid=" + doc_id + "&inBoxId=" + inbox_id + "&email=" + Email + "&esdalRef=" + esdalRef + "&contactId=" + loggedInContactId + "&route=@ViewBag.routeType.Replace(" ", "")&IS_MOST_RECENT=" + IS_MOST_RECENT + "&routeOriginal=" + rt_type + "&NextNoLongerAffected=" + NextNoLongerAffected + "&WipNENCollab=1&analysisId=0", function () {
            stopAnimation();
            $('#viewCollabModal').modal({ keyboard: false, backdrop: 'static' });
            $("#viewCollabModal").modal('show');
            $("#overlay").css("display","block");
            $("#popupDialogue").css("display", "block");
          });

    }

    function view_hualierdetails1() {
        var MSG = "<div id='tbl_route_details' style='margin-left: 12px'>";
        var desc ;
        $.ajax({
            url: '/NENNotification/GethualierRouteInfo',
            type: 'POST',
            async: true,
            //contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
            },
            data: JSON.stringify({ InboxitemId: '@TempData["InBoxId"]', NEN_ID: $('#hNEN_Id').val() }),
            success: function (result) {
                desc = result.desc;
                if ((desc.hualierGROUTE_DESC == null || desc.hualierGROUTE_DESC == "") && (desc.hualierDescpReturnLeg == null || desc.hualierDescpReturnLeg == ""))
                    MSG = MSG + " <div>Hualier route description not found.</div> </div>";
                else {

                    if (desc.hualierGROUTE_DESC != null && desc.hualierGROUTE_DESC != "")
                        MSG = MSG + "  <div> <span style='color: #050554;font-weight: 400 !important;'> Outward route description : </span>"+   desc.hualierGROUTE_DESC+"</div><br />";
                    if (desc.hualierDescpReturnLeg != null && desc.hualierDescpReturnLeg != "")
                        MSG = MSG + "  <div> <span style='color: #050554;font-weight: 400 !important;'> Inward route description : </span>" + desc.hualierDescpReturnLeg + "</div><br />";
                    MSG = MSG + "</div>";
                }
            },
            complete: function () {
                if (IsmyWindowOpen == 1) {
                    myWindow.close();
                    IsmyWindowOpen = 0;
                    }
                 myWindow = window.open("", "myWindow", "width=400,height=300");
                 myWindow.document.write("<p>"+MSG+"</p>");
                 myWindow.document.title = "Haulier route description";
                 myWindow.document.body.style.background = "#f0f0f0";
                 myWindow.document.body.style.color = "#414193";
                IsmyWindowOpen = 1;
                stopAnimation();
            }
        });

    }
    function view_hualierdetails() {
        var VNEN_id = $("#hNEN_Id").val(), VIsReturnleg = false;
        if (($('#hIsReturnLeg').val() == 'true' || $('#hIsReturnLeg').val() == 'True')) { VIsReturnleg = true }
        var VInboxID = $("#hinboxId").val();
        var link = '../NENNotification/hualierRouteInfo?InboxitemId=' + VInboxID + '&NEN_ID=' + VNEN_id + '&IsReturleg=' + VIsReturnleg;
        var myWindow = window.open(link, 'myWindow', "width=500,height=300,scrollbars=no,resizable =no");
    }

    function Assign_popup() {
        $("#overlay").show();
        $('.loading').show();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "../NENNotification/NENotificationAssignPopUp",
            //contentType: "application/json; charset=utf-8",
            data: { inboxId: HdnInboxID, NenID: HdnNENID},
            datatype: "json",
            beforeSend: function (){
                startAnimation();
            },
            success: function (data) {
                $('#generalPopupContent').html(data);
                $('#generalPopup').modal(options);
                $('#generalPopup').modal('show');
                $('.loading').hide();
                CheckSessionTimeOut();
            },
            error: function (data) {
                alert("Dynamic content load failed.");
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function VehicleNENDetails() {
        var ComponentCntrId = "viewNENConfigurationDetails";
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon').style.display = "none"
            document.getElementById('chevlon-down-icon').style.display = "block"
            $('#spnNENStatus').text("Show Details");
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon').style.display = "block"
            document.getElementById('chevlon-down-icon').style.display = "none"
            $('#spnNENStatus').text("Hide Details");
        }
    }
