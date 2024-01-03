
var hf_SignedHeightStatus = $("#hf_SignedHeightStatus").val();
var hf_SignedLengthStatus = $("#hf_SignedLengthStatus").val();
var hf_SignedWidthStatus = $("#hf_SignedWidthStatus").val();
var hf_SignedGrossWeightStatus = $("#hf_SignedGrossWeightStatus").val();
var hf_SignedAxleWeightStatus = $("#hf_SignedAxleWeightStatus").val();
var hf_HALoading = $("#hf_HALoading").val();
var hf_HeightMeter = $("#hf_HeightMeter").val();
var hf_HeightFeet = $("#hf_HeightFeet").val();
var hf_HeightInches = $("#hf_HeightInches").val();
var hf_WidthMeter = $("#hf_WidthMeter").val();
var hf_WidthFeet = $("#hf_WidthFeet").val();
var hf_WidthInches = $("#hf_WidthInches").val();
var hf_LengthMeter = $("#hf_LengthMeter").val();
var hf_LengthFeet = $("#hf_LengthFeet").val();
var hf_LengthInches = $("#hf_LengthInches").val();
var hf_GrossWeight = $("#hf_GrossWeight").val();
var hf_AxleWeight = $("#hf_AxleWeight").val();
var hf_MaxWeightOverMinDistanceWeight = $("#hf_MaxWeightOverMinDistanceWeight").val();
var hf_structureId = $('#hf_structureId').val();
var hf_sectionId = $('#hf_sectionId').val();
var hf_StructureId = $('#hf_StructureId').val();

