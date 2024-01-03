
$(function () {


    removeHLinks();

    PaginateGrid();

    fillPageSizeSelect();

});

$(document).ready(function () {

    $('#btn_bck').click(function (e) {
        location.href = '@Url.Content("~/RoadOwnership/showRoadOwnership/")';
    });

    $('#span-close').click(function () {
        addscroll();//----------->> add scrollbar to the body
        $('#overlay').hide();
    });
});


function removeHLinks() {
    $('#div_dispensation_org').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    $('#div_dispensation_org').find('.pagination').find('.active a').css("z-index", 0);
}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}

function SearchOrganisation() {
    var randomNumber = Math.random();
    var searchFlag = $('#searchFlag').val();
    var pageSize = $('#pageSizeVal').val();
    var pageNum = $('#pageNum').val();
    var searchKey1 = $('#searchKey').val();
    var searchKey = escape(searchKey1);

    $.ajax({
        url: '../../RoadOwnership/ListRoadContactList?page=' + pageNum + '&pageSize=' + pageSize + '&SearchString=' + searchKey + '&searchFlag=' + searchFlag,
        type: 'GET',
        cache: false,
        async: false,
        beforeSend: function () {
        },
        success: function (result) {
            
            $('#div_grid_org_list').html($(result).find('#div_grid_org_list').html());
        },
        error: function (jq, status, message) {

            alert('A jQuery error has occurred. Status: ' + status + ' - Message: ' + message);
        },
        complete: function () {
            removeHLinks();
            addscroll();
        }
    });
    PaginateGrid();
}

function changePageSize(_this) {
    var randomNumber = Math.random();
    var keyword = $('#SearchString').val();
    var pageSize = $(_this).val();
    var searchFlag = $('#searchFlag').val();
    $.ajax({
        url: '../RoadOwnership/ListRoadContactList?random=' + randomNumber,
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, SearchString: keyword, searchFlag: searchFlag },
        beforeSend: function () {

        },
        success: function (result) {

            $('#div_grid_org_list').html($(result).find('#div_grid_org_list').html());

            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#pagesize').val(pageSize);
            var x = fix_tableheader();
            if (x == 1) $('#tableheader').show();
        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

            $('#searchKey').val(keyword);
            removeHLinks();
        }
    });

    PaginateGrid();
}

function PaginateGrid() {

    //method to paginate through page numbers
    $('#div_dispensation_org .pagination').on('click', 'li a', function () {
      
        var smt = $(this).closest('li').attr('class');
        switch (smt) {
            case 'active':
                break;
            case 'PagedList-skipToLast':
                var pageCount = $('#div_dispensation_org').find('#TotalPages').val();
                AjaxPagination(pageCount);
                break;
            case 'PagedList-skipToNext':
                var thisPage = $('#div_dispensation_org').find('.active').find('a').html();
                var nextPage = parseInt(thisPage) + 1;
                AjaxPagination(nextPage);
                break;
            case 'PagedList-skipToFirst':
                AjaxPagination(1);
                break;
            case 'PagedList-skipToPrevious':
                var thisPage = $('#div_dispensation_org').find('.active').find('a').html();
                var prevPage = parseInt(thisPage) - 1;
                AjaxPagination(prevPage);
                break;
            default:
                var pageNum = $(this).html();
                AjaxPagination(pageNum);
                break;
        }
    });
}

//function Ajax call fro pagination
function AjaxPagination(pageNum) {
    var randomNumber = Math.random();
    var pageSize = $('#pageSizeVal').val();
    var keyword = $('#searchKey').val();
    var origin = $('#origin').val();
    var searchFlag = $('#searchFlag').val();
    $.ajax({
        url: '../../RoadOwnership/ListRoadContactList?page=' + pageNum + '&pageSize=' + pageSize + '&SearchString=' + keyword + '&random=' + randomNumber + '&searchFlag=' + searchFlag,
        type: 'GET',
        cache: false,
        async: false,
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (result) {

            $('#div_dispensation_org').html($(result).find('#div_dispensation_org').html());

            var x = fix_tableheader();

            if (x == 1) $('#tableheader').show();

        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

            $('.loading').hide();
            $('#searchKey').val(keyword);

            removeHLinks();
            PaginateGrid();
        }
    });
}
