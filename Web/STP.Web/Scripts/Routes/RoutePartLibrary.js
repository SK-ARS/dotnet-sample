var rt_id, rt_name, rt_type;
var sortTypeGlobal = 1;//1-desc
var sortOrderGlobal = 1;//routename
function clearRouteSearch() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('');

    $("#FilterFavouritesRoutes").prop("checked", false);
    SearchRoutePartByName(false);

    $('#filterimage').hide();
    $('#filterimages').hide();
    closeFilters();
}
function SearchRoutePartByName(isSort = true) {
    var searchString = $('#searchText').val();
    var searchFavourites = $('#FilterFavouritesRoutes').is(":checked");
    var filterFavouritesRoutes = 0;
    var searchString = $('#searchText').val();
    var sd = $('#searchText').attr('placeholder');
    var planMove = $('#hf_IsPlanMovmentGlobal').val();
    if (planMove == 'True' || planMove == 'true') {
        var importFlag = true;
    }
    if (searchFavourites) {
        filterFavouritesRoutes = 1;
    }
    else {
        filterFavouritesRoutes = 0;
    }
    closeFilters();
    $.ajax({
        url: '../Routes/SaveRouteSearch',
        type: 'POST',
        cache: false,
        //async: false,
        data: {
            SearchString: searchString, filterFavouritesRoutes: filterFavouritesRoutes, importFlag: importFlag,
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(),
            page: isSort ? $('#pageNum').val() : "1", pageSize: $('#pageSizeVal').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            stopAnimation();
            //This is for route import in plan movement to avoid pagination redirection issue
            if ($("#ImportFrom").length > 0) {//this will call on sorting during import
                var result = $(response).find('#routelibrary').html();
                $('#routelibrary').html(result);
                removeHrefLinksMovement('#route_importlist_cntr');
                PaginateListMovement('#route_importlist_cntr');
            } else {//normal listing page
                var result = $(response).find('section#banner').html();
                $('section#banner').html(result);
                //$('.ImportRouteInAppLibraryCls').hide();

            }
        },
        error: function (response) {
        }
    });
}
function Delete(id, un) {
    startAnimation();
    Id = id; UN = un;
    var isAccessible = true;
    if (isAccessible == 'False') {
        ShowErrorPopup("You are not authorized to disable it");
    }
    else {
        var RouteName = "Do you want to delete '" + "" + "'" + un + "'" + "" + "' ?";
        ShowWarningPopup(RouteName, "DeleteRouteAjax");
    }
    stopAnimation();
}
function DeleteRouteAjax() {
    CloseWarningPopup();
    $.ajax({
        url: '../Routes/DeleteRoute',
        type: 'POST',
        cache: false,
        data: { PlannedRouteId: Id },
        beforeSend: function () {
            startAnimation();
        },
        success: function (Success) {
            if (Success) {
                stopAnimation();
                ShowModalPopup("Planned route " + "'" + UN + "'" + " " + "deleted successfully");
            }
            else {
                stopAnimation();
                ShowErrorPopup("Error on the page");
                ReloadLocation();
            }
        }
    });
}

jQuery(document).ready(function ($) {
    $(document).on('click', '.bs-canvas-overlay', function () {
        $('.bs-canvas-overlay').remove();
        $("#filters").css('margin-right', "-400px");
        return false;
    });
    $('input[type="checkbox"]').change(function (event) {
        event.preventDefault();
        $('.bs-canvas-overlay').remove();
    });
});


