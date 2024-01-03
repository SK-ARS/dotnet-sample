var current_page = '';//variable for setting which is the current page

$(document).ready(function () {

    $("#close-pop").on('click', CloseGeneralPopup);

    $("#btn-close").on('click', CloseGeneralPopup);
    if ($('#hf_outsidepage').length > 0) {
        $('body').css('pointer-events', 'all') //activate all pointer-events on body 
    }
});
//HE-8487
window.onload = function () {
    setTimeout(function () {
        $('body').css('pointer-events', 'all') //activate all pointer-events on body 
    }, 100);
}

function noBack() {
    window.history.b;
}
var targetContainerGlobal;
function closeFilters() {
    var targetContainer = targetContainerGlobal != undefined ? $(targetContainerGlobal) : $("#filters");
    if ($('.planRouteFilter').is(':visible')) {
        targetContainer = $('.planRouteFilter');
    }
    $('.bs-canvas-overlay').remove();
    targetContainer.css("margin", "0 -" + targetContainer.width() + "px 0 0");
    targetContainerGlobal = undefined;
}
function openFilters(width, elem, targetContainer, targetId) {
    width = width || 400;
    targetId = targetId != undefined && targetId != null && targetId != '' ? "#" + targetId : "#filters";
    targetContainer = targetContainer != undefined ? targetId + targetContainer : targetId;
    $(targetContainer).css("width", width + "px");
    if (elem == undefined) {
        $(targetContainer).css("margin", "0 0 0 0");
    } else if (elem == "DispensationManageDispensation") {
        var url = geturl(location.href);
        //if (url == 'DispensationManageDispensation') {
            $(targetContainer).css("margin", "0 0 0 0");
        //}
        //else {
        //    $("#filterDivDispensation").find(targetContainer).css("margin", "0 0 0 0");
        //}
    }
    targetContainerGlobal = targetContainer;
    
    $('body').prepend('<div class="bs-canvas-overlay bg-dark position-fixed w-100 h-100"></div>');
    function myFunction(x) {
        if (x.matches) { // If media query matches
            $(targetContainer).css("width","200px");
        }
    }

    var mediaMaxWidth = targetId == "#mySidenav" ? "992px" : "770px";

    var x = window.matchMedia("(max-width: " + mediaMaxWidth+")")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}

function IsCandidateRouteView() {
    if (($("#CandidateModifyflag").length > 0 && $("#CandidateModifyflag").val() == "True") ||
        ($("#SortStatus").length > 0 && $("#SortStatus").val() == "CandidateRT")) {
        return true;
    }
    return false;
}

