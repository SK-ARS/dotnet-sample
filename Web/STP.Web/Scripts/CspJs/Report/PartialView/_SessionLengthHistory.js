    $(document).ready(function () {
        selectedmenu("Reports");
        $("#BackAfftParty").click(BackToPreviousPage);
    });

    $('body').on('click', '.ShowPeriodicSessionPolice', function (e) {

        e.preventDefault();
        var startdatem = $(this).data('startdatem');
        var startdatey = $(this).data('startdatey');
        var utypeid = $(this).data('utype');
        var usertype = $(this).data('usertype');
        ShowPeriodicSessionLengthDetails(startdatem, startdatey, utypeid, usertype);
    });

    $('body').on('click', '.ShowPeriodicSessionsoa', function (e) {

        e.preventDefault();
        var startdatem = $(this).data('startdatem');
        var startdatey = $(this).data('startdatey');
        var utypeid = $(this).data('utype');
        var usertype = $(this).data('usertype');
        ShowPeriodicSessionLengthDetails(startdatem, startdatey, utypeid, usertype);
    });

    $('body').on('click', '.ShowPeriodicSessionhaulier', function (e) {

        e.preventDefault();
        var startdatem = $(this).data('startdatem');
        var startdatey = $(this).data('startdatey');
        var utypeid = $(this).data('utype');
        var usertype = $(this).data('usertype');
        ShowPeriodicSessionLengthDetails(startdatem, startdatey, utypeid, usertype);
    });
    
    $("#btnExport").click(function () { 
        var link = "../Report/SessionLengthExportToCSV" + EncodedQueryString("month=" + $("#hdnMonth").val() + "&year=" + $("#hdnYear").val() + "&userType=" + $("#hdnUserType").val());
        window.location.href = link;
    });
     
    function ShowPeriodicSessionLengthDetails(month, year, userTypeId, userType) { 
        $.ajax({
            url: '../Report/PeriodicSessionLengthDetails',
            type: 'POST',
            data: { month: month, year: year, userTypeId: userTypeId, userType: userType },
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

    function BackToPreviousPage() {
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
            },
            error: function () {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
