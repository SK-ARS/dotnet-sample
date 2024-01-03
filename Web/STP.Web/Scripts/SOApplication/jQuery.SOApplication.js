$(document).ready(function () {
$('body').on('click','#back-route', function(e) { 
  e.preventDefault();
  Backtoroute(this);
}); 
$('body').on('click','#btn1-BackToImportRouteList', function(e) { 
  e.preventDefault();
  BackToImportRouteList(this);
}); 
$('body').on('click','#btn2-BackToImportRouteList', function(e) { 
  e.preventDefault();
  BackToImportRouteList(this);
}); 
$('body').on('click','#btn3-BackToImportRouteList', function(e) { 
  e.preventDefault();
  BackToImportRouteList(this);
}); 
$('body').on('click','#btn-Backtoroute', function(e) { 
  e.preventDefault();
  Backtoroute(this);
}); 
$('body').on('click','.btn-cloneroutes', function(e) { 
  e.preventDefault();
  CloneRoutes(this);
}); 
});
var delvehicleName;
var routePartIdVR1;
$(function () {
    var selectedVal = $('#tab_4 #pageSizeVal').val();
    var pageSize = selectedVal;

    $("#tab_4 #pageSizeSelect option[value='" + pageSize + "']").attr("selected", "selected");
});

//fetching VR1 app value from querystring
function getQuery(key) {
    var temp = location.search.match(new RegExp(key + "=(.*?)($|\&)", "i"));
    if (!temp) return
    else
        return temp[1];
}

function SearchComponent() {
    var txt_srch = $('#searchString').val();
    var vehicleIntend = $('#vehicleIntend').val();
    var vhclType = $('#vehicleType').val();
    if (vehicleIntend == 'Construction and use') {
        vehicleIntend = 'c and u';
    }
    AjaxSearchGrid(txt_srch, vehicleIntend, vhclType);

    var isVR1 = $('#vr1appln').val();

    if (isVR1 == 'True') {
        movclassification = "VR1";
    }
    else {
        movclassification = "Special Order"
    }
    $('#form_pagination input:hidden').remove();
    $('#form_pagination').append('<input type="hidden" id="movclassification" name="movclassification" value="' + movclassification + '" /><input type="hidden" id="isApplicationVehicle" name="isApplicationVehicle" value="true" /><input type="hidden" id="srchId" name="searchString" value="' + txt_srch + '" /><input type="hidden" id="searchVhclType" name="searchVhclType" value="' + vhclType + '" />');
}

function SerachApplicationRoute() {
    var txt_srch = $('.searchbox').val();
    
    // AjaxRouteSearchGrid(txt_srch);
}

function TextchangeForRouteTab() {
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    if (AppRevId != 0 && $('#submittedRoute').val() == 0) {
        if ($('#TextChangeFlagSO').val() == true) {
            showWarningPopDialog('You have changes to be saved, do you want to continue?', 'Yes', 'No', 'CloneRoutes', '', 1, 'info');
        }
    }
}

function CloneRoutes() {
    var VR1Applciation = $('#VR1Applciation').val();
    var SORTApplicationchk = $('#SORTApplication').val();
    if (VR1Applciation == 'False' && SORTApplicationchk == 'True') {
        SOvalidationfun1();
        SORTShowSORoutePage();
    }
    else {
        $('#TabOption').show();
        $(".slidingpanelstructures").removeClass("show").addClass("hide");
        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        //added by poonam (13.8.14)
        if ($('#CRNo').val() == undefined) {
            var vr1contrefno = $('#VR1ContentRefNo').val();
        }
        else {
            var vr1contrefno = $('#CRNo').val();// $('#VR1ContentRefNo').val();
        }
        var vr1versionid = $('#VersionId').val();
        //-----------
        var vr1 = $('#vr1appln').val();
        if (AppRevId != 0 && $('#submittedRoute').val() == 0) {
      
            $.ajax({
                url: "../Application/ListImportedRouteFromLibrary",
                type: 'post',
                async: false,
                cache: false,
                //contentType: 'application/json; charset=utf-8',
                data: { apprevisionId: AppRevId, CONT_REF_NUM: vr1contrefno, VersionId: vr1versionid },
                success: function (data) {
                    $('#tab_4').show();
                    $('#RoutePart').html('');
                    $('#RoutePart').html(data);
                    $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
                    $("#leftpanel").html('');
                    $("#leftpanel").show();
                    LoadLeftPanal_SoRoute();

                    CheckSessionTimeOut();
                    if (vr1 == 'True') {
                        var reddet = $('#Reduceddetailed').val();
                        VR1validationfun1(reddet);
                    }
                    else {
                        SOvalidationfun1();
                    }
                    ListImportedRouteFromLibraryInit();
                },
                error: function () {
                }
            });
        
        }
        addscroll();
        $("#mapTitle").html('');
    }
}

function LoadLeftPanal_SoRoute() {
    $("#leftpanel").html('');
    $.ajax({
        url: "../Application/SoRoute",
        type: 'post',
        async: false,
        cache: false,
        success: function (Page) {
            $("#leftpanel").html(Page);
        },
        error: function () {
        }
    });
}

//so validationbefore submit
function chksosubmitvalidation() {
    var returnresult = "";
    var validres = "";
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: "../Application/CheckSOValidation",
        type: 'post',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId },
        beforeSend: function () {
            startAnimation();
        },

        success: function (data) {

            var ajaxdata = data;
            $('#SOSubmit').val(ajaxdata);
            if (data == "1") //ready to submit
            {
                validres = "SO application is now ready for submission";// To submit, go to the General pane and press the submit button";
               
            }
            else if (data == "2")  // vehicle not added
            {
                validres = "SO application is not ready to submit no vehicle found";
            }
            else if (data == "3")//route not added
            {
                validres = "SO application is not ready to submit no route found";
            }
            else if (data == "4") //no vehicle - no route
            {
                validres = "SO not ready to submit no vehicle configurations found no route found";
            }
            else if (data == "5") //vehicle not assigned to route
            {
                validres = "In SO applciation vehicle configurations not assigned to any route part.";
            }
            else {
                validres = "";
            }
               
           
            returnresult = validres;
            return returnresult;
        },
        complete: function () {
           
            FilterApplied();
            stopAnimation();
            return returnresult;
        },

        error: function () {    
        }
    });
   
   
     
    // showdialog("mSG " + returnresult);
    //  setTimeout(function () { return;  }, 6000);
   
    //setTimeout(function () { }, 1250)
   // showWarningDialog('successfully'+returnresult, 'Ok', '', ReloadLocation, null, 1, 'info');
    return returnresult;
}


