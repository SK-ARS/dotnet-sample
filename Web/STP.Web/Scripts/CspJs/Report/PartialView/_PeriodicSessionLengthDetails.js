    $(document).ready(function () {
        $("#BackAfftParty").click(BackToPreviousPage);
    });
    $("#btnExport").click(function () {

        $("#btnExport").removeClass("hideExportBtn");
        $("#btnExport").addClass("showExportBtn");

        var link = "../Report/PeriodicSessionLengthExportToCSV" + EncodedQueryString("month=" + $("#hdnMonth").val() + "&year=" + $("#hdnYear").val() + "&userTypeId=" + $("#hdnUserTypeId").val());
        window.location.href = link;
    });

    function BackToPreviousPage() {
        var month = $("#hdnMonth").val();
        var year = $("#hdnYear").val();
        var userType = $("#hdnUserType").val();

        $.ajax({
            url: '../Report/SessionLengthHistory',
            type: 'POST',
            data: { month: month, year: year, userType: userType },
            beforeSend: function () {
                startAnimation();
            },
            success: function (html) {

                $('#SessionLengthReport').html(html);
            },
            error: function () {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    $('#periodicsessionlengthpaginator').on('click', 'a', function (e) {
        if (this.href == '') {
            return false;
        }
        else {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#SessionLengthReport').html(result);
                    stopAnimation();
                },
                error: function (xhr, status, error) {
                    stopAnimation();
                },
                complete: function () {
                    stopAnimation();
                }
            });
            return false;
        }
    });
