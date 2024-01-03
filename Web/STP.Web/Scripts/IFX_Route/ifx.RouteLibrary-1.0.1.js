//function for document ready()
function RouteLibReady() {
    removeHLinks();
    PaginateGrid();

    RemoveDelete();
    RemoveLinks_Map();

   ViewRoute();
    //Map_back();
   $('#btn_back').click(function () { alter("Go Back"); });
}

function removeHLinks() {

    $('#Config-body').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");

    //activate first link
    //$('.pagination').find('li:first').addClass('activated');
}

//function Pagination
function PaginateGrid() {
    //method to paginate through page numbers
    $('#Config-body').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
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
    $('#Config-body').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxPagination(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPage() {
    $('#Config-body').find('.PagedList-skipToFirst').click(function () {
        AjaxPagination(1);
    });
}

//method to paginate to Next page
function PaginateToNextPage() {
    $('#Config-body').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#Config-body').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPagination(nextPage);
    });
}

//method to paginate to Previous page
function PaginateToPrevPage() {
    $('#Config-body').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#Config-body').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPagination(prevPage);
    });
}

//function Ajax call fro pagination
function AjaxPagination(pageNum) {
    //var compType = GetComponentType();
    $.ajax({
        url: "../Routes/RoutePartLibrary",
        type: 'GET',
        cache: false,
        async: false,
        data: { page: pageNum },
        beforeSend: function () {
            //$("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            $('#Config-body').html($(result).find('#div_Route').html());
            $('#div_Route').find('.head1').hide();
            removeHLinks();
            PaginateGrid();
            RemoveDelete();

            RemoveLinks_Map();
            //$('#pageSizeVal').val(pageSize);
            //$('#pageSizeSelect').val(pageSize);
            //$('#pagesize').val(pageSize);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            // //location.reload();
        },
        complete: function () {
            //$("#overlay").hide();
            $('.loading').hide();
        }
    });
    function changePageSize(_this) { alert("");}
}

function RemoveDelete() {
    $('#tbl_User tr').each(function () {
        $(this).find('td').eq(2).remove();
    });

    $('#tbl_User tr').each(function () {
        $(this).find('th').eq(2).remove();
    });
}

function RemoveLinks_Map() {
    $('#tbl_User').find('a').removeAttr('href').css("cursor", "pointer");
}

function ViewRoute() {
    $('#tbl_User').find('a').live('click', function () {
        var id = $(this).attr('id');
      //  alert(id);

        $.ajax({
            url: "../Routes/LibraryRoutePartDetails",
            type: 'GET',
            cache: false,
            async: false,
            data: { routeName: id },
            beforeSend: function () {
                //$("#overlay").show();
                $('.loading').show();
            },
            success: function (result) {
                $('#Config-body').html($(result).find('#Map_View').html());
                LibraryRoutePartDetailsInit();
                CheckSessionTimeOut();
                //$('#div_Route').find('.head1').hide();
                removeHLinks();
                PaginateGrid();
                RemoveDelete();

                RemoveLinks_Map();
                //$('#pageSizeVal').val(pageSize);
                //$('#pageSizeSelect').val(pageSize);
                //$('#pagesize').val(pageSize);
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                // //location.reload();
            },
            complete: function () {
                //$("#overlay").hide();
                $('.loading').hide();
            }
        });

    });
}


   
