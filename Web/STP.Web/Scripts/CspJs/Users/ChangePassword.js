    $(document).ready(function ()
    {
        $('body').on('click', '#btncontinue', Continue);
$('body').on('click','#btncloseerrpopup', function() { window['CloseErrorPopup'](); }); 
        $("body").on('keyup', '#txtreceiviedpwd', errorHide);
        $("body").on('keyup', '#txtrepeatpassword', errorRepeat);
        $("body").on('keyup', '#txtpassword', errorReqOTP);
        $('#txtpassword').keyup(function () {
            $("#pwdcompare").hide();
            $("#pwdpolicy").hide();
        });
        $('#txtrepeatpassword').keyup(function () {
            $("#pwdcompare").hide();
            $("#pwdpolicy").hide();
       });
    });
    $('body').on('click', '#Img_Hide', function (e) {
        e.preventDefault();
        var flag = $(this).data('flag');
        showPassword(flag);
    });
    $('body').on('click', '#Img_show', function (e) {
        e.preventDefault();
        var flag = $(this).data('flag');
        showPassword(flag);
    });
    $('body').on('click', '#Img_Hide1', function (e) {
        e.preventDefault();
        var flag = $(this).data('flag');
        showPassword(flag);
    });
    $('body').on('click', '#Img_show1', function (e) {
        e.preventDefault();
        var flag = $(this).data('flag');
        showPassword(flag);
    });
    $('body').on('click', '#Img_Hide2', function (e) {
        e.preventDefault();
        var flag = $(this).data('flag');
        showPassword(flag);
    });
    $('body').on('click', '#Img_show2', function (e) {
        e.preventDefault();
        var flag = $(this).data('flag');
        showPassword(flag);
    });
    function showPassword( flag)
    {
        switch (flag)
        {
            case 1:
                var textboxtype = document.getElementById("txtreceiviedpwd");
                var showPwd = document.getElementById("Img_show");
                var hidePwd = document.getElementById("Img_Hide");
                break;
            case 2:
                var textboxtype = document.getElementById("txtpassword");
                var showPwd = document.getElementById("Img_show1");
                var hidePwd = document.getElementById("Img_Hide1");
                break;
            case 3:
                var textboxtype = document.getElementById("txtrepeatpassword");
                var showPwd = document.getElementById("Img_show2");
                var hidePwd = document.getElementById("Img_Hide2");
                break;
            default:
                break;
        }

        if (textboxtype.type == "password")
        {
            textboxtype.type = "text";
            showPwd.classList.remove("displayNone");
            hidePwd.classList.remove("displayBlock");
            hidePwd.classList.add("displayNone");
        }
        else
        {
            textboxtype.type = "password";
            showPwd.classList.add("displayNone");
            hidePwd.classList.add("displayBlock");
        }
    }

    function Continue()
    {

        var flagReturn = true;
        var currentpassword = $('#txtreceiviedpwd').val();
        var password = $('#txtpassword').val();
        var confirmpassword = $('#txtrepeatpassword').val();
        if (currentpassword.length == 0) {
            $("#ReqValidateOTP").show();
            flagReturn =  false;
        }
        else
        {
            $("#ReqValidateOTP").hide();
        }
        if (password.length == 0) {
            $("#Reqpassword").show();
            $("#pwdpolicy").hide();
            flagReturn = false;
        }
        else {
            $("#Reqpassword").hide();
            $("#pwdpolicy").hide();
        }
        if (password != '') {
            $.ajax({
                url: '../Account/ValidatePassword',
                data: { password: password },
                type: 'POST',
                dataType: 'json',
                async: false,
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    if (data.result) { $("#pwdpolicy").hide(); }
                    else {
                        $("#pwdpolicy").show();
                        flagReturn = false;
                    }
                },
                error: function () {
                    stopAnimation();
                }
            });
        }

        if (confirmpassword.length == 0) {
            $("#Reqrepeatpassword").show();
            flagReturn =  false;
        }
        else {
            $("#Reqrepeatpassword").hide();
        }

        //Comparing Password and Repeat Password.
        if (currentpassword.length == 0) {
            $("#ReqValidateOTP").show();
        }
       
        if ((password != '') && (confirmpassword != '')) {
            if (password != confirmpassword) {
                $("#pwdcompare").show();
                flagReturn = false;
            }
        }
            else {
                $("#pwdcompare").hide();
            }
        
        if (flagReturn)
        {
            var changePasswordData = {};
            changePasswordData.ExistingPassword = currentpassword;
            changePasswordData.NewPassword = password;
            $.ajax({
                async: false,
                type: "POST",
                url: '../Account/ChangePassword',
                dataType: "json",
                //contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ changePasswordInfo: changePasswordData,type: 6 }),/*, existingpassword: currentpassword, password: password*/
                beforeSend: function () {
                    startAnimation();
                },
                processdata: true,
                success: function (result)
                {
                    
                    stopAnimation();
                    if (result=="1")
                    {                       
                        ShowSuccessModalPopup("Invalid OTP.", "Nothingtodo");
                       
                    }
                    else if (result == "2")
                    {
                        ShowSuccessModalPopup("Password updated sucessfully.", "RedirectLocation");                   
                       
                    }
                    else
                    {
                        ShowSuccessModalPopup("Password updation failed.", "Nothingtodo");                       
                    }
                },
                error: function (result) {

                    stopAnimation();
                    ShowErrorPopup("Error on the page.");
                }
            });         
           

        }


    }

    function RedirectLocation()
    {

        window.location.href = "../Account/LogOut";
    }
    function Nothingtodo()
    {
        $('#pop-warning').hide();
        CloseSuccessModalPopup();
        return false;
    }
    function errorHide() {
        if (($('#txtreceiviedpwd').val().trim())!= "") {
            $('#ReqValidateOTP').hide();
        }
        else {
            $('#ReqValidateOTP').show();
        }
    }
    function errorReqOTP() {
        if (($('#txtpassword').val().trim()) != "") {
            $('#Reqpassword').hide();
        } else {
            $('#Reqpassword').show();
        }
    }
    function errorRepeat() {
        if (($('#txtrepeatpassword').val().trim())!='') {
            $('#Reqrepeatpassword').hide();
        }
        else{
            $('#Reqrepeatpassword').show();
        }
    }

