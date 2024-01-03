var returnLegNotif = false;
var rt = 0, MoreHight = 311;
var routePart1, resultroutePart;
var status = 0, Pcount = 0;
var VObj_err, Vobj_NonError;
var position = 0, onWay = 0;
var Wayposition = -1, routeId_forChStatua = 0;
var MainRouteErrorMsg = "", hei = 0;
var ActionForReturnRout = 1, VWaypointCount = 0;
var IsAutomaticPlaning = false, IsReturnPlanned = false, IsrestrictionsErrror = false;
var mainRouteLegalRestriction = false;
var IsReturnRestriction = false, differentQASResult = false;
var mainRouteCheck = false, returnRouteError = false;//  Two flags added to check whether return route is unplanned or not when main route is planned

//set return leg
$('#RetJournyNotif').click(function () {
    if ($("#RetJournyNotif").is(":checked")) {
        returnLegNotif = true;
        if ($('#AddingRouteBy').val() == 3) {
            setPageType('NOMAPDISPLAY');
        }
    }
    else {
        returnLegNotif = false;
    }
});
function notifClearRoute() {
    objifxStpMap.clearRoutePart();
}

//saving automatic planned route
function automaticPlanRoute(routePart) {
    startAnimation();
    IsAutomaticPlaning = true;
    var VsearchKeyword, VEasting, VNorthing;
    var VponitType;     // pointType = 0 Start, 1 End, 2 Waypoint, 3 Viapoint, 4 Merge, 5 Diverge, 6 Manoeuvre
    loadmap('NOMAPDISPLAY');
    $(".slidingpanelstructures").removeClass("show").addClass("hide");
    Pcount = routePart.length;
    var i = 0, Loopresult = 0;
    processData(routePart, i);
}
function asyncFunction(VsearchKeyword, VEasting, VNorthing, VponitType, i, callback) {
    startAnimation();

    var Pont = i + 1;
    if (VponitType == 0)
        position = 0;
    else if (VponitType == 1)
        position = 1;
    else if (VponitType == 2) {

        position = Wayposition;
    }
    setRoutePoint(VsearchKeyword, position, VEasting, VNorthing, VponitType, function (result) {
        startAnimation();
        //ShowInfoPopup("Automatic route planning successfully done.");
        if (i < Pcount && Pont < Pcount) {
            return callback()
        }
        else  {
            if (result == true && i == (Pcount - 1)) {
                if (MainRouteErrorMsg == "") {
                    if ($("#hIsReturnLeg").val() == "True")
                        returnLegNotif = true;
                    planRoute(returnLegNotif, function (result) {
                        if (result == true || (result == false && mainRouteLegalRestriction == false && IsReturnRestriction == true)) {// here if the result is true there is no legal 
                            var returnRoutePart = null;
                            ////for main route
                            routePart = getRouteDetails();
                            if (returnLegNotif == true && IsReturnRestriction == false) {
                                returnRoutePart = getRouteDetails(true);
                            }
                            var rtId = $('#hNENRoute_Id').val();
                            var retnRtId = 0;
                            var routeDesc = $('#hMainRouteDesc').val();
                            routePart.routePartDetails.routeType = "planned";
                            routePart.routePartDetails.routeName = "NEN Route";
                            routePart.routePartDetails.routeDescr = routeDesc;
                            $('#RouteName1').html('NEN Route 1');             //name assigned to span for view
                            saveNotifRoutes(routePart, rtId, retnRtId, returnLegNotif, returnRoutePart);
                        }
                    });
                }
                else {
                    returnLegNotif = true;
                    planRoute(returnLegNotif, function (result) {
                        if (result == true) {
                            var returnRoutePart = null;
                            ////for return route                           
                            var retnRtId = $("#hreturnLeg_routeID").val();
                            var mainRouteId = $("#hNENRoute_Id").val();
                            var returnRoutePart = getRouteDetails(true);
                            if ($('#PortalType').val() == '696008') {
                                try {
                                    if ($("#chkdockcaution").is(':checked')) {
                                        returnRoutePart.routePartDetails.Dockcaution = "True";
                                    }
                                }
                                catch (err) { }
                            }
                            returnRoutePart.routePartDetails.routeType = "planned";
                            returnRoutePart.routePartDetails.routeName = "NEN Return Route";
                            returnRoutePart.routePartDetails.routeDescr = $('#hReturnRouteDesc').val();
                            // Compress the data and convert to base64 for transmission
                            var compressedRoutePart = pako.gzip(JSON.stringify({ RoutePart: returnRoutePart }));
                            var base64RoutePartData = btoa(String.fromCharCode.apply(null, compressedRoutePart));
                            $.ajax({
                                url: '/Routes/SaveCompressedRoute',
                                type: 'POST',
                                async: false,
                                //contentType: 'application/json; charset=utf-8',
                                data: JSON.stringify({ compressedRoutePart: base64RoutePartData, PlannedRouteId: retnRtId, RouteFlag: 3, isSimple: true, IsNEN: true, IsAutoPlan: true }),
                                beforesend: function () {
                                    IsAutomaticPlaning = false;
                                    startAnimation();
                                },
                                success: function (val) {
                                    var Retrt = val.value;
                                    if (Retrt > 0) {
                                        $("#hNENRoute_Id").val(val.VNENMainRouteID);
                                        $("#hreturnLeg_routeID").val(val.value);
                                        routeId_forChStatua = retnRtId;
                                        ChangeRouteStatus();
                                        SetRouteStatus_PlanningError(mainRouteId); //setting main route with planning error
                                        $("#status_" + retnRtId).text("Planned");
                                    }
                                    else {
                                        MainRouteErrorMsg = MainRouteErrorMsg + "<br/> Return leg is not saved properly.";
                                        $('.POP-dialogue111').css({ 'width': '503px' });
                                        $('.pop-message').css({ 'margin-top': '30px' });
                                        routeId_forChStatua = retnRtId;
                                        SetRouteStatus_PlanningError(retnRtId); // setting error for return leg
                                        hei = hei + 5;
                                        hei = hei + "px";
                                        $('.popup111').css({ 'height': hei });
                                        MainRouteErrorMsg = MainRouteErrorMsg + "</br></br> <button type='button' id='IdOk' style='margin-left: 43px!important;' class='create btngrad btnrds btnbdr pointerCursor nen-route-plan-close-pop-dialog' aria-hidden='true'>OK</button>";
                                       
                                        showWarningPopDialogBig( MainRouteErrorMsg.replace("***", " "), 'Ok', '', 'ClosePopDialogBig', '', 1, 'info');
                                        $('.footer111').hide();
                                        $('.box_warningBtn1').hide();
                                        stopAnimation();
                                        return;
                                    }
                                },
                                complete: function () {
                                        MainRouteErrorMsg = MainRouteErrorMsg + "<br/> Return leg planned successfully.";
                                    $('.POP-dialogue111').css({ 'width': '503px' });
                                    $('.pop-message').css({ 'margin-top': '30px' });
                                    routeId_forChStatua = $("#hNENRoute_Id").val(); //already the error is updated along with the return route planned status
                                    hei = hei + 5;
                                    hei = hei + "px";
                                    $('.popup111').css({ 'height': hei });
                                    MainRouteErrorMsg = MainRouteErrorMsg + "</br></br> <button type='button' id='IdOk' style='margin-left: 43px !important;' class='create btngrad btnrds btnbdr pointerCursor nen-route-plan-close-pop-dialog-for-return' aria-hidden='true'>OK</button>";
                                   
                                    showWarningPopDialogBig( MainRouteErrorMsg.replace("***", " "), 'Ok', '', 'ClosePopDialogBig', '', 1, 'info');
                                    $('.footer111').hide();
                                    $('.box_warningBtn1').hide();
                                    stopAnimation();
                                }
                            });
                        }
                        else {// If return route has legal restriction error
                            var restrictionMsg = "</br>" + (VObj_err.length + 1) + ". Due to legal restrictions on the return route.";
                            restrictionMsg =  MainRouteErrorMsg.replace("***", restrictionMsg);
                            $('.POP-dialogue111').css({ 'width': '503px' });
                            $('.pop-message').css({ 'margin-top': '30px' });
                            routeId_forChStatua = $("#hreturnLeg_routeID").val();
                            SetRouteStatus_PlanningError($("#hreturnLeg_routeID").val()); // setting error for return leg
                            hei = hei + 21;
                            hei = hei + "px";
                            $('.popup111').css({ 'height': hei });
                            restrictionMsg = restrictionMsg + "</br></br> <button type='button' id='IdOk' style='margin-left: 43px !important;' class='create btngrad btnrds btnbdr pointerCursor nen-route-plan-close-pop-dialog' aria-hidden='true'>OK</button>";
                            showWarningPopDialogBig(restrictionMsg, 'Ok', '', 'ClosePopDialogBig', '', 1, 'info');
                            $('.footer111').hide();
                            $('.box_warningBtn1').hide();
                            stopAnimation();
                            return;
                        }
                    });
                }
            }
        }
    });
}



