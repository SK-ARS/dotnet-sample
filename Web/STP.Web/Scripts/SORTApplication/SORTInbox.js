$(document).ready(function () {
    //$('#filterimageERNSort').click(function () { return false; });
    mapsearchflag = 0;
    mapsearchtrigger = 0;
    resetMapSearch();
    var esdalReference = $('#filter_SORT').find('#ESDALReference').val();
    $('#ESDALReference').val(esdalReference);
    $("#div_MoveList_MapSearch").hide();
    $("#menu-btn").show();
    //outside  ToggleShowMyProjectsOutSide()
    $('body').on('change', '#outsideFilterMoveInboxSORT #ShowMyProjects', function () {
        if ($("#ShowMyProjects").prop('checked') == true) {
            $('#ShowMyProjects').val(true);
            $('#hdnShowMyProjects').val("1");
            $('#filter_SORT').find('#ShowMyProjects').prop('checked', true);

        }
        else if ($("#ShowMyProjects").prop('checked') == false) {
            $('#ShowMyProjects').val(false);
            $('#hdnShowMyProjects').val("0");
            $('#filter_SORT').find('#ShowMyProjects').prop('checked', false);

        }
        SearchSortListOutside(1);
    });
    //ToggleShowMyProjectsInside
    $('body').on('change', '#filter_SORT #ShowMyProjects', function () {
        if ($('#filter_SORT').find('#ShowMyProjects').prop('checked') == true) {
            //$('#ShowMyProjects').val(true);
            //$('#ShowMyProjects').prop('checked', true);
            $('#hdnShowMyProjects').val("1");
        }
        else if ($('#filter_SORT').find('#ShowMyProjects').prop('checked') == false) {
            //$('#ShowMyProjects').val(false);
            //$('#ShowMyProjects').prop('checked', false);
            $('#hdnShowMyProjects').val("0");
        }
    });
    $('body').on('click', '.movement-inbox .sortpagination a,#previousMovementListDiv .sortpagination a,#divCurrentMovement .sortpagination a', function (e) {
        e.preventDefault();
        startAnimation();
        var page = getUrlParameterByName("page", this.href);
        $('#pageNumsort').val(page);
        var pageSize = $('#pageSizeSelect').val();
        try {
            structureId = currentMouseOverFeature.data.name.match("STRUCTURE (.*) NAME");

            StrucID = structureId[1];
        }
        catch (err) {
            try {
                if (hf_StructureId == null || hf_StructureId == '0' || hf_StructureId == undefined) {
                    FilterSuccessdata(isSort = true, true, pageSize);//using sorting as true to avoid page reset
                }
            }
            catch (err) {
                FilterSuccessdata(isSort = true, true, pageSize);//using sorting as true to avoid page reset
            }
        }
    });
    $('body').on('click', '#btnfiltersuccessdata', function (e) {
        e.preventDefault();
        FilterSuccessdata();
        $('#outsideFilterMoveInboxSORT').find('#ESDALReference').val($('#filter_SORT #ESDALReference').val());
    });
    $('body').on('click', '.clrsortdata', function (e) { e.preventDefault(); ClearSORTData(); });
    $('body').on('click', '.sortwithdrawn', function (e) {
        e.preventDefault();
        var param1 = $(this).data('arg1');
        var param2 = $(this).data('arg2');
        var param3 = $(this).data('arg3');
        var param4 = $(this).data('arg4');
        SortWithdraw1(param1, param2, param3, param4);
    });
    $('body').on('click', '.withdrawsoapplcn', function (e) {
        e.preventDefault();
        var param1 = $(this).data('arg1');
        var param2 = $(this).data('arg2');
        var param3 = $(this).data('arg3');
        var param4 = $(this).data('arg4');
        var param5 = $(this).data('arg5');
        WithdrawSoApplicationFromSort(param1, param2, param3, param4, param5, 2);
    });
    $('body').on('click', '#showcandidatevehicle,.showcandidatevehicle', function (e) { e.preventDefault(); Show_CandidateRTVehicles(); });
    $('body').on('click', '#tblSortInbox #filterimage', function (e) { e.preventDefault(); ClearSORTData(); });
    $('body').on('click', '.selectmovement', function (e) {
        e.preventDefault();
        SelectPrevitMovementsVehicleFn(this);
    });
    $('body').on('click', '.slctcrntmvmt', function (e) {
        e.preventDefault();
        var param1 = $(this).attr('arg1');
        var param2 = $(this).attr('arg2');
        var param3 = $(this).attr('arg3');
        var param4 = $(this).attr('arg4');
        var param5 = $(this).attr('arg5');
        SelectCurrentMovementsRoute(0, param1, param2, param3, param4, param5);
    });
    $('body').on('click', '.selectprevmovementvr1', function (e) {
        e.preventDefault();
        var param1 = $(this).attr('arg1');
        var param2 = $(this).attr('arg2');
        var param3 = $(this).attr('arg3');
        var param4 = $(this).attr('arg4');
        var param5 = $(this).attr('arg5');
        SelectPrevitMovementsVehicle(0, param1, param2, param3, param4, param5);
    });
    $('body').on('click', '.selectcurrentmovementvr1', function (e) {
        e.preventDefault();
        var param1 = $(this).attr('arg1');
        var param2 = $(this).attr('arg2');
        var param3 = $(this).attr('arg3');
        var param4 = $(this).attr('arg4');
        var param5 = $(this).attr('arg5');
        SelectCurrentMovementsRoute(0, param1, param2, param3, param4, param5);
    });
    $('body').on('change', '.SORTInbox-Pag #pageSizeSelect', function () {
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        var page = getUrlParameterByName("page", this.href);
        $('#pageNumsort').val(page);
        FilterSuccessdata(isSort = true, true, pageSize);
    });
    $('body').on('keyup', '#outsideFilterMoveInboxSORT #ESDALReference', function () {
        $('#filter_SORT').find('#ESDALReference').val($('#ESDALReference').val());
        SearchSortEsdalReference(showAnimation = false);
    });
});
function SelectPrevitMovementsVehicleFn(e) {
    var param1 = $(e).attr('arg1');
    var param2 = $(e).attr('arg2');
    var param3 = $(e).attr('arg3');
    var param4 = $(e).attr('arg4');
    var param5 = $(e).attr('arg5');
    SelectPrevitMovementsVehicle(0, param1, param2, param3, param4, param5);
}
function ReviseSOApplication(ApprevId) {
    //WarningCancelBtn();
    $("#overlay").show();
    $('.loading').show();
    $.ajax({
        url: "../Application/ReviseSOApplication",
        type: 'post',
        data: { apprevid: ApprevId },
        success: function (data) {
            // var url = '../SORTApplication/SORTListMovemnets?SORTStatus=CreateSO&cloneapprevid=' + data + '&revisionId=' + revisionId + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&apprevid=' + ApprevId + '&pageflag=2';
            if (data != 0) {
                window.location.href = "../SORTApplication/SORTListMovemnets?SORTStatus=CreateSO&cloneapprevid=" + data + '&revisionId=' + data + '&versionId=' + versionId + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&OrganisationId=' + OrgID + '&projecid=' + projectid + '&arev_no=' + revision_no + '&apprevid=' + data + '&pageflag=2' + '&EditRev=true' + '&WorkStatus=' + Work_status;
            }
            // window.location = url;
            $("#overlay").hide();
            $('.loading').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}
// showing user-setting-info-filter


// showing user-setting inside vertical menu
function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}
function clearflag() {
    mapsearchflag = 0;
}
function handleAddressSearch(e, obj) {

    if (e.keyCode === 13) {
        mapsearchflag = 1;
        mapsearchtrigger = 1;
        var text = $.trim($(obj).val());
        searchAddress(text);
        $("#Mapfilterdiv").show();
        $("#div_MoveList_MapSearch").show();
        $("#menu-btn").hide();
        $("#Mapfilterdiv").show();
        $("#map").html('');
        $("#map").load('/Routes/A2BPlanning?routeID=0', function () {
            loadmap('SORTMAPFILTER_VIEWANDEDIT');
            $("#div_MoveList_advanceSearch").hide();
            closeFilters();
            // roadOwnerShip_leftPanel();
        });
    }
}
function viewStatus() {
    if (document.getElementById('viewstatus').style.display !== "none") {
        document.getElementById('viewstatus').style.display = "none"
        document.getElementById('chevlon-up-icon').style.display = "none"
        document.getElementById('chevlon-down-icon').style.display = "block"
    }
    else {
        document.getElementById('viewstatus').style.display = "block"
        document.getElementById('chevlon-up-icon').style.display = "block"
        document.getElementById('chevlon-down-icon').style.display = "none"
    }
}
// showing filter-roads in side-nav
function viewmovements() {
    if (document.getElementById('viewotheroptions').style.display !== "none") {
        document.getElementById('viewotheroptions').style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById('viewotheroptions').style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}
// showing filter-roads in side-nav
function viewSORTAdvHaulier(_this) {
    if (typeof _this == 'undefined') {
        if ($('#viewSORTAdvHaulier').is(':visible')) {
            $('#viewSORTAdvHaulier').css('display', "none");
            $('#chevlon-up-icon2').css('display', "none");
            $('#chevlon-down-icon2').css('display', "block");
        }
        else {
            $('#viewSORTAdvHaulier').css('display', "block");
            $('#chevlon-up-icon2').css('display', "block");
            $('#chevlon-down-icon2').css('display', "none");
        }
    } else {
        var parentElem = $(_this).closest('.sidenav');
        if (parentElem.find('#viewSORTAdvHaulier').is(':visible')) {//style.display !== "none"
            parentElem.find('#viewSORTAdvHaulier').css('display', "none");
            parentElem.find('#chevlon-up-icon2').css('display', "none");
            parentElem.find('#chevlon-down-icon2').css('display', "block");
        }
        else {
            parentElem.find('#viewSORTAdvHaulier').css('display', "block");
            parentElem.find('#chevlon-up-icon2').css('display', "block");
            parentElem.find('#chevlon-down-icon2').css('display', "none");
        }
    }
}

function hidemapfilter() {
    $('#viewMapFilter').css("display", "none");
    $('#chevlon-up-icon4').css("display", "none");
    $('#chevlon-down-icon4').css("display", "block");
    $(".chevlon-up-icon-map").hide();
    $(".chevlon-down-icon-map").show();
    $("#Mapfilterdiv").hide();
    $("#div_MoveList_MapSearch").hide();
    $("#menu-btn").show();
    $("#div_MoveList_advanceSearch").show();
    $.ajax({
        url: '/SORTApplication/SORTInboxFilter',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
}
function searchclear() {
    mapsearchflag = 0;
    mapsearchtrigger = 0;
}
// showing filter-status in side-nav
function viewMapFilter() {
    if (document.getElementById('viewMapFilter').style.display !== "none") {
        document.getElementById('viewMapFilter').style.display = "none"
        document.getElementById('chevlon-up-icon4').style.display = "none"
        document.getElementById('chevlon-down-icon4').style.display = "block"
        $(".chevlon-up-icon-map").hide();
        $(".chevlon-down-icon-map").show();
        $("#Mapfilterdiv").hide();
        $("#div_MoveList_MapSearch").hide();
        $("#menu-btn").show();
        $("#div_MoveList_advanceSearch").show();
        $.ajax({
            url: '/SORTApplication/SORTInboxFilter',
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
    }
    else {
        document.getElementById('viewMapFilter').style.display = "block"
        document.getElementById('chevlon-up-icon4').style.display = "block"
        document.getElementById('chevlon-down-icon4').style.display = "none"
        $(".chevlon-up-icon-map").show();
        $(".chevlon-down-icon-map").hide();
        $("#div_MoveList_MapSearch").show();
        $("#menu-btn").hide();
        $("#Mapfilterdiv").show();
        $("#map").html('');
        $("#map").load('/Routes/A2BPlanning?routeID=0', function () {
            loadmap('SORTMAPFILTER_VIEWANDEDIT');
        });
        if (mapaddresssearch != undefined && mapaddresssearch != "") {
            $("#txtAddressSearch").val(mapaddresssearch);
        }
        $("#div_MoveList_advanceSearch").hide();
    }
}
// show pop-up card in the column
function showcard() {
    if (document.getElementById('showcard').style.display !== "none") {
        document.getElementById('showcard').style.display = "none"
    }
    else {
        document.getElementById('showcard').style.display = "block"
    }
}
// Attach listener function on state changes
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
    $.each($("#viewotheroptions input:checkbox"), function () {
        togglecheckbox($(this));
    });
}
function ClearSORTData(flag, reset = true) {
    closeFilters();
    //document.getElementById('viewMapFilter').style.display = "none";
    var _advFilter = $('#filter_SORT');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:checkbox:not("#ShowMyProjects")').prop('checked', false);
    $('#filter_SORT').find('option:selected').prop("selected", false);
    $("#ApplicationType").val('');
    EnableDisableDatePicker();
    mapsearchflag = 0;
    mapsearchtrigger = 0;

    $("#div_MoveList_MapSearch").hide();
    $("#menu-btn").show();
    $("#Mapfilterdiv").hide();
    if (flag == 0 || flag == undefined) {
        $('#ShowMyProjects').prop('checked', false);
        $('#filter_SORT').find('#ShowMyProjects').prop('checked', false);
        $('#hdnShowMyProjects').val("0");
    }
    $(".VehicleFilter").slice(1).remove();
    $('#AddOption ,#SORTAddOption').show();
    $('#RemoveOption ,#SORTRemoveOption').hide();
    $('.gross_weightUnit').text('kg');
    $('.gross_weight1Unit').text('kg');
    //location.reload();
    $("#filterimage").hide();
    $('#filterimageERNSort').hide();
    $('#FilterMoveInboxSORT #ESDALReferenceNumber').val('');
    var value = $('#ESDALReference').val($('#filter_SORT').find('#ESDALReference').val());
    if (value == '') {
        $('#filterimageERNSort').hide();
    }
    resetMapSearch();
    if (reset) {
        hidemapfilter();
        ResetDataSORT();
    }
}
function ResetDataSORT(reloadList = true) {
    $.ajax({
        url: '../SORTApplication/ClearInboxAdvancedFilterSORT',
        type: 'POST',
        success: function (data) {
            //ClearAdvancedData();
            if (reloadList)
                FilterSuccessdata();
            
        }
    });
}

function FilterSuccessdata(isSort = false, showAnimation = true, pageSize = 10, page = 1) {
    var appTableAlias = "b.";
    var queryString = "";
    var strVehiclebtw = "";
    var strVehicle = "";
    var operator = "";
    var totalLength = $('#VehicleFilterDiv').find('.VehicleFilter').length;
    var filterIndex = 0;
    $('#VehicleFilterDiv').find('.VehicleFilter').each(function (index, elem) {
        filterIndex++;
        operator = $(this).find(".vehicleOperator option:selected").val() != "" && $(this).find(".vehicleOperator option:selected").val() != "0" && filterIndex<totalLength ? $(this).find(".vehicleOperator option:selected").text() : "";
        if ($(this).find('#OperatorCount').val() == "between") {
            strVehiclebtw = $(this).find('select[name="VehicleDimensionCount"]').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() + " and " + $(this).find('.vehicletextbox1').val() + " " + operator;
            queryString += " " + appTableAlias + strVehiclebtw;
        }
        else if ($(this).find('.vehicletextbox').val() != "") {
            strVehicle = $(this).find('select[name="VehicleDimensionCount"]').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() + " " + operator;
            queryString += " " + appTableAlias + strVehicle;
        }
       
    });
    //var trimedString = queryString.trim();
    //var lastIndex = trimedString.lastIndexOf(" ");
    //str = trimedString.substring(0, lastIndex);
    if (queryString != "") {
        queryString = "(" + queryString + ")";
    }
    $('#QueryString').val(queryString);

    var page = $('#pageNumsort').val();

    var sortType = $('#FilterMoveInboxSORT #SortTypeValue').val();
    var sortOrder = $('#FilterMoveInboxSORT #SortOrderValue').val();
    var params = isSort ? "page=" + page : "page=1";
    var param = isSort ? "pageSize=" + pageSize : "pageSize =10";
    var paramPlanMovement = "";
    if ($('#hf_IsPlanMovmentGlobal').length > 0) {
        $('div#viewstatus #input-entry').remove();
        paramPlanMovement = '&planMovement=' + true;
    }

    var isVehicleFlag = $("#IsVehicle").val() == 'true';
    if ($('#hf_IsPlanMovmentGlobal').length > 0 && isVehicleFlag)
        paramPlanMovement = '&planMovement=' + true + '&IsPrevtMovementsVehicle=' + true;
    if ($('#previousMovementListDiv').length > 0 && $('#previousMovementListDiv').is(':visible')) {
        paramPlanMovement = '&IsPrevtMovementsVehicle=' + true;
        $("#ShowMyProjects").prop('checked', false);
    } else if ($('#divCurrentMovement').length > 0 && $('#divCurrentMovement').is(':visible')) {//CANDIDATE ROUTE - ROUTE IMPORT
        paramPlanMovement = '&IsPrevtMovementsVehicleRoute=' + true;
        $("#ShowMyProjects").prop('checked', false);
    }
        //PLAN MOVEMENT - ROUTE IMPORT
    else if ($('#hf_IsPlanMovmentGlobal').length > 0 && !isVehicleFlag && $('#route_importlist_cntr').length > 0 && $('#route_importlist_cntr').is(':visible')) {
        paramPlanMovement = '&planMovement=' + true + '&IsPrevtMovementsVehicleRoute=' + true;
        $("#ShowMyProjects").prop('checked', false);
    }

    var $haulierNameElem = $("#FilterMoveInboxSORT #viewstatus #HaulierName");//In previous movement import the haulier name search issue fix
    var $advHaulierElm = $("#FilterMoveInboxSORT #viewSORTAdvHaulier #HaulierName");
    if ($advHaulierElm.is(':visible') || $advHaulierElm.val()!='') //In movement list, the advanced search hauliername field will be empty
        $haulierNameElem.val($advHaulierElm.val());

    $.ajax({

        url: '/SORTApplication/SetSORTFilter?' + params + '&' + param + paramPlanMovement + "&sortOrder=" + sortOrder + "&sortType=" + sortType,
        type: 'POST',
        cache: false,
        data: $("#FilterMoveInboxSORT").serialize(),
        beforeSend: function () {
            if (showAnimation)
                startAnimation();
            else
                openContentLoader('#sort-movement-table');
        },
        success: function (result) {
            if (mapsearchflag == 1 || mapsearchtrigger == 1) {
                $("#div_MoveList_advanceSearch").hide();
                $("#Mapfilterdiv").show();
                $("#div_MoveList_MapSearch").show();
                $("#menu-btn").hide();
                $("#Mapfilterdiv").show();
                $("#map").html('');
                $("#map").load('/Routes/A2BPlanning?routeID=0', function () {
                    loadmap('SORTMAPFILTER_VIEWANDEDIT');
                });
                mapsearchtrigger = 0;

            }
            else {
                var parent = "#div_MoveList_advanceSearch";
                if ($('#hf_IsPlanMovmentGlobal').length > 0)
                    parent = isVehicleFlag ? "#importlist_cntr" : "#route_importlist_cntr";
                

                $("#div_MoveList_MapSearch").hide();
                $("#menu-btn").show();
                $("#Mapfilterdiv").hide();
                $(parent).show();
                //$('.div_so_movementdata').html('');
                //$('.div_so_movementdata').html($(result).find('.div_so_movementdata').html());

                if ($('#previousMovementListDiv').length > 0 && $('#previousMovementListDiv').is(':visible')) {
                    $('#previousMovementListDiv #sort-movement-table').html('');
                    $('#previousMovementListDiv #sort-movement-table').html($(result).find('#sort-movement-table').html());
                    $('.divViewMapFilter').remove();
                } else if ($(parent).length > 0) {
                    $(parent).html('');
                    $(parent).html($(result).find('#div_MoveList_advanceSearch').html());
                } else if ($('#divCurrentMovement').length > 0) {//Candidate route
                    var divCurrentMovement = '#divCurrentMovement';
                    $(divCurrentMovement).html($(result).find(parent).html());
                }
                closeFilters();
                $("#Mapfilterdiv").hide();
                if ($('#divCurrentMovement').length > 0 && $('#divCurrentMovement').is(':visible')) {
                    //Candidate Route Back button add
                    $('#divCurrentMovement').append('<div id="divbtn_prevmove1" class="row mt-4" style="float: right;"><button class= "btn outline-btn-primary SOAButtonHelper mr-2 mb-2" id="bindroutes1" >BACK</button></div>');
                }
            }
            if ($('#hdnShowMyProjects').val() == "0") {
                $("#ShowMyProjects").prop('checked', false);
                $('#filter_SORT').find('#ShowMyProjects').prop('checked', false);
            }
            else {
                $("#ShowMyProjects").prop('checked', true);
                $('#filter_SORT').find('#ShowMyProjects').prop('checked', true);
            }
        },
        error: function (xhr, status) {
            location.reload();
        },
        complete: function () {
            mapsearchflag = 0;
            mapsearchtrigger = 0;
            isSearchStarted = false;
            if (isApiCallPending) {
                isApiCallPending = false;
                FilterSuccessdata(false,false);
            } else {
                if (showAnimation)
                    stopAnimation();
                else
                    closeContentLoader('#sort-movement-table');
                showAnimation = true;
            }
            
        }
    });
}
//function FilterSuccessdatas(isSort = false, showAnimation = true,page, pageSize = 10) {
//    var appTableAlias = "b.";
//    var queryString = "";
//    var strVehiclebtw = "";
//    var strVehicle = "";
//    var operator = "";
//    $('#VehicleFilterDiv').find('.VehicleFilter').each(function () {
//        operator = $(this).find(".vehicleOperator option:selected").val() != "" ? $(this).find(".vehicleOperator option:selected").text() : "";
//        if ($(this).find('#OperatorCount').val() == "between") {
//            strVehiclebtw = $(this).find('#VehicleDimensionCount').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() + " and " + $(this).find('.vehicletextbox1').val() + " " + operator;
//            queryString += " " + appTableAlias + strVehiclebtw;
//        }
//        else if ($(this).find('.vehicletextbox').val() != "") {
//            strVehicle = $(this).find('#VehicleDimensionCount').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() + " " + operator;
//            queryString += " " + appTableAlias + strVehicle;
//        }
//    });
//    var trimedString = queryString.trim();
//    var lastIndex = trimedString.lastIndexOf(" ");
//    str = trimedString.substring(0, lastIndex);
//    if (str != "") {
//        str = "(" + str + ")";
//    }
//    $('#QueryString').val(str);

//    //if (isSort == true) {
//    //    var page = $('#pageNum').val();
//    //}

//    var sortType = $('#FilterMoveInboxSORT #SortTypeValue').val();
//    var sortOrder = $('#FilterMoveInboxSORT #SortOrderValue').val();
//    var params = isSort ? "page=" + page : "page=1";
//    var param = isSort ? "pageSize=" + pageSize : "pageSize =10";
//    var paramPlanMovement = "";
//    if ($('#hf_IsPlanMovmentGlobal').length > 0 && $("#IsVehicle").val() == 'true')
//        paramPlanMovement = '&planMovement=' + true + '&IsPrevtMovementsVehicle=' + true;
//    //else
//    //    paramPlanMovement = '&planMovement=' + true + '&IsPrevtMovementsVehicleRoute=' + true;
//    $.ajax({

//        url: '/SORTApplication/SetSORTFilter?' + params + '&' + param + paramPlanMovement + "&sortOrder=" + sortOrder + "&sortType=" + sortType,
//        type: 'POST',
//        cache: false,
//        async: false,
//        data: $("#FilterMoveInboxSORT").serialize(),
//        beforeSend: function () {
//            if (showAnimation)
//                startAnimation();
//        },
//        success: function (result) {


//            if (mapsearchflag == 1 || mapsearchtrigger == 1) {
//                $("#div_MoveList_advanceSearch").hide();
//                $("#Mapfilterdiv").show();
//                $("#div_MoveList_MapSearch").show();
//                $("#Mapfilterdiv").show();
//                $("#map").html('');
//                $("#map").load('/Routes/A2BPlanning?routeID=0', function () {
//                    loadmap('SORTMAPFILTER_VIEWANDEDIT');
//                });
//                mapsearchtrigger = 0;

//            }
//            else {
//                var parent = "#div_MoveList_advanceSearch";
//                if ($('#hf_IsPlanMovmentGlobal').length > 0)
//                    parent = ($("#IsVehicle").val() == 'true') ? "#importlist_cntr" : "#route_importlist_cntr";
//                $("#div_MoveList_MapSearch").hide();
//                $("#Mapfilterdiv").hide();
//                $(parent).show();
//                //$('.div_so_movementdata').html('');
//                //$('.div_so_movementdata').html($(result).find('.div_so_movementdata').html());
//                $(parent).html('');
//                $(parent).html($(result).find('#div_MoveList_advanceSearch').html());
//                closeFilters();
//                $("#Mapfilterdiv").hide();

//                //to fix folder style issue
//                var totalCount = $('#TotalItemsCount').val() || 0;
//                //if (parseInt(totalCount) <= 0) {
//                //    $('.div_so_movementdata').removeClass('sort-listing-folder-parent');
//                //} else {
//                //    $('.div_so_movementdata').addClass('sort-listing-folder-parent');
//                //}
//            }
//            if ($('#hdnShowMyProjects').val() == "0") {
//                $("#ShowMyProjects").prop('checked', false);
//                $('#filter_SORT').find('#ShowMyProjects').prop('checked', false);
//            }
//            else {
//                $("#ShowMyProjects").prop('checked', true);
//                $('#filter_SORT').find('#ShowMyProjects').prop('checked', true);
//            }
//            var value = $('#filter_SORT').find('#ESDALReference').val();


//        },
//        error: function (xhr, status) {
//            location.reload();
//        },
//        complete: function () {
//            mapsearchflag = 0;
//            mapsearchtrigger = 0;
//            if (showAnimation)
//                stopAnimation();
//        }
//    });
//}
function OnFailure(result) {
    closeFilters();
    stopAnimation();
}
function OnBegin(result) {
    startAnimation();
}

var Flag_App_Status = "";
function SortWithdraw1(EnterBySort, HaulierMnemonic, ProjectEsdalReference, projectid) {
    var esdal_ref_no = HaulierMnemonic + "/" + ProjectEsdalReference;
    var projectid = projectid;
    var Msg = "Do you want to withdraw application \"" + esdal_ref_no + '\" ?';
    flag = 1;
    if (EnterBySort == 1) {
        ShowDialogWarningPop(Msg, 'No', 'Yes', 'WarningCancelBtn', 'SORTApplication', 1, 'warning', esdal_ref_no, projectid);
        //location.reload();
    } else {
        ShowDialogWarningPop('Haulier created application cannot be withdrawn from SORT!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
}
function SORTApplication(esdal_ref_no = "", projectid = "", flag) {
    var esdalRef = esdal_ref_no;
    //var esdalref = hauliermnemonic + "/" + esdalref;
    var project_id = projectid;
    var lastrevisionno = $('#revisionno').val();
    var lastversionno = $('#hf_LatVer').val() != '' ? $('#hf_LatVer').val() : 0;
    //flag: 1= Withdraw; 2= Decline;
    $.ajax({
        url: '../SORTApplication/ApplicationWithAndDecline',
        type: 'POST',
        cache: false,
        async: true,
        data: { Project_ID: project_id, flag: flag, EsdalRef: esdalRef, lastrevisionno: lastrevisionno, lastversionno: lastversionno },
        beforeSend: function () {
            WarningCancelBtn();
            startAnimation();
            CloseWarningPopupDialog();
        },
        success: function (result) {
            if (result.Success == '1') {
                if (flag == 1) {
                    ShowDialogWarningPop('"' + esdalRef + '" application is withdrawn.', 'Ok', '', 'linktomovementinbox', '', 1, 'info');
                } else {
                    ShowDialogWarningPop('"' + esdalRef + '" application is declined.', 'Ok', '', 'linktomovementinbox', '', 1, 'info');
                }
            } else {
                if (flag == 1) {
                    ShowDialogWarningPop('Application is not withdrawn.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                } else {
                    ShowDialogWarningPop('Application is not declined.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function linktomovementinbox() {
    WarningCancelBtn();
    window.location.href = '/SORTApplication/SORTInbox';
}
function MovementSort(event, param) {
    var sortTypeGlobal = 1;//desc
    var sortOrderGlobal = 4;//date
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;//1 -desc            //0-asc
    var page = $('#pageNumsort').val();
    var pageSize = $('#pageSizeSelect').val();
    $('#FilterMoveInboxSORT #SortTypeValue').val(sortTypeGlobal);
    $('#FilterMoveInboxSORT #SortOrderValue').val(sortOrderGlobal);
    FilterSuccessdata(isSort = true, true, pageSize, page);
}
var isSearchStarted = false;
var isApiCallPending = false;
function SearchSortEsdalReference(showAnimation) {
    if (isSearchStarted) {
        isApiCallPending = true;
        return false;
    }
    isSearchStarted = true;
    FilterSuccessdata(false, showAnimation);
}
function SearchSortListOutside(showAnimation = true) {
    FilterSuccessdata(false, showAnimation);
}
function ClearSORTDatas(_this) {
    ClearSORTData(0);
}
function WithdrawSoApplicationSORT(projectid, Enteredbysort, ESDALReference, ApplicationRevId, movementType, RedirectParam) {
    var project_id = projectid ? projectid : 0;
    var Enteredby = Enteredbysort;
    var ESDAL_Reference = ESDALReference;
    if (Enteredby == 1) {
        CheckLatestAppStatus(project_id);// && app status checked for #7855
        let Msg = "Do you want to withdraw application?";
        if (ESDAL_Reference != '')
            Msg = "Do you want to withdraw application \"" + ESDAL_Reference + '\" ?';

        ShowWarningPopup(Msg, 'WithdrawSoApp', '', projectid, ApplicationRevId, ESDAL_Reference, movementType, RedirectParam);
    }
    else {
        ShowErrorPopup('Haulier created application cannot be withdrawn from SORT!');
    }
}