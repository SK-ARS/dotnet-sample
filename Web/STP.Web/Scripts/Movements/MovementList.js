var isPlanMovement = $('#hf_IsPlanMovmentGlobal').length > 0 ? true : false;
var tableContainerElem = !isPlanMovement ? ".div_so_movement" : "#div_so_movement_list";
$(document).ready(function () {
    isPlanMovement = $('#hf_IsPlanMovmentGlobal').length > 0 ? true : false;
    tableContainerElem = !isPlanMovement ? ".div_so_movement" : "#div_so_movement_list";
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    $('#pageSizeValsort').val(pageSize);
    var esdalReference = $('#viewAdvHaulier').find('#ESDALReference').val();
    $('#ESDALReference').val(esdalReference);
    if (esdalReference == '') {
        $('#filterimageERN').hide();
    }

    $('.dropdown-toggle').dropdown();
    $('body').on('click', '#PlanMovement', function (e) {
        e.preventDefault();
        PlanMovement(this);
    });
    $('body').on('change', '#NeedsAttention', function (e) {
        ToggleNeedAttentionOutSide(this);
    });
    
    $('body').on('click', '#ClearHaulierAdvancedData', function () { window['ClearHaulierAdvancedData'](); });
    $('body').on('click', '#SearchHaulierList', function (e) {
        e.preventDefault();
        var isclear = $("#NeedsAttention").prop('checked');
        SearchHaulierList(false, pageSize, isclear);
    });

    $('body').on('keyup', '.txt-ml-esdal-ref-search', function () {
        SearchEsdalReference();
    });

    //Below function belongs to SOMOVEMENT LIST FILTER
});


jQuery(document).ready(function ($) {
    $(document).on('click', '.bs-canvas-overlay', function () {
        $('.bs-canvas-overlay').remove();
        $("#movementFilters").css('margin-right', "-760px");
        return false;
    });
});


