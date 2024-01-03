var tabindex;
var prevtabindex = 0;
var contactCnt = 0;
var routePart, returnRoutePart;
var _RouteID = 0, _RouteName = "";
var flag = 0;
var status;
var dock_chk;
var activePath;
var UDockCautionVal;
var UFlagVal;
var URouteNameVal;
var URouteIDVal;
var RouteFlagSessionVal;
var dock_caution;
var SuccesSave = false;
function EnterRouteDetailsInit() {
    status = $('#SortStatus').val();
    UDockCautionVal = $('#hf_UDockCaution').val();
    UFlagVal = $('#hf_UFlag').val();
    URouteNameVal = $('#hf_URouteName').val();
    URouteIDVal = $('#hf_URouteID').val();
    RouteFlagSessionVal = $('#hf_RouteFlagSession').val();
    dock_caution = $('#chk_dock_caution').val();
    $('#exampleModalCenterRoute').show();
    $('#overlay').show();
    $('#dialogue').show();
    activePath = 0;//currently active path is zero
    if (status == "CandidateRT") {
        $('#tr_SORT_candidate').show();
        dock_chk = UDockCautionVal;
        if (dock_chk == "True") {
            $('#chkdockcaution').prop('checked', true);
        }
    }

    $('#tbl_route_details').find('#div_saving').hide();
    prevIndex = 0;
    Resize_PopUp(440);
    $("#PontTy").val('Start');
    routePart = getRouteDetails();
    returnRoutePart = getRouteDetails(true);
    addtablist();
    $("#pathcounttabsId1").addClass('selectcol');
    $("#pathcounttabs").tabs();
    tabindex = 0;
    var IsNewRt = getRouteID();
    if (IsNewRt != 0) {
        if (UFlagVal == "U" && URouteNameVal != "" && URouteNameVal != null) {
            routePart.routePartDetails.routeName = URouteNameVal;
        }
        else {
            routePart.routePartDetails.routeName = InfRtName;
        }
    }
    else {
        routePart.routePartDetails.routeName = "";
    }
    if (UFlagVal == "U") {
        if (routePart.routePartDetails.routeName == "" || String(routePart.routePartDetails.routeName) == "undefined") {
            routePart.routePartDetails.routeName = URouteNameVal;
            routePart.routePartDetails.routeID = URouteIDVal;
            if (status == "CandidateRT") {
                dock_chk = UDockCautionVal;
                if (dock_chk == "True") {
                    $('#chk_dock_caution').prop('checked', true);   
                }
            }
            $("#startDesc").attr("disabled", true);
        }
        if (returnRoutePart != null && returnRoutePart != undefined && returnRoutePart.routePartDetails.routeName == "") {
            routePart.routePartDetails.routeName = URouteNameVal;
            routePart.routePartDetails.routeID = URouteIDVal;
            if (status == "CandidateRT") {
                dock_chk = UDockCautionVal;
                if (dock_chk == "True") {
                    $('#chk_dock_caution').prop('checked', true);
                }
            }
            $("#startDesc").attr("disabled", true);
        }
    }

    if (routePart.routePartDetails.routeType != 'undefined' && routePart.routePartDetails.routeType == 'outline') {

        $('.dyntitleConfig').html('Textual description route details');
        $('#tbl_route_details').find('#pathdescr').hide();
    }

    $("#Routename").val(routePart.routePartDetails.routeName);
    $("#routeDescr").val(routePart.routePartDetails.routeDescr);
    $("#pathDescr").val(routePart.routePathList[0].pathDescr);

    if (routePart.routePathList[0].routePointList.length > 2) {
        for (var i = 2; i < routePart.routePathList[0].routePointList.length; i++) {
            if (routePart.routePathList[0].routePointList[i].pointType == 2)
                $("#PontTy").append($("<option />").val(i).text("Waypoint" + (i - 1)));
            else
                //$("#PontTy").append($("<option />").val(i).text("Viapoint" + (i - 1)));     //cmnd by nk HE-8764 on 6.12.2023
                $("#PontTy").append($("<option />").val(i).text("Stopping point" + (i - 1)));//added by nk HE-8764 on 6.12.2023
        }
    }

    if (routePart.routePathList.length >= 1)
        setValues(0, 0);

    $("#startDesc").attr("disabled", false);
    if ($("#IsSimplifiedNotif").val() == "YES") {
        $("#SaveNewDIV").css("display", "none");
    }

    if ($("#hIs_NEN").val() == "true") {
        $('#spn_ViaPoint').hide();
        $("#SaveNewDIV").css("display", "none");
        $("#SaveAsNew").css("display", "none");
    }

    if ($('#chk_dock_caution').is(':checked')) {
        flag = 1;
    }
    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        $('#divSaveToRouteLibrary').show();
    }
}
$(document).ready(function () {
    $('body').on('keyup paste','#startDesc' ,function (e) {
        var tabIndex = activePath;
        var p = document.getElementById("PontTy");
        var pointIndex = p.selectedIndex;
        updateFullAddress(tabIndex, pointIndex, $("#startDesc").val());
    });
    $('body').on('keydown', '.txt-routename-valid', function (e) {
        return isNonAlphaNumforRoute(e);
    });
    $('body').on('keypress', '.txt-routename-valid', function (e) {
        return isNonAlphaNumforRoute(e);
    });
    $('body').on('click', '#IDCloseRtpop', function (e) {
        e.preventDefault();
        CloseRtpop(this);
    });
    $('body').on('click', '.div-enter-route-detail #span-Edit_help', function (e) {
        e.preventDefault();
        EditHelp_popup(this);
    });
    $('body').on('click', '.div-enter-route-detail #span-help', function (e) {
        e.preventDefault();
        help_poup(this);
    });
    $('body').on('click', '#span-close', function (e) {
        e.preventDefault();
        closeAnotation(this);
    });
    $('body').on('click', '.btn-erd-path-click', function (e) {
        var indexval = $(this).data('indexval');
        PathClicked(indexval);
    });
    $('body').on('click', '#chk_dock_caution', function (e) {
        if ($('#chk_dock_caution').is(':checked')) {
            flag = 1;
        }
        else {
            flag = 0;
        }
    });
    $('body').on('click', '#Back_RtSave', function (e) {
        $('#exampleModalCenterRoute').hide();
        $('#overlay').hide();
        $('#dialogue').hide();
    });
    $('body').on('click', '#tbl_route_details', function (e) {
        var key = e.which;
        if (key == 13) {  // the enter key code
            $('#btn_save').click();
        }
    });
    $('body').on('click', '#btn_save', function (e) {
        var isFromRouteLibrary=$('#hf_IsLibrary').val();
        var isAddToLibrary = false;
        try {
            if (IsRoutePlanned != undefined) {
                IsRoutePlanned = false;
            }
        }
        catch (err) {}
        if ($('#hf_IsPlanMovmentGlobal').length > 0 && $("#chk_saveToRouteLibrary").is(':checked')) {
            isAddToLibrary = true;
        }
        $("#RN_validatn").html("");
        $("#RP_validatn").html("");

        if ($("#Routename").val() == "") {
            $("#RN_validatn").html("The route name is required.");
            return;
        }
        var isValid = 0;
        if ($("#SaveAsNew").is(':checked')) {
            routePart.routePartDetails.SaveAsNew = true;
        }
        if ($('#hf_IsPlanMovmentGlobal').length == 0) {
            if ($("#SaveAsNew").is(':checked') || getRouteID() == 0) {
                if (RouteFlagSessionVal == 1 || RouteFlagSessionVal == 2) {
                    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                    $.ajax({
                        url: '/Routes/ValidateApplicationRouteName',
                        type: 'POST',
                        async: false,
                        //contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ ROUTE_NAME: $("#Routename").val(), REVISION_ID: +ApplicationRevId, ROUTE_FOR: 1 }),
                        success: function (result) {
                            isValid = result;
                        }
                    });
                    if (isValid == 0) {
                        $.ajax({
                            url: '/Routes/ValidateApplicationRouteName',
                            type: 'POST',
                            async: false,
                            //contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify({ ROUTE_NAME: $("#Routename").val(), REVISION_ID: +ApplicationRevId, ROUTE_FOR: 2 }),
                            success: function (result) {
                                isValid = result;
                            }
                        });
                    }
                }
                else if (RouteFlagSessionVal == 3) {
                    var CRNoId = $('#CRNo').val() ? $('#CRNo').val() : 0;
                    if (CRNoId == 0)
                        CRNoId = $('#ContentRefNo').val() ? $('#ContentRefNo').val() : 0;
                    $.ajax({
                        url: '/Routes/ValidateApplicationRouteName',
                        type: 'POST',
                        async: false,
                        //contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ ROUTE_NAME: $("#Routename").val(), CONTENT_REF_NO: +CRNoId, ROUTE_FOR: 3 }),
                        success: function (result) {
                            isValid = result;
                        }
                    });
                }
                else if (isFromRouteLibrary.toLowerCase() == "true"){
                    $.ajax({
                        url: '/Routes/CheckRouteName',
                        type: 'POST',
                        async: false,
                        //contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ RouteName: $("#Routename").val() }),
                        success: function (result) {
                            isValid = result.success;
                        }
                    });
                }
            }
        }

        if (isAddToLibrary) {
            $.ajax({
                url: '/Routes/CheckRouteName',
                type: 'POST',
                async: false,
                data: JSON.stringify({ RouteName: $("#Routename").val() }),
                success: function (result) {
                    isValid = result.success;
                }
            });
        }

        if (isValid != 0) {
            $("#RN_validatn").html("The route name already exists.");
            $("#RN_validatn").show();
            return;
        }

        var id = 0;
        if (!$("#SaveAsNew").is(':checked'))
            id = getRouteID();
        if (RouteFlagSessionVal == 2) {
            routePart.routePartDetails.routeType = "outline";
        }
        else {
            routePart.routePartDetails.routeType = "planned";
            if (returnRoutePart != null && returnRoutePart != undefined)
                returnRoutePart.routePartDetails.routeType = "planned";
        }

        var e = document.getElementById("PontTy");
        var index = e.selectedIndex;
        var tabindex = activePath;

        routePart.routePartDetails.routeName = $("#Routename").val();
        routePart.routePartDetails.routeDescr = $("#routeDescr").val();

        if (returnRoutePart != null && returnRoutePart != undefined) {
            returnRoutePart.routePartDetails.routeName = routePart.routePartDetails.routeName + ' (Return)';
            returnRoutePart.routePartDetails.routeDescr = 'Return - ' + routePart.routePartDetails.routeName;
            $("#IsReturnRoute").val(1);
            $("#IsReturnRouteAvailable_Flag").val(true);
        }

        getValues(tabindex, index);

        var invalidPoints = validateRoutePoints();//ESDAL 2022  validateRoutePoints();
        if (invalidPoints != "") {
            var newString = "Fill in valid/full address for " + invalidPoints;
            if (newString.length > 100)
                newString = newString.slice(0, 100) + "...";
            $("#RP_validatn").html(newString);
            return;
        }

        startLoading();
        document.getElementById("btn_save").disabled = true;
        document.getElementById("Back_RtSave").disabled = true;
        document.getElementById("span-close").disabled = true;

        //added by poonam (13.8.14)
        if ($('#CRNo').val() == undefined) {
            var vr1contrefno = $('#VR1ContentRefNo').val();
        }
        else {
            var vr1contrefno = $('#CRNo').val();
        }

        var vr1versionid = $('#AppVersionId').val();
        var notif_flag = 0;
        notif_flag = $("#AddingRouteBy").val();
        var routerevisionid = $('#revisionId').val();
        var notifid = $('#NotificatinId').val();
        if (typeof NextnUpdate !== "undefined") {
            if (NextnUpdate == 1) {
                $('#btn_GeneralSubmit').hide();
                if (typeof routePart.routePartDetails.routeNo === "undefined" || routePart.routePartDetails.routeNo == 1) {
                    id = $('#PlanRouteOnMapId').val();
                }
                else {
                    id = $('#PlanRouteOnMapId1').val();
                    if ($("#AddingRouteBy").val() == 3)
                        $("#returnRtSaved").val(true);
                }
            }
        }
        //-----------
        var isSimple = 0;
        if ($('#IsSimplifiedNotif').val() == "YES") {
            isSimple = 1;
        }
        if (isSimple == 1 && routePart.Esdal2Broken == 0) {
            routePart.LibRtBrok = $('#LibRoutePartID').val();//setting up library routes
        }
        var VIsNEN = false, retunId = 0, VIsAutoPlan = false;;
        var VRouteFlag = $('#hf_RouteFLag').val();
        try {
            if (VRouteFlag == "" && $('#IsCandVersion').val() == 'True') {
                VRouteFlag = '4';
                $('#hf_RouteFLag').val('4');
            }
        }
        catch (err) {}
        if ($("#hIs_NEN").val() == "true" || $("#hIs_NEN").val() == "True") {
            VIsNEN = true; VRouteFlag = 3;
            rowID1 = "#status_" + $('#hreturnLeg_routeID').val();
            if ($(rowID1).html() == "Unplanned")
                returnRoutePart = true;
        }
        var checkflag = 0;
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
            async: false,
            //contentType: 'application/json; charset=utf-8',
            beforeSend: startAnimation(),
            processData: false,
            data: JSON.stringify({ compressedRoutePart: base64RoutePartData, PlannedRouteId: id, orgID: 1101, RouteFlag: VRouteFlag, VR1ContentRefNo: vr1contrefno, VR1VersionId: vr1versionid, RouteRevisionId: routerevisionid, Dock_Flag: flag, NotificationId: notifid, isSimple: isSimple, IsNEN: VIsNEN, IsNENApi: IsNenApi(), IsAddToLibrary: isAddToLibrary }),
            beforeSend: function () {
                startAnimation();
            },
            success: function (val) {
                if (val.value != 0) {
                    isUnsavedRouteExist = false;
                    var RN = $("#Routename").val();
                    _RouteID = val.value;
                    _RouteName = RN;
                    var mainrouteid = val.value
                    var mainroutename = $("#Routename").val();
                    setRouteID(_RouteID);//setting route id for when user try to create route after the save without go back to page. Then it should change to edit mode with out page reload//setting route id for when user try to create route after the save without go back to page. Then it should change to edit mode with out page reload
                    setRouteNameEdit(_RouteName);
                    $('#viewHfRouteId').val(_RouteID);
                    if (typeof mapInputValuesGlobal != 'undefined' && typeof getMapInputValues != 'undefined') {
                        mapInputValuesGlobal = getMapInputValues();
                    }
                    if (returnRoutePart != null && returnRoutePart != undefined) {
                        if (VIsNEN == true) {
                            retunId = val.VNENRturnRouteID; //$('#hreturnLeg_routeID').val();
                            VIsAutoPlan = false;
                            returnRoutePart.routePartDetails.routeType = "planned";
                            returnRoutePart.routePartDetails.routeName = "NEN Return Route";
                            returnRoutePart.routePartDetails.routeDescr = $('#hReturnRouteDesc').val();
                        }
                        if ($('#PortalType').val() == '696008') {
                            try {
                                if ($("#chkdockcaution").is(':checked')) {
                                    returnRoutePart.routePartDetails.dockcaution = "True";
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
                            data: JSON.stringify({ compressedRoutePart: base64RoutePartData, PlannedRouteId: retunId, orgID: 1101, RouteFlag: VRouteFlag, VR1ContentRefNo: vr1contrefno, VR1VersionId: vr1versionid, RouteRevisionId: routerevisionid, Dock_Flag: flag, NotificationId: notifid, IsNEN: VIsNEN, IsNENApi: IsNenApi(), IsAutoPlan: VIsAutoPlan, IsReturnRoute: true, IsAddToLibrary: isAddToLibrary }),
                            success: function (val) {
                                RN = RN + "' and '" + RN + " (Return)";
                                if (VIsNEN == true) {
                                    routeId_forChStatua = retunId;
                                    NENChangeRouteStatus();
                                }
                                saveRoute(id, val, RN, true);
                            },
                            async: true
                        });
                    }
                    if ($("#chk_addReturnRoute").is(':checked')) {
                        AddNotificationReturnRoute(mainrouteid, mainroutename, returnRoutePart, id, val, RN);
                        checkflag = 1;
                    }
                    if (checkflag == 0) {
                        if (returnRoutePart == null || returnRoutePart == undefined) {
                            saveRoute(id, val, RN, false);
                        }
                    }
                }
                else {
                    saveRoute(null, null, null, null, false);
                }
            },
            error: function (request, status, thrownError) {
                saveRoute(null, null, null, null, false);
            },
            complete: function () {
                if (checkflag == 0) {
                    if (VIsNEN == true) {
                        if (returnRoutePart == null && returnRoutePart == undefined) {
                            stopAnimation();
                        }
                    }
                    else {
                        stopAnimation();
                    }
                }
                // to update route list with modified route
                //if (notif_flag != 0 || notif_flag != undefined) {
                if (notif_flag != undefined) {
                    // EditNotifRoute();
                    addscroll();
                }
                //show save button on sucessfull plan
                $('#btn_saveRoute').show();
                //IsRoutePlanned = true;
            },
            async: true
        });
    });
    $('body').on('change', '#PontTy', function (e) {
        var e = document.getElementById("PontTy");
        var index = e.selectedIndex;
        var tabindex = activePath;
        if (index === 0)
            document.getElementById('GroupboxDetails').innerHTML = "Start point details";
        else
            if (index === 1)
                document.getElementById('GroupboxDetails').innerHTML = "End point details";
            else {
                var SelectedText = e[index].text;
                var contains = SelectedText.includes("Way");
                if (contains) {
                    document.getElementById('GroupboxDetails').innerHTML = "Way point details";
                }
                else {
                    document.getElementById('GroupboxDetails').innerHTML = "Via point details";
                }
            }

        if (prevIndex != index) {
            getValues(tabindex, prevIndex);
            setValues(tabindex, index);
            prevIndex = index;
        }
    });
});

