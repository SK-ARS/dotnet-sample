$('#showAuditLogPaginator').on('click', 'a', function (e) {
    if (this.href == '') {
        return false;
    }
    else {
        $.ajax({
            url: this.href,
            type: 'POST',
            cache: false,
            success: function (result) {
                $('#Config-body').html(result);
            }
        });
        return false;
    }
});
function AutheriseAuditlogSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = $(event).hasClass('sorting_desc') || $(event).find('.sorting').hasClass('sorting_desc')  ? 0 : 1;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    var pageSize = $('#pageSizeVal').val();
    var pageNum = $('#pageNum').val()
    showList(sortOrderGlobal, sortTypeGlobal, pageSize, pageNum);
}