function processData(routePart, i) {
    
    IsAutomaticPlaning = true;
    if (i < routePart.length) {
        VsearchKeyword = routePart[i].pointDescr;
        VponitType = routePart[i].pointType;
        VEasting = routePart[i].Easting;
        VNorthing = routePart[i].Northing;
        if (VponitType == 239003)
            VponitType = 2;
        else if (VponitType == 0)
            VponitType = 3;
        else if (VponitType == 239001)
            VponitType = 0;
        else if (VponitType == 239002)
            VponitType = 1;
        asyncFunction(VsearchKeyword, VEasting, VNorthing, VponitType, i, function () {
            i = i + 1;
            processData(routePart, i++)
        });
    }
    else {
        ShowInfoPopup('Automatic route planned successfully');
    }
}
function getReturnRouteNorthingEsting(page,RouteName, VRouteId, showretleg) {
    var url = "../NENNotification/getReturnRouteNorthingEsting";
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        data: { RouteID : VRouteId},
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            Vobj_NonError = result.Obj_NONerr;
        },
        error: function () {
        },
        complete: function () {
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
                    LibraryRoutePartDetailsInit();
                    CheckSessionTimeOut();
                    Map_size_fit();
                    if ($('#hf_sessionRouteFlag').val()== '3') {
                        $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr nen-route-plan-clone-route-notify" type="button" data-icon="" aria-hidden="true">Back</button>');
                    }
                    else {
                        $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr nen-route-plan-bind-route-parts" type="button" data-icon="" aria-hidden="true">Back</button>');
                    }
                    $("#mapTitle").html('');
                    addscroll();

                },
                error: function (xhr, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {                    
                }
            });
            // stopAnimation();

        }
    });
}
function verifyRouteDetails(routePart, Vreturnleg = false) {
    
    var RESULT1 = "", RESULT2, IS_returnLeg = false;
    hei = 0;
    differentQASResult = false;
    IsrestrictionsErrror = false;
    IsReturnRestriction = false;
    mainRouteLegalRestriction = false;
    var isPlanningError = false;
    if (routePart.result.length > 0) {
        var url = "../NENNotification/GetAddressNEN";
        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            data: { Isretunleg: Vreturnleg },
            beforeSend: function () {
                //startAnimation();
                MainRouteErrorMsg = "";
            },
            success: function (result) {
                RESULT1 = result.result;
                RESULT2 = result.Obj_err;
                Vobj_NonError = result.Obj_NONerr;
                IS_returnLeg = result.IsreturnRoutePart;
                resultroutePart = result.resultroutePart;
                VWaypointCount = result.WaypointCount;
                hei = result.errorHigh;
                for (i = 0; i < Vobj_NonError.length; i++) {       // loop added for  result is contain atleast one mis-matched result
                    if (Vobj_NonError[i].differentQASSearchFlag == true) {
                        differentQASResult = true;
                    }
                }
            },
            error: function () {
                stopAnimation();
            },
            complete: function () {
                
                if (RESULT1 == "find") {
                    isPlanningError = false;
                    automaticPlanRoute(resultroutePart);
                }
                else {
                    isPlanningError = true;
                    if (IS_returnLeg == true) {
                        MainRouteErrorMsg = RESULT1;
                        $("#err_list").val(RESULT2);
                        VObj_err = RESULT2;
                        automaticPlanRoute(resultroutePart);
                    }
                    else {
                        MainRouteErrorMsg = "";
                        $("#err_list").val(RESULT2);
                        VObj_err = RESULT2;
                        routeId_forChStatua = $("#hNENRoute_Id").val();
                        SetRouteStatus_PlanningError($("#hNENRoute_Id").val()); // setting error for main route
                        ShowErrorPopup(RESULT1.replace("***", " "));//error warning
                        $('#RouteLibrary').attr('checked', false);
                    }
                }
            }
        });
    }
    else {
        ShowErrorPopup('Route details are not found. Please manually plan the route before proceeding.');
        //showWarningPopDialog('Route details are not found. Please manually plan the route before proceeding.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        $('#RouteLibrary').attr('checked', false);
        stopAnimation();
    }
    return isPlanningError;
}
function ClosePopDialogBig1() {
    $('.box_warningBtn1').show();
    ClosePopDialogBig();
    $('.footer111').show();
    $('.box_warningBtn1').show();
}
function ClosePopDialogBig_forReturnLeg() {
    $('.box_warningBtn1').show();
    PlannNENotification(0);//function will check and update the list if another user has parallely edited the route if input param is 0
    ClosePopDialogBig();
    $('.footer111').show();
    $('.box_warningBtn1').show();
}

