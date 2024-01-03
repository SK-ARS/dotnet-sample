
$(document).ready(function () {
    SelectMenu(5);
    var d = new Date();
    n = d.getMonth();

    $('#DDStartMonth option:eq(' + n + ')').prop('selected', true);

    selectedmenu('Reports');
});

$("#btnSearch").click(function () {

    var month = $("#DDStartMonth").val();
    var year = $("#DDStartYear").val();

    $.ajax({
        url: '../Report/IndustryLiaisonHistory',
        type: 'POST',
        data: { month: month, year: year },
        beforeSend: function () {
            startAnimation();
        },
        success: function (html) {
            $('#IndustryLiaisonReportHTML').html(html);
            CheckSessionTimeOut();
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
});

$('body').on('click', '#btnExport', function (e) {
    var monthValue = $('#DDStartMonth').val();
    var yearValue = $('#DDStartYear').val();

    var link = "../Report/IndustryLiaisonExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue);
    window.location.href = link;
});

