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
