$(document).ready(function () {
    $("#validationQue").hide();
    $("#validationAns").hide();
    $('#form_FTChangePassword').hide();
    Chkpassword();


    var FirstLogin = $('#FirstLogin').val();
    var needTerm = $('#NeedTerms').val();
    if (FirstLogin != 'False') {
        if (needTerm == 1) {
            var link = "../Account/TermsAndConditions";
            $('#TermConditions').load(link, function () {
                OpenBootstrapPopup();
                $('#form_FTChangePassword').hide();
                $('#btn_Decline').show();
                $('#divTerms').show();
                $('#divTermsButtons').show();
                $('#dyntitle').show();
                $('#dyntitle').html("TERMS AND CONDITIONS");
            });
        }
    }
    else {
        if ($("#ShowonlyNeedTerms").val() == 1 || $("#varTandConlyAndPass").val() == 1) {
            if (needTerm == 1) {
                var link = "../Account/TermsAndConditions";
                $('#TermConditions').load(link, function () {
                    OpenBootstrapPopup();
                    $('#form_FTChangePassword').hide();
                    $('#divCookies').hide();
                    $('#divCookiesButtons').hide();
                    $('#btn_Decline').show();
                    $('#divTerms').show();
                    $('#divTermsButtons').show();
                    $('#dyntitle').show();
                    $('#dyntitle').html("TERMS AND CONDITIONS");
                });
            }
        }
        if ($("#ShowonlyNeedTerms").val() != 1 || $("#varTandConlyAndPass").val() == 1) {
            OpenBootstrapPopup();
            $('#form_FTChangePassword').show();
            $("#validationQue").hide();
            $("#validationAns").hide();
            $("#btn_Decline").hide();

            Chkpassword();
        }

    }
});
$('#securityAns').keyup(function (e) {
    var sAns = $('#securityAns').val();
    if ((sAns.length < 2) || (sAns.length > 50)) {
        $("#validationAns").show();
    }
    else {
        $("#validationAns").hide();
    }
});

function AcceptTermsAndConditions() {
    /* if ($("#ShowonlyNeedTerms").val() != 1 && $("#varTandConlyAndPass").val() != 1) {*/
	$('#CookiesDetail').load('../Account/CookiesDetails', function () {
        $("#FirstTimeLoginPopup").scrollTop(0);
        $('#divTerms').hide();
        $('#divTermsButtons').hide();
        $('#form_FTChangePassword').hide();
        $('#divCookies').show();
        $('#divCookiesButtons').show();
        $('#btn_Decline').show();
        $("#dyntitle").show();
        $('#dyntitle').html("COOKIES DETAILS");
    });
    // }
    //else {
    //    if ($("#varTandConlyAndPass").val() == 1) {
    //        $("#FirstTimeLoginPopup").scrollTop(0);
    //        $('#divTerms').hide();
    //        $('#divCookies').hide();
    //        $('#divTermsButtons').hide();
    //        $('#divCookiesButtons').hide();
    //        $('#btn_Decline').show();
    //        if ($('#form_FTChangePassword').show()) {
    //            document.getElementById("hdn_AcceptedTerms").value = '1';
    //            $("#dyntitle").show();
    //            $('#dyntitle').html("CHANGE PASSWORD");
    //            $('#btn_Decline').hide();
    //        }
    //    }
    //    else {
    //        ShowonlytermsandConditions();
    //    }
    // }
    //  return true;
}

function AcceptCookies() {
    $('#divTerms').hide();
    $('#divCookies').hide();
    $('#divTermsButtons').hide();
    $('#divCookiesButtons').hide();
    $('#btn_Decline').show();
    if ($('#form_FTChangePassword').show()) {
        $("#FirstTimeLoginPopup").scrollTop(0);
        document.getElementById("hdn_AcceptedTerms").value = '1';
        $('#dyntitle').html("CHANGE PASSWORD");
        $('#btn_Decline').hide();
        try {
            if (iSecurityQuestion == undefined) {
                $("#SecurityQuestion").val(0);
            }
            else {
                $("#SecurityQuestion").val(iSecurityQuestion);
            }
        }
        catch (err) {
            $("#SecurityQuestion").val(0);
        }
    }
}

