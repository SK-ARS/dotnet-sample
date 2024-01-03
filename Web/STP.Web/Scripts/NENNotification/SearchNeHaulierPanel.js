$("#SearchString").val($("#hdnSearchString").val());
if ($("#hdnSearchValid").val() == 1) {
    $('#ShowValid').prop('checked', true);
}
else {
    $('#ShowValid').prop('checked', false);
}
function ShowValidNe() {
    if ($("#Isval").is(':checked')) {
        $('#Isval').val(1);

    } else {
        $('#Isval').val(0);
    }
}

