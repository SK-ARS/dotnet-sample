

// Get all img elements without alt attribute
const imagesWithoutAlt = document.querySelectorAll('img:not([alt])');

// Loop through each image without alt attribute
imagesWithoutAlt.forEach((img, index) => {
    // Set the alt attribute with an incrementing value
    img.alt = `icon${index + 1}`;
});




$(document).ready(function () {
    NewsTrigger();
    //setInterval(function () {
    //    //function to get latest news-HE-5327
    //    NewsTrigger();
    //}, 60000);
    //Get current url to display latest news
    var url = window.location.pathname.split("/");
    var methodName = url[2];
    var count = $('#latestNews').val();
    //if (count == 1 && methodName != 'NewsOverview' && url[1] != 'Home') {
    //    $("#newNewsIcon").css("display", "block");
    //}
    //else {
    //    $("#newNewsIcon").css("display", "none");
    //}

   

    //get configured time from web.config
    var timeInterval = $('#LatestNewsTimeInterval').val();
    var triggerDuration = timeInterval * 60 * 1000;

    //Triggering NewsTrigger() function for every time interval
    //setInterval(NewsTrigger, triggerDuration);


    $('.dropdown-toggle').dropdown();

    $('#menu_container li').click(function () {
        //$('.nav-link').removeClass('active');
        //$(this).find('a.nav-link').addClass('active');
    });
    $('body').on('click', '#span-help', function () { help(); });

    $('body').on('click', '.btn-open-movement-by-histroy-ref', function (e) {
        e.preventDefault();
        var esdalRefNo = $(this).data('esdalrefno');
        $.ajax({
            async: true,
            type: "GET",
            url: '../Movements/GetMovementLinkByRefNo',
            dataType: "json",
            data: {
                ESDALReference: esdalRefNo
            },
            //contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                stopAnimation();
                if (result.success && result.data && result.data!='') {
                    var win = window.open(result.data, '_blank');
                    if (win) {
                        //Browser has allowed it to be opened
                        win.focus();
                    } else {
                        //Browser has blocked it
                        showToastMessage({
                            message: 'Please allow popups for this website',
                            type: "error"
                        })
                    }
                } else{
                    showToastMessage({
                        message: 'No data found!',
                        type: "error"
                    })
                }

            },
            error: function (result) {
                stopAnimation();
            }

        });
    });
});


//function to get latest news in mentioned interval
function NewsTrigger() {
    
    var url = window.location.pathname.split("/");
    var methodName = url[2];
    $.ajax({
        async: true,
        type: "GET",
        url: '../Information/GetLatestNews',
        dataType: "json",
        data: {
            //timeInterval: $('#LatestNewsTimeInterval').val(), urlMethod: methodName, urlController: url[1]
            timeInterval: 5, urlMethod: methodName, urlController: url[1]
        },
        //contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        success: function (result) {
            //{"result":[{"NewsId":2741,"IsRead":false}],"data":true}
            var portalType = $('#PortalType').val();
            if (result.data && portalType != EsdalUserType.Admin) {
                $("#newNewsIcon").css("display", "block");
            }
            else {
                $("#newNewsIcon").css("display", "none");
            }
        },
        error: function (result) {
        }
    });
}
$('body').on('click', '#newNewsIcon', function (e) {
    e.preventDefault();
    NewsSections();
});
function NewsSections() {
    if (_NewsContentId > 0) {
        window.location.href = '../Information/HotNewsprompticon?ContentId=' + (_NewsContentId || 0);
    } else {
        window.location.href = '../Information/NewsOverview';
    }
}

//redirect to news overview on click latest news icon
function NewsSection() {

    window.location.href = '../Information/NewsOverview';
}

$('body').on('click', '.btn-layout-update-preferences', function () {
    
    Updatepreferences();
});

