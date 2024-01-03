var ReturnUrlVal = $('#hf_ReturnUrl').val();
var textboxtype = document.getElementById("Txt_Password");
var showPwd = document.getElementById("Img_show");
var hidePwd = document.getElementById("Img_Hide");
function userreg() {
    window.location.href = "https://www.gov.uk/register-with-esdal";
}
$(document).ready(function () {
    $('body').on('click', '#Img_Hide', showPassword);
    $('body').on('click', '#Img_show', showPassword);
    $('body').on('click', '#btnregister', userreg);
    $(".edit-normal").keypress(function (event) {
        if (event.keyCode == 13) {
            textboxes = $("input.edit-normal");
            currentBoxNumber = textboxes.index(this);
            if (textboxes[currentBoxNumber + 1] != null) {
                nextBox = textboxes[currentBoxNumber + 1]
                nextBox.focus();
                nextBox.select();
                event.preventDefault();
                return false;
            }
        }
    });
    $("#Txt_Password").keypress(function (event) {
        if (event.keyCode == 13) {
            LoginClick();
        }
        if ($("#Txt_Password").val().trim() == '') {
            showPwd.classList.add("displayNone");
            hidePwd.classList.add("displayBlock");
        }
    });
    $("#Txt_Password").keyup(function () {
        if ($("#Txt_Password").val().trim() == '') {
            showPwd.classList.add("displayNone");
            hidePwd.classList.add("displayBlock");
        }
        else if (hidePwd.classList.contains("displayNone") && hidePwd.classList.contains("displayBlock")) {
            textboxtype.type = "password";
        }
    });
    $("input").keyup(function (event) {
        if (event.keyCode == 13) {
            LoginClick();
        }
        else {
            $("#Span_Error").addClass("displayNone");
            $('#Txt_Error').text('');
        }
    });
    $("#Btn_Login").click(function () {
        LoginClick();
    });
});
function showPassword() {
    if ($("#Txt_Password").val().trim() != '') {
        if (textboxtype.type === "password") {
            textboxtype.type = "text";
            showPwd.classList.remove("displayNone");
            hidePwd.classList.remove("displayBlock");
            hidePwd.classList.add("displayNone");
        } else {
            textboxtype.type = "password";
            showPwd.classList.add("displayNone");
            hidePwd.classList.add("displayBlock");
        }
    }
}
function RefreshToken(url) {
    $.ajax({
        url: '../Account/RefreshToken',
        type: 'GET',
        beforeSend: function () {
        },
        success: function (result) {
            window.location.href = url;
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
        }
    });
}
function LoginClick() {
    $('#Btn_Login').hide();
    $('#loginLoader').show();
    $("#Span_Error").addClass("displayNone");
    var validatlogin = validatlogincred();
    if (validatlogin) {
        var userName = $('#Txt_Username').val();
        var passWord = $('#Txt_Password').val();
        var UserInfo = { //generating stopsystem class in json format
            UserName: userName,
            Password: passWord
        };
        $.ajax({
            url: '../Account/Login',
            data: {
                userLogin: UserInfo,
                returnUrl: ReturnUrlVal
            },
            type: 'POST',
            beforeSend: function () {
                //$('.loading').show();
            },
            success: function (result) {
                if (result.Errormessage != null) {
                    $('#Btn_Login').show();
                    $('#loginLoader').hide();
                    $("#Span_Error").removeClass("displayNone");
                    $('#Txt_Error').text(result.Errormessage);

                    //setTimeout(function () {
                    //    $("#Span_Error").addClass("displayNone");
                    //    $('#Txt_Error').text('');
                    //}, 5000);
                }
                else {
                    RefreshToken(result.redirectToUrl);
                    //window.location.href = result.redirectToUrl;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                $('#Btn_Login').show();
                $('#loginLoader').hide();
            },
            complete: function () {
                //$('.loading').hide();
            }
        });
    }
    else {
        var userName = $('#Txt_Username').val();
        var passWord = $('#Txt_Password').val();
        $("#Span_Error").removeClass("displayNone");
        if (userName == '' && passWord == '') {
            $('#Txt_Error').text('Username and Password is required');
        }
        if (userName != '' && passWord == '') {
            $('#Txt_Error').text('Password is required');
        }
        if (userName == '' && passWord != '') {
            $('#Txt_Error').text('Username is required');
        }
        $('#Btn_Login').show();
        $('#loginLoader').hide();
    }
}
function validatlogincred() {
    var loginValid = true;
    if ($("#Txt_Username").val() != '' && $("#Txt_Password").val() != '') {
        loginhValid = true;
    }
    else {
        loginValid = false;
    }
    return loginValid;
}