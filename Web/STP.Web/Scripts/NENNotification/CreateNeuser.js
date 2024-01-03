let isvalid = true;
let mousedownHappened = false;
let flag = true;

function CreateNeUserInit() {
    if ($('#hf_mode').val() != "SORTSO") {
        selectedmenu('Admin');
    }
    if ($('#hf_AuthKey').val() != '') {
        document.getElementById("IsValidNe").checked = true;

    }
    document.getElementById("AuthKey").readOnly = true;

    $('#addrchk').text('');

}
$(document).ready(function () {

    $('body').on('click', '#SaveNeUser', function (e) {
        e.preventDefault();
        SaveNeUser(this);
    });

    $('body').on('click', '#btn_reset', function (e) {
        $("#HaulName").val('');
        $("#AuthKey").val('');
        $("#OrgNam").val('');
        $('#IsValidNe').attr('checked', false);
        $('#IsValidNe').prop('checked', false);
        $("#NeLimit").val('');
        $(".error").html("");

    });
    $('body').on('change', '#HaulName', function (e) {
        let NehaulName = $("#HaulName").val();
        let Encrypt = btoa(NehaulName);
        Encrypt = RandomEncry(Encrypt);
        Encrypt = Encrypt.substring(0, 20);
        $("#AuthKey").val(Encrypt);
        if ($("#AuthKey").val() != '') {
            document.getElementById("IsValidNe").checked = true;
        }
    });

});
function EncryDecry(e) {
    $('#err_required_haulname').text('');
    let NehaulName = $("#HaulName").val();
    let Encrypt = btoa(NehaulName);
    Encrypt = RandomEncry(Encrypt);
    Encrypt = Encrypt.substring(0, 20);
    $("#AuthKey").val(Encrypt);
    if ($("#AuthKey").val() != '') {
        document.getElementById("IsValidNe").checked = true;

    }
    let code = (e.keyCode ? e.keyCode : e.which);
    if (code == 13) { //Enter keycode
        SaveNeUser();
    }

}
function SaveNeUser() {
    startAnimation();
    let NehaulName = $("#HaulName").val();
    let AuthKey = $("#AuthKey").val();
    let OrgName = $("#OrgNam").val();
    let NenLimit = $("#NeLimit").val();
    let AuthKeyId = $('#hf_AuthKeyId').val();
    if (document.getElementById("IsValidNe").checked != true) {
        AuthKey = "";
    }
    if (AuthKeyId == 0) {
        var msg = "Non esdal haulier saved successfully";
    }
    else if (AuthKey == "") {
        var msg = "Non esdal haulier updated successfully";
    }
    else {
        var msg = "Non esdal haulier updated successfully and new authentication key is generated";
    }

    if (checkValidate(AuthKeyId)) {
        $.ajax({
            url: '../NENNotification/SaveNeUser',
            type: 'GET',
            cache: false,
            async: false,
            data: { Haulname: NehaulName, authKey: AuthKey, OrgName: OrgName, NeLimit: NenLimit, KeyId: AuthKeyId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result == 1) {
                    ShowSuccessModalPopup(msg, 'ReloadNeUsers');
                }
                else {
                    ShowWarningPopup('Saving Failed', 'ReloadNeUsers');

                }

            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();

            }
        });
    }

}
function checkValidate(IsCreate) {
    //IsCreate flag is 0 then we have to check duplication of haulier
    let NehaulName = $("#HaulName").val();
    let OrgName = $("#OrgNam").val();
    let NenLimit = $("#NeLimit").val();
    if (NehaulName == '') {
        $('#err_required_haulname').text('Haulier Name is required');
        $('#err_required_haulname').show();
        if (OrgName == '') {
            $('#err_required_orgname').text('Organisation Name is required');
            $('#err_required_orgname').show();
        }
        if (NenLimit != "") {
            if (isNaN(NenLimit)) {
                $('#err_number_nelimit').text('NEN limit must be a number');
                $('#err_number_nelimit').show();
            }

        }
        stopAnimation();
        return false;

    }
    if (OrgName == '') {
        $('#err_required_orgname').text('Organisation Name is required');
        $('#err_required_orgname').show();
        if (NenLimit != "") {
            if (isNaN(NenLimit)) {
                $('#err_number_nelimit').text('NEN limit must be a number');
                $('#err_number_nelimit').show();
            }

        }
        stopAnimation();
        return false;

    }
    if (NenLimit != "") {
        if (isNaN(NenLimit)) {
            $('#err_number_nelimit').text('NEN limit must be a number');
            $('#err_number_nelimit').show();
            stopAnimation();
            return false;

        }

    }
    if (IsCreate == 0) {
        let IsValHaul = HaulierValidation();
        if (IsValHaul > 0) {
            $('#err_required_haulname').text('Haulier Name already exists');
            $('#err_required_haulname').show();
            stopAnimation();
            return false;

        }

    }
    return true;
}
function HaulierValidation() {
    let NehaulName = $("#HaulName").val();
    let OrgName = $("#OrgNam").val();
    let IsVal = 0;
    $.ajax({
        url: '../NENNotification/HaulierValid',
        type: 'GET',
        cache: false,
        async: false,
        data: { haulname: NehaulName, orgname: OrgName },
        success: function (result) {
            IsVal = result;
        },

    });
    return IsVal;
}
function OrgKeypress() {
    $('#err_required_orgname').text('');
    let code = (e.keyCode ? e.keyCode : e.which);
    if (code == 13) { //Enter keycode
        SaveNeUser();
    }

}
function RandomEncry(possible) {   // function for making a random string to appent
    let text = "";

    for (let i = 0; i < 20; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    text = text.substring(0, 20);
    return text;
}
