$(document).ready(function () {
    $('body').on('click', '#supplyinfo', function (e) {
        e.preventDefault();
        ViewSupplimentaryDetails(this);
    });
});
function ViewSupplimentaryDetails() {
    if ($('#viewsuplimentaryinfo').length > 0) {
        if (document.getElementById('viewsuplimentaryinfo').style.display !== "none") {
            document.getElementById('viewsuplimentaryinfo').style.display = "none"
            document.getElementById('chevlon-up-icon_supp_info').style.display = "none"
            document.getElementById('chevlon-down-icon_supp_info').style.display = "block"
        }
        else {
            document.getElementById('viewsuplimentaryinfo').style.display = "block"
            document.getElementById('chevlon-up-icon_supp_info').style.display = "block"
            document.getElementById('chevlon-down-icon_supp_info').style.display = "none"
        }
    }
}
