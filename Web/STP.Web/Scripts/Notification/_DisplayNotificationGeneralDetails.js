var ContentReferenceNo;
var AnalysisId;
var ProjectId;
var VersionId;
var ESDALReference;

function DisplayNotificationGeneralDetailsInit() {
    ContentReferenceNo = $('#hf_ContentReferenceNo').val();
    AnalysisId = $('#hf_AnalysisId').val();
    ProjectId = $('#hf_ProjectId').val();
    VersionId = $('#hf_VersionId').val();
    ESDALReference = $('#hf_ESDALReference').val();
    $("#CRNo").val(ContentReferenceNo);
    $("#NotifAnalysisId").val(AnalysisId);
    $("#ProjectId").val(ProjectId);
    $("#VersionId").val(VersionId);
    $("#ProjectStatus").val(ProjectStatus);
    $('#NotificationCode').val(ESDALReference);
    var string = $('#NotificationCode').val() + ' - ' + $("#MoveStat").val();
    $('#notif_code').text(string);
}

$(document).ready(function () {
    $('body').on('click', '#PrintNotificationReport', function (e) {
        e.preventDefault();
        PrintNotificationReport(this);
    });
    $('body').on('click', '#ShowHideDetails', function (e) {
        e.preventDefault();
        ShowHideDetails(this);
    });
    $('body').on('click', '#Renotify', function (e) {
        e.preventDefault();
        PreviousNotificationCode = $(this).attr('previousnotificationcode');
        notificationId = $(this).attr('notificationid');
        RevisionId = $(this).attr('revisionid');
        var versionStatus = $(this).data('versionstatus');
        var vr1_renotify = 0;
        if (RevisionId > 0) {
            vr1_renotify = 1;
        }
        Renotify(PreviousNotificationCode, notificationId, vr1_renotify, versionStatus);
    });
    $('body').on('click', '#Clone', function (e) {
        e.preventDefault();
        var notifCode = $(this).data('notifcode');
        var notifId = $(this).data('notifid');
        var isHistoric = $(this).data('historic');
        var contentReferenceNo = $(this).data('contentreferencenum');
        CloneNotification(notifCode, notifId, isHistoric, contentReferenceNo);
    });
});

function PrintNotificationReport(_this) {
    var NotificationId = $(_this).attr('notificationid');
    var ContactId = $(_this).attr('contactid');
    var link = "../Notification/HaulierNotificationDocument?notificationId=" + NotificationId + "&contactId=" + ContactId + "";
    window.open(link, '_blank');
}
function ShowHideDetails(_this) {
    var target = $(_this).attr('target');
    switch (parseInt(target)) {
        case 6:
            //req-notes
            if (document.getElementById('dispensation-div').style.display !== "none") {
                document.getElementById('dispensation-div').style.display = "none"
                document.getElementById('chevlon-up-sog-icon6').style.display = "none"
                document.getElementById('chevlon-down-sog-icon6').style.display = "block"
            }
            else {
                document.getElementById('dispensation-div').style.display = "block"
                document.getElementById('chevlon-up-sog-icon6').style.display = "block"
                document.getElementById('chevlon-down-sog-icon6').style.display = "none"
            }
            break;
    }

}