function Updatepreferences() {

    var VehUnits;
    var DrivInstr;
    var Enable = "false";
    var emailid = $('#Txtemail').val();
    var maxlistitems = $('#ListItemsSelect').val();
    var commMethod;
    var XMLEnable;
    if ($("#RBVCMetric").is(":checked")) {

        VehUnits = "692001";
    }
    else if ($("#RBVCImperial").is(":checked")) {
        VehUnits = "692002";
    }

    if ($("#RBDIMetric").is(":checked")) {
        DrivInstr = "692001";
    }
    else if ($("#RBDIImperial").is(":checked")) {
        DrivInstr = "692002";
    }
    else {
        DrivInstr = "692002";
    }
    if ($("#IsEnable").is(":checked")) {
        Enable = "true";
    }
    if ($("#ChkXMLAttached").is(":checked")) {
        XMLEnable = "true";
    }
    else {
        XMLEnable = "false";
    }
    if ($("#RBInbox").is(":checked")) {
        commMethod = "695004";
    }
    else if ($("#RBemail").is(":checked")) {
        commMethod = "695005";
    }

    if (emailid == "" || emailid == null && XMLEnable != "" && commMethod == "" && DrivInstr != "" && VehUnits == "") {
        ShowErrorPopup("Email id cannot be blank");
    }

    else {
        $.ajax({
            async: false,
            type: "POST",
            url: '../Account/SetUserPreference',
            dataType: "json",
            // contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ VehiUnits: VehUnits, DriveInstr: DrivInstr, IsEnable: Enable, CommMethod: commMethod, MaxListItems: maxlistitems, EmailUpdate: emailid, IsXMLAttached: XMLEnable }),
            beforeSend: function () {
                startAnimation();
            },
            processdata: true,
            success: function (result) {
                stopAnimation();
                if (result == "1") {
                    ShowSuccessModalPopup("User preferences saved successfully", "ClearUserPrefence");
                    
                }
                else if (result == "2") {
                    stopAnimation();
                    ShowErrorPopup("Invalid email id");

                }
                else {
                    stopAnimation();
                    ShowErrorPopup("User preferences updation failed");
                }
            },
            error: function (result) {

                stopAnimation();
                ShowErrorPopup("Error on the page");
            }
        });
    }
}


$('body').on('click', '.btn-layout-close-error-popup', function () {
    CloseErrorPopup();
});

$('body').on('click', '.btn-layout-close-popup-ref', function () {
    CloseModalPopupRef();
});

$('body').on('click', '.btn-layout-update-password', function () {
    UpdatePassword();
});
function UpdatePassword() {
    var oldPass = $('#OldPassword').val();
    var newPass = $('#NewPassword').val();
    var confirmpass = $('#ConfirmuserPassword').val();
    if (newPass.indexOf('\'') >= 0 || newPass.indexOf('"') >= 0) {
        ShowErrorPopup("Password must contain: Minimum 6 and Maximum 12 characters atleast 1 Upper Case Alphabet, 1 Lower Case Alphabet, 1 Number and 1 Special Character (except ',\" ). ", "CloseErrorPopup");
    }
    else {
        $.ajax({
            url: '../Account/ValidatePassword',
            data: { password: newPass },
            type: 'POST',
            dataType: 'json',
            async: false,
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {

                if (data.result) {
                    if ((newPass.length < 6 || newPass.length > 12)) {
                        ShowErrorPopup("The password length should be greater than 6 and less than 12.", "CloseErrorPopup");
                    }
                    else if (newPass != confirmpass) {
                        ShowErrorPopup("New password and confirm password does not match.", "CloseErrorPopup");
                    }
                    else {
                        $.ajax({
                            url: '../Account/UpdateUserPassword',
                            data: { oldpassword: oldPass, newpassword: newPass },
                            type: 'POST',
                            dataType: 'json',
                            async: false,
                            beforeSend: function () {
                                startAnimation();
                            },
                            success: function (data) {

                                if (data == 'Success') {
                                    stopAnimation();
                                    ShowSuccessModalPopup("Password updated successfully", "CloseSuccessPasswordNew");
                                }
                                else {
                                    stopAnimation();
                                    ShowErrorPopup(data, "CloseErrorPopup");

                                }
                            },
                            error: function () {
                                stopAnimation();
                                ShowErrorPopup("Error on the page.", "CloseErrorPopup");

                            }
                        });

                    }
                }
                else {
                    ShowErrorPopup("Password must contain: Minimum 6 and Maximum 12 characters atleast 1 Upper Case Alphabet, 1 Lower Case Alphabet, 1 Number and 1 Special Character (except ',\" ). ", "CloseErrorPopup");
                }
            },
            error: function () {
                stopAnimation();
                ShowErrorPopup("Error on the page.", "CloseErrorPopup");

            }
        });
    }
    stopAnimation();
}


$('body').on('click', '.btn-layout-logout', function (e) {
    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        e.preventDefault();
    } else {
        location.href = '/Account/LogOut';
    }
});

$('body').on('click', '.btn-layout-show-user-info', function () {
    showuserinfo();
});



function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}
function showmorenews() {
    if (document.getElementById('more-news').style.display !== "none") {
        document.getElementById('more-news').style.display = "none"
        document.getElementById('more-info').style.display = "block"
    }
    else {
        document.getElementById('more-news').style.display = "block";
        document.getElementById('more-info').style.display = "none"
    }
}

$('body').on('click', '.btn-layout-view-preferences', function () {
    viewPrefernces();
});

function viewPrefernces() {
    if (document.getElementById('viewPrefernces').style.display !== "none") {
        document.getElementById('viewPrefernces').style.display = "none"
        document.getElementById('chevlon-up-icon3').style.display = "none"
        document.getElementById('chevlon-down-icon3').style.display = "block"
    }
    else {
        //alert("expanding");
        document.getElementById('viewPrefernces').style.display = "block"
        document.getElementById('chevlon-up-icon3').style.display = "block"
        document.getElementById('chevlon-down-icon3').style.display = "none"
        viewUserPreference();
    }
}

