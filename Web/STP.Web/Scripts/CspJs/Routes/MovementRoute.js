    var RouteVehicleAssignFlag = false;
    var routeId;
    var routeType;
    StepFlag = 4;
    SubStepFlag = 0;
    CurrentStep = "Route Details";
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(4);

    $('#save_btn').hide();
    $('#apply_btn').hide();

    var routeIdArr = [];
    var vehicleIdArr = [];
    var returnRouteIdArr = [];
    @foreach (var routeId in RouteIdArr)
    {
        @:routeIdArr.push("@routeId");
    }
    @foreach (var vehicleId in VehicleIdArr)
    {
        @:vehicleIdArr.push("@vehicleId");
    }

   
    var isChecked = $("#ReturnLeg").is(":checked");
    var isReturnRouteAvailableFlag = $('#IsReturnRouteAvailable_Flag').val();
 
     
    if (isReturnRouteAvailableFlag == 'true' || isReturnRouteAvailableFlag == 'True') {
    @foreach (var returnRouteType in ReturnRouteIdArr)
    {
        @:returnRouteIdArr.push("@returnRouteType");
    }
    }
    if (@RouteList.Count == 0) {
        $('#confirm_btn').hide();
    }
    else {
        if (@RouteList.Count > 1 && (isReturnRouteAvailableFlag == 'false' || isReturnRouteAvailableFlag == 'False')) {
            RouteVehicleAssignFlag = true;
        }
        $('#confirm_btn').removeClass('blur-button');
        $('#confirm_btn').attr('disabled', false);
        $('#confirm_btn').show();
    }
    $(document).ready(function () {
       
        $("#IDImportFromLibrary").on('click', ImportFromLibraryFirst);
        $("#IDSelectRouteFromPrevMovements").on('click', SelectRouteFromPrevMovementsFirst);
        $("#IDSelectCurrentMovements").on('click', SelectCurrentMovements);
        $("#IDImportFromLibrary").on('click', ImportFromLibrarySec);
       
    });
    function ImportFromLibraryFirst() {
        var Isfav = e.currentTarget.dataset.DataISFAV;
        var Issort = e.currentTarget.dataset.DataIsSort;
        ImportFromLibrary(Isfav, Issort)
    }
    function ImportFromLibrarySec() {
        var Issort = e.currentTarget.dataset.DataIsSort3;
        ImportFromLibrary(1, Issort)
    }
    function SelectRouteFromPrevMovementsFirst() {
        var Issort = e.currentTarget.dataset.DataIsort2;
        SelectRouteFromPrevMovements(Issort)
    }
    function CreateRoute() {
        $("#select_route_section").html('');
        $("#select_route_section").hide();
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

        var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        var page = '@Session["pageflag"]';
        var ShowReturnLeg = 0;
        if (page == 3) {
            ApplicationRevId = $('#CRNo').val() ? $('#CRNo').val() : 0;
            ShowReturnLeg= 2
        }
        $("#WichPage").val(page);
        $('#route').show();
        $('#route').html('');
        $('#back_btn_Altered').show();
        $('#back_btn').hide();
        $('#confirm_btn').hide();
        $.ajax({
            url: '../Routes/LibraryRoutePartDetails',

            data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, workflowProcess: 'HaulierApplication', ShowReturnLeg: ShowReturnLeg },
            type: 'GET',
            success: function (page) {
                SubStepFlag = 4.3;
                $('#banner-container').find('div#filters').remove();
                document.getElementById("vehicles").style.filter = "unset";
                $('#route').html('');
                $('#route').append($(page).find('#CreateRoute').html());
                CheckSessionTimeOut();
                Map_size_fit();
                addscroll();
            }
        });
    }
    function ImportFromLibrary(isFav, IsSORT = false, IsPlanMovement = false) {
        //$('#IsRouteAssessment').val(false);
        var isVr1Appln = false;
        if ($('#IsVR1').val() === "true") {
            isVr1Appln = true;
        }

        if (IsSORT) { //sort side integration -- workflow also needs a rewrite
            var dataModelPassed = { importFrm: 'library', isFavourite: isFav, workflowProcess: "HaulierApplication", Iscandidiate: true, NotificationId: $("#NotificationId").val(), ApplicationId: $("#AppRevisionId").val(), IsVr1Appln: isVr1Appln };
            var result = ApplicationRouting(2, dataModelPassed);
            if (result != undefined || result != "NOROUTE") {
                //../Routes/MovementSelectRouteByImport
                LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#select_route_section');
            }
        }
        else {
            var dataModelPassed = { importFrm: 'library', isFavourite: isFav, workflowProcess: "HaulierApplication", NotificationId: $("#NotificationId").val(), ApplicationId: $("#AppRevisionId").val(), IsVr1Appln: isVr1Appln };
            var result = ApplicationRouting(2, dataModelPassed);
            if (result != undefined || result != "NOROUTE") {
                //../Routes/MovementSelectRouteByImport
                LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#select_route_section');
            }
        }
    }
    $('body').on('click', '#select-route', function (e) {
        debugger;
        e.preventDefault();
        var var1 = $(this).data('var1');
        SelectRouteFromPrevMovements(var1);
    });
    function SelectRouteFromPrevMovements(IsSORT,backtopreviouslist) {
        //$('#IsRouteAssessment').val(false);
        var isVr1Appln = false;

        $('#divbtn_prevmove2').hide();
        $('#divbtn_prevmove2').remove();
        if ($('#IsVR1').val() === "true") {
            isVr1Appln = true;
        }
        if (IsSORT) {

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
                //../Routes/MovementSelectRouteByImport
                LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#select_route_section');
            }
            //LoadContentForAjaxCalls("POST", '../Routes/MovementSelectRouteByImport', { importFrm: 'prevMov', workflowProcess: "HaulierApplication" }, '#select_route_section');

        }
    }
    function SelectCurrentMovements() {
       
        var _isvr1Candi = $('#VR1Applciation').val();
        ;
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
                addscroll();
                stopAnimation();
            });
        startAnimation();
        $('#generalDetailDiv').show();
        $("#back_currentmovmnt_vehicle").hide();
        $("#back_currentmovmnt").show();
        if ($("#back_currentmovmnt").length == 0) {           
            $('#generalDetailDiv').append('<div class="button main-button mr-0 col-lg-2" id="back_currentmovmnt"><div class="button main-button mr-0 col-lg-3 pt-5"><button class="btn btn-outline-primary btn-normal" style="position:absolute;width:15%;" role="button" onclick="BindRouteParts()" aria-pressed="true">BACK</button></div></div>');
        }

    }

    function EditRoute(RouteId, RouteName, Routetype) {
        //$("#select_route_section").html('');
        $("#select_route_section").hide();
        if (RouteId != null && RouteId != 0) {
            $('#chkRouteID').val(RouteId);

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
        //function for resize map
        if ($('#ApplicationrevId').val() == undefined) {
            var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        }
        else {
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        }
        var page = '@Session["pageflag"]';
        if (page == 3)
            ApplicationRevId = $('#CRNo').val() ? $('#CRNo').val() : 0;
        $("#WichPage").val(page);
        $('#route').show();
        $('#route').html('');
        $('#back_btn').hide();
        $('#back_btn_Altered').show();
        $('#confirm_btn').hide();
        var isNotification = $('#IsNotification').val();
        var ShowReturnLeg = 0;
        if (isNotification == "true" || isNotification == "True")
            ShowReturnLeg = 2;
        $.ajax({
            url: '../Routes/LibraryRoutePartDetails',
            data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, plannedRouteName: RouteName, plannedRouteId: RouteId, PageFlag: "U", routeType: Routetype, ShowReturnLeg: ShowReturnLeg },
            type: 'GET',
            success: function (page) {
                SubStepFlag = 4.3;
                $('#banner-container').find('div#filters').remove();
                document.getElementById("vehicles").style.filter = "unset";
                $('#route').html('');
                $('#route').append($(page).find('#CreateRoute').html());
                CheckSessionTimeOut();
                Map_size_fit();//, function () {
                addscroll();
            }
        });
    }

    function AutoAssignVehiclesToRoute() {
        var RouteId = (routeIdArr.length == 1) ? routeIdArr[0] : 0;
        var Params = [{ RoutePartId: RouteId, VehicleIds: vehicleIdArr }]
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
                    if ($('#IsNotif').val() == 'true' || $('#IsNotif').val() == 'True') {
                        $('#IsRouteAssessment').val(true);
                        LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess:'HaulierApplication'}, '#route_assessment_section');
                    }
                    else {

                        if ('@planMovePayLoad.IsSupplimentarySaved' == 'True') {
                            LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
                            $('#back_btn').show();
                            $('#save_btn').hide();
                            $('#confirm_btn').show();
                        }
                        else {
                            LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section');
                        }
                    }
                }
            },
            error: function (result) {
            },
            complete: function () {
            }
        });
    }

    function DeleteRoute(RouteId, RouteType,RouteName) {
        routeId = RouteId;
        routeType = RouteType;
        var isReturnRouteAvailable = $('#IsReturnRouteAvailable_Flag').val();
        var IsVehicleAutoAssignedFlag = $('#IsVehicleAutoAssigned_Flag').val();
        var msg = "Do you want to delete the Route?";
        if (((isReturnRouteAvailable == 'True' || isReturnRouteAvailable == 'true') && (IsVehicleAutoAssignedFlag == 'true' || IsVehicleAutoAssignedFlag == 'True') && returnRouteIdArr.length == 1) || ($('#IsAgreedNotify').val() == 'true' || $('#IsAgreedNotify').val() == 'True')) {
            msg = RouteName + " route is attached to a vehicle. Please delete the vehicle in the vehicle tab or reassign it to available routes (if not automatically assigned by the system)."
        }
        ShowWarningPopup(msg, 'DeleteAppRoute');
    }
    function DeleteAppRoute() {
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
                CloseWarningPopup();
                if (response.Success) {
                    ShowSuccessModalPopup('The route deleted successfully.', 'RedirectToRouteList')
                }
                else {
                    ShowErrorPopup('Route delete failed.');
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
        //$('#IsRouteAssessment').val(false);
        $('#IsReturnRouteAvailable_Flag').val(false);
        $('#IsVehicleAutoAssigned_Flag').val(false);
        CloseSuccessModalPopup();
        LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: $('#AppRevisionId').val(), versionId: $('#AppVersionId').val(), contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section');
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
                    ShowSuccessModalPopup('Route added to Library successfully.', 'CloseSuccessModalPopup()');
                }
                else if (response.RouteExist > 0) {
                    ShowErrorPopup('The route name already exist.');
                }
            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    /*functions for sort processing of candidate route modification*/
    var ProjectID = $('#projectid').val();
    var OwnerName = $('#OwnerName').val();
    var plannruserid = $('#PlannrUserId').val();
    var checkingstatus = $('#CheckerStatus').val();
    var _isvr1 = $('#VR1Applciation').val();
    var Enter_BY_SORT = $('#EnterBySort').val();
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
            stopAnimation()
        });
}
function Candi_Candidate_route_version() {
    /* WarningCancelBtn();*/
    startAnimation();
    var wstatus  = $('#hf_PStatus').val(); 
    $('#Candidate_route_version').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + analysis_id + '&Prj_Status=' + encodeURIComponent(wstatus) + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&projectid=' + projectid + '&IsCandPermision=' + '@ViewBag.CandidatePermission', {},
        function () {
            //ClosePopUp();
            //$("#dialogue").html('');
            //$("#dialogue").hide();
            $("#overlay").hide();
            //addscroll();
            stopAnimation();
        });
    }
    $('body').on('click', '#view-map', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('RouteID');
        var RouteName = $(this).data('RouteName');
        ViewMapRoute(RouteID, RouteName);
    });
    $('body').on('click', '#btn-viewmap', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('RouteID');
        var RouteName = $(this).data('RouteName');
        ViewMapRoute(RouteID, RouteName);
    });
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
            $("#route").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
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
        data: { RouteID: RouteId, IsfromImp: true },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
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
            $("#select_route_section").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                //StepFlag = 7;
                SubStepFlag = 4.9;
                $("#map").addClass("context-wrap olMap");
                $('#map').css("height", "590px");
                loadmap('DISPLAYONLY', routeDetails);
                CheckSessionTimeOut();
                Map_size_fit();//, function () {
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
            $("#select_route_section_agredRt").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
             
                $("#map").addClass("context-wrap olMap");
                $('#map').css("height", "590px");
                loadmap('DISPLAYONLY', routeDetails);
                CheckSessionTimeOut();
                Map_size_fit();//, function () {
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

    function AutoAssignVehiclesToRouteAndReturnRoute() {
        
             $.ajax({
                type: 'POST',
                 url: '../Movements/CopyMovementVehicle',
                 data: { movementId: $('#MovementId').val(), vehicleId: @vehicleMoveId, flag: 4, notificationId: $('#NotificationId').val() },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (response) {
                    if (response.data) {
                        $('#IsVehicleAutoAssigned_Flag').val(true);
                        var assignedList = [];
                        for (i = 0; i < routeIdArr.length; i++) {
                            var vehicleIds = [];
                            var RouteId = routeIdArr[i];
                            var vehicleId = response.VehicleIds[i];
                            vehicleIds.push(vehicleId)

                            var assignObj = {};
                            assignObj['RoutePartId'] = RouteId;
                            assignObj['VehicleIds'] = vehicleIds;

                            assignedList.push(assignObj);
                            //console.log('RoutePartId' + RouteId);
                            //console.log('VehicleIds' + vehicleIds);
                        }
                        console.log('assignedList' + assignedList)
                        $.ajax({
                            type: "POST",
                            url: "../Movements/AssignMovementVehicle",
                            data: {
                                vehicleAssignment: assignedList,
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
                                    LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section');
                                }
                            },
                            error: function (result) {
                                alert("failed2");
                            },
                            complete: function () {
                                stopAnimation();
                            }
                        });

                    }
                },
                error: function (result) {
                    alert("failed1");
                },
                complete: function () {
                    stopAnimation();
                }
            });        
    }