//so validationbefore submit
function chkvr1submitvalidation() {
    var returnresult = "";
    var validres = "";
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: "../Application/CheckVR1Validation",
        type: 'post',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId },
        beforeSend: function () {
            startAnimation();
        },

        success: function (data) {
                        
            var ajaxdata = data;
            $('#SOSubmit').val(ajaxdata);
            if (data == "1") //ready to submit
            {
                validres = "VR1 application is now ready for submission";// To submit, go to the General pane and press the submit button";

            }
            else if (data == "2")  // vehicle not added
            {
                validres = "VR1 application is not ready to submit no vehicle found";
            }
            else if (data == "3")//route not added
            {
                validres = "VR1 application is not ready to submit no route found";
            }
            else if (data == "4") //no vehicle - no route
            {
                validres = "VR1 application is not ready to submit no vehicle configurations found  no route found";
            }
            else if (data == "5") //vehicle not assigned to route
            {
                validres = "VR1 application is not ready to submit vehicle configurations not assigned to any route part.";
            }

            else if (data == "6") //vehicle not assigned to route
            {
            }
            else {
                validres = "";
            }

            
            returnresult = validres;
            return returnresult;
        },
        complete: function () {
            
            FilterApplied();
            stopAnimation();
            return returnresult;
        },

        error: function () {
        
            //location.reload();
        }

    });
    // console.log('valdres'+returnresult +'dasdsa');    
    // showdialog("mSG " + returnresult);
    //  setTimeout(function () { return;  }, 6000);
    //console.log(returnresult);
    //setTimeout(function () { }, 1250)

    return returnresult;
}
//function remove href from pagination ul li
function removeHLinks() {
    $('#tab_3').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    
    //activate first link
    //$('.pagination').find('li:first').addClass('activated');
}

//function Pagination
function PaginateGrid() {
    //method to paginate through page numbers
    $('#tab_3').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPagination(pageNum);
    });
    PaginateToLastPage();
    PaginateToFirstPage();
    PaginateToNextPage();
    PaginateToPrevPage();
}

//method to paginate to last page
function PaginateToLastPage() {
    $('#tab_3').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxPagination(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPage() {
    $('#tab_3').find('.PagedList-skipToFirst').click(function () {
        AjaxPagination(1);
    });
}

//method to paginate to Next page
function PaginateToNextPage() {
    $('#tab_3').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#tab_3').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPagination(nextPage);
    });
}

//method to paginate to Previous page
function PaginateToPrevPage() {
    $('#tab_3').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#tab_3').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPagination(prevPage);
    });
}

//function Ajax call fro pagination
function AjaxPagination(pageNum) {
    var movclassification;
    var isVR1 = $('#vr1appln').val();
   
    if (isVR1 == 'True') {
        movclassification = "VR1";
    }
    else {
        movclassification = "Special Order";
    }
   
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    var txt_srch = $('.serchlefttxt').val();
    $.ajax({
        url: '../VehicleConfig/VehicleConfigList',
        data: { page: pageNum, movclassification: movclassification,isApplicationVehicle: true, searchString: txt_srch },
        type: 'GET',
        async: false,
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (result) {
            $('#tab_3').html($(result).find('#div_fleet').html(), function () {
            });
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            removeHLinks();
            PaginateGrid();
            //fillPageSizeSelect();
           
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            $('.loading').hide();
            $('.serchlefttxt').val(txt_srch);
        }
    });
}


//route pagination using in VR 1 application routes from route list------------------------------------------------------------------

function PaginateGridvrRoute() {
    //method to paginate through page numbers
    $('#tab_3').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPaginationforsoroute(pageNum);
        //console.log($('.active a').html());
    });
    PaginateVrRouteToLastPage();
    PaginateVrRouteToFirstPage();
    PaginateVrRouteToNextPage();
    PaginateVrRouteToPrevPage();
}

//method to paginate to last page
function PaginateVrRouteToLastPage() {
    $('#tab_3').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxPaginationforsoroute(pageCount);
    });
}

//method to paginate to first page
function PaginateVrRouteToFirstPage() {
    $('#tab_3').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationforsoroute(1);
    });
}

//method to paginate to Next page
function PaginateVrRouteToNextPage() {
    $('#tab_3').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#tab_3').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationforsoroute(nextPage);
    });
}

//method to paginate to Previous page
function PaginateVrRouteToPrevPage() {
    $('#tab_3').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#tab_3').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationforsoroute(prevPage);
    });
}


function changePageSizeRoutePartLibrary(_this) {
   
    var isVR1 = $('#vr1appln').val();
    if (isVR1 == "True") {
        var routeType = 2;
    }
    else {
        var routeType = 1;
    }

    var pageSize = $(_this).val();
    var SearchString = $('#SearchString').val();
    // var SearchType = $('#hdnUserSearchType').val();
    $.ajax({
        url: '../Routes/RoutePartLibrary',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, SearchString: SearchString, isApplicationroute: true, RouteType: routeType },
        beforeSend: function () {

            startAnimation();

        },
        success: function (result) {
            
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html($(result).find('#div_Route').html());
            //$('#VR1route').html('');
            //$('#VR1route').html($(result).find('#div_Route').html());
            removeHLinks();
            PaginateGridvrRoute();
            removerouteHLinks();
            PaginateGridsoroute();

            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#pagesize').val(pageSize);

            $("#pageSizeSelect option[value='"+pageSize+"']").attr("selected", "selected");
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {

            stopAnimation();
        }
    });
}

function load_leftpanel() {
    var IsSOApplication = $('#hdnIsSOApplication').val();
    var SearchString = $('#hdnSearchString').val();
    var url = '../Routes/searchRouteLibrary';
    $.ajax({
        url: url,
        type: 'POST',
        async: false,
        data: { IsSOApplication: IsSOApplication, SearchString: SearchString, isApplicationroute: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html($(page));
            $('#leftpanel').find("form").removeAttr('action', "");
            // $('#leftpanel_quickmenu').find("#SearchString").removeAttr('keyup', "");
            $('#leftpanel').find("#SearchString").unbind("keyup");
           
            $('#leftpanel').find("form").submit(function (event) {
                showleftpanelforroutesearch();
                event.preventDefault();
            });

            $('#leftpanel').find("#SearchString").keyup(function () {
               // routesearchblank();
            });
            $('#leftpanel').find('#btnCreate').hide();
            $('#leftpanel').show();
            removerouteHLinks();
            PaginateGridsoroute();
        },
        complete: function () {
            stopAnimation();
        }
    });

}