function viewUserPreference() {
    $.ajax({
        async: false,
        type: "POST",
        url: "../Account/UserPreferences",
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        processdata: true,
        success: function (result) {

            if (result != null) {

                //alert(1);
                $('#Txtemail').val(result.result[1].EmailText);
         var x= $('#ListItemsSelect').val(result.result[1].MaxListItems);
                if (result.result[1].VehicleUnits == 692001) {
                    $('#RBVCMetric').prop('checked', 'checked');
                }
                else if (result.result[1].VehicleUnits == 692002) {
                    $('#RBVCImperial').prop('checked', 'checked');
                }
                if (result.result[1].RouteplanUnit == 692001) {
                    $('#RBDIMetric').prop('checked', 'checked');
                }
                else if (result.result[1].RouteplanUnit == 692002) {
                    $('#RBDIImperial').prop('checked', 'checked');
                }
                if (result.result[1].CommonMethod == 695004) {
                    $('#RBInbox').prop('checked', 'checked');
                }
                else if (result.result[1].CommonMethod == 695005) {
                    $('#RBemail').prop('checked', 'checked');
                }

                $("#IsEnable").prop("checked", result.result[1].IsEnable||false);
                $("#ChkXMLAttached").prop("checked", result.result[1].IsXMLAttached||false);
            }
        },
        error: function (result) {
        }
    });
}

//$('body').on('click', '#ShowSuccessPopup', function () {
//    ClearUserPrefence();
//});
$('body').on('click', '.btn-layout-clear-preferences', function () {
    ClearUserPrefence();
   
});
function ClearUserPrefence() {
    closeFilters();
    location.reload();
}

function CloseSuccessPassword() {
    closeFilters();
    viewpasswordchange();
    CloseSuccessModalPopup();
}

function selectlistitems(_this) {
    ListItems = $(_this).val();
    $('#ListItemsSelect').val(ListItems);
}

$('body').on('click', '.btn-layout-clear-fields', function () {
    Clearfields();
});
function Clearfields() {
    $('#OldPassword').val("");
    $('#NewPassword').val("");
    $('#ConfirmuserPassword').val("");
}

$('body').on('click', '.btn-layout-view-password-changes', function () {
    viewpasswordchange();
});
function viewpasswordchange() {
    if (document.getElementById('viewpasswordchange').style.display !== "none") {
        document.getElementById('viewpasswordchange').style.display = "none"
        document.getElementById('chevlon-up-icon4').style.display = "none"
        document.getElementById('chevlon-down-icon4').style.display = "block"
    }
    else {
        document.getElementById('viewpasswordchange').style.display = "block"
        document.getElementById('chevlon-up-icon4').style.display = "block"
        document.getElementById('chevlon-down-icon4').style.display = "none"
    }
}

$(document.body).scroll(function () {
    try {
        clearMouseCache();
    }
    catch (err) {
    }
});
function notifiedSOAPolice() {
    $('#notifiedVSOModal').modal({ keyboard: false, backdrop: 'static' });
    $("#notifiedVSOModal").modal('show');
    $("#overlay").css("display", "block");
    $("#notifiedVSO").css("display", "block");
}
$('body').on('click', '.btn-layout-close-notif-popup', function () {
    closenotifiedSOAPolice();
});
function closenotifiedSOAPolice() {
    closeVSO();
    if ($("#hf_VSOTypeNew").length)
        $("#hf_VSOTypeNew").val('');
    //window.location.href = '../Home/Hauliers';
    if (typeof StepFlag != 'undefined' && StepFlag >= NavigationEnum.MOVEMENT_TYPE) {
        RedirectToNavigateFlow(NavigationEnum.SELECT_VEHICLE);
    }
}
function closeVSO() {
    $("#notifiedVSO").hide();
    $('#notifiedVSOModal').modal('hide')
    $("#overlay").css("display", "none");
    $("#notifiedVSO").css("display", "none");
}

$('body').on('click', '.btn-layout-news-section', function () {
    NewsSection();
});

$('body').on('click', '.btn-layout-edit-help', function () {
    EditHelp();
});

$('body').on('click', '.btn-layout-save-vso', function () {
    SaveVSO();
});

function SaveVSO() {
    var NotiVSO = 0;
    NotiVSO = $('input[name=statusradiogroup]:radio:checked').val();
    if (NotiVSO == undefined) {
        showToastMessage({
            message: "Please specify the parties required to be notified by the VSO.",
            type: "error"
        })
        return false;
    }

    $("#NotiVSO").val(NotiVSO);
    $("#hf_VSOTypeNew").val(NotiVSO);
    $("#TypeVSO").val("FromVSO");
    $.ajax({
        url: '../Movements/SelectVSOType',
        data: { NotiVSO: NotiVSO },
        type: 'POST',
        dataType: 'json',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data == 'Success') {
                stopAnimation();
                closeVSO();
                if (typeof StepFlag!='undefined' && StepFlag >= 3) {
                    NavigationMethods.VehicleDetailsConfirm();
                }
            }
            else {
                stopAnimation();
                ShowErrorPopup(data);

            }
        },
        error: function () {
            stopAnimation();
            ShowErrorPopup("Error on the page");

        }
    });

}

