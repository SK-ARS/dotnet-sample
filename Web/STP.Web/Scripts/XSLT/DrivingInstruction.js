$(document).ready(function () {
    $('body').on('click', '.viewmoredetails', function (e) {
        e.preventDefault();
        var count = $(this).attr("count");
        viewmoredetails(count);
    });
});

function viewmoredetails(count) {
    if (document.getElementById(count).style.display !== "none") {
        $("." + count).css("display", "none")
        document.getElementById('chevlon-up-icon1' + count).style.display = "none"
        document.getElementById('chevlon-down-icon1' + count).style.display = "block"
    }
    else {
        $("." + count).css("display", "block")
        document.getElementById('chevlon-up-icon1' + count).style.display = "block"
        document.getElementById('chevlon-down-icon1' + count).style.display = "none"
    }
}