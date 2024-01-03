
var structureID = $('#hf_structureId').val();
$(document).ready(function () {
    $('body').on('click', '.sort-portal .filters', function (e) {
        var targetElem = $(this).data('target');
        if ($('.sort-portal #' + targetElem).is(":visible")) {//visible
            $('.sort-portal #' + targetElem).css("display", "none");
            $('.sort-portal #' + targetElem +" #chevlon-up-icon").css("display", "none");
            $('.sort-portal #' + targetElem + " #chevlon-down-icon").css("display", "block");

            $(this).find(".chevlon-up-icon").css("display", "none");
            $(this).find(".chevlon-down-icon").css("display", "block");

        } else {
            $('.sort-portal #' + targetElem).css("display", "block");
            $('.sort-portal #' + targetElem + " #chevlon-up-icon").css("display", "block");
            $('.sort-portal #' + targetElem + " #chevlon-down-icon").css("display", "none");

            $(this).find(".chevlon-up-icon").css("display", "block");
            $(this).find(".chevlon-down-icon").css("display", "none");
        }
    });

    $('body').on('click', '#SearchSOAList', function (e) {
        SearchSOAList();
        $('#outsideFilterMoveInboxSOA').find('#ESDALReference').val($('#frmFilterMoveInbox #ESDALReference').val());
    });
    $('body').on('click', '#ClearAdvanced', function (e) {
        e.preventDefault();
        ClearAdvancedfn(this);
    });
    $('body').on('click', '#filters #table-head', function (e) {
        
        viewAdvanceSearch(this);
    });
    $('body').on('change', '#UnopenedToggle', function (e) {
        ToggleUnopened();
    });
    $('body').on('change', '.soa-police-movement-filter #Unopened', function (e) {
        if ($(this).prop('checked') == true) {
            $('#UnopenedToggle').prop('checked',true);
        }
        else if ($(this).prop('checked') == false) {
            $('#UnopenedToggle').prop('checked',false);
        }
    });

    if (structureID == "0") {
        SelectMenu(2);
    }
     
    $('body').on('click', '.movements .soa-pagination a', function (e) {
        e.preventDefault();
        startAnimation();
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        SearchSOAList(isSearch = false, isSort = true);//using sorting as true to avoid page reset
    });

    $('body').on('change', '.MovementInboxList-Pag #pageSizeSelect', function () {
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        SearchSOAList(isSearch = false, isSort = true);
    });

    $('body').on('keyup', '.txt-mib-esdal-ref-search', function () {
        SearchSOAEsdalReference();
    });
});

function ClearAdvancedfn() {
    ClearAdvanced(0);
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
// showing user-setting-info-filter
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

function BackRevStructSummary() {
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structureID);
}

var isSearchStarted = false;
var isApiCallPending = false;
function SearchSOAList(isSearch = false, isSort = false) {
    if (isSearchStarted) {
        isApiCallPending = true;
        return false;
    }
    console.log('isSearchStarted', isSearchStarted);
    isSearchStarted = true;
    //var insideValue = $('#ESDALReference').val();
    var notiTableAlias = "noti.";
    var appTableAlias = "a.";

    var queryString = "";
    var strVehiclebtw = "";
    var strVehicle = "";
    var operator = "";
    var totalLength = $('#VehicleFilterDiv').find('.VehicleFilter').length;
    var filterIndex = 0;
    var dynamic = [];
    $('#VehicleFilterDiv').find('.VehicleFilter').each(function () {
        filterIndex++;
        operator = $(this).find(".vehicleOperator option:selected").val() != "" && $(this).find(".vehicleOperator option:selected").val() != "0" && filterIndex < totalLength  ? $(this).find(".vehicleOperator option:selected").text() : "";
        if ($(this).find('#OperatorCount').val() == "between") {
            strVehiclebtw = $(this).find('select[name="VehicleDimensionCount"]').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() + " and " + $(this).find('.vehicletextbox1').val();
            queryString += " (" + notiTableAlias + strVehiclebtw + " or " + appTableAlias + strVehiclebtw + ") " + operator;
        }
        else if ($(this).find('.vehicletextbox').val() != "") {
            strVehicle = $(this).find('select[name="VehicleDimensionCount"]').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val();
            queryString += " (" + notiTableAlias + strVehicle + " or " + appTableAlias + strVehicle + ") " + operator;
        }
        var dobj = {
            SOAVehicleDimension: $(this).find('select[name="VehicleDimensionCount"]').val(), OperatorCount: $(this).find('#OperatorCount').val(), Operator: $(this).find(".vehicleOperator option:selected").val(),
            GrossWeight: $(this).find('.vehicletextbox').val(), GrossWeight1: $(this).find('.vehicletextbox1').val()
        }
        dynamic.push(dobj);

    });
    //var trimedString = queryString.trim();
    //var lastIndex = trimedString.lastIndexOf(" ");
    //str = trimedString.substring(0, lastIndex);
    if (queryString != "") {
        queryString = "(" + queryString + ")";
    }
    $('#QueryString').val(queryString);

    //$('#ESDALReference').val(insideValue);
    var params = isSort ? "page=" + $('#pageNum').val() : "page=1";
    params += "&pageSize=" + $('#pageSizeVal').val();
    var obj = GetDivObjectJson('#frmFilterMoveInbox');
    obj.dynamicfilters = dynamic;
    $.ajax({
        url: '/Movements/FilterMoveInbox?' + params,
        //dataType: 'json',
        type: 'POST',
        cache: false,
        data: obj,
        //data: $("#frmFilterMoveInbox").serialize(),
        beforeSend: function () {
            if (!isSearch) {
                startAnimation();
            }
        },
        success: function (response) {

            $('.movements').html('');
            $('.movements').html($(response).find('.movements').html());
            //$('.div_so_movement').find("#filterimage").css("display", "block");
            closeFilters();
            isSearchStarted = false;
            if (isApiCallPending) {
                isApiCallPending = false;
                SearchSOAList();
            }
            PoliceMovementInboxInit();
        },
        error: function (xhr, status) {

            location.reload();
        },
        complete: function () {
            if (!isSearch) {
                stopAnimation();
            }
        }

    });
}
