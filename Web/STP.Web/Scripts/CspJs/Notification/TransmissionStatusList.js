$(document).ready(function () {
    $("#close-trans").on('click', closeFilters_trans);
    var url = geturl(location.href);
    if (url == 'NotificationDisplayNotification') {
        $("#trans").hide();
    }
    $("#trans").on('click', openFilters_trans);
    $(".btn_ViewProposal").on('click', btn_ViewProposal);
    $(".btn_ViewReproposed").on('click', btn_ViewReproposed);
    $(".btn_ViewAgreed").on('click', btn_ViewAgreed);
    $(".btn_ViewAgreedRevised").on('click', btn_ViewAgreedRevised);
    $(".btn_ViewAgreedRecleared").on('click', btn_ViewAgreedRecleared);
    $(".RetransmitButton").on('click', RetransmitButton);
});
function btn_ViewProposal(data) {
    var trans_id = data.currentTarget.attributes.TransmissionId.value;
    var cntId = data.currentTarget.attributes.ContactId.value;
    var Org_id = data.currentTarget.attributes.OrganisationId.value;
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;

    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewProposedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=proposal&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() == '1') {
        if ($('#hf_catcherr').val() == "True") {
            showWarningPopDialog('No data found.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        } else {
            window.open(link, "_blank");
        }
    }
    else {
        showWarningPopDialog('Functionality not implemented ...', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}
function RetransmitButton(data) {
    startAnimation();
    var TransmissionId = data.currentTarget.attributes.Id.value;
    $("#dialogue").load('../DistributionStatus/Retransmit?transmissionId=' + TransmissionId, function () {
        $("div.modal-backdrop").remove();
        $("#dialogue").show();
        $("#overlay").show();
        $("#exampleModalCenter4").find($('#transmission_id')).val(TransmissionId);
        $("#exampleModalCenter4").modal('show');
    }
    );

}

function btn_ViewReproposed(data) {
    var trans_id = data.currentTarget.attributes.TransmissionId.value;
    var cntId = data.currentTarget.attributes.ContactId.value;
    var Org_id = data.currentTarget.attributes.OrganisationId.value;
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewReProposedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=reproposal&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() == '1') {
        if ($('#hf_catcherr').val() == "True") {
            showWarningPopDialog('No data found.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        } else {
            window.open(link, "_blank");
        }
    }
    else {
        showWarningPopDialog('Functionality not implemented ...', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}

function btn_ViewAgreed(data) {
    var trans_id = data.currentTarget.attributes.TransmissionId.value;
    var cntId = data.currentTarget.attributes.ContactId.value;
    var Org_id = data.currentTarget.attributes.OrganisationId.value;
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewAgreedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=agreement&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() == '1') {
        if ($('#hf_catcherr').val() == "True") {
            showWarningPopDialog('No data found.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        } else {
            window.open(link, "_blank");
        }
    }
    else {
        showWarningPopDialog('Functionality not implemented ...', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}

function btn_ViewAgreedRevised(data) {
    var trans_id = data.currentTarget.attributes.TransmissionId.value;
    var cntId = data.currentTarget.attributes.ContactId.value;
    var Org_id = data.currentTarget.attributes.OrganisationId.value;
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewAmendmentToAgreementReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=amendment to agreement&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() == '1') {
        if ($('#hf_catcherr').val() == "True") {
            showWarningPopDialog('No data found.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        } else {
            window.open(link, "_blank");
        }
    }
    else {
        showWarningPopDialog('Functionality not implemented ...', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}

function btn_ViewAgreedRecleared(data) {
    var trans_id = data.currentTarget.attributes.TransmissionId.value;
    var cntId = data.currentTarget.attributes.ContactId.value;
    var Org_id = data.currentTarget.attributes.OrganisationId.value;
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewAgreedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&DocType=agreement&contactId=' + cntId;
    if ($('#hf_NoDataFound').val() == '1') {
        if ($('#hf_catcherr').val() == "True") {
            showWarningPopDialog('No data found.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        } else {
            window.open(link, "_blank");
        }
    }
    else {
        showWarningPopDialog('Functionality not implemented ...', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}


// showing filter-settings
function openFilters_trans() {

    //document.getElementById("filters").style.width = "500px";
    ////document.getElementById("TransmissionStatus").style.filter = "brightness(1)";
    //////document.getElementById("TransmissionStatus").style.background = "white";
    //document.getElementById("divTransmission").style.filter = "brightness(0.5)";

    //document.getElementById("navbar").style.filter = "brightness(0.5)";
    //document.getElementById("navbar").style.background = "white";

    ////document.getElementById("banner").style.filter = "brightness(0.5)";
    ////document.getElementById("banner").style.background = "white";
    ////document.getElementById("filters").style.filter = "brightness(1.0)";
    ////document.getElementById("filters").style.background = "white";
    ////document.getElementById("TransmissionStatus").style.filter = "brightness(1)";

    ////function myFunction(x) {
    ////    if (x.matches) { // If media query matches
    ////        document.getElementById("filters").style.width = "200px";
    ////    }
    ////}
    //var x = window.matchMedia("(max-width: 770px)")
    //myFunction(x) // Call listener function at run time
    //x.addListener(myFunction)
    document.getElementById("filters_trans").style.width = "350px";
    document.getElementById("banner").style.filter = "brightness(0.5)";
    document.getElementById("banner").style.background = "white";
    document.getElementById("navbar").style.filter = "brightness(0.5)";
    document.getElementById("navbar").style.background = "white";
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters_trans").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters_trans() {
    document.getElementById("filters_trans").style.width = "0";
    document.getElementById("banner").style.filter = "unset";
    document.getElementById("navbar").style.filter = "unset";


}
