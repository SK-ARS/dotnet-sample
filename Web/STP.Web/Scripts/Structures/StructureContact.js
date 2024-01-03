function StructureContactInit() {
    $('#overlay').show();
    $("#dialogue").show();
    $("#dialogue").css("display", "block");
}
$(document).ready(function () {
    $('body').on('click', '.structure-contact #closcontact', function (e) {
        CloseContactDetailsStructureContact();
    });
});
function CloseContactDetailsStructureContact() {
    $('#exampleModalCenter22').hide();
    $('#overlay').hide();
    $("#dialogue").hide();
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    addscroll();
    resetdialogue();
    stopAnimation();
}