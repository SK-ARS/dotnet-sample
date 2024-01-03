$(document).ready(function () {
    $('body').on('click', '#text', function (e) {
        $('#errorcomment').css("display", "none");
    });
    $('body').on('click', '#spanCloseGeneralPopup', CloseGeneralPopup);
    $('body').on('click', '#btnSaveDistributionComment', SaveDistributionComment);
    $('body').on('click', '#btnCloseGeneralPopup', CloseGeneralPopup);
});
function DistributionCommentsInit() {
    Resize_PopUp(440);
}
function ClosePopUp() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    addscroll();
}
function validateDistributionComment() {
    var comment = $("#text").val();
    if (comment == "") {
        $("#errorcomment").show();
        return false;
    }
    else {
        SaveDistributionComment();
    }
}
function SaveDistributionComment() {
  
    CloseGeneralPopup();
    var _verno = $('#versionid').val();
    var comment = $("#text").val();
    var esdal_ref_no = hauliermnemonic + "/" + esdalref + "/S" + _verno;
    var _versionid = $('#versionid').val();
    var mov_analysisid = $('#analysis_id').val();
    var cand_analysisid = $('#candAnalysisId').val();
    var hajobfileref = $('#HaJobFileRef').val();
    var _pstatuscode = $('#AppStatusCode').val();
    var _preverdistr = $('#PreVerDistr').val();
    var projectid = $('#projectid').val();
    var revisionno = $('#revisionno').val();
    var versionno = $('#versionno').val();
    $.ajax({
        url: '../SORTApplication/SaveDistributionComments',
        type: 'POST',
        data: $("#CommentInfoForm").serialize() + "&EsdalReference=" + esdal_ref_no + "&HaulierMnemonic=" + hauliermnemonic + "&EsdalRef=" + esdalref + "&VersionNo=" + versionno + "&VersionId=" + _versionid + '&HaJobFileRef=' + hajobfileref + '&ProjectStatus=' + _pstatuscode + '&PreVersionDistr=' + _preverdistr + '&lastrevisionno=' + revisionno + '&ProjectId=' + projectid,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data.result == 1) {
                ShowSuccessModalPopup('Distribution to affected parties completed', 'Redirect_ProjectOverview');
            }
            else if (data.result == 2) {
                ShowInfoPopup('Distribution is not completed, please contact helpdesk.', 'Redirect_ProjectOverview');
            }
            else {
                ShowInfoPopup('No possible contact found for distribution.', 'Redirect_ProjectOverview');
            }
            ClosePopUp();
            stopAnimation();
        },
        error: function (xhr, textStatus, errorThrown) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });
}