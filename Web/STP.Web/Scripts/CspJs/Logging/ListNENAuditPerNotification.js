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
        debugger
        sortOrderGlobal = param;
        sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
        $('#SortTypeValue').val(sortTypeGlobal);
        $('#SortOrderValue').val(sortOrderGlobal);
        var pageSize = $('#pageSizeVal').val();
        var pageNum = $('#pageNum').val()
        showList(sortOrderGlobal = 1, sortTypeGlobal = 0, pageSize, pageNum);
    }
