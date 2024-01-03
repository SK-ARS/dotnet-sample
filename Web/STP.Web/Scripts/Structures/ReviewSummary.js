var StructureId = $('#hf_StructureId').val();
var SectionType = $('#hf_SectionType').val();
var ESRN = $('#hf_ESRN').val();
var objListListStructureGeneralDetails = $('#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').val();
var StructureName = $('#hf_StructureName').val();
var ESRNum = $('#hf_ESRnum').val();


oldId = 'li1';
oldIdDiv = 'generalSettingsId';

function displayBlockers(oid) {
    document.getElementById(oid).style.display = 'none';
}

$(document).ready(function () {
    
    $('body').on('click', '.showselectiondetails', function (e) {
        showSectionDetailsSectionReviewSummary(this);
    });
    if ($(".showselectiondetails").length == 1) {
        $(".showselectiondetails:first").attr('checked', 'checked').trigger("click");
    }

    $('body').on('click', '.showcontact', function (e) {
        ShowContactDetailsSectionReviewSummary(this);
    });

    $('body').on('click', '#closing', function (e) {
        e.preventDefault();
        closeNav(this);
    });
    startAnimation();
    SelectMenu(3);

    var SectionID = $("#hdnSectionID").val();
    $("#leftSection").load('../Structures/StructureReviewSummaryLeftPanel',
        { sectionType: SectionType, structureId: StructureId, structureName: StructureName, sectionId: SectionID, ESRN: ESRNum },

        function () {
            $('#li7').hide();
            $('#generalSettingsId').show();
            $('#backButtonId').show();
            StructureReviewSummaryLeftPanelInit();
            stopAnimation();
        }
    );

    var textElement = $("#text13"); 
    var text = textElement.text().toLowerCase().replace(/\b\w/g, function (char) {
        return char.toUpperCase();
    });
    textElement.text(text);
});
function showSectionDetailsSectionReviewSummary(e) {
    var arg1 = $(e).attr("arg1");

    showSectionDetailsReviewSummary(arg1, e.id);
}
function ShowContactDetailsSectionReviewSummary(e) {
    var arg1 = $(e).attr("arg1");

    ShowContactDetailsReviewSummary(arg1);
}
function showSectionDetailsReviewSummary(structureId, sectionId) {
    startAnimation();
    var sectionType = $('#' + sectionId).parent().find('input:hidden:first').val();
    $('#structureDimension').empty();
    $('#bottomSection').empty();
    $('#structureDimension').load("../Structures/ReviewStructureSectionSummary?structureId=" + structureId + "&sectionId=" + sectionId + "&sectionType=" + sectionType, function (responseTxt, statusTxt, xhr) {
        $('#structureDimension').html(responseTxt);

        $("#leftSection").load('../Structures/StructureReviewSummaryLeftPanel',
            { sectionType: sectionType, structureId: structureId, structureName: StructureName, sectionId: sectionId, ESRN: ESRN },

            function () {
                $('#li7').hide();
                $('#generalSettingsId').show();
                $('#backButtonId').show();
                StructureReviewSummaryLeftPanelInit();


                $('#tableSOA').appendTo('#bottomSection');
                $('#icaMethods').appendTo('#bottomSection');
                $('#imposedSection').appendTo('#bottomSection');
                $('#tableSVData').appendTo('#bottomSection');

                $('#li7').show();
                //$('#li12').show();
                if (objListListStructureGeneralDetails != null) {
                    $('#msd1').show();
                    $('#msd').show();
                    $('#sm').show();
                    $('#sm1').show();
                }
                $('#backButtonId').show();

                //$("#imposedSection").hide();
                //$("#icaMethods").hide();
                //$('#tableSVData').hide();
                $(".span-type-values").each(function () {
                    ChangeCase(this);
                });
                stopAnimation();

            }
        );

        
    });

}

function ChangeCase(id) {
    var text = $(id).text().toLowerCase().replace(/\b\w/g, function (char) {
        return char.toUpperCase();
    });
    $(id).text(text);
}

function ShowContactDetailsReviewSummary(ContactId) {
    $("#overlay").show();
    startAnimation();
    $("#causionReport").load('../Application/ViewContactDetails?ContactId=' + ContactId + "", function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });

        $('#contactDetails').modal('show');
        stopAnimation();
    });

}
//spnclosemap
$('body').on('click', '#spnclosemap', function (e) {
    e.preventDefault();
    $('#contactDetails').modal('hide');
});
function BackToPrevious() {
    window.location.href = "../Structures/StructureList";
}
$('body').on('click', '#BackStructureList', function (e) {

    
    //window.history.back().reload();
    $.ajax({
        url: '../Structures/ViewRouteMapOnBack',
        type: 'POST',
        cache: false,
        async: false,
        data: {},
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (data) {
           
            if (data.result == "DelegStructureList") {
                window.location.href = "../Structures/StructureList" + EncodedQueryString("Mode=Edit");
            }
            else if (data.result == "StructureList") {
                window.location.href = "../Structures/StructureList";
            }
            //else if (data.result == "NotDelegStructureList") {
            //    window.location.href = "../Structures/NotDelegStructureList";
            //}
            else {
                window.history.back();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //location.reload();
            console.error(errorThrown)
        },
        complete: function () {
            $('.loading').hide();
        }
    });

})

