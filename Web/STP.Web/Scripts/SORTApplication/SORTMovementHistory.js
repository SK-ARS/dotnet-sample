$(document).ready(function () {
    var page = getUrlParameterByName("page", this.href);
    $('#Pageflag').val(page);
    var pageNum = $('#Pageflag').val();
    
});
$('body').on('click', '#SORTHistory a', function (e) {
    e.preventDefault();
    var page = getUrlParameterByName("page", this.href);
    $('#Pageflag').val(page);
    var sortType = $('#sortType').val();
    var sortOrder = $('#sortOrder').val();
    var pageNum = $('#Pageflag').val();
    DisplayHistory(sortOrder, sortType, pageNum);
});
function SORTMovementHistoryInit() {
    $('#leftpanel').hide();
}
function HistorySort(event, param) {
    sortOrder = param;
    sortType = event.classList.contains('sorting_asc') ? 1 : 0;
    var page = getUrlParameterByName("page", this.href);
    var pageNum = $('#Pageflag').val();
    var issort = true;
    DisplayHistory(sortOrder, sortType, pageNum);
}
