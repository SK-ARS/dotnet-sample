var getValue = 0, routePartData;
var HdnInboxID = $('#hinboxId').val();
var HdnNENID = $('#hNEN_Id').val();
var IsmyWindowOpen = 0;
var hf_routeType = $('#hf_routeType').val();
var hf_EsdalRefNum = $('#hf_EsdalRefNum').val();
var encryptNotifId = $('#hf_encryptNotifId').val();
var encryptInboxId = $('#hf_encryptInBoxId').val();
var encryptEsdalRef = $('#hf_encryptEsdalRef').val();

$("#sort-menu-list .card").click(function () {
    if ($(this).hasClass('no-active-class-required')==false) {
        $("#sort-menu-list .active-card").each(function () {
            $(this).removeClass('active-card');
        });
        $(this).addClass('active-card');
    }
});

function GetRouteDetails(RoutePartId, Vreturnleg = false) {

    //var VRouteId = $("#hNENRoute_Id").val(), RouteName = "NEN Route";
    var isPlanningError = false;
    $.ajax({
        url: '../NENNotification/Get_Route_Points',
        data: { PlanRouteID: RoutePartId },
        async: false,
        type: 'POST',
        cache: false,
        success: function (result) {
            isPlanningError=verifyRouteDetails(result, Vreturnleg);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error(errorThrown);
            location.reload();
        },
        complete: function () {
        }
    });
    return isPlanningError;
}

$(document).ready(function () {
    SelectMenu(2);
    $("#btnShowRouteTab").css("display", "none");
    var myWindow;
    var result1, VVehicleID = 0;
    var element_pos = 0;    // POSITION OF THE NEWLY CREATED ELEMENTS.
    var iCnt = 0;
    $('#dyntitleConfig').hide();
    if ($('#hf_User_id').val() != '') {
        $("#OrganisationUserId").val($('#hf_User_id').val());
    }
    else {
        $("#OrganisationUserId").val($('#hUserId').val());
    }
    $('#hdnScrutinyUser').val(OrganisationUserId.options[OrganisationUserId.selectedIndex].text);
    ShowGeneralDetails(1);

    if (window.history.forward(1) != null)
        window.history.forward(1);

    //-------Below click events for tab click feature , copied from CustomContraol.cs file
    $('body').on('click', '.btn-showgeneraldetails', function (e) { e.preventDefault(); ShowGeneralDetails(); });
    $('body').on('click', '.btn-plannnenotification', function (e) { e.preventDefault(); PlannNENotification(1); });
    $('body').on('click', '.btn-view_hualierdetails', function (e) { e.preventDefault(); view_hualierdetails_nen(); });
    $('body').on('click', '#btn_confirmRnV', function (e) { e.preventDefault(); ConfirmRouteAndVehicle(); });
    $('body').on('click', '#btn_NE_AcceptRejectMovement', function (e) { e.preventDefault(); AcceptRejectMovement(); });
    $('body').on('click', '#btn_NE_AssignUser', function (e) { e.preventDefault(); AssignUserNE(); });

    $('body').on('click', '.btn-ne-display-list', function (e) {
        displayList();
    });
    $('body').on('click', '.btn-ne-bind-route-parts', function (e) {
        BindRouteParts();
    });

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
    var VRoutePartID = $('#hNENRoute_Id').val();
    var VehicleID = $('#hdnNENVehicleID').val();
    $('#pageheader').html('');
    $('#pageheader').html('<h3> NENotification - General</h3>');
    $.ajax({
        type: "GET",
        url: "../NENNotification/Display_NENotification",
        data: { NEN_ID: HdnNENID, Route_Id: VRoutePartID, Notif_Id: $('#hf_NotificationID').val(), Inbox_ItemID: HdnInboxID, NENVehicleID: VehicleID },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#tab_1').find('#div_general').html('');
            $('#tab_1').find('#div_general').html(result);
            var hf_HauliEmail = $('#hf_HauliEmail').val();
            $("#HaulierEmailAddress").val(hf_HauliEmail);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error(errorThrown);
            location.reload();
        },
        complete: function () {
            NENViewConfiguration(VehicleID);
        }
    });
}

