var rt_id, rt_name, rt_type;
$(document).ready(function () {
    //localStorage.clear();

    //Vehicle import List pagination
    $('body').off('click', ".planMovementContainer #vehiclepaginator .pagination li a");
    $('body').on('click', ".planMovementContainer #vehiclepaginator .pagination li a", function (e) {
        e.preventDefault();
        var pageNum = getUrlParameterByName("page", this.href);
        AjaxPaginationForVehicleList(pageNum);
    });

    //SORT Inbox List pagination
    $('body').off('click', ".planMovementContainer #sortInboxTotalCount .pagination li a,.planMovementContainer .haulier-pagination-container li a");
    $('body').on('click', ".planMovementContainer #sortInboxTotalCount .pagination li a,.planMovementContainer .haulier-pagination-container li a", function (e) {
        e.preventDefault();
        var pageNum = getUrlParameterByName("page", this.href);
        AjaxPaginationForMovementPM(pageNum);
    });

    //Route List pagination
    $('body').off('click', ".planMovementContainer .routelibrary-pagination .pagination li a");
    $('body').on('click', ".planMovementContainer .routelibrary-pagination .pagination li a", function (e) {
        e.preventDefault();
        var pageNum = getUrlParameterByName("page", this.href);
        AjaxPaginationforSORoute(pageNum);
    });

});


