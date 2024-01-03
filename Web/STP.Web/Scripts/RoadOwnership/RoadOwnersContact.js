function RoadOwnersContactInit() {
    Resize_PopUp(650);
    addscroll();
}
$(document).ready(function () {
    $('body').on('click', '.road-owner-contact #span-close', function (e) {
        closeSpanRoadOwnersContact();
    });
    $('body').on('click', '.road-owner-contact #IDclose', function (e) {
        CloseContactRoadOwnersContact();
    });
});

function closeSpanRoadOwnersContact() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function CloseContactRoadOwnersContact() {
    $('#exampleModalCenter22').hide();
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
