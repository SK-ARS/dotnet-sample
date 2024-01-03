$(document).ready(function () {
    $('body').on('click', '.closeViewCaution', function () {
        closeViewCaution()
    });
});
function closeViewCaution() {
    $('#cautionDetails').modal('hide');
}