function AddNotificationReturnRoute(mainRouteId, mainRouteName, returnRoutePart, id, val, RN) {
    $.ajax({
        type: "POST",
        url: "../Routes/AddReturnRoute",
        data: { routePartId: mainRouteId },
        beforeSend: function () {
            startAnimation('Planning return leg');
        },
        success: function (response) {
            AddReturnRoutePart(response, mainRouteName, 1, function (returnRouteId) {
                setReturnRouteCheckboxWorkFlowPayload(response.result.RouteId, returnRouteId, function () {
                    RN=RN + "' and '" + RN + " (Return)"
                    if (returnRoutePart == null || returnRoutePart == undefined) {
                        saveRoute(id, val, RN, false);
                    }
                    stopAnimation();
                });
            });
        }
    });
}

function setReturnRouteCheckboxWorkFlowPayload(mainRouteId, returnRouteId, callbackFn) {
    $.ajax({
        type: "POST",
        url: "../Routes/SetWorkFlowPayload",
        data: { mainRoutePartId: mainRouteId, returnRoutePartId: returnRouteId },
        beforeSend: function () {
            startAnimation('Planning return leg');
        },
        success: function () {
            callbackFn();
        }
    });
}

function isNonAlphaNumforRoute(evt) {
    // Validation For Special Characters & < >
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var charStr = String.fromCharCode(charCode);
    if ((evt.ctrlKey == true && (evt.key == "v" || evt.key == "V")) || charCode == 221 || charCode == 191 || charCode == 186 ||
        charCode == 219 || charCode == 17 || charCode == 222 || charCode == 220 ||
        charStr == "<" || charStr == ">" || charStr == "#" || charStr == "^" || charCode == 192 ||
        charStr == "!" || charStr == ">" ||  charStr == "%" || charStr == "(" || charStr == ")" ||
        charStr == "&" || charStr == "*" || charStr == "$") {
        // alert(charStr + " is not Allowed ");

        return false;
    }
    else {
        $('#RN_validatn').hide();
        return true;
    }
}
function setPathDetailsForTab(tabindex) {

    var e = document.getElementById("PontTy");
    var index = e.selectedIndex;
    var waycount = e.length;
    getValues(prevtabindex, prevIndex);
    prevIndex = 0;

    removeCombolistItems(prevtabindex);
    setCombolistItems(tabindex);
    setValues(tabindex, 0);
    if (prevtabindex != tabindex) {//setting Start point details
        document.getElementById('GroupboxDetails').innerHTML = "Start point details";
    }
    prevtabindex = tabindex;
}
function removeCombolistItems(prevtabindex) {
    if (routePart.routePathList[prevtabindex].routePointList.length > 2) {
        for (var i = 2; i < routePart.routePathList[prevtabindex].routePointList.length; i++)
            $('#PontTy option[value=' + i + ']').remove();
    }
}
function setCombolistItems(tabindex) {

    if (routePart.routePathList[tabindex].routePointList.length > 2) {
        for (var i = 2; i < routePart.routePathList[tabindex].routePointList.length; i++) {
            if (routePart.routePathList[tabindex].routePointList[i].pointType == 2)
                $("#PontTy").append($("<option />").val(i).text("Waypoint" + (i - 1)));
            else
                //$("#PontTy").append($("<option />").val(i).text("Viapoint" + (i - 1)));// cmnd by nk HE-8764 on 6.12.2023
            $("#PontTy").append($("<option />").val(i).text("Stopping point" + (i - 1)));// added by nk HE-8764 on 6.12.2023

        }
    }
}
function addtablist() {
    var count = routePart.routePathList.length;
    var i = 0;
    for (i = 1; i <= count; i++) {
        //$("#pathnumberTb").append("<li><a href='#ui-tabs-" + i + "'>" + 'Path' + i + "</a></li>");
        //$('#pathcounttabs').append("<div id='ui-tabs-" + i + "'>&nbsp;</div>");
        $("#pathnumberTb").append("<div class='column btn-erd-path-click' data-indexval=" + i + "><div class='card' id='pathcounttabsId" + i + "' style='width: 5rem!important;'> <span class='text-normal-hyperlink'> Path " + i + "</span></div></div>");
    }
}
function PathClicked(pathNum) {
    activePath = pathNum - 1;
    var PathId = "#pathcounttabsId" + pathNum;
    $(".selectcol").removeClass("selectcol");
    $(PathId).addClass('selectcol');
    setPathDetailsForTab(activePath);
    $('#PontTy').val('Start');
}
function saveRoute(id, val, RN, returnLeg, res) {
    $('#div_saving').hide();
    if (res != false) {
        SuccesSave = true;
        if (id == 0) {
            id = val.value;
            $('#RoutePartIdS').val(val.value);
            _RouteID = val.value;
            _RouteName = RN;
            $('#chkRouteID').val(_RouteID);
            if ($('#viewHfRouteId').length > 0)
                $('#viewHfRouteId').val(_RouteID);
            if (returnLeg == true)
                RN = "Routes " + "'" + RN + "' " + "saved successfully";
            else
                RN = "Route " + "'" + RN + "' " + "saved successfully";

            if (($("#hIs_NEN").val() == "true" || $("#hIs_NEN").val() == "True")) {
                if (ActionForReturnRout == 1)
                    $("#hreturnLeg_routeID").val(val.value);
            }
            var Pre_tital = String($('#tabTitale').html());
            if (String(Pre_tital) == "Notification form") {
                ClosePopUp12();
            }
            else {
                if ($('#hf_IsPlanMovmentGlobal').length == 0)
                    ShowSuccessModalPopup(RN, "fullScreen")
                else {
                    if ($("#MaxmizeIocn").is(":visible"))
                        IsFullScreen = 1;
                    else
                        IsFullScreen = 0;
                    ShowSuccessModalPopup(RN, "RedirectToRouteList")
                }
                
                if (RouteFlagSessionVal != 4) {
                    $("#pageheader").find("h3").text($("#Routename").val() + "       " + (routePart.routePathList[0].routePointList[0].pointDescr) + "       " + "to " + "     " + routePart.routePathList[0].routePointList[1].pointDescr);
                }
            }

        }
        else {
            if (val != 0) {
                if (id != 0) {
                    $('#RoutePartIdS').val(id);
                    $('#chkRouteID').val(id);
                }
                var url = '../Routes/RouteUpdateFlagSessionClear';
                $.ajax({
                    url: url,
                    type: 'POST',
                    cache: false,
                    beforeSend: function () {
                    },
                    success: function (page) {
                    },
                    complete: function () {
                    }
                });
                RN = "Route " + "'" + RN + "' " + "updated successfully";
                //showWarningDialog(RN, 'Ok', '', close_alert, '', 1, 'info');
                if (($("#hIs_NEN").val() == "true" || $("#hIs_NEN").val() == "True")) {
                    if (ActionForReturnRout != 1)
                        VObj_err = null;
                    routeId_forChStatua = id;
                    $('.message1').css({ 'text-align': 'center' });
                    ShowSuccessModalPopup(RN, "NENChangeRouteStatusPopupcall");
                    //showWarningPopDialog(RN, 'Ok', '', 'NENChangeRouteStatus', '', 1, 'info');
                    IsrestrictionsErrror = false;
                }
                else {
                    if (RouteFlagSessionVal != 2 && RouteFlagSessionVal != 1 && RouteFlagSessionVal != 3) {
                        //ShowSuccessModalPopup(RN, 'stayBack');
                        if ($('#hf_IsPlanMovmentGlobal').length == 0)
                            ShowSuccessModalPopup(RN, "fullScreen")
                        else {
                            if ($("#MaxmizeIocn").is(":visible"))
                                IsFullScreen = 1;
                            else
                                IsFullScreen = 0;
                            ShowSuccessModalPopup(RN, "RedirectToRouteList")
                        }
                    }
                    else {
                        if ($('#hf_IsPlanMovmentGlobal').length == 0)
                            ShowSuccessModalPopup(RN, "fullScreen")
                        else {
                            if ($("#MaxmizeIocn").is(":visible"))
                                IsFullScreen = 1;
                            else
                                IsFullScreen = 0;
                            ShowSuccessModalPopup(RN, "RedirectToRouteList")
                        }
                    }
                }
                if (RouteFlagSessionVal != 4) {
                    $("#pageheader").find("h3").text($("#Routename").val() + "       " + (routePart.routePathList[0].routePointList[0].pointDescr) + "       " + "to " + "     " + routePart.routePathList[0].routePointList[1].pointDescr);
                }
            }
            else
                ShowErrorPopup('Update failed', 'fullScreen');
        }
    }
    else {
        SuccesSave = false;
        if ($("#hIs_NEN").val() == "true")
            close_alert();
        else
            ShowErrorPopup('Saving failed', 'fullScreen');
    }
}
function startLoading() {
    $("#dialogue").find('#div_save').append("<div id='div_saving' class='search_anim'></div>");
    $('#div_saving').show();
}
function ClosePopUp12() {
    CloseSuccessModalPopup();
    CloseErrorPopup();
    if (document.getElementById("RouteEditedAfterViewing")) {
        $('#RouteEditedAfterViewing').val(true);
    }
    var WichPage = $("#WichPage").val();
    var VR1Applciation = $('#VR1Applciation').val();
    var SORTApplicationchk = $('#SORTApplication').val();
    var status = $('#SortStatus').val();
    var Pre_tital = String($('#tabTitale').html());
    if (RouteFlagSessionVal == "" && $('#IsCandVersion').val() == 'True') {
        RouteFlagSessionVal = 4;
    }
    if (WichPage == 2 || WichPage == 1 || WichPage == 3 || RouteFlagSessionVal == 4 || RouteFlagSessionVal == 2) {
        close_alert();
        if (WichPage == 3) {
            if (String(Pre_tital) == "Notification form") {
                var url = '../Routes/getRouteID';
                $.ajax({
                    url: url,
                    type: 'POST',
                    cache: false,
                    beforeSend: function () {
                    },
                    success: function (value) {

                        //RouteId = $('#PreRouteID').val();
                        //if (RouteId != 0)
                        //    DeleteRouteSimliNoti(value.value);
                        var rID = $('#RoutePartIdS').val();
                        if (value != undefined && value.value != undefined && value.value != 0) {

                            if (routePart != undefined && routePart != null && routePart.routePartDetails.routeNo == 2)
                                $('#NotiplannedRouteId1').val(value.value);
                            else
                                $('#NotiplannedRouteId').val(value.value);

                            $('#NotifEditedRouteId').val(value.value);//edited route id is saved here temporarily
                            DisplayPiontsforNotifi(value.value);
                        }
                        else if (rID != 0) {
                            if (routePart != undefined && routePart != null && routePart.routePartDetails.routeNo == 2)
                                $('#NotiplannedRouteId1').val(rID);
                            else
                                $('#NotiplannedRouteId').val(rID);

                            $('#NotifEditedRouteId').val(rID);//edited route id is saved here temporarily
                            DisplayPiontsforNotifi(rID);
                        }
                        else {
                            showWarningPopDialog('Route in this Notification is not saved properly. Please re-plan or import new route before proceeding.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                            $('#PlanRoute').attr('checked', false);
                            return;
                        }


                    },
                    complete: function () {
                    }
                });
                GotoSimNoti();

                if (Pre_tital == "Notification form")
                    $('#pageheader').html('<h3> Simplified notification</h3>');

                SessionClear();
            }
            else {

                if (document.getElementById("NotifEditedRouteId")) {
                    var routeIdTemp = $('#RoutePartIdS').val();
                    $('#NotifEditedRouteId').val(routeIdTemp);
                    GotoDetNoti(routeIdTemp);
                }
                else {
                    CloseSuccessModalPopup();
                    //CloneRoutesNotify();
                    $('#pageheader').html('<h3> Detailed notification - Route</h3>');
                }

            }
        }
        else if (RouteFlagSessionVal == 4) {
            if (VR1Applciation == 'False' && SORTApplicationchk == 'True') {
                //showmap
            }
            else {
                BindRouteParts();    //ShowCandidateRoutes();
            }
        }
        else if (VR1Applciation == 'False' && SORTApplicationchk == 'True') {
            //SOvalidationfun1();
            //BackToImportRouteList();
            // SORTShowSORoutePage();
        }
        else {
            //CloneRoutes();
            CloseSuccessModalPopup();
            var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        }

    }
    else if (RouteFlagSessionVal == 3) {
        close_alert();
        NENRouteSaveCompletion();
    }
    else {
        close_alert_RouteLibrary();
    }
    resetdialogue();
    //Writing name to a route
    $("#RoteHeading").text(_RouteName);
}
function SessionClear() {
    var url = '../Routes/RouteSearchSessionClear';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        beforeSend: function () {
        },
        success: function (page) {
        },
        complete: function () {
        }
    });
}
function getValues(tabindex, prevIndex) {
    var contact = {};

    contact['addressLine1'] = $("#locationAddress").val();
    contact['telephone'] = $("#locationContact").val();
    routePart.routePathList[tabindex].pathDescr = $("#pathDescr").val();
    routePart.routePathList[tabindex].routePointList[prevIndex].pointDescr = $("#startDesc").val();
    routePart.routePathList[tabindex].routePointList[prevIndex].routeContactList = [];
    routePart.routePathList[tabindex].routePointList[prevIndex].routeContactList.push(contact);
}
function setValues(tabindex, index) {
    $("#pathDescr").val(routePart.routePathList[tabindex].pathDescr);
    $("#startDesc").val(routePart.routePathList[tabindex].routePointList[index].pointDescr);
    if ((routePart.routePathList[tabindex].routePointList[index].pointType == 2))
        $('#startDesc').prop('readonly', true);
    else
        $("#startDesc").prop("readonly", false);
    $('#locationAddress').val("");
    $('#locationContact').val("");
    //$('#startLocationAddress').val("");
    //$('#startLocationContact').val("");

    //if (routePart.routePathList[tabindex].routePointList[index].routeContactList != null && routePart.routePathList[tabindex].routePointList[index].routeContactList.length > 0) {
    //    $("#locationAddress").val(routePart.routePathList[tabindex].routePointList[index].routeContactList[0].addressLine1);
    //    $("#locationContact").val(routePart.routePathList[tabindex].routePointList[index].routeContactList[0].telephone);
    //    //   $("#startLocationAddress").val(routePart.routePathList[tabindex].routePointList[index].routeContactList[0].addressLine1);
    //    //$("#startLocationContact").val(routePart.routePathList[tabindex].routePointList[index].routeContactList[0].telephone);
    //}
}
function close_alert_RouteLibrary() {
    close_alert();
    var VSearchString = $("#Routename").val();
    var VID = $("#HFrouteID").val();
    showSavedRouteDetails(_RouteID, "planned", _RouteName, "U");
    //  window.location = '../Routes/RoutePartLibrary';
}
function close_alert() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#pop-warning").hide();
    $("#overlay").hide();
    addscroll();

    resetdialogue();
    return false;

}
function DisplayPiontsforNotifi(LibraryrouteId) {
    $('#RoutePartIdS').val(LibraryrouteId);
    $.ajax({
        url: '../Routes/GetPlannedRoute',
        type: 'POST',
        async: false,
        data: { RouteID: LibraryrouteId, routeType: "planned", IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },
        success: function (result) {
            if (result != null && result.result != undefined && result.result.routePathList != undefined && result.result.routePathList.length > 0) {
                var datacollection = result.result.routePathList[0].routePointList, count = -1;
                for (var x = 0; x < result.result.routePathList.length; x++) {
                    result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
                }
                count = datacollection.length;
                if (result.result.routePartDetails.routeNo == 1) {
                    $('#NotiplannedRouteId').val(LibraryrouteId);
                    $('#LibRoutePartID').val(LibraryrouteId);
                    $('#LibRouteName').val(result.result.routePartDetails.routeName);
                    for (var i = 0; i < count; i++) {
                        if (datacollection[i].pointType == 0)
                            $('#FromAddress').val(datacollection[i].pointDescr);
                        else if (datacollection[i].pointType == 1)
                            $('#ToAddress').val(datacollection[i].pointDescr);
                    }
                }
                else {
                    $('#NotiplannedRouteId1').val(LibraryrouteId);
                }
                $('#AddtoLib').show();
                var u = $('#Update').val();
                if (u != 0) {
                    $('#UpdateRoute').val(1);
                }
            }
            else {
                showWarningPopDialog('Route in this Notification is not saved properly. Please re-plan or import new route before proceeding.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                $('#PlanRoute').attr('checked', false);
                return;
            }
        }
    });
    $('#pageheader').html('<h3> Simplified notification</h3>');
}
function fullScreen() {
    if (SuccesSave) {//if route saved succesfully then update the route details in left panel
        SetupRouteDeatil();
    }
    if ($("#Minimzeicon").is(":visible")) {
        ClosePopUp12();
    }
    else {
        //CloseSuccessModalPopup();
        ClosePopUp12();

    }
}
function SetupRouteDeatil() {
    if (routePart != null && (routePart.routePathList.length > 0 || routePart.routePathList.Count > 0)) {

        if (routePart.routePathList[0].routePointList.length > 0) {
            var fromIndex = -1, toIndex = -1;
            for (var i = 0; i < routePart.routePathList[0].routePointList.length; i++) {
                if (fromIndex == -1 && routePart.routePathList[0].routePointList[i].pointType == 0) {
                    $("#From_location").val(routePart.routePathList[0].routePointList[i].pointDescr);
                    fromIndex = i;
                }
                if (toIndex == -1 && routePart.routePathList[0].routePointList[i].pointType == 1) {
                    $("#To_location").val(routePart.routePathList[0].routePointList[i].pointDescr);
                    toIndex = i;
                }
            }
        }
        if (routePart.routePathList.length > 1) {
            clear_selection();
            $(".account").html('Path1');
            $(".root > li:first > a").addClass("active");
            change_select(0, 0);
        }
    }
}
function stayBack() {
    CloseSuccessModalPopup();
    $("#RoteHeading").text(_RouteName);
}
function CloseRtpop() {
    $('#exampleModalCenterRoute').hide();
    $('#overlay').hide();
    $('#dialogue').hide();
}