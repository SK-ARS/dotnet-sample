$(document).ready(function () {

    $(".view-feedbackDetails").on('click', ViewFeedbackdetail);

    $("body").on('click', '.btn-more-feedback', function (e) {
        e.preventDefault();
        var feedbackId = $(this).data('feedabckid');
        if ($(this).hasClass('more')) {
            $('.feedBack-less-' + feedbackId).hide();
            $('.feedBack-more-' + feedbackId).slideDown(1000, function () {
            });
            
            $(this).html('Read less');
            $(this).removeClass('more');
        } else {
            $(this).html('Read more');
            $(this).addClass('more');
            $('.feedBack-more-' + feedbackId).slideUp(1000, function () {
                $('.feedBack-less-' + feedbackId).show();
            });
            
        }
        
    });
});

function ViewFeedbackdetail(data) {

    var FeedbackId = data.currentTarget.attributes.FeedbackId.value;
    var OpenCheck = data.currentTarget.attributes.OpenCheck.value;
    ViewFeedbackdetails(FeedbackId, OpenCheck);
}
function ViewFeedbackdetails(id, chk) {

    startAnimation();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: "../Feedback/ViewFeedbackPopup",
        //contentType: "application/json; charset=utf-8",
        data: { feedid: id, openChk: chk },
        datatype: "json",
        success: function (data) {
            $('#generalPopupContent').html(data);
            $('#generalPopup').modal(options);
            $('#generalPopup').modal('show');
            $('.loading').hide();
            stopAnimation();
        },
        error: function () {
            location.reload();
        }
    });
}
$('body').on('click', '#ClosePopup', function (e) {
    CloseGeneralPopup();
});
function CloseGeneralPopup() {
    $('#generalPopup').modal('hide');
}