//route list search button
function showleftpanelforroutesearch() {
    var isVR1 = $('#vr1appln').val();
    if (isVR1 == "True") {
        var routeType = 2;
    }
    else {
        var routeType = 1;
    }
    var SearchString = $('#SearchString').val();
    $.ajax({
        url: '../Routes/RoutePartLibrary',
        data: { SearchString: SearchString, flag: 1, RouteType: routeType },
        type: 'GET',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            //$('#tab_3').html('');
            // $('#tab_3').show();
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html($(page).find('#div_Route').html());
            $('#RoutePart').find('.green').removeAttr('href').css("cursor", "pointer");
            //$('#RoutePart').find('.green').bind('click', unhook);
            removerouteHLinks();
            PaginateGridsoroute();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function routesearchblank() {
    var isVR1 = $('#vr1appln').val();
    if (isVR1 == "True" ) {
        var routeType = 2;
    }
    else {
        var routeType = 1;
    }

    var val = $('#SearchString').val();
   // if (val == "") {
      
        $.ajax({
            url: '../Routes/RoutePartLibrary',
            data: { flag: 1, isApplicationroute: true, RouteType: routeType },
            type: 'POST',
            async: false,
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#tab_4').show();
                $('#RoutePart').html('');
                $('#RoutePart').html($(page).find('#div_Route').html());
                $('#RoutePart').find('.green').removeAttr('href').css("cursor", "pointer");
                //$('#RoutePart').find('.green').bind('click', unhook);
                removerouteHLinks();
                PaginateGridsoroute();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    //}
}


//Back for route
function Backtoroute() {
    routelistfromlibrary();
    $('#leftpanel_quickmenu').html('');
    $("#leftpanel").html('');
    $("#leftpanel").show();
    load_leftpanel();
    $("#mapTitle").html('');
    $("#mapTitle").html('<h4 style="color:#414193;">Import list</h4> <br/>');
}


//Backtoroute movement
function Backtoroutemovement() {
    $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
    routelistfrommovement();
}

// Import Route in VR1Application
var Routeid;
var RouteName;
var rt_type;
var route_id = 0;

function ImportrouteinappLibrary(routeID, routetype) {
    WarningCancelBtn();
    var routename = $('#btnrouteimport_' + routeID).data('name');
    var routeType = "PLANNED";
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    //added by poonam (13.8.14)
    if ($('#CRNo').val() == undefined) {
        var vr1contrefno = $('#VR1ContentRefNo').val();
    }
    else {
        var vr1contrefno = $('#CRNo').val();// $('#VR1ContentRefNo').val();
    }
    var vr1versionid = $('#VersionId').val();
    $.ajax({
        url: '../Application/SaveRouteInRouteParts',
        type: 'POST',
        async: true,
        cache: false,
        data: { routepartId: routeID, routetype: routetype, AppRevId: AppRevId, CONTENT_REF: vr1contrefno, VersionId: vr1versionid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            
            if (result.result != 0) {
                CheckIsBroken({ RoutePartId: result.result, IsReplanRequired: true }, function (brokenRouteList) {
                    BrokenRouteReplanSoAppJs(brokenRouteList, true);
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function checkESDAL1route(LibraryrouteId) {
        var routevar = 0;            //name assigned to span for view
        var ESDAL1sys = 0;
        $.ajax({

            url: '../Routes/GetNotifRouteAddress',
            type: 'POST',
            async: false,
            data: { LibraryRouteID: LibraryrouteId },
            success: function (result) {
                var datacollection = result, count = -1;
                
                if (datacollection != null)
                    count = datacollection.result.length;
                if (count == 0 || count == -1) {
                    //showWarningPopDialog('The route you are trying to import is from the previous ESDAL system and may contain gaps in the road network. Please import another route or create a new route.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                    ESDAL1sys = 1;//gaps in route points(ESDAL1 created route)
                }
                else {
                    routevar = datacollection.result[0].pointRouteVar;
                    if (routevar == 0) {
                        //showWarningPopDialog('The route you are trying to import is from the previous ESDAL system and may contain gaps in the road network. Please import another route or create a new route.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        ESDAL1sys = 1;//gaps in route points(ESDAL1 created route)
                    }
                    else {
                        ESDAL1sys = 0;//ESDAL2 created route
                    }
                }

            }
        });
        return ESDAL1sys;
}

var rt_Id = 0;
var Rt_type_pl = "planned";
$('body').on('click', '#import-inapp', function (e) {
   
    e.preventDefault();
    var RouteId = $(this).data('routeid');
    var HfRouteType = $(this).data('hfroutetype');
    Importrouteinapp(RouteId, HfRouteType);
});
$('body').on('click', '#importinapp', function (e) {

    e.preventDefault();
    var RouteId = $(this).data('rid');
    var HfRouteType = $(this).data('type');
    Importrouteinapp(RouteId, HfRouteType);
});
function Importrouteinapp(routeID, routetype) {
    var routename = $('#btnrouteimport_' + routeID).data('name');
    if (routename == null)
        routename = $("#RouteName1").text();
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var SOVersionID = $('#RevisionID').val(); //Previous Movement version id
    var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRT").val();
    //added by poonam (14.8.14)
    var vr1contrefno = $('#VR1ContentRefNo').val();
    var vr1versionid = $('#VersionId').val();
    //-----------
    $("#divCurrentMovement").hide();
    $.ajax({
        url: '../Application/SaveRouteInAppParts',
        type: 'POST',
        async: false,
        cache: false,
        data: { routepartId: routeID, routeType: routetype, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            
            if (result != 0) {
                CheckIsBroken({ RoutePartId: result, IsReplanRequired: true }, function (response) {
                    BrokenRouteReplanSoAppJs(response);
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function BrokenRouteReplanSoAppJs(response, isLib = false) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) { //check in the existing route is broken
        var res = response.Result[0];
        if (res.IsReplan > 1) {
            GetReplanMessageSoAppJs(false, isLib);
        }
        else {
            GetReplanMessageSoAppJs(true, isLib, response.autoReplanSuccess > 0);
        }
    } else {
        ShowWarningPopupMapupgarde('"' + routename + '" route imported for this application', function () {
            $('#WarningPopup').modal('hide');
            SelectedRouteFromLibraryForVR1();
        });
    }
    $("#RoutePart").show();
    $("#ShowDetail").hide();
    $("#RouteMap").html('');
    if ($('#UserTitle').html() == "SORT Portal")
        CloneRoutesSort();
    else
        CloneRoutes();
}
function GetReplanMessageSoAppJs(isReplan, isLib, isReplanSuccess = false) {
    var msg = "";
    if (!isReplan) {
        msg = isLib ? BROKEN_ROUTE_MESSAGES.SO_APP_MESSAGE_IS_REPLAN_NOT_POSSIBLE_FROM_LIBRARY : BROKEN_ROUTE_MESSAGES.SO_APP_MESSAGE_IS_REPLAN_NOT_POSSIBLE_NOT_FROM_LIBRARY;
    }
    else {
        if (isReplanSuccess) {
            $('#IsRouteReplanned').val("true");
            msg = isLib ? BROKEN_ROUTE_MESSAGES.SO_APP_MESSAGE_IS_REPLAN_SUCCESS_FROM_LIBRARY : BROKEN_ROUTE_MESSAGES.SO_APP_MESSAGE_IS_REPLAN_SUCCESS_NOT_FROM_LIBRARY;
        }
        else {
            msg = isLib ? BROKEN_ROUTE_MESSAGES.SO_APP_MESSAGE_IS_REPLAN_FAILED_FROM_LIBRARY : BROKEN_ROUTE_MESSAGES.SO_APP_MESSAGE_IS_REPLAN_FAILED_NOT_FROM_LIBRARY;
        }
    }
    ShowWarningPopupMapupgarde(msg, function () {
        $('#WarningPopup').modal('hide');
        SelectedRouteFromLibraryForVR1();
    });
}
function SelectedRouteFromLibraryForVR1() {
    var VR1Applciation = $('#VR1Applciation').val();
    var SORTApplicationchk=$('#SORTApplication').val();
    WarningCancelBtn();
    addscroll();
    $("#mapTitle").html('');
    $('#leftpanel').html('');
    $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
    if ($('#CRNo').val() != "" &&  String($('#CRNo').val()) != "undefined") {
        CloneRoutesNotify();
    }
    if (VR1Applciation == 'False' && SORTApplicationchk == 'True') {
        SOvalidationfun1();
        SORTShowSORoutePage();
    }
    else {
        $("#leftpanel").load('../Application/SoRoute', function () {
            CheckSessionTimeOut();
        });
        CloneRoutes();
    }
}
//function Load Grid for Vr1
function ListSelectedRouteforVR1() {
  
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: "../Application/ListImportedRouteFromLibrary",
        type: 'GET',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html(data);
            ListImportedRouteFromLibraryInit();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
    setTittle();
}

function setTittle() {
    var routeFlag = $('#hf_RouteFlag').val();
    if (routeFlag == '2')
    { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for SO  - Route</h3>'); }
    else if (routeFlag == '1' || routeFlag == '3')
    { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for VR-1 - Route</h3>'); }
}
function SelectedRouteFromLibraryForSO() {
    ListSelectedRouteforSO();
}

//function Load Grid for SO
function ListSelectedRouteforSO() {
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: "../Application/ListImportedRouteFromLibrary",
        type: 'POST',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html(data);
            $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
            ListImportedRouteFromLibraryInit();
        },
        error: function () {
            location.reload();
        }
    });
}


function BackToImportRouteList() {
    
    ListSelectedRouteforVR1();
    $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
    $('#leftpanel').html('');
    $("#mapTitle").html('');
    $("#leftpanel").html('');
    $("#leftpanel").show();
    $(".slidingpanelstructures").removeClass("show").addClass("hide");
    var status = $('#SortStatus').val();
    if (status == "CandidateRT") {
        //$("#leftpanel").load('../SORTApplication/SortRoutes?CheckerId=' + _checkerid + '&CheckerStatus=' + _checkerstatus + '&IsCandLastVersion=' + iscandlastversion + '&planneruserId=' + plannruserid + '&appStatusCode=' + appstatuscode);
        BindRouteParts();
    }
    else {
        $("#leftpanel").load('../Application/SoRoute');
    }
    CheckSessionTimeOut();
}
//view selected route 

//-----------------------------------------------------------------------------------


//for route pagination
//function remove href from pagination ul li
function removerouteHLinks() {
    $('#tab_4').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}
//function Pagination
function PaginateGridsoroute() {
    //method to paginate through page numbers
    $('#tab_4').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPaginationforsoroute(pageNum);
        //console.log($('.active a').html());
    });
    PaginateRouteToLastPage();
    PaginateRouteToFirstPage();
    PaginateRouteToNextPage();
    PaginateRouteToPrevPage();
}

//method to paginate to last page
function PaginateRouteToLastPage() {
    $('#tab_4').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#tab_4 #TotalPages').val();
        AjaxPaginationforsoroute(pageCount);
    });
}

//method to paginate to first page
function PaginateRouteToFirstPage() {
    $('#tab_4').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationforsoroute(1);
    });
}

//method to paginate to Next page
function PaginateRouteToNextPage() {
    $('#tab_4').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#tab_4').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationforsoroute(nextPage);
    });
}

//method to paginate to Previous page
function PaginateRouteToPrevPage() {
    $('#tab_4').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#tab_4').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationforsoroute(prevPage);
    });
}

function AjaxPaginationforsoroute(pageNum) {
    //mirza 02-06-2014
    var SearchString = $('#SearchString').val();
    var isVR1 = $('#vr1appln').val();
    if (isVR1 == "True") {
        var routeType = 2;
    }
    else {
        var routeType = 1;
    }
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $('#divMap').hide();
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    $.ajax({

        url: '../Routes/RoutePartLibrary',
        type: 'GET',
        cache: false,
        async: false,
        data: {SearchString: SearchString, page: pageNum, pageSize: pageSize, isApplicationroute: true, RouteType: routeType },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {

            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html($(result).find('#div_Route').html());
            //$('#RoutePart').find('.green').removeAttr('href').css("cursor", "pointer");
            //$('#RoutePart').find('.green').bind('click', unhook);
            ////$('#VR1route').html('');
            ////$('#VR1route').html($(result).find('#div_Route').html());
            ////removeHLinks();
            ////PaginateGridvrRoute();
            //removerouteHLinks();
            //PaginateGridsoroute();
            //fillPageSizeSelect();
            //$('#pageSizeVal').val(pageSize);
            //$('#pageSizeSelect').val(pageSize);


            $('#RoutePart').find('.green').removeAttr('href').css("cursor", "pointer");
            $("#mapTitle").html('<h4 style="color:#414193;">Import list</h4> <br/>');
            //$('#RoutePart').find('.green').bind('click',unhook);
            //removeHLinks();
            //PaginateGridvrRoute();
            removerouteHLinks();
            PaginateGridsoroute();
            fillPageSizeSelect();
            load_leftpanel();


            $("#pageSizeSelect option[value='" + pageSize + "']").attr("selected", "selected");
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#overlay").hide();
            $('.loading').hide();
        }
    });
    ;
}






//function Pagination for SO Movement------------------------------
function PaginateGridsomovement() {
    //method to paginate through page numbers
    $('#tab_3').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPaginationforsomovemenet(pageNum);
        //console.log($('.active a').html());
    });
    PaginateToLastPagesomovement();
    PaginateToFirstPagesomovement();
    PaginateToNextPagesomovement();
    PaginateToPrevPagesomovement();
}

//method to paginate to last page
function PaginateToLastPagesomovement() {
    $('#tab_3').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxPaginationforsomovemenet(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPagesomovement() {
    $('#tab_3').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationforsomovemenet(1);
    });
}

//method to paginate to Next page
function PaginateToNextPagesomovement() {
    $('#tab_3').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#tab_3').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationforsomovemenet(nextPage);
    });
}
//method to paginate to Previous page
function PaginateToPrevPagesomovement() {
    $('#tab_3').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#tab_3').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationforsomovemenet(prevPage);
    });
}