function StructureImposedContraintInit() {
    hf_SignedHeightStatus = $("#hf_SignedHeightStatus").val();
    hf_SignedLengthStatus = $("#hf_SignedLengthStatus").val();
    hf_SignedWidthStatus = $("#hf_SignedWidthStatus").val();
    hf_SignedGrossWeightStatus = $("#hf_SignedGrossWeightStatus").val();
    hf_SignedAxleWeightStatus = $("#hf_SignedAxleWeightStatus").val();
    hf_HALoading = $("#hf_HALoading").val();
    hf_HeightMeter = $("#hf_HeightMeter").val();
    hf_HeightFeet = $("#hf_HeightFeet").val();
    hf_HeightInches = $("#hf_HeightInches").val();
    hf_WidthMeter = $("#hf_WidthMeter").val();
    hf_WidthFeet = $("#hf_WidthFeet").val();
    hf_WidthInches = $("#hf_WidthInches").val();
    hf_LengthMeter = $("#hf_LengthMeter").val();
    hf_LengthFeet = $("#hf_LengthFeet").val();
    hf_LengthInches = $("#hf_LengthInches").val();
    hf_GrossWeight = $("#hf_GrossWeight").val();
    hf_AxleWeight = $("#hf_AxleWeight").val();
    hf_MaxWeightOverMinDistanceWeight = $("#hf_MaxWeightOverMinDistanceWeight").val();
    hf_structureId = $('#hf_structureId').val();
    hf_sectionId = $('#hf_sectionId').val();
    hf_StructureId = $('#hf_StructureId').val();

    if (hf_SignedHeightStatus == "251001") {

        $("#SignedHeightCheck").prop('checked', true);
    }
    else {

        $("#SignedHeightCheck").prop('checked', false);
    }
    if (hf_SignedLengthStatus == "251001") {
        $("#SignedLenCheck").prop('checked', true);
    }
    else {
        $("#SignedLenCheck").prop('checked', false);
    }
    if (hf_SignedWidthStatus == "251001") {
        $("#SignedWidthCheck").prop('checked', true);
    }
    else {
        $("#SignedWidthCheck").prop('checked', false);
    }
    if (hf_SignedGrossWeightStatus == "251001") {
        $("#SignedGrossWeightCheck").prop('checked', true);
    }
    else {
        $("#SignedGrossWeightCheck").prop('checked', false);
    }
    if (hf_SignedAxleWeightStatus == "251001") {
        $("#SignedAxleWeightCheck").prop('checked', true);
    }
    else {
        $("#SignedAxleWeightCheck").prop('checked', false);
    }

    if (hf_HALoading == 1) {
        $("#HALoadingTrue").prop('checked', true);
        $("#HALoadingFalse").prop('checked', false);
    }
    else {
        $("#HALoadingFalse").prop('checked', true);
        $("#HALoadingTrue").prop('checked', false);
    }




    if (hf_HeightMeter == "" && hf_HeightFeet == "" && hf_HeightInches == "") {
        $("#SignedHeightNotKnownSigned").prop('checked', true);
        $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', true);
        $("#SignedHeightCheck").prop('disabled', true);
    }
    else if (hf_HeightMeter == 0.0 && hf_HeightFeet == 0.0 && hf_HeightInches == 0.0) {
        $("#SignedHeightNoSignedConst").prop('checked', true);
        $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', true);
        $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").val("");
        $("#SignedHeightCheck").prop('disabled', true);
    }
    else {
        $("#SignedHeightMadeByEsdal").prop('checked', true);
        $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', false);
        $("#SignedHeightCheck").prop('disabled', false);
    }


    if (hf_WidthMeter == "" && hf_WidthFeet == "" && hf_WidthInches == "") {
        $("#SignedWidthNotKnownSigned").prop('checked', true);
        $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', true);
        $("#SignedWidthCheck").prop('disabled', true);
    }
    else if (hf_WidthMeter == 0.0 && hf_WidthFeet == 0.0 && hf_WidthInches == 0.0) {
        $("#SignedWidthNoSignedConst").prop('checked', true);
        $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', true);
        $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").val("");
        $("#SignedWidthCheck").prop('disabled', true);
    }
    else {
        $("#SignedWidthMadeByEsdal").prop('checked', true);
        $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', false);
        $("#SignedWidthCheck").prop('disabled', false);
    }




    if (hf_LengthMeter == "" && hf_LengthFeet == "" && hf_LengthInches == "") {
        $("#SignedLenNotKnownSigned").prop('checked', true);
        $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', true);
        $("#SignedLengthCheck").prop('disabled', true);
    }
    else if (hf_LengthMeter == 0.0 && hf_LengthFeet == 0.0 && hf_LengthInches == 0.0) {
        $("#SignedLenNoSignedConst").prop('checked', true);
        $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', true);
        $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").val("");
        $("#SignedLengthCheck").prop('disabled', true);
    }
    else {
        $("#SignedLenMadeByEsdal").prop('checked', true);
        $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', false);
        $("#SignedLengthCheck").prop('disabled', false);
    }




    if (hf_GrossWeight == "" || hf_GrossWeight == null) {
        $("#SignedGrossWeightNotKnownSigned").prop('checked', true);
        $("#Signed_Gross_Weight").prop('readOnly', true);
        $("#SignedGrossWeightCheck").prop('disabled', true);

    }
    else if (hf_GrossWeight == 0.0) {
        $("#SignedGrossWeightNoSignedConst").prop('checked', true);
        $("#Signed_Gross_Weight").prop('readOnly', true);
        $("#Signed_Gross_Weight").val("");
        $("#SignedGrossWeightCheck").prop('disabled', true);
    }
    else {
        $("#SignedGrossWeightMadeByEsdal").prop('checked', true);
        $("#Signed_Gross_Weight").prop('readOnly', false);
        $("#SignedGrossWeightCheck").prop('disabled', false);
    }



    if (hf_AxleWeight == "" || hf_AxleWeight == null) {
        $("#SignedAxleWeightNotKnownSigned").prop('checked', true);
        $("#Signed_Axcel_Weight").prop('readOnly', true);
        $("#SignedAxleWeightCheck").prop('disabled', true);
    }
    else if (hf_AxleWeight == 0.0) {
        $("#SignedAxleWeightNoSignedConst").prop('checked', true);
        $("#Signed_Axcel_Weight").prop('readOnly', true);
        $("#Signed_Axcel_Weight").val("");
        $("#SignedAxleWeightCheck").prop('disabled', true);
    }
    else {
        $("#SignedAxleWeightMadeByEsdal").prop('checked', true);
        $("#Signed_Axcel_Weight").prop('readOnly', false);
        $("#SignedAxleWeightCheck").prop('disabled', false);
    }

    $("#btnMaxWeightOvrDstAdd").prop('disabled', true);

    if (hf_strucType == 1 && (hf_MaxWeightOverMinDistanceWeight != "" && hf_MaxWeightOverMinDistanceWeight != null)) {
        var MaxWeightOverminDistWeight = hf_MaxWeightOverMinDistanceWeight;
        var MaxWeightOverminDistDistance = ModelObject.MaxWeightOverMinDistanceDistance;

        var arrMaxWeightOverminDistWeight = MaxWeightOverminDistWeight.split(',');
        var arrMaxWeightOverminDistDistance = MaxWeightOverminDistDistance.split(',');

        for (var i = 0; i < arrMaxWeightOverminDistWeight.length; i++) {
            rowID = i + 1;
            var lblRow = "HeightEnv" + (rowID);
            var DivRow = "weightover" + (rowID);


            $("#tabMaxWeightOvrDst").append("<div class='row mb-3' id='" + DivRow + "'><div class='col-lg-3 col-md-6 col-sm-12 edit-normal weight' style='width: 33%; padding-right: 0px; padding-left: 0px;'>" + arrMaxWeightOverminDistWeight[i] + "</div><div class='col-lg-3 col-md-3 col-sm-12 edit-normal distance' style='width: 33%; padding-right: 0px; padding-left: 0px;'>" + arrMaxWeightOverminDistDistance[i] + "</div><div class='col-lg-4 col-md-3 col-sm-12 edit-normal'><a class='text-normal-hyperlink a_deleteMaxWeight' id='" + lblRow + "' RowId=" + '"' + DivRow + '"' + "  style='text-decoration: underline;line-height: 18px;'>Remove</a></div></div>");
            $("#tabMaxWeightOvrDst").css("display", "block");

            $('body').on('click', '.a_deleteMaxWeight', function (e) {
                e.preventDefault();
                DeleteMaxWeight(this);
            });
        }
    }

}