function OnFailure(result) {
    closeFilters();
    stopAnimation();
}
function OnBegin(result) {
    startAnimation();
}
function SearchHaulierList(isSort = false, pageSize = 10, isclear = false) {
    var x = $('#filterimage').css('display');    
    if (($("#WorkInProgressNotification").prop('checked') == true || ($("#ProposedRoute").prop('checked') == true))) {
        $(tableContainerElem).find('#status').prop('checked', true);
    }

    if (($("#Notifications").prop('checked') == true) || ($("#ReceivedByHA").prop('checked') == true)) {
        $(tableContainerElem).find('#status').prop('checked', true);
    }
    if (($("#DeclinedApplications").prop('checked') == true) || ($("#WithdrawnApplications").prop('checked') == true)) {
        $(tableContainerElem).find('#status').prop('checked', true);
    }
    if (($("#SO").prop('checked') == true || ($("#VSO").prop('checked') == true))) {
        $(tableContainerElem).find('#so-movements').prop('checked', true);
    }
    if ($('div#movementFilters.so-movementlist-filter').length >1) {
        $('div#movementFilters.so-movementlist-filter:first').remove();
    var insideValue = $('#viewAdvHaulier').find('#ESDALReference').val();
    } else {
        var insideValue = $('#viewAdvHaulier').find('#ESDALReference').val();
    }
    var operator = "";
    var totalLength = $('#VehicleFilterDiv').find('.VehicleFilter').length;
    var filterIndex = 0;
    var objDimensionArr = [];
    $('#VehicleFilterDiv').find('.VehicleFilter').each(function () {
        filterIndex++;
        var SearchVal = $(this).find('.vehicletextbox').val();
        if (SearchVal != '' && SearchVal != undefined) {
            var objDimension = {};
            operator = $(this).find(".vehicleOperator option:selected").val() != "" && $(this).find(".vehicleOperator option:selected").val() != "0" && filterIndex < totalLength ? $(this).find(".vehicleOperator option:selected").text() : "";
            objDimension.DimensionType = $(this).find('select.haul-vehicle-dimension').val();
            objDimension.ComparisonOperator = $(this).find('select.haul-operator-count').val();
            objDimension.SearchValue = $(this).find('.vehicletextbox').val();
            objDimension.SearchValueBetween = $(this).find('.vehicletextbox1').val();
            objDimension.LogicalOperator = operator;
            objDimensionArr.push(objDimension);
        }
    });

    $('#AdvancedDimensionFilterString').val(JSON.stringify(objDimensionArr));
    $('#ESDALReference').val(insideValue);
    let params = isSort ? 'page=' + $('#pageNum').val() : "page=1";
    var vehicleClass = typeof hf_Vr1SoExistingPopUp != 'undefined' && hf_Vr1SoExistingPopUp ? $('#hf_VehicleClassNew').val() : 0;
    $.ajax({
        url: '/Movements/SetSearchData?' + params + '&' + "pageSize=" + pageSize + '&' + "planMovement=" + isPlanMovement + '&' + "PrevMovImport=" + true + '&' + "isclear=" + isclear + '&isExistVR1SoClass=' + vehicleClass,
        //dataType: 'json',
        type: 'POST',
        cache: false,
        data: $("#frmFilterMoveInbox").serialize(),
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            $(tableContainerElem).html('');
            $(tableContainerElem).html($(response).find(tableContainerElem).html());
            if (($("#WorkInProgressNotification").prop('checked') == true || ($("#ProposedRoute").prop('checked') == true))) {
                $(tableContainerElem).find('#status').prop('checked', true);
            }
            if (($("#Notifications").prop('checked') == true) || ($("#ReceivedByHA").prop('checked') == true)) {
                $(tableContainerElem).find('#status').prop('checked', true);
            }
            if (($("#WorkInProgressApplication").prop('checked') == true) || ($("#Submitted").prop('checked') == true)) {
                $(tableContainerElem).find('#status').prop('checked', true);
            }
            if (($("#Agreed").prop('checked') == true) || ($("#ApprovedVR1").prop('checked') == true)) {
                $(tableContainerElem).find('#status').prop('checked', true);
            }
            if (($("#DeclinedApplications").prop('checked') == true) || ($("#WithdrawnApplications").prop('checked') == true)) {
                $(tableContainerElem).find('#status').prop('checked', true);
            }
            if (($("#SO").prop('checked') == true || ($("#VSO").prop('checked') == true))) {
                $(tableContainerElem).find('#so-movements').prop('checked', true);
            }
            if (($("#STGO").prop('checked') == true || ($("#CandU").prop('checked') == true))) {
                $(tableContainerElem).find('#so-movements').prop('checked', true);
            }

            if (($("#Tracked").prop('checked') == true || ($("#StgoVR1").prop('checked') == true))) {
                $(tableContainerElem).find('#so-movements').prop('checked', true);
            }
            let esdalReference = $('#viewAdvHaulier').find('#ESDALReference').val();
            if (esdalReference != '') {
                $(tableContainerElem).find('#so-movements').prop('checked', true);
            }
            let myReference = $('#viewAdvHaulier').find('#MyReference').val();
            if (myReference != '') {
                $(tableContainerElem).find('#so-movements').prop('checked', true);
            }
            
            closeFilters();
        },
        error: function (xhr, status) {

            location.reload();
        },
        complete: function () {
            stopAnimation();

        }

    });
}
function PlanMovement() {
	localStorage.clear();
    window.location = '../Movements/CreateMovement';
}
function RedirectNotification(revisionId, movementId, versionId, haulierMnemonic, esdalReference, revisionNumber, versionNumber, appRevisionId, projectId, pageFlag) {
    var data = EncodedQueryString("revisionId=" + revisionId + ",movementId=" + movementId + ",versionId=" + versionId + ",hauliermnemonic=" + haulierMnemonic + ",esdalref=" + esdalReference + ",revisionno=" + revisionNumber + ",versionno=" + versionNumber + ",apprevid=" + appRevisionId + ",projecid=" + projectId + ",pageflag=" + pageFlag);
    var url = '../Application/ListSOMovements';
    window.location = url + data;
}
function ToggleNeedAttentionOutSide(_this) {
    
    if ($(_this).prop('checked') == true) {
        //$('#NeedsAttention').val(true);
        $('#viewHaulierStatus').find('#NeedsAttention').prop('checked', true);
        isclear = true;
    }
    else if ($(_this).prop('checked') == false) {
        //$('#NeedsAttention').val(false);
        $('#viewHaulierStatus').find('#NeedsAttention').prop('checked', false);
        $('#filterimage').hide();
        isclear = false;

    }
    var pageSize = $('#pageSizeSelect').val();
    SearchHaulierList(false,pageSize,isclear);


}
function ToggleNeedAttentionInside() {
    
    if ($('#viewHaulierStatus').find('#NeedsAttention').prop('checked') == true) {
        $('#NeedsAttention').val(true);
        $('#NeedsAttention').prop('checked', true);

    }
    else if ($('#viewHaulierStatus').find('#NeedsAttention').prop('checked') == false) {
        $('#NeedsAttention').val(false);
        $('#NeedsAttention').prop('checked', false);

    }

}
function SearchEsdalReference() {

    //if ($('#ESDALReference').val() == '') {
    //    ClearAdvancedData();
    //}
    //else {
    $('#viewAdvHaulier').find('#ESDALReference').val($('#ESDALReference').val());
    SearchHaulierListOutside();
    $('#filterimageERN').show();
    $('#filterimage').show();
    //}
}
var isSearchStarted = false;
var isApiCallPending = false;
function SearchHaulierListOutside() {

    if (isSearchStarted) {
        isApiCallPending = true;
        return false;
    }
    console.log('isSearchStarted', isSearchStarted);
    isSearchStarted = true;
    $.ajax({
        url: '/Movements/SetSearchData?page=1',
        type: 'POST',
        cache: false,
        async: true,
        data: $("#frmFilterMoveInbox").serialize(),
        beforeSend: function () {
            openContentLoader('.so-movements-table');
        },


        success: function (response) {
            $(tableContainerElem).html('');
            $(tableContainerElem).html($(response).find('#div_so_movement_list').html());

            isSearchStarted = false;
            if (isApiCallPending) {
                isApiCallPending = false;
                SearchHaulierListOutside();
            } else {
                closeContentLoader('.so-movements-table');
            }
        },
        error: function (xhr, status) {

            location.reload();
        },
        complete: function () {
        }

    });

}
$('body').on('change', '.haulier-pagination-container #pageSizeSelect', function () {
    
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    $('#pageSizeValsort').val(pageSize);
    SearchHaulierList(isSort = false, pageSize);
});
$('body').on('click', '.haulier-pagination-container a', function (e) {
    e.preventDefault();
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $('#pageSizeVal').length > 0 ? $('#pageSizeVal').val() : $('#pageSizeValsort').val();
    if (pageSize == "" || pageSize == undefined) 
        pageSize = $('.haulier-pagination-container #pageSizeSelect').val();
    SearchHaulierList(isSort = true, pageSize);
});
var sortTypeGlobal = 0;//desc
var sortOrderGlobal = 11;//esdalrefno
function HaulierSort(event, param) {
  
    var page = $('#pageNum').val(page);
    var pagesize = $('#pageSizeSelect').val();
    sortOrderGlobal = param;
    //0 -asec            //1-desc
    sortTypeGlobal = $(event).hasClass('sorting_desc') || $(event).find('.sorting').hasClass('sorting_desc') ? 3 : 0;
    $('#frmFilterMoveInbox #SortTypeValue').val(sortTypeGlobal);
    $('#frmFilterMoveInbox #SortOrderValue').val(sortOrderGlobal);
    $('#frmFilterMoveInbox #SortOrder').val(sortOrderGlobal);
    SearchHaulierList(isSort = true,pagesize);
}
$('body').on('click', '.so-movements-table #filterimage', function () { window['ClearHaulierAdvancedData'](); });
$('body').on('click', '#redirectntfctn', function (e) {
    e.preventDefault();
    var revisionId = $(this).data('revisionid');
    var movementId = $(this).data('movementid');
    var versionId = $(this).data('versionid');
    var haulierMnemonic = $(this).data('hauliermnemonic');
    var esdalReference = $(this).data('esdalreference');
    var revisionNumber = $(this).data('revisionnumber');
    var versionNumber = $(this).data('versionnumber');
    var appRevisionId = $(this).data('apprevisionrd');
    var projectId = $(this).data('projectid');
    var pageFlag = $(this).data('pageflag');

    RedirectNotification(revisionId, movementId, versionId, haulierMnemonic, esdalReference, revisionNumber, versionNumber, appRevisionId, projectId, pageFlag);
});