ViewContactDetails_Obj = {}
var StructureId = $('#hf_StructureId').val();
var sectionType1 = $('#hf_sectionType').val();
var hf_sectionType = $('#hf_sectionType').val();

function ReviewSummaryPopupInit() {
    StructureId = $('#hf_StructureId').val();
    sectionType1 = $('#hf_sectionType').val();
    hf_sectionType = $('#hf_sectionType').val();
    Resize_PopUp(650);
    addscroll();
    ViewContactDetails_Obj.ContactParent = {StructureId: StructureId, Method: 'viewStructureDetails'};//this
}
$(document).ready(function () {

    $('body').on('click', '.review-summary-popup .closestructdetail', function (e) {
        CloseStructureDetails(this);
    });

    $('body').on('click', '.review-summary-popup .displaycontact', function (e) {
        var arg1 = $(this).attr("arg1");
        DisplayContactDetailsReviewSummary(arg1);
    });

    $('body').on('click', '.review-summary-popup .showsection', function (e) {
        var arg1 = $(this).attr("arg1");
        showSectionDetails(arg1, e.target.id);
    });
    $('body').on('click', '.review-summary-popup #span-close', function (e) {
        $('#overlay').hide();
        addscroll();
    });

    $('body').on('click', '#close_contact_popup', function (e) {
        e.preventDefault();
        closeContactPopup();
    });
});

function closeSpan() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function CloseStructureDetails() {
    $('#exampleModalCenter22').hide();
    $('#overlay').hide();
    addscroll();
    resetdialogue();
    stopAnimation();
}
function DisplayContactDetailsReviewSummary(ContactId) {
    startAnimation();
    $('#exampleModalCenter22').hide();
    $("#dialogue1").load('../Application/ViewContactDetails?ContactId=' + ContactId + "", function () {
        stopAnimation();
        removescroll();
        $('#contactDetails').hide();        
        $('#contactDetailsForMap').show();
        $("#dialogue1").show();
        $("#overlay").show();
    });

}
function closeContactPopup() {
    $('#contactDetailsForMap').hide();
    $('#exampleModalCenter22').show();
    $("#dialogue").show();
    $("#dialogue1").hide();
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

