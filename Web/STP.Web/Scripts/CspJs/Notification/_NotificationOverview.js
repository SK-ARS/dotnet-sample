    $(document).ready(function () {
        StepFlag = 6;
        SubStepFlag = 0;
        CurrentStep = "Notification Overview";
        $('#current_step').text(CurrentStep);
        SetWorkflowProgress(6);

        $('#confirm_btn').hide();
        ViewTermsAndConditions();

        $("#EditNotificationGeneral").on('click', EditNotificationGeneralDetails);
    });

    function EditNotificationGeneralDetails() {
        LoadContentForAjaxCalls("POST", '../Notification/SetNotificationGeneralDetails', { notificationId: $('#NotificationId').val() }, '#overview_info_section');
    }

    function IsApplyForNotification() {
        let Msg = "Do you want to submit notification?";
        if ($('#HaulierReference').val() != '')
            Msg = "Do you want to submit notification \"" + $('#HaulierReference').val() + '\" ?';
        ShowWarningPopup(Msg, "SubmitNotification");
    }

    function SubmitNotification() {
        CloseWarningPopup();
        $.ajax({
            url: '../Notification/GenerateNotification',
            type: 'POST',
            datatype: 'json',
            data: { NotificationId: $('#NotificationId').val(), GenerateCode: 1, ImminentMovestatus: $('#ImminentFlag').val(), ProjectId: $('#NotifProjectId').val(), workflowProcess: 'HaulierApplication' },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                stopAnimation();
                var Msg = 'Notification submitted successfully. The ESDAL reference is ' + "\"" + data.EsdalRefNo + "\"";
                ShowSuccessModalPopup(Msg, "RedirectToNotificationOverview('" + $('#NotificationId').val() + "','" + data.EsdalRefNo + "')");
            },
            complete: function () {
                stopAnimation();
            }
        });
    }


