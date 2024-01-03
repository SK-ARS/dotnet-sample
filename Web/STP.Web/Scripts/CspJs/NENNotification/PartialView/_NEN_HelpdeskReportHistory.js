
    $("#btnExportCategory").click(function () {
        var monthValue = $('#DDStartMonth').val();
        var yearValue = $('#DDStartYear').val();
        var notificationcategory = $("#DDNotificationCategory").val();

        var link = "../NENNotification/NENHelpdeskReportExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue + "&notificationcategory=" + notificationcategory);
        window.location.href = link;
    });

