$(document).ready(function () {
    stopAnimation();
    $("#overlay").show();
    $("#closeSpan").click(function () {
        $(".hide_div").hide();
    });

    $('body').on('click', '.btn-close-contraints-popup', function () {
        closeContraintsPopup();
    });
});


function closeContraintsPopup() {
    $('#overlay').hide();
    resetdialogue();
}