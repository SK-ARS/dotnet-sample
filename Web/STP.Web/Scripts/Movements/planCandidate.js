function UseMovement() {  
    var div = '#route_importlist_cntr';
    if ($("#IsVehicle").val() == 'true')
        div = '#importlist_cntr';
   
    var data = { planMovement: true };
    
    $.ajax({
        url: '../Movements/MovementList',
        data: data,
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $(div).html($(page).find('.div_so_movement').html(), function () {
                event.preventDefault();
            });
            $(div).find("form").removeAttr('action', "");
            $(div).find("form").submit(function (event) {
                event.preventDefault();
            });
            if ($("#IsVehicle").val() == 'true')
                $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
            else
                $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
            var filters = $(page).find('div#filters');
            $(filters).find('form .filters:first').remove();
            $(filters).find('form .filters:first').remove();
            $(filters).appendTo('#banner-container');
            removeHrefLinks();
            PaginateList();
            fillPageSizeSelect();
        },
        error: function () {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SelectVehicleFromFleet() {
    ;
    $.ajax({
        type: "POST",
        url: '../VehicleConfig/VehicleConfigList',
        data: { importFlag: true, IsFromMenu:2 },
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

function SelectRouteFromLibrary() {
    var IsFavourite = $('#IsFavouriteRoute').val();
    var routetype = 2;//to get planned routes only
    $.ajax({
        url: '../Routes/RoutePartLibrary',
        data: { importFlag: true, filterFavouritesRoutes: IsFavourite, RouteType: routetype },
        type: 'POST',
        async: false,
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

            removeHrefLinks();
            PaginateList();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function removeHrefLinks() {
    var div = '#route_importlist_cntr';
    if ($("#IsVehicle").val())
        div = '#importlist_cntr';
    $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}

function PaginateList() {
    
    var cntrId = '#route_importlist_cntr';
    if ($("#IsVehicle").val() == 'true')
        cntrId = '#importlist_cntr';
    $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        var pageNum = $(this).html();
        if ($("#IsVehicle").val() == 'true' && $("#ImportFrom").val() == 'fleet') {
            AjaxPaginationForVehicleList(pageNum);
        }
        else if ($("#IsVehicle").val() == 'false' && $("#ImportFrom").val() == 'library') {
            AjaxPaginationforSORoute(pageNum);
        }
        else if ($("#ImportFrom").val() == 'prevMov') {
            AjaxPaginationForMovement(pageNum);
        }
        else {
            AjaxPaginationForComponentList(pageNum);
        }
    });
    PaginateToLastPagesomovement(cntrId);
    PaginateToFirstPagesomovement(cntrId);
    PaginateToNextPagesomovement(cntrId);
    PaginateToPrevPagesomovement(cntrId);
}
//method to paginate to last page
function PaginateToLastPagesomovement(ContainerId) {
    $(ContainerId).find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        if ($("#IsVehicle").val() == 'true' && $("#ImportFrom").val() == 'fleet') {
            AjaxPaginationForVehicleList(pageCount);
        }
        else if ($("#IsVehicle").val() == 'false' && $("#ImportFrom").val() == 'library') {
            AjaxPaginationforSORoute(pageCount);
        }
        else if ($("#ImportFrom").val() == 'prevMov') {
            AjaxPaginationForMovement(pageCount);
        }
        else {
            AjaxPaginationForComponentList(pageCount);
        }
    });
}

//method to paginate to first page
function PaginateToFirstPagesomovement(ContainerId) {
    $(ContainerId).find('.PagedList-skipToFirst').click(function () {
        if ($("#IsVehicle").val() == 'true' && $("#ImportFrom").val() == 'fleet') {
            AjaxPaginationForVehicleList(1);
        }
        else if ($("#IsVehicle").val() == 'false' && $("#ImportFrom").val() == 'library') {
            AjaxPaginationforSORoute(1);
        }
        else if ($("#ImportFrom").val() == 'prevMov') {
            AjaxPaginationForMovement(1);
        }
        else {
            AjaxPaginationForComponentList(1);
        }
    });
}

//method to paginate to Next page
function PaginateToNextPagesomovement(ContainerId) {
    $(ContainerId).find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#importlist_cntr').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        if ($("#IsVehicle").val() == 'true' && $("#ImportFrom").val() == 'fleet') {
            AjaxPaginationForVehicleList(nextPage);
        }
        else if ($("#IsVehicle").val() == 'false' && $("#ImportFrom").val() == 'library') {
            AjaxPaginationforSORoute(nextPage);
        }
        else if ($("#ImportFrom").val() == 'prevMov') {
            AjaxPaginationForMovement(nextPage);
        }
        else {
            AjaxPaginationForComponentList(nextPage);
        }
    });
}
//method to paginate to Previous page
function PaginateToPrevPagesomovement(ContainerId) {
    $(ContainerId).find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#importlist_cntr').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        if ($("#IsVehicle").val() && $("#ImportFrom").val() == 'fleet') {
            AjaxPaginationForVehicleList(prevPage);
        }
        else if (!($("#IsVehicle").val()) && $("#ImportFrom").val() == 'library') {
            AjaxPaginationforSORoute(prevPage);
        }
        else if ($("#ImportFrom").val() == 'prevMov') {
            AjaxPaginationForMovement(prevPage);
        }
        else {
            AjaxPaginationForComponentList(prevPage);
        }
    });
}

