var LatRev;
var VR1Applciation;
function SortAppRevisionInit() {
    LatRev = $('#hf_latrev').val();
    VR1Applciation = $('#VR1Applciation').val();
    if (VR1Applciation == "True") {
        $('#arev_no').val(LatRev);
    }
}
$(document).ready(function () {
    $('body').on('click', '#ahrefShowRevisions1', function (e) {
        e.preventDefault();
        var RevisionNo = $(this).data('revisionno');
        var RevisionID = $(this).data('revisionid');
        var VersionNo = $(this).data('versionno');
        var AppStatus = $(this).data('appstatus');
        ShowRevisions(RevisionNo, RevisionID, VersionNo, AppStatus);
    });
    $('body').on('click', '#ahrefCurrentMovementList2', function (e) {
        e.preventDefault();

        var RevisionID = $(this).data('revisionid');

        CurreMovemenVehicleList(RevisionID, 'A');
    });
    $('body').on('click', '#ahrefCurrentMovementList1', function (e) {
        e.preventDefault();
        var RevisionID = $(this).data('revisionid');
        CurreMovemenRouteList(RevisionID, 'A');
    });
    $('body').on('click', '#ahrefShowRevisions', function (e) {
        e.preventDefault();

        var RevisionNo = $(this).data('revisionno');
        var RevisionID = $(this).data('revisionid');
        var VersionNo = $(this).data('versionno');
        var AppStatus = $(this).data('appstatus');
        ShowRevisions(RevisionNo, RevisionID, VersionNo, AppStatus);
    });
});
function ShowRevisions(rev_no, rev_Id, ver_no, applStatus) {

    var OrgID = $('#OrganisationId').val();
    var Owner = $('#hf_OwnerName').val();
    var Checker = $('#hf_Checker').val();
    var VR1Applciation = $('#VR1Applciation').val();
    var Work_status = $('#hdnWork_Status').val();
    var ProjectID = $('#ProjectID').val();
    versionId = $('#versionId').val();
    var Enter_BY_SORT = $('#EnterBySort').val();
    var vehicleClass = $('#VehicleClass').val();
    var MovementType;
    if (applStatus == '308001') {

        var redirectData = EncodedQueryString("revisionId=" + rev_Id);
        var redirectUrl = '../Movements/OpenMovement';
        RedirectToPlanMovement(redirectUrl + redirectData);
    }
    else {
        startAnimation();
        window.location.href = '../SORTApplication/SORTListMovemnets' + EncodedQueryString('SORTStatus=Revisions&projecid=' + ProjectID + "&OrganisationId=" + OrgID + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + reduceddetailed + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionId=' + rev_Id + '&versionId=' + versionId + '&movementId=' + movementId + '&apprevid=' + rev_Id + '&revisionno=' + rev_no + '&versionno=' + versionno + '&pageflag=' + Pageflag + '&arev_no=' + rev_no + '&arev_Id=' + rev_Id + '&ver_no=' + ver_no + '&WorkStatus=' + Work_status + '&Checker=' + Checker + '&EnterBySORT=' + Enter_BY_SORT + '&Owner=' + Owner.replace(/ /g, '%20'));
        stopAnimation();
    }

}