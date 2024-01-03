function ManageAddressBookInit() {

    view = false;
    SelectMenu(6);
    if ($("#CommunicationMethodName").val() != "" && $("#CommunicationMethodName").val()) {

        if ($("#CommunicationMethodName").val().toUpperCase().indexOf('EMAIL') != -1) {
            $('#MethodByEmail').attr('checked', true);
            $('#Fax').css('display', 'none');
         
        }
        else {
            $('#MethodByFax').attr('checked', true);
            $('#Email').css('display', 'none');
        }
    }
    else {
        $('#MethodByEmail').attr('checked', true);
        $('#Fax').css('display', 'none');
    }

    var Hauliercontactid = $("#HAULIER_CONTACT_ID").val();

    if (Hauliercontactid > 0) {

        var hdnFax = $("#hdnFax").val();
        var hdnEmail = $("#hdnEmail").val();

        if (hdnFax != "") {
            $("#Fax").val(hdnFax);
            $('#Email').hide();
            $('#Email').val('');
        }
        else {
            $("#Email").val(hdnEmail);
            $('#Fax').hide();
            $('#Fax').val('');
        }
    }
    else {

        $('#Fax').hide();
        $('#MethodByEmail').attr('checked', true);
    }
    /*$("#dialogue").show();*/
    //$("#overlay").show();
    $("#nonEsdalModel").show();
    ValidateFaxEmailKeyUp();
}
$(document).ready(function () {
    $('body').on('click', '#backtoaddress', function (e) {
        e.preventDefault();
        BackToAddressBook(this);
    });
    $('body').on('click', '#MethodByFax', function (e) {      
        ValidateFaxEmailKeyUp(this);
    });
    $('body').on('click', '#MethodByEmail', function (e) {
        ValidateFaxEmailKeyUp(this);
    });
    $('body').on('click', '#back-address', function (e) {
       
        e.preventDefault();
        BackToAddressBook(this);
    });
    $('body').on('click', '#manage-address', function (e) {
        e.preventDefault();
        ManageAddressDetails(this);
    });

    $('#Name').keyup(function () {
        if (($('#Name').val().length != 0) || ($('#Name').val().length != null)) {
            // $('#nameValidate').hide();
            $("#nameValidate").addClass("hideValidationMsg");
            $("#nameValidate").removeClass("showValidationMsg");
            $("#ValidationHideFlag").val("");
        }
        if (($('#Name').val().trim() == "") && ($("#ValidationHideFlag").val() == "")) {
            // $('#nameValidate').show();
            $("#nameValidate").removeClass("hideValidationMsg");
            $("#nameValidate").addClass("showValidationMsg");
            $("#ValidationHideFlag").val("shown");
        }
    });
    $('#OrganisationName').keyup(function () {
        if (($('#OrganisationName').val().length != 0) || ($('#OrganisationName').val().length != null)) {
            //  $('#orgNameValidate').hide();
            $("#orgNameValidate").addClass("hideValidationMsg");
            $("#orgNameValidate").removeClass("showValidationMsg");
            $("#ValidationHideFlag").val("");
        }
        if (($('#OrganisationName').val().trim() == "") && ($("#ValidationHideFlag").val() == "")) {
            //  $('#orgNameValidate').show();
            $("#orgNameValidate").removeClass("hideValidationMsg");
            $("#orgNameValidate").addClass("showValidationMsg");
            $("#ValidationHideFlag").val("shown");
        }
    });

    $('#Email').keyup(function () {
        if (($('#Email').val().length != 0) || ($('#Email').val().length != null)) {

            $('#emailFaxValidate').hide();

        }
        if (($('#Email').val().trim() == "")) {

            $("#emailFaxValidate").text('Email address is required');
            $("#emailFaxValidate").show();
        }
    });
    

    //CheckInputValues(Name,nameValidate);
    //CheckInputValues(OrganisationName, orgNameValidate );
    //CheckInputValues(Fax, emailFaxValidate);
    //CheckInputValues(Email, emailFaxValidate);

   

});

//$('body').on('click', '#description1', function (e) {

//    e.preventDefault();
//    var HaulierContactId = $(this).data('hauliercontactid');
//    viewDiscription(HaulierContactId);
//});
function closeNonEsdalPopup() {
    $("#dialogue").hide();
    $("#overlay").hide();
    $("#nonEsdalModel").hide();
    stopAnimation();
    CloseSuccessModalPopup();
}
$("#MethodByFax").click(function () {
    $("#emailFaxValidate").text('');
    $('#Fax').show();
    $('#Email').hide();
    $('#MethodByFax').attr('checked', true);
    $('#MethodByEmail').attr('checked', false);
    ValidateFaxEmailKeyUp();
});
$("#emailFaxValidate").text('');
$("#MethodByEmail").click(function () {
    $("#emailFaxValidate").text('');
    $('#Fax').hide();
    $('#Email').show();
    $('#MethodByEmail').attr('checked', true);
    $('#MethodByFax').attr('checked', false);
});