function AjaxPaginationForMovement(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    var div = '#route_importlist_cntr';
    if ($("#IsVehicle").val() == 'true')
        div = '#importlist_cntr';
    var data = { page: pageNum, pageSize: pageSize, planMovement: true };
    $.ajax({
        url: '../Movements/MovementList',
        type: 'GET',
        cache: false,
        data: data,//{ page: pageNum, pageSize: pageSize, MovementListForSO: true, showrtveh: ShowPrevMoveSortRoute, IsNotify: true, PrevMovImport: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#banner-container').find('div#filters').remove();
            //document.getElementById("vehicles").style.filter = "unset";
            $(div).html($(result).find('.div_so_movement').html(), function () {
                event.preventDefault();
            });
            if ($("#IsVehicle").val())
                $(div).prepend('<span id="list_heading" class="title">Select vehicle from previous movements</span>');
            else
                $(div).prepend('<span id="list_heading" class="title">Select route from previous movements</span>');
            var filters = $(result).find('div#filters');
            $(filters).find('form .filters:first').remove();
            $(filters).find('form .filters:first').remove();
            $(filters).appendTo('#banner-container');
            removeHrefLinks();
            PaginateList();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

//function Ajax call for pagination
function AjaxPaginationForVehicleList(pageNum) {
    var txt_srch = $('.serchlefttxt').val();
    $.ajax({
        url: '../VehicleConfig/VehicleConfigList',
        data: { page: pageNum, importFlag: true, searchString: txt_srch },
        type: 'GET',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            ;
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
                event.preventDefault();
            });
            $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');
            var filters = $(response).find('div#filters');
            $(filters).appendTo('#banner-container');
            removeHrefLinks();
            PaginateList();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function AjaxPaginationforSORoute(pageNum) {
    ;
    var SearchString = $('#SearchString').val();
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    $.ajax({
        url: '../Routes/RoutePartLibrary',
        type: 'GET',
        cache: false,
        async: false,
        data: { SearchString: SearchString, page: pageNum, pageSize: pageSize, importFlag: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            $('#route_importlist_cntr').html($(response).find('#route-list'), function () {
                event.preventDefault();
            });
            $("#route_importlist_cntr").prepend('<span id="list_heading" class="title">Select route from library</span>');
            $('#route-list').find('.text-color2').removeAttr('href').css("cursor", "pointer");

            var filters = $(response).find('div#filters');
            $(filters).appendTo('#banner-container');

            removeHrefLinks();
            PaginateList();
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
            SetResponse(response);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function SetResponse(response) {
    ;
    $('#banner-container').find('div#filters').remove();
    document.getElementById("vehicles").style.filter = "unset";
    $('#importlist_cntr').html($(response).find('#vehicle-config-list'), function () {
        event.preventDefault();
    });
    $("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');
    var filters = $(response).find('div#filters');
    $(filters).appendTo('#banner-container');
    removeHrefLinks();
    PaginateList();
    //fillPageSizeSelect();
}

function LoadApplicationVehicleRoute(MovementVersionId, MovementType) {
    var cntrId = '#select_route_section';
    if ($("#IsVehicle").val() == 'true')
        cntrId = '#select_vehicle_section';
    LoadContentForAjaxCalls("POST", '../VehicleConfig/GetPreviousMovementList',
        { versionId: MovementVersionId, isVehicleImport: $("#IsVehicle").val(), movementType: MovementType }, cntrId, true);
}
function CurreMovemenRouteList(V_ReviosionId, VRList_type) {
    if ($("#IsCreateApplicationRoute").val() == "true") {
        showPreviousMovement(V_ReviosionId, VRList_type);
    }
    else {
        showRelatedMovements(V_ReviosionId, VRList_type);
    }
}
function showPreviousMovement(V_ReviosionId, VRList_type) {
    $("#divCandiRouteDeatils").html('');
    var rtrevisionId = V_ReviosionId;
    var iscandlastversion = $('#IsCandVersion').val();
    var plannruserid = $('#PlannrUserId').val();
    var appstatuscode = $('#AppStatusCode').val();
    var movversionno = $('#versionno').val();
    var movdistributed = $('#IsMovDistributed').val();
    var sonumber = $('#SONumber').val();
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
    var prjstatus = $('#Proj_Status').val();
    var VIsIsCreateApplication = false;
    var VIsRelStruMov = false;
    if ($("#IsMyStructure").val() == "Yes")
        VIsRelStruMov = true;
    $('#tem_ReviosionId').val(V_ReviosionId);
    $('#temRList_type').val(VRList_type);
    removescroll();
    $("#overlay").show();
    $('.loading').show();
    if ($("#IsCreateApplicationRoute").val() == "true" && $('#SortStatus').val() == "CreateSO")
        VIsIsCreateApplication = true;
    $.ajax({
        url: "../SORTApplication/CandiVersionRoutesList",
        type: 'post',
        async: false,
        beforeSend: startAnimation(),
        data: { routerevision_id: rtrevisionId, CheckerId: _checkerid, CheckerStatus: _checkerstatus, IsCandLastVersion: iscandlastversion, planneruserId: plannruserid, appStatusCode: appstatuscode, SONumber: sonumber, RList_type: VRList_type, IsIsCreateApplication: VIsIsCreateApplication, IsRelaStruMov: VIsRelStruMov },
        success: function (data) {
            $('#divCandiRouteDeatils').html(data);


            $("#overlay").show();
            $("#divCandiRouteDeatils").show();
            $('.loading').hide();
            $('#generalDetailDiv').hide();//hide generaldiv
            stopAnimation();
        },
        error: function () {
            stopAnimation();
        }
    });
}
function showRelatedMovements(V_ReviosionId, VRList_type) {
    var app_id = V_ReviosionId;
    var type = VRList_type;
    var link = '';
    $.ajax({
        url: "../SORTApplication/RelatedMovements",
        type: 'post',
        async: false,
        data: { app_Id: app_id, type: VRList_type },
        success: function (data) {
            var project_id = data.result.ProjectID;
            var hauliermnemonic = data.result.HaulierMnemonic;
            var esdalrefnum = data.result.ESDALReference;
            var revision_id = data.result.ApplicationRevisionId;
            var version_id = data.result.VersionID;
            var version_no = data.result.VersionNo;
            var revision_no = data.result.RevisionNo;
            var max_version_no = data.result.LastVersionNo;
            var enter_by_sort = data.result.EnteredBySORT;
            var organisation_id = data.result.OrganisationID;
            var owner_name = data.result.OwnerName;
            var esdal_reference = data.result.ESDALReference;
            var cand_analysis_id = data.result.CandidateAnalysisID;
            var cand_revision_id = data.result.CandidateRevisionID;
            var cand_rev_no = data.result.CandidateRevisionNo;
            var last_can_rev_no = data.result.LastCandidateRevisionNo;
            var cand_name = data.result.CandidateRouteName;
            var cand_rt_id = data.result.CandidateRouteID;
            var flag = false;
            if (last_can_rev_no == cand_rev_no)
                flag = true;
            if (VRList_type == 'M')
                link = '../SORTApplication/SORTListMovemnets?SORTStatus=MoveVer&projecid=' + project_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalrefnum + '&revisionId=' + revision_id + '&movementId=' + 270006 + '&apprevid=' + revision_id + '&revisionno=' + revision_no + '&OrganisationId=' + organisation_id + '&versionno=' + version_no + '&versionId=' + version_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&pageflag=' + 2 + '&esdal_history=' + esdal_reference + '&LatestVer=' + max_version_no + '&WorkStatus=' + 'undefined' + '&EnterBySORT=' + enter_by_sort + '&Checker=' + '' + '&Owner=' + owner_name + '&ViewFlag=' + 1;
            else if (VRList_type == 'C')
                link = '../SORTApplication/SORTListMovemnets?SORTStatus=CandidateRT&projecid=' + project_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalrefnum + '&revisionId=' + cand_revision_id + '&movementId=' + 27006 + '&apprevid=' + revision_id + '&revisionno=' + revision_no + '&OrganisationId=' + organisation_id + '&versionno=' + version_no + '&versionId=' + version_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&pageflag=' + 2 + '&esdal_history=' + esdal_reference + '&candName=' + cand_name + '&candVersionno=' + cand_rev_no + '&CandRouteId=' + cand_rt_id + '&LatestRevisionId=' + '' + '&analysisId=' + cand_analysis_id + '&IsLastVersion=' + flag + '&EnterBySORT=' + enter_by_sort + '&Owner=' + owner_name + '&ViewFlag=' + 1;
            else if (VRList_type == 'A')
                link = '../SORTApplication/SORTListMovemnets?SORTStatus=Revisions&projecid=' + project_id + "&OrganisationId=" + organisation_id + '&VR1Applciation=' + false + '&reduceddetailed=' + false + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalrefnum + '&revisionId=' + revision_id + '&versionId=' + version_id + '&movementId=' + 270006 + '&apprevid=' + revision_id + '&revisionno=' + revision_no + '&versionno=' + version_no + '&pageflag=' + 2 + '&arev_no=' + revision_no + '&arev_Id=' + revision_id + '&ver_no=' + 0 + '&WorkStatus=' + 'undefined' + '&Checker=' + '' + '&EnterBySORT=' + enter_by_sort + '&Owner=' + owner_name + '&ViewFlag=' + 1;;
        },
        complete: function (data) {
            window.open(link, '_blank');
        },
        error: function () {
        }
    });
}
function SetWorkflowProgress(currentStep) { // a skelten of function implimented 
    //workflow needs to be integrated here.. function may move to other page 
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
                    var country = $("#CountryID option[value=" + datacollection.result[0].CountryId + "]").text();
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
                                $("#ddlHaulierContactName").append("<option value=''>-- Select contact name --</option>");
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
                                $("#hdnphonenumber").val(arrPhone);
                                $("#hdncontactId").val(arrContactId);
                                $("#hdncontactId").val(arrContactId);
                                $("#hdnCountryID").val(arrCountyId);
                            }
                            else {
                                $("#ddlHaulierContactName Option").each(function () { $(this).remove(); });
                                $("#ddlHaulierContactName").append("<option value=''>-- No contact found --</option>");
                                $('#EmailIdSORT').val("");
                                $('#FaxSORT').val("");
                                $("#ddlHaulierContactName").val("0");

                            }
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
                    $('#backbutton').show();
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
    $('#err_OrgCode_exists').text('');
    $('#ownerValidationMsg').hide();
    $("#codeIsRequiredMsg").hide();
    $("#countryValidationMsg").hide();
    $("#postCodeIsRequiredMsg").hide();
    $("#phoneIsRequiredMsg").hide();
    $('#save_btn').show();
    $('#confirm_btn').hide();
}
function LoadContentForAjaxCalls(Type, Url, Params, ResLoadContnr, routeVehicleFlag) {
    $.ajax({
        type: Type,
        url: Url,
        data: Params,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            if (routeVehicleFlag && response.flag) {
                ShowInfoPopup("The movement does not contain the vehicle/route details");
            }
            else {
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

                $(ResLoadContnr).show();
                $(ResLoadContnr).html(response);
            }
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
$(document).ready(function () {
    $('body').on('click', '#ahrefVersinRevision', function (e) {
        e.preventDefault();
        var VersinRevisionId = $(this).data('versinrevisionid');
        var VersinVrListType = $(this).data('versinvrlisttype');
        CurreMovemenRouteList(VersinRevisionId, VersinVrListType);
    });
});