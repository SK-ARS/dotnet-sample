    $(document).ready(function () {
        StepFlag = 5;
        SubStepFlag = 0;
        CurrentStep = "Route Assessment";

        $('#current_step').text(CurrentStep);
        $('#VSO_Type').val(@VSOType);
        SetWorkflowProgress(5,'notif');

        $('#back_btn').show();
        $('#confirm_btn').show();
        $('#save_btn').hide();
        $('#apply_btn').hide();
        var isRenotify = false;
        if ('@payLoad.IsRenotify' == "True") {
            isRenotify = true;
        }
        GenerateNotificationRouteAssessment('@payLoad.ContenRefNo', @payLoad.NotificationId, @payLoad.AnalysisId, @VSOType, isRenotify);

        $.ajax({
            url: '../Application/RouteAnalysisPanel',
            type: 'POST',
            async: false,
            cache: false,
            data: { analysisId: @payLoad.AnalysisId, contentRefNo: '@payLoad.ContenRefNo', IsNotifRouteassessment : true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $('#leftpanel1').html(response);
            },
            complete: function () {
                stopAnimation();
            }
        });
    });

    function IsRouteAnalysisComplete() {
        var structCheck = $("#IncrListStructure").val();
        var constraintCheck = $("#IncrListConstraint").val();
        var cautionCheck = $("#IncrListCaution").val();
        if (structCheck == 0 && constraintCheck == 0 && cautionCheck == 0) {
            ConfirmRouteAssessment();
        }
        else {
            ShowWarningPopup("The notification contains unsuitable structures/constraints/cautions. Do you want to continue?","ConfirmRouteAssessment");
        }
    }
    function ConfirmRouteAssessment() {

                    $.ajax({
                url: '../Notification/IsRouteAnalysisComplete',
                type: 'POST',
                async: false,
                cache: false,
                data: { analysisId: @payLoad.AnalysisId },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    CloseWarningPopupRef();
                    if (data.result) {
                        if ($('#OverviewInfoSaveFlag').val() == 'true') {

                            LoadContentForAjaxCalls("POST", '../Notification/NotificationOverview', { notificationId: @payLoad.NotificationId, workflowProcess: 'HaulierApplication' }, '#overview_info_section');

                        }
                        else {
                            var dataModelPassed = { notificationId: @payLoad.NotificationId, workflowProcess: 'HaulierApplication' };
                            var result = ApplicationRouting(3, dataModelPassed);
                            if (result != undefined || result != "NOROUTE") {
                                //../Notification/SetNotificationGeneralDetails
                                LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#overview_info_section');
                            }
                        }
                    }
                    else {
                        ShowErrorPopup(data.message);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {

                },
                        complete: function () {
                            $('.remove1').hide();
                    stopAnimation();
                }
            });

    }