$(document).ready(function () {
    $('body').on('click', '#SignedHeightNotKnownSigned, #SignedHeightNoSignedConst', function (e) {
        $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', true);
        $("#SignedHeightCheck").prop('disabled', true);
    });

    $('body').on('click', '#SignedHeightMadeByEsdal', function (e) {
        $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', false);
        $("#SignedHeightCheck").prop('disabled', false);
    });

    $('body').on('click', '#SignedWidthNotKnownSigned, #SignedWidthNoSignedConst', function (e) {
        $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', true);
        $("#SignedWidthCheck").prop('disabled', true);
    });

    $('body').on('click', '#SignedWidthMadeByEsdal', function (e) {
        $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', false);
        $("#SignedWidthCheck").prop('disabled', false);
    });

    $('body').on('click', '#SignedLenNotKnownSigned, #SignedLenNoSignedConst', function (e) {
        $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', true);
        $("#SignedLenCheck").prop('disabled', true);
    });

    $('body').on('click', '#SignedLenMadeByEsdal', function (e) {
        $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches, #SignedLenCheck").prop('readOnly', false);
        $("#SignedLenCheck").prop('disabled', false);
    });

    $('body').on('click', '#SignedGrossWeightNotKnownSigned, #SignedGrossWeightNoSignedConst', function (e) {
        $("#Signed_Gross_Weight").prop('readOnly', true);
        $("#SignedGrossWeightCheck").prop('disabled', true);
    });
    $('body').on('click', '#SignedGrossWeightMadeByEsdal', function (e) {
        $("#Signed_Gross_Weight").prop('readOnly', false);
        $("#SignedGrossWeightCheck").prop('disabled', false);
    });

    $('body').on('click', '#SignedAxleWeightNotKnownSigned, #SignedAxleWeightNoSignedConst', function (e) {
        $("#Signed_Axcel_Weight").prop('readOnly', true);
        $("#SignedAxleWeightCheck").prop('disabled', true);
    });

    $('body').on('click', '#SignedAxleWeightMadeByEsdal', function (e) {
        $("#Signed_Axcel_Weight").prop('readOnly', false);
        $("#SignedAxleWeightCheck").prop('disabled', false);
    });

    $("#frmEditSTRUCT_IMPOSED").submit(function (e) {

        if ($('#hf_strucType').val() == 1) {
            var weight = "", distance = "";

            $('#tabMaxWeightOvrDst tr').each(function () {
                weight = weight + $(this).find(".weight").html() + ",";
                distance = distance + $(this).find(".distance").html() + ",";
            });
            if (weight.length > 0 && distance.length > 0) {
                weight = weight.substring(0, (weight.length - 1));
                distance = distance.substring(0, (distance.length - 1));
            }
            $('#MaxWeightOverminDist_Weight').val(weight);
            $('#MaxWeightOverminDist_Distance').val(distance);
        }

        return ValidationCheckStructImposed();
    });
    $('body').on('click', '#btnMaxWeightOvrDstAdd', function (e) {
        e.preventDefault();
        insRowMaxWeight(this);
    });
    $('body').on('click', '#btnsaveSVData1', function (e) {
        e.preventDefault();
        saveManageImposedConstraint(this);
    });
});


