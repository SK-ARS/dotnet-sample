
    $(document).ready(function () {
        SelectMenu(5);
        var d = new Date();
        n = d.getMonth();

        $('#DDStartMonth option:eq(' + n + ')').prop('selected', true);
        selectedmenu("Reports");
    });

    $("#btnSearch").click(function () {

        var startMonth = $("#DDStartMonth").val();
        var startYear = $("#DDStartYear").val();

        $.ajax({
            url: '../Report/CommunicationHistory',
            type: 'POST',
            data: { startMonth: startMonth, startYear: startYear },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {

                $('#CommunicationReport').html(html);
            },
            error: function () {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }); 
    