//
function AjaxPaginationforsomovemenet(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRoute").val();
    var pageSize = selectedVal;
    $.ajax({
        url: '../Movements/MovementList',
        type: 'GET',
        cache: false,
        async: false,
        data: { page: pageNum, pageSize: pageSize, MovementListForSO: true, showrtveh:ShowPrevMoveSortRoute },
        beforeSend: function () {
            $("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
                       
            $('#tab_3').html($(result).find('#divforsofilter').html());
            //location.reload();
            removeHLinks();
            PaginateGridsomovement();
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#overlay").hide();
            $('.loading').hide();
        }
    });
}

//Advanced search for vehicle tab in SO app
function AdvSearchforSOApplivation() {
    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRoute").val();
    var SortOrgId = $("#SortOrgId").val();
    var isVR1 = $('#vr1appln').val();
    if (isVR1 == 'True')
    {
        MovementListForSO = false;
        VR1route = true;
    }
    else
    {
        MovementListForSO = true;
        VR1route = false;
    }
    var formData = {
        'MyReference': $('input[name=MyReference]').val(),
        'Keyword': $('input[name=Keyword]').val(),
        'ESDALReference': $('input[name=ESDALReference]').val(),
        'GrossWeight': $('input[name=GrossWeight]').val(),
        'OverallWidth': $('input[name=OverallWidth]').val(),
        'Length': $('input[name=Length]').val(),
        'Height': $('input[name=Height]').val(),
        'AxleWeight': $('input[name=AxleWeight]').val(),
        'MovementFromDate': $('input[name=MovementFromDate]').val(),
        'MovementToDate': $('input[name=MovementToDate]').val(),
        'HaulierName': $('input[name=HaulierName]').val(),
        'SONum': $('input[name=SONum]').val(),
        'VRNum': $('input[name=VRNum]').val(),
        'ApplType': $('input[name=ApplType]').val(),
        'LoadDetails': $('input[name=LoadDetails]').val(),
        'StartPoint': $('input[name=StartPoint]').val(),
        'EndPoint': $('input[name=EndPoint]').val(),
        'StartOrEnd': $('input[name=StartPoint]').val(),
        'StartOrEnd': $('input[name=EndPoint]').val()
        
    };

    $.ajax({
        type: 'POST',
        data: formData,
        url: '../Movements/SetAdvancedSearchData?MovementListForSO=' + MovementListForSO + '&VR1route=' + VR1route + '&ShowPrevMoveSortRoute=' + ShowPrevMoveSortRoute+'&SortOrgId='+SortOrgId,
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#tab_3').show();
            $('#tab_3').html('');
            $('#tab_3').html($(page).find('#divforsofilter').html());
            //$('#RoutePart').find('h4').text('Route from previous movement ');
            $('#tab_3').find("form").removeAttr('action', "");
            $('#tab_3').find("form").submit(function (event) {
                AdvSearchforSOApplivation();
                event.preventDefault();
            });
            //removeHLinks();
            //PaginateGridsomovement();
           // PaginateGridSO();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

//function Pagination for SO Movement---------------------------------


//function Pagination for VR1 Movement------------------------------

function removeroutemovementHLinks() {
    $('#tab_4').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}

function PaginateGridvrmovement() {
    //method to paginate through page numbers
    $('#tab_4').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPaginationforvrmovement(pageNum);
        //console.log($('.active a').html());
    });
    
    PaginateToLastPagevrmovement();
    PaginateToFirstPagevrmovement();
    PaginateToNextPagevrmovement();
    PaginateToPrevPagevrmovement();
}

//method to paginate to last page
function PaginateToLastPagevrmovement() {
    $('#tab_4').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#tab_4 #TotalPages').val();
       
        pageCount = $("#totalPageCount").val();
        AjaxPaginationforvrmovement(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPagevrmovement() {
    $('#tab_4').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationforvrmovement(1);
    });
}

//method to paginate to Next page
function PaginateToNextPagevrmovement() {
    $('#tab_4').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#tab_4').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationforvrmovement(nextPage);
    });
}
//method to paginate to Previous page
function PaginateToPrevPagevrmovement() {
    $('#tab_4').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#tab_4').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationforvrmovement(prevPage);
    });
}

