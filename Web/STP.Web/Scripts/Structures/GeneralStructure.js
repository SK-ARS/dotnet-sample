var hf_StructureId = $('#hf_StructureId').val();
var hf_StructureClass = $('#hf_StructureClass').val();

function GeneralStructureInit() {
    hf_StructureId = $('#hf_StructureId').val();
    hf_StructureClass = $('#hf_StructureClass').val();
    if ($('#hf_Helpdest_redirect').val() == "true") {
        $("#idStructureName").attr("readonly", "readonly");
        $("#idStructureAlternateNameOne").attr("readonly", "readonly");
        $("#idStructureAlternateNameTwo").attr("readonly", "readonly");
        $("#idStructureAlternateNameThree").attr("readonly", "readonly");
        $("#idStructureKey").attr("readonly", "readonly");
        $("#idDescription").attr("readonly", "readonly");
        $("#idNotes").attr("readonly", "readonly");
        $("#CategoryUserDefined").attr("readonly", "readonly");
        $("#TypeUserDefined").attr("readonly", "readonly");
        $("#TypeOneUserDefined").attr("readonly", "readonly");
        $("#TypeTwoUserDefined").attr("readonly", "readonly");
        $("#idLength").attr("readonly", "readonly");
        $("#structCategory").attr("disabled", "disabled");
        $("#structType").attr("disabled", "disabled");
        $("#structTypeOne").attr("disabled", "disabled");
        $("#structTypeTwo").attr("disabled", "disabled");
    }

    $("#structCategory").val($("#hdnCategory").val());
    $("#structType").val($("#hdnType").val());
    $("#structTypeOne").val($("#hdnTypeOne").val());
    $("#structTypeTwo").val($("#hdnTypeTwo").val());


    $("#hdnCategoryName").val($("#structCategory option:selected").text());
    $("#hdnTypeName").val($("#structType option:selected").text());
    $("#hdnTypeOneName").val($("#structTypeOne option:selected").text());
    $("#hdnTypeTwoName").val($("#structTypeTwo option:selected").text());

    if ($("#hdnCategory").val() == "999999999") {
        $("#CategoryUserDefined").prop("disabled", false);
    }
    else {
        $("#CategoryUserDefined").prop("disabled", true);
    }

    if ($("#hdnType").val() == "999999999") {
        $("#TypeUserDefined").prop("disabled", false);
    }
    else {
        $("#TypeUserDefined").prop("disabled", true);
    }

    if ($("#hdnTypeOne").val() == "999999999") {
        $("#TypeOneUserDefined").prop("disabled", false);
    }
    else {
        $("#TypeOneUserDefined").prop("disabled", true);
    }

    if ($("#hdnTypeTwo").val() == "999999999") {
        $("#TypeTwoUserDefined").prop("disabled", false);
    }
    else {
        $("#TypeTwoUserDefined").prop("disabled", true);
    }


    if (hf_StructureClass.trim().toLowerCase() == "level crossing") {
        $("#CategoryUserDefined, #TypeUserDefined, #TypeOneUserDefined, #TypeTwoUserDefined").hide();
    }

    $("#structCategory, #structType ,#structTypeOne, #structTypeTwo").find("option").each(function () {
        var text = $(this).text().toLowerCase().replace(/\b\w/g, function (char) {
            return char.toUpperCase();
        });
        $(this).text(text);
    });
}
$(document).ready(function () {
    //For Document status viewer
    //------------------------------#6029 Unable to save structure details------------------------------------------------------------------------------
    $('body').on('click', '#btn_sumbit', function (e) {
        e.preventDefault();
        var firstAltName = $('#idStructureAlternateNameOne').val();
        var secondAltName = $('#idStructureAlternateNameTwo').val();
        var thirdAltName = $('#idStructureAlternateNameThree').val();
        if (firstAltName.search("<") != -1 || firstAltName.search(">") != -1) {
            firstAltName = firstAltName.replace('<', '_*').replace('>', '*_');
            $('#idStructureAlternateNameOne').val(firstAltName);
        }
        if (secondAltName.search("<") != -1 || secondAltName.search(">") != -1) {
            secondAltName = secondAltName.replace('<', '_*').replace('>', '*_');
            $('#idStructureAlternateNameTwo').val(secondAltName);
        }
        if (thirdAltName.search("<") != -1 || thirdAltName.search(">") != -1) {
            thirdAltName = thirdAltName.replace('<', '_*').replace('>', '*_');
            $('#idStructureAlternateNameThree').val(thirdAltName);
        }
        saveManageGeneralDetails(this);
    });
    //------------------------------#6029 Unable to save structure details------------------------------------------------------------------------------

    $('body').on('keyup', '#idLength', function (e) {
        if ($(this).val() > 9999) {
            $(this).val($(this).val().slice(0, -1));
        }
    });

});

function ShowReviewSummary() {
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + hf_StructureId);
}

function saveManageGeneralDetails() {

    $.ajax({
        url: '../Structures/EditStructureGeneralDetails',
        dataType: 'json',
        type: 'POST',
        data: $("#DimensionInfo").serialize(),
        success: function (result) {
            if (result == true) {
                ShowSuccessModalPopup("Structure general details saved successfully", "ShowReviewSummary")
            }
            else {
                ShowErrorPopup("Structure general details save failed")
            }
        },
        error: function (xhr, status) {
        }
    });
}

$('body').on('change', '#structCategory', function (e) {
    $("#hdnCategoryName").val($("#structCategory option:selected").text());
    if ($("#structCategory").val() == "999999999") {
        $("#CategoryUserDefined").prop("disabled", false);
    }
    else {
        $("#CategoryUserDefined").val("");
        $("#CategoryUserDefined").prop("disabled", true);
    }
});

$('body').on('change', '#structType', function (e) {
    $("#hdnTypeName").val($("#structType option:selected").text());
    if ($("#structType").val() == "999999999") {
        $("#TypeUserDefined").prop("disabled", false);
    }
    else {
        $("#TypeUserDefined").val("");
        $("#TypeUserDefined").prop("disabled", true);
    }
});

$('body').on('change', '#structTypeOne', function (e) {
    $("#hdnTypeOneName").val($("#structTypeOne option:selected").text());
    if ($("#structTypeOne").val() == "999999999") {
        $("#TypeOneUserDefined").prop("disabled", false);
    }
    else {
        $("#TypeOneUserDefined").val("");
        $("#TypeOneUserDefined").prop("disabled", true);
    }
});

$('body').on('change', '#structTypeTwo', function (e) {
    $("#hdnTypeTwoName").val($("#structTypeTwo option:selected").text());
    if ($("#structTypeTwo").val() == "999999999") {
        $("#TypeTwoUserDefined").prop("disabled", false);
    }
    else {
        $("#TypeTwoUserDefined").val("");
        $("#TypeTwoUserDefined").prop("disabled", true);
    }
});

$('body').on('click', '#btnCancel', function (e) {
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + hf_StructureId);
})
