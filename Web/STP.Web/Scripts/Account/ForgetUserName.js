//window.onloadstart = HideTopBottomLayout();
//window.onload = HideTopBottomLayout();
//$(document).ready(function () {
//    HideTopBottomLayout();
//});

function HideTopBottomLayout() {
    $("#navbar").hide();
    $("#footerdiv").hide();
}
$('body').on('click', '.btn-cancel', function () {
    location.href = '../Account/Login';
});