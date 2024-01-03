(function () { 

    $(window).scroll(function () {
        $("#ui-datepicker-div").fadeOut(500);
    });
    $('body').on('focus', '.datepicker', function () {
        $('.datepicker').datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy' });
    });

    $('body').on('focus','input[control-type="datepicker"]', function () {
        $('input[control-type="datepicker"]').datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy' });
    });

    $('body').on('focus','input[control-type="datepicker_min"]', function () {
        $('input[control-type="datepicker_min"]').datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy' });
    });
    AjaxPagination();

    $(document).not('.createVehicleDiv').on('keydown', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 9) {
            var focused = $(':focus');
            var window_height = parseInt($(window).height() / 2);
            var headersection_height = parseInt($('#headersection').height());
            var activeelement_top = $(focused).offset().top;
            
            if (activeelement_top > window_height) {
                var scrolltop_amount = activeelement_top - headersection_height;
                $('html, body').animate({
                    scrollTop: (scrolltop_amount)
                }, 100);
            }
            if (activeelement_top < headersection_height) {
                var scrolltop_amount = activeelement_top - headersection_height;
                $('html, body').animate({
                    scrollTop: (0)
                }, 100);
            }

        }
    });
    $('body').on('click', '.ui-control-span-close', function (e) {
        closehelp_popup(this);
    });
    $('body').on('click', '.ui-control-axle-content', function (e) {
        AxleAllContent();
    });
    $('body').on('click', '#helppopup', function () {
        closehelp_popup();
    });
});
//HE-8487
window.onload = function () {
    setTimeout(function () {
        $('body').css('pointer-events', 'all') //activate all pointer-events on body 
    }, 100);
}
//function for reset dialogue
function resetdialogue()
{
    Resize_PopUp(900);
    ClearPopUp();
    return false;

}
//function for checking decimal values
function checkDec(el) {
    var num = new RegExp('^[0-9]+$'); //regex for checking number
    var dec = new RegExp('[0-9]+(\.[0-9][0-9]?)?'); //regex for checking decimal number
    if (el.value == '') el.value = '0.00';
    if (num.test(el.value)) el.value = el.value + '.00';
    if (el.value.substr(0, 1) == '.') el.value = '0' + el.value;
    if (el.value.indexOf('.') == el.value.length - 1) el.value = el.value + '00';
    if (dec.test(el.value)) {
        var index = el.value.indexOf(".");
        var first = el.value.substring(0, index);
        var second = el.value.substring(index, index + 3);
        el.value = first + second;
    }
    return false;
}

