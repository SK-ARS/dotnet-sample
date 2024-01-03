var from = $('#hf_Ch_From').val();
$(document).ready(function () {
    $('body').on('click', '.btn-ch-close-history', function () {
        CloseHistoryPopUp();
    });
    $('body').on('click', '.btn-close-history', function () {
       
        onremove();
    });
    $('body').css('overflow-y', 'scroll');
    if (from == 'viewconstrainst') {
        $("#map").hide();
        $(".filter-icon").hide();
        $("#soa-portal-map").hide();
        $("#overlay").hide();
        $("#dialogue").hide();
        $("#route").hide();
    }
    else {
        $("#dialogue").show();
        $("#overlay").show();
    }
    window.addEventListener('click', function (event) {
        
        //if ($('#btnHistoryBack').contains(event.target) && from == 'viewconstrainst') {
        //    $("#overlay").show();
        //}

    });
});

function CloseHistoryPopUp() {
    if (from == 'viewconstrainst') {
        $("#vehicles").show();
        $("#displayConstraintHistory").html("");
        $("#dialogue").show();
        $("#overlay").show();
        $("#map").show();
        $(".filter-icon").show();
        $("#soa-portal-map").show();
        $("#route").show();
    }
    else {

        $("#dialogue").hide();
        window.location = '../Structures/MyStructures';
        $("#dialogue").show();
        $("#overlay").show();
    }
}

function CloseHistoryPopUp1() {
    window.location = '../Structures/MyStructures';
}
