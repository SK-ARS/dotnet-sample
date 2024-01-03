function RoadContactInit() {
    Resize_PopUp(650);
    addscroll();
}
$(document).ready(function () {
    $('body').on('click', '#span-close', function (e) {
        closeSpan();
    });
    $('body').on('click', '#closingcontact', function (e) {
        CloseContact();
    });
});

function closeSpan() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function CloseContact() {
    $('#exampleModalCenter22').hide();
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}

