var ConstraintID = parseInt($("#ConstraintID").val());
$(document).ready(function () {
    $('body').on('click', '.view-caution .btn-vc-open-review-report', function () {
        OpenReviewCautionFromReport();
        return false;
    });

    $('body').on('click', '.view-caution .btn-vc-close-caution-span', function () {
        closeCautionSpanViewCaution();
        return false;
    });

});

function OpenDialogueViewCautions(url, container) {
    startAnimation();
    $.ajax({
        async: false,
        type: "GET",
        url: url,
        processdata: true,
        success: function (response) {
            if (container != '' && container != undefined) {
                $("#dialogue").html($(response).closest('.' + container)[0]);
            } else {
                $("#dialogue").html(response);
            }
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
        },
        error: function (result) {
        }
    });
}
function closeCautionSpanViewCaution() {
    $("#dialogue").show();
    var randomNumber = Math.random();
    $(".modal-backdrop").css("height", "0px");
    var flageditmode = $('#flageditmode').val();
    OpenDialogueViewCautions('../Constraint/ReviewCautionsList?constraintId=' + ConstraintID + '&random=' + randomNumber, 'review-caution-list');
}
function OpenReviewCautionFromReport() {
    var randomNumber = Math.random();
    OpenDialogueViewCautions('../Constraint/ReviewCautionsList?ConstraintId=' + ConstraintID + '&random=' + randomNumber, 'review-caution-list');
}

function redirectpage() {
    $("#dialogue").hide();
    $("#overlay").show();
    window.location.href = "../Account/Login";
}