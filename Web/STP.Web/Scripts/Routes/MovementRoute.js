var RouteVehicleAssignFlag ;
var routeId;
var routeType;
var routeIdsVal;
var vehicleIdsVal;
var vehicleMoveId;
var routeIdArr = [];
var vehicleIdArr = [];
var RouteListCount;
var CandidatePermission;
var pageflag;
var IsSupplimentarySaved;
var ProjectID;
var OwnerName;
var plannruserid;
var checkingstatus;
var _isvr1;
var Enter_BY_SORT;
var AppRouteList = [];
var existingRouteIds;
var IsFullScreen = 0;
function MovementRouteInit(IsRouteSummary = false, viewRoute = false) {
    IsRoutePlanned = false;
    if (typeof mapInputValuesGlobal != 'undefined') {
        mapInputValuesGlobal = [];
    }
    RouteVehicleAssignFlag = false;
    AppRouteList = [];
    StepFlag = 4;
    SubStepFlag = 0;
    CurrentStep = "Route Details";
    $('#current_step').text(CurrentStep);
    if (typeof SetWorkflowProgress!='undefined')
        SetWorkflowProgress(4);
    $('#save_btn').hide();
    $('#apply_btn').hide();
    routeIdsVal = $('#hf_routeIds').val();
    vehicleIdsVal = $('#hf_route_vehicleIds').val();
    vehicleMoveId = $('#hf_vehicleMoveId').val();
    CurrentAssessmentstatus = $('#hf_Assessmentstatus').val();
    Notificationflag = $('#hf_Notificationflag').val();

    AutoAssignRouteVehicle = Boolean($('#AutoAssignRouteVehicle').val().toLowerCase() == 'true');
    $('#hf_VehicleAssignedGlobal').val($('#IsRouteVehicleAssigned').val());
    VehicleAssignedList = JSON.parse($('#AssignedVehicleList').val());

    routeIdArr = [];
    vehicleIdArr = [];

    if (routeIdsVal != undefined && routeIdsVal != null) {
        routeIdsVal = JSON.parse(routeIdsVal);
        for (var i = 0; i < routeIdsVal.length; i++) {
            var id = routeIdsVal[i];
            routeIdArr.push(id);
        }
    }

    if (isTopNavClicked) {
        existingRouteIds = JSON.stringify(routeIdArr);
        isTopNavClicked = false;
    }

    if (vehicleIdsVal != undefined && vehicleIdsVal != null) {
        vehicleIdsVal = JSON.parse(vehicleIdsVal);
        for (var i = 0; i < vehicleIdsVal.length; i++) {
            var id = vehicleIdsVal[i];
            vehicleIdArr.push(id);
        }
    }
    
    RouteListCount = parseInt($('#hf_RouteListCount').val());

    $('#assignVehicleToRoute').hide();
    if (RouteListCount == 0) {
        $('#confirm_btn').hide();
    }
    else {
        $('#confirm_btn').removeClass('blur-button');
        $('#confirm_btn').attr('disabled', false);
        $('#confirm_btn').show();
        $('#backbutton').show();
    }
    CandidatePermission = $('#hf_CandidatePermission').val();
    pageflag = $('#hf_pageflag').val();
    IsSupplimentarySaved = $('#hf_IsSupplimentarySaved').val();
    ProjectID = $('#projectid').val();
    OwnerName = $('#OwnerName').val();
    plannruserid = $('#PlannrUserId').val();
    checkingstatus = $('#CheckerStatus').val();
    _isvr1 = $('#VR1Applciation').val();
    Enter_BY_SORT = $('#EnterBySort').val();

    routeNamesVal = $('#hf_routeNames').val();
    routeNamesVal = JSON.parse(routeNamesVal);
    routeTypesVal = $('#hf_routeTypes').val();
    routeTypesVal = JSON.parse(routeTypesVal);
    

    for (var i = 0; i < routeNamesVal.length; i++) {
        var routeName = routeNamesVal[i];
        var routeId = routeIdsVal[i];
        var routeType = routeTypesVal[i];
        AppRouteList.push({ RouteID: routeId, RouteName: routeName, RouteType: routeType });
    }

    if (IsRouteSummary == false) {
        var viewHfRouteId = parseInt($('#viewHfRouteId').val());
        if (viewEditRouteFlagStructures != 0) {
            $('#showRouteIdMap').val(viewHfRouteId);
        }
        if (!viewRoute) {
            if (AddNewRoute > 0) {
                $('#showRouteIdMap').val(0);
                AddNewRoute = 0;
            }
            else if (viewHfRouteId > 0) {
                $('#showRouteIdMap').val(viewHfRouteId);
                $('#viewHfRouteId').val(0);
            }
            else if (AppRouteList != undefined && AppRouteList.length > 0) {
                $('#showRouteIdMap').val(AppRouteList[0].RouteID);
            }
            else {
                $('#showRouteIdMap').val(0);
            }
        }
        CreateRoute(AppRouteList);
        $('#IsRouteSummary_Flag').val(0);
        $('#backbutton').show();
    }
    else {
        $('#backbutton').hide();
        if (!AutoAssignRouteVehicle) {
            $('#assignVehicleToRoute').show();
        }
        if (AutoAssignRouteVehicle || RouteListCount == 1) {
            $('#backbutton').show();
            $('#confirm_btn').show();
            $('#back_btn').hide();
        }
        
        if (AppRouteList != undefined && AppRouteList.length > 0) {
            $('#divViewRouteContainer').show();
            $('#btn_continue_to_route_assessment').show();
            $('#btn_back_to_movement_type_confirm').hide();
            $('#btn_continue_to_route_summary').show();
        }
        else {
            $('#divViewRouteContainer').hide();
            $('#btn_continue_to_route_assessment').hide();
            $('#btn_back_to_movement_type_confirm').show();
            $('#btn_continue_to_route_summary').hide();
        }
        $('#IsRouteSummary_Flag').val(1);
        stopAnimation();
        closeContentLoader('html');
    }

    //sortable
    $(function () {
        var $clone = "";
        $(".route-list-sortable-container").sortable({
            start: function (event, ui) {
               // $('html').css('overflow', 'visible');
                ui.placeholder.html($clone[0].outerHTML);
            },
            stop: function (event, ui) {
               // $('html').css('overflow', 'hidden');
            },
            placeholder: "highlight",
            update: function (event, ui) {
                var items = $(this).sortable('toArray').toString();
                //console.log(items);
                startAnimation();
                $.ajax({
                    url: '../Routes/ReOrderRoutePart',
                    data: { routePartIds: items },
                    type: 'POST',
                    success: function (data) {
                        stopAnimation();
                        if (data.result) {
                            showToastMessage({
                                message: "Route order has been successfully changed.",
                                type: "success"
                            });
                        } else {
                            showToastMessage({
                                message: "Something went wrong!",
                                type: "error"
                            });
                        }
                    }
                });
            },
            connectWith: '.route-list-sortable-container',
            revert: 0,
            forcePlaceholderSize: true,
            axis: "y",
            containment: "parent",
            cursor: "move",
            tolerance: "pointer",
            helper: function (event, ui) {
                $clone = $(ui).clone();
                $clone.css('position', 'absolute');
                $clone.css('width', '92%');
                $clone.css('background-color', 'rgb(231 231 231)');
                $clone.css('margin-left', '10px');
                return "<div></div>";
            },
        });
    });

    if (routeIdArr.length == 0) {
        $('#divRouteActionsContainer .dropdwon_new').text('IMPORT ROUTE');
    } else {
        $('#divRouteActionsContainer .dropdwon_new').text('ADD ROUTE');
    }

}
$(document).ready(function () {
    $('body').on('click', '#ImportRouteFromLibrary', function (e) {
        e.preventDefault();
        var Isfav = $(this).attr("dataisfav1");
        var Issort = $(this).attr("dataissort3");
        var isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();
        if (isUnsavedChange) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "ImportFromLibrary", "CloseWarningPopupRef", Isfav, Issort);
        }
        else {
            ImportFromLibrary(Isfav, Issort)
        }
    });
    $('body').on('click', '#ImportRouteFromPrevMovements', function (e) {
        if ($('div#VehicleFilterDiv .VehicleFilter').length > 1) {
            $('div#VehicleFilterDiv .VehicleFilter').not(':first').remove();
            $('#HaulAddOption').show();
            $('#HaulRemoveOption').hide();
        }
        var Issort = $(this).attr("dataisort2");
        var isUnsavedChange = false;
        if (Issort!="true")
            isUnsavedChange = ValidateUnSavedChange("map") || CheckMapInputHasChanges();

        if (isUnsavedChange) {
            ShowWarningPopup("There are unsaved changes. Do you want to continue without saving?", "SelectRouteFromPrevMovements", "CloseWarningPopupRef", Issort);
        }
        else {
            SelectRouteFromPrevMovements(Issort)
        }
    });
    $('body').on('click', '#ImportRouteCurrentMovements', function (e) {
        SelectCurrentMovements(this);
    });
   
    $('body').on('click', '#select-route', function (e) {
        var var1 = $(this).data('var1');
        SelectRouteFromPrevMovements(var1);
    });
    $('body').on('click', '#view-map', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('routeid');
        var RouteName = $(this).data('routename');
        ViewMapRoute(RouteID, RouteName);
    });
    $('body').on('click', '#btn-viewmap', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('routeid');
        var RouteName = $(this).data('routename');
        ViewMapRoute(RouteID, RouteName);
    });

    $('body').on('click', '.btn-mr-bind-route-parts', function (e) {
        BindRouteParts();
    });
});
function CreateRoute(AppRouteList) {
    $("#select_route_section").html('');
    $("#select_route_section").hide();
    ClearRouteSessionFlag();
    var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
    var page = pageflag;
    var ShowReturnLeg = 0;
    if (page == 3) {
        ApplicationRevId = $('#CRNo').val() ? $('#CRNo').val() : 0;
        ShowReturnLeg = 0;
    }
    $("#WichPage").val(page);
    $('#route').show();
    $('#route').html('');
    $('#divViewRouteContainer').show();//TODO - Need to add logic to display
    if (IsFullScreen != 1) {
        $('#divRouteActionsContainer').show();
    }
    $('#confirm_btn').hide();
    $('#back_btn').hide();

    if ($('#IsNotif').val() != "true") {
        $('#btn_continue_to_route_assessment').html('CONFIRM AND CONTINUE');
    }
    else {
        $('#btn_continue_to_route_assessment').html('CONTINUE TO ROUTE ASSESSMENT');
    }
    $.ajax({
        url: '../Routes/LibraryRoutePartDetails',
        data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, workflowProcess: 'HaulierApplication', ShowReturnLeg: ShowReturnLeg, routeLists: AppRouteList },
        type: 'POST',
        success: function (page) {
            //SubStepFlag = 4.3;
            $('#banner-container').find('div#filters').remove();
            $('#route').html('');
            $('#route').append($(page).find('#CreateRoute').html());

            LibraryRoutePartDetailsInit();
            CheckSessionTimeOut();
            Map_size_fit();
            addscroll();
            if (AppRouteList != undefined && AppRouteList.length > 0) {
                $('#divViewRouteContainer').show();
                $('#btn_continue_to_route_assessment').show();
                $('#btn_back_to_movement_type_confirm').hide();
                $('#btn_continue_to_route_summary').show();
                $('#divRouteActionsContainer').find('li #IDCreateRoute').removeClass('active-route');
            }
            else {
                $('#divViewRouteContainer').hide();
                $('#btn_continue_to_route_assessment').hide();
                $('#btn_back_to_movement_type_confirm').show();
                $('#btn_continue_to_route_summary').hide();
                $('#divRouteActionsContainer').find('li #IDCreateRoute').addClass('active-route');
            }
            if (IsFullScreen == 1) {
                EnLargeMap();
                IsFullScreen = 0;
            }
        }
    });
}
function ImportFromLibrary(isFav, IsSORT = false, IsPlanMovement = false) {
    CloseWarningPopupRef();
    var isVr1Appln = false;
    if ($('#IsVR1').val() === "true") {
        isVr1Appln = true;
    }

    if (IsSORT) { //sort side integration -- workflow also needs a rewrite
        var dataModelPassed = { importFrm: 'library', isFavourite: isFav, workflowProcess: "HaulierApplication", Iscandidiate: true, NotificationId: $("#NotificationId").val(), ApplicationId: $("#AppRevisionId").val(), IsVr1Appln: isVr1Appln };
        var result = ApplicationRouting(2, dataModelPassed);
        if (result != undefined || result != "NOROUTE") {
            LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#select_route_section', '', function () {
                SelectRouteByImportInit();
            });
        }
    }
    else {
        var dataModelPassed = { importFrm: 'library', isFavourite: isFav, workflowProcess: "HaulierApplication", NotificationId: $("#NotificationId").val(), ApplicationId: $("#AppRevisionId").val(), IsVr1Appln: isVr1Appln };
        var result = ApplicationRouting(2, dataModelPassed);
        if (result != undefined || result != "NOROUTE") {
            LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#select_route_section', '', function () {
                SelectRouteByImportInit();
            });
        }
    }
}
function SelectRouteFromPrevMovements(IsSORT, backtopreviouslist) {
    var isVr1Appln = false;

    $('#divbtn_prevmove2').hide();
    $('#divbtn_prevmove2').remove();
    if ($('#IsVR1').val() === "true") {
        isVr1Appln = true;
    }
    if (IsSORT == 'True' || IsSORT == 'true' || IsSORT == true ) {
        SelectPrevtMovementsRoute();
    }
    else {
        var applicationId = $("#AppRevisionId").val();
        if (applicationId.length === 0) {
            applicationId = 0;
        }
        var dataModelPassed = { importFrm: 'prevMov', workflowProcess: "HaulierApplication", NotificationId: $("#NotificationId").val(), ApplicationId: applicationId, IsVr1Appln: isVr1Appln };
        var result = ApplicationRouting(2, dataModelPassed);

        if (result != undefined || result != "NOROUTE") {
            LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#select_route_section', '', function () {
             SelectRouteByImportInit();
            });
        }
    }
}
function SelectCurrentMovements() {

    var _isvr1Candi = $('#VR1Applciation').val();
    $('#ViewRouetDetail').hide();
    $('#divViewRouetDetail').hide();
    $('#RoutePart').hide();
    $('#SelectCurrentMovementsVehicle').show();
    $("#IsCreateApplicationRoute").val("true");
    $('#divCurrentMovement').hide();
    var wstatus = 'ggg';
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    $("#IsPrevMoveOpion").val(false);
    $('#SelectCurrentMovementsVehicle').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + 'true' + '&IsCurrentMovenet=true', {},
        function () {
            $("#dialogue").html('');
            $("#dialogue").hide();
            $("#overlay").hide();
            $("#select-route").hide();
            addscroll();
            stopAnimation();
            SORTAppCandidateRouteVersionInit();
            LoadAppAndMoveVersions(hauliermnemonic, esdalref, projectid);
        });
    startAnimation();
    $('#generalDetailDiv').show();
    $("#back_currentmovmnt_vehicle").hide();
    $("#back_currentmovmnt").show();
    if ($("#back_currentmovmnt").length == 0) {
        $('#generalDetailDiv').append('<div class="button main-button mr-0 col-lg-2" id="back_currentmovmnt"><div class="button main-button mr-0 col-lg-3 pt-5"><button class="btn btn-outline-primary btn-normal btn-mr-bind-route-parts" style="position:absolute;width:15%;" role="button" aria-pressed="true">BACK</button></div></div>');
    }
}
function EditRoute(RouteId, RouteName, Routetype,callback) {
    clearRoute();
    setRouteID(RouteId);
    $('.EditRouteCLS').removeClass('active-route');
    $('.EditRouteCLS[data-routeid=' + RouteId + ']').addClass('active-route');
    ShowRouteOnMap(RouteId, callback);
}
function CheckVehicleRouteAssigned(routeId) {
    var hasMatch = false;
    if (VehicleAssignedList != undefined && VehicleAssignedList.length > 0) {
        for (var i = 0; i < VehicleAssignedList.length; i++) {
            var assignedList = VehicleAssignedList[i];
            if (assignedList.RoutePartId == routeId) {
                hasMatch = true;
                break;
            }
        }
    }
    return hasMatch;
}
function DeleteRoute(RouteId, RouteType, RouteName) {
    routeId = RouteId;
    routeType = RouteType;
    var assignmentMatch = CheckVehicleRouteAssigned(RouteId);
    var msg = "Do you want to delete the Route?";
    if (assignmentMatch || $('#IsAgreedNotify').val().toLowerCase() == 'true') {
        msg = RouteName + " route is attached to a vehicle. Please delete the vehicle in the vehicle tab or reassign it to available routes (if not automatically assigned by the system)."
    }
    ShowWarningPopup(msg, 'DeleteAppRoute');
}
function DeleteAppRoute() {
    $('#btn_re_plan').hide();
    CloseWarningPopup();
    $('#IsRouteModify').val(1);
    $('#IsReturnRoute').val(0);
    $('#IsReturnRouteAvailable_Flag').val(false);
    $.ajax({
        type: "POST",
        url: "../Routes/DeleteApplicationRoute",
        data: {
            routeId: routeId, routeType: routeType
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            if (typeof IsRoutePlanned != 'undefined') {
                IsRoutePlanned = false;
            }
            if (response.Success) {
                var index = AppRouteList.indexOf(routeId);
                if (index >= 0) {
                    AppRouteList.splice(index, 1);
                }
                showToastMessage({
                    message: 'The route deleted successfully',
                    type: "success"
                });                
                RedirectToRouteList();
            }
            else {
                showToastMessage({
                    message: 'Route delete failed',
                    type: "error"
                })
            }
        },
        error: function (result) {
            CloseWarningPopup();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function RedirectToRouteList() {
    CloseSuccessModalPopup();
    var IsRouteSummary = false;
    if ($('#IsRouteSummary_Flag').val() == 1)
        IsRouteSummary = true;
    $('#IsReturnRouteAvailable_Flag').val(false);
    $('#IsVehicleAutoAssigned_Flag').val(false);
    stopAnimation();openContentLoader('html');
    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication', IsRouteSummaryPage: $('#IsRouteSummary_Flag').val() }, '#select_route_section', '', function () {
        MovementRouteInit(IsRouteSummary);
    });
}

function AddRouteToLibrary(RouteId, RouteType, RouteName) {
    $.ajax({
        type: "POST",
        url: "../Routes/AddRouteToLibrary",
        data: {
            routePartId: RouteId, routeType: RouteType, routeName: RouteName
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            if (response.RouteId > 0) {
                ShowSuccessModalPopup("Route " + "'" + RouteName + "'" + " added to Library successfully", 'CloseSuccessModalPopup');
            }
            else if (response.RouteExist > 0) {
                ShowErrorPopup('The route name already exists');
            }
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function Candi_ApplRevisions() {
    startAnimation()
    Checker = Checker.replace(/ /g, '%20');
    var _pstatus = $('#AppStatusCode').val();

    $('#ApplRevisions').load('../SORTApplication/SORTApplRevisions?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&Checker=' + Checker + '&PlannerId=' + plannruserid + '&OwnerName=' + OwnerName.replace(/ /g, '%20') + '&ProjectStatus=' + _pstatus, {},
        function () {
            $("#overlay").hide();
            stopAnimation()
        });
}
function Candi_Movement_Version() {
    startAnimation()
    Checker = Checker.replace(/ /g, '%20');
    $('#Movement_Version').load('../SORTApplication/SORTAppMovementVersion?ProjectID=' + ProjectID + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&EnterBySORT=' + Enter_BY_SORT + '&IsVR1App=' + _isvr1 + '&Checker=' + Checker + '&OwnerName=' + OwnerName.replace(/ /g, '%20'), {},
        function () {
            $("#overlay").hide();
            stopAnimation()
        });
}
function Candi_Special_order() {
    startAnimation();
    $('#Special_order').load('../SORTApplication/SORTSpecialOrderView?ProjectID=' + projectid, {},
        function () {
            $("#overlay").hide();
            stopAnimation();
            SORTSpecialOrderViewInit();
        });
}
function Candi_Candidate_route_version() {
    startAnimation();
    var wstatus = $('#hf_PStatus').val();
    $('#Candidate_route_version').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + CandidatePermission, {},
        function () {
            $("#overlay").hide();
            stopAnimation();
        });
}
function ViewMapRoute(RouteId, RoutePartName) {
    $("#select_route_section").hide();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetRouteVehicleDetails',
        data: { routePartId: RouteId },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            var routeDetails = result.routeDetails;
            $("#route").html('');
            $("#route").show();
            $("#route").load('../Routes/A2BPlanning?routeID=0', function () {
                SubStepFlag = 4.2;
                $("#map").addClass("context-wrap olMap");
                $('#map').css("height", "590px");
                loadmap('DISPLAYONLY', routeDetails);
                CheckSessionTimeOut();
                Map_size_fit();//, function () {
                addscroll();
                $("<h3 class='route-head' id='RouteHeading'></h3>").insertBefore("#map");
                $("#RouteHeading").text(RoutePartName);
            });
            $('#back_btn_Rt_prv').hide();
            $('#back_btn').show();
            setTimeout(function () {
                if ($('#hf_IsPlanMovmentGlobal').length > 0) {
                    $('#map').css('height', '100%');
                }
            }, 500);
            
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ViewRoutes(RouteId, routeName) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetPlannedRoute', //controller changed for to get routes from library
        data: { RouteID: RouteId, IsfromImp: true, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            for (var x = 0; x < result.result.routePathList.length; x++) {
                result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
            }
            var routeDetails = result.result;
            $('#haulier_details_section').hide();
            $('#select_vehicle_section').hide();
            $('#vehicle_details_section').hide();
            $('#movement_type_confirmation').hide();
            $('#select_route_section').hide();
            $('#route').html('');
            $('#route').hide();
            $('#route_vehicle_assign_section').hide();
            $('#supplimentary_info_section').hide();
            $('#overview_info_section').hide();
            $('#vehicle_edit_section').html('');
            $('#vehicle_Component_edit_section').html('');
            $('#vehicle_Create_section').html('');
            $('#vehicle_edit_section').hide();
            $('#vehicle_Component_edit_section').hide();
            $('#vehicle_Create_section').hide();
            $("#select_route_section").html('');
            $("#select_route_section").show();
            $("#select_route_section").load('../Routes/A2BPlanning?routeID=0', function () {
                SubStepFlag = 4.9;
                $("#map").addClass("context-wrap olMap");
                $('#map').css("height", "590px");
                loadmap('DISPLAYONLY', routeDetails);
                CheckSessionTimeOut();
                Map_size_fit();
                addscroll();
                $("<h3 class='route-head' id='RouteHeading'></h3>").insertBefore("#map");
                $("#RouteHeading").text(routeName);
            });
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ViewNotifgareedRoutes(RouteId, routeName) {

    $('#select_route_section_agredRt').show();
    $('#select_route_section_agredRt').html('');
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Routes/GetRouteVehicleDetails',
        data: { routePartId: RouteId },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            var routeDetails = result.routeDetails;
            $("#select_route_section_agredRt").load('../Routes/A2BPlanning?routeID=0', function () {

                $("#map").addClass("context-wrap olMap");
                $('#map').css("height", "590px");
                loadmap('DISPLAYONLY', routeDetails);
                CheckSessionTimeOut();
                Map_size_fit();
                addscroll();
                $("<h3 class='route-head' id='RouteHeading'></h3>").insertBefore("#map");
                $("#RouteHeading").text(routeName);
            });
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function AutoAssignVehiclesToRoute() {
    var Params = [];
    var param = {};
    for (var i = 0; i < routeIdArr.length; i++) {
        param = { RoutePartId: routeIdArr[i], VehicleIds: vehicleIdArr };
        Params.push(param);
    }
    $.ajax({
        type: "POST",
        url: "../Movements/AssignMovementVehicle",
        data: {
            vehicleAssignment: Params,
            revisionId: $('#AppRevisionId').val(),
            versionId: $('#AppVersionId').val(),
            contRefNum: $('#CRNo').val(),
            workflowProcess: 'HaulierApplication'
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            if (response) {
                NavigateToRouteAssessmnetSupply();
            }
        }
    });
}
function ShowRouteOnMap(routeId,callback) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "../Routes/GetPlannedRoute",
        data: { RouteID: routeId, routeType: "planned", IsNEN: false, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            stopAnimation();
            for (var x = 0; x < result.result.routePathList.length; x++) {
                result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
            }
            routePart = result.result;
            ShowRouteViewMap(routePart, callback);
        }
    });
}
function ClearRouteSessionFlag() {
    var url = '../Routes/RouteUpdateFlagSessionClear';
    $.ajax({
        url: url,
        type: 'POST',
        async: false,
        beforeSend: function () {
        },
        success: function (page) {
        },
        complete: function () {
        }
    });
}