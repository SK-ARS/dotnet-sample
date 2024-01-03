var esdal_ref = "";
var Flag_App_Status = "";
var LatestAppRevID = "";
var newUrl = "";

function ManageMovement(MovementRevisionId, MovementVersionId, MovementType, VehicleType, MovementStatus, HaulierMnemonic,
    ProjectESDALReference, ApplicationRevisionNo, MovementVersionNumber, ProjectId, NotificationId, ContentReference, MovementVehicleType, IsHistoric) {
    var data;
    var url;
    localStorage.removeItem("WorkInProgressNotifId");
    if (MovementStatus == "work in progress") {
        localStorage.setItem("WorkInProgressNotifId", NotificationId);
        if (NotificationId == 0) {
            if ($('#hf_MovementListForSO').val() == 'False' && ($('#hf_PlanMovement1').val() == 'False')) {
                data = "?revisionId=" + MovementRevisionId + "&notifId=" + NotificationId;
                url = '../Movements/OpenMovement';
                RedirectToPlanMovement(url + data);
            }
            else {
                if ($('#hf_IsRoutePrevMoveOpion').val() == 'True') {
                    // SelectCurrentMovementsRoute(0, MovementStatus, HaulierMnemonic, ProjectESDALReference,ProjectId);
                }
                if ($('#hf_IsVehiclePrevMoveOpion').val() == 'True') {
                    // SelectPrevitMovementsVehicle(0, MovementStatus, HaulierMnemonic, ProjectESDALReference,ProjectId);
                }
                else {
                    LoadApplicationVehicleRoute(MovementVersionId, MovementVehicleType, ContentReference, MovementRevisionId);
                }
            }
        }
        else {
            if ($('#hf_IsNotify').val() == 'True' || $('#hf_PlanMovement1').val() == 'True') {
                LoadApplicationVehicleRoute(0, MovementVehicleType, ContentReference, MovementRevisionId);
            }
            else {
                data = "?revisionId=" + MovementRevisionId + "&notifId=" + NotificationId;
                url = '../Movements/OpenMovement';
                RedirectToPlanMovement(url + data);
            }
        }
    }
    else {
        if (NotificationId == 0) {
            var x = $('#hf_MovementListForSO').val();
            if ($('#hf_MovementListForSO').val() == 'False' && ($('#hf_PlanMovement1').val() == 'False')) {
                if (VehicleType == 241002) {
                    data = "?revisionId=" + MovementRevisionId + "&movementId=" + VehicleType + "&versionId=" + MovementVersionId + "&hauliermnemonic=" + HaulierMnemonic + "&esdalref=" + ProjectESDALReference + "&revisionno=" + ApplicationRevisionNo + "&versionno=" + MovementVersionNumber + "&apprevid=" + MovementRevisionId + "&projecid=" + ProjectId + "&pageflag=2" + "&Ishistoric=" + IsHistoric ;
                }
                else {
                    data = "?revisionId=" + MovementRevisionId + "&movementId=" + VehicleType + "&versionId=" + MovementVersionId + "&hauliermnemonic=" + HaulierMnemonic + "&esdalref=" + ProjectESDALReference + "&revisionno=" + ApplicationRevisionNo + "&versionno=" + MovementVersionNumber + "&apprevid=" + MovementRevisionId + "&projecid=" + ProjectId + "&pageflag=2&VR1Applciation=true" + "&Ishistoric=" + IsHistoric ;
                }
                url = '../Application/ListSOMovements';
                window.location = url + data;
            }
            else {
                if ($('#hf_IsRoutePrevMoveOpion').val() == 'True') {
                    // selectcurrentmovementsroute(0, MovementStatus, HaulierMnemonic, ProjectESDALReference,ProjectId);
                }
                if ($('#hf_IsVehiclePrevMoveOpion').val() == 'True') {
                    // SelectPrevitMovementsVehicle(0, MovementStatus, HaulierMnemonic, ProjectESDALReference,ProjectId);
                }
                else {
                    LoadApplicationVehicleRoute(MovementVersionId, MovementVehicleType, ContentReference, MovementRevisionId);
                }
            }
        }
        else {
            if ($('#hf_IsNotify').val() == 'True' || $('#hf_PlanMovement1').val() == 'True') {
                LoadApplicationVehicleRoute(0, MovementVehicleType, ContentReference, MovementRevisionId);
            }
            else {
                if (VehicleType == 241002) {
                    data = "?revisionId=" + MovementRevisionId + "&movementId=" + VehicleType + "&versionId=" + MovementVersionId + "&hauliermnemonic=" + HaulierMnemonic + "&esdalref=" + ProjectESDALReference + "&revisionno=" + ApplicationRevisionNo + "&versionno=" + MovementVersionNumber + "&apprevid=" + MovementRevisionId + "&projecid=" + ProjectId + "&pageflag=2" + "&Ishistoric=" + IsHistoric ;
                }
                else {
                    data = "?revisionId=" + MovementRevisionId + "&movementId=" + VehicleType + "&versionId=" + MovementVersionId + "&hauliermnemonic=" + HaulierMnemonic + "&esdalref=" + ProjectESDALReference + "&revisionno=" + ApplicationRevisionNo + "&versionno=" + MovementVersionNumber + "&apprevid=" + MovementRevisionId + "&projecid=" + ProjectId + "&pageflag=2&VR1Applciation=true" + "&Ishistoric=" + IsHistoric ;
                }
                url = '../Application/ListSOMovements';
                window.location = url + data;
            }
        }
    }
}
$('body').on('click', '.managemovement', function (e) {
    var MovementRevisionId = $(this).data("movementrevisionid");
    var MovementVersionId = $(this).data("movementversionid");
    var MovementType = $(this).data("movementtype");
    var VehicleType = $(this).data("vehicletype");
    var MovementStatus = $(this).data("movementstatus");
    var HaulierMnemonic = $(this).data("movehauliermnemonicmentversionid");
    var ProjectESDALReference = $(this).data("projectesdalreference");
    var ApplicationRevisionNo = $(this).data("applicationrevisionno");
    var MovementVersionNumber = $(this).data("movementversionnumber");
    var ProjectId = $(this).data("projectid");
    var NotificationId = $(this).data("notificationid");
    var ContentReference = $(this).data("contentreference");
    var MovementVehicleType = $(this).data("movementvehicletype");
    var isHistoric = $(this).data("ishistoric");
    var isExistVr1So = $(this).closest('tr').data('isexistvr1so');
    ManageMovement(MovementRevisionId, MovementVersionId, MovementType, VehicleType, MovementStatus, HaulierMnemonic,
        ProjectESDALReference, ApplicationRevisionNo, MovementVersionNumber, ProjectId, NotificationId, ContentReference, MovementVehicleType, isHistoric);
});
$('body').on('click', '.loadappvehicleroute', function (e) {

    e.preventDefault();
    var MovementVersionId = $(this).data('movementversionid');
    var MovementType = $(this).data('movementtype');
    var ContentRefNo = $(this).data('contentrefno');
    var ApplicationRevisionId = $(this).data('applicationrevisionid');
    var isExistVr1So = $(this).closest('tr').data('isexistvr1so');
    var IsHistoric = $(this).data('ishistoric');

    LoadApplicationVehicleRoute(MovementVersionId, MovementType, ContentRefNo, ApplicationRevisionId, IsHistoric);

});
$('body').on('click', '#loadnotificationroute', function (e) {

    e.preventDefault();
    var NotificationId = $(this).data('notificationid');

    LoadNotificationRoute(NotificationId);
    return false;
});