function CloseSuccessPasswordNew() {
    viewpasswordchange();
    CloseSuccessModalPopup();
    closeFilters();
}

$('body').on('click', '.btn-layout-show-change-password', function () {
    var number = $(this).data('number');
    ShowChangePasswordInPreferences(parseInt(number));
});

function ShowChangePasswordInPreferences(flag) {
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
            var textboxtype = document.getElementById("ConfirmuserPassword");
            var showPwd = document.getElementById("Img_show2");
            var hidePwd = document.getElementById("Img_Hide2");
            break;
        default:
            break;
    }

    if (textboxtype.type == "password") {
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

function SelectMenu(index) {0
    //$('.nav-link').removeClass('active');
    //$('#menu_container li:nth-child(' + index + ')').find('a.nav-link').addClass('active');
}

//--- for HE-2294: for closing filter when click on outside
window.addEventListener('click', function (e) {
    if (document.getElementById('filters') != null && document.getElementById('filters') != 'undefined' && $('#filters').is(':visible') == true && $('#annotationTextmodal').is(':visible')==false) {
        if (!document.getElementById('filters').contains(e.target)) {
            let filterIcon = document.getElementsByClassName('filter-icon');
            if (filterIcon.length == 0) {
            }
            else if (!filterIcon[0].contains(e.target)) {
                if (document.getElementById("filters").style.width != '0px') {
                    $("#overlay").hide();
                    $("#overlay").css("z-index", "");
                    $("#filters").css("width", "0");
                    if (document.getElementById("banner") != null)
                        $("#banner").css("filter", "unset");
                        $("#navbar").css("filter", "unset");
                }
            }
        }
    }
});

function ShowBrokenRoutesInit() {
    $('#revN').replaceWith($('#LastCandVersion').val());
    $('#versionno').val();
    $('#verN').replaceWith($('#versionno').val());
    $('#span-close2').click(function () {
        resetdialogue();
        if ($('#hf_FlagValue').val() == 7) {
            $('#overlay').hide();
            addscroll();
        }
        else {
            if (check_Type == 'checking' && check_Type_Name == 'CSChecking') {
                $('#IsBrokenCSCheck').val(1);
                UpdateCheckingStatus_Click('checking', 'CSChecking');
                $('.loading').show();
                removescroll();
            }
            else {
                $('#overlay').hide();
                addscroll();
            }
        }
    });
}



var TypeConfiguration = {
    BALLAST_TRACTOR: 234001,
    CONVENTIONAL_TRACTOR: 234002,
    DRAWBAR_TRAILER: 234006,
    RIGID_VEHICLE: 234003,
    SEMI_TRAILER: 234005,
    SPMT: 234007,
    TRACKED_VEHICLE: 234004,
    MOBILE_CRANE: 234008,
    ENGINEERING_PLANT: 234009,
    ENGINEERING_PLANT_SEMI_TRAILER: 234010,
    ENGINEERING_PLANT_DRAWBAR_TRAILER: 234011,
    RECOVERY_VEHICLE: 234012,
    GIRDER_SET: 234013
};
var SubTypeConfiguration = {
    BALLAST_TRACTOR: 224001,
    OTHER_TRACTOR: 224003,
    CONVENTIONAL_TRACTOR: 224002,
    DRAWBAR_TRAILER: 224008,
    GIRDER_SET: 224015,
    OTHER_DRAWBAR_TRAILER: 224009,
    TWIN_BOGIES: 224011,
    RIGID_VEHICLE: 224013,
    ENGINEERING_PLANT: 224020,
    BOGIE: 224010,
    OTHER_SEMI_TRAILER: 224007,
    TROMBONE_TRAILER: 224006,
    SEMI_TRAILER: 224004,
    SEMI_LOW_LOADER: 224005,
    RECOVERED_VEHICLE: 224018,
    WHEELED_LOAD: 224016,
    SPMT: 224014,

    ENGPLANT_CONVENTIONAL_TRACTOR: 224021,
    ENGPLANT_RIGID: 224022,
    ENGPLANT_TRACKED: 224023,
    ENGPLANT_BALLAST_TRACTOR: 224024,
    ENGINEERING_PLANT_SEMI_TRAILER: 224025,
    ENGINEERING_PLANT_DRAWBAR_TRAILER: 224026,
    RECOVERY_VEHICLE: 224027,

    GIRDER_SET: 224028

};
var VEHICLE_CLASSIFICATION_TYPE_CONFIG =
{
    VEHICLE_SPECIAL_ORDER: 241001,
    SPECIAL_ORDER: 241002,
    STGO_AIL_CAT1: 241003,
    STGO_AIL_CAT2: 241004,
    STGO_AIL_CAT3: 241005,
    STGO_MOBILE_CRANE_CAT_A: 241006,
    STGO_MOBILE_CRANE_CAT_B: 241007,
    STGO_MOBILE_CRANE_CAT_C: 241008,
    STGO_ENGINEERING_PLANT: 241009,
    STGO_ROAD_RECOVERY_VEHICLE: 241010,
    WHEELED_CONSTRUCTION_AND_USE: 241011,
    TRACKED: 241012,
    STGO_ENGINEERIN_GPLANT_WHEELED: 241013,
    STGO_ENGINEERIN_GPLANT_TRACKED: 241014,
    NO_VEHICLE_CLASSIFICATION: 241015,
    NO_CRANE: 241016
};

var VEHICLE_PURPOSE_CONFIG =
{
    WHEELED_CONSTRUCTION_AND_USE : 270001,
    STGO_AIL : 270002,
    STGO_MOBILE_CRANE : 270003,
    STGO_ENGINEERING_PLANT : 270004,
    STGO_ROAD_RECOVERY : 270005,
    SPECIAL_ORDER : 270006,
    VEHICLE_SPECIAL_ORDER : 270007,
    TRACKED : 270008,
    STGO_AIL_CAT1 : 270009,
    STGO_AIL_CAT2 : 270010,
    STGO_AIL_CAT3 : 270011,
    STGO_MOBILE_CRANE_CAT_A : 270012,
    STGO_MOBILE_CRANE_CAT_B : 270013,
    STGO_MOBILE_CRANE_CAT_C : 270014,
    STGO_ENGINEERING_PLANT_TRACKED : 270015,
    UN_CLASSIFIED : 270101
};

var VEHICLE_CONFIGURATION_TYPE_CONFIG =
{
    SEMI_TRAILER: 244002,
    BOAT_MAST_EXCEPTION: 244008,
    DRAWBAR_TRAILER: 244001,
    RIGID: 244003,
    SEMI_TRAILER_3_TO_8: 244009,
    DRAWBAR_TRAILER_3_TO_8: 244010,
    RIGID_AND_DRAG: 244011,
    TRACKED: 244004,
    SPMT: 244005,
    CRANE: 244012,
    RECOVER_VEHICLE: 244013,
    OTHER_INLINE: 244006,
    SIDE_BY_SIDE: 244007
};

var BROKEN_ROUTE_MESSAGES =
{
    RI_NOT_IS_REPLAN_IS_LIB: 'Please be aware that due to the map upgrade the route you are trying to import from the library contains previous map data and will need to be re-planned.</br> Please re-plan your current route, import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map.</br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    RI_NOT_IS_REPLAN_IS_NOT_LIB: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    RI_IS_REPLAN_IS_LIB: 'Please be aware that due to the map upgrade the route you have imported from the library contained previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.',
    RI_IS_REPLAN_IS_NOT_LIB: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.',
    RI_IS_REPLAN_FAIL_IS_LIB: 'Please be aware that due to the map upgrade the route you are trying to import from the library contains previous map data and will need to be re-planned. <br>Please re-plan your current route, import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map. <br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    RI_IS_REPLAN_FAIL_IS_NOT_LIB: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. <br> Please re-plan this route, import a new route or create a new route. <br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    PLAN_MOV_ON_MAP_PAGE_CONFIRM_APPLICATION: 'Please be aware that due to the map upgrade route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.<br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    PLAN_MOV_ON_MAP_PAGE_CONFIRM_VR1_APPLICATION: 'Please be aware that due to the map upgrade route(s) in this VR1 application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.<br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    PLAN_MOV_ON_MAP_PAGE_CONFIRM: 'Please be aware that due to the map upgrade the route you are trying to notify contains previous map data and needs to be re-planned. Please re-plan your current route, import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',

    NOTIFY_PROPOSED_REPROPOSED: 'Please be aware that due to the map upgrade your proposed Special Order route contains previous map data. You may continue to notify this route, however PLEASE DO NOT EDIT YOUR CURRENT ROUTE(S) AS IT MAY NO LONGER BE VALID. Please also check the affected parties carefully to ensure that all the relevant parties are included in this notification. If not, please add any missing parties manually.If you are unsure if any affected parties are missing, please contact the ESDAL Helpdesk or Highway England Abnormal Loads Team to discuss further.',
    NOTIFY_VR1: 'Please be aware that due to the map upgrade your approved VR1 route contains previous map data. You may continue to notify this route, however PLEASE DO NOT EDIT YOUR CURRENT ROUTE(S) AS IT MAY NO LONGER BE VALID. Please also check the affected parties carefully to ensure that all the relevant parties are included in this notification. If not, please add any missing parties manually.If you are unsure if any affected parties are missing, please contact the ESDAL Helpdesk or Highway England Abnormal Loads Team to discuss further.',
    NOTIFY_SO: 'Please be aware that due to the map upgrade your agreed Special Order route contains previous map data. You may continue to notify this route, however please check the affected parties carefully to ensure that all the relevant parties are included in this notification. If not, please add any missing parties manually. If you are unsure if any affected parties are missing, please contact the ESDAL Helpdesk or Highway England Abnormal Loads Team to discuss further.',

    RENOTIFY_CLONE_NOTIFICATION_REPLAN_SUCCESS:'Please be aware that due to the map upgrade the route(s) in this notification contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.',
    RENOTIFY_CLONE_NOTIFICATION_REPLAN_FAILED_SINGLE_ROUTE:'Please be aware that due to the map upgrade the route you are trying to notify contains previous map data and needs to be re-planned. Please re-plan your current route, import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map.<br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    RENOTIFY_CLONE_NOTIFICATION_REPLAN_FAILED_MULTIPLE_ROUTE:'Please be aware that due to the map upgrade the route(s) you are trying to notify contains previous map data and need to be re-planned. Please re-plan your current route(s), import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map.<br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',

    COMMON_MESSAGE_SPECIAL_MANOUER_NOTIF: 'Please be aware that due to the map upgrade the route you are trying to notify contains previous map data and needs to be re-planned. Please re-plan your current route, import a new route or create a new route - You can re-enter start and end points or create a new route on the map.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    COMMON_MESSAGE_SPECIAL_MANOUER_NOTIF_MULTIPLE: 'Please be aware that due to the map upgrade the route(s) you are trying to notify contains previous map data and need to be re-planned. Please re-plan your current route(s), import a new route or create a new route - You can re-enter start and end points or create a new route on the map.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    COMMON_MESSAGE_SPECIAL_MANOUER: 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    COMMON_MESSAGE_SPECIAL_MANOUER_VR1: 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    COMMON_MESSAGE_AUTO_REPLAN_FAIL: 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    COMMON_MESSAGE_AUTO_REPLAN_FAIL_VR1: 'Please be aware that due to the map upgrade the route(s) in this VR1 application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    COMMON_MESSAGE_AUTO_REPLAN_SUCCESS: 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.',
    COMMON_MESSAGE_AUTO_REPLAN_SUCCESS_VR1: 'Please be aware that due to the map upgrade the route(s) in this VR1 application contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.',

    LIBRARY_ROUTEPART_DETAILS_IS_REPLAN_NOT_POSSIBLE: 'Please be aware that due to the map upgrade this library route contains previous map data. Before you can reuse the route you will need to Plan and Save it again.</br> </br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    LIBRARY_ROUTEPART_DETAILS_IS_REPLAN_POSSIBLE: 'Please be aware that due to the map upgrade this library route contains previous map data. Click the \'Replan\' button to automatically re-plan the route.',

    A2B_LEFT_PANEL_MESSAGE: "The route you are trying to save hasn't been re-planned. Please re-plan it before saving.",

    ROUTE_DETAILS_MESSAGE_IS_REPLAN_POSSIBLE: "Please be aware that due to the map upgrade this library route contains previous map data. Click the <b>'Replan'</b> button to automatically re-plan the route.",
    ROUTE_DETAILS_MESSAGE_IS_REPLAN_NOT_POSSIBLE: 'Please be aware that due to the map upgrade this library route contains previous map data. Before you can reuse the route you will need to Plan and Save it again.</br> </br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',

    SO_APP_MESSAGE_IS_REPLAN_NOT_POSSIBLE_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route you are trying to import from the library contains previous map data and will need to be re-planned.</br> Please re-plan your current route, import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map.</br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    SO_APP_MESSAGE_IS_REPLAN_NOT_POSSIBLE_NOT_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    SO_APP_MESSAGE_IS_REPLAN_SUCCESS_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route you have imported from the library contained previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.',
    SO_APP_MESSAGE_IS_REPLAN_SUCCESS_NOT_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.',
    SO_APP_MESSAGE_IS_REPLAN_FAILED_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route you are trying to import from the library contains previous map data and will need to be re-planned. Please re-plan your current route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    SO_APP_MESSAGE_IS_REPLAN_FAILED_NOT_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',

    SORT_ROUTE_BY_IMPORT_IS_REPLAN_NOT_POSSIBLE_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route you are trying to import from the library contains previous map data and will need to be re-planned.</br> Please re-plan your current route, import a new route or create a new route <span>-</span> You can re-enter start and end points or create a new route on the map.</br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.' ,
    SORT_ROUTE_BY_IMPORT_IS_REPLAN_NOT_POSSIBLE_NOT_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.' ,
    SORT_ROUTE_BY_IMPORT_IS_REPLAN_SUCCESS_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route you have imported from the library contained previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.' ,
    SORT_ROUTE_BY_IMPORT_IS_REPLAN_SUCCESS_NOT_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data. However, the route has been automatically re-planned based on new map data. Please check the route before proceeding.' ,
    SORT_ROUTE_BY_IMPORT_IS_REPLAN_FAILED_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route you are trying to import from the library contains previous map data and will need to be re-planned.</br> Please re-plan your current route, import a new route or create a new route - You can re-enter start and end points or create a new route on the map.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.' ,
    SORT_ROUTE_BY_IMPORT_IS_REPLAN_FAILED_NOT_FROM_LIBRARY: 'Please be aware that due to the map upgrade the route(s) in this previous movement contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.' ,

    CANDIDATE_ROUTE_LIST_IS_REPLAN_NOT_POSSIBLE_FROM_PREV_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this previous movement contains previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br> </br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    CANDIDATE_ROUTE_LIST_IS_REPLAN_NOT_POSSIBLE_FROM_CURRENT_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this current movement contains previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br> </br> When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    CANDIDATE_ROUTE_LIST_IS_REPLAN_SUCCESS_FROM_PREV_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.',
    CANDIDATE_ROUTE_LIST_IS_REPLAN_SUCCESS_FROM_CURRENT_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.',
    CANDIDATE_ROUTE_LIST_IS_REPLAN_FAILED_FROM_PREV_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    CANDIDATE_ROUTE_LIST_IS_REPLAN_FAILED_FROM_CURRENT_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',


    CREATE_CANDIDATE_VERSION_SPECIAL_MANOUER_IS_FROM_APPLICATION_ROUTES: "Please be aware that due to the map upgrade route(s) in the application version (##*##) contain previous map data and will need to be re-planned. </br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.",
    CREATE_CANDIDATE_VERSION_SPECIAL_MANOUER_IS_FROM_CANDIDATE: "Please be aware that due to the map upgrade route(s) in the candidate route version (##*##) contain previous map data and will need to be re-planned. Please re-plan or ask the haulier to provide a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.",
    CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_SINGLE_BROKEN_ROUTE_IS_FROM_APPLICATION_ROUTES: "Please be aware that due to the map upgrade route(s) in the application version (##*##) contain previous map data and will need to be re-planned.Please re-plan a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.",
    CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_SINGLE_BROKEN_ROUTE_IS_FROM_CANDIDATE: "Please be aware that due to the map upgrade route(s) in the candidate route version (##*##) contain previous map data and will need to be re-planned.Please re-plan or ask the haulier to provide a new route before proceeding.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.",
    CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_MULTIPLE_BROKEN_ROUTE_IS_FROM_APPLICATION_ROUTES: "Please be aware that due to the map upgrade route(s) in the application version (##*##) contain previous map data.  One or more route(s) have not been replanned automatically which may be due to legal restrictions or the presence of special manoeuvres or the presence of alternative paths on the route(s). Please re-plan your current route, import a new route or create a new route  You can re-enter start and end points or create a new route on the map.</br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.",
    CREATE_CANDIDATE_VERSION_AUTO_REPLAN_FAIL_MULTIPLE_BROKEN_ROUTE_IS_FROM_CANDIDATE: "Please be aware that due to the map upgrade route(s) in the candidate route version (##*##) contain previous map data.  One or more route(s) have not been replanned automatically which may be due to legal restrictions or the presence of special manoeuvres or the presence of alternative paths on the route(s). Please re-plan your current route, import a new route or create a new route  You can re-enter start and end points or create a new route on the map.</br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.",
    CREATE_CANDIDATE_VERSION_ALL_ROUTES_IS_REPLANNED_IS_FROM_APPLICATION_ROUTES: "Please be aware that due to the map upgrade route(s) in the application version (##*##) contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.",
    CREATE_CANDIDATE_VERSION_ALL_ROUTES_IS_REPLANNED_IS_FROM_CANDIDATE: "Please be aware that due to the map upgrade route(s) in the candidate route version (##*##) contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.",

    CANDIDATE_ROUTE_DETAILS_IS_REPLAN_NOT_POSSIBLE_FROM_PREV_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.',
    CANDIDATE_ROUTE_DETAILS_IS_REPLAN_NOT_POSSIBLE_NOT_FROM_CURRENT_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.',
    CANDIDATE_ROUTE_DETAILS_IS_REPLAN_SUCCESS_FROM_PREV_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.',
    CANDIDATE_ROUTE_DETAILS_IS_REPLAN_SUCCESS_NOT_FROM_CURRENT_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data. However, the route has been automatically re-planned based on new map data.  Please check the route before proceeding.',
    CANDIDATE_ROUTE_DETAILS_IS_REPLAN_FAILED_FROM_PREV_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this previous movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or RouteTypeouteTypeypereRouteTypete a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',
    CANDIDATE_ROUTE_DETAILS_IS_REPLAN_FAILED_NOT_FROM_CURRENT_MVMNT: 'Please be aware that due to the map upgrade the route(s) in this current movement version contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',

    SORT_LIST_MOV_IS_CHECKING_S_CHECKING: 'Please be aware that due to the map upgrade route(s) in VR1 movement version (##*##) contain previous map data and have been updated since the haulier applied for the movement.',
    SORT_LIST_MOV_IS_CHECKING_NOT_S_CHECKING: 'Please be aware that due to the map upgrade route(s) in VR1 movement version (##*##) contain previous map data and have been updated since the haulier applied for the movement.',
    SORT_LIST_MOV_IS_QACHECKING: 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (##*##) contain previous map data. Please review before sending for QA checking.',
    SORT_LIST_MOV_NOT_QACHECKING: 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (##*##) contain previous map data. Please review before completing QA checking',
    SORT_LIST_MOV_IS_SIGNOFF_F_CHECKING: 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (##*##) contain previous map data. Please review before sending for final checking.',
    SORT_LIST_MOV_IS_SIGNOFF_NOT_F_CHECKING: 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (##*##) contain previous map data. Please review route before completing final checking.',

    NEN_IS_REPLAN_POSSIBLE: 'Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please select to edit the route and click the \'Replan\' button to automatically re-plan the route and then save it again.',
    NEN_IS_REPLAN_NOT_POSSIBLE: 'Please be aware that due to the map upgrade the route(s) in this NEN notification contain previous map data and will need to be re-planned. Please re-plan a new route before proceeding </br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.',


    SOA_POLICE_NOTIFICATIONS: 'Please be aware that due to the map upgrade the notified route you are trying to view contains previous map data. You can still respond to this notification.',
    SOA_POLICE_SO_APPLICATIONS: 'Please be aware that due to the map upgrade the Special Order movement you are trying to view contains previous map data. You can still respond to this movement.',

};

var EsdalUserType = {
    Haulier : 696001,
    PoliceALO : 696002,
    OPSPORTAL : 696003,
    MISPORTAL : 696004,
    PUBLICPORTAL : 696005,
    Admin : 696006,
    SOA : 696007,
    Sort : 696008,
    AdminSU : 696009
}

var _NewsContentId = 0;
$(document).ready(function () {

    // Declare a proxy to reference the hub.
    this.watcher = $.connection.newsHub;
    
    this.watcher.client.addMessage = function (contentId, newsItem) {
        if (contentId > 0 && newsItem != null && newsItem != undefined) {
            var portalType = $('#PortalType').val();
            var newsObj = JSON.parse(newsItem);
            if (portalType != EsdalUserType.Admin && newsObj && newsObj.PortalId != '' && newsObj.PortalId != undefined && newsObj.PortalId != null) {
                var hasPortalType = newsObj.PortalId.indexOf(portalType) != -1;
                _NewsContentId = contentId;
                if (hasPortalType && newsObj.Suppressed == 0 && newsObj.Retracted==0) {
                    //$("#newNewsIcon").css("display", "block");
                    NewsTrigger();
                    //showToastMessage({
                    //    message: "New news has been added. &nbsp;" +
                    //        "<a style='color: #fff;text-decoration: underline;font-weight: bold;' href='/Information/ViewInformation/" + contentId + "' target='_blank'>View</a>",
                    //    type: "success"
                    //});
                    showToastMessage({
                        message: "New news has been added.",
                        type: "success"
                    });
                    //beep(500,600,100);
                }
            }
        }
    };
    $.connection.hub.start();
});


const newsAudioContext = new AudioContext();

/**
 * Helper function to emit a beep sound in the browser using the Web Audio API.
 * 
 * @param {number} duration - The duration of the beep sound in milliseconds.
 * @param {number} frequency - The frequency of the beep sound.
 * @param {number} volume - The volume of the beep sound.
 * 
 * @returns {Promise} - A promise that resolves when the beep sound is finished.
 */
function beep(duration, frequency, volume) {
    return new Promise((resolve, reject) => {
        // Set default duration if not provided
        duration = duration || 200;
        frequency = frequency || 440;
        volume = volume || 100;

        try {
            let oscillatorNode = newsAudioContext.createOscillator();
            let gainNode = newsAudioContext.createGain();
            oscillatorNode.connect(gainNode);

            // Set the oscillator frequency in hertz
            oscillatorNode.frequency.value = frequency;

            // Set the type of oscillator
            oscillatorNode.type = "square";
            gainNode.connect(newsAudioContext.destination);

            // Set the gain to the volume
            gainNode.gain.value = volume * 0.01;

            // Start audio with the desired duration
            oscillatorNode.start(newsAudioContext.currentTime);
            oscillatorNode.stop(newsAudioContext.currentTime + duration * 0.001);

            // Resolve the promise when the sound is finished
            oscillatorNode.onended = () => {
                resolve();
            };
        } catch (error) {
            reject(error);
        }
    });
}