// Ajax pagination for routes  from previous movements
function AjaxPaginationforvrmovement(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var VIsVR1route = $('#HFVR1route').val();
    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRoute").val();
   
    var pageSize = selectedVal;
    $.ajax({
        url: '../Movements/MovementList',
        type: 'GET',
        cache: false,
        //async: false,
        data: { page: pageNum, pageSize: pageSize, MovementListForSO: true,showrtveh:ShowPrevMoveSortRoute, VR1route: VIsVR1route },
        beforeSend: function ()
        {
            startAnimation();
        },
        success: function (result)
        {
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html($(result).find('#divforsofilter').html());
            $('#RoutePart').find('h4').text('Route from previous movement ');
            removeroutemovementHLinks();
            PaginateGridvrmovement();
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function ()
        {
            stopAnimation();
        }
    });
}

// change page size for movement list in VR 1 application for route tab
function changePageSizeforroutemov(_this) {
   
    var IsSOroute = $("#IsSOroute").val();
    var IsSOvehicle = $("#IsSOvehicle").val();
    var IsNotifPreMovRoute = $("#IsNotifPreMovRoute").val();
    var VIsVR1route = $('#HFVR1route').val();
    var pageSize = $(_this).val();
    $.ajax({
        url: '../Movements/MovementList',
        type: 'GET',
        cache: false,
        //async: false,
        data: { pageSize: pageSize, MovementListForSO: true, VR1route: VIsVR1route, IsSOroute: IsSOroute, IsNotifPreMovRoute: IsNotifPreMovRoute },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            //flag ..route & vehi


            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html($(result).find('#divforsofilter').html());
            $('#RoutePart').find('h4').text('Route from previous movement ');
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            removeroutemovementHLinks();

            PaginateGridvrmovement();

            $("#tab_4 #pageSizeSelect option[value='" + pageSize + "']").attr("selected", "selected");
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });


}

// change page size for movement list in VR 1 application for vehicle tab
function changePageSizeforvehiclemov(_this, ShowPrevMoveSortRoute)
{
   
    var IsSOroute = $("#IsSOroute").val();
    var IsSOvehicle = $("#IsSOvehicle").val();
   // var ShowPrevMoveSortRoute =  $("#ShowPrevMoveSortRoute").val();
    
    if ($("#IsSOroute").val() == true) {
        IsSOroute = true;
    }
    if ($("#IsSOvehicle").val() == true) {
        IsSOvehicle = true;
    }
    var pageSize = $(_this).val();
    $.ajax({
        url: '../Movements/MovementList',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, MovementListForSO: true, showrtveh: ShowPrevMoveSortRoute, IsSOvehicle: IsSOvehicle, IsSOroute: IsSOroute },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#tab_3').show();
            $('#tab_3').html('');
            $('#tab_3').html($(result).find('#divforsofilter').html());
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            //removeroutemovementHLinks();
            // PaginateGridvrmovement();
            removeHLinks();
            PaginateGridsomovement();
            fillPageSizeSelect();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });


}


function AdvSearchforApplivation()
{
    var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRoute").val();
    var SortOrgId = $("#SortOrgId").val();
    var isVR1 = $('#vr1appln').val();
   
    if (isVR1 == 'True' ) {
        MovementListForSO = false;
        VR1route = true;
        IsNotify = false;
    }
    else {
        MovementListForSO = true;
        VR1route = false;
        IsNotify = false;
    }
    var IsNotify = $("#IsNotifyRouteTab").val();
    if (IsNotify == "YES") {
        MovementListForSO = false;
        VR1route = false;
        IsNotify = true;
    }
    else {
        IsNotify = false;
    }
    var formData = {
        'MyReference': $('input[name=MyReference]').val(),
        'Keyword': $('input[name=Keyword]').val(),
        'ESDALReference': $('input[name=ESDALReference]').val(),
        'GrossWeight': $('input[name=GrossWeight]').val(),
        'OverallWidth': $('input[name=OverallWidth]').val(),
        'Length': $('input[name=Length]').val(),
        'Height': $('input[name=Height]').val(),
        'AxleWeight': $('input[name=AxleWeight]').val(),
        'MovementFromDate': $('input[name=MovementFromDate]').val(),
        'MovementToDate': $('input[name=MovementToDate]').val(),
        'HaulierName': $('input[name=HaulierName]').val(),
        'SONum': $('input[name=SONum]').val(),
        'VRNum': $('input[name=VRNum]').val(),
        'ApplType': $('input[name=ApplType]').val(),
        'LoadDetails': $('input[name=LoadDetails]').val(),
        'StartPoint': $('input[name=StartPoint]').val(),
        'EndPoint': $('input[name=EndPoint]').val(),
        'StartOrEnd': $('input[name=StartPoint]').val(),
        'StartOrEnd': $('input[name=EndPoint]').val()
    };

    $.ajax({
        type: 'POST',
        data: formData,
        url: '../Movements/SetAdvancedSearchData?MovementListForSO=' + MovementListForSO + '&isFromNotif=' + IsNotify + '&VR1route=' + VR1route + '&ShowPrevMoveSortRoute=' + ShowPrevMoveSortRoute + '&SortOrgId=' + SortOrgId,
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html($(page).find('#divforsofilter').html());
            $('#RoutePart').find('h4').text('Route from previous movement ');
            $('#RoutePart').find("form").removeAttr('action', "");
            $('#RoutePart').find("form").submit(function (event) {
                AdvSearchforApplivation();
                event.preventDefault();
            });
            removeroutemovementHLinks();
            PaginateGridvrmovement();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
            $('#tab_3').html('');
        }
    });
}

