$("#EmailIdSORT").change(function () {
    $("#emailFaxValid").html("");
});
$("#FaxSORT").change(function () {
    $("#emailFaxValid").html("");
});

$('#HaulierContactName').keypress(function () {
    $('#err_Haulier_contact_name').text('');
});
$('#EmailIdSORT').keypress(function () {
    $('#emailFaxValid').text('');
});
$('#FaxSORT').keypress(function () {
    $('#emailFaxValid').text('');
});
$('#EmailId').keypress(function () {
    $('#emailFaxValid1').text('');
});
$('#Fax').keypress(function () {
    $('#emailFaxValid1').text('');
});
function addresschk() {
    $('#AddressLine_1, #AddressLine_2, #AddressLine_3, #AddressLine_4, #AddressLine_5').keypress(function () {
        $('#addrchk').text('');
    });
}

function OrgNameKeypress() {
    $('#OrgName').keypress(function () {
        $('#err_OrgName_exists').text('');
    });
}

function OrgCodeKeypress() {
    $('#OrgCode').keypress(function () {
        $('#err_OrgCode_exists').text('');
    });
}