function UseMovement(IsSortUser, BackToPreviousList, isSoVr1ExistingApp = false) {
    var data;
    var url;
    var importFromDiv;
    var div = '#route_importlist_cntr';
    if ($("#IsVehicle").val() == 'true' || isSoVr1ExistingApp)
        div = '#importlist_cntr';
    var filterDiv = '#filters';
    if (IsSortUser == 'True') {
        if (BackToPreviousList == 'false' || BackToPreviousList == 'False' || BackToPreviousList == undefined) {
            clearTextFields();
        }
        if ($("#IsVehicle").val() == 'true')
            url = '../SORTApplication/SetSORTFilter?structID=' + null + '&page =' + 1 + '&planMovement=' + true + '&IsPrevtMovementsVehicle=' + true;
        else
            url = '../SORTApplication/SetSORTFilter?structID=' + null + '&page =' + 1 + '&planMovement=' + true + '&IsPrevtMovementsVehicleRoute=' + true;

        data = $("#FilterMoveInboxSORT").serialize();
        importFromDiv = '#div_MoveList_advanceSearch';
        filterDiv = "#sortFilters";
    }
    else {

        if (BackToPreviousList == 'false' || BackToPreviousList == 'False' || BackToPreviousList == undefined) {
            if (typeof ClearHaulierAdvancedData != 'undefined')
                ClearHaulierAdvancedData(false);
            if (typeof ClearAdvanced != 'undefined') {
                ClearAdvanced();
            }
        }
        var vehicleClass = 0;
        if (isSoVr1ExistingApp)
            vehicleClass = $('#hf_VehicleClassNew').val();

        data = $("#frmFilterMoveInbox").serialize();
        url = '../Movements/SetSearchData?page=' + 1 + '&planMovement=' + true + '&PrevMovImport=' + true + '&isExistVR1SoClass=' + vehicleClass;
        importFromDiv = '.div_so_movement';
        filterDiv = "#movementFilters";
    }

    $.ajax({
        url: url,
        data: data,
        type: 'GET',
        beforeSend: function () {
            openContentLoader('html');
        },
        success: function (page) {
            $('#banner-container').find('div' + filterDiv + '').remove();
            $('div' + filterDiv + '.sort-movement-filter').remove();
            $('div' + filterDiv + '.so-movementlist-filter').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $(div).html($(page).find(importFromDiv).html(), function () {
                event.preventDefault();
            });
            $(div).find("form").removeAttr('action', "");
            $(div).find("form").submit(function (event) {
                event.preventDefault();
            });
            if (isSoVr1ExistingApp)
                $(div).prepend('<span id="list_heading" class="title">Select from movements</span>');
            else if ($("#IsVehicle").val() == 'true')
                $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
            else
                $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
            //if (IsSortUser == 'True') {
            var filters = $(page).find('div' + filterDiv + '');
            $(filters).find('form .filters:first').remove();
            $(filters).find('form .filters:first').remove();
            if (IsSortUser == 'True')
                $(filters).find('form .filters:last').remove();
            $(filters).insertAfter('#banner');
            //}
            //removeHrefLinksMovement();
            //PaginateListMovement();
            MoveInboxAdvancedSearchInit(); // To initiate advanced search dropdown default value
            if (IsSortUser != 'True') {//Haulier
                SOAdvancedSearchInitDatePicker();
            }
            $('.filters').click();//To open advanced search by default
        },
        error: function () {
        },
        complete: function () {
            closeContentLoader('html');
            stopAnimation();
        }
    });
}
function SelectVehicleFromFleet() {
    let page = 1;
    if (SubStepFlag == 2.3) {
        page = sessionStorage.getItem('vehiclePage');
        sessionStorage.removeItem('vehiclePage');
    }
    var selectvehicle = "true";
    var isvso = $('#IsVSO').val();
    if (isvso == "True") {
        selectvehicle = "false"
    }
    $.ajax({
        type: "POST",
        url: '../VehicleConfig/VehicleConfigList',
        data: { importFlag: true, page: page, IsFromMenu: 2,flag:selectvehicle},
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            SetResponse(response);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SelectRouteFromLibrary(clearSearch) {
    var IsFavourite = $('#IsFavouriteRoute').val();
    var routetype = 2;//to get planned routes only
    $.ajax({
        url: '../Routes/RoutePartLibrary',
        data: { importFlag: true, filterFavouritesRoutes: IsFavourite, RouteType: routetype, clearSearch },
        type: 'POST',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('div#filters.route-filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $('#planRouteFilterDiv').remove();

            $('#route_importlist_cntr').html($(response).find('#route-list'), function () {
                event.preventDefault();
            });
            if ($('#IsFavouriteRoute').val() == '0')
                $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from library</span>');
            else
                $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from favourites</span>');
            $('#route-list').find('.text-color2').removeAttr('href').css("cursor", "pointer");

            var filters = $(response).find('div#filters.route-filters');
            $(filters).insertAfter('#banner');

            //removeHrefLinksMovement();
            //PaginateListMovement();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function removeHrefLinksMovement(div = '#route_importlist_cntr') {
    if ($("#IsVehicle").val() == 'true')
        div = '#importlist_cntr';
    $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}

function AjaxPaginationForMovementPM(pageNum) {

    var data;
    var url;
    var importFromDiv;
    var param;
    var order = -1;
    
    var div = '#route_importlist_cntr';
    if ($("#IsVehicle").val() == 'true' || (typeof hf_Vr1SoExistingPopUp != 'undefined' && hf_Vr1SoExistingPopUp ))
        div = '#importlist_cntr';
    if ($('#IsSortUser').val() == 'True') {
        if ($(".ssort:visible").not(".ssort[order ='0']").length != 0) {
            param = $(".ssort:visible").not(".ssort[order ='0']").attr('param');
            order = $(".ssort:visible").not(".ssort[order ='0']").attr('order');
        
            if ($("#IsVehicle").val() == 'true')
                data = { structID: null, page: pageNum, IsPrevtMovementsVehicle: true, planMovement: true, sortType: order, sortOrder: param };
            else
                data = { structID: null, page: pageNum, IsPrevtMovementsVehicleRoute: true, planMovement: true, sortType: order, sortOrder: param };
        }
        else {
            if ($("#IsVehicle").val() == 'true')
                data = { structID: null, page: pageNum, IsPrevtMovementsVehicle: true, planMovement: true };
            else
                data = { structID: null, page: pageNum, IsPrevtMovementsVehicleRoute: true, planMovement: true };
        }
        url = '../SORTApplication/SORTInbox';
        importFromDiv = '#div_MoveList_advanceSearch';
    }
    else {
        var vehicleClass = typeof hf_Vr1SoExistingPopUp != 'undefined' && hf_Vr1SoExistingPopUp ? $('#hf_VehicleClassNew').val() : 0;
        if ($('.esdal-table > thead .sorting_desc').length == 1) {
            param = $('.esdal-table > thead .sorting_desc').attr('data-sortval');
            order = 0;
            data = { page: pageNum, planMovement: true, PrevMovImport: true, sortType: order, sortOrder: param, isExistVR1SoClass: vehicleClass};
        }
        else if ($('.esdal-table > thead .sorting_asc').length == 1) {
            param = $('.esdal-table > thead .sorting_asc ').attr('data-sortval');
            order = 3;
            data = { page: pageNum, planMovement: true, PrevMovImport: true, sortType: order, sortOrder: param, isExistVR1SoClass: vehicleClass };
        }
        else {
            data = { page: pageNum, planMovement: true, PrevMovImport: true, isExistVR1SoClass: vehicleClass};
        }
        url = '../Movements/MovementList';
        importFromDiv = '.div_so_movement';
    }

    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        data: data,//{ page: pageNum, pageSize: pageSize, MovementListForSO: true, showrtveh: ShowPrevMoveSortRoute, IsNotify: true, PrevMovImport: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#banner-container').find('div#filters').remove();
            $('div#filters.sort-movement-filter').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $(div).html($(result).find(importFromDiv).html(), function () {
                event.preventDefault();
            });
            if ($("#IsVehicle").val() == 'true')
                $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
            else
                $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
            var filters = $(result).find('div#filters');
            $(filters).find('form .filters:first').remove();
            $(filters).find('form .filters:first').remove();
            if ($('#IsSortUser').val() == 'True')
                $(filters).find('form .filters:last').remove();
            $(filters).insertAfter('#banner');
            //removeHrefLinksMovement();
            //PaginateListMovement();
            if ($('#IsSortUser').val() != 'True') {
                if (order != -1) {

                }
            }
            else {
                if (order != -1) {
                    $(".ssort[param='" + param + "']").css('display', 'none');
                    $(".ssort[order='" + order + "'][param='" + param + "']").css('display', 'block');// display current sort element
                }
            }
        },
        complete: function () {
            stopAnimation();
        }
    });
}
//function Ajax call for pagination
function AjaxPaginationForVehicleList(pageNum) {
    
    var sortOrder = $('#SortOrderValue').val();
    var sortType = $('#SortTypeValue').val();
    var txt_srch = $('.serchlefttxt').val();

    var searchFavourites = $('#FilterFavouritesVehConfig').is(":checked");
    if (searchFavourites) {
        filterFavourites = 1;
    }
    else {
        filterFavourites = 0;
    }
    $.ajax({
        url: '../VehicleConfig/VehicleConfigList',
        data: {
            page: pageNum, importFlag: true, searchString: txt_srch, filterFavouritesVehConfig: filterFavourites,
            sortOrder: sortOrder, sortType: sortType
        },
        type: 'GET',
        //async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            $('#banner-container').find('div#filters').remove();
            $('div#filters.vehicle-filter').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
                event.preventDefault();
            });
            $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');
            var filters = $(response).find('div#filters');
            //$(filters).appendTo('#banner-container');
            $(filters).insertAfter('#banner');
            //removeHrefLinksMovement();
            //PaginateListMovement();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function AjaxPaginationforSORoute(pageNum) {
    
    var SearchString = $('#SearchString').val();
    var selectedVal = $('#pageSizeVal').val();
    var sortOrder = $('#SortOrderValue').val();
    var sortType = $('#SortTypeValue').val();
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    var searchFavourites = $('#FilterFavouritesRoutes').is(":checked");
    if (searchFavourites) {
        filterFavourites = 1;
    }
    else {
        filterFavourites = 0;
    }
    var favRoute = $('#IsFavouriteRoute').val();
    $.ajax({
        url: '../Routes/RoutePartLibrary',
        type: 'GET',
        cache: false,
        async: false,
        data: {
            SearchString: SearchString, page: pageNum, pageSize: pageSize, importFlag: true, filterFavouritesRoutes: filterFavourites,
            sortOrder: sortOrder, sortType: sortType
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('div#filters.route-filters').remove();
            if ($("#vehicles").length > 0)
                $("#vehicles").css('filter', "unset");
            if ($("#planRouteFilterDiv").length > 0)
                $('#planRouteFilterDiv').remove();

            $('#route_importlist_cntr').html($(response).find('#route-list'), function () {
                event.preventDefault();
            });
            $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from library</span>');
            $('#route-list').find('.text-color2').removeAttr('href').css("cursor", "pointer");

            var filters = $(response).find('div#filters');
            $(filters).insertAfter('#banner');

            //removeHrefLinksMovement();
            //PaginateListMovement();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function clearSearch() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
    $("#FilterFavouritesVehConfig").prop("checked", false);
    SearchVehicle();
}
function SetResponse(response) {

    $('#banner-container').find('div#filters').remove();
    $('div#filters.vehicle-config-list-filter').remove();
    $('div#filters.vehicle-filter').remove();
    document.getElementById("vehicles").style.filter = "unset";
    $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
        event.preventDefault();
    });
    $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');
    var filters = $(response).find('div#filters');
    /*  $(filters).insertAfter('#banner');*/
    $('#banner-container').append(filters);
    $("#select_vehicle_section").show();
    //removeHrefLinksMovement();
    //PaginateListMovement();
}
function LoadApplicationVehicleRoute(MovementVersionId, MovementType, ContentRefNo = 0, ApplicationRevisionId, IsHistoric=0) {

    var cntrId = '#select_route_section';
    BackToPreviousMovementList = true;
    if ($("#IsVehicle").val() == 'true')
        cntrId = '#select_vehicle_section';
    LoadContentForAjaxCalls("POST", '../VehicleConfig/GetPreviousMovementList',
        { versionId: MovementVersionId, cont_Ref_No: ContentRefNo, appRevisionId: ApplicationRevisionId, isVehicleImport: $("#IsVehicle").val(), movementType: MovementType, isHistoric: IsHistoric }, cntrId,'',null,true);
}
function ViewVehicleDetails(VehicleId) {
    var ComponentCntrId = "viewcomponentdetails_" + VehicleId;
    if (document.getElementById(ComponentCntrId).style.display !== "none") {
        document.getElementById(ComponentCntrId).style.display = "none"
        document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "none"
        document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "block"
    }
    else {
        document.getElementById(ComponentCntrId).style.display = "block"
        document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "block"
        document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "none"
    }
}
function Import(OrgId) {
    var modevalue = $('#mode').val();
    var OrganisationId = $('#OrganisationId').val();
    var sortApplication = $('#sortApplication').val();
    var revisionId = $('#revisionId').val();
    var sortStatus = $('#SortStatus').val();
    var showHaulCnt = $('#showHaulCnt').val();
    $.ajax({

        url: '../Organisation/ViewOrganisation',
        type: 'POST',
        data: { mode: modevalue, orgID: OrganisationId, sortApp: sortApplication, RevisionId: revisionId, SORTStatus: sortStatus, showHaulierCount: showHaulCnt },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#viewExistingOrganisation').show();
            $('#viewExistingOrganisation').html(result);
            $('#existingOrganisationList').hide();
            $("#createOrganisation").hide();
            $("#Go_To_Organisations").show();
            $('.organisation-filter').remove();
            $.ajax({

                url: '../Organisation/ViewOrganisationByID',
                type: 'POST',
                async: false,
                data: { orgId: OrgId },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                    var datacollection = result;
                    var dataCollection1;
                    $('#OrgID').val(datacollection.result[0].OrgId);
                    $('#Organisation_ID').val(datacollection.result[0].OrgId);
                    $('#spnOrgName').text(datacollection.result[0].OrgName);
                    $('#Organisation_Name').val(datacollection.result[0].OrgName);
                    // $('#OrgContact').val(datacollection.result.OrgName);
                    $('#spnOrgCode').text(datacollection.result[0].OrgCode);
                    $('#spnLICENSE_NR').text(datacollection.result[0].LicenseNR);

                    $('#EmailIdSORT').val('');
                    $('#FaxSORT').val('');
                    $("#HaulierContactNameExist").val("");

                    $("#hdnApplCountryID").val((datacollection.result[0].CountryId));
                    $.ajax({

                        url: '../Organisation/GetHaulierContactByOrgID',
                        type: 'POST',
                        async: false,
                        data: { orgId: OrgId },
                        success: function (result) {
                            dataCollection1 = result;
                            var len = dataCollection1.result.length;
                            var arrFax = "", arrEmail = "", arrAddress1 = "", arrAddress2 = "", arrAddress3 = "", arrAddress4 = "", arrAddress5 = "", arrpostCode = "", arrCountyId = "", arrPhone = "", arrContactId = "";
                            var SortUserId = "";
                            if (len > 0) {
                                $("#HaulierContactNameExist").val("");

                                $("#ddlHaulierContactName Option").each(function () { $(this).remove(); });
                                $("#ddlHaulierContactName").append("<option value=''>Select Contact Name</option>");
                                for (var i = 0; i < len; i++) {
                                    $("#ddlHaulierContactName").append("<option value='" + dataCollection1.result[i].ContactName + "'>" + dataCollection1.result[i].ContactName + "</option>");
                                    arrFax = arrFax + dataCollection1.result[i].Fax + ',';
                                    arrEmail = arrEmail + dataCollection1.result[i].Email + ',';
                                    arrAddress1 = arrAddress1 + dataCollection1.result[i].AddressLine_1 + ",";
                                    arrAddress2 = arrAddress2 + dataCollection1.result[i].AddressLine_2 + ",";
                                    arrAddress3 = arrAddress3 + dataCollection1.result[i].AddressLine_3 + ",";
                                    arrAddress4 = arrAddress4 + dataCollection1.result[i].AddressLine_4 + ",";
                                    arrAddress5 = arrAddress5 + dataCollection1.result[i].AddressLine_5 + ",";
                                    arrpostCode = arrpostCode + dataCollection1.result[i].PostCode + ",";
                                    arrCountyId = arrCountyId + dataCollection1.result[i].CountryID + ",";
                                    arrPhone = arrPhone + dataCollection1.result[i].Phone + ",";
                                    arrContactId = arrContactId + dataCollection1.result[i].contactId + ",";
                                }

                                $("#hdnArrFax").val(arrFax);
                                $("#hdnArrEmail").val(arrEmail);
                                $("#hdnAddress1").val(arrAddress1);
                                $("#hdnAddress2").val(arrAddress2);
                                $("#hdnAddress3").val(arrAddress3);
                                $("#hdnAddress4").val(arrAddress4);
                                $("#hdnAddress5").val(arrAddress5);
                                $("#hdnpostcode").val(arrpostCode);
                                $("#hdncountryid").val(arrCountyId);
                                $("#hdnCountryID").val(arrCountyId);
                                $("#hdnphonenumber").val(arrPhone);
                                $("#hdncontactId").val(arrContactId);
                                $("#hdncontactId").val(arrContactId);
                            }
                            else {
                                $("#ddlHaulierContactName Option").each(function () { $(this).remove(); });
                                $("#ddlHaulierContactName").append("<option value=''>-- No contact found --</option>");
                                $('#EmailIdSORT').val("");
                                $('#FaxSORT').val("");
                                $("#ddlHaulierContactName").val("0");

                            }

                            ViewExistingOrganisationInit();
                        }
                    });
                },
                error: function (xhr, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                    $('#save_btn').hide();
                    $('#confirm_btn').show();
                    $('#confirm_btn').prop('disabled', false);
                    $('#confirm_btn').removeClass('blur-button');
                }
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });

}
function BackToMainPage() {
    $("#createOrganisation").show();
    $("#Go_To_Organisations").show();
    $('#existingOrganisationList').hide();
    $('#viewExistingOrganisation').hide();
    $('#save_btn').show();
    $('#confirm_btn').hide();
}
function BackToCreateHaulier() {
    $("#createOrganisation").show();
    $("#Go_To_Organisations").show();
    $('#existingOrganisationList').hide();
    $('#viewExistingOrganisation').hide();
    $('#addrchk').text('');
    $('#emailFaxValid1').html('');
    $('#err_Haulier_contact_name').text('');
    $('#err_OrgName_exists').text('');
    $('#orgCodeCodeIsRequiredMsg').text('');
    $('#err_OrgCode_exists').text('');
    $('#ownerValidationMsg').hide();
    $("#codeIsRequiredMsg").hide();
    $("#countryValidationMsg").hide();
    $("#postCodeIsRequiredMsg").hide();
    $("#phoneIsRequiredMsg").hide();
    $('#save_btn').show();
    $('#confirm_btn').hide();
}
function clearRouteSearch() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('');
    $("#FilterFavouritesRoutes").prop("checked", false);
    SearchRoutePartByName();
}
function SearchRoutePartByName() {
    
    var searchString = $('#searchText').val();
    var searchFavourites = $('#FilterFavouritesRoutes').is(":checked");
    var filterFavouritesRoutes = 0;
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
        async: false,
        data: { SearchString: searchString, filterFavouritesRoutes: filterFavouritesRoutes, importFlag: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $('#route_importlist_cntr').html($(response).find('#route-list'), function () {
                event.preventDefault();
            });
            if ($('#IsFavouriteRoute').val() == '0')
                $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from library</span>');
            else
                $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from favourites</span>');
            $('#route-list').find('.text-color2').removeAttr('href').css("cursor", "pointer");

            var filters = $(response).find('div#filters');
            $(filters).appendTo('#banner-container');

            //removeHrefLinksMovement();
            //PaginateListMovement();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function openFilters() {
    var filterWindowWidth = "";
    if ($('#ImportFrom').val() == 'prevMov' && ($('#IsSortUser').val() != 'True')) {
        filterWindowWidth = "660px";
    }
    else if ($('#ImportFrom').val() == 'prevMov' && ($('#IsSortUser').val() == 'True')) {
        filterWindowWidth = "630px";
    }
    else if ($('#ImportFrom').val() == 'fleet') {
        filterWindowWidth = "350px";
    }
    else {
        filterWindowWidth = "350px";
    }
    document.getElementById("filters").style.width = filterWindowWidth;
    document.getElementById("vehicles").style.filter = "brightness(0.5)";
    document.getElementById("vehicles").style.background = "white";

    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("vehicles").style.filter = "unset";
}
function CreateNewConfiguration() {
    $.ajax({
        url: '../VehicleConfig/CreateVehicle',
        type: 'GET',
        cache: false,
        async: false,
        data: { isMovement: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#haulier_details_section').hide();
            $('#select_vehicle_section').hide();
            $('#vehicle_details_section').hide();
            $('#movement_type_confirmation').hide();
            $('#select_route_section').hide();
            $('#route').html('');
            $('#route').hide();
            $('#route_vehicle_assign_section').hide();
            $('#supplimentary_info_section').hide();
            $('#overview_info_section').hide();
            $('#vehicle_edit_section').hide();
            $('#vehicle_Component_edit_section').hide();

            SubStepFlag = 1.3;
            var result = $(response).find('#vehicles');
            $('#vehicle_Create_section').show();
            $('#vehicle_Create_section').html($(response).find('#vehicles'));
            $('.createConfig').unwrap('#banner-container');
            $('.createConfig').attr("style", "padding-left:0px !important");
            $('#confirm_btn').hide();
            $("#back_btn").prop('disabled', false);
            var scollIcon = $(response).find('div#scroll-btns');
            $(scollIcon).insertAfter('.new-vehicle');
            VehicleConfigCreateVehicleInit();
            },
        complete: function () {
            stopAnimation();
        }
    });
}
/**************** Vehicle select page related sctripts ********************/
function ImportVehicle(source, backtopreviouslist, isSoVr1ExistingApp = false) {
   
    LoadContentForAjaxCalls("POST", '../Movements/MovementSelectVehicleByImport', { importFrm: source, backToPreviousList: backtopreviouslist }, '#select_vehicle_section', '', function () {
        MovementSelectVehicleByImportInit(isSoVr1ExistingApp);
    });
}
function ChooseFromSimilarCombinations() {
    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicleConfiguration', {}, '#select_vehicle_section', '', function () {
        MovementSelectVehicleCombInit();
    });
}
$('body').on('click', '#use-vehicle', function (e) {
    e.preventDefault();
    var VehicleId = $(this).data('vehicleid');
    var flag = $(this).data('flag');
    UseVehicleForMovement(VehicleId, flag);
});
function UseVehicleForMovement(vehicleId, flag) {//flag => 1 - applicationvehicle, 2 - fleet, 3 - route vehicle
    var count = CheckVehicleIsValid(vehicleId, flag);
    if (count == 0) {
        LoadContentForAjaxCalls("POST", '../VehicleConfig/InsertMovementVehicle', { movementId: $('#MovementId').val() || 0, vehicleId: vehicleId, flag: flag }, '#vehicle_details_section', '', function () {
            MovementSelectedVehiclesInit();
            VehicleDetailsInit();
            ViewConfigurationGeneralInit();
            GeneralVehicCompInit();
            MovementAssessDetailsInit();
        });
    }
    else if (count==2) {
        ShowErrorPopup("The vehicle you have selected to import is not compatible with ESDAL4. Please import a different vehicle or create a new vehicle.");
    }
    else {
        ShowErrorPopup("Components missing. Choose a valid vehicle");
    }
}
function ViewConfigurationLoad(VehicleId) {
    var thisPage = $('#importlist_cntr').find('.active').find('a').html();

    $.ajax({
        url: '../VehicleConfig/ViewConfiguration',
        data: { vehicleId: VehicleId, importFlag: true },
        type: 'POST',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            SubStepFlag = 2.3;
            sessionStorage.removeItem('vehiclePage');
            sessionStorage.setItem('vehiclePage', thisPage);
            $('#importlist_cntr').html($(response).find('#vehicles'), function () {
                event.preventDefault();
            });

            $('#btn_back_to_config').hide();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
/************ Vehicle select page related sctripts ***********************/

/*********** SORT Movement Inbox filter and import functions *************/
function SearchSORTData() {

    $.ajax({
        url: '/SORTApplication/SetSORTFilter',
        type: 'POST',
        cache: false,
        async: false,
        data: $("#FilterMoveInboxSORT").serialize(),
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#div_MoveList_advanceSearch').html('');
            $('#div_MoveList_advanceSearch').html($(response).find('#div_MoveList_advanceSearch').html());
            closeFilters();
        },
        error: function (xhr, status) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}
var PrePrjId = 0;//global declaration for back button 
var PreMovType = "";
function SelectPrevitMovementsVehicle(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid, Vtype) {
    BackToPreviousMovementList = true;
    LoadContentForAjaxCalls("POST", '../SORTApplication/GetPreviousMovemntList', { projectId: Vprojectid, isVehicleImport: $("#IsVehicle").val(), movmntType: Vtype }, '#select_vehicle_section');
}
$('body').on('click', '#mivBtnVehicleList', function (e) {
    e.preventDefault();
    VehicleListToImportFromList(this);
});
$('body').on('click', '#mivBtnAVehicle', function (e) {
    e.preventDefault();
    var ReviosionId = $(this).data('reviosionid');
    var var1 = $(this).data('var1');
    var var2 = $(this).data('var2');
    VehicleListToImport(ReviosionId, var1, var2);
});
$('body').on('click', '.mivBtnVehicle', function (e) {
    e.preventDefault();
    var VersionID = $(this).data('versionid');
    var var1 = $(this).data('var1');
    var var2 = $(this).data('var2');
    VehicleListToImport(VersionID, var1, var2);
});
function VehicleListToImportFromList(e) {
    var RevisionID = $(e).attr('data-revisionid');
    var appFlag = $(e).attr('data-appflag');
    var flag = $(e).attr('data-flag');
    VehicleListToImport(RevisionID, appFlag, flag);
}
function VehicleListToImport(RevisionId, ListType, Flag) {
    SubStepFlag = 1.4;
    LoadContentForAjaxCalls("POST", '../SORTApplication/GetPreviousMovementVehicleList', { revisionId: RevisionId, listType: ListType, flag: Flag }, '#vehicle_import_section','',null,true);
}
$('body').on('click', '#mivBtnRouteList', function (e) {
    e.preventDefault();
    var RevisionID = $(this).data('revisionid');
    var appFlag = $(this).data('appflag');
    RouteListToImport(RevisionID, appFlag);
});
$('body').on('click', '#mivBtnARoute', function (e) {
    e.preventDefault();
    var ReviosionId = $(this).data('reviosionid');
    var var1 = $(this).data('var1');
    RouteListToImport(ReviosionId, var1);
});
$('body').on('click', '.mivBtnRoute', function (e) {
    e.preventDefault();
    var VersionID = $(this).data('versionid');
    var var1 = $(this).data('var1');
    RouteListToImport(VersionID, var1);
});
function RouteListToImport(RevisionId, ListType) {
    $('#back_btn').hide();
    $('#back_btn_Rt_prv').show();
    LoadContentForAjaxCalls("POST", '../SORTApplication/GetPreviousMovementRouteList', { revisionId: RevisionId, listType: ListType }, '#select_route_section');
}
function SelectCurrentMovementsRoute(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid, Vtype) {

    BackToPreviousRouteMovementList = true;
    PrePrjId = Vprojectid;//setting details for back button load
    PreMovType = Vtype;
    LoadContentForAjaxCalls("POST", '../SORTApplication/GetPreviousMovemntList', { projectId: Vprojectid, isVehicleImport: $("#IsVehicle").val(), movmntType: Vtype }, '#select_route_section');
}
function EditSortRoute(RouteId, RouteName) {
    $('#RoutePart').hide(); //hide routedetails
    //$("#select_route_section").html('');
    $("#select_route_section").hide();
    var url = '../Routes/RouteUpdateFlagSessionClear';
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
    //function for resize map
    var ApplicationRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
    var page = '@Session["pageflag"]';
    $("#WichPage").val(page);
    $('#route').show();
    $('#route').html('');
    $('#back_btn_Rt').show();
    $('#confirm_btn').hide();

    $.ajax({
        url: '../Routes/LibraryRoutePartDetails',
        data: { RouteFlag: page, ApplicationRevId: ApplicationRevId, plannedRouteName: RouteName, plannedRouteId: RouteId, PageFlag: "U", ShowReturnLeg: 0 },
        type: 'GET',
        success: function (page) {
            SubStepFlag = 4.2;
            $('#route').html('');
            $('#route').append($(page).find('#CreateRoute').html());
            LibraryRoutePartDetailsInit();
            CheckSessionTimeOut();
            Map_size_fit();//, function () {
            addscroll();
        }
    });
}
function viewSORTAdvHaulier() {
    if (document.getElementById('viewSORTAdvHaulier').style.display !== "none") {
        document.getElementById('viewSORTAdvHaulier').style.display = "none"
        document.getElementById('chevlon-up-icon2').style.display = "none"
        document.getElementById('chevlon-down-icon2').style.display = "block"
    }
    else {
        document.getElementById('viewSORTAdvHaulier').style.display = "block"
        document.getElementById('chevlon-up-icon2').style.display = "block"
        document.getElementById('chevlon-down-icon2').style.display = "none"
    }
}
function togglecheckbox(_this) {

    if (_this.is(':checked')) {
        _this.closest('.row').find('input:text').attr('disabled', false);
    }
    else {
        _this.closest('.row').find('input:text').attr('disabled', true);
        _this.closest('.row').find('input:text').val("");
    }
}
function EnableDisableDatePicker() {
    $.each($("#viewmovements input:checkbox"), function () {
        togglecheckbox($(this));
    });
}
function ClearSORTData() {
    var _advFilter = $('#filter_SORT');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:checkbox').prop('checked', false);
    EnableDisableDatePicker();
    ResetDataSORT();
    //location.reload();
}
function ResetDataSORT() {
    $.ajax({
        url: '../SORTApplication/ClearInboxAdvancedFilterSORT',
        type: 'POST',
        success: function (data) {
            //ClearAdvancedData();
        }
    });
}
/************ SORT Movement Inbox filter and import functions ***********/
function EditSupplementaryInfo() {
    LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
        ApplicationSupplimentaryInfoInit();
    });
}
function CancelEditingSupplInfo() {
    LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
        ViewSupplementaryInit();
    });
}
function IsApplyToApplication() {
    let Msg = "Do you want to submit application?";
    if ($('#HaulierReference').val() != '')
        Msg = "Do you want to submit application \"" + $('#HaulierReference').val() + '\" ?';

    ShowWarningPopup(Msg, "SubmitApplication");
}
function showRouteDetails(routeID, routeType, routeName, flag) {
    if (routeType == "outline") {
        $.ajax({
            type: 'POST',
            dataType: 'json',

            url: '../Routes/GetPlannedRoute',
            data: { RouteID: routeID, IsPlanMovement: $("#hf_IsPlanMovmentGlobal").length > 0, IsCandidateView: IsCandidateRouteView() },


            beforeSend: function (xhr) {
                startAnimation();
            },
            success: function (result) {
                stopAnimation();
                for (var x = 0; x < result.result.routePathList.length; x++) {
                    result.result.routePathList[x].routePointList.sort((a, b) => a.routePointId - b.routePointId);
                }
                routePath = result.result;
                if (routePresentable(routePath)) {
                    window.location = '../Routes/LibraryRoutePartDetails' + EncodedQueryString('plannedRouteId=' + routeID + '&routeType=' + routeType + '&plannedRouteName=' + routeName + '&PageFlag=' + "U");
                    CheckSessionTimeOut();
                }
                else {
                    var IsTextualRoute = false;
                    var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                    for (var i = 0; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointType != 0 || result.result.routePathList[0].routePointList[i].pointType != 1) {
                            if (result.result.routePathList[0].routePointList[i].pointDescr == "")
                                IsTextualRoute = true;
                        }
                    }
                    $("#dialogue").load('../Routes/LibraryRoutePartDetails?plannedRouteId=' + routeID + '&routeType=' + routeType + '&IsTextualRouteType=' + IsTextualRoute, function () {
                        removescroll();
                        $("#dialogue").show();
                        $("#overlay").show();
                        LibraryRoutePartDetailsInit();
                        CheckSessionTimeOut();
                    });
                    $("#dialogue").show();
                    $("#overlay").show();
                }
            },
        });
    }
    else {
        var routeType = "PLANNED";
        rt_id = routeID;
        rt_name = routeName;
        rt_type = routeType;
        CheckIsBroken({ LibraryRouteId: routeID }, function (response) {
            ShowBrokenRouteMessage(response, rt_id, rt_name, rt_type);
        });
    }
}
function ShowBrokenRouteMessage(response, routeID, routeName, routeType) {
    var url = '../Routes/LibraryRoutePartDetails' + EncodedQueryString('plannedRouteId=' + routeID + '&routeType=' + routeType + '&plannedRouteName=' + routeName + '&PageFlag=' + "U");;
    if (response && response.Result && response.Result.length > 0 && response.Result[0].IsBroken > 0) {//isBroken[0].isBroken > 0  //check in the existing route is broken   Extra condition added for handling to ESDAL4 once the Mapupgarde service activated then the condition can be moved
        var msg = (response.Result[0].IsReplan > 1) ? BROKEN_ROUTE_MESSAGES.LIBRARY_ROUTEPART_DETAILS_IS_REPLAN_NOT_POSSIBLE : BROKEN_ROUTE_MESSAGES.LIBRARY_ROUTEPART_DETAILS_IS_REPLAN_POSSIBLE;////=1 replan possible
        ShowWarningPopupMapupgarde(msg, function () {
            $('#WarningPopup').modal('hide');
            RedirectToLibraryRouteUrl(url)
        });
    }
    else {
        RedirectToLibraryRouteUrl(url)
    }
}
function RedirectToLibraryRouteUrl(url) {
    window.location = url;
    CheckSessionTimeOut();
}
function clearConfigSearch() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
    $("#FilterFavouritesVehConfig").prop("checked", false);
    SearchVehicle();
}
function SearchVehicle() {
    var searchString = $('#searchText').val();
    var vehicleIntend = $('#Indend').val();
    var vehicleType = $('#VehType').val();
    var searchFavourites = $('#FilterFavouritesVehConfig').is(":checked");
    var filterFavourites = 0;
    if (searchFavourites) {
        filterFavourites = 1;
    }
    else {
        filterFavourites = 0;
    }
    closeFilters();
    $.ajax({
        url: '../VehicleConfig/SaveVehicleConfigSearch',
        type: 'POST',
        cache: false,
        beforeSend: function () {
            startAnimation();
        },
        data: {
            searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, importFlag: true, filterFavouritesVehConfig: filterFavourites
        },
        success: function (response) {

            stopAnimation();
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
                event.preventDefault();
            });
            $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');
            var filters = $(response).find('div#filters');
            $(filters).appendTo('#banner-container');
            $("#select_vehicle_section").show();
            //removeHrefLinksMovement();
            //PaginateListMovement();

        }
    });
}
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
function clearTextFields() {
    var _advFilter = $('#filter_SORT');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:checkbox').prop('checked', false);
}
function ClearAdvancedData() {

    var _advFilter = $('#div_so_advanced_search');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:radio').eq(0).prop('checked', true);
    _advFilter.find('input:checkbox').attr('checked', false);

    _advFilter.find('input:checkbox').closest('tr').find('input:text').attr('disabled', 'disabled');
}
function ViewHaulierDetails() {
    var revisionId = $('#AppRevisionId').val();
    $.ajax({
        url: '../SORTApplication/HaulierDetails',
        type: 'POST',
        async: false,
        data: {
            isBackCall: true, SORTStatus: 'CreateSO', mode: 'SORTSO', OrgID: 0, revisionId: revisionId
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            $('#haulier_details_section').hide();
            $('#select_vehicle_section').hide();
            $('#vehicle_details_section').hide();
            $('#movement_type_confirmation').hide();
            $('#select_route_section').hide();
            $('#route').html('');
            $('#route').hide();
            $('#route_vehicle_assign_section').hide();
            $('#supplimentary_info_section').hide();
            $('#overview_info_section').hide();
            $('#vehicle_edit_section').html('');
            $('#vehicle_Component_edit_section').html('');
            $('#vehicle_Create_section').html('');
            $('#vehicle_edit_section').hide();
            $('#vehicle_Component_edit_section').hide();
            $('#vehicle_Create_section').hide();

            $('#haulier_details_section').show();
            $('#haulier_details_section').html(response);


            $('#sortApplication').val('SORTSO');
            var modevalue = $('#mode').val();

            var sortApplication = $('#sortApplication').val();

            var sortStatus = $('#SortStatus').val();
            $('#OrganisationId').val(0);
            var OrganisationId = $('#OrganisationId').val();

            $.ajax({

                url: '../Organisation/ViewOrganisation',
                type: 'POST',
                data: {
                    mode: modevalue, organisationId: OrganisationId, sortApplication: sortApplication, revisionId: revisionId, sortStatus: sortStatus
                },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                    $("#haulier_details_section").find('#existingOrganisationList').hide();
                    $("#haulier_details_section").find('#createOrganisation').hide();
                    $("#createOrganisation").html('');
                    $('#viewExistingOrganisation').show();
                    $('#viewExistingOrganisation').html('');
                    $('#viewExistingOrganisation').html(result);

                    $("#Go_To_Organisations").hide();
                    $.ajax({

                        url: '../Organisation/ViewOrganisationByID',
                        type: 'POST',
                        async: false,
                        data: { orgId: OrganisationId, RevisionId: revisionId },
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (result) {
                            var dataCollection = result;
                            if (dataCollection.result.length > 0) {
                                var VIsNENsReceive = true;
                                if (dataCollection.result[0].IsNENsReceive != VIsNENsReceive)
                                    VIsNENsReceive = false;
                                $("#HdnOrgIDSORT").val(dataCollection.result[0].OrgId);
                                $("#HdnOrgNameSORT").val(dataCollection.result[0].OrgName);
                                //$("#hdnOrgType").val(dataCollection.result[0].OrgType)
                                $("#spnOrgCode").html(dataCollection.result[0].OrgCode)

                                $("#spnLICENSE_NR").html(dataCollection.result[0].LicenseNR);
                                $("#spnIsReceiveNEN").html(VIsNENsReceive);
                                $("#spnAddressLine_1").html(dataCollection.result[0].AddressLine1);
                                $("#spnAddressLine_2").html(dataCollection.result[0].AddressLine2);
                                $("#spnAddressLine_3").html(dataCollection.result[0].AddressLine3);
                                $("#spnAddressLine_4").html(dataCollection.result[0].AddressLine4);
                                $("#spnAddressLine_5").html(dataCollection.result[0].AddressLine5);
                                $("#spnPostCode").html(dataCollection.result[0].PostCode);

                                $('#OrgID').val(dataCollection.result[0].OrgId);
                                $('#Organisation_ID').val(dataCollection.result[0].OrgId);
                                var country = $("#CountryID option[value=" + dataCollection.result[0].CountryId + "]").text();
                                $("#hdnCountryID").val(dataCollection.result[0].CountryId);
                                $("#spnCountryID").html(country);
                                // $("#hdnPhone").val(dataCollection.result[0].Phone)
                                $("#spnPhone").html(dataCollection.result[0].Phone);
                                /*$("#hdnWeb").val(dataCollection.result[0].Web)*/
                                $("#HdnHaulierContactName").val(dataCollection.result[0].HAContact)
                                $("#HdnOrgEmailId").val(dataCollection.result[0].EmailId)
                                $("#HdnOrgFax").val(dataCollection.result[0].Fax)



                                $("#spnOrgName").html($("#HdnOrgNameSORT").val());
                                $("#spnOrgCode").html($("#hdnOrgCode").val());



                                $("#spnHaulierContactName").html($("#HdnHaulierContactName").val());
                                $("#spnEmail").html($("#HdnOrgEmailId").val());
                                $("#spnFax").html($("#HdnOrgFax").val());
                                $('#btn_SelectHaul').hide();
                                $("#Organisation_Name").val(dataCollection.result[0].OrgName);
                                //$("#SOHndContactID").val();
                                //$("#hdnCONTACTID").val($("#SOHndContactID").val());
                            }


                            var selectedOrgType = $('#OrgType option:selected').val();
                            if (selectedOrgType == 237013 || selectedOrgType == 237014 || selectedOrgType == "") {
                                $('#divReceiveNENs').hide();
                            }
                            else {
                                $('#divReceiveNENs').show();
                            }


                        },
                        error: function (xhr, textStatus, errorThrown) {
                            location.reload();
                        },
                        complete: function () {
                            stopAnimation();
                            $('#save_btn').hide();
                            $('#confirm_btn').show();
                            $('#apply_btn').hide();
                        }
                    });
                },
                error: function (xhr, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
            $('#save_btn').hide();
            $('#confirm_btn').show();
            $('#apply_btn').hide();
        }
    });

}
function openSortFilters() {
    document.getElementById("navbar").style.background = "white";
    $("#sortFilters").css('width', "630px");
    $("#banner").css('filter', "brightness(0.5)");
    $("#banner").css('background', "white");
    $("#navbar").css('filter', "brightness(0.5)");
    $("#navbar").css('background', "white");

    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeSortFilters() {
    $('#sortFilters').css('width', "0");
    $("#banner").css('filter', "unset");
    $("#navbar").css('filter', "unset");
}
function ValidateUnSavedChange(pageName) {
    var isUnsavedChange = false;
    switch (pageName) {
        case "map":
            if (IsRoutePlanned == true) {
                isUnsavedChange = true;
            }
            break;
        default:
            break;
    }
    return isUnsavedChange;
}


