
$(document).ready(function () {
    selectedmenu('Structures');
    SelectMenu(3);
    // fillPageSizeSelect();
    var OrgFromId = $('#OrgFromId').val();
    var arrId = $('#arrangId').val();

    var OrgFromId = $('#OrgFromId').val();

    $('#review_deleg').load('../Structures/ReviewDelegation?arrangId=' + arrId + '&OrgFromId=' + OrgFromId);

    $('#BackDeleg').click(function () {
        window.location.href = '../Structures/MyDelegationArrangement';
    });
    $('body').on('click', '.StructureCodeClick', function () {
        StructureCodeClick(this);
    });
});

function linkStructureCodeClick(structId) {
    var a = EncodedQueryString("structureId=" + structId);
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structId);
}

function StructureCodeClick(_this) {
    var structureId = $(_this).attr("structureid");
    linkStructureCodeClick(structureId);
}

function StructureCodeClick(_this) {
    var structureId = $(_this).attr("structureid");
    linkStructureCodeClick(structureId);
}
