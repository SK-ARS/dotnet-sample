
    var update = false;
    var globalData = [];

    var routePart;

    function addWaypoint(index, routePart) {
        if (routePart.routePathList[index].routePointList.length > 2) {
            for (var i = 2; i < routePart.routePathList[index].routePointList.length; i++) {
                if (routePart.routePathList[index].routePointList[i].pointType == 2) {
                    insertwaypoint(routePart.routePathList[index].routePointList[i].pointType);
                    var wp = "Waypoint" + (i - 1);
                    var waypoint = '#' + wp;
                    $(waypoint).val(routePart.routePathList[index].routePointList[i].pointDescr);
                    //document.getElementById(wp).value = routePart.routePathList[index].routePointList[i].pointDescr;
                }
                else if (routePart.routePathList[index].routePointList[i].pointType == 3) {
                    insertwaypoint(routePart.routePathList[index].routePointList[i].pointType);
                    var wp = "Viapoint" + (i - 1);
                    var viapoint = '#' + wp;
                    $(viapoint).val(routePart.routePathList[index].routePointList[i].pointDescr);
                    //document.getElementById(wp).value = routePart.routePathList[index].routePointList[i].pointDescr;
                }
            }
        }
    }

    $('#btn_showRoute').click(function (e) {
        window.location.reload();
    });

    $('#Outline').click(function (e) {
        $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + 0 + '&routeType=' + 'outline', function () {
            $("#PopupTitale").val('');
            removescroll();
            $("#dialogue").show();
            $("#overlay").show();
            CheckSessionTimeOut();
            $("#PopupTitale").val('Textual description route details');
        });




    });
    function removeprevWaypoint() {
        $('ul#sortable').empty();
    }
    //method for adding values to the select
    function addtoselect(x) {
        routePart = getRouteDetails();

        removeprevWaypoint();
        //$("#From_location").val('');
        //$("#To_location").val('');
        clear_selection();
        if (x != '') {
            $(".account").html(x);
            $(".root").append("<li ><a class='active'>" + x + "</a></li>");


        }
        if (routePart.routePathList.length > 0) {
            $('.dropdown11').show();
            $('#deletepath').show();
        }
    }

    function confirmRemovePath(x) {
        ShowWarningPopup("Are you sure you want to delete the path?", "removepath");
    }

    function removepath(x, enableFlash) {
        close_alert();
        CloseWarningPopup();
        var str = $(".account").html();

        if (str == "Path1") {
            $('.dropdown11').hide();
            $('#deletepath').hide();
            $("#From_location").val(' ');

            $("#To_location").val(' ');
            removeprevWaypoint();
            clearOffRoad();
            clearRoute();
            return;
        }
        else {
            var Cnt = $('.root > li').length;
            $('.root li:last').remove();
            change_select(0, enableFlash);
        }

        var routePathIndex = Number(str.substr(str.length - 1));

        removeRoutePath(routePathIndex);

        $(".account").html(' ');
        $(".account").html('Path1');
        change_select(0, enableFlash);
    }

   // methode for on change of select         ESDAL2022
    $(document.body).on('click', '.root>li', function () {
    //$(".root>li").on("click", function () {
        var num = $('.root > li').length;

        $('.account').html($(this).find('a').html());
        clear_selection();
        $(this).find('a').addClass("active");
        var X = $(".account").attr('id');
        if (X == 1) { $(".route-label").css("color", "White"); $(".submenu").hide(); $(".account").attr('id', '0'); }
        else { $(".route-label").css("color", "black"); $(".submenu").show(); $(".account").attr('id', '1'); }
        if (num == 1)
            return;
        change_select($(this).index());
    });

    function RoutePathClick() {
        var num = $('.root > li').length;

        $('.account').html($(this).find('a').html());
        clear_selection();
        $(this).find('a').addClass("active");
        var X = $(".account").attr('id');
        if (X == 1) { $(".route-label").css("color", "White"); $(".submenu").hide(); $(".account").attr('id', '0'); }
        else { $(".route-label").css("color", "black"); $(".submenu").show(); $(".account").attr('id', '1'); }
        if (num == 1)
            return;
        change_select(1);
    }

    //method for chage event of select
    function change_select(x, enableFlash) {
        deactivateToolbarControls();
        if (enableFlash != 0) {
            flashRoute(x);
        }
        index = x;
        routePart = getRouteDetails();
        var ptcount = routePart.routePathList[index].routePointList.length;

        $("#From_location").val(routePart.routePathList[index].routePointList[0].pointDescr);

        $("#To_location").val(routePart.routePathList[index].routePointList[1].pointDescr);

        removeprevWaypoint();
        addWaypoint(index, routePart);
        setCurrentActiveRoutePath(index);

        clearselect();

    }

    //method for clear selection
    function clear_selection() {
        $('.root>li>a').removeClass("active");
    }
    //method for updatig waypoints in leftpanel after dragging a route point marker
    function updatewaypoints() {
        removeprevWaypoint();
        addWaypoint(0, routePart);
    }
    //method for clearing values from the select
    function clearselect() {
        routePart = getRouteDetails();
        if (routePart.routePathList.length < 2) {
            $(".account").html('Path1');
            $(".root").html('');
            $(".root").append("<li ><a class='active'>Path1</a></li>");
            $('.dropdown11').hide();
            $('#deletepath').hide();
            $('#spn_WayPoint').hide();
            $("#spn_ViaPoint").hide();
        }
    }
    function btnBackClick()
    {
        location.href = '@Url.Content("/Routes/RoutePartLibrary")';
    }

    $('#Outline_Save').click(function (e) {
        $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + 0 + '&routeType=' + 'outline', function () {

            removescroll();
            $("#dialogue").show();
            $("#overlay").show();
            CheckSessionTimeOut();
        });


        $("#dialogue").show();
        $("#overlay").show();

        $("#PopupTitale").val('Start and end point only route details');
    });

    $('#btn_updateRoute').click(function (e) {
        deactivateToolbarControls();
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var routePart = getRouteDetails();
        var isBrokenAnnot = [];//isBroken = 0;
        var is_NEN = false; // This flag is used for identifying NEN
        if (typeof $("#hIs_NEN").val() != "undefined" && $("#hIs_NEN").val() == "true") {
            is_NEN = true;
        }
        if (objifxStpMap.pageType == "ROUTELIBRARY" && is_NEN == false) {
            isBrokenAnnot = CheckIsBroken(0, 0, 0, routePart.routePartDetails.routeID, 0, 0);//isBroken = CheckIsBrokenAnnot(routePart.routePartDetails.routeID);
        }
        else {
            isBrokenAnnot = CheckIsBroken(routePart.routePartDetails.routeID, 0, 0, 0, 0, 0);//getting broken routes or not by passing route_Id
        }
        var routeID = 0;
        if (routePart != null) {

            if ('@Session["RouteFlag"]' == "4" && String(routePart.routePartDetails.routeID) == "undefined") {
                routeID = $('#hfCandidatRouteID').val();
                routePart.routePartDetails.routeID = routeID;
                routePart.routePartDetails.routeDescr = $('#hfCandidatDescription').val();
                routePart.routePartDetails.routeName = $('#hfCandidatName').val();

                routePart.routePartDetails.routeType = 'planned';
            }
            else {
                routeID = routePart.routePartDetails.routeID;
                if (routePart.routePartDetails.routeType != 'planned')
                    routePart.routePartDetails.routeType = 'outline';
            }
        }

        if (routePart.routePartDetails.routeType != 'undefined' && routePart.routePartDetails.routeType == 'outline') {
            $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + routeID + '&PageFlag=' + 'updateOutline' + '&routeType=' + 'outline&ApplicationRevId=' + ApplicationRevId, function () {
                removescroll();

                $("#dialogue").show();
                $("#overlay").show();
                CheckSessionTimeOut();

                return;
            });
            return;
        }
        var RouteFlag  = $('#hf_RouteFlag').val(); 
        var IsbrokenAnnotation = 0;
        if (isBrokenAnnot.length>0) {
            IsbrokenAnnotation = isBrokenAnnot[0].isBroken;
        }
        //var IsbrokenAnnotation = isBrokenAnnot[0].isBroken; //setting isbroken flag for libraray routes only
        if (IsbrokenAnnotation > 0 && routePart.Esdal2Broken == 0 ) {//blocking broken routes without replanning
            var msg = 'The route you are trying to save hasn'+ "'" +'t been re-planned. Please re-plan it before saving.';
            showWarningPopDialog(msg, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }

        else {


            var link = '@Url.Action("enterRouteDetail", "Routes")?mode=U&routeFlag=_RouteFlag&origin=A2Bleft';

            $("#dialogue").load(link.replace("_RouteFlag", RouteFlag), function () {
                globalData = DataAvailable();
                if ($('#hNEN_Id').val() != undefined) { $('#span-help').hide(); }//Added for NEN
            });
            $("#overlay").show();
            DynamicTitle('Route Details');
            $("#dialogue").show();
            $("#dialogue").draggable({ handle: ".head" });
            removescroll();
            update = true;
        }
    });
    $('#btn_details').click(function (e) {
        var RouteFlag  = $('#hf_RouteFlag').val(); 
        var link = '@Url.Action("enterRouteDetail", "Routes", new { mode = "U", routeFlag = "_RouteFlag" })';
        var link = '@Url.Action("enterRouteDetail", "Routes")?mode=U&routeFlag=_RouteFlag&origin=A2Bleft';
        $("#dialogue").load(link.replace("_RouteFlag", RouteFlag), function () {
            CheckSessionTimeOut();
            DisableText();
            if ($('#hNEN_Id').val() != undefined) { $('#span-help').hide(); }//Added for NEN project
        });
        $("#overlay").show();
        DynamicTitle('Route Details');
        Resize_PopUp(440);
        $("#dialogue").show();
        $("#dialogue").draggable({ handle: ".head" });
        removescroll();
        //$("#MainDiv").css("pointer-events", "none");
        update = false;
    });
    $('#btn_saveRoute').click(function (e) {
        deactivateToolbarControls();
        var RouteFlag  = $('#hf_RouteFlag').val(); 

        var link = '@Url.Action("enterRouteDetail", "Routes")?mode=S&routeFlag=_RouteFlag&origin=A2Bleft';
        $("#dialogue").load(link.replace("_RouteFlag", RouteFlag), function () {
            CheckSessionTimeOut();
        });
        $("#overlay").show();
        DynamicTitle('Route Details');
        Resize_PopUp(440);
        $("#dialogue").show();
        $("#dialogue").draggable({ handle: ".head" });

        $("#PopupTitale").val('Planned route details');
        removescroll();
    });
    function DynamicTitle(title) {
        $('.dyntitleConfig').html(title);
    }
    $(document).ready(function () {
        //----------------$(document).ready(function () { external script load function start here }-----------
if($('#hf_IsAffectedStructure').val() ==  'False') {
            RouteDetailsReady('@ViewBag.ShowReturnLegFlag');
            WaypointReady();
            //----------------$(document).ready(function () { external script load function end here }-------------
        }
        else {
            ShowAgreedApplStructureOnMap();
        }

        $('#div_saving').hide();
        $('.dropdown11').hide();
        $('#deletepath').hide();
        $("#spn_WayPoint").click(function () {
            // addWaypointRow();
            insertwaypoint(2);

});
        $("#spn_ViaPoint").click(function () {
            insertwaypoint(3);
        });


        $('.ui-state-default').sortable();
        $(".account").click(function () {
            var X = $(this).attr('id');
            if (X == 1) {
                $(".route-label").css("color", "white");
                $(".submenu").hide(); $(this).attr('id', '0');
            }
            else {
                $(".route-label").css("color", "black");
                $(".submenu").show(); $(this).attr('id', '1');
            }
        });
        $("#ddlRt").click(function () {
            var X = $(this).attr('id');
            if (X == 1) { $(".route-label").css("color", "white");
                        $(".submenu").hide(); $(this).attr('id', '0'); }
            else {
                $(".route-label").css("color", "black");
                $(".submenu").show(); $(this).attr('id', '1');
            }
        });

        //Mouse click on sub menu
        $(".submenu").mouseup(function () {
            return false
        });

        //Mouse click on my account link
        $(".account").mouseup(function () {
            return false
        });
        //Document Click
        $(document).mouseup(function ()
        {
            $(".route-label").css("color", "white");
            $(".submenu").hide(); $(".account").attr('id', '');
        });

        //$('body').live('click', function () { $('.route_search_popup').hide(); });  
        if ('@Session["RouteFlag"]' == "4") {
            //$('#btn_plan').show();
            $('#btn_re_plan').hide();
        }

        $('#btn_back').click(function () {
            btnBackClick();
        });
        if ( typeof $("#hIs_NEN").val() === "undefined") {
            $("#btn_hualierdetails").hide();
        }
        else if ($("#hIs_NEN").val() == "true" ) {
            $("#btnCreate").hide();
            $("#btn_hualierdetails").show();
        }



        $(function () { $('#divContainer').draggable(); });

        if (typeof $("#hIs_NEN").val() != "undefined" && $("#hIs_NEN").val() == "true") {
            $('#info2NEN').show();
            $('#info1NEN').show();

            $('#info3ESDAL').hide();
            $('#info2ESDAL').hide();
            $('#info1ESDAL').hide();
        }
        else {
            $('#info2NEN').hide();
            $('#info1NEN').hide();

            $('#info3ESDAL').show();
            $('#info2ESDAL').show();
            $('#info1ESDAL').show();
        }
    });

        function addWaypointRow(type) {

            var rowCnt = $('#tbl_Waypoint tr').length;
            var rowID;
            var gblstring = "";
            if (rowCnt <= 99) {
                if (rowCnt == 0) {
                    rowID = 0;
                }
                else {
                    rowID = $('#tbl_Waypoint tr:last').attr('id');
                }

                if (rowCnt == 99) {
                    $("#spn_WayPoint").hide();
                    $("#spn_ViaPoint").hide();
                }
                var name = document.getElementsByName("lblwaypoint");
                rowID = (name.length) + 1;
                var lblRow = "Waypoint" + (rowID);
                var tooltip = "Waypoint #" + (rowID);
                if (type == null || type == 1 || type == 2) {
                    var str = "<tr id=" + rowID + " name='trwaypoint'><td style='display:none;'><span name='lblwaypoint'>" + lblRow + "</span></td> <td class='searchtableleft editor-field'><span class='waypointicon' name='waypointicon'>" + rowID + "</span><input class='serchlefttxt1one AutocompleteElement' url='../QASController/Search' pointNo = '" + lblRow + "' title='" + tooltip + "' type='text' id='" + lblRow + "' name='txtwaypoint' /> <div class='canwaypoint' onclick='deleteWaypoint(this);'></div> <div class='swapicon'  onclick='swap(" + rowID + ");'></div> </tr>";
                }
                else if (type == 3) {
                    str = "<tr id=" + rowID + " name='trwaypoint'><td style='display:none;'><span name='lblwaypoint'>" + lblRow + "</span></td> <td class='searchtableleft editor-field'><span class='waypointicon_green' name='waypointicon'>" + rowID + "</span><input class='serchlefttxt1one AutocompleteElement' url='../QASController/Search' pointNo ='" + lblRow + "' title='" + tooltip + "' type='text' id='" + lblRow + "' name='txtwaypoint' /> <div class='canwaypoint' onclick='deleteWaypoint(this);'></div> <div class='swapicon'  onclick='swap(" + rowID + ");'></div> </tr>";
                }
                $("#tbl_Waypoint").append(str);
                $('.swapicon').show();
                $('.swapicon').last().hide();

            }

        }

        function swap(x) {
            swapStartEnd();
        }

        function deleteWaypoint(a) {
            var tr = $(a).closest('tr');
            tr.remove();
            $("#spn_WayPoint").show();
            $("#spn_ViaPoint").show();
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
                txtname[i].title = "Waypoint #" + (i + 1);
            }
            deleteViaMarker($(tr).attr("id") - 1);
            $('.swapicon').last().hide();
        }
        function DisableText() {
            $('#tbl_route_details').find('input:text').attr('disabled', true);
            $('#tbl_route_details').find('input:checkbox').attr('disabled', true);
            $('#tbl_route_details').find('#div_save').hide();
            $('#tbl_route_details').find('#routetype').hide();
        }

        function ClosePopUp() {

            if (update) {
                showWarningPopDialog('Do you want to close?', 'No', 'Yes', 'WarningCancelBtn', 'close_alert', 1, 'warning');
            }
            else {
                ReloadLocation();
            }
            addscroll();
        }
        function close_alert() {
            $("#dialogue").hide();
            $("#pop-warning").hide();
            $("#overlay").hide();
        }
        function DataAvailable() {
            var data = [];
            $('#tbl_route_details').find('input:text').each(function () {
                var _thisValue = $(this).val();
                data.push(_thisValue);
            });
            return data;
        }
        function CheckForChange() {
            var newData = DataAvailable();
            $.each(newData, function () {
                var index = $(this).val();
                console.log(index);
            });
        }

        //for Aoutocomplete
        $(document).ready(function () {

        })



    $('#btnCreate').click(function (e) {
        location.href = '@Url.Content("/Routes/LibraryRoutePartDetails")';
    });

    function showSavedRouteDetails(routeID, routeType, routeName, flag) {
        if (routeType == "outline") {
            $.ajax({
                type: 'POST',
                dataType: 'json',

                url: '../Routes/GetPlannedRoute',
                data: { RouteID: routeID },


                beforeSend: function (xhr) {

                },
                success: function (result) {
                    routePath = result.result;

                    var IsMap = true;
                    if (String(routePath) == "undefined" && routePath.routePathList[0].routePointList[0].pointGeom == null) {
                        IsMap = false;
                    }
                    else if (routePath != "undefined" && routePath != null) {
                        if (routePath.routePathList[0].routePointList[0].pointGeom == null)
                            IsMap = false;
                    }

                    if (IsMap) {
                        window.location = '../Routes/LibraryRoutePartDetails' + EncodedQueryString('plannedRouteId=' + routeID + '&routeType=' + routeType + '&plannedRouteName=' + routeName + '&PageFlag=' + "U");
                        CheckSessionTimeOut();
                    }
                    else {
                        var IsTextualRoute = false;
                        var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                        for (var i = 0 ; i < count; i++) {
                            if (result.result.routePathList[0].routePointList[i].pointType != 0 || result.result.routePathList[0].routePointList[i].pointType != 1) {
                                if (result.result.routePathList[0].routePointList[i].pointDescr == "")
                                    IsTextualRoute = true;
                            }
                        }
                        $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + routeID + '&routeType=' + routeType + '&IsTextualRouteType=' + IsTextualRoute, function () {
                            removescroll();
                            $("#dialogue").show();
                            $("#overlay").show();
                            CheckSessionTimeOut();
                        });

                        $("#dialogue").show();
                        $("#overlay").show();
                    }


                },
            });
        }

        else {
            window.location = '../Routes/LibraryRoutePartDetails' + EncodedQueryString('plannedRouteId=' + routeID + '&routeType=' + routeType + '&plannedRouteName=' + routeName + '&PageFlag=' + "U");

            //var Vfrom = $('#From_location').val(), Vto = $('#To_location').val();
            //$("#pageheader").find("h3").text(routeName + "       " + Vfrom + "       " + "to " + "     " + Vto);
            //$("#pageheader").show();
            //applysize();
        }
    }
    // Search

    function FillNormalFilter() {
        $('#SearchPrevMoveVeh').val("Yes");
        $('#so_movement_filter').find('input:text').each(function () {
            var id = $(this).attr('id');
            var value = $(this).val();

            $('#div_normal_hidden').append('<input type="hidden" id="' + id + '" name="' + id + '" value="' + value + '"></hidden>');
        });

        $('#so_movement_filter').find('input:checkbox').each(function () {
            var id = $(this).attr('id');
            var value = $(this).is(':checked');

            $('#div_normal_hidden').append('<input type="hidden" id="' + id + '" name="' + id + '" value="' + value + '"></hidden>');
        });

    }
    function ClearAdvancedSORT() {
        ClearAdvancedDataSORT();
        ResetDataSORT();
        $("#frmFilterMoveInboxSORT").submit();

        return false;
    }
    function ClearAdvancedDataSORT() {
        var _advInboxFilter = $('#div_sort_adv_search');
        _advInboxFilter.find('input:text').each(function () {
            $(this).val('');
        });

        _advInboxFilter.find('select').each(function () {
            $(this).val('0');
        });
    }

    function ResetDataSORT() {
        $.ajax({
            url: '../SORTApplication/ClearInboxAdvancedFilterSORT',
            type: 'POST',
            success: function (data) {
                //ClearAdvancedData();
            }
        });
    }

    function SelectPrevtMovementsVehicle() {
        var structureId = currentMouseOverFeature.data.name.match("STRUCTURE (.*) NAME");
        var url = '@Url.Action("SORTInbox","SORTApplication")';
        $.ajax({
            type: 'POST',
            cache: false,
            url: '../SORTApplication/SORTInbox',
            data: { structID: structureId[1], pageSize: 10, IsPrevtMovementsVehicle: false },
            beforeSend: function () {
                startAnimation('Loading related movements...');
                if ($('#SearchPrevMoveVeh').val() == "No") {
                  //  ClearAdvancedSORT()
                }

                $('#RoutePart').hide();
            },
            success: function (result) {
                $('#StruRelatedMov').show();
                $(".slidingpanelstructures").removeClass("show").addClass("hide");
                $('#StruRelatedMov').html($(result).find('#div_MoveList_advanceSearch').html());

                $('#StruRelatedMov').append("<button type='button' class='btn_reg_back next btngrad btnrds btnbdr' onclick='BackTomap()'>Back</button>");
                CheckSessionTimeOut();
                $('#leftpanel').hide();
            },
            complete: function () {
                if ($('#SearchPrevMoveVeh').val() == "Yes") {
                    ShowAdvanced();

                }
                stopAnimation();
            }
        });
    }
    function replanRoute() { //function for replanning button param Islibrary: true means the current route is libarary route if it is false: The route type will get from user schema
        var route_id = $('#RouteID').val();
        var replan = false;
        var result = [];
        var routePart;
        startAnimation('Replanning in Progress');
        setTimeout(function () {
if($('#hf_IsLibrary').val() ==  'True') {
                result = ReplanBrokenRoutes(route_id, 1, true);
            }
            else {
                result = ReplanBrokenRoutes(route_id, 0, true);
            }
            replan = result.result;
            if (replan) {
                routePart = result.routePart;
                clearRouteReplan();
                selectRoute(routePart);
                //addWaypoint(0, routePart); //commented code for shoiwng multple waypoint issues
                routePart.Esdal2Broken = 1;//indication this field is a broken and auto replanned
                routePart.IsAutoReplan = 1;//set 1 for auto replanned route using replan button
            }
            else {
                var msg = 'No route can be planned. Which may be due to legal restrictions on the route. Please change the start/end/way points and plan the route.';
                showWarningPopDialogBig(msg , 'Ok', '', 'close_alert', '', 1, 'info');
            }
            $('#btn_re_plan').hide();
            stopAnimation();
        }, 100);
    }
