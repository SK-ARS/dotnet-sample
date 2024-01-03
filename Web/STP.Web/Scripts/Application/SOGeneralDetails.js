var esdal_ref = "";
var Flag_App_Status = "";
var LatestAppRevID = "";
var newUrl = "";
var hf_VersionId;
var hf_EnteredBySORT;
var hf_ApplicationRevId;
var hf_ESDALReference;
var hf_VersionStatus;
var hf_NumberofPieces;
var hf_MovementDateFrom;
var hf_MovementDateTo;
var hf_ProjectId;
var hf_revision_no;
var hf_version_no;
var inputData;
function SoGeneralDetailInit() {
    hf_VersionId = $('#hf_VersionId').val();
    hf_EnteredBySORT = $('#hf_EnteredBySORT').val();
    hf_ApplicationRevId = $('#hf_ApplicationRevId').val();
    hf_ESDALReference = $('#hf_ESDALReference').val();
    hf_VersionStatus = $('#hf_VersionStatus').val();
    hf_NumberofPieces = $('#hf_NumberofPieces').val();
    hf_MovementDateFrom = $('#hf_MovementDateFrom').val();
    hf_MovementDateTo = $('#hf_MovementDateTo').val();
    hf_ProjectId = $('#hf_ProjectId').val();
    hf_revision_no = $('#revisionno').val();
    hf_version_no = $('#versionno').val();
    var Helpdesk_redirect = $('#hf_Helpdesk_redirect').val();
    if (Helpdesk_redirect == "true") {
        $("#menu-buttons").hide();
    }
}
$(document).ready(function () {
    $('body').on('click', '#PrintProposedReport', function (e) { e.preventDefault(); PrintProposedReport(this); });
    $('body').on('click', '#PrintAgreedReport', function (e) { e.preventDefault(); PrintAgreedReport(this); });
    $('body').on('click', '#notify', function (e) {
        e.preventDefault();
        inputData = { versionId: hf_VersionId, MaxPieces: hf_NumberofPieces, MoveStartDate: hf_MovementDateFrom, MoveEndDate: hf_MovementDateTo, ApplrevisionId: hf_ApplicationRevId, isVR1: 0, versionStatus: hf_VersionStatus};
        CheckIsBroken({ VersionId: inputData.versionId }, function (response) {
            GetBrokenRouteNotifyAppInitial(false, inputData, response);
        });
    });
    $('body').on('click', '#revise', function (e) {
        e.preventDefault();
        if (hf_EnteredBySORT == 0) {
            var MSG = "Click Yes to create a new version of \"" + hf_ESDALReference + '\" for editing.';
            ShowWarningPopupCloneRenotif(MSG, function () {
                $('#WarningPopup').modal('hide');
                ReviseApplication(hf_ESDALReference, hf_revision_no, hf_ApplicationRevId, 'special order', hf_VersionId, hf_version_no);
            });
        }
        else {
            ShowErrorPopup('SORT created application cannot be revised from haulier!');
        }
    });
    $('body').on('click', '#clone', function (e) {
        e.preventDefault();
        var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;
        CloneApplication(hf_ESDALReference, hf_VersionId, AppRevId, 'so');
    });
    $('body').on('click', '#WithdrawSOApp.SOGeneralDetails', function () { WithdrawSoApplication(); });
    $('body').on('click', '.ShowHideDetails', function (e) { e.preventDefault(); ShowHideDetails(this); });
});
$('#back-link').click(function () {
    history.go(-1);
});
function WithdrawSoApplication() {
    deleteFlag = 1;
    if (hf_EnteredBySORT == 0) {
        if (hf_ESDALReference != null && hf_ESDALReference != 0) {
            esdal_ref = hf_ESDALReference;
        }
        Msg = "Do you want to withdraw application " + hf_ESDALReference + "?";
        ShowWarningPopup(Msg, 'WithdrawSoApp');
    }
    else {
        ShowErrorPopup('SORT created application cannot be withdrawn from haulier!');
    }
}
function WithdrawSoApp() {
    CloseWarningPopup();
    var project_id = hf_ProjectId;
    CheckLatestAppStatusSO(project_id);    
}
function CheckLatestAppStatusSO(Proj_ID) {// && app status checked for #7855
    $.ajax({
        url: "../Application/CheckLatestAppStatus",
        type: 'POST',
        cache: false,
        async: false,
        data: { Project_ID: Proj_ID },
        beforeSend: function () {
            startAnimation();
        },
        success: function (Result) {
            var dataCollection = Result;
            if (dataCollection.result.ApplicationStatus > 0) {
                Flag_App_Status = dataCollection.result.ApplicationStatus;
                LatestAppRevID = dataCollection.result.ApplicationRevId;

                if (Flag_App_Status != 308001) {//app status checked for #7855
                    $.ajax({
                        url: "../Application/WithdrawApplication",
                        type: 'POST',
                        cache: false,
                        data: { Project_ID: hf_ProjectId, Doc_type: 'Special Order', EsdalRefNumber: hf_ESDALReference, app_rev_id: hf_ApplicationRevId },
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (result) {
                            if (result.Success) {
                                var Msg = "\"" + esdal_ref + '\" application withdrawn successfully';
                                ShowSuccessModalPopup(Msg, 'NavigateToSamePage');
                            }
                            else {
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
                else {
                    var Msg = "A WIP application exists for the current project and hence cannot be withdrawn. Please delete the WIP application to continue.";
                    ShowErrorPopup(Msg);
                }
            }
            else {
                ShowErrorPopup("Error occurred while withdraw");
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
function PrintProposedReport(_this) {
    var ESDALREf = $(_this).attr('esdalreff');
    var ContactId = $(_this).attr('contactid');
    var OrganisationId = $(_this).attr('organisationid');

    var link = "../Notification/HaulierProposedRouteDocument?esdalRefNo=" + ESDALREf + "&organisationId=" + OrganisationId + "&contactId=" + ContactId + "";
    window.open(link, '_blank');
}
function PrintAgreedReport(_this) {
    var ESDALREf = $(_this).attr('esdalreff');
    var ContactId = $(_this).attr('contactid');
    var OrderNo = $(_this).attr('orderno');

    var link = "../Notification/HaulierAgreedRouteDocument?esDALRefNo=" + ESDALREf + "&order_no=" + OrderNo + "&contactId=" + ContactId + "";
    window.open(link, '_blank');
}
function ShowHideDetails(_this) {
    var target = parseInt($(_this).attr('target'));
    switch (target) {
        case 1:
            //contact-details
            if (document.getElementById('contact-details').style.display !== "none") {
                document.getElementById('contact-details').style.display = "none"
                document.getElementById('chevlon-up-icon1').style.display = "none"
                document.getElementById('chevlon-down-icon1').style.display = "block"
            }
            else {
                document.getElementById('contact-details').style.display = "block"
                document.getElementById('chevlon-up-icon1').style.display = "block"
                document.getElementById('chevlon-down-icon1').style.display = "none"
            }
            break;
        case 2:
            //div_notes_from_ha
            if (document.getElementById('div_notes_from_ha').style.display !== "none") {
                document.getElementById('div_notes_from_ha').style.display = "none"
                document.getElementById('chevlon-up-icon2').style.display = "none"
                document.getElementById('chevlon-down-icon2').style.display = "block"
            }
            else {
                document.getElementById('div_notes_from_ha').style.display = "block"
                document.getElementById('chevlon-up-icon2').style.display = "block"
                document.getElementById('chevlon-down-icon2').style.display = "none"
            }
            break;
        case 3:
            //div_notes_to_ha
            if (document.getElementById('div_notes_to_ha').style.display !== "none") {
                document.getElementById('div_notes_to_ha').style.display = "none"
                document.getElementById('chevlon-up-sog-icon3').style.display = "none"
                document.getElementById('chevlon-down-sog-icon3').style.display = "block"
            }
            else {
                document.getElementById('div_notes_to_ha').style.display = "block"
                document.getElementById('chevlon-up-sog-icon3').style.display = "block"
                document.getElementById('chevlon-down-sog-icon3').style.display = "none"
            }
            break;
        case 4:
            //div_distribution_notes
            if (document.getElementById('div_distribution_notes').style.display !== "none") {
                document.getElementById('div_distribution_notes').style.display = "none"
                document.getElementById('chevlon-up-sog-icon4').style.display = "none"
                document.getElementById('chevlon-down-sog-icon4').style.display = "block"
            }
            else {
                document.getElementById('div_distribution_notes').style.display = "block"
                document.getElementById('chevlon-up-sog-icon4').style.display = "block"
                document.getElementById('chevlon-down-sog-icon4').style.display = "none"
            }
            break;
        case 5:
            //req-notes
            if (document.getElementById('req-notes').style.display !== "none") {
                document.getElementById('req-notes').style.display = "none"
                document.getElementById('chevlon-up-sog-icon5').style.display = "none"
                document.getElementById('chevlon-down-sog-icon5').style.display = "block"
            }
            else {
                document.getElementById('req-notes').style.display = "block"
                document.getElementById('chevlon-up-sog-icon5').style.display = "block"
                document.getElementById('chevlon-down-sog-icon5').style.display = "none"
            }
            break;
        case 6:
            //req-notes
            if (document.getElementById('special-orders').style.display !== "none") {
                document.getElementById('special-orders').style.display = "none"
                document.getElementById('chevlon-up-sog-icon6').style.display = "none"
                document.getElementById('chevlon-down-sog-icon6').style.display = "block"
            }
            else {
                document.getElementById('special-orders').style.display = "block"
                document.getElementById('chevlon-up-sog-icon6').style.display = "block"
                document.getElementById('chevlon-down-sog-icon6').style.display = "none"
            }
            break;
    }
}
function NavigateToSamePage() {
    window.location.href = '/Movements/MovementList';
}