//function Pagination for VR 1 Movement---------------------------------



var selectVehID;
var configurationName;
function viewConfig(id, configName) {
    configurationName = configName;
    selectVehID = id;
    ViewConfiguration(id);

}


function ViewConfiguration(id) {
    var isImportConfig = false;
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    if (AppRevId != 0) {
        isImportConfig = true
    }
    $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
        DynamicTitle('View configuration');

        $.ajax({
            url: '../VehicleConfig/ViewConfiguration',
            type: 'GET',
            cache: false,
            data: { vehicleID: id, isImportConfiguration: isImportConfig },
            beforeSend: function () {
                $("#overlay").show();
                $("#dialogue").hide();
                $('.loading').show();
            },
            success: function (result) {
                $('#Config-body').html(result);
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                $("#dialogue").show();
                removescroll();
                $("#overlay").show();
                //$("#Component_Id").val(id);
                $('.loading').hide();
            }
        });

    });
}

//$('#span-close').live('click', function () {
$('#span-close').click(function () {
    $('#overlay').hide();
    addscroll();
    WarningCancelBtn();
       });

function changePageSize(_this) {
   
    var prevRoutepagesize = $('#PrevMoveRoutePageSize').val();
    var prevVehpagesize = $('#PrevMoveVehPageSize').val();
    var pageSize = $(_this).val();
    var isVR1 = $('#vr1appln').val();
    var Isnotif = $('#ISNotif').val();
    var MovementClassCode = $('#MoveClassCode').val();
    var txt_srch = $('#searchString').val();
    var vehicleIntend = $('#vehicleIntend').val();
    
    if (prevRoutepagesize == 'True') {
        $('#SelectCurrentMovementsVehicle1').hide();
        $('#SelectCurrentMovementsVehicle2').hide();
        $.ajax({
            url: '../SORTApplication/SORTInbox',
            type: 'GET',
            cache: false,
            data: { structID: null, pageSize: pageSize, IsPrevtMovementsVehicleRoute: true },
            beforeSend: function () {
                startAnimation('Loading related movements...');
                //if ($('#SearchPrevMove').val() == "No") {
                //    ClearAdvancedSORT()
                //}
            },
            success: function (result) {
                $('#RoutePart').hide();
                $('#divCurrentMovement').show();

                $('#divCurrentMovement').html($(result).find('#div_MoveList_advanceSearch').html());

               // CheckSessionTimeOut();
            },
            complete: function () {
                //if ($('#SearchPrevMove').val() == "Yes") {
                //    ShowAdvanced();

                //}
                $('#pageSizeSelect').val(pageSize);
                stopAnimation();
               // scrolltotop();
            }
        });

    }
    else if (prevVehpagesize == 'True')
    {       
        $.ajax({
            url:  '../SORTApplication/SORTInbox',
            type: 'POST',
            cache: false,
            data: { structID: null, pageSize:pageSize, IsPrevtMovementsVehicle: true },
            beforeSend: function () {
                startAnimation('Loading related movements...');
                //if ($('#SearchPrevMoveVeh').val() == "No") {
                //    ClearAdvancedSORT()
                //}
            },
            success: function (result) {
                $('#tab_2').hide();
                $('#SelectCurrentMovementsVehicle').show();

                $('#SelectCurrentMovementsVehicle').html($(result).find('#div_MoveList_advanceSearch').html());
               // CheckSessionTimeOut();
            },
            complete: function () {
                //if ($('#SearchPrevMoveVeh').val() == "Yes") {
                //    ShowAdvanced();

                //}
                $('#pageSizeSelect').val(pageSize);
                stopAnimation();
            }
        });
    }
    else {
        if (Isnotif == 'True') {
            $.ajax({
                url: '../VehicleConfig/VehicleConfigList',
                type: 'GET',
                cache: false,
                async: false,
                data: { pageSize: pageSize, MovementClassCode: MovementClassCode, isNotifVehicle: true, searchString: txt_srch, searchVhclIntend: vehicleIntend },
                beforeSend: function () {
                    $("#overlay").show();
                    $('.loading').show();
                },
                success: function (result) {

                    $('#tab_2').html($(result).find('#div_fleet').html(), function () {
                    });
                    $('#pageSizeVal').val(pageSize);
                    $('#pageSizeSelect').val(pageSize);
                    removeHLinks();
                    PaginateGrid();
                    var x = fix_tableheader();
                    if (x == 1) $('#tableheader').show();
                    //location.reload();

                },
                error: function (xhr, textStatus, errorThrown) {
                    //other stuff
                    location.reload();
                },
                complete: function () {
                    $("#overlay").hide();
                    $('.loading').hide();
                }
            });
        }
        else {
            var movclassification = '';
            if ($('#vr1appln').length > 0) {
                if (isVR1 == 'True') {
                    movclassification = "VR1";
                }
                else {
                    movclassification = "Special Order"
                }
            }

            $.ajax({
                url: '../VehicleConfig/VehicleConfigList',
                type: 'GET',
                cache: false,
                async: false,
                data: { pageSize: pageSize, isApplicationVehicle: true, movclassification: movclassification, searchString: txt_srch, searchVhclIntend: vehicleIntend },
                beforeSend: function () {
                    $("#overlay").show();
                    $('.loading').show();
                },
                success: function (result) {

                    $('#tab_3').html($(result).find('#div_fleet').html(), function () {
                    });
                    $('#pageSizeVal').val(pageSize);
                    $('#pageSizeSelect').val(pageSize);
                    removeHLinks();
                    PaginateGrid();
                    //location.reload();

                },
                error: function (xhr, textStatus, errorThrown) {
                    //other stuff
                    location.reload();
                },
                complete: function () {
                    $("#overlay").hide();
                    $('.loading').hide();
                }
            });

        }
    }
}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}

//function Ajax call fro pagination
function AjaxSearchGrid(keyword, vehicleIntend, vhclType) {

    var isVR1 = $('#vr1appln').val();

    if (isVR1 == 'True') {
        movclassification = "VR1";
    }
    else {
        movclassification = "Special Order"
    }

    $.ajax({
        url: '../VehicleConfig/VehicleConfigList',
        data: { searchString: keyword, searchVhclIntend: vehicleIntend, searchVhclType: vhclType, isApplicationVehicle: true, movclassification: movclassification },
        type: 'GET',
        async: false,
        beforeSend: function () {
            $('.loading').show();
            startAnimation();
        },
        success: function (result) {
            $('#tab_3').html($(result).find('#div_fleet').html(), function () {
            });
            //removeHLinks();
            //PaginateGrid();
            fillPageSizeSelect();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
            $('.loading').hide();
            $('.serchlefttxt').val(keyword);
        }
    });
}


