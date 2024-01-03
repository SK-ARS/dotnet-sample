$(document).ready(function () {
    $('body').on('click', '#close-general', function (e) {
        e.preventDefault();
        CloseGeneralPopup1();
    });
    $('body').on('click', '#close-popup', function (e) {
        e.preventDefault();
        CloseGeneralPopup1();
    });
    $('body').on('click', '#btn-transmit', function (e) {
        e.preventDefault();
        var TransmissionId = $(this).data('transmissionid');
        ReTransmitData(TransmissionId);
    });
});
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
function ValidateEmailRetransmit(email) {

    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (!expr.test(email)) {
        $("#ValidEmail").text("Enter a valid email address.");
        $("#ValidEmail").css("display", "block");
        return false;
    }
    else {
        return true;
    }

}
function ReTransmitData(trans_id) {
    var txt_email = $('#txt_email').val();
    if (txt_email == "") {
        $("#ValidEmail").text("Enter a valid email address.");
        $("#ValidEmail").css("display", "block");
        return false;
    }
    else {
        if (ValidateEmailRetransmit(txt_email)) {
            $("#ValidEmail").css("display", "none");
        }
        else {
            return false;
        }
    }
    var data = { transmissionId: trans_id, emailId: txt_email };
    $.ajax({
        type: "POST",
        url: '../DistributionStatus/Retransmit',
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {

            if (result == 1) {
                CloseModalPopup();
                $("#overlay").hide();
                $("#faxEmailValidationErr").removeClass("showValidationMsg");
                $("#faxEmailValidationErr").addClass("hideValidationMsg");
                $("#faxEmailValidationErr").text("");
                CloseGeneralPopup1();
                $('#retransmitPopup').modal('hide');
                if ($('#NotificationId').val() != undefined && $('#NotificationId').val() != '') {
                    showToastMessage({
                        message: "Movement document is retransmitted",
                        type: "success"
                    });
                    DiplayTransmissionStatus();
                }
                else if ($('#IsViewDistirbutionPage').length > 0) {
                    showToastMessage({
                        message: "Movement document is retransmitted",
                        type: "success"
                    });
                    ViewDistReload();
                }
                else {
                    showToastMessage({
                        message: "Movement document is retransmitted",
                        type: "success"
                    });
                    DisplayTransStatus();
                }
            }
            else if (result == 901) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Fax number is required.");
            }
            else if (result == 902) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Enter a valid 12 digit Fax number.");
            }
            else if (result == 903) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Enter a valid 12 digit Fax number.");
            }
            else if (result == 904) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Email address is required.");
            }
            else if (result == 905) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Enter a valid email address.");
            }
            else {
                CloseModalPopup();
                $('#retransmitPopup').modal('hide');
                $("#faxEmailValidationErr").removeClass("showValidationMsg");
                $("#faxEmailValidationErr").addClass("hideValidationMsg");
                $("#faxEmailValidationErr").text("");
                showToastMessage({
                    message: "Failed to retransmit !!! please specify email and try or contact helpdesk",
                    type: "error"
                });
                ViewDistReload();
            }
        },
        error: function (ex) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function CloseInfoPopup(cntrlName) {
    if (cntrlName != undefined) {
        $('#' + cntrlName).modal('hide');
    }
    $('#retransmitPopup').modal('hide');
    $('#pop-warning').hide();
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    ViewDistReload();
}
function CloseGeneralPopup1() {
    $('#retransmitPopup').modal('hide');
    $("#retransmitPopup").attr('style', 'display:none;');
}