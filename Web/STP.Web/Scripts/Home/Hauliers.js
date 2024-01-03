$(document).ready(function () {
    $("#more-info-news").click(function () {
        $(".news-toggle").show(500);
    });
    $("#less-info-news").click(function () {
        $(".news-toggle").hide(500);
    });
    $("#btn-planmovement").click(PlanMovement);
    $("#btn-notifsovr1").click(notifySOVR1);
    $("#btn-planvso").click(planVSO);
    $("#btn-planmovement").click(PlanMovement);
    SelectMenu(1);
});
function notifySOVR1() {
    window.location = '../Movements/haulierMovement';
}
function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}
function showmorenews() {
    if (document.getElementById('more-news').style.display !== "none") {
        document.getElementById('more-news').style.display = "none"
        document.getElementById('more-info').style.display = "block"
    }
    else {
        document.getElementById('more-news').style.display = "block";
        document.getElementById('more-info').style.display = "none"
    }
}
function PlanMovement() {
	localStorage.clear();
    window.location = '../Movements/CreateMovement';
}
function planVSO() {
    localStorage.clear();
    window.location = '../Movements/CreateVSOMovement';
}
function ManageMovement(MovementRevisionId, MovementVersionId, MovementType, VehicleType, MovementStatus, HaulierMnemonic, ProjectESDALReference, ApplicationRevisionNo, MovementVersionNumber, ProjectId, NotificationId, ContentReference) {
    var data;
    var url;
    if (MovementStatus == "work in progress") {

        data = "?revisionId=" + MovementRevisionId + "&notifId=" + NotificationId;
        url = '../Movements/OpenMovement';
        RedirectToPlanMovement(url + data);
    }
    else {
        if (VehicleType == 241002) {
            data = EncodedQueryString("revisionId=" + MovementRevisionId + "&movementId=" + VehicleType + "&versionId=" + MovementVersionId + "&hauliermnemonic=" + HaulierMnemonic + "&esdalref=" + ProjectESDALReference + "&revisionno=" + ApplicationRevisionNo + "&versionno=" + MovementVersionNumber + "&apprevid=" + MovementRevisionId + "&projecid=" + ProjectId + "&pageflag=2");
        }
        else {
            data = EncodedQueryString("revisionId=" + MovementRevisionId + "&movementId=" + VehicleType + "&versionId=" + MovementVersionId + "&hauliermnemonic=" + HaulierMnemonic + "&esdalref=" + ProjectESDALReference + "&revisionno=" + ApplicationRevisionNo + "&versionno=" + MovementVersionNumber + "&apprevid=" + MovementRevisionId + "&projecid=" + ProjectId + "&pageflag=2&VR1Applciation=true");
        }
        url = '../Application/ListSOMovements';
        window.location = url + data;
    }
}

$(document).ready(function () {
    var createAlertMsg = $('#CreateAlert').val();
    var SetPreference = $('#SetPreferenceMessage').val();
    if (createAlertMsg == "True") {
        ShowDialogWarningPop('Password changed successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    if (SetPreference == "True") {
        ShowDialogWarningPop('User preferences saved succesfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

    }

    var userTypeId = $('#userTypeId').val();
    var logged_in = $('#Logged_In').val();
    var msg = "";
    //if (userTypeId == 696001)    //haulier
        //msg = 'Dear ESDAL Users,<br/><br/>Once logged in you will now have access to the new updated ESDAL Maps. The new maps will provide improved abnormal load routing and a more accurate reflection of UK public highways. Users can route along newly added roads which will ensure more accurate and safe notifications are plotted and processed.<br/><br/>As the new maps contain a large number of new road and structural information, users will need to notify all routes using the new maps. Users can still access previously planned routes in ESDAL. More complex routes will need to be edited/re-planned by users to ensure they can be reformatted to the new maps, however to improve user experience, some less complex routes will be automatically re-planned by the system.<br/><br/>If you require any further information or assistance regarding the map update, please refer to the ‘Haulier Guide to Re-planning ESDAL Routes’ document in the Information Tab – Help & information or contact the ESDAL Helpdesk Team on 0300 470 3733, 8am - 6pm Monday - Friday (excluding bank holidays), email address: esdalenquiries@nationalhighways.co.uk”';
    if (logged_in != 1 && msg != "")
        showWarningBrokenPopUpDialog(msg, 'Ok', '', 'CloseBrokenPopUp', '', 1, 'info');
});

function logout() {
    location.replace('../Account/LogOut');
}

function CloseBrokenPopUp() {
    $.ajax({
        type: "POST",
        url: "../Account/SetLoginStatus",
        data: {},
        success: function (result) {
            if (result == 1)
                WarningCancelBtn();
        },
        error: function (xhr, status, error) {
            alert("error");
        }
    });
}

