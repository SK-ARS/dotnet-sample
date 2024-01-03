
$(document).ready(function () {
    selectedmenu('Movements'); // for selected menu
    $('#div_ListConstraints').find('.route_select').eq(0).attr({ "checked": true });
});

function ViewConstraints(code) {

    var constraintID = 0;
    $.ajax({
        url: '../RouteAssessment/GetConstraintId',
        type: 'POST',
        cache: false,
        async: false,
        data: { ConstraintCode: code },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            //alert(data.result.check);
            constraintID = data.result.check;
            var randomNumber = Math.random();

            $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + constraintID + '&flageditmode=false' + '&random=' + randomNumber);
            //removescroll();
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();

        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

            //stopAnimation();
        }
    });



}

