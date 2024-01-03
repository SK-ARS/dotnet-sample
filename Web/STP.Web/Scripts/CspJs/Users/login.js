    function userreg() {

        window.location.href="https://www.gov.uk/register-with-esdal";
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
    });

    AntiForgeryTokenInclusionRequest('@Html.AntiForgeryToken()');
    
	$("#Txt_Password").keyup(function ()
    {
        var textboxtype = document.getElementById("Txt_Password");
        var showPwd = document.getElementById("Img_show");
        var hidePwd = document.getElementById("Img_Hide");
        if ($("#Txt_Password").val().trim() == '') {

            showPwd.classList.add("displayNone");
            hidePwd.classList.add("displayBlock");
        }
        else if (hidePwd.classList.contains("displayNone") && hidePwd.classList.contains("displayBlock")) {
            textboxtype.type = "password";
        }


    });
    function showPassword() {
        var textboxtype = document.getElementById("Txt_Password");
        var showPwd = document.getElementById("Img_show");
        var hidePwd = document.getElementById("Img_Hide");
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

    $("#Btn_Login").click(function () {
        LoginClick();
    });

    function RefreshToken() {
        $.ajax({
            url: '../Account/RefreshToken',
            type: 'GET',
            async: false,
            beforeSend: function () {
            },
            success: function (result) {
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
                appReturnUrl: '@ViewBag.ReturnUrl'
            },
            type: 'POST',
            async: false,
            beforeSend: function () {
                //$('.loading').show();
            },
            success: function (result) {
                //$('#token').load('../Account/Tokens');
                RefreshToken();
                if (result.Errormessage != null) {
                    $('#Btn_Login').show();
                    $('#loginLoader').hide();
                    $("#Span_Error").removeClass("displayNone");
                    $('#Txt_Error').text(result.Errormessage);
                    setTimeout(function () {
                        $("#Span_Error").addClass("displayNone");
                        $('#Txt_Error').text('');
                    }, 5000);
                } else {
                    window.location.href = result.redirectToUrl;
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

