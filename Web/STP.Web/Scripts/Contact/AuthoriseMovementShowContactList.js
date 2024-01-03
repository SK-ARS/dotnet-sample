function ViewContactDetails1(ID, isClosed) {

    Resize_PopUp(540);
    resetdialogue();
    var analysisId = $('#hdnConstraintID').val();
    var notificationID = $('#hdnNotificationID').val();

    var esdalRefNumber = $('#hdnESDALRefNumber').val();
    var type = $('#hdnType').val();

    var random = Math.random();

    var URL = "../Application/ViewContactDetails?ContactId=" + ID + "&EsdalRefNo=&structOwner=&isClosed=true&random=" + random;

    isClosed = false;

    removescroll();
    $("#dialogue").load(URL);

    $("#dialogue").show();
    $("#overlay").show();
}

$('#reviewCausionPaginator').on('click', 'a', function (e) {
    if (this.href == '') {
        return false;
    }
    else {
        $.ajax({
            url: this.href,
            type: 'GET',
            cache: false,
            success: function (result) {
                $('#General').html(result);
            }
        });
        return false;
    }
});

