
var ApprevId;
var VR1_Applciation;
var esdalref;
var revisionno;
var revision_no;
var versionno;
var versionId;
var projectid;
var Work_status;
let movement_Type;
var OrgID;
var chk_status;
var sort_user_id;
var checker_id;
var proj_status;

function ProjectOverviewLeftPanelInit() {
    ApprevId = $('#ApprevId').val();
    VR1_Applciation = "";
    esdalref = $('#esdalref').val();
    revisionno = $('#revisionno').val();
    revision_no = $('#arev_no').val();
    versionno = $('#versionno').val();
    versionId = $('#versionId').val();
    projectid = $('#projectid').val();
    Work_status = $('#hdnWork_Status').val();
    movement_Type = 207001;

    OrgID = $('#OrganisationId').val();
    chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    proj_status = $('#AppStatusCode').val() ? $('#AppStatusCode').val() : 0;

    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
        $('#tr_decline').hide();
        if (proj_status == 307011) {
            $('#inactive-button-candidate').show();
        }
        else {
            $('#inactive-button-candidate').hide();
        }
    }
    if ((proj_status == 307005 || proj_status == 307007) && $('#VR1Applciation').val() == "False") {
        $('#inactive-button-candidate').hide();
    }
    if (proj_status == 307006 && $('#VR1Applciation').val() == "False") {
        if (chk_status == 301003) {
            $('#inactive-button-candidate').show();
        }
        else {
            $('#inactive-button-candidate').hide();
        }
    }

    if (chk_status == 301006 && $('#VR1Applciation').val() == "False") {
        if (proj_status == 307011) {
            $('#inactive-button-candidate').show();
            $('#tr_decline').hide();
            $('#tr_withdraw').hide();
        }
        else {
            $('#tr_decline').hide();
            $('#inactive-button-candidate').hide();
            $('#tr_qa_check').hide();
        }
    }

    if ($('#VR1Applciation').val() === "True") {
        $.ajax({
            url: "../Workflow/GetSORTVR1ProcessingNextTask", /*Expected route : ../SORTApplication/GetSORTVR1ProcessingNextTask */
            type: 'GET',
            data: {
                esdalReference: hauliermnemonic + '_' + esdalref
            },
            dataType: "json",
            beforeSend: function () {

            },
            success: function (result) {
                if (result.nextTask.length > 1) {
                    console.log('NEXT TASK :' + result.nextTask);
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
    else {
        $.ajax({
            url: "../Workflow/GetSORTSOProcessingNextTask", /*Expected route : ../SORTApplication/GetSORTSOProcessingNextTask */
            type: 'GET',
            data: {
                esdalReference: hauliermnemonic + '_' + esdalref
            },
            dataType: "json",
            beforeSend: function () {

            },
            success: function (result) {
                if (result.nextTask.length > 1) {
                    console.log('NEXT TASK :' + result.nextTask);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
    stopAnimation();
}
$(document).ready(function () {
    $('body').on('click', '#inactive-button-allocate', function (e) { e.preventDefault(); Select_AllocatePOP(); });
    $('body').on('click', '#tr_decline', function (e) { e.preventDefault(); SortDecline(); });
    $('body').on('click', '#tr_withdraw', function (e) { e.preventDefault(); SortWithdraw(); });
    $('body').on('click', '#tr_unwithdraw', function (e) { e.preventDefault(); SortUnWithdraw(); });
    $('body').on('click', '#btnMovAgreefuntchck', function (e) { e.preventDefault(); Mov_Agree_funt_chck(); });
    $('body').on('click', '#btnMovUnagreefunt', function (e) { e.preventDefault(); Mov_Unagree_funt(); });
    $('body').on('click', '#btnCreatemovementversionClick', function (e) { e.preventDefault(); Createmovementversion_Click(); });
    $('body').on('click', '#btnDistributeMovement', function (e) { e.preventDefault(); DistributeMovement(); });
    $('body').on('click', '#btnDistributeAgreedRoute', function (e) { e.preventDefault(); DistributeMovement(); });
    $('body').on('click', '#btnCreateAmendementOrder', function (e) { e.preventDefault(); CreateAmendementOrder(); });
    $('body').on('click', '#inactive-button-candidate', function (e) { e.preventDefault(); CandidateCreateversion_Click(); });
    $('body').on('click', '#imgCreateNew', function (e) { e.preventDefault(); createnew(); });
    $('body').on('click', '#btnVR1approvalChck', function (e) { e.preventDefault(); VR1approvalChck(); });
    $('body').on('click', '#btnGenerateVR1Number', function (e) { e.preventDefault(); GenerateVR1Number(); });
    $('body').on('click', '#td_VR1_doc', function (e) { e.preventDefault(); GenerateVR1Document(); });
    $('body').on('click', '#btnAcknowledgementClick', function (e) { e.preventDefault(); Acknowledgement_Click(); });
    $('body').on('click', '#btnCandidateWorkAllocation', function (e) {
        e.preventDefault();
        var checking = $(this).data('checking');
        var sChecking = $(this).data('schecking');
        CandidateWorkAllocation(checking, sChecking);
    });
    $('body').on('click', '#tr_qa_check', function (e) {
        e.preventDefault();
        var checking = $(this).data('qachecking');
        CandidateWorkAllocation(checking, checking);
    });
    $('body').on('click', '#btnSignOffVr1Application', function (e) {
        e.preventDefault();
        var SignOff = $(this).data('signoff');
        var FChecking = $(this).data('fchecking');
        CandidateWorkAllocation(SignOff, FChecking);
    });
    $('body').on('click', '#btnCompleteChecking', function (e) {
        e.preventDefault();
        var CompleteChecking = $(this).data('completechecking');
        var CSChecking = $(this).data('cschecking');
        CandidateWorkAllocation(CompleteChecking, CSChecking);
    });
    $('body').on('click', '#btnCompleteQAChecking', function (e) {
        e.preventDefault();
        var QAChecking = $(this).data('qachecking');
        var CQAChecking = $(this).data('cqachecking');
        CandidateWorkAllocation(QAChecking, CQAChecking);
    });
    $('body').on('click', '#btnSignOff', function (e) {
        e.preventDefault();
        var SignOff = $(this).data('signoff');
        var CFChecking = $(this).data('cfchecking');
        CandidateWorkAllocation(SignOff, CFChecking);
    });
});
function ReviseSO(ESDAL_Reference) {
    CloseWarningPopup();
    var is_replan = 0;
    $.ajax({
        url: "../Application/ReviseSOApplication",
        type: 'post',
        data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode },
        success: function (data) {
            if (VR1_Applciation == "True") {
                movement_Type = 207002;
                var url = '../Movements/PlanMovement' + EncodedQueryString('appRevisionId=' + data.ApplicationRevisionId + '&cloneRevise=2&appVersionId=' + data.VersionId + '&vehicleType=' + data.SubMovementClass + '&movementId=' + data.MovementId + '&movementType=' + movement_Type);
                window.location = url;
            }
            else {
                movement_Type = 207001;
                var url = '../Movements/PlanMovement' + EncodedQueryString('appRevisionId=' + data.ApplicationRevId + '&cloneRevise=2&appVersionId=' + versionId + '&vehicleType=' + data.VehicleClassification + '&movementId=' + data.MovementId + '&movementType=' + movement_Type);
                window.location = url;
            }
            $("#overlay").hide();
            $('.loading').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}
function WarningBrokenPopUp() {
    CloseWarningPopup();
    window.location = newUrl;
}
function ReviseVR1(EsdalrefCode) {
    CloseWarningPopup();
    $("#overlay").show();
    $('.loading').show();
    $.ajax({
        url: "../SORTApplication/ReviseVR1Application",
        type: 'post',
        data: { apprevid: ApprevId, ESDALRefCode: EsdalrefCode },
        success: function (data) {
            if (data != 0) {
                window.location.href = "../SORTApplication/SORTListMovemnets" + EncodedQueryString("SORTStatus=CreateVR1&cloneapprevid=" + data + "&revisionId=" + data + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&arev_no=' + revision_no + '&apprevid=' + data + '&VR1Applciation=' + VR1_Applciation + '&pageflag=2' + '&EditRev=true' + '&WorkStatus=' + Work_status + '&Checker=' + Checker + '&Owner=' + Owner.replace(/ /g, '%20'));
            }
            $("#overlay").hide();
            $('.loading').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}