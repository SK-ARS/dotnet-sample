var NotifID = 0;
var AnalID = 0;
var ContentREfNo = "";
var VehCode = 0;
var clone = 0;
var esdal_ref = "";
var Flag_App_Status = "";
var LatestAppRevID = "";
var newUrl = '';
var cloneRevise = 0;
var hf_versionid;
var hf_revisionId;
var hf_ReducedDetails;
var hf_ESDALReference;
var hf_MaxPiecesPerLoad;
var hf_MovementDateFrom;
var hf_MovementDateTo;
var hf_revision_no;
var hf_version_no;
var hf_versionStatus;
var inputData;

function VR1GeneralDetailInit() {
    hf_versionid = $('#hf_versionid').val();
    hf_revisionId = $('#hf_revisionId').val();
    hf_ReducedDetails = $('#hf_ReducedDetails').val();
    hf_ESDALReference = $('#hf_ESDALReference').val();
    hf_MaxPiecesPerLoad = $('#hf_MaxPiecesPerLoad').val();
    hf_MovementDateFrom = $('#hf_MovementDateFrom').val();
    hf_MovementDateTo = $('#hf_MovementDateTo').val();
    hf_revision_no = $('#revisionno').val();
    hf_version_no = $('#versionno').val();
    hf_versionStatus = $('#versionStatus').val();
    if ($('#hf_Helpdest_redirect').val() == "true") {
        $('.norifyvr1').css("display", "none");
        $('.vr1reviseclone').css("display", "none");
        $('#WithdrawSOApp').css("display", "none");
        $('#vr1applyforfolder').css("display", "none");
        $('#vr1IdFolderName').css("display", "none");
    }
    if (hf_ESDALReference != 0 || hf_ESDALReference != null) {
        $('#hdn_esdalrefnumberVR1').val(hf_ESDALReference);
    }
}
$(document).ready(function () {
    $('body').on('click', '.vr1reviseclone', function (e) {
        e.preventDefault();
        clone = $(this).data('clone');
        var msg = '';
        if (clone > 0) {
            msg = 'Click Yes to create a new application which is a cloned version of ' + hf_ESDALReference + ' application.';
            ShowWarningPopupCloneRenotif(msg, function () {
                $('#WarningPopup').modal('hide');
                CloneApplication(hf_ESDALReference, hf_versionid, hf_revisionId, 'vr1');
            });
        }
        else {
            msg = 'Click Yes to create a new version of ' + hf_ESDALReference + ' for editing.';
            ShowWarningPopupCloneRenotif(msg, function () {
                $('#WarningPopup').modal('hide');
                ReviseApplication(hf_ESDALReference, hf_revision_no, hf_revisionId, 'vr1', hf_versionid, hf_version_no);
            });
        }
    });
    $('body').on('click', '#WithdrawSOApp.VR1GeneralDetails', function (e) {
        e.preventDefault();
        WithdrawVR1Application(this);
    });
    $('body').on('click', '.norifyvr1', function (e) {
        e.preventDefault();
        inputData = { versionId: hf_versionid, MaxPieces: hf_MaxPiecesPerLoad, MoveStartDate: hf_MovementDateFrom, MoveEndDate: hf_MovementDateTo, ApplrevisionId: hf_revisionId, isVR1: 1, versionStatus: hf_versionStatus };
        CheckIsBroken({ VersionId: inputData.versionId }, function (response) {
            GetBrokenRouteNotifyAppInitial(true, inputData, response);
        });
    });
    $('body').on('click', '#vr1applyforfolder', function (e) {
        e.preventDefault();
        VR1ApplyFolder(this);
    });
});

//For apply folders
function VR1ApplyFolder () {
    let projectID = $('#projid').val();
    let folderId = $('#vr1IdFolderName').val();
    let ApplicationRevId = $('#ApplicationRevisionId').val() ? $('#ApplicationRevisionId').val() : 0;
    $.ajax({
        url: '../Application/GetSetCommonFolderDetails',
        type: 'POST',
        cache: false,
        async: false,
        data: { flag: 1, folderID: folderId, projectID: projectID, revisionID: ApplicationRevId },//flag to insert folder on perticular project Id
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            showWarningPopDialog('Folder name changed.', 'Ok', '', 'WarningCancelBtn', 'loadFeedList', 1, 'info');
        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

            stopAnimation();

        }
    });
};
function WithdrawVR1Application() {
    let project_id = $('#Project_ID').length > 0 ? $('#Project_ID').val() : 0;
    if (project_id == 0 && $('#hf_ProjectId').length > 0) 
        project_id = $('#hf_ProjectId').val()
    
    CheckLatestAppStatusVR1(project_id);// && app status checked for #7855
    
}
function WithdrawVR1App() {
    CloseWarningPopup();
    let AppRevId = $('#ApplicationRevisionId').val() ? $('#ApplicationRevisionId').val() : 0;
    let ProjectID = $('#Project_ID').val() ? $('#Project_ID').val() : 0;
    if (Flag_App_Status != 308001) {//app status checked for #7855
        $.ajax({
            url: "../Application/WithdrawApplication",
            type: 'POST',
            cache: false,
            data: { Project_ID: ProjectID, Doc_type: 'VR-1', EsdalRefNumber: hf_ESDALReference, app_rev_id: AppRevId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.Success) {
                    let Msg = "\"" + esdal_ref + '\" Application withdrawn successfully';
                    ShowSuccessModalPopup(Msg, 'linktomovementinbox');
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
                cntTr.remove();
            }
        });
    }
    else {
        let Msg = "A WIP application exists for the current project and hence cannot be withdrawn. Please delete the WIP application to continue.";
        ShowErrorPopup(Msg);
    }
}
function CheckLatestAppStatusVR1(Proj_ID) {// && app status checked for #7855
    $.ajax({
        url: "../Application/CheckLatestAppStatus",
        type: 'POST',
        cache: false,
        async: false,
        data: { Project_ID: Proj_ID },
        beforeSend: function () {
            startAnimation();
            ClearPopUp();
        },
        success: function (Result) {
            let dataCollection = Result;
            if (dataCollection.result.ApplicationStatus > 0) {
                Flag_App_Status = dataCollection.result.ApplicationStatus;
                LatestAppRevID = dataCollection.result.ApplicationRevId;

                if (hf_ESDALReference != null && hf_ESDALReference != 0) {
                    esdal_ref = hf_ESDALReference;
                }
                let Msg = "Do you want to withdraw application \"" + esdal_ref + '\" ?';
                ShowWarningPopup(Msg, 'WithdrawVR1App');
            }
            else {
                ShowErrorPopup("Error occurred while withdraw");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function linktomovementinbox() {
    CloseSuccessModalPopup();
    window.location.href = '/Movements/MovementList';
}
