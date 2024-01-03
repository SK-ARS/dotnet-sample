
var pageSizeTemp = $('#hf_pageSize').val();
var hf_analysisID = $('#hf_analysisID').val();
var hf_pageNum = $('#hf_pageNum').val();

function ContactedPartiesInit() {
    pageSizeTemp = $('#hf_pageSize').val();
    hf_analysisID = $('#hf_analysisID').val();
    hf_pageNum = $('#hf_pageNum').val();
    selectedmenu('Movements');
    fillPageSizeSelect();
    removeHLinksContactedParties();
    PaginationAjax();
}
function ViewContactedParties(id) {
    removescroll();
    $("#dialogue").load("../Application/ViewContactDetails", { ContactId: id }, function () {
        stopAnimation();
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $("#dialogue").show();
        $("#overlay").show();
        $('#contactDetails').modal('show');
    });
}
$(document).ready(function () {
    $('body').on('click', '.viewcontactparties', function (e) {
        e.preventDefault();
        ViewContactParties(this);
    });
    $('body').on('click', '.contactonbehalfofid', function (e) {
        e.preventDefault();
        ViewContactParty(this);
    });
});
function ViewContactParties(e) {
    var arg1 = $(e).attr("arg1");
    ViewContactedParties(arg1);
}
function ViewContactParty(e) {
    var arg1 = $(e).attr("arg1");
    ViewContactedParties(arg1);
}
function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}
function changePageSize(_this) {

    $("#overlay").show();
    $('.loading').show();

    var pageSize = $(_this).val();
    var aID = hf_analysisID;
    $.ajax({
        url: '../Contact/ContactedPartiesList',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, analysisID: aID },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#div_list').html(result);
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            var x = fix_tableheader();
            if (x == 1) $('#tableheader').show();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            $("#overlay").hide();
            $('.loading').hide();
        }
    });
}
function removeHLinksContactedParties() {
    $('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}
function PaginationAjax() {
    $('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        var pageNum = $(this).html();
        ContactPagination(pageNum);
    });
    PaginateToLastPage();
    PaginateToFirstPage();
    PaginateToNextPage();
    PaginateToPrevPage();
}
function ContactPagination(pageNo) {
    var analysisId = $('#analysisId').val();
    var url = '../Contact/ContactedPartiesList';
    $.ajax({
        url: url,
        type: 'POST',
        data: {
            page: pageNo, pageSize: hf_pageNum, analysisID: hf_analysisID
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#tab_8').html(page);
            $("#tab_8").show();
            removeHLinksContactedParties();
            PaginationAjax();
            CheckSessionTimeOut();
        },
        error: function (err, ex, xhr) {

            showWarningPopDialog('An error occured. Please try again.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
        },
        complete: function () {
            stopAnimation();
            contactedParties = true;
        }
    });
}
function PaginateToLastPage() {
    $('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        ContactPagination(pageCount);
    });
}
function PaginateToFirstPage() {
    $('.PagedList-skipToFirst').click(function () {
        ContactPagination(1);
    });
}
function PaginateToNextPage() {
    $('.PagedList-skipToNext').click(function () {
        var thisPage = $('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        ContactPagination(nextPage);
    });
}
function PaginateToPrevPage() {
    $('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        ContactPagination(prevPage);
    });
}