function ClosePopDialogBig() {
    $('.pop-message').css({ 'margin-top': '45px' });
    //$('.POP-dialogue111').css({ 'width': '475px' });
    $('.popup111').css({ 'height': '186px' });
    //SetRouteStatus_PlanningError(); //not used call can be commented
    WarningCancelBtn();
}
function ClosePopDialogBigRouteNen() {
    $('#div_WarningMessageDialogRoute').html('');
    $('#pop-warning-nen-route').modal('hide');
    stopAnimation();
}

function saveNotifRoutes(routePart, rtId, retnRtId, returnLegNotif, returnRoutePart) {
    
    routePart.routePartDetails.routeDescr = $('#hMainRouteDesc').val();
    if ($('#PortalType').val() == '696008') {
        try {
            if ($("#chkdockcaution").is(':checked')) {
                routePart.routePartDetails.Dockcaution = "True";
            }
        }
        catch (err) { }
    }
    // Compress the data and convert to base64 for transmission
    var compressedRoutePart = pako.gzip(JSON.stringify({ RoutePart: routePart }));
    var base64RoutePartData = btoa(String.fromCharCode.apply(null, compressedRoutePart));
    $.ajax({
        url: '/Routes/SaveCompressedRoute',
        type: 'POST',
        async: true,
        //contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ compressedRoutePart: base64RoutePartData, PlannedRouteId: rtId, RouteFlag: 3, isSimple: true, IsNEN: true, IsAutoPlan: true }),
        beforeSend: function () {
            IsAutomaticPlaning = false;
            LodingText_Change("Automatic route planning successfully done. Route details are saving...");
            startAnimation();
        },
        success: function (val) {
            rt = val.value;
            $("#hNENRoute_Id").val(rt);
            $("#hreturnLeg_routeID").val(val.VNENRturnRouteID);
            if (rt > 0 && val.routeSaveRes == true) {
                ShowInfoPopup("Route saved successfully");
                if (returnLegNotif) {//opted for return leg
                    if (IsReturnRestriction == false) { //legal restriction in return leg.
                        routeId_forChStatua = rtId; //setting the route id to change the status.
                        ChangeRouteStatus();
                        $("#status_" + routeId_forChStatua).text("Planned");
                        var retnRtId = $("#hreturnLeg_routeID").val();
                        var returnRoutePart = getRouteDetails(true);
                        returnRoutePart.routePartDetails.routeType = "planned";
                        returnRoutePart.routePartDetails.routeName = "NEN Return Route";
                        returnRoutePart.routePartDetails.routeDescr = $('#hReturnRouteDesc').val();
                        if ($('#PortalType').val() == '696008') {
                            try {
                                if ($("#chkdockcaution").is(':checked')) {
                                    returnRoutePart.routePartDetails.Dockcaution = "True";
                                }
                            }
                            catch (err) { }
                        }
                        // Compress the data and convert to base64 for transmission
                        var compressedRoutePart = pako.gzip(JSON.stringify({ RoutePart: returnRoutePart }));
                        var base64RoutePartData = btoa(String.fromCharCode.apply(null, compressedRoutePart));
                        $.ajax({
                            url: '/Routes/SaveCompressedRoute',
                            type: 'POST',
                            async: false,
                            //contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify({ compressedRoutePart: base64RoutePartData, PlannedRouteId: retnRtId, RouteFlag: 3, isSimple: true, IsNEN: true, IsAutoPlan: true }),
                            beforesend: function () { startAnimation(); IsAutomaticPlaning = false; },
                            success: function (val) {

                                var Retrt = val.value;
                                $("#hReturnRouteDesc").val(Retrt);
                                if (Retrt > 0) {
                                    $('#NotiplannedRouteId1').val(Retrt);
                                    routeId_forChStatua = retnRtId;
                                    if (differentQASResult == false)
                                        ShowInfoPopup('The routes have been planned using the Start and End points provided by the haulier; however they may not match the haulier' + "'" + 's desired routes. Waypoints added to the outbound leg are not considered for the return leg and will need adding manually. Click on "Haulier route description" to compare and edit if necessary.', 'ChangeRouteStatus');
                                    else
                                        ShowInfoPopup('The routes have been planned by trying to match the Start and End addresses provided by the haulier; however they may not match the haulier' + "'" + 's desired routes. Waypoints added to the outbound leg are not considered for the return leg and will need adding manually. Click on "Haulier route description" to compare and edit if necessary.', 'ChangeRouteStatus');
                                    stopAnimation();

                                }
                                else {
                                    ShowInfoPopup('Return leg is not saved properly');
                                    $('#RetJournyNotif').attr('checked', false);
                                    stopAnimation();
                                    return;
                                }
                            },
                            complete: function () {

                            }
                        });
                    }// not restriction
                    else {
                        var errorMsg;
                        routeId_forChStatua = rtId;
                        if (differentQASResult == false) {
                            errorMsg = 'The route has been planned using the Start and End points provided by the haulier; however it may not match the haulier' + "'" + 's desired route. Waypoints added to the outbound leg are not considered for the return leg and will need adding manually. Click on "Haulier route description" to compare and edit if necessary. </br><br>The return route could not be planned due to legal restrictions. Please change the start/end and plan the route.';
                            errorMsg = errorMsg + "</br></br> <button type='button' id='IdOk' style='margin-left: 43px !important;' class='create btngrad btnrds btnbdr pointerCursor nen-route-plan-close-restriction-popup' aria-hidden='true'>OK</button>";
                            $('.popup111').css({ 'height': '226px' });
                            ShowInfoPopup(errorMsg.replace("***", " "), 'ChangeRouteStatus');//error warning
                            $('.footer111').hide();
                            $('.box_warningBtn1').hide();
                        }   
                        else {
                            $('.popup111').css({ 'height': '226px' });
                            errorMsg = 'The route has been planned by trying to match the Start and End addresses provided by the haulier; however it may not match the haulier' + "'" + 's desired route. Click on "Haulier route description" to compare and edit if necessary. </br><br>The return route could not be planned due to legal restrictions. Please change the start/end and plan the route.';
                            errorMsg = errorMsg + "</br></br> <button type='button' id='IdOk' style='margin-left: 43px !important;' class='create btngrad btnrds btnbdr pointerCursor nen-route-plan-close-restriction-popup' aria-hidden='true'>OK</button>";
                            ShowInfoPopup(errorMsg.replace("***", " "), 'ChangeRouteStatus');
                            $('.footer111').hide();
                            $('.box_warningBtn1').hide();
                        }
                    }
                }//retun leg if
                else {
                    routeId_forChStatua = rtId;
                    if (differentQASResult == false)
                        ShowInfoPopup('The route has been planned using the Start and End points provided by the haulier; however it may not match the haulier' + "'" + 's desired route. Click on "Haulier route description" to compare and edit if necessary.', 'ChangeRouteStatus');
                    else
                        ShowInfoPopup('The route has been planned by trying to match the Start and End addresses provided by the haulier; however it may not match the haulier' + "'" + 's desired route. Click on "Haulier route description" to compare and edit if necessary.', 'ChangeRouteStatus');

                    stopAnimation();
                }
            }
            else {
                ShowInfoPopup('Route planning failed please edit route and correct before proceeding');
                $('#RouteLibrary').attr('checked', false);
                stopAnimation();
                return;
            }
        },
        complete: function () {
            stopAnimation();
        }
    });
}
//set status as planned
function ChangeRouteStatus() {
    WarningCancelBtn();
    var VUserId = $("#hUserId").val(), VInboxId = $("#hinboxId").val(), VRoute_ID =$("#hNENRoute_Id").val();
    $.ajax({
        url: '/NENNotification/UpdateRouteAssessmentStatus',
        type: 'POST',
        async: true,
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            startAnimation("Displaying route list. Please wait...");
        },
        data: JSON.stringify({ InboxId: VInboxId, USER_ID: VUserId, Route_ID: VRoute_ID, routeStatus: 911002, IsmainRoute: IsThisMainRoute(VRoute_ID) }),
        success: function (result) {
            if ($("#hIsReturnLeg").val() == "true" || $("#hIsReturnLeg").val() == "True") {

                $.ajax({
                    url: '/NENNotification/UpdateRouteAssessmentStatus',
                    type: 'POST',
                    async: true,
                    //contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        startAnimation("Displaying route list. Please wait...");
                    },
                    data: JSON.stringify({ InboxId: VInboxId, USER_ID: VUserId, Route_ID: $("#hreturnLeg_routeID").val(), routeStatus: 911002, IsmainRoute: IsThisMainRoute(VRoute_ID) }),
                    success: function (result) {
                      
                    },
                    complete: function () {
                        //$('#status_' + VRoute_ID).text('Planned');
                        //$('#hNEN_RouteStatus').val(911002);
                        //PlannNENotification(0);
                        // stopAnimation();

                    }
                });


            }

        },
        complete: function () {
            
            $('#status_' + VRoute_ID).text('Planned');
            $('#hNEN_RouteStatus').val(911002);
            PlannNENotification(0);
            // stopAnimation();
           
        }
    });

    
    stopAnimation();
}

