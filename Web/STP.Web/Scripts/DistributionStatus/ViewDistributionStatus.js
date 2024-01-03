let movementData = $('#hf_movementData').val();
let userTypeID = $('#hf_userTypeID').val();
let DateTime = $('#hf_DateTime').val();
let NonESDALUSER = $('#hf_NonESDALUSER').val();
let retransmitEmail = '';
let retransmitFax = '';
let sortTypeGlobal = 1;//1-desc
let sortOrderGlobal = 1;//1-date

// Attach listener function on state changes

$(document).ready(function () {
    SelectMenu(2);
    $('body').on('click', '.RetransmitClick', function (e) {
        e.preventDefault();
        var TransmissionId = $(this).attr("transmissionId");
        var DistFlag = $(this).attr("distflag");
        RetransmitClick(TransmissionId, DistFlag);

    });
    $('body').on('click', '#check-soa', function (e) {
        e.preventDefault();
        let ESDALReference = $(this).data('esdalreference');
        let OrganisationId = $(this).data('organisationid');
        let contactId = $(this).data('contactid');
        let TransmissionId = $(this).data('transmissionid');
        let checkAs = $(this).data('checkas');

        CheckAsSOAPolice(ESDALReference, OrganisationId, contactId, TransmissionId, checkAs);
    });
    $('body').on('click', '#check-police', function (e) {
        e.preventDefault();
        let ESDALReference = $(this).data('esdalreference');
        let OrganisationId = $(this).data('organisationid');
        let contactId = $(this).data('contactid');
        let TransmissionId = $(this).data('transmissionid');
        let checkAs = $(this).data('checkas');

        CheckAsSOAPolice(ESDALReference, OrganisationId, contactId, TransmissionId, checkAs);
    });
    $('body').on('click', '.PrintDocuments', function (e) {
        e.preventDefault();
        let ESDALReference = $(this).data('esdalreference');
        let OrganisationId = $(this).data('organisationid');
        let contactId = $(this).data('contactid');
        let TransmissionId = $(this).data('transmissionid');
        let OrgTypeName = $(this).data('orgtype');
        let IsManuallyAdded = $(this).data('is_manually_added');
        let fromOrgName = $(this).data('fromOrgName');
        PrintDocument(ESDALReference, contactId, OrganisationId, TransmissionId, OrgTypeName, IsManuallyAdded, IsManuallyAdded);
    });
    $('body').on('click', '#check-haulier', function (e) {
        e.preventDefault();
        let ESDALReference = $(this).data('esdalreference');
        let TransmissionId = $(this).data('transmissionid');


        CheckAsHaulier(ESDALReference, TransmissionId);
    });
    $('body').on('click', '#filterimage', function (e) {
        e.preventDefault();
        ClearDistributionFilter(this);
    });
    $('body').on('click', '#btn_clr', function (e) {
        e.preventDefault();
        ClearDistributionFilter(this);
    });
    $('body').on('click', '#ShowSuccessPopup', function () {
        location.reload();
    });
    $('body').on('click', '#btn_search', function () {
        $("#tableheader").hide();
        $("#pageNum").val('1');
        $("#tableheader").css("display", "none");
        $("#rightpanel").find("#tableheader").css("display", "none");
    });
    $('body').on('change', '.ViewDistributionStatusPagination #pageSizeSelect', function () {

        $('#pageNum').val();
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        $('#SortTypeValue').val(sortTypeGlobal);
        $('#SortOrderValue').val(sortOrderGlobal);
        $('#filters form').submit();
    });
    $('body').on('click', '.closebtn', function (e) {
        closeFilters();
    });
    $('body').on('click', '#chk_creation_date', function (e) {
        if ($('#chk_creation_date').is(':checked')) {
            $('#txt_start_time').attr('disabled', false);
            $('#txt_end_time').attr('disabled', false);
            $('#txt_start_time').val(DateTime);
            $('#txt_end_time').val(DateTime);
        } else {
            $('#txt_start_time').attr('disabled', 'disabled');
            $('#txt_start_time').val('');
            $('#txt_end_time').attr('disabled', 'disabled');
            $('#txt_end_time').val('');
            $('#spn_error').html('');
        }
    });
    if ($('#hf_showalert').val() == "on") {
        $('#showalert').attr('checked', true);
    }
    if ($('#hf_movementData').val() != "0") {
        $('#movementData').val(movementData);
    }
    if (userTypeID != 696008) {
        $('#movementData').val(movementData);
    }
    else {
        $('#movementData').val(2);
    }
    if (NonESDALUSER == "True") {
        showWarningPopDialog('Manually added contacts cannot login to ESDAL.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    if ($('#hf_DateFilter').val() == "true") {
        $('#chk_creation_date').attr('checked', true);
        $('#txt_start_time').attr('disabled', false);
        $('#txt_end_time').attr('disabled', false);
    }
    $("#tableheader").hide();
    $("#txt_start_time").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            var endDate = $("#txt_end_time").datepicker('getDate');
            dt.setDate(dt.getDate() + 1);
            $("#txt_end_time").datepicker("option", "minDate", dt);
            if (dt > endDate) {
                $("#txt_end_time").datepicker("setDate", dt);
            }
        }
    });
    $("#txt_end_time").datepicker({
        dateFormat: "dd-M-yy",
        changeYear: true,
        changeMonth: true,
        onSelect: function (selected) {
            var dt = new Date(selected);
            dt.setDate(dt.getDate() - 1);
            $("#txt_start_time").datepicker("option", "maxDate", dt);
        }
    });
});
function ClearDistributionFilter() {
    ClearFilterData();
    ResetFilterData();
    $('#filters form').submit();
    return true;
}
function ClearFilterData() {
    $('#showalert').prop('checked', false);
    $('#esdalData').val('');
    if (userTypeID != 696008) {
        $('#movementData').val(0);
    } else {
        $('#movementData').val(2);
    }
    $('#Org_From').val('');
    $('#Org_To').val('');
    $('#txt_start_time').val('');
    $('#txt_end_time').val('');
    $('#chk_creation_date').prop('checked', false);
    $('#txt_start_time').attr('disabled', 'disabled');
    $('#txt_start_time').val('');
    $('#txt_end_time').attr('disabled', 'disabled');
    $('#txt_end_time').val('');
}
function ResetFilterData() {
    $.ajax({
        url: '../DistributionStatus/ClearHelpdeskFilter',
        type: 'POST',
        async: false,
        success: function (data) {
        }
    });
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("banner").style.filter = "unset"
    document.getElementById("navbar").style.filter = "unset";
    $("#overlay").hide();
}
function SortDistributionStatus(event, param) {
    sortOrderGlobal =param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    $('#filters form').submit();
}
function RetransmitClick(TransmissionId, DistFlag) {
    $.ajax({
        type: 'GET',
        url: '../DistributionStatus/Retransmit',
        data: { transmissionId: TransmissionId, distflag: DistFlag },
        beforeSend: function (xhr) {
            startAnimation();
        },
        success: function (result) {
            $("#dialogue").attr('style', 'width:420px;');
            $("#dialogue").html(result);
            $("#retransmitPopup").attr('style', 'display:block;');
            removescroll();
        },
        complete: function () {
            stopAnimationRetransmit();
        }
    });
}
function CheckAsHaulier(ESDALREf, trsmission_id) {
    let checkAs = "Haulier";
    startAnimation();
    $.ajax({
        type: "GET",
        url: "../Account/CheckAsSOAPoliceHaulier",
        contentType: "application/json; charset=utf-8",
        data: { ESDALREf: ESDALREf, trsmission_id: trsmission_id, checkAs: checkAs },
        datatype: "json",
        success: function (data) {
            stopAnimation();
            if (data != null) {
                Redirect(data);
            }
        },
        error: function () {
            stopAnimation();
        }
    });
}
function CheckAsSOAPolice(ESDALREf, orgid, contactid, trsmission_id, checkAs) {
    startAnimation();
    $.ajax({
        type: "GET",
        url: "../Account/CheckAsSOAPoliceHaulier",
        contentType: "application/json; charset=utf-8",
        data: { ESDALREf: ESDALREf, orgid: orgid, contactid: contactid, trsmission_id: trsmission_id, checkAs: checkAs },
        success: function (data) {
            stopAnimation();
            if (data != null) {
                Redirect(data);
            }
        },
        error: function () {
            stopAnimation();
        }
    });
}
function Redirect(data) {
    console.log(data.notif_id);
    if (data.userType_ID == 696007 || data.userType_ID == 696002) {
        window.location.href = '../Movements/AuthorizeMovementGeneral?Notificationid=' + data.notif_id + '&esdal_ref=' + data.ESDALREf + '&route=' + data.Item_Type + '&inboxId=' + data.Inbox_Id;
    } else if (data.userType_ID == 696001) {

        if (data.IsNotification == "true") {

            window.location.href = '../Notification/DisplayNotification?notificationId=' + data.notif_id + '&notificationCode=' + data.ESDALREf;
        } else {
            let esdalRefPro = data.ESDALREf.split('/');
            window.location.href = '../Application/ListSOMovements?revisionId=' + data.Revision_id + '&movementId=' + data.Veh_purpose + '&versionId=' + data.Version_id + '&hauliermnemonic=' + esdalRefPro[0] + '&esdalref=' + esdalRefPro[1] + '&revisionno=' + data.Revision_no + '&versionno=' + data.Version_no + '&apprevid=' + data.Revision_id + '&projecid=' + data.Project_id + '&pageflag=2';
        }
    }
}
function changePageSize(_this) {
    let pageSize = $(_this).val();
    $('#pageSize').val(pageSize);
    $(_this).closest('form').submit();
}
function LoadPageGrid(result) {
    closeFilters();
    $('#distribution-status').html($(result).find('#distribution-status').html());
}
function ResetDate() {
    $('#filterData').val('');
    return true;
}
function OpenRetransmissionPopup(htm) {
    $("#dialogue").attr('style', 'width:420px;');
    $("#dialogue").html(htm);
    removescroll();
}
function stopAnimationRetransmit() {
    stopAnimation();
    $("#dialogue").show();
    $("#overlay").show();
}
function ClosePopUp() {
    $("#dialogue").hide();
    $("#overlay").hide();
    $("#dialogue").html('');
    $("#dialogue").attr('style', '');
    addscroll();
}
function TransmitData(data) {

    if (data == 1) {
        ClosePopUp();
        $("#faxEmailValidationErr").removeClass("showValidationMsg");
        $("#faxEmailValidationErr").addClass("hideValidationMsg");
        $("#faxEmailValidationErr").text("");
        showToastMessage({
            message: "Movement document is retransmitted",
            type: "success"
        });
    }
    else if (data == 901) {
        $("#faxEmailValidationErr").removeClass("hideValidationMsg");
        $("#faxEmailValidationErr").addClass("showValidationMsg");
        $("#faxEmailValidationErr").text("Fax number is required.");
    }
    else if (data == 902) {
        $("#faxEmailValidationErr").removeClass("hideValidationMsg");
        $("#faxEmailValidationErr").addClass("showValidationMsg");
        $("#faxEmailValidationErr").text("Enter a valid 12 digit Fax number.");
    }
    else if (data == 903) {
        $("#faxEmailValidationErr").removeClass("hideValidationMsg");
        $("#faxEmailValidationErr").addClass("showValidationMsg");
        $("#faxEmailValidationErr").text("Enter a valid 12 digit Fax number.");
    }
    else if (data == 904) {
        $("#faxEmailValidationErr").removeClass("hideValidationMsg");
        $("#faxEmailValidationErr").addClass("showValidationMsg");
        $("#faxEmailValidationErr").text("Email address is required.");
    }
    else if (data == 905) {
        $("#faxEmailValidationErr").removeClass("hideValidationMsg");
        $("#faxEmailValidationErr").addClass("showValidationMsg");
        $("#faxEmailValidationErr").text("Enter a valid email address.");
    }
    else {
        ClosePopUp();
        $("#faxEmailValidationErr").removeClass("showValidationMsg");
        $("#faxEmailValidationErr").addClass("hideValidationMsg");
        $("#faxEmailValidationErr").text("");
        showWarningPopDialog('Failed to retransmit, please specify email and try or contact helpdesk', 'Ok', '', 'CloseDistributionPopup', '', 1, 'warning');
    }
}
function ChkEmailClick(_this) {
    if ($(_this).is(':checked')) {
        $('#txt_email').attr('disabled', false);
        $('#txt_fax').attr('disabled', true);
        $('#txt_fax').val('');
    }
}
function ChkFaxClick(_this) {
    if ($(_this).is(':checked')) {
        $('#txt_email').attr('disabled', true);
        $('#txt_fax').attr('disabled', false);
        $('#txt_email').val('');
    }
}
function PrintDocument(ESDALREf, ContactId, OrganisationId, trasmission_id, orgtype, Is_manually_added, fromOrgName) {
    if (ESDALREf.indexOf('#') > -1) {
        if (Is_manually_added == 1) {
            Is_manually_added = "true";
        }
        ESDALREf = ESDALREf.replace("#", "*");
        PrintNotificationDoc(ESDALREf, ContactId, OrganisationId, trasmission_id, orgtype, Is_manually_added, fromOrgName);
    }
    else { //start else
        $.ajax
            ({
                url: '../ReportDocument/ViewNotifDoc',
                type: 'post',
                async: false,
                data: { ESDALREf: ESDALREf, orgid: OrganisationId, contactid: ContactId, trsmission_id: trasmission_id, fromOrgName: fromOrgName },

                beforesend: function () {
                    startAnimation();
                },
                success: function (data) {
                    switch (data.DOCStatus) {
                        case "NO DATA FOUND":
                            showWarningPopDialog('All details related to this application not found ', 'Ok', '', 'ViewDistReload', '', 1, 'info');
                            break;
                        case "305002"://proposed
                            var link = '../SORTApplication/ViewProposedReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                        case "305003"://reproposed
                            var link = '../SORTApplication/ViewReProposedReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");

                            break;
                        case "305004"://agreed
                            var link = '../SORTApplication/ViewAgreedReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&DocType=agreement&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                        case "305005"://agreed revised
                            var link = '../SORTApplication/ViewAmendmentToAgreementReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                        case "305006"://agreed recleared
                            var link = '../SORTApplication/ViewAmendmentToAgreementReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                    }

                },
                error: function (xhr, textstatus, errorthrown) {
                },
                complete: function () {
                    stopAnimation();
                }

            });
    }//end else

}
function PrintNotificationDoc(ESDALREf, ContactId, OrganisationId, transmission_id, orgtype, Is_manually_added, fromOrgName) {
    let link = '../ReportDocument/ViewNotifDoc?ESDALREf=' + ESDALREf + '&transmission_id=' + transmission_id + '&OrganisationId=' + OrganisationId + '&ContactId=' + ContactId + '&orgtype=' + orgtype + '&Is_manually_added=' + Is_manually_added + '&fromOrgName=' + fromOrgName;
    window.open(link, "_blank");
}
function CloseDistributionPopup() {
    $('#pop-warning').hide();
    startAnimation();
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    ViewDistReload();
    stopAnimation();
}
function ViewDistReload() {
    WarningCancelBtn();
    let pgSiz = $('#pageSizeSelect').val();
    let pg = $('#pageNum').val();
    $.ajax({
        url: '../DistributionStatus/ViewDistributionStatus',
        type: 'post',
        async: false,
        data: { page: pg, pageSize: pgSiz },
        beforesend: function () {
            startAnimation();
        },
        success: function (data) {
            $('#distribution-status').html($(data).find('#distribution-status').html());
            scrolltotop();
        },
        error: function (xhr, textstatus, errorthrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}