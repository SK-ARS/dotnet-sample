$(document).ready(function () {
    SelectMenu(5);
    var d = new Date();
    n = d.getMonth();
    $('#DDStartMonth option:eq(' + n + ')').prop('selected', true);
    selectedmenu('Reports');
    $("#DDNotificationCategory").trigger("change");
});

$("#DDNotificationCategory").change(function () {
    $('#div_export_categorylist').hide();
});
$("#DDStartMonth").change(function () {
    $('#div_export_categorylist').hide();
});
$("#DDStartYear").change(function () {
    $('#div_export_categorylist').hide();
});

$("#btnSearch").click(function () {
    var month = $("#DDStartMonth").val();
    var year = $("#DDStartYear").val();
    var notificationcategory = $("#DDNotificationCategory").val();

    $.ajax({
        url: '../NENNotification/NEN_HelpdeskReportHistory',
        type: 'POST',
        data: { month: month, year: year, notificationcategory: notificationcategory },
        beforeSend: function () {
            startAnimation();
        },
        success: function (html) {
            $('#NENHelpdeskReport').html(html);
            CheckSessionTimeOut();
        },
        error: function () {
            alert('error');
            location.reload();
        },
        complete: function () {
            $('#div_export_categorylist').show();
            stopAnimation();
        }
    });
});

$('body').on('click', '#btnExportCategory', function (e) {
    var monthValue = $('#DDStartMonth').val();
    var yearValue = $('#DDStartYear').val();
    var notificationcategory = $("#DDNotificationCategory").val();

    var link = "../NENNotification/NENHelpdeskReportExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue + "&notificationcategory=" + notificationcategory);
    window.location.href = link;
});