function NENChangeRouteStatusPopupcall() {
    CloseSuccessModalPopup();
    NENChangeRouteStatus();
}


function NENChangeRouteStatus() {
    
    var NENRouteStatus = 911004;
    if ($("#hNEN_RouteStatus").val() == "911001" || $("#hNEN_RouteStatus").val() == "911005")
        NENRouteStatus = 911002;
    if ($("#hNEN_RouteStatus").val() == "911003")
        NENRouteStatus = 911004;
    if ($("#hNEN_RouteStatus").val() == "911002" || $("#hNEN_RouteStatus").val() == "911010")
        NENRouteStatus = 911004;
    var VUserId = $("#hUserId").val(), VInboxId = $("#hinboxId").val(), VNEN_Id = $("#hNEN_Id").val();
    $.ajax({
        url: '/NENNotification/UpdateRouteAssessmentStatus',
        type: 'POST',
        async: true,
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            startAnimation();
        },
        data: JSON.stringify({ InboxId: VInboxId, USER_ID: VUserId, Route_ID: routeId_forChStatua, routeStatus: NENRouteStatus, IsmainRoute: IsThisMainRoute(routeId_forChStatua) }),
        success: function (result) {

        },
        complete: function () {
            $("#hNEN_RouteStatus").val(NENRouteStatus);
            PlannNENotification(0);
            stopAnimation();
        }
    });
}
function SetRouteStatus_PlanningError(routeId) {
    var VUserId = $("#hUserId").val(), VInboxId = $("#hinboxId").val(), VNEN_Id = $("#hNEN_Id").val();
    $.ajax({
        url: '/NENNotification/UpdateRouteAssessmentStatus',
        type: 'POST',
        async: true,
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            startAnimation();
        },
        data: JSON.stringify({ InboxId: VInboxId, USER_ID: VUserId, Route_ID: routeId, routeStatus: 911003, IsmainRoute: IsThisMainRoute(routeId) }),
        success: function (result) {
            //WarningCancelBtn();
            //stopAnimation();
        },
        complete: function () {
            $("#status_" + routeId).text("Planning error");
            $("#hNEN_RouteStatus").val("911003");
            stopAnimation();
        }
    });
}
function setPageType(type) {
    objifxStpMap.setPageType(type);
}

