$(document).ready(function () {
    view = false;
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        SelectMenu(6);
    }
    $('body').on('click', '#CloseManageAddressDetailsPopUp', CloseManageAddressDetailsPopUp);
    $('body').on('click', '#BackToAddressList', BackToAddressList);
    $('body').on('click', '#ManageAddressDetails', ManageAddressDetailsSave);


    $('body').on('keyup', '#Fax', function () {
        $("#emailFaxValidate").removeClass("showValidationMsg");
        $("#emailFaxValidate").addClass("hideValidationMsg");
        if ($('#MethodByFax').attr('checked')) {
            if (($('#Fax').val().length != 0) || ($('#Fax').val().length != null)) {

                $('#emailFaxValidate').hide();

            }
            if (($('#Fax').val().trim() == "")) {

                $("#emailFaxValidate").text('Fax is required');
                $('#emailFaxValidate').show();

            }

        }

    });
  
  
    $('body').on('keyup', '#Name', function () {
        if (($('#Name').val().length != 0) || ($('#Name').val().length != null)) {
            $("#nameValidate").addClass("hideValidationMsg");
            $("#nameValidate").removeClass("showValidationMsg");
            $("#ValidationHideFlag").val("");
        }
        if (($('#Name').val().trim() == "") && ($("#ValidationHideFlag").val() == "")) {
            $("#nameValidate").removeClass("hideValidationMsg");
            $("#nameValidate").addClass("showValidationMsg");
            $("#ValidationHideFlag").val("shown");
        }
    });
    $('body').on('keyup', '#OrganisationName', function () {
        if (($('#OrganisationName').val().length != 0) || ($('#OrganisationName').val().length != null)) {
            $("#orgNameValidate").addClass("hideValidationMsg");
            $("#orgNameValidate").removeClass("showValidationMsg");
            $("#ValidationHideFlag").val("");
        }
        if (($('#OrganisationName').val().trim() == "") && ($("#ValidationHideFlag").val() == "")) {
            $("#orgNameValidate").removeClass("hideValidationMsg");
            $("#orgNameValidate").addClass("showValidationMsg");
            $("#ValidationHideFlag").val("shown");
        }
    });
    $('body').on('keyup', '#Email', function () {
        $("#emailFaxValidate").removeClass("showValidationMsg");
        $("#emailFaxValidate").addClass("hideValidationMsg");
        if (($('#Email').val().length != 0) || ($('#Email').val().length != null)) {

            $('#emailFaxValidate').hide();

        }
        if (($('#Email').val().trim() == "")) {

            $("#emailFaxValidate").text('Email address is required');
            $("#emailFaxValidate").show();
        }

    });


});
function ManageAddressBookDetailsInit() {

    $("#dialogue").show();
    $("#overlay").show();
    $("#nonEsdalModel").show();
    $("#Fax").hide();
  
    
    //if ($("#CommunicationMethodName").val() != "" && $("#CommunicationMethodName").val()) {

    //    if ($("#CommunicationMethodName").val().toUpperCase().indexOf('EMAIL') != -1) {
    //        $('#MethodByEmail').attr('checked', true);
    //    }
    //    else {
    //        $('#MethodByFax').attr('checked', true);
    //    }
    //}
    //else {
    //    $('#MethodByEmail').attr('checked', true);
    //}
  
    let Hauliercontactid = $("#HAULIER_CONTACT_ID").val();
    //if (Hauliercontactid > 0) {

    //    let hdnFax = $("#hdnFax").val();
    //    let hdnEmail = $("#hdnEmail").val();

    //    if (hdnFax != "") {
    //        $("#Fax").val(hdnFax);
    //        $('#Email').hide();
    //        $('#Email').val('');
    //    }
    //    else {
    //        $("#Email").val(hdnEmail);
    //        $('#Fax').hide();
    //        $('#Fax').val('');
    //    }
    //}
    //else {

    //    $('#Fax').hide();
    //    $('#MethodByEmail').attr('checked', true);
    //}
    $("#emailFaxValidate").text('');
}
function CloseManageAddressDetailsPopUp() {
    $("#manageAddressBookmodal").css('display', 'none');
    //$(".displayNone").css('display', 'none !important');
    //$(".#overlay").css('z-index', '1100!important');
    $("#overlay").hide();
}
function closeNonEsdalPopups() {
    $("#dialogue").hide();
    $("#overlay").hide();
    $("#nonEsdalModel").hide();
    stopAnimation();
    CloseSuccessModalPopup();
    BacktoAffectedParties();
}
function ManageAddressDetailsSave() {
    let HAULIER_CONTACT_ID = $("#HAULIER_CONTACT_ID").val();
    let NAME = $("#Name").val();
    let ORGANISATION_ID = $("#ORGANISATION_ID").val();
    let ORGANISATION_NAME = $("#OrganisationName").val();
    let FAX = $("#Fax").val();
    let EMAIL = $("#Email").val();

    let COMMUNICATION_METHOD = parseInt($("#CommunicationMethod").val());
    let COMMUNICATION_METHOD_NAME = "";

    //if ($('#MethodByFax')[0].checked) {
    //    COMMUNICATION_METHOD_NAME = $("#MethodByFax").val();
    //}
    //else { COMMUNICATION_METHOD_NAME = $("#MethodByEmail").val(); }

    let COMMUNICATION_METHOD_TYPE = $("#CommunicationMethodType").val();

    let paramList = {
        HaulierContactId: HAULIER_CONTACT_ID,
        Name: NAME,
        OrganisationId: ORGANISATION_ID,
        OrganisationName: ORGANISATION_NAME,
        Fax: FAX,
        Email: EMAIL,
        CommunicationMethod: "0",
        CommunicationMethodName: "email html",
        CommunicationMethodType: "0"
    }
    if ((ORGANISATION_NAME == null || ORGANISATION_NAME == '') && $("#ValidationHideFlag").val() == "") {

        $("#orgNameValidate").removeClass("hideValidationMsg");
        $("#orgNameValidate").addClass("showValidationMsg");
        $("#ValidationHideFlag").val("shown");

    }


    if (NAME == null || NAME == "") {
        $("#nameValidate").removeClass("hideValidationMsg");
        $("#nameValidate").addClass("showValidationMsg");
        $("#ValidationHideFlag").val("shown");
    }

    let checkFlag = 0;
    var result = validateContactName();
    if (!result) {
        checkFlag = 0;
    }
    else {
        checkFlag = 1;
        var result = ValidateOrganisationName();
        if (!result) {
            checkFlag = 0;
        }
        else {
            checkFlag = 1;
            //if ($('#MethodByFax').attr('checked')) {
            //    var result = ValidateFax();
            //    if (!result) {
            //        checkFlag = 0;
            //    }
            //    else {
            //        checkFlag = 1;
            //    }
            //}
            //else {
            //    var result = ValidateEmail();
            //    if (!result) {
            //        checkFlag = 0;
            //    }
            //    else {
            //        checkFlag = 1;
            //    }
            //}
        }
    }
    if (checkFlag == 1) {
        startAnimation();
        let addtohaul = false;
        if ($('#chk_add_to_haul').length > 0 && $('#chk_add_to_haul').is(':checked'))
            addtohaul = true;
        let nonEsdalContact = $('#hf_NonEsdalContact').val();
        nonEsdalContact = nonEsdalContact.toLowerCase();
        let analysisId = 0;
        analysisId = $('#hidden_and').val();
        if ($('#hidden_and').length == 0 || analysisId == '')
            analysisId = 0;

        $.ajax({
            async: false,
            type: "POST",
            url: '../AddressBook/UpdateAddress?isNonEsdalContact=' + $('#hf_NonEsdalContact').val() + '&addToHaul=' + addtohaul + '&analysisId=' + analysisId,
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramList),
            processdata: true,
            success: function (result) {
                if (result.Success) {
                    paramList = null;
                    CloseManageAddressDetailsPopUp();
                    if ($('#hf_mode').val() == 'Edit') {
                        if (nonEsdalContact == 'True' || nonEsdalContact == 'true') {
                            ShowSuccessModalPopup("Contact updated successfully", 'closeNonEsdalPopups');
                        }
                        else {
                            ShowSuccessModalPopup("Contact updated successfully", 'ReloadLocation');
                        }
                        //closeNonEsdalPopup();
                    }
                    else {
                        if (nonEsdalContact == 'True' || nonEsdalContact == 'true') {
                            if (result.ContactExists) {
                                loadAddedContactDiv(analysisId);
                                ShowErrorPopup('Contact already exist', 'closeNonEsdalPopups');
                            }
                            else {
                                loadAddedContactDiv(analysisId);
                                ShowSuccessModalPopup("New contact created successfully", 'closeNonEsdalPopups');

                            }
                        }
                        else {
                            ShowSuccessModalPopup("New contact created successfully", 'closeAddressBookPopup');
                        }
                    }

                }
                else {

                    $("#emailFaxValidate").text(result.ErrorMessage);
                    $("#emailFaxValidate").show();


                }
            },
            error: function () {
                stopAnimation();
            },
            complete: function () {

                stopAnimation();
            }
        });
    }

}
function BackToAddressList() {
    //if ($('#hf_NonEsdalContact').val() == 'true' || $('#hf_NonEsdalContact1').val() == 'True') {
    //    const element = document.getElementById("contactDivPage");
        
        CloseManageAddressDetailsPopUp();
    //}
    //else {
    //    window.location.href = "/AddressBook/AddressList";
    //}
}
function validateContactName() {
    let nameValid = true;
    if (($('#Name').val().length == 0) || ($('#Name').val().length == null)) {
        nameValid = false;
        $("#nameValidate").removeClass("hideValidationMsg");
        $("#nameValidate").addClass("showValidationMsg");
        $("#ValidationHideFlag").val("shown");
    }
    else {
        nameValid = true;
        $("#nameValidate").addClass("hideValidationMsg");
        $("#nameValidate").removeClass("showValidationMsg");
        $("#ValidationHideFlag").val("");
    }
    return nameValid;
}
function ValidateOrganisationName() {
    let nameValid = true;
    if (($('#OrganisationName').val().length == 0) || ($('#OrganisationName').val().length == null)) {
        nameValid = false;
        $("#orgNameValidate").removeClass("hideValidationMsg");
        $("#orgNameValidate").addClass("showValidationMsg");
        $("#ValidationHideFlag").val("shown");
    }
    else {
        nameValid = true;
        $("#orgNameValidate").removeClass("showValidationMsg");
        $("#orgNameValidate").addClass("hideValidationMsg");
        $("#ValidationHideFlag").val("");
    }
    return nameValid;
}
function ValidateFax() {
    let nameValid = true;
    if ($('#Fax').val().length == 0) {
        $("#emailFaxValidate").text('Fax is required')
        $("#emailFaxValidate").show();
        nameValid = false;
    }
    else {
        $("#emailFaxValidate").hide();
        nameValid = true;
    }
    return nameValid;
}
function ValidateEmail() {

    let nameValid = true;
    if ($('#Email').val().length == 0) {
        $("#emailFaxValidate").text(' Email address is required');
        $("#emailFaxValidate").show();
        nameValid = false;
    }
    else {
        $("#emailFaxValidate").hide();
        nameValid = true;
    }
    return nameValid;
}
function ReloadAffectedParties() {

    startAnimation();
    let analysisId = 0;
    analysisId = $('#hidden_and').val();
    $('#affectedParties').load('../SORTApplication/AffectedParties?analysisId=' + analysisId + '&anal_type=' + 7, function () {
        stopAnimation();
        $('#leftpanel').show();
        $('#route-assessment').show();
        $("#General").hide();
        stopAnimation();
    });
}
function loadAddedContactDiv(analysisId) {
    let notifId = $('#NotificationId').val();
    $.ajax({
        url: '../Notification/ManualAddedParties',
        type: 'POST',
        async: false,
        data: { NotifID: notifId, analysisId: analysisId },

        beforeSend: function () {
            startAnimation();
        },

        success: function (data) {
            $('#ManuallyAddedContact').html(data);
            $('#route-assessment').show();
            $("#General").hide();
            $("#backbutton").show();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }

    });
}
function closeAddressBookPopup() {
    $("#dialogue").hide();
    $("#overlay").hide();
    $("#nonEsdalModel").hide();
    stopAnimation();
    CloseSuccessModalPopup();
    window.location.href = "../AddressBook/AddressList";
}