function startAnimation_popup(string) {
    if (string == undefined || string == "" || string == "[object Object]" || string instanceof Array) string = "Please wait...!";
    $('#overlay_anim').show();
    $('#overlay_anim').find('.loading').show();
    //LodingText_Change(string);
}
function stopAnimation_popup() {
    $('#overlay_anim').hide();
    $('#overlay_anim').find('.loading').hide();
}
//function for reset dialogue
function resetpopup() { $("#dialogue").html(''); }
function ClearPopUp() {
    $("#dialogue").html('');
}
function loadLeftSearchPanel(url) {
    $.ajax({
        url: url,
        type: 'POST',
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html(page);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function loadLeftSearchInnerPanel(url) {
    $.ajax({
        url: url,
        type: 'POST',
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html(page);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function LoadGridTable(result) {
    var str = '<div>' + result + '</div>';
    var islogin = $(str).find('#isloginPage').val();
    
    if (islogin == 1) {
        location.href = '../Account/Login';
    }   
    else {
        $('.GridTable').html($(result).find('.GridTable').html());
        $('.GridTable').parent('div').find('.pagination-container').html($(result).find('.GridTable').parent('div').find('.pagination-container').html());
        var x = fix_tableheader();
        if (x == 1) $('#tableheader').show();

        //AjaxPagination();
    }
}
/*Ajax pagination*/
function AjaxPagination() {
    $('body').off('click', '.pagination li:not(.active, .disabled) a');
    //$('.pagination').find('li:not(.active, .disabled)').find('a').live('click', '.pagination li:not(.active, .disabled)', function () {
    $('body').on('click', '.pagination li:not(.active, .disabled) a', function () {
        var _this = $(this);
        //this_element = $(this);
        var url = _this.attr('href');
        if (url != undefined) {
            fix_tableheader();
            clearlinks();
            var pagenum = _this.attr('href').split('page=');
            $('#form_pagination').attr('action', url);
            $('#form_pagination').find('#page').val(pagenum[pagenum.length - 1].match(/\d+/g)[0]);
            $('#form_pagination').submit();
                       
        }
        return false;
    });
}
/*Ajax pagination*/
//function for showing help builder form
function help_builder() {
    $('#dialogue').load( "../Content/Helpbuilder/index.html" );
    $('#overlay').show();
    removescroll();
    $('#dialogue').show();
}
function getUrlFromPathName() {
    var pathName = location.pathname;
    var urlFormatted = pathName.replace(/\//g, '');
    return urlFormatted;
}
//method for showing help to the user
function help() {
    var url = getUrlFromPathName();// href
    var urlkey = $("#URLKEY").val();
    if (url == "MovementsMovementInboxList" || url == "HomeSORT")
    {
        url = "HelpdeskIndex";
    }

    if (urlkey == "RoutesLibraryRoutePartDetails") {
        url = urlkey;
    }
    else {
        if (urlkey != "") {
            url = url + urlkey;
        }
    }
    url=url.replace("#", "");

    $('#url_html_page').val(url);
    $('#fflag').val(0);
    $.ajax({
        url: '/Generic/GetHtmlPage',
        type: 'POST',
        data: { fileName: url, flag: 0 },
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            GetUrlName(page);
        },
        error: function (xhr, textStatus, errorThrown) {
            GetPageException(errorThrown);
        },
        complete: function () {
            stopAnimation();
        }
    });
    //$('#form_html_fetch').submit();    
}
//method for showing help to the user (popup)
function help_poup() {
    //var url = geturl(location.href);
    var div_name = $('.body').find('.PopupHelpName').attr('id');
    var url = "PopUp" + div_name;
    $('#fflag').val(1);
    $('#url_html_page').val(url);
    $.ajax({
        url: '/Generic/GetHtmlPage',
        type: 'POST',
        data: { fileName: url, flag: 1 },
        beforeSend: function () {
            startAnimation_popup();
        },
        success: function (page) {
            GetUrlName(page);
        },
        error: function(xhr, textStatus, errorThrown){
            GetPageException(errorThrown);
        },
        complete: function () {
            stopAnimation_popup();
        }
    });
}
//method for showing help to the user (popup)
function help_poup_Firsttime() {
    //var url = geturl(location.href);
    var div_name = $('.PopupHelpName').attr('id');
    var url = "PopUp" + div_name;
    $('#fflag').val(1);
    $('#url_html_page').val(url);
    $.ajax({
        url: '/Generic/GetHtmlPage',
        type: 'POST',
        data: { fileName: url, flag: 1 },
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            GetUrlName(page);
        },
        complete: function () {
            $('.loading').hide();
        }
    });
}
//method for showing help to the user (popup)
function help_poupResMsg() {
    //var url = geturl(location.href);
    //var div_name = $('.body').find('.PopupHelpResMsg').attr('id');
    var div_name = "ResponseMessage";

    var url = "PopUp" + div_name;
    $('#fflag').val(1);
    $('#url_html_page').val(url);
    $.ajax({
        url: '/Generic/GetHtmlPage',
        type: 'POST',
        data: { fileName: url, flag: 1 },
        beforeSend: function () {
            startAnimation_popup();
        },
        success: function (page) {
            GetUrlName(page);
        },
        error: function (xhr, textStatus, errorThrown) {
            GetPageException(errorThrown);
        },
        complete: function () {
            stopAnimation_popup();
        }
    });
}
//function for submit the form for loading html
function EditHelp() {
    var url = getUrlFromPathName();//geturl(location.href);
    var urlkey = $("#URLKEY").val();
    if (urlkey != "") { url = url + urlkey; }
    $('#url_html').val(url);
    $('#flag').val(0);
    window.open('/Generic/HelpBuilder?url=' + url + '&flag=0', '_blank');
   // $('#form_html_helpbuilder').submit();
}
//function for submit the form for loading html (popup)
function EditHelp_popup() {
    //var url = geturl(location.href);
    var div_name = $('.body').find('.PopupHelpName').attr('id');
    var url = "PopUp" + div_name;
    
    $('#url_html').val(url);
    $('#flag').val(1);
    $('#form_html_helpbuilder').submit();
}
//function for submit the form for loading html (popup)
function EditHelp_popup_Firsttime() {
    //var url = geturl(location.href);
    var div_name = $('.PopupHelpName').attr('id');
    var url = "PopUp" + div_name;
    window.open('/Generic/HelpBuilder?url=' + url + '&flag=1', '_blank');
}
//function for load help page to the user
function GetUrlName(result) {
        if (result.page != '') {
            $('#HelpPopupContent').html(result.page);
            $('#HelpPopup').modal({ keyboard: false, backdrop: 'static' });
            $('#HelpPopup').modal('show');
            removescroll();
            }
        else {
            showNotification("Help information is not available.")
            }
           
}
//function for load help page to the user (popup)
function GetUrlName_popup(result) {
    if (result != '') {
        $('#dialogue_popup').html(result);
    }
    else {
        $('#dialogue_popup').html("<div class='Help_form form_component' id='div_help_builder'><div class='head headgrad head_component' ><div class='dyntitle'>Help</div><span aria-hidden='true' data-icon='&#xe0f3;' id='span-close1' class='ui-control-span-close'></span></div><div class='Help_body'> </div></div>");
    }
    $('#overlay_popup').show();
    removescroll();
    $('#dialogue_popup').show();
}
//function for get page exception
function GetPageException(ex) {
    // console.log(ex.responseText);
    $('#dialogue_popup').html("<div class='Help_form form_component' id='div_help_builder'><div class='head headgrad head_component' ><div class='dyntitle'>Help</div><span aria-hidden='true' data-icon='&#xe0f3;' id='span-close1' class='ui-control-span-close'></span></div><div class='Help_body'><h2> Help not available.</h2> </div></div>");
    //$('.Help_body').load('../Home/Error');
    $('#overlay_popup').show();
    removescroll();
    $('#dialogue_popup').show();
}
//function for edit html
function EditableUrl(result) {
    $('#dialogue').load("../Content/Helpbuilder/index.html", function () {
        if (result.flag == 0) {

            if (result.page != '') {
                LoadHtmlContent(result.page);
            }
            else {
                $('#overlay').show();
                removescroll();
                $('#dialogue').show();
            }
        }
        else {
            if (result.page != '') {
                LoadHtmlContent(result.page);
            }
            else {
                $('#overlay_popup').show();
                removescroll();
                $('#dialogue_popup').show();
            }
        }
    });
}
//function for load html content into the textarea
function LoadHtmlContent(result) {
    var parsed = $('<div/>').append(result.page);
    $('#redactor_content').text(parsed.find(".Help_body").html());
    $('.redactor_editor').html(parsed.find(".Help_body").html());
    
}
//function for closing popup
function closeModal() {
    addscrolls();
    $('#dialogue').html('');
    $('#overlay').hide();
    $('#dialogue').hide();
}
//function for get date
function getdate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var hours = today.getHours();
    var minutes = today.getMinutes();
    var seconds = today.getSeconds();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    var date = mm + dd + yyyy + hours + minutes + seconds;
    return date;
}
//function for geting sliced url
function geturl(x) {
    var url = x;
    for (var i = 0; i < 3; i++) { url = url.substring(url.indexOf('/') + 1, url.length); }
    var sub1 = url.substring(0, url.lastIndexOf('/'));
    var sub2 = url.substring(url.lastIndexOf('/') + 1, url.length + 1);
    if (sub2.indexOf('?') > 0) {
        sub2 = sub2.substring(0, sub2.indexOf('?'));
    }
    url = sub1 + sub2;
    return url;
}
//method for set filename
function SetFileName() {
    var date = getdate();
    var pageUrl = $('#page_url').val();
    var url = geturl(pageUrl);
    var user = $('#page_username').val();
    user = user.replace(/[^a-z0-9\s]/gi, '-');
    var filename = 'Help' + url + '_' + user + '_' + date + '.html';

    $('#fileName').val(filename);
    $('#flag1').val($('#flag').val());
    return true;
}
//function for add scroll bar 
function addscrolls() {
    $('body').removeClass('overflowhidden');
    return false;
}
//function for changing state
function changestate(x) {
    setworkflow();
    for (var i = 0; i < x; i++) {
        $(".meter li").eq(i).removeClass('active1').addClass("active");
        //$("#workflow li").eq(i).removeClass('active').addClass("completed");
    }
    $(".meter li").eq(i).addClass("active1");
    return false;
}
//function for setting workflow tabs
function setworkflow() {
    $(".meter li").each(function () {
        $(this).removeClass("active1");
        $(this).removeClass("active");
    });


}
//function for scrolling top
function scrolltotop() {
    $('body,html').animate({scrollTop: 0}, 800);
}
//method for setting time in 12 hours format
function GetTime() {
    var currentTime = new Date();
    var currentHoursAP = currentTime.getHours();
    var currentHours = currentTime.getHours();
    var currentMinutes = currentTime.getMinutes();
    var currentSeconds = currentTime.getSeconds();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;
    currentSeconds = (currentSeconds < 10 ? "0" : "") + currentSeconds;

    // Choose either "AM" or "PM" as appropriate
    var timeOfDay = (currentHours < 12) ? "am" : "pm";

    // Convert the hours component to 12-hour format if needed
    currentHoursAP = (currentHours > 12) ? currentHours - 12 : currentHours;

    // Convert an hours component of "0" to "12"
    currentHoursAP = (currentHoursAP == 0) ? 12 : currentHoursAP;

    // Compose the string for display
    var currentTimeString = currentHoursAP + ":" + ("0" + currentMinutes).slice(-2) +' '+ timeOfDay;

    return currentTimeString;
}
//function for showing Error_Notification
function Error_showNotification(object, string) {

    $(object).parent().find('.error').text('' + string + '');
    $(object).parent().find('.error').show();
    if ($(object).parent().find('.error').length >= 1)
    setTimeout(function () { $(object).parent().find('.error').fadeOut('slow'); }, 5000);
}
//function for ChecksessionTimeout
function CheckSessionTimeOut() {
    var isTimeOut = $('#isloginPage').val();
    if (isTimeOut == "1") {
         //location.reload();
    }
}
//function for closing help popup of a popup
function closehelp_popup() {
    $('#dialogue_popup').html('');
    $('#dialogue_popup').hide();
    $('#overlay_popup').hide();
    if ($("#overlay").find(".body").length == 1) {
        removescroll();
    }
    else { addscroll();}
    //$('#dialogue').hide();
    //$('#overlay').hide();
}
//function for display axle table in notification
function show_AxleListTable(x) {
    if (x == null || x < 1) {
        $("#AxleCounter").focus();
        return false;
    }
    var count = 0;
    var html = "<div class='axlewrapper'><span class='error' id='axelvalid'></span><div class='axlebasic'><table class='axle table' id='tbl_Axle_N' style='float:left; width:492px !important;'>"+
                    "<thead><tr><th class='headgrad white' rowspan='2'>Axle</th><th class='headgrad white' rowspan='2' style='width: 157px;'>Number of wheels</th>"+
                      "<th class='headgrad white' rowspan='2'>Axle weight </th><th class='headgrad white' rowspan='2'>Axle spacing</th><th class='headgrad white' style='width: 92px;'></th></tr></thead><tbody>";
    for (var i = 0; i < x; i++) {
        count = parseInt(count) + 1;
        var innerhtml = "<tr>" +
                                "<td style='width: 20px;'>" + count + "</td>" +
                                "<td style='width: 130px;'><input class='axletextWheel frmbdr nowheels right_text_content' id='Wheel_" + i + "' isrequired='1' maxlength='5' name='[" + i + "].NoOfWheels' style='width: 50px; height: 11px;' type='text' value=''></td>" +
                                "<td style='width: 130px;'><input class='frmbdr axleweight right_text_content' id='AxWht_" + i + "' isrequired='1' maxlength='8' name='[" + i + "].AxelWeight' style='width: 50px; height: 11px;' type='text' value=''></td>";
        if (i != (parseInt(x)-1)){
            innerhtml = innerhtml + "<td style='width: 130px;'><input class=' frmbdr disttonext right_text_content' id='AxSpc_"+i+"' isrequired='1' maxlength='8' name='["+i+"].AxelSpacing' style='width: 50px; height: 11px;' type='text' value=''></td>";
        }
        if(i==0){
            innerhtml = innerhtml+"<td><button type='button' class='btn_CopyAll next btngrad btnrds btnbdr axlebtncpy ui-control-axle-content' aria-hidden='true' data-icon='&#xe027;'>Copy to All</button></td>";
        }
        innerhtml = innerhtml+"</tr>";

        html = html+innerhtml;
        
    }
    html = html + "</tbody></table></div></div>";
    return html;
}
//function for axle copy to all functionality
function AxleAllContent() {
    var i = 1;
    $('#tbl_Axle_N tbody tr').eq(0).find('input:text').each(function () {
        var _thisValue = $(this).val();
        $('#tbl_Axle_N tbody tr:not(:eq(0))').each(function () {
            $(this).find('td:eq(' + i + ')').find('input:text').val(_thisValue);
        });
        i++;
    });
}
//function for clear links
function clearlinks() {
   $table = $('#Config-body').find('#tbl_User');
    $('table').find('tr').each(function () {
        var text = $(this).find('td').eq(0).find('span').text();
        //$(this).find('td').eq(0).find('span').remove();
        //$(this).find('td').eq(0).html('<span>' + text + '</span>');
        $(this).find('td').eq(0).find('span').removeClass("green link");
    });
}
/*------------------------------------ESDAL 4 code starts here-----------------------------------------------*/
//function for loding loader
function startAnimation(string) {
    $('#overlay').show();
    $("#dialogue").hide();
    $('.loading').show();
    $("#overlay_load").fadeIn(300);　
}
//function for removing loader
function stopAnimation() {
    $('#overlay').hide();
    $('.loading').hide();
    //setTimeout(function () {
        $("#overlay_load").fadeOut(300);
    //}, 500);
}
//function for loding loader
function openContentLoader(parentDivId,loadingText) {
    $(parentDivId).css("position", "relative");
    $(parentDivId + ' .content-loader').remove();
    if (parentDivId == 'body') {
        $('html,body').css({
            overflow: 'hidden',
            height: '100%'
        });
    }
    if (loadingText != undefined) {
        //openContentLoader('#banner','Performing Route Assessment..');        
        $(parentDivId).append('<div class="content-loader"><div class="cv-spinner"><span class="spinner"></span></div><div class="content-loader-loading" data-loading-text="'+loadingText+'"></div></div>');
    } else {
        $(parentDivId).append('<div class="content-loader"><div class="cv-spinner"><span class="spinner"></span></div></div>');
    }
    //$(parentDivId).fadeIn(300);
}
//function for removing loader
function closeContentLoader(parentDivId) {
    if (parentDivId == 'body') {
        $('html,body').css({
            overflow: '',
            height: ''
        });
        $(parentDivId).css("position", "absolute");
    } else {
        $(parentDivId).css("position", "inherit");
    }
    
    $(parentDivId +' .content-loader:first').remove();
    //$(parentDivId).fadeOut(300);
}
var toastLive = document.getElementById('esdalToast');
/*showToastMessage({
    message:"This is test message",
    type:"success"
})*/
function showToastMessage(options) {
    if (options == undefined)
        return false;
    if (options != undefined) {
        var message = options.message;
        if (message == undefined || message == null || message == '')
            return false;
        $('#esdalToast').removeClass('bg-danger');
        $('#esdalToast').removeClass('bg-warning');
        $('#esdalToast').removeClass('bg-success');
        $('.toast-body').html('');
        options.type = options.type || 'success';
        switch (options.type) {
            case 'success':
                $('#esdalToast').addClass('bg-success');
                $('#esdalToast .toast-body').html('<div class="d-flex align-items-center"><i class="fa-regular fa-circle-check me-2 fs-3"></i>  '+options.message+'</div>');
                break;
            case 'error':
                $('#esdalToast').addClass('bg-danger');
                $('#esdalToast .toast-body').html('<div class="d-flex align-items-center"><i class="fa-regular fa-circle-xmark me-2 fs-3"></i>  ' + options.message + '</div>');
                break;
            case 'warning':
                $('#esdalToast').addClass('bg-warning');
                $('#esdalToast .toast-body').html('<div class="d-flex align-items-center"><i class="fa-regular fa-exclamation me-2 fs-3"></i>  ' + options.message + '</div>');
                break;
        }        
    }
    var animation = (options && options.animation) || true;
    var autohide = (options && options.autohide) || true;
    var delay = options && options.delay || 5000;

    var toast = new bootstrap.Toast(toastLive, { animation: animation, autohide: autohide, delay: delay });
    toast.show();
}
//<div class="content-loader"><div class="cv-spinner"><span class="spinner"></span></div></div>
function openRoutenav() {
    document.getElementById("mySidenav1").style.width = "400px";
    document.getElementById("mySidenav1").style.display = "block"
    document.getElementById("route-content-2").style.display = "block"
    document.getElementById("route-content-1").style.display = "none"
    document.getElementById("card-swipe1").style.display = "block"
    document.getElementById("card-swipe2").style.display = "block"
    document.getElementById("esdaldclaimer").style.display = "block";
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("mySidenav1").style.width = "auto";
        }
    }
    var x = window.matchMedia("(max-width: 410px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
//function for showing Notification
function showNotification(message) {

    var x = document.getElementById("notifBar");
    x.className = "show";
    document.getElementById("MsgContent").innerHTML = message;
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}
function showNotificationMap(message) {

    var x = document.getElementById("notifBarMap");
    x.className = "show";
    document.getElementById("MsgContentMap").innerHTML = message;
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}
function GetDivObjectJson(containerId, includeHidden = true, onlyVisible = false) {
    var obj = {};
    var input = includeHidden ? 'input' : 'input:not([type=hidden])';
    $($(containerId).find(input + ",textarea,select")).each(function (index, value) {
        var isItemVisible = true;
        if (onlyVisible) {
            if (!$(value).is(":visible")) {
                isItemVisible = false;
            }
        }
        if (isItemVisible) {
            if ($(value).attr("id") != undefined) {
                if ($(value).is(':checkbox') || $(value).is(':radio')) {
                    obj[$(value).attr("id")] = $(value).prop("checked");
                }
                else {
                    if ($(value).attr("id") != 'Number_of_Axles') {
                        obj[$(value).attr("id")] = $(value).val();
                    } else {
                        var axledropcount = $(value).attr("axledropcount") || 0;
                        obj[$(value).attr("id") + "##" + axledropcount] = $(value).val();
                    }
                }
            }
        }
    });
    return obj;
}

function showMultiToastMessage(options) {
    const multiToast = document.querySelector(".multitoast");
    var toastLi = document.createElement("li");
    var id = 0;
    var msgAlreadyExist = false;
    if (options == undefined)
        return false;
    if (options != undefined) {
        var message = options.message;
        if (message == undefined || message == null || message == '')
            return false;
        options.type = options.type || 'error';
        if ($('.multitoast').html().length > 0) {
            id = parseInt($('.multitoast').find('li').length) + 1;
            if (parseInt($('.multitoast').find('li').length) > 1) {
                $(".multitoast li .multiToastMsg").each((id, elem) => {
                    if (elem.innerText == options.message) {
                        //removeToast(elem.closest("li"));
                        msgAlreadyExist = true;
                    }
                });
            }
        }
        if (!msgAlreadyExist) {
            switch (options.type) {
                case 'error':
                    toastLi.className = "mb-2 " + id;
                    toastLi.style = "list-style-type: none";
                    toastLi.innerHTML = `<div id="esdalMultiToast" class="multi_toast align-items-center text-white border-0 bg-danger" role="alert" aria-live="assertive" aria-atomic="true">
                                        <div class="d-flex">
                                            <div class="toast-body">
                                                <div class="d-flex align-items-center">
                                                    <i class="fa-regular fa-circle-xmark me-2 fs-3"></i>
                                                    <span class="multiToastMsg">  ${options.message}</span>
                                                </div>
                                            </div>
                                            <button type="button" class="btn-close btn-close-white btn-close-multitoast me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                                        </div>
                                    </div>`;
                    break;
                case 'warning':
                    toastLi.className = "mb-2 " + id;
                    toastLi.style = "list-style-type: none";
                    toastLi.innerHTML = `<div id="esdalMultiToast" class="multi_toast align-items-center text-white border-0 bg-warning" role="alert" aria-live="assertive" aria-atomic="true">
                                        <div class="d-flex">
                                            <div class="toast-body">
                                                <div class="d-flex align-items-center">
                                                    <i class="fa-regular fa-exclamation me-2 fs-3"></i>
                                                    <span class="multiToastMsg">  ${options.message} </span>
                                                </div>
                                            </div>
                                            <button type="button" class="btn-close btn-close-white btn-close-multitoast me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                                        </div>
                                    </div>`;
                    break;
            }
            multiToast.appendChild(toastLi);
            toastLi.timeoutId = setTimeout(() => removeToast(toastLi, 5000), 5000);
        }
    }
}

function removeToast(toastLi, timeOut) {
    setTimeout(() => toastLi.remove(), timeOut);
}
$('body').on('click', '.btn-close-multitoast', function (e) {
    removeToast(this.closest("li"),50);
});

function IsNenApi() {
    var routeNameNen = $("#RouteName").length > 0 ? $("#RouteName").val().toLowerCase():"";
    return routeNameNen == 'ne notification api' || routeNameNen == 'ne renotification api';
}
/*------------------------------------ESDAL 4 code ends here-------------------------------------------------*/