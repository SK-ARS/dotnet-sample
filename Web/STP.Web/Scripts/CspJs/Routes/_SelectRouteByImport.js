    $(document).ready(function () {
        StepFlag = 4;
        SubStepFlag = 4.1;
        CurrentStep = "Route Details";
        SelectMenu(2);
        SetWorkflowProgress(4);
        $('#confirm_btn').hide();
        $('#back_btn').show();
        $('#IsVehicle').val(false);
        $('#ImportFrom').val('@ViewBag.ImportFrm');
        $('#IsFavouriteRoute').val('@ViewBag.IsFavourite');

if($('#hf_ImportFrm').val() ==  'library') {
            $('#list_heading').text("Select route from library");
            SelectRouteFromLibrary();
        }
if($('#hf_ImportFrm').val() ==  'prevMov') {
            
            $('#IsVehicle').val(false);
            $('#list_heading').text("Select route from previous movements");
            UseMovement($('#IsSortUser').val(), '@ViewBag.BackToMovementPreviousList');
        }
    });

    @*function SelectRouteFromLibrary() {
        ;
        //var isApplicationVehicle  = $('#hf_isApplicationVehicle' == 'True').val(); 
        var isApplicationVehicle = false;
        var IsFavourite  = $('#hf_IsFavourite').val(); 
        $.ajax({
            url: '../Routes/RoutePartLibrary',
            data: { importFlag: true, filterFavouritesRoutes: IsFavourite},
            type: 'POST',
            async: false,
            beforeSend: function () {
                //startAnimation();
            },
            success: function (response) {
                ;
                $('#route_importlist_cntr').html($(response).find('#route-list'), function () {
                });
                $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from library</span>');

                $('#route-list').find('.text-color2').removeAttr('href').css("cursor", "pointer");

                removeHrefLinks();
                PaginateList();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                //stopAnimation();
            }
        });
    }*@
    //function removeHrefLinks() {
    //    ;
    //    $('#route_importlist_cntr').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    //}
    @*function PaginateList() {
        ;
        //method to paginate through page numbers
        $('#route_importlist_cntr').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            //var pageCount = $('#TotalPages').val();
            var pageNum = $(this).html();
if($('#hf_ImportFrm').val() ==  'library') {
                AjaxPaginationforSORoute(pageNum);
            }
if($('#hf_ImportFrm').val() ==  'prevMov') {

            }
        });
        PaginateToLastPagesomovement();
        PaginateToFirstPagesomovement();
        PaginateToNextPagesomovement();
        PaginateToPrevPagesomovement();
    }*@
    //method to paginate to last page
    @*function PaginateToLastPagesomovement() {
        $('#route_importlist_cntr').find('.PagedList-skipToLast').click(function () {
            var pageCount = $('#TotalPages').val();
            //AjaxPaginationForMovement(pageCount);
if($('#hf_ImportFrm').val() ==  'library') {
                AjaxPaginationforSORoute(pageCount);
            }
if($('#hf_ImportFrm').val() ==  'prevMov') {

            }
        });
    }*@

    //method to paginate to first page
    @*function PaginateToFirstPagesomovement() {
        $('#route_importlist_cntr').find('.PagedList-skipToFirst').click(function () {
if($('#hf_ImportFrm').val() ==  'library') {
                AjaxPaginationforSORoute(1);
            }
if($('#hf_importFrm').val() ==  'prevMov') {

            }
        });
    }*@

    //method to paginate to Next page
    @*function PaginateToNextPagesomovement() {
        $('#route_importlist_cntr').find('.PagedList-skipToNext').click(function () {
            var thisPage = $('#route_importlist_cntr').find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
if($('#hf_ImportFrm').val() ==  'library') {
                AjaxPaginationforSORoute(nextPage);
            }
if($('#hf_importFrm').val() ==  'prevMov') {

            }
        });
    }*@
    //method to paginate to Previous page
    @*function PaginateToPrevPagesomovement() {
        $('#route_importlist_cntr').find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $('#route_importlist_cntr').find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
if($('#hf_ImportFrm').val() ==  'library') {
                AjaxPaginationforSORoute(prevPage);
            }
if($('#hf_importFrm').val() ==  'prevMov') {

            }
        });
    }*@
    //function AjaxPaginationforSORoute(pageNum) {
    //    ;
    //    var SearchString = $('#SearchString').val();
    //    var isVR1 = $('#vr1appln').val();
    //    if (isVR1 == "True") {
    //        var routeType = 2;
    //    }
    //    else {
    //        var routeType = 1;
    //    }
    //    var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;

    //    var selectedVal = $('#pageSizeVal').val();
    //    var pageSize = selectedVal;
    //    $.ajax({
    //        url: '../Routes/RoutePartLibrary',
    //        type: 'GET',
    //        cache: false,
    //        async: false,
    //        data: { SearchString: SearchString, page: pageNum, pageSize: pageSize, importFlag: true },
    //        beforeSend: function () {
    //            startAnimation();
    //        },
    //        success: function (response) {
    //            ;
    //            $('#route_importlist_cntr').html($(response).find('#route-list'), function () {
    //            });
    //            $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from library</span>');

    //            $('#route-list').find('.text-color2').removeAttr('href').css("cursor", "pointer");

    //            removeHrefLinks();
    //            PaginateList();

    //        },
    //        error: function (xhr, textStatus, errorThrown) {
    //            //other stuff
    //            //location.reload();
    //        },
    //        complete: function () {
    //            stopAnimation();
    //        }
    //    });
    //    ;
    //}

    function ImportRouteInAppLibrary(routeID, routetype) {
        
        var routename = $('#btnrouteimport_' + routeID).data('name');
        var routeType = "PLANNED";
        var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        $('#IsRouteModify').val(1);
        $('#IsReturnRoute').val(0);
        $('#IsReturnRouteAvailable_Flag').val(false);
        if ($('#CRNo').val() == undefined) {
            var vr1contrefno = $('#VR1ContentRefNo').val();
        }
        else {
            var vr1contrefno = $('#CRNo').val();// $('#VR1ContentRefNo').val();
        }
        var vr1versionid = $('#AppVersionId').val();

        $.ajax({
            url: '../Routes/ImportRouteFromLibrary',
            type: 'POST',
            async: true,
            cache: false,
            data: { routepartId: routeID, routetype: routetype, AppRevId: AppRevId, CONTENT_REF: vr1contrefno, VersionId: vr1versionid },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {              
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { apprevisionId: AppRevId, versionId: vr1versionid, contRefNum: $('#CRNo').val(), isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val(), workflowProcess: 'HaulierApplication' }, '#select_route_section');
            },
            error: function (xhr, textStatus, errorThrown) {                
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function ImportRouteInAppParts(RouteId, RouteType) {
		if (RouteType != 'planned') {
			ShowErrorPopup('An outline route cannot be imported to the movement.');
        }
        else {
            $('#IsRouteModify').val(1);
            $('#IsReturnRoute').val(0);
            $('#IsReturnRouteAvailable_Flag').val(false);
        var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        var SOVersionID = $('#RevisionID').val() ? $('#RevisionID').val() : 0; //Previous Movement version id
        var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
        var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRT").val();
        //added by poonam (14.8.14)
        var vr1contrefno = $('#CRNo').val() ? $('#CRNo').val() : null;
        var vr1versionid = $('#AppVersionId').val();
        //-----------
            
        $.ajax({
            url: '../Routes/ImportRouteFromPrevious',
            type: 'POST',
            async: false,
            cache: false,
            data: { routepartId: RouteId, routeType: RouteType, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                //if (result != 0) {
                //    var is_replan = 0;
                //    is_replan = CheckIsBroken(result, 0, 0, 0, 0, 0);
                //    if (is_replan[0].isBroken > 0) { //check in the existing route is broken
                //        var msg = "";
                //        if (is_replan[0].isReplan > 1) {
                //            $('.popup111 .message111').css({ 'height': '183px' });
                //            $('.popup111').css({ 'height': '221px' });
                //            msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                //        }
                //        else {
                //            replan = ReplanBrokenRoutes(is_replan[0].plannedRouteId, 0, false);
                //            if (replan) {
                //                msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.';
                //            }
                //            else {
                //                $('.popup111 .message111').css({ 'height': '183px' });
                //                $('.popup111').css({ 'height': '221px' });
                //                msg = 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re - plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                //            }
                //        }
                //        showWarningPopDialogBig(msg, 'Ok', '', 'SelectedRouteFromLibraryForVR1', '', 1, 'info');
                //    }
                //    else {
                //        showWarningPopDialog('"' + routename + '" route imported for this application', 'Ok', '', 'SelectedRouteFromLibraryForVR1', '', 1, 'info');
                //    }
                //    $("#RoutePart").show();
                //    $("#ShowDetail").hide();
                //    $("#RouteMap").html('');
                //    if ($('#UserTitle').html() == "SORT Portal")
                //        CloneRoutesSort();
                //    else
                //        CloneRoutes();

                //}
                $('#back_btn').show();
                $('#back_btn_Rt_prv').hide();
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { workflowProcess: 'HaulierApplication', apprevisionId: AppRevId, versionId: vr1versionid, contRefNum: vr1contrefno, isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                //location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    }




    @*function UseMovement() {
        alert('UseMovement');
        var isVR1 = $('#vr1appln').val();
        var VIsRoutePrevMoveOpion = false;

        if ($("#IsPrevMoveOpion").val() == "routeMoveOpion")
            VIsRoutePrevMoveOpion = true;
        $('#TabOption').hide();
        if ('@Session["RouteFlag"]' == 3)
        {
            $("#IsSOroute").val(false);
            $("#IsSOvehicle").val(false);
            $("#IsVR1PreMovroute").val(false);
            $("#IsNotifPreMovRoute").val(true);
            $("#IsNotifyRouteTab").val("YES");

            $.ajax({
                url: '../Movements/MovementList',
                data: { MovementListForSO: false, IsNotify: true, VR1route: false, IsNotifyRoute: true},
                type: 'GET',
                beforeSend: function () {
                    startAnimation();
                },
                success: function (page) {
                    $('#tab_4').show();
                    $('#RoutePart').show();
                    $('#RoutePart').html('');
                    $('#RoutePart').html($(page).find('#divforsofilter').html(), function () {
                    });
                    $('#RoutePart').find('h4').text('Route from previous movement ');
                    $('#RoutePart').find("form").removeAttr('action', "");
                    $('#RoutePart').find("form").submit(function (event) {
                        AdvSearchforApplivation();
                        event.preventDefault();
                    });
                    removeroutemovementHLinks();
                    PaginateGridvrmovement();
                    fillPageSizeSelect();
                },
                error: function (xhr, textStatus, errorThrown) {
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
        else if (isVR1 == 'True') {
            $("#IsSOvehicle").val(false);
            $("#IsSOroute").val(false);
            $("#IsVR1PreMovroute").val(true);
            $("#IsNotifPreMovRoute").val(false);

            $.ajax({
                url: '../Movements/MovementList',
                data: { VR1route: true, MovementListForSO: false, showrtveh: 2, IsNotify: false, IsVR1PreMovroute: true },
                type: 'GET',
                beforeSend: function () {
                    startAnimation();
                },
                success: function (page) {
                    $('#tab_4').show();
                    $('#RoutePart').show();
                    $('#RoutePart').html('');
                    $('#RoutePart').html($(page).find('#divforsofilter').html(), function () {
                    });
                    $('#RoutePart').find('h4').text('Route from previous movement ');
                    $('#RoutePart').find("form").removeAttr('action', "");
                    $('#RoutePart').find("form").submit(function (event) {
                        AdvSearchforApplivation();
                        event.preventDefault();
                    });
                    removeroutemovementHLinks();
                    PaginateGridvrmovement();
                    fillPageSizeSelect();
                },
                error: function (xhr, textStatus, errorThrown) {
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
        else {
            alert('else');
            ;
            $("#IsSOvehicle").val(false);
            $("#IsSOroute").val(true);
            $("#IsVR1PreMovroute").val(false);
            $("#IsNotifPreMovRoute").val(false);
            $.ajax({
                url: '../Movements/MovementList',
                data: { MovementListForSO: true, showrtveh: 2, IsSOroute: true, IsRoutePrevMoveOpion: VIsRoutePrevMoveOpion },
                type: 'GET',
                beforeSend: function () {
                    startAnimation();
                },
                success: function (page) {

                    $('#route_importlist_cntr').html($(page).find('.div_so_movement').html(), function () {
                    });
                    $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from previous movement</span>');

                    $('#route_importlist_cntr').find("form").removeAttr('action', "");
                    //$('#route_importlist_cntr').find("form").submit(function (event) {
                    //    //AdvSearchforApplivation();
                    //    //event.preventDefault();
                    //});
                    //removeroutemovementHLinks();
                    //PaginateGridvrmovement();
                    //fillPageSizeSelect();
                },
                error: function (xhr, textStatus, errorThrown) {
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
    }*@

    @*function SelectRouteFromPrevMovements()
    {
        $("#IsPrevMoveOpion").val("routeMoveOpion");
        $("#IsCreateApplicationRoute").val("true");

        $("#IsSOvehicle").val(false);
        $("#IsSOroute").val(true);
        $("#IsVR1PreMovroute").val(false);
        $("#IsNotifPreMovRoute").val(false);
if($('#hf_IsSort').val() ==  'True') {
            SelectPrevtMovementsRoute();
        }
        else {
            UseMovement();
        }
    }*@

