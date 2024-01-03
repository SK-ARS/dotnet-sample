    $(document).ready(function () {
        StepFlag = 6;
        SubStepFlag = 0;
        CurrentStep = "Notification Overview";
        $('#current_step').text(CurrentStep);
        SetWorkflowProgress(6);

        $('#save_btn').hide();
        $('#apply_btn').hide();
        $('#confirm_btn').hide();
    });