$(function () {
    $('.filter-table-icons img').click(function () { return false; });

    $('body').on('click', '#IDCloseGeneralPopup', function () {
        $('#generalPopup').modal('hide');
    });
    //Filter enter key press
    $('body').on('keypress', '#filters input,#movementFilters input,#sortFilters input', function (e) {
        var id = event.key || event.which || event.keyCode || 0;
        if (id == 13 || id == "Enter") {
            e.preventDefault();
            var filterIdParent = '#filters';
            if ($(this).closest("#movementFilters").length > 0) 
                filterIdParent = '#movementFilters';
            else if ($(this).closest("#sortFilters").length > 0) 
                filterIdParent = '#sortFilters';
            var searchButton = $(this).closest(filterIdParent).find('button').filter(function (index, val) {
                return $(val).text().trim().toUpperCase() == "SEARCH";
            });
            if (searchButton && searchButton.length>0)
                $(searchButton).trigger('click');
        }
    });

    //open filter global
    $('body').on('click', '.open-filter-icon-common', function () {
        var width = $(this).data("width");
        var elem = $(this).data("elem");
        var targetContainer = $(this).data("targetcontainer");//use a unique class to avoid multiple filter issue in plan movement
        var targetId = $(this).data("target");//if filter div id is different
        openFilters(width, elem, targetContainer, targetId);
    });

    //close filter global
    $('body').on('click', '.close-filter-icon-common', function () {
        closeFilters();
    });

    $('body').on('click', '#closehelp', function () {
        CloseHelpIPopup();
    });

    $(document).on('click', '.bs-canvas-overlay', function () {
        closeFilters();
        return false;
    });

    //Global handler to manage sorting click event
    $('body').on('click', '.spnSortIconItem', function () {
        let action = $(this).data('action');
        let sortval = $(this).data('sortval');
        window[action](this, sortval);
    });
    //*********************Show filter blue image on table header hover and reset on mouse leave
    var filterItem;
    $('body').on('mouseenter', 'th', function () {
        filterItem = undefined;
        if ($(this).find('.sorting').length > 0 && $(this).find('.filter-table-icons img,.filter-table-icon img').length > 0) {
            $(this).find('.filter-table-icons img,.filter-table-icon img').attr("src", "/Content/assets/images/filtered-icon.svg");
            filterItem = $(this).find('.filter-table-icons img,.filter-table-icon img');
            $(this).addClass('table-header-hover');
        } else if ($(this).find('.sorting').length > 0) {
            $(this).addClass('table-header-hover');
        }
    });
    $('body').on('mouseleave', 'th', function () {
        if ($(this).find('.sorting').length > 0 && filterItem) {
            filterItem.attr("src", "/Content/assets/images/filter-table-icon.svg");
            filterItem = undefined;
            $(this).removeClass('table-header-hover');
        } else if ($(this).find('.sorting').length > 0) {
            $(this).removeClass('table-header-hover');
        }
    });

    LodingText_Reset()
    $(function () {
        return false;
    }); //---------------------------------->>> Method for setting css according to the screen fit
    $(window).resize(function () {
        return false;
    });//-------------------->>> Method for setting css according to the screen fit on window resize
    $('.clox').click(function () { $('#pop-warning').hide(); WarningCancelSOBtn(); });//--------------------->>> Messagebox close button
    $('.box_cancel').click(function () { $('#pop-overlay1').hide(); $('#pop-overlay1').hide(); });//--------------------->>> Messagebox close button           
    //$("#overlay").hide();
    let newsbartext = $(".newslink").html();
    if (newsbartext == null) { $("#newsbar").hide(); }
    MenuOverride();
    changepageheading();

    //Menu item for list dispensation
    $('#Dispensations').click(function () {
        location.replace("../Dispensation/ListDispensation");
    });

    //Menu item for Audit log
    $("[id='Audit log']").click(function () {

        location.replace("../Notification/AuditLog");
    });
    //Menu item for Holiday
    $('#Holiday').click(function () {
        location.replace("../Holidays/HolidayCalender");
    });



    $('#My constraints map').click(function () {
        location.replace("../Structures/MyStructures");
    });

    //Menu item for list movement inbox
    let portal = $('#PortalType').val();
    if (portal == 696002 || portal == 696007) {
        $('#Movements').click(function () {
            location.replace("../Movements/MovementInboxList");
        });
    }


    //Menu for soa reports
    $('#Reports').click(function () {
        if (portal == 696007 || portal == 696002) {
            location.replace("../NENNotification/NEN_SOAReport");
        }

    });

    $('#40002').click(function () {
        let url = '../Routes/RouteFlagSessionClear';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            beforeSend: function () {
            },
            success: function (page) {
            },
            complete: function () {
            }
        });
    });

    $('#10001').click(function () {
        let url = '../Routes/RouteFlagSessionClear';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            beforeSend: function () {
            },
            success: function (page) {
            },
            complete: function () {
            }
        });
    });
    $('#10002').click(function () {
        let url = '../Routes/RouteFlagSessionClear';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            beforeSend: function () {
            },
            success: function (page) {
            },
            complete: function () {
            }
        });
    });

    $('#40001').click(function () {
        let url = '../Routes/RouteFlagSessionClear';
        $.ajax({
            url: url,
            type: 'POST',
            beforeSend: function () {
            },
            success: function (page) {
            },
            complete: function () {
            }
        });
    });

    $('#20001').click(function () {
        let url = '../Routes/RouteFlagSessionClear';
        $.ajax({
            url: url,
            type: 'POST',
            beforeSend: function () {
            },
            success: function (page) {
            },
            complete: function () {
            }
        });
    });

    $('#13004').click(function () {
        $(this).closest('a').attr("href", "../SORTApplication/SORTListMovemnets?SORTStatus=CreateSO")
    });
    $('#13005').click(function () {
        $(this).closest('a').attr("href", "../SORTApplication/SORTListMovemnets?SORTStatus=CreateVR1&VR1Applciation=true")
    });


    //methode for body click removing Autocomplete
    $('body').click(function () {
        $('#route_search_popup').remove();
        $(document).find("#Map_View").css("z-index", 0);
        $(document).find("#map").css("z-index", 0);
        $(document).find("#wraper_leftpanel_content").css("overflow", 'auto');
        $('body').removeClass('removeScroll');
    });


    //*****START***************** Back to top
    var amountScrolled = 200;
    var amountScrolledNav = 25;

    $('body').scroll(function () {
        if ($('body').scrollTop() > amountScrolled) {
            $('button.back-to-top').addClass('show');
        } else {
            $('button.back-to-top').removeClass('show');
        }
    });

    $('button.back-to-top').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 800);
        return false;
    });
    //***END******************* Back to top

});

function getUrlParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    let regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

//To manage favourites
function ManageFavourites(categoryId, categoryType, isFavourites) {
    let isFavouritesFlag;
    let CatIdEnStar = categoryId + "__isFavouriteEn";
    let CatIdDisStar = categoryId + "__isFavouriteDis";
    let enabledStar = document.getElementById(CatIdEnStar);
    let disabledStar = document.getElementById(CatIdDisStar);
    if (isFavourites == 0) {   //condition for changing favorite icon without reloading the entire page
        isFavouritesFlag = 1;
        disabledStar.classList.remove("displayBlock");
        disabledStar.classList.add("displayNone");
        enabledStar.classList.remove("displayNone");
        enabledStar.classList.add("displayBlock");

    }
    else {
        isFavouritesFlag = 0;

        enabledStar.classList.remove("displayBlock");
        enabledStar.classList.add("displayNone");
        disabledStar.classList.remove("displayNone");
        disabledStar.classList.add("displayBlock");
    }


    $.ajax(
        {
            type: "POST",
            url: '../Contents/ManageFavourites',
            dataType: "json",
            data: { categoryId: categoryId, categoryType: categoryType, isFavourites: isFavouritesFlag },
            success: function (result) {

            },
            error: function (result) {

            }
        });
}

function WarningCancelBtn() {
    $('#ErrorPopup').modal('hide');
    $('#SuccessPopup').modal('hide');
    $('#WarningPopup').modal('hide');
    $('#pop-warning').modal('hide');
    $('#pop-warning-nen-route').modal('hide');
    $('#SuccessPopupAction').modal('hide');
    $('#overlay').hide();
    $('#pop-warning').css('display', 'none')
    $('.bs-canvas-overlay').remove();
    CloseInfoPopup('InfoPopup');

    stopAnimation();
}

function WarningCancelBtn1() {
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    $('#pop-warning').html('');
    $('#pop-warning').hide();

    $('.box_warningBtn1').unbind();
    $('.box_warningBtn2').unbind();
    addscroll();
}

function warningcancelforalert() {
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    $('#pop-warning').hide();

    $('.box_warningBtn1').unbind();
    $('.box_warningBtn2').unbind();
    EnableBackButton();
}

function ReloadLocation() {
    location.reload();
}

function CloseRenotifyWarning() {
    $('.pop-message').html('');
    $('.box_warningBtn1').html('');
    $('.box_warningBtn2').html('');
    $('#pop-warning').hide();
    $('.box_warningBtn1').unbind();
    $('.box_warningBtn2').unbind();
    window.location.href = '../Movements/MovementList';
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
        case 'Reports': $("#Reports").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Holiday': $("#Holiday").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Distribution': $("#Distribution").addClass('active'); $('.bar').css({ "display": 'block' }); break;
        case 'Audit log': $("[id='Audit log']").addClass('active'); $('.bar').css({ "display": 'block' }); break;
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
    let header = $('#div_layout_body').find('h3').eq(0).html();

    $('#pageheader').find('h3').text(header);

    return false;
}



function MenuOverride() {

    $('#80001').closest('a').removeAttr('href').css("cursor", "pointer");
    $('#80002').closest('a').removeAttr('href').css("cursor", "pointer");
}