function saveManageImposedConstraint() {
    var weight = "", distance = "";

    $('#tabMaxWeightOvrDst').children('div').each(function () {
        weight = weight + $(this).find(".weight").html() + ",";
        distance = distance + $(this).find(".distance").html() + ",";

    });

    if (weight.length > 0 && distance.length > 0) {
        weight = weight.substring(0, (weight.length - 1));
        distance = distance.substring(0, (distance.length - 1));
    }
    $('#MaxWeightOverminDist_Weight').val(weight);
    $('#MaxWeightOverminDist_Distance').val(distance);
    var test = $("#frmEditSTRUCT_IMPOSED").serialize();

    $.ajax({
        url: '../Structures/StoreEditSTRUCT_IMPOSED',
        dataType: 'json',
        type: 'POST',
        data: $("#frmEditSTRUCT_IMPOSED").serialize(),
        success: function (result) {
            if (result == true) {
                saveDimensionImposedConstraint();
            }

        },
        error: function (xhr, status) {
        }
    });
}

function saveDimensionImposedConstraint() {
    startAnimation();
    $.ajax({
        url: '../Structures/EditSTRUCT_IMPOSED',
        dataType: 'json',
        type: 'POST',
        data: {
            StructId: hf_structureId, SectionId: hf_sectionId
        },
        success: function (result) {
            stopAnimation();
            if (result) {
                ShowSuccessModalPopup("Structure imposed constraints saved successfully", "ShowReviewSummary");
            }
            else {
                ShowErrorPopup("Structure imposed constraints save failed")
            }

        },
        error: function (xhr, status) {
        }
    });
}


function ValidationCheckStructImposed() {

    if ($('#hf_strucType').val() == 3) {
        if ($("#Vertical_Alignment_EntryDistance").val().length <= 0 && $("#Vertical_Alignment_EntryHeight").val().length <= 0 &&
            $("#Vertical_Alignment_MaxHeighDistance").val().length <= 0 && $("#Vertical_Alignment_MaxHeighHeight").val().length <= 0 &&
            $("#Vertical_Alignment_ExitDistance").val().length <= 0 && $("#Vertical_Alignment_ExitHeight").val().length <= 0) {

            $("#errVerticalAlignment").text("");
            return true;
        }
        else if ($("#Vertical_Alignment_EntryDistance").val().length > 0 && $("#Vertical_Alignment_EntryHeight").val().length > 0 &&
            $("#Vertical_Alignment_MaxHeighDistance").val().length > 0 && $("#Vertical_Alignment_MaxHeighHeight").val().length > 0 &&
            $("#Vertical_Alignment_ExitDistance").val().length > 0 && $("#Vertical_Alignment_ExitHeight").val().length > 0) {

            $("#errVerticalAlignment").text("");
            return true;
        }
        else {
            $("#errVerticalAlignment").text("There must be an entry for all distance and height");
            return false;
        }
    }
}

function ShowReviewSummary() {
    CloseSuccessModalPopup();
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + hf_StructureId);
}

function insRow() {
    var name = document.getElementsByName("spnRemove");
    rowID = (name.length) + 1;
    let lblRow = "HeightEnv" + (rowID);

    str = "<tr><td> " + $('#Offset').val() + " </td><td> " + $('#EnvelopeWidth').val() + " </td><td> " + $('#EnvelopeHeight').val() + " </td><td><button id='" + lblRow + "' RowId='" + lblRow + "' class='btnbdr btngrad btnrds btnDeleteRow' aria-hidden='true' data-icon='' type='button'>Remove</button></td></tr>";
    $("#tab").append(str);
    $("#Offset, #EnvelopeWidth, #EnvelopeHeight").val("");

    $('body').on('click', '.btnDeleteRow', function (e) {
        e.preventDefault();
        DeleteRowFn(this);
    });
}
function deleteRow(rowID) {
    let tr = $("#" + rowID).closest('tr');
    tr.remove();
}

