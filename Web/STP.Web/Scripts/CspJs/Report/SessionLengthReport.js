
    $(document).ready(function () {
       
        if ($('#haulierportal').val() == "True") {
            SelectMenu(7);
        }
        else {
            SelectMenu(5);
        }
       
       
        var d = new Date();
        n = d.getMonth(); 
        $('#DDStartMonth option:eq(' + n + ')').prop('selected', true); 
    });
    $("#DDStartMonth").change(function () {
        $('#div_export_soareport').hide();
        $('#SessionLengthReport').html('');
    });
    $("#DDStartYear").change(function () {
        $('#div_export_soareport').hide();
        $('#SessionLengthReport').html('');
    });
    $("#btnSearch").click(function () {
        var month = $("#DDStartMonth").val();
        var year = $("#DDStartYear").val();

        $.ajax({
            url: '../Report/SessionLengthHistoryTypeWise',
            type: 'POST',
            data: { month: month, year: year },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {

                $('#SessionLengthReport').html(html);
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

