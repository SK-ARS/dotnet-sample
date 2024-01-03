var Proj_Status;
var Work_status;
var isDistributed;
var chk_status;
var sort_user_id;
var checker_id;
var proj_status;
function MovementSummaryLeftPanelInit() {
    Proj_Status = $('#Proj_Status').val();
    Work_status = $('#hdnWork_Status').val();
    isDistributed = $('#VersionDistributed').val();
    if (isDistributed == 1) {
        $('#btndistributemovement').hide();
    }
    chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    proj_status = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
        $('#btncretaeso').hide();
    }
}
$(document).ready(function () {
    $('body').on('click', '#btncretaeso', function (e) {
        if ($("#SONumber").val() == "") {
            var _plannerId = $('#PlannrUserId').val();
            var _project_status = $('#Proj_Status').val();
            window.location = "/SORTApplication/CreateSpecialOrder" + EncodedQueryString("sedalno=" + hauliermnemonic + "/" + esdalref + "/S" + versionno + "&ProjectId=" + projectid + "&VersionId=" + versionId + '&PlannerId=' + _plannerId + '&ProjectStatus=' + _project_status);
        }
        else {
            CreateSpecialOrder();
        }
    });
});
function CreateSpecialOrder() {
    ShowDialogWarningPop('Do you want to create a new P number?', 'No', 'Yes', 'UpdateSpecialOrder', 'CreateSpecialOrderNew', 1, 'warning');
}
function CreateSpecialOrderNew() {

    var _plannerId = $('#PlannrUserId').val();
    var _project_status = $('#Proj_Status').val();
    var esdalReferenceNumber = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    var esdalReferenceWorkflow = hauliermnemonic + "_" + esdalref;
    window.location = "/SORTApplication/CreateSpecialOrder" + EncodedQueryString("sedalno=" + hauliermnemonic + "/" + esdalref + "/S" + versionno + "&ProjectId=" + projectid + "&VersionId=" + versionId + '&PlannerId=' + _plannerId + '&ProjectStatus=' + _project_status + '&EsdalReferenceWorkflow=' + esdalReferenceWorkflow);
}
function UpdateSpecialOrder() {

    var esdal_ref_val = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    $.ajax({
        url: '../SORTApplication/UpdateSpecialOrder',
        type: 'POST',
        data: { VersionId: versionId, ProjectId: projectid, esdal_ref: esdal_ref_val },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data == 1) {
                Redirect_ProjectOverview();
            }
        },
        complete: function () {
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
