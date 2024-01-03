function ReviseApplication(ESDALReference, revisionno, ApplicationrevId, movementType, versionId, VersionNo) {
    movementTypeGlobal = movementType;
    var AppRevId = ApplicationrevId ? ApplicationrevId : 0;
    var URL = "../Application/ReviseVR1Application";
    var Inputdata = { Revision_id: AppRevId, Reduced_det: 0, Clone_app: 0, VersionID: versionId, ESDALRefCode: ESDALReference, revisionno: revisionno, VersionNo: VersionNo };
    if (movementType == 'special order' || movementType == 'so') {
        URL = "../Application/ReviseSOApplication";
        Inputdata = { apprevid: AppRevId, ESDALRefCode: ESDALReference, revisionno: revisionno, VersionNo: VersionNo };
    }
    $.ajax({
        url: URL,
        type: 'post',
        data: Inputdata,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            //Uncommented on 22-03-2023
            var revisionId = typeof (data.ApplicationRevId) != undefined && data.ApplicationRevId != null ? data.ApplicationRevId : data.ApplicationRevisionId;
            var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("revisionId=" + revisionId);
            var inputDataObj = {};
            if (movementType == 'special order' || movementType == 'so') {
                inputDataObj = { AppRevisonId: revisionId, IsReplanRequired: true }
            }
            else {
                inputDataObj = { VersionId: data.VersionId, IsReplanRequired: true };
            }
            CheckIsBroken(inputDataObj, function (response) {
                if (response != null && response != undefined) {
                    ShowBrokenRouteMessageAndRedirectToPlanMovement(response, redirectUrl);
                }
            });
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
var movementTypeGlobal="";
function CloneApplication(ESDAL_Reference, VersionID, ApplicationrevId, movementType, historic) {
    movementTypeGlobal = movementType;
    var AppRevId = ApplicationrevId ? ApplicationrevId : 0;
    var URL = '';
    var Inputdata = {};
    if (movementType == 'special order' || movementType == 'so') {
        URL = "../Application/CloneSOApplication";
        Inputdata = { apprevid: AppRevId, ESDALRefCode: ESDAL_Reference, isHistory: historic };
    }
    else {
        URL = "../Application/ReviseVR1Application";
        Inputdata = { Revision_id: AppRevId, Reduced_det: 0, Clone_app: 1, VersionID: VersionID, ESDALRefCode: ESDAL_Reference, isHistory: historic };
    }
    $.ajax({
        url: URL,
        type: 'post',
        data: Inputdata,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            var revisionId = typeof (data.ApplicationRevId) != undefined && data.ApplicationRevId != null ? data.ApplicationRevId : data.ApplicationRevisionId;
            var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("revisionId=" + revisionId);
            var inputDataObj = {};
            if (movementType == 'special order' || movementType == 'so') {
                inputDataObj = { AppRevisonId: revisionId, IsReplanRequired: true }
            }
            else {
                inputDataObj = { VersionId: data.VersionId, IsReplanRequired: true};
            }
            CheckIsBroken(inputDataObj, function (response) {
                if (response != null && response != undefined) {
                    ShowBrokenRouteMessageAndRedirectToPlanMovement(response, redirectUrl);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function NotifySOAlertmsg(inputData) {
    stopAnimation();
    var msg = 'You are notifying a Proposed route, but need to re-notify once the Agreed route has been received. Do you want to continue?';
    ShowWarningPopupCloneRenotif(msg, function () {
        $('#WarningPopup').modal('hide');
        setTimeout(function () { NotifyApplication(inputData); }, 500);
    }, function () { $('#WarningPopup').modal('hide'); });
}
function NotifyApplication(inputData) {
    stopAnimation();
    $.ajax({
        url: "../Notification/NotifyApplication",
        type: 'post',
        data: inputData,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            NotifID = data.result.NotificationId;
            var contRefNum = data.result.ContentReferenceNo;
            var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("notifId=" + NotifID);
            CheckIsBroken({ ConteRefNo: contRefNum, IsReplanRequired: true }, function (response) {
                if (response != null && response != undefined) {
                    ShowBrokenRouteMessageAndRedirectToPlanMovement(response, redirectUrl);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}
function CloneNotification(NotificationCode, NotificationId, Historic, contentReferenceNoOld) {
    $.ajax({
        type: "POST",
        url: '../Notification/CloneNotification',
        data: { notificationId: NotificationId, notificationCode: NotificationCode, isHisto: Historic },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            var notifId = data.result.NotificationId;
            var contRefNum = data.result.ContentReferenceNo;
            if (notifId > 0 || contRefNum != null) {
                var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("notifId=" + notifId);
                var inputData = { ConteRefNo: contRefNum, IsReplanRequired: true };
                if (Historic > 0) {//If historic, the route is already planned, just need to display the message if it is broken
                    inputData = { ConteRefNo: contentReferenceNoOld, IsReplanRequired: false };
                }
                CheckIsBroken(inputData, function (response) {
                    if (response != null && response != undefined) {
                        if (Historic > 0) {//Need to handle historic clone separately
                            ShowBrokenRouteMessageAndRedirectToPlanMovementForHistoric(response, redirectUrl, true);
                        } else {
                            ShowBrokenRouteMessageAndRedirectToPlanMovement(response, redirectUrl, true);
                        }
                    }
                });
            } else {
                showToastMessage({
                    message: "Unable to clone the notification, please try again.",
                    type: "error"
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function Renotify(PreviousNotificationCode, NotificationId, vr1renotify, versionStatus) {
    var notificationId = NotificationId;
    var PrevNotifCode = PreviousNotificationCode;
    $.ajax({
        type: "POST",
        url: '../Notification/ReNotify',
        data: { NotificationID: notificationId, VR1_ReNotify: vr1renotify, notifcode: PrevNotifCode, versionStatus: versionStatus },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data.result.NotificationId == 0) {
                window.location.reload();
            }
            else {
                var notifId = data.result.NotificationId;
                var contRefNum = data.result.ContentReferenceNo;
                var redirectUrl = '../Movements/OpenMovement' + EncodedQueryString("notifId=" + notifId);
                CheckIsBroken({ ConteRefNo: contRefNum, IsReplanRequired: true }, function (response) {
                    if (response != null && response != undefined) {
                        ShowBrokenRouteMessageAndRedirectToPlanMovement(response, redirectUrl, true);
                    }
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function ShowBrokenRouteMessageAndRedirectToPlanMovement(response,redirectUrl,isNotificationClone=false) {
    var msg = "";
    var isVr1 = false;
    if ($('#hf_VR1Applciation').length > 0) {
        isVr1 = $('#hf_VR1Applciation').val() == "True";
    } else {
        if (movementTypeGlobal != "") {
            isVr1 =(movementTypeGlobal == 'special order' || movementTypeGlobal == 'so')? false:true;
        }
    }
    movementTypeGlobal = "";
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

function ShowBrokenRouteMessageAndRedirectToPlanMovementForHistoric(response, redirectUrl, isNotificationClone = false) {
    var msg = "";
    var isVr1 = false;
    if ($('#hf_VR1Applciation').length > 0) {
        isVr1 = $('#hf_VR1Applciation').val() == "True";
    } else {
        if (movementTypeGlobal != "") {
            isVr1 = (movementTypeGlobal == 'special order' || movementTypeGlobal == 'so') ? false : true;
        }
    }
    movementTypeGlobal = "";
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
        else { // if all routes is replanned  if (response.autoReplanSuccess > 0) 

            var isReplanPossible = true;
            $.each(response.Result, function (key, valueObj) {
                if (valueObj.IsBroken > 0 && valueObj.IsReplan > 1) {
                    isReplanPossible = false;
                }
            });

            if (isReplanPossible) {
                if (!isNotificationClone) {
                    msg = isVr1 ? BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_SUCCESS_VR1 : BROKEN_ROUTE_MESSAGES.COMMON_MESSAGE_AUTO_REPLAN_SUCCESS;
                } else {
                    msg = BROKEN_ROUTE_MESSAGES.RENOTIFY_CLONE_NOTIFICATION_REPLAN_SUCCESS;
                }
            } else {
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
        }
        if (msg != "") {
            ShowWarningPopupMapupgarde(msg, function () {
                $('#WarningPopup').modal('hide');
                RedirectToPlanMovement(redirectUrl);
            });
        } else {
            RedirectToPlanMovement(redirectUrl);
        }
    }
    else {
        RedirectToPlanMovement(redirectUrl);
    }
}


function GetBrokenRouteNotifyAppInitial(isVr1, inputData, response) {
    var msg = "";
    if (response && response.Result && response.Result.length > 0 && response.brokenRouteCount > 0) {
        var flag = 0;
        if (inputData.versionStatus == 305002 || inputData.versionStatus == 305003) { //proposed or reproposed
            msg = BROKEN_ROUTE_MESSAGES.NOTIFY_PROPOSED_REPROPOSED;
        }
        else {
            msg = isVr1 ? BROKEN_ROUTE_MESSAGES.NOTIFY_VR1 : BROKEN_ROUTE_MESSAGES.NOTIFY_SO;
            flag = 1;
        }
        ShowWarningPopupMapupgarde(msg, function () {
            $('#WarningPopup').modal('hide');
            if (flag == 1) {
                setTimeout(function () { NotifyApplication(inputData);}, 500);
            }
            else {
                setTimeout(function () { NotifySOAlertmsg(inputData); }, 500);
            }
        });
    }
    else {
        if (inputData.versionStatus == 305002 || inputData.versionStatus == 305003) {//proposed or reproposed
            stopAnimation();
            msg = 'You are notifying without an agreement and should notify again once the route has been agreed. The haulier should not move until the agreed route and Special Order permit has been issued by National Highways and acceptable notification has been given. Do you want to continue?';
            ShowWarningPopupCloneRenotif(msg, function () {
                $('#WarningPopup').modal('hide');
                setTimeout(function () { NotifyApplication(inputData); }, 500);
            }, function () { $('#WarningPopup').modal('hide'); });
        }
        else {
            NotifyApplication(inputData);
        }
    }
}