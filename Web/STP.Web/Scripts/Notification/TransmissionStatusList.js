$(document).ready(function () {
    $('body').on('click', '#close-trans', function (e) { e.preventDefault(); closeFilters_trans(this); });

    var url = geturl(location.href);
    if (url == 'NotificationDisplayNotification') {
        $("#trans").hide();
    }
    $('body').on('click', '#trans', function (e) { e.preventDefault(); openFilters_trans(this); });
    $('body').on('click', '.btn_ViewProposal', function (e) { e.preventDefault(); btn_ViewProposal(this); });
    $('body').on('click', '.btn_ViewReproposed', function (e) { e.preventDefault(); btn_ViewReproposed(this); });
    $('body').on('click', '.btn_ViewAgreed', function (e) { e.preventDefault(); btn_ViewAgreed(this); });
    $('body').on('click', '.btn_ViewAgreedRevised', function (e) { e.preventDefault(); btn_ViewAgreedRevised(this); });
    $('body').on('click', '.btn_ViewAgreedRecleared', function (e) { e.preventDefault(); btn_ViewAgreedRecleared(this); });
    $('body').on('click', '.RetransmitButton', function (e) {
        e.preventDefault();
        RetransmitButton(this);
    });

    $('body').on('click', '#transamissionListPagination a', function (e) {
        e.preventDefault();
        startAnimation();
        $.ajax({
            url: this.href,
            type: 'GET',
            cache: false,
            async: false,
            beforeSend: function () {
                
            },
            success: function (response) {
                var divTransmission = $(response).find('#divTransmission');
                $("#divTransmission").html(divTransmission);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error(errorThrown);
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    });
});
function btn_ViewProposal(_this) {
    var trans_id = $(_this).attr('transmissionid');
    var cntId = $(_this).attr('contactid');
    var Org_id = $(_this).attr('organisationid');
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;

    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewProposedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=proposal&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() != '1') {
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
function RetransmitButton(_this) {
    startAnimation();
    var online = $(_this).attr('arg1');
    var TransmissionId = $(_this).attr('id');
    //var mailid = $(_this).attr('emailid');
    $("#dialogue").load('../DistributionStatus/Retransmit?transmissionId=' + TransmissionId, function () {
        stopAnimation();
        $("div.modal-backdrop").remove();
        $("#dialogue").show();
        $("#overlay").show();
        //$("#exampleModalCenter4").find($('#transmission_id')).val(TransmissionId);
        $("#retransmitPopup").find($('#transmission_id')).val(TransmissionId);
        if (online!="") {
            $("#txt_email").val("");
        }
       // $("#txt_email").val("");
        //$("#exampleModalCenter4").modal('show');
        $("#retransmitPopup").modal('show');
    }
    );

}

function btn_ViewReproposed(_this) {
    var trans_id = $(_this).attr('transmissionid');
    var cntId = $(_this).attr('contactid');
    var Org_id = $(_this).attr('organisationid');
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewReProposedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=reproposal&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() != '1') {
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

function btn_ViewAgreed(_this) {
    
   // startAnimation();
    var trans_id = $(_this).attr('transmissionid');
    var cntId = $(_this).attr('contactid');
    var Org_id = $(_this).attr('organisationid');
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewAgreedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=agreement&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() != '1') {
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

function btn_ViewAgreedRevised(_this) {
    var trans_id = $(_this).attr('transmissionid');
    var cntId = $(_this).attr('contactid');
    var Org_id = $(_this).attr('organisationid');
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewAmendmentToAgreementReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&contactId=' + cntId + '&DocType=amendment to agreement&VersionId=' + $('#versionId').val();
    if ($('#hf_NoDataFound').val() != '1') {
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

function btn_ViewAgreedRecleared(_this) {
    var trans_id = $(_this).attr('transmissionid');
    var cntId = $(_this).attr('contactid');
    var Org_id = $(_this).attr('organisationid');
    var esdalRef = hauliermnemonic + "/" + esdalref + "/S" + versionno;
    //var Org_id = $('#OrganisationId').val();
    var link = '../SORTApplication/ViewAgreedReport?esdalRefno=' + esdalRef + '&trans_id=' + trans_id + '&OrgId=' + Org_id + '&DocType=agreement&contactId=' + cntId;
    if ($('#hf_NoDataFound').val() != '1') {
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
    $('#filters_trans').css("width", "350px");
    $('#banner').css("filter", "brightness(0.5)");
    $('#banner').css("background", "white");
    $('#navbar').css("filter", "brightness(0.5)");
    $('#navbar').css("background", "white");

    function myFunction(x) {
        if (x.matches) { // If media query matches
            $('#filters_trans').css("width", "200px");
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters_trans() {
    $('#filters_trans').css("width", "0");
    $('#banner').css("filter", "unset");
    $('#navbar').css("filter", "unset");
}
