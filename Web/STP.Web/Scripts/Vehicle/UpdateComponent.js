
$(document).ready(function () {
    $('.btnOpenAxlePopUp').remove();
    SelectMenu(5);
    Comp_AxleInit(0);
    suppressKey();
    IterateThroughText();
    ShowFeet();
    AxleValidationCalculation();
    $('body').on('click', '#btn_comp_edit', function (e) {
        e.preventDefault();
        UpdateComponent(this);
    });
    $("#btn_Comp_Update_back").on('click', CancelUpdate);
    $('.tbl_registration').css('width', '46%');
    $('.axle-table-contents-container').css('width', '90%');

});
function CancelUpdate() {
    window.history.back();
}
$(function () {
    $(".axledrop").change(function () {

        $("#btn_cancel").show();
        $('#componentBtn').show();
        var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
        var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
        //if (configurableAxles == 'True') {

            loadAxles(numberOfAxles);

        //}
    });
});

function UpdateComponent(_this) {
    var componentId = $('#Component_Id').val();

    var component = $('#' + componentId);
    var unit = $('#UnitValue').val();
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = 'True';
    }
    //var vd = validation(componentId);
    var axleweightsum = 0;
    $('.comp').find('.axleweight').each(function () {
        var _thisVal = $(this).val();
        axleweightsum = axleweightsum + parseFloat(_thisVal);
    });
    var result = checkaxle_weight(axleweightsum);
    if (result) {
        UpdateData(component);
    }

    return false;
}