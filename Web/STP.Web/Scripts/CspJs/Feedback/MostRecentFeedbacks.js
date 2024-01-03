$(document).ready(function () {
        
        $(".view-feedbackDetails").on('click', ViewFeedbackdetail);
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