function AjaxRouteSearchGrid(keyword) {
    $.ajax({
        url: '../VehicleConfig/VehicleConfigList',
        data: { searchString: keyword, isApplicationVehicle: true },
        type: 'GET',
        async: false,
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (result) {
            $('#tab_3').html($(result).find('#div_fleet').html(), function () {
            });
            removerouteHLinks();
            PaginateGridsomovement();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            $('.loading').hide();
            $('.serchlefttxt').val(keyword);
        }
    });
}

var AppRevId;
//function submitdata(id, ApplicationRevId) {
    
//    SaveFleetConfiguration(id, ApplicationRevId);
//    //$('#leftpanel_quickmenu').html('');
//    //$("#leftpanel").html('');
//    //$("#leftpanel").show();
//    //$("#leftpanel").load('../Application/SoVehicle');
//}

function SelectVehicle() {
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var confName = configurationName;
    SaveFleetConfiguration(selectVehID, AppRevId, confName);
}

function Back() {
    SelectedFleetConfiguration();
    //$('#leftpanel_quickmenu').html('');
    //$("#leftpanel").html('');
    //$("#leftpanel").show();
    //$("#leftpanel").load('../Application/SoVehicle');
}

function SelectedFleetConfiguration() {
    LoadSelectedFleetConfiguration();
}

//function Load Grid
function LoadSelectedFleetConfiguration() {
    //-------sort part----
    var VR1Applciation = $('#VR1Applciation').val();
    var SORTApplicationchk = $('#SORTApplication').val();
    //--------------------
    //check from here
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var routePartId = $('#RoutePartId').val();
    if (routePartId == null || routePartId == 0)
    {
        routePartId = routePartIdVR1;
    }
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();

    $('#hidden_apprevisionId').val(AppRevId);
    $('#hidden_routepartId').val(routePartId);
    $('#hidden_VRAPP').val(isVR1);

    $('#form_list_vehicle').submit();
    
    $.ajax({
        url: "../Application/ListImportedVehicleConfiguration",
        type: 'GET',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId, routepartId: routePartId, VRAPP: isVR1 },
        before: function () {
            startAnimation();
        },
        success: function (data) {
            $('#tab_3').html('');
            $('#tab_3').html(data);
            $('#leftpanel').html('');
            $("#leftpanel").html('');
            $("#leftpanel").show();
            $("#leftpanel").load('../Application/SoVehicle');
            //$("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
            if (VR1Applciation == 'False' && SORTApplicationchk == 'True') {
                SOvalidationfun1();
                SORTShowSOVehiclePage();
            } else if (SORTApplicationchk == 'False') {
                SOvalidationfun1();
            }
            ListImportedVehicleConfigurationInit();
        },
        complete: function () {
            stopAnimation();
        },
        error: function () {
            location.reload();
        }
    });
}