function setPageState(state) {
    objifxStpMap.setPageState(state);
}

function getCurrentBoundsAndZoom() {
    return objifxStpMap.getCurrentBoundsAndZoom();
}
function highlight_error() {
    var errorCount, wayPoinId = "Waypoint";
    if (VObj_err != null) {
        errorCount = VObj_err.length;
        for (var i = 0 ; i < errorCount; i++) {
            if (VObj_err[i].Point == "startPoint")
                $('#From_location').css({ 'border': '1px solid yellow', 'background-color': 'yellow','color':'black' });
            else if (VObj_err[i].Point == "endPoint")
                $("#To_location").css({ 'border': '1px solid yellow', 'background-color': 'yellow', 'color': 'black' });
            else {
                $("#Waypoint" + VObj_err[i].add_index).css({ 'border': '1px solid yellow', 'background-color': 'yellow', 'color': 'black' });
            }
        }
    }
}
function remove_highlight_error() {
    var errorCount, wayPoinId = "Waypoint";
    if (VObj_err != null) {
        errorCount = VObj_err.length;
        for (var i = 0 ; i < errorCount; i++) {
            if (VObj_err[i].Point == "startPoint") {
                $('#From_location').css({ 'border': '', 'background-color': '', 'color': 'white' });
            }
            else if (VObj_err[i].Point == "endPoint") {
                $('#To_location').css({ 'border': '', 'background-color': '', 'color': 'white' });
            }
            else {
                $('#Waypoint' + VObj_err[i].add_index).css({ 'border': '', 'background-color': '', 'color': 'white' });
            }
        }
    }
}


