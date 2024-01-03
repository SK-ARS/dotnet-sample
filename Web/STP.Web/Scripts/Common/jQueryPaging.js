$(function () {
    fillPageSizeSelect();
});

function changePageSize(_this, sURL) {
    var pageSize = $(_this).val();
    $.ajax({
        url: '../' + sURL,
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            $('#div_paging_resultSet').html($(result).find('#div_paging_resultSet').html());
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
        }
    });
}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}