    var html = "";

    $(document).ready(function () {
        $('.redactor_dropareabox').hide();

        $('#redactor_content').redactor({
            imageUpload: '@Url.Action("Uploadimage","Generic")',//../Generic/Uploadimage
            fileUpload: '@Url.Action("Uploadimage","Generic")'
            
        });
        $("#flag2").val($("#flag").val());
        $("#CloseHelpp").on('click', CloseHelpp);
        $("#saveashtml1").on('click', SetFileName);
        $("#CloseHelpp").on('click', CloseHelpp);
        $("#Closehelp1").on('click', CloseHelpp);
    });

    function LoadPage(data) {
        if (data == 1) {
            showWarningPopDialog('Document saved successfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
    }

    function showWarningPopDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {
       
        if (btn1_txt == 'Ok') {
            btn1_txt = 'OK';
    }

        if (btn2_txt == 'Ok') {
            btn2_txt = 'OK';
        }

        $('.pop-message').html(message);
        if (btn1_txt == '') { $('.box_warningBtn1').hide(); } else { $('.box_warningBtn1').html(btn1_txt); }
        if (btn2_txt == '') { $('.box_warningBtn2').hide(); } else { $('.box_warningBtn2').html(btn2_txt); }

        //if (autofocus == 1) $('.box_warningBtn1').focus(); else $('.box_warningBtn2').focus();
        if (btn1Action != '')
           
        $("body").on('click', '.box_warningBtn1', function () { window[btn1Action](); });
        if (btn2Action != '')
           
        $("body").on('click', '.box_warningBtn2', function () { window[btn2Action](); });
        switch (autofocus) {
            //case 1: $('.box_warningBtn1').focus(); break;
            //case 2: $('.box_warningBtn2').focus(); break;
            case 1: $('.box_warningBtn1').attr("autofocus", 'autofocus'); break;
            case 2: $('.box_warningBtn2').attr("autofocus", 'autofocus'); break;
            default: break;
    }
   
        switch (type) {
            case 'error': $('.message1').addClass("errror"); $('.popup1').css({ "background": '#fcd1d1' }); break;
            case 'info': $('.popup1').css({ "background": 'white' }); break;
            case 'warning': $('.message1').addClass("warning"); $('.popup1').css({ "background": 'white' }); break;
            default: break;

        }

        $('#pop-warning').show();
    }
                     
    function WarningCancelBtn() {       
        $('.pop-message').html('');
        $('.box_warningBtn1').html('');
        $('.box_warningBtn2').html('');
        $('#pop-warning').hide();

        $('.box_warningBtn1').unbind();
        $('.box_warningBtn2').unbind();

        window.close();
        //EnableBackButton();
    }
    function CloseHelpp(){
        window.close();
    }
