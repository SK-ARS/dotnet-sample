$(document).ready(function () {
   
    if ($('#haulierportal').val() == "True" || $('#haulierportal').val() == "true") {
        SelectMenu(7);
    }
    else {
        SelectMenu(5);
    }
    //$("#btnbackprev").click(BackToPreviousPage);
});
$('body').on('click', '#btnbackprev', function (e) {
    e.preventDefault();
    BackToPreviousPage();
});
function BackToPreviousPage() {

    window.history.back();
}
