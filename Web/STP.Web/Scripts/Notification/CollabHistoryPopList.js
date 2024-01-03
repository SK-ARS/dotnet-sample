$(document).ready(function () {
    $('body').on('click', '#span-close', function (e) {
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
        $("#dialogue").hide();
        $("#overlay").hide();
        addscroll();
        resetdialogue();
    });
});
function SpanClose() {
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    $('#overlay').hide();
    $('#dialogue').hide();
    addscroll();
    resetdialogue();
    DisplayCollabStatus();
}