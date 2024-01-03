$(document).ready(function () {
    $('body').on('click', '#btnEditSupplementaryInfo', function (e) {
        EditSupplementaryInfo();
    });
});
function ViewSupplementaryInit() {

    StepFlag = 5;
    SubStepFlag = 0;
    CurrentStep = "Supplementary Information";
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(5);
    $('#save_btn').hide();
    $('#apply_btn').hide();
    $('#confirm_btn').show();
}
