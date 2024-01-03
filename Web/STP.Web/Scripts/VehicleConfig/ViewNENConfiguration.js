$(document).ready(function () {
    ShowHideHeaderTyreSpaceViewNen();
    HeaderTyreSpaceCount();
    HideAxleSpacingForLastComp();
    $('body').on('click', '.vehicleNen', function (e) {
        VehicleNENDetails();
    });
});
/* SelectMenu(5);*/

function HeaderTyreSpaceCount() {
    var grtValue = 0;
    $('#vehicle-table table tr').each(function () {
        var _thisVal = parseInt($(this).find('.wheel_space').length);
        if (_thisVal > grtValue) {
            grtValue = _thisVal;
        }
    });
    for (var i = 1; i <= grtValue; i++) {
        $('.tyreSpaceCnt').append('<th style="width: 10%;">' + i + '</th>');
    }
    $('.headgrad2').attr('colspan', grtValue);
}

//function to show tyre spacing header
function ShowHideHeaderTyreSpaceViewNen() {
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
function ViewBackbutton() {
    window.history.back();
}
function HideAxleSpacingForLastComp() {

    if ($('#CompoCount').val() == 1) {
        var axl = $("div[id='view-Axle Spacing To Following']");
        axl.hide();
    }
    else if ($('#CompoCount').val() > 1) {
        var cnt = $('#CompoCount').val() - 1;
        var divcomp = $('div#viewCompDetail:eq(' + cnt + ')');
        var axl = $(divcomp).find($("div[id='view-Axle Spacing To Following']"));
        axl.hide();
    }
}
