var hf_StructureId = $('#hf_StructureId').val();
var hf_sectionType = $('#hf_sectionType').val();

function AffectedstructureSummaryPopupInit() {
    hf_StructureId = $('#hf_StructureId').val();
    hf_sectionType = $('#hf_sectionType').val();
    hf_Structurecode = $('#hf_Structurecode').val();
    hf_SectionId = $('#hf_SectionId').val();
    Resize_PopUp(650);
    addscroll();
    ChangeFontStyle();
}

$(document).ready(function () {
    var value = $("#AuthorizeMovementGeneral").val();
    $('body').off('click', '#span-close');
    $('body').on('click', '#span-close', function (e) {
        $('#overlay').hide();
        addscroll();
    });
    $('body').off('click', '.span-CloseStructDetails');
    $('body').on('click', '.span-CloseStructDetails', function (e) {
        CloseStructureDetailsAffSumm(this);
    });
    $('body').off('click', '.AffectedstructureSummaryContainer .DisplayContact');
    $('body').on('click', '.AffectedstructureSummaryContainer .DisplayContact', function (e) {
        var ContactDetails = $(this).attr("displaycontactdetails") || $(this).attr("contactdetails");
        DisplayContactDetailsAffectedSummary(ContactDetails);
    });
    $('body').off('click', '.sectiondetails');
    $('body').on('click', '.sectiondetails', function (e) {
        SectionDetailsAffSumm(this);
    });
    $('body').off('click', '#viewMap');
    $('body').on('click', '#viewMap', function (e) {
        ViewStructureOnMapAffSumm(this);
    });
    $('body').off('click', '.Showrelatedmovements');
    $('body').on('click', '.Showrelatedmovements', function (e) {
        ShowRelatedMovm();
    });
});

function ChangeFontStyle() {
    var textElement = $(".affected-type-text");
    var text = textElement.text().toLowerCase().replace(/\b\w/g, function (char) {
        return char.toUpperCase();
    });
    textElement.text(text);
}
function SectionDetailsAffSumm(e) {
    var SectionDetails = $(e).attr("sectiondetails");
    var arg2 = $(e).attr("arg2");
    showSectionDetails(SectionDetails, arg2);
}
function closeSpan() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function CloseStructureDetailsAffSumm() {
    addscroll();
    resetdialogue();
    stopAnimation();
    $('#dialogue').modal('hide');
    $("#overlay").css("display", "none");
}
function DisplayContactDetailsAffectedSummary(ContactId) {
    startAnimation();
    $('#exampleModalCenter22').hide();
    $("#dialogue1").load('../Application/ViewContactDetails?ContactId=' + ContactId + "", function () {
        removescroll();
        stopAnimation();
        $('#contactDetails').modal('hide');
        $('#contactDetailsForMap').show();
        $("#dialogue1").show();
        $("#overlay").show();
    });

}
function closeContactPopup() {
    $('#contactDetailsForMap').hide();
    $("#dialogue1").hide();
    $('#exampleModalCenter22').show();
}
function showSectionDetails(StructureId, sectionId) {
    $("#hdnSectionID").val(sectionId);
    var sectionType = $('#' + sectionId).parent().find('input:hidden:first').val();
    $('#divStructureSectionDetails').load('../Structures/ReviewStructureSectionImposedConstraints',
        { structureId: StructureId, sectionId: sectionId, sectionType: sectionType },
        function () {
            $('#divStructureSectionDetails').show();
        }
    );
}
function ViewStructureOnMapAffSumm() {
    var OSGREast, OSGRNorth;
    OSGREast = $("#OSGREast").val();
    OSGRNorth = $("#OSGRNorth").val();
    var url = window.location.href;
    sessionStorage.setItem("AuthorizeGeneralUrl", url);
    var urlRedirect = "../Structures/MyStructures?x=" + OSGREast + "&y=" + OSGRNorth + "&structId=" + hf_StructureId + "&StructureCode=" + hf_Structurecode +"&assessmentFlag="+1;
    window.location.href = urlRedirect;
}