$('body').on('keyup', '#MaxWeightOverMinDistance_TonnesOver, #MaxWeightOverMinDistance_Meter', function (e) {
    let lenTonnes = $("#MaxWeightOverMinDistance_TonnesOver").val().length;
    let lenMeter = $("#MaxWeightOverMinDistance_Meter").val().length;
    if (lenTonnes > 0 || lenMeter > 0) {
        $("#btnMaxWeightOvrDstAdd").prop('disabled', false);
    }
    else {
        $("#btnMaxWeightOvrDstAdd").prop('disabled', true);
    }
});

function insRowMaxWeight() {

    let weight = "";


    $('#tabMaxWeightOvrDst').children('div').each(function () {
        weight = weight + $(this).find(".weight").html() + ",";

    });
    let words = weight.split(",");

    if (weight.length > 0) {
        weight = weight.substring(0, (weight.length - 1));
    }

    let flag = 0;


    if ($("#MaxWeightOverMinDistance_TonnesOver").val().length == 0 && $("#MaxWeightOverMinDistance_Meter").val().length == 0) {
        flag = 1;
    }
    else if ($("#MaxWeightOverMinDistance_TonnesOver").val().length > 0 && $("#MaxWeightOverMinDistance_Meter").val().length > 0) {
        flag = 1;
    }
    else {
        flag = 0;
    }


    if (flag == 1) {

        rowID = (words.length) + 1;
        let lblRow = "HeightEnv" + (rowID);
        let DivRow = "weightover" + (rowID);

        str = "<tr><td class='weight'> " + $('#MaxWeightOverMinDistance_TonnesOver').val() + " </td><td class='distance'> " + $('#MaxWeightOverMinDistance_Meter').val() + " </td><td><button RowId='" + lblRow + "' id='" + lblRow + "' class='btnbdr btngrad btnrds a_deleteMaxWeight' aria-hidden='true' data-icon='' type='button'>Remove</button></td></tr>";

        $("#tabMaxWeightOvrDst").append("<div class='row mb-3' id='" + DivRow + "'><div class='col-lg-3 col-md-6 col-sm-12 edit-normal weight' style='width:33%; padding-right: 0px; padding-left: 0px;'>" + $('#MaxWeightOverMinDistance_TonnesOver').val() + "</div><div class='col-lg-3 col-md-3 col-sm-12 edit-normal distance' style='width:33%; padding-right: 0px; padding-left: 0px;'>" + $('#MaxWeightOverMinDistance_Meter').val() + "</div><div class='col-lg-4 col-md-3 col-sm-12 edit-normal'><a class='text-normal-hyperlink a_deleteMaxWeight' id='" + lblRow + "' RowId=" + '"' + DivRow + '"' + " style='text-decoration: underline;line-height: 18px;'>Remove</a></div></div>");
        $("#tabMaxWeightOvrDst").css("display", "block");

        $('body').on('click', '.a_deleteMaxWeight', function (e) {
            e.preventDefault();
            DeleteMaxWeight(this);
        });

        $('#MaxWeightOverMinDistance_TonnesOver').val('');
        $('#MaxWeightOverMinDistance_Meter').val('');
        $("#errMaxWeightOvrDst").text("");
    }
    else {
        $("#errMaxWeightOvrDst").text("There must be an entry for weight and distance");
    }
}
function deleteRowMaxWeight(rowID) {
    $("#" + rowID).remove();
}

//$('body').on('click', '#manageImposed #btnCancel', function (e) {
//    $("#structureDimension").html('');
//    $("#bottomSection #tableSOA").remove();
//    $("#bottomSection #icaMethods").remove();
//    $("#bottomSection #imposedSection").remove();
//    $("#bottomSection #tableSVData").remove();
//    $('#msd1').hide();
//    $('#msd').hide();
//    $('#sm').show();
//    $('#sm1').show();
//});

function DeleteMaxWeight(e) {
    let rowId = $(e).attr("rowid");
    deleteRowMaxWeight(rowId);
}
function DeleteRowFn(e) {
    let rowId = $(e).attr("rowid");
    deleteRow(rowId);
}
