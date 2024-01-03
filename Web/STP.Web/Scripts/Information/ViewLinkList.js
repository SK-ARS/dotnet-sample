    $(document).ready(function () {
        if ($('#haulierportal').val() == "True") {
            SelectMenu(7);
        }
        else {
            SelectMenu(5);
        }
    });
function SearchListLink(isSort = false) {
    let url = '../Information/ViewLinkList';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: {
            sortType: $('#SortTypeValue').val(), sortOrder: $('#SortOrderValue').val(),
            page: (isSort ? $('#pageNum').val() : 1), pageSize: $('#pageSizeVal').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('section#banner').html('');
            $('section#banner').html($(response).find('section#banner'));
        },
        error: function (result) {
            location.readload();

        },
        complete: function () {
            stopAnimation();
        }
    });
}
$('body').on('change', '#pageSizeLink', function () {

    $('#pageNum').val();
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchListLink();
});