function SetCorrectElementOnMap(i, IsFoReturn) {
    if (i == null)
        i = 0;
    if (Vobj_NonError != null && Vobj_NonError.length > 0) {//condition to check if it doesnt have any errors
        if (Vobj_NonError[i].point != 2) {

            setRoutePoint(Vobj_NonError[i].Address, Vobj_NonError[i].Point, Vobj_NonError[i].Easting, Vobj_NonError[i].Northing, Vobj_NonError[i].Point, function () {  // pinned start and end flag on the map
                i = i + 1;
                if (i < Vobj_NonError.length) {
                    SetCorrectElementOnMap(i);
                }
            });

        }
        else {
            point = getPoint(Vobj_NonError[i].moniker, function (result) {
                var pointNo = Vobj_NonError[i].pointIndex;
                var pointType = 2;
                var str = "Waypoint";
                if (pointNo == undefined) {
                    pointNo = Number(str.substr(str.length - 1)) + 1;
                }
                else
                    pointNo = Number(pointNo);
                if (Vobj_NonError[i].moniker != null) {
                    if (ValidXY(x, result.Easting, result.Northing) == true)
                        // setRoutePoint(Vobj_NonError[i].address, Vobj_NonError[i].point, result.Easting, result.Northing, pointType, function () {
                        setRoutePoint(Vobj_NonError[i].address, Number(pointNo + 1), result.Easting, result.Northing, pointType, function () { // pinned waypoints on the map

                            i = i + 1;
                            if (i < Vobj_NonError.length) {
                                SetCorrectElementOnMap(i);
                            }
                        });
                }
                else {
                    if (ValidXY(x, Vobj_NonError[i].Esting, Vobj_NonError[i].Northing) == true) {
                        setRoutePoint(Vobj_NonError[i].address, Number(pointNo + 1), Vobj_NonError[i].Esting, Vobj_NonError[i].Northing, pointType, function () { // pinned waypoints on the map
                            i = i + 1;
                            if (i < Vobj_NonError.length) {
                                SetCorrectElementOnMap(i, IsFoReturn);
                            }
                        });
                    }
                }
            });
        }
    }

}


