
$(document).ready(function () {
    SelectMenu(5);
    var d = new Date();
    n = d.getMonth();
    $('#DDStartMonth option:eq(' + n + ')').prop('selected', true);
    selectedmenu('Reports');
    $("#DDUserType").trigger("change");
});

$("#btnSearch").click(function () {
    var month = $("#DDStartMonth").val();
    var year = $("#DDStartYear").val();
    var userType = $("#DDUserType").val();

    $.ajax({
        url: '../Report/ReportPerUserHistory',
        type: 'POST',
        data: { month: month, year: year, userType: userType },
        beforeSend: function () {
            startAnimation();
        },
        success: function (html) {
            $('#ReportPerUserHTML').html(html);
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
});

$("#DDUserType").on("change", function () {
    $("#rptTitle").html('Report per Haulier');
    if ($(this).val() == '696001') {
        $("#rptTitle").html('Report per Haulier');
    }
    else if ($(this).val() == '696002') {
        $("#rptTitle").html('Report per Police');
    }
    else {
        $("#rptTitle").html('Report per SOA');
    }
});

$('body').on('click', '#btnExport', function (e) {
    var monthValue = $('#DDStartMonth').val();
    var yearValue = $('#DDStartYear').val();
    var userType = $("#DDUserType").val();

    var link = "../Report/ReportPerUserExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue + "&userType=" + userType);
    window.location.href = link;
});