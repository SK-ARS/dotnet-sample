$(document).ready(function () {
    $('body').on('click', '.viewststus', function (e) {
        e.preventDefault();
        viewStatus(this);
    });
    $('body').on('click', '.viewmovement', function (e) {
        e.preventDefault();
        viewMovement(this);
    });
    $('body').on('click', '#filters #table-head', function (e) {
        e.preventDefault();
        viewStatus(this);
    });
});
// showing Form Post-method
function FilterSuccess(result) {

    $('.div_so_movement').html('');
    $('.div_so_movement').html(result);
    if (($("#Accepted").prop('checked') == true || ($("#Opened").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#Rejected").prop('checked') == true || ($("#Withdrawn").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#Unspecified").prop('checked') == true || ($("#Declined").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#Unopened").prop('checked') == true || ($("#ImminentMovement").prop('checked') == true))) {
        $('.div_so_movement').find('#portal-table').prop('checked', true);
    }
    if (($("#UnderAssessmentbyOtherUser").prop('checked') == true || ($("#AssignedToMe").prop('checked') == true))) {
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
        $('#filterimagePoliceERN').hide();
    }
    var hauliername = $('#viewMovement').find('#HaulierName').val();
    if (hauliername != "") {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    if (hauliername == "") {
        $('#filterimageOrg').hide();
    }
    if ($("#MovementDate").prop('checked') == true) {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    if ($("#ReceiveDate").prop('checked') == true) {
        $('.div_so_movement').find('#viewMovement').prop('checked', true);
    }
    // document.getElementById('chevlon-down-icon').style.display = "block"
    closeFilters();
    stopAnimation();
}
function OnFailure(result) {
    closeFilters();
    stopAnimation();
}
function OnBegin(result) {
    startAnimation();
}
function ClearAdvanced(flag = 0, resetData = 1) {
    $('#outsideFilterMoveInboxSOA').find('#ESDALReference').val('');
    $('#GrossWeightAndOr').prop("checked", "checked");
    $('#OverallWidthAndOr').prop("checked", "checked");
    $('#OverallLengthAndOr').prop("checked", "checked");
    $('#RigidLengthAndOr').prop("checked", "checked");
    $('#HeightAndOr').prop("checked", "checked");
    var _advFilter = $('#filter_movement');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:radio').eq(0).prop('checked', true);
    _advFilter.find('input:checkbox:not("#Unopened")').prop('checked', false);
    if (flag == 0) {
        _advFilter.find('#Unopened').prop('checked', false);
        $('#UnopenedToggle').prop('checked', false);
    }
    _advFilter.find('#ObjectDelegationList').val('');
    _advFilter.find('option:selected').prop("selected", false);
    $(".VehicleFilter").slice(1).remove();
    $('#AddOption').show();
    $('#RemoveOption').hide();
    $('.gross_weight1').hide();
    $('.gross_weightUnit').text('kg');
    $('.gross_weight1Unit').text('kg');
    if (resetData == 1) {
        ResetData();
    }
}
function ResetData() {
    $.ajax({
        url: '../Movements/ClearInboxFilter',
        type: 'POST',
        success: function (data) {
            SearchSOAList();
        },
        error: function (xhr, status) {
            location.reload();
        }
    });
}
// showing filter-status in side-nav
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
// showing filter-movements in side-nav
function viewMovement() {
    if (document.getElementById('viewMovement').style.display !== "none") {
        document.getElementById('viewMovement').style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById('viewMovement').style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}
// showing filter-advance search in side-nav
function viewAdvanceSearch() {

    if (document.getElementById('div_movement_inbox_filter_advanced').style.display !== "none") {
        document.getElementById('div_movement_inbox_filter_advanced').style.display = "none"
        document.getElementById('chevlon-up-icon2').style.display = "none"
        document.getElementById('chevlon-down-icon2').style.display = "block"
    }
    else {
        document.getElementById('div_movement_inbox_filter_advanced').style.display = "block"
        document.getElementById('chevlon-up-icon2').style.display = "block"
        document.getElementById('chevlon-down-icon2').style.display = "none"
    }
}