function ManageAddressDetails() {
    var HAULIER_CONTACT_ID = $("#HAULIER_CONTACT_ID").val();
    var NAME = $("#Name").val();
    var ORGANISATION_ID = $("#ORGANISATION_ID").val();
    var ORGANISATION_NAME = $("#OrganisationName").val();
    var FAX = $("#Fax").val();
    var EMAIL = $("#Email").val();

    /*var COMMUNICATION_METHOD = parseInt($("#CommunicationMethod").val());*/
    var COMMUNICATION_METHOD_NAME = "";

    //if ($('#MethodByFax')[0].checked) {
    //    COMMUNICATION_METHOD_NAME = $("#MethodByFax").val();
    //}
    //else { COMMUNICATION_METHOD_NAME = $("#MethodByEmail").val(); }

    var COMMUNICATION_METHOD_TYPE = $("#CommunicationMethodType").val();

    var paramList = {
        HaulierContactId: HAULIER_CONTACT_ID,
        Name: NAME,
        OrganisationId: ORGANISATION_ID,
        OrganisationName: ORGANISATION_NAME,
        Fax: FAX,
        Email: EMAIL,
        CommunicationMethod: 0,
        CommunicationMethodName: "email html",
        CommunicationMethodType: 309
    }
    if ((ORGANISATION_NAME == null || ORGANISATION_NAME == '') && $("#ValidationHideFlag").val() == "") {

        $("#orgNameValidate").removeClass("hideValidationMsg");
        $("#orgNameValidate").addClass("showValidationMsg");
        /*  $("#orgNameValidate").text("Organisation is required");*/
        $("#ValidationHideFlag").val("shown");

    }


    if (NAME == null || NAME == "") {
        $("#nameValidate").removeClass("hideValidationMsg");
        $("#nameValidate").addClass("showValidationMsg");
        /*    $("#NameValidationMsg").text("Name is required");*/
        $("#ValidationHideFlag").val("shown");
    }
    if (EMAIL == null || Email == "" || FAX == null || FAX == "") {
            $("#emailFaxValidate").removeClass("hideValidationMsg");
            $("#emailFaxValidate").show();
            $("#emailFaxValidate").text('Email address is required');
        /*    $("#NameValidationMsg").text("Name is required");*/
       
    }

    var checkFlag = 0;
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
            if ($('#MethodByFax').attr('checked')) {
                var result = ValidateFax();
                if (!result) {
                    checkFlag = 0;
                }
                else {
                    checkFlag = 1;
                }
            }
            else {
                var result = ValidateEmail();
                if (!result) {
                    checkFlag = 0;
                }
                else {
                    checkFlag = 1;
                }
            }
        }
    }
    if (checkFlag == 1) {
        startAnimation();
        var addtohaul = false;
        if ($('#chk_add_to_haul').length > 0 && $('#chk_add_to_haul').is(':checked'))
            addtohaul = true;
        var nonEsdalContact = $('#hf_NonEsdalContact').val();
        nonEsdalContact = nonEsdalContact.toLowerCase();
        var analysisId = 0;
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
                    if ($('#hf_mode').val() == 'Edit') {
                        if (nonEsdalContact == 'True' || nonEsdalContact == 'true') {
                            ShowSuccessModalPopup("Contact updated successfully", 'closeNonEsdalPopup');
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
                                ShowErrorPopup('Contact already exist', 'closeNonEsdalPopup');
                            }
                            else {
                                loadAddedContactDiv(analysisId);
                                ShowSuccessModalPopup("New contact created successfully", 'closeNonEsdalPopup');

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
function BackToAddressBook() {
   
    if ($('#hf_NonEsdalContact').val() == 'true' || $('#hf_NonEsdalContact').val() == 'True') {
        const element = document.getElementById("contactDivPage");
        element.scrollIntoView();
        $("#dialogue").hide();
        $("#overlay").hide();
    }
    else {
        window.location.href = "/AddressBook/AddressList";
    }
}

function ValidateFaxEmailKeyUp() {
    //$("#emailFaxValidate").text('');
    //if ($('#MethodByFax').attr('checked')) {

    //    $('#Fax').keyup(function () {
    //        if (($('#Fax').val().length != 0) || ($('#Fax').val().length != null)) {

    //            $('#emailFaxValidate').hide();

    //        }
    //        if (($('#Fax').val().trim() == "")) {

    //            $("#emailFaxValidate").text('Fax is required');
    //            $('#emailFaxValidate').show();

    //        }
    //    });
    //}
    //else {

        $('#Email').keyup(function () {
            if (($('#Email').val().length != 0) || ($('#Email').val().length != null)) {

                $('#emailFaxValidate').hide();

            }
            if (($('#Email').val().trim() == "")) {

                $("#emailFaxValidate").text('Email address is required');
                $("#emailFaxValidate").show();
            }
        });
    /*}*/
}
function validateContactName() {
    var nameValid = true;
    var NAME = $("#Name").val();
    if (($('#Name').val().length == 0) || ($('#Name').val().length == null)) {
        //$("#nameValidate").show();
        nameValid = false;
        $("#nameValidate").removeClass("hideValidationMsg");
        $("#nameValidate").addClass("showValidationMsg");
        $("#ValidationHideFlag").val("shown");
    }
    else {
        // $("#nameValidate").hide();
        nameValid = true;
        $("#nameValidate").addClass("hideValidationMsg");
        $("#nameValidate").removeClass("showValidationMsg");
        $("#ValidationHideFlag").val("");
    }
    return nameValid;
}
function ValidateOrganisationName() {
    var nameValid = true;
    if (($('#OrganisationName').val().length == 0) || ($('#OrganisationName').val().length == null)) {
        //$("#orgNameValidate").show();
        nameValid = false;
        $("#orgNameValidate").removeClass("hideValidationMsg");
        $("#orgNameValidate").addClass("showValidationMsg");
        $("#ValidationHideFlag").val("shown");
    }
    else {
        // $("#orgNameValidate").hide();
        nameValid = true;
        $("#orgNameValidate").removeClass("showValidationMsg");
        $("#orgNameValidate").addClass("hideValidationMsg");
        $("#ValidationHideFlag").val("");
    }
    return nameValid;
}
function ValidateFax() {
    var nameValid = true;
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

    var nameValid = true;
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
    var analysisId = 0;
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
    var notifId = $('#NotificationId').val();
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

