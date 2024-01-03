$(document).ready(function () {
    
});

function Vehcile_VehicleComponentDetailsInit() {
    ShowHideHeaderTyreSpaceVehicleComponent();
    HeaderTyreSpaceCount();
}

function HeaderTyreSpaceCount() {

    var grtValue = 0;
    $('#vehicle-table table tr').each(function () {
        var _thisVal = parseInt($(this).find('.wheel_space').length);
        if (_thisVal > grtValue) {
            grtValue = _thisVal;
        }
    });
    $('.headgrad1').remove();
    for (var i = 1; i <= grtValue; i++) {
        $('.sub').append('<th class="headgrad1">' + i + '</th>');
        $('.headgrad2').show();
    }
    $('.headgrad2').attr('colspan', grtValue);
}

//function to show tyre spacing header
function ShowHideHeaderTyreSpaceVehicleComponent() {
    if ($('.wheel_space').length == 0) {
        $('.headgrad2').hide();
        //$('.sub1').hide();
    }
    else {
        $('.headgrad2').show();
        $('.sub1').show();
    }

    if ($('.tyre_size').length == 0) {
        $('.headgrad_tyreSize').hide();
    }
    else {
        $('.headgrad_tyreSize').show();
    }

}