//function for closing help popup
function closehelp() {
    $('#dialogue_popup').html('');
    $('#dialogue_popup').hide();
    $('#overlay_popup').hide();
    if ($("#overlay").find(".body").length == 1) {
        removescroll();
    }
    else { addscroll(); }
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

    if (btn1Action != '') {
        $('body').off('click', '.box_warningBtn1');
        $('body').on('click', '.box_warningBtn1', function () {
            window[btn1Action]();
        });
    }
    if (btn2Action != '') {
        $('body').off('click', '.box_warningBtn2');
        $('body').on('click', '.box_warningBtn2', function () {
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
}


//methode for changing the text of loding Gif
function LodingText_Change(x) {
    $("#overlay").find("#loading_text").text(x);
}

//methode for reset the text of loding Gif
function LodingText_Reset() {
    $("#overlay").find("#loading_text").text('Please wait...!');
}


//close Anotation popup
function closeAnotation() {
    $("#dialogue").draggable("destroy");
    $('#overlay').hide();
    $('#annottaiondiv').hide();
    $("#dialogue").hide();
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    stopAnimation();
    $('body').css("overflow", "scroll");
}

/*------------------------------ Class For ESDAL4---------------------------------------------------*/
function ShowModalPopup(Message) {
    $("#div_Message").text(Message);
    $('#SuccessPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#SuccessPopup').modal('show');
}
function CloseModalPopup() {
    $('#SuccessPopup').modal('hide');
}
function CloseHelpIPopup() {
    $('#HelpPopup').modal('hide');
}
function CloseModalPopupRef() {
    $('#SuccessPopup').modal('hide');
    location.reload();
}
function ShowWarningPopup(Message, btn1Action, btn2Action = '', param1, param2, param3, param4, param5, param6, param7, param8, param9) {
    $("#div_WarningMessage").html(Message);
    if (btn1Action != '') {
        $('body').off('click', '#WarningSuccess');
        $('body').on('click', '#WarningSuccess', function () { window[btn1Action](param1, param2, param3, param4, param5, param6, param7, param8, param9); });
    }

    if (btn2Action != '') {
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', function () { window[btn2Action](param1, param2, param3, param4, param5, param6, param7, param8, param9); });
    }
    else {
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', function () { window['CloseWarningPopupRef'](); })
    }
    $('#WarningPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#WarningSuccess').text("Yes");
    $('#WarningFailure').text("No");
    $('#WarningFailure').show();
    $('#WarningPopup').modal('show');
}
function ShowWarningPopupParam(Message, btn1Action, btn2Action = '', param1, param2, param3, param4, param5, param6, param7, param8, param9) {
    $("#div_WarningMessage").html(Message);
    if (btn1Action != '') {
        $('body').off('click', '#WarningSuccess');
        $('body').on('click', '#WarningSuccess', function () { window[btn1Action](param1, param2, param3, param4, param5, param6, param7, param8, param9); });
    }
    if (btn2Action != '') {
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', function () { window[btn2Action](param1, param2, param3, param4, param5, param6, param7, param8, param9); });
    }
    else {
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', function () { window['CloseWarningPopupRef'](); })
    }
    $('#WarningPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#WarningPopup').modal('show');
}
function ShowWarningPopupDelegate(Message, btn1Action, btn2Action = '') {
    $("#div_WarningMessage").html(Message);
    if (btn1Action != '') {
        $('body').off('click', '#WarningSuccess');
        $('body').on('click', '#WarningSuccess', btn1Action);
    }
    if (btn2Action != '') {
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', btn2Action);
    }
    else {
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', CloseWarningPopupRef);
    }

    $('#WarningPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#WarningPopup').modal('show');
}
function ShowWarningPopupMapupgarde(Message, btn1Action) {
    $("#div_WarningMessage").html(Message);
    if (btn1Action != '') {
        $('body').off('click', '#WarningSuccess');
        $('body').on('click', '#WarningSuccess', btn1Action);
    }

    $('#WarningPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#WarningSuccess').text("OK");
    $('#WarningFailure').hide();
    $('#WarningPopup').modal('show');
}
function ShowWarningPopupCloneRenotif(Message, btn1Action, btn2Action= '') {
    $("#div_WarningMessage").html(Message);
    if (btn1Action != '') {
        $('body').off('click', '#WarningSuccess');
        $('body').on('click', '#WarningSuccess', btn1Action);
    }
    if (btn2Action != '') {
        $('#WarningFailure').text("No");
        $('#WarningFailure').show();
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', btn2Action);
    }
    else {
        //$('#WarningFailure').hide();
        $('#WarningFailure').text("No");
        $('#WarningFailure').show();
        $('body').off('click', '#WarningFailure');
        $('body').on('click', '#WarningFailure', CloseWarningPopupRef);
    }
    $('#WarningPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#WarningSuccess').text("Yes");
    $('#WarningPopup').modal('show');
}



function CloseWarningPopup() {
    $('#WarningPopup').modal('hide');
    let url = geturl(location.href); //condition check is added for roiadownership 
    if (url == "RoadOwnershipshowRoadOwnership") {
        $("#dialogue").html('');
    }
}
function CloseWarningPopupRef() {
    $('#WarningPopup').modal('hide');
    let url = geturl(location.href);
    if (url == "RoadOwnershipshowRoadOwnership") {
        $("#dialogue").html('');
    }
    if ($('.bs-canvas-overlay').is(":visible")) {
        closeFilters();
    }
}

function CloseSuccessModalPopup() {
    $('#SuccessPopupAction').modal('hide');
}
function CloseSuccessModalPopupNotes() {
    $('#SuccessPopupAction').addClass('fade');
    $('#SuccessPopupAction').hide();
}
function ShowSuccessModalPopup(Message, btn1Action, param1 = null, param2 = null) {
    $("#SuccessMessage").text(Message);
    if (btn1Action != '') {
        $('body').off('click', '#ShowSuccessPopup');
        $('body').on('click', '#ShowSuccessPopup', function () { window[btn1Action](param1, param2); });
        CloseSuccessModalPopup();
    }

    else {
        $('body').off('click', '#ShowSuccessPopup');
        $('body').on('click', '#ShowSuccessPopup', function () { window['CloseSuccessModalPopup'](); });
    }

    $('#SuccessPopupAction').modal({ keyboard: false, backdrop: 'static' });
    $('#SuccessPopupAction').modal('show');
}

function ShowSuccessModalPopupNotes(Message, btn1Action) {
    $("#SuccessMessage").text(Message);
    if (btn1Action != '') {
        $('body').off('click', '#ShowSuccessPopup');
        $('body').on('click', '#ShowSuccessPopup', function () { window[btn1Action](); });
    }

    else {
        $('body').off('click', '#ShowSuccessPopup');
        $('body').on('click', '#ShowSuccessPopup', function () { window['CloseSuccessModalPopupNotes'](); });
    }

    $('#SuccessPopupAction').removeClass('fade');
    $('#SuccessPopupAction').show();
}

function ShowDialogWarningPop(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type, params1 = null, params2 = null,params3 = null) {
    ResetPopUp();

    if (btn1_txt == 'Ok') {
        btn1_txt = 'OK';
    }

    if (btn2_txt == 'Ok') {
        btn2_txt = 'OK';
    }

    $('#div_WarningMessageDialog').html(message);
    if (btn1_txt == '') { $('.box_warningBtn1').hide(); } else { $('.box_warningBtn1').html(btn1_txt); }
    if (btn2_txt == '') { $('.box_warningBtn2').hide(); } else { $('.box_warningBtn2').html(btn2_txt); }
    if (btn1Action != '') {
        $('body').off('click', '#WarningFailureDialog');
        $('body').on('click', '#WarningFailureDialog', function () {
            window[btn1Action](params1, params2, params3);
        });
    }
    if (btn2Action != '') {
        $('body').off('click', '#WarningSuccessDialog');
        $('body').on('click', '#WarningSuccessDialog', function () {
            window[btn2Action](params1, params2,params3);
        });
    }

    switch (autofocus) {
        case 1: $('#WarningFailureDialog').attr("autofocus", 'autofocus'); break;
        case 2: $('#WarningSuccessDialog').attr("autofocus", 'autofocus'); break;
        default: break;
    }

    switch (type) {
        case 'info': $('#warningImage').attr('src', '/Content/assets/images/completed.svg'); break;
        default: break;
    }

    $('#pop-warning').modal({ keyboard: false, backdrop: 'static' });
    $('#pop-warning').modal('show');
}


function ResetPopUp() {
    $('.message1').removeClass("errror");
    $('.message1').removeClass("info");
    $('.message1').removeClass("warning");
    $('.box_warningBtn1').show();
    $('.box_warningBtn2').show();
}
function CloseWarningPopupDialog() {
    stopAnimation();
    $('#pop-warning').modal('hide');
    let url = geturl(location.href); //condition check is added for roiadownership 
    if (url == "RoadOwnershipshowRoadOwnership") {
        $("#dialogue").html('');
    }
}

function ShowErrorPopup(Message) {
    $("#div_ErrorMessage").html(Message);
    $('#ErrorPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#ErrorPopup').modal('show');
}
function ShowErrorPopup(Message, btn1Action) {
    $("#div_ErrorMessageWithAction").html(Message);
    if (btn1Action != '' && btn1Action != undefined) {

        $('body').off('click', '#showErrorPopupWithAction');
        $('body').on('click', '#showErrorPopupWithAction', function () {
            window[btn1Action]();
        });
    }
    else {
        $('body').off('click', '#showErrorPopupWithAction');
        $('body').on('click', '#showErrorPopupWithAction', function () { window['CloseErrorPopup'](); })

    }
    $('#ErrorPopupWithAction').modal({ keyboard: false, backdrop: 'static' });
    $('#ErrorPopupWithAction').modal('show');
}
function CloseErrorPopup() {
    $('#ErrorPopupWithAction').modal('hide');
    Clearfields();
    closeFilters();
}
function CloseErrorPopupRef() {
    $('#ErrorPopup').modal('hide');
    location.reload();
}
function CloseInfoPopup(cntrlName) {
    if (cntrlName != undefined) {
        $('#' + cntrlName).modal('hide');
    }
}
function ExecuteInfoPopupAction(btnAction) {
    eval(btnAction + "()");
    CloseInfoPopup('InfoPopup');
}
function ShowInfoPopup(Message, btnAction, param1, param2, param3, param4, param5) {
    $("#InfoMessage").text(Message);
    if (btnAction != '' && btnAction != undefined) {
        $('body').off('click', '#InfoPopupButton');
        $('body').on('click', '#InfoPopupButton', function () { window[btnAction](param1, param2, param3, param4, param5); });
    }
    else {
        $('body').off('click', '#InfoPopupButton');
        $('body').on('click', '#InfoPopupButton', function () { window['CloseInfoPopup']("InfoPopup"); });
    }
    let options = { "backdrop": "static", keyboard: true };
    $('#InfoPopup').modal(options);
    $('#InfoPopup').modal('show');
    $('#generalPopup').modal('hide');
}
function CloseGeneralPopup() {
    $('#generalPopup').modal('hide');
}
function ValidateMomentDateTime(fromDateTime, toDateTime, reqTimeCheck, format = "DD-MM-YYYY HH:mm") {
    var result = 0;
    var res1;
    var res2;
    var res3 = false;
    var currentDate = moment();
    var startDate = moment(fromDateTime, format);
    var toDate = moment(toDateTime, format);

    if (reqTimeCheck) {//to compare with date and time
        res1 = currentDate.isSameOrBefore(startDate, "minutes");
        res2 = toDate.isSameOrAfter(startDate, "minutes");
        res3 = toDate.isSame(startDate, 'minutes');
    }
    else {//to compare with date
        res1 = currentDate.isSameOrBefore(startDate, "days");
        res2 = toDate.isSameOrAfter(startDate, "days");
    }
    if (!res1) {// start date/date&time is less than current date/date&time
        result = 1;
    }
    else if (!res2) {// end date/date&time is less than start date/date&time
        result = 2;
    }
    else if (res3) {
        result = 3;
    }
    return result;
}
/*-------------------------------Code ends Here ----------------------------------------------------*/

function ShowDialogWarningPopWithCallback(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {
    ResetPopUp();

    if (btn1_txt == 'Ok') {
        btn1_txt = 'OK';
    }

    if (btn2_txt == 'Ok') {
        btn2_txt = 'OK';
    }

    $('#div_WarningMessageDialog').html(message);
    if (btn1_txt == '') { $('.box_warningBtn1').hide(); } else { $('.box_warningBtn1').html(btn1_txt); }
    if (btn2_txt == '') { $('.box_warningBtn2').hide(); } else { $('.box_warningBtn2').html(btn2_txt); }  

    if (btn1Action != '') {
        $('body').off('click', '#WarningFailureDialog');
        $('body').on('click', '#WarningFailureDialog', btn1Action);
    }
    if (btn2Action != '') {
        $('body').off('click', '#WarningSuccessDialog');
        $('body').on('click', '#WarningSuccessDialog', btn2Action);
    }
    else {
        $('body').off('click', '#WarningSuccessDialog');
        $('body').on('click', '#WarningFailure', CloseWarningPopupRef);
    }

    switch (autofocus) {
        case 1: $('#WarningFailureDialog').attr("autofocus", 'autofocus'); break;
        case 2: $('#WarningSuccessDialog').attr("autofocus", 'autofocus'); break;
        default: break;
    }

    switch (type) {
        case 'info': $('#warningImage').attr('src', '/Content/assets/images/completed.svg'); break;
        default: break;
    }

    $('#pop-warning').modal({ keyboard: false, backdrop: 'static' });
    $('#pop-warning').modal('show');
}

function RedirectToPlanMovement(url) {
    var workInProgressNotifIdExisting = localStorage.getItem("WorkInProgressNotifId") || 0;
    localStorage.clear();
    localStorage.setItem("WorkInProgressNotifId", workInProgressNotifIdExisting);// This is needed to check the WIP status to verify route assessment needed or not
    window.location = url;
}