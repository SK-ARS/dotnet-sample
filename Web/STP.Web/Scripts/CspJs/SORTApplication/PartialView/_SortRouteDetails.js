    $(document).ready(function () {
        $("#bindroutes1").on('click', BindRouteParts);
        $("#bindroute").on('click', BindRouteParts);
        $("#create-sort").on('click', CreateSortRoute);
        $("#btn-create-sort").on('click', CreateSortRoute);

    });
    var Rt_Id;
    var Rt_Name;
    function DeleteSortRoute(RouteId, RouteType,RouteNam) {
        routeId = RouteId;
        routeType = RouteType;
        Rt_Id = RouteId;
        Rt_Name = RouteNam;
        if (routeType == "Planned") {
            routeType ="planned"
        }
        ShowWarningPopup("Do you want to delete the Route?", 'CheckRouteVehicleAttach');
    }
    //function for checking route is assossiated with a vehicle
    function CheckRouteVehicleAttach()
    {
        $.ajax({
            url: '../Routes/CheckRouteVehicleAttach',
            type: 'GET',
            cache: false,
            async: false,
            data: { routePartId: Rt_Id},
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (data.success > 0) {
                    ShowInfoPopup('"' + Rt_Name + '" route is attached to a vehicle. Please delete the vehicle in the vehicle tab or reassign it to available routes (if not automatically assigned by the system).','DeleteSortAppRoute');
                }
                else {
                    DeleteSortAppRoute();
                }
                /*commented code if any changes occured in the flow of route vehicle assighnemnt please apply logic here*/
                //if (data.success == 1)
                //{
                //    showWarningPopDialog('"' + Rt_Name + '" route is attached to a vehicle. Please delete the vehicle in the vehicle tab or reassign it to available routes (if not automatically assigned by the system).', 'ok', '', 'Delete_Route_After_Warning', '', 1, 'warning');
                //}
                //else if (data.success == 2)
                //{
                //    showWarningPopDialog('As one route remains in the route tab, all vehicles will be assigned to the available route. Please delete the vehicle if not needed.', 'ok', '', 'Delete_Route_After_Warning', '', 1, 'warning');
                //}
                //else
                //{
                //    Delete_Route_After_Warning();
                //}

            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function DeleteSortAppRoute() {
        CloseWarningPopup();
        CloseInfoPopup('InfoPopup');
        var revisionId = $('#revisionId').val() ? $('#revisionId').val() : 0;
        $.ajax({
            type: "POST",
            url: "../Routes/DeleteApplicationRoute",
            data: {
                routeId: routeId, routeType: routeType, Iscandi: true, revisionId: revisionId
            },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                CloseWarningPopup();
                if (response.Success) {
                    ShowSuccessModalPopup('The route deleted successfully.', 'BindRouteParts');
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
    $('body').on('click', '#btn-editsort', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('RouteID');
        var RouteName = $(this).data('RouteName');
        EditSortRoute(RouteID, RouteName);
    });
    $('body').on('click', '#btn-deletesort', function (e) {
        e.preventDefault();
        var RouteID = $(this).data('RouteID');
        var RouteType = $(this).data('RouteType');
        var RouteName = $(this).data('RouteName');
        DeleteSortRoute(RouteID, RouteType, RouteName);
    });
    function EditSortRoute(RouteId, RouteName) {
        $('#RoutePart').hide(); //hide routedetails
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
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var page = '@Session["pageflag"]';
        $("#WichPage").val(page);
        $('#route').show();
        $('#route').html('');
        $('#back_btn_Rt').show();
        $('#confirm_btn').hide();

        $.ajax({
            url: '../Routes/LibraryRoutePartDetails',
            data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, plannedRouteName: RouteName, plannedRouteId: RouteId, PageFlag: "U", ShowReturnLeg: 0 },
            type: 'GET',
            success: function (page) {
                SubStepFlag = 4.2;
                $('#route').html('');
                $('#route').append($(page).find('#CreateRoute').html());
                CheckSessionTimeOut();
                Map_size_fit();//, function () {
                addscroll();
            }
        });
    }
     function CreateSortRoute() {
         $("#select_route_section").hide();
         $('#RoutePart').hide(); //hide routedetails
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
        $("#WichPage").val(page);
        $('#route').show();
        $('#route').html('');
         $('#back_btn_Rt').show();
        $.ajax({
            url: '../Routes/LibraryRoutePartDetails',

            data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, workflowProcess:'HaulierApplication' },
            type: 'GET',
            success: function (page) {
                SubStepFlag = 4.2;
                $('#route').html('');
                $('#route').append($(page).find('#CreateRoute').html());
                CheckSessionTimeOut();
                Map_size_fit();
                addscroll();
            }
        });
    }
    function SelectPrevtMovementsRoute() {
        $('#ViewRouetDetail').hide();
        $('#divViewRouetDetail').hide();
        $('#App_Candidate_Ver').hide();
        $('#SelectCurrentMovementsVehicle1').hide();
        $('#SelectCurrentMovementsVehicle2').hide();
        $('#SelectCurrentMovementsVehicle').hide();
        $("#IsCreateApplicationRoute").val("true");
        $("#IsPreviousMvmnt").val(1);
        $.ajax({
            url: '../SORTApplication/SORTInbox',
            type: 'POST',
            cache: true,
            data: { structID: null, pageSize: 10, IsPrevtMovementsVehicleRoute: true },
            beforeSend: function () {
                startAnimation('Loading related movements...');
                if ($('#SearchPrevMove').val() == "No") {
                    ClearAdvancedSORT()
                }
            },
            success: function (result) {
                $('#banner-container').find('div#filters').remove();
                document.getElementById("vehicles").style.filter = "unset";
                $('#RoutePart').hide();
                $('#divCurrentMovement').show();

               // $('#divCurrentMovement').html($(result).find('#div_MoveList_advanceSearch').html());
             //   $('#banner-container').find('div#filters').remove();

                //New code
                var importFromDiv = '#div_MoveList_advanceSearch';
                var div = '#divCurrentMovement';
                $(div).html($(result).find(importFromDiv).html(), function () {
                    event.preventDefault();
                });
                $(div).find("form").removeAttr('action', "");
                $(div).find("form").submit(function (event) {
                    event.preventDefault();
                });

                var filters = $(result).find('div#filters');
                $(filters).find('form .filters:first').remove();
                $(filters).find('form .filters:first').remove();
                if ($('#IsSortUser').val() == 'True')
                    $(filters).find('form .filters:last').remove();
                //$(filters).appendTo('#divCurrent');
                $(filters).appendTo('#banner-container');
                $("#Mapfilterdiv").hide();
                removeHrefCandidateLinks();
                PaginateCandidateList();

                //end new code;

                //var filters = $(result).find('div#filters');
                //$(filters).appendTo('#divCurrent');
                $('#divCurrentMovement').append('<div id="divbtn_prevmove1" class="row mt-4" style="float: right;"><button class= "btn outline-btn-primary SOAButtonHelper mr-2 mb-2" id="bindroutes1" >BACK</button></div>');

                CheckSessionTimeOut();
            },
            complete: function () {
                if ($('#SearchPrevMove').val() == "Yes") {
                    ShowAdvanced();

                }
                stopAnimation();
                scrolltotop();
            }
        });


    }
    function SelectCurrentMovementsRoute(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid) {
        
        $('#StruRelatedMove').val("No");
        $("#PrevMove_projectid").val(Vprojectid);
        $("#PrevMove_hauliermnemonic").val(Vhauliermnemonic);
        $("#PrevMove_esdalref").val(Vesdalref);
        $("#IsPrevMoveOpion").val(true);
        $("#back_currentmovmnt").hide();
        $('#RoutePart').hide();
        $('#SelectCurrentMovementsVehicle').show();
        $('#divCurrentMovement').html('');
        WarningCancelBtn();
        startAnimation();
        var wstatus = 'ggg';
        $('#SelectCurrentMovementsVehicle').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + VAnalysisId + '&Prj_Status=' + VPrj_Status + '&hauliermnemonic=' + Vhauliermnemonic + '&esdalref=' + Vesdalref + '&projectid=' + Vprojectid + '&IsCandPermision=' + 'true' + '&IsCurrentMovenet=true', {},
            function () {

                //ClosePopUp();
                $("#dialogue").html('');
                $("#dialogue").hide();
                $("#overlay").hide();
                $('#tab3').show();
                $('#generalDetailDiv').show();
                $('#generalDetailDiv').append('<div id="divbtn_prevmove2" class="mt-4" style="float: right;"><button class= "btn outline-btn-primary SOAButtonHelper mr-2 mb-2" id="select-route" data-var1=true >BACK</button></div>');
                addscroll();
                stopAnimation();
            });
    }

    function removeHrefCandidateLinks() {
        var div = '#divCurrentMovement';
        $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    }

    function PaginateCandidateList() {
        var cntrId = '#divCurrentMovement';
        $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            var pageNum = $(this).html();
            AjaxPaginationForCandidateMovement(pageNum);
        });
        PaginateToLastPagesomovement(cntrId);
        PaginateToFirstPagesomovement(cntrId);
        PaginateToNextPagesomovement(cntrId);
        PaginateToPrevPagesomovement(cntrId);
    }

    function AjaxPaginationForCandidateMovement(pageNum) {
        var data;
        var url;
        var importFromDiv;
        var div = '#divCurrentMovement';
                data = { structID: null, page: pageNum, IsPrevtMovementsVehicleRoute: true };
            url = '../SORTApplication/SORTInbox';
            importFromDiv = '#div_MoveList_advanceSearch';
        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            async: false,
            data: data,
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $('#banner-container').find('div#filters').remove();
                document.getElementById("vehicles").style.filter = "unset";
                $(div).html($(result).find(importFromDiv).html(), function () {
                    event.preventDefault();
                });
                var filters = $(result).find('div#filters');
                $(filters).find('form .filters:first').remove();
                $(filters).find('form .filters:first').remove();
                if ($('#IsSortUser').val() == 'True')
                    $(filters).find('form .filters:last').remove();
                $(filters).appendTo('#banner-container');
                removeHrefCandidateLinks();
                PaginateCandidateList();

                $('#divCurrentMovement').append('<div id="divbtn_prevmove1" class="row mt-4" style="float: right;"><button class= "btn outline-btn-primary SOAButtonHelper mr-2 mb-2" id="bindroute" >BACK</button></div>');
                CheckSessionTimeOut();
            },
            complete: function () {

                if ($('#SearchPrevMove').val() == "Yes") {
                    ShowAdvanced();

                }
                stopAnimation();
                scrolltotop();
            }
        });
    }

    //method to paginate to last page
    function PaginateToLastPagesomovement(ContainerId) {
        $(ContainerId).find('.PagedList-skipToLast').click(function () {
            var pageCount = $('#TotalPages').val();
                AjaxPaginationForCandidateMovement(pageCount);

        });
    }
    // showing filter-settings
    //function openFilters() {
    //    document.getElementById("filters").style.width = "630px";
    //    document.getElementById("banner").style.filter = "brightness(0.5)";
    //    document.getElementById("banner").style.background = "white";
    //    document.getElementById("navbar").style.filter = "brightness(0.5)";
    //    document.getElementById("navbar").style.background = "white";
    //    document.getElementById("divCurrent").style.background = "white";


    //    function myFunction(x) {
    //        if (x.matches) { // If media query matches
    //            document.getElementById("filters").style.width = "200px";
    //        }
    //    }
    //    var x = window.matchMedia("(max-width: 770px)")
    //    myFunction(x) // Call listener function at run time
    //    x.addListener(myFunction)
    //}
    //function closeFilters() {
    //    document.getElementById("filters").style.width = "0";
    //    document.getElementById("banner").style.filter = "unset"
    //    document.getElementById("navbar").style.filter = "unset";

    //}
        // showing filter-settings
    // showing filter-status in side-nav
    function viewStatus() {
        if (document.getElementById('viewstatus').style.display !== "none") {
            document.getElementById('viewstatus').style.display = "none"
            document.getElementById('chevlon-up-icon').style.display = "none"
            document.getElementById('chevlon-down-icon').style.display = "block"
        }
        else {
            document.getElementById('viewstatus').style.display = "block"
            document.getElementById('chevlon-up-icon').style.display = "block"
            document.getElementById('chevlon-down-icon').style.display = "none"
        }
    }
    // showing filter-roads in side-nav
    function viewmovements() {

        if (document.getElementById('viewotheroptions').style.display !== "none") {
            document.getElementById('viewotheroptions').style.display = "none"
            document.getElementById('chevlon-up-icon1').style.display = "none"
            document.getElementById('chevlon-down-icon1').style.display = "block"
        }
        else {
            document.getElementById('viewotheroptions').style.display = "block"
            document.getElementById('chevlon-up-icon1').style.display = "block"
            document.getElementById('chevlon-down-icon1').style.display = "none"
        }
    }
    // showing filter-roads in side-nav
    function viewSORTAdvHaulier() {
       
        if (document.getElementById('viewSORTAdvHaulier').style.display !== "none") {
            document.getElementById('viewSORTAdvHaulier').style.display = "none"
            document.getElementById('chevlon-up-icon2').style.display = "none"
            document.getElementById('chevlon-down-icon2').style.display = "block"
        }
        else {
            document.getElementById('viewSORTAdvHaulier').style.display = "block"
            document.getElementById('chevlon-up-icon2').style.display = "block"
            document.getElementById('chevlon-down-icon2').style.display = "none"
        }
    }
    function viewMapFilter() {
       
            if (document.getElementById('viewMapFilter').style.display !== "none") {
                document.getElementById('viewMapFilter').style.display = "none"
                document.getElementById('chevlon-up-icon4').style.display = "none"
                document.getElementById('chevlon-down-icon4').style.display = "block"
				$("#Mapfilterdiv").hide();
                $("#div_MoveList_MapSearch").hide();
                $("#div_MoveList_advanceSearch").show();
                $.ajax({
                    url: '/SORTApplication/SORTInboxFilter',
                    contentType: 'application/html; charset=utf-8',
                    type: 'GET',
                    dataType: 'html'
                })


            }
            else {
                document.getElementById('viewMapFilter').style.display = "block"
                document.getElementById('chevlon-up-icon4').style.display = "block"
                document.getElementById('chevlon-down-icon4').style.display = "none"



                $("#div_MoveList_MapSearch").show();
				$("#Mapfilterdiv").show();
                   $("#map").html('');
            $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {
                loadmap('SORTMAPFILTER_VIEWANDEDIT');
                // roadOwnerShip_leftPanel();
            });
				if (mapaddresssearch != undefined && mapaddresssearch != "") {
					$("#txtAddressSearch").val(mapaddresssearch);
				}
                $("#div_MoveList_advanceSearch").hide();

              
            }
        }



    //method to paginate to first page
    function PaginateToFirstPagesomovement(ContainerId) {
        $(ContainerId).find('.PagedList-skipToFirst').click(function () {

                AjaxPaginationForCandidateMovement(1);

        });
    }

    //method to paginate to Next page
    function PaginateToNextPagesomovement(ContainerId) {
        $(ContainerId).find('.PagedList-skipToNext').click(function () {
            var thisPage = $(ContainerId).find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;

                AjaxPaginationForCandidateMovement(nextPage);

        });
    }
    //method to paginate to Previous page
    function PaginateToPrevPagesomovement(ContainerId) {
        $(ContainerId).find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $(ContainerId).find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;

            AjaxPaginationForCandidateMovement(prevPage);

        });
    }

    function FilterSuccessdata() {
        startAnimation();
        var div = '#divCurrentMovement';
        var importFromDiv = '#div_MoveList_advanceSearch';

        $.ajax({
            url: '/SORTApplication/SetSORTFilter?IsPrevtMovementsVehicleRoute=' + true,
            type: 'POST',
            cache: false,
            async: false,
            data: $("#FilterMoveInboxSORT").serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $(div).html($(response).find(importFromDiv).html(), function () {
                    event.preventDefault();
                });
                $(div).find("form").removeAttr('action', "");
                $(div).find("form").submit(function (event) {
                    event.preventDefault();
                });

                var filters = $(response).find('div#filters');
                $(filters).find('form .filters:first').remove();
                $(filters).find('form .filters:first').remove();
                $(filters).appendTo('#banner-container');
                removeHrefCandidateLinks();
                PaginateCandidateList();
                closeFilters();
                $("#Mapfilterdiv").hide();
                stopAnimation();
            },
            error: function (xhr, status) {
                location.reload();
            },
            complete: function () {
                mapsearchflag = 0;
                mapsearchtrigger = 0;

            }
        });
    }

    function EnableDisableDatePicker() {
        $.each($("#viewmovements input:checkbox"), function () {
            togglecheckbox($(this));
        });
    }
    function togglecheckbox(_this) {

        if (_this.is(':checked')) {
            _this.closest('.row').find('input:text').attr('disabled', false);
        }
        else {
            _this.closest('.row').find('input:text').attr('disabled', true);
            _this.closest('.row').find('input:text').val("");
        }
    }
    function ClearSORTData() {
        var _advFilter = $('#filter_SORT');
        _advFilter.find('input:text').each(function () {
            $(this).val('');
        });
        _advFilter.find('input:checkbox').prop('checked', false);
        EnableDisableDatePicker();
        ResetDataSORT();
        //location.reload();
    }
    function ResetDataSORT() {
        $.ajax({
            url: '../SORTApplication/ClearInboxAdvancedFilterSORT',
            type: 'POST',
            success: function (data) {
                //ClearAdvancedData();
                FilterSuccessdata();
            }
        });
    }
    function openFilters() {
        var filterWindowWidth = "630px";
        document.getElementById("filters").style.width = filterWindowWidth;
        document.getElementById("divCurrentMovement").style.filter = "brightness(0.5)";
        document.getElementById("divCurrentMovement").style.background = "white";

        function myFunction(x) {
            if (x.matches) { // If media query matches
                document.getElementById("filters").style.width = "200px";
            }
        }
        var x = window.matchMedia("(max-width: 770px)")
        myFunction(x) // Call listener function at run time
        x.addListener(myFunction)
    }
    function closeFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("divCurrentMovement").style.filter = "unset";
    }
