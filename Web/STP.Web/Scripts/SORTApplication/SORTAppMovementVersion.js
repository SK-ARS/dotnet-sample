var Enter_BY_SORT;
var LatVerVal;
function SortAppMovementVersionInit() {
    Enter_BY_SORT = $('#EnterBySort').val();
    LatVerVal = $('#hf_LatVer').val();
    if ($('#hf_LatVer').val() != 0 && LatVerVal != null) {
        $('#MovLatestVer').val(LatVerVal);
    }
    $('#analysis_id').val($('#hf_LatestAnalysisId').val());
}
$(document).ready(function () {
    $('body').on('click', '.currentmovement', function (e) {
        e.preventDefault();
        CurreMovemenRouteListing(this);
    });
    $('body').on('click', '.currentmovementvehicle', function (e) {
        e.preventDefault();
        CurreMovemenVehicleListListing(this);
    });
    $('body').on('click', '.showversion', function (e) {
        e.preventDefault();
        ShowingVersions(this);
    });
});
function CurreMovemenRouteListing(e) {
    var param1 = $(e).attr("arg1");
    var param2 = $(e).attr("arg2");
    CurreMovemenRouteList(param1, param2);
}
function CurreMovemenVehicleListListing(e) {
    var param1 = $(e).attr("arg1");
    var param2 = $(e).attr("arg2");
    CurreMovemenVehicleList(param1, param2);
}
function ShowingVersions(e) {
    var param1 = $(e).attr("arg1");
    var param2 = $(e).attr("arg2");
    var param3 = $(e).attr("arg3");
    var param4 = $(e).attr("arg4");
    var param5 = $(e).attr("arg5");
    ShowVersions(param1, param2, param3, param4, param5);
}
function ShowVersions(vern_vo, vern_id, rev_no, rev_Id, esdal_history) {
    var LatestVer = $('#hf_LatVer').val();
    var Owner = $('#hf_OwnerName').val();
    var Checker = $('#hf_Checker').val();
    var Work_status = $('#hdnWork_Status').val();
    var Org_Id = $('#mov_ver_' + vern_id).data('id');

    startAnimation();
    window.location.href = '../SORTApplication/SORTListMovemnets' + EncodedQueryString('SORTStatus=MoveVer&projecid=' + ProjectID + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + reduceddetailed + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionId=' + rev_Id + '&movementId=' + movementId + '&apprevid=' + rev_Id + '&revisionno=' + rev_no + '&OrganisationId=' + Org_Id + '&versionno=' + vern_vo + '&versionId=' + vern_id + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + reduceddetailed + '&pageflag=' + Pageflag + '&esdal_history=' + esdal_history + '&LatestVer=' + LatestVer + '&WorkStatus=' + Work_status + '&EnterBySORT=' + Enter_BY_SORT + '&Checker=' + Checker + '&Owner=' + Owner.replace(/ /g, '%20'));
    stopAnimation();

}