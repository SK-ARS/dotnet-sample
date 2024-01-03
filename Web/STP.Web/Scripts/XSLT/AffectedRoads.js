function AffectedRoadsInit() {
    $('.road_select').eq(0).attr({ "checked": true });
    $('.roaddivclass').hide();
    $('.roaddivclass').eq(0).show();
    var ismyAffRoad = $('.form-check-input.road_select').attr("myaffroad");
    var elem = $('.road_select').eq(0);

    select_route_ar(elem.val(), ismyAffRoad);
    $('body').on('change', '.road_select', function (e) {
        e.preventDefault();
        var ismyAffRoad = $(this).attr("myaffroad");
        select_route_ar($(this).val(), ismyAffRoad);
    });
}

function select_route_ar(x, isMyAffRoad) {
    $('.roaddivclass').hide();
    $('#' + x).show();
    if (isMyAffRoad > 0) {
        if ($('#' + x + ' td').length > 0) {
            $('#NoAffectedRoads').hide();
        }
        else {
            $('#NoAffectedRoads').show();
        }
    }
}