$(document).ready(function () {
    selectedmenu('Reports');
});
$('body').on('click', '.showsession', function (e) {
    e.preventDefault();
    var usertype = $(this).data('usertype');

    ShowSessionLengthDetails(usertype);
});
$('body').on('click', '.session-length-report #btnExport', function (e) {

    e.preventDefault();
    var link = "../Report/SessionLengthTypeWiseExportToCSV" + EncodedQueryString("month=" + $("#hdnMonth").val() + "&year=" + $("#hdnYear").val());
    window.location.href = link;
});

$('body').on('click', '.ShowPeriodicSession', function (e) {
    e.preventDefault();
    var usertype = $(this).data('usertype');
    var startdatem = $(this).data('startdatem');
    var startdatey = $(this).data('startdatey');
    var utypeid = $(this).data('utypeid');

    ShowPeriodicSessionLengthDetails(startdatem, startdatey, utypeid, usertype);
});

$('body').on('click', '#BackAfftParty', function (e) {
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

$('body').on('click', '#BackShowPeriodic', function (e) {
    var UserType = $("#hdnUserType").val();
    ShowSessionLengthDetails(UserType);
});
function ShowSessionLengthDetails(userType) {

    $.ajax({
        url: '../Report/SessionLengthHistory',
        type: 'POST',
        data: { userType: userType, month: $("#hdnMonth").val(), year: $("#hdnYear").val() },
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
            CheckSessionTimeOut();
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}