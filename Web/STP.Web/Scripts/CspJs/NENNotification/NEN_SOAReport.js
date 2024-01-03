    $(document).ready(function () {
        var d = new Date();
        n = d.getMonth();
        $('#DDStartMonth option:eq(' + n + ')').prop('selected', true);
    });

    $("#DDStartMonth").change(function () {
        $('#div_export_soareport').hide();
        $('#NENHelpdeskReport').html('');
    });
    $("#DDStartYear").change(function () {
        $('#div_export_soareport').hide();
        $('#NENHelpdeskReport').html('');
    });

    $("#btnSearch").click(function () {
        var month = $("#DDStartMonth").val();
        var year = $("#DDStartYear").val();

        $.ajax({
            url: '../NENNotification/NEN_SOAReportHistory',
            type: 'POST',
            data: { month: month, year: year },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {
                $('#NENHelpdeskReport').html(html);
            },
            error: function () {
                alert('error');
                location.reload();
            },
            complete: function () {
                $('#div_export_soareport').show();
                stopAnimation();

            }
        });
    });
