    $(document).ready(function () {
        StepFlag = 1;
        SubStepFlag = 1.1;
        CurrentStep = "Select Vehicle";
        SetWorkflowProgress(1);
        $('#ImportFrom').val('@ViewBag.importFrm');
        $('#IsVehicle').val(true);
        $('#confirm_btn').hide();

if($('#hf_importFrm').val() ==  'fleet') {
            $('#list_heading').text("Select vehicle from fleet");
            SelectVehicleFromFleet();
        }
if($('#hf_importFrm').val() ==  'prevMov') {
            $('#list_heading').text("Select vehicle from previous movements");
            UseMovement($('#IsSortUser').val(),'@ViewBag.backToPreviousList');
        }

    });
    $(document).on("click", ".ssort", function () {
        //...
        MovementSort(event);
    });
    @*function SelectVehicleFromFleet() {
       var  isApplicationVehicle  = $('#hf_isApplicationVehicle' == 'True').val(); 
        $.ajax({
            type: "POST",
            url: '../VehicleConfig/VehicleConfigList',
            data: { importFlag: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
                });
                $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');

                var filters = $(response).find('div#filters');
                $(filters).appendTo('#banner-container');

                removeHrefLinks();
                PaginateList();
                fillPageSizeSelect();

                stopAnimation();
            },
            error: function (result) {
                alert("failed");
            },
            complete: function () {

            }
        });
    }*@
    function removeHrefLinks() {
        var div = '#route_importlist_cntr';
        if ($("#IsVehicle").val())
            div = '#importlist_cntr';
        $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    }
    function PaginateList() {

        var cntrId = '#route_importlist_cntr';
        if ($("#IsVehicle").val() == 'true')
            cntrId = '#importlist_cntr';
        $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            var pageNum = $(this).html();
            if ($("#IsVehicle").val() == 'true' && $("#ImportFrom").val() == 'fleet') {
                AjaxPaginationForVehicleList(pageNum);
            }
            else if ($("#IsVehicle").val() == 'false' && $("#ImportFrom").val() == 'library') {
                AjaxPaginationforSORoute(pageNum);
            }
            else if ($("#ImportFrom").val() == 'prevMov') {
                AjaxPaginationForMovement(pageNum);
            }
            else {
                AjaxPaginationForComponentList(pageNum);
            }
        });
        PaginateToLastPagesomovement(cntrId);
        PaginateToFirstPagesomovement(cntrId);
        PaginateToNextPagesomovement(cntrId);
        PaginateToPrevPagesomovement(cntrId);
    }

    function SearchHaulierList() {
    
        startAnimation();
        
        var div = '#route_importlist_cntr';
        if ($("#IsVehicle").val() == 'true')
            div = '#importlist_cntr';
        $.ajax({
            url: '/Movements/SetSearchData?page='+1+'&planMovement=' + true + '&PrevMovImport=' + true,
            type: 'POST',
            cache: false,
            async: false,
            data: $("#frmFilterMoveInbox").serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $(div).html($(response).find('.div_so_movement').html(), function () {
                    event.preventDefault();
                });
                $(div).find("form").removeAttr('action', "");
                $(div).find("form").submit(function (event) {
                    event.preventDefault();
                });
                if ($("#IsVehicle").val())
                    $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
                else
                    $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
                //var filters = $(response).find('div#movementFilters');
                //$(filters).find('form .filters:first').remove();
                //$(filters).find('form .filters:first').remove();
                //$(filters).appendTo('#banner-container');
                removeHrefLinksMovement();
                PaginateListMovement();
                if (typeof closeMovementFilters !='undefined')
                    closeMovementFilters();
                //if (typeof closeFilters != 'undefined')
                //    closeFilters();
            },
            error: function (xhr, status) {

                location.reload();
            },
            complete: function () {
                stopAnimation();
            }

        });
    }

   

    //function UseMovement() {
    //    ;
    //    var OrgID = $('#OrgID').val() ? $('#OrgID').val() : 0;
    //    var VIsRoutePrevMoveOpion = false;

    //    if ($("#IsPrevMoveOpion").val() == "routeMoveOpion")
    //        VIsRoutePrevMoveOpion = true;
    //    $.ajax
    //        ({
    //            url: '../Movements/MovementList',
    //            data: { MovementListForSO: true, showrtveh: 1, OrgID: OrgID, IsSOvehicle: true, IsVehiclePrevMoveOpion: false, IsNotify: false, PrevMovImport: true },
    //            type: 'GET',
    //            beforeSend: function () {
    //                startAnimation();
    //            },
    //            success: function (page) {
    //                $('#importlist_cntr').html($(page).find('.div_so_movement').html(), function () {
    //                });
    //                $('#importlist_cntr').find("form").removeAttr('action', "");
    //                $('#importlist_cntr').find("form").submit(function (event) {
    //                    AdvforSOApplivation();
    //                    event.preventDefault();
    //                });
    //                $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
    //                var filters = $(page).find('div#filters');
    //                $(filters).find('form .filters:first').remove();
    //                $(filters).find('form .filters:first').remove();
    //                $(filters).appendTo('#banner-container');
    //                removeHrefLinks();
    //                PaginateList();
    //                fillPageSizeSelect();

    //            },
    //            error: function (xhr, textStatus, errorThrown) {
    //            },
    //            complete: function () {

    //            }
    //        }).done(function () { //use this
    //            //alert("DONE!");
    //            stopAnimation();
    //        });;
    //}

    //function remove href from pagination ul li
    @*function removeHrefLinks() {
        ;
        $('#importlist_cntr').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");

        //activate first link
        //$('.pagination').find('li:first').addClass('activated');
    }*@

    //function fillPageSizeSelect() {
    //    var selectedVal = $('#pageSizeVal').val();
    //    $('#pageSizeSelect').val(selectedVal);
    //}

    //function Pagination for SO Movement------------------------------
    @*function PaginateList() {
        ;
        //method to paginate through page numbers
        $('#importlist_cntr').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            //var pageCount = $('#TotalPages').val();
            var pageNum = $(this).html();
if($('#hf_importFrm').val() ==  'fleet') {
                AjaxPaginationForVehicleList(pageNum);
            }
if($('#hf_importFrm').val() ==  'prevMov') {
                AjaxPaginationForMovement(pageNum);
            }
        });
        PaginateToLastPagesomovement();
        PaginateToFirstPagesomovement();
        PaginateToNextPagesomovement();
        PaginateToPrevPagesomovement();
    }*@

    //method to paginate to last page
    @*function PaginateToLastPagesomovement() {
        $('#importlist_cntr').find('.PagedList-skipToLast').click(function () {
            var pageCount = $('#TotalPages').val();
            //AjaxPaginationForMovement(pageCount);
if($('#hf_importFrm').val() ==  'fleet') {
                AjaxPaginationForVehicleList(pageCount);
            }
if($('#hf_importFrm').val() ==  'prevMov') {
                AjaxPaginationForMovement(pageCount);
            }
        });
    }*@

    //method to paginate to first page
    @*function PaginateToFirstPagesomovement() {
        $('#importlist_cntr').find('.PagedList-skipToFirst').click(function () {
if($('#hf_importFrm').val() ==  'fleet') {
                AjaxPaginationForVehicleList(1);
            }
if($('#hf_importFrm').val() ==  'prevMov') {
                AjaxPaginationForMovement(1);
            }
        });
    }*@

    //method to paginate to Next page
    @*function PaginateToNextPagesomovement() {
        $('#importlist_cntr').find('.PagedList-skipToNext').click(function () {
            var thisPage = $('#importlist_cntr').find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
if($('#hf_importFrm').val() ==  'fleet') {
                AjaxPaginationForVehicleList(nextPage);
            }
if($('#hf_importFrm').val() ==  'prevMov') {
                AjaxPaginationForMovement(nextPage);
            }
        });
    }*@
    //method to paginate to Previous page
    @*function PaginateToPrevPagesomovement() {
        $('#importlist_cntr').find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $('#importlist_cntr').find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
if($('#hf_importFrm').val() ==  'fleet') {
                AjaxPaginationForVehicleList(prevPage);
            }
if($('#hf_importFrm').val() ==  'prevMov') {
                AjaxPaginationForMovement(prevPage);
            }
        });
    }*@

    //
    //function AjaxPaginationForMovement(pageNum) {
    //    var selectedVal = $('#pageSizeVal').val();
    //    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRoute").val();
    //    var pageSize = selectedVal;
    //    $.ajax({
    //        url: '../Movements/MovementList',
    //        type: 'GET',
    //        cache: false,
    //        async: false,
    //        data: { page: pageNum, pageSize: pageSize, MovementListForSO: true, showrtveh: ShowPrevMoveSortRoute, IsNotify: true, PrevMovImport: true },
    //        beforeSend: function () {
    //            startAnimation();
    //        },
    //        success: function (result) {

    //            $('#importlist_cntr').html($(result).find('.div_so_movement').html(), function () {
    //            });

    //            $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
    //            var filters = $(result).find('div#filters');
    //            $(filters).find('form .filters:first').remove();
    //            $(filters).find('form .filters:first').remove();
    //            $(filters).appendTo('#banner-container');

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
    //}

    //function Ajax call for pagination
    //function AjaxPaginationForVehicleList(pageNum) {
    //    var selectedVal = $('#pageSizeVal').val();
    //    var pageSize = selectedVal;
    //    var txt_srch = $('.serchlefttxt').val();
    //    $.ajax({
    //        url: '../VehicleConfig/VehicleConfigList',
    //        data: { page: pageNum, importFlag: true, searchString: txt_srch },
    //        type: 'GET',
    //        async: false,
    //        beforeSend: function () {
    //            startAnimation();
    //        },
    //        success: function (response) {
    //            ;
    //            $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
    //            });
    //            $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');

    //            var filters = $(response).find('div#filters');
    //            $(filters).appendTo('#banner-container');

    //            removeHrefLinks();
    //            PaginateList();
    //        },
    //        error: function (xhr, textStatus, errorThrown) {
    //        },
    //        complete: function () {
    //            stopAnimation();
    //        }
    //    });
    //}
    //function clearSearch() {
    //    $(':input', '#filters')
    //        .not(':button, :submit, :reset, :hidden')
    //        .val('')
    //        .removeAttr('selected');
    //    SearchVehicle();
    //}

    //function SearchVehicle() {
    //    var searchString = $('#searchText').val();
    //    var vehicleIntend = $('#Indend').val();
    //    var vehicleType = $('#VehType').val();
    //    closeFilters();
    //    $.ajax({
    //        url: '../VehicleConfig/SaveVehicleConfigSearch',
    //        type: 'POST',
    //        cache: false,
    //        async: false,
    //        beforeSend: function () {
    //            startAnimation();
    //        },
    //        data: {
    //            searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, importFlag: true
    //        },
    //        success: function (response) {
    //

    //            $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
    //            });
    //            $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');

    //            var filters = $(response).find('div#filters');
    //            $(filters).appendTo('#banner-container');

    //            removeHrefLinks();
    //            PaginateList();
    //            fillPageSizeSelect();
    //        },
    //        complete: function () {
    //            stopAnimation();
    //        }
    //    });
    //}

    function FilterSuccessdata() {
        
        startAnimation();
        var div = '#route_importlist_cntr';
        if ($("#IsVehicle").val() == 'true')
            div = '#importlist_cntr';
        var url = '/SORTApplication/SetSORTFilter?planMovement=' + true + '&page =' + 1 + '&IsPrevtMovementsVehicle=' + true;
        if ($("#IsVehicle").val() == 'true')
            url = '/SORTApplication/SetSORTFilter?planMovement=' + true + '&page =' + 1 + '&IsPrevtMovementsVehicle=' + true;
        else
            url = '/SORTApplication/SetSORTFilter?planMovement=' + true + '&page =' + 1 + '&IsPrevtMovementsVehicleRoute=' + true;

        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            async: false,
            data: $("#FilterMoveInboxSORT").serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {               

                    $(div).html($(response).find('#div_MoveList_advanceSearch').html(), function () {
                        event.preventDefault();
                    });
                    $(div).find("form").removeAttr('action', "");
                    $(div).find("form").submit(function (event) {
                        event.preventDefault();
                    });
                    if ($("#IsVehicle").val())
                        $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
                    else
                        $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
                    //var filters = $(response).find('div#filters');
                    //$(filters).find('form .filters:first').remove();
                    //$(filters).find('form .filters:first').remove();
                    //$(filters).appendTo('#banner-container');
                    removeHrefLinksMovement();
                    PaginateListMovement();
                    closeSortFilters();
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
    function clearTextFields() {
        var _advFilter = $('#filter_SORT');
        _advFilter.find('input:text').each(function () {
            $(this).val('');
        });
        _advFilter.find('input:checkbox').prop('checked', false);
    }
    //function HaulierSort(event, param) {
    //    var sort_Order = param;
    //    var sort_Type = 1;
    //    if (event.classList.contains('sorting_asc')) {
    //        sort_Type = 0;
    //    }
    //    else if (event.classList.contains('sorting_desc')) {
    //        sort_Type = 3;
    //    }
    //    else if (!event.classList.contains('sorting_asc') && !event.classList.contains('sorting_desc')) {
    //        sort_Type = 0;
    //    }
    //    var div = '#route_importlist_cntr';
    //    if ($("#IsVehicle").val() == 'true')
    //        div = '#importlist_cntr';
    //    var url = '../Movements/MovementList';
    //    $.ajax({
    //        url: url,
    //        type: 'POST',
    //        cache: false,
    //        data: { sortType: sort_Type, sortOrder: sort_Order,planMovement: true, PrevMovImport: true },
    //        beforeSend: function () {
    //            startAnimation();
    //        },
    //        success: function (response) {
    //            $(div).html($(response).find('.div_so_movement').html(), function () {
    //                event.preventDefault();
    //            });
    //            $(div).find("form").removeAttr('action', "");
    //            $(div).find("form").submit(function (event) {
    //                event.preventDefault();
    //            });
    //            if ($("#IsVehicle").val())
    //                $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
    //            else
    //                $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
    //            var filters = $(response).find('div#filters');
    //            $(filters).find('form .filters:first').remove();
    //            $(filters).find('form .filters:first').remove();
    //            $(filters).appendTo('#banner-container');
    //            removeHrefLinksMovement();
    //            PaginateListMovement();
    //            $('.esdal-table > thead .sorting').removeClass('sorting_asc sorting_desc');
    //            $(".esdal-table > thead .sorting").each(function () {
                    
    //                var item = $(this);
    //                if ((sort_Type == 0 || sort_Type == 1) && item.find('span').attr('param') == sort_Order) {
    //                    item.addClass('sorting_desc');
    //                }
    //                else if (sort_Type == 3 && item.find('span').attr('param') == sort_Order) {
    //                    item.addClass('sorting_asc');
    //                }
    //            });
    //        },
    //        error: function (result) {
    //            location.readload();
    //        },
    //        complete: function () {
    //            stopAnimation();
    //        }
    //    });
    //}
    function MovementSort(event) {
        
        var param = event.target.attributes.param.value;
        var sort_order = event.target.attributes.order.value;
        var new_sort = sort_order == 0 ? 1 : sort_order == 1 ? 2 : 0; //sort_asc-2,desc-1

        var url = '../SORTApplication/SORTInbox';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            data: { sortType: new_sort, sortOrder: param, planMovement: true, IsPrevtMovementsVehicle: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                
                $("#div_MoveList_MapSearch").hide();
                $("#Mapfilterdiv").hide();
                $("#div_MoveList_advanceSearch").show();
                $('#div_sort_inbox').html('');
                $('#div_sort_inbox').html($(response).find('#div_sort_inbox').html());
                $("#Mapfilterdiv").hide();

                //$('#div_MoveList_advanceSearch').html('');
                //$('#div_MoveList_advanceSearch').html($(response).find('#div_MoveList_advanceSearch').html());
                $(".ssort").css('display', 'none');//Clear all filter value
                $(".ssort[order='0']").not(".ssort[param ='" + param + "']").css('display', 'block');//set all filter pipeline
                $(".ssort[order='" + new_sort + "'][param='" + param + "']").css('display', 'block');// display current sort element
            },
            error: function (result) {
                location.readload();
            },
            complete: function () {
                stopAnimation();
            }
        });

    }
