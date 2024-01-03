var ImportFrm;
var backToPreviousList;

function MovementSelectVehicleByImportInit(isSoVr1ExistingApp = false) {
    var previousMoveText = "Select vehicle from previous movements";
    if (!isSoVr1ExistingApp) {
        StepFlag = 1;
        SubStepFlag = 1.1;
        CurrentStep = "Select Vehicle";
        SetWorkflowProgress(1);
        $('#ImportFrom').val(ImportFrm);
        $('#IsVehicle').val(true);
        ImportFrm = $('#hf_ImportFrm').val() || $('#hf_importFrm').val();
        backToPreviousList = $('#hf_BackToPreviousList').val();
        if ($('#hf_importFrm').val() == 'fleet') {
            $('#list_heading').text("Select vehicle from fleet");
            SelectVehicleFromFleet();
        }
        $('#back_btn').show();
    }
    else {
        SubStepFlag = 3.1;
        $('#backBtnExistingSOVr1').show();
        $('#back_btn').hide();
        previousMoveText = "Select from movements";
    }
    $('#confirm_btn').hide();
    if ($('#hf_importFrm').val() == 'prevMov') {
        $('#list_heading').text(previousMoveText);
        UseMovement($('#IsSortUser').val(), backToPreviousList, isSoVr1ExistingApp);
    }
}
$('body').on("click", ".ssort", function () {
    MovementSort(event);
});
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
function SearchHaulierList(isSort = false, pageSize = 10) {
   
    startAnimation();

    var div = '#route_importlist_cntr';
    if ($("#IsVehicle").val() == 'true')
        div = '#importlist_cntr';
    var params = isSort ? 'page=' + $('#pageNum').val() : "page=1";
    $.ajax({
        url: '/Movements/SetSearchData?' + params + '&' + "pageSize=" + pageSize + '&' + "planMovement=" + true + "&PrevMovImport=" + true,
      //  url: '/Movements/SetSearchData?page=' + 1 + '&planMovement=' + true + '&PrevMovImport=' + true,
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
            removeHrefLinksMovement();
            PaginateListMovement();
            if (typeof closeMovementFilters != 'undefined')
                closeMovementFilters();
        },
        error: function (xhr, status) {

            location.reload();
        },
        complete: function () {
            stopAnimation();
        }

    });
}
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