$(document).ready(function () {
    
    if (($("#Accepted").prop('checked') == true || ($("#Opened").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#Rejected").prop('checked') == true || ($("#Withdrawn").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#UnderAssessment").prop('checked') == true || ($("#Declined").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#Unopened").prop('checked') == true || ($("#ImminentMovement").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#UnderAssessmentbyOtherUser").prop('checked') == true || ($("#UnderAssessmentbyMe").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#SO").prop('checked') == true || ($("#CAndU").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#STGOVR1").prop('checked') == true || ($("Tracked").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#STGO").prop('checked') == true || ($("VSO").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    var esdalReference = $('#viewMovement').find('#ESDALReference').val();
    if (esdalReference != "") {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    if (esdalReference == "") {
    }
    var hauliername = $('#viewMovement').find('#HaulierName').val();
    if (hauliername != "") {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    if (hauliername == "") {
    }
    if ($("#MovementDate").prop('checked') == true) {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    if ($("#ReceiveDate").prop('checked') == true) {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    PoliceMovementInboxInit();
    
    $('#ESDALReference').val($('#viewMovement').find('#ESDALReference').val());

    $('body').on('click', '#filterimage', function () { window['ClearAdvanced'](0); });

});
function PoliceMovementInboxInit() {
    $('html, body').animate({
        scrollTop: ($('.esdal-table').position().top)
    }, 1000);
}

function BackRevStructSummary(structId) {
    window.location.href = "../Structures/ReviewSummary?structureId=" + structId + "";
}

function ToggleUnopened() {
    if ($("#UnopenedToggle").prop('checked') == true) {
        $('#UnopenedToggle').val(true);
        $('#Unopened').val(true);
        $('#viewstatus').find('#Unopened').prop('checked', true);
    }
    else if ($("#UnopenedToggle").prop('checked') == false) {
        $('#UnopenedToggle').val(true);
        $('#Unopened').val(true);
        $('#viewstatus').find('#Unopened').prop('checked', false);
    }
    SearchSOAList();
}

var timeout = null;
function SearchSOAEsdalReference() {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
        console.log('searching.....');
        $('#viewMovement').find('#ESDALReference').val($('#ESDALReference').val());
        SearchSOAList(true);
    }, 1000);
}
function SearchSOAListOutside(flag) {
    SearchSOAList();
}

var sortTypeGlobal = 1;//1-desc
var sortOrderGlobal = 6;//esdalrefno
function HaulierSortPolice(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#frmFilterMoveInbox #SortTypeValue').val(sortTypeGlobal);
    $('#frmFilterMoveInbox #SortOrderValue').val(sortOrderGlobal);
    SearchSOAList(false, isSort = true);
}
$('body').on('click', '#btnback', function (e) {
    var structId = $(this).data("structid");

    BackRevStructSummary(structId);
});
$('body').on('click', '.related-inbox-filter-image', function () {
    window['ClearAdvanced'](0);
});