$('body').on('click', '#verifyRoute', function (e) {
    e.preventDefault();
    $("#generalTab").removeClass('active-card');
    $("#routeTab").addClass('active-card');
    PlannNENotification(1);
});

// Display automatic route planning details
function PlannNENotification(parm) {
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
        data: { NEN_ID: VNEN_Id, IsNEN: true, nenInboxId: $('#hdnNENInboxID').val() },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            CheckSessionTimeOut();
            $('#tab_3').show();
            $('#RoutePart').html('');
            $('#RoutePart').html(data);
            $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
            ListImportedRouteFromLibraryInit();
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
            var isPlanningError = false;
            var Vreturnleg = false;
            var VRouteId = $("#hNENRoute_Id").val();
            var VReturnRouteId = $("#hreturnLeg_routeID").val();
            var RouteCount = $("#totalRouteCount").val();

            var RouteList = [];

            for (var i = 0; i < RouteCount; i++) {
                if (i == 0) {
                    RouteList.push({ RouteID: VRouteId, IsReturnRoute: false });
                }
                else {
                    if ($("#hIsReturnLeg").val() == "true" || $("#hIsReturnLeg").val() == "True")
                        Vreturnleg = true;
                    RouteList.push({ RouteID: VReturnRouteId, IsReturnRoute: Vreturnleg });                    
                }
            }
            NenRoutePlanning(RouteList);
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

function EditPlannedRoute(RouteId, RN) {
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
            else { IsReturnPlanned = true; }
        }
        else {
            ActionForReturnRout = 0;
            if ($(rowID1).html() == "Unplanned" || $(rowID1).html() == "Planning error") { IsReturnPlanned = false; }
            else { IsReturnPlanned = true; }
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
        var routeFlag = $('#hf_sessionRouteFlag').val();
        var contentRefNo = $('#nenContentRefNo').val();

        $('#RoutePart').show();
        $('#RoutePart').html('');
        if (returnRouteError == true && mainRouteCheck == false) {
            getReturnRouteNorthingEsting(page, RouteName, VRouteId, showretleg);
        }
        else {
            $.ajax({
                url: '../Routes/LibraryRoutePartDetails',
                data: { RouteFlag: page, ApplicationRevId: contentRefNo, plannedRouteName: RouteName, PlannedRouteId: VRouteId, PageFlag: "U", ShowReturnLeg: showretleg, IsNEN: true },
                beforeSend: function () {
                    startAnimation();
                },
                type: 'GET',
                cache: false,
                success: function (page) {
                    $('#tab_3').show();
                    $('#RoutePart').html('');
                    $('#RoutePart').append($(page).find('#CreateRoute').html());
                    LibraryRoutePartDetailsInit();
                    CheckSessionTimeOut();
                    Map_size_fit();
                    if ($('#hf_sessionRouteFlag').val() == 3) {
                        $('#RoutePart').append('<div class="row mt-4" style="float: right; ">' + '<div class= ' + "mt-2 pl-0 ml-0" + '>' + '<button title="Go back to the route list" class="btn outline- btn-primary SOAButtonHelper ml2 mb-2 btn-ne-display-list btn-RouteBackClass" aria-hidden="true" type="button">BACK</button>' + '</div >' + '</div >');
                    }
                    else {
                        $('#RoutePart').append('<button id="back_btn" class="btn_reg_back next btngrad btnrds btnbdr btn-ne-bind-route-parts" type="button" data-icon="" aria-hidden="true">Back</button>');
                    }
                    $("#mapTitle").html('');
                    addscroll();

                },
                error: function (xhr, textStatus, errorThrown) {
                    console.error(errorThrown);
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
    var VehicleId = $(this).data('vehicleid');
    NENViewConfiguration(VehicleId);
});
$('body').on('click', '#span2-NENViewConfiguration', function (e) {
    e.preventDefault();
    var VehicleId = $(this).data('vehicleid');
    NENViewConfiguration(VehicleId);
});
function NENViewConfiguration(id) {
    if (id != 0) {
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
                console.error(errorThrown);
                location.reload();
            },
            complete: function () {
                $("#overlay").hide();
                $('.loading').hide();
                stopAnimation();
            }
        });
    }
}

//Assign User For Scrutiny
function AssignUserNE() {

    getValue = $('#OrganisationUserId').val();
    var FromUserDet = "--Select User--";
    if ($('#hdnScrutinyUser').val() != "--Select user--") {
        FromUserDet = $('#hdnScrutinyUser').val();
    }
    if ($('#OrganisationUserId').val() != "" && $('#OrganisationUserId').val() != undefined) {

        if ($('#hdnScrutinyUser').val() != OrganisationUserId.options[OrganisationUserId.selectedIndex].text) {
            var encryptNotificationId = encryptNotifId;
            var paramList = {
                InboxId: encryptInboxId,
                UserId: getValue,
                NotificationId: encryptNotificationId,
                NenId: $('#hNEN_Id').val(),
                NenProcess: 1,
                UserName: OrganisationUserId.options[OrganisationUserId.selectedIndex].text,
                FromUserName: FromUserDet,
                ESDALReference: encryptEsdalRef
            }
            $.ajax({
                type: "POST",
                url: '../NENNotification/NENAssignUserForScrutiny',
                dataType: "json",
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
           
            ShowInfoPopup('Already assigned to user.', 'WarningCancelBtn');
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
var Rt_Id_main = $('#hNENRoute_Id').val();
var Rt_Id_return = $('#hreturnLeg_routeID').val();
function ConfirmRouteAndVehicle() {
    //Check oute status in Route_assessment table
    getValue = $('#OrgUserId').val() ? $('#OrgUserId').val() : 0;
    var paramList = {
        InboxId: encryptInboxId,
        USER_ID: getValue,
        NEN_ID: $('#hNEN_Id').val()
    }
    Rt_Id_main = $('#hNENRoute_Id').val();
    Rt_Id_return = $('#hreturnLeg_routeID').val();

    $.ajax({
        type: "POST",
        url: '../NENNotification/GetBothRouteAssessmentStatus',//Please comment this for old changes
        dataType: "json",
        data: JSON.stringify(paramList),
        processdata: true,
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            //New changes done on 17-11-2017 (If any problem occurs please comment new changes and uncomment old changes)
            //--------------NEW Changes----------------
            var msg = "";
            var dataCollection = result;
            
            if (dataCollection.result.length > 0) {
                if (dataCollection.result.length > 1) {// Checking Main route status and return route status
                    if (dataCollection.result[0].RouteStatus == 911001 || dataCollection.result[1].RouteStatus == 911001) {//Unplanned
                        msg = 'For ESDAL to assess the movement you need to plan all routes - please click on the Route tab';
                        ShowInfoPopup(msg, 'WarningCancelBtn');
                    }
                    else if ((dataCollection.result[0].RouteStatus == 911002 && dataCollection.result[1].RouteStatus == 911002) ||
                        (dataCollection.result[0].RouteStatus == 911010 && dataCollection.result[1].RouteStatus == 911010) ||
                        (dataCollection.result[0].Route_Status == 911011 && dataCollection.result[1].RouteStatus == 911011)) {//Planned & Replanned with scrutiny
                        //implimentation for showing broken routes after migration
                        CheckIsBroken({ RoutePartId: Rt_Id_main }, function (response) {
                            BrokenNenRouteReplan(response, Rt_Id_return > 0 ? Rt_Id_return : 0);
                        });
                    }
                    else if ((dataCollection.result[0].RouteStatus == 911002 && dataCollection.result[1].RouteStatus == 911004) || (dataCollection.result[0].RouteStatus == 911004 && dataCollection.result[1].RouteStatus == 911002)) {//Planned and Replanned
                        //implimentation for showing broken routes after migration
                        CheckIsBroken({ RoutePartId: Rt_Id_main }, function (response) {
                            BrokenNenRouteReplan(response, Rt_Id_return > 0 ? Rt_Id_return : 0);
                        });
                    }
                    else if ((dataCollection.result[0].RouteStatus == 911003 || dataCollection.result[1].RouteStatus == 911003)) {//Planning error
                        msg = 'For ESDAL to assess the movement you need to plan all routes - please click on the Route tab';
                        ShowInfoPopup(msg, 'WarningCancelBtn');
                    }
                    else if ((dataCollection.result[0].RouteStatus == 911004 && dataCollection.result[1].RouteStatus == 911004)) {//Replanned
                        //implimentation for showing broken routes after migration
                        CheckIsBroken({ RoutePartId: Rt_Id_main }, function (response) {
                            BrokenNenRouteReplan(response, Rt_Id_return > 0 ? Rt_Id_return : 0);
                        });
                    }
                    else if ((dataCollection.result[0].RouteStatus == 911005 && dataCollection.result[1].RouteStatus == 911005)) {//Under scrutiny with unplanned
                        var username = $("#OrgUserId option:selected").text() != "--Select user--" ? $("#OrgUserId option:selected").text() : "";
                        msg = 'For ESDAL to assess the movement you need to plan all routes - please click on the Route tab';
                        ShowInfoPopup(msg, 'WarningCancelBtn');
                    }
                }
                else {//Checking only Main route status
                    if (dataCollection.result[0].RouteStatus == 911001) {//Unplanned
                        msg = 'For ESDAL to assess this movement you need to plan the route - please click on the Route tab';
                        ShowInfoPopup(msg, 'WarningCancelBtn');
                    }
                    else if (dataCollection.result[0].RouteStatus == 911002 || dataCollection.result[0].RouteStatus == 911010 || dataCollection.result[0].RouteStatus == 911011) {//Planned & Replanned
                        //implimentation for showing broken routes after migration
                        CheckIsBroken({ RoutePartId: Rt_Id_main }, function (response) {
                            BrokenNenRouteReplan(response);
                        });
                    }
                    else if (dataCollection.result[0].RouteStatus == 911003) {//Planning error
                        msg = 'For ESDAL to assess this movement you need to plan the route - please click on the Route tab';
                        ShowInfoPopup(msg, 'WarningCancelBtn');
                    }
                    else if (dataCollection.result[0].RouteStatus == 911004) {//Replanned
                        //implimentation for showing broken routes after migration
                        CheckIsBroken({ RoutePartId: Rt_Id_main }, function (response) {
                            BrokenNenRouteReplan(response);
                        });
                    }
                    else if (dataCollection.result[0].RouteStatus == 911005) {//Under scrutiny with unplanned
                        var username = $("#OrgUserId option:selected").text() != "--Select user--" ? $("#OrgUserId option:selected").text() : "";
                        msg = 'For ESDAL to assess this movement you need to plan the route - please click on the Route tab';
                        ShowInfoPopup(msg, 'WarningCancelBtn');
                    }
                }
            }
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
var brokenRouteCount;
var isReplanCount;
var specialManeourCount;
function BrokenNenCountInit() {
    brokenRouteCount = 0;
    isReplanCount = 0;
    specialManeourCount=0;
}

function BrokenNenRouteReplan(response, retunRouteId = 0) {
    BrokenNenCountInit();
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        brokenRouteCount++;
        specialManeourCount += response.specialManouer;
        if (res.IsReplan > 1) {
            isReplanCount++;
        }
    }
    if (retunRouteId > 0) {
        CheckIsBroken({ RoutePartId: retunRouteId }, function (returnBrokenResponse) {
            BrokenNenReturnRouteReplan(returnBrokenResponse);
        });
    }
    else {
        BrokenNenRouteMessage();
    }
}
function BrokenNenReturnRouteReplan(response) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        brokenRouteCount++;
        specialManeourCount += response.specialManouer;
        if (res.IsReplan > 1) {
            isReplanCount++;
        }
    }

    BrokenNenRouteMessage();
}
function BrokenNenRouteMessage() {
    var msg = "";
    if (brokenRouteCount > 0) { //check in the existing route is broken
        msg = isReplanCount <= 1 && specialManeourCount<1 ? BROKEN_ROUTE_MESSAGES.NEN_IS_REPLAN_POSSIBLE : BROKEN_ROUTE_MESSAGES.NEN_IS_REPLAN_NOT_POSSIBLE;
        ShowWarningPopupMapupgarde(msg, function () {
            $('#WarningPopup').modal('hide');
        });
    }
    else {
        msg = 'By confirming the notification details you will no longer be able to edit the route. Select ‘yes’ to proceed and ‘no’ to return.';
        ShowWarningPopup(msg, 'RedirectToAuthorisedMovement', 'WarningCancelBtn');
    }
}
function RedirectToAuthorisedMovement() {
    var encryptNotificationId = encryptNotifId;
    var Inbox_Id =encryptInboxId;
    var EsdalRefNo = encryptEsdalRef;
    var NENroute = $('#hdnNENroute').val();
    var NENInboxID = $('#hdnNENInboxID').val();
    var NENEsdal_Ref = $('#hdnNENEsdal_ref').val();
    var NENNotifID = $('#hdnNENNotifID').val();
    //update inbox_asssessment table
    getValue = $('#OrgUserId').val();
    var VRoutePartID = $('#hNENRoute_Id').val();
    CloseWarningPopup();
    var paramList = {
        InboxId: Inbox_Id,
        USER_ID: getValue,
        Route_ID: VRoutePartID,
        routeStatus: 911006
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
        url: '../NENNotification/NENRouteAnalysis',
        dataType: "json",
        data: JSON.stringify(analysisParamList),
        processdata: true,
        beforeSend: function () {
            startAnimation('Performing route analysis');
        },
        success: function (result) {
            var RAPerformed = result;
            if (RAPerformed.result == true) {
                $.ajax({
                    type: "POST",
                    url: '../NENNotification/UpdateRouteAssessmentStatus',
                    dataType: "json",
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
                        //stopAnimation();
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
}

function NEN_RouteAnalysis() {
    var Inbox_ID = $('#hdnNENInboxID').val();
    var Notif_ID = $('#hdnNENNotifID').val();
    $.ajax({
        type: "POST",
        url: '../NENNotification/NENRouteAnalysis',
        dataType: "json",
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
}

function AcceptRejectMovement() {
    var paramList = {
        InboxId: encryptInboxId,
        USER_ID: $('#hUserId').val(),
        NEN_ID: $('#hNEN_Id').val()
    }
    $.ajax({
        type: "POST",
        url: '../NENNotification/GetRouteAssessmentStatus',
        dataType: "json",
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
    var loggedInContactId = $('#hdnContactId').val() != 0 ? $('#hdnContactId').val() : 0;
    var loggedInOrgId = $('#hdnOrgId').val();
    var IS_MOST_RECENT = $('#hdnIsMostRecent').val();
    var NextNoLongerAffected = $('#hf_NextNoLongerAffected').val();
    var Email = $('#hf_NENEmail').val();
    var esdalRef = $('#hf_EsdalRefNum').val().replace("#", "~");
    var notifid = $('#hf_NotificationID').val();
    var doc_id = $('#hf_DOCUMENT_ID').val();
    var inbox_id = $('#hf_inboxId').val();
    var rt_type = hf_routeType.replace(" ", "");
    var nenId = $('#hNEN_Id').val();
    startAnimation();
    $("#popupDialogue").load("../Movements/ViewCollaborationStatusAndNotes?Notificationid=" + notifid + "&documentid=" + doc_id + "&inBoxId=" + inbox_id + "&email=" + encodeURIComponent(Email) + "&esdalRef=" + encodeURIComponent(esdalRef) + "&contactId=" + loggedInContactId + "&route=" + encodeURIComponent(rt_type) + "&IS_MOST_RECENT=" + IS_MOST_RECENT + "&routeOriginal=" + encodeURIComponent(rt_type) + "&NextNoLongerAffected=" + NextNoLongerAffected + "&WipNENCollab=1&analysisId=0" +"&NEN_ID="+nenId, function () {
        stopAnimation();
        $('#viewCollabModal').modal({ keyboard: false, backdrop: 'static' });
        $("#viewCollabModal").modal('show');
        $("#overlay").css("display", "block");
        $("#popupDialogue").css("display", "block");
        ViewCollaborationStatusAndNotesInit();
    });

}

function view_hualierdetails_nen() {
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
        data: { inboxId: HdnInboxID, NenID: HdnNENID },
        datatype: "json",
        beforeSend: function () {
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