$('body').on('click', '.routepartlist-proposed', function (e) {

    e.preventDefault();
    var MovementVersionId = $(this).data('movementversionid');
    var VersionStatus = $(this).data('versionstatus');
    routepartlist(MovementVersionId, VersionStatus);
    return false;
});
$('body').on('click', '.routepartlist-vr1route', function (e) {

    e.preventDefault();
    var ApplicationRevisionId = $(this).data('applicationrevisionid');

    routepartlist(ApplicationRevisionId);
    return false;
});
$('body').on('click', '.selectprevitmovementsvehicle', function (e) {
    var VAnalysisId = $(this).data("vanalysisid");
    var VPrj_Status = $(this).data("vprj-status");
    var Vhauliermnemonic = $(this).data("vhauliermnemonic");
    var Vesdalref = $(this).data("vesdalref");
    var Vprojectid = $(this).data("vprojectid");
    var Vtype = $(this).data("vtype");
    SelectPrevitMovementsVehicle(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid, Vtype);
});
$('body').on('click', '.selectcurrentmovementsroute', function (e) {
    var VAnalysisId = $(this).data("vanalysisid");
    var VPrj_Status = $(this).data("vprj-status");
    var Vhauliermnemonic = $(this).data("vhauliermnemonic");
    var Vesdalref = $(this).data("vesdalref");
    var Vprojectid = $(this).data("vprojectid");
    var Vtype = $(this).data("vtype");
    SelectCurrentMovementsRoute(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid, Vtype);
});
