$(document).ready(function () {
    $('body').on('click', '.affected-structure-open-filter', function (e) {
        openFilters(this);
    });
    $('body').on('click', '.affected-structure-select-unselect-all', function (e) {
        var id = $(this).attr("id");
        var divid = $(this).attr("divid");
        SelectUnselectAll(id, divid);
    });
    $('body').on('click', '.affected-structure-list-struct-ids', function (e) {
        var structId = $(this).attr("structno");
        var structCode = $(this).attr("esrn");
        var routeId = $(this).attr("routeid");
        var sectionId = $(this).attr("sectionid");
        var structCount = $(this).attr("structcount");
        ListStructureIDs(structId, structCode, routeId, sectionId, structCount);
    });

    $('body').on('click', '.affected-structure-display-structure-gent', function (e) {
        var structCode = $(this).attr("esrn");
        var routeId = $(this).attr("routeid");
        var sectionId = $(this).attr("sectionid");
        DisplayStructureGENT(structCode, routeId, sectionId);
    });

    $('body').on('click', '.affected-structure-show-note-to-haulier', function (e) {
        var structCode = $(this).attr("esrn");
        var note = $(this).attr("hauliertext");
        ShowNoteToHaulier(note, structCode);
    });
    $('body').on('click', '.affected-structure-view-assess-comment', function (e) {
        var structCode = $(this).attr("esrn");
        var comment = $(this).attr("assesscomments");
        ViewAssessmentComment(comment, structCode);
    });
});
function AffectedStructuresInit(selectedRadio) {
    if (selectedRadio == undefined || selectedRadio == null) {
        $('#AffectedStructureXslt').find('.route_select').eq(0).attr({ "checked": true });
        $('.routedivclass').hide();
        $('.routedivclass').eq(0).show();
    } else {
        $('[name=route_select][value="'+selectedRadio+'"]').prop('checked', true).trigger('change');
    }
    $('body').off('change', '#AffectedStructureXslt .route_select');
    $('body').on('change', '#AffectedStructureXslt .route_select', function (e) {
        var assesscheck = $(this).attr("assesscheck");
        select_route_as(this, assesscheck);
    });
    
}

function select_route_assessment(x) {
    $('.routedivclass').hide();
    $('#' + x.value).show();
    
}
function select_route_as(x, assessCheck) {
    $('.routedivclass').hide();
    if (hf_affectstarray.includes(x.value)) {
        var pos = hf_affectstarray.indexOf(x.value);
        hf_affectstarray.splice(pos,1);
    }
    
    $('#' + x.value).show();
    if (assessCheck == 0) {
        if ($('#' + x.value + ' td').length > 0) {
            $('#divNoAffected').hide();
        }
        else {
            $('#divNoAffected').show();
        }
    }
    else {
        CheckAssessmentResult();
    }
}
