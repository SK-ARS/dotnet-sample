var hauliermnemonic;
var esdalref;
var revisionno;
var revision_no;
var versionno;
var versionId;
var revisionId;
var ApprevId;
var projectid;
var Pageflag;
var OrgID;
var AppEdit;
var Proj_Status;
var AppStatusCode;
var Work_status;
var VR1_Applciation;
var chk_status;
var sort_user_id;
var checker_id;
var newUrl = '';

function ApplSummaryLeftPanelInit() {
    hauliermnemonic = $('#hauliermnemonic').val();
    esdalref = $('#esdalref').val();
    revisionno = $('#revisionno').val();
    revision_no = $('#arev_no').val();
    versionno = $('#versionno').val();
    versionId = $('#versionId').val();
    revisionId = $('#revisionId').val();
    ApprevId = $('#ApprevId').val();
    projectid = $('#projectid').val();
    Pageflag = $('#Pageflag').val();
    OrgID = $('#OrganisationId').val();
    AppEdit = $('#AppEdit').val();
    Proj_Status = $('#Proj_Status').val();
    AppStatusCode = $('#AppStatusCode').val();
    Work_status = $('#hdnWork_Status').val();
    VR1_Applciation = $('#VR1Applciation').val();
    chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    if (Proj_Status == 'Withdrawn' || Proj_Status == 'Declined') {
        $('#menu-buttons').hide();
        $('#tr_edit_rev').hide();
    }
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
        $('#menu-buttons').hide();
        $('#tr_edit_rev').hide();
    }
    if (chk_status == 301006 && $('#VR1Applciation').val() == "False") {
        if (AppStatusCode == 307014 || AppStatusCode == 307011) {
            $('#menu-buttons').show();
            $('#tr_edit_rev').show();
        }
        else {
            $('#menu-buttons').hide();
            $('#tr_edit_rev').hide();
        }
    }
}
$(document).ready(function () {
    $('body').on('click', '#EditCurrRev', function (e) {
        if (VR1_Applciation == "True" || VR1_Applciation == "true") {
            window.location.href = "../SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=CreateVR1&revisionId=" + revisionId + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&apprevid=' + ApprevId + '&VR1Applciation=' + VR1Applciation + '&pageflag=' + Pageflag + '&EditRev=true' + '&WorkStatus=' + Work_status + '&EditFlag=1' + '&Checker=' + Checker + '&Owner=' + Owner.replace(/ /g, '%20'));
        }
        else {
            window.location.href = "../SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=CreateSO&revisionId=" + revisionId + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&apprevid=' + ApprevId + '&pageflag=' + Pageflag + '&EditRev=true' + '&WorkStatus=' + Work_status + '&EditFlag=1');
        }
    });
    $('body').on('click', '#CreateNewRev', function (e) {
        hauliermnemonic = $('#hauliermnemonic').val();
        esdalref = $('#esdalref').val();
        var newesdalref = $('#ESDALReferenceSORT').val();
        var VR1application = $('#VR1Applciation').val();
        var ESDAL_Reference = hauliermnemonic + "/" + esdalref;
        if (newesdalref != null || newesdalref != "") {
            ESDAL_Reference = newesdalref;
        }
        if (VR1application == "True" || VR1application == "true") {
            ShowWarningPopup('Click Yes to create a new version of "' + ESDAL_Reference + '" for editing.', "ReviseVR1ApplSummary", '', ESDAL_Reference);
        } else {
            var isenterdsort = $('#hf_EnterdbySort').val();
            if (isenterdsort == 1) {

                ShowWarningPopup('Click Yes to create a new version of "' + ESDAL_Reference + '" for editing.', "ReviseSOApplSummary", '', ESDAL_Reference);
                movement_Type = 207001;
            }
            else {
                ShowErrorPopup('Haulier created application cannot be revised from sort!');
                movement_Type = 207002;

            }
        }
    });
});
function ReviseSOApplSummary(EsdalrefCode) {
    CloseWarningPopup();
    startAnimation();
    var versionno = $('#versionno').val();
    var revisionno = $('#revisionno').val();
    var is_replan = 0;
    $.ajax({
        url: "../Application/ReviseSOApplication",
        type: 'post',
        data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode, RevisionNo:revisionno,VersionNo: versionno  },
        success: function (data) {
            var revisionId = typeof (data.ApplicationRevId) != undefined && data.ApplicationRevId != null ? data.ApplicationRevId : data.ApplicationRevisionId;
            var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("revisionId=" + revisionId);
            var inputDataObj = { AppRevisonId: revisionId, IsReplanRequired: true }

            CheckIsBroken(inputDataObj, function (response) {
                if (response != null && response != undefined) {
                    ShowBrokenRouteMessageAndRedirectToPlanMovementSORT(response, redirectUrl);
                }
            });

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
function WarningBrokenPopUp() {
    CloseWarningPopup();
    window.location = newUrl;
}
function ReviseVR1ApplSummary(EsdalrefCode) {
    var versionno = $('#versionno').val();
    var revisionno = $('#revisionno').val();
    CloseWarningPopup();
    startAnimation();
    $.ajax({
        url: "../SORTApplication/ReviseVR1Application",
        type: 'post',
        data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode, VersionNo: versionno, RevisionNo: revisionno },
        success: function (data) {
            var revisionId = typeof (data.ApplicationRevId) != undefined && data.ApplicationRevId != null ? data.ApplicationRevId : data.ApplicationRevisionId;
            var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("revisionId=" + revisionId);
            var inputDataObj = { VersionId: data.VersionId, IsReplanRequired: true };
            
            CheckIsBroken(inputDataObj, function (response) {
                if (response != null && response != undefined) {
                    ShowBrokenRouteMessageAndRedirectToPlanMovementSORT(response, redirectUrl);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}


function ShowBrokenRouteMessageAndRedirectToPlanMovementSORT(response,redirectUrl,isNotificationClone=false) {
    var msg = "";
    var isVr1 = $('#hf_VR1Applciation').length > 0 && $('#hf_VR1Applciation').val() == "True";
    if (response.brokenRouteCount != 0) {
        if (response.specialManouer > 0) {
            if (response.Result.length > 1 && response.autoReplanSuccess < response.brokenRouteCount) { //1<2
                if (!isNotificationClone) {
                    msg = isVr1 ? BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_SPECIAL_MANOUER_VR1 : BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_SPECIAL_MANOUER;
                } else {
                    msg = BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_SPECIAL_MANOUER_NOTIF_MULTIPLE;
                }
            }
            else { //
                if (!isNotificationClone) {
                    msg = isVr1 ? BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_SPECIAL_MANOUER_VR1 : BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_SPECIAL_MANOUER;
                } else {
                    msg = BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_SPECIAL_MANOUER_NOTIF;
                }
            }
        }
        else if (response.autoReplanFail > 0) { //if auto replan fails
            if (response.brokenRouteCount == 1) { //single broken route
                if (!isNotificationClone) {
                    msg = isVr1 ? BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_FAIL_VR1 : BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_FAIL;
                } else {
                    msg = BROKEN_ROUTE_MESSAGES.RENOTIFY_CLONE_NOTIFICATION_REPLAN_FAILED_SINGLE_ROUTE;
                }
            }
            else if (response.brokenRouteCount > 1) { //multiple broken route
                if (!isNotificationClone) {
                    msg = isVr1 ? BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_FAIL_VR1 : BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_FAIL;
                } else {
                    msg = BROKEN_ROUTE_MESSAGES.RENOTIFY_CLONE_NOTIFICATION_REPLAN_FAILED_MULTIPLE_ROUTE;
                }
            }
        }
        else if (response.autoReplanSuccess > 0) { // if all routes is replanned
            if (!isNotificationClone) {
                msg = isVr1 ? BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_SUCCESS_VR1 : BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_SUCCESS;
            } else {
                msg = BROKEN_ROUTE_MESSAGES.RENOTIFY_CLONE_NOTIFICATION_REPLAN_SUCCESS;
            }
        }

        ShowWarningPopupMapupgarde(msg, function () {
            $('#WarningPopup').modal('hide');
            RedirectToPlanMovement(redirectUrl);
        });
    }
    else {
        RedirectToPlanMovement(redirectUrl);
    }
}