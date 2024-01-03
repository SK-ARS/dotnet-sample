var VSOType;
var ContenRefNo;
var NotificationId;
var AnalysisId;
var IsRenotify;
var prevContacName;

function NotificationRouteAssessmentInit() {
    StepFlag = 5;
    SubStepFlag = 0;
    CurrentStep = "Route Assessment";
    VSOType = $('#hf_VSOType').val();
    ContenRefNo = $('#hf_ContenRefNo').val();
    NotificationId = $('#hf_NotificationId').val();
    AnalysisId = $('#hf_AnalysisId').val();
    IsRenotify = $('#hf_IsRenotify').val();
    prevContacName = $('#hf_PeviousContactName').val()

    $('#current_step').text(CurrentStep);
    $('#VSO_Type').val(VSOType);
    SetWorkflowProgress(5, 'notif');

    $('#back_btn').show();
    $('#backbutton').show();
    $('#confirm_btn').hide();
    $('#route_assessment_next_btn').show();
    $('#save_btn').hide();
    $('#apply_btn').hide();
    var isRenotify = false;
    if (IsRenotify == "True") {
        isRenotify = true;
    }
    GenerateNotificationRouteAssessment(ContenRefNo, NotificationId, AnalysisId, VSOType, isRenotify, prevContacName, function () {
        $.ajax({
            url: '../Application/RouteAnalysisPanel',
            type: 'POST',
            data: { analysisId: AnalysisId, contentRefNo: ContenRefNo, IsNotifRouteassessment: true },
            success: function (response) {
                $('#leftpanel1').html(response);
                closeContentLoader('body');
                RouteAnalysisNotifInit();
            }
        });
    });
}
function IsRouteAnalysisComplete() {
    var structCheck = $("#IncrListStructure").val();
    var constraintCheck = $("#IncrListConstraint").val();
    var cautionCheck = $("#IncrListCaution").val();
    if ((structCheck == 0 && constraintCheck == 0 && cautionCheck == 0) || isTopNavOverviewClicked) {
        isTopNavOverviewClicked = false;
        ConfirmRouteAssessment();
    }
    else {
        ShowWarningPopup("The notification contains unsuitable structures/constraints/cautions. Do you want to continue?", "ConfirmRouteAssessment");
    }
}
function ConfirmRouteAssessment() {
    $.ajax({
        url: '../Notification/IsRouteAnalysisComplete',
        type: 'POST',
        async: false,
        cache: false,
        data: { analysisId: AnalysisId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            CloseWarningPopupRef();
            if (data.result) {
                var dataModelPassed = { notificationId: NotificationId, workflowProcess: 'HaulierApplication' };
                var result = ApplicationRouting(3, dataModelPassed);
                if (result != undefined || result != "NOROUTE") {
                    //console.log(result);
                    LoadContentForAjaxCalls("POST", result.route, result.dataJson, '#overview_info_section', '', function () {
                        NotificationGeneralDetailsInit();
                        scrolltotop();
                    });
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
