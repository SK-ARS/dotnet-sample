var orgMinWeight = $('#hf_OrgMinWeight').val();
var orgMaxWeight = $('#hf_OrgMaxWeight').val();
var orgMinSV = $('#hf_OrgMinSV').val();
var orgMaxSV = $('#hf_OrgMaxSV').val();
var structid = $('#hf_structureid').val();

function ConfigureBandingsInit() {
    orgMinWeight = $('#hf_OrgMinWeight').val();
    orgMaxWeight = $('#hf_OrgMaxWeight').val();
    orgMinSV = $('#hf_OrgMinSV').val();
    orgMaxSV = $('#hf_OrgMaxSV').val();
    structid = $('#hf_structureid').val();

    if ($('#hf_Helpdest_redirect').val() == "true") {
        $("#ESDALDefaultWeight").attr("disabled", "disabled");
        $("#OrgDefaultWeight").attr("disabled", "disabled");
        $("#ESDALDefaultSV").attr("disabled", "disabled");
        $("#OrgDefaultSV").attr("disabled", "disabled");
    }

    $('#StructName').val($('#StructureName').val());


    if ((orgMinWeight != null && orgMinWeight != "") || (orgMaxWeight != null && orgMaxWeight != "")) {
        $('#OrgMinWeight').attr('disabled', false);
        $('#OrgMaxWeight').attr('disabled', false);
        $("#OrgDefaultWeight").attr("checked", true);
    }
    else {
        $('#OrgMinWeight').attr('disabled', true);
        $('#OrgMaxWeight').attr('disabled', true);
        $("#ESDALDefaultWeight").attr("checked", true);
    }

    if ((orgMinSV != null && orgMinSV != "") || (orgMaxSV != null && orgMaxSV != "")) {
        $('#OrgMinSV').attr('disabled', false);
        $('#OrgMaxSV').attr('disabled', false);
        $("#OrgDefaultSV").attr("checked", true);

    }
    else {
        $('#OrgMinSV').attr('disabled', true);
        $('#OrgMaxSV').attr('disabled', true);
        $("#ESDALDefaultSV").attr("checked", true);
    }

    if ($('#hf_msg').val() == "1") {
        if ($("#ESDALDefaultWeight").is(":checked")) {

            $('#OrgMinWeight').attr('disabled', true);
            $('#OrgMaxWeight').attr('disabled', true);
        }
        else if ($("#OrgDefaultWeight").is(":checked")) {
            $('#OrgMinWeight').attr('disabled', false);
            $('#OrgMaxWeight').attr('disabled', false);
        }
        if ($("#ESDALDefaultSV").is(":checked")) {
            $('#OrgMinSV').attr('disabled', true);
            $('#OrgMaxSV').attr('disabled', true);
        }
        else if ($("#OrgDefaultSV").is(":checked")) {
            $('#OrgMinSV').attr('disabled', false);
            $('#OrgMaxSV').attr('disabled', false);
        }
        showWarningPopDialog('Default bandings data saved succesfully', 'Ok', '', 'BackToPreviousPage', '', 1, 'info');

    }
}

$(document).ready(function () {
    $('body').on('click', '#bndngsavebtn', function (e) {
        e.preventDefault();
        bandingSave(this);
    });

    $('body').on('click', '.bcancelbtnbndng', function (e) {
        e.preventDefault();
        Back(this);
    });

    $('body').on('click', '#ESDALDefaultWeight', function (e) {
        $('#OrgMinWeight').attr('disabled', true);
        $('#OrgMaxWeight').attr('disabled', true);
        $('#OrgMinWeight').val('');
        $('#OrgMaxWeight').val('');
    });

    $('body').on('click', '#OrgDefaultWeight', function (e) {
        $('#OrgMinWeight').attr('disabled', false);
        $('#OrgMaxWeight').attr('disabled', false);
    });

    $('body').on('click', '#ESDALDefaultSV', function (e) {
        $('#OrgMinSV').attr('disabled', true);
        $('#OrgMaxSV').attr('disabled', true);
        $('#OrgMinSV').val('');
        $('#OrgMaxSV').val('');
    });

    $('body').on('click', '#OrgDefaultSV', function (e) {

        $('#OrgMinSV').attr('disabled', false);
        $('#OrgMaxSV').attr('disabled', false);
    });
    $('body').on('mouseenter', '#OrgMinSV', function (e) {
        $("#errordate1").hide();
    });
    $('body').on('mouseenter', '#OrgMaxSV', function (e) {
        $("#errordate1").hide();
    });
    $('body').on('mouseenter', '#OrgMinWeight', function (e) {
        $("#errordate").hide();
    });
    $('body').on('mouseenter', '#OrgMaxWeight', function (e) {
        $("#errordate").hide();
    });

});

function Back() {
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structid);
};
function bandingSave() {
    var flagcheck = true;
    var OrgId = $('#OrgId').val();
    var OrgMinWt = $('#OrgMinWeight').val();
    var OrgMaxWt = $('#OrgMaxWeight').val();
    var OrgMinSV = $('#OrgMinSV').val();
    var OrgMaxSV = $('#OrgMaxSV').val();
    var check = $('#OrgDefaultWeight').val();
    var data = document.getElementById('OrgDefaultWeight').checked;
    var data1 = document.getElementById('OrgDefaultSV').checked;

    if (data1 == true && OrgMinSV == "" && OrgMaxSV == "") {
        $('#errordate1').show();
        flagcheck = false;
    }
    if (data == true && OrgMinWt == "" && OrgMaxWt == "") {
        $('#errordate').show();
        flagcheck = false;
    }

    if (document.getElementById('ESDALDefaultSV').checked && document.getElementById('ESDALDefaultWeight').checked) {
        $('#errordate').hide();
        $('#errordate1').hide();
        flagcheck = true;
    }
    var data = document.getElementById('OrgDefaultWeight').checked;
    var data1 = document.getElementById('OrgDefaultSV').checked;

    if (data == false) {
        $('#errordate').hide();

    }
    if (data1 == false) {
        $('#errordate1').hide();

    }
    if (flagcheck == false) {
        return flase;
    }
    else
        $.ajax({
            url: '../Structures/UpdateConfigureBandings',
            type: 'POST',
            dataType: 'json',
            async: false,
            data: { OrgId: OrgId, OrgMinWeight: OrgMinWt, OrgMaxWeight: OrgMaxWt, OrgMinSV: OrgMinSV, OrgMaxSV: OrgMaxSV },
            success: function () {
                $('#cautionPopup').modal('show');
                $("#dialogue").html('');
                $('#overlay').hide();
                resetdialogue();
                showWarningPopDialog('Default bandings data saved succesfully', 'Ok', '', 'Back', '', 1, 'info');
                addscroll();
                stopAnimation();

            },
            error: function () {

                location.reload();
            },
            complete: function () {
            }
        });
}
function close() {
    $('#cautionPopup').modal('hide');
}
function isNonAlphaNum(evt) { // Validation For Numbers and dots only

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var charStr = String.fromCharCode(charCode);
    var rgx = /^[0-9]*\.?[0-9]*$/;

    if (!(rgx.test(charStr))) {

        $('#errdesc').show();
        return false;
    }
    else {
        $('#errdesc').hide();
        return true;
    }

}
function isNonAlphaNumber(evt) {

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var charStr = String.fromCharCode(charCode);
    var rgx = /^[0-9]*\.?[0-9]*$/;

    if (!(rgx.test(charStr))) {

        $('#errordesc').show();
        return false;
    }
    else {
        $('#errordesc').hide();
        return true;
    }

}
