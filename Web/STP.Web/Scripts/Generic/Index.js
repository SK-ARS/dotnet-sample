var html = "";

$(document).ready(function () {
    $('.redactor_dropareabox').hide();
    $('#redactor_content').redactor({
        imageUpload: '../Generic/Uploadimage',
        fileUpload: '../Generic/Uploadimage'
    });
    $("#flag2").val($("#flag").val());
$('body').on('click','#CloseHelpp', function(e) { 
  e.preventDefault();
  CloseHelpp(this);
}); 
$('body').on('click','#saveashtml1', function(e) { 
  SetFileName(this);
}); 
$('body').on('click','#CloseHelpp', function(e) { 
  e.preventDefault();
  CloseHelpp(this);
}); 
$('body').on('click','#Closehelp1', function(e) { 
  e.preventDefault();
  CloseHelpp(this);
}); 
});

function LoadPage(data) {
    if (data == 1) {
        showWarningPopDialog('Document saved successfully', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
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

    if (btn1Action != '')

        $("body").on('click', '.box_warningBtn1', function () { window[btn1Action](); });
    if (btn2Action != '')

        $("body").on('click', '.box_warningBtn2', function () { window[btn2Action](); });
    switch (autofocus) {
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
}
function CloseHelpp() {
    window.close();
}
