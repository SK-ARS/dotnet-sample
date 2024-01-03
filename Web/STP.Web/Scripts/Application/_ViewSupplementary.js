$(document).ready(function () {
    $('body').on('click', '#btnEditSupplementaryInfo', function (e) {
        EditSupplementaryInfo();
    });
});
function ViewSupplementaryInit() {
    isVehicleHasChanged = false;
    StepFlag = 5;
    SubStepFlag = 0;
    CurrentStep = "Supplementary Information";
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(5);
    $('#save_btn').hide();
    $('#apply_btn').hide();
    $('#confirm_btn').show();
    $('#backbutton').show();
    $('#back_btn').show();
    $('#confirm_btn').removeClass('blur-button');
    $('#confirm_btn').attr('disabled', false);
    stopAnimation();
}
