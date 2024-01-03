$(document).ready(function () {
    $('#ComponentCount').val(@compCount);
    if (@compCount > 3) {
        $('#scroll-btns').show();
    }
    else {
        $('#scroll-btns').hide();
    }
    if (@compType1== 234002 && @compType2== 234005) {
        $('#divBoatMast').show();
        $('#BoatMastException').prop('checked', false);
    }
    else {
        $('#BoatMastException').val('false');
        $('#BoatMastException').prop('checked', false);
        $('#divBoatMast').hide();
    }
    if ($('#ComponentCount').val() == 0) {
        $('#vehicle_back_btn').hide();
        $('#vehicle_next_btn').hide();
    }
    else {
        $('#vehicle_next_btn').show();
        $('#vehicle_back_btn').hide();
    }
});
