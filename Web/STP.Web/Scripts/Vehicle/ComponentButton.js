$(document).ready(function () {
    $("#btn_cancel").on('click', Cancel);
});
function Cancel() {
    var isMovement = $('#IsMovement').val();
    if ($('#IsFromConfig').val() == '1' || isMovement == "True") {
        OnBackBtnClick();
    }
    else {
        location.reload();
    }
}