var routeId;
//function to save vehicle Configuration
function SaveFleetConfiguration(id, ApplicationRevId, configName) {
   
    var isVR1 = $('#vr1appln').val();
    var routePartID = $('#RoutePartId').val();
    //added by poonam (13.8.14)
    var vr1contrefno = $('#VR1ContentRefNo').val();
    var vr1versionid = $('#VersionId').val();
    //-----------
    if (routePartID == "") {
        routePartID = 0;
    }
    routeId = routePartID;
    $.ajax({
        url: '../Application/SaveExistingFleetConfiguration',
        type: 'POST',
        cache: false,
        data: { vehicleId: id, apprevisionId: ApplicationRevId, isVR1: isVR1, routePartId: routePartID, versionid: vr1versionid, contentrefno: vr1contrefno },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            
            if (isVR1 == 'True') {
                //routePartIdVR1 = result;
                $('#RoutePartId').val(result);
            }

            showPopUpDialog('Configuration "'+configName+'" imported successfully', 'Ok', '', 'CloseModelPop', '', 1, 'info');
            //CloseModelPop
            var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var routePartId = $('#RoutePartId').val();
            if (routePartId == null || routePartId == 0)
            {
                routePartId = routePartIdVR1;
            }
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();

            $('#hidden_apprevisionId').val(AppRevId);
            $('#hidden_routepartId').val(routePartId);
            $('#hidden_VRAPP').val(isVR1);
            $('#form_list_vehicle').submit();
            //SelectedFleetConfiguration();
            $('#tab_3').find('#FleetConfiguration').hide();
            $('#leftpanel').html('');

            if (isVR1 == 'True')
            {
                var redDetails = $('#Reduceddetailed').val();
                VR1validationfun1(redDetails);
            }
            else {
                SOvalidationfun1();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

var vehID;
var cntTr;
////function delete component Config with alert
function DeleteSelectedComponent(_this, id) {
    delvehicleName = $(_this).attr('name');
    vehID = id;
    cntTr = $(_this).closest("tr");
    var Msg = "Do you want to delete '" + "" + "'" + delvehicleName + "'" + "" + "' ?";
    //showWarningDialog(Msg, 'No', 'Yes', WarningCancelBtn, DeleteComponent, 1, 'warning');
    showPopUpDialog(Msg, 'No', 'Yes', 'CloseModelPop', 'DeleteComponent', 1, 'warning');
   
}
//funciton to copy vehicle to route  part
function CopytootherRoutePart(_this, id) {
    //delvehicleName = $(_this).attr('name');
    // vehID = id;
    //  cntTr = $(_this).closest("tr");
    //var Msg = "Do you want to delete '" + "" + "'" + delvehicleName + "'" + "" + "' ?";
    ////showWarningDialog(Msg, 'No', 'Yes', WarningCancelBtn, DeleteComponent, 1, 'warning');
    //showWarningDialog(Msg, 'No', 'Yes', WarningCancelBtn, DeleteComponent, 1, 'warning');
    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    $.ajax({
        url: "../Application/CopytootherRoutePart",
        type: 'GET',
        async: false,
        //contentType: 'application/json; charset=utf-8',
        data: { apprevisionId: AppRevId },
        success: function (data) {
            $('#tab_4').show();
            $('#RoutePart').html('');
            $('#RoutePart').html(data);
        },
        error: function () {
            location.reload();
        }
    });
}

//function to delete component
function DeleteComponent() {
   
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = true;
    }
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;//added for log
    $.ajax({
        url: "../Application/DeleteSelectedVehicleComponent",
        type: 'POST',
        cache: false,
        async: false,
        data: { vehicleId: vehID, isVR1: isVR1, appRevID: ApplicationRevId },
        beforeSend: function () {
            startAnimation();
            CloseModelPop();
        },
        success: function (result) {
            if (result.Success) {
                //method
                showPopUpDialog('Configuration  "' + delvehicleName + '"  deleted successfully', 'Ok', '', 'CloseModelPop', '', 1, 'info');
                if (isNotify == 'True' || isNotify == 'true') {
                    // method to display notification vehicle
                    var contentNum = $('#CRNo').val();
                    var routePartID = $('#RoutePartId').val();
                    $('#tab_2').load('../Application/ListImportedVehicleConfiguration?routepartId=' + routePartID + '&ContentRefNo=' + contentNum + '&IsNotif=' + true, function () {
                        $('#div_notification_vehicle').find('input:radio').attr('checked', false);
                        ListImportedVehicleConfigurationInit();
                    });
                }
                else {
                SelectedFleetConfiguration();
            }
            }
            else {
               
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
            cntTr.remove();
        }

    });
}



//this section defines add component to fleet 
var vehicleappId;
var flag;
$('body').on('click', '.tofleet', function (e) {
    e.preventDefault();
    var vname = $(this).data('vname');

    AddToFleet(this, vname);
});
//function to Add to Fleet()
function AddToFleet(_this, vehName) {
    //console.log("AddToFleet");
    vehicleappId = $(_this).attr('id');
    CheckFormalNameExists(vehName);
}

//function to check the formal name exists during add to fleet
function CheckFormalNameExists(vehName) {
    
    $.ajax({
        url: '../Application/CheckFormalName',
        type: 'POST',
        cache: false,
        async: false,
        data: { VehicleName: vehName },
        success: function (data) {
            //veh_name = vehName;

            if (data.success > 0) {
                flag = 1;
                showWarningPopDialog('Configuration already exists. Do you want to over write?', 'No', 'Yes', 'CloseModelPop', 'AddComponentToFleet', 1, 'warning');
                //if (data.success > 1)

            }
            else {
                flag = data.success;
                AddComponentToFleet();
                //WarningCancelBtn();
            }
        }
    });
}

//function to add to fleet
function AddComponentToFleet() {
   
    var isVR1 = $('#vr1appln').val();
    var notifid=$('#NotificatinId').val();
    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = 'True';
    }
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;//added for log
    var orgId = 0;
    
    if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
        orgId = $('#Organisation_ID').val();
    }
    $.ajax({
        url: '../Application/AddConfigToFleet',
        type: 'POST',
        cache: false,
        async: false,
        data: { VehicleId: vehicleappId, flag: flag, isVR1: isVR1, appRevID: ApplicationRevId, isNotify: isNotify, NotificationId: notifid, organisationId: orgId },
        beforeSend: function () {
            $('#pop-warning').hide();
        },
        success: function (result) {
            if (result.success > 0) {

                showWarningPopDialog('Configuration added to fleet', 'Ok', '', 'CloseModelPop', '', 1, 'info');
                //WarningCancelBtn();
            }
            else {
                showWarningPopDialog('Not saved', 'Ok', '', 'CloseModelPop', '', 1, 'error');
                //WarningCancelBtn();
            }
        }
    });
}

function showPopUpDialog(message, btn1_txt, btn2_txt, btnOneAction, btnTwoAction, autofocus, type) {
    ResetDialog();
    $('.pop-message').html(message);
    if (btn1_txt == '') { $('.box_Cancel_btn').hide(); } else { $('.box_Cancel_btn').html(btn1_txt); $('.box_Cancel_btn').attr("onclick", btnOneAction + '();'); }
    if (btn2_txt == '') { $('.box_Ok_btn').hide(); } else { $('.box_Ok_btn').html(btn2_txt); $('.box_Ok_btn').attr("onclick", btnTwoAction+'();'); }

    //if (autofocus == 1) $('.box_warningBtn1').focus(); else $('.box_warningBtn2').focus();

    switch (autofocus) {
        //case 1: $('.box_warningBtn1').focus(); break;
        //case 2: $('.box_warningBtn2').focus(); break;
        case 1: $('.box_Cancel_btn').attr("autofocus", 'autofocus'); break;
        case 2: $('.box_Ok_btn').attr("autofocus", 'autofocus');  break;
        default: break;
    }

    switch (type) {
        case 'error': $('.message1').addClass("errror"); $('.popup1').css({ "background": '#fcd1d1' }); break;
        case 'info': $('.message1').addClass("info"); $('.popup1').css({ "background": '#cdecfe' }); break;
        case 'warning': $('.message1').addClass("warning"); $('.popup1').css({ "background": '#ffffd0' }); break;
        default: break;

    }

    $('#pop_dialog').show();

    //$('.box_Cancel_btn').click(function () {
    //    btnOneAction(function () {
    //        $('.box_Cancel_btn').unbind();
    //    })
    //});
    //$('.box_Ok_btn').click(function () {
    //    btnTwoAction(function () {
    //        $('.box_Ok_btn').unbind();
    //    })
    //});
    
}

//function for reseting the popup
function ResetDialog() {
    $('.message1').removeClass("errror");
    $('.message1').removeClass("info");
    $('.message1').removeClass("warning");
    $('.box_Cancel_btn').show();
    $('.box_Ok_btn').show();
}


function CloseModelPop() {
    $('.pop-message').html('');
    $('.box_Cancel_btn').html('');
    $('.box_Ok_btn').html('');
    $('#pop_dialog').hide();
    $('#pop-warning').hide();

    addscroll();
}


//function Pagination
function PaginateGridPrevMovVeh() {
    //method to paginate through page numbers
    $('#SelectCurrentMovementsVehicle').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        pageNum = $(this).html();
        AjaxPaginationPrevMovVeh(pageNum);
        //console.log($('.active a').html());
    });
    PaginateToLastPagePrevMovVeh();
    PaginateToFirstPagePrevMovVeh();
    PaginateToNextPagePrevMovVeh();
    PaginateToPrevPagePrevMovVeh();
}

//function Ajax call fro pagination
function AjaxPaginationPrevMovVeh(pageNum) {
    var pageSize = $('#SelectCurrentMovementsVehicle #pageSizeSelect').val();

   // var url = '@Url.Action("SORTInbox","SORTApplication")';
    $.ajax({
        url: '../SORTApplication/SORTInbox',
        type: 'GET',
        cache: false,
        //async: false,
        data: { page: pageNum, IsPrevtMovementsVehicle: true },
        beforeSend: function () {
            startAnimation('Loading movement inbox...');
        },
        success: function (result) {
            $('#tab_2').hide();
            $('#SelectCurrentMovementsVehicle').show();

            $('#SelectCurrentMovementsVehicle').html($(result).find('#div_MoveList_advanceSearch').html());
            $('#SelectCurrentMovementsVehicle #pageSizeSelect').val(pageSize);
            $('#SelectCurrentMovementsVehicle').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");

            $('#SelectCurrentMovementsVehicle #pageSizeSelect').change(function () {
                changePageSizePrevMovVeh($(this));
            });
            //  removeHLinksStructMov();
           PaginateGridPrevMovVeh();

        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });

}
//function changePageSizePrevMovVeh(_this) {
//    var pageSize = $(_this).val();
//    $('#SelectCurrentMovementsVehicle #pageSize').val(pageSize);
//    $('#SelectCurrentMovementsVehicle #pageSizeSelect').val(pageSize);
//    SelectPrevtMovementsVehicle(pageNum, pageSize);
//}

//method to paginate to last page
function PaginateToLastPagePrevMovVeh() {
    $('#SelectCurrentMovementsVehicle').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#SelectCurrentMovementsVehicle #TotalPages').val();
        AjaxPaginationPrevMovVeh(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPagePrevMovVeh() {
    $('#SelectCurrentMovementsVehicle').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationPrevMovVeh(1);
    });
}

//method to paginate to Next page
function PaginateToNextPagePrevMovVeh() {
    $('#SelectCurrentMovementsVehicle').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#SelectCurrentMovementsVehicle').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationPrevMovVeh(nextPage);
    });
}

//method to paginate to Previous page
function PaginateToPrevPagePrevMovVeh() {
    $('#SelectCurrentMovementsVehicle').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#SelectCurrentMovementsVehicle').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationPrevMovVeh(prevPage);
    });
}