function viewRouteDiscription(i) {
    if (document.getElementById('description_' + i).style.display !== "none") {
        document.getElementById('description_' + i).style.display = "none"
        document.getElementById('chevlon-up-icon_' + i).style.display = "none"
        document.getElementById('chevlon-down-icon_' + i).style.display = "revert"
    }
    else {
        document.getElementById('description_' + i).style.display = "revert"
        document.getElementById('chevlon-up-icon_' + i).style.display = "revert"
        document.getElementById('chevlon-down-icon_' + i).style.display = "none"
    }
}
function ShowBrokenRouteMessageRouteDetails(response, routeID, routeType, routeName) {
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) {//isBroken[0].isBroken > 0  //check in the existing route is broken   Extra condition added for handling to ESDAL4 once the Mapupgarde service activated then the condition can be moved
        var msg = response.Result[0].IsReplan < 2 ? BROKEN_ROUTE_MESSAGES.ROUTE_DETAILS_MESSAGE_IS_REPLAN_POSSIBLE : BROKEN_ROUTE_MESSAGES.ROUTE_DETAILS_MESSAGE_IS_REPLAN_NOT_POSSIBLE;
        ShowWarningPopupMapupgarde(msg, function () {
            $('#WarningPopup').modal('hide');
            callLibRoutes(routeID, routeType, routeName);
        });
    }
    else {
        callLibRoutes(routeID, routeType, routeName);
    }
}
function showRouteDetails(routeID, routeType, routeName, flag) {
    rt_id = routeID;
    rt_name = routeName;
    rt_type = routeType;
    if (routeType == "outline") {
        showNotification("No visual representation is available for this route");
    }
    else {
        var routeType = "PLANNED";
        CheckIsBroken({ LibraryRouteId: routeID }, function (response) {
            ShowBrokenRouteMessageRouteDetails(response, rt_id, rt_type, rt_name);
        });
    }
}
function callLibRoutes(routeID, routeType, routeName) {
    window.location = '../Routes/LibraryRoutePartDetails' + EncodedQueryString('plannedRouteId=' + routeID + '&routeType=' + routeType + '&plannedRouteName=' + routeName + '&PageFlag=' + "U");
    CheckSessionTimeOut();
}
function routePresentable(routePath) {
    if (String(routePath.result) == "undefined" && routePath.routePathList[0].routePointList[0].pointGeom == null) {
        return false;
    }
    else if (routePath.result != "undefined" && routePath.result != null) {
        if (routePath.result.routePathList[0].routePointList[0].pointGeom == null)
            return false;
    }
    return true;
}
function RouteLibrarySort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    var page = $('#pageNum').val();
    SearchRoutePartByName(isSort = true);
}
function Importrouteinnotif(LibraryrouteId, routename) {
    var P_CONTENT_REF = $('#CRNo').val() ? $('#CRNo').val() : 0;
    $.ajax({
        url: '../Application/SaveRouteInRouteParts',
        type: 'POST',
        async: false,
        cache: false,
        data: { routepartId: LibraryrouteId, routetype: 'planned', AppRevId: 0, CONTENT_REF: P_CONTENT_REF },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            var msg = '"' + routename + '" ' + "route imported for this application.";
            showWarningPopDialog(msg, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            CloneRoutes();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
    $('#mapTitle').html('');
}

$('body').on('change', '#pageSizeSelectRouteLibrary', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchRoutePartByName(isSort = true);
});


$(document).ready(function () {
    if ($('#hf_IsPlanMovmentGlobal').length == 0)
        SelectMenu(4);
    $('body').on('click', '#SearchRoutePartByName', function (e) {
        SearchRoutePartByName(false);
    });
    $('body').on('click', '#IDclearRouteSearch', function (e) {
        clearRouteSearch();
    });
   
    $('body').on('click', '#IDclearRouteSearch', function (e) {
        clearRouteSearch();
    });
    $('body').on('click', '.route-part-library-container #filterimage, .route-table #filterimage', function (e) {
        clearRouteSearch();
    });
    $('body').on('click', '.ViewRoutescls', function (e) {
        var r1 = $(this).attr("datarouteid1");
        var r3 = $(this).attr("datartname1");
        ViewRoutes(r1, r3);
    });
    $('body').on('click', '.showRouteDetailscls', function (e) {
        var r1 = $(this).attr("datarouteid3");
        var r2 = $(this).attr("dataroutetype3");
        var r3 = $(this).attr("datartname3");
        showRouteDetails(r1, r2, r3);
    });
    $('body').on('click', '.viewRouteDiscriptionCls', function (e) {
        var r1 = $(this).attr("datai");
        viewRouteDiscription(r1);
    });
    $('body').on('click', '.ImportRouteInAppLibraryCls', function (e) {
        var r1 = $(this).attr("datarouteidim");
        var r2 = $(this).attr("dataroutetypeim");
        ImportRouteInAppLibrary(r1, r2);
    });
    $('body').on('click', '.Importrouteinnotifcls', function (e) {
        var r1 = $(this).attr("datarouteidlkm");
        var r2 = $(this).attr("datarouterneim");
        Importrouteinnotif(r1, r2);
    });
    $('body').on('click', '.DLTECSL', function (e) {
        var r1 = $(this).attr("datarouteiddm");
        var r2 = $(this).attr("datarouterneims");
        Delete(r1, r2);
    });
    $('body').on('click', '.disabled_star1', function (e) {
        var r1 = $(this).attr("datartid");
        ManageFavourites(r1, 3, 0);
    });
    $('body').on('click', '.enabled_star1', function (e) {
        var r1 = $(this).attr("datartid2");
        ManageFavourites(r1, 3, 1);
    });
    $('body').on('click', '.enabled_star2', function (e) {
        var r1 = $(this).attr("datartid3");
        ManageFavourites(r1, 1, 1);
    });
    $('body').on('click', '.disabled_star2', function (e) {
        var r1 = $(this).attr("datartid4");
        ManageFavourites(r1, 1, 0);
    });
    $('body').on('click', '#btnCreateRT', function (e) {
        var flag = "true";
        location.href = '../Routes/LibraryRoutePartDetails?plannedRouteId=C&&flag='+flag;
        
    });
    $('body').on('change', '.route-part-library-container #pageSizeSelect', function () {
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        SearchRoutePartByName(isSort = true);
    });
    $('body').off('click', ".route-part-library-container .routelibrary-pagination .pagination li a");
    $('body').on('click', ".route-part-library-container .routelibrary-pagination .pagination li a", function (e) {
        e.preventDefault();
        var pageNum = getUrlParameterByName("page", this.href);
        $('#pageNum').val(pageNum);
        SearchRoutePartByName(isSort = true);
    });
});