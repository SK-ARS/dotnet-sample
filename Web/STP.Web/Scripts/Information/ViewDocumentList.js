$(document).ready(function () {
    $("body").on('change', '.documntsearch,.docsearch', function () {
        DocumentSerach(this);
    });
    if ($('#haulierportal').val() == "True") {
        SelectMenu(7);
    }
    else {
        SelectMenu(5);
    }
});

function DocumentSerach(thisVal) {
    var documentTypeId = $(thisVal).val();
    var documentType = $("#DDsearchCriteria option:selected").text();
    var IMF = {
        SearchColumn: documentType,
        SearchValue: documentTypeId
    };

    $.ajax({
        async: false,
        type: "POST",
        url: '../Information/ViewDocumentList',
        data: { IMF: IMF },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            var result = $(response).find('section#banner').html();
            $('section#banner').html(result);

        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SearchListDocument(isSort = false) {
    let url = '../Information/ViewDocumentList';
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
$('body').on('change', '#pageSizeSelectDocument', function () {

    $('#pageNum').val();
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchListDocument();
});
