$(function () {
    fillPageSizeSelect();
    PaginateGrid();
    removeHLinks();
});

function changePageSize(_this, sURL) {  
    var pageSize = $(_this).val();
    var id = $('#anId').val();
    $.ajax({
        url: '../'+ sURL,
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, analysisID: id },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            $('#div_grid').html($(result).find('#div_grid').html());
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            var x = fix_tableheader();
            if (x == 1) $('#tableheader').show();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
             //location.reload();
        },
        complete: function () {
            $("#overlay").hide();
            $('.loading').hide();
            removeHLinks();
        }
    });
}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}

//function remove href from pagination ul li
function removeHLinks() {
    //$('#div_fleet_component').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    $('#div_so_movement_list').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    $('#div_so_movement_list').find('.pagination').find('.active a').css("z-index", 0);
    //activate first link
    //$('.pagination').find('li:first').addClass('activated');
}

//function Pagination
function PaginateGrid() {
    //method to paginate through page numbers
    $('#div_grid').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').live('click', function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPagination(pageNum);
        //console.log($('.active a').html());
    });
    PaginateToLastPage();
    PaginateToFirstPage();
    PaginateToNextPage();
    PaginateToPrevPage();
}


//method to paginate to last page
function PaginateToLastPage() {
    $('#div_grid').find('.PagedList-skipToLast').live('click', function () {
        var pageCount = $('#TotalPages').val();
        AjaxPagination(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPage() {
    $('#div_grid').find('.PagedList-skipToFirst').live('click', function () {
        AjaxPagination(1);
    });
}

//method to paginate to Next page
function PaginateToNextPage() {
    $('#div_grid').find('.PagedList-skipToNext').live('click', function () {
        var thisPage = $('#div_grid').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPagination(nextPage);
    });
}

//method to paginate to Previous page
function PaginateToPrevPage() {
    $('#div_grid').find('.PagedList-skipToPrevious').live('click', function () {
        var thisPage = $('#div_grid').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPagination(prevPage);
    });
}


//function Ajax call fro pagination
function AjaxPagination(pageNum) {
    var id = $('#anId').val();
    $.ajax({
        url: "../Contact/ContactedPartiesList",
        data: { page: pageNum, analysisID: id },
        type: 'POST',
        async: false,
        beforeSend: function () {
            //startAnimation();
        },
        success: function (result) {
             $('#div_grid').html($(result).find('#div_grid').html());
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            // //location.reload();
        },
        complete: function () {
            //stopAnimation();
            removeHLinks();
        }
    });
}

