var StructureIDVal = $('#CautionAddReportContainer #hf_StructureID').val();
var SectionIDVal = $('#CautionAddReportContainer #hf_SectionID').val();

function CautionAddReportInit() {
    StructureIDVal = $('#CautionAddReportContainer #hf_StructureID').val();
    SectionIDVal = $('#CautionAddReportContainer #hf_SectionID').val();
}
$(document).ready(function () {

    $('body').on('click', '#CautionAddReportContainer #SaveCaution', function (e) {
        e.preventDefault();
        SaveCautionsFromReport(this);
        $('html, body').animate({
            scrollTop: ($('#CreateCautionContainer').position().top)
        }, 1000);
    });
    $('body').on('click', '#CautionAddReportContainer .backbtn', function (e) {
        e.preventDefault();
        Back(this);
    });

    $('body').on('click', '#CautionAddReportContainer .Reviewbtn', function (e) {
        e.preventDefault();
        ReviewCaution(this);
    });
});
function Back() {
    $('#causionReport').empty();
    $('#generalSettingsId').show();
}
function failed() {
    $('#generalSettingsId').css("display", "block");
    $('#check-caution').css("display", "none");
}
function closecaution() {
    $('#check-caution').css("display", "none");
    $('#add-caution').css("display", "block");
}
function SaveCautionsFromReport() {
    $.ajax({
        url: '../Structures/SaveCautions',
        dataType: 'json',
        type: 'POST',
        data: $("#CautionInfo").serialize(),
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result) {
                if ($('#hf_mode').val() == "add") {
                    stopAnimation();
                    showToastMessage({
                        message: "Caution added successfully",
                        type: "success"
                    });
                    ReviewCaution();
                }
                else {
                    stopAnimation();
                    showToastMessage({
                        message: "Caution updated successfully",
                        type: "success"
                    });
                    ReviewCaution();
                }
            }
            else {
                redirectpage();
            }
        },
        error: function (xhr, status) {
        }
    });
}
function ReviewCaution() {
    CloseSuccessModalPopup();
    $('#cautionPopup').modal('hide');
    startAnimation();
    var randomNumber = Math.random();
    $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + StructureIDVal + "&sectionId=" + SectionIDVal + '&random=' + randomNumber,
        function () {
            stopAnimation();
            $('#causionReport').empty();
            $('#generalSettingsId').show();
            $('#manageCautionId').show();
            ReviewCautionsListInit();
        }
    );
};