function showPassword(flag) {
    switch (flag) {
        case 1:
            var textboxtype = document.getElementById("OldPassword");
            var showPwd = document.getElementById("Img_show");
            var hidePwd = document.getElementById("Img_Hide");
            break;
        case 2:
            var textboxtype = document.getElementById("NewPassword");
            var showPwd = document.getElementById("Img_show1");
            var hidePwd = document.getElementById("Img_Hide1");
            break;
        case 3:
            var textboxtype = document.getElementById("ConfirmPassword");
            var showPwd = document.getElementById("Img_show2");
            var hidePwd = document.getElementById("Img_Hide2");
            break;
        default:
            break;
    }

    if (textboxtype.type === "password") {
        textboxtype.type = "text";
        showPwd.classList.remove("displayNone");
        hidePwd.classList.remove("displayBlock");
        hidePwd.classList.add("displayNone");
    }
    else {
        textboxtype.type = "password";
        showPwd.classList.add("displayNone");
        hidePwd.classList.add("displayBlock");
    }
}

function ShowonlytermsandConditions() {
    var url = "../Account/ShowTermsandconditiononly";
    window.location.href = url;
}

$('body').on('click', '.btn-close-popup', function () {
    closePopup();
});

function closePopup() {
    window.location.href = "../Account/Logout";
    $("#dialog").html('');
    $("#dialog").hide();
    $("#overlay").hide();
}

function Chkpassword() {
    $('#OldPassword').click(function () {
        $('#errorOldPassword').text('');
    });
    $('#NewPassword').click(function () {
        $('#errorNewPassword').text('');
    });
}



$("form").submit(function () {
    return (ValidatePassword());
});

function ValidatePassword() {
    var isvalid = true;
    var oldPass = $('#OldPassword').val();
    var newPass = $('#NewPassword').val();
    var confirPass = $('#ConfirmPassword').val();

    var sQue = $("#securityQue option:selected").text();
    var sAns = $('#securityAns').val();

    if (sQue == "" || sQue == null || sAns == "undefined") {

        if ((newPass.length < 6 || newPass.length > 12)) {
            isvalid = false;
        }
        else if ((confirPass != newPass)) {
            isvalid = false;
        }
        else {
            $.ajax({
                url: '../Account/CheckForPassword',
                data: { newPassword: newPass, oldPassword: oldPass },
                type: 'POST',
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.result == 'success') {
                    }
                    else {
                        if (data.result == "Wrong password") {
                            $('#errorOldPassword').html(data.result);
                            isvalid = false;
                        }
                        else {
                            $('#errorNewPassword').html(data.result);
                            isvalid = false;
                        }

                    }
                },
                error: function () {
                    location.reload();
                }
            });
        }
    }
    else {
        if (sQue == "Select") {
            $("#validationQue").show();
            isvalid = false;
        }
        else {
            $("#validationQue").hide();
        }

        if (sAns.length < 2 || sAns.length > 50) {
            $("#validationAns").show();
            isvalid = false;
        }
        else {
            $("#validationAns").hide();
        }


        if ((newPass.length < 6 || newPass.length > 12) || sQue == "Select" || sAns.length < 2 || sAns.length > 50) {
            isvalid = false;
        }
        else if ((confirPass != newPass)) {
            isvalid = false;
        }
        else {
            $.ajax({
                url: '../Account/CheckForPassword',
                data: { newPassword: newPass, oldPassword: oldPass },
                type: 'POST',
                dataType: 'json',
                async: false,
                success: function (data) {

                    if (data.result == 'success') {
                    }
                    else {
                        if (data.result == "Wrong password") {
                            $('#errorOldPassword').html(data.result);
                            isvalid = false;
                        }
                        else {
                            $('#errorNewPassword').html(data.result);
                            isvalid = false;
                        }
                    }
                },
                error: function () {
                    location.reload();
                }
            });
        }
    }
    return isvalid;
}

function OpenBootstrapPopup() {
    $('#FirstTimeLoginPopup').modal({
        keyboard: false,
        backdrop: 'static'
    });
    $("#FirstTimeLoginPopup").modal('show');
}
$('#securityQue').change(function () {
    if ($('#securityQue').val() > 0) {
        $('#validationQue').text('');
    }
    else {
        $('#validationQue').text('Please select question');
    }
});

//
$('body').on('click', '#FirstTimeLoginPopup #span-close', function () {
    closePopup();
});

$('body').on('click', '.show-password', function () {
    var number = $(this).data('number');
    showPassword(parseInt(number));
});

$('body').on('click', '.btn-accept-terms-cp', function () {
    AcceptTermsAndConditions();
});

$('body').on('click', '.btn-accept-cookies-cp', function () {
    AcceptCookies();
});