function ViewMoreDetails() {
    var hei = MoreHight + "px";
    $('.popup111').css({ 'height': hei });
    MoreHight = MoreHight + 20;
}

function IsThisMainRoute(ID) {
    if (ID == $("#hreturnLeg_routeID").val())
        return false;
    else return true;
}

function view_ErrorList(MSG) {
    $("#dialogue").show();
    $("#overlay").show();
    $('.loading').hide();
    startAnimation();
    var newDiv = $('<div/>', { id: 'POP-warning', class: 'POP-dialogue111' })
    var newDiv1 = $('<div/>', { id: 'DivErrorList', class: 'popup111' })
    $(newDiv).appendTo('#tblroutelist').draggable();
    // iCnt = iCnt + 1;
    //var VInboxID = 1, VNEN_id = $("#hNEN_Id").val();
    //var VInboxID = $("#hinboxId").val();
    MSG = MSG + "<br/><button class='btngrad btnrds btnbdr box_warningBtn1 nen-route-plan-close-pop-dialog' autofocus='autofocus'>OK</button>";
    $("#POP-warning").append(newDiv1);
    $("#DivErrorList").append(MSG);
}
function ClosePopupForRestrictionError() {
    $('.box_warningBtn1').show();
    $('.footer1').show();
    $('.popup1').css({ 'height': '160px' });
    ChangeRouteStatus();

}
function ShowDialogWarningPopRouteNen(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {
    ResetPopUp();

    if (btn1_txt == 'Ok') {
        btn1_txt = 'OK';
    }

    if (btn2_txt == 'Ok') {
        btn2_txt = 'OK';
    }

    $('#div_WarningMessageDialogRoute').html(message);
    if (btn1_txt == '') { $('.box_warningBtn1').hide(); } else { $('.box_warningBtn1').html(btn1_txt); }
    if (btn2_txt == '') { $('.box_warningBtn2').hide(); } else { $('.box_warningBtn2').html(btn2_txt); }
    if (btn1Action != '') {
        $("body").on('click', '#IdOk', function () {
            window[btn1Action]();
        });
    }
    if (btn2Action != '')
    {
        $("body").on('click', '#WarningSuccessDialog', function () {
            window[btn2Action]();
        });
    }
        

    switch (autofocus) {
        case 1: $('#IdOk').attr("autofocus", 'autofocus'); break;
        case 2: $('#WarningSuccessDialog').attr("autofocus", 'autofocus'); break;
        default: break;
    }
    //switch (type) {
    //    case 'error': $('#warningImage').src = ""; break;
    //    case 'info': $('.message1').addClass("info"); $('.popup1'); break;
    //    case 'warning': $('.message1').addClass("warning"); $('.popup1'); break;
    //    default: break;

    //}
    $('#pop-warning-nen-route').modal({ keyboard: false, backdrop: 'static' });
    $('#pop-warning-nen-route').modal('show');
}
$('body').on('click', '.nen-route-plan-close-pop-dialog', function (e) {
    ClosePopDialogBig1(this);
});
$('body').on('click', '.nen-route-plan-close-pop-dialog-for-return', function (e) {
    ClosePopDialogBig_forReturnLeg(this);
});
$('body').on('click', '.nen-route-plan-clone-route-notify', function (e) {
    CloneRoutesNotify(this);
});
$('body').on('click', '.nen-route-plan-bind-route-parts', function (e) {
    BindRouteParts(this);
});
$('body').on('click', '.nen-route-plan-close-restriction-popup', function (e) {
    ClosePopupForRestrictionError(this);
});

//function closecheckingErrors() {
//    close_alert();
//    return false;
//}

function NenRoutePlanning(RouteList) {

    var url = "../NENNotification/NenAutoRoutePlanning";
    $.ajax({
        url: url,
        type: 'POST',
        data: { ImportedRoutelist: RouteList },
        beforeSend: function () {
            MainRouteErrorMsg = "";
        },
        success: function (result) {
            if (result.nenRouteModelList.length > 0) {
                var data = result.nenRouteModelList;
                for (var i = 0; i < data.length; i++) {
                    RESULT1 = data[i].ErrorMoniker;
                    RESULT2 = data[i].ErrorLists;
                    Vobj_NonError = data[i].NonErrorLists;
                    IS_returnLeg = data[i].IsStartEndError;
                    resultroutePart = data[i].RoutePointJsons;
                    VWaypointCount = data[i].WaypointCount;
                    hei = data[0].ErrorHigh;
                    for (var j = 0; j < Vobj_NonError.length; j++) {       // loop added for  result is contain atleast one mis-matched result
                        if (Vobj_NonError[i].differentQASSearchFlag == true) {
                            differentQASResult = true;
                        }
                    }

                    if (RESULT1 == "find") {
                        isPlanningError = false;
                        automaticPlanRoute(resultroutePart);
                    }
                    else {
                        isPlanningError = true;
                        if (IS_returnLeg == true) {
                            MainRouteErrorMsg = RESULT1;
                            $("#err_list").val(RESULT2);
                            VObj_err = RESULT2;
                            automaticPlanRoute(resultroutePart);
                        }
                        else {
                            MainRouteErrorMsg = "";
                            $("#err_list").val(RESULT2);
                            VObj_err = RESULT2;
                            routeId_forChStatua = $("#hNENRoute_Id").val();
                            SetRouteStatus_PlanningError($("#hNENRoute_Id").val()); // setting error for main route
                        }
                        ShowErrorPopup(RESULT1.replace("***", " "));//error warning
                        $('#RouteLibrary').attr('checked', false);
                        stopAnimation();
                    }
                }
            }
            else {
                ShowErrorPopup('Route details are not found. Please manually plan the route before proceeding.');
                $('#RouteLibrary').attr('checked', false);
                stopAnimation();
            }
        },
        error: function () {
            stopAnimation();
        },
        complete: function () {
        }
    });
}