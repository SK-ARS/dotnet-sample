$(document).ready(function () {
    if (($("#Accepted").prop('checked') == true || ($("#OpenedByUser").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageStatus').show();
        //$('#filterimage').show();
    }
    if (($("#Rejected").prop('checked') == true || ($("#Withdrawn").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageStatus').show();
        //$('#filterimage').show();
    }
    if (($("#Unspecified").prop('checked') == true || ($("#Declined").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageStatus').show();
        //$('#filterimage').show();
    }
    if (($("#UnopenedByUser").prop('checked') == true || ($("#ImminentMovement").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageStatus').show();
        //$('#filterimage').show();
    }
    if (($("#UnopenedByOrganisation").prop('checked') == true || ($("#AssignedToMe").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageStatus').show();
        //$('#filterimage').show();
    }
    if ($("#UnderAssessmentbyOtherUser").prop('checked') == true) {


        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }


    if (($("#SO").prop('checked') == true || ($("#CAndU").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);

        //$('#filterimageMsgType').show();
        //$('#filterimage').show();
    }
    if (($("#STGOVR1").prop('checked') == true || ($("Tracked").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageMsgType').show();
        //$('#filterimage').show();
    }
    if (($("#STGO").prop('checked') == true || ($("VSO").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
        //$('#filterimageMsgType').show();
        //$('#filterimage').show();
    }
    var esdalReference = $('#viewMovement').find('#ESDALReference').val();
    if (esdalReference != "") {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
        //$('#filterimagePoliceERN').show();
        //$('#filterimage').show();
    }
    if (esdalReference == "") {
        //$('#filterimagePoliceERN').hide();
    }
    var hauliername = $('#viewMovement').find('#HaulierName').val();
    if (hauliername != "") {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
        //$('#filterimageOrg').show();
        //$('#filterimage').show();
    }
    if (hauliername == "") {
        //$('#filterimageOrg').hide();
    }
    if ($("#MovementDate").prop('checked') == true) {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
        //$('#filterimagemovement').show();
        //$('#filterimage').show();
    }
    if ($("#ReceiveDate").prop('checked') == true) {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
        //$('#filterimageRecieved').show();
        //$('#filterimage').show();
    }


    if ($('#hf_ChkUnopenedByUser').val() == "0") {
        $("#UnopenedByUser").attr('checked', false);
        $('#viewstatus').find('#UnopenedByUser').attr('checked', false);
    }
    else {
        $("#UnopenedByUser").attr('checked', true);
        $('#viewstatus').find('#UnopenedByUser').attr('checked', true);
    }

    $('#ESDALReference').val($('#viewMovement').find('#ESDALReference').val());

    $('body').on('click', '#imgopenfilters', function () { window['openFilters'](); });

    $('body').on('click', '#filterimage', function () { window['ClearAdvanced'](0); });

});


function BackRevStructSummary(structId) {
    window.location.href = "../Structures/ReviewSummary?structureId=" + structId + "";
}


function ToggleUnopenedByUserOutSide() {
    if ($("#UnopenedByUser").prop('checked') == true) {
        $('#UnopenedByUser').val(true);
        $('#viewstatus').find('#UnopenedByUser').prop('checked', true);

    }
    else if ($("#UnopenedByUser").prop('checked') == false) {
        $('#UnopenedByUser').val(false);
        $('#viewstatus').find('#UnopenedByUser').prop('checked', false);
    }
    SearchSOAList();
}
function ToggleUnopenedByUserInside() {
    if ($('#viewstatus').find('#UnopenedByUser').prop('checked') == true) {
        $('#UnopenedByUser').val(true);
        $('#UnopenedByUser').prop('checked', true);
    }
    else if ($('#viewstatus').find('#UnopenedByUser').prop('checked') == false) {
        $('#UnopenedByUser').val(false);
        $('#UnopenedByUser').prop('checked', false);
    }

}
var timeout = null;
function SearchSOAEsdalReference() {
    //if ($('#ESDALReference').val() == '') {
    //    ClearAdvanced(1);
    //}
    //else {
    //    $('#viewMovement').find('#ESDALReference').val($('#ESDALReference').val());
    //}
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
$('body').on('dragstart', '.table_folder_item', function (e) {
    var movementRevisionId = $(this).data("revisionid");
    var movementVersionId = $(this).data("versionid");
    var notificationId = $(this).data("notificationid");
    var movementType = $(this).data("movementtype");
    var projectId = $(this).data("projectid");
    dragStart(e, movementRevisionId, movementVersionId, notificationId, movementType, projectId);
});
$('body').on('dragend', '.table_folder_item', function (e) {
    dragEnd(e);
});
