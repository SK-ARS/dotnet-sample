$(function () {

    $(function () { applysize(); return false; }); //---------------------------------->>> Method for setting css according to the screen fit
    $(window).resize(function () { applysize(); });//-------------------->>> Method for setting css according to the screen fit on window resize
    $('.clox').click(function () { $('#pop-warning').hide(); WarningCancelSOBtn(); });//--------------------->>> Messagebox close button
    $('.box_ok').click(function () {  //location.reload(); });//-------------------------->>> Messagebox close and page reload
    $('.box_cancel').click(function () { $('#pop-overlay1').hide(); $('#pop-overlay1').hide(); });//--------------------->>> Messagebox close button           


    $('.box_ok1').click(function () {
        //DeleteData();
    });//-------------------------->>> Messagebox close and page reload    


    // code for hiding newsbar
    var isNewsEmpty = $('#EmptyNews').length;
    if (isNewsEmpty == 1) { Newsbar(); }

    MenuOverride();

    changepageheading();

    //stopAnimation();

    //Menu item for list dispensation
    $('#Dispensations').click(function () {
        location.replace("../Dispensation/ListDispensation");
    });

    //Menu item for list movement inbox
    var portal = $('#PortalType').val();
    if (portal == 696002 || portal == 696007) {
        $('#Movements').click(function () {
            location.replace("../Movements/MovementInboxList");
        });
    }

});


function noBack() {
    window.history.b;
}


function showWarningPopDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {//code commented for ESDAL 4 
  
    alert(message);
}

function WarningCancelBtn() {

    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    $('#pop-warning').hide();

    $('.box_warningBtn1').unbind();
    $('.box_warningBtn2').unbind();
    $('#WarningPopup').modal('hide');
    EnableBackButton();
    addscroll();
}


function ReloadLocation() {
     //location.reload();

}


function selectedmenu(id) {
    $("#menu").find("li").each(function () {
        $(this).removeClass('active'); $('.bar').css({ "display": 'none' });
    });
    switch (id) {

        case 'Movements': $("#Movements").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Applications': $("#Applications").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Notifications': $("#Notifications").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Dispensations': $("#Dispensations").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Routes': $("#Routes").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Fleet': $("#Fleet").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Contacts': $("#Contacts").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Information': $("#Information").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Map': $("#Map").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Admin': $("#Admin").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Structures': $("#Structures").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Publishing': $("#Publishing").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Distribution': $("#Distribution").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Reports': $("#Reports").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Holiday': $("#Holiday").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        default: break;
    }
}

//function for remove scroll bar 
function removescroll() {
    $('body').addClass('overflowhidden');
    return false;
}

//function for add scroll bar 
function addscroll() {
    $('body').removeClass('overflowhidden');
    return false;
}

//function for reseting the popup
function ResetPopUp() {
    $('.message1').removeClass("errror");
    $('.message1').removeClass("info");
    $('.message1').removeClass("warning");
    $('.box_warningBtn1').show();
    $('.box_warningBtn2').show();
}

//function for changing the page header
function changepageheading() {
    var header = $('#div_layout_body').find('h3').eq(0).html();

    $('#pageheader').find('h3').text(header);

    return false;
}



function MenuOverride() {

    $('#80001').closest('a').removeAttr('href').css("cursor", "pointer");
    $('#80002').closest('a').removeAttr('href').css("cursor", "pointer");
}


function showWarningPopDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {
  
    ResetPopUp();

    if (btn1_txt == 'Ok') {
        btn1_txt = 'OK';
    }

    if (btn2_txt == 'Ok') {
        btn2_txt = 'OK';
    }

    $('.pop-message').html(message);
    if (btn1_txt == '') { $('.box_warningBtn1').hide(); }
    else {
        $('.box_warningBtn1').html(btn1_txt);
       
        $("body").on('click', '.box_warningBtn1', function () {
            window[btn1Action]();
        });
    }
    if (btn2_txt == '')
    { $('.box_warningBtn2').hide(); }
    else {
        $('.box_warningBtn2').html(btn2_txt);
        $("body").on('click', '.box_warningBtn2', function () {
            window[btn2Action]();
        });
       
    }

    

    switch (autofocus) {
      
        case 1: $('.box_warningBtn1').attr("autofocus", 'autofocus'); break;
        case 2: $('.box_warningBtn2').attr("autofocus", 'autofocus'); break;
        default: break;
    }

    switch (type) {
        case 'error': $('.message1').addClass("errror"); $('.popup1').css({ "background": '#fcd1d1' }); break;
        case 'info': $('.message1').addClass("info"); $('.popup1').css({ "background": '#cdecfe' }); break;
        case 'warning': $('.message1').addClass("warning"); $('.popup1').css({ "background": '#ffffd0' }); break;
        default: break;

    }

    $('#pop-warning').show();
  //  $('.pop-overlay1').show();
}