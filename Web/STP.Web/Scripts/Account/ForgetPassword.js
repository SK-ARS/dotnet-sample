window.onload = HideTopBottomLayout();
$(document).ready(function () {
    HideTopBottomLayout();
});
var hfAntiForgeryToken = $('#hfAntiForgeryToken').val();
AntiForgeryTokenInclusionRequest(hfAntiForgeryToken);

$('body').on('click', '.btn-cancel', function () {
    cancel();
});
$('body').on('click', '.btn-continue', function () {
    Continue();
});
$('body').on('click', '.btn-close-error-popup', function () {
    CloseErrorPopup();
});


function HideTopBottomLayout() {
    $("#navbar").hide();
    $("#footerdiv").hide();
}
function cancel() {
    location.href = '../Account/Login';
}
function Continue() {
    $("#EmailReqValidate2").hide();
    $("#UserNameReqValidate1").hide();

    var flagReturn = true;
    var username = $('#txtusername').val();
    var email = $('#txtEmail').val();
    if ($('#txtusername').val().length == 0) {
        $("#arrnameReqValidate").show();
        $("#UserNameEmailValidate").hide();
        $("#EmailReqValidate").hide();
        $("#UserNameReqValidate1").hide();

        flagReturn = false;
    }
    else {
        $("#arrnameReqValidate").hide();
        $("#UserNameEmailValidate").hide();
        $("#EmailReqValidate").hide();
        $("#UserNameReqValidate1").hide();
    }


    if ($('#txtEmail').val().length == 0) {
        $("#EmailReqValidate").show();
        //$("#EmailReqValidate1").hide();
        $("#UserNameReqValidate1").hide();
        $("#UserNameEmailValidate").hide();
        $("#arrnameReqValidate").hide();

        flagReturn = false;
    }
    else {

       // var reg = /^([\w-\.]+)@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        var reg = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
        if (!reg.test(email)) {
            $("#UserNameReqValidate1").show();
            $("#EmailReqValidate").hide();
            $("#UserNameEmailValidate").hide();
            email.focus;
            flagReturn = false;
        }
    }
    if ($('#txtEmail').val().length == 0 && $('#txtusername').val().length == 0) {
        $("#EmailReqValidate").hide();
        $("#arrnameReqValidate").hide();
        $("#UserNameEmailValidate").show();
        $("#UserNameReqValidate1").hide();
        flagReturn = false;
    }
    if (flagReturn) {
        $("#UserNameReqValidate1").hide();
        $("#UserNameEmailValidate").hide();

        $.ajax({
            async: false,
            type: "POST",
            url: '../Account/ForgetPassword',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ userName: username, email: email }),
            beforeSend: function () {
                startAnimation();
            },
            processdata: true,
            success: function (result) {

                stopAnimation();
                if (result == "-3") {
                    //ShowErrorPopup("Please enter registered emailId.", "Nothingtodo");
                    $("#UserNameReqValidate1").show();
                    $("#UserNameEmailValidate").hide();
                }
                else {
                    if (result == "1")
                        ShowSuccessModalPopup("New Temporary Password Sent, Please Check Your Emails", "RedirectLocation");
                    else
                        ShowErrorPopup("OTP not sent");
                }
            },
            error: function (result) {

                stopAnimation();
                ShowErrorPopup("Error on the page");
            }
        });
    }
}
function RedirectLocation() {

    window.location.href = "../Account/Login";
}
function Nothingtodo() {
    $('#pop-warning').hide();
    return false;
}

$("#txtusername").change(function () {
    $("#UserNameReqValidate1").hide();
    $("#UserNameEmailValidate").hide();
});

$("#txtEmail").change(function () {
    $("#UserNameReqValidate1").hide();
    $("#UserNameEmailValidate").hide();
});