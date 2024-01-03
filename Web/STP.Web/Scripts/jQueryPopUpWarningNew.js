//function for  showing bigger dialogue box
function showWarningBrokenPopUpDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {

    ResetPopUp();
    $(".POP-dialogue1").hide();
    $(".POP-dialogue111").show();
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
    if (btn1Action != '') {
        $("body").on('click', '.box_warningBtn1', function () {
            window[btn1Action]();
        });
    }
        
    if (btn2Action != '') {
        $("body").on('click', '.box_warningBtn2', function () {
            window[btn2Action]();
        });}
      

    switch (autofocus) {
        //case 1: $('.box_warningBtn1').focus(); break;
        //case 2: $('.box_warningBtn2').focus(); break;
        case 1: $('.box_warningBtn1').attr("autofocus", 'autofocus'); break;
        case 2: $('.box_warningBtn2').attr("autofocus", 'autofocus'); break;
        default: break;
    }

    switch (type) {
        case 'error': $('.message111').addClass("errror"); $('.popup111').css({ "background": '#fcd1d1' }); break;
        case 'info': $('.message111').addClass("info"); $('.popup111').css({ "background": '#cdecfe' }); break;
        case 'warning': $('.message111').addClass("warning"); $('.popup111').css({ "background": '#ffffd0' }); break;
        default: break;

    }

    $('#pop-warning').show();
    $('.POP-dialogue111').css({ 'margin': '12% auto auto' });
    $('.POP-dialogue111').css({ 'width': '600px' });
    $('.popup111').css({ 'width': '600px' });
    $('.popup111 .message111').css({ 'width': '600px' });
    $('.pop-message').css({ 'width': 'auto' });
    $('.popup111 .footer111').css({ 'width': '540px' });
    if ($('#userTypeId').val() == 696007 || $('#userTypeId').val() == 696002) {
        $('.popup111').css({ 'height': '275px' });
        $('.popup111 .message111').css({ 'height': '240px' });
    }
    else if ($('#userTypeId').val() == 696001) {
        $('.popup111 .message111').css({ 'height': '300px' });
        $('.popup111').css({ 'height': '340px